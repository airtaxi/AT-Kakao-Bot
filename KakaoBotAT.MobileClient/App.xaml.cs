using Microsoft.Extensions.DependencyInjection;

namespace KakaoBotAT.MobileClient;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var mainPage = this.Handler?.MauiContext?.Services.GetRequiredService<MainPage>();
        return new Window(mainPage!);
    }
}