using System;
using System.Windows.Input;

namespace ThePreaching.Base
{
    public class DelegateCommand : ICommand
    {
        private Action _executeAction;
        public DelegateCommand(Action funktion)
        {
            _executeAction = funktion;
        }
        public virtual bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _executeAction?.Invoke();
        }

        public event EventHandler CanExecuteChanged;
    }
    public class DelegateCommand<T> : ICommand
    {
        private Action<T> _executeAction;
        public DelegateCommand(Action<T> funktion)
        {
            _executeAction = funktion;
        }
        public virtual bool CanExecute(T parameter)
        {
            return true;
        }

        public virtual void Execute(T parameter)
        {
            _executeAction?.Invoke(parameter);
        }

        public bool CanExecute(object parameter)
        {
            if (parameter is T)
                return CanExecute((T)parameter);
            return true;
        }

        public void Execute(object parameter)
        {
            if (parameter is T)
                Execute((T)parameter);
        }

        public event EventHandler CanExecuteChanged;
    }
}