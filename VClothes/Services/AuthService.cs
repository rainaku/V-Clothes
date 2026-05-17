using System.Security.Cryptography;
using System.Text;
using VClothes.Data;

namespace VClothes.Services;

public class AuthService
{
    private static UserDto? _currentUser;
    private static RoleDto? _currentRole;

    public static UserDto? CurrentUser => _currentUser;
    public static RoleDto? CurrentRole => _currentRole;

    public static bool Login(string username, string password)
    {
        var passwordHash = ComputeMd5Hash(password);

        var users = SupabaseClient.Get<UserDto>("users",
            $"username=eq.{Uri.EscapeDataString(username)}&password_hash=eq.{passwordHash}&is_active=eq.true");

        if (users.Count > 0)
        {
            _currentUser = users[0];

            var roles = SupabaseClient.Get<RoleDto>("roles", $"id=eq.{_currentUser.RoleId}");
            _currentRole = roles.FirstOrDefault();

            // Update last login
            SupabaseClient.Update("users", $"id=eq.{_currentUser.Id}", new { last_login = DateTime.UtcNow });
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

    public static string ComputeMd5Hash(string input)
    {
        var inputBytes = Encoding.UTF8.GetBytes(input);
        var hashBytes = MD5.HashData(inputBytes);
        var sb = new StringBuilder();
        foreach (var b in hashBytes)
            sb.Append(b.ToString("x2"));
        return sb.ToString();
    }
}
