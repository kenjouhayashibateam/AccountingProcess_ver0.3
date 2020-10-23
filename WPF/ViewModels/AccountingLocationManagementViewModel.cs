using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF.ViewModels
{
    /// <summary>
    /// 経理担当場所データ管理画面
    /// </summary>
    public class AccountingLocationManagementViewModel:BaseViewModel
    {
        private string locationIDField;
        private string locationName;

        public string LocationIDField
        {
            get => locationIDField;
            set
            {
                locationIDField = value;
                CallPropertyChanged();
            }
        }

        public string LocationName
        {
            get => locationName;
            set
            {
                locationName = value;
                CallPropertyChanged();
            }
        }
    }
}
