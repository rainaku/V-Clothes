using System.Collections.ObjectModel;
using System.Data;
using System.Windows.Input;

namespace VClothes.ViewModels;

public class ReportViewModel : BaseViewModel
{
    private ObservableCollection<string> _reportTypes = new()
    {
        "Báo cáo doanh thu theo ngày",
        "Báo cáo doanh thu theo tháng",
        "Báo cáo sản phẩm bán chạy",
        "Báo cáo tồn kho",
        "Báo cáo nhập hàng"
    };
    private string _selectedReportType = "Báo cáo doanh thu theo ngày";
    private DateTime _fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
    private DateTime _toDate = DateTime.Now;
    private string _reportTitle = "BÁO CÁO DOANH THU";
    private string _reportDateRange = string.Empty;
    private string _reportGeneratedDate = string.Empty;
    private bool _hasSummary;
    private int _summaryCount;
    private decimal _summaryRevenue;
    private decimal _summaryProfit;
    private DataTable? _reportData;

    public ObservableCollection<string> ReportTypes { get => _reportTypes; set => SetProperty(ref _reportTypes, value); }
    public string SelectedReportType { get => _selectedReportType; set => SetProperty(ref _selectedReportType, value); }
    public DateTime FromDate { get => _fromDate; set => SetProperty(ref _fromDate, value); }
    public DateTime ToDate { get => _toDate; set => SetProperty(ref _toDate, value); }
    public string ReportTitle { get => _reportTitle; set => SetProperty(ref _reportTitle, value); }
    public string ReportDateRange { get => _reportDateRange; set => SetProperty(ref _reportDateRange, value); }
    public string ReportGeneratedDate { get => _reportGeneratedDate; set => SetProperty(ref _reportGeneratedDate, value); }
    public bool HasSummary { get => _hasSummary; set => SetProperty(ref _hasSummary, value); }
    public int SummaryCount { get => _summaryCount; set => SetProperty(ref _summaryCount, value); }
    public decimal SummaryRevenue { get => _summaryRevenue; set => SetProperty(ref _summaryRevenue, value); }
    public decimal SummaryProfit { get => _summaryProfit; set => SetProperty(ref _summaryProfit, value); }
    public DataTable? ReportData { get => _reportData; set => SetProperty(ref _reportData, value); }

    public ICommand GenerateReportCommand { get; }

    public ReportViewModel()
    {
        GenerateReportCommand = new RelayCommand(_ => GenerateReport());
        ReportDateRange = $"Từ {FromDate:dd/MM/yyyy} đến {ToDate:dd/MM/yyyy}";
        ReportGeneratedDate = $"Ngày xuất: {DateTime.Now:dd/MM/yyyy HH:mm}";
    }

    private void GenerateReport()
    {
        ReportDateRange = $"Từ {FromDate:dd/MM/yyyy} đến {ToDate:dd/MM/yyyy}";
        ReportGeneratedDate = $"Ngày xuất: {DateTime.Now:dd/MM/yyyy HH:mm}";
        // TODO: Implement report generation
    }
}
