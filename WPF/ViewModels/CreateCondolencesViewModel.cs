using Domain.Entities;
using Domain.Entities.ValueObjects;
using Domain.Repositories;
using Infrastructure;
using System;
using System.Collections.ObjectModel;
using static Domain.Entities.Helpers.TextHelper;

namespace WPF.ViewModels
{
    /// <summary>
    /// お布施一覧作成ViewModel
    /// </summary>
    public class CreateCondolencesViewModel : BaseViewModel,ICondolenceObserver
    {
        private ObservableCollection<Condolence> condolences;

        public CreateCondolencesViewModel(IDataBaseConnect dataBaseConnect) : base(dataBaseConnect)
        {
        }
        public CreateCondolencesViewModel() : this(DefaultInfrastructure.GetDefaultDataBaseConnect()) { }
        /// <summary>
        /// お布施一覧のアイテムリスト
        /// </summary>
        public ObservableCollection<Condolence> Condolences
        {
            get => condolences;
            set
            {
                condolences = value;
                CallPropertyChanged();
            }
        }

        public override void SetRep(Rep rep)
        {
            if (rep == null || string.IsNullOrEmpty(rep.Name)) WindowTitle = DefaultWindowTitle;
            else
            {
                IsAdminPermisson = rep.IsAdminPermisson;
                WindowTitle = $"{DefaultWindowTitle}（ログイン : {GetFirstName(rep.Name)}）";
            }
        }

        public override void ValidationProperty(string propertyName, object value)
        {
            throw new System.NotImplementedException();
        }

        protected override void SetWindowDefaultTitle() => 
            DefaultWindowTitle = $"お布施一覧データ出力 : {AccountingProcessLocation.Location}";

        public void CondolenceNotify()
        {
            throw new NotImplementedException();
        }
    }
}
