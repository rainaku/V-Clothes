using System.Collections.ObjectModel;
using System.Windows.Input;

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
    private DateTime _fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
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
        FilterCommand = new RelayCommand(_ => LoadStatistics());
    }

    private void LoadStatistics()
    {
        // TODO: Implement data loading
    }
}
