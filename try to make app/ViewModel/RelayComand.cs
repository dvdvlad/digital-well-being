using System;
using System.Windows.Input;

namespace try_to_make_app.ViewModel;

public class RelayComand: ICommand
{
   
    Action<object?> execute;
    Func<object?, bool>? canExecute;
 
    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }
 
    public RelayComand(Action<object?> execute, Func<object?, bool>? canExecute = null)
    {
        this.execute = execute;
        this.canExecute = canExecute;
    }
 
    public bool CanExecute(object? parameter)
    {
        return canExecute == null || canExecute(parameter);
    }
 
    public void Execute(object? parameter)
    {
        execute(parameter);
    } 
}