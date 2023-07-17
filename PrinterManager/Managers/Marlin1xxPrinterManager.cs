using PrinterManager.Poco;
using PrinterManager.Requests;
using PrinterManager.Serializer;
using PrinterManager.Templates;
using System.Text.RegularExpressions;

namespace PrinterManager.Managers;

/// <summary>
/// Printer manager for Marlin 1.x.x versions.
/// </summary>
public class Marlin1xxPrinterManager : IPrinterManager, IDisposable
{
    private readonly ICommunicator communicator;

    /// <inheritdoc />
    public ICommunicator Communicator => communicator;

    /// <inheritdoc />
    public event Action<IPrinterResponse>? Messages;

    public Marlin1xxPrinterManager(ICommunicator communicator)
    {
        this.communicator = communicator;
        this.communicator.OnMessage += OnMessage;
    }

    ~Marlin1xxPrinterManager()
    {
        Dispose();
    }

    public void SendCommand<T>(T command) where T : IPrinterRequest
    {
        var gcode = GCodeSerializer.Serialize(command, Marlin1xxTemplate.Template);
        communicator.Send(gcode);
    }

    public void Dispose()
    {
        this.communicator.OnMessage -= OnMessage;
    }

    private void OnMessage(string message)
    {
        if (Parser.TryParseTemperatureReport(message, out var tempReport))
        {
            Messages?.Invoke(tempReport);
        }
    }

    private static class Parser
    {
        private static readonly Regex TemperatureReportParser = new Regex("T:(?<HotendCurrent>\\d+(.\\d*)?)\\s*/(?<HotendTarget>\\d+(.\\d*)?)\\s*B:(?<BedCurrent>\\d+(.\\d*)?)\\s*/(?<BedTarget>\\d+(.\\d*)?)", RegexOptions.Compiled);

        public static bool TryParseTemperatureReport(string message, out TemperatureReport result)
        {
            var tempReport = TemperatureReportParser.Match(message);

            if (tempReport.Success)
            {
                float hotendCurrent = float.Parse(tempReport.Groups["HotendCurrent"].Value);
                float hotendTarget = float.Parse(tempReport.Groups["HotendTarget"].Value);
                float bedCurrent = float.Parse(tempReport.Groups["BedCurrent"].Value);
                float bedTarget = float.Parse(tempReport.Groups["BedTarget"].Value);

                result = new TemperatureReport(hotendCurrent, hotendTarget, bedCurrent, bedTarget);
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }
    }
}
