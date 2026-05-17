using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VClothes.Models;

public class PurchaseInvoice
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string InvoiceCode { get; set; } = string.Empty;

    public DateTime InvoiceDate { get; set; } = DateTime.Now;

    public int SupplierId { get; set; }
    public Supplier Supplier { get; set; } = null!;

    public int EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }

    [MaxLength(500)]
    public string? Note { get; set; }

    public ICollection<PurchaseInvoiceDetail> Details { get; set; } = new List<PurchaseInvoiceDetail>();
}
