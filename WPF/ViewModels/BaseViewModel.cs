using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using WPF.ViewModels.Commands;
using WPF.Views.Datas;

namespace WPF.ViewModels
{
    /// <summary>
    /// ビューモデルの共通処理クラス
    /// </summary>
    public abstract class BaseViewModel : INotifyPropertyChanged,INotifyDataErrorInfo
    {
        private bool callShowWindow;

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        /// <summary>
        /// フォーム表示コマンド
        /// </summary>
        public DelegateCommand ShowWindowCommand { get; set; }
        /// <summary>
        /// 表示するウィンドウデータ
        /// </summary>
        public ShowWindowData ShowWindow { get; set; }
        /// <summary>
        /// ウィンドウを表示するタイミングを管轄
        /// </summary>
        public bool CallShowWindow
        {
            get => callShowWindow;
            set
            {
                if(callShowWindow==value)
                {
                    return;
                }
                callShowWindow = value;
                CallPropertyChanged();
                callShowWindow = false;
            }
        }
        /// <summary>
        /// エラーを所持しているかを返す
        /// </summary>
        public bool HasErrors
        {
            get
            {
                return CurrentErrors.Count > 0;
            }
        }
        /// <summary>
        /// プロパティ変更通知イベントを呼び出します
        /// </summary>
        protected void CallPropertyChanged()
        {
            StackFrame caller = new StackFrame(1);
            string[] methodNames = caller.GetMethod().Name.Split('_');
            int i = methodNames.Length - 1;
            string propertyName = methodNames[i];

            CallPropertyChanged(propertyName);
        }
        /// <summary>
        /// 引数のプロパティの変更を通知します
        /// </summary>
        /// <param name="propertyName">プロパティ名</param>
        protected void CallPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        /// <summary>
        /// ウィンドウ表示コマンドを設定します
        /// </summary>
        /// <param name="window"></param>
        protected void CreateShowWindowCommand(Window window)
        {
            ShowWindowCommand = new DelegateCommand(() =>SetShowWindow(window) , () => true);
            CallPropertyChanged(nameof(ShowWindowCommand));
            CallShowWindow = true;
        }
        /// <summary>
        /// 表示するウィンドウを設定します
        /// </summary>
        /// <param name="window"></param>
        private void SetShowWindow(Window window)
        {
            ShowWindow = new ShowWindowData()
            {
                WindowData = window
            };

            CallPropertyChanged(nameof(ShowWindow));
        }
        /// <summary>
        /// プロパティを検証し、エラーがあればリストに追加します
        /// </summary>
        /// <param name="propertyName">検証するプロパティ名</param>
        /// <param name="value">プロパティの値</param>
        public abstract void ValidationProperty(string propertyName, object value);
        /// <summary>
        /// 保持しているエラーを返します
        /// </summary>
        /// <param name="propertyName">取得するエラーのプロパティ名</param>
        /// <returns></returns>
        public IEnumerable GetErrors(string propertyName)
        {
            if (!CurrentErrors.ContainsKey(propertyName))
                return null;

            return CurrentErrors[propertyName];
        }
        /// <summary>
        /// エラーリスト
        /// </summary>
        public Dictionary<string, string> CurrentErrors=new Dictionary<string, string>();
        /// <summary>
        /// エラーを追加します
        /// </summary>
        /// <param name="propertyName">追加するエラーのプロパティ名</param>
        /// <param name="error">エラー内容</param>
        protected void AddError(string propertyName,string error)
        {
            if (!CurrentErrors.ContainsKey(propertyName))
                CurrentErrors.Add(propertyName, error);

            OnErrorsChanged();
        }
        /// <summary>
        /// エラーを削除します
        /// </summary>
        /// <param name="propertyName">削除するエラーのプロパティ名</param>
        protected void RemoveError(string propertyName)
        {
            if (CurrentErrors.ContainsKey(propertyName))
                CurrentErrors.Remove(propertyName);

            OnErrorsChanged();
        }
        /// <summary>
        /// エラーリストに要素が追加、削除されたことを通知するイベントを呼び出します
        /// </summary>
        /// <param name="proeprtyName">変化した要素のプロパティ名</param>
        protected void OnErrorsChanged(string proeprtyName)
        {
            this.ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(proeprtyName));
        }
        /// <summary>
        /// エラーリストに要素が追加、削除されたことを通知するイベントを呼び出します
        /// </summary>
        protected void OnErrorsChanged()
        {
            StackFrame coller = new StackFrame(1);
            string[] methodNames = coller.GetMethod().Name.Split('_');
            int i = methodNames.Length - 1;
            string propertyName = methodNames[i];

            OnErrorsChanged(propertyName);
        }
        /// <summary>
        /// エラーリストを操作します
        /// </summary>
        /// <param name="hasError">プロパティがエラーかの判断</param>
        /// <param name="propertyName">リストに追加、削除するプロパティ名</param>
        /// <param name="exeption">エラー内容</param>
        protected void ErrorsListOperation(bool hasError, string propertyName,string exeption)
        {
            if(hasError)
            {
                AddError(propertyName, exeption);
            }
            else
            {
                RemoveError(propertyName);
            }
        }
        /// <summary>
        /// エラーをクリアします
        /// </summary>
        protected void ErrorsClear()
        {
            CurrentErrors.Clear();
        }
    }
}
