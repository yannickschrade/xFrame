using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Text;

namespace xFrame.Core.ExtensionMethodes
{
    public static class NotifyPropertyChangedExtnesions
    {
        public static void SubscribePropertyChanged(this INotifyPropertyChanged @class, string propertyName, Action<object> action)
        {

            @class.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == propertyName)
                {
                    var val = @class.GetType().GetProperty(propertyName).GetValue(s);
                    action(val);
                }
            };
        }

        public static void SubscribePropertyChanging(this INotifyPropertyChanging @class, string propertyName, Action<object> action)
        {
            @class.PropertyChanging += (s, e) =>
            {
                if (e.PropertyName == propertyName)
                {
                    var val = @class.GetType().GetProperty(propertyName).GetValue(s);
                    action(val);
                }
            };
        }
    }
}
