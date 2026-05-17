using System.Collections.ObjectModel;
using System.Windows.Input;
using VClothes.Data;

namespace VClothes.ViewModels;

public class MucChiTietHoaDon : ViewModelCha
{
    public int SanPhamId { get; set; }
    public string TenSanPham { get; set; } = string.Empty;
    public int SoLuong { get; set; }
    public decimal DonGia { get; set; }
    public decimal ThanhTien => SoLuong * DonGia;
}

public class PhieuNhapViewModel : ViewModelCha
{
    private ObservableCollection<PhieuNhapDto> _danhSachPhieu = new();
    private ObservableCollection<NhaCungCapDto> _danhSachNCC = new();
    private ObservableCollection<SanPhamDto> _danhSachSanPham = new();
    private ObservableCollection<MucChiTietHoaDon> _danhSachChiTiet = new();
    private ObservableCollection<MucChiTietHoaDon> _chiTietPhieuDuocChon = new();
    private PhieuNhapDto? _phieuDuocChon;
    private bool _dangXemChiTiet;
    private string _timKiem = string.Empty;
    private bool _dangTao;
    private string _thongBaoXacThuc = string.Empty;
    private DateTime? _locTuNgay;
    private DateTime? _locDenNgay;
    private string _cotSapXep = "invoice_date";
    private bool _sapXepTang = false;

    private string _suaMaPhieu = string.Empty;
    private DateTime _suaNgayNhap = DateTime.Now;
    private NhaCungCapDto? _suaNCC;
    private string _suaGhiChu = string.Empty;
    private SanPhamDto? _sanPhamDuocChon;
    private int _suaSoLuong = 1;
    private decimal _suaDonGia;
    private decimal _tongTien;

    public ObservableCollection<PhieuNhapDto> DanhSachPhieu { get => _danhSachPhieu; set => GanGiaTri(ref _danhSachPhieu, value); }
    public ObservableCollection<NhaCungCapDto> DanhSachNCC { get => _danhSachNCC; set => GanGiaTri(ref _danhSachNCC, value); }
    public ObservableCollection<SanPhamDto> DanhSachSanPham { get => _danhSachSanPham; set => GanGiaTri(ref _danhSachSanPham, value); }
    public ObservableCollection<MucChiTietHoaDon> DanhSachChiTiet { get => _danhSachChiTiet; set => GanGiaTri(ref _danhSachChiTiet, value); }
    public ObservableCollection<MucChiTietHoaDon> ChiTietPhieuDuocChon { get => _chiTietPhieuDuocChon; set => GanGiaTri(ref _chiTietPhieuDuocChon, value); }
    public PhieuNhapDto? PhieuDuocChon { get => _phieuDuocChon; set { if (GanGiaTri(ref _phieuDuocChon, value)) TaiChiTietPhieu(); } }
    public bool DangXemChiTiet { get => _dangXemChiTiet; set => GanGiaTri(ref _dangXemChiTiet, value); }
    public string TimKiem { get => _timKiem; set { if (GanGiaTri(ref _timKiem, value)) TimKiemPhieu(); } }
    public bool DangTao { get => _dangTao; set => GanGiaTri(ref _dangTao, value); }
    public string ThongBaoXacThuc { get => _thongBaoXacThuc; set => GanGiaTri(ref _thongBaoXacThuc, value); }
    public DateTime? LocTuNgay { get => _locTuNgay; set { if (GanGiaTri(ref _locTuNgay, value)) TimKiemPhieu(); } }
    public DateTime? LocDenNgay { get => _locDenNgay; set { if (GanGiaTri(ref _locDenNgay, value)) TimKiemPhieu(); } }
    public string CotSapXep { get => _cotSapXep; set => GanGiaTri(ref _cotSapXep, value); }
    public bool SapXepTang { get => _sapXepTang; set => GanGiaTri(ref _sapXepTang, value); }

    public string SuaMaPhieu { get => _suaMaPhieu; set => GanGiaTri(ref _suaMaPhieu, value); }
    public DateTime SuaNgayNhap { get => _suaNgayNhap; set => GanGiaTri(ref _suaNgayNhap, value); }
    public NhaCungCapDto? SuaNCC { get => _suaNCC; set => GanGiaTri(ref _suaNCC, value); }
    public string SuaGhiChu { get => _suaGhiChu; set => GanGiaTri(ref _suaGhiChu, value); }
    public SanPhamDto? SanPhamDuocChon { get => _sanPhamDuocChon; set { if (GanGiaTri(ref _sanPhamDuocChon, value) && value != null) SuaDonGia = value.GiaNhap; } }
    public int SuaSoLuong { get => _suaSoLuong; set => GanGiaTri(ref _suaSoLuong, value); }
    public decimal SuaDonGia { get => _suaDonGia; set => GanGiaTri(ref _suaDonGia, value); }
    public decimal TongTien { get => _tongTien; set => GanGiaTri(ref _tongTien, value); }

    public ICommand LenhTao { get; }
    public ICommand LenhLuu { get; }
    public ICommand LenhHuy { get; }
    public ICommand LenhThemChiTiet { get; }
    public ICommand LenhXemChiTiet { get; }
    public ICommand LenhDongChiTiet { get; }
    public ICommand LenhSapXep { get; }

