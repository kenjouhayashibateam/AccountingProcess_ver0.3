
namespace Domain.Entities.ValueObjects
{
    /// <summary>
    /// 貸方勘定クラス
    /// </summary>
    public class CreditAccount
    {
        /// <summary>
        /// ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 貸方勘定
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 有効性
        /// </summary>
        public bool IsValidity { get; set; }

        public CreditAccount(string iD, string account,bool isValidity)
        {
            ID = iD;
            Account = account;
            IsValidity = isValidity;
        }
    }
}
