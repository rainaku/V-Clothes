using System.Collections.ObjectModel;
using System.Windows.Input;
using VClothes.Models;

namespace VClothes.ViewModels;

public class InvoiceDetailItem : BaseViewModel
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Total => Quantity * UnitPrice;
}

public class PurchaseInvoiceViewModel : BaseViewModel
{
    private ObservableCollection<PurchaseInvoice> _invoices = new();
    private ObservableCollection<Supplier> _suppliers = new();
    private ObservableCollection<Product> _products = new();
    private ObservableCollection<InvoiceDetailItem> _invoiceDetails = new();
    private PurchaseInvoice? _selectedInvoice;
    private string _searchText = string.Empty;
    private bool _isCreating;
    private string _validationMessage = string.Empty;
    private DateTime? _filterFromDate;
    private DateTime? _filterToDate;

    private string _editInvoiceCode = string.Empty;
    private DateTime _editInvoiceDate = DateTime.Now;
    private Supplier? _editSupplier;
    private string _editNote = string.Empty;
    private Product? _selectedProduct;
    private int _editQuantity;
    private decimal _editUnitPrice;
    private decimal _totalAmount;

    public ObservableCollection<PurchaseInvoice> Invoices { get => _invoices; set => SetProperty(ref _invoices, value); }
    public ObservableCollection<Supplier> Suppliers { get => _suppliers; set => SetProperty(ref _suppliers, value); }
    public ObservableCollection<Product> Products { get => _products; set => SetProperty(ref _products, value); }
    public ObservableCollection<InvoiceDetailItem> InvoiceDetails { get => _invoiceDetails; set => SetProperty(ref _invoiceDetails, value); }
    public PurchaseInvoice? SelectedInvoice { get => _selectedInvoice; set => SetProperty(ref _selectedInvoice, value); }
    public string SearchText { get => _searchText; set => SetProperty(ref _searchText, value); }
    public bool IsCreating { get => _isCreating; set => SetProperty(ref _isCreating, value); }
    public string ValidationMessage { get => _validationMessage; set => SetProperty(ref _validationMessage, value); }
    public DateTime? FilterFromDate { get => _filterFromDate; set => SetProperty(ref _filterFromDate, value); }
    public DateTime? FilterToDate { get => _filterToDate; set => SetProperty(ref _filterToDate, value); }

    public string EditInvoiceCode { get => _editInvoiceCode; set => SetProperty(ref _editInvoiceCode, value); }
    public DateTime EditInvoiceDate { get => _editInvoiceDate; set => SetProperty(ref _editInvoiceDate, value); }
    public Supplier? EditSupplier { get => _editSupplier; set => SetProperty(ref _editSupplier, value); }
    public string EditNote { get => _editNote; set => SetProperty(ref _editNote, value); }
    public Product? SelectedProduct { get => _selectedProduct; set => SetProperty(ref _selectedProduct, value); }
    public int EditQuantity { get => _editQuantity; set => SetProperty(ref _editQuantity, value); }
    public decimal EditUnitPrice { get => _editUnitPrice; set => SetProperty(ref _editUnitPrice, value); }
    public decimal TotalAmount { get => _totalAmount; set => SetProperty(ref _totalAmount, value); }

    public ICommand CreateCommand { get; }
    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }
    public ICommand AddDetailCommand { get; }

    public PurchaseInvoiceViewModel()
    {
        CreateCommand = new RelayCommand(_ => { IsCreating = true; });
        SaveCommand = new RelayCommand(_ => { /* TODO */ });
        CancelCommand = new RelayCommand(_ => { IsCreating = false; });
        AddDetailCommand = new RelayCommand(_ => { /* TODO */ });
    }
}
