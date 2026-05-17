using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VClothes.Models;

public class ChiTietHoaDonBan
{
    [Key]
    public int Id { get; set; }

    public int HoaDonBanId { get; set; }
    public HoaDonBan HoaDonBan { get; set; } = null!;

    public int SanPhamId { get; set; }
    public SanPham SanPham { get; set; } = null!;

    public int SoLuong { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal DonGia { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal GiamGia { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal ThanhTien { get; set; }
}
