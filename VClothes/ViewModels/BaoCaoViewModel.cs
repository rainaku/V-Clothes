using System.Collections.ObjectModel;
using System.Data;
using System.Windows.Input;
using VClothes.Data;

namespace VClothes.ViewModels;

public class BaoCaoViewModel : ViewModelCha
{
    private ObservableCollection<string> _danhSachLoaiBaoCao = new()
    {
        "Báo cáo doanh thu theo ngày",
        "Báo cáo doanh thu theo tháng",
        "Báo cáo sản phẩm bán chạy",
        "Báo cáo tồn kho",
        "Báo cáo nhập hàng"
    };
    private string _loaiBaoCaoDuocChon = "Báo cáo doanh thu theo ngày";
    private DateTime _tuNgay = new DateTime(2024, 1, 1);
    private DateTime _denNgay = DateTime.Now;
    private string _tieuDeBaoCao = "BÁO CÁO DOANH THU";
    private string _khoangThoiGian = string.Empty;
    private string _ngayXuatBaoCao = string.Empty;
    private bool _coTomTat;
    private int _tomTatSoLuong;
    private decimal _tomTatDoanhThu;
    private decimal _tomTatLoiNhuan;
    private DataView? _duLieuBaoCao;

    public ObservableCollection<string> DanhSachLoaiBaoCao { get => _danhSachLoaiBaoCao; set => GanGiaTri(ref _danhSachLoaiBaoCao, value); }
    public string LoaiBaoCaoDuocChon { get => _loaiBaoCaoDuocChon; set => GanGiaTri(ref _loaiBaoCaoDuocChon, value); }
    public DateTime TuNgay { get => _tuNgay; set => GanGiaTri(ref _tuNgay, value); }
    public DateTime DenNgay { get => _denNgay; set => GanGiaTri(ref _denNgay, value); }
    public string TieuDeBaoCao { get => _tieuDeBaoCao; set => GanGiaTri(ref _tieuDeBaoCao, value); }
    public string KhoangThoiGian { get => _khoangThoiGian; set => GanGiaTri(ref _khoangThoiGian, value); }
    public string NgayXuatBaoCao { get => _ngayXuatBaoCao; set => GanGiaTri(ref _ngayXuatBaoCao, value); }
    public bool CoTomTat { get => _coTomTat; set => GanGiaTri(ref _coTomTat, value); }
    public int TomTatSoLuong { get => _tomTatSoLuong; set => GanGiaTri(ref _tomTatSoLuong, value); }
    public decimal TomTatDoanhThu { get => _tomTatDoanhThu; set => GanGiaTri(ref _tomTatDoanhThu, value); }
    public decimal TomTatLoiNhuan { get => _tomTatLoiNhuan; set => GanGiaTri(ref _tomTatLoiNhuan, value); }
    public DataView? DuLieuBaoCao { get => _duLieuBaoCao; set => GanGiaTri(ref _duLieuBaoCao, value); }

    public ICommand LenhXuatBaoCao { get; }

    public BaoCaoViewModel()
    {
        LenhXuatBaoCao = new LenhRelay(_ => XuatBaoCaoAsync());
        KhoangThoiGian = $"Từ {TuNgay:dd/MM/yyyy} đến {DenNgay:dd/MM/yyyy}";
        NgayXuatBaoCao = $"Ngày xuất: {DateTime.Now:dd/MM/yyyy HH:mm}";
        DangTai = true;
        XuatBaoCaoAsync();
    }

    private async void XuatBaoCaoAsync()
    {
        DangTai = true;
        try { await Task.Run(() => XuatBaoCao()); }
        catch { }
        finally { DangTai = false; }
    }

    private void XuatBaoCao()
    {
        KhoangThoiGian = $"Từ {TuNgay:dd/MM/yyyy} đến {DenNgay:dd/MM/yyyy}";
        NgayXuatBaoCao = $"Ngày xuất: {DateTime.Now:dd/MM/yyyy HH:mm}";

        try
        {
            switch (LoaiBaoCaoDuocChon)
            {
                case "Báo cáo doanh thu theo ngày": BaoCaoDoanhThuTheoNgay(); break;
                case "Báo cáo doanh thu theo tháng": BaoCaoDoanhThuTheoThang(); break;
                case "Báo cáo sản phẩm bán chạy": BaoCaoSanPhamBanChay(); break;
                case "Báo cáo tồn kho": BaoCaoTonKho(); break;
                case "Báo cáo nhập hàng": BaoCaoNhapHang(); break;
            }
        }
        catch { }
    }

    private void BaoCaoDoanhThuTheoNgay()
    {
        TieuDeBaoCao = "BÁO CÁO DOANH THU THEO NGÀY";
        CoTomTat = true;

        var hoaDon = SupabaseClient.LayDanhSach<HoaDonBanDto>("sales_invoices",
            $"invoice_date=gte.{TuNgay:yyyy-MM-dd}&invoice_date=lte.{DenNgay:yyyy-MM-dd}&order=invoice_date.asc");

        var nhom = hoaDon.GroupBy(i => i.NgayBan.Date)
            .Select(g => new { Ngay = g.Key, SoLuong = g.Count(), DoanhThu = g.Sum(i => i.ThanhToan) }).ToList();

        TomTatSoLuong = hoaDon.Count;
        TomTatDoanhThu = hoaDon.Sum(i => i.ThanhToan);
        TomTatLoiNhuan = TomTatDoanhThu * 0.3m;

        var bang = new DataTable();
        bang.Columns.Add("Ngày", typeof(string));
        bang.Columns.Add("Số hóa đơn", typeof(int));
        bang.Columns.Add("Doanh thu", typeof(string));
        foreach (var muc in nhom)
            bang.Rows.Add(muc.Ngay.ToString("dd/MM/yyyy"), muc.SoLuong, muc.DoanhThu.ToString("N0") + " ₫");
        DuLieuBaoCao = bang.DefaultView;
    }

