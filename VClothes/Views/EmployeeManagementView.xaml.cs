using System.Windows.Controls;
using System.Windows.Input;

namespace VClothes.Views;

public partial class EmployeeManagementView : UserControl
{
    public EmployeeManagementView()
    {
        InitializeComponent();
    }

    private void MucDuocChon_Click(object sender, MouseButtonEventArgs e)
    {
        if (sender is System.Windows.FrameworkElement element && element.DataContext != null)
        {
            var vm = DataContext as VClothes.ViewModels.QuanLyNhanVienViewModel;
            vm?.GetType().GetProperty("NhanVienDuocChon")?.SetValue(vm, element.DataContext);
        }
    }
}
