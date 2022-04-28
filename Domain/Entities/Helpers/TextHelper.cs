using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Domain.Entities.Helpers
{
    /// <summary>
    /// 文字列の入力補助クラス
    /// </summary>
    public static class TextHelper
    {
        public const string SHUNJUEN = "春秋苑";
        public const string WIZECORE = "ワイズコア";
        /// <summary>
        /// 金額を3桁ごとのカンマ区切りにして、単位をつけて返します
        /// </summary>
        /// <param name="amount">金額</param>
        /// <returns>00,000,000 円</returns>
        public static string AmountWithUnit(int amount)
        {
            return $"{CommaDelimitedAmount(amount)}{Space}{Properties.Resources.Unit}";
        }
        /// <summary>
        /// 金額を3桁ごとのカンマ区切りにした文字列を返します
        /// </summary>
        /// <param name="amount">金額</param>
        /// <returns>00,000,000</returns>
        public static string CommaDelimitedAmount(int amount) { return $"{amount:N0}"; }
        /// <summary>
        /// 金額を3桁ごとのカンマ区切りにした文字列を返します。数字と認識できない場合はEmptyを返します
        /// </summary>
        /// <param name="amount">金額</param>
        /// <returns>00,000,000</returns>
        public static string CommaDelimitedAmount(string amount)
        {
            if (string.IsNullOrEmpty(amount)) { return string.Empty; }
            string s = amount.Replace(",", string.Empty);
            s = s.Replace(Properties.Resources.Unit, string.Empty);
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
            //***,*** 円に対応する。円を削除
            string s = amount.Replace(Properties.Resources.Unit, string.Empty);
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
        public static string GetHashValue(string value, string soltValue)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(soltValue)) { return string.Empty; }

            byte[] soltHash = Encoding.UTF8.GetBytes(soltValue);
            byte[] returnValue = KeyDerivation.Pbkdf2
                (value, soltHash, KeyDerivationPrf.HMACSHA256, 10000, 256 / 8);

            return string.Concat(returnValue.Select(b => $"{b:x2}"));
        }
        /// <summary>
        /// 元号のイニシャルを返します
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string GetEraInitial(DateTime dateTime)
        {
            JapaneseCalendar jc = new JapaneseCalendar();
            return DataHelper.EraInitials[jc.GetEra(dateTime)];
        }
        /// <summary>
        /// 春秋苑墓地番号を生成して返します
        /// </summary>
        /// <param name="graveNumberKu">区</param>
        /// <param name="graveNumberKuiki">区域</param>
        /// <param name="graveNumberGawa">側</param>
        /// <param name="graveNumberBan">番</param>
        /// <param name="graveNumberEdaban">枝番</param>
        /// <returns></returns>
        public static string AdjustmentGraveNumber
            (string graveNumberKu, string graveNumberKuiki, string graveNumberGawa, 
                string graveNumberBan, string graveNumberEdaban)
        {
            StringBuilder value = new StringBuilder();

            value.Append(ConvertKu());
            value.Append($"{ConvertNumber(graveNumberKuiki)}区");
            value.Append($"{ConvertNumber(graveNumberGawa)}側");
            value.Append($"{ConvertNumber(graveNumberBan)}");
            value.Append($"{ConvertNumber(graveNumberEdaban)}番");

            return value.ToString();
            
            string ConvertNumber(string number)
            {
                Regex regex = new Regex(@"^[0-9]+$");

                if(string.IsNullOrEmpty(number)) { return string.Empty; }
                //数字と認識できれば、0を抜いて返す。0010なら10
                if (regex.IsMatch(number)) { return ReturnString(); }

                int j = default;
                int k;
                for (int i = 0; i < number.Length - 1; i++)
                {
                    //数字ではない文字列の場所を特定する
                    if (!regex.IsMatch(number.Substring(i, 1)))
                    {
                        j = i;
                        break;
                    }
                    //0以外の数字の場所を特定する
                    k = number.IndexOf(number.Substring(i, 1));
                    if (k > 0)
                    {
                        j = i;
                        break;
                    }
                }
                //0を抜いた文字列を返す
                return number.Substring(j);

                string ReturnString()
                {
                    int i = int.Parse(number);
                    return i == 0 ? string.Empty : i.ToString();
                }
            }
            //区の一覧
            string ConvertKu()
            {
                return graveNumberKu switch
                {
                    "01" => "東",
                    "02" => "西",
                    "03" => "南",
                    "04" => "北",
                    "05" => "中",
                    "10" => "東特",
                    "11" => "二特",
                    "12" => "北特",
                    "20" => "御廟",
                    "東" => "01",
                    "西" => "02",
                    "南" => "03",
                    "北" => "04",
                    "中" => "05",
                    "東特" => "10",
                    "二特" => "11",
                    "北特" => "12",
                    "御廟" => "20",
                    _ => string.Empty,
                };
            }
        }
    }
}
