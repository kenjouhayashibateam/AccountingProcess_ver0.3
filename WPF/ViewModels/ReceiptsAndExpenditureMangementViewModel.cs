using Domain.Entities;
using Domain.Entities.ValueObjects;
using Domain.Repositories;
using Infrastructure;

namespace WPF.ViewModels
{
    public class ReceiptsAndExpenditureMangementViewModel : BaseViewModel
    {
        #region Properties
        private readonly Cashbox Cashbox = Cashbox.GetInstance();
        private string cashBoxTotalAmount;
        private bool isRegistrationCheck;
        private bool isUpdateCheck;
        private bool isPaymentCheck;
        private string isDepositAndWithdrawalContetnt;
        private readonly IDataBaseConnect DataBaseConnect;

        #endregion

        public ReceiptsAndExpenditureMangementViewModel(IDataBaseConnect dataBaseConnect)
        {
            DataBaseConnect = dataBaseConnect;
            CashBoxTotalAmount = $"金庫の金額 : {Cashbox.GetTotalAmountWithUnit()}";
        }

        public ReceiptsAndExpenditureMangementViewModel() : this(DefaultInfrastructure.GetDefaultDataBaseConnect()) { }

        /// <summary>
        /// 金庫の総計金額
        /// </summary>
        public string CashBoxTotalAmount
        {
            get => cashBoxTotalAmount;
            set
            {
                cashBoxTotalAmount = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// データ操作　登録チェック
        /// </summary>
        public bool IsRegistrationCheck
        {
            get => isRegistrationCheck;
            set
            {
                isRegistrationCheck = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// データ操作　更新チェック
        /// </summary>
        public bool IsUpdateCheck
        {
            get => isUpdateCheck;
            set
            {
                isUpdateCheck = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 入出金チェック　入金がTrue
        /// </summary>
        public bool IsPaymentCheck
        {
            get => isPaymentCheck;
            set
            {
                isPaymentCheck = value;
                if (value) DepositAndWithdrawalContetnt = "入金";
                else DepositAndWithdrawalContetnt = "出金";

                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 入出金のトグルコンテント
        /// </summary>
        public string DepositAndWithdrawalContetnt
        {
            get => isDepositAndWithdrawalContetnt;
            set
            {
                isDepositAndWithdrawalContetnt = value;
                CallPropertyChanged();
            }
        }

        public override void ValidationProperty(string propertyName, object value)
        {
            
        }

        protected override string SetWindowDefaultTitle()
        {
            DefaultWindowTitle = $"出納管理 : {AccountingProcessLocation.Location}";
            return DefaultWindowTitle;
        }
    }
}
