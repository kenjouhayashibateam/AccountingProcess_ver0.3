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
    /// 花売り出納データ登録画面ViewModel
    /// </summary>
    public class RegistrationFlowerSellReceiptsAndExpenditureViewModel :
        JustRegistraterDataViewModel
    {
        #region Properties
        #region strings
        private string cemeteryFlower;
        private string highFlower;
        private string specialFlower;
        private string orderFlower;
        private string sakaki;
        private string anisatum;
        private string incenseStickVertical;
        private string incenseStickHorizontal;
        private string basketFlower;
        private string cemeteryFlowerUnitPriceValue;
        private string highFlowerUnitPriceValue;
        private string specialFlowerUnitPriceValue;
        private string orderFlowerUnitPriceValue;
        private string sakakiUnitPriceValue;
        private string anisatumUnitPriceValue;
        private string incenseStickVerticalUnitPriceValue;
        private string incenseStickHorizontalUnitPriceValue;
        private string basketFlowerUnitPriceValue;
        private string totalAmountValue;
        #endregion
        #region ints
        /// <summary>
        /// 墓地花単価
        /// </summary>
        private int CemeteryFlowerUnitPrice
        {
            get => CemeteryFlowerUnitPrice;
            set
            {
                CemeteryFlowerUnitPrice = value;
                CemeteryFlowerUnitPriceValue = CommaDelimitedAmount(value);
            }
        }
        /// <summary>
        /// 上花単価
        /// </summary>
        private int HighFlowerUnitPrice
        {
            get => HighFlowerUnitPrice;
            set
            {
                HighFlowerUnitPrice = value;
                HighFlowerUnitPriceValue = CommaDelimitedAmount(value);
            }
        }
        /// <summary>
        /// 特花単価
        /// </summary>
        private int SpecialFlowerUnitPrice
        {
            get => SpecialFlowerUnitPrice;
            set
            {
                SpecialFlowerUnitPrice = value;
                SpecialFlowerUnitPriceValue = CommaDelimitedAmount(value);
            }
        }
        /// <summary>
        /// その他の花単価
        /// </summary>
        private int OrderFlowerUnitPrice
        {
            get => OrderFlowerUnitPrice;
            set
            {
                OrderFlowerUnitPrice = value;
                OrderFlowerUnitPriceValue = CommaDelimitedAmount(value);
            }
        }
        /// <summary>
        /// 榊単価
        /// </summary>
        private int SakakiUnitPrice
        {
            get => SakakiUnitPrice;
            set
            {
                SakakiUnitPrice = value;
                SakakiUnitPriceValue = CommaDelimitedAmount(value);
            }
        }
        /// <summary>
        /// 榊単価
        /// </summary>
        private int AnisatumUnitPrice
        {
            get => AnisatumUnitPrice;
            set
            {
                AnisatumUnitPrice = value;
                AnisatumUnitPriceValue = CommaDelimitedAmount(value);
            }
        }
        /// <summary>
        /// 線香縦の単価
        /// </summary>
        private int IncenseStickVerticalUnitPrice
        {
            get => IncenseStickVerticalUnitPrice;
            set
            {
                IncenseStickVerticalUnitPrice = value;
                IncenseStickVerticalUnitPriceValue = CommaDelimitedAmount(value);
            }
        }
        /// <summary>
        /// 線香横の単価
        /// </summary>
        private int IncenseStickHorizontalUnitPrice
        {
            get => IncenseStickHorizontalUnitPrice;
            set
            {
                IncenseStickHorizontalUnitPrice = value;
                IncenseStickHorizontalUnitPriceValue = CommaDelimitedAmount(value);
            }
        }
        /// <summary>
        /// 籠花単価
        /// </summary>
        private int BasketFlowerUnitPrice
        {
            get => BasketFlowerUnitPrice;
            set
            {
                BasketFlowerUnitPrice = value;
                BasketFlowerUnitPriceValue = CommaDelimitedAmount(value);
            }
        }
        private int cemeteryFlowerCount;
        private int highFlowerCount;
        private int specialFlowerCount;
        private int orderFlowerCount;
        private int sakakiCount;
        private int anisatumCount;
        private int incenseStickVerticalCount;
        private int incenseStickHorizontalCount;
        private int ticketCount;
        /// <summary>
        /// 総計
        /// </summary>
        private int TotalAmount
        {
            get => TotalAmount;
            set
            {
                TotalAmount = value;
                TotalAmountValue = CommaDelimitedAmount(value);
            }
        }
        /// <summary>
        /// 墓地花小計
        /// </summary>
        private int CemeteryFlowerTotalAmount
        {
            get => CemeteryFlowerTotalAmount;
            set
            {
                CemeteryFlowerTotalAmount = value;
                TotalAmountCalculation();
            }
        }
        /// <summary>
        /// 上花小計
        /// </summary>
        private int HighFlowerTotalAmount
        {
            get => HighFlowerTotalAmount;
            set
            {
                HighFlowerTotalAmount = value;
                TotalAmountCalculation();
            }
        }
        /// <summary>
        /// 特花小計
        /// </summary>
        private int SpecialFlowerTotalAmount
        {
            get => SpecialFlowerTotalAmount;
            set
            {
                SpecialFlowerTotalAmount = value;
                TotalAmountCalculation();
            }
        }
        /// <summary>
        /// 榊小計
        /// </summary>
        private int SakakiTotalAmount
        {
            get => SakakiTotalAmount;
            set
            {
                SakakiTotalAmount = value;
                TotalAmountCalculation();
            }
        }
        /// <summary>
        /// 樒小計
        /// </summary>
        private int AnisatumTotalAmount
        {
            get => AnisatumTotalAmount;
            set
            {
                AnisatumTotalAmount = value;
                TotalAmountCalculation();
            }
        }
        /// <summary>
        /// 線香縦小計
        /// </summary>
        private int IncenseStickVerticalTotalAmount
        {
            get => IncenseStickVerticalTotalAmount;
            set
            {
                IncenseStickVerticalTotalAmount = value;
                TotalAmountCalculation();
            }
        }
        /// <summary>
        /// 線香横小計
        /// </summary>
        private int IncenseStickHorizontalTotalAmount
        {
            get => IncenseStickHorizontalTotalAmount;
            set
            {
                IncenseStickHorizontalTotalAmount = value;
                TotalAmountCalculation();
            }
        }
        /// <summary>
        /// その他の花の小計
        /// </summary>
        private int OrderFlowerTotalAmount
        {
            get => OrderFlowerTotalAmount;
            set
            {
                OrderFlowerTotalAmount = value;
                TotalAmountCalculation();
            }
        }
        #endregion
        #endregion

        public RegistrationFlowerSellReceiptsAndExpenditureViewModel
            (IDataBaseConnect dataBaseConnect) : base(dataBaseConnect)
        {
        }
        public RegistrationFlowerSellReceiptsAndExpenditureViewModel() :
            this(DefaultInfrastructure.GetDefaultDataBaseConnect())
        { }
        /// <summary>
        /// 総計を計算します
        /// </summary>
        private void TotalAmountCalculation()
        {
            TotalAmount = CemeteryFlowerTotalAmount + HighFlowerTotalAmount +
                SpecialFlowerTotalAmount + SakakiTotalAmount + AnisatumTotalAmount +
                IncenseStickVerticalTotalAmount + IncenseStickHorizontalTotalAmount +
                OrderFlowerTotalAmount;
        }
        /// <summary>
        /// 墓地花
        /// </summary>
        public string CemeteryFlower
        {
            get => cemeteryFlower;
            set
            {
                cemeteryFlower = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 上花
        /// </summary>
        public string HighFlower
        {
            get => highFlower;
            set
            {
                highFlower = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 特花
        /// </summary>
        public string SpecialFlower
        {
            get => specialFlower;
            set
            {
                specialFlower = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// その他の花
        /// </summary>
        public string OrderFlower
        {
            get => orderFlower;
            set
            {
                orderFlower = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 榊
        /// </summary>
        public string Sakaki
        {
            get => sakaki;
            set
            {
                sakaki = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 樒
        /// </summary>
        public string Anisatum
        {
            get => anisatum;
            set
            {
                anisatum = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 線香縦
        /// </summary>
        public string IncenseStickVertical
        {
            get => incenseStickVertical;
            set
            {
                incenseStickVertical = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 線香横
        /// </summary>
        public string IncenseStickHorizontal
        {
            get => incenseStickHorizontal;
            set
            {
                incenseStickHorizontal = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 籠花
        /// </summary>
        public string BasketFlower
        {
            get => basketFlower;
            set
            {
                basketFlower = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 墓地花単価表示
        /// </summary>
        public string CemeteryFlowerUnitPriceValue
        {
            get => cemeteryFlowerUnitPriceValue;
            set
            {
                cemeteryFlowerUnitPriceValue = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 上花単価表示
        /// </summary>
        public string HighFlowerUnitPriceValue
        {
            get => highFlowerUnitPriceValue;
            set
            {
                highFlowerUnitPriceValue = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 特花単価表示
        /// </summary>
        public string SpecialFlowerUnitPriceValue
        {
            get => specialFlowerUnitPriceValue;
            set
            {
                specialFlowerUnitPriceValue = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// その他の花の単価表示
        /// </summary>
        public string OrderFlowerUnitPriceValue
        {
            get => orderFlowerUnitPriceValue;
            set
            {
                orderFlowerUnitPriceValue = value;
                OrderFlowerUnitPrice = IntAmount(value);
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 榊単価表示
        /// </summary>
        public string SakakiUnitPriceValue
        {
            get => sakakiUnitPriceValue;
            set
            {
                sakakiUnitPriceValue = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 樒単価表示
        /// </summary>
        public string AnisatumUnitPriceValue
        {
            get => anisatumUnitPriceValue;
            set
            {
                anisatumUnitPriceValue = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 線香縦単価表示
        /// </summary>
        public string IncenseStickVerticalUnitPriceValue
        {
            get => incenseStickVerticalUnitPriceValue;
            set
            {
                incenseStickVerticalUnitPriceValue = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 線香横単価表示
        /// </summary>
        public string IncenseStickHorizontalUnitPriceValue
        {
            get => incenseStickHorizontalUnitPriceValue;
            set
            {
                incenseStickHorizontalUnitPriceValue = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 籠花単価表示
        /// </summary>
        public string BasketFlowerUnitPriceValue
        {
            get => basketFlowerUnitPriceValue;
            set
            {
                basketFlowerUnitPriceValue = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 墓地花数量
        /// </summary>
        public int CemeteryFlowerCount
        {
            get => cemeteryFlowerCount;
            set
            {
                cemeteryFlowerCount = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 上花数量
        /// </summary>
        public int HighFlowerCount
        {
            get => highFlowerCount;
            set
            {
                highFlowerCount = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 特花数量
        /// </summary>
        public int SpecialFlowerCount
        {
            get => specialFlowerCount;
            set
            {
                specialFlowerCount = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// その他の花の数量
        /// </summary>
        public int OrderFlowerCount
        {
            get => orderFlowerCount;
            set
            {
                orderFlowerCount = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 榊数量
        /// </summary>
        public int SakakiCount
        {
            get => sakakiCount;
            set
            {
                sakakiCount = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 樒数量
        /// </summary>
        public int AnisatumCount
        {
            get => anisatumCount;
            set
            {
                anisatumCount = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 線香縦数量
        /// </summary>
        public int IncenseStickVerticalCount 
        { 
            get => incenseStickVerticalCount; 
            set 
            {
                incenseStickVerticalCount = value;
                CallPropertyChanged();
            } 
        }
        /// <summary>
        /// 線香横数量
        /// </summary>
        public int IncenseStickHorizontalCount 
        { 
            get => incenseStickHorizontalCount; 
            set 
            {
                incenseStickHorizontalCount = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// チケット数量
        /// </summary>
        public int TicketCount
        {
            get => ticketCount;
            set
            {
                ticketCount = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 総計表示
        /// </summary>
        public string TotalAmountValue
        {
            get => totalAmountValue;
            set
            {
                totalAmountValue = value;
                CallPropertyChanged();
            }
        }

        public override void ValidationProperty(string propertyName, object value)
        {
            
        }

        protected override void SetWindowDefaultTitle() { DefaultWindowTitle = "花売りデータ登録"; }
    }
}