    private void BaoCaoDoanhThuTheoThang()
    {
        TieuDeBaoCao = "BÁO CÁO DOANH THU THEO THÁNG";
        CoTomTat = true;

        var hoaDon = SupabaseClient.LayDanhSach<HoaDonBanDto>("sales_invoices",
            $"invoice_date=gte.{TuNgay:yyyy-MM-dd}&invoice_date=lte.{DenNgay:yyyy-MM-dd}");

        var nhom = hoaDon.GroupBy(i => new { i.NgayBan.Year, i.NgayBan.Month })
            .Select(g => new { Thang = $"{g.Key.Month:D2}/{g.Key.Year}", SoLuong = g.Count(), DoanhThu = g.Sum(i => i.ThanhToan) }).ToList();

        TomTatSoLuong = hoaDon.Count;
        TomTatDoanhThu = hoaDon.Sum(i => i.ThanhToan);
        TomTatLoiNhuan = TomTatDoanhThu * 0.3m;

        var bang = new DataTable();
        bang.Columns.Add("Tháng", typeof(string));
        bang.Columns.Add("Số hóa đơn", typeof(int));
        bang.Columns.Add("Doanh thu", typeof(string));
        foreach (var muc in nhom)
            bang.Rows.Add(muc.Thang, muc.SoLuong, muc.DoanhThu.ToString("N0") + " ₫");
        DuLieuBaoCao = bang.DefaultView;
    }

    private void BaoCaoSanPhamBanChay()
    {
        TieuDeBaoCao = "BÁO CÁO SẢN PHẨM BÁN CHẠY";
        CoTomTat = false;

        var chiTiet = SupabaseClient.LayDanhSach<ChiTietHoaDonBanDto>("sales_invoice_details",
            "select=*,products(name,product_code)");

        var nhom = chiTiet.GroupBy(d => new { d.SanPhamId, Ten = d.SanPham?.Ten ?? "", Ma = d.SanPham?.MaSanPham ?? "" })
            .Select(g => new { g.Key.Ma, g.Key.Ten, SoLuong = g.Sum(d => d.SoLuong), DoanhThu = g.Sum(d => d.ThanhTien) })
            .OrderByDescending(g => g.SoLuong).Take(20).ToList();

        var bang = new DataTable();
        bang.Columns.Add("Mã SP", typeof(string));
        bang.Columns.Add("Tên sản phẩm", typeof(string));
        bang.Columns.Add("Số lượng bán", typeof(int));
        bang.Columns.Add("Doanh thu", typeof(string));
        foreach (var muc in nhom)
            bang.Rows.Add(muc.Ma, muc.Ten, muc.SoLuong, muc.DoanhThu.ToString("N0") + " ₫");
        DuLieuBaoCao = bang.DefaultView;
    }

    private void BaoCaoTonKho()
    {
        TieuDeBaoCao = "BÁO CÁO TỒN KHO";
        CoTomTat = false;

        var sanPham = SupabaseClient.LayDanhSach<SanPhamDto>("products",
            "select=*,categories(name)&is_active=eq.true&order=stock_quantity.asc");

        var bang = new DataTable();
        bang.Columns.Add("Mã SP", typeof(string));
        bang.Columns.Add("Tên sản phẩm", typeof(string));
        bang.Columns.Add("Loại", typeof(string));
        bang.Columns.Add("Tồn kho", typeof(int));
        bang.Columns.Add("Giá trị tồn", typeof(string));
        bang.Columns.Add("Trạng thái", typeof(string));

        foreach (var sp in sanPham)
        {
            var trangThai = sp.SoLuongTon < 10 ? "⚠️ Sắp hết" : "✅ Đủ hàng";
            bang.Rows.Add(sp.MaSanPham, sp.Ten, sp.LoaiSanPham?.Ten ?? "", sp.SoLuongTon,
                (sp.SoLuongTon * sp.GiaNhap).ToString("N0") + " ₫", trangThai);
        }
        DuLieuBaoCao = bang.DefaultView;
    }

    private void BaoCaoNhapHang()
    {
        TieuDeBaoCao = "BÁO CÁO NHẬP HÀNG";
        CoTomTat = true;

        var phieuNhap = SupabaseClient.LayDanhSach<PhieuNhapDto>("purchase_invoices",
            $"select=*,suppliers(name)&invoice_date=gte.{TuNgay:yyyy-MM-dd}&invoice_date=lte.{DenNgay:yyyy-MM-dd}&order=invoice_date.desc");

        TomTatSoLuong = phieuNhap.Count;
        TomTatDoanhThu = phieuNhap.Sum(i => i.TongTien);
        TomTatLoiNhuan = 0;

        var bang = new DataTable();
        bang.Columns.Add("Mã phiếu", typeof(string));
        bang.Columns.Add("Ngày nhập", typeof(string));
        bang.Columns.Add("Nhà cung cấp", typeof(string));
        bang.Columns.Add("Tổng tiền", typeof(string));
        bang.Columns.Add("Ghi chú", typeof(string));

        foreach (var pn in phieuNhap)
            bang.Rows.Add(pn.MaPhieu, pn.NgayNhap.ToString("dd/MM/yyyy"),
                pn.NhaCungCap?.Ten ?? "", pn.TongTien.ToString("N0") + " ₫", pn.GhiChu ?? "");
        DuLieuBaoCao = bang.DefaultView;
    }
}
