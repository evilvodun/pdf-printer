namespace Ders.Printer;

using Ders.Printer.Contracts;
using Ders.Printer.Logger;
using Ders.Printer.Requests;
using Microsoft.Extensions.Logging;
using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;

internal sealed class App
{
    private readonly IPrinterService _printerService;
    private readonly ILogger<App> _logger;

    public App(ILogger<App> logger, IPrinterService printerService)
    {
        _logger = logger;
        _printerService = printerService;
    }
    public async Task Run(string[] args)
    {
        await CreateCommandLineBuilder(args);
    }

    public async Task<int> CreateCommandLineBuilder(string[] args)
    {
        var allPrintersOptions = new Option<bool>(new[] { "--all-printers", "-a" }, "Show all printers on the system");
        var selectPrinterOption = new Option<string>(new[] { "--printer", "-p" }, "Select a printer to use");
        var filepathOption = new Option<string>(new[] { "--file-path", "-f" }, "Select a file to print");


        var command = new RootCommand("Select printers for events")
        {
            allPrintersOptions,
            selectPrinterOption,
            filepathOption
        };

        command.SetHandler(OnHandle, allPrintersOptions, selectPrinterOption, filepathOption);

        var commandLineBuilder = new CommandLineBuilder(command).UseDefaults();
        var parser = commandLineBuilder.Build();

        return await parser.InvokeAsync(args).ConfigureAwait(false);
    }

    public void OnHandle(bool allPrinters, string printerName, string filePath)
    {
        if (allPrinters)
        {
            GetAllPrinters();
        }
        else
        {
            PrintFile(printerName, filePath);
        }
    }

    private void GetAllPrinters()
    {
        var printers = _printerService.GetAllPrinters();

        foreach (var printer in printers)
        {
            Console.WriteLine(printer.Name);
        }
    }
    
    private void PrintFile(string printerName, string filePath)
    {
        var printer = _printerService.SelectPrinter(printerName);

        if (printer is null || string.IsNullOrWhiteSpace(printer.Name))
        {
            _logger.LogIsNullOrEmptyStringError(printerName);
        }
        else
        {
            _logger.LogPrinterSelectedInformation(printerName);

            var filename = ExtractFileName(filePath);
            var printerRequest = new PrinterRequest()
            {
                PrinterName = printer.Name,
                FileName = filename,
                FilePath = filePath
            };

            var response = _printerService.Print(printerRequest);

            if (response)
            {
                _logger.LogFilePrintedInformation(printerRequest.FileName);
            }
            else
            {
                _logger.LogFileNotPrintedError(printerRequest.FileName);
            }
        }
    }

    private static string ExtractFileName(string filePath)
    {
        return Path.GetFileName(filePath);
    }
}