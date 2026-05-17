using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using VClothes.Data;

namespace VClothes.ViewModels;

public class ProductManagementViewModel : BaseViewModel
{
    private ObservableCollection<ProductDto> _products = new();
    private ObservableCollection<CategoryDto> _categories = new();
    private ObservableCollection<CategoryDto> _allCategories = new();
    private ObservableCollection<SupplierDto> _allSuppliers = new();
    private ProductDto? _selectedProduct;
    private CategoryDto? _selectedFilterCategory;
    private string _searchText = string.Empty;
    private bool _isEditing;
    private string _validationMessage = string.Empty;

    private string _editProductCode = string.Empty;
    private string _editName = string.Empty;
    private string _editDescription = string.Empty;
    private decimal _editPrice;
    private decimal _editCostPrice;
    private string _editSize = string.Empty;
    private string _editColor = string.Empty;
    private string _editMaterial = string.Empty;
    private string _editImagePath = string.Empty;
    private CategoryDto? _editCategory;
    private SupplierDto? _editSupplier;
    private bool _editIsActive = true;

    public ObservableCollection<ProductDto> Products { get => _products; set => SetProperty(ref _products, value); }
    public ObservableCollection<CategoryDto> Categories { get => _categories; set => SetProperty(ref _categories, value); }
    public ObservableCollection<CategoryDto> AllCategories { get => _allCategories; set => SetProperty(ref _allCategories, value); }
    public ObservableCollection<SupplierDto> AllSuppliers { get => _allSuppliers; set => SetProperty(ref _allSuppliers, value); }
    public ProductDto? SelectedProduct { get => _selectedProduct; set { if (SetProperty(ref _selectedProduct, value) && value != null) LoadProductForEdit(value); } }
    public CategoryDto? SelectedFilterCategory { get => _selectedFilterCategory; set { if (SetProperty(ref _selectedFilterCategory, value)) SearchProducts(); } }
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
    public CategoryDto? EditCategory { get => _editCategory; set => SetProperty(ref _editCategory, value); }
    public SupplierDto? EditSupplier { get => _editSupplier; set => SetProperty(ref _editSupplier, value); }
    public bool EditIsActive { get => _editIsActive; set => SetProperty(ref _editIsActive, value); }

    public ICommand AddCommand { get; }
    public ICommand SaveCommand { get; }
    public ICommand DeleteCommand { get; }
    public ICommand CancelCommand { get; }
    public ICommand BrowseImageCommand { get; }

    public ProductManagementViewModel()
    {
        AddCommand = new RelayCommand(_ => ExecuteAdd());
        SaveCommand = new RelayCommand(_ => ExecuteSave());
        DeleteCommand = new RelayCommand(_ => ExecuteDelete());
        CancelCommand = new RelayCommand(_ => ExecuteCancel());
        BrowseImageCommand = new RelayCommand(_ => ExecuteBrowseImage());
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
        var products = SupabaseClient.Get<ProductDto>("products", "select=*,categories(*),suppliers(*)&order=name.asc");
        Products = new ObservableCollection<ProductDto>(products);

        var categories = SupabaseClient.Get<CategoryDto>("categories", "is_active=eq.true&order=name.asc");
        AllCategories = new ObservableCollection<CategoryDto>(categories);
        Categories = new ObservableCollection<CategoryDto>(categories);

        var suppliers = SupabaseClient.Get<SupplierDto>("suppliers", "is_active=eq.true&order=name.asc");
        AllSuppliers = new ObservableCollection<SupplierDto>(suppliers);
    }

    private void SearchProducts()
    {
        try
        {
            var filters = new List<string> { "select=*,categories(*),suppliers(*)", "order=name.asc" };

            if (!string.IsNullOrWhiteSpace(SearchText))
                filters.Add($"or=(name.ilike.%25{Uri.EscapeDataString(SearchText)}%25,product_code.ilike.%25{Uri.EscapeDataString(SearchText)}%25)");
            if (SelectedFilterCategory != null)
                filters.Add($"category_id=eq.{SelectedFilterCategory.Id}");

            var products = SupabaseClient.Get<ProductDto>("products", string.Join("&", filters));
            Products = new ObservableCollection<ProductDto>(products);
        }
        catch { }
    }

