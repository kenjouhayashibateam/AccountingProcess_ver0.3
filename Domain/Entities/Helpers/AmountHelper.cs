
namespace Domain.Entities.Helpers
{
    /// <summary>
    /// 金額の入力補助クラス
    /// </summary>
    public class AmountHelper
    {
        /// <summary>
        /// 金額を3桁ごとのカンマ区切りにして、単位をつけて返します
        /// </summary>
        /// <param name="amount">金額</param>
        /// <returns></returns>
        public static string AmountWithUnit(int amount)
        {
            return $"{CommaDelimitedAmount(amount)} {Properties.Resources.Unit}";
        }
        /// <summary>
        /// 金額を3桁ごとのカンマ区切りにして返します
        /// </summary>
        /// <param name="amount">金額</param>
        /// <returns></returns>
        public static string CommaDelimitedAmount(int amount)
        {
            return $"{amount:N0}";
        }
    }
}
