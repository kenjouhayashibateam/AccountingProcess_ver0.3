using ClosedXML.Excel;
using Domain.Entities;
using System;
using static Domain.Entities.Helpers.TextHelper;
using static Domain.Entities.Helpers.DataHelper;
using Domain.Entities.ValueObjects;

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
        /// 蓮華庵春秋苑振替
        /// </summary>
        private readonly int RengeanShunjuenTrancefer;
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
        /// 春秋庵春秋苑振替
        /// </summary>
        private readonly int ShunjuanShunjuenTrancefer;
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
        /// 香華春秋苑振替
        /// </summary>
        private readonly int KougeShunjuenTrancefer;
        /// <summary>
        /// 横浜銀行残高
        /// </summary>
        private readonly int YokohamaBankAmount;
        /// <summary>
        /// 春秋苑仮払金
        /// </summary>
        private readonly int ShunjuenAmount;
        /// <summary>
        /// 未出力出納データを含ませるか
        /// </summary>
        private readonly bool IsContainDailyReportToNotOutputData;
        /// <param name="rengeanPreviousDayFinalAccount">蓮華庵前日繰越</param>
        /// <param name="rengeanPayment">蓮華庵入金</param>
        /// <param name="rengeanWithdrawal">蓮華庵出金</param>
        /// <param name="rengeanTranceferAmount">蓮華庵銀行振替</param>
        /// <param name="rengeanShunjuenTranceferAmount">蓮華庵春秋苑振替</param>
        /// <param name="shunjuanPreviousDayFinalAccount">春秋庵前日繰越</param>
        /// <param name="shunjuanPayment">春秋庵入金</param>
        /// <param name="shunjuanWithdrawal">春秋庵出金</param>
        /// <param name="shunjuanTranceferAmount">春秋庵銀行振替</param>
        /// <param name="shunjuanShunjuenTranceferAmount">春秋庵春秋苑振替</param>
        /// <param name="kougePreviousDayFinalAccount">香華前日繰越</param>
        /// <param name="kougePayment">香華入金</param>
        /// <param name="kougeWithdrawal">香華出金</param>
        /// <param name="kougeTranceferAmount">香華銀行振替</param>
        /// <param name="kougeShunjuenTranceferAmount">香華春秋苑振替</param>
        /// <param name="yokohamaBankAmount">横浜銀行残高</param>
        /// <param name="shunjuenAmount">春秋苑仮払金</param>
        public WizeCoreBalanceFinalAccountOutput
            (bool isContainDailyReportToNotOutputData, int rengeanPreviousDayFinalAccount,
                int rengeanPayment, int rengeanWithdrawal, int rengeanTranceferAmount,
                int rengeanShunjuenTranceferAmount, int shunjuanPreviousDayFinalAccount,
                int shunjuanPayment, int shunjuanWithdrawal, int shunjuanTranceferAmount,
                int shunjuanShunjuenTranceferAmount, int kougePreviousDayFinalAccount,
                int kougePayment, int kougeWithdrawal, int kougeTranceferAmount,
                int kougeShunjuenTranceferAmount, int yokohamaBankAmount, int shunjuenAmount)
        {
            RengeanPreviousDayFinalAccount = rengeanPreviousDayFinalAccount;
            RengeanPayment = rengeanPayment;
            RengeanWithdrawal = rengeanWithdrawal;
            RengeanTrancefer = rengeanTranceferAmount;
            RengeanShunjuenTrancefer = rengeanShunjuenTranceferAmount;
            ShunjuanPreviousDayFinalAccount = shunjuanPreviousDayFinalAccount;
            ShunjuanPayment = shunjuanPayment;
            ShunjuanWithdrawal = shunjuanWithdrawal;
            ShunjuanTrancefer = shunjuanTranceferAmount;
            ShunjuanShunjuenTrancefer = shunjuanShunjuenTranceferAmount;
            KougePreviousDayFinalAccount = kougePreviousDayFinalAccount;
            KougePayment = kougePayment;
            KougeWithdrawal = kougeWithdrawal;
            KougeTrancefer = kougeTranceferAmount;
            KougeShunjuenTrancefer = kougeShunjuenTranceferAmount;
            YokohamaBankAmount = yokohamaBankAmount;
            ShunjuenAmount = shunjuenAmount;
            IsContainDailyReportToNotOutputData = isContainDailyReportToNotOutputData;
        }

        protected override void SetBorderStyle()
        {
            if (!IsContainDailyReportToNotOutputData)
            { SetKanriJimushoStyle(); }
            else { SetContainDailyReportToNotOutputDataStyle(); }

            void SetKanriJimushoStyle()
            {
                //印欄
                _ = MySheetCellRange(4, 7, 5, 9).Style
                    .Border.SetRightBorder(XLBorderStyleValues.Thin)
                    .Border.SetLeftBorder(XLBorderStyleValues.Thin)
                    .Border.SetTopBorder(XLBorderStyleValues.Thin)
                    .Border.SetBottomBorder(XLBorderStyleValues.Thin);

                //残高、仮払金
                _ = MySheetCellRange(5, 1, 6, 5).Style
                    .Border.SetBottomBorder(XLBorderStyleValues.Thin);
                //金額表
                _ = MySheetCellRange(8, 1, 12, SetColumnSizes().Length).Style
                    .Border.SetRightBorder(XLBorderStyleValues.Thin)
                    .Border.SetLeftBorder(XLBorderStyleValues.Thin)
                    .Border.SetTopBorder(XLBorderStyleValues.Thin)
                    .Border.SetBottomBorder(XLBorderStyleValues.Thin);
            }

            void SetContainDailyReportToNotOutputDataStyle()
            {
                //印欄
                _ = MySheetCellRange(4, 9, 5, 9).Style
                    .Border.SetRightBorder(XLBorderStyleValues.Thin)
                    .Border.SetLeftBorder(XLBorderStyleValues.Thin)
                    .Border.SetTopBorder(XLBorderStyleValues.Thin)
                    .Border.SetBottomBorder(XLBorderStyleValues.Thin);
                //金額表
                _=MySheetCellRange(7,1,11, SetColumnSizes().Length).Style
                    .Border.SetRightBorder(XLBorderStyleValues.Thin)
                    .Border.SetLeftBorder(XLBorderStyleValues.Thin)
                    .Border.SetTopBorder(XLBorderStyleValues.Thin)
                    .Border.SetBottomBorder(XLBorderStyleValues.Thin);
            }
        }

        protected override void SetCellsStyle()
        {
            //タイトル
            _ = myWorksheet.Cell(1, 1).Style
                .Font.SetFontSize(22)
                .Font.SetUnderline()
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            //タイトル以下のフォントサイズ
            _ = MySheetCellRange(2, 1, 12, SetColumnSizes().Length).Style
                .Font.SetFontSize(11);
            //日付、社名
            _ = MySheetCellRange(2, 7, 3, 8).Style
                .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            //係印
            _ = MySheetCellRange(4, 7, 5, 9).Style
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            //横浜銀行残高の縦位置
            _ = MySheetCellRange(5, 1, 5, 3).Style
                .Alignment.SetVertical(XLAlignmentVerticalValues.Bottom);
            //残高、仮払金文字列の横位置
            _ = MySheetCellRange(5, 1, 6, 1).Style
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            //残高、仮払金の金額横位置
            _ = MySheetCellRange(5, 3, 6, 3).Style
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            //仮払金文字列、金額の縦位置
            _ = MySheetCellRange(6, 1, 6, 3).Style
                .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            //表タイトルの位置設定
            int column1 = 8;
            if (IsContainDailyReportToNotOutputData)
            { column1 = 7; }
            _ = MySheetCellRange(column1, 1, column1, SetColumnSizes().Length).Style
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            //内訳タイトルの位置設定
            column1++;
            _ = MySheetCellRange(column1, 1, 12, 1).Style
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            //表の値欄の位置設定
            _ = MySheetCellRange(column1, 2, 12, SetColumnSizes().Length).Style
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        }

        protected override double[] SetColumnSizes()
        { return new double[] { 8.43, 4.14, 6.71, 6.14, 4.71, 11.57, 11.57, 11.57, 11.57 }; }

        protected override void SetDataStrings()
        {
            //タイトル
            myWorksheet.Cell(1, 1).Value = "収支日報";
            //日付
            DateTime d = DateTime.Now;
            myWorksheet.Cell(2, 7).Value =
                $"{d.ToString("ggy", JapanCulture)}年{d.Month}月{d.Day}日（{d:ddd}）{d.Hour}:{d.Minute}現在";
            //社名
            myWorksheet.Cell(3, 8).Value = "(株)ワイズ・コア";
            //合計金額
            int totalPre = RengeanPreviousDayFinalAccount + ShunjuanPreviousDayFinalAccount +
                KougePreviousDayFinalAccount;
            int totalPay = RengeanPayment + ShunjuanPayment + KougePayment;
            int totalWith = RengeanWithdrawal + ShunjuanWithdrawal + KougeWithdrawal;
            int totalTra = RengeanTrancefer + ShunjuanTrancefer + KougeTrancefer;
            int totalShun =
                RengeanShunjuenTrancefer + ShunjuanShunjuenTrancefer + KougeShunjuenTrancefer;

            if (IsContainDailyReportToNotOutputData)
            { SetContainDailyReportToNotOutputDataStyle(); }
            else { SetKanriJimushoStyle(); }

            void SetKanriJimushoStyle()
            {
                //印欄
                myWorksheet.Cell(4, 7).Value = "承認印";
                myWorksheet.Cell(4, 8).Value = "経理印";
                myWorksheet.Cell(4, 9).Value = "係印";
                myWorksheet.Cell(5, 9).Value = LoginRep.GetInstance().Rep.FirstName;
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
                myWorksheet.Cell(8, 8).Value = "春秋苑\r\n仮払金";
                myWorksheet.Cell(8, 9).Value = "残高";
                myWorksheet.Cell(9, 1).Value = "蓮華庵";
                myWorksheet.Cell(10, 1).Value = "春秋庵";
                myWorksheet.Cell(11, 1).Value = "香華";
                myWorksheet.Cell(12, 1).Value = "現金合計";
                //蓮華庵金額
                myWorksheet.Cell(9, 2).Value = AmountWithUnit(RengeanPreviousDayFinalAccount);
                myWorksheet.Cell(9, 4).Value = AmountWithUnit(RengeanPayment);
                myWorksheet.Cell(9, 6).Value = AmountWithUnit(RengeanWithdrawal);
                myWorksheet.Cell(9, 7).Value = AmountWithUnit(RengeanTrancefer);
                myWorksheet.Cell(9, 8).Value = AmountWithUnit(RengeanShunjuenTrancefer);
                myWorksheet.Cell(9, 9).Value = AmountWithUnit(RengeanPreviousDayFinalAccount +
                    RengeanPayment - RengeanWithdrawal - RengeanTrancefer - RengeanShunjuenTrancefer);
                //春秋庵金額
                myWorksheet.Cell(10, 2).Value = AmountWithUnit(ShunjuanPreviousDayFinalAccount);
                myWorksheet.Cell(10, 4).Value = AmountWithUnit(ShunjuanPayment);
                myWorksheet.Cell(10, 6).Value = AmountWithUnit(ShunjuanWithdrawal);
                myWorksheet.Cell(10, 7).Value = AmountWithUnit(ShunjuanTrancefer);
                myWorksheet.Cell(10, 8).Value = AmountWithUnit(ShunjuanShunjuenTrancefer);
                myWorksheet.Cell(10, 9).Value = AmountWithUnit(ShunjuanPreviousDayFinalAccount +
                    ShunjuanPayment - ShunjuanWithdrawal - ShunjuanTrancefer - ShunjuanShunjuenTrancefer);
                //香華金額
                myWorksheet.Cell(11, 2).Value = AmountWithUnit(KougePreviousDayFinalAccount);
                myWorksheet.Cell(11, 4).Value = AmountWithUnit(KougePayment);
                myWorksheet.Cell(11, 6).Value = AmountWithUnit(KougeWithdrawal);
                myWorksheet.Cell(11, 7).Value = AmountWithUnit(KougeTrancefer);
                myWorksheet.Cell(11, 8).Value = AmountWithUnit(KougeShunjuenTrancefer);
                myWorksheet.Cell(11, 9).Value = AmountWithUnit(KougePreviousDayFinalAccount +
                    KougePayment - KougeWithdrawal - KougeTrancefer - KougeShunjuenTrancefer);
                myWorksheet.Cell(12, 2).Value = AmountWithUnit(totalPre);
                myWorksheet.Cell(12, 4).Value = AmountWithUnit(totalPay);
                myWorksheet.Cell(12, 6).Value = AmountWithUnit(totalWith);
                myWorksheet.Cell(12, 7).Value = AmountWithUnit(totalTra);
                myWorksheet.Cell(12, 8).Value = AmountWithUnit(totalShun);
                myWorksheet.Cell(12, 9).Value =
                    AmountWithUnit(totalPre + totalPay - totalWith - totalTra - totalShun);
            }

            void SetContainDailyReportToNotOutputDataStyle()
            {
                //印欄
                myWorksheet.Cell(4, 9).Value = "係印";
                myWorksheet.Cell(5, 9).Value = LoginRep.GetInstance().Rep.FirstName;
                //経理担当場所
                myWorksheet.Cell(6, 1).Value = $"経理担当場所：{AccountingProcessLocation.Location}";
                //表タイトル
                myWorksheet.Cell(7, 2).Value = $"入金";
                myWorksheet.Cell(7, 6).Value = "出金";
                myWorksheet.Cell(7, 8).Value = "合計";
                myWorksheet.Cell(8, 1).Value = "蓮華庵";
                myWorksheet.Cell(9, 1).Value = "春秋庵";
                myWorksheet.Cell(10, 1).Value = "香華";
                myWorksheet.Cell(11, 1).Value = "合計";
                myWorksheet.Cell(8, 2).Value = AmountWithUnit(RengeanPayment);
                myWorksheet.Cell(8, 6).Value = AmountWithUnit(RengeanWithdrawal);
                myWorksheet.Cell(8, 8).Value = AmountWithUnit(RengeanPayment - RengeanWithdrawal);
                myWorksheet.Cell(9, 2).Value = AmountWithUnit(ShunjuanPayment);
                myWorksheet.Cell(9, 6).Value = AmountWithUnit(ShunjuanWithdrawal);
                myWorksheet.Cell(9, 8).Value = AmountWithUnit(ShunjuanPayment - RengeanWithdrawal);
                myWorksheet.Cell(10, 2).Value = AmountWithUnit(KougePayment);
                myWorksheet.Cell(10, 6).Value = AmountWithUnit(KougeWithdrawal);
                myWorksheet.Cell(10, 8).Value = AmountWithUnit(KougePayment - KougeWithdrawal);
                myWorksheet.Cell(11, 2).Value = AmountWithUnit(totalPay);
                myWorksheet.Cell(11, 6).Value = AmountWithUnit(totalWith);
                myWorksheet.Cell(11, 8).Value = AmountWithUnit(totalPay - totalWith);
            }
        }

        protected override double SetMarginsBottom() { return ToInch(1.9); }

        protected override double SetMarginsLeft() { return ToInch(0.8); }

        protected override double SetMarginsRight() { return ToInch(0.8); }

        protected override double SetMarginsTop() { return ToInch(1.9); }

        protected override void SetMerge()
        {
            _ = MySheetCellRange(1, 1, 1, SetColumnSizes().Length).Merge();//タイトル
            _ = MySheetCellRange(2, 7, 2, 9).Merge();//日付
            _ = MySheetCellRange(3, 8, 3, 9).Merge();//社名

            if (!IsContainDailyReportToNotOutputData)
            { SetKanriJimushoStyle(); }
            else { SetContainDailyReportToNotOutputDataStyle(); }

            void SetContainDailyReportToNotOutputDataStyle()
            {
                _ = MySheetCellRange(6, 1, 6, 5).Merge();//経理担当場所
                _ = MySheetCellRange(7, 2, 7, 5).Merge();//表タイトル入金
                _ = MySheetCellRange(7, 6, 7, 7).Merge();//表タイトル出金
                _ = MySheetCellRange(7, 8, 7, 9).Merge();//表タイトル合計
                _ = MySheetCellRange(8, 2, 8, 5).Merge();//蓮華庵入金
                _ = MySheetCellRange(8, 6, 8, 7).Merge();//蓮華庵出金
                _ = MySheetCellRange(8, 8, 8, 9).Merge();//蓮華庵合計
                _ = MySheetCellRange(9, 2, 9, 5).Merge();//春秋庵入金
                _ = MySheetCellRange(9, 6, 9, 7).Merge();//春秋庵出金
                _ = MySheetCellRange(9, 8, 9, 9).Merge();//春秋庵合計
                _ = MySheetCellRange(10, 2, 10, 5).Merge();//香華入金
                _ = MySheetCellRange(10, 6, 10, 7).Merge();//香華出金
                _ = MySheetCellRange(10, 8, 10, 9).Merge();//香華合計
                _ = MySheetCellRange(11, 2, 11, 5).Merge();//入金
                _ = MySheetCellRange(11, 6, 11, 7).Merge();//出金
                _ = MySheetCellRange(11, 8, 11, 9).Merge();//合計
            }

            void SetKanriJimushoStyle()
            {
                _ = MySheetCellRange(5, 1, 5, 2).Merge();//残高文字列
                _ = MySheetCellRange(5, 3, 5, 5).Merge();//残高金額
                _ = MySheetCellRange(6, 1, 6, 2).Merge();//仮払金文字列
                _ = MySheetCellRange(6, 3, 6, 5).Merge();//仮払金額
                _ = MySheetCellRange(8, 2, 8, 3).Merge();//表タイトル繰越
                _ = MySheetCellRange(8, 4, 8, 5).Merge();//表タイトル入金
                _ = MySheetCellRange(9, 2, 9, 3).Merge();//蓮華庵繰越
                _ = MySheetCellRange(9, 4, 9, 5).Merge();//蓮華庵入金
                _ = MySheetCellRange(10, 2, 10, 3).Merge();//春秋庵繰越
                _ = MySheetCellRange(10, 4, 10, 5).Merge();//春秋庵入金
                _ = MySheetCellRange(11, 2, 11, 3).Merge();//香華繰越
                _ = MySheetCellRange(11, 4, 11, 5).Merge();//香華入金
                _ = MySheetCellRange(12, 2, 12, 3).Merge();//合計繰越
                _ = MySheetCellRange(12, 4, 12, 5).Merge();//合計入金            
            }
        }

        protected override double[] SetRowSizes()
        { return new double[] { 42, 18.75, 18.75, 18.75, 55.5, 18.75, 18.75, 41.25, 45, 45, 45, 45 }; }

        protected override string SetSheetFontName() { return "游ゴシック"; }

        protected override void SetSheetStyle()
        { myWorksheet.Style.NumberFormat.Format = "@"; }

        protected override XLPaperSize SheetPaperSize() { return XLPaperSize.B5Paper; }
    }
}
