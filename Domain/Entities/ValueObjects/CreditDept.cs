
namespace Domain.Entities.ValueObjects
{
    /// <summary>
    /// 貸方部門クラス
    /// </summary>
    public class CreditDept
    {
        /// <summary>
        /// ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 部門
        /// </summary>
        public string Dept { get; set; }
        /// <summary>
        /// 有効性
        /// </summary>
        public bool IsValidity { get; set; }
        /// <summary>
        /// 春秋苑会計に掲載されるデータかの判定
        /// </summary>
        public bool IsShunjuenAccount { get; set; }

        public CreditDept(string iD, string dept,bool isValidity,bool isShunjuenAccount)
        {
            ID = iD;
            Dept = dept;
            IsValidity = isValidity;
            IsShunjuenAccount = isShunjuenAccount;
        }
    }
}
