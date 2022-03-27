using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using xFrame.Core.Modularity;

namespace xFrame.WPF.Samples.Module1
{
    public class TestModule : IModule
    {
        private readonly ILogger _logger;

        public string Name => "TestModule";
        public string Description => "Module for Test propouse";
        public Version Version { get; } = new Version(0, 1);


        public TestModule(ILogger<TestModule> logger)
        {
            _logger = logger;
        }

        public void Initialize(IServiceProvider services)
        {
            _logger.LogInformation("Module Initialized");
        }
    }
}