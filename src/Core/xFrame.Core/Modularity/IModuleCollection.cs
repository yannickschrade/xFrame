using System.Collections.Generic;
using System.Reflection;

namespace xFrame.Core.Modularity
{
    public interface IModuleCollection : IList<ModuleDescription>, ICollection<ModuleDescription>, IEnumerable<ModuleDescription>
    {
        void AddFromCollection(IModuleCollection moduleCollection);
    }
}