using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using xFrame.Core.MVVM;
using xFrame.WPF.Extensions;
using xFrame.WPF.Samples.ViewModels;

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
                .UseXFrameLifetime()
                .RunConsoleAsync();
        }
    }
}
