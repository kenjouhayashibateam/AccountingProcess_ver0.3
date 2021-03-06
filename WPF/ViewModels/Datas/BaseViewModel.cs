﻿using Domain.Entities;
using Domain.Entities.ValueObjects;
using Domain.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using WPF.ViewModels.Commands;
using WPF.Views.Datas;

namespace WPF.ViewModels.Datas
{
    /// <summary>
    /// ビューモデルの共通処理クラス
    /// </summary>
    public abstract class BaseViewModel : NotifyPropertyChanged, INotifyDataErrorInfo, ILoginRepObserver
    {
        #region Properties
        private MessageBoxInfo messageBox;
        private DelegateCommand<Window> windowCloseCommand;
        private string windowTitle;
        private bool isAdminPermisson;
        private bool callShowWindow;
        private bool callShowMessageBox;
        #endregion
        
        public IDataBaseConnect DataBaseConnect;

        /// <summary>
        /// 画面タイトル
        /// </summary>
        protected string DefaultWindowTitle { get; set; }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        /// <summary>
        /// ウィンドウ表示コマンド
        /// </summary>
        public DelegateCommand ShowWindowCommand { get; set; }
        /// <summary>
        /// 表示するウィンドウデータ
        /// </summary>
        public ShowWindowData ShowWindow { get; set; }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        protected BaseViewModel(IDataBaseConnect dataBaseConnect)
        {
            DataBaseConnect = dataBaseConnect;
            LoginRep loginRep = LoginRep.GetInstance();
            loginRep.Add(this);
            SetWindowDefaultTitle();
            WindowTitle = DefaultWindowTitle;
            if (loginRep.Rep == null) { IsAdminPermisson = false; }
            else
            {
                SetRep(loginRep.Rep);
                IsAdminPermisson = loginRep.Rep.IsAdminPermisson;
            }
        }
        /// <summary>
        /// 表示するメッセージボックスデータ
        /// </summary>
        public MessageBoxInfo MessageBox
        {
            get => messageBox;
            set
            {
                messageBox = value;
                CallShowMessageBox = true;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// メッセージボックス表示コマンド
        /// </summary>
        public DelegateCommand MessageBoxCommand { get; set; }
        /// <summary>
        /// ウィンドウを表示するタイミングを管轄
        /// </summary>
        public bool CallShowWindow
        {
            get => callShowWindow;
            set
            {
                if (callShowWindow == value) { return; }
                callShowWindow = value;
                CallPropertyChanged();
                callShowWindow = false;
            }
        }
        /// <summary>
        /// エラーを所持しているかを返す
        /// </summary>
        public bool HasErrors => CurrentErrors.Count > 0;
        /// <summary>
        /// メッセージボックスを表示するタイミングを管轄
        /// </summary>
        public bool CallShowMessageBox
        {
            get => callShowMessageBox;
            set
            {
                if (callShowMessageBox == value) { return; }
                callShowMessageBox = value;
                CallPropertyChanged();
                callShowMessageBox = false;
            }
        }
        /// <summary>
        /// ウインドウクローズコマンド
        /// </summary>
        public DelegateCommand<Window> WindowCloseCommand
        {
            get
            {
                if (windowCloseCommand == null)
                {
                    windowCloseCommand = new DelegateCommand<Window>
                        (
                            (Window) => DoCloseWindow(Window),
                            (Window) => true
                        ); 
                }
                return windowCloseCommand;
            }
            set => windowCloseCommand = value;
        }
        /// <summary>
        /// ウィンドウタイトル
        /// </summary>
        public string WindowTitle
        {
            get => windowTitle;
            set
            {
                windowTitle = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// アドミン権限
        /// </summary>
        public bool IsAdminPermisson
        {
            get => isAdminPermisson;
            set
            {
                isAdminPermisson = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// ウインドウを閉じます
        /// </summary>
        /// <param name="window"></param>
        protected void DoCloseWindow(Window window) { window.Close(); }

        /// <summary>
        /// ウィンドウ表示コマンドを設定します
        /// </summary>
        /// <param name="window"></param>
        protected void CreateShowWindowCommand(Window window)
        {
            ShowWindowCommand = new DelegateCommand(() => SetShowWindow(window), () => true);
            CallPropertyChanged(nameof(ShowWindowCommand));
            CallShowWindow = true;
        }
        /// <summary>
        /// 表示するウィンドウを設定します
        /// </summary>
        /// <param name="window"></param>
        private void SetShowWindow(Window window)
        {
            ShowWindow = new ShowWindowData() { WindowData = window };
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
        /// <returns>エラー内容</returns>
        public IEnumerable GetErrors(string propertyName)
        {
            return !CurrentErrors.ContainsKey(propertyName) ? null : (IEnumerable)CurrentErrors[propertyName];
        }
        /// <summary>
        /// エラーリスト
        /// </summary>
        public Dictionary<string, string> CurrentErrors = new Dictionary<string, string>();
        /// <summary>
        /// データが存在しない時のエラー操作
        /// </summary>
        /// <param name="propertyName">プロパティ名</param>
        /// <param name="value"></param>
        protected void SetNullOrEmptyError(string propertyName, object value)
        {
            ErrorsListOperation(string.IsNullOrEmpty(value.ToString()), propertyName,
                Properties.Resources.NullErrorInfo);
        }

        /// <summary>
        /// エラーを追加します
        /// </summary>
        /// <param name="propertyName">追加するエラーのプロパティ名</param>
        /// <param name="error">エラー内容</param>
        protected void AddError(string propertyName, string error)
        {
            if (!CurrentErrors.ContainsKey(propertyName))
            { CurrentErrors.Add(propertyName, error); }

            OnErrorsChanged(propertyName);
        }
        /// <summary>
        /// エラーを削除します
        /// </summary>
        /// <param name="propertyName">削除するエラーのプロパティ名</param>
        protected void RemoveError(string propertyName)
        {
            if (CurrentErrors.ContainsKey(propertyName))
            { _ = CurrentErrors.Remove(propertyName); }

            OnErrorsChanged(propertyName);
        }
        /// <summary>
        /// エラーリストに要素が追加、削除されたことを通知するイベントを呼び出します
        /// </summary>
        /// <param name="proeprtyName">変化した要素のプロパティ名</param>
        protected void OnErrorsChanged(string proeprtyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(proeprtyName));
        }
        /// <summary>
        /// エラーリストを操作します
        /// </summary>
        /// <param name="hasError">プロパティがエラーかの判断</param>
        /// <param name="propertyName">リストに追加、削除するプロパティ名</param>
        /// <param name="exeption">エラー内容</param>
        protected void ErrorsListOperation(bool hasError, string propertyName,string exeption)
        {
            if (hasError) { AddError(propertyName, exeption); }
            else { RemoveError(propertyName); }
        }
        /// <summary>
        /// エラーをクリアします
        /// </summary>
        protected void ErrorsClear() { CurrentErrors.Clear(); }

        public virtual void SetRep(Rep rep)
        {
            if (rep == null || string.IsNullOrEmpty(rep.Name)) { WindowTitle = DefaultWindowTitle; }
            else
            {
                IsAdminPermisson = rep.IsAdminPermisson;
                WindowTitle =
                    $"{DefaultWindowTitle}（ログイン : {rep.FirstName}）";
            }
        }

        /// <summary>
        /// 画面タイトルのみをセットします
        /// </summary>
        protected abstract void SetWindowDefaultTitle();
    }
}
