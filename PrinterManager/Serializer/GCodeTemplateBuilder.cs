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

    public GCodeTemplateCommandBuilder<T> AddType<T>(string code)
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

    public GCodeTemplateParameterBuilder<T> WithParameter<TProp>(Expression<Func<T, TProp>> propertySelector, string prefix)
    {
        if (propertySelector is LambdaExpression l)
        {
            if (l.Body is MemberExpression m)
            {
                if (m.Member is PropertyInfo p)
                {
                    return new GCodeTemplateParameterBuilder<T>(this, p, prefix);
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

    public GCodeTemplateCommandBuilder<T1> AddType<T1>(string code)
        where T1 : IPrinterRequest
    {
        Finish();
        return parentBuilder.AddType<T1>(code);
    }

    public GCodeCommandTemplate[] Build()
    {
        Finish();
        return parentBuilder.Build();
    }

    internal void FinishParameter(GCodeCommandParameter parameter)
    {
        currentParameters.Add(parameter);
    }

    private void Finish()
    {
        parentBuilder.FinishCommand(new GCodeCommandTemplate(typeof(T), code, currentParameters.ToArray()));
    }
}

public class GCodeTemplateParameterBuilder<T> : IGCodeParameterBuilder<T>
    where T : IPrinterRequest
{
    private readonly GCodeTemplateCommandBuilder<T> parentBuilder;
    private readonly PropertyInfo property;
    private readonly string prefix;

    private List<GCodeCommandParameter> currentParameters = new List<GCodeCommandParameter>();
    private bool currentParameterForceInclude = false;
    private bool currentParameterFlag = false;

    internal GCodeTemplateParameterBuilder(GCodeTemplateCommandBuilder<T> parentBuilder, PropertyInfo property, string prefix)
    {
        this.parentBuilder = parentBuilder;
        this.property = property;
        this.prefix = prefix;
    }

    public GCodeTemplateParameterBuilder<T> ForceInclude()
    {
        this.currentParameterForceInclude = true;
        return this;
    }

    public GCodeTemplateParameterBuilder<T> Flag()
    {
        if (property.PropertyType != typeof(bool))
        {
            throw new Exception("Can not mark parameter as flag if the property type is not bool");
        }

        currentParameterFlag = true;
        return this;
    }

    public GCodeTemplateParameterBuilder<T> WithParameter<TProp>(Expression<Func<T, TProp>> propertySelector, string prefix)
    {
        Finish();
        return parentBuilder.WithParameter(propertySelector, prefix);
    }

    public GCodeTemplateCommandBuilder<T1> AddType<T1>(string code)
        where T1 : IPrinterRequest
    {
        Finish();
        return parentBuilder.AddType<T1>(code);
    }

    public GCodeCommandTemplate[] Build()
    {
        Finish();
        return parentBuilder.Build();
    }

    private void Finish()
    {
        this.parentBuilder.FinishParameter(new GCodeCommandParameter(property.Name, prefix, currentParameterForceInclude, currentParameterFlag));
    }
}