namespace xFrame.Core.Modularity
{
    public interface ISortingStage
    {
        IModuleManager UseModuleInitializer(IModuleInitializer moduleInitializer);
        IModuleManager UseModuleInitializer<T>()
            where T : IModuleInitializer;
    }
}