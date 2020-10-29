using Domain.Entities.ValueObjects;
using System.Collections.Generic;
using WPF.ViewModels.Commands;

namespace WPF.ViewModels
{
    /// <summary>
    /// データ管理画面
    /// </summary>
    public class DataManagementViewModel:BaseViewModel
    {
        #region Properties
        private string repIDField;
        private string repName;
        private string repCurrentPassword;
        private string repNewPassword;
        private string repReenterPassword;
        private bool isValidity;
        private bool isRepDataLock;
        private bool isRepPasswordLock=false;
        private string newPasswordTooltip;
        private string reenterPasswordTooltip;
        private string dataOperationButtonContent;
        private bool isCheckedRegistration=true;
        private bool isCheckdUpdate;
        private string referenceRep;
        private bool repValidity;
        public Dictionary<string, Rep> RepList = new Dictionary<string, Rep>();
      
        #endregion

        

        private enum DataOperation
        {
            登録,
            更新
        }

        private readonly Rep CurrentRep = new Rep();

        public DataManagementViewModel()
        {
            SetDataRegistrationCommand = new DelegateCommand(() => SetDataOperation(DataOperation.登録), () => true);
            SetDataUpdateCommand = new DelegateCommand(() => SetDataOperation(DataOperation.更新), () => true);
        }

        public DelegateCommand SetDataRegistrationCommand { get; }
        public DelegateCommand SetDataUpdateCommand { get; }

        private void SetDataOperation(DataOperation Operation)
        {
            IsCheckedRegistration = (Operation==DataOperation.登録);
            IsCheckdUpdate = (Operation == DataOperation.更新);
            DataOperationButtonContent = Operation.ToString();
        }

        public string RepIDField
        {
            get => repIDField;
            set
            {
                repIDField = value;
                CallPropertyChanged();
            }
        }

        public string RepName
        {
            get => repName;
            set
            {
                repName = value;
                CallPropertyChanged();
            }
        }

        public string RepCurrentPassword
        {
            get => repCurrentPassword;
            set
            {
                repCurrentPassword = value;
                CallPropertyChanged();
            }
        }

        public string RepNewPassword
        {
            get => repNewPassword;
            set
            {
                repNewPassword = value;
                ValidationProperty(nameof(RepNewPassword), value);
                CallPropertyChanged();
            }
        }

        public string RepReenterPassword
        {
            get => repReenterPassword;
            set
            {
                repReenterPassword = value;
                ValidationProperty(nameof(RepReenterPassword), value);
                CallPropertyChanged();
            }
        }

        public bool IsValidity
        {
            get => isValidity;
            set
            {
                isValidity = value;
                CallPropertyChanged();
            }
        }

        public bool IsRepDataLock
        {
            get => isRepDataLock;
            set
            {
                isRepDataLock = value;
                CallPropertyChanged();
            }
        }

        public bool IsRepPasswordLock
        {
            get => isRepPasswordLock;
            set
            {
                isRepPasswordLock = value;
                CallPropertyChanged();
            }
        }

        public string NewPasswordTooltip
        {
            get => newPasswordTooltip;
            set
            {
                newPasswordTooltip = value;
                CallPropertyChanged();
            }
        }

        public string ReenterPasswordTooltip
        {
            get => reenterPasswordTooltip;
            set
            {
                reenterPasswordTooltip = value;
                CallPropertyChanged();
            }
        }

        public string DataOperationButtonContent
        {
            get => dataOperationButtonContent;
            set
            {
                dataOperationButtonContent = value;
                CallPropertyChanged();
            }
        }

        public bool IsCheckedRegistration
        {
            get => isCheckedRegistration;
            set
            {
                isCheckedRegistration = value;
                CallPropertyChanged();
            }
        }

        public bool IsCheckdUpdate
        {
            get => isCheckdUpdate;
            set
            {
                isCheckdUpdate = value;
                CallPropertyChanged();
            }
        }

        public string ReferenceRep
        {
            get => referenceRep;
            set
            {
                referenceRep = value;
                CallPropertyChanged();
            }
        }

        public bool RepValidity
        {
            get => repValidity;
            set
            {
                repValidity = value;
                CallPropertyChanged();
            }
        }

        public override void ValidationProperty(string propertyName, object value)
        {
            int originalCount = CurrentErrors.Count;

            switch (propertyName)
            {
                case nameof(RepNewPassword):
                    ErrorsListOperation(RepNewPassword != RepReenterPassword, propertyName, "パスワードが一致していません。");
                    break;
                case nameof(RepReenterPassword):
                    ErrorsListOperation(RepNewPassword != RepReenterPassword, propertyName, "パスワードが一致していません。");
                    if (GetErrors(propertyName) == null)
                        ErrorsListOperation(RepReenterPassword == string.Empty, propertyName, "!!!");
                    if (originalCount < CurrentErrors.Count)
                        ReenterPasswordTooltip = CurrentErrors[propertyName];
                    break;

                default:
                    break;
            }
        }
    }
}