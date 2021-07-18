using static Domain.Entities.Helpers.TextHelper;

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
        public string ID { get; set; }
        /// <summary>
        /// 担当者名
        /// </summary>
        public string Name { get; set; }
        public string FirstName => GetFirstName(Name);
        /// <summary>
        /// パスワード
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 有効性
        /// </summary>
        public bool IsValidity { get; set; }
        /// <summary>
        /// アドミン権限
        /// </summary>
        public bool IsAdminPermisson { get; set; }

        public Rep(string repID, string name, string password, bool isValidity, bool isAdminPermisson)
        {
            ID = repID;
            Name = name;
            Password = password;
            IsValidity = isValidity;
            IsAdminPermisson = isAdminPermisson;
        }
    }
}
