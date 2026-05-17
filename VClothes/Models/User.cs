using System.ComponentModel.DataAnnotations;

namespace VClothes.Models;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
    [MaxLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Mật khẩu không được để trống")]
    [MaxLength(256)]
    public string PasswordHash { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? DisplayName { get; set; }

    public bool IsActive { get; set; } = true;

    public int RoleId { get; set; }
    public Role Role { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? LastLogin { get; set; }
}
