using System.Windows;
using VClothes.Data;
using VClothes.Views;

namespace VClothes;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Xử lý ngoại lệ toàn cục để tránh crash
        DispatcherUnhandledException += (s, args) =>
        {
            args.Handled = true;
        };

        AppDomain.CurrentDomain.UnhandledException += (s, args) =>
        {
            // Ghi log nếu cần
        };

        // Kiểm tra kết nối Supabase
        try
        {
            SupabaseClient.KiemTraKetNoi();
        }
        catch
        {
            // App vẫn chạy - sẽ hiện lỗi khi truy cập dữ liệu
        }
    }
}
