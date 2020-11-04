using Domain.Entities.ValueObjects;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;
using WPF.ViewModels.Commands;

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

        /// <summary>
        /// 担当者リスト
        /// </summary>
        public ObservableCollection<Rep> Reps { get; }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public LoginViewModel()
        {
            Reps = new ObservableCollection<Rep>
            {
                new Rep("aaa", "bbb", "ccc", true),
                new Rep("xxx","yyy","zzz",true)
            };
            PasswordCheckReversCommand = new DelegateCommand(() => CheckRevers(), () => true);
        }
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
        /// パスワードの文字を隠すかのチェックを反転させます
        /// </summary>
        public void CheckRevers()
        {
            PasswordCharCheck = !PasswordCharCheck;
        }

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
    }
}
