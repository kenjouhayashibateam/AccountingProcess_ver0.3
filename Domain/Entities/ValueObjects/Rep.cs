namespace Domain.Entities.ValueObjects
{
    /// <summary>
    /// 担当者クラス
    /// </summary>
    public class Rep
    {
        /// <summary>
        /// ID
        /// </summary>
        public string RepID { get; set; }
        /// <summary>
        /// 担当者名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// パスワード
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 有効性
        /// </summary>
        public bool IsValidity { get; set; }

        public Rep(string repID, string name, string password, bool isValidity)
        {
            RepID = repID;
            Name = name;
            Password = password;
            IsValidity = isValidity;
        }
    }
}
