using System.Collections.ObjectModel;
using System.Windows.Input;
using VClothes.Models;

namespace VClothes.ViewModels;

public class ProductManagementViewModel : BaseViewModel
{
    private ObservableCollection<Product> _products = new();
    private ObservableCollection<Category> _categories = new();
    private ObservableCollection<Category> _allCategories = new();
    private ObservableCollection<Supplier> _allSuppliers = new();
    private Product? _selectedProduct;
    private Category? _selectedFilterCategory;
    private string _searchText = string.Empty;
    private bool _isEditing;
    private string _validationMessage = string.Empty;

    // Edit fields
    private string _editProductCode = string.Empty;
    private string _editName = string.Empty;
    private string _editDescription = string.Empty;
    private decimal _editPrice;
    private decimal _editCostPrice;
    private string _editSize = string.Empty;
    private string _editColor = string.Empty;
    private string _editMaterial = string.Empty;
    private string _editImagePath = string.Empty;
    private Category? _editCategory;
    private Supplier? _editSupplier;
    private bool _editIsActive = true;

    public ObservableCollection<Product> Products { get => _products; set => SetProperty(ref _products, value); }
    public ObservableCollection<Category> Categories { get => _categories; set => SetProperty(ref _categories, value); }
    public ObservableCollection<Category> AllCategories { get => _allCategories; set => SetProperty(ref _allCategories, value); }
    public ObservableCollection<Supplier> AllSuppliers { get => _allSuppliers; set => SetProperty(ref _allSuppliers, value); }
    public Product? SelectedProduct { get => _selectedProduct; set { if (SetProperty(ref _selectedProduct, value) && value != null) LoadProductForEdit(value); } }
    public Category? SelectedFilterCategory { get => _selectedFilterCategory; set { if (SetProperty(ref _selectedFilterCategory, value)) SearchProducts(); } }
    public string SearchText { get => _searchText; set { if (SetProperty(ref _searchText, value)) SearchProducts(); } }
    public bool IsEditing { get => _isEditing; set => SetProperty(ref _isEditing, value); }
    public string ValidationMessage { get => _validationMessage; set => SetProperty(ref _validationMessage, value); }

    public string EditProductCode { get => _editProductCode; set => SetProperty(ref _editProductCode, value); }
    public string EditName { get => _editName; set => SetProperty(ref _editName, value); }
    public string EditDescription { get => _editDescription; set => SetProperty(ref _editDescription, value); }
    public decimal EditPrice { get => _editPrice; set => SetProperty(ref _editPrice, value); }
    public decimal EditCostPrice { get => _editCostPrice; set => SetProperty(ref _editCostPrice, value); }
    public string EditSize { get => _editSize; set => SetProperty(ref _editSize, value); }
    public string EditColor { get => _editColor; set => SetProperty(ref _editColor, value); }
    public string EditMaterial { get => _editMaterial; set => SetProperty(ref _editMaterial, value); }
    public string EditImagePath { get => _editImagePath; set => SetProperty(ref _editImagePath, value); }
    public Category? EditCategory { get => _editCategory; set => SetProperty(ref _editCategory, value); }
    public Supplier? EditSupplier { get => _editSupplier; set => SetProperty(ref _editSupplier, value); }
    public bool EditIsActive { get => _editIsActive; set => SetProperty(ref _editIsActive, value); }

    public ICommand AddCommand { get; }
    public ICommand SaveCommand { get; }
    public ICommand DeleteCommand { get; }
    public ICommand CancelCommand { get; }
    public ICommand BrowseImageCommand { get; }

    public ProductManagementViewModel()
    {
        AddCommand = new RelayCommand(_ => ExecuteAdd());
        SaveCommand = new RelayCommand(_ => { /* TODO */ });
        DeleteCommand = new RelayCommand(_ => { /* TODO */ });
        CancelCommand = new RelayCommand(_ => ExecuteCancel());
        BrowseImageCommand = new RelayCommand(_ => { /* TODO */ });
    }

    private void LoadProductForEdit(Product product)
    {
        EditProductCode = product.ProductCode;
        EditName = product.Name;
        EditDescription = product.Description ?? string.Empty;
        EditPrice = product.Price;
        EditCostPrice = product.CostPrice;
        EditSize = product.Size ?? string.Empty;
        EditColor = product.Color ?? string.Empty;
        EditMaterial = product.Material ?? string.Empty;
        EditImagePath = product.ImagePath ?? string.Empty;
        EditIsActive = product.IsActive;
        IsEditing = true;
    }

    private void ExecuteAdd()
    {
        SelectedProduct = null;
        EditProductCode = string.Empty;
        EditName = string.Empty;
        EditDescription = string.Empty;
        EditPrice = 0;
        EditCostPrice = 0;
        EditSize = string.Empty;
        EditColor = string.Empty;
        EditMaterial = string.Empty;
        EditImagePath = string.Empty;
        EditIsActive = true;
        IsEditing = true;
        ValidationMessage = string.Empty;
    }

    private void ExecuteCancel()
    {
        IsEditing = false;
        SelectedProduct = null;
    }

    private void SearchProducts()
    {
        // TODO: Implement search
    }
}
