using PrinterManager.Requests;
using System.Linq.Expressions;

namespace PrinterManager.Serializer;

public interface IGcodeTemplateBuilder
{
    GCodeTemplateCommandBuilder<T> AddCommand<T>(string code)
        where T : IPrinterRequest;
    GCodeCommandTemplate[] Build();
}

public interface IGCodeCommandBuilder<T> : IGcodeTemplateBuilder
    where T : IPrinterRequest
{
}