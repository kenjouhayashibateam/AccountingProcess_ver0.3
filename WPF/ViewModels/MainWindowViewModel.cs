using System.Collections;
using System.Windows;
using WPF.ViewModels.Commands;
using WPF.Views.Datas;

namespace WPF.ViewModels
{
    public class MainWindowViewModel : BaseViewModel, IClosing
    {
        private MessageBoxInfo messageInfo;
        private bool callClosingMessage;
        private bool processFeatureEnabled;
        private int locationID;
        private readonly ScreenTransition screenTransition = new ScreenTransition();

        public DelegateCommand SetKanriJimushoIDCommand { get; }
        public DelegateCommand SetShorendoIDCommand { get; }
        public DelegateCommand ShowRemainingMoneyCalculation { get; }
        public DelegateCommand ShowDataManagement { get; }
        public DelegateCommand MessageBoxCommand { get; }

        public MainWindowViewModel()
        {
            ShowRemainingMoneyCalculation =
                new DelegateCommand(() => SetShowRemainingMoneyCalculationView(), () => true);
            MessageBoxCommand =
                new DelegateCommand(() => ClosingMessage(), () => true);
            SetKanriJimushoIDCommand =
                new DelegateCommand(() => SetKanriJimushoID(), () => true);
            SetShorendoIDCommand =
                new DelegateCommand(() => SetShorendoID(), () => true);
            ShowDataManagement =
                new DelegateCommand(() => SetShowDataManagementView(), () => true);
        }

        private void SetShowDataManagementView()
        {
            CreateShowFormCommand(screenTransition.DataManagement());
            CallPropertyChanged();
        }

        private void SetShowRemainingMoneyCalculationView()
        {
            CreateShowFormCommand(screenTransition.RemainingMoneyCalculation());
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

        /// <summary>
        /// 処理機能コントロールのEnabled
        /// </summary>
        public bool ProcessFeatureEnabled
        {
            get => processFeatureEnabled;
            set
            {
                processFeatureEnabled = value;
                CallPropertyChanged();
            }
        }

        public int LocationID
        {
            get => locationID;
            set
            {
                locationID = value;
                ProcessFeatureEnabled = true;
            }
        }

        public bool KanriJimushoChecked
        {
            get => kanriJimushoChecked;
            set
            {
                kanriJimushoChecked = value;
                CallPropertyChanged();
            }
        }

        public bool ShorendoChecked
        {
            get => shorendoChecked;
            set
            {
                shorendoChecked = value;
                CallPropertyChanged();
            }
        }

        private bool shorendoChecked;

        private bool kanriJimushoChecked;

        private void SetKanriJimushoID()
        {
            KanriJimushoChecked = true;
            LocationID = 0;
        }

        private void SetShorendoID()
        {
            ShorendoChecked = true;
            LocationID = 1;
        }

        public bool OnClosing()
        {
            CallClosingMessage = true;
            return MessageInfo.Result != MessageBoxResult.Yes;
        }

        public override void ValidationProperty(string propertyName, object value)
        {
            throw new System.NotImplementedException();
        }
    }
}