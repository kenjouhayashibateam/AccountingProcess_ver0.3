using System;
using System.Collections.Generic;
using System.Globalization;

namespace Domain.Entities.Helpers
{
    /// <summary>
    /// データ入力補助クラス
    /// </summary>
    public static class DataHelper
    {
        /// <summary>
        /// 消費税率
        /// </summary>
        private const double TaxRate = 0.1;
        /// <summary>
        /// 基本管理料
        /// </summary>
        private const int BasicManagementFee = 2800;
        /// <summary>
        /// 管理料下限
        /// </summary>
        private const int Minimum = (int)(6000 * (1 + TaxRate));
        /// <summary>
        /// 和暦のイニシャル
        /// </summary>
        internal static readonly Dictionary<int, string> EraInitials = new Dictionary<int, string>()
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
        /// 出納データ一覧のソートカテゴリ
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, string> ReceiptsAndExpenditureListSortColumns()
        {
            return new Dictionary<int, string>()
            {
                {0, "ID"},
                {1,"科目コード" },
                {2,"入出金日" },
                {3,"伝票出力日" }
            };
        }
        /// <summary>
        /// 管理料を計算して返します
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        public static int ReturnManagementFee(double area)
        {
            //消費税を設定
            double d = 1 + TaxRate;
            //墓地面積を小数点第一位で切り捨てる
            double e = Math.Truncate(area * Math.Pow(10, 1)) / Math.Pow(10, 1);
            //管理料計算
            int i = (int)(BasicManagementFee * e * d);
            //管理料の一の位を切り捨てる
            i -= i % 10;
            //管理料下限を下回る金額は下限金額にして返す
            return i < Minimum ? Minimum : i;
        }
    }
}
