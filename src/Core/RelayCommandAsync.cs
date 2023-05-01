using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CESI_WPF_2023.Core
{
    public class RelayCommandAsync : ICommand
    {
        private readonly Func<object, Task> _execute;
        private readonly Func<object, bool> _canExecute;
        private readonly Func<object, bool> _canExecuteGlobal;
        private bool _suspendedExecute;

        #region Constructor

        public RelayCommandAsync(Func<object, Task> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
            if (_canExecute == null)
            {
                _canExecuteGlobal = o => !_suspendedExecute;
            }
            else
            {
                _canExecuteGlobal = o => !_suspendedExecute && _canExecute(o);
            }
        }

        public RelayCommandAsync(Func<Task> execute)
            : this(o => execute())
        {
        }

        public RelayCommandAsync(Func<Task> execute, Func<bool> canExecute)
            : this(o => execute(), o => canExecute())
        {
        }

        #endregion

        public bool CanExecute(object parameter)
        {
            return _canExecuteGlobal(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler CanExecuteChanged;

        private void Suspend(bool isSuspended)
        {
            _suspendedExecute = isSuspended;
            RaiseCanExecuteChanged();
        }

        public void Execute(object parameter)
        {
            ExecuteAsync(parameter);//.ExecuteWithoutAwait();
        }

        public async Task ExecuteAsync(object parameter)
        {
            Suspend(true);
            try
            {
                await _execute(parameter);
            }
            finally
            {
                Suspend(false);
                AfterExecuteAction?.Invoke(parameter);
            }
        }

        public Action<object> AfterExecuteAction { get; set; }

        #region Static

        public static RelayCommandAsync Create(Func<Task> execute, Func<bool> canExecute = null)
        {
            return new RelayCommandAsync(o => execute(), canExecute == null ? (Func<object, bool>)null : o => canExecute());
        }

        public static RelayCommandAsync Create<T>(Func<T, Task> execute, Func<T, bool> canExecute = null)
        {
            return new RelayCommandAsync(o => execute((T)o), canExecute == null ? (Func<object, bool>)null : o => canExecute((T)o));
        }

        #endregion
    }

    public class RelayCommandAsync<T> : RelayCommandAsync
    {
        public RelayCommandAsync(Func<T, Task> execute, Func<T, bool> canExecute = null)
            : base(o => execute((T)o), canExecute == null ? (Func<object, bool>)null : o => canExecute((T)o))
        {
        }
    }
}
