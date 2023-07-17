using Microsoft.AspNetCore.Components.WebView.WindowsForms;
using Microsoft.Extensions.DependencyInjection;
using PrinterManager;
using PrinterManager.Communicators;

namespace UI;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();

        // Set up Blazor view
        var services = new ServiceCollection();

        services.RegisterAllScoped<IService>();
        services.AddSingleton<ICommunicator, SerialPortCommunicator>();
        services.AddSingleton<SerialPortCommunicator>();

        services.AddWindowsFormsBlazorWebView();
#if DEBUG
        services.AddBlazorWebViewDeveloperTools();
#endif
        blazorWebView1.HostPage = "wwwroot\\index.html";
        blazorWebView1.Services = services.BuildServiceProvider();
        blazorWebView1.RootComponents.Add<Pages.Index>("#app");
    }
}
