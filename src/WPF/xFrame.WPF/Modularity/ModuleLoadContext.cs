using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;

namespace xFrame.WPF.Modularity
{
    internal class ModuleLoadContext : AssemblyLoadContext
    {
        private readonly AssemblyDependencyResolver _resolver;

        public ModuleLoadContext(string pluginPath, string name) : base(name)
            => _resolver = new AssemblyDependencyResolver(pluginPath);

        public string ResolveAssemblyPath(AssemblyName assemblyName)
            => _resolver.ResolveAssemblyToPath(assemblyName);

        /// <inheritdoc />
        protected override Assembly Load(AssemblyName assemblyName)
        {
            // Try to get the assembly from the AssemblyLoadContext.Default, when it is already loaded
            if (TryGetAssembly(assemblyName, out var alreadyLoadedAssembly))
            {
                return alreadyLoadedAssembly;
            }
            var assemblyPath = ResolveAssemblyPath(assemblyName);
            if (assemblyPath == null)
            {
                return null;
            }

            var resultAssembly = LoadFromAssemblyPath(assemblyPath);
            return resultAssembly;
        }


        private bool TryGetAssembly(AssemblyName assemblyName, out Assembly alreadyLoadedAssembly)
        {
            foreach (var assembly in Assemblies)
            {
                var name = assembly.GetName().Name;
                if (name == null)
                {
                    continue;
                }
                if (!name.Equals(assemblyName.Name))
                {
                    continue;
                }
                alreadyLoadedAssembly = assembly;
                return true;
            }
            alreadyLoadedAssembly = null;
            return false;
        }

        /// <inheritdoc />
        protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
        {
            var libraryPath = _resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
            if (libraryPath == null)
            {
                return IntPtr.Zero;
            }

            return LoadUnmanagedDllFromPath(libraryPath);
        }
    }
}
