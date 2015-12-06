using System;
using System.Windows.Input;

namespace ThePreaching.Base
{
    public class DelegateCommand : ICommand
    {
        private Action<object> _executeAction;
        public DelegateCommand(Action<object> funktion)
        {
            _executeAction = funktion;
        }
        public virtual bool CanExecute(object parameter)
        {
            return true;
        }

        public virtual void Execute(object parameter)
        {
            _executeAction.Invoke(parameter);
        }

        public event EventHandler CanExecuteChanged;
    }
}