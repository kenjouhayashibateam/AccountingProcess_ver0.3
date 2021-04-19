using System;
using System.Windows.Input;

namespace WPF.ViewModels.Commands
{
    /// <summary>
    /// デリゲートコマンドクラス
    /// </summary>
    public class DelegateCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested += value; }
        }

        private readonly Action _Execute;
        private readonly Func<bool> _CanExecute;

        public DelegateCommand(Action execute, Func<bool> canExecute)
        {
            _Execute = execute ??
                throw new ArgumentNullException(nameof(DelegateCommand) + ":" + nameof(execute));
            _CanExecute = canExecute ??
                throw new ArgumentNullException(nameof(DelegateCommand) + ":" + nameof(canExecute));
        }

        public bool CanExecute(object parameter) => _CanExecute();

        public bool CanExecute() => _CanExecute();

        public void Execute(object parameter) => _Execute();

        public void Execute() => _Execute();

        public void RaiseCanExecuteChanged() => CommandManager.InvalidateRequerySuggested();
    }

    /// <summary>
    /// デリゲートコマンドクラス(ジェネリック）
    /// </summary>
    public class DelegateCommand<T> : ICommand
    {
        // 警告：CS0067を非表示にする
#pragma warning disable 0067

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested += value; }
        }

        private readonly Action<T> _Execute;
        private readonly Func<T, bool> _CanExecute;

        public DelegateCommand(Action<T> execute, Func<T, bool> canExecute)
        {
            _Execute = execute ??
                throw new ArgumentNullException(nameof(DelegateCommand) + ":" + nameof(execute));
            _CanExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _CanExecute((T)parameter);

        public void Execute(object parameter) => _Execute((T)parameter);

        public void RaiseCanExecuteChanged() => CommandManager.InvalidateRequerySuggested();
    }
}
