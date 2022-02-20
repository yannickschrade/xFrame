using System;
using System.Collections.Generic;
using System.Text;

namespace xFrame.Core.ViewInjection
{
    public interface INavigationService
    {
        void NavigateTo(Type viewModelType);
    }
}
