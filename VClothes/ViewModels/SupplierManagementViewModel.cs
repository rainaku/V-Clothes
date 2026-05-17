using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using VClothes.Data;

namespace VClothes.ViewModels;

public class SupplierManagementViewModel : BaseViewModel
{
    private ObservableCollection<SupplierDto> _suppliers = new();
    private SupplierDto? _selectedSupplier;
    private string _searchText = string.Empty;
    private string _editName = string.Empty;
    private string _editAddress = string.Empty;
    private string _editPhone = string.Empty;
    private string _editEmail = string.Empty;
    private string _editNote = string.Empty;
    private bool _editIsActive = true;
    private bool _isEditing;
    private string _validationMessage = string.Empty;

    public ObservableCollection<SupplierDto> Suppliers { get => _suppliers; set => SetProperty(ref _suppliers, value); }
    public SupplierDto? SelectedSupplier
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
                ValidationMessage = string.Empty;
            }
        }
    }
    public string SearchText { get => _searchText; set { if (SetProperty(ref _searchText, value)) SearchSuppliers(); } }
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
        AddCommand = new RelayCommand(_ => ExecuteAdd());
        SaveCommand = new RelayCommand(_ => ExecuteSave());
        DeleteCommand = new RelayCommand(_ => ExecuteDelete());
        CancelCommand = new RelayCommand(_ => ExecuteCancel());
        IsLoading = true;
        LoadAsync();
    }

    private async void LoadAsync()
    {
        try { await Task.Run(() => LoadSuppliers()); }
        catch { }
        finally { IsLoading = false; }
    }

    private void LoadSuppliers()
    {
        var suppliers = SupabaseClient.Get<SupplierDto>("suppliers", "order=name.asc");
        Suppliers = new ObservableCollection<SupplierDto>(suppliers);
    }

    private void SearchSuppliers()
    {
        try
        {
            var query = "order=name.asc";
            if (!string.IsNullOrWhiteSpace(SearchText))
                query = $"or=(name.ilike.%25{Uri.EscapeDataString(SearchText)}%25,phone.ilike.%25{Uri.EscapeDataString(SearchText)}%25,email.ilike.%25{Uri.EscapeDataString(SearchText)}%25)&order=name.asc";

            var suppliers = SupabaseClient.Get<SupplierDto>("suppliers", query);
            Suppliers = new ObservableCollection<SupplierDto>(suppliers);

            if (!suppliers.Any() && !string.IsNullOrWhiteSpace(SearchText))
                ValidationMessage = "Không tìm thấy nhà cung cấp nào phù hợp";
            else
                ValidationMessage = string.Empty;
        }
        catch { }
    }

    private void ExecuteAdd()
    {
        SelectedSupplier = null;
        EditName = EditAddress = EditPhone = EditEmail = EditNote = string.Empty;
        EditIsActive = true;
        IsEditing = true;
        ValidationMessage = string.Empty;
    }

    private void ExecuteSave()
    {
        ValidationMessage = string.Empty;
        if (string.IsNullOrWhiteSpace(EditName))
        { ValidationMessage = "Tên nhà cung cấp không được để trống"; return; }
        if (!string.IsNullOrWhiteSpace(EditEmail) && !EditEmail.Contains('@'))
        { ValidationMessage = "Email không hợp lệ"; return; }

        try
        {
            var data = new
            {
                name = EditName,
                address = string.IsNullOrWhiteSpace(EditAddress) ? (string?)null : EditAddress,
                phone = string.IsNullOrWhiteSpace(EditPhone) ? (string?)null : EditPhone,
                email = string.IsNullOrWhiteSpace(EditEmail) ? (string?)null : EditEmail,
                note = string.IsNullOrWhiteSpace(EditNote) ? (string?)null : EditNote,
                is_active = EditIsActive
            };

            if (SelectedSupplier != null)
                SupabaseClient.Update("suppliers", $"id=eq.{SelectedSupplier.Id}", data);
            else
                SupabaseClient.Insert<SupplierDto>("suppliers", data);

            LoadSuppliers();
            ExecuteCancel();
            ValidationMessage = "Lưu thành công!";
        }
        catch (Exception ex) { ValidationMessage = $"Lỗi: {ex.Message}"; }
    }

    private void ExecuteDelete()
    {
        if (SelectedSupplier == null) return;
        var result = MessageBox.Show($"Bạn có chắc muốn xóa nhà cung cấp '{SelectedSupplier.Name}'?",
            "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (result == MessageBoxResult.Yes)
        {
            try
            {
                var hasProducts = SupabaseClient.Get<ProductDto>("products", $"supplier_id=eq.{SelectedSupplier.Id}&select=id&limit=1");
                if (hasProducts.Any())
                { ValidationMessage = "Không thể xóa nhà cung cấp đang có sản phẩm liên kết"; return; }
                SupabaseClient.Delete("suppliers", $"id=eq.{SelectedSupplier.Id}");
                LoadSuppliers();
                ExecuteCancel();
            }
            catch (Exception ex) { ValidationMessage = $"Lỗi: {ex.Message}"; }
        }
    }

    private void ExecuteCancel()
    {
        SelectedSupplier = null;
        EditName = EditAddress = EditPhone = EditEmail = EditNote = string.Empty;
        EditIsActive = true;
        IsEditing = false;
    }
}
