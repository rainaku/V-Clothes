using System.ComponentModel.DataAnnotations;

namespace VClothes.Models;

public class Supplier
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Tên nhà cung cấp không được để trống")]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Address { get; set; }

    [Phone]
    [MaxLength(20)]
    public string? Phone { get; set; }

    [EmailAddress]
    [MaxLength(100)]
    public string? Email { get; set; }

    [MaxLength(500)]
    public string? Note { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public ICollection<Product> Products { get; set; } = new List<Product>();
}
