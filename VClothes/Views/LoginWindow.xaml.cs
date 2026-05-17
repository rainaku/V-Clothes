using System.Windows;
using System.Windows.Input;
using VClothes.ViewModels;

namespace VClothes.Views;

public partial class LoginWindow : Window
{
    public LoginWindow()
    {
        InitializeComponent();

        if (DataContext is LoginViewModel vm)
        {
            vm.LoginSuccessful += OnLoginSuccessful;
        }
    }

    private void OnLoginSuccessful()
    {
        var mainWindow = new MainWindow();
        mainWindow.Show();
        Close();
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }

    private void MinimizeButton_Click(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }

    private void DragArea_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        DragMove();
    }
}
