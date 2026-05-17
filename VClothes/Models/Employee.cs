using System.ComponentModel.DataAnnotations;

namespace VClothes.Models;

public class Employee
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Mã nhân viên không được để trống")]
    [MaxLength(20)]
    public string EmployeeCode { get; set; } = string.Empty;

    [Required(ErrorMessage = "Họ tên không được để trống")]
    [MaxLength(100)]
    public string FullName { get; set; } = string.Empty;

    [MaxLength(10)]
    public string? Gender { get; set; }

    public DateTime? DateOfBirth { get; set; }

    [Phone]
    [MaxLength(20)]
    public string? Phone { get; set; }

    [EmailAddress]
    [MaxLength(100)]
    public string? Email { get; set; }

    [MaxLength(500)]
    public string? Address { get; set; }

    [MaxLength(100)]
    public string? Position { get; set; }

    public DateTime HireDate { get; set; } = DateTime.Now;

    public bool IsActive { get; set; } = true;

    public int? UserId { get; set; }
    public User? User { get; set; }
}
