using System.Collections.ObjectModel;
using System.Windows.Input;
using VClothes.Data;

namespace VClothes.ViewModels;

public class HoaDonBanViewModel : ViewModelCha
{
    private ObservableCollection<HoaDonBanDto> _danhSachHoaDon = new();
    private ObservableCollection<KhachHangDto> _danhSachKhachHang = new();
    private ObservableCollection<SanPhamDto> _danhSachSanPham = new();
    private ObservableCollection<MucChiTietHoaDon> _danhSachChiTiet = new();
    private ObservableCollection<MucChiTietHoaDon> _chiTietHoaDonDuocChon = new();
    private HoaDonBanDto? _hoaDonDuocChon;
    private bool _dangXemChiTiet;
    private string _timKiem = string.Empty;
    private bool _dangTao;
    private string _thongBaoXacThuc = string.Empty;
    private DateTime? _locTuNgay;
    private DateTime? _locDenNgay;
    private string _cotSapXep = "invoice_date";
    private bool _sapXepTang = false;

    private string _suaMaHoaDon = string.Empty;
    private DateTime _suaNgayBan = DateTime.Now;
    private KhachHangDto? _suaKhachHang;
    private string _suaGhiChu = string.Empty;
    private SanPhamDto? _sanPhamDuocChon;
    private int _suaSoLuong = 1;
    private decimal _suaDonGia;
    private decimal _suaGiamGia;
    private decimal _tongTien;
    private decimal _thanhToan;

    public ObservableCollection<HoaDonBanDto> DanhSachHoaDon { get => _danhSachHoaDon; set => GanGiaTri(ref _danhSachHoaDon, value); }
    public ObservableCollection<KhachHangDto> DanhSachKhachHang { get => _danhSachKhachHang; set => GanGiaTri(ref _danhSachKhachHang, value); }
    public ObservableCollection<SanPhamDto> DanhSachSanPham { get => _danhSachSanPham; set => GanGiaTri(ref _danhSachSanPham, value); }
    public ObservableCollection<MucChiTietHoaDon> DanhSachChiTiet { get => _danhSachChiTiet; set => GanGiaTri(ref _danhSachChiTiet, value); }
    public ObservableCollection<MucChiTietHoaDon> ChiTietHoaDonDuocChon { get => _chiTietHoaDonDuocChon; set => GanGiaTri(ref _chiTietHoaDonDuocChon, value); }
    public HoaDonBanDto? HoaDonDuocChon { get => _hoaDonDuocChon; set { if (GanGiaTri(ref _hoaDonDuocChon, value)) TaiChiTietHoaDon(); } }
    public bool DangXemChiTiet { get => _dangXemChiTiet; set => GanGiaTri(ref _dangXemChiTiet, value); }
    public string TimKiem { get => _timKiem; set { if (GanGiaTri(ref _timKiem, value)) TimKiemHoaDon(); } }
    public bool DangTao { get => _dangTao; set => GanGiaTri(ref _dangTao, value); }
    public string ThongBaoXacThuc { get => _thongBaoXacThuc; set => GanGiaTri(ref _thongBaoXacThuc, value); }
    public DateTime? LocTuNgay { get => _locTuNgay; set { if (GanGiaTri(ref _locTuNgay, value)) TimKiemHoaDon(); } }
    public DateTime? LocDenNgay { get => _locDenNgay; set { if (GanGiaTri(ref _locDenNgay, value)) TimKiemHoaDon(); } }
    public string CotSapXep { get => _cotSapXep; set => GanGiaTri(ref _cotSapXep, value); }
    public bool SapXepTang { get => _sapXepTang; set => GanGiaTri(ref _sapXepTang, value); }

