using System;
using System.Windows.Input;

namespace CESI_WPF_2023.Core
{
    public class RelayCommand : ICommand
    {
        private Action<object> _execute;

        private Func<object, bool> _canExecute;

        public Action<object> AfterExecuteAction { get; set; }

        public event EventHandler CanExecuteChanged;

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public RelayCommand(Action execute)
            : this((Action<object>)delegate
            {
                execute();
            }, (Func<object, bool>)null)
        {
        }

        public RelayCommand(Action execute, Func<bool> canExecute)
            : this(delegate
            {
                execute();
            }, (object o) => canExecute())
        {
        }

        public static RelayCommand Create(Action execute, Func<bool> canExecute = null)
        {
            return new RelayCommand(delegate
            {
                execute();
            }, (canExecute == null) ? null : ((Func<object, bool>)((object o) => canExecute())));
        }

        public static RelayCommand Create<T>(Action<T> execute, Func<T, bool> canExecute = null)
        {
            return new RelayCommand(delegate (object o)
            {
                execute((T)o);
            }, (canExecute == null) ? null : ((Func<object, bool>)((object o) => canExecute((T)o))));
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            if (this.CanExecuteChanged != null)
            {
                this.CanExecuteChanged(this, EventArgs.Empty);
            }
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
            AfterExecute(parameter);
        }

        private void AfterExecute(object parameter)
        {
            AfterExecuteAction?.Invoke(parameter);
        }
    }

    public class RelayCommand<T> : RelayCommand
    {
        /// <summary>
        /// Creates a new command that can always execute.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        public RelayCommand(Action<T> execute, Func<T, bool> canExecute = null)
            : base(o => execute((T)o), o => canExecute == null ? true : canExecute((T)o))
        {
        }
    }
}
