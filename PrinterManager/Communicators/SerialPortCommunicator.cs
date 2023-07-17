using System.IO.Ports;

namespace PrinterManager.Communicators;

/// <summary>
/// An <see cref="ICommunicator"/> that uses a serial port.
/// </summary>
public class SerialPortCommunicator : ICommunicator, IDisposable
{
    private SerialPort port = new SerialPort();
    private Task? listenerTask;

    /// <inheritdoc />
    public event Action<string>? OnMessage;

    /// <inheritdoc />
    public event Action? OnConnected;

    /// <inheritdoc />
    public bool IsOpen => port.IsOpen;

    /// <summary>
    /// The name of the current port.
    /// </summary>
    public string PortName => port.PortName;

    /// <summary>
    /// Gets the available serial ports.
    /// </summary>
    /// <returns>The list of serial port names.</returns>
    public static string[] GetAvailablePorts()
    {
        return SerialPort.GetPortNames();
    }

    /// <inheritdoc />
    public void Send(string command)
    {
        if (port == null)
        {
            throw new Exception("Port not opened");
        }

        //port.WriteLine(command);
        port.Write($"{command}\r\n");
    }

    /// <summary>
    /// Opens a serial port on the given name.
    /// </summary>
    /// <param name="portName">The serial port to open.</param>
    public void Open(string portName)
    {
        if (string.IsNullOrEmpty(portName))
        {
            throw new Exception("Invalid port name");
        }

        if (port.IsOpen)
        {
            Close();
        }

        port.ReadTimeout = -1;
        port.WriteTimeout = -1;
        port.PortName = portName;
        port.BaudRate = 115200;
        port.Open();
        listenerTask = Listener();

        OnConnected?.Invoke();
    }

    /// <summary>
    /// Closes the current serial port.
    /// </summary>
    public void Close()
    {
        port.Close();
        listenerTask = null;
    }

    /// <summary>
    /// Disposes the resources.
    /// </summary>
    public void Dispose()
    {
        port.Dispose();
    }

    private async Task Listener()
    {
        while (port.IsOpen)
        {
            try
            {
                var line = await Task.Run(port.ReadLine);
                OnMessage?.Invoke(line);
            }
            catch (Exception e)
            {
            }
        }
    }
}
