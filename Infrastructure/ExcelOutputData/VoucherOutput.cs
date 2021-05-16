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

        public VoucherOutput(Voucher voucher) => VoucherData = voucher;        

        private int CopyColumnPosition(int originalColumn) =>
            originalColumn + SetColumnSizes().Length / 2 + 1;

        protected override void SetBorderStyle()
        {
            //ボーダーをすべて消去
            myWorksheet.Style
                .Border.SetLeftBorder(XLBorderStyleValues.None)
                .Border.SetTopBorder(XLBorderStyleValues.None)
                .Border.SetRightBorder(XLBorderStyleValues.None)
                .Border.SetBottomBorder(XLBorderStyleValues.None);
            //宛名欄
            SetBottomBorderOriginalAndCopy(3, 1, 3, 5);
            //総額欄
            SetBottomBorderOriginalAndCopy(5, 2, 5, 8);
            //但し書き欄
            SetBottomBorderOriginalAndCopy(10, 2, 10, 8);
            //係印欄
            SetClerkMarkField(14, 8, 16, 9);
            SetClerkMarkField(14, CopyColumnPosition(8), 16, CopyColumnPosition(9));

            void SetBottomBorderOriginalAndCopy(int row1,int column1,int row2,int column2)
            {
                SetBottomBorderThin(row1, column1, row2, column2);
                SetBottomBorderThin(row1, CopyColumnPosition(column1), row2, CopyColumnPosition(column2));
            }
            void SetBottomBorderThin(int row1, int column1, int row2, int column2) =>
                MySheetCellRange(row1, column1, row2, column2).Style.
                    Border.SetBottomBorder(XLBorderStyleValues.Thin);

            void SetClerkMarkField(int row1, int column1, int row2, int column2)
            {
                MySheetCellRange(row1, column1, row2, column2).Style
                    .Border.SetBottomBorder(XLBorderStyleValues.Thin)
                    .Border.SetRightBorder(XLBorderStyleValues.Thin)
                    .Border.SetLeftBorder(XLBorderStyleValues.Thin)
                    .Border.SetDiagonalBorder(XLBorderStyleValues.Thin)
                    .Border.SetTopBorder(XLBorderStyleValues.Thin);
                MySheetCellRange(14, CopyColumnPosition(8), 16, CopyColumnPosition(9)).Style
                    .Border.SetBottomBorder(XLBorderStyleValues.Thin)
                    .Border.SetRightBorder(XLBorderStyleValues.Thin)
                    .Border.SetLeftBorder(XLBorderStyleValues.Thin)
                    .Border.SetDiagonalBorder(XLBorderStyleValues.Thin)
                    .Border.SetTopBorder(XLBorderStyleValues.Thin);
            }
        }

        protected override void SetCellsStyle()
        {
            XLAlignmentHorizontalValues horizontal;
            XLAlignmentVerticalValues vertical;
            int fontSize;

            //タイトル欄
            SetLocalProperty(XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, 26);
            SetCellPropertyOriginalAndCopy(1, 1);
            //日付欄
            SetLocalProperty(XLAlignmentHorizontalValues.Right, XLAlignmentVerticalValues.Center, 11);
            SetCellPropertyOriginalAndCopy(2, 6);
            //宛名欄
            SetLocalProperty(XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Bottom, 18);
            SetRangeProperyOriginalAndCopy(3, 1, 3, 5);
            myWorksheet.Cell(3, 1).Style.Alignment.SetShrinkToFit(true);
            myWorksheet.Cell(3, CopyColumnPosition(1)).Style.Alignment.SetShrinkToFit(true);
            //冥加金文字列
            SetLocalProperty(XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Bottom, 11);
            SetCellPropertyOriginalAndCopy(4, 2);
            //総額欄
            SetLocalProperty(XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Bottom, 24);
            SetCellPropertyOriginalAndCopy(5, 3);
            //円也文字列
            SetLocalProperty(XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Bottom, 11);
            SetCellPropertyOriginalAndCopy(5, 7);
            //但し文字列
            SetLocalProperty(XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Bottom, 11);
            SetCellPropertyOriginalAndCopy(6, 2);
            //但し書き欄
            SetLocalProperty(XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, 14);
            SetRangeProperyOriginalAndCopy(7, 2, 10, 8);
            MySheetCellRange(7, 2, 10, 8).Style.Alignment.SetShrinkToFit(true);
            MySheetCellRange(7, CopyColumnPosition(2), 10, CopyColumnPosition(8)).Style
                .Alignment.SetShrinkToFit(true);
            //上記有難くお受けしました文字列
            SetLocalProperty(XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Bottom, 11);
            SetCellPropertyOriginalAndCopy(11, 2);
            //宗派、法人名文字列
            SetLocalProperty(XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Bottom, 10);
            SetCellPropertyOriginalAndCopy(14, 1);
            //団体名文字列
            SetLocalProperty(XLAlignmentHorizontalValues.Distributed, XLAlignmentVerticalValues.Bottom, 18);
            SetCellPropertyOriginalAndCopy(13, 5);
            //係文字列、係印欄
            SetLocalProperty(XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, 11);
            SetRangeProperyOriginalAndCopy(14, 8, 16, 9);
            //郵便番号欄
            SetLocalProperty(XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, 10);
            SetCellPropertyOriginalAndCopy(15, 1);
            //住所欄
            SetLocalProperty(XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, 10);
            SetCellPropertyOriginalAndCopy(15, 3);
            //電話番号欄
            SetLocalProperty(XLAlignmentHorizontalValues.Right, XLAlignmentVerticalValues.Center, 10);
            SetCellPropertyOriginalAndCopy(16, 2);

            void SetLocalProperty(XLAlignmentHorizontalValues horizontalValues,
                                                XLAlignmentVerticalValues verticalValues,int size)
            {
                horizontal = horizontalValues;
                vertical = verticalValues;
                fontSize = size;
            }

            void SetCellPropertyOriginalAndCopy(int row,int column)
            {
                SetAlignmentAndFontSize(row, column);
                SetAlignmentAndFontSize(row, CopyColumnPosition(column));
            }

            void SetRangeProperyOriginalAndCopy(int row1,int column1,int row2,int column2)
            {
                SetRangeAlignmentAndFontSize(row1, column1, row2, column2);
                SetRangeAlignmentAndFontSize(row1, CopyColumnPosition(column1), row2,
                                                                    CopyColumnPosition(column2));
            }

            void SetRangeAlignmentAndFontSize(int row1, int column1, int row2, int column2) =>
                MySheetCellRange(row1, column1, row2, column2).Style
                    .Alignment.SetHorizontal(horizontal)
                    .Alignment.SetVertical(vertical)
                    .Font.SetFontSize(fontSize);

            void SetAlignmentAndFontSize(int row, int column) =>
                myWorksheet.Cell(row, column).Style
                    .Alignment.SetHorizontal(horizontal)
                    .Alignment.SetVertical(vertical)
                    .Font.SetFontSize(fontSize);
        }

        protected override double[] SetColumnSizes() => new double[]
        { 3.13, 6.88, 3.13, 1.38, 6.88, 12.63, 3.13, 3.13, 3.5, 14.86,
            3.13, 6.88, 3.13, 1.38, 6.88, 12.63, 3.13, 3.13, 3.5 };

        protected override void SetDataStrings()
        {
            //タイトル
            SetStringOriginalAndCopy(1, 1, "受　納　証");
            //日付
            SetStringOriginalAndCopy(2, 6, DateTime.Now.ToString("yyyy年MM月dd日"));
            //宛名
            SetStringOriginalAndCopy(3, 1, $"{VoucherData.Addressee}　様");
            //総額
            SetStringOriginalAndCopy(4, 2, "冥加金");
            SetStringOriginalAndCopy(5, 3, 
                $"{TextHelper.CommaDelimitedAmount(VoucherData.TotalAmount)}-");
            SetStringOriginalAndCopy(5, 7, "円也");
            SetStringOriginalAndCopy(6, 2, "但し");
            //但し書き
            if (VoucherData.ReceiptsAndExpenditures.Count < 5) SingleLineOutput();
            else MultipleLineOutput();
            SetStringOriginalAndCopy(11, 2, "上記有難くお受けいたしました");
            //団体名、電話番号  
            if (VoucherData.ReceiptsAndExpenditures[0].CreditDept.Dept == "春秋苑")
            {
                SetStringOriginalAndCopy(14, 1, "宗教法人信行寺");
                SetStringOriginalAndCopy(13, 5, "春秋苑");
                SetStringOriginalAndCopy(16, 2, "電話０４４－９７６－０１１５㈹");
            }
            else
            {
                SetStringOriginalAndCopy(14, 1, "浄土真宗本願寺派");
                SetStringOriginalAndCopy(13, 5, "信行寺");
                SetStringOriginalAndCopy(16, 2, "電話０４４－９７７－３４６６㈹");
            }
            //郵便番号
            SetStringOriginalAndCopy(15, 1, "〒214-0036");
            //住所
            SetStringOriginalAndCopy(15, 3, "川崎市多摩区南生田８－１－１");
            //係
            SetStringOriginalAndCopy(14, 8, "係");
            LoginRep loginRep = LoginRep.GetInstance();
            SetStringOriginalAndCopy(15, 8, TextHelper.GetFirstName(loginRep.Rep.Name));

            void SetStringOriginalAndCopy(int row,int column,string value)
            {
                SetString(row, column, value);
                SetString(row, CopyColumnPosition(column), value);
            }

            void SetString(int row, int column, string value) => myWorksheet.Cell(row, column).Value = value;

            void SingleLineOutput()
            {
                int i = VoucherData.ReceiptsAndExpenditures.Count - 1;
                foreach(ReceiptsAndExpenditure rae in VoucherData.ReceiptsAndExpenditures)
                {
                    SetStringOriginalAndCopy((10 - i), 2, ProvisoString(rae));
                    i--;
                }         
            }

            string ProvisoString(ReceiptsAndExpenditure rae)
                => VoucherData.ReceiptsAndExpenditures.Count == 1 ?
                $"{rae.Content.Text}{AppendSupplement(rae)}" : 
                $"{rae.Content.Text}{AppendSupplement(rae)}{TextHelper.Space}:{TextHelper.Space}" +
                $"{rae.PriceWithUnit}";
            
            void MultipleLineOutput()
            {
                int i = 7;
                int j = 3;
                foreach(ReceiptsAndExpenditure rae in VoucherData.ReceiptsAndExpenditures)
                {
                    if (i > 3) SetStringOriginalAndCopy(10 - j, 2, ProvisoString(rae));
                    else SetStringOriginalAndCopy(10 - i, 6, ProvisoString(rae));
                    j--;
                    i--;
                }
            }

            string AppendSupplement(ReceiptsAndExpenditure rae)
            {
                if (rae.Content.Text.Contains("管理料"))
                {
                    string[] array = rae.Detail.Split(' ');
                    string s = default;
                    foreach (string t in array)
                        if (t.Contains("年度分")) s = $"{TextHelper.Space}{t}";
                    return s;
                }
                else return string.Empty;
            }
        } 
  
        protected override double SetMaeginsBottom() => ToInch(1.4);

        protected override double SetMaeginsLeft() => ToInch(1.3);

        protected override double SetMaeginsRight() => ToInch(1.3);

        protected override double SetMaeginsTop() => ToInch(1.9);

        protected override void SetMerge()
        {
            //タイトル
            SetMergeOriginalAndCopy(1, 1, 1, 9);
            //日付
            SetMergeOriginalAndCopy(2, 6, 2, 9);
            //宛名
            SetMergeOriginalAndCopy(3, 1, 3, 5);
            //総額
            SetMergeOriginalAndCopy(5, 3, 5, 6);
            //円也
            SetMergeOriginalAndCopy(5, 7, 5, 8);
            //但し書き
            if(VoucherData.ReceiptsAndExpenditures.Count>4)
            {
                SetMergeOriginalAndCopy(7, 2, 7, 5);
                SetMergeOriginalAndCopy(8, 2, 8, 5);
                SetMergeOriginalAndCopy(9, 2, 9, 5);
                SetMergeOriginalAndCopy(10, 2, 10, 5);
                SetMergeOriginalAndCopy(7, 6, 7, 8);
                SetMergeOriginalAndCopy(8, 6, 8, 8);
                SetMergeOriginalAndCopy(9, 6, 9, 8);
                SetMergeOriginalAndCopy(10, 6, 10, 8);
            }
            else
            {
                SetMergeOriginalAndCopy(7, 2, 7, 8);
                SetMergeOriginalAndCopy(8, 2, 8, 8);
                SetMergeOriginalAndCopy(9, 2, 9, 8);
                SetMergeOriginalAndCopy(10, 2, 10, 8);
            }
            //上記有難くお受けしました
            SetMergeOriginalAndCopy(11, 2, 11, 8);
            //団体名
            SetMergeOriginalAndCopy(13, 5, 14, 6);
            //団体肩書
            SetMergeOriginalAndCopy(14, 1, 14, 3);
            //係
            SetMergeOriginalAndCopy(14, 8, 14, 9);
            //郵便番号
            SetMergeOriginalAndCopy(15, 1, 15, 2);
            //住所
            SetMergeOriginalAndCopy(15, 3, 15, 6);
            //係印
            SetMergeOriginalAndCopy(15, 8, 16, 9);
            //電話番号
            SetMergeOriginalAndCopy(16, 2, 16, 6);

            void SetMergeOriginalAndCopy(int row1, int column1, int row2, int column2)
            {
                MySheetCellRange(row1, column1, row2, column2).Merge();
                MySheetCellRange
                    (row1, CopyColumnPosition(column1), row2, CopyColumnPosition(column2)).Merge();
            }
        }

        protected override double[] SetRowSizes() => new double[] 
            { 37, 18, 37.5, 37.5, 37.5, 37.5, 22.5, 22.5, 22.5, 22.5, 37.5, 18, 18.5, 18.5, 18, 18 };

        protected override string SetSheetFontName() => "ＭＳ 明朝";

        protected override void SetSheetStyle()
        {
            myWorksheet.Style.NumberFormat.Format = "@";
            myWorksheet.PageSetup.PageOrientation = XLPageOrientation.Landscape;
        }

        protected override XLPaperSize SheetPaperSize() => XLPaperSize.B5Paper;
    }
}