    public string SuaMaHoaDon { get => _suaMaHoaDon; set => GanGiaTri(ref _suaMaHoaDon, value); }
    public DateTime SuaNgayBan { get => _suaNgayBan; set => GanGiaTri(ref _suaNgayBan, value); }
    public KhachHangDto? SuaKhachHang { get => _suaKhachHang; set => GanGiaTri(ref _suaKhachHang, value); }
    public string SuaGhiChu { get => _suaGhiChu; set => GanGiaTri(ref _suaGhiChu, value); }
    public SanPhamDto? SanPhamDuocChon { get => _sanPhamDuocChon; set { if (GanGiaTri(ref _sanPhamDuocChon, value) && value != null) SuaDonGia = value.GiaBan; } }
    public int SuaSoLuong { get => _suaSoLuong; set => GanGiaTri(ref _suaSoLuong, value); }
    public decimal SuaDonGia { get => _suaDonGia; set => GanGiaTri(ref _suaDonGia, value); }
    public decimal SuaGiamGia { get => _suaGiamGia; set { if (GanGiaTri(ref _suaGiamGia, value)) TinhThanhToan(); } }
    public decimal TongTien { get => _tongTien; set { if (GanGiaTri(ref _tongTien, value)) TinhThanhToan(); } }
    public decimal ThanhToan { get => _thanhToan; set => GanGiaTri(ref _thanhToan, value); }

    public ICommand LenhTao { get; }
    public ICommand LenhLuu { get; }
    public ICommand LenhHuy { get; }
    public ICommand LenhThemChiTiet { get; }
    public ICommand LenhXemChiTiet { get; }
    public ICommand LenhDongChiTiet { get; }
    public ICommand LenhSapXep { get; }

    public HoaDonBanViewModel()
    {
        LenhTao = new LenhRelay(_ => ThucHienTao());
        LenhLuu = new LenhRelay(_ => ThucHienLuu());
        LenhHuy = new LenhRelay(_ => { DangTao = false; });
        LenhThemChiTiet = new LenhRelay(_ => ThucHienThemChiTiet());
        LenhXemChiTiet = new LenhRelay(ThucHienXemChiTiet);
        LenhDongChiTiet = new LenhRelay(_ => { DangXemChiTiet = false; HoaDonDuocChon = null; });
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
        DanhSachHoaDon = new ObservableCollection<HoaDonBanDto>(
            SupabaseClient.LayDanhSach<HoaDonBanDto>("sales_invoices", $"select=*,customers(*)&{sapXep}"));
        DanhSachKhachHang = new ObservableCollection<KhachHangDto>(
            SupabaseClient.LayDanhSach<KhachHangDto>("customers", "order=full_name.asc"));
        DanhSachSanPham = new ObservableCollection<SanPhamDto>(
            SupabaseClient.LayDanhSach<SanPhamDto>("products", "is_active=eq.true&stock_quantity=gt.0&order=name.asc"));
    }

    private void TimKiemHoaDon()
    {
        try
        {
            var sapXep = $"order={CotSapXep}.{(SapXepTang ? "asc" : "desc")}";
            var boLoc = new List<string> { "select=*,customers(*)", sapXep };
            if (!string.IsNullOrWhiteSpace(TimKiem))
                boLoc.Add($"invoice_code=ilike.%25{Uri.EscapeDataString(TimKiem)}%25");
            if (LocTuNgay.HasValue)
                boLoc.Add($"invoice_date=gte.{LocTuNgay.Value:yyyy-MM-dd}");
            if (LocDenNgay.HasValue)
                boLoc.Add($"invoice_date=lte.{LocDenNgay.Value:yyyy-MM-dd}");
            DanhSachHoaDon = new ObservableCollection<HoaDonBanDto>(
                SupabaseClient.LayDanhSach<HoaDonBanDto>("sales_invoices", string.Join("&", boLoc)));
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
            SapXepTang = cot == "invoice_date" ? false : true;
        }
        TimKiemHoaDon();
    }

    private void TinhThanhToan() { ThanhToan = Math.Max(0, TongTien - SuaGiamGia); }

    private void ThucHienTao()
    {
        var soLuong = DanhSachHoaDon.Count + 1;
        SuaMaHoaDon = $"HD{soLuong:D3}";
        SuaNgayBan = DateTime.Now;
        SuaKhachHang = null; SuaGhiChu = ""; SuaGiamGia = 0;
        DanhSachChiTiet = new ObservableCollection<MucChiTietHoaDon>();
        TongTien = 0; ThanhToan = 0; DangTao = true; ThongBaoXacThuc = "";
    }

