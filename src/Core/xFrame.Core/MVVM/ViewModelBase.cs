using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using xFrame.Core.Validation;

namespace xFrame.Core.MVVM
{

    public abstract class ViewModelBase : IViewModel
    {

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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

        
        public virtual void OnViewStateChanged(bool IsActive) { }

        public void OnValidated(ValidationResult result)
        {
            if (result.IsValid)
            {
                ClearErrors(result.ValidatedProperty);
                return;
            }

            foreach (var message in result.Messages.Where(m => m.Severity == Severity.Error))
            {
                AddError(message, result.ValidatedProperty);
            }
        }

        public virtual void OnLoaded()
        {
            SetupValidation();
        }

        public abstract void SetupValidation();
    }
}