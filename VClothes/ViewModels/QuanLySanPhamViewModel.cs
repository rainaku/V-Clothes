using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using VClothes.Data;
using VClothes.Services;

namespace VClothes.ViewModels;

public class QuanLySanPhamViewModel : ViewModelCha
{
    private ObservableCollection<SanPhamDto> _danhSachSanPham = new();
    private ObservableCollection<LoaiSanPhamDto> _danhSachLoai = new();
    private ObservableCollection<LoaiSanPhamDto> _tatCaLoai = new();
    private ObservableCollection<NhaCungCapDto> _tatCaNCC = new();
    private SanPhamDto? _sanPhamDuocChon;
    private LoaiSanPhamDto? _loaiLocDuocChon;
    private string _timKiem = string.Empty;
    private bool _dangSua;
    private string _thongBaoXacThuc = string.Empty;
    private string _cotSapXep = "name";
    private bool _sapXepTang = true;

    private string _suaMaSanPham = string.Empty;
    private string _suaTen = string.Empty;
    private string _suaMoTa = string.Empty;
    private decimal _suaGiaBan;
    private decimal _suaGiaNhap;
    private string _suaKichCo = string.Empty;
    private string _suaMauSac = string.Empty;
    private string _suaChatLieu = string.Empty;
    private string _suaDuongDanAnh = string.Empty;
    private LoaiSanPhamDto? _suaLoai;
    private NhaCungCapDto? _suaNCC;
    private bool _suaDangHoatDong = true;

    public ObservableCollection<SanPhamDto> DanhSachSanPham { get => _danhSachSanPham; set => GanGiaTri(ref _danhSachSanPham, value); }
    public ObservableCollection<LoaiSanPhamDto> DanhSachLoai { get => _danhSachLoai; set => GanGiaTri(ref _danhSachLoai, value); }
    public ObservableCollection<LoaiSanPhamDto> TatCaLoai { get => _tatCaLoai; set => GanGiaTri(ref _tatCaLoai, value); }
    public ObservableCollection<NhaCungCapDto> TatCaNCC { get => _tatCaNCC; set => GanGiaTri(ref _tatCaNCC, value); }
    public SanPhamDto? SanPhamDuocChon { get => _sanPhamDuocChon; set { if (GanGiaTri(ref _sanPhamDuocChon, value) && value != null) TaiSanPhamDeSua(value); } }
    public LoaiSanPhamDto? LoaiLocDuocChon { get => _loaiLocDuocChon; set { if (GanGiaTri(ref _loaiLocDuocChon, value)) TimKiemSanPham(); } }
    public string TimKiem { get => _timKiem; set { if (GanGiaTri(ref _timKiem, value)) TimKiemSanPham(); } }
    public bool DangSua { get => _dangSua; set => GanGiaTri(ref _dangSua, value); }
    public string ThongBaoXacThuc { get => _thongBaoXacThuc; set => GanGiaTri(ref _thongBaoXacThuc, value); }
    public string CotSapXep { get => _cotSapXep; set => GanGiaTri(ref _cotSapXep, value); }
    public bool SapXepTang { get => _sapXepTang; set => GanGiaTri(ref _sapXepTang, value); }

    public string SuaMaSanPham { get => _suaMaSanPham; set => GanGiaTri(ref _suaMaSanPham, value); }
    public string SuaTen { get => _suaTen; set => GanGiaTri(ref _suaTen, value); }
    public string SuaMoTa { get => _suaMoTa; set => GanGiaTri(ref _suaMoTa, value); }
    public decimal SuaGiaBan { get => _suaGiaBan; set => GanGiaTri(ref _suaGiaBan, value); }
    public decimal SuaGiaNhap { get => _suaGiaNhap; set => GanGiaTri(ref _suaGiaNhap, value); }
    public string SuaKichCo { get => _suaKichCo; set => GanGiaTri(ref _suaKichCo, value); }
    public string SuaMauSac { get => _suaMauSac; set => GanGiaTri(ref _suaMauSac, value); }
    public string SuaChatLieu { get => _suaChatLieu; set => GanGiaTri(ref _suaChatLieu, value); }
    public string SuaDuongDanAnh { get => _suaDuongDanAnh; set => GanGiaTri(ref _suaDuongDanAnh, value); }
    public LoaiSanPhamDto? SuaLoai { get => _suaLoai; set => GanGiaTri(ref _suaLoai, value); }
    public NhaCungCapDto? SuaNCC { get => _suaNCC; set => GanGiaTri(ref _suaNCC, value); }
    public bool SuaDangHoatDong { get => _suaDangHoatDong; set => GanGiaTri(ref _suaDangHoatDong, value); }