    private void ThucHienThemChiTiet()
    {
        ThongBaoXacThuc = "";
        if (SanPhamDuocChon == null) { ThongBaoXacThuc = "Vui lòng chọn sản phẩm"; return; }
        if (SuaSoLuong <= 0) { ThongBaoXacThuc = "Số lượng phải lớn hơn 0"; return; }
        if (SuaDonGia <= 0) { ThongBaoXacThuc = "Đơn giá phải lớn hơn 0"; return; }
        if (SuaSoLuong > SanPhamDuocChon.SoLuongTon) { ThongBaoXacThuc = $"Tồn kho chỉ còn {SanPhamDuocChon.SoLuongTon}"; return; }

        DanhSachChiTiet.Add(new MucChiTietHoaDon
        {
            SanPhamId = SanPhamDuocChon.Id,
            TenSanPham = SanPhamDuocChon.Ten,
            SoLuong = SuaSoLuong,
            DonGia = SuaDonGia
        });
        TongTien = DanhSachChiTiet.Sum(d => d.ThanhTien);
        SanPhamDuocChon = null; SuaSoLuong = 1; SuaDonGia = 0;
    }

    private void ThucHienLuu()
    {
        ThongBaoXacThuc = "";
        if (string.IsNullOrWhiteSpace(SuaMaHoaDon)) { ThongBaoXacThuc = "Mã hóa đơn không được để trống"; return; }
        if (!DanhSachChiTiet.Any()) { ThongBaoXacThuc = "Vui lòng thêm ít nhất 1 sản phẩm"; return; }

        try
        {
            var hoaDon = SupabaseClient.ThemMoi<HoaDonBanDto>("sales_invoices", new
            {
                invoice_code = SuaMaHoaDon,
                invoice_date = SuaNgayBan.ToString("yyyy-MM-dd"),
                customer_id = SuaKhachHang?.Id,
                employee_id = 2,
                total_amount = TongTien,
                discount = SuaGiamGia,
                final_amount = ThanhToan,
                note = string.IsNullOrWhiteSpace(SuaGhiChu) ? (string?)null : SuaGhiChu
            });

            if (hoaDon != null)
            {
                foreach (var ct in DanhSachChiTiet)
                {
                    SupabaseClient.ThemMoi<ChiTietHoaDonBanDto>("sales_invoice_details", new
                    {
                        sales_invoice_id = hoaDon.Id,
                        product_id = ct.SanPhamId,
                        quantity = ct.SoLuong,
                        unit_price = ct.DonGia,
                        discount = 0,
                        sub_total = ct.ThanhTien
                    });

                    var sanPham = SupabaseClient.LayDanhSach<SanPhamDto>("products", $"id=eq.{ct.SanPhamId}&select=id,stock_quantity").FirstOrDefault();
                    if (sanPham != null)
                        SupabaseClient.CapNhat("products", $"id=eq.{ct.SanPhamId}", new { stock_quantity = sanPham.SoLuongTon - ct.SoLuong });
                }
            }

            TaiDuLieu(); DangTao = false; ThongBaoXacThuc = "Lưu hóa đơn thành công!";
        }
        catch (Exception ex) { ThongBaoXacThuc = $"Lỗi: {ex.Message}"; }
    }

    private void ThucHienXemChiTiet(object? thamSo)
    {
        if (thamSo is HoaDonBanDto hoaDon)
        {
            if (HoaDonDuocChon?.Id == hoaDon.Id && DangXemChiTiet)
            {
                DangXemChiTiet = false;
                HoaDonDuocChon = null;
                return;
            }
            HoaDonDuocChon = hoaDon;
        }
    }

    private void TaiChiTietHoaDon()
    {
        if (HoaDonDuocChon == null) { DangXemChiTiet = false; return; }

        try
        {
            var chiTiet = SupabaseClient.LayDanhSach<ChiTietHoaDonBanDto>("sales_invoice_details",
                $"sales_invoice_id=eq.{HoaDonDuocChon.Id}&select=*,products(name,product_code)");

            ChiTietHoaDonDuocChon = new ObservableCollection<MucChiTietHoaDon>(
                chiTiet.Select(ct => new MucChiTietHoaDon
                {
                    SanPhamId = ct.SanPhamId,
                    TenSanPham = ct.SanPham?.Ten ?? $"SP #{ct.SanPhamId}",
                    SoLuong = ct.SoLuong,
                    DonGia = ct.DonGia
                }));
            DangXemChiTiet = true;
        }
        catch { DangXemChiTiet = false; }
    }
}
