using ClosedXML.Excel;
using Domain.Entities;
using static Domain.Entities.Helpers.TextHelper;
using Domain.Repositories;
using System;

namespace Infrastructure.ExcelOutputData
{
    /// <summary>
    /// 受納証出力クラス
    /// </summary>
    internal class VoucherOutput : OutputSingleSheetData
    {
        private readonly Voucher VoucherData;
        private readonly bool IsReissue;
        private readonly DateTime PrepaidDate;

        public VoucherOutput(Voucher voucher, bool isReissue, DateTime prepaidDate)
        {
            VoucherData = voucher;
            IsReissue = isReissue;
            PrepaidDate = prepaidDate;
        }
        /// <summary>
        /// 受納証の複製部分のColumnを設定します
        /// </summary>
        /// <param name="originalColumn"></param>
        /// <returns></returns>
        private int CopyColumnPosition(int originalColumn)
        { return originalColumn + (SetColumnSizes().Length / 2) + 1; }

        protected override void SetBorderStyle()
        {
            //ボーダーをすべて消去
            _ = myWorksheet.Style
                .Border.SetLeftBorder(XLBorderStyleValues.None)
                .Border.SetTopBorder(XLBorderStyleValues.None)
                .Border.SetRightBorder(XLBorderStyleValues.None)
                .Border.SetBottomBorder(XLBorderStyleValues.None);
            //宛名欄
            SetBottomBorderOriginalAndCopy(3, 1, 3, 5);
            //総額欄
            SetBottomBorderOriginalAndCopy(5, 2, 5, 9);
            //但し書き欄
            SetBottomBorderOriginalAndCopy(10, 2, 10, 9);
            //係印欄
            SetClerkMarkField(14, 9, 16, 10);
            SetClerkMarkField(14, CopyColumnPosition(9), 16, CopyColumnPosition(10));

            void SetBottomBorderOriginalAndCopy(int row1,int column1,int row2,int column2)
            {
                SetBottomBorderThin(row1, column1, row2, column2);
                SetBottomBorderThin(row1, CopyColumnPosition(column1), row2, CopyColumnPosition(column2));
            }
            void SetBottomBorderThin(int row1, int column1, int row2, int column2)
            {
                _ = MySheetCellRange(row1, column1, row2, column2).Style.
                    Border.SetBottomBorder(XLBorderStyleValues.Thin);
            }

            void SetClerkMarkField(int row1, int column1, int row2, int column2)
            {
                _ = MySheetCellRange(row1, column1, row2, column2).Style
                    .Border.SetBottomBorder(XLBorderStyleValues.Thin)
                    .Border.SetRightBorder(XLBorderStyleValues.Thin)
                    .Border.SetLeftBorder(XLBorderStyleValues.Thin)
                    .Border.SetDiagonalBorder(XLBorderStyleValues.Thin)
                    .Border.SetTopBorder(XLBorderStyleValues.Thin);
                _ = MySheetCellRange(14, CopyColumnPosition(9), 16, CopyColumnPosition(10)).Style
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
            SetLocalProperty(XLAlignmentHorizontalValues.Right, XLAlignmentVerticalValues.Center, 26);
            SetCellPropertyOriginalAndCopy(1, 1);
            //ナンバー
            SetLocalProperty(XLAlignmentHorizontalValues.Right, XLAlignmentVerticalValues.Bottom, 11);
            SetCellPropertyOriginalAndCopy(1, 8);
            //日付欄
            SetLocalProperty(XLAlignmentHorizontalValues.Right, XLAlignmentVerticalValues.Center, 11);
            SetCellPropertyOriginalAndCopy(2, 7);
            //宛名欄
            SetLocalProperty(XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Bottom, 18);
            SetRangeProperyOriginalAndCopy(3, 1, 3, 4);
            _ = myWorksheet.Cell(3, 1).Style.Alignment.SetShrinkToFit(true);
            _ = myWorksheet.Cell(3, CopyColumnPosition(1)).Style.Alignment.SetShrinkToFit(true);
            SetLocalProperty(XLAlignmentHorizontalValues.Right, XLAlignmentVerticalValues.Bottom, 18);
            SetCellPropertyOriginalAndCopy(3, 5);
            //冥加金文字列
            SetLocalProperty(XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Bottom, 11);
            SetCellPropertyOriginalAndCopy(4, 2);
            //総額欄
            SetLocalProperty(XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Bottom, 24);
            SetCellPropertyOriginalAndCopy(5, 3);
            //円也文字列
            SetLocalProperty(XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Bottom, 11);
            SetCellPropertyOriginalAndCopy(5, 8);
            //但し文字列
            SetLocalProperty(XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Bottom, 11);
            SetCellPropertyOriginalAndCopy(6, 2);
            //但し書き欄
            SetLocalProperty(XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, 14);
            SetRangeProperyOriginalAndCopy(7, 2, 10, 9);
            _ = MySheetCellRange(7, 2, 10, 9).Style.Alignment.SetShrinkToFit(true);
            _ = MySheetCellRange(7, CopyColumnPosition(2), 10, CopyColumnPosition(9)).Style
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
            SetRangeProperyOriginalAndCopy(14, 9, 16, 10);
            //郵便番号欄
            SetLocalProperty(XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, 10);
            SetCellPropertyOriginalAndCopy(15, 1);
            //住所欄
            SetLocalProperty(XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, 10);
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

            void SetRangeAlignmentAndFontSize(int row1, int column1, int row2, int column2)
            {
                _ = MySheetCellRange(row1, column1, row2, column2).Style
                    .Alignment.SetHorizontal(horizontal)
                    .Alignment.SetVertical(vertical)
                    .Font.SetFontSize(fontSize);
            }
            
            void SetAlignmentAndFontSize(int row, int column)
            {
                _ = myWorksheet.Cell(row, column).Style
                    .Alignment.SetHorizontal(horizontal)
                    .Alignment.SetVertical(vertical)
                    .Font.SetFontSize(fontSize);
            }
        }

        protected override double[] SetColumnSizes()
        {
            //受納証左側のColumn＋左右の間のColumn＋受納証右側のColumn
            return new double[]
            { 3.13, 6.88, 3.13, 1.38, 6.88, 4,7.86, 3.13, 3.13, 3.5, 14.86,
                3.13, 6.88, 3.13, 1.38, 6.88,  4,7.86, 3.13, 3.13, 3.5 };
        }

        protected override void SetDataStrings()
        {
            string prevText = default;
            string addresseeText;
            int provisoAmount = default;

            //タイトル
            SetStringOriginalAndCopy(1, 1, "受　納　証");
            if (IsReissue) { SetStringOriginalAndCopy(2, 2, "※再発行"); }
            //ナンバー
            SetStringOriginalAndCopy(1, 8, $"№{VoucherData.ID}");
            //日付
            SetStringOriginalAndCopy(2, 7, VoucherData.OutputDate.ToString("yyyy年MM月dd日"));
            //宛名
            addresseeText = VoucherData.Addressee.Length == 2
                ? $"{VoucherData.Addressee.Substring(0, 1)}{SpaceF}" +
                    $"{VoucherData.Addressee.Substring(1, 1)}"
                : VoucherData.Addressee;
            SetStringOriginalAndCopy(3, 1, addresseeText);
            SetStringOriginalAndCopy(3, 5, "様");
            //総額
            SetStringOriginalAndCopy(4, 2, "冥加金");
            SetStringOriginalAndCopy(5, 3,
                $"{CommaDelimitedAmount(VoucherData.TotalAmount)}-");
            SetStringOriginalAndCopy(5, 8, "円也");
            SetStringOriginalAndCopy(6, 2, "但し");
            //事前領収の日付
            if (PrepaidDate != DefaultDate) { SetStringOriginalAndCopy(6, 7, $"※{PrepaidDate:M/d}ご法事"); }
            //但し書き
            if (VoucherData.ReceiptsAndExpenditures.Count < 5) { SingleLineOutput(); }
            else { MultipleLineOutput(); }
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
            string imagePath = @".\files\ReceiptStamp.png";
            _ = myWorksheet.AddPicture(imagePath).MoveTo(myWorksheet.Cell(13, 7));
            _ = myWorksheet.AddPicture(imagePath).MoveTo(myWorksheet.Cell(13, 18));
            //係
            SetStringOriginalAndCopy(14, 9, "係");
            LoginRep loginRep = LoginRep.GetInstance();
            SetStringOriginalAndCopy(15, 9, loginRep.Rep.FirstName);

            void SetStringOriginalAndCopy(int row, int column, string value)
            {
                SetString(row, column, value);
                SetString(row, CopyColumnPosition(column), value);
            }

            void SetString(int row, int column, string value) { myWorksheet.Cell(row, column).Value = value; }

            void SingleLineOutput()
            {
                int i = VoucherData.ReceiptsAndExpenditures.Count - 1;
                foreach (ReceiptsAndExpenditure rae in VoucherData.ReceiptsAndExpenditures)
                {
                    if (prevText == ReturnProvisoContent(rae))
                    {
                        provisoAmount += rae.Price;
                        SetStringOriginalAndCopy(10 - (i + 1), 2, string.Empty);
                    }
                    else
                    {
                        provisoAmount = rae.Price;
                        prevText = ReturnProvisoContent(rae);
                    }
                    SetStringOriginalAndCopy(10 - i, 2, ProvisoString(rae));

                    i--;
                }
            }

            string ReturnProvisoContent(ReceiptsAndExpenditure rae)
            {
                IDataBaseConnect dbc = DefaultInfrastructure.GetDefaultDataBaseConnect();

                return dbc.CallContentConvertText(rae.Content.ID) ?? rae.Content.Text;
            }

            string ProvisoString(ReceiptsAndExpenditure rae)
            {
                string s = VoucherData.ReceiptsAndExpenditures.Count == 1 ?
                    $"{ReturnProvisoContent(rae)}{AppendSupplement(rae)}" :
                    $"{ReturnProvisoContent(rae)}{AppendSupplement(rae)}{Space}:{Space}" +
                    $"{AmountWithUnit(provisoAmount)}";
                s += rae.IsReducedTaxRate ? $"{Space}※軽減税率" : string.Empty;
                return s;
            }

            void MultipleLineOutput()
            {
                int i = 8;
                int j = 4;
                foreach(ReceiptsAndExpenditure rae in VoucherData.ReceiptsAndExpenditures)
                {
                    if (prevText == ReturnProvisoContent(rae))
                    { provisoAmount += rae.Price; }
                    else
                    {
                        provisoAmount = rae.Price;
                        j--;
                        i--;
                    }

                    if (i > 3) { SetStringOriginalAndCopy(10 - j, 2, ProvisoString(rae)); }
                    else { SetStringOriginalAndCopy(10 - i, 6, ProvisoString(rae)); }
                }
            }

            string AppendSupplement(ReceiptsAndExpenditure rae)
            {
                string s = default;
                if (rae.Content.Text.Contains("管理料"))
                {
                    string[] array = rae.Detail.Split(' ');
                    foreach (string t in array)
                    {
                        if (t.Contains("年度分")) { s = $"{Space}{t}"; }
                    }
                }
                return s;
            }
        }

        protected override double SetMaeginsBottom() { return ToInch(1.3); }

        protected override double SetMaeginsLeft() { return ToInch(1.3); }

        protected override double SetMaeginsRight() { return ToInch(1.3); }

        protected override double SetMaeginsTop() { return ToInch(1.9); }

        protected override void SetMerge()
        {
            //タイトル
            SetMergeOriginalAndCopy(1, 1, 1, 7);
            SetMergeOriginalAndCopy(1, 8, 1, 10);
            SetMergeOriginalAndCopy(2, 2, 2, 5);
            //日付
            SetMergeOriginalAndCopy(2, 7, 2, 10);
            //宛名
            SetMergeOriginalAndCopy(3, 1, 3, 4);
            //総額
            SetMergeOriginalAndCopy(5, 3, 5, 7);
            //円也
            SetMergeOriginalAndCopy(5, 8, 5, 9);
            //事前領収日
            SetMergeOriginalAndCopy(6, 7, 6, 9);
            //但し書き
            if (VoucherData.ReceiptsAndExpenditures.Count > 4)
            {
                SetMergeOriginalAndCopy(7, 2, 7, 5);
                SetMergeOriginalAndCopy(8, 2, 8, 5);
                SetMergeOriginalAndCopy(9, 2, 9, 5);
                SetMergeOriginalAndCopy(10, 2, 10, 5);
                SetMergeOriginalAndCopy(7, 6, 7, 9);
                SetMergeOriginalAndCopy(8, 6, 8, 9);
                SetMergeOriginalAndCopy(9, 6, 9, 9);
                SetMergeOriginalAndCopy(10, 6, 10, 9);
            }
            else
            {
                SetMergeOriginalAndCopy(7, 2, 7, 9);
                SetMergeOriginalAndCopy(8, 2, 8, 9);
                SetMergeOriginalAndCopy(9, 2, 9, 9);
                SetMergeOriginalAndCopy(10, 2, 10, 9);
            }
            //上記有難くお受けしました
            SetMergeOriginalAndCopy(11, 2, 11, 9);
            //団体名
            SetMergeOriginalAndCopy(13, 5, 14, 7);
            //団体肩書
            SetMergeOriginalAndCopy(14, 1, 14, 4);
            //係
            SetMergeOriginalAndCopy(14, 9, 14, 10);
            //郵便番号
            SetMergeOriginalAndCopy(15, 1, 15, 2);
            //住所
            SetMergeOriginalAndCopy(15, 3, 15, 8);
            //係印
            SetMergeOriginalAndCopy(15, 9, 16, 10);
            //電話番号
            SetMergeOriginalAndCopy(16, 2, 16, 7);

            void SetMergeOriginalAndCopy(int row1, int column1, int row2, int column2)
            {
                _ = MySheetCellRange(row1, column1, row2, column2).Merge();
                _ = MySheetCellRange
                    (row1, CopyColumnPosition(column1), row2, CopyColumnPosition(column2)).Merge();
            }
        }

        protected override double[] SetRowSizes()
        {
            return new double[]
            { 37, 18, 37.5, 37.5, 37.5, 37.5, 22.5, 22.5, 22.5, 22.5, 37.5, 18, 18.5, 18.5, 18, 18 };
        }

        protected override string SetSheetFontName() { return "ＭＳ 明朝"; }

        protected override void SetSheetStyle()
        {
            myWorksheet.Style.NumberFormat.Format = "@";
            myWorksheet.PageSetup.PageOrientation = XLPageOrientation.Landscape;
        }

        protected override XLPaperSize SheetPaperSize() { return XLPaperSize.B5Paper; }
    }
}
