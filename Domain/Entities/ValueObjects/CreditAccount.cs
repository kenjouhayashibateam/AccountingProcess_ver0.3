
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
        /// <summary>
        /// 春秋苑会計に掲載されるデータかの判定
        /// </summary>
        public bool IsShunjuenAccount { get; set; }

        public CreditAccount(string iD, string account,bool isValidity,bool isShunjuenAccount)
        {
            ID = iD;
            Account = account;
            IsValidity = isValidity;
            IsShunjuenAccount = isShunjuenAccount;
        }
    }
}
