﻿using System.IO.Ports;

namespace PrinterManager.Communicators;

/// <summary>
/// An <see cref="ICommunicator"/> that uses a serial port.
/// </summary>
public class SerialPortCommunicator : ICommunicator, IDisposable
{
    private SerialPort port = new SerialPort();
    private Task? listenerTask;
    private bool listen = false;

    public event Action<string>? OnMessage;

    /// <inheritdoc />
    public bool IsOpen => port.IsOpen;

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

        port.WriteLine(command);
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

        port.ReadTimeout = 1000;
        port.WriteTimeout = 1000;
        port.PortName = portName;
        port.BaudRate = 115200;
        port.Open();

        listen = true;
        listenerTask = Listener();
    }

    /// <summary>
    /// Closes the current serial port.
    /// </summary>
    public void Close()
    {
        listen = false;
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
        while (listen)
        {
            var line = port.ReadLine();
            OnMessage?.Invoke(line);
            await Task.Delay(100);
        }
    }
}
