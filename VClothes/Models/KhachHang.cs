using System.ComponentModel.DataAnnotations;

namespace VClothes.Models;

public class KhachHang
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Tên khách hàng không được để trống")]
    [MaxLength(100)]
    public string HoTen { get; set; } = string.Empty;

    [Phone]
    [MaxLength(20)]
    public string? DienThoai { get; set; }

    [EmailAddress]
    [MaxLength(100)]
    public string? Email { get; set; }

    [MaxLength(500)]
    public string? DiaChi { get; set; }

    public DateTime NgayTao { get; set; } = DateTime.Now;

    public ICollection<HoaDonBan> DanhSachHoaDonBan { get; set; } = new List<HoaDonBan>();
}
