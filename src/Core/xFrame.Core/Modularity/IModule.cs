using System;
using System.Collections.Generic;
using System.Text;

namespace xFrame.Core.Modularity
{
    public interface IModule
    {
        string Name { get; }
        string Description { get; }
        Version Version { get; }
        void Initialize(IServiceProvider services);
    }
}
