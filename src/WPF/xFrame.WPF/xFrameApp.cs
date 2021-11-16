using xFrame.Core.IoC;
using xFrame.Core.MVVM;

namespace xFrame.WPF
{
    public abstract class XFrameApp<T>: BaseApplication<T>
        where T : ViewModelBase
    {

        protected override ITypeService CreateTypeService()
        {
            return new DryIocContainerWrapper();
        }
    }
}