using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using VClothes.Data;
using VClothes.Models;

namespace VClothes.Services;

public class AuthService
{
    private static User? _currentUser;
    private static Role? _currentRole;

    public static User? CurrentUser => _currentUser;
    public static Role? CurrentRole => _currentRole;

    public static bool Login(string username, string password)
    {
        using var context = new VClothesDbContext();
        var passwordHash = ComputeMd5Hash(password);

        var user = context.Users
            .Include(u => u.Role)
            .FirstOrDefault(u => u.Username == username && u.PasswordHash == passwordHash && u.IsActive);

        if (user != null)
        {
            _currentUser = user;
            _currentRole = user.Role;
            user.LastLogin = DateTime.Now;
            context.SaveChanges();
            return true;
        }

        return false;
    }

    public static void Logout()
    {
        _currentUser = null;
        _currentRole = null;
    }

    public static bool IsAdmin => _currentRole?.Name == "Admin";
    public static bool IsManager => _currentRole?.Name == "Manager" || IsAdmin;
    public static bool IsStaff => _currentRole?.Name == "Staff" || IsManager;

    public static bool HasPermission(string permission)
    {
        return permission switch
        {
            "ManageUsers" => IsAdmin,
            "ManageEmployees" => IsManager,
            "ManageProducts" => IsManager,
            "ManageCategories" => IsManager,
            "ManageSuppliers" => IsManager,
            "CreatePurchaseInvoice" => IsManager,
            "CreateSalesInvoice" => IsStaff,
            "ViewReports" => IsManager,
            "ViewStatistics" => IsManager,
            _ => IsStaff
        };
    }

    public static string ComputeMd5Hash(string input)
    {
        var inputBytes = Encoding.UTF8.GetBytes(input);
        var hashBytes = MD5.HashData(inputBytes);
        var sb = new StringBuilder();
        foreach (var b in hashBytes)
        {
            sb.Append(b.ToString("x2"));
        }
        return sb.ToString();
    }
}
