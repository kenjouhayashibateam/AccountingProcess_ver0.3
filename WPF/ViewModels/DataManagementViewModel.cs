using Domain.Entities.ValueObjects;
using System.Collections.Generic;
using WPF.ViewModels.Commands;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System;

namespace WPF.ViewModels
{
    /// <summary>
    /// データ管理画面
    /// </summary>
    public class DataManagementViewModel : BaseViewModel
    {
        #region Properties
        private string repIDField;
        private string repName;
        private string repCurrentPassword;
        private string repNewPassword;
        private bool isValidity;
        private bool isRepNameDataEnabled;
        private bool isRepPasswordEnabled;
        private bool isRepNewPasswordEnabled;
        private string dataOperationButtonContent;
        private bool isCheckedRegistration;
        private bool isCheckedUpdate;
        private string referenceRep;
        private bool repValidity;
        private bool newPasswordCharCheck;
        private bool currentPasswordCharCheck;
        private DataOperation CurrentOperation;
        private string currentRepPasswordBorderBrush;
        private string currentRepPasswordBackground;
        private string newRepPasswordBorderBrush;
        private string newRepPasswordBackground;
        private readonly string TrueControlBorderBrush = "#000000";
        private readonly string FalseControlBorderBrush = "#FFABADB3";
        private readonly string TrueControlBackground = "#FFFFFF";
        private readonly string FalseControlBackground = "#A9A9A9";
        private bool isReferenceMenuEnabled;
        private Rep currentRep;
        private ObservableCollection<Rep> repList;
        #endregion

        private enum DataOperation
        {
            登録,
            更新
        }

        public DataManagementViewModel()
        {
            RepList = new ObservableCollection<Rep>();
            CurrentRep = new Rep(null, "a a", "aaa", true);
            ListAdd(new Rep(null,"a a","aaa",true));
            ListAdd(new Rep(null, "b b", "bbb", true));
            SetDataRegistrationCommand = new DelegateCommand(() => SetDataOperation(DataOperation.登録), () => true);
            SetDataUpdateCommand = new DelegateCommand(() => SetDataOperation(DataOperation.更新), () => true);
            RepNewPasswordCharCheckedReversCommand = new DelegateCommand(() => RepNewPasswordCharCheckedRevers(), () => true);
            RepCurrentPasswordCharCheckedReversCommand = new DelegateCommand(() => RepCurrentPasswordCharCheckedRevers(), () => true);
            RepRegistrationCommand = new DelegateCommand(() => RepRegistration(), () => true);
            SetDataOperation(DataOperation.登録);
        }

        public void ListAdd(Rep rep)
        {
            RepList.Add(rep);
            CallPropertyChanged(nameof(RepList));
        }

        public DelegateCommand RepNewPasswordCharCheckedReversCommand { get; }
        public DelegateCommand RepCurrentPasswordCharCheckedReversCommand { get; }
        public DelegateCommand SetDataRegistrationCommand { get; }
        public DelegateCommand SetDataUpdateCommand { get; }
        public DelegateCommand RepRegistrationCommand { get; }

        private void RepRegistration()
        {
            ListAdd(new Rep(null, "c c", "ccc", true));
        }

        private void RepNewPasswordCharCheckedRevers() => NewPasswordCharCheck = !NewPasswordCharCheck;

        private void RepCurrentPasswordCharCheckedRevers() => CurrentPasswordCharCheck = !CurrentPasswordCharCheck;

        private void SetDataOperation(DataOperation Operation)
        {
            CurrentOperation = Operation;
            IsCheckedRegistration = (Operation == DataOperation.登録);
            IsCheckedUpdate = (Operation == DataOperation.更新);
            DataOperationButtonContent = Operation.ToString();
            SetDetailLocked();
        }

        private void SetDetailLocked()
        {
            switch (CurrentOperation)
            {
                case DataOperation.登録:
                    SetFieldRegisterRep();
                    break;
                case DataOperation.更新:
                    SetFieldUpdaterRep();
                    break;
                default:
                    break;
            }
        }

        private void SetFieldUpdaterRep()
        {
            IsRepNameDataEnabled = false;
            IsRepPasswordEnabled = true;
            IsRepNewPasswordEnabled = false;
            IsReferenceMenuEnabled = true;
            RepName = CurrentRep.Name;
            IsValidity = CurrentRep.IsValidity;
        }

