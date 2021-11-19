using ClosedXML.Excel;
using Domain.Entities;
using Domain.Entities.Helpers;
using System;
using static Domain.Entities.ValueObjects.MoneyCategory.Denomination;

namespace Infrastructure.ExcelOutputData
{
    /// <summary>
    /// 金庫金額をエクセルに出力するクラス
    /// </summary>
    internal class CashBoxOutput : OutputSingleSheetData
    {
        protected override void SetBorderStyle()
        {
            _ = MySheetCellRange(1, 5, 2, 7).Style
                .Border.SetBottomBorder(XLBorderStyleValues.Thin)
                .Border.SetTopBorder(XLBorderStyleValues.Thin)
                .Border.SetLeftBorder(XLBorderStyleValues.Thin)
                .Border.SetRightBorder(XLBorderStyleValues.Thin);

            _ = MySheetCellRange(3, 1, 12, 3).Style
                .Border.SetBottomBorder(XLBorderStyleValues.Thin)
                .Border.SetTopBorder(XLBorderStyleValues.Thin)
                .Border.SetLeftBorder(XLBorderStyleValues.Thin)
                .Border.SetRightBorder(XLBorderStyleValues.Thin);

            _ = MySheetCellRange(6, 4, 12, 7).Style
                .Border.SetBottomBorder(XLBorderStyleValues.Thin)
                .Border.SetTopBorder(XLBorderStyleValues.Thin)
                .Border.SetLeftBorder(XLBorderStyleValues.Thin)
                .Border.SetRightBorder(XLBorderStyleValues.Thin);

            _ = MySheetCellRange(15, 1, 19, 7).Style
                .Border.SetBottomBorder(XLBorderStyleValues.Thin)
                .Border.SetTopBorder(XLBorderStyleValues.Thin)
                .Border.SetLeftBorder(XLBorderStyleValues.Thin)
                .Border.SetRightBorder(XLBorderStyleValues.Thin);
            _ = MySheetCellRange(6, 4, 12, 4).Style.Border.SetLeftBorder(XLBorderStyleValues.Double);
        }

