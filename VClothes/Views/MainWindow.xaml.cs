using System.Windows;
using System.Windows.Input;
using VClothes.ViewModels;

namespace VClothes.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        if (DataContext is MainViewModel vm)
        {
            vm.LogoutRequested += OnLogoutRequested;
        }
    }

    private void OnLogoutRequested()
    {
        var loginWindow = new LoginWindow();
        loginWindow.Show();
        Close();
    }

    private void CloseButton_Click(object sender, MouseButtonEventArgs e)
    {
        Application.Current.Shutdown();
    }

    private void MinimizeButton_Click(object sender, MouseButtonEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }

    private void MaximizeButton_Click(object sender, MouseButtonEventArgs e)
    {
        WindowState = WindowState == WindowState.Maximized 
            ? WindowState.Normal 
            : WindowState.Maximized;
    }

    private void DragArea_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ClickCount == 2)
        {
            MaximizeButton_Click(sender, e);
        }
        else
        {
            DragMove();
        }
    }
}
