using System;

namespace Domain.Entities
{
    /// <summary>
    /// お布施一覧のアイテムクラス
    /// </summary>
    public class Condolence
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 経理担当場所
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// 施主名
        /// </summary>
        public string OwnerName { get; set; }
        /// <summary>
        /// 僧侶の名前
        /// </summary>
        public string SoryoName { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// お布施
        /// </summary>
        public int Almsgiving { get; set; }
        /// <summary>
        /// 御車代
        /// </summary>
        public int CarTip { get; set; }
        /// <summary>
        /// 御膳料
        /// </summary>
        public int MealTip { get; set; }
        /// <summary>
        /// 御車代御膳料
        /// </summary>
        public int CarAndMealTip { get; set; }
        /// <summary>
        /// 懇志
        /// </summary>
        public int SocialGathering { get; set; }
        /// <summary>
        /// 合計金額
        /// </summary>
        public int TotalAmount => Almsgiving + CarTip + MealTip + CarAndMealTip + SocialGathering;
        /// <summary>
        /// 備考
        /// </summary>
        public string Note { get; set; }
        /// <summary>
        /// 日付
        /// </summary>
        public DateTime AccountActivityDate { get; set; }
        /// <summary>
        /// 窓口受付者
        /// </summary>
        public string CounterReceiver { get; set; }
        /// <summary>
        /// 郵送担当者
        /// </summary>
        public string MailRepresentative { get; set; }

        public Condolence
            (int iD, string location, string ownerName, string soryoName, string content, int almsgiving,
                int carTip, int mealTip, int carAndMealTip, int socialGethering, string note,
                DateTime accountActivityDate, string counterReceiver, string mailRepresentative)
        {
            ID = iD;
            Location = location;
            OwnerName = ownerName;
            SoryoName = soryoName;
            Content = content;
            Almsgiving = almsgiving;
            CarTip = carTip;
            MealTip = mealTip;
            CarAndMealTip = carAndMealTip;
            SocialGathering = socialGethering;
            Note = note;
            AccountActivityDate = accountActivityDate;
            CounterReceiver = counterReceiver;
            MailRepresentative = mailRepresentative;
        }
    }
}
