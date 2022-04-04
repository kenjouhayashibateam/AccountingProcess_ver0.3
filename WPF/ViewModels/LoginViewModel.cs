using Domain.Entities;
using Domain.Entities.ValueObjects;
using Domain.Repositories;
using Infrastructure;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using WPF.ViewModels.Commands;
using WPF.ViewModels.Datas;
using WPF.Views.Datas;
using static Domain.Entities.Helpers.TextHelper;

namespace WPF.ViewModels
{
    /// <summary>
    /// ログイン画面
    /// </summary>
    public class LoginViewModel : BaseViewModel
    {
        private Rep currentRep;
        private string password;
        private bool passwordCharCheck;
        private bool windowCloseSwich;
        private string repName;
        private bool isLoginButtonEnabled;

        /// <summary>
        /// 担当者のパスワードを検証してログインします
        /// </summary>
        private void Login()
        {
            if (GetHashValue(Password, CurrentRep.ID) == CurrentRep.Password)
            {
                MessageBox = new MessageBoxInfo()
                {
                    Message = $"担当者 : {CurrentRep.Name} でログインしました。",
                    Image = MessageBoxImage.Information,
                    Title = "ログイン成功",
                    Button = MessageBoxButton.OK
                };
                CallPropertyChanged(nameof(MessageBox));
                LoginRep loginRep = LoginRep.GetInstance();
                loginRep.SetRep(CurrentRep);
                WindowCloseSwich = true;
            }
            else
            {
                MessageBox = new MessageBoxInfo()
                {
                    Message = "ログインできません",
                    Image = MessageBoxImage.Warning,
                    Title = "ログイン失敗",
                    Button = MessageBoxButton.OK
                };
                CallPropertyChanged(nameof(MessageBox));
            }
        }
        /// <summary>
        /// 担当者リスト
        /// </summary>
        public ObservableCollection<Rep> Reps { get; }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public LoginViewModel(IDataBaseConnect dataBaseConnect) : base(dataBaseConnect)
        {
            Reps = DataBaseConnect.ReferenceRep(string.Empty, true);
            PasswordCheckReversCommand = new DelegateCommand(() => CheckRevers(), () => true);
            LoginCommand = new DelegateCommand(() => Login(), () => IsLoginButtonEnabled);
            CurrentRep = Reps[0];
        }
        public LoginViewModel() : this(DefaultInfrastructure.GetDefaultDataBaseConnect()) { }

        public DelegateCommand PasswordCheckReversCommand { get; }
        /// <summary>
        /// 選択された担当者
        /// </summary>
        public Rep CurrentRep
        {
            get => currentRep;
            set
            {
                currentRep = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// パスワード
        /// </summary>
        public string Password
        {
            get => password;
            set
            {
                password = value;
                ValidationProperty(nameof(Password), value);
                IsLoginButtonEnabled = GetErrors(nameof(Password)) == null;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// パスワードの文字を隠すかのチェック
        /// </summary>
        public bool PasswordCharCheck
        {
            get => passwordCharCheck;
            set
            {
                passwordCharCheck = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// ログイン担当者名
        /// </summary>
        public string RepName
        {
            get => repName;
            set
            {
                if (repName == value) { return; }
                repName = value;

                CurrentRep = Reps.FirstOrDefault(r => r.Name == repName);

                if (CurrentRep == null) { RepName = string.Empty; }
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// ログインコマンド
        /// </summary>
        public DelegateCommand LoginCommand { get; }
        /// <summary>
        /// ログインボタンEnable
        /// </summary>
        public bool IsLoginButtonEnabled
        {
            get => isLoginButtonEnabled;
            set
            {
                isLoginButtonEnabled = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// ウィンドウを閉じるフラグを立てるbool
        /// </summary>
        public bool WindowCloseSwich
        {
            get => windowCloseSwich;
            set
            {
                if (windowCloseSwich == value) { return; }
                windowCloseSwich = value;
                CallPropertyChanged();
                windowCloseSwich = false;
            }
        }

        /// <summary>
        /// パスワードの文字を隠すかのチェックを反転させます
        /// </summary>
        public void CheckRevers() { PasswordCharCheck = !PasswordCharCheck; }

        public override void ValidationProperty(string propertyName, object value)
        {
            switch (propertyName)
            {
                case nameof(Password):
                    ErrorsListOperation
                        (string.IsNullOrEmpty((string)value), propertyName, Properties.Resources.NullErrorInfo);
                    if (string.IsNullOrEmpty((string)value)) { break; }
                    ErrorsListOperation
                        (password != CurrentRep.Password, propertyName,
                            Properties.Resources.PasswordErrorInfo);
                    if (GetErrors(propertyName) != null)
                    {
                        ErrorsListOperation
                            (GetHashValue((string)value, CurrentRep.ID) != CurrentRep.Password,
                                propertyName, Properties.Resources.PasswordErrorInfo);
                    }
                    break;
                default:
                    break;
            }
        }

        protected override void SetWindowDefaultTitle() { DefaultWindowTitle = "担当者ログイン"; }
    }
}
