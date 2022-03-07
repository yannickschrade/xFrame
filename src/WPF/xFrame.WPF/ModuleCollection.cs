using System.Collections;
using System.Collections.Generic;
using xFrame.Core.Modularity;

namespace xFrame.WPF
{
    internal class ModuleCollection : IModuleCollection
    {
        private readonly List<ModuleDescriptor> _moduleDescriptors = new List<ModuleDescriptor>();
        public ModuleDescriptor this[int index]
        {
            get => _moduleDescriptors[index];
            set => _moduleDescriptors[index] = value;
        }

        public int Count => _moduleDescriptors.Count;
        public bool IsReadOnly => false;

        public void Add(ModuleDescriptor item)
        {
            _moduleDescriptors.Add(item);
        }

        public void Clear()
        {
            _moduleDescriptors.Clear();
        }

        public bool Contains(ModuleDescriptor item)
        {
            return _moduleDescriptors.Contains(item);
        }

        public void CopyTo(ModuleDescriptor[] array, int arrayIndex)
        {
            _moduleDescriptors.CopyTo(array, arrayIndex);
        }

        public IEnumerator<ModuleDescriptor> GetEnumerator()
        {
            return _moduleDescriptors.GetEnumerator();
        }

        public int IndexOf(ModuleDescriptor item)
        {
            return _moduleDescriptors.IndexOf(item);
        }

        public void Insert(int index, ModuleDescriptor item)
        {
            _moduleDescriptors.Insert(index, item);
        }

        public bool Remove(ModuleDescriptor item)
        {
            return _moduleDescriptors.Remove(item);
        }

        public void RemoveAt(int index)
        {
            _moduleDescriptors.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}