using Domain.Entities.Helpers;

namespace Domain.Entities.ValueObjects
{
    /// <summary>
    /// 経理担当場所（シングルトン）
    /// </summary>
    public sealed class AccountingProcessLocation
    {
        private static readonly AccountingProcessLocation accountingLocation =
            new AccountingProcessLocation();
        /// <summary>
        /// 担当場所
        /// </summary>
        public static string Location { get; set; }
        /// <summary>
        /// 金庫計算、出納登録前の金額
        /// </summary>
        public static int OriginalTotalAmount { get; set; }
        /// <summary>
        /// 担当場所インスタンス
        /// </summary>
        /// <returns></returns>
        public static AccountingProcessLocation GetInstance() { return accountingLocation; }
        /// <summary>
        /// 担当場所を設定します
        /// </summary>
        /// <param name="location">場所</param>
        public static void SetLocation(string location) { Location = location; }
        /// <summary>
        /// 春秋苑の会計かﾜｲｽﾞｺｱの会計か
        /// </summary>
        public static bool IsAccountingGenreShunjuen { get; set; }
    }
}
