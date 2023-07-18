using PrinterManager.Requests;
using PrinterManager.Serializer;

namespace PrinterManager.Test.Serializer;

[TestClass]
public class GCodeTemplateBuilderTests
{
    [TestMethod]
    public void ShouldBuildSimpleTemplate()
    {
        var builder = new GCodeTemplateBuilder();
        var template = builder.AddType<Command1>("M1").Build();

        template.Should().HaveCount(1);
        template[0].RequestType.Should().Be(typeof(Command1));
        template[0].GCode.Should().Be("M1");

        template[0].Parameters.Should().HaveCount(0);
    }

    [TestMethod]
    public void ShouldBuildParameterizedTemplate()
    {
        var builder = new GCodeTemplateBuilder();
        var template = builder
            .AddType<Command2>("M1")
                .WithParameter(c => c.Param, "P")
            .Build();

        template.Should().HaveCount(1);
        template[0].RequestType.Should().Be(typeof(Command2));
        template[0].GCode.Should().Be("M1");

        template[0].Parameters.Should().HaveCount(1);
        template[0].Parameters[0].Name.Should().Be(nameof(Command2.Param));
        template[0].Parameters[0].Prefix.Should().Be("P");
    }

    [TestMethod]
    public void ShouldBuildParameterizedExtraTemplate()
    {
        var builder = new GCodeTemplateBuilder();
        var template = builder
            .AddType<Command3>("M1")
                .WithParameter(c => c.Param2, "P")
                .ForceInclude()
                .Flag()
            .Build();

        template.Should().HaveCount(1);
        template[0].RequestType.Should().Be(typeof(Command3));
        template[0].GCode.Should().Be("M1");

        template[0].Parameters.Should().HaveCount(1);
        template[0].Parameters[0].Name.Should().Be(nameof(Command3.Param2));
        template[0].Parameters[0].Prefix.Should().Be("P");
        template[0].Parameters[0].ForceInclude.Should().Be(true);
        template[0].Parameters[0].Flag.Should().Be(true);
    }

    [TestMethod]
    public void ShouldThrowErrorForNonBoolFlagParmeter()
    {
        var builder = new GCodeTemplateBuilder();
        var action = () => builder
            .AddType<Command2>("M1")
                .WithParameter(c => c.Param, "P")
                .Flag()
            .Build();

        action.Should().Throw<Exception>();
    }

    [TestMethod]
    public void ShouldBuildMultiParameterizedTemplate()
    {
        var builder = new GCodeTemplateBuilder();
        var template = builder
            .AddType<Command4>("M1")
                .WithParameter(c => c.Param2, "C")
                    .ForceInclude()
                    .Flag()
                .WithParameter(c => c.Param, "P")
            .Build();

        template.Should().HaveCount(1);
        template[0].RequestType.Should().Be(typeof(Command4));
        template[0].GCode.Should().Be("M1");

        template[0].Parameters.Should().HaveCount(2);
        template[0].Parameters[0].Name.Should().Be(nameof(Command4.Param2));
        template[0].Parameters[0].Prefix.Should().Be("C");
        template[0].Parameters[0].ForceInclude.Should().Be(true);
        template[0].Parameters[0].Flag.Should().Be(true);

        template[0].Parameters[1].Name.Should().Be(nameof(Command4.Param));
        template[0].Parameters[1].Prefix.Should().Be("P");
    }

    [TestMethod]
    public void ShouldBuildComplexTemplate()
    {
        var builder = new GCodeTemplateBuilder();
        var template = builder
            .AddType<Command4>("M1")
                .WithParameter(c => c.Param2, "C")
                    .ForceInclude()
                    .Flag()
                .WithParameter(c => c.Param, "P")
            .AddType<Command2>("M2")
                .WithParameter(c => c.Param, "G")
            .AddType<Command1>("M3")
            .Build();

        template.Should().HaveCount(3);
        template[0].RequestType.Should().Be(typeof(Command4));
        template[0].GCode.Should().Be("M1");

        template[0].Parameters.Should().HaveCount(2);
        template[0].Parameters[0].Name.Should().Be(nameof(Command4.Param2));
        template[0].Parameters[0].Prefix.Should().Be("C");
        template[0].Parameters[0].ForceInclude.Should().Be(true);
        template[0].Parameters[0].Flag.Should().Be(true);

        template[0].Parameters[1].Name.Should().Be(nameof(Command4.Param));
        template[0].Parameters[1].Prefix.Should().Be("P");

        template[1].RequestType.Should().Be(typeof(Command2));
        template[1].GCode.Should().Be("M2");

        template[1].Parameters.Should().HaveCount(1);
        template[1].Parameters[0].Name.Should().Be(nameof(Command2.Param));
        template[1].Parameters[0].Prefix.Should().Be("G");

        template[2].RequestType.Should().Be(typeof(Command1));
        template[2].GCode.Should().Be("M3");
    }

    private readonly record struct Command1() : IPrinterRequest;
    private readonly record struct Command2(int Param) : IPrinterRequest;
    private readonly record struct Command3(bool Param2) : IPrinterRequest;
    private readonly record struct Command4(int Param, bool Param2) : IPrinterRequest;
}
