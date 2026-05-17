using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using VClothes.ViewModels;

namespace VClothes.Views;

public partial class DashboardView : UserControl
{
    public DashboardView()
    {
        InitializeComponent();
    }

    private void ThaoTacNhanh_Click(object sender, MouseButtonEventArgs e)
    {
        if (sender is FrameworkElement element && element.Tag is string tenView)
        {
            var cuaSoChinh = Window.GetWindow(this);
            if (cuaSoChinh?.DataContext is ChinhViewModel chinhVm)
            {
                chinhVm.LenhDieuHuong.Execute(tenView);
            }
        }
    }
}
