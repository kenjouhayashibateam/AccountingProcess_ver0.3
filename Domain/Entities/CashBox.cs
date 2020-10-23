using Domain.Entities.Helpers;
using Domain.Entities.ValueObjects;
using System.Collections.Generic;
using static Domain.Entities.ValueObjects.MoneyCategory;
using static Domain.Entities.ValueObjects.MoneyCategory.Denomination;
using static System.Console;

namespace Domain.Entities
{
    public sealed class Cashbox
    {
        public static int BundleCount = 50;
        public  Dictionary<Denomination, MoneyCategory> MoneyCategorys = new Dictionary<Denomination, MoneyCategory>();
        public  OtherMoney[] OtherMoneys = new OtherMoney[8];
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

        public void SetOtherMoneyTitleDefault()
        {
            OtherMoneys[0].Title = "青蓮堂";
            OtherMoneys[1].Title = "香華売り場";
            OtherMoneys[2].Title = "春秋庵";
            OtherMoneys[3].Title = "石材工事部";
        }

        public static Cashbox GetInstance()
        {
            return cashbox;
        }

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

        public string GetTotalAmountWithUnit() => AmountHelper.AmountWithUnit(GetTotalAmount());
    }
}
