using System.ComponentModel.DataAnnotations;

namespace VClothes.Models;

public class NguoiDung
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
    [MaxLength(50)]
    public string TenDangNhap { get; set; } = string.Empty;

    [Required(ErrorMessage = "Mật khẩu không được để trống")]
    [MaxLength(256)]
    public string MatKhauHash { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? TenHienThi { get; set; }

    public bool DangHoatDong { get; set; } = true;

    public int VaiTroId { get; set; }
    public VaiTro VaiTro { get; set; } = null!;

    public DateTime NgayTao { get; set; } = DateTime.Now;
    public DateTime? LanDangNhapCuoi { get; set; }
}
