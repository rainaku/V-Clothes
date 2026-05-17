using System.Collections.ObjectModel;
using System.Windows.Input;
using VClothes.Data;

namespace VClothes.ViewModels;

public class SalesInvoiceDisplayItem
{
    public string InvoiceCode { get; set; } = string.Empty;
    public DateTime InvoiceDate { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string EmployeeName { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public decimal Discount { get; set; }
    public decimal FinalAmount { get; set; }
}

public class RevenueStatisticsViewModel : BaseViewModel
{
    private DateTime _fromDate = new DateTime(2024, 1, 1);
    private DateTime _toDate = DateTime.Now;
    private decimal _totalRevenue;
    private int _totalInvoices;
    private decimal _averagePerInvoice;
    private decimal _estimatedProfit;
    private ObservableCollection<SalesInvoiceDisplayItem> _salesInvoices = new();

    public DateTime FromDate { get => _fromDate; set => SetProperty(ref _fromDate, value); }
    public DateTime ToDate { get => _toDate; set => SetProperty(ref _toDate, value); }
    public decimal TotalRevenue { get => _totalRevenue; set => SetProperty(ref _totalRevenue, value); }
    public int TotalInvoices { get => _totalInvoices; set => SetProperty(ref _totalInvoices, value); }
    public decimal AveragePerInvoice { get => _averagePerInvoice; set => SetProperty(ref _averagePerInvoice, value); }
    public decimal EstimatedProfit { get => _estimatedProfit; set => SetProperty(ref _estimatedProfit, value); }
    public ObservableCollection<SalesInvoiceDisplayItem> SalesInvoices { get => _salesInvoices; set => SetProperty(ref _salesInvoices, value); }

    public ICommand FilterCommand { get; }

    public RevenueStatisticsViewModel()
    {
        FilterCommand = new RelayCommand(_ => LoadStatisticsAsync());
        IsLoading = true;
        LoadStatisticsAsync();
    }

    private async void LoadStatisticsAsync()
    {
        IsLoading = true;
        try { await Task.Run(() => LoadStatistics()); }
        catch { }
        finally { IsLoading = false; }
    }

    private void LoadStatistics()
    {
        try
        {
            var invoices = SupabaseClient.Get<SalesInvoiceDto>("sales_invoices",
                $"select=*,customers(*),employees(*)&invoice_date=gte.{FromDate:yyyy-MM-dd}&invoice_date=lte.{ToDate:yyyy-MM-dd}&order=invoice_date.desc");

            TotalInvoices = invoices.Count;
            TotalRevenue = invoices.Sum(i => i.FinalAmount);
            AveragePerInvoice = TotalInvoices > 0 ? TotalRevenue / TotalInvoices : 0;
            EstimatedProfit = TotalRevenue * 0.3m; // Estimate

            var displayItems = invoices.Select(i => new SalesInvoiceDisplayItem
            {
                InvoiceCode = i.InvoiceCode,
                InvoiceDate = i.InvoiceDate,
                CustomerName = i.Customer?.FullName ?? "Khách lẻ",
                EmployeeName = i.Employee?.FullName ?? "N/A",
                TotalAmount = i.TotalAmount,
                Discount = i.Discount,
                FinalAmount = i.FinalAmount
            }).ToList();

            SalesInvoices = new ObservableCollection<SalesInvoiceDisplayItem>(displayItems);
        }
        catch { }
    }
}
