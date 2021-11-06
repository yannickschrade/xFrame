using CSU.Core.ExtensionMethodes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace CSU.Core.PropertyChanged
{
    public abstract class NotifyPropertyChanged : INotifyPropertyChanged, INotifyPropertyChanging
    {

        protected Dictionary<string, object?> _properties = new Dictionary<string, object?>();

        #region Set

        protected virtual SetterContext<T> Set<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if(propertyName is null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return new SetterContext<T>(propertyName, false, value, OnPropertyChanged);
            }

            OnPropertyChanging(propertyName);
            field = value;
            OnPropertyChanged(propertyName);
            return new SetterContext<T>(propertyName, true, value, OnPropertyChanged);
        }

        protected virtual SetterContext<T> Set<T>(T value, [CallerMemberName] string? propertyName = null)
        {

            if(propertyName is null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            if (!_properties.TryGetValue(propertyName, out var property))
            {
                OnPropertyChanging(propertyName);
                _properties.Add(propertyName, value);
                OnPropertyChanged(propertyName);
                return new SetterContext<T>(propertyName, true, value, OnPropertyChanged);
            }

            if (EqualityComparer<T>.Default.Equals(value, (T?)property))
            {
                return new SetterContext<T>(propertyName, false, value, OnPropertyChanged);
            }

            OnPropertyChanging(propertyName);
            _properties[propertyName] = value;
            OnPropertyChanged(propertyName);
            return new SetterContext<T>(propertyName, true, value, OnPropertyChanged);
        }

        #endregion

        #region Get

        protected virtual T? Get<T>([CallerMemberName] string propertyName = null)
        {
            if (propertyName is null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            if (!_properties.TryGetValue(propertyName, out var value))
            {
                _properties.Add(propertyName, default);
                return default!;
            }

            return (T?)value;
        }

        protected virtual T? Get<T>(T defaultValue, [CallerMemberName] string? propertyName = null)
        {
            if (propertyName is null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            if (!_properties.TryGetValue(propertyName, out var value))
            {
                _properties.Add(propertyName, defaultValue);
                return defaultValue;
            }

            return (T?)value;
        }

        #endregion

        #region event implementation

        public event PropertyChangingEventHandler? PropertyChanging;
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        protected virtual void OnPropertyChanging([CallerMemberName] string? PropertyName = null)
        {
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(PropertyName));
        }

        #endregion
    }
}
