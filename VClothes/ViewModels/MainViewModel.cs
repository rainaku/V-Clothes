using System.Windows.Input;
using VClothes.Services;

namespace VClothes.ViewModels;

public class MainViewModel : BaseViewModel
{
    private BaseViewModel _currentViewModel;
    private string _currentViewTitle = "Tổng quan";
    private string _selectedMenuItem = "Dashboard";

    public BaseViewModel CurrentViewModel
    {
        get => _currentViewModel;
        set => SetProperty(ref _currentViewModel, value);
    }

    public string CurrentViewTitle
    {
        get => _currentViewTitle;
        set => SetProperty(ref _currentViewTitle, value);
    }

    public string SelectedMenuItem
    {
        get => _selectedMenuItem;
        set => SetProperty(ref _selectedMenuItem, value);
    }

    public string UserDisplayName
    {
        get
        {
            try { return AuthService.CurrentUser?.DisplayName ?? "Người dùng"; }
            catch { return "Người dùng"; }
        }
    }

    public string UserRole
    {
        get
        {
            try { return AuthService.CurrentRole?.Description ?? "N/A"; }
            catch { return "N/A"; }
        }
    }

    public bool IsAdmin
    {
        get
        {
            try { return AuthService.IsAdmin; }
            catch { return false; }
        }
    }

    public bool IsManager
    {
        get
        {
            try { return AuthService.IsManager; }
            catch { return false; }
        }
    }

    public ICommand NavigateCommand { get; }
    public ICommand LogoutCommand { get; }

    public event Action? LogoutRequested;

    public MainViewModel()
    {
        _currentViewModel = new DashboardViewModel();
        NavigateCommand = new RelayCommand(ExecuteNavigate);
        LogoutCommand = new RelayCommand(ExecuteLogout);
    }

    private void ExecuteNavigate(object? parameter)
    {
        var viewName = parameter?.ToString();
        if (string.IsNullOrEmpty(viewName)) return;

        SelectedMenuItem = viewName;

        try
        {
            switch (viewName)
            {
                case "Dashboard":
                    CurrentViewModel = new DashboardViewModel();
                    CurrentViewTitle = "Tổng quan";
                    break;
                case "Categories":
                    CurrentViewModel = new CategoryManagementViewModel();
                    CurrentViewTitle = "Quản lý loại sản phẩm";
                    break;
                case "Suppliers":
                    CurrentViewModel = new SupplierManagementViewModel();
                    CurrentViewTitle = "Quản lý nhà cung cấp";
                    break;
                case "Products":
                    CurrentViewModel = new ProductManagementViewModel();
                    CurrentViewTitle = "Quản lý sản phẩm";
                    break;
                case "Employees":
                    CurrentViewModel = new EmployeeManagementViewModel();
                    CurrentViewTitle = "Quản lý nhân viên";
                    break;
                case "PurchaseInvoices":
                    CurrentViewModel = new PurchaseInvoiceViewModel();
                    CurrentViewTitle = "Phiếu nhập hàng";
                    break;
                case "SalesInvoices":
                    CurrentViewModel = new SalesInvoiceViewModel();
                    CurrentViewTitle = "Hóa đơn bán hàng";
                    break;
                case "Revenue":
                    CurrentViewModel = new RevenueStatisticsViewModel();
                    CurrentViewTitle = "Thống kê doanh thu";
                    break;
                case "Reports":
                    CurrentViewModel = new ReportViewModel();
                    CurrentViewTitle = "Báo cáo";
                    break;
            }
        }
        catch (Exception)
        {
            // Prevent crash on navigation - view will remain unchanged
        }
    }

    private void ExecuteLogout(object? parameter)
    {
        AuthService.Logout();
        LogoutRequested?.Invoke();
    }
}
