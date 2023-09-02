using Microsoft.AspNetCore.Components.WebView.WindowsForms;
using Microsoft.Extensions.DependencyInjection;
using PrinterManager;
using PrinterManager.Clients;
using PrinterManager.CommandSerializers;
using PrinterManager.Communicators;
using PrinterManager.GCodeTemplates;

namespace UI;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();

        // Set up Blazor view
        var services = new ServiceCollection();

        services.RegisterAllScoped<IService>();
        services.AddSingleton<SerialPortCommunicator>();
        services.AddSingleton<ICommunicator>(x => x.GetRequiredService<SerialPortCommunicator>());

        services.AddSingleton(new GCodePrinterSerializer(Marlin1xxTemplate.CommandTemplate, Marlin1xxTemplate.ResponseTemplates));
        services.AddSingleton<IPrinterSerializer>(x => x.GetRequiredService<GCodePrinterSerializer>());

        services.AddSingleton<CommunicatorPrinterClient>();
        services.AddSingleton<IPrinterClient>(x => x.GetRequiredService<CommunicatorPrinterClient>());

        services.AddWindowsFormsBlazorWebView();
#if DEBUG
        services.AddBlazorWebViewDeveloperTools();
#endif
        blazorWebView1.HostPage = "wwwroot\\index.html";
        blazorWebView1.Services = services.BuildServiceProvider();
        blazorWebView1.RootComponents.Add<Pages.Index>("#app");
    }
}
