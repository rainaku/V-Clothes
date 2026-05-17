using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using VClothes.Data;
using VClothes.Models;

namespace VClothes.ViewModels;

public class CategoryManagementViewModel : BaseViewModel
{
    private ObservableCollection<Category> _categories = new();
    private Category? _selectedCategory;
    private string _searchText = string.Empty;
    private string _editName = string.Empty;
    private string _editDescription = string.Empty;
    private bool _editIsActive = true;
    private bool _isEditing;
    private string _validationMessage = string.Empty;

    public ObservableCollection<Category> Categories
    {
        get => _categories;
        set => SetProperty(ref _categories, value);
    }

    public Category? SelectedCategory
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
            }
        }
    }

    public string SearchText
    {
        get => _searchText;
        set
        {
            if (SetProperty(ref _searchText, value))
                SearchCategories();
        }
    }

    public string EditName
    {
        get => _editName;
        set => SetProperty(ref _editName, value);
    }

    public string EditDescription
    {
        get => _editDescription;
        set => SetProperty(ref _editDescription, value);
    }

    public bool EditIsActive
    {
        get => _editIsActive;
        set => SetProperty(ref _editIsActive, value);
    }

    public bool IsEditing
    {
        get => _isEditing;
        set => SetProperty(ref _isEditing, value);
    }

    public string ValidationMessage
    {
        get => _validationMessage;
        set => SetProperty(ref _validationMessage, value);
    }

    public ICommand AddCommand { get; }
    public ICommand SaveCommand { get; }
    public ICommand DeleteCommand { get; }
    public ICommand CancelCommand { get; }
    public ICommand SearchCommand { get; }

    public CategoryManagementViewModel()
    {
        AddCommand = new RelayCommand(ExecuteAdd);
        SaveCommand = new RelayCommand(ExecuteSave);
        DeleteCommand = new RelayCommand(ExecuteDelete);
        CancelCommand = new RelayCommand(ExecuteCancel);
        SearchCommand = new RelayCommand(ExecuteSearch);
        try { LoadCategories(); } catch { }
    }

    private void LoadCategories()
    {
        using var context = new VClothesDbContext();
        var categories = context.Categories.OrderBy(c => c.Name).ToList();
        Categories = new ObservableCollection<Category>(categories);
    }

    private void SearchCategories()
    {
        using var context = new VClothesDbContext();
        var query = context.Categories.AsQueryable();

        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            query = query.Where(c => c.Name.Contains(SearchText) || 
                                     (c.Description != null && c.Description.Contains(SearchText)));
        }

        var categories = query.OrderBy(c => c.Name).ToList();
        Categories = new ObservableCollection<Category>(categories);

        if (!categories.Any() && !string.IsNullOrWhiteSpace(SearchText))
        {
            ValidationMessage = "Không tìm thấy loại sản phẩm nào phù hợp";
        }
        else
        {
            ValidationMessage = string.Empty;
        }
    }

    private void ExecuteSearch(object? parameter)
    {
        SearchCategories();
    }

    private void ExecuteAdd(object? parameter)
    {
        SelectedCategory = null;
        EditName = string.Empty;
        EditDescription = string.Empty;
        EditIsActive = true;
        IsEditing = true;
        ValidationMessage = string.Empty;
    }

    private void ExecuteSave(object? parameter)
    {
        ValidationMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(EditName))
        {
            ValidationMessage = "Tên loại sản phẩm không được để trống";
            return;
        }

        if (EditName.Length > 100)
        {
            ValidationMessage = "Tên loại sản phẩm không được quá 100 ký tự";
            return;
        }

        using var context = new VClothesDbContext();

        // Check duplicate name
        var currentId = SelectedCategory?.Id ?? 0;
        var existingCategory = context.Categories
            .FirstOrDefault(c => c.Name == EditName && c.Id != currentId);
        if (existingCategory != null)
        {
            ValidationMessage = "Tên loại sản phẩm đã tồn tại";
            return;
        }

        if (SelectedCategory != null)
        {
            var category = context.Categories.Find(SelectedCategory.Id);
            if (category != null)
            {
                category.Name = EditName;
                category.Description = EditDescription;
                category.IsActive = EditIsActive;
                context.SaveChanges();
            }
        }
        else
        {
            var newCategory = new Category
            {
                Name = EditName,
                Description = EditDescription,
                IsActive = EditIsActive,
                CreatedAt = DateTime.Now
            };
            context.Categories.Add(newCategory);
            context.SaveChanges();
        }

        LoadCategories();
        ExecuteCancel(null);
        ValidationMessage = "Lưu thành công!";
    }

    private void ExecuteDelete(object? parameter)
    {
        if (SelectedCategory == null) return;

        var result = MessageBox.Show(
            $"Bạn có chắc muốn xóa loại sản phẩm '{SelectedCategory.Name}'?",
            "Xác nhận xóa",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
            using var context = new VClothesDbContext();
            var category = context.Categories.Find(SelectedCategory.Id);
            if (category != null)
            {
                var hasProducts = context.Products.Any(p => p.CategoryId == category.Id);
                if (hasProducts)
                {
                    ValidationMessage = "Không thể xóa loại sản phẩm đang có sản phẩm liên kết";
                    return;
                }

                context.Categories.Remove(category);
                context.SaveChanges();
                LoadCategories();
                ExecuteCancel(null);
            }
        }
    }

    private void ExecuteCancel(object? parameter)
    {
        SelectedCategory = null;
        EditName = string.Empty;
        EditDescription = string.Empty;
        EditIsActive = true;
        IsEditing = false;
    }
}
