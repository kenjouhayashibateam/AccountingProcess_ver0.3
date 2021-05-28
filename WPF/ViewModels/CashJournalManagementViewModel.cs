using Domain.Entities.ValueObjects;
using Domain.Repositories;
using Infrastructure;
using WPF.ViewModels.Datas;

namespace WPF.ViewModels
{
    public class CashJournalManagementViewModel : BaseViewModel
    {
        public CashJournalManagementViewModel(IDataBaseConnect dataBaseConnect) : base(dataBaseConnect) { }
        public CashJournalManagementViewModel() : this(DefaultInfrastructure.GetDefaultDataBaseConnect()) { }

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
