using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using VClothes.Data;

namespace VClothes.ViewModels;

public class EmployeeManagementViewModel : BaseViewModel
{
    private ObservableCollection<EmployeeDto> _employees = new();
    private EmployeeDto? _selectedEmployee;
    private string _searchText = string.Empty;
    private bool _isEditing;
    private string _validationMessage = string.Empty;
    private string _sortColumn = "employee_code";
    private bool _sortAscending = true;

    private string _editEmployeeCode = string.Empty;
    private string _editFullName = string.Empty;
    private string? _editGender;
    private DateTime? _editDateOfBirth;
    private string _editPhone = string.Empty;
    private string _editEmail = string.Empty;
    private string _editAddress = string.Empty;
    private string _editPosition = string.Empty;
    private bool _editIsActive = true;

    public ObservableCollection<EmployeeDto> Employees { get => _employees; set => SetProperty(ref _employees, value); }
    public string SortColumn { get => _sortColumn; set => SetProperty(ref _sortColumn, value); }
    public bool SortAscending { get => _sortAscending; set => SetProperty(ref _sortAscending, value); }
    public EmployeeDto? SelectedEmployee { get => _selectedEmployee; set { if (SetProperty(ref _selectedEmployee, value) && value != null) LoadForEdit(value); } }
    public string SearchText { get => _searchText; set { if (SetProperty(ref _searchText, value)) Search(); } }
    public bool IsEditing { get => _isEditing; set => SetProperty(ref _isEditing, value); }
    public string ValidationMessage { get => _validationMessage; set => SetProperty(ref _validationMessage, value); }

    public string EditEmployeeCode { get => _editEmployeeCode; set => SetProperty(ref _editEmployeeCode, value); }
    public string EditFullName { get => _editFullName; set => SetProperty(ref _editFullName, value); }
    public string? EditGender { get => _editGender; set => SetProperty(ref _editGender, value); }
    public DateTime? EditDateOfBirth { get => _editDateOfBirth; set => SetProperty(ref _editDateOfBirth, value); }
    public string EditPhone { get => _editPhone; set => SetProperty(ref _editPhone, value); }
    public string EditEmail { get => _editEmail; set => SetProperty(ref _editEmail, value); }
    public string EditAddress { get => _editAddress; set => SetProperty(ref _editAddress, value); }
    public string EditPosition { get => _editPosition; set => SetProperty(ref _editPosition, value); }
    public bool EditIsActive { get => _editIsActive; set => SetProperty(ref _editIsActive, value); }

    public ICommand AddCommand { get; }
    public ICommand SaveCommand { get; }
    public ICommand DeleteCommand { get; }
    public ICommand CancelCommand { get; }
    public ICommand SortCommand { get; }

    public EmployeeManagementViewModel()
    {
        AddCommand = new RelayCommand(_ => ExecuteAdd());
        SaveCommand = new RelayCommand(_ => ExecuteSave());
        DeleteCommand = new RelayCommand(_ => ExecuteDelete());
        CancelCommand = new RelayCommand(_ => ExecuteCancel());
        SortCommand = new RelayCommand(ExecuteSort);
        IsLoading = true;
        LoadAsync();
    }

    private async void LoadAsync()
    {
        try { await Task.Run(() => Load()); }
        catch { }
        finally { IsLoading = false; }
    }

    private void Load()
    {
        var order = $"order={SortColumn}.{(SortAscending ? "asc" : "desc")}";
        var employees = SupabaseClient.Get<EmployeeDto>("employees", order);
        Employees = new ObservableCollection<EmployeeDto>(employees);
    }

    private void Search()
    {
        try
        {
            var order = $"order={SortColumn}.{(SortAscending ? "asc" : "desc")}";
            var query = order;
            if (!string.IsNullOrWhiteSpace(SearchText))
                query = $"or=(full_name.ilike.%25{Uri.EscapeDataString(SearchText)}%25,employee_code.ilike.%25{Uri.EscapeDataString(SearchText)}%25,phone.ilike.%25{Uri.EscapeDataString(SearchText)}%25)&{order}";
            Employees = new ObservableCollection<EmployeeDto>(SupabaseClient.Get<EmployeeDto>("employees", query));
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
            SortAscending = true;
        }
        Search();
    }

    private void LoadForEdit(EmployeeDto e)
    {
        EditEmployeeCode = e.EmployeeCode; EditFullName = e.FullName;
        EditGender = e.Gender; EditDateOfBirth = e.DateOfBirth;
        EditPhone = e.Phone ?? ""; EditEmail = e.Email ?? "";
        EditAddress = e.Address ?? ""; EditPosition = e.Position ?? "";
        EditIsActive = e.IsActive; IsEditing = true; ValidationMessage = "";
    }

    private void ExecuteAdd()
    {
        SelectedEmployee = null;
        EditEmployeeCode = EditFullName = EditPhone = EditEmail = EditAddress = EditPosition = "";
        EditGender = null; EditDateOfBirth = null; EditIsActive = true;
        IsEditing = true; ValidationMessage = "";
    }

    private void ExecuteSave()
    {
        ValidationMessage = "";
        if (string.IsNullOrWhiteSpace(EditEmployeeCode)) { ValidationMessage = "Mã nhân viên không được để trống"; return; }
        if (string.IsNullOrWhiteSpace(EditFullName)) { ValidationMessage = "Họ tên không được để trống"; return; }

        try
        {
            var existing = SupabaseClient.Get<EmployeeDto>("employees",
                $"employee_code=eq.{Uri.EscapeDataString(EditEmployeeCode)}&id=neq.{SelectedEmployee?.Id ?? 0}&select=id");
            if (existing.Any()) { ValidationMessage = "Mã nhân viên đã tồn tại"; return; }

            var data = new
            {
                employee_code = EditEmployeeCode,
                full_name = EditFullName,
                gender = string.IsNullOrWhiteSpace(EditGender) ? (string?)null : EditGender,
                date_of_birth = EditDateOfBirth?.ToString("yyyy-MM-dd"),
                phone = string.IsNullOrWhiteSpace(EditPhone) ? (string?)null : EditPhone,
                email = string.IsNullOrWhiteSpace(EditEmail) ? (string?)null : EditEmail,
                address = string.IsNullOrWhiteSpace(EditAddress) ? (string?)null : EditAddress,
                position = string.IsNullOrWhiteSpace(EditPosition) ? (string?)null : EditPosition,
                is_active = EditIsActive
            };

            if (SelectedEmployee != null)
                SupabaseClient.Update("employees", $"id=eq.{SelectedEmployee.Id}", data);
            else
                SupabaseClient.Insert<EmployeeDto>("employees", data);

            Load(); ExecuteCancel(); ValidationMessage = "Lưu thành công!";
        }
        catch (Exception ex) { ValidationMessage = $"Lỗi: {ex.Message}"; }
    }

    private void ExecuteDelete()
    {
        if (SelectedEmployee == null) return;
        if (MessageBox.Show($"Xóa nhân viên '{SelectedEmployee.FullName}'?", "Xác nhận", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
        {
            try { SupabaseClient.Delete("employees", $"id=eq.{SelectedEmployee.Id}"); Load(); ExecuteCancel(); }
            catch (Exception ex) { ValidationMessage = $"Lỗi: {ex.Message}"; }
        }
    }

    private void ExecuteCancel() { IsEditing = false; SelectedEmployee = null; }
}
