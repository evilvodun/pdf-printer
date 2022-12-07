namespace Ders.Printer;

using Ders.Printer.Constants;
using Ders.Printer.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

internal class Program
{
    public static async Task Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File(BusinessConstants.LogPath, rollingInterval: RollingInterval.Day)
            .CreateLogger();

        using var host = HostBuilderExtension.CreateBuilder(args).Build();
        using var scope = host.Services.CreateScope();

        var services = scope.ServiceProvider;

        try
        {
            await services.GetRequiredService<App>().Run(args);
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, ex.Message);
        }
        finally
        {
            await Log.CloseAndFlushAsync();
        }
    }
}