        private void SetFieldRegisterRep()
        {
            IsReferenceMenuEnabled = false;
            IsRepNameDataEnabled = true;
            IsRepPasswordEnabled = false;
            IsRepNewPasswordEnabled = true;
            //ReferenceMenuの実装を終えて、CurrentRepが変化した際に値を反映できるようになったら削除
            RepName = string.Empty;
            RepCurrentPassword = string.Empty;
            RepNewPassword = string.Empty;
            IsValidity = true;
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
                ValidationProperty(nameof(RepName), value);
                CallPropertyChanged();
            }
        }

        public string RepCurrentPassword
        {
            get => repCurrentPassword;
            set
            {
                repCurrentPassword = value;
                ValidationProperty(nameof(RepCurrentPassword), value);
                if (CurrentOperation == DataOperation.更新) IsRepNewPasswordEnabled = value == CurrentRep.Password;
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

        public bool IsValidity
        {
            get => isValidity;
            set
            {
                isValidity = value;
                CallPropertyChanged();
            }
        }

        public bool IsRepNameDataEnabled
        {
            get => isRepNameDataEnabled;
            set
            {
                isRepNameDataEnabled = value;
                CallPropertyChanged();
            }
        }


        public bool IsRepPasswordEnabled
        {
            get => isRepPasswordEnabled;
            set
            {
                isRepPasswordEnabled = value;
                if (value)
                {
                    CurrentRepPasswordBorderBrush = TrueControlBorderBrush;
                    CurrentRepPasswordBackground = TrueControlBackground;
                }
                else
                {
                    CurrentRepPasswordBorderBrush = FalseControlBorderBrush;
                    CurrentRepPasswordBackground = FalseControlBackground;
                }
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

        public bool IsCheckedUpdate
        {
            get => isCheckedUpdate;
            set
            {
                isCheckedUpdate = value;
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

        public bool NewPasswordCharCheck
        {
            get => newPasswordCharCheck;
            set
            {
                newPasswordCharCheck = value;
                Invoke(nameof(NewPasswordCharCheck));
            }
        }

        public bool CurrentPasswordCharCheck
        {
            get => currentPasswordCharCheck;
            set
            {
                currentPasswordCharCheck = value;
                CallPropertyChanged();
            }
        }

        public bool IsRepNewPasswordEnabled
        {
            get => isRepNewPasswordEnabled;
            set
            {
                isRepNewPasswordEnabled = value;
                if (value)
                {
                    NewRepPasswordBorderBrush = TrueControlBorderBrush;
                    NewRepPasswordBackground = TrueControlBackground;
                }
                else
                {
                    NewRepPasswordBorderBrush = FalseControlBorderBrush;
                    NewRepPasswordBackground = FalseControlBackground;
                }
                CallPropertyChanged();
            }
        }

        public string CurrentRepPasswordBorderBrush
        {
            get => currentRepPasswordBorderBrush;
            set
            {
                currentRepPasswordBorderBrush = value;
                CallPropertyChanged();
            }
        }

        public string CurrentRepPasswordBackground
        {
            get => currentRepPasswordBackground;
            set
            {
                currentRepPasswordBackground = value;
                CallPropertyChanged();
            }
        }

        public string NewRepPasswordBorderBrush
        {
            get => newRepPasswordBorderBrush;
            set
            {
                newRepPasswordBorderBrush = value;
                CallPropertyChanged();
            }
        }
        public string NewRepPasswordBackground
        {
            get => newRepPasswordBackground;
            set
            {
                newRepPasswordBackground = value;
                CallPropertyChanged();
            }
        }

        public bool IsReferenceMenuEnabled
        {
            get => isReferenceMenuEnabled;
            set
            {
                isReferenceMenuEnabled = value;
                CallPropertyChanged();
            }
        }

        public Rep CurrentRep
        {
            get => currentRep;
            set
            {
                currentRep = value;
                CallPropertyChanged();
            }
        }

        public ObservableCollection<Rep> RepList
        {
            get => repList;
            set
            {
                repList = value;
                CallPropertyChanged();
            }
        }

        public override void ValidationProperty(string propertyName, object value)
        {
            switch (propertyName)
            {
                case nameof(RepName):
                    ErrorsListOperation(string.IsNullOrEmpty(RepName), propertyName, Properties.Resources.NullErrorInfo);
                    break;
                case nameof(RepCurrentPassword):
                    if (IsRepPasswordEnabled) ErrorsListOperation(RepCurrentPassword != CurrentRep.Password, propertyName, Properties.Resources.PasswordErrorInfo);
                    break;
                case nameof(RepNewPassword):
                    if(IsRepNewPasswordEnabled) ErrorsListOperation(string.IsNullOrEmpty(RepNewPassword), propertyName, Properties.Resources.NullErrorInfo);
                    break;

                default:
                    break;
            }
        }
    }
}