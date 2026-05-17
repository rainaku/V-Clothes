using System.Windows;
using VClothes.Data;

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

        // Ensure database is created
        try
        {
            using var context = new VClothesDbContext();
            context.Database.EnsureCreated();
        }
        catch
        {
            // App can still run without DB for UI purposes
        }
    }
}
