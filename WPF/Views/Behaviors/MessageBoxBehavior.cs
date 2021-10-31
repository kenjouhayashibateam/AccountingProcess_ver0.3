using System.Windows;
using System.Windows.Interactivity;
using WPF.ViewModels.Commands;
using WPF.Views.Datas;

namespace WPF.Views.Behaviors
{
    /// <summary>
    /// メッセージボックス動作クラス
    /// </summary>
    public class MessageBoxBehavior : TriggerAction<FrameworkElement>
    {
        /// <summary>
        /// MessageBox.Showを実行します
        /// </summary>
        /// <param name="parameter">メッセージボックスパラメータ</param>
        protected override void Invoke(object parameter)
        {
            if (parameter is DependencyPropertyChangedEventArgs e
                && e.NewValue is MessageBoxInfo info)
            {
                PlaySoundEffect(info);
                info.Result = MessageBox.Show
                    (info.Message, info.Title, info.Button, info.Image, info.DefaultResult, info.Options);
            }
        }
        /// <summary>
        /// メッセージボックスに応じてSEを再生します
        /// </summary>
        /// <param name="messageBoxInfo"></param>
        private void PlaySoundEffect(MessageBoxInfo messageBoxInfo)
        {
            switch (messageBoxInfo.Image)
            {
                case MessageBoxImage.Information:
                    break;
                case MessageBoxImage.None:
                    break;
                case MessageBoxImage.Hand:
                    break;
                case MessageBoxImage.Question:
                    WavSoundPlayCommand.Play("Notify.wav");
                    break;
                case MessageBoxImage.Exclamation:
                    WavSoundPlayCommand.Play("CriticalStop.wav"); 
                    break;
                default:
                    break;
            }
        }
    }
}
