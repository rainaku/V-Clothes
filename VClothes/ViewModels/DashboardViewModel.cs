using Microsoft.EntityFrameworkCore;
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

    public int TotalProducts
    {
        get => _totalProducts;
        set => SetProperty(ref _totalProducts, value);
    }

    public int TotalCategories
    {
        get => _totalCategories;
        set => SetProperty(ref _totalCategories, value);
    }

    public int TotalEmployees
    {
        get => _totalEmployees;
        set => SetProperty(ref _totalEmployees, value);
    }

    public int TotalCustomers
    {
        get => _totalCustomers;
        set => SetProperty(ref _totalCustomers, value);
    }

    public decimal TodayRevenue
    {
        get => _todayRevenue;
        set => SetProperty(ref _todayRevenue, value);
    }

    public int TodayOrders
    {
        get => _todayOrders;
        set => SetProperty(ref _todayOrders, value);
    }

    public int LowStockCount
    {
        get => _lowStockCount;
        set => SetProperty(ref _lowStockCount, value);
    }

    public decimal MonthRevenue
    {
        get => _monthRevenue;
        set => SetProperty(ref _monthRevenue, value);
    }

    public DashboardViewModel()
    {
        LoadDashboardData();
    }

    private void LoadDashboardData()
    {
        try
        {
            using var context = new VClothesDbContext();
            TotalProducts = context.Products.Count(p => p.IsActive);
            TotalCategories = context.Categories.Count(c => c.IsActive);
            TotalEmployees = context.Employees.Count(e => e.IsActive);
            TotalCustomers = context.Customers.Count();

            var today = DateTime.Today;
            var todayInvoices = context.SalesInvoices
                .Where(s => s.InvoiceDate.Date == today)
                .ToList();
            TodayRevenue = todayInvoices.Sum(s => s.FinalAmount);
            TodayOrders = todayInvoices.Count;

            LowStockCount = context.Products.Count(p => p.IsActive && p.StockQuantity < 10);

            var monthStart = new DateTime(today.Year, today.Month, 1);
            MonthRevenue = context.SalesInvoices
                .Where(s => s.InvoiceDate >= monthStart && s.InvoiceDate <= today)
                .Sum(s => s.FinalAmount);
        }
        catch
        {
            // Database might not exist yet
        }
    }
}
