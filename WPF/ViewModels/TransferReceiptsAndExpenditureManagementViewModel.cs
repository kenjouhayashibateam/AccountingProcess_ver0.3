using Domain.Entities.ValueObjects;
using Domain.Repositories;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.ViewModels.Datas;

namespace WPF.ViewModels
{
    public class TransferReceiptsAndExpenditureManagementViewModel : BaseViewModel
    {
        #region Properties
        private bool isLimitCreditDept;
        private ObservableCollection<CreditDept> creditDepts;
        private CreditDept selectedCreditDept;
        #endregion

        public TransferReceiptsAndExpenditureManagementViewModel
            (IDataBaseConnect dataBaseConnect) : base(dataBaseConnect)
        {
        }
        public TransferReceiptsAndExpenditureManagementViewModel() :
            this(DefaultInfrastructure.GetDefaultDataBaseConnect()) { }
        /// <summary>
        /// 検索で貸方部門を限定するか
        /// </summary>
        public bool IsLimitCreditDept
        {
            get => isLimitCreditDept;
            set
            {
                isLimitCreditDept = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 貸方部門リスト
        /// </summary>
        public ObservableCollection<CreditDept> CreditDepts
        {
            get => creditDepts;
            set
            {
                creditDepts = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 選択された貸方部門
        /// </summary>
        public CreditDept SelectedCreditDept
        {
            get => selectedCreditDept;
            set
            {
                selectedCreditDept = value;
                CallPropertyChanged();
            }
        }

        public override void ValidationProperty(string propertyName, object value)
        {
            throw new NotImplementedException();
        }

        protected override void SetWindowDefaultTitle()
        { DefaultWindowTitle = $"出納管理 : {AccountingProcessLocation.Location}"; }
    }
}
