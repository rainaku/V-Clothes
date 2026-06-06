using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using VClothes.Data;
using VClothes.Services;

namespace VClothes.ViewModels;

public class QuanLyNhaCungCapViewModel : ViewModelCha
{
    private ObservableCollection<NhaCungCapDto> _danhSachNCC = new();
    private NhaCungCapDto? _nccDuocChon;
    private string _timKiem = string.Empty;
    private string _suaTen = string.Empty;
    private string _suaDiaChi = string.Empty;
    private string _suaDienThoai = string.Empty;
    private string _suaEmail = string.Empty;
    private string _suaGhiChu = string.Empty;
    private bool _suaDangHoatDong = true;
    private bool _dangSua;
    private string _thongBaoXacThuc = string.Empty;
    private string _cotSapXep = "name";
    private bool _sapXepTang = true;

    public ObservableCollection<NhaCungCapDto> DanhSachNCC { get => _danhSachNCC; set => GanGiaTri(ref _danhSachNCC, value); }
    public string CotSapXep { get => _cotSapXep; set => GanGiaTri(ref _cotSapXep, value); }
    public bool SapXepTang { get => _sapXepTang; set => GanGiaTri(ref _sapXepTang, value); }
    public NhaCungCapDto? NccDuocChon
    {
        get => _nccDuocChon;
        set
        {
            if (GanGiaTri(ref _nccDuocChon, value) && value != null)
            {
                SuaTen = value.Ten;
                SuaDiaChi = value.DiaChi ?? string.Empty;
                SuaDienThoai = value.DienThoai ?? string.Empty;
                SuaEmail = value.Email ?? string.Empty;
                SuaGhiChu = value.GhiChu ?? string.Empty;
                SuaDangHoatDong = value.DangHoatDong;
                DangSua = true;
                ThongBaoXacThuc = string.Empty;
            }
        }
    }
    public string TimKiem { get => _timKiem; set { if (GanGiaTri(ref _timKiem, value)) TimKiemNCC(); } }
    public string SuaTen { get => _suaTen; set => GanGiaTri(ref _suaTen, value); }
    public string SuaDiaChi { get => _suaDiaChi; set => GanGiaTri(ref _suaDiaChi, value); }
    public string SuaDienThoai { get => _suaDienThoai; set => GanGiaTri(ref _suaDienThoai, value); }
    public string SuaEmail { get => _suaEmail; set => GanGiaTri(ref _suaEmail, value); }
    public string SuaGhiChu { get => _suaGhiChu; set => GanGiaTri(ref _suaGhiChu, value); }
    public bool SuaDangHoatDong { get => _suaDangHoatDong; set => GanGiaTri(ref _suaDangHoatDong, value); }
    public bool DangSua { get => _dangSua; set => GanGiaTri(ref _dangSua, value); }
    public string ThongBaoXacThuc { get => _thongBaoXacThuc; set => GanGiaTri(ref _thongBaoXacThuc, value); }

    public ICommand LenhThem { get; }
    public ICommand LenhLuu { get; }
    public ICommand LenhXoa { get; }
    public ICommand LenhHuy { get; }
    public ICommand LenhSapXep { get; }