    public ICommand LenhThem { get; }
    public ICommand LenhLuu { get; }
    public ICommand LenhXoa { get; }
    public ICommand LenhHuy { get; }
    public ICommand LenhChonAnh { get; }
    public ICommand LenhSapXep { get; }

    public QuanLySanPhamViewModel()
    {
        LenhThem = new LenhRelay(_ => ThucHienThem());
        LenhLuu = new LenhRelay(_ => ThucHienLuu());
        LenhXoa = new LenhRelay(_ => ThucHienXoa());
        LenhHuy = new LenhRelay(_ => ThucHienHuy());
        LenhChonAnh = new LenhRelay(_ => ThucHienChonAnh());
        LenhSapXep = new LenhRelay(ThucHienSapXep);
        DangTai = true;
        TaiAsync();
    }

    private async void TaiAsync()
    {
        try { await Task.Run(() => TaiDuLieu()); }
        catch { }
        finally { DangTai = false; }
    }

    private void TaiDuLieu()
    {
        var sapXep = $"order={CotSapXep}.{(SapXepTang ? "asc" : "desc")}";
        var sanPham = SupabaseClient.LayDanhSach<SanPhamDto>("products", $"select=*,categories(*),suppliers(*)&{sapXep}");
        DanhSachSanPham = new ObservableCollection<SanPhamDto>(sanPham);

        var loai = SupabaseClient.LayDanhSach<LoaiSanPhamDto>("categories", "is_active=eq.true&order=name.asc");
        TatCaLoai = new ObservableCollection<LoaiSanPhamDto>(loai);
        DanhSachLoai = new ObservableCollection<LoaiSanPhamDto>(loai);

        var ncc = SupabaseClient.LayDanhSach<NhaCungCapDto>("suppliers", "is_active=eq.true&order=name.asc");
        TatCaNCC = new ObservableCollection<NhaCungCapDto>(ncc);
    }

