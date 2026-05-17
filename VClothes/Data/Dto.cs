using System.Text.Json.Serialization;

namespace VClothes.Data;

// DTOs ánh xạ trực tiếp với Supabase PostgREST JSON (snake_case)

public class VaiTroDto
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("name")] public string Ten { get; set; } = string.Empty;
    [JsonPropertyName("description")] public string? MoTa { get; set; }
}

public class NguoiDungDto
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("username")] public string TenDangNhap { get; set; } = string.Empty;
    [JsonPropertyName("password_hash")] public string MatKhauHash { get; set; } = string.Empty;
    [JsonPropertyName("display_name")] public string? TenHienThi { get; set; }
    [JsonPropertyName("is_active")] public bool DangHoatDong { get; set; }
    [JsonPropertyName("role_id")] public int VaiTroId { get; set; }
    [JsonPropertyName("created_at")] public DateTime? NgayTao { get; set; }
    [JsonPropertyName("last_login")] public DateTime? LanDangNhapCuoi { get; set; }
}

public class LoaiSanPhamDto
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("name")] public string Ten { get; set; } = string.Empty;
    [JsonPropertyName("description")] public string? MoTa { get; set; }
    [JsonPropertyName("is_active")] public bool DangHoatDong { get; set; } = true;
    [JsonPropertyName("created_at")] public DateTime? NgayTao { get; set; }
}

public class NhaCungCapDto
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("name")] public string Ten { get; set; } = string.Empty;
    [JsonPropertyName("address")] public string? DiaChi { get; set; }
    [JsonPropertyName("phone")] public string? DienThoai { get; set; }
    [JsonPropertyName("email")] public string? Email { get; set; }
    [JsonPropertyName("note")] public string? GhiChu { get; set; }
    [JsonPropertyName("is_active")] public bool DangHoatDong { get; set; } = true;
    [JsonPropertyName("created_at")] public DateTime? NgayTao { get; set; }
}

public class NhanVienDto
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("employee_code")] public string MaNhanVien { get; set; } = string.Empty;
    [JsonPropertyName("full_name")] public string HoTen { get; set; } = string.Empty;
    [JsonPropertyName("gender")] public string? GioiTinh { get; set; }
    [JsonPropertyName("date_of_birth")] public DateTime? NgaySinh { get; set; }
    [JsonPropertyName("phone")] public string? DienThoai { get; set; }
    [JsonPropertyName("email")] public string? Email { get; set; }
    [JsonPropertyName("address")] public string? DiaChi { get; set; }
    [JsonPropertyName("position")] public string? ChucVu { get; set; }
    [JsonPropertyName("hire_date")] public DateTime? NgayVaoLam { get; set; }
    [JsonPropertyName("is_active")] public bool DangHoatDong { get; set; } = true;
    [JsonPropertyName("user_id")] public int? NguoiDungId { get; set; }
}

public class KhachHangDto
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("full_name")] public string HoTen { get; set; } = string.Empty;
    [JsonPropertyName("phone")] public string? DienThoai { get; set; }
    [JsonPropertyName("email")] public string? Email { get; set; }
    [JsonPropertyName("address")] public string? DiaChi { get; set; }
    [JsonPropertyName("created_at")] public DateTime? NgayTao { get; set; }
}

public class SanPhamDto
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("product_code")] public string MaSanPham { get; set; } = string.Empty;
    [JsonPropertyName("name")] public string Ten { get; set; } = string.Empty;
    [JsonPropertyName("description")] public string? MoTa { get; set; }
    [JsonPropertyName("price")] public decimal GiaBan { get; set; }
    [JsonPropertyName("cost_price")] public decimal GiaNhap { get; set; }
    [JsonPropertyName("stock_quantity")] public int SoLuongTon { get; set; }
    [JsonPropertyName("size")] public string? KichCo { get; set; }
    [JsonPropertyName("color")] public string? MauSac { get; set; }
    [JsonPropertyName("material")] public string? ChatLieu { get; set; }
    [JsonPropertyName("image_path")] public string? DuongDanAnh { get; set; }
    [JsonPropertyName("category_id")] public int LoaiSanPhamId { get; set; }
    [JsonPropertyName("supplier_id")] public int NhaCungCapId { get; set; }
    [JsonPropertyName("is_active")] public bool DangHoatDong { get; set; } = true;
    [JsonPropertyName("created_at")] public DateTime? NgayTao { get; set; }

    [JsonPropertyName("categories")] public LoaiSanPhamDto? LoaiSanPham { get; set; }
    [JsonPropertyName("suppliers")] public NhaCungCapDto? NhaCungCap { get; set; }
}

public class PhieuNhapDto
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("invoice_code")] public string MaPhieu { get; set; } = string.Empty;
    [JsonPropertyName("invoice_date")] public DateTime NgayNhap { get; set; }
    [JsonPropertyName("supplier_id")] public int NhaCungCapId { get; set; }
    [JsonPropertyName("employee_id")] public int NhanVienId { get; set; }
    [JsonPropertyName("total_amount")] public decimal TongTien { get; set; }
    [JsonPropertyName("note")] public string? GhiChu { get; set; }

    [JsonPropertyName("suppliers")] public NhaCungCapDto? NhaCungCap { get; set; }
    [JsonPropertyName("employees")] public NhanVienDto? NhanVien { get; set; }
}

public class ChiTietPhieuNhapDto
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("purchase_invoice_id")] public int PhieuNhapId { get; set; }
    [JsonPropertyName("product_id")] public int SanPhamId { get; set; }
    [JsonPropertyName("quantity")] public int SoLuong { get; set; }
    [JsonPropertyName("unit_price")] public decimal DonGia { get; set; }
    [JsonPropertyName("sub_total")] public decimal ThanhTien { get; set; }

    [JsonPropertyName("products")] public SanPhamDto? SanPham { get; set; }
}

public class HoaDonBanDto
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("invoice_code")] public string MaHoaDon { get; set; } = string.Empty;
    [JsonPropertyName("invoice_date")] public DateTime NgayBan { get; set; }
    [JsonPropertyName("customer_id")] public int? KhachHangId { get; set; }
    [JsonPropertyName("employee_id")] public int NhanVienId { get; set; }
    [JsonPropertyName("total_amount")] public decimal TongTien { get; set; }
    [JsonPropertyName("discount")] public decimal GiamGia { get; set; }
    [JsonPropertyName("final_amount")] public decimal ThanhToan { get; set; }
    [JsonPropertyName("note")] public string? GhiChu { get; set; }

    [JsonPropertyName("customers")] public KhachHangDto? KhachHang { get; set; }
    [JsonPropertyName("employees")] public NhanVienDto? NhanVien { get; set; }
}

public class ChiTietHoaDonBanDto
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("sales_invoice_id")] public int HoaDonBanId { get; set; }
    [JsonPropertyName("product_id")] public int SanPhamId { get; set; }
    [JsonPropertyName("quantity")] public int SoLuong { get; set; }
    [JsonPropertyName("unit_price")] public decimal DonGia { get; set; }
    [JsonPropertyName("discount")] public decimal GiamGia { get; set; }
    [JsonPropertyName("sub_total")] public decimal ThanhTien { get; set; }

    [JsonPropertyName("products")] public SanPhamDto? SanPham { get; set; }
}
