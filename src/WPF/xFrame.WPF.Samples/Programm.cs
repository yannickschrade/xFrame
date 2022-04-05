using xFrame.WPF.Controls;
using xFrame.WPF.Hosting;
using xFrame.WPF.Theming;
using xFrame.Core.Modularity;
using xFrame.WPF.Samples.ViewModels;

var builder = XFrameHost.CreateBuilder()
    .UseColorTheme(ThemeType.SystemDefault)
    .UseFluentControls()
    .UseSplashScreen<SplashViewModel>();

builder.Modules.AddFromFolder(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Modules\");

builder.AddResourceDictionary("Resources/Resources.xaml");

await builder.StartAppAsync();