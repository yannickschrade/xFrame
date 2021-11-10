using System.Windows.Input;

namespace CSU.Core.Commands;

public class RelayCommand : BaseCommand
{  

    public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null) : base(execute, canExecute)
    {
    }

    public override bool CanExecute(object? parameter)
    {
        return CanExecute != null ? CanExecute(parameter) : true;
    }

    public override void Execute(object? parameter)
    {
        if (CanExecute(parameter))
            ExecuteAction(parameter);
    }
}
