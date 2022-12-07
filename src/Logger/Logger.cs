namespace Ders.Printer.Logger;

using Microsoft.Extensions.Logging;

public static partial class Logger
{
    [LoggerMessage(EventId = 1000, Level = LogLevel.Error, Message = "No printer was found: {printer}")]
    public static partial void LogPrinterNotFoundError(this ILogger logger, string printer);

    [LoggerMessage(EventId = 1001, Level = LogLevel.Error, Message = "{propertyName} cannot be null or empty")]
    public static partial void LogIsNullOrEmptyStringError(this ILogger logger, string propertyName);

    [LoggerMessage(EventId = 1002, Level = LogLevel.Information, Message = "Printer Selected: {propertyName}")]
    public static partial void LogPrinterSelectedInformation(this ILogger logger, string propertyName);

    [LoggerMessage(EventId = 1003, Level = LogLevel.Error, Message = "Exception with message: {exceptionMessage}")]
    public static partial void LogExceptionError(this ILogger logger, string exceptionMessage, Exception exception);

    [LoggerMessage(EventId = 1004, Level = LogLevel.Information, Message = "File Successfully printed: {fileName}")]
    public static partial void LogFilePrintedInformation(this ILogger logger, string fileName);

    [LoggerMessage(EventId = 1005, Level = LogLevel.Error, Message = "File not printed: {fileName}")]
    public static partial void LogFileNotPrintedError(this ILogger logger, string fileName);
}

