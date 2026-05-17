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

    private async void ExecuteNavigate(object? parameter)
    {
        var viewName = parameter?.ToString();
        if (string.IsNullOrEmpty(viewName)) return;

        SelectedMenuItem = viewName;

        try
        {
            BaseViewModel? vm = null;

            await Task.Run(() =>
            {
                vm = viewName switch
                {
                    "Dashboard" => new DashboardViewModel(),
                    "Categories" => new CategoryManagementViewModel(),
                    "Suppliers" => new SupplierManagementViewModel(),
                    "Products" => new ProductManagementViewModel(),
                    "Employees" => new EmployeeManagementViewModel(),
                    "PurchaseInvoices" => new PurchaseInvoiceViewModel(),
                    "SalesInvoices" => new SalesInvoiceViewModel(),
                    "Revenue" => new RevenueStatisticsViewModel(),
                    "Reports" => new ReportViewModel(),
                    _ => null
                };
            });

            if (vm != null)
            {
                CurrentViewModel = vm;
                CurrentViewTitle = viewName switch
                {
                    "Dashboard" => "Tổng quan",
                    "Categories" => "Quản lý loại sản phẩm",
                    "Suppliers" => "Quản lý nhà cung cấp",
                    "Products" => "Quản lý sản phẩm",
                    "Employees" => "Quản lý nhân viên",
                    "PurchaseInvoices" => "Phiếu nhập hàng",
                    "SalesInvoices" => "Hóa đơn bán hàng",
                    "Revenue" => "Thống kê doanh thu",
                    "Reports" => "Báo cáo",
                    _ => CurrentViewTitle
                };
            }
        }
        catch (Exception)
        {
            // Prevent crash on navigation
        }
    }

    private void ExecuteLogout(object? parameter)
    {
        AuthService.Logout();
        LogoutRequested?.Invoke();
    }
}
