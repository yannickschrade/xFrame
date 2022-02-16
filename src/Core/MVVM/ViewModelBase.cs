using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using xFrame.Core.ExtensionMethodes;
using xFrame.Core.Fluent;

namespace xFrame.Core.MVVM
{
    public interface IViewModel : INotifyPropertyChanged, INotifyPropertyChanging, INotifyDataErrorInfo
    {
        void OnViewStateChanged(bool IsActive);

    }

    public abstract class ViewModelBase<T> : IViewModel
        where T : ViewModelBase<T>, new()
    {

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region INotifyPropertyChanging

        public event PropertyChangingEventHandler PropertyChanging;

        protected virtual void NotifyPropertyChanging([CallerMemberName] string propertyName = null)
        {
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
        }
        #endregion

        #region INotifyDataErrorInfo

        private readonly ConcurrentDictionary<string, List<string>> _errorsByPropertyName = new ConcurrentDictionary<string, List<string>>();

        public bool HasErrors => _errorsByPropertyName.Any();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            if (propertyName == null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            _errorsByPropertyName.TryGetValue(propertyName, out var errors);
            return errors;
        }

        protected virtual void AddError(string error, string propertyName)
        {
            if (!_errorsByPropertyName.ContainsKey(propertyName))
            {
                _errorsByPropertyName[propertyName] = new List<string>();
            }

            if (!_errorsByPropertyName[propertyName].Contains(error))
            {
                _errorsByPropertyName[propertyName].Add(error);
                NotifyErrosChanged(propertyName);
            }
        }

        protected virtual void AddErrors(IEnumerable<string> errors, string propertyName)
        {
            foreach (var error in errors)
            {
                AddError(error, propertyName);
            }
        }

        protected virtual void ClearErrors(string propertyName)
        {
            if (_errorsByPropertyName.ContainsKey(propertyName))
            {
                _errorsByPropertyName.TryRemove(propertyName, out _);
                NotifyErrosChanged(propertyName);
            }
        }

        protected virtual void NotifyErrosChanged([CallerMemberName] string propertyName = null)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        #endregion

        
        public IPropertyContext<T,TProperty> When<TProperty>(Expression<Func<T,TProperty>> expression)
        {
            var propName = expression.GetPropertyInfo();
            return new PropertyContext<T, TProperty>(propName, (T)this);
        }

        public virtual void OnViewStateChanged(bool IsActive) { }
    }
}