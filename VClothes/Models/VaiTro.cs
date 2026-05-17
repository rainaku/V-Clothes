using System.ComponentModel.DataAnnotations;

namespace VClothes.Models;

public class VaiTro
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Ten { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? MoTa { get; set; }

    public ICollection<NguoiDung> DanhSachNguoiDung { get; set; } = new List<NguoiDung>();
}
