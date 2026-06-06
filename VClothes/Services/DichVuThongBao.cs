namespace VClothes.Services;

public enum LoaiThongBao
{
    ThanhCong,
    Loi,
    CanhBao,
    ThongTin
}

/// <summary>
/// Dịch vụ thông báo toast dùng chung toàn ứng dụng.
/// Các ViewModel gọi <see cref="HienThi"/> để bắn thông báo, lớp giao diện chính
/// (ChinhViewModel/MainWindow) lắng nghe sự kiện này để hiển thị toast đồng bộ.
/// </summary>
public static class DichVuThongBao
{
    public static event Action<string, LoaiThongBao>? KhiCoThongBao;

    public static void HienThi(string thongDiep, LoaiThongBao loai = LoaiThongBao.ThongTin)
        => KhiCoThongBao?.Invoke(thongDiep, loai);

    public static void ThanhCong(string thongDiep) => HienThi(thongDiep, LoaiThongBao.ThanhCong);
    public static void Loi(string thongDiep) => HienThi(thongDiep, LoaiThongBao.Loi);
    public static void CanhBao(string thongDiep) => HienThi(thongDiep, LoaiThongBao.CanhBao);
}
