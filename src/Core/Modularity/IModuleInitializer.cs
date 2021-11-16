namespace xFrame.Core.Modularity
{
    public interface IModuleInitializer
    {
        void InitializeModule(IModuleInfo moduleInfo);
        bool CanInitializeModule(IModuleInfo moduleInfo);
    }
}