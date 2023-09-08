using System.IO.Ports;
using System.Text;

namespace PrinterManager.Communicators;

/// <summary>
/// An <see cref="ICommunicator"/> that uses a serial port.
/// </summary>
public class SerialPortCommunicator : ICommunicator
{
    private SerialPort port = new SerialPort();

    /// <inheritdoc />
    public event Action? OnConnected;

    /// <inheritdoc />
    public event Action? OnConnectionChanged;

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
    public void Send(Span<byte> command)
    {
        if (port == null)
        {
            throw new Exception("Port not opened");
        }

        port.Write($"{Encoding.ASCII.GetString(command)}\r\n");
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

        OnConnected?.Invoke();
        OnConnectionChanged?.Invoke();
    }

    /// <summary>
    /// Closes the current serial port.
    /// </summary>
    public void Close()
    {
        port.Close();
    }

    public byte[] Read()
    {
        if (port.IsOpen)
        {
            var msg = port.ReadExisting();
            var data = Encoding.ASCII.GetBytes(msg);

            return data;
        }
        else
        {
            return Array.Empty<byte>();
        }
        
    }
}
