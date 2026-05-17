using System.Windows;
using System.Windows.Input;
using VClothes.Services;

namespace VClothes.ViewModels;

public class DangNhapViewModel : ViewModelCha
{
    private string _tenDangNhap = string.Empty;
    private string _matKhau = string.Empty;
    private string _thongBaoLoi = string.Empty;
    private bool _dangTai;

    public string TenDangNhap
    {
        get => _tenDangNhap;
        set => GanGiaTri(ref _tenDangNhap, value);
    }

    public string MatKhau
    {
        get => _matKhau;
        set => GanGiaTri(ref _matKhau, value);
    }

    public string ThongBaoLoi
    {
        get => _thongBaoLoi;
        set => GanGiaTri(ref _thongBaoLoi, value);
    }

    public new bool DangTai
    {
        get => _dangTai;
        set => GanGiaTri(ref _dangTai, value);
    }

    public ICommand LenhDangNhap { get; }

    public event Action? DangNhapThanhCong;

    public DangNhapViewModel()
    {
        LenhDangNhap = new LenhRelay(ThucHienDangNhap, CoTheDangNhap);
    }

    private bool CoTheDangNhap(object? parameter)
    {
        return !string.IsNullOrWhiteSpace(TenDangNhap) && !DangTai;
    }

    private void ThucHienDangNhap(object? parameter)
    {
        ThongBaoLoi = string.Empty;
        DangTai = true;

        try
        {
            if (parameter is System.Windows.Controls.PasswordBox hopMatKhau)
            {
                MatKhau = hopMatKhau.Password;
            }

            if (string.IsNullOrWhiteSpace(TenDangNhap))
            {
                ThongBaoLoi = "Vui lòng nhập tên đăng nhập";
                return;
            }

            if (string.IsNullOrWhiteSpace(MatKhau))
            {
                ThongBaoLoi = "Vui lòng nhập mật khẩu";
                return;
            }

            var thanhCong = DichVuXacThuc.DangNhap(TenDangNhap, MatKhau);
            if (thanhCong)
            {
                DangNhapThanhCong?.Invoke();
            }
            else
            {
                ThongBaoLoi = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
        }
        catch (Exception ex)
        {
            ThongBaoLoi = $"Lỗi đăng nhập: {ex.Message}";
        }
        finally
        {
            DangTai = false;
        }
    }
}
