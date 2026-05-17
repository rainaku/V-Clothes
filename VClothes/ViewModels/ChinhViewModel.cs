using System.Windows.Input;
using VClothes.Services;

namespace VClothes.ViewModels;

public class ChinhViewModel : ViewModelCha
{
    private ViewModelCha _viewModelHienTai;
    private string _tieuDeHienTai = "Tổng quan";
    private string _menuDuocChon = "TongQuan";

    public ViewModelCha ViewModelHienTai
    {
        get => _viewModelHienTai;
        set => GanGiaTri(ref _viewModelHienTai, value);
    }

    public string TieuDeHienTai
    {
        get => _tieuDeHienTai;
        set => GanGiaTri(ref _tieuDeHienTai, value);
    }

    public string MenuDuocChon
    {
        get => _menuDuocChon;
        set => GanGiaTri(ref _menuDuocChon, value);
    }

    public string TenHienThiNguoiDung
    {
        get
        {
            try { return DichVuXacThuc.NguoiDungHienTai?.TenHienThi ?? "Người dùng"; }
            catch { return "Người dùng"; }
        }
    }

    public string VaiTroNguoiDung
    {
        get
        {
            try { return DichVuXacThuc.VaiTroHienTai?.MoTa ?? "N/A"; }
            catch { return "N/A"; }
        }
    }

    public bool LaQuanTriVien
    {
        get
        {
            try { return DichVuXacThuc.LaQuanTriVien; }
            catch { return false; }
        }
    }

    public bool LaQuanLy
    {
        get
        {
            try { return DichVuXacThuc.LaQuanLy; }
            catch { return false; }
        }
    }

    public ICommand LenhDieuHuong { get; }
    public ICommand LenhDangXuat { get; }

    public event Action? YeuCauDangXuat;

    public ChinhViewModel()
    {
        _viewModelHienTai = new TongQuanViewModel();
        LenhDieuHuong = new LenhRelay(ThucHienDieuHuong);
        LenhDangXuat = new LenhRelay(ThucHienDangXuat);
    }

    private void ThucHienDieuHuong(object? thamSo)
    {
        var tenView = thamSo?.ToString();
        if (string.IsNullOrEmpty(tenView)) return;

        MenuDuocChon = tenView;

        TieuDeHienTai = tenView switch
        {
            "TongQuan" => "Tổng quan",
            "LoaiSanPham" => "Quản lý loại sản phẩm",
            "NhaCungCap" => "Quản lý nhà cung cấp",
            "SanPham" => "Quản lý sản phẩm",
            "NhanVien" => "Quản lý nhân viên",
            "PhieuNhap" => "Phiếu nhập hàng",
            "HoaDonBan" => "Hóa đơn bán hàng",
            "DoanhThu" => "Thống kê doanh thu",
            "BaoCao" => "Báo cáo",
            _ => TieuDeHienTai
        };

        try
        {
            ViewModelCha vm = tenView switch
            {
                "TongQuan" => new TongQuanViewModel(),
                "LoaiSanPham" => new QuanLyLoaiSanPhamViewModel(),
                "NhaCungCap" => new QuanLyNhaCungCapViewModel(),
                "SanPham" => new QuanLySanPhamViewModel(),
                "NhanVien" => new QuanLyNhanVienViewModel(),
                "PhieuNhap" => new PhieuNhapViewModel(),
                "HoaDonBan" => new HoaDonBanViewModel(),
                "DoanhThu" => new ThongKeDoanhThuViewModel(),
                "BaoCao" => new BaoCaoViewModel(),
                _ => _viewModelHienTai
            };
            ViewModelHienTai = vm;
        }
        catch (Exception)
        {
            // Tránh crash khi điều hướng
        }
    }

    private void ThucHienDangXuat(object? thamSo)
    {
        DichVuXacThuc.DangXuat();
        YeuCauDangXuat?.Invoke();
    }
}
