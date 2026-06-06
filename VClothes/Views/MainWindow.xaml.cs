using System.Windows;
using System.Windows.Input;
using VClothes.ViewModels;

namespace VClothes.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        if (DataContext is ChinhViewModel vm)
        {
            vm.YeuCauDangXuat += KhiYeuCauDangXuat;
            Closed += (_, _) => vm.HuyDangKy();
        }
    }

    private void KhiYeuCauDangXuat()
    {
        var cuaSoDangNhap = new LoginWindow();
        cuaSoDangNhap.Show();
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

    private void NutPhongTo_Click(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState == WindowState.Maximized 
            ? WindowState.Normal 
            : WindowState.Maximized;
    }

    private void VungKeo_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ClickCount == 2)
        {
            NutPhongTo_Click(sender, e);
        }
        else
        {
            DragMove();
        }
    }
}
