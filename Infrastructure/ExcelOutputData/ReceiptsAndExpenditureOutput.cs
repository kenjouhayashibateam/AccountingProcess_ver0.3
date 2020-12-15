using ClosedXML.Excel;
using Domain.Entities;
using System;

namespace Infrastructure.ExcelOutputData
{
    internal class ReceiptsAndExpenditureOutput : OutputData
    {
        /// <summary>
        /// 出納データ
        /// </summary>
        private readonly ReceiptsAndExpenditure ReceiptsAndExpenditureData;
        /// <summary>
        /// 出納データのインデックス
        /// </summary>
        private int ItemIndex;
        /// <summary>
        /// エクセルに出力する出納データの入出金日
        /// </summary>
        private DateTime CurrentDate;

        public ReceiptsAndExpenditureOutput(ReceiptsAndExpenditure receiptsAndExpenditure, int itemIndex)
        {
            ReceiptsAndExpenditureData = receiptsAndExpenditure;
            ItemIndex = itemIndex;
            if(ItemIndex==0)
            {
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
        }

        protected override void SetBorderStyle()
        {
            MySheetCellRange(ItemIndex + 1, 1, ItemIndex + 1, 8).Style
                .Border.SetLeftBorder(XLBorderStyleValues.Thin)
                .Border.SetTopBorder(XLBorderStyleValues.Thin)
                .Border.SetRightBorder(XLBorderStyleValues.Thin)
                .Border.SetBottomBorder(XLBorderStyleValues.Thin);
        }

        protected override void SetCellsAlignment()
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
                .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        }

        protected override double[] SetColumnSizes() => new double[] { 1, 1, 1, 1, 1, 1, 1, 1 };

        protected override void SetDataStrings()
        {
            if (CurrentDate != ReceiptsAndExpenditureData.AccountActivityDate)
            {
                CurrentDate = ReceiptsAndExpenditureData.AccountActivityDate;
                myWorksheet.Cell(ItemIndex + 1, 1).Value = ReceiptsAndExpenditureData.AccountActivityDate;
                ItemIndex++;
            }
            myWorksheet.Cell(ItemIndex + 1, 2).Value = ReceiptsAndExpenditureData.Content.AccountingSubject.SubjectCode;
            myWorksheet.Cell(ItemIndex + 1, 3).Value = ReceiptsAndExpenditureData.Content.AccountingSubject.Subject;
            myWorksheet.Cell(ItemIndex + 1, 4).Value = ReceiptsAndExpenditureData.Content.Text;
            myWorksheet.Cell(ItemIndex + 1, 5).Value = ReceiptsAndExpenditureData.Detail;
            if (ReceiptsAndExpenditureData.IsPayment) myWorksheet.Cell(ItemIndex + 1, 6).Value = ReceiptsAndExpenditureData.Price;
            else myWorksheet.Cell(ItemIndex + 1, 7).Value = ReceiptsAndExpenditureData.Price;
        }

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