    private void LoadProductForEdit(ProductDto product)
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
        EditCategory = AllCategories.FirstOrDefault(c => c.Id == product.CategoryId);
        EditSupplier = AllSuppliers.FirstOrDefault(s => s.Id == product.SupplierId);
        EditIsActive = product.IsActive;
        IsEditing = true;
        ValidationMessage = string.Empty;
    }

    private void ExecuteAdd()
    {
        SelectedProduct = null;
        EditProductCode = EditName = EditDescription = EditSize = EditColor = EditMaterial = EditImagePath = string.Empty;
        EditPrice = EditCostPrice = 0;
        EditCategory = null; EditSupplier = null;
        EditIsActive = true; IsEditing = true; ValidationMessage = string.Empty;
    }

    private void ExecuteSave()
    {
        ValidationMessage = string.Empty;
        if (string.IsNullOrWhiteSpace(EditProductCode)) { ValidationMessage = "Mã sản phẩm không được để trống"; return; }
        if (string.IsNullOrWhiteSpace(EditName)) { ValidationMessage = "Tên sản phẩm không được để trống"; return; }
        if (EditCategory == null) { ValidationMessage = "Vui lòng chọn loại sản phẩm"; return; }
        if (EditSupplier == null) { ValidationMessage = "Vui lòng chọn nhà cung cấp"; return; }
        if (EditPrice <= 0) { ValidationMessage = "Giá bán phải lớn hơn 0"; return; }

        try
        {
            var existing = SupabaseClient.Get<ProductDto>("products",
                $"product_code=eq.{Uri.EscapeDataString(EditProductCode)}&id=neq.{SelectedProduct?.Id ?? 0}&select=id");
            if (existing.Any()) { ValidationMessage = "Mã sản phẩm đã tồn tại"; return; }

            var data = new
            {
                product_code = EditProductCode,
                name = EditName,
                description = string.IsNullOrWhiteSpace(EditDescription) ? (string?)null : EditDescription,
                price = EditPrice,
                cost_price = EditCostPrice,
                size = string.IsNullOrWhiteSpace(EditSize) ? (string?)null : EditSize,
                color = string.IsNullOrWhiteSpace(EditColor) ? (string?)null : EditColor,
                material = string.IsNullOrWhiteSpace(EditMaterial) ? (string?)null : EditMaterial,
                image_path = string.IsNullOrWhiteSpace(EditImagePath) ? (string?)null : EditImagePath,
                category_id = EditCategory.Id,
                supplier_id = EditSupplier.Id,
                is_active = EditIsActive
            };

            if (SelectedProduct != null)
                SupabaseClient.Update("products", $"id=eq.{SelectedProduct.Id}", data);
            else
                SupabaseClient.Insert<ProductDto>("products", data);

            LoadData();
            ExecuteCancel();
            ValidationMessage = "Lưu thành công!";
        }
        catch (Exception ex) { ValidationMessage = $"Lỗi: {ex.Message}"; }
    }

    private void ExecuteDelete()
    {
        if (SelectedProduct == null) return;
        if (MessageBox.Show($"Bạn có chắc muốn xóa '{SelectedProduct.Name}'?", "Xác nhận", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
        {
            try
            {
                SupabaseClient.Delete("products", $"id=eq.{SelectedProduct.Id}");
                LoadData(); ExecuteCancel();
            }
            catch (Exception ex) { ValidationMessage = $"Lỗi: {ex.Message}"; }
        }
    }

    private void ExecuteCancel() { IsEditing = false; SelectedProduct = null; }

    private void ExecuteBrowseImage()
    {
        var dialog = new OpenFileDialog { Filter = "Image files|*.png;*.jpg;*.jpeg;*.bmp" };
        if (dialog.ShowDialog() == true) EditImagePath = dialog.FileName;
    }
}
