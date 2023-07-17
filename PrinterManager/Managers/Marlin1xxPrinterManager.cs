using PrinterManager.Poco;
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

    /// <inheritdoc />
    public void QueryTemperatures()
    {
        communicator.Send(GCode.QueryTemperatures);
    }

    /// <inheritdoc />
    public void MonitorTemperatures(int interval)
    {
        communicator.Send($"{GCode.MonitorTemperatures} S{interval}");
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

    private static class GCode
    {
        /// <summary>
        /// Instructs the printer to report the temperatures.
        /// </summary>
        public const string QueryTemperatures = "M105";

        /// <summary>
        /// Instructs the printer to continously report the temperatures.
        /// </summary>
        public const string MonitorTemperatures = "M155";
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
