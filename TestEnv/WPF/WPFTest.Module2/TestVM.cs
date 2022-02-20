using xFrame.Core.Attributes;
using xFrame.Core.MVVM;

namespace WPFTest.Module2
{
    public partial class TestVM : ViewModelBase<TestVM>
    {
        [Generateproperty]
        private string _testProp = "Hello from module 2";
    }
}
