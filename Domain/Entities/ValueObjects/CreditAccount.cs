
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
        public int ID;
        /// <summary>
        /// 貸方勘定
        /// </summary>
        public string Account;

        public CreditAccount(int iD, string account)
        {
            ID = iD;
            Account = account;
        }
    }
}
