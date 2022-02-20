using xFrame.Core.IoC;
using xFrame.Core.MVVM;

namespace xFrame.WPF
{
    public abstract class App: BaseApplication
    {

        protected override ITypeService CreateTypeService()
        {
            return new DryIocContainerWrapper();
        }
    }
}