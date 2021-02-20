using ClosedXML.Excel;
using System;

namespace Infrastructure.ExcelOutputData
{
    /// <summary>
    /// 収支日報出力
    /// </summary>
    internal class BalanceFinalAccountOutput : OutputSingleSheetData
    {
        /// <summary>
        /// 前日決算額
        /// </summary>
        private readonly string PreviousDayFinalAccountWithUnit;
        /// <summary>
        /// 入金額
        /// </summary>
        private readonly string PaymentWithUnit;
        /// <summary>
        /// 出金額
        /// </summary>
        private readonly string WithdrawalWithUnit;
        /// <summary>
        /// 社内振替額
        /// </summary>
        private readonly string TranceferAmountWithUnit;
        /// <summary>
        /// 当日決算額
        /// </summary>
        private readonly string TodayFinalAccountWithUnit;
        /// <summary>
        /// 横浜銀行残高
        /// </summary>
        private readonly string YokohamaBankAmountWithUnit;
        /// <summary>
        /// セレサ川崎残高
        /// </summary>
        private readonly string CeresaAmountWithUnit;
        /// <summary>
        /// ワイズコア仮受金
        /// </summary>
        private readonly string WizeCoreAmountWithUnit;
        /// <param name="previousDayFinalAccountWithUnit">前日決算額</param>
        /// <param name="paymentWithUnit">入金額</param>
        /// <param name="withdrawalWithUnit">出金額param>
        /// <param name="tranceferAmountWithUnit">社内振替額</param>
        /// <param name="todayFinalAccountWithUnit">当日決算額</param>
        /// <param name="yokohamaBankAmountWithUnit">横浜銀行残高</param>
        /// <param name="ceresaAmountWithUnit">セレサ川崎残高</param>
        /// <param name="wizeCoreAmountWithUnit">ワイズコア仮受金</param>
        public BalanceFinalAccountOutput
            (
                string previousDayFinalAccountWithUnit, string paymentWithUnit, string withdrawalWithUnit, string tranceferAmountWithUnit, string todayFinalAccountWithUnit,
                string yokohamaBankAmountWithUnit, string ceresaAmountWithUnit, string wizeCoreAmountWithUnit
            )
        {
            PreviousDayFinalAccountWithUnit = previousDayFinalAccountWithUnit;
            PaymentWithUnit = paymentWithUnit;
            WithdrawalWithUnit = withdrawalWithUnit;
            TranceferAmountWithUnit = tranceferAmountWithUnit;
            TodayFinalAccountWithUnit = todayFinalAccountWithUnit;
            YokohamaBankAmountWithUnit = yokohamaBankAmountWithUnit;
            CeresaAmountWithUnit = ceresaAmountWithUnit;
            WizeCoreAmountWithUnit = wizeCoreAmountWithUnit;
        }

        protected override void SetBorderStyle()
        {
            MySheetCellRange(5,3,6,5).Style
                .Border.SetBottomBorder(XLBorderStyleValues.Thin)
                .Border.SetTopBorder(XLBorderStyleValues.Thin)
                .Border.SetLeftBorder(XLBorderStyleValues.Thin)
                .Border.SetRightBorder(XLBorderStyleValues.Thin);
            MySheetCellRange(9,1,10,5).Style
                .Border.SetBottomBorder(XLBorderStyleValues.Thin)
                .Border.SetTopBorder(XLBorderStyleValues.Thin)
                .Border.SetLeftBorder(XLBorderStyleValues.Thin)
                .Border.SetRightBorder(XLBorderStyleValues.Thin);
            MySheetCellRange(12, 2, 14, 3).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin);
        }

