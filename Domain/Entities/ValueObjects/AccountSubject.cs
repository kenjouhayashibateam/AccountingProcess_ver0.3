namespace Domain.Entities.ValueObjects
{
    /// <summary>
    /// 勘定科目クラス
    /// </summary>
    public class AccountSubject
    {
        /// <summary>
        /// ID
        /// </summary>
        public int AccountSubjectID;
        /// <summary>
        /// 勘定科目コード
        /// </summary>
        public int SubjectCode;
        /// <summary>
        /// 勘定科目
        /// </summary>
        public string Subject;

        public AccountSubject()
        {

        }
    }
}