    public QuanLyNhaCungCapViewModel()
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
        var danhSach = SupabaseClient.LayDanhSach<NhaCungCapDto>("suppliers", sapXep);
        DanhSachNCC = new ObservableCollection<NhaCungCapDto>(danhSach);
    }

    private void TimKiemNCC()
    {
        try
        {
            var sapXep = $"order={CotSapXep}.{(SapXepTang ? "asc" : "desc")}";
            var truyVan = sapXep;
            if (!string.IsNullOrWhiteSpace(TimKiem))
                truyVan = $"or=(name.ilike.%25{Uri.EscapeDataString(TimKiem)}%25,phone.ilike.%25{Uri.EscapeDataString(TimKiem)}%25,email.ilike.%25{Uri.EscapeDataString(TimKiem)}%25)&{sapXep}";
            var danhSach = SupabaseClient.LayDanhSach<NhaCungCapDto>("suppliers", truyVan);
            DanhSachNCC = new ObservableCollection<NhaCungCapDto>(danhSach);
            if (!danhSach.Any() && !string.IsNullOrWhiteSpace(TimKiem))
                ThongBaoXacThuc = "Không tìm thấy nhà cung cấp nào phù hợp";
            else
                ThongBaoXacThuc = string.Empty;
        }
        catch { }
    }

    private void ThucHienSapXep(object? thamSo)
    {
        var cot = thamSo?.ToString();
        if (string.IsNullOrEmpty(cot)) return;
        if (CotSapXep == cot) SapXepTang = !SapXepTang;
        else { CotSapXep = cot; SapXepTang = true; }
        TimKiemNCC();
    }

    private void ThucHienThem()
    {
        NccDuocChon = null;
        SuaTen = SuaDiaChi = SuaDienThoai = SuaEmail = SuaGhiChu = string.Empty;
        SuaDangHoatDong = true; DangSua = true; ThongBaoXacThuc = string.Empty;
    }

    private void ThucHienLuu()
    {
        ThongBaoXacThuc = string.Empty;
        if (string.IsNullOrWhiteSpace(SuaTen))
        { ThongBaoXacThuc = "Tên nhà cung cấp không được để trống"; return; }
        if (!string.IsNullOrWhiteSpace(SuaEmail) && !SuaEmail.Contains('@'))
        { ThongBaoXacThuc = "Email không hợp lệ"; return; }
        try
        {
            var duLieu = new
            {
                name = SuaTen,
                address = string.IsNullOrWhiteSpace(SuaDiaChi) ? (string?)null : SuaDiaChi,
                phone = string.IsNullOrWhiteSpace(SuaDienThoai) ? (string?)null : SuaDienThoai,
                email = string.IsNullOrWhiteSpace(SuaEmail) ? (string?)null : SuaEmail,
                note = string.IsNullOrWhiteSpace(SuaGhiChu) ? (string?)null : SuaGhiChu,
                is_active = SuaDangHoatDong
            };
            if (NccDuocChon != null)
                SupabaseClient.CapNhat("suppliers", $"id=eq.{NccDuocChon.Id}", duLieu);
            else
                SupabaseClient.ThemMoi<NhaCungCapDto>("suppliers", duLieu);
            TaiDanhSach(); ThucHienHuy();
            DichVuThongBao.ThanhCong("Lưu thành công!");
        }
        catch (Exception ex) { ThongBaoXacThuc = $"Lỗi: {ex.Message}"; }
    }

    private void ThucHienXoa()
    {
        if (NccDuocChon == null) return;
        var ketQua = MessageBox.Show($"Bạn có chắc muốn xóa nhà cung cấp '{NccDuocChon.Ten}'?",
            "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (ketQua == MessageBoxResult.Yes)
        {
            try
            {
                var coSanPham = SupabaseClient.LayDanhSach<SanPhamDto>("products", $"supplier_id=eq.{NccDuocChon.Id}&select=id&limit=1");
                if (coSanPham.Any())
                { ThongBaoXacThuc = "Không thể xóa nhà cung cấp đang có sản phẩm liên kết"; return; }
                SupabaseClient.Xoa("suppliers", $"id=eq.{NccDuocChon.Id}");
                TaiDanhSach(); ThucHienHuy();
            }
            catch (Exception ex) { ThongBaoXacThuc = $"Lỗi: {ex.Message}"; }
        }
    }

    private void ThucHienHuy()
    {
        NccDuocChon = null;
        SuaTen = SuaDiaChi = SuaDienThoai = SuaEmail = SuaGhiChu = string.Empty;
        SuaDangHoatDong = true; DangSua = false;
    }
}
