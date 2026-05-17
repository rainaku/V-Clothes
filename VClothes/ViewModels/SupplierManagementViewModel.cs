using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using VClothes.Data;
using VClothes.Models;

namespace VClothes.ViewModels;

public class SupplierManagementViewModel : BaseViewModel
{
    private ObservableCollection<Supplier> _suppliers = new();
    private Supplier? _selectedSupplier;
    private string _searchText = string.Empty;
    private string _editName = string.Empty;
    private string _editAddress = string.Empty;
    private string _editPhone = string.Empty;
    private string _editEmail = string.Empty;
    private string _editNote = string.Empty;
    private bool _editIsActive = true;
    private bool _isEditing;
    private string _validationMessage = string.Empty;

    public ObservableCollection<Supplier> Suppliers
    {
        get => _suppliers;
        set => SetProperty(ref _suppliers, value);
    }

    public Supplier? SelectedSupplier
    {
        get => _selectedSupplier;
        set
        {
            if (SetProperty(ref _selectedSupplier, value) && value != null)
            {
                EditName = value.Name;
                EditAddress = value.Address ?? string.Empty;
                EditPhone = value.Phone ?? string.Empty;
                EditEmail = value.Email ?? string.Empty;
                EditNote = value.Note ?? string.Empty;
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
                SearchSuppliers();
        }
    }

    public string EditName { get => _editName; set => SetProperty(ref _editName, value); }
    public string EditAddress { get => _editAddress; set => SetProperty(ref _editAddress, value); }
    public string EditPhone { get => _editPhone; set => SetProperty(ref _editPhone, value); }
    public string EditEmail { get => _editEmail; set => SetProperty(ref _editEmail, value); }
    public string EditNote { get => _editNote; set => SetProperty(ref _editNote, value); }
    public bool EditIsActive { get => _editIsActive; set => SetProperty(ref _editIsActive, value); }
    public bool IsEditing { get => _isEditing; set => SetProperty(ref _isEditing, value); }
    public string ValidationMessage { get => _validationMessage; set => SetProperty(ref _validationMessage, value); }

    public ICommand AddCommand { get; }
    public ICommand SaveCommand { get; }
    public ICommand DeleteCommand { get; }
    public ICommand CancelCommand { get; }

    public SupplierManagementViewModel()
    {
        AddCommand = new RelayCommand(ExecuteAdd);
        SaveCommand = new RelayCommand(ExecuteSave);
        DeleteCommand = new RelayCommand(ExecuteDelete);
        CancelCommand = new RelayCommand(ExecuteCancel);
        try { LoadSuppliers(); } catch { }
    }

    private void LoadSuppliers()
    {
        using var context = new VClothesDbContext();
        var suppliers = context.Suppliers.OrderBy(s => s.Name).ToList();
        Suppliers = new ObservableCollection<Supplier>(suppliers);
    }

    private void SearchSuppliers()
    {
        using var context = new VClothesDbContext();
        var query = context.Suppliers.AsQueryable();

        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            query = query.Where(s => s.Name.Contains(SearchText) ||
                                     (s.Phone != null && s.Phone.Contains(SearchText)) ||
                                     (s.Email != null && s.Email.Contains(SearchText)));
        }

        var suppliers = query.OrderBy(s => s.Name).ToList();
        Suppliers = new ObservableCollection<Supplier>(suppliers);

        if (!suppliers.Any() && !string.IsNullOrWhiteSpace(SearchText))
            ValidationMessage = "Không tìm thấy nhà cung cấp nào phù hợp";
        else
            ValidationMessage = string.Empty;
    }

    private void ExecuteAdd(object? parameter)
    {
        SelectedSupplier = null;
        EditName = string.Empty;
        EditAddress = string.Empty;
        EditPhone = string.Empty;
        EditEmail = string.Empty;
        EditNote = string.Empty;
        EditIsActive = true;
        IsEditing = true;
        ValidationMessage = string.Empty;
    }

    private void ExecuteSave(object? parameter)
    {
        ValidationMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(EditName))
        {
            ValidationMessage = "Tên nhà cung cấp không được để trống";
            return;
        }

        if (!string.IsNullOrWhiteSpace(EditEmail) && !EditEmail.Contains('@'))
        {
            ValidationMessage = "Email không hợp lệ";
            return;
        }

        using var context = new VClothesDbContext();

        if (SelectedSupplier != null)
        {
            var supplier = context.Suppliers.Find(SelectedSupplier.Id);
            if (supplier != null)
            {
                supplier.Name = EditName;
                supplier.Address = EditAddress;
                supplier.Phone = EditPhone;
                supplier.Email = EditEmail;
                supplier.Note = EditNote;
                supplier.IsActive = EditIsActive;
                context.SaveChanges();
            }
        }
        else
        {
            var newSupplier = new Supplier
            {
                Name = EditName,
                Address = EditAddress,
                Phone = EditPhone,
                Email = EditEmail,
                Note = EditNote,
                IsActive = EditIsActive,
                CreatedAt = DateTime.Now
            };
            context.Suppliers.Add(newSupplier);
            context.SaveChanges();
        }

        LoadSuppliers();
        ExecuteCancel(null);
        ValidationMessage = "Lưu thành công!";
    }

    private void ExecuteDelete(object? parameter)
    {
        if (SelectedSupplier == null) return;

        var result = MessageBox.Show(
            $"Bạn có chắc muốn xóa nhà cung cấp '{SelectedSupplier.Name}'?",
            "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
            using var context = new VClothesDbContext();
            var supplier = context.Suppliers.Find(SelectedSupplier.Id);
            if (supplier != null)
            {
                var hasProducts = context.Products.Any(p => p.SupplierId == supplier.Id);
                if (hasProducts)
                {
                    ValidationMessage = "Không thể xóa nhà cung cấp đang có sản phẩm liên kết";
                    return;
                }
                context.Suppliers.Remove(supplier);
                context.SaveChanges();
                LoadSuppliers();
                ExecuteCancel(null);
            }
        }
    }

    private void ExecuteCancel(object? parameter)
    {
        SelectedSupplier = null;
        EditName = string.Empty;
        EditAddress = string.Empty;
        EditPhone = string.Empty;
        EditEmail = string.Empty;
        EditNote = string.Empty;
        EditIsActive = true;
        IsEditing = false;
    }
}
