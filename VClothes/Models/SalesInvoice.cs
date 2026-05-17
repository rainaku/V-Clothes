using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VClothes.Models;

public class SalesInvoice
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string InvoiceCode { get; set; } = string.Empty;

    public DateTime InvoiceDate { get; set; } = DateTime.Now;

    public int? CustomerId { get; set; }
    public Customer? Customer { get; set; }

    public int EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Discount { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal FinalAmount { get; set; }

    [MaxLength(500)]
    public string? Note { get; set; }

    public ICollection<SalesInvoiceDetail> Details { get; set; } = new List<SalesInvoiceDetail>();
}
