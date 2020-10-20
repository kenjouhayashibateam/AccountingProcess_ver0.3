using System;

namespace Domain.Entities.ValueObjects
{
    /// <summary>
    /// 伝票認証クラス
    /// </summary>
    public class SlipApproval
    {
        /// <summary>
        /// 伝票ID
        /// </summary>
        public int SlipID;
        /// <summary>
        /// 担当者ID
        /// </summary>
        public int RepID;
        /// <summary>
        /// 認証日
        /// </summary>
        public DateTime ApprovalDate;
    }
}
