using System.Threading.Tasks;
using xFrame.WPF.Controls;
using xFrame.WPF.Hosting;
using xFrame.WPF.Theming;
using xFrame.Core.Modularity;
using System;

namespace xFrame.WPF.Samples
{
    public class Programm
    {
        public static async Task Main()
        {
            var builder = XFrameHost.CreateBuilder()
                .UseColorTheme(ThemeType.SystemDefault)
                .UseFluentControls();

            builder.Modules.AddFromFolder(Environment.GetFolderPath(Environment.SpecialFolder.Desktop)+ @"\Modules\");

            await builder.Build()
                .StartAsync();
        }
    }
}
