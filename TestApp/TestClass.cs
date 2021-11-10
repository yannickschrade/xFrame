using CSU.Core.MVVM;
using CSU.Core.PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
