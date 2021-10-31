using Domain.Repositories;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.ViewModels.Datas;

namespace WPF.ViewModels
{
    /// <summary>
    /// 花売り出納データ登録画面ViewModel
    /// </summary>
    public class RegistrationFlowerSellReceiptsAndExpenditureViewModel : BaseViewModel
    {
        public RegistrationFlowerSellReceiptsAndExpenditureViewModel
            (IDataBaseConnect dataBaseConnect) : base(dataBaseConnect)
        {
        }
        public RegistrationFlowerSellReceiptsAndExpenditureViewModel() :
            this(DefaultInfrastructure.GetDefaultDataBaseConnect())
        { }

        public override void ValidationProperty(string propertyName, object value)
        {
            
        }

        protected override void SetWindowDefaultTitle() { DefaultWindowTitle = "花売りデータ登録"; }
    }
}
