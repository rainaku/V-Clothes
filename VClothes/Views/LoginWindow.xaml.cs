using System.Windows;
using System.Windows.Input;
using VClothes.ViewModels;

namespace VClothes.Views;

public partial class LoginWindow : Window
{
    public LoginWindow()
    {
        InitializeComponent();

        if (DataContext is DangNhapViewModel vm)
        {
            vm.DangNhapThanhCong += KhiDangNhapThanhCong;
        }
    }

    private void KhiDangNhapThanhCong()
    {
        var cuaSoChinh = new MainWindow();
        cuaSoChinh.Show();
        Close();
    }

    private void NutDong_Click(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }

    private void NutThuNho_Click(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }

    private void VungKeo_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        DragMove();
    }
}
