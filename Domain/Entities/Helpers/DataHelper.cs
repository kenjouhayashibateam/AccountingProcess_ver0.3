using System;
using System.Collections.Generic;
using System.Globalization;
using static Domain.Entities.Helpers.TextHelper;

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
        public static string ReturnManagementFee(double area)
        {
            double d = 1 + TaxRate;

            double e = Math.Truncate(area * Math.Pow(10, 1)) / Math.Pow(10, 1);

            int i = (int)(BasicManagementFee * e * d);

            i -= i % 10;

            i = i < Minimum ? Minimum : i;

            return AmountWithUnit(i);
        }
    }
}
