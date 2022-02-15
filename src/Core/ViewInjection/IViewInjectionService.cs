using System;
using System.Runtime.Versioning;
using xFrame.Core.MVVM;

namespace xFrame.Core.ViewInjection
{
    public interface IViewInjectionService
    {
        void Inject(Type viewModelType, object key);

        void Inject<T>(object key) 
            where T : IViewModel;

        void Inject(IViewModel vm, object key);

        void Remove(IViewModel vm, object key);


        void AttachContainer(object view, object key);
    }
}
