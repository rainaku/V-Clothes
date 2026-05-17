using System.Windows.Controls;
using System.Windows.Input;

namespace VClothes.Views;

public partial class SupplierManagementView : UserControl
{
    public SupplierManagementView()
    {
        InitializeComponent();
    }

    private void MucDuocChon_Click(object sender, MouseButtonEventArgs e)
    {
        if (sender is System.Windows.FrameworkElement element && element.DataContext != null)
        {
            var vm = DataContext as VClothes.ViewModels.QuanLyNhaCungCapViewModel;
            vm?.GetType().GetProperty("NccDuocChon")?.SetValue(vm, element.DataContext);
        }
    }
}
