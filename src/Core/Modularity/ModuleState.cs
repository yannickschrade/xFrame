namespace xFrame.Core.Modularity
{
    public enum ModuleState
    {
        NotLoaded,
        Loading,
        Loaded,
        RegisteringTypes,
        Initializing,
        Initialized,
    }
}