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

    private void QuickAction_Click(object sender, MouseButtonEventArgs e)
    {
        if (sender is FrameworkElement element && element.Tag is string viewName)
        {
            // Navigate via MainViewModel
            var mainWindow = Window.GetWindow(this);
            if (mainWindow?.DataContext is MainViewModel mainVm)
            {
                mainVm.NavigateCommand.Execute(viewName);
            }
        }
    }
}
