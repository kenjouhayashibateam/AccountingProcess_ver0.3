using Domain.Entities.ValueObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF.ViewModels
{
    /// <summary>
    /// データ管理画面
    /// </summary>
    public class DataManagementViewModel:BaseViewModel
    {
        private string repIDField;
        private string repName;
        private string repCurrentPassword;
        private string repNewPassword;
        private string repReenterPassword;
        private bool isValidity;
        private bool isRepDataLock;
        private bool isRepPasswordLock=false;

        public Rep CurrentRep = new Rep();

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

        public override void ValidationProperty(string propertyName, object value)
        {
            switch (propertyName)
            {
                case nameof(RepNewPassword):
                    ErrorsListOperation(RepNewPassword != RepReenterPassword, propertyName, "パスワードが一致していません。");
                    break;
                case nameof(RepReenterPassword):
                    ErrorsListOperation(RepNewPassword != RepReenterPassword, propertyName, "パスワードが一致していません。");
                    if (GetErrors(propertyName) == null)
                        ErrorsListOperation(RepReenterPassword == string.Empty, propertyName, "!!!");
                    break;

                default:
                    break;
            }
        }
    }
}