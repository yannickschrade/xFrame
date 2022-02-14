using xFrame.Core.Attributes;
using xFrame.Core.MVVM;

namespace WPFTestApp
{
    public partial class ViewModel : ViewModelBase
    {
        [Generateproperty]
        private string _test;

        public ViewModel()
        {
        }
    }
}
