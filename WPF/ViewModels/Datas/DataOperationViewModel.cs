﻿using Domain.Entities;
using Domain.Repositories;
using System.Windows;
using WPF.ViewModels.Commands;
using WPF.Views.Datas;

namespace WPF.ViewModels.Datas
{
    public abstract class DataOperationViewModel : BaseViewModel
    {
        private bool _isCheckedRegistration;
        private bool _isCheckedUpdate;

        protected DataOperation CurrentOperation;
        protected readonly LoginRep LoginRep = LoginRep.GetInstance();

        /// <summary>
        /// データ操作の判別
        /// </summary>
        protected enum DataOperation
        {
            登録,
            更新
        }

        public DataOperationViewModel(IDataBaseConnect dataBaseConnect) : base(dataBaseConnect)
        {
            SetDelegateCommand();
            SetDataList();
            SetDataRegistrationCommand = new DelegateCommand(() =>
                SetDataOperation(DataOperation.登録), () => true);
            SetDataUpdateCommand = new DelegateCommand(() =>
                SetDataOperation(DataOperation.更新), () => true);
        }
        /// <summary>
        /// データ操作を「登録」にするコマンド
        /// </summary>
        public DelegateCommand SetDataRegistrationCommand { get; set; }
        /// <summary>
        /// データ操作を「更新」にするコマンド
        /// </summary>
        public DelegateCommand SetDataUpdateCommand { get; set; }
        /// <summary>
        /// 登録、更新確認メッセージボックスを生成、呼び出します
        /// </summary>
        /// <param name="confirmationMessage">確認内容（プロパティ等）</param>
        /// <param name="titleOperationCategory">タイトルに表示するクラス名</param>
        /// <returns>OK、Cancelを返します</returns>
        protected MessageBoxResult CallConfirmationDataOperation
            (string confirmationMessage, string titleOperationCategory)
        {
            MessageBox = new MessageBoxInfo()
            {
                Message = confirmationMessage,
                Title = $"{titleOperationCategory}データ操作確認",
                Button = MessageBoxButton.OKCancel,
                Image = MessageBoxImage.Question,
            };
            return MessageBox.Result;
        }
        /// <summary>
        /// 更新完了メッセージを生成、呼び出します
        /// </summary>
        protected void CallCompletedUpdate() { CallOkInfomationMessageBox("更新完了", "更新しました"); }

        /// <summary>
        /// 登録完了メッセージを生成、呼び出します
        /// </summary>
        protected void CallCompletedRegistration() { CallOkInfomationMessageBox("登録完了", "登録しました"); }

        private void CallOkInfomationMessageBox(string title, string message)
        {
            MessageBox = new MessageBoxInfo
            {
                Button = MessageBoxButton.OK,
                Image = MessageBoxImage.Information,
                Title = title,
                Message = message
            };
        }
        /// <summary>
        /// データ操作の登録Checked
        /// </summary>
        public bool IsCheckedRegistration
        {
            get => _isCheckedRegistration;
            set
            {
                _isCheckedRegistration = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// データ操作の更新Checked
        /// </summary>
        public bool IsCheckedUpdate
        {
            get => _isCheckedUpdate;
            set
            {
                _isCheckedUpdate = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// データ操作のジャンルによって、各詳細のコントロールの値をクリア、Enableの設定をします
        /// </summary>
        protected abstract void SetDetailLocked();
        /// <summary>
        /// 各データ操作ボタンのContentを設定します
        /// </summary>
        protected abstract void SetDataOperationButtonContent(DataOperation operation);
        /// <summary>
        /// 各データのリストを生成します
        /// </summary>
        protected abstract void SetDataList();
        /// <summary>
        /// データ操作のジャンルを切り替え、操作ボタンの表示文字列を入力し、
        /// コントロールの値をクリアして、Enableを設定します
        /// </summary>
        /// <param name="Operation"></param>
        protected void SetDataOperation(DataOperation Operation)
        {
            CurrentOperation = Operation;
            switch (CurrentOperation)
            {
                case DataOperation.登録:
                    IsCheckedRegistration = true;
                    break;
                case DataOperation.更新:
                    IsCheckedUpdate = true;
                    SetDataList();
                    break;
                default:
                    break;
            }
            SetDataOperationButtonContent(Operation);
            SetDetailLocked();
        }
        /// <summary>
        /// 各デリゲートコマンドを生成します
        /// </summary>
        protected abstract void SetDelegateCommand();
        /// <summary>
        /// 更新の必要がないことをメッセージボックスで知らせます
        /// </summary>
        protected void CallNoRequiredUpdateMessage()
        {
            MessageBox = new MessageBoxInfo()
            {
                Message = "更新の必要はありません",
                Image = MessageBoxImage.Exclamation,
                Title = "データ操作案内",
                Button = MessageBoxButton.OK
            };
        }
    }
}
