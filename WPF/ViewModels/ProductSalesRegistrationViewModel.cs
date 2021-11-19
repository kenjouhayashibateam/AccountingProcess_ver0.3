using Domain.Entities.ValueObjects;
using Domain.Repositories;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.ViewModels.Datas;
using static Domain.Entities.Helpers.TextHelper;

namespace WPF.ViewModels
{
    /// <summary>
    /// 物販売上登録画面ViewModel
    /// </summary>
    public class ProductSalesRegistrationViewModel : JustRegistraterDataViewModel
    {
        private int lighterCount;
        /// <summary>
        /// ライター小計
        /// </summary>
        private int LighterTotalAmount
        {
            get => LighterTotalAmount;
            set
            {
                LighterTotalAmount = value;
                LighterTotalAmountValue = CommaDelimitedAmount(value);
            }
        }
        /// <summary>
        /// ライター単価
        /// </summary>
        private int LighterUnitPrice
        {
            get => LighterUnitPrice;
            set
            {
                LighterUnitPrice = value;
                LighterUnitPriceValue = CommaDelimitedAmount(value);
            }
        }
        private string lighterUnitPriceValue;
        private string lighterTotalAmountValue;

        public ProductSalesRegistrationViewModel(IDataBaseConnect dataBaseConnect) :
            base(dataBaseConnect)
        {
            SetUnitPrice();
        }
        public ProductSalesRegistrationViewModel() :
            base(DefaultInfrastructure.GetDefaultDataBaseConnect())
        { }
        /// <summary>
        /// 単価設定
        /// </summary>
        private void SetUnitPrice()
        {
            LighterUnitPrice = DataBaseConnect.ReferenceContent
                    ("ライター", string.Empty, string.Empty, true, true)[0].FlatRate;
        }
        /// <summary>
        /// ライター個数
        /// </summary>
        public int LighterCount
        {
            get => lighterCount;
            set
            {
                lighterCount = value;
                CallPropertyChanged();
                LighterTotalAmount = value * LighterUnitPrice;
            }
        }
        /// <summary>
        /// ライター単価表示
        /// </summary>
        public string LighterUnitPriceValue
        {
            get => lighterUnitPriceValue;
            set
            {
                lighterUnitPriceValue = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// ライター小計表示
        /// </summary>
        public string LighterTotalAmountValue
        {
            get => lighterTotalAmountValue;
            set
            {
                lighterTotalAmountValue = value;
                CallPropertyChanged();
            }
        }

        public override void ValidationProperty(string propertyName, object value)
        { }

        protected override void SetWindowDefaultTitle()
        { DefaultWindowTitle = "物販売上登録"; }
    }
}
