using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VClothes.Models;

public class SanPham
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Mã sản phẩm không được để trống")]
    [MaxLength(50)]
    public string MaSanPham { get; set; } = string.Empty;

    [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
    [MaxLength(200)]
    public string Ten { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? MoTa { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal GiaBan { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal GiaNhap { get; set; }

    public int SoLuongTon { get; set; }

    [MaxLength(50)]
    public string? KichCo { get; set; }

    [MaxLength(50)]
    public string? MauSac { get; set; }

    [MaxLength(50)]
    public string? ChatLieu { get; set; }

    public string? DuongDanAnh { get; set; }

    public int LoaiSanPhamId { get; set; }
    public LoaiSanPham LoaiSanPham { get; set; } = null!;

    public int NhaCungCapId { get; set; }
    public NhaCungCap NhaCungCap { get; set; } = null!;

    public bool DangHoatDong { get; set; } = true;

    public DateTime NgayTao { get; set; } = DateTime.Now;

    public ICollection<ChiTietPhieuNhap> DanhSachChiTietPhieuNhap { get; set; } = new List<ChiTietPhieuNhap>();
    public ICollection<ChiTietHoaDonBan> DanhSachChiTietHoaDonBan { get; set; } = new List<ChiTietHoaDonBan>();
}
