using xFrame.WPF.Hosting;

namespace xFrame.WPF.Controls
{
    public static class HostExtensions
    {
        public static XFrameHostBuilder UseFluentControls(this XFrameHostBuilder builder)
        {
            builder.ConfigureResources(r => r.MergedDictionaries.Add(new FluentControls()));
            return builder;
        }
    }
}
