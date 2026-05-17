using System.ComponentModel.DataAnnotations;

namespace VClothes.Models;

public class NhanVien
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Mã nhân viên không được để trống")]
    [MaxLength(20)]
    public string MaNhanVien { get; set; } = string.Empty;

    [Required(ErrorMessage = "Họ tên không được để trống")]
    [MaxLength(100)]
    public string HoTen { get; set; } = string.Empty;

    [MaxLength(10)]
    public string? GioiTinh { get; set; }

    public DateTime? NgaySinh { get; set; }

    [Phone]
    [MaxLength(20)]
    public string? DienThoai { get; set; }

    [EmailAddress]
    [MaxLength(100)]
    public string? Email { get; set; }

    [MaxLength(500)]
    public string? DiaChi { get; set; }

    [MaxLength(100)]
    public string? ChucVu { get; set; }

    public DateTime NgayVaoLam { get; set; } = DateTime.Now;

    public bool DangHoatDong { get; set; } = true;

    public int? NguoiDungId { get; set; }
    public NguoiDung? NguoiDung { get; set; }
}
