using CSU.Core.ExtensionMethodes;
using CSU.Core.PropertyChanged;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CSU.Tests.Core
{
    public class SetterContextTests : NotifyPropertyChanged
    {
        public int NotifyedWithIntValue { get; set; }

        public int IntValue
        {
            get => Get<int>();
            set => Set(value)
                .Notify(nameof(NotifyedWithIntValue));
        }


        [Fact]
        public void SetterContextHasChangedShouldOnlyBeSettedIfPropertyChanged()
        {
            var context = Set(5, nameof(IntValue));
            Assert.True(context.HasChanged);
            context = Set(5, nameof(IntValue));
            Assert.False(context.HasChanged);
        }

        [Fact]
        public void NotifyShouldRisePropertyChangedForPropertyName()
        {
            Assert.PropertyChanged(this, nameof(NotifyedWithIntValue), () => IntValue = 5);
        }

    }
}
