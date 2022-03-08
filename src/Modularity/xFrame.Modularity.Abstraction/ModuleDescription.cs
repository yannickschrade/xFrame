using System;
using System.Text.Json.Serialization;

namespace xFrame.Modularity.Abstraction
{
    public class ModuleDescription
    {
        public IModule Instance { get; set; }
        public string Name { get; }
        public string Location { get; }
        public bool LoadModule { get; set; }

        [JsonIgnore]
        public bool IsRegistered { get; set; }


        public ModuleDescription(Type type)
        {
            Location = type.Assembly.Location;
            Name = type.Name;
            LoadModule = true;
        }

        public ModuleDescription(string name, string location)
        {
            Name = name;
            Location = location;
            LoadModule = true;
        }

        public ModuleDescription(string name, string location, bool loadModule)
        {
            Name = name;
            Location = location;
            LoadModule = loadModule;
        }
    }
}
