using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VClothes.Models;

public class PhieuNhap
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string MaPhieu { get; set; } = string.Empty;

    public DateTime NgayNhap { get; set; } = DateTime.Now;

    public int NhaCungCapId { get; set; }
    public NhaCungCap NhaCungCap { get; set; } = null!;

    public int NhanVienId { get; set; }
    public NhanVien NhanVien { get; set; } = null!;

    [Column(TypeName = "decimal(18,2)")]
    public decimal TongTien { get; set; }

    [MaxLength(500)]
    public string? GhiChu { get; set; }

    public ICollection<ChiTietPhieuNhap> DanhSachChiTiet { get; set; } = new List<ChiTietPhieuNhap>();
}
