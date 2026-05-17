using System.Collections.ObjectModel;
using System.Data;
using System.Windows.Input;
using VClothes.Data;

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
    private DateTime _fromDate = new DateTime(2024, 1, 1);
    private DateTime _toDate = DateTime.Now;
    private string _reportTitle = "BÁO CÁO DOANH THU";
    private string _reportDateRange = string.Empty;
    private string _reportGeneratedDate = string.Empty;
    private bool _hasSummary;
    private int _summaryCount;
    private decimal _summaryRevenue;
    private decimal _summaryProfit;
    private DataView? _reportData;

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
    public DataView? ReportData { get => _reportData; set => SetProperty(ref _reportData, value); }

    public ICommand GenerateReportCommand { get; }

    public ReportViewModel()
    {
        GenerateReportCommand = new RelayCommand(_ => GenerateReport());
        ReportDateRange = $"Từ {FromDate:dd/MM/yyyy} đến {ToDate:dd/MM/yyyy}";
        ReportGeneratedDate = $"Ngày xuất: {DateTime.Now:dd/MM/yyyy HH:mm}";
        try { GenerateReport(); } catch { }
    }

    private void GenerateReport()
    {
        ReportDateRange = $"Từ {FromDate:dd/MM/yyyy} đến {ToDate:dd/MM/yyyy}";
        ReportGeneratedDate = $"Ngày xuất: {DateTime.Now:dd/MM/yyyy HH:mm}";

        try
        {
            switch (SelectedReportType)
            {
                case "Báo cáo doanh thu theo ngày": GenerateRevenueByDayReport(); break;
                case "Báo cáo doanh thu theo tháng": GenerateRevenueByMonthReport(); break;
                case "Báo cáo sản phẩm bán chạy": GenerateTopProductsReport(); break;
                case "Báo cáo tồn kho": GenerateInventoryReport(); break;
                case "Báo cáo nhập hàng": GeneratePurchaseReport(); break;
            }
        }
        catch { }
    }

    private void GenerateRevenueByDayReport()
    {
        ReportTitle = "BÁO CÁO DOANH THU THEO NGÀY";
        HasSummary = true;

        var invoices = SupabaseClient.Get<SalesInvoiceDto>("sales_invoices",
            $"invoice_date=gte.{FromDate:yyyy-MM-dd}&invoice_date=lte.{ToDate:yyyy-MM-dd}&order=invoice_date.asc");

        var grouped = invoices.GroupBy(i => i.InvoiceDate.Date)
            .Select(g => new { Date = g.Key, Count = g.Count(), Revenue = g.Sum(i => i.FinalAmount) }).ToList();

        SummaryCount = invoices.Count;
        SummaryRevenue = invoices.Sum(i => i.FinalAmount);
        SummaryProfit = SummaryRevenue * 0.3m;

        var table = new DataTable();
        table.Columns.Add("Ngày", typeof(string));
        table.Columns.Add("Số hóa đơn", typeof(int));
        table.Columns.Add("Doanh thu", typeof(string));
        foreach (var item in grouped)
            table.Rows.Add(item.Date.ToString("dd/MM/yyyy"), item.Count, item.Revenue.ToString("N0") + " ₫");
        ReportData = table.DefaultView;
    }

    private void GenerateRevenueByMonthReport()
    {
        ReportTitle = "BÁO CÁO DOANH THU THEO THÁNG";
        HasSummary = true;

        var invoices = SupabaseClient.Get<SalesInvoiceDto>("sales_invoices",
            $"invoice_date=gte.{FromDate:yyyy-MM-dd}&invoice_date=lte.{ToDate:yyyy-MM-dd}");

        var grouped = invoices.GroupBy(i => new { i.InvoiceDate.Year, i.InvoiceDate.Month })
            .Select(g => new { Month = $"{g.Key.Month:D2}/{g.Key.Year}", Count = g.Count(), Revenue = g.Sum(i => i.FinalAmount) }).ToList();

        SummaryCount = invoices.Count;
        SummaryRevenue = invoices.Sum(i => i.FinalAmount);
        SummaryProfit = SummaryRevenue * 0.3m;

        var table = new DataTable();
        table.Columns.Add("Tháng", typeof(string));
        table.Columns.Add("Số hóa đơn", typeof(int));
        table.Columns.Add("Doanh thu", typeof(string));
        foreach (var item in grouped)
            table.Rows.Add(item.Month, item.Count, item.Revenue.ToString("N0") + " ₫");
        ReportData = table.DefaultView;
    }

    private void GenerateTopProductsReport()
    {
        ReportTitle = "BÁO CÁO SẢN PHẨM BÁN CHẠY";
        HasSummary = false;

        var details = SupabaseClient.Get<SalesInvoiceDetailDto>("sales_invoice_details",
            "select=*,products(name,product_code)");

        var grouped = details.GroupBy(d => new { d.ProductId, Name = d.Product?.Name ?? "", Code = d.Product?.ProductCode ?? "" })
            .Select(g => new { g.Key.Code, g.Key.Name, Qty = g.Sum(d => d.Quantity), Revenue = g.Sum(d => d.SubTotal) })
            .OrderByDescending(g => g.Qty).Take(20).ToList();

        var table = new DataTable();
        table.Columns.Add("Mã SP", typeof(string));
        table.Columns.Add("Tên sản phẩm", typeof(string));
        table.Columns.Add("Số lượng bán", typeof(int));
        table.Columns.Add("Doanh thu", typeof(string));
        foreach (var item in grouped)
            table.Rows.Add(item.Code, item.Name, item.Qty, item.Revenue.ToString("N0") + " ₫");
        ReportData = table.DefaultView;
    }

    private void GenerateInventoryReport()
    {
        ReportTitle = "BÁO CÁO TỒN KHO";
        HasSummary = false;

        var products = SupabaseClient.Get<ProductDto>("products",
            "select=*,categories(name)&is_active=eq.true&order=stock_quantity.asc");

        var table = new DataTable();
        table.Columns.Add("Mã SP", typeof(string));
        table.Columns.Add("Tên sản phẩm", typeof(string));
        table.Columns.Add("Loại", typeof(string));
        table.Columns.Add("Tồn kho", typeof(int));
        table.Columns.Add("Giá trị tồn", typeof(string));
        table.Columns.Add("Trạng thái", typeof(string));

        foreach (var p in products)
        {
            var status = p.StockQuantity < 10 ? "⚠️ Sắp hết" : "✅ Đủ hàng";
            table.Rows.Add(p.ProductCode, p.Name, p.Category?.Name ?? "", p.StockQuantity,
                (p.StockQuantity * p.CostPrice).ToString("N0") + " ₫", status);
        }
        ReportData = table.DefaultView;
    }

    private void GeneratePurchaseReport()
    {
        ReportTitle = "BÁO CÁO NHẬP HÀNG";
        HasSummary = true;

        var invoices = SupabaseClient.Get<PurchaseInvoiceDto>("purchase_invoices",
            $"select=*,suppliers(name)&invoice_date=gte.{FromDate:yyyy-MM-dd}&invoice_date=lte.{ToDate:yyyy-MM-dd}&order=invoice_date.desc");

        SummaryCount = invoices.Count;
        SummaryRevenue = invoices.Sum(i => i.TotalAmount);
        SummaryProfit = 0;

        var table = new DataTable();
        table.Columns.Add("Mã phiếu", typeof(string));
        table.Columns.Add("Ngày nhập", typeof(string));
        table.Columns.Add("Nhà cung cấp", typeof(string));
        table.Columns.Add("Tổng tiền", typeof(string));
        table.Columns.Add("Ghi chú", typeof(string));

        foreach (var i in invoices)
            table.Rows.Add(i.InvoiceCode, i.InvoiceDate.ToString("dd/MM/yyyy"),
                i.Supplier?.Name ?? "", i.TotalAmount.ToString("N0") + " ₫", i.Note ?? "");
        ReportData = table.DefaultView;
    }
}
