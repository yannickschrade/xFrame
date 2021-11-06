using CSU.Core.PropertyChanged;
using Moq;
using System;
using System.ComponentModel;
using Xunit;

namespace CSU.Tests.Core
{
    public class PropertyChangedTests : NotifyPropertyChanged
    {
        private int refVal;
        public int RefVal
        {
            get => refVal;
            set => Set(ref refVal, value);
        }

        public int IntValue
        {
            get => Get<int>();
            set => Set(value);
        }

        public int? NullableInt
        {
            get => Get<int?>();
            set => Set(value);
        }

        public int WithDefault
        {
            get => Get(5);
            set => Set(value);
        }

        [Fact]
        public void PropertyChangedEventShouldRiseIfPropertyChangesFirstTime()
        {
            Assert.PropertyChanged(this, nameof(IntValue), () => IntValue = 5);
        }


        [Fact]
        public void PropertyChangedEventShouldRiseIfPropertyChanges()
        {
            IntValue = 3;
            Assert.PropertyChanged(this, nameof(IntValue), () => IntValue = 5);
            Assert.PropertyChanged(this, nameof(RefVal), () => RefVal = 5);
        }

        [Fact]
        public void RefValueShouldBeSetBySetMethode()
        {
            RefVal = 5;
            Assert.Equal(5, refVal);
        }

        [Fact]
        public void PropertyChangedEventShouldNotBeRisedIfPropertyIsEqual()
        {
            var intIsUpdated = false;
            var refIsUpdated = false;
            IntValue = 5;
            RefVal= 5;
            PropertyChanged += (s,e) => intIsUpdated = true;
            PropertyChanged += (s,e) => refIsUpdated = true;
            IntValue = 5;
            RefVal = 5;

            Assert.False(intIsUpdated);
            Assert.False(refIsUpdated);
        }


        [Fact]
        public void NullValueShouldNotBeAProblem()
        {
            NullableInt = 5;
            var val = NullableInt;
            NullableInt = null;
            val = NullableInt;
        }

        [Fact]
        public void GetShouldReturnDefaultValueIfUnset()
        {
            Assert.Equal(default, IntValue);
            Assert.Equal(default, NullableInt);
            Assert.Equal(5, WithDefault);
        }
        

        [Fact]
        public void PropertyNameIsNullThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => Set(5, null));
            Assert.Throws<ArgumentNullException>(() => Get<int>(propertyName: null));
        }
        
        
        private class MockPropertyChanged : NotifyPropertyChanged
        {

          

        }
    }
}
