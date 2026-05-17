using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using VClothes.Data;

namespace VClothes.ViewModels;

public class CategoryManagementViewModel : BaseViewModel
{
    private ObservableCollection<CategoryDto> _categories = new();
    private CategoryDto? _selectedCategory;
    private string _searchText = string.Empty;
    private string _editName = string.Empty;
    private string _editDescription = string.Empty;
    private bool _editIsActive = true;
    private bool _isEditing;
    private string _validationMessage = string.Empty;

    public ObservableCollection<CategoryDto> Categories { get => _categories; set => SetProperty(ref _categories, value); }
    public CategoryDto? SelectedCategory
    {
        get => _selectedCategory;
        set
        {
            if (SetProperty(ref _selectedCategory, value) && value != null)
            {
                EditName = value.Name;
                EditDescription = value.Description ?? string.Empty;
                EditIsActive = value.IsActive;
                IsEditing = true;
                ValidationMessage = string.Empty;
            }
        }
    }
    public string SearchText { get => _searchText; set { if (SetProperty(ref _searchText, value)) SearchCategories(); } }
    public string EditName { get => _editName; set => SetProperty(ref _editName, value); }
    public string EditDescription { get => _editDescription; set => SetProperty(ref _editDescription, value); }
    public bool EditIsActive { get => _editIsActive; set => SetProperty(ref _editIsActive, value); }
    public bool IsEditing { get => _isEditing; set => SetProperty(ref _isEditing, value); }
    public string ValidationMessage { get => _validationMessage; set => SetProperty(ref _validationMessage, value); }

    public ICommand AddCommand { get; }
    public ICommand SaveCommand { get; }
    public ICommand DeleteCommand { get; }
    public ICommand CancelCommand { get; }
    public ICommand SearchCommand { get; }

    public CategoryManagementViewModel()
    {
        AddCommand = new RelayCommand(_ => ExecuteAdd());
        SaveCommand = new RelayCommand(_ => ExecuteSave());
        DeleteCommand = new RelayCommand(_ => ExecuteDelete());
        CancelCommand = new RelayCommand(_ => ExecuteCancel());
        SearchCommand = new RelayCommand(_ => SearchCategories());
        try { LoadCategories(); } catch { }
    }

    private void LoadCategories()
    {
        var categories = SupabaseClient.Get<CategoryDto>("categories", "order=name.asc");
        Categories = new ObservableCollection<CategoryDto>(categories);
    }

    private void SearchCategories()
    {
        try
        {
            var query = "order=name.asc";
            if (!string.IsNullOrWhiteSpace(SearchText))
                query = $"or=(name.ilike.%25{Uri.EscapeDataString(SearchText)}%25,description.ilike.%25{Uri.EscapeDataString(SearchText)}%25)&order=name.asc";

            var categories = SupabaseClient.Get<CategoryDto>("categories", query);
            Categories = new ObservableCollection<CategoryDto>(categories);

            if (!categories.Any() && !string.IsNullOrWhiteSpace(SearchText))
                ValidationMessage = "Không tìm thấy loại sản phẩm nào phù hợp";
            else
                ValidationMessage = string.Empty;
        }
        catch { }
    }

    private void ExecuteAdd()
    {
        SelectedCategory = null;
        EditName = string.Empty;
        EditDescription = string.Empty;
        EditIsActive = true;
        IsEditing = true;
        ValidationMessage = string.Empty;
    }

    private void ExecuteSave()
    {
        ValidationMessage = string.Empty;
        if (string.IsNullOrWhiteSpace(EditName))
        { ValidationMessage = "Tên loại sản phẩm không được để trống"; return; }
        if (EditName.Length > 100)
        { ValidationMessage = "Tên loại sản phẩm không được quá 100 ký tự"; return; }

        try
        {
            // Check duplicate
            var existing = SupabaseClient.Get<CategoryDto>("categories",
                $"name=eq.{Uri.EscapeDataString(EditName)}&id=neq.{SelectedCategory?.Id ?? 0}");
            if (existing.Any())
            { ValidationMessage = "Tên loại sản phẩm đã tồn tại"; return; }

            if (SelectedCategory != null)
            {
                SupabaseClient.Update("categories", $"id=eq.{SelectedCategory.Id}", new
                {
                    name = EditName,
                    description = string.IsNullOrWhiteSpace(EditDescription) ? (string?)null : EditDescription,
                    is_active = EditIsActive
                });
            }
            else
            {
                SupabaseClient.Insert<CategoryDto>("categories", new
                {
                    name = EditName,
                    description = string.IsNullOrWhiteSpace(EditDescription) ? (string?)null : EditDescription,
                    is_active = EditIsActive
                });
            }

            LoadCategories();
            ExecuteCancel();
            ValidationMessage = "Lưu thành công!";
        }
        catch (Exception ex)
        {
            ValidationMessage = $"Lỗi: {ex.Message}";
        }
    }

    private void ExecuteDelete()
    {
        if (SelectedCategory == null) return;
        var result = MessageBox.Show($"Bạn có chắc muốn xóa loại sản phẩm '{SelectedCategory.Name}'?",
            "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
            try
            {
                var hasProducts = SupabaseClient.Get<ProductDto>("products",
                    $"category_id=eq.{SelectedCategory.Id}&select=id&limit=1");
                if (hasProducts.Any())
                { ValidationMessage = "Không thể xóa loại sản phẩm đang có sản phẩm liên kết"; return; }

                SupabaseClient.Delete("categories", $"id=eq.{SelectedCategory.Id}");
                LoadCategories();
                ExecuteCancel();
            }
            catch (Exception ex) { ValidationMessage = $"Lỗi: {ex.Message}"; }
        }
    }

    private void ExecuteCancel()
    {
        SelectedCategory = null;
        EditName = string.Empty;
        EditDescription = string.Empty;
        EditIsActive = true;
        IsEditing = false;
    }
}
