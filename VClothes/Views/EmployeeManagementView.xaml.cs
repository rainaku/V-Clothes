using System.Windows.Controls;
using System.Windows.Input;

namespace VClothes.Views;

public partial class EmployeeManagementView : UserControl
{
    public EmployeeManagementView()
    {
        InitializeComponent();
    }

    private void Item_Click(object sender, MouseButtonEventArgs e)
    {
        if (sender is System.Windows.FrameworkElement element && element.DataContext != null)
        {
            var vm = DataContext as VClothes.ViewModels.EmployeeManagementViewModel;
            vm?.GetType().GetProperty("SelectedEmployee")?.SetValue(vm, element.DataContext);
        }
    }
}
