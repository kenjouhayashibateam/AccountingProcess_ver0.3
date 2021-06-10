using ClosedXML.Excel;
using Domain.Entities;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using static Domain.Entities.Helpers.TextHelper;

namespace Infrastructure.ExcelOutputData
{
    /// <summary>
    /// 御布施一覧データ出力クラス
    /// </summary>
    internal class CondolencesOutput : OutputSingleSheetData
    {
        private readonly ObservableCollection<Condolence> Condolences;
        private int pageNumber = 1;
        //Rowのスタート位置インデックス
        private int StartRowIndex { get => ((pageNumber - 1) * SetRowSizes().Length); }

        public CondolencesOutput(ObservableCollection<Condolence> condolences) =>
            Condolences = condolences;

        protected override void SetBorderStyle()
        {
            MySheetCellRange(StartRowIndex + 2, 1, SetRowSizes().Length * pageNumber, 15).Style
                .Border.SetBottomBorder(XLBorderStyleValues.Thin)
                .Border.SetTopBorder(XLBorderStyleValues.Thin)
                .Border.SetLeftBorder(XLBorderStyleValues.Thin)
                .Border.SetRightBorder(XLBorderStyleValues.Thin);
        }

        protected override void SetCellsStyle()
        {
            //シートのフォントサイズ
            myWorksheet.Style.Font.FontSize = 10;
            //シートのセルを「縮小して全体を表示」設定にする
            myWorksheet.Style.Alignment.SetShrinkToFit(true);
            //用紙の向き
            myWorksheet.PageSetup.PageOrientation = XLPageOrientation.Landscape;
            //タイトル欄
            MySheetCellRange(StartRowIndex + 1, 1, StartRowIndex + 2, SetColumnSizes().Length).Style
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            //データ全体
            MySheetCellRange
                (StartRowIndex + 3, 1, SetRowSizes().Length * pageNumber, SetColumnSizes().Length).Style
                .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            //日付、施主名、内容、担当僧侶
            MySheetCellRange(StartRowIndex + 3, 1, SetRowSizes().Length * pageNumber, 4).Style
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            //合計金額、御布施、御車代、御膳料、御車代御膳料、懇志
            MySheetCellRange(StartRowIndex + 3, 5, SetRowSizes().Length * pageNumber, 10).Style
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            //窓口、郵送
            MySheetCellRange(StartRowIndex + 3, 11, SetRowSizes().Length * pageNumber, 12).Style
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            //備考
            MySheetCellRange(StartRowIndex + 3, 15, SetRowSizes().Length * pageNumber, 16).Style
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
        }

        protected override double[] SetColumnSizes() => new double[]
            {4.86,9,6.29,6.29,11.71,11.71,11.71,11.71,11.71,11.71,6.14,6.14,6.14,6.14,12.71};

