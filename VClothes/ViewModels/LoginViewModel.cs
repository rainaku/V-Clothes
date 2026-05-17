using System.Windows;
using System.Windows.Input;
using VClothes.Services;

namespace VClothes.ViewModels;

public class LoginViewModel : BaseViewModel
{
    private string _username = string.Empty;
    private string _password = string.Empty;
    private string _errorMessage = string.Empty;
    private bool _isLoading;

    public string Username
    {
        get => _username;
        set => SetProperty(ref _username, value);
    }

    public string Password
    {
        get => _password;
        set => SetProperty(ref _password, value);
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        set => SetProperty(ref _errorMessage, value);
    }

    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    public ICommand LoginCommand { get; }

    public event Action? LoginSuccessful;

    public LoginViewModel()
    {
        LoginCommand = new RelayCommand(ExecuteLogin, CanLogin);
    }

    private bool CanLogin(object? parameter)
    {
        return !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password) && !IsLoading;
    }

    private void ExecuteLogin(object? parameter)
    {
        ErrorMessage = string.Empty;
        IsLoading = true;

        try
        {
            if (parameter is System.Windows.Controls.PasswordBox passwordBox)
            {
                Password = passwordBox.Password;
            }

            if (string.IsNullOrWhiteSpace(Username))
            {
                ErrorMessage = "Vui lòng nhập tên đăng nhập";
                return;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Vui lòng nhập mật khẩu";
                return;
            }

            var success = AuthService.Login(Username, Password);
            if (success)
            {
                LoginSuccessful?.Invoke();
            }
            else
            {
                ErrorMessage = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Lỗi đăng nhập: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }
}
