using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Konek.Client;
using Konek.Desktop.ViewModels;
using Konek.Desktop.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Konek.Desktop;

public partial class App : Application
{
    public static MainWindow MainWindow { get; private set; } = null!;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var services = BuildServices();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = services.GetRequiredService<MainWindowViewModel>(),
            };
            MainWindow = (MainWindow)desktop.MainWindow;
        }

        base.OnFrameworkInitializationCompleted();
    }

    private ServiceProvider BuildServices()
    {
        // Configuration
        var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "";
        var localConfigPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var config = new ConfigurationBuilder()
            .AddJsonFile(Path.Combine(assemblyPath, "config.json"), optional: true)
            .AddJsonFile(Path.Combine(localConfigPath, "konek", "config.json"), optional: true)
            .Build();

        var services = new ServiceCollection()
            .AddTransient<MainWindowViewModel>()
            .AddTransient<LightControlViewModel>()
            .AddTransient<RoutineViewModel>()
            .AddTransient<AddLampViewModel>()
            .AddSingleton(_ => config);

        string serverUrl = config["serverUrl"]!;
        services.AddHttpClient<IHubClient, HubClient>(client => client.BaseAddress = new Uri(serverUrl))
            .ConfigurePrimaryHttpMessageHandler(CreateHttpClientHandler);

        services.AddHttpClient<IGroupClient, GroupClient>(client => client.BaseAddress = new Uri(serverUrl))
            .ConfigurePrimaryHttpMessageHandler(CreateHttpClientHandler);

        services.AddHttpClient<ILampClient, LampClient>(client => client.BaseAddress = new Uri(serverUrl))
            .ConfigurePrimaryHttpMessageHandler(CreateHttpClientHandler);

        services.AddHttpClient<IRoutineDefinitionClient, RoutineDefinitionClient>(client => client.BaseAddress = new Uri(serverUrl))
            .ConfigurePrimaryHttpMessageHandler(CreateHttpClientHandler);

        return services.BuildServiceProvider();
    }

    private HttpClientHandler CreateHttpClientHandler()
    {
        #if DEBUG
        return new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (_, _, _, _) => true,
        };
        #else
        return new HttpClientHandler();
        #endif
    }
}