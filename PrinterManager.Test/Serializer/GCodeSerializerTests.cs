using PrinterManager.Requests;
using PrinterManager.Serialization;

namespace PrinterManager.Test.Serializer;

[TestClass]
public class GCodeSerializerTests
{
    private static GCodeCommandTemplate[] Template =
        new GCodeTemplateBuilder()
            .AddCommand<TestRequestNoParam>("R1")
            .AddCommand<TestRequestP1>("R2")
                .WithParameter(c => c.Param, "A")
            .AddCommand<TestRequestP2>("R3")
                .WithParameter(c => c.Param1, "B")
                .WithParameter(c => c.Param2, "C")
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
    public void ShouldSkipNullValue()
    {
        var command = GCodeSerializer.Serialize(new TestRequestP1(null), Template);
        command.Should().Be("R2");
    }

    private readonly record struct TestRequestNoParam() : IPrinterRequest;
    private readonly record struct TestRequestP1(int? Param) : IPrinterRequest;
    private readonly record struct TestRequestP2(bool Param2, int Param1 = 3) : IPrinterRequest;
}
