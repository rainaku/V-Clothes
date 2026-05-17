using VClothes.Data;

namespace VClothes.ViewModels;

public class DashboardViewModel : BaseViewModel
{
    private int _totalProducts;
    private int _totalCategories;
    private int _totalEmployees;
    private int _totalCustomers;
    private decimal _todayRevenue;
    private int _todayOrders;
    private int _lowStockCount;
    private decimal _monthRevenue;

    public int TotalProducts { get => _totalProducts; set => SetProperty(ref _totalProducts, value); }
    public int TotalCategories { get => _totalCategories; set => SetProperty(ref _totalCategories, value); }
    public int TotalEmployees { get => _totalEmployees; set => SetProperty(ref _totalEmployees, value); }
    public int TotalCustomers { get => _totalCustomers; set => SetProperty(ref _totalCustomers, value); }
    public decimal TodayRevenue { get => _todayRevenue; set => SetProperty(ref _todayRevenue, value); }
    public int TodayOrders { get => _todayOrders; set => SetProperty(ref _todayOrders, value); }
    public int LowStockCount { get => _lowStockCount; set => SetProperty(ref _lowStockCount, value); }
    public decimal MonthRevenue { get => _monthRevenue; set => SetProperty(ref _monthRevenue, value); }

    public DashboardViewModel()
    {
        IsLoading = true;
        LoadDashboardDataAsync();
    }

    private async void LoadDashboardDataAsync()
    {
        try
        {
            await Task.Run(() => LoadDashboardData());
        }
        catch { }
        finally { IsLoading = false; }
    }

    private void LoadDashboardData()
    {
        try
        {
            TotalProducts = SupabaseClient.Get<ProductDto>("products", "is_active=eq.true&select=id").Count;
            TotalCategories = SupabaseClient.Get<CategoryDto>("categories", "is_active=eq.true&select=id").Count;
            TotalEmployees = SupabaseClient.Get<EmployeeDto>("employees", "is_active=eq.true&select=id").Count;
            TotalCustomers = SupabaseClient.Get<CustomerDto>("customers", "select=id").Count;

            var today = DateTime.UtcNow.Date.ToString("yyyy-MM-dd");
            var todayInvoices = SupabaseClient.Get<SalesInvoiceDto>("sales_invoices",
                $"invoice_date=gte.{today}&select=id,final_amount");
            TodayRevenue = todayInvoices.Sum(i => i.FinalAmount);
            TodayOrders = todayInvoices.Count;

            LowStockCount = SupabaseClient.Get<ProductDto>("products",
                "is_active=eq.true&stock_quantity=lt.10&select=id").Count;

            var monthStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyy-MM-dd");
            var monthInvoices = SupabaseClient.Get<SalesInvoiceDto>("sales_invoices",
                $"invoice_date=gte.{monthStart}&select=id,final_amount");
            MonthRevenue = monthInvoices.Sum(i => i.FinalAmount);
        }
        catch
        {
            // Database might not be accessible
        }
    }
}
