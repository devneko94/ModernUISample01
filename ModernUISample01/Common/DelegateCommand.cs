using System;
using System.Windows.Input;

namespace ModernUISample01.Common
{
    /// <summary>
    /// デリゲートコマンドクラス
    /// </summary>
    public class DelegateCommand : ICommand
    {
        #region フィールド
        private Action _execute;
        private Func<bool> _canExecute;
        #endregion

        #region イベント
        public event EventHandler CanExecuteChanged;
        #endregion

        #region コンストラクタ
        public DelegateCommand(Action action) : this(action, () => true) { }

        public DelegateCommand(Action action, Func<bool> func)
        {
            this._execute = action;
            this._canExecute = func;
        }
        #endregion

        #region パブリックメソッド
        public bool CanExecute(object parameter)
        {
            return this._canExecute();
        }

        public void Execute(object parameter)
        {
            this._execute();
        }

        public void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }

    /// <summary>
    /// デリゲートコマンドクラス（ジェネリック）
    /// </summary>
    public class DelegateCommand<T> : ICommand
    {
        #region フィールド
        private Action<T> _execute;
        private Func<T, bool> _canExecute;
        #endregion

        #region イベント
        public event EventHandler CanExecuteChanged;
        #endregion

        #region コンストラクタ
        public DelegateCommand(Action<T> action) : this(action, (T) => true) { }

        public DelegateCommand(Action<T> action, Func<T, bool> func)
        {
            this._execute = action;
            this._canExecute = func;
        }
        #endregion

        #region パブリックメソッド
        public bool CanExecute(object parameter)
        {
            return this._canExecute((T)parameter);
        }

        public void Execute(object parameter)
        {
            this._execute((T)parameter);
        }

        public void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }
}
