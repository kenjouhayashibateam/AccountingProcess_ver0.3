using ClosedXML.Excel;
using Domain.Entities;
using Domain.Entities.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Infrastructure.ExcelOutputData
{
    /// <summary>
    /// 出納データ出力
    /// </summary>
    internal class ReceiptsAndExpenditureOutput : OutputData
    {
        /// <summary>
        /// 出納データのインデックス
        /// </summary>
        private int ItemIndex;
        /// <summary>
        /// エクセルに出力する出納データの入出金日
        /// </summary>
        private DateTime CurrentDate; 
        /// <summary>
        /// 出納データリスト
        /// </summary>
        private readonly ObservableCollection<ReceiptsAndExpenditure> ReceiptsAndExpenditures;
        /// <summary>
        /// 前日残高
        /// </summary>
        private int PreviousDayBalance;

        public ReceiptsAndExpenditureOutput(ObservableCollection<ReceiptsAndExpenditure> receiptsAndExpenditures,int previousDayBalance) : base()
        {
            ReceiptsAndExpenditures = receiptsAndExpenditures;
            PreviousDayBalance = previousDayBalance;
            ItemIndex = 0;
            MySheetCellRange(1, 1, 1, 8).Style
                .Border.SetLeftBorder(XLBorderStyleValues.Thin)
                .Border.SetTopBorder(XLBorderStyleValues.Thin)
                .Border.SetRightBorder(XLBorderStyleValues.Thin)
                .Border.SetBottomBorder(XLBorderStyleValues.Thin);

            myWorksheet.Cell(1, 1).Value = "日付";
            myWorksheet.Cell(1, 2).Value = "コード";
            myWorksheet.Cell(1, 3).Value = "勘定科目";
            myWorksheet.Cell(1, 4).Value = "内容";
            myWorksheet.Cell(1, 5).Value = "詳細";
            myWorksheet.Cell(1, 6).Value = "入金";
            myWorksheet.Cell(1, 7).Value = "出金";
            myWorksheet.Cell(1, 8).Value = "合計";
            ItemIndex++;
        
        }
        public void DataOutput()
        {
            int payment = 0;
            int withdrawal = 0;
            foreach (ReceiptsAndExpenditure rae in ReceiptsAndExpenditures.OrderBy(r=>r.AccountActivityDate).ThenBy(r=>r.Content.AccountingSubject.SubjectCode ))
            {
                if (CurrentDate != rae.AccountActivityDate)
                {
                    myWorksheet.Cell(ItemIndex + 1, 3).Value = "収支";
                    myWorksheet.Cell(ItemIndex + 1, 6).Value = payment;
                    myWorksheet.Cell(ItemIndex + 1, 7).Value = withdrawal;
                    myWorksheet.Cell(ItemIndex + 1, 8).Value = PreviousDayBalance;
                    PreviousDayBalance = PreviousDayBalance + payment - withdrawal;
                    payment = 0;
                    withdrawal = 0;
                    SetStyleAndNextIndex();
                    CurrentDate = rae.AccountActivityDate;
                    myWorksheet.Cell(ItemIndex + 1, 1).Value = rae.AccountActivityDate;
                    SetStyleAndNextIndex();
                }
                myWorksheet.Cell(ItemIndex + 1, 2).Value = rae.Content.AccountingSubject.SubjectCode;
                myWorksheet.Cell(ItemIndex + 1, 3).Value = rae.Content.AccountingSubject.Subject;
                myWorksheet.Cell(ItemIndex + 1, 4).Value = rae.Content.Text;
                myWorksheet.Cell(ItemIndex + 1, 5).Value = rae.Detail;
                if (rae.IsPayment)
                {
                    myWorksheet.Cell(ItemIndex + 1, 6).Value =rae.Price;
                    payment += rae.Price;
                }
                else
                {
                    myWorksheet.Cell(ItemIndex + 1, 7).Value = rae.Price;
                    withdrawal += rae.Price;
                }
                SetStyleAndNextIndex();
            }
            myWorksheet.Cell(ItemIndex + 1, 3).Value = "収支";
            myWorksheet.Cell(ItemIndex + 1, 6).Value = payment;
            myWorksheet.Cell(ItemIndex + 1, 7).Value = withdrawal;
            SetStyleAndNextIndex();
            myWorkbook.SaveAs(openPath);
            ExcelOpen();
        }
        private void SetStyleAndNextIndex()
        {
            SetBorderStyle();
            SetCellsStyle();
            ItemIndex++;
        }
        protected override void SetBorderStyle()
        {
            MySheetCellRange(ItemIndex + 1, 1, ItemIndex + 1, 8).Style
                .Border.SetLeftBorder(XLBorderStyleValues.Thin)
                .Border.SetTopBorder(XLBorderStyleValues.Thin)
                .Border.SetRightBorder(XLBorderStyleValues.Thin)
                .Border.SetBottomBorder(XLBorderStyleValues.Thin);
        }

        protected override void SetCellsStyle()
        {
            myWorksheet.Cell(ItemIndex + 1, 1).Style
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            myWorksheet.Cell(ItemIndex + 1, 2).Style
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            MySheetCellRange(ItemIndex + 1, 3, ItemIndex + 1, 5).Style
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            MySheetCellRange(ItemIndex + 1, 6, ItemIndex + 1, 8).Style
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                .NumberFormat.SetFormat("#,##0");
        }

        protected override double[] SetColumnSizes() => new double[] { 1, 1, 1, 1, 1, 1, 1, 1 };

        protected override double SetMaeginsBottom() => ToInch(1.9);

        protected override double SetMaeginsLeft() => ToInch(1.8);

        protected override double SetMaeginsRight() => ToInch(1.8);

        protected override double SetMaeginsTop() => ToInch(1.9);

        protected override void SetMerge() {}

        protected override double[] SetRowSizes() => new double[] { 1 };

        protected override string SetSheetFontName() => "ＭＳ Ｐゴシック";

        protected override void SetSheetFontStyle() {}

        protected override XLPaperSize SheetPaperSize() => XLPaperSize.A4Paper;
    }
}