        protected override void SetCellsStyle()
        {
            myWorksheet.Cell(2, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            myWorksheet.Cell(3, 1).Style
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            MySheetCellRange(5,3,5,5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            myWorksheet.Cell(8, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            MySheetCellRange(9, 1, 9, 5).Style
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                .Alignment.SetVertical(XLAlignmentVerticalValues.Bottom);
            MySheetCellRange(10, 1, 10, 5).Style
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                .Alignment.SetVertical(XLAlignmentVerticalValues.Bottom);
            MySheetCellRange(12, 2, 14, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        }

        protected override double[] SetColumnSizes() => new double[] { 16, 16, 16, 16, 16 };

        protected override void SetDataStrings()
        {
            myWorksheet.Cell(2, 4).Value = DateTime.Today.ToString("yyyy年MM月dd日（ddd）");
            myWorksheet.Cell(3, 1).Value = "証憑綴り";
            myWorksheet.Cell(5, 3).Value = "本部長";
            myWorksheet.Cell(5, 4).Value = "副住職";
            myWorksheet.Cell(5, 5).Value = "係";
            myWorksheet.Cell(8, 1).Value = "収支";
            myWorksheet.Cell(9, 1).Value = "前日より繰越";
            myWorksheet.Cell(9, 2).Value = "入金";
            myWorksheet.Cell(9, 3).Value = "出金";
            myWorksheet.Cell(9, 4).Value = "社内振替";
            myWorksheet.Cell(9, 5).Value = "残高";
            myWorksheet.Cell(10, 1).Value = PreviousDayFinalAccountWithUnit;
            myWorksheet.Cell(10, 2).Value = PaymentWithUnit;
            myWorksheet.Cell(10, 3).Value = WithdrawalWithUnit;
            myWorksheet.Cell(10, 4).Value = TranceferAmountWithUnit;
            myWorksheet.Cell(10, 5).Value = TodayFinalAccountWithUnit;
            myWorksheet.Cell(12, 1).Value = "横浜銀行残高";
            myWorksheet.Cell(12, 2).Value = YokohamaBankAmountWithUnit;
            myWorksheet.Cell(13, 1).Value = "セレサ川崎残高";
            myWorksheet.Cell(13, 2).Value = CeresaAmountWithUnit;
            myWorksheet.Cell(14, 1).Value = "ワイズコア仮受金";
            myWorksheet.Cell(14, 2).Value = WizeCoreAmountWithUnit;
            myWorksheet.Cell(16, 4).Value = "信行寺　春秋苑";
        }

        protected override double SetMaeginsBottom() => ToInch(1.2);

        protected override double SetMaeginsLeft() => ToInch(1);

        protected override double SetMaeginsRight() => ToInch(0.5);

        protected override double SetMaeginsTop() => ToInch(1.3);

        protected override void SetMerge()
        {
            MySheetCellRange(2, 4, 2, 5).Merge();
            MySheetCellRange(3, 1, 3, 5).Merge();
            MySheetCellRange(12, 2, 12, 3).Merge();
            MySheetCellRange(13, 2, 13, 3).Merge();
            MySheetCellRange(14, 2, 14, 3).Merge();
            MySheetCellRange(16, 4, 16, 5).Merge();
        }

        protected override double[] SetRowSizes() => new double[] { 51, 18, 64.5, 57, 18, 42, 126, 18.75, 20.25, 42, 27.75, 21, 21, 21, 40.5, 20.25 };

        protected override string SetSheetFontName() => "ＭＳ ゴシック";

        protected override void SetSheetFontStyle()
        {
            myWorksheet.Style.Font.FontSize = 11;
            myWorksheet.Style.Font.FontName = "ＭＳ Ｐゴシック";
            myWorksheet.Cell(2, 4).Style.Font.FontSize = 14;
            myWorksheet.Cell(3, 1).Style.Font.FontSize = 28;
            myWorksheet.Cell(8, 1).Style.Font.FontSize = 16;
            MySheetCellRange(10, 1, 10, 5).Style.Font.FontSize = 14;
            MySheetCellRange(12, 2, 14, 3).Style.Font.FontSize = 14;
            MySheetCellRange(12, 2, 14, 3).Style.Font.Bold = true;
            myWorksheet.Cell(16, 4).Style.Font.FontSize = 16;
        }

        protected override XLPaperSize SheetPaperSize() => XLPaperSize.B5Paper;
    }
}
