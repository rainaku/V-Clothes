using System.Collections.ObjectModel;
using System.Windows.Input;
using VClothes.Models;

namespace VClothes.ViewModels;

public class EmployeeManagementViewModel : BaseViewModel
{
    private ObservableCollection<Employee> _employees = new();
    private Employee? _selectedEmployee;
    private string _searchText = string.Empty;
    private bool _isEditing;
    private string _validationMessage = string.Empty;

    private string _editEmployeeCode = string.Empty;
    private string _editFullName = string.Empty;
    private string? _editGender;
    private DateTime? _editDateOfBirth;
    private string _editPhone = string.Empty;
    private string _editEmail = string.Empty;
    private string _editAddress = string.Empty;
    private string _editPosition = string.Empty;
    private bool _editIsActive = true;

    public ObservableCollection<Employee> Employees { get => _employees; set => SetProperty(ref _employees, value); }
    public Employee? SelectedEmployee { get => _selectedEmployee; set { if (SetProperty(ref _selectedEmployee, value) && value != null) LoadEmployeeForEdit(value); } }
    public string SearchText { get => _searchText; set { if (SetProperty(ref _searchText, value)) SearchEmployees(); } }
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

    public EmployeeManagementViewModel()
    {
        AddCommand = new RelayCommand(_ => ExecuteAdd());
        SaveCommand = new RelayCommand(_ => { /* TODO */ });
        DeleteCommand = new RelayCommand(_ => { /* TODO */ });
        CancelCommand = new RelayCommand(_ => ExecuteCancel());
    }

    private void LoadEmployeeForEdit(Employee emp)
    {
        EditEmployeeCode = emp.EmployeeCode;
        EditFullName = emp.FullName;
        EditGender = emp.Gender;
        EditDateOfBirth = emp.DateOfBirth;
        EditPhone = emp.Phone ?? string.Empty;
        EditEmail = emp.Email ?? string.Empty;
        EditAddress = emp.Address ?? string.Empty;
        EditPosition = emp.Position ?? string.Empty;
        EditIsActive = emp.IsActive;
        IsEditing = true;
    }

    private void ExecuteAdd()
    {
        SelectedEmployee = null;
        EditEmployeeCode = string.Empty;
        EditFullName = string.Empty;
        EditGender = null;
        EditDateOfBirth = null;
        EditPhone = string.Empty;
        EditEmail = string.Empty;
        EditAddress = string.Empty;
        EditPosition = string.Empty;
        EditIsActive = true;
        IsEditing = true;
        ValidationMessage = string.Empty;
    }

    private void ExecuteCancel()
    {
        IsEditing = false;
        SelectedEmployee = null;
    }

    private void SearchEmployees()
    {
        // TODO: Implement search
    }
}
