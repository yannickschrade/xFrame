using Microsoft.Extensions.Configuration;

namespace xFrame.WPF.Hosting.Internal
{
    internal class TrackingChainedConfigurationSource : IConfigurationSource
    {
        private readonly ChainedConfigurationSource _chainedConfigurationSource = new ChainedConfigurationSource();

        public TrackingChainedConfigurationSource(ConfigurationManager configManager)
        {
            _chainedConfigurationSource.Configuration = configManager;
        }

        public IConfigurationProvider? BuiltProvider { get; set; }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            BuiltProvider = _chainedConfigurationSource.Build(builder);
            return BuiltProvider;
        }
    }
}
