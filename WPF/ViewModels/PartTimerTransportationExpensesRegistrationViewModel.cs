using Domain.Entities.Helpers;
using Domain.Entities.ValueObjects;
using Domain.Repositories;
using Infrastructure;
using System;

namespace WPF.ViewModels
{
    /// <summary>
    /// パート交通費データ登録画面ViewModel
    /// </summary>
    public class PartTimerTransportationExpensesRegistrationViewModel : BaseViewModel
    {
        public PartTimerTransportationExpensesRegistrationViewModel(IDataBaseConnect dataBaseConnect) : base(dataBaseConnect)
        {
        }
        public PartTimerTransportationExpensesRegistrationViewModel() : this(DefaultInfrastructure.GetDefaultDataBaseConnect()) { }

        public override void SetRep(Rep rep)
        {
            {
                if (rep == null || string.IsNullOrEmpty(rep.Name)) WindowTitle = DefaultWindowTitle;
                else
                {
                    IsAdminPermisson = rep.IsAdminPermisson;
                    WindowTitle =
                        $"{DefaultWindowTitle}（ログイン : {TextHelper.GetFirstName(rep.Name)}）";
                }
            }
        }

        public override void ValidationProperty(string propertyName, object value)
        {
            throw new NotImplementedException();
        }

        protected override void SetWindowDefaultTitle() => DefaultWindowTitle = "パート交通費データ登録";
    }
}
