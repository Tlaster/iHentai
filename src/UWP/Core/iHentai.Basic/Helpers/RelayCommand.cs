using System;
using System.Threading.Tasks;
using System.Windows.Input;
using PropertyChanged;

namespace iHentai.Basic.Helpers
{
    public interface IAsyncCommand : ICommand
    {
        bool IsRunning { get; }
        bool IsNotRunning { get; }
    }

    [AddINotifyPropertyChangedInterface]
    public class RelayAsyncCommand : IAsyncCommand
    {
        private readonly Func<bool> _canExecute;
        private readonly Func<Task> _execute;

        public RelayAsyncCommand(Func<Task> execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool IsRunning { get; private set; }
        
        [DependsOn(nameof(IsRunning))]
        public bool IsNotRunning => !IsRunning;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            if (IsRunning)
                return false;
            return _canExecute == null || _canExecute();
        }

        public async void Execute(object parameter)
        {
            if (IsRunning)
                return;
            IsRunning = true;
            OnCanExecuteChanged();
            try
            {
                await _execute.Invoke();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine(e.StackTrace);
            }
            IsRunning = false;
            OnCanExecuteChanged();
        }

        public void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Func<bool> _canExecute;
        private readonly Action _execute;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }

        public void Execute(object parameter)
        {
            _execute();
        }

        public void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public class RelayCommand<T> : ICommand
    {
        private readonly Func<T, bool> _canExecute;
        private readonly Action<T> _execute;

        public RelayCommand(Action<T> execute, Func<T, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute((T) parameter);
        }

        public void Execute(object parameter)
        {
            _execute((T) parameter);
        }

        public void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}