        protected override void SetDataStrings()
        {
            int i = 1;
            int currentRow=1;
            int dateMergeStartRow = 3;
            DateTime currentDate = DefaultDate;

            foreach (Condolence condolence in Condolences.OrderBy(c=>c.AccountActivityDate))
            {
                if(i==17)
                {
                    pageNumber++;
                    MySheetCellRange(dateMergeStartRow, 1, currentRow, 1).Merge();
                    i = 1;
                }
                if(i==1)
                {
                    SetNewPage();
                    //タイトル欄　年
                    myWorksheet.Cell(StartRowIndex + i, 1).Value =
                        $"{condolence.AccountActivityDate.ToString($"gg{Space}y{Space}年",JapanCulture)}";
                    i++;
                    currentRow = StartRowIndex + i;
                    //タイトル欄フィールドタイトル
                    myWorksheet.Cell(currentRow, 1).Value = "日付";
                    myWorksheet.Cell(currentRow, 2).Value = "施主名";
                    myWorksheet.Cell(currentRow, 3).Value = "内容";
                    myWorksheet.Cell(currentRow, 4).Value = "担当僧侶";
                    myWorksheet.Cell(currentRow, 5).Value = "合計金額";
                    myWorksheet.Cell(currentRow, 6).Value = "御布施";
                    myWorksheet.Cell(currentRow, 7).Value = "御車代";
                    myWorksheet.Cell(currentRow, 8).Value = "御膳料";
                    myWorksheet.Cell(currentRow, 9).Value = "御車代御膳料";
                    myWorksheet.Cell(currentRow, 10).Value = "懇志";
                    myWorksheet.Cell(currentRow, 11).Value = "窓口";
                    myWorksheet.Cell(currentRow, 12).Value = "郵送";
                    myWorksheet.Cell(currentRow, 13).Value = "支配人";
                    myWorksheet.Cell(currentRow, 14).Value = "本部長";
                    myWorksheet.Cell(currentRow, 15).Value = "備考";
                    i++;
                    currentDate = condolence.AccountActivityDate;
                    dateMergeStartRow = StartRowIndex + i;
                }
                currentRow = StartRowIndex + i;
                //前のデータと日付が変わった時点で、同じ日付のセルを結合する
                if(currentDate!=condolence.AccountActivityDate)
                {
                    MySheetCellRange(dateMergeStartRow, 1, currentRow - 1, 1).Merge();
                    currentDate = condolence.AccountActivityDate;
                    dateMergeStartRow = currentRow;
                }
                //各フィールド入力
                myWorksheet.Cell(currentRow, 1).Value =
                    $"{condolence.AccountActivityDate:M/d}\r\n{condolence.AccountActivityDate:(ddd)}";
                myWorksheet.Cell(currentRow, 2).Value = condolence.OwnerName;
                myWorksheet.Cell(currentRow, 3).Value = condolence.Content;
                myWorksheet.Cell(currentRow, 4).Value = GetFirstName(condolence.SoryoName);
                myWorksheet.Cell(currentRow, 5).Value = AmountWithUnit(condolence.TotalAmount);
                myWorksheet.Cell(currentRow, 6).Value = AmountWithUnit(condolence.Almsgiving);
                myWorksheet.Cell(currentRow, 7).Value = AmountWithUnit(condolence.CarTip);
                myWorksheet.Cell(currentRow, 8).Value = AmountWithUnit(condolence.MealTip);
                myWorksheet.Cell(currentRow, 9).Value = AmountWithUnit(condolence.CarAndMealTip);
                myWorksheet.Cell(currentRow, 10).Value = AmountWithUnit(condolence.SocialGathering);
                myWorksheet.Cell(currentRow, 11).Value = condolence.CounterReceiver;
                myWorksheet.Cell(currentRow, 12).Value = condolence.MailRepresentative;
                myWorksheet.Cell(currentRow,15).Value= condolence.Note;
                i++;
            }
            MySheetCellRange(dateMergeStartRow, 1, currentRow, 1).Merge();
            MySheetCellRange
                (currentRow + 1, 1, SetRowSizes().Length * pageNumber, SetColumnSizes().Length).Style
                    .Border.SetBottomBorder(XLBorderStyleValues.None)
                    .Border.SetRightBorder(XLBorderStyleValues.None)
                    .Border.SetLeftBorder(XLBorderStyleValues.None)
                    .Border.SetTopBorder(XLBorderStyleValues.None);

            void SetNewPage()
            {
                if (pageNumber == 1) return;
                SetCellsStyle();
                SetMerge();
                SetBorderStyle();
                for (int i = 0; i < SetRowSizes().Length; i++)
                { myWorksheet.Row(StartRowIndex + i + 1).Height = SetRowSizes()[i]; }
            }
        }

        protected override double SetMaeginsBottom() => ToInch(1.4);

        protected override double SetMaeginsLeft() => ToInch(0.1);

        protected override double SetMaeginsRight() => ToInch(0.1);

        protected override double SetMaeginsTop() => ToInch(1.9);

        protected override void SetMerge() => 
            MySheetCellRange(StartRowIndex + 1, 1, StartRowIndex + 1, SetColumnSizes().Length).Merge();

        protected override double[] SetRowSizes() => new double[]
            { 20.5, 13, 34.5, 34.5, 34.5, 34.5, 34.5, 34.5, 34.5, 34.5, 34.5, 34.5, 34.5, 34.5, 34.5, 34.5};

        protected override string SetSheetFontName() => "ＭＳ ゴシック";

        protected override void SetSheetStyle()=>
            myWorksheet.Style.NumberFormat.Format = "@";

        protected override XLPaperSize SheetPaperSize() => XLPaperSize.A4Paper;
    }
}
