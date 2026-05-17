using System.Windows.Controls;
using System.Windows.Input;

namespace VClothes.Views;

public partial class ProductManagementView : UserControl
{
    public ProductManagementView()
    {
        InitializeComponent();
    }

    private void Item_Click(object sender, MouseButtonEventArgs e)
    {
        if (sender is System.Windows.FrameworkElement element && element.DataContext != null)
        {
            var vm = DataContext as VClothes.ViewModels.ProductManagementViewModel;
            vm?.GetType().GetProperty("SelectedProduct")?.SetValue(vm, element.DataContext);
        }
    }
}
