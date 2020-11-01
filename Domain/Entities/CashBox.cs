using Domain.Entities.Helpers;
using Domain.Entities.ValueObjects;
using System.Collections.Generic;
using static Domain.Entities.ValueObjects.MoneyCategory;
using static Domain.Entities.ValueObjects.MoneyCategory.Denomination;
using static System.Console;

namespace Domain.Entities
{
    /// <summary>
    /// 金庫クラス（シングルトン）
    /// </summary>
    public sealed class Cashbox
    {
        /// <summary>
        /// 金銭束の枚数
        /// </summary>
        public static int BundleCount { get=>50;}
        /// <summary>
        /// 金種リスト
        /// </summary>
        public Dictionary<Denomination, MoneyCategory> MoneyCategorys = new Dictionary<Denomination, MoneyCategory>();
        /// <summary>
        /// その他の金銭リスト
        /// </summary>
        public  OtherMoney[] OtherMoneys = new OtherMoney[8];
        /// <summary>
        /// メモリーに保持する金庫クラス
        /// </summary>
        private readonly static Cashbox cashbox = new Cashbox();

        private Cashbox()
        {
            MoneyCategorys.Add(OneYen, new MoneyCategory(1));
            MoneyCategorys.Add(FiveYen, new MoneyCategory(5));
            MoneyCategorys.Add(TenYen, new MoneyCategory(10));
            MoneyCategorys.Add(FiftyYen, new MoneyCategory(50));
            MoneyCategorys.Add(OneHundredYen, new MoneyCategory(100));
            MoneyCategorys.Add(FiveHundredYen, new MoneyCategory(500));
            MoneyCategorys.Add(OneThousandYen, new MoneyCategory(1000));
            MoneyCategorys.Add(FiveThousandYen, new MoneyCategory(5000));
            MoneyCategorys.Add(TenThousandYen, new MoneyCategory(10000));
            MoneyCategorys.Add(OneYenBundle, new MoneyCategory(1 * BundleCount));
            MoneyCategorys.Add(FiveYenBundle, new MoneyCategory(5 * BundleCount));
            MoneyCategorys.Add(TenYenBundle, new MoneyCategory(10 * BundleCount));
            MoneyCategorys.Add(FiftyYenBundle, new MoneyCategory(50 * BundleCount));
            MoneyCategorys.Add(OneHundredYenBundle, new MoneyCategory(100 * BundleCount));
            MoneyCategorys.Add(FiveHundredYenBundle, new MoneyCategory(500 * BundleCount));

            OtherMoneys[0] = new OtherMoney();
            OtherMoneys[1] = new OtherMoney();
            OtherMoneys[2] = new OtherMoney();
            OtherMoneys[3] = new OtherMoney();
            OtherMoneys[4] = new OtherMoney();
            OtherMoneys[5] = new OtherMoney();
            OtherMoneys[6] = new OtherMoney();
            OtherMoneys[7] = new OtherMoney();

            SetOtherMoneyTitleDefault();
        }
        /// <summary>
        /// その他釣り銭等欄にデフォルト値を入力します
        /// </summary>
        public void SetOtherMoneyTitleDefault()
        {
            OtherMoneys[0].Title = "青蓮堂";
            OtherMoneys[1].Title = "香華売り場";
            OtherMoneys[2].Title = "春秋庵";
            OtherMoneys[3].Title = "石材工事部";
        }
        /// <summary>
        /// 金庫クラスのインスタンスを取得します
        /// </summary>
        /// <returns></returns>
        public static Cashbox GetInstance()
        {
            return cashbox;
        }
        /// <summary>
        /// 送金額を取得します
        /// </summary>
        /// <returns></returns>
        public int GetTotalAmount()
        {
            int I = 0;

            foreach(OtherMoney om in OtherMoneys)
            {
                I += om.Amount;
            }

            foreach (KeyValuePair<Denomination, MoneyCategory> mc in MoneyCategorys)
            {
                I += mc.Value.Amount;
            }

            return I;
        }
        /// <summary>
        /// 総金額をカンマ区切り、単位をつけて返します
        /// </summary>
        /// <returns></returns>
        public string GetTotalAmountWithUnit() => AmountHelper.AmountWithUnit(GetTotalAmount());
    }
}
