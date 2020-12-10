using ClosedXML.Excel;
using Domain.Entities;
using System;
using static Domain.Entities.ValueObjects.MoneyCategory.Denomination;

namespace Infrastructure.ExcelOutputData
{
    /// <summary>
    /// 金庫金額をエクセルに出力するクラス
    /// </summary>
    internal class CashBoxOutput : OutputData
    {
        protected override void SetBorderStyle()
        {
            myWorksheet.Range(myWorksheet.Cell(1, 5), myWorksheet.Cell(2, 7)).Style
                .Border.SetBottomBorder(XLBorderStyleValues.Thin)
                .Border.SetTopBorder(XLBorderStyleValues.Thin)
                .Border.SetLeftBorder(XLBorderStyleValues.Thin)
                .Border.SetRightBorder(XLBorderStyleValues.Thin);

            myWorksheet.Range(myWorksheet.Cell(3, 1), myWorksheet.Cell(12, 3)).Style
                .Border.SetBottomBorder(XLBorderStyleValues.Thin)
                .Border.SetTopBorder(XLBorderStyleValues.Thin)
                .Border.SetLeftBorder(XLBorderStyleValues.Thin)
                .Border.SetRightBorder(XLBorderStyleValues.Thin);

            myWorksheet.Range(myWorksheet.Cell(6, 4), myWorksheet.Cell(12, 7)).Style
                .Border.SetBottomBorder(XLBorderStyleValues.Thin)
                .Border.SetTopBorder(XLBorderStyleValues.Thin)
                .Border.SetLeftBorder(XLBorderStyleValues.Thin)
                .Border.SetRightBorder(XLBorderStyleValues.Thin);

            myWorksheet.Range(myWorksheet.Cell(15, 1), myWorksheet.Cell(19, 7)).Style
                .Border.SetBottomBorder(XLBorderStyleValues.Thin)
                .Border.SetTopBorder(XLBorderStyleValues.Thin)
                .Border.SetLeftBorder(XLBorderStyleValues.Thin)
                .Border.SetRightBorder(XLBorderStyleValues.Thin);
            myWorksheet.Range(myWorksheet.Cell(15, 4), myWorksheet.Cell(19, 4)).Style.Border.SetLeftBorder(XLBorderStyleValues.Double);
        }