        protected override void SetCellsStyle()
        {
            _ = myWorksheet.Cell(1, 1).Style
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            _ = MySheetCellRange(1, 5, 2, 7).Style
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            _ = MySheetCellRange(3, 1, 3, 3).Style
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            _ = MySheetCellRange(4, 1, 12, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            _ = MySheetCellRange(4, 1, 12, 3).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            _ = MySheetCellRange(4, 2, 12, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            _ = MySheetCellRange(4, 3, 12, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            _ = MySheetCellRange(6, 4, 6, 6).Style
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            _ = MySheetCellRange(7, 4, 12, 4).Style
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            _ = MySheetCellRange(7, 5, 12, 12).Style.Alignment.Horizontal =
                XLAlignmentHorizontalValues.Center;
            _ = MySheetCellRange(7, 5, 12, 12).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            _ = MySheetCellRange(7, 6, 12, 6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            _ = MySheetCellRange(7, 6, 12, 6).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            _ = myWorksheet.Cell(14, 1).Style
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            _ = MySheetCellRange(15, 1, 19, 7).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            _ = MySheetCellRange(15, 1, 15, 7).Style.Alignment.Horizontal =
                XLAlignmentHorizontalValues.Center;
            _ = MySheetCellRange(16, 1, 19, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            _ = MySheetCellRange(16, 2, 19, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            _ = MySheetCellRange(16, 4, 19, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            _ = MySheetCellRange(16, 5, 19, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            _ = myWorksheet.Cell(21, 1).Style
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        }

        protected override double[] SetColumnSizes()
        {
            return new double[] { 12.86, 6.88, 14.38, 12.86, 6.75, 6.75, 6.75 };
        }

        protected override void SetDataStrings()
        {
            Cashbox myCashbox = Cashbox.GetInstance();

            myWorksheet.Cell(1, 1).Value = DateTime.Now.ToString("yyyy年MM月dd日");

            myWorksheet.Cell(1, 5).Value = "本部長";
            myWorksheet.Cell(1, 6).Value = "副住職";
            myWorksheet.Cell(1, 7).Value = "係";
            LoginRep loginRep = LoginRep.GetInstance();
            if (loginRep.Rep != null)
            {
                myWorksheet.Cell(2, 7).Value = TextHelper.GetFirstName(loginRep.Rep.Name);
            }

            myWorksheet.Cell(3, 1).Value = "金種";
            myWorksheet.Cell(3, 2).Value = "数量";
            myWorksheet.Cell(3, 3).Value = "金額";

            myWorksheet.Cell(4, 1).Value = "1万円";
            myWorksheet.Cell(5, 1).Value = "5千円";
            myWorksheet.Cell(6, 1).Value = "1千円";
            myWorksheet.Cell(7, 1).Value = "500円";
            myWorksheet.Cell(8, 1).Value = "100円";
            myWorksheet.Cell(9, 1).Value = "50円";
            myWorksheet.Cell(10, 1).Value = "10円";
            myWorksheet.Cell(11, 1).Value = "5円";
            myWorksheet.Cell(12, 1).Value = "1円";

            myWorksheet.Cell(6, 4).Value = $"{Cashbox.BundleCount}枚束";
            myWorksheet.Cell(6, 5).Value = "数量";
            myWorksheet.Cell(6, 6).Value = "金額";

            myWorksheet.Cell(7, 4).Value = "500円";
            myWorksheet.Cell(8, 4).Value = "100円";
            myWorksheet.Cell(9, 4).Value = "50円";
            myWorksheet.Cell(10, 4).Value = "10円";
            myWorksheet.Cell(11, 4).Value = "5円";
            myWorksheet.Cell(12, 4).Value = "1円";

            myWorksheet.Cell(4, 2).Value = myCashbox.MoneyCategorys[TenThousandYen].Count;
            myWorksheet.Cell(5, 2).Value = myCashbox.MoneyCategorys[FiveThousandYen].Count;
            myWorksheet.Cell(6, 2).Value = myCashbox.MoneyCategorys[OneThousandYen].Count;
            myWorksheet.Cell(7, 2).Value = myCashbox.MoneyCategorys[FiveHundredYen].Count;
            myWorksheet.Cell(8, 2).Value = myCashbox.MoneyCategorys[OneHundredYen].Count;
            myWorksheet.Cell(9, 2).Value = myCashbox.MoneyCategorys[FiftyYen].Count;
            myWorksheet.Cell(10, 2).Value = myCashbox.MoneyCategorys[TenYen].Count;
            myWorksheet.Cell(11, 2).Value = myCashbox.MoneyCategorys[FiveYen].Count;
            myWorksheet.Cell(12, 2).Value = myCashbox.MoneyCategorys[OneYen].Count;

            myWorksheet.Cell(4, 3).Value =
                myCashbox.MoneyCategorys[TenThousandYen].AmountWithUnit();
            myWorksheet.Cell(5, 3).Value =
                myCashbox.MoneyCategorys[FiveThousandYen].AmountWithUnit();
            myWorksheet.Cell(6, 3).Value =
                myCashbox.MoneyCategorys[OneThousandYen].AmountWithUnit();
            myWorksheet.Cell(7, 3).Value =
                myCashbox.MoneyCategorys[FiveHundredYen].AmountWithUnit();
            myWorksheet.Cell(8, 3).Value =
                myCashbox.MoneyCategorys[OneHundredYen].AmountWithUnit();
            myWorksheet.Cell(9, 3).Value =
                myCashbox.MoneyCategorys[FiftyYen].AmountWithUnit();
            myWorksheet.Cell(10, 3).Value =
                myCashbox.MoneyCategorys[TenYen].AmountWithUnit();
            myWorksheet.Cell(11, 3).Value =
                myCashbox.MoneyCategorys[FiveYen].AmountWithUnit();
            myWorksheet.Cell(12, 3).Value =
                myCashbox.MoneyCategorys[OneYen].AmountWithUnit();

            myWorksheet.Cell(7, 5).Value =
                myCashbox.MoneyCategorys[FiveHundredYenBundle].Count;
            myWorksheet.Cell(8, 5).Value =
                myCashbox.MoneyCategorys[OneHundredYenBundle].Count;
            myWorksheet.Cell(9, 5).Value = myCashbox.MoneyCategorys[FiftyYenBundle].Count;
            myWorksheet.Cell(10, 5).Value = myCashbox.MoneyCategorys[TenYenBundle].Count;
            myWorksheet.Cell(11, 5).Value = myCashbox.MoneyCategorys[FiveYenBundle].Count;
            myWorksheet.Cell(12, 5).Value = myCashbox.MoneyCategorys[OneYenBundle].Count;

            myWorksheet.Cell(7, 6).Value =
                myCashbox.MoneyCategorys[FiveHundredYenBundle].AmountWithUnit();
            myWorksheet.Cell(8, 6).Value =
                myCashbox.MoneyCategorys[OneHundredYenBundle].AmountWithUnit();
            myWorksheet.Cell(9, 6).Value = myCashbox.MoneyCategorys[FiftyYenBundle].AmountWithUnit();
            myWorksheet.Cell(10, 6).Value = myCashbox.MoneyCategorys[TenYenBundle].AmountWithUnit();
            myWorksheet.Cell(11, 6).Value = myCashbox.MoneyCategorys[FiveYenBundle].AmountWithUnit();
            myWorksheet.Cell(12, 6).Value = myCashbox.MoneyCategorys[OneYenBundle].AmountWithUnit();

            myWorksheet.Cell(14, 1).Value = "釣り銭等";
            myWorksheet.Cell(15, 1).Value = "内容";
            myWorksheet.Cell(15, 2).Value = "金額";
            myWorksheet.Cell(15, 4).Value = "内容";
            myWorksheet.Cell(15, 5).Value = "金額";

            for (int i = 0; i < myCashbox.OtherMoneys.Length; i++)
            {
                if (i < 4)
                {
                    myWorksheet.Cell(16 + i, 1).Value = myCashbox.OtherMoneys[i].Title;
                    myWorksheet.Cell(16 + i, 2).Value =
                        TextHelper.AmountWithUnit(myCashbox.OtherMoneys[i].Amount);
                }
                else
                {
                    myWorksheet.Cell(16 + (i - 4), 4).Value = myCashbox.OtherMoneys[i].Title;
                    myWorksheet.Cell(16 + (i - 4), 5).Value =
                        TextHelper.AmountWithUnit(myCashbox.OtherMoneys[i].Amount);
                }
            }

            myWorksheet.Cell(21, 1).Value = $"合計　{myCashbox.GetTotalAmountWithUnit()}";
        }

        protected override double SetMarginsBottom() { return ToInch(1.91); }

        protected override double SetMarginsLeft() { return ToInch(1.78); }

        protected override double SetMarginsRight() { return ToInch(1.78); }

        protected override double SetMarginsTop() { return ToInch(1.91); }

        protected override void SetMerge()
        {
            _ = MySheetCellRange(1, 1, 1, 3).Merge();
            _ = MySheetCellRange(6, 6, 6, 7).Merge();
            _ = MySheetCellRange(7, 6, 7, 7).Merge();
            _ = MySheetCellRange(8, 6, 8, 7).Merge();
            _ = MySheetCellRange(9, 6, 9, 7).Merge();
            _ = MySheetCellRange(10, 6, 10, 7).Merge();
            _ = MySheetCellRange(11, 6, 11, 7).Merge();
            _ = MySheetCellRange(12, 6, 12, 7).Merge();

            _ = MySheetCellRange(15, 2, 15, 3).Merge();
            _ = MySheetCellRange(16, 2, 16, 3).Merge();
            _ = MySheetCellRange(17, 2, 17, 3).Merge();
            _ = MySheetCellRange(18, 2, 18, 3).Merge();
            _ = MySheetCellRange(19, 2, 19, 3).Merge();

            _ = MySheetCellRange(15, 5, 15, 7).Merge();
            _ = MySheetCellRange(16, 5, 16, 7).Merge();
            _ = MySheetCellRange(17, 5, 17, 7).Merge();
            _ = MySheetCellRange(18, 5, 18, 7).Merge();
            _ = MySheetCellRange(19, 5, 19, 7).Merge();
            _ = MySheetCellRange(21, 1, 21, 7).Merge();
        }

        protected override double[] SetRowSizes()
        {
            return new double[] 
                { 18.75, 41.25, 18.75, 18.75, 18.75, 18.75, 18.75, 18.75, 18.75, 18.75, 18.75, 18.75,
                                    18.75, 18.75, 18.75, 18.75, 18.75, 18.75, 18.75, 18.75, 32.25 };
        }

        protected override XLPaperSize SheetPaperSize() { return XLPaperSize.B5Paper; }

        protected override void SetSheetStyle()
        {
            MySheetCellRange(1, 1, 20, 7).Style.NumberFormat.Format = "@";
            myWorksheet.Cell(21, 1).Style.Font.Bold = true;
            myWorksheet.Cell(21, 1).Style.Font.FontSize = 20;
        }

        protected override string SetSheetFontName() { return "ＭＳ ゴシック"; }
    }
}
