namespace xFrame.Core.Modularity;

public interface ISortingStage
{
    IInitialisationStage UseModuleInitializer(IModuleInitializer moduleInitializer);
}