        protected override void SetCellsAlignment()
        {
            myWorksheet.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            myWorksheet.Cell(1, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            myWorksheet.Range(myWorksheet.Cell(1, 5), myWorksheet.Cell(1, 7)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            myWorksheet.Range(myWorksheet.Cell(1, 5), myWorksheet.Cell(1, 7)).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            myWorksheet.Range(myWorksheet.Cell(3, 1), myWorksheet.Cell(3, 3)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            myWorksheet.Range(myWorksheet.Cell(3, 1), myWorksheet.Cell(3, 3)).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            myWorksheet.Range(myWorksheet.Cell(4, 1), myWorksheet.Cell(12, 1)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            myWorksheet.Range(myWorksheet.Cell(4, 1), myWorksheet.Cell(12, 3)).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            myWorksheet.Range(myWorksheet.Cell(4, 2), myWorksheet.Cell(12, 2)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            myWorksheet.Range(myWorksheet.Cell(4, 3), myWorksheet.Cell(12, 3)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            myWorksheet.Range(myWorksheet.Cell(6, 4), myWorksheet.Cell(6, 6)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            myWorksheet.Range(myWorksheet.Cell(6, 4), myWorksheet.Cell(6, 6)).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            myWorksheet.Range(myWorksheet.Cell(7, 4), myWorksheet.Cell(12, 4)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            myWorksheet.Range(myWorksheet.Cell(7, 4), myWorksheet.Cell(12, 4)).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            myWorksheet.Range(myWorksheet.Cell(7, 5), myWorksheet.Cell(12, 12)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            myWorksheet.Range(myWorksheet.Cell(7, 5), myWorksheet.Cell(12, 12)).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            myWorksheet.Range(myWorksheet.Cell(7, 6), myWorksheet.Cell(12, 6)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            myWorksheet.Range(myWorksheet.Cell(7, 6), myWorksheet.Cell(12, 6)).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            myWorksheet.Cell(14, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            myWorksheet.Cell(14, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            myWorksheet.Range(myWorksheet.Cell(15, 1), myWorksheet.Cell(19, 7)).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            myWorksheet.Range(myWorksheet.Cell(15, 1), myWorksheet.Cell(15, 7)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            myWorksheet.Range(myWorksheet.Cell(16, 1), myWorksheet.Cell(19, 1)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            myWorksheet.Range(myWorksheet.Cell(16, 2), myWorksheet.Cell(19, 2)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            myWorksheet.Range(myWorksheet.Cell(16, 4), myWorksheet.Cell(19, 4)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            myWorksheet.Range(myWorksheet.Cell(16, 5), myWorksheet.Cell(19, 5)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            myWorksheet.Cell(21, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            myWorksheet.Cell(21, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        }

        protected override double[] SetColumnSizes() => new Double[] { 12.86, 6.88, 14.38, 12.86, 6.75, 6.75, 6.75 };

        protected override void SetDataStrings()
        {
            Cashbox myCashbox = Cashbox.GetInstance();

            myWorksheet.Cell(1, 1).Value = DateTime.Now.ToString("yyyy年MM月dd日");

            myWorksheet.Cell(1, 5).Value = "本部長";
            myWorksheet.Cell(1, 6).Value = "副住職";
            myWorksheet.Cell(1, 7).Value = "係";

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

            myWorksheet.Cell(4, 3).Value = myCashbox.MoneyCategorys[TenThousandYen].AmountWithUnit();
            myWorksheet.Cell(5, 3).Value = myCashbox.MoneyCategorys[FiveThousandYen].AmountWithUnit();
            myWorksheet.Cell(6, 3).Value = myCashbox.MoneyCategorys[OneThousandYen].AmountWithUnit();
            myWorksheet.Cell(7, 3).Value = myCashbox.MoneyCategorys[FiveHundredYen].AmountWithUnit();
            myWorksheet.Cell(8, 3).Value = myCashbox.MoneyCategorys[OneHundredYen].AmountWithUnit();
            myWorksheet.Cell(9, 3).Value = myCashbox.MoneyCategorys[FiftyYen].AmountWithUnit();
            myWorksheet.Cell(10, 3).Value = myCashbox.MoneyCategorys[TenYen].AmountWithUnit();
            myWorksheet.Cell(11, 3).Value = myCashbox.MoneyCategorys[FiveYen].AmountWithUnit();
            myWorksheet.Cell(12, 3).Value = myCashbox.MoneyCategorys[OneYen].AmountWithUnit();

            myWorksheet.Cell(7, 5).Value = myCashbox.MoneyCategorys[FiveHundredYenBundle].Count;
            myWorksheet.Cell(8, 5).Value = myCashbox.MoneyCategorys[OneHundredYenBundle].Count;
            myWorksheet.Cell(9, 5).Value = myCashbox.MoneyCategorys[FiftyYenBundle].Count;
            myWorksheet.Cell(10, 5).Value = myCashbox.MoneyCategorys[TenYenBundle].Count;
            myWorksheet.Cell(11, 5).Value = myCashbox.MoneyCategorys[FiveYenBundle].Count;
            myWorksheet.Cell(12, 5).Value = myCashbox.MoneyCategorys[OneYenBundle].Count;

            myWorksheet.Cell(7, 6).Value = myCashbox.MoneyCategorys[FiveHundredYenBundle].AmountWithUnit();
            myWorksheet.Cell(8, 6).Value = myCashbox.MoneyCategorys[OneHundredYenBundle].AmountWithUnit();
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
                    myWorksheet.Cell(16 + i, 2).Value = myCashbox.OtherMoneys[i].AmountWithUnit();
                }
                else
                {
                    myWorksheet.Cell(16 + (i - 4), 4).Value = myCashbox.OtherMoneys[i].Title;
                    myWorksheet.Cell(16 + (i - 4), 5).Value = myCashbox.OtherMoneys[i].AmountWithUnit();
                }
            }
            
            myWorksheet.Cell(21, 1).Value = $"合計　{myCashbox.GetTotalAmountWithUnit()}";
        }

        protected override double SetMaeginsBottom() => ToInch(1.91);

        protected override double SetMaeginsLeft() => ToInch(1.78);

        protected override double SetMaeginsRight() => ToInch(1.78);

        protected override double SetMaeginsTop() => ToInch(1.91);

        protected override void SetMerge()
        {
            myWorksheet.Range(myWorksheet.Cell(1, 1), myWorksheet.Cell(1, 3)).Merge();
            myWorksheet.Range(myWorksheet.Cell(6, 6), myWorksheet.Cell(6, 7)).Merge();
            myWorksheet.Range(myWorksheet.Cell(7, 6), myWorksheet.Cell(7, 7)).Merge();
            myWorksheet.Range(myWorksheet.Cell(8, 6), myWorksheet.Cell(8, 7)).Merge();
            myWorksheet.Range(myWorksheet.Cell(9, 6), myWorksheet.Cell(9, 7)).Merge();
            myWorksheet.Range(myWorksheet.Cell(10, 6), myWorksheet.Cell(10, 7)).Merge();
            myWorksheet.Range(myWorksheet.Cell(11, 6), myWorksheet.Cell(11, 7)).Merge();
            myWorksheet.Range(myWorksheet.Cell(12, 6), myWorksheet.Cell(12, 7)).Merge();

            myWorksheet.Range(myWorksheet.Cell(15, 2), myWorksheet.Cell(15, 3)).Merge();
            myWorksheet.Range(myWorksheet.Cell(16, 2), myWorksheet.Cell(16, 3)).Merge();
            myWorksheet.Range(myWorksheet.Cell(17, 2), myWorksheet.Cell(17, 3)).Merge();
            myWorksheet.Range(myWorksheet.Cell(18, 2), myWorksheet.Cell(18, 3)).Merge();
            myWorksheet.Range(myWorksheet.Cell(19, 2), myWorksheet.Cell(19, 3)).Merge();

            myWorksheet.Range(myWorksheet.Cell(15, 5), myWorksheet.Cell(15, 7)).Merge();
            myWorksheet.Range(myWorksheet.Cell(16, 5), myWorksheet.Cell(16, 7)).Merge();
            myWorksheet.Range(myWorksheet.Cell(17, 5), myWorksheet.Cell(17, 7)).Merge();
            myWorksheet.Range(myWorksheet.Cell(18, 5), myWorksheet.Cell(18, 7)).Merge();
            myWorksheet.Range(myWorksheet.Cell(19, 5), myWorksheet.Cell(19, 7)).Merge();
            myWorksheet.Range(myWorksheet.Cell(21, 1), myWorksheet.Cell(21, 7)).Merge();
        }

        protected override double[] SetRowSizes() => new Double[] { 18.75, 41.25, 18.75, 18.75, 18.75, 18.75, 18.75, 18.75, 18.75, 18.75, 18.75, 18.75, 18.75, 18.75, 18.75, 18.75, 18.75, 18.75, 18.75, 18.75, 32.25 };

        protected override string SetSheetFontName() => "ＭＳ ゴシック";

        protected override XLPaperSize SheetPaperSize() => XLPaperSize.B5Paper;        

        protected override void SetSheetFontStyle()
        {
            myWorksheet.Range(myWorksheet.Cell(1, 1), myWorksheet.Cell(20, 7)).Style.NumberFormat.Format = "@";
            myWorksheet.Cell(21, 1).Style.Font.Bold = true;
            myWorksheet.Cell(21, 1).Style.Font.FontSize = 20;
        }
    }
}
