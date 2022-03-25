using System.Threading.Tasks;
using xFrame.WPF.Controls;
using xFrame.WPF.Hosting;
using xFrame.WPF.Theming;

namespace xFrame.WPF.Samples
{
    public class Programm
    {
        public static async Task Main()
        {
            var builder = XFrameHost.CreateBuilder();
            builder.UseColorTheme(ThemeType.SystemDefault);
            builder.UseFluentControls();
            
            await builder.Build()
                .StartAsync();
        }
    }
}
