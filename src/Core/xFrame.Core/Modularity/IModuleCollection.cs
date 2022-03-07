using System.Collections;
using System.Collections.Generic;

namespace xFrame.Core.Modularity
{
    public interface IModuleCollection: IList<ModuleDescriptor>, ICollection<ModuleDescriptor>, IEnumerable<ModuleDescriptor>
    {
    }
}