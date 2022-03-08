using System.Collections.Generic;

namespace xFrame.Modularity.Abstraction
{
    public interface IModuleCollection : IList<ModuleDescription>, ICollection<ModuleDescription>, IEnumerable<ModuleDescription>
    {
        void AddFromCollection(IModuleCollection moduleCollection);
    }
}