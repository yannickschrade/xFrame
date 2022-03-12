using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using xFrame.Core.MVVM;
using xFrame.WPF.Controls;
using xFrame.WPF.Extensions;
using xFrame.WPF.Modularity;
using xFrame.WPF.Samples.Module1;
using xFrame.WPF.Samples.ViewModels;
using xFrame.WPF.Theming;

namespace xFrame.WPF.Samples
{
    public class Programm
    {
        public static async Task Main()
        {
            await Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
                .ConfigureXFrame(x =>
                {
                    x.UseShellViewModel<MainViewModel>();
                })
                .ConfigureModules(x =>
                {
                    x.AddModule<Module>();
                })
                .UseXFrameLifetime()
                .UseColorTheme(ThemeType.SystemDefault)
                .UseFluentControls()
                .Build()
                .RunAsync();
        }
    }
}
