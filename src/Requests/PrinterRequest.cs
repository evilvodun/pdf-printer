namespace Ders.Printer.Requests;

public class PrinterRequest
{
    public string PrinterName { get; set; } = default!;

    public string FilePath { get; set; } = default!;

    public string FileName { get; set; } = default!;
}
