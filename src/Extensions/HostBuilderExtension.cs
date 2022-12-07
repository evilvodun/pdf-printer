namespace Ders.Printer.Extensions;

using Ders.Printer.Contracts;
using Ders.Printer.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RawPrint;
using Serilog;

internal static class HostBuilderExtension
{
    public static IHostBuilder CreateBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureServices((_, services) =>
            {
                services.AddScoped<IPrinterService, PrinterService>();
                services.AddScoped<App>();
                services.AddScoped<IPrinter, RawPrint.Printer>();
            })
            .UseSerilog();
    }
}
