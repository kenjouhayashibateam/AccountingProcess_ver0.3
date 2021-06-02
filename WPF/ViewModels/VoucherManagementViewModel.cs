using Domain.Entities.ValueObjects;
using Domain.Repositories;
using Infrastructure;
using WPF.ViewModels.Datas;

namespace WPF.ViewModels
{
    /// <summary>
    /// 受納証管理画面ViewModel
    /// </summary>
    public class VoucherManagementViewModel : BaseViewModel
    {
        public VoucherManagementViewModel(IDataBaseConnect dataBaseConnect) : base(dataBaseConnect)
        {
        }
        public VoucherManagementViewModel() : this(DefaultInfrastructure.GetDefaultDataBaseConnect()) { }
        public override void SetRep(Rep rep)
        {
            throw new System.NotImplementedException();
        }

        public override void ValidationProperty(string propertyName, object value)
        {
            throw new System.NotImplementedException();
        }

        protected override void SetWindowDefaultTitle()
        {
            throw new System.NotImplementedException();
        }
    }
}
