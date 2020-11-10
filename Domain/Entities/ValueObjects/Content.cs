namespace Domain.Entities.ValueObjects
{
    /// <summary>
    /// 伝票内容クラス
    /// </summary>
    public class Content
    {
        public Content(int iD, AccountingSubject accountingSubject, int flatRate, string text, bool isValidity)
        {
            ID = iD;
            AccountingSubject = accountingSubject;
            FlatRate = flatRate;
            Text = text;
            IsValidity = isValidity;
        }

        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 勘定科目
        /// </summary>
        public AccountingSubject AccountingSubject { get; set; }
        /// <summary>
        /// 定額
        /// </summary>
        public int FlatRate { get; set; }
        /// <summary>
        /// 伝票内容
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// 有効性
        /// </summary>
        public bool IsValidity { get; set; }
    }
}
