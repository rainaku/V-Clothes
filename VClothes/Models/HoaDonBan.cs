using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VClothes.Models;

public class HoaDonBan
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string MaHoaDon { get; set; } = string.Empty;

    public DateTime NgayBan { get; set; } = DateTime.Now;

    public int? KhachHangId { get; set; }
    public KhachHang? KhachHang { get; set; }

    public int NhanVienId { get; set; }
    public NhanVien NhanVien { get; set; } = null!;

    [Column(TypeName = "decimal(18,2)")]
    public decimal TongTien { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal GiamGia { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal ThanhToan { get; set; }

    [MaxLength(500)]
    public string? GhiChu { get; set; }

    public ICollection<ChiTietHoaDonBan> DanhSachChiTiet { get; set; } = new List<ChiTietHoaDonBan>();
}
