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
    public class TransferReceiptsAndExpenditureManagementViewModel : BaseViewModel
    {
        public TransferReceiptsAndExpenditureManagementViewModel
            (IDataBaseConnect dataBaseConnect) : base(dataBaseConnect)
        {
        }
        public TransferReceiptsAndExpenditureManagementViewModel() :
            this(DefaultInfrastructure.GetDefaultDataBaseConnect()) { }
        public override void ValidationProperty(string propertyName, object value)
        {
            throw new NotImplementedException();
        }

        protected override void SetWindowDefaultTitle()
        {
            throw new NotImplementedException();
        }
    }
}
