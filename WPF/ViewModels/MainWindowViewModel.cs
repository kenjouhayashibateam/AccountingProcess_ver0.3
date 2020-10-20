using System.Windows;
using WPF.ViewModels.Commands;
using WPF.Views.Datas;

namespace WPF.ViewModels
{
    public class MainWindowViewModel : BaseViewModel, IClosing
    {
        private MessageBoxInfo messageInfo;
        private bool callClosingMessage;

        public DelegateCommand ShowRemainingMoneyCalculation { get; }

        public MainWindowViewModel()
        {
            ShowRemainingMoneyCalculation =
                new DelegateCommand(() => SetShowRemainingMoneyCalculationView(), () => true);
            MessageBoxCommand =
                new DelegateCommand(() => ClosingMessage(), () => true);
        }

        private void SetShowRemainingMoneyCalculationView()
        {
            CreateShowFormCommand(new Views.RemainingMoneyCalculationView());
            CallPropertyChanged();
        }

        private void ClosingMessage()
        {
            messageInfo = new MessageBoxInfo()
            {
                Message = "終了します。よろしいですか？",
                Button = System.Windows.MessageBoxButton.YesNo,
                Title = "経理システムの終了",
                Image = System.Windows.MessageBoxImage.Question
            };

            CallPropertyChanged(nameof(MessageInfo));
        }

        public DelegateCommand MessageBoxCommand { get; }

        public MessageBoxInfo MessageInfo
        {
            get => messageInfo;
            set
            {
                messageInfo = value;
                CallPropertyChanged();
            }
        }

        public bool CallClosingMessage
        {
            get => callClosingMessage;
            set
            {
                if (callClosingMessage == value) return;
                callClosingMessage = value;
                CallPropertyChanged();
                callClosingMessage = false;
            }
        }

        public bool OnClosing()
        {
            CallClosingMessage = true;
            return MessageInfo.Result != MessageBoxResult.Yes;
        }
    }
}