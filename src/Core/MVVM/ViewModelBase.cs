using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace xFrame.Core.MVVM
{
    public abstract class ViewModelBase : INotifyPropertyChanged, INotifyPropertyChanging, INotifyDataErrorInfo
    {
        public virtual void OnViewStateChanged(bool IsActive) { }

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
            PropertyChanging?.Invoke(this,new PropertyChangingEventArgs(propertyName));
        }

        #endregion

    }
}