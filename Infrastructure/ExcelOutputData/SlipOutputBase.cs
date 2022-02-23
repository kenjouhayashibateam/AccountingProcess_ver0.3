using ClosedXML.Excel;
using System.Collections;

namespace Infrastructure.ExcelOutputData
{
    internal abstract class SlipOutputBase : OutputList
    {
        protected SlipOutputBase(IEnumerable outputList) : base(outputList)
        {}

        protected override void PageStyle()
        {
            SetBorderStyle();
            SetCellsStyle();
            SetMargins();
            SetMerge();
            myWorksheet.PageSetup.PageOrientation = XLPageOrientation.Landscape;
        }

        protected override void SetBorderStyle()
        {
            _ = myWorksheet.Style
                .Border.SetLeftBorder(XLBorderStyleValues.None)
                .Border.SetTopBorder(XLBorderStyleValues.None)
                .Border.SetRightBorder(XLBorderStyleValues.None)
                .Border.SetBottomBorder(XLBorderStyleValues.None);
        }

        protected override void SetCellsStyle()
        {
            _ = MySheetCellRange(StartRowPosition + 1, 1, StartRowPosition + 5, 20).Style
                .Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            _ = myWorksheet.Cell(StartRowPosition + 1, 1).Style.
                Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            _ = myWorksheet.Cell(StartRowPosition + 1, 16).Style.
                Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            _ = myWorksheet.Cell(StartRowPosition + 2, 16).Style.
                Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            _ = myWorksheet.Cell(StartRowPosition + 3, 16).Style.
                Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            for (int i = StartRowPosition + 1; i < StartRowPosition + 7; i++)
            {
                _ = myWorksheet.Cell(i, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                _ = myWorksheet.Cell(i, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                _ = myWorksheet.Cell(i, 1).Style.Alignment.SetShrinkToFit(true);
                _ = myWorksheet.Cell(i, 11).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                _ = myWorksheet.Cell(i, 11).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                _ = myWorksheet.Cell(i, 11).Style.Alignment.SetShrinkToFit(true);
            }
            _ = MySheetCellRange(StartRowPosition + 6, 18, StartRowPosition + 6, 20).Style
                .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            _ = MySheetCellRange(StartRowPosition + 9, 4, StartRowPosition + 9, 16).Style
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                .Alignment.SetVertical(XLAlignmentVerticalValues.Bottom)
                .Alignment.SetShrinkToFit(true);
            _ = MySheetCellRange(StartRowPosition + 10, 1, StartRowPosition + 10, 13).Style
                .Alignment.SetVertical(XLAlignmentVerticalValues.Bottom);
            _ = myWorksheet.Cell(StartRowPosition + 10, 1).Style
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            _ = MySheetCellRange(StartRowPosition + 10, 2, StartRowPosition + 10, 3).Style
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            _ = MySheetCellRange(StartRowPosition + 9, 4, StartRowPosition + 10, 13).Style
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
        }

        protected override double[] SetColumnSizes()
        {
            return new double[]
                { 5, 4.71, 5.14, 1.43, 1.29, 1.29, 1.43, 1.29, 1.29, 1.43, 1.29, 1.29, 1.43, 10.29,
                    10.43, 5.29, 0.75, 5.43, 0.83, 4.57 };
        }

        protected override double SetMarginsBottom() { return ToInch(0); }

        protected override double SetMarginsLeft() { return ToInch(2); }

        protected override double SetMarginsRight() { return ToInch(0.5); }

        protected override double SetMarginsTop() { return ToInch(0); }

        protected override void SetMerge()
        {
            _ = MySheetCellRange(StartRowPosition + 1, 1, StartRowPosition + 1, 15).Merge();
            for (int i = 1; i < 6; i++)
            {
                _ = MySheetCellRange(StartRowPosition + i, 1, StartRowPosition + i, 9).Merge();
                _ = MySheetCellRange(StartRowPosition + i, 11, StartRowPosition + i, 15).Merge();
            }
            for (int i = 1; i < 5; i++)
            { _ = MySheetCellRange(StartRowPosition + i, 16, StartRowPosition + i, 20).Merge(); }
            _ = MySheetCellRange(StartRowPosition + 6, 18, StartRowPosition + 7, 18).Merge();
            _ = MySheetCellRange(StartRowPosition + 6, 20, StartRowPosition + 7, 20).Merge();
            _ = MySheetCellRange(StartRowPosition + 9, 4, StartRowPosition + 9, 13).Merge();
            _ = MySheetCellRange(StartRowPosition + 9, 14, StartRowPosition + 9, 15).Merge();
            _ = MySheetCellRange(StartRowPosition + 9, 16, StartRowPosition + 9, 19).Merge();
        }

        protected override double[] SetRowSizes()
        {
            return new double[]
                { 24, 20.25, 20.25, 20.25, 20.25, 20.25, 18.75, 18.75, 9, 30, 30 };
        }

        protected override string SetSheetFontName() { return "ＭＳ 明朝"; }

        protected override void SetSheetStyle() { myWorksheet.Style.Font.FontSize = 11; }

        protected override XLPaperSize SheetPaperSize() { return XLPaperSize.A4Paper; }
    }
}
