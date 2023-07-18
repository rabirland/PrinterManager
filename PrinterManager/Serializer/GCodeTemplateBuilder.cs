using PrinterManager.Requests;
using System.Linq.Expressions;
using System.Reflection;

namespace PrinterManager.Serializer;

/// <summary>
/// Creates GCode templates in fluent style
/// </summary>
public class GCodeTemplateBuilder : IGcodeTemplateBuilder
{
    private List<GCodeCommandTemplate> commands = new List<GCodeCommandTemplate>();

    public GCodeTemplateCommandBuilder<T> AddCommand<T>(string code)
        where T : IPrinterRequest
    {
        return new GCodeTemplateCommandBuilder<T>(this, code);
    }

    public GCodeCommandTemplate[] Build()
    {
        return commands.ToArray();
    }

    internal void FinishCommand(GCodeCommandTemplate command)
    {
        commands.Add(command);
    }
}

public class GCodeTemplateCommandBuilder<T> : IGCodeCommandBuilder<T>
    where T : IPrinterRequest
{
    private readonly GCodeTemplateBuilder parentBuilder;
    private readonly string code;

    private List<GCodeCommandParameter> currentParameters = new List<GCodeCommandParameter>();

    internal GCodeTemplateCommandBuilder(GCodeTemplateBuilder parentBuilder, string code)
    {
        this.parentBuilder = parentBuilder;
        this.code = code;
    }

    public GCodeTemplateCommandBuilder<T> WithParameter<TProp>(Expression<Func<T, TProp>> propertySelector, string prefix)
    {
        if (propertySelector is LambdaExpression l)
        {
            if (l.Body is MemberExpression m)
            {
                if (m.Member is PropertyInfo p)
                {
                    var param = new GCodeCommandParameter(p.Name, prefix);
                    this.currentParameters.Add(param);
                    return this;
                }
                else
                {
                    throw new Exception("Invalid expression, must be a lamba pointing to a property");
                }
            }
            else
            {
                throw new Exception("Invalid expression, must be a lamba pointing to a property");
            }
        }
        else
        {
            throw new Exception("Invalid expression, must be a lamba pointing to a property");
        }
    }

    public GCodeTemplateCommandBuilder<T1> AddCommand<T1>(string code)
        where T1 : IPrinterRequest
    {
        Finish();
        return parentBuilder.AddCommand<T1>(code);
    }

    public GCodeCommandTemplate[] Build()
    {
        Finish();
        return parentBuilder.Build();
    }

    private void Finish()
    {
        parentBuilder.FinishCommand(new GCodeCommandTemplate(typeof(T), code, currentParameters.ToArray()));
    }
}