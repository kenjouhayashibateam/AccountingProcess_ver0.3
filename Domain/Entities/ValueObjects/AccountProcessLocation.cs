namespace Domain.Entities.ValueObjects
{
    /// <summary>
    /// 経理担当場所（シングルトン）
    /// </summary>
    public sealed class AccountProcessLocation
    {
        private static readonly AccountProcessLocation accountingLocation = new AccountProcessLocation();
        /// <summary>
        /// 担当場所
        /// </summary>
        public static string Location { get; set; }

        public static AccountProcessLocation GetInstance()
        {
            return accountingLocation;
        }

        public static void SetLocation(string location)
        {
            Location = location;
        }
    }
}
