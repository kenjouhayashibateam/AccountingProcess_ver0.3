using ClosedXML.Excel;
using Domain.Entities;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;

namespace Infrastructure.ExcelOutputData
{
    /// <summary>
    /// 出納データ出力
    /// </summary>
    internal class ReceiptsAndExpenditureOutput : OutputList
    {
        /// <summary>
        /// エクセルに出力する出納データの入出金日
        /// </summary>
        private DateTime CurrentDate;
        /// <summary>
        /// 1ページあたりの行数
        /// </summary>
        private readonly int OnePageRowCount = 50;
        /// <summary>
        /// 出納データリスト
        /// </summary>
        private ObservableCollection<ReceiptsAndExpenditure> ReceiptsAndExpenditures;
        /// <summary>
        /// 前日残高
        /// </summary>
        private int PreviousDayBalance;

        public ReceiptsAndExpenditureOutput(ObservableCollection<ReceiptsAndExpenditure> receiptsAndExpenditures,int previousDayBalance) : base(receiptsAndExpenditures)
        {
            PreviousDayBalance = previousDayBalance;
        }

        public override void Output()
        {
            int payment = 0;
            int withdrawal = 0;
            int itemCount = 0;

            foreach (ReceiptsAndExpenditure rae in ReceiptsAndExpenditures.OrderBy(r => r.AccountActivityDate)
                .ThenByDescending(r => r.IsPayment)
                .ThenBy(r => r.Content.AccountingSubject.SubjectCode))
            {
                if (CurrentDate != rae.AccountActivityDate)
                {
                    myWorksheet.Cell(ItemIndex + 1, 3).Value = "収支";
                    myWorksheet.Cell(ItemIndex + 1, 6).Value = payment;
                    myWorksheet.Cell(ItemIndex + 1, 7).Value = withdrawal;
                    PreviousDayBalance = PreviousDayBalance + payment - withdrawal;
                    myWorksheet.Cell(ItemIndex + 1, 8).Value = PreviousDayBalance;
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
                    myWorksheet.Cell(ItemIndex + 1, 6).Value = rae.Price;
                    payment += rae.Price;
                }
                else
                {
                    myWorksheet.Cell(ItemIndex + 1, 7).Value = rae.Price;
                    withdrawal += rae.Price;
                }
                SetStyleAndNextIndex();
                itemCount++;
                if(itemCount>OnePageRowCount)
                {
                    itemCount = 0;
                    NextPage();
                }
            }

            myWorksheet.Cell(ItemIndex + 1, 3).Value = "収支";
            myWorksheet.Cell(ItemIndex + 1, 6).Value = payment;
            myWorksheet.Cell(ItemIndex + 1, 7).Value = withdrawal;
            PreviousDayBalance = PreviousDayBalance + payment - withdrawal;
            myWorksheet.Cell(ItemIndex + 1, 8).Value = PreviousDayBalance;
            SetStyleAndNextIndex();
            ExcelOpen();
        }
        /// <summary>
        ///  インデックスに値を加える際に、前のデータのセルのスタイルを設定します
        ///  </summary>
        private void SetStyleAndNextIndex()
        {
            SetBorderStyle();
            SetCellsStyle();
            SetMargins();
            SetMerge();
            myWorksheet.Style.Alignment.SetShrinkToFit(true);
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
            myWorksheet.Style.Font.FontName = "ＭＳ Ｐゴシック";
        }

        protected override double[] SetColumnSizes() => new double[] { 10.86, 4.71, 16.43, 16.43, 16.43, 9.14, 9.14, 9.14 };

        protected override double SetMaeginsBottom() => ToInch(1);

        protected override double SetMaeginsLeft() => ToInch(1);

        protected override double SetMaeginsRight() => ToInch(1);

        protected override double SetMaeginsTop() => ToInch(1);

        protected override void SetMerge() {}

        protected override double[] SetRowSizes()
        {
            double[] d=new double[OnePageRowCount];
            for (int i = 0; i < d.Length; i++) { d[i] = 15; }
            return d;
        }

        protected override void SetSheetFontStyle() => myWorksheet.Style.Font.FontSize = 11;

        protected override XLPaperSize SheetPaperSize() => XLPaperSize.A4Paper;

        protected override void PageStyle()
        {
            myWorksheet.Cell(StartRowPosition + 1, 1).Value = "日付";
            myWorksheet.Cell(StartRowPosition + 1, 2).Value = "コード";
            myWorksheet.Cell(StartRowPosition + 1, 3).Value = "勘定科目";
            myWorksheet.Cell(StartRowPosition + 1, 4).Value = "内容";
            myWorksheet.Cell(StartRowPosition + 1, 5).Value = "詳細";
            myWorksheet.Cell(StartRowPosition + 1, 6).Value = "入金";
            myWorksheet.Cell(StartRowPosition + 1, 7).Value = "出金";
            myWorksheet.Cell(StartRowPosition + 1, 8).Value = "合計";
            SetStyleAndNextIndex();
        }

        protected override void SetList(IEnumerable outputList)
        {
            ReceiptsAndExpenditures =(ObservableCollection<ReceiptsAndExpenditure>) outputList;
        }
    }
}
