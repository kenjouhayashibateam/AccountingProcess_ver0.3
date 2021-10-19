using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.Entities.Helpers.TextHelper;

namespace Infrastructure.ExcelOutputData
{
    /// <summary>
    /// ワイズコア収支日報出力
    /// </summary>
    internal class WizeCoreBalanceFinalAccountOutput : OutputSingleSheetData
    {
        /// <summary>
        /// 蓮華庵前日繰越
        /// </summary>
        private readonly string RengeanPreviousDayFinalAccountWithUnit;
        /// <summary>
        /// 蓮華庵入金
        /// </summary>
        private readonly string RengeanPaymentWithUnit;
        /// <summary>
        /// 蓮華庵出金
        /// </summary>
        private readonly string RengeanWithdrawalWithUnit;
        /// <summary>
        /// 蓮華庵銀行振替
        /// </summary>
        private readonly string RengeanTranceferWithUnit;
        /// <summary>
        /// 春秋庵前日繰越
        /// </summary>
        private readonly string ShunjuanPreviousDayFinalAccountWithUnit;
        /// <summary>
        /// 春秋庵入金
        /// </summary>
        private readonly string ShunjuanPaymentWithUnit;
        /// <summary>
        /// 春秋庵出金
        /// </summary>
        private readonly string ShunjuanWithdrawalWithUnit;
        /// <summary>
        /// 春秋庵銀行振替
        /// </summary>
        private readonly string ShunjuanTranceferWithUnit;
        /// <summary>
        /// 香華前日繰越
        /// </summary>
        private readonly string KougePreviousDayFinalAccountWithUnit;
        /// <summary>
        /// 香華入金
        /// </summary>
        private readonly string KougePaymentWithUnit;
        /// <summary>
        /// 香華出金
        /// </summary>
        private readonly string KougeWithdrawalWithUnit;
        /// <summary>
        /// 香華銀行振替
        /// </summary>
        private readonly string KougeTranceferWithUnit;
        /// <summary>
        /// 横浜銀行残高
        /// </summary>
        private readonly string YokohamaBankAmountWithUnit;
        /// <summary>
        /// 春秋苑仮払金
        /// </summary>
        private readonly string ShunjuenAmountWithUnit;
        /// <param name="rengeanPreviousDayFinalAccountWithUnit">蓮華庵前日繰越</param>
        /// <param name="rengeanPaymentWithUnit">蓮華庵入金</param>
        /// <param name="rengeanWithdrawalWithUnit">蓮華庵出金</param>
        /// <param name="rengeanTranceferWithUnit">蓮華庵銀行振替</param>
        /// <param name="shunjuanPreviousDayFinalAccountWithUnit">春秋庵前日繰越</param>
        /// <param name="shunjuanPaymentWithUnit">春秋庵入金</param>
        /// <param name="shunjuanWithdrawalWithUnit">春秋庵出金</param>
        /// <param name="shunjuanTranceferWithUnit">春秋庵銀行振替</param>
        /// <param name="kougePreviousDayFinalAccountWithUnit">香華前日繰越</param>
        /// <param name="kougePaymentWithUnit">香華入金</param>
        /// <param name="kougeWithdrawalWithUnit">香華出金</param>
        /// <param name="kougeTranceferWithUnit">香華銀行振替</param>
        /// <param name="yokohamaBankAmountWithUnit">横浜銀行残高</param>
        /// <param name="shunjuenAmountWithUnit">春秋苑仮払金</param>
        public WizeCoreBalanceFinalAccountOutput
            (string rengeanPreviousDayFinalAccountWithUnit, string rengeanPaymentWithUnit,
                string rengeanWithdrawalWithUnit, string rengeanTranceferWithUnit,
                string shunjuanPreviousDayFinalAccountWithUnit, string shunjuanPaymentWithUnit,
                string shunjuanWithdrawalWithUnit, string shunjuanTranceferWithUnit,
                string kougePreviousDayFinalAccountWithUnit, string kougePaymentWithUnit,
                string kougeWithdrawalWithUnit, string kougeTranceferWithUnit,
                string yokohamaBankAmountWithUnit, string shunjuenAmountWithUnit)
        {
            RengeanPreviousDayFinalAccountWithUnit = rengeanPreviousDayFinalAccountWithUnit;
            RengeanPaymentWithUnit = rengeanPaymentWithUnit;
            RengeanWithdrawalWithUnit = rengeanWithdrawalWithUnit;
            RengeanTranceferWithUnit = rengeanTranceferWithUnit;
            ShunjuanPreviousDayFinalAccountWithUnit = shunjuanPreviousDayFinalAccountWithUnit;
            ShunjuanPaymentWithUnit = shunjuanPaymentWithUnit;
            ShunjuanWithdrawalWithUnit = shunjuanWithdrawalWithUnit;
            ShunjuanTranceferWithUnit = shunjuanTranceferWithUnit;
            KougePreviousDayFinalAccountWithUnit = kougePreviousDayFinalAccountWithUnit;
            KougePaymentWithUnit = kougePaymentWithUnit;
            KougeWithdrawalWithUnit = kougeWithdrawalWithUnit;
            KougeTranceferWithUnit = kougeTranceferWithUnit;
            YokohamaBankAmountWithUnit = yokohamaBankAmountWithUnit;
            ShunjuenAmountWithUnit = shunjuenAmountWithUnit;
        }

