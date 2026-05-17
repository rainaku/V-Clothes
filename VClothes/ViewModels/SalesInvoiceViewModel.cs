using System.Collections.ObjectModel;
using System.Windows.Input;
using VClothes.Data;

namespace VClothes.ViewModels;

public class SalesInvoiceViewModel : BaseViewModel
{
    private ObservableCollection<SalesInvoiceDto> _invoices = new();
    private ObservableCollection<CustomerDto> _customers = new();
    private ObservableCollection<ProductDto> _products = new();
    private ObservableCollection<InvoiceDetailItem> _invoiceDetails = new();
    private ObservableCollection<InvoiceDetailItem> _selectedInvoiceDetails = new();
    private SalesInvoiceDto? _selectedInvoice;
    private bool _isViewingDetail;
    private string _searchText = string.Empty;
    private bool _isCreating;
    private string _validationMessage = string.Empty;
    private DateTime? _filterFromDate;
    private DateTime? _filterToDate;
    private string _sortColumn = "invoice_date";
    private bool _sortAscending = false;

    private string _editInvoiceCode = string.Empty;
    private DateTime _editInvoiceDate = DateTime.Now;
    private CustomerDto? _editCustomer;
    private string _editNote = string.Empty;
    private ProductDto? _selectedProduct;
    private int _editQuantity = 1;
    private decimal _editUnitPrice;
    private decimal _editDiscount;
    private decimal _totalAmount;
    private decimal _finalAmount;

    public ObservableCollection<SalesInvoiceDto> Invoices { get => _invoices; set => SetProperty(ref _invoices, value); }
    public ObservableCollection<CustomerDto> Customers { get => _customers; set => SetProperty(ref _customers, value); }
    public ObservableCollection<ProductDto> Products { get => _products; set => SetProperty(ref _products, value); }
    public ObservableCollection<InvoiceDetailItem> InvoiceDetails { get => _invoiceDetails; set => SetProperty(ref _invoiceDetails, value); }
    public ObservableCollection<InvoiceDetailItem> SelectedInvoiceDetails { get => _selectedInvoiceDetails; set => SetProperty(ref _selectedInvoiceDetails, value); }
    public SalesInvoiceDto? SelectedInvoice { get => _selectedInvoice; set { if (SetProperty(ref _selectedInvoice, value)) LoadInvoiceDetail(); } }
    public bool IsViewingDetail { get => _isViewingDetail; set => SetProperty(ref _isViewingDetail, value); }
    public string SearchText { get => _searchText; set { if (SetProperty(ref _searchText, value)) SearchInvoices(); } }
    public bool IsCreating { get => _isCreating; set => SetProperty(ref _isCreating, value); }
    public string ValidationMessage { get => _validationMessage; set => SetProperty(ref _validationMessage, value); }
    public DateTime? FilterFromDate { get => _filterFromDate; set { if (SetProperty(ref _filterFromDate, value)) SearchInvoices(); } }
    public DateTime? FilterToDate { get => _filterToDate; set { if (SetProperty(ref _filterToDate, value)) SearchInvoices(); } }
    public string SortColumn { get => _sortColumn; set => SetProperty(ref _sortColumn, value); }
    public bool SortAscending { get => _sortAscending; set => SetProperty(ref _sortAscending, value); }

    public string EditInvoiceCode { get => _editInvoiceCode; set => SetProperty(ref _editInvoiceCode, value); }
    public DateTime EditInvoiceDate { get => _editInvoiceDate; set => SetProperty(ref _editInvoiceDate, value); }
    public CustomerDto? EditCustomer { get => _editCustomer; set => SetProperty(ref _editCustomer, value); }
    public string EditNote { get => _editNote; set => SetProperty(ref _editNote, value); }
    public ProductDto? SelectedProduct { get => _selectedProduct; set { if (SetProperty(ref _selectedProduct, value) && value != null) EditUnitPrice = value.Price; } }
    public int EditQuantity { get => _editQuantity; set => SetProperty(ref _editQuantity, value); }
    public decimal EditUnitPrice { get => _editUnitPrice; set => SetProperty(ref _editUnitPrice, value); }
    public decimal EditDiscount { get => _editDiscount; set { if (SetProperty(ref _editDiscount, value)) CalculateFinal(); } }
    public decimal TotalAmount { get => _totalAmount; set { if (SetProperty(ref _totalAmount, value)) CalculateFinal(); } }
    public decimal FinalAmount { get => _finalAmount; set => SetProperty(ref _finalAmount, value); }

