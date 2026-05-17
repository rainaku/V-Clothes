using VClothes.Data;

namespace VClothes.ViewModels;

public class TongQuanViewModel : ViewModelCha
{
    private int _tongSanPham;
    private int _tongLoaiSanPham;
    private int _tongNhanVien;
    private int _tongKhachHang;
    private decimal _doanhThuHomNay;
    private int _donHangHomNay;
    private int _sapHetHang;
    private decimal _doanhThuThang;

    public int TongSanPham { get => _tongSanPham; set => GanGiaTri(ref _tongSanPham, value); }
    public int TongLoaiSanPham { get => _tongLoaiSanPham; set => GanGiaTri(ref _tongLoaiSanPham, value); }
    public int TongNhanVien { get => _tongNhanVien; set => GanGiaTri(ref _tongNhanVien, value); }
    public int TongKhachHang { get => _tongKhachHang; set => GanGiaTri(ref _tongKhachHang, value); }
    public decimal DoanhThuHomNay { get => _doanhThuHomNay; set => GanGiaTri(ref _doanhThuHomNay, value); }
    public int DonHangHomNay { get => _donHangHomNay; set => GanGiaTri(ref _donHangHomNay, value); }
    public int SapHetHang { get => _sapHetHang; set => GanGiaTri(ref _sapHetHang, value); }
    public decimal DoanhThuThang { get => _doanhThuThang; set => GanGiaTri(ref _doanhThuThang, value); }

    public TongQuanViewModel()
    {
        DangTai = true;
        TaiDuLieuTongQuanAsync();
    }

    private async void TaiDuLieuTongQuanAsync()
    {
        try
        {
            await Task.Run(() => TaiDuLieuTongQuan());
        }
        catch { }
        finally { DangTai = false; }
    }

    private void TaiDuLieuTongQuan()
    {
        try
        {
            TongSanPham = SupabaseClient.LayDanhSach<SanPhamDto>("products", "is_active=eq.true&select=id").Count;
            TongLoaiSanPham = SupabaseClient.LayDanhSach<LoaiSanPhamDto>("categories", "is_active=eq.true&select=id").Count;
            TongNhanVien = SupabaseClient.LayDanhSach<NhanVienDto>("employees", "is_active=eq.true&select=id").Count;
            TongKhachHang = SupabaseClient.LayDanhSach<KhachHangDto>("customers", "select=id").Count;

            var homNay = DateTime.UtcNow.Date.ToString("yyyy-MM-dd");
            var hoaDonHomNay = SupabaseClient.LayDanhSach<HoaDonBanDto>("sales_invoices",
                $"invoice_date=gte.{homNay}&select=id,final_amount");
            DoanhThuHomNay = hoaDonHomNay.Sum(i => i.ThanhToan);
            DonHangHomNay = hoaDonHomNay.Count;

            SapHetHang = SupabaseClient.LayDanhSach<SanPhamDto>("products",
                "is_active=eq.true&stock_quantity=lt.10&select=id").Count;

            var dauThang = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyy-MM-dd");
            var hoaDonThang = SupabaseClient.LayDanhSach<HoaDonBanDto>("sales_invoices",
                $"invoice_date=gte.{dauThang}&select=id,final_amount");
            DoanhThuThang = hoaDonThang.Sum(i => i.ThanhToan);
        }
        catch { }
    }
}
