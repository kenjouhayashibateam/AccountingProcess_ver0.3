using System.Collections;
using System.Windows;
using WPF.ViewModels.Commands;
using WPF.Views.Datas;

namespace WPF.ViewModels
{
    /// <summary>
    /// メインウィンドウ
    /// </summary>
    public class MainWindowViewModel : BaseViewModel, IClosing
    {
        #region Properties
        private MessageBoxInfo messageInfo;
        private bool callClosingMessage;
        private bool processFeatureEnabled;
        private string location;
        private readonly ScreenTransition screenTransition = new ScreenTransition();
        private bool shorendoChecked;
        private bool kanriJimushoChecked;
        #endregion

        public enum Locations
        {
            管理事務所,
            青蓮堂
        }

        /// <summary>
        /// 経理担当場所を管理事務所に設定するコマンド
        /// </summary>
        public DelegateCommand SetLocationKanriJimushoCommand { get; }
        /// <summary>
        /// 経理担当場所を青蓮堂に設定するコマンド
        /// </summary>
        public DelegateCommand SetLodationShorendoCommand { get; }
        /// <summary>
        /// 金庫金額計算画面表示コマンド
        /// </summary>
        public DelegateCommand ShowRemainingMoneyCalculationCommand { get; }
        /// <summary>
        /// データ管理画面表示コマンド
        /// </summary>
        public DelegateCommand ShowDataManagement { get; }
        /// <summary>
        /// メッセージボックス表示コマンド
        /// </summary>
        public DelegateCommand MessageBoxCommand { get; }

        /// <summary>
        /// コンストラクタ　DelegateCommandのインスタンスを生成します
        /// </summary>
        public MainWindowViewModel()
        {
            ShowRemainingMoneyCalculationCommand =
                new DelegateCommand(() => SetShowRemainingMoneyCalculationView(), () => true);
            MessageBoxCommand =
                new DelegateCommand(() => ClosingMessage(), () => true);
            SetLocationKanriJimushoCommand =
                new DelegateCommand(() => SetLocationKanriJimusho(), () => true);
            SetLodationShorendoCommand =
                new DelegateCommand(() => SetLocationShorendo(), () => true);
            ShowDataManagement =
                new DelegateCommand(() => SetShowDataManagementView(), () => true);
        }
        /// <summary>
        /// データ管理画面を表示します
        /// </summary>
        private void SetShowDataManagementView()
        {
            CreateShowWindowCommand(screenTransition.DataManagement());
            CallPropertyChanged();
        }
        /// <summary>
        /// 金庫金額計算画面を表示します
        /// </summary>
        private void SetShowRemainingMoneyCalculationView()
        {
            CreateShowWindowCommand(screenTransition.RemainingMoneyCalculation());
            CallPropertyChanged();
        }
        /// <summary>
        /// 経理システムの終了確認のメッセージボックスを設定します
        /// </summary>
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
        /// <summary>
        /// 経理システムの終了確認メッセージボックスを呼び出します
        /// </summary>
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
        /// <summary>
        /// 経理担当場所
        /// </summary>
        public string Location
        {
            get => location;
            set
            {
                location = value;
                ProcessFeatureEnabled = true;
            }
        }
        /// <summary>
        /// 管理事務所チェック欄
        /// </summary>
        public bool KanriJimushoChecked
        {
            get => kanriJimushoChecked;
            set
            {
                kanriJimushoChecked = value;
                ValidationProperty(nameof(KanriJimushoChecked), value);
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 青蓮堂チェック欄
        /// </summary>
        public bool ShorendoChecked
        {
            get => shorendoChecked;
            set
            {
                shorendoChecked = value;
                ValidationProperty(nameof(ShorendoChecked), value);
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 経理担当場所を管理事務所に設定します
        /// </summary>
        private void SetLocationKanriJimusho()
        {
            KanriJimushoChecked = true;
            Location = Locations.管理事務所.ToString() ;
        }
        /// <summary>
        /// 経理担当場所を青蓮堂に設定します
        /// </summary>
        private void SetLocationShorendo()
        {
            ShorendoChecked = true;
            Location = Locations.青蓮堂.ToString();
        }
        /// <summary>
        /// 画面を閉じるメソッドを使用するかのチェック
        /// </summary>
        /// <returns></returns>
        public bool OnClosing()
        {
            CallClosingMessage = true;
            return MessageInfo.Result != MessageBoxResult.Yes;
        }

        public override void ValidationProperty(string propertyName, object value)
        {
            switch(propertyName)
            {
                case nameof(kanriJimushoChecked):
                    SetLocationErrorsListOperation(propertyName);
                    break;
                case nameof(shorendoChecked):
                    SetLocationErrorsListOperation(propertyName);
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 経理担当場所のエラーを判定します
        /// </summary>
        private void SetLocationErrorsListOperation(string propertyName)
        {
            ErrorsListOperation(KanriJimushoChecked == false & ShorendoChecked == false, propertyName, "経理担当場所を設定して下さい");
        }
    }
}