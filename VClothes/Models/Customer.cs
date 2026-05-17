using System.ComponentModel.DataAnnotations;

namespace VClothes.Models;

public class Customer
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Tên khách hàng không được để trống")]
    [MaxLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Phone]
    [MaxLength(20)]
    public string? Phone { get; set; }

    [EmailAddress]
    [MaxLength(100)]
    public string? Email { get; set; }

    [MaxLength(500)]
    public string? Address { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public ICollection<SalesInvoice> SalesInvoices { get; set; } = new List<SalesInvoice>();
}
