using System.Windows.Controls;
using System.Windows.Input;
using VClothes.ViewModels;

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
            var vm = DataContext as ProductManagementViewModel;
            vm?.GetType().GetProperty("SelectedProduct")?.SetValue(vm, element.DataContext);
        }
    }

    private void SortByCode_Click(object sender, MouseButtonEventArgs e)
    {
        (DataContext as ProductManagementViewModel)?.SortCommand.Execute("product_code");
    }

    private void SortByName_Click(object sender, MouseButtonEventArgs e)
    {
        (DataContext as ProductManagementViewModel)?.SortCommand.Execute("name");
    }

    private void SortByPrice_Click(object sender, MouseButtonEventArgs e)
    {
        (DataContext as ProductManagementViewModel)?.SortCommand.Execute("price");
    }

    private void SortByStock_Click(object sender, MouseButtonEventArgs e)
    {
        (DataContext as ProductManagementViewModel)?.SortCommand.Execute("stock_quantity");
    }
}