        protected override void SetBorderStyle()
        {
            //印欄
            MySheetCellRange(4, 6, 5, 8).Style
                .Border.SetRightBorder(XLBorderStyleValues.Thin)
                .Border.SetLeftBorder(XLBorderStyleValues.Thin)
                .Border.SetTopBorder(XLBorderStyleValues.Thin)
                .Border.SetBottomBorder(XLBorderStyleValues.Thin);
            //残高、仮払金
            MySheetCellRange(5, 1, 6, 3).Style
                .Border.SetBottomBorder(XLBorderStyleValues.Thin);
            //金額表
            MySheetCellRange(8, 1, 12, 8).Style
                .Border.SetRightBorder(XLBorderStyleValues.Thin)
                .Border.SetLeftBorder(XLBorderStyleValues.Thin)
                .Border.SetTopBorder(XLBorderStyleValues.Thin)
                .Border.SetBottomBorder(XLBorderStyleValues.Thin);
        }

        protected override void SetCellsStyle()
        {
            //タイトル
            myWorksheet.Cell(1, 1).Style
                .Font.SetFontSize(22)
                .Font.SetUnderline()
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            //タイトル以下のフォントサイズ
            MySheetCellRange(2, 1, 12, 8).Style
                .Font.SetFontSize(11);
            //日付、社名
            MySheetCellRange(2, 7, 3, 7).Style
                .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            //横浜銀行残高の縦位置
            MySheetCellRange(5, 1, 5, 3).Style
                .Alignment.SetVertical(XLAlignmentVerticalValues.Bottom);
            //残高、仮払金文字列の横位置
            MySheetCellRange(5, 1, 6, 1).Style
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            //残高、仮払金の金額横位置
            MySheetCellRange(5, 3, 6, 3).Style
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            //仮払金文字列、金額の縦位置
            MySheetCellRange(6, 1, 6, 3).Style
                .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            //表タイトルの位置設定
            MySheetCellRange(8, 1, 8, 1).Style
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            //内訳タイトルの位置設定
            MySheetCellRange(9, 1, 12, 1).Style
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            //表の値欄の位置設定
            MySheetCellRange(9, 2, 12, 8).Style
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        }

        protected override double[] SetColumnSizes()
        { return new double[] { 8.38, 2.75, 9.75, 6.5, 7.25, 14.38, 14.38, 14.38 }; }

