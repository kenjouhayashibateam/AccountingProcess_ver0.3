using Domain.Entities;

namespace WPF.ViewModels
{
    public class SlipMangementViewModel : BaseViewModel
    {
        private readonly Cashbox Cashbox = Cashbox.GetInstance();
        private string cashBoxTotalAmount;

        public SlipMangementViewModel()
        {
            CashBoxTotalAmount = $"金庫の金額 : {Cashbox.GetTotalAmountWithUnit()}";
        }
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

        public override void ValidationProperty(string propertyName, object value)
        {
            
        }

        protected override string SetWindowDefaultTitle()
        {
            DefaultWindowTitle = "伝票管理";
            return DefaultWindowTitle;
        }
    }
}
