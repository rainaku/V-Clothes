using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using VClothes.Data;
using VClothes.Services;

namespace VClothes.ViewModels;

public class QuanLyNhanVienViewModel : ViewModelCha
{
    private ObservableCollection<NhanVienDto> _danhSachNhanVien = new();
    private NhanVienDto? _nhanVienDuocChon;
    private string _timKiem = string.Empty;
    private bool _dangSua;
    private string _thongBaoXacThuc = string.Empty;
    private string _cotSapXep = "employee_code";
    private bool _sapXepTang = true;

    private string _suaMaNhanVien = string.Empty;
    private string _suaHoTen = string.Empty;
    private string? _suaGioiTinh;
    private DateTime? _suaNgaySinh;
    private string _suaDienThoai = string.Empty;
    private string _suaEmail = string.Empty;
    private string _suaDiaChi = string.Empty;
    private string _suaChucVu = string.Empty;
    private bool _suaDangHoatDong = true;

    public ObservableCollection<NhanVienDto> DanhSachNhanVien { get => _danhSachNhanVien; set => GanGiaTri(ref _danhSachNhanVien, value); }
    public string CotSapXep { get => _cotSapXep; set => GanGiaTri(ref _cotSapXep, value); }
    public bool SapXepTang { get => _sapXepTang; set => GanGiaTri(ref _sapXepTang, value); }
    public NhanVienDto? NhanVienDuocChon { get => _nhanVienDuocChon; set { if (GanGiaTri(ref _nhanVienDuocChon, value) && value != null) TaiDeSua(value); } }
    public string TimKiem { get => _timKiem; set { if (GanGiaTri(ref _timKiem, value)) TimKiemNhanVien(); } }
    public bool DangSua { get => _dangSua; set => GanGiaTri(ref _dangSua, value); }
    public string ThongBaoXacThuc { get => _thongBaoXacThuc; set => GanGiaTri(ref _thongBaoXacThuc, value); }

    public string SuaMaNhanVien { get => _suaMaNhanVien; set => GanGiaTri(ref _suaMaNhanVien, value); }
    public string SuaHoTen { get => _suaHoTen; set => GanGiaTri(ref _suaHoTen, value); }
    public string? SuaGioiTinh { get => _suaGioiTinh; set => GanGiaTri(ref _suaGioiTinh, value); }
    public DateTime? SuaNgaySinh { get => _suaNgaySinh; set => GanGiaTri(ref _suaNgaySinh, value); }
    public string SuaDienThoai { get => _suaDienThoai; set => GanGiaTri(ref _suaDienThoai, value); }
    public string SuaEmail { get => _suaEmail; set => GanGiaTri(ref _suaEmail, value); }
    public string SuaDiaChi { get => _suaDiaChi; set => GanGiaTri(ref _suaDiaChi, value); }
    public string SuaChucVu { get => _suaChucVu; set => GanGiaTri(ref _suaChucVu, value); }
    public bool SuaDangHoatDong { get => _suaDangHoatDong; set => GanGiaTri(ref _suaDangHoatDong, value); }

    public ICommand LenhThem { get; }
    public ICommand LenhLuu { get; }
    public ICommand LenhXoa { get; }
    public ICommand LenhHuy { get; }
    public ICommand LenhSapXep { get; }

    public QuanLyNhanVienViewModel()
    {
        LenhThem = new LenhRelay(_ => ThucHienThem());
        LenhLuu = new LenhRelay(_ => ThucHienLuu());
        LenhXoa = new LenhRelay(_ => ThucHienXoa());
        LenhHuy = new LenhRelay(_ => ThucHienHuy());
        LenhSapXep = new LenhRelay(ThucHienSapXep);
        DangTai = true;
        TaiAsync();
    }

    private async void TaiAsync()
    {
        try { await Task.Run(() => TaiDanhSach()); }
        catch { }
        finally { DangTai = false; }
    }

    private void TaiDanhSach()
    {
        var sapXep = $"order={CotSapXep}.{(SapXepTang ? "asc" : "desc")}";
        var nhanVien = SupabaseClient.LayDanhSach<NhanVienDto>("employees", sapXep);
        DanhSachNhanVien = new ObservableCollection<NhanVienDto>(nhanVien);
    }