    public ICommand CreateCommand { get; }
    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }
    public ICommand AddDetailCommand { get; }
    public ICommand ViewDetailCommand { get; }
    public ICommand CloseDetailCommand { get; }
    public ICommand SortCommand { get; }

    public SalesInvoiceViewModel()
    {
        CreateCommand = new RelayCommand(_ => ExecuteCreate());
        SaveCommand = new RelayCommand(_ => ExecuteSave());
        CancelCommand = new RelayCommand(_ => { IsCreating = false; });
        AddDetailCommand = new RelayCommand(_ => ExecuteAddDetail());
        ViewDetailCommand = new RelayCommand(ExecuteViewDetail);
        CloseDetailCommand = new RelayCommand(_ => { IsViewingDetail = false; SelectedInvoice = null; });
        SortCommand = new RelayCommand(ExecuteSort);
        IsLoading = true;
        LoadAsync();
    }

    private async void LoadAsync()
    {
        try { await Task.Run(() => LoadData()); }
        catch { }
        finally { IsLoading = false; }
    }

    private void LoadData()
    {
        var order = $"order={SortColumn}.{(SortAscending ? "asc" : "desc")}";
        Invoices = new ObservableCollection<SalesInvoiceDto>(
            SupabaseClient.Get<SalesInvoiceDto>("sales_invoices", $"select=*,customers(*)&{order}"));
        Customers = new ObservableCollection<CustomerDto>(
            SupabaseClient.Get<CustomerDto>("customers", "order=full_name.asc"));
        Products = new ObservableCollection<ProductDto>(
            SupabaseClient.Get<ProductDto>("products", "is_active=eq.true&stock_quantity=gt.0&order=name.asc"));
    }

    private void SearchInvoices()
    {
        try
        {
            var order = $"order={SortColumn}.{(SortAscending ? "asc" : "desc")}";
            var filters = new List<string> { "select=*,customers(*)", order };
            if (!string.IsNullOrWhiteSpace(SearchText))
                filters.Add($"invoice_code=ilike.%25{Uri.EscapeDataString(SearchText)}%25");
            if (FilterFromDate.HasValue)
                filters.Add($"invoice_date=gte.{FilterFromDate.Value:yyyy-MM-dd}");
            if (FilterToDate.HasValue)
                filters.Add($"invoice_date=lte.{FilterToDate.Value:yyyy-MM-dd}");
            Invoices = new ObservableCollection<SalesInvoiceDto>(
                SupabaseClient.Get<SalesInvoiceDto>("sales_invoices", string.Join("&", filters)));
        }
        catch { }
    }

    private void ExecuteSort(object? parameter)
    {
        var column = parameter?.ToString();
        if (string.IsNullOrEmpty(column)) return;
        if (SortColumn == column)
            SortAscending = !SortAscending;
        else
        {
            SortColumn = column;
            SortAscending = column == "invoice_date" ? false : true;
        }
        SearchInvoices();
    }

    private void CalculateFinal() { FinalAmount = Math.Max(0, TotalAmount - EditDiscount); }

    private void ExecuteCreate()
    {
        var count = Invoices.Count + 1;
        EditInvoiceCode = $"HD{count:D3}";
        EditInvoiceDate = DateTime.Now;
        EditCustomer = null; EditNote = ""; EditDiscount = 0;
        InvoiceDetails = new ObservableCollection<InvoiceDetailItem>();
        TotalAmount = 0; FinalAmount = 0; IsCreating = true; ValidationMessage = "";
    }

    private void ExecuteAddDetail()
    {
        ValidationMessage = "";
        if (SelectedProduct == null) { ValidationMessage = "Vui lòng chọn sản phẩm"; return; }
        if (EditQuantity <= 0) { ValidationMessage = "Số lượng phải lớn hơn 0"; return; }
        if (EditUnitPrice <= 0) { ValidationMessage = "Đơn giá phải lớn hơn 0"; return; }
        if (EditQuantity > SelectedProduct.StockQuantity) { ValidationMessage = $"Tồn kho chỉ còn {SelectedProduct.StockQuantity}"; return; }

        InvoiceDetails.Add(new InvoiceDetailItem
        {
            ProductId = SelectedProduct.Id,
            ProductName = SelectedProduct.Name,
            Quantity = EditQuantity,
            UnitPrice = EditUnitPrice
        });
        TotalAmount = InvoiceDetails.Sum(d => d.Total);
        SelectedProduct = null; EditQuantity = 1; EditUnitPrice = 0;
    }

    private void ExecuteSave()
    {
        ValidationMessage = "";
        if (string.IsNullOrWhiteSpace(EditInvoiceCode)) { ValidationMessage = "Mã hóa đơn không được để trống"; return; }
        if (!InvoiceDetails.Any()) { ValidationMessage = "Vui lòng thêm ít nhất 1 sản phẩm"; return; }

        try
        {
            var invoice = SupabaseClient.Insert<SalesInvoiceDto>("sales_invoices", new
            {
                invoice_code = EditInvoiceCode,
                invoice_date = EditInvoiceDate.ToString("yyyy-MM-dd"),
                customer_id = EditCustomer?.Id,
                employee_id = 2,
                total_amount = TotalAmount,
                discount = EditDiscount,
                final_amount = FinalAmount,
                note = string.IsNullOrWhiteSpace(EditNote) ? (string?)null : EditNote
            });

            if (invoice != null)
            {
                foreach (var d in InvoiceDetails)
                {
                    SupabaseClient.Insert<SalesInvoiceDetailDto>("sales_invoice_details", new
                    {
                        sales_invoice_id = invoice.Id,
                        product_id = d.ProductId,
                        quantity = d.Quantity,
                        unit_price = d.UnitPrice,
                        discount = 0,
                        sub_total = d.Total
                    });

                    var product = SupabaseClient.Get<ProductDto>("products", $"id=eq.{d.ProductId}&select=id,stock_quantity").FirstOrDefault();
                    if (product != null)
                        SupabaseClient.Update("products", $"id=eq.{d.ProductId}", new { stock_quantity = product.StockQuantity - d.Quantity });
                }
            }

            LoadData(); IsCreating = false; ValidationMessage = "Lưu hóa đơn thành công!";
        }
        catch (Exception ex) { ValidationMessage = $"Lỗi: {ex.Message}"; }
    }

    private void ExecuteViewDetail(object? parameter)
    {
        if (parameter is SalesInvoiceDto invoice)
        {
            if (SelectedInvoice?.Id == invoice.Id && IsViewingDetail)
            {
                IsViewingDetail = false;
                SelectedInvoice = null;
                return;
            }
            SelectedInvoice = invoice;
        }
    }

    private void LoadInvoiceDetail()
    {
        if (SelectedInvoice == null) { IsViewingDetail = false; return; }

        try
        {
            var details = SupabaseClient.Get<SalesInvoiceDetailDto>("sales_invoice_details",
                $"sales_invoice_id=eq.{SelectedInvoice.Id}&select=*,products(name,product_code)");

            SelectedInvoiceDetails = new ObservableCollection<InvoiceDetailItem>(
                details.Select(d => new InvoiceDetailItem
                {
                    ProductId = d.ProductId,
                    ProductName = d.Product?.Name ?? $"SP #{d.ProductId}",
                    Quantity = d.Quantity,
                    UnitPrice = d.UnitPrice
                }));
            IsViewingDetail = true;
        }
        catch { IsViewingDetail = false; }
    }
}