    private void TimKiemSanPham()
    {
        try
        {
            var sapXep = $"order={CotSapXep}.{(SapXepTang ? "asc" : "desc")}";
            var boLoc = new List<string> { "select=*,categories(*),suppliers(*)", sapXep };

            if (!string.IsNullOrWhiteSpace(TimKiem))
                boLoc.Add($"or=(name.ilike.%25{Uri.EscapeDataString(TimKiem)}%25,product_code.ilike.%25{Uri.EscapeDataString(TimKiem)}%25)");
            if (LoaiLocDuocChon != null)
                boLoc.Add($"category_id=eq.{LoaiLocDuocChon.Id}");

            var sanPham = SupabaseClient.LayDanhSach<SanPhamDto>("products", string.Join("&", boLoc));
            DanhSachSanPham = new ObservableCollection<SanPhamDto>(sanPham);
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
        TimKiemSanPham();
    }

    private void TaiSanPhamDeSua(SanPhamDto sp)
    {
        SuaMaSanPham = sp.MaSanPham;
        SuaTen = sp.Ten;
        SuaMoTa = sp.MoTa ?? string.Empty;
        SuaGiaBan = sp.GiaBan;
        SuaGiaNhap = sp.GiaNhap;
        SuaKichCo = sp.KichCo ?? string.Empty;
        SuaMauSac = sp.MauSac ?? string.Empty;
        SuaChatLieu = sp.ChatLieu ?? string.Empty;
        SuaDuongDanAnh = sp.DuongDanAnh ?? string.Empty;
        SuaLoai = TatCaLoai.FirstOrDefault(c => c.Id == sp.LoaiSanPhamId);
        SuaNCC = TatCaNCC.FirstOrDefault(s => s.Id == sp.NhaCungCapId);
        SuaDangHoatDong = sp.DangHoatDong;
        DangSua = true;
        ThongBaoXacThuc = string.Empty;
    }

    private void ThucHienThem()
    {
        SanPhamDuocChon = null;
        SuaMaSanPham = SuaTen = SuaMoTa = SuaKichCo = SuaMauSac = SuaChatLieu = SuaDuongDanAnh = string.Empty;
        SuaGiaBan = SuaGiaNhap = 0;
        SuaLoai = null; SuaNCC = null;
        SuaDangHoatDong = true; DangSua = true; ThongBaoXacThuc = string.Empty;
    }

    private void ThucHienLuu()
    {
        ThongBaoXacThuc = string.Empty;
        if (string.IsNullOrWhiteSpace(SuaMaSanPham)) { ThongBaoXacThuc = "Mã sản phẩm không được để trống"; return; }
        if (string.IsNullOrWhiteSpace(SuaTen)) { ThongBaoXacThuc = "Tên sản phẩm không được để trống"; return; }
        if (SuaLoai == null) { ThongBaoXacThuc = "Vui lòng chọn loại sản phẩm"; return; }
        if (SuaNCC == null) { ThongBaoXacThuc = "Vui lòng chọn nhà cung cấp"; return; }
        if (SuaGiaBan <= 0) { ThongBaoXacThuc = "Giá bán phải lớn hơn 0"; return; }

        try
        {
            var daTonTai = SupabaseClient.LayDanhSach<SanPhamDto>("products",
                $"product_code=eq.{Uri.EscapeDataString(SuaMaSanPham)}&id=neq.{SanPhamDuocChon?.Id ?? 0}&select=id");
            if (daTonTai.Any()) { ThongBaoXacThuc = "Mã sản phẩm đã tồn tại"; return; }

            var duLieu = new
            {
                product_code = SuaMaSanPham,
                name = SuaTen,
                description = string.IsNullOrWhiteSpace(SuaMoTa) ? (string?)null : SuaMoTa,
                price = SuaGiaBan,
                cost_price = SuaGiaNhap,
                size = string.IsNullOrWhiteSpace(SuaKichCo) ? (string?)null : SuaKichCo,
                color = string.IsNullOrWhiteSpace(SuaMauSac) ? (string?)null : SuaMauSac,
                material = string.IsNullOrWhiteSpace(SuaChatLieu) ? (string?)null : SuaChatLieu,
                image_path = string.IsNullOrWhiteSpace(SuaDuongDanAnh) ? (string?)null : SuaDuongDanAnh,
                category_id = SuaLoai.Id,
                supplier_id = SuaNCC.Id,
                is_active = SuaDangHoatDong
            };

            if (SanPhamDuocChon != null)
                SupabaseClient.CapNhat("products", $"id=eq.{SanPhamDuocChon.Id}", duLieu);
            else
                SupabaseClient.ThemMoi<SanPhamDto>("products", duLieu);

            TaiDuLieu();
            ThucHienHuy();
            DichVuThongBao.ThanhCong("Lưu thành công!");
        }
        catch (Exception ex) { ThongBaoXacThuc = $"Lỗi: {ex.Message}"; }
    }

    private void ThucHienXoa()
    {
        if (SanPhamDuocChon == null) return;
        if (MessageBox.Show($"Bạn có chắc muốn xóa '{SanPhamDuocChon.Ten}'?", "Xác nhận", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
        {
            try
            {
                SupabaseClient.Xoa("products", $"id=eq.{SanPhamDuocChon.Id}");
                TaiDuLieu(); ThucHienHuy();
            }
            catch (Exception ex) { ThongBaoXacThuc = $"Lỗi: {ex.Message}"; }
        }
    }

    private void ThucHienHuy() { DangSua = false; SanPhamDuocChon = null; }

    private void ThucHienChonAnh()
    {
        var dialog = new OpenFileDialog { Filter = "Image files|*.png;*.jpg;*.jpeg;*.bmp" };
        if (dialog.ShowDialog() == true) SuaDuongDanAnh = dialog.FileName;
    }
}
