using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VClothes.Models;

public class ChiTietPhieuNhap
{
    [Key]
    public int Id { get; set; }

    public int PhieuNhapId { get; set; }
    public PhieuNhap PhieuNhap { get; set; } = null!;

    public int SanPhamId { get; set; }
    public SanPham SanPham { get; set; } = null!;

    public int SoLuong { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal DonGia { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal ThanhTien { get; set; }
}