    public PhieuNhapViewModel()
    {
        LenhTao = new LenhRelay(_ => ThucHienTao());
        LenhLuu = new LenhRelay(_ => ThucHienLuu());
        LenhHuy = new LenhRelay(_ => { DangTao = false; });
        LenhThemChiTiet = new LenhRelay(_ => ThucHienThemChiTiet());
        LenhXemChiTiet = new LenhRelay(ThucHienXemChiTiet);
        LenhDongChiTiet = new LenhRelay(_ => { DangXemChiTiet = false; PhieuDuocChon = null; });
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
        DanhSachPhieu = new ObservableCollection<PhieuNhapDto>(
            SupabaseClient.LayDanhSach<PhieuNhapDto>("purchase_invoices", $"select=*,suppliers(*)&{sapXep}"));
        DanhSachNCC = new ObservableCollection<NhaCungCapDto>(
            SupabaseClient.LayDanhSach<NhaCungCapDto>("suppliers", "is_active=eq.true&order=name.asc"));
        DanhSachSanPham = new ObservableCollection<SanPhamDto>(
            SupabaseClient.LayDanhSach<SanPhamDto>("products", "is_active=eq.true&order=name.asc"));
    }

    private void TimKiemPhieu()
    {
        try
        {
            var sapXep = $"order={CotSapXep}.{(SapXepTang ? "asc" : "desc")}";
            var boLoc = new List<string> { "select=*,suppliers(*)", sapXep };
            if (!string.IsNullOrWhiteSpace(TimKiem))
                boLoc.Add($"invoice_code=ilike.%25{Uri.EscapeDataString(TimKiem)}%25");
            if (LocTuNgay.HasValue)
                boLoc.Add($"invoice_date=gte.{LocTuNgay.Value:yyyy-MM-dd}");
            if (LocDenNgay.HasValue)
                boLoc.Add($"invoice_date=lte.{LocDenNgay.Value:yyyy-MM-dd}");
            DanhSachPhieu = new ObservableCollection<PhieuNhapDto>(
                SupabaseClient.LayDanhSach<PhieuNhapDto>("purchase_invoices", string.Join("&", boLoc)));
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
        TimKiemPhieu();
    }

    private void ThucHienTao()
    {
        var soLuong = DanhSachPhieu.Count + 1;
        SuaMaPhieu = $"PN{soLuong:D3}";
        SuaNgayNhap = DateTime.Now;
        SuaNCC = null; SuaGhiChu = "";
        DanhSachChiTiet = new ObservableCollection<MucChiTietHoaDon>();
        TongTien = 0; DangTao = true; ThongBaoXacThuc = "";
    }

    private void ThucHienThemChiTiet()
    {
        ThongBaoXacThuc = "";
        if (SanPhamDuocChon == null) { ThongBaoXacThuc = "Vui lòng chọn sản phẩm"; return; }
        if (SuaSoLuong <= 0) { ThongBaoXacThuc = "Số lượng phải lớn hơn 0"; return; }
        if (SuaDonGia <= 0) { ThongBaoXacThuc = "Đơn giá phải lớn hơn 0"; return; }

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
        if (string.IsNullOrWhiteSpace(SuaMaPhieu)) { ThongBaoXacThuc = "Mã phiếu không được để trống"; return; }
        if (SuaNCC == null) { ThongBaoXacThuc = "Vui lòng chọn nhà cung cấp"; return; }
        if (!DanhSachChiTiet.Any()) { ThongBaoXacThuc = "Vui lòng thêm ít nhất 1 sản phẩm"; return; }

        try
        {
            var phieu = SupabaseClient.ThemMoi<PhieuNhapDto>("purchase_invoices", new
            {
                invoice_code = SuaMaPhieu,
                invoice_date = SuaNgayNhap.ToString("yyyy-MM-dd"),
                supplier_id = SuaNCC.Id,
                employee_id = 1,
                total_amount = TongTien,
                note = string.IsNullOrWhiteSpace(SuaGhiChu) ? (string?)null : SuaGhiChu
            });

            if (phieu != null)
            {
                foreach (var ct in DanhSachChiTiet)
                {
                    SupabaseClient.ThemMoi<ChiTietPhieuNhapDto>("purchase_invoice_details", new
                    {
                        purchase_invoice_id = phieu.Id,
                        product_id = ct.SanPhamId,
                        quantity = ct.SoLuong,
                        unit_price = ct.DonGia,
                        sub_total = ct.ThanhTien
                    });

                    // Cập nhật tồn kho
                    var sanPham = SupabaseClient.LayDanhSach<SanPhamDto>("products", $"id=eq.{ct.SanPhamId}&select=id,stock_quantity").FirstOrDefault();
                    if (sanPham != null)
                        SupabaseClient.CapNhat("products", $"id=eq.{ct.SanPhamId}", new { stock_quantity = sanPham.SoLuongTon + ct.SoLuong });
                }
            }

            TaiDuLieu(); DangTao = false; ThongBaoXacThuc = "Lưu phiếu nhập thành công!";
        }
        catch (Exception ex) { ThongBaoXacThuc = $"Lỗi: {ex.Message}"; }
    }

    private void ThucHienXemChiTiet(object? thamSo)
    {
        if (thamSo is PhieuNhapDto phieu)
        {
            if (PhieuDuocChon?.Id == phieu.Id && DangXemChiTiet)
            {
                DangXemChiTiet = false;
                PhieuDuocChon = null;
                return;
            }
            PhieuDuocChon = phieu;
        }
    }

    private void TaiChiTietPhieu()
    {
        if (PhieuDuocChon == null) { DangXemChiTiet = false; return; }

        try
        {
            var chiTiet = SupabaseClient.LayDanhSach<ChiTietPhieuNhapDto>("purchase_invoice_details",
                $"purchase_invoice_id=eq.{PhieuDuocChon.Id}&select=*,products(name,product_code)");

            ChiTietPhieuDuocChon = new ObservableCollection<MucChiTietHoaDon>(
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
