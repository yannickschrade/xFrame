using xFrame.Core.MVVM;

namespace TestApp
{
    internal class TestClass : ViewModelBase
    {
        public int Value
        {
            get => Get<int>();
            set => Set(value);
        }
    }
}
