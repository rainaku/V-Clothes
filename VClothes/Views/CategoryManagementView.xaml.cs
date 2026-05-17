using System.Windows.Controls;
using System.Windows.Input;

namespace VClothes.Views;

public partial class CategoryManagementView : UserControl
{
    public CategoryManagementView()
    {
        InitializeComponent();
    }

    private void Item_Click(object sender, MouseButtonEventArgs e)
    {
        // Trigger selection via DataContext binding
        if (sender is System.Windows.FrameworkElement element && element.DataContext != null)
        {
            var vm = DataContext as VClothes.ViewModels.CategoryManagementViewModel;
            if (vm != null)
            {
                vm.SelectedCategory = element.DataContext as VClothes.Data.CategoryDto;
            }
        }
    }
}
