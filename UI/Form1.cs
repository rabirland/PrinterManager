using Microsoft.AspNetCore.Components.WebView.WindowsForms;
using Microsoft.Extensions.DependencyInjection;
using PrinterManager;
using PrinterManager.Communicators;
using PrinterManager.Managers;

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

        services.AddSingleton<Marlin1xxPrinterManager>();
        services.AddSingleton<IPrinterManager>(x => x.GetRequiredService<Marlin1xxPrinterManager>());

        services.AddWindowsFormsBlazorWebView();
#if DEBUG
        services.AddBlazorWebViewDeveloperTools();
#endif
        blazorWebView1.HostPage = "wwwroot\\index.html";
        blazorWebView1.Services = services.BuildServiceProvider();
        blazorWebView1.RootComponents.Add<Pages.Index>("#app");
    }
}