    private void TimKiemNhanVien()
    {
        try
        {
            var sapXep = $"order={CotSapXep}.{(SapXepTang ? "asc" : "desc")}";
            var truyVan = sapXep;
            if (!string.IsNullOrWhiteSpace(TimKiem))
                truyVan = $"or=(full_name.ilike.%25{Uri.EscapeDataString(TimKiem)}%25,employee_code.ilike.%25{Uri.EscapeDataString(TimKiem)}%25,phone.ilike.%25{Uri.EscapeDataString(TimKiem)}%25)&{sapXep}";
            DanhSachNhanVien = new ObservableCollection<NhanVienDto>(SupabaseClient.LayDanhSach<NhanVienDto>("employees", truyVan));
        }
        catch { }
    }

    private void ThucHienSapXep(object? thamSo)
    {
        var cot = thamSo?.ToString();
        if (string.IsNullOrEmpty(cot)) return;
        if (CotSapXep == cot)
            SapXepTang = !SapXepTang;
        else
        {
            CotSapXep = cot;
            SapXepTang = true;
        }
        TimKiemNhanVien();
    }

    private void TaiDeSua(NhanVienDto nv)
    {
        SuaMaNhanVien = nv.MaNhanVien; SuaHoTen = nv.HoTen;
        SuaGioiTinh = nv.GioiTinh; SuaNgaySinh = nv.NgaySinh;
        SuaDienThoai = nv.DienThoai ?? ""; SuaEmail = nv.Email ?? "";
        SuaDiaChi = nv.DiaChi ?? ""; SuaChucVu = nv.ChucVu ?? "";
        SuaDangHoatDong = nv.DangHoatDong; DangSua = true; ThongBaoXacThuc = "";
    }

    private void ThucHienThem()
    {
        NhanVienDuocChon = null;
        SuaMaNhanVien = SuaHoTen = SuaDienThoai = SuaEmail = SuaDiaChi = SuaChucVu = "";
        SuaGioiTinh = null; SuaNgaySinh = null; SuaDangHoatDong = true;
        DangSua = true; ThongBaoXacThuc = "";
    }

    private void ThucHienLuu()
    {
        ThongBaoXacThuc = "";
        if (string.IsNullOrWhiteSpace(SuaMaNhanVien)) { ThongBaoXacThuc = "Mã nhân viên không được để trống"; return; }
        if (string.IsNullOrWhiteSpace(SuaHoTen)) { ThongBaoXacThuc = "Họ tên không được để trống"; return; }

        try
        {
            var daTonTai = SupabaseClient.LayDanhSach<NhanVienDto>("employees",
                $"employee_code=eq.{Uri.EscapeDataString(SuaMaNhanVien)}&id=neq.{NhanVienDuocChon?.Id ?? 0}&select=id");
            if (daTonTai.Any()) { ThongBaoXacThuc = "Mã nhân viên đã tồn tại"; return; }

            var duLieu = new
            {
                employee_code = SuaMaNhanVien,
                full_name = SuaHoTen,
                gender = string.IsNullOrWhiteSpace(SuaGioiTinh) ? (string?)null : SuaGioiTinh,
                date_of_birth = SuaNgaySinh?.ToString("yyyy-MM-dd"),
                phone = string.IsNullOrWhiteSpace(SuaDienThoai) ? (string?)null : SuaDienThoai,
                email = string.IsNullOrWhiteSpace(SuaEmail) ? (string?)null : SuaEmail,
                address = string.IsNullOrWhiteSpace(SuaDiaChi) ? (string?)null : SuaDiaChi,
                position = string.IsNullOrWhiteSpace(SuaChucVu) ? (string?)null : SuaChucVu,
                is_active = SuaDangHoatDong
            };

            if (NhanVienDuocChon != null)
                SupabaseClient.CapNhat("employees", $"id=eq.{NhanVienDuocChon.Id}", duLieu);
            else
                SupabaseClient.ThemMoi<NhanVienDto>("employees", duLieu);

            TaiDanhSach(); ThucHienHuy();
            DichVuThongBao.ThanhCong("Lưu thành công!");
        }
        catch (Exception ex) { ThongBaoXacThuc = $"Lỗi: {ex.Message}"; }
    }

    private void ThucHienXoa()
    {
        if (NhanVienDuocChon == null) return;
        if (MessageBox.Show($"Xóa nhân viên '{NhanVienDuocChon.HoTen}'?", "Xác nhận", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
        {
            try { SupabaseClient.Xoa("employees", $"id=eq.{NhanVienDuocChon.Id}"); TaiDanhSach(); ThucHienHuy(); }
            catch (Exception ex) { ThongBaoXacThuc = $"Lỗi: {ex.Message}"; }
        }
    }

    private void ThucHienHuy() { DangSua = false; NhanVienDuocChon = null; }
}
