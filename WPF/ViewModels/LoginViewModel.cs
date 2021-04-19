using Domain.Entities;
using Domain.Entities.Helpers;
using Domain.Entities.ValueObjects;
using Domain.Repositories;
using Infrastructure;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using WPF.ViewModels.Commands;
using WPF.Views.Datas;

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
        private string repName;
        private bool isLoginButtonEnabled;

        /// <summary>
        /// 担当者のパスワードを検証してログインします
        /// </summary>
        private void Login()
        {
            if(Password==CurrentRep.Password)
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
        public LoginViewModel(IDataBaseConnect dataBaseConnect):base(dataBaseConnect)
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
                if (repName == value) return;
                repName = value;
                CurrentRep = Reps.FirstOrDefault(r => r.Name == repName);
                if (CurrentRep == null) RepName = string.Empty;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// ログインコマンド
        /// </summary>
        public DelegateCommand LoginCommand{get;}
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
        /// パスワードの文字を隠すかのチェックを反転させます
        /// </summary>
        public void CheckRevers() => PasswordCharCheck = !PasswordCharCheck;

        public override void ValidationProperty(string propertyName, object value)
        {
            switch(propertyName)
            {
                case nameof(Password):
                    ErrorsListOperation(string.IsNullOrEmpty(Password), propertyName, Properties.Resources.NullErrorInfo);
                    ErrorsListOperation(password != CurrentRep.Password, propertyName, Properties.Resources.PasswordErrorInfo);
                    break;
                default:
                    break;
            }
        }

        protected override void SetWindowDefaultTitle() => DefaultWindowTitle = "担当者ログイン";

        public override void SetRep(Rep rep) 
        {
            {
                if (rep == null || string.IsNullOrEmpty(rep.Name)) WindowTitle = DefaultWindowTitle;
                else
                {
                    IsAdminPermisson = rep.IsAdminPermisson;
                    WindowTitle = $"{DefaultWindowTitle}（ログイン : {TextHelper.GetFirstName(rep.Name)}）";
                }
            }
        }
    }
}
