using System.Collections;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using xFrame.Core.PropertyChanged;

namespace xFrame.Core.MVVM;

public abstract class ViewModelBase : NotifyPropertyChanged, INotifyDataErrorInfo
{

    private readonly ConcurrentDictionary<string, List<string>> _errorsByPropertyName = new();

    public bool HasErrors => _errorsByPropertyName.Any();

    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    public IEnumerable GetErrors(string? propertyName)
    {
        ArgumentNullException.ThrowIfNull(propertyName, nameof(propertyName));
        _errorsByPropertyName.TryGetValue(propertyName, out var errors);
        return errors;
    }

    protected virtual void OnErrorsChanged([CallerMemberName] string? propertyName = null)
    {
        if (propertyName == null)
        {
            throw new ArgumentNullException(nameof(propertyName));
        }

        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
    }


    protected virtual SetterContext<T> SetAndValidateWithAnnotations<T>(T value, [CallerMemberName] string? propertyName = null)
    {
        ArgumentNullException.ThrowIfNull(propertyName, nameof(propertyName));

        return Set(value, propertyName);
    }

    protected virtual SetterContext<T> SetAndValidateWithFunction<T>(T value, Func<T, List<string>, bool> validationFunction, [CallerMemberName] string? propertyName = null)
    {
        ArgumentNullException.ThrowIfNull(propertyName, nameof(propertyName));
        var results = new List<string>();
        var isValid = validationFunction(value, results);

        if (!isValid)
        {
            AddErrors(results, propertyName);
        }
        else
        {
            ClearErrors(propertyName);
        }

        return Set(value, propertyName);
    }


    protected virtual SetterContext<T> SetAndValidateWithAnnotations<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        ArgumentNullException.ThrowIfNull(propertyName, nameof(propertyName));
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateProperty(value, new ValidationContext(this) { MemberName = propertyName }, results);

        if (!isValid)
        {
            AddErrors(results.Select(r => r.ErrorMessage!), propertyName);
        }
        else
        {
            ClearErrors(propertyName);
        }

        return Set(ref field, value, propertyName);
    }

    protected virtual SetterContext<T> SetAndValidateWithFunction<T>(ref T field, T value, Func<T, List<string>, bool> validationFunction, [CallerMemberName] string? propertyName = null)
    {
        ArgumentNullException.ThrowIfNull(propertyName, nameof(propertyName));
        var results = new List<string>();
        var isValid = validationFunction(value, results);

        if (!isValid)
        {
            AddErrors(results, propertyName);
        }
        else
        {
            ClearErrors(propertyName);
        }

        return Set(ref field, value, propertyName);
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
            OnErrorsChanged(propertyName);
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
            OnErrorsChanged(propertyName);
        }
    }
}
