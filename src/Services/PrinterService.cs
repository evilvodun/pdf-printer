namespace Ders.Printer.Services;

using Ders.Printer.Contracts;
using Ders.Printer.Logger;
using Ders.Printer.Requests;
using Microsoft.Extensions.Logging;
using RawPrint;
using static System.Drawing.Printing.PrinterSettings;

internal sealed class PrinterService : IPrinterService
{
    private readonly ILogger<PrinterService> _logger;
    private readonly IPrinter _printer;

    public PrinterService(ILogger<PrinterService> logger, IPrinter printer)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _printer = printer ?? throw new ArgumentNullException(nameof(printer));
    }

    public IList<Models.Printer> GetAllPrinters()
    {
        return (from object? printer in InstalledPrinters select new Models.Printer() { Name = printer.ToString() }).ToList();
    }

    public Models.Printer? SelectPrinter(string printerName)
    {
        if (string.IsNullOrWhiteSpace(printerName))
        {
            _logger.LogIsNullOrEmptyStringError(printerName);
            return null;
        }

        var printers = GetAllPrinters();
        var printer = printers.FirstOrDefault(p => p.Name != null && p.Name.ToLower().Contains(printerName.ToLower()));

        if (printer is null)
        {
            _logger.LogPrinterNotFoundError(printerName);
            return null;
        }

        return printer;
    }

    public bool Print(PrinterRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (string.IsNullOrWhiteSpace(request.PrinterName))
        {
            _logger.LogIsNullOrEmptyStringError(nameof(request));
            return false;
        }

        if (string.IsNullOrWhiteSpace(request.FilePath))
        {
            _logger.LogIsNullOrEmptyStringError(nameof(request));
            return false;
        }

        if (string.IsNullOrWhiteSpace(request.FileName))
        {
            _logger.LogIsNullOrEmptyStringError(nameof(request));
            return false;
        }

        _printer.PrintRawFile(request.PrinterName, request.FilePath, request.FileName);
        return true;
        
    }
}
