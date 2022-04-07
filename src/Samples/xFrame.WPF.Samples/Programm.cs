using xFrame.WPF.Controls;
using xFrame.WPF.Hosting;
using xFrame.WPF.Theming;
using xFrame.Core.Modularity;
using xFrame.WPF.Samples.ViewModels;
using xFrame.WPF.Samples.Module1;

var builder = XFrameHost.CreateBuilder()
    .UseColorTheme(ThemeType.SystemDefault)
    .UseFluentControls()
    .UseSplashScreen<SplashViewModel>();

builder.Modules.AddModule<TestModule>();

builder.AddResourceDictionary("Resources/Resources.xaml");

await builder.StartAppAsync();