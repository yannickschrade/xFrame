using System;
namespace xFrame.Core.ViewService
{
    public interface IViewFor
    {
        Type ViewModelType { get; }
        object DataContext { get; set; }
    }

    public interface IViewFor<T> : IViewFor
    {
        Type IViewFor.ViewModelType => typeof(T);
    }
}
