using PrinterManager.Requests;
using System.Linq.Expressions;

namespace PrinterManager.Serializer;

public interface IGcodeTemplateBuilder
{
    GCodeTemplateCommandBuilder<T> AddType<T>(string code)
        where T : IPrinterRequest;
    GCodeCommandTemplate[] Build();
}

public interface IGCodeCommandBuilder<T> : IGcodeTemplateBuilder
    where T : IPrinterRequest
{
    GCodeTemplateParameterBuilder<T> WithParameter<TProp>(Expression<Func<T, TProp>> propertySelector, string prefix);
}

public interface IGCodeParameterBuilder<T> : IGCodeCommandBuilder<T>
    where T : IPrinterRequest
{
    GCodeTemplateParameterBuilder<T> ForceInclude();
    GCodeTemplateParameterBuilder<T> Flag();
}