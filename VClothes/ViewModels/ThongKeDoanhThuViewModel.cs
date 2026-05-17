using System.Collections.ObjectModel;
using System.Windows.Input;
using VClothes.Data;

namespace VClothes.ViewModels;

public class MucHienThiHoaDonBan
{
    public string MaHoaDon { get; set; } = string.Empty;
    public DateTime NgayBan { get; set; }
    public string TenKhachHang { get; set; } = string.Empty;
    public string TenNhanVien { get; set; } = string.Empty;
    public decimal TongTien { get; set; }
    public decimal GiamGia { get; set; }
    public decimal ThanhToan { get; set; }
}

public class ThongKeDoanhThuViewModel : ViewModelCha
{
    private DateTime _tuNgay = new DateTime(2024, 1, 1);
    private DateTime _denNgay = DateTime.Now;
    private decimal _tongDoanhThu;
    private int _tongHoaDon;
    private decimal _trungBinhMoiDon;
    private decimal _loiNhuanUocTinh;
    private ObservableCollection<MucHienThiHoaDonBan> _danhSachHoaDonBan = new();

    public DateTime TuNgay { get => _tuNgay; set => GanGiaTri(ref _tuNgay, value); }
    public DateTime DenNgay { get => _denNgay; set => GanGiaTri(ref _denNgay, value); }
    public decimal TongDoanhThu { get => _tongDoanhThu; set => GanGiaTri(ref _tongDoanhThu, value); }
    public int TongHoaDon { get => _tongHoaDon; set => GanGiaTri(ref _tongHoaDon, value); }
    public decimal TrungBinhMoiDon { get => _trungBinhMoiDon; set => GanGiaTri(ref _trungBinhMoiDon, value); }
    public decimal LoiNhuanUocTinh { get => _loiNhuanUocTinh; set => GanGiaTri(ref _loiNhuanUocTinh, value); }
    public ObservableCollection<MucHienThiHoaDonBan> DanhSachHoaDonBan { get => _danhSachHoaDonBan; set => GanGiaTri(ref _danhSachHoaDonBan, value); }

    public ICommand LenhLoc { get; }

    public ThongKeDoanhThuViewModel()
    {
        LenhLoc = new LenhRelay(_ => TaiThongKeAsync());
        DangTai = true;
        TaiThongKeAsync();
    }

    private async void TaiThongKeAsync()
    {
        DangTai = true;
        try { await Task.Run(() => TaiThongKe()); }
        catch { }
        finally { DangTai = false; }
    }

    private void TaiThongKe()
    {
        try
        {
            var hoaDon = SupabaseClient.LayDanhSach<HoaDonBanDto>("sales_invoices",
                $"select=*,customers(*),employees(*)&invoice_date=gte.{TuNgay:yyyy-MM-dd}&invoice_date=lte.{DenNgay:yyyy-MM-dd}&order=invoice_date.desc");

            TongHoaDon = hoaDon.Count;
            TongDoanhThu = hoaDon.Sum(i => i.ThanhToan);
            TrungBinhMoiDon = TongHoaDon > 0 ? TongDoanhThu / TongHoaDon : 0;
            LoiNhuanUocTinh = TongDoanhThu * 0.3m;

            var danhSachHienThi = hoaDon.Select(i => new MucHienThiHoaDonBan
            {
                MaHoaDon = i.MaHoaDon,
                NgayBan = i.NgayBan,
                TenKhachHang = i.KhachHang?.HoTen ?? "Khách lẻ",
                TenNhanVien = i.NhanVien?.HoTen ?? "N/A",
                TongTien = i.TongTien,
                GiamGia = i.GiamGia,
                ThanhToan = i.ThanhToan
            }).ToList();

            DanhSachHoaDonBan = new ObservableCollection<MucHienThiHoaDonBan>(danhSachHienThi);
        }
        catch { }
    }
}
