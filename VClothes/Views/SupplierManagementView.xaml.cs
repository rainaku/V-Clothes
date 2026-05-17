using System.Windows.Controls;
using System.Windows.Input;

namespace VClothes.Views;

public partial class SupplierManagementView : UserControl
{
    public SupplierManagementView()
    {
        InitializeComponent();
    }

    private void Item_Click(object sender, MouseButtonEventArgs e)
    {
        if (sender is System.Windows.FrameworkElement element && element.DataContext != null)
        {
            var vm = DataContext as VClothes.ViewModels.SupplierManagementViewModel;
            vm?.GetType().GetProperty("SelectedSupplier")?.SetValue(vm, element.DataContext);
        }
    }
}
