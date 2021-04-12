using ClosedXML.Excel;
using Domain.Entities;
using Domain.Entities.Helpers;
using System;

namespace Infrastructure.ExcelOutputData
{
    /// <summary>
    /// 受納証出力クラス
    /// </summary>
    internal class VoucherOutput : OutputSingleSheetData
    {
        private readonly Voucher VoucherData;

        public VoucherOutput(Voucher voucher)
        {
            VoucherData = voucher;
        }

        protected override void SetBorderStyle()
        {
            myWorksheet.Style
                .Border.SetLeftBorder(XLBorderStyleValues.None)
                .Border.SetTopBorder(XLBorderStyleValues.None)
                .Border.SetRightBorder(XLBorderStyleValues.None)
                .Border.SetBottomBorder(XLBorderStyleValues.None);
            MySheetCellRange(4, 1, 4, 4).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin);
            MySheetCellRange(6, 2, 6, 7).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin);
            MySheetCellRange(12, 2, 12, 7).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin);
            MySheetCellRange(17, 6, 19, 8).Style
                .Border.SetRightBorder(XLBorderStyleValues.Thin)
                .Border.SetLeftBorder(XLBorderStyleValues.Thin)
                .Border.SetBottomBorder(XLBorderStyleValues.Thin)
                .Border.SetDiagonalBorder(XLBorderStyleValues.Thin)
                .Border.SetTopBorder(XLBorderStyleValues.Thin);
        }

        protected override void SetCellsStyle()
        {
            myWorksheet.Cell(1, 1).Style
                .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                .Font.SetFontSize(26)
                .Font.SetBold(true);
            myWorksheet.Cell(3, 5).Style
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                .Font.SetFontSize(11);
            MySheetCellRange(4, 1, 4, 4).Style
                .Alignment.SetVertical(XLAlignmentVerticalValues.Bottom)
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                .Font.SetFontSize(18);
            myWorksheet.Cell(5, 2).Style
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                .Alignment.SetVertical(XLAlignmentVerticalValues.Bottom)
                .Font.SetFontSize(11);
            myWorksheet.Cell(6, 4).Style
                .Alignment.SetVertical(XLAlignmentVerticalValues.Bottom)
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                .Font.SetFontSize(24);
            myWorksheet.Cell(6, 7).Style
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                .Alignment.SetVertical(XLAlignmentVerticalValues.Bottom)
                .Font.SetFontSize(11);
            myWorksheet.Cell(7, 2).Style
                .Alignment.SetVertical(XLAlignmentVerticalValues.Bottom)
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                .Font.SetFontSize(11);
            MySheetCellRange(8, 2, 11, 8).Style
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                .Font.SetFontSize(11);
            myWorksheet.Cell(12, 2).Style
                .Alignment.SetVertical(XLAlignmentVerticalValues.Bottom)
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                .Font.SetFontSize(11);
            myWorksheet.Cell(15, 1).Style
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                .Alignment.SetVertical(XLAlignmentVerticalValues.Bottom)
                .Font.SetFontSize(10);
            myWorksheet.Cell(14, 5).Style
                .Alignment.SetVertical(XLAlignmentVerticalValues.Bottom)
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Distributed)
                .Font.SetFontSize(18);
            myWorksheet.Cell(15, 8).Style
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                .Font.SetFontSize(11);
            myWorksheet.Cell(16, 1).Style
                .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                .Font.SetFontSize(10);
            myWorksheet.Cell(16, 3).Style
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                .Font.SetFontSize(10);
            myWorksheet.Cell(16, 8).Style
                .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                .Font.SetFontSize(10);
            myWorksheet.Cell(17, 2).Style
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                .Font.SetFontSize(10);
        }

        protected override double[] SetColumnSizes() => new double[] { 3.13, 6.88, 3.13, 1.38, 6.88, 12.63, 3.13, 3.13, 3.5 };

        protected override void SetDataStrings()
        {
            myWorksheet.Cell(1, 1).Value = "受　納　証";
            myWorksheet.Cell(2, 6).Value = DateTime.Today.ToString("d");
            myWorksheet.Cell(3, 1).Value = VoucherData.Addressee;
            myWorksheet.Cell(3, 5).Value = "様";
            myWorksheet.Cell(4, 2).Value = "冥加金";
            myWorksheet.Cell(5, 3).Value = $"{TextHelper.CommaDelimitedAmount(VoucherData.TotalAmount)}-";
            myWorksheet.Cell(5, 7).Value = "円也";
            myWorksheet.Cell(6, 2).Value = "但し";
            if (VoucherData.ReceiptsAndExpenditures.Count < 5) SingleLineOutput();
        }

        private void SingleLineOutput()
        {
            int i = VoucherData.ReceiptsAndExpenditures.Count - 1;
            foreach(ReceiptsAndExpenditure rae in VoucherData.ReceiptsAndExpenditures)
            {
                myWorksheet.Cell((10 - i), 2).Value = $"{rae.Content.Text} : {rae.PriceWithUnit}";
                i--;
            }
        }

        private void MultipleLineOutput()
        {
            int i = VoucherData.ReceiptsAndExpenditures.Count - 1;
            int j = 4;
            foreach(ReceiptsAndExpenditure rae in VoucherData.ReceiptsAndExpenditures)
            {
            }
        }

        protected override double SetMaeginsBottom()
        {
            throw new NotImplementedException();
        }

        protected override double SetMaeginsLeft()
        {
            throw new NotImplementedException();
        }

        protected override double SetMaeginsRight()
        {
            throw new NotImplementedException();
        }

        protected override double SetMaeginsTop()
        {
            throw new NotImplementedException();
        }

        protected override void SetMerge()
        {
            throw new NotImplementedException();
        }

        protected override double[] SetRowSizes()
        {
            throw new NotImplementedException();
        }

        protected override string SetSheetFontName()
        {
            throw new NotImplementedException();
        }

        protected override void SetSheetFontStyle()
        {
            throw new NotImplementedException();
        }

        protected override XLPaperSize SheetPaperSize()
        {
            throw new NotImplementedException();
        }
    }
}
