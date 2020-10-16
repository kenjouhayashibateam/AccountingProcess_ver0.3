using Domain.Entities.Helpers;
using Domain.Entities.ValueObjects;
using System.Collections.Generic;
using static Domain.Entities.ValueObjects.MoneyCategory;

namespace Domain.Entities
{
    public class Cashbox
    {
        public static int BundleCount = 50;
        public Dictionary<Denomination, MoneyCategory> MoneyCategorys = new Dictionary<Denomination, MoneyCategory>();
        public OtherMoney[] OtherMoneys = new OtherMoney[8];

        public Cashbox()
        {
            MoneyCategorys.Add(Denomination.OneYen, new MoneyCategory(1));
            MoneyCategorys.Add(Denomination.FiveYen, new MoneyCategory(5));
            MoneyCategorys.Add(Denomination.TenYen, new MoneyCategory(10));
            MoneyCategorys.Add(Denomination.FiftyYen, new MoneyCategory(50));
            MoneyCategorys.Add(Denomination.OneHundredYen, new MoneyCategory(100));
            MoneyCategorys.Add(Denomination.FiveHundredYen, new MoneyCategory(500));
            MoneyCategorys.Add(Denomination.OneThousandYen, new MoneyCategory(1000));
            MoneyCategorys.Add(Denomination.FiveThousandYen, new MoneyCategory(5000));
            MoneyCategorys.Add(Denomination.TenThousandYen, new MoneyCategory(10000));
            MoneyCategorys.Add(Denomination.OneYenBundle, new MoneyCategory(1 * BundleCount));
            MoneyCategorys.Add(Denomination.FiveYenBundle, new MoneyCategory(5 * BundleCount));
            MoneyCategorys.Add(Denomination.TenYenBundle, new MoneyCategory(10 * BundleCount));
            MoneyCategorys.Add(Denomination.FiftyYenBundle, new MoneyCategory(50 * BundleCount));
            MoneyCategorys.Add(Denomination.OneHundredYenBundle, new MoneyCategory(100 * BundleCount));
            MoneyCategorys.Add(Denomination.FiveHundredYenBundle, new MoneyCategory(500 * BundleCount));

            OtherMoneys[0] = new OtherMoney();
            OtherMoneys[1] = new OtherMoney();
            OtherMoneys[2] = new OtherMoney();
            OtherMoneys[3] = new OtherMoney();
            OtherMoneys[4] = new OtherMoney();
            OtherMoneys[5] = new OtherMoney();
            OtherMoneys[6] = new OtherMoney();
            OtherMoneys[7] = new OtherMoney();
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
