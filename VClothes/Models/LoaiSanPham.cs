using System.ComponentModel.DataAnnotations;

namespace VClothes.Models;

public class LoaiSanPham
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Tên loại sản phẩm không được để trống")]
    [MaxLength(100)]
    public string Ten { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? MoTa { get; set; }

    public bool DangHoatDong { get; set; } = true;

    public DateTime NgayTao { get; set; } = DateTime.Now;

    public ICollection<SanPham> DanhSachSanPham { get; set; } = new List<SanPham>();
}
