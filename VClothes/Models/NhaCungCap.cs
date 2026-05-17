using System.ComponentModel.DataAnnotations;

namespace VClothes.Models;

public class NhaCungCap
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Tên nhà cung cấp không được để trống")]
    [MaxLength(200)]
    public string Ten { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? DiaChi { get; set; }

    [Phone]
    [MaxLength(20)]
    public string? DienThoai { get; set; }

    [EmailAddress]
    [MaxLength(100)]
    public string? Email { get; set; }

    [MaxLength(500)]
    public string? GhiChu { get; set; }

    public bool DangHoatDong { get; set; } = true;

    public DateTime NgayTao { get; set; } = DateTime.Now;

    public ICollection<SanPham> DanhSachSanPham { get; set; } = new List<SanPham>();
}
