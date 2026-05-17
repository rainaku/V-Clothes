using System.Windows;
using VClothes.Data;
using VClothes.Views;

namespace VClothes;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Global exception handlers to prevent crashes
        DispatcherUnhandledException += (s, args) =>
        {
            args.Handled = true;
        };

        AppDomain.CurrentDomain.UnhandledException += (s, args) =>
        {
            // Log if needed
        };

        // Test Supabase connection
        try
        {
            SupabaseClient.TestConnection();
        }
        catch
        {
            // App can still run - will show errors when accessing data
        }
    }
}
