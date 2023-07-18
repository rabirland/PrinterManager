using PrinterManager.Requests;
using PrinterManager.Serializer;

namespace PrinterManager.Test.Serializer;

[TestClass]
public class GCodeSerializerTests
{
    private static GCodeCommandTemplate[] Template =
        new GCodeTemplateBuilder()
            .AddType<TestRequestNoParam>("R1")
            .AddType<TestRequestP1>("R2")
                .WithParameter(c => c.Param, "A")
            .AddType<TestRequestP2>("R3")
                .WithParameter(c => c.Param1, "B")
                    .ForceInclude()
                .WithParameter(c => c.Param2, "C")
                    .Flag()
            .Build();

    [TestMethod]
    public void ShouldBuildSimpleCommand()
    {
        var command = GCodeSerializer.Serialize(new TestRequestNoParam(), Template);
        command.Should().Be("R1");
    }

    [TestMethod]
    public void ShouldBuildParameterizedCommand()
    {
        var command = GCodeSerializer.Serialize(new TestRequestP1(1), Template);
        command.Should().Be("R2 A1");
    }

    [TestMethod]
    public void ShouldSkipParameterDefaultValue()
    {
        var command = GCodeSerializer.Serialize(new TestRequestP1(5), Template);
        command.Should().Be("R2");
    }

    [TestMethod]
    public void ShouldNotSkipForcedParameter()
    {
        var command = GCodeSerializer.Serialize(new TestRequestP2(false, 3), Template);
        command.Should().Be("R3 B3");
    }

    [TestMethod]
    public void ShouldAddFlagCorrectly()
    {
        var command = GCodeSerializer.Serialize(new TestRequestP2(true, 3), Template);
        command.Should().Be("R3 B3 C");
    }

    private readonly record struct TestRequestNoParam() : IPrinterRequest;
    private readonly record struct TestRequestP1(int Param = 5) : IPrinterRequest;
    private readonly record struct TestRequestP2(bool Param2, int Param1 = 3) : IPrinterRequest;
}
