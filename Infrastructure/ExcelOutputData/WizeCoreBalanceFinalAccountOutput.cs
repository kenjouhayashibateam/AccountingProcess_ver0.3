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
        private readonly int RengeanPreviousDayFinalAccount;
        /// <summary>
        /// 蓮華庵入金
        /// </summary>
        private readonly int RengeanPayment;
        /// <summary>
        /// 蓮華庵出金
        /// </summary>
        private readonly int RengeanWithdrawal;
        /// <summary>
        /// 蓮華庵銀行振替
        /// </summary>
        private readonly int RengeanTrancefer;
        /// <summary>
        /// 春秋庵前日繰越
        /// </summary>
        private readonly int ShunjuanPreviousDayFinalAccount;
        /// <summary>
        /// 春秋庵入金
        /// </summary>
        private readonly int ShunjuanPayment;
        /// <summary>
        /// 春秋庵出金
        /// </summary>
        private readonly int ShunjuanWithdrawal;
        /// <summary>
        /// 春秋庵銀行振替
        /// </summary>
        private readonly int ShunjuanTrancefer;
        /// <summary>
        /// 香華前日繰越
        /// </summary>
        private readonly int KougePreviousDayFinalAccount;
        /// <summary>
        /// 香華入金
        /// </summary>
        private readonly int KougePayment;
        /// <summary>
        /// 香華出金
        /// </summary>
        private readonly int KougeWithdrawal;
        /// <summary>
        /// 香華銀行振替
        /// </summary>
        private readonly int KougeTrancefer;
        /// <summary>
        /// 横浜銀行残高
        /// </summary>
        private readonly int YokohamaBankAmount;
        /// <summary>
        /// 春秋苑仮払金
        /// </summary>
        private readonly int ShunjuenAmount;
        /// <param name="rengeanPreviousDayFinalAccount">蓮華庵前日繰越</param>
        /// <param name="rengeanPayment">蓮華庵入金</param>
        /// <param name="rengeanWithdrawal">蓮華庵出金</param>
        /// <param name="rengeanTrancefer">蓮華庵銀行振替</param>
        /// <param name="shunjuanPreviousDayFinalAccount">春秋庵前日繰越</param>
        /// <param name="shunjuanPayment">春秋庵入金</param>
        /// <param name="shunjuanWithdrawal">春秋庵出金</param>
        /// <param name="shunjuanTrancefer">春秋庵銀行振替</param>
        /// <param name="kougePreviousDayFinalAccount">香華前日繰越</param>
        /// <param name="kougePayment">香華入金</param>
        /// <param name="kougeWithdrawal">香華出金</param>
        /// <param name="kougeTrancefer">香華銀行振替</param>
        /// <param name="yokohamaBankAmount">横浜銀行残高</param>
        /// <param name="shunjuenAmount">春秋苑仮払金</param>
        public WizeCoreBalanceFinalAccountOutput
            (int rengeanPreviousDayFinalAccount, int rengeanPayment,int rengeanWithdrawal,
                int rengeanTrancefer,int shunjuanPreviousDayFinalAccount, int shunjuanPayment,
                int shunjuanWithdrawal, int shunjuanTrancefer,int kougePreviousDayFinalAccount,
                int kougePayment,int kougeWithdrawal, int kougeTrancefer,int yokohamaBankAmount,
                int shunjuenAmount)
        {
            RengeanPreviousDayFinalAccount = rengeanPreviousDayFinalAccount;
            RengeanPayment = rengeanPayment;
            RengeanWithdrawal = rengeanWithdrawal;
            RengeanTrancefer = rengeanTrancefer;
            ShunjuanPreviousDayFinalAccount = shunjuanPreviousDayFinalAccount;
            ShunjuanPayment = shunjuanPayment;
            ShunjuanWithdrawal = shunjuanWithdrawal;
            ShunjuanTrancefer = shunjuanTrancefer;
            KougePreviousDayFinalAccount = kougePreviousDayFinalAccount;
            KougePayment = kougePayment;
            KougeWithdrawal = kougeWithdrawal;
            KougeTrancefer = kougeTrancefer;
            YokohamaBankAmount = yokohamaBankAmount;
            ShunjuenAmount = shunjuenAmount;
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
            MySheetCellRange(5, 1, 6, 4).Style
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
            //係印
            MySheetCellRange(4, 6, 4, 8).Style
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
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
            MySheetCellRange(8, 1, 8, 8).Style
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
        { return new double[] { 8.42, 2.75, 8.58, 6.08, 5.25, 11.92, 11.92, 11.92 }; }

        protected override void SetDataStrings()
        {
            //タイトル
            myWorksheet.Cell(1, 1).Value = "収支日報";
            //日付
            DateTime d = DateTime.Today;
            myWorksheet.Cell(2, 7).Value = 
                $"{d.ToString("ggy", JapanCulture)}年{d.Month}月{d.Day}日（{d:ddd}）";
            //社名
            myWorksheet.Cell(3, 7).Value = "(株)ワイズ・コア";
            //印欄
            myWorksheet.Cell(4, 6).Value = "承認印";
            myWorksheet.Cell(4, 7).Value = "経理印";
            myWorksheet.Cell(4, 8).Value = "係印";
            //残高
            myWorksheet.Cell(5, 1).Value = "横浜銀行残高";
            myWorksheet.Cell(5, 3).Value = AmountWithUnit(YokohamaBankAmount);
            //仮払金
            myWorksheet.Cell(6, 1).Value = "春秋苑仮払金";
            myWorksheet.Cell(6, 3).Value = AmountWithUnit(ShunjuenAmount);
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
            myWorksheet.Cell(9, 2).Value = AmountWithUnit(RengeanPreviousDayFinalAccount);
            myWorksheet.Cell(9, 4).Value = AmountWithUnit(RengeanPayment);
            myWorksheet.Cell(9, 6).Value = AmountWithUnit(RengeanWithdrawal);
            myWorksheet.Cell(9, 7).Value = AmountWithUnit(RengeanTrancefer);
            myWorksheet.Cell(9, 8).Value = AmountWithUnit(RengeanPreviousDayFinalAccount + 
                RengeanPayment - RengeanWithdrawal - RengeanTrancefer);
            //春秋庵金額
            myWorksheet.Cell(10, 2).Value = AmountWithUnit(ShunjuanPreviousDayFinalAccount);
            myWorksheet.Cell(10, 4).Value = AmountWithUnit(ShunjuanPayment);
            myWorksheet.Cell(10, 6).Value = AmountWithUnit(ShunjuanWithdrawal);
            myWorksheet.Cell(10, 7).Value = AmountWithUnit(ShunjuanTrancefer);
            myWorksheet.Cell(10, 8).Value = AmountWithUnit(ShunjuanPreviousDayFinalAccount +
                ShunjuanPayment - ShunjuanWithdrawal - ShunjuanTrancefer);
            //香華金額
            myWorksheet.Cell(11, 2).Value = AmountWithUnit(KougePreviousDayFinalAccount);
            myWorksheet.Cell(11, 4).Value = AmountWithUnit(KougePayment);
            myWorksheet.Cell(11, 6).Value = AmountWithUnit(KougeWithdrawal);
            myWorksheet.Cell(11, 7).Value = AmountWithUnit(KougeTrancefer);
            myWorksheet.Cell(11, 8).Value = AmountWithUnit(KougePreviousDayFinalAccount +
                KougePayment - KougeWithdrawal - KougeTrancefer);
            //合計金額
            int totalPre = RengeanPreviousDayFinalAccount + ShunjuanPreviousDayFinalAccount +
                KougePreviousDayFinalAccount;
            int totalPay = RengeanPayment + ShunjuanPayment + KougePayment;
            int totalWith = RengeanWithdrawal + ShunjuanWithdrawal + KougeWithdrawal;
            int totalTra = RengeanTrancefer + ShunjuanTrancefer + KougeTrancefer;
            myWorksheet.Cell(12, 2).Value = AmountWithUnit(totalPre);
            myWorksheet.Cell(12, 4).Value = AmountWithUnit(totalPay);
            myWorksheet.Cell(12, 6).Value = AmountWithUnit(totalWith);
            myWorksheet.Cell(12, 7).Value = AmountWithUnit(totalTra);
            myWorksheet.Cell(12, 8).Value = 
                AmountWithUnit(totalPre + totalPay - totalWith - totalTra);
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

        protected override XLPaperSize SheetPaperSize() { return XLPaperSize.B5Paper; }
    }
}
