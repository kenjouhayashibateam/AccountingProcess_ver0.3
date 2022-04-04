using Domain.Repositories;
using System.Windows;
using WPF.Views.Datas;

namespace WPF.ViewModels.Datas
{
    /// <summary>
    /// データ登録のみのViewModel
    /// </summary>
    public abstract class JustRegistraterDataViewModel : BaseViewModel
    {
        protected JustRegistraterDataViewModel
            (IDataBaseConnect dataBaseConnect) : base(dataBaseConnect) {}
        /// <summary>
        /// 登録完了メッセージを生成、呼び出します
        /// </summary>
        protected void CallCompletedRegistration() 
        { CallOkInfomationMessageBox("登録完了", "登録しました"); }
        /// <summary>
        /// OKメッセージを生成します
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        protected void CallOkInfomationMessageBox(string title, string message)
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
        /// データ操作確認メッセージボックスを生成、呼び出します
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
            CallShowMessageBox = true;
            return MessageBox.Result;
        }
    }
}
