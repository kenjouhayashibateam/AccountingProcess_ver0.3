using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace Domain.Entities.Helpers
{
    /// <summary>
    /// 文字列の入力補助クラス
    /// </summary>
    public class TextHelper
    {
        /// <summary>
        /// 和暦のイニシャル
        /// </summary>
        private static readonly Dictionary<int, string> EraInitials = new Dictionary<int, string>()
        {
            {1,"M" },
            {2,"T" },
            {3,"S" },
            {4,"H" },
            {5,"R" }
        };
        /// <summary>
        /// 和暦Culture
        /// </summary>
        public static CultureInfo JapanCulture
        {
            get
            {
                CultureInfo value = new CultureInfo("ja-JP", true);//和暦データを代入
                value.DateTimeFormat.Calendar = new JapaneseCalendar();//データのカレンダーを和暦にする
                return value;
            }
        }
        /// <summary>
        /// 今年度初日
        /// </summary>
        public static DateTime CurrentFiscalYearFirstDate
        {
            get
            {
                int year = DateTime.Today.Year;
                if (DateTime.Today.Month < 4) { year--; }//1月から3月までは前年の年度になるので年を1引く
                return new DateTime(year, 4, 1);//年度の4月1日を返す
            }
        }
        /// <summary>
        /// nullに出来ない場合のdefaultの日付
        /// </summary>
        public static DateTime DefaultDate = DateTime.Parse("1900/01/01");
        /// <summary>
        /// 金額を3桁ごとのカンマ区切りにして、単位をつけて返します
        /// </summary>
        /// <param name="amount">金額</param>
        /// <returns>00,000,000 円</returns>
        public static string AmountWithUnit(int amount)
        {
            return $"{CommaDelimitedAmount(amount)} {Properties.Resources.Unit}";
        }
        /// <summary>
        /// 金額を3桁ごとのカンマ区切りにした文字列を返します
        /// </summary>
        /// <param name="amount">金額</param>
        /// <returns>00,000,000</returns>
        public static string CommaDelimitedAmount(int amount)
        {
            return $"{amount:N0}";
        }
        /// <summary>
        /// 金額を3桁ごとのカンマ区切りにした文字列を返します。数字と認識できない場合はEmptyを返します
        /// </summary>
        /// <param name="amount">金額</param>
        /// <returns>00,000,000</returns>
        public static string CommaDelimitedAmount(string amount)
        {
            string s = amount.Replace(",", string.Empty);
            return int.TryParse(s, out int i) ? CommaDelimitedAmount(i) : string.Empty;
        }
        /// <summary>
        /// 金額をIntで返します。単位とカンマを削除しても数字と認識できない場合は0を返します
        /// </summary>
        /// <param name="amount">金額</param>
        /// <returns>000000</returns>
        public static int IntAmount(string amount)
        {
            if (string.IsNullOrEmpty(amount)) { return 0; }
            string s = amount.Replace(Properties.Resources.Unit, string.Empty);//***,*** 円に対応する。円を削除
            s = s.Replace(",", string.Empty);
            return CommaDelimitedAmount(s) == string.Empty ? 0 : int.Parse(s);
        }
        /// <summary>
        /// 名前の最初のスペースまでの文字列を返します
        /// </summary>
        /// <param name="name">名前</param>
        /// <returns>苗字</returns>
        public static string GetFirstName(string name)
        {
            //空白でSplitして、最初の要素を返す
            string[] nameArray = name.Split(' ');

            return nameArray[0];
        }
        /// <summary>
        /// 半角スペース
        /// </summary>
        public const string Space = " ";
        /// <summary>
        /// 全角スペース
        /// </summary>
        public const string SpaceF = "　";
        /// <summary>
        /// ハッシュ値を返します
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetHashValue(string value)
        {
            byte[] soltValue = Encoding.UTF8.GetBytes(value);
            byte[] valueHash = new SHA256CryptoServiceProvider().ComputeHash(soltValue);

            StringBuilder returnValue = new StringBuilder();

            foreach (byte b in valueHash)
            {
                _ = returnValue.Append(b.ToString("x2"));
            }

            return returnValue.ToString();
        }
        public static string GetEraInitial(DateTime dateTime)
        {
            JapaneseCalendar jc = new JapaneseCalendar();
            return EraInitials[jc.GetEra(dateTime)];
        }
    }
}
