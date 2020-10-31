using Domain.Entities.Helpers;

namespace Domain.Entities.ValueObjects
{
    /// <summary>
    /// その他の金銭クラス
    /// </summary>
    public class OtherMoney
    {
        /// <summary>
        /// 金銭の題名
        /// </summary>
        public string Title;
        /// <summary>
        /// 金額
        /// </summary>
        public int Amount;
        /// <summary>
        /// 金額をカンマ区切り、単位をつけて返します
        /// </summary>
        /// <returns></returns>
        public string AmountWithUnit()
        {
            if(Amount<1)
            {
                return string.Empty;
            }
            else
            {
                return AmountHelper.AmountWithUnit(Amount);
            }
        }
    }
}
