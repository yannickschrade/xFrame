using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using xFrame.Modularity.Abstraction;

namespace xFrame.WPF.Modularity
{
    internal class ModuleCollection : IModuleCollection
    {
        private List<ModuleDescription> _modules = new List<ModuleDescription>();

        public ModuleDescription this[int index] 
        {
            get => _modules[index];
            set => _modules[index] = value;
        }

        public int Count => _modules.Count;
        public bool IsReadOnly => false;

        public void Add(ModuleDescription item)
        {
            _modules.Add(item);
        }

        public void AddFromCollection(IModuleCollection moduleCollection)
        {
            _modules.AddRange(moduleCollection);
        }

        public void Clear()
        {
            _modules.Clear();
        }

        public bool Contains(ModuleDescription item)
        {
            return _modules.Contains(item);
        }

        public void CopyTo(ModuleDescription[] array, int arrayIndex)
        {
            _modules.CopyTo(array, arrayIndex);
        }

        public IEnumerator<ModuleDescription> GetEnumerator()
        {
            return _modules.GetEnumerator();
        }

        public int IndexOf(ModuleDescription item)
        {
            return _modules.IndexOf(item);
        }

        public void Insert(int index, ModuleDescription item)
        {
            _modules.Insert(index, item);
        }

        public bool Remove(ModuleDescription item)
        {
            return _modules.Remove(item);
        }

        public void RemoveAt(int index)
        {
            _modules.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