        protected override void SetDataStrings()
        {
            //タイトル
            myWorksheet.Cell(1, 1).Value = "収支日報";
            //日付
            DateTime d = DateTime.Today;
            myWorksheet.Cell(2, 7).Value = $"{d.ToString("ggy", JapanCulture)}年{d.Month}月{d.Day}日（{d:ddd}）";
            //社名
            myWorksheet.Cell(3, 7).Value = "(株)ワイズ・コア";
            //印欄
            myWorksheet.Cell(4, 6).Value = "承認印";
            myWorksheet.Cell(4, 7).Value = "経理印";
            myWorksheet.Cell(4, 8).Value = "係印";
            //残高
            myWorksheet.Cell(5, 1).Value = "横浜銀行残高";
            myWorksheet.Cell(5, 3).Value = YokohamaBankAmountWithUnit;
            //仮払金
            myWorksheet.Cell(6, 1).Value = "春秋苑仮払金";
            myWorksheet.Cell(6, 3).Value = ShunjuenAmountWithUnit;
            //表タイトル
            myWorksheet.Cell(8, 1).Value = "内訳";
            myWorksheet.Cell(8, 2).Value = $"前日より\r\n繰越";
            myWorksheet.Cell(8, 4).Value = "入金";
            myWorksheet.Cell(8, 6).Value = "出金";
            myWorksheet.Cell(8, 7).Value = "銀行振替";
            myWorksheet.Cell(8, 8).Value = "残高";
            myWorksheet.Cell(9, 1).Value = "蓮華庵";
            myWorksheet.Cell(10, 1).Value = "春秋庵";
            myWorksheet.Cell(11, 1).Value = "香華";
            myWorksheet.Cell(12, 1).Value = "現金合計";
            //蓮華庵金額
            myWorksheet.Cell(9, 2).Value = RengeanPreviousDayFinalAccountWithUnit;
            myWorksheet.Cell(9, 4).Value = RengeanPaymentWithUnit;
            myWorksheet.Cell(9, 6).Value = RengeanWithdrawalWithUnit;
            myWorksheet.Cell(9, 7).Value = RengeanTranceferWithUnit;
            int renPre = IntAmount(RengeanPreviousDayFinalAccountWithUnit);
            int renPay = IntAmount(RengeanPaymentWithUnit);
            int renWith = IntAmount(RengeanWithdrawalWithUnit);
            int renTra = IntAmount(RengeanTranceferWithUnit);
            myWorksheet.Cell(9, 8).Value = AmountWithUnit(renPre + renPay - renWith - renTra);
            //春秋庵金額
            myWorksheet.Cell(10, 2).Value = ShunjuanPreviousDayFinalAccountWithUnit;
            myWorksheet.Cell(10, 4).Value = ShunjuanPaymentWithUnit;
            myWorksheet.Cell(10, 6).Value = ShunjuanWithdrawalWithUnit;
            myWorksheet.Cell(10, 7).Value = ShunjuanTranceferWithUnit;
            int shuPre = IntAmount(ShunjuanPreviousDayFinalAccountWithUnit);
            int shuPay = IntAmount(ShunjuanPaymentWithUnit);
            int shuWith = IntAmount(ShunjuanWithdrawalWithUnit);
            int shuTra = IntAmount(ShunjuanTranceferWithUnit);
            myWorksheet.Cell(10, 8).Value = AmountWithUnit(shuPre + shuPay - shuWith - shuTra);
            //香華金額
            myWorksheet.Cell(11, 2).Value = KougePreviousDayFinalAccountWithUnit;
            myWorksheet.Cell(11, 4).Value = KougePaymentWithUnit;
            myWorksheet.Cell(11, 6).Value = KougeWithdrawalWithUnit;
            myWorksheet.Cell(11, 7).Value = KougeTranceferWithUnit;
            int kouPre = IntAmount(KougePreviousDayFinalAccountWithUnit);
            int kouPay = IntAmount(KougePaymentWithUnit);
            int kouWith = IntAmount(KougeWithdrawalWithUnit);
            int kouTra = IntAmount(KougeTranceferWithUnit);
            myWorksheet.Cell(11, 8).Value = AmountWithUnit(kouPre + kouPay - kouWith - kouTra);
            //合計金額
            int totalPre = renPre + shuPre + kouPre;
            int totalPay = renPay + shuPay + kouPay;
            int totalWith = renWith + shuWith + kouWith;
            int totalTra = renTra + shuTra + kouTra;
            myWorksheet.Cell(12, 2).Value = AmountWithUnit(totalPre);
            myWorksheet.Cell(12, 4).Value = AmountWithUnit(totalPay);
            myWorksheet.Cell(12, 6).Value = AmountWithUnit(totalWith);
            myWorksheet.Cell(12, 7).Value = AmountWithUnit(totalTra);
            myWorksheet.Cell(12, 8).Value = AmountWithUnit(totalPre + totalPay - totalWith - totalTra);
        }

        protected override double SetMaeginsBottom() { return ToInch(1.9); }

        protected override double SetMaeginsLeft() { return ToInch(1.3); }

        protected override double SetMaeginsRight() { return ToInch(1.3); }

        protected override double SetMaeginsTop() { return ToInch(1.9); }

        protected override void SetMerge()
        {
            MySheetCellRange(1, 1, 1, SetColumnSizes().Length).Merge();//タイトル
            MySheetCellRange(2, 7, 2, 8).Merge();//日付
            MySheetCellRange(3, 7, 3, 8).Merge();//社名
            MySheetCellRange(5, 1, 5, 2).Merge();//残高文字列
            MySheetCellRange(5, 3, 5, 4).Merge();//残高金額
            MySheetCellRange(6, 1, 6, 2).Merge();//仮払金文字列
            MySheetCellRange(6, 3, 6, 4).Merge();//仮払金額
            MySheetCellRange(8, 2, 8, 3).Merge();//表タイトル繰越
            MySheetCellRange(8, 4, 8, 5).Merge();//表タイトル入金
            MySheetCellRange(9, 2, 9, 3).Merge();//蓮華庵繰越
            MySheetCellRange(9, 4, 9, 5).Merge();//蓮華庵入金
            MySheetCellRange(10, 2, 10, 3).Merge();//春秋庵繰越
            MySheetCellRange(10, 4, 10, 5).Merge();//春秋庵入金
            MySheetCellRange(11, 2, 11, 3).Merge();//香華繰越
            MySheetCellRange(11, 4, 11, 5).Merge();//香華入金
            MySheetCellRange(12, 2, 12, 3).Merge();//合計繰越
            MySheetCellRange(12, 4, 12, 5).Merge();//合計入金
        }

        protected override double[] SetRowSizes()
        { return new double[] { 42, 18.75, 18.75, 18.75, 55.5, 18.75, 18.75, 41.25, 45, 45, 45, 45 }; }

        protected override string SetSheetFontName() { return "游ゴシック"; }

        protected override void SetSheetStyle()
        { myWorksheet.Style.NumberFormat.Format = "@"; }

        protected override XLPaperSize SheetPaperSize() { return XLPaperSize.A4Paper; }
    }
}
