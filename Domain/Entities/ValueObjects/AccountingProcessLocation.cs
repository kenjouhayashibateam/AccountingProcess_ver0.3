namespace Domain.Entities.ValueObjects
{
    /// <summary>
    /// 経理担当場所（シングルトン）
    /// </summary>
    public sealed class AccountingProcessLocation
    {
        private static readonly AccountingProcessLocation accountingLocation = new AccountingProcessLocation();
        /// <summary>
        /// 担当場所
        /// </summary>
        public static string Location { get; set; }

        public static AccountingProcessLocation GetInstance()
        {
            return accountingLocation;
        }

        public static void SetLocation(string location)
        {
            Location = location;
        }
    }
}
