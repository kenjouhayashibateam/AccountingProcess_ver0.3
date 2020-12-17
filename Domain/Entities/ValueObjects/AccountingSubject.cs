namespace Domain.Entities.ValueObjects
{
    /// <summary>
    /// 勘定科目クラス
    /// </summary>
    public class AccountingSubject
    {
        /// <summary>
        /// ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 勘定科目コード
        /// </summary>
        public string SubjectCode { get; set; }
        /// <summary>
        /// 勘定科目
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// 有効性
        /// </summary>
        public bool IsValidity { get; set; }

        public AccountingSubject(string id,string subjectCode,string subject,bool isValidity)
        {
            ID = id;
            SubjectCode = subjectCode;
            Subject = subject;
            IsValidity = isValidity;
        }

        public override int GetHashCode() => base.GetHashCode();

        public override bool Equals(object obj)
        {
            if (obj == null || !this.GetType().Equals(obj.GetType())) return false;
            else
            {
                AccountingSubject accountingSubject = (AccountingSubject)obj;
                return this.ID == accountingSubject.ID;
            }
        }
    }
}
