namespace Ders.Printer.Contracts;

using Ders.Printer.Models;
using Ders.Printer.Requests;

public interface IPrinterService
{
    IList<Printer> GetAllPrinters();

    Models.Printer? SelectPrinter(string printerName);
    
    bool Print(PrinterRequest request);
}
