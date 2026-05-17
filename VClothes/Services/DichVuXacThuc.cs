using System.Security.Cryptography;
using System.Text;
using VClothes.Data;

namespace VClothes.Services;

public class DichVuXacThuc
{
    private static NguoiDungDto? _nguoiDungHienTai;
    private static VaiTroDto? _vaiTroHienTai;

    public static NguoiDungDto? NguoiDungHienTai => _nguoiDungHienTai;
    public static VaiTroDto? VaiTroHienTai => _vaiTroHienTai;

    public static bool DangNhap(string tenDangNhap, string matKhau)
    {
        var matKhauHash = TinhMd5Hash(matKhau);

        var danhSachNguoiDung = SupabaseClient.LayDanhSach<NguoiDungDto>("users",
            $"username=eq.{Uri.EscapeDataString(tenDangNhap)}&password_hash=eq.{matKhauHash}&is_active=eq.true");

        if (danhSachNguoiDung.Count > 0)
        {
            _nguoiDungHienTai = danhSachNguoiDung[0];

            var danhSachVaiTro = SupabaseClient.LayDanhSach<VaiTroDto>("roles", $"id=eq.{_nguoiDungHienTai.VaiTroId}");
            _vaiTroHienTai = danhSachVaiTro.FirstOrDefault();

            // Cập nhật lần đăng nhập cuối
            SupabaseClient.CapNhat("users", $"id=eq.{_nguoiDungHienTai.Id}", new { last_login = DateTime.UtcNow });
            return true;
        }

        return false;
    }

    public static void DangXuat()
    {
        _nguoiDungHienTai = null;
        _vaiTroHienTai = null;
    }

    public static bool LaQuanTriVien => _vaiTroHienTai?.Ten == "Admin";
    public static bool LaQuanLy => _vaiTroHienTai?.Ten == "Manager" || LaQuanTriVien;
    public static bool LaNhanVien => _vaiTroHienTai?.Ten == "Staff" || LaQuanLy;

    public static string TinhMd5Hash(string dauVao)
    {
        var inputBytes = Encoding.UTF8.GetBytes(dauVao);
        var hashBytes = MD5.HashData(inputBytes);
        var sb = new StringBuilder();
        foreach (var b in hashBytes)
            sb.Append(b.ToString("x2"));
        return sb.ToString();
    }
}
