using ClosedXML.Excel;
using Domain.Entities;
using Domain.Entities.Helpers;
using Domain.Entities.ValueObjects;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Infrastructure.ExcelOutputData
{
    /// <summary>
    /// 入金伝票データ出力
    /// </summary>
    internal class PaymentSlipOutput : OutputList
    {
        readonly ObservableCollection<ReceiptsAndExpenditure> ReceiptsAndExpenditures;
        readonly Rep OutputRep;

        public PaymentSlipOutput(ObservableCollection<ReceiptsAndExpenditure> outputDatas, Rep outputRep) : base(outputDatas)
        {
            ReceiptsAndExpenditures = outputDatas;
            OutputRep = outputRep;
        }

        public override void Output()
        {
            string code = string.Empty;
            string subject = string.Empty;
            bool isGoNext = false;
            int contentCount = 0;
            int inputRow = 0;
            int inputColumn = 0;
            int TotalPrice = 0;
            DateTime currentDate =new DateTime(1900,1,1);
            //日付、入出金チェック、科目コード、勘定科目でソートして、伝票におこす
            foreach (ReceiptsAndExpenditure rae in ReceiptsAndExpenditures.OrderBy(r => r.AccountActivityDate)
                .ThenBy(r => r.IsPayment)
                .ThenBy(r => r.Content.AccountingSubject.SubjectCode)
                .ThenBy(r => r.Content.AccountingSubject.Subject))
            {
                if (!rae.IsPayment) continue;   //入金伝票なので、Falseなら次のデータへ
                if (code == string.Empty) code = rae.Content.AccountingSubject.SubjectCode;//codeの初期値を設定する
                if (subject == string.Empty) subject = rae.Content.AccountingSubject.Subject;//subjectの初期値を設定する
                if (currentDate == new DateTime(1900,1,1)) currentDate = rae.AccountActivityDate;//currentDateの初期値を設定する
                //codeが同じならisGoNextにsubjectの比較結果を代入する
                if (code == rae.Content.AccountingSubject.SubjectCode)
                {
                    isGoNext = IsStringEqualsReverse(subject, rae.Content.AccountingSubject.Subject);
                    if (!isGoNext) isGoNext = currentDate != rae.AccountActivityDate;//入金日比較
                }
                else
                {
                    isGoNext = true;
                    code = rae.Content.AccountingSubject.SubjectCode;
                    subject = rae.Content.AccountingSubject.Subject;
                }
                currentDate = rae.AccountActivityDate;//入金日を代入
                if (!isGoNext) isGoNext = contentCount > 8;//8件以上は次のページに出力
                //頁移動の有無による動作
                if (isGoNext)
                {
                    TotalPrice = rae.Price;
                    contentCount = 0;
                    NextPage();//次のページへ
                }
                else TotalPrice += rae.Price;//ページ移動がなければ、総額に現在のデータのPriceを加算
                myWorksheet.Cell(StartRowPosition + 1, 1).Value = $"{rae.AccountActivityDate:M/d} {rae.Content.Text}";//伝票の一番上にタイトルとして入金日、Contentを出力                
                //伝票1件目から4件目は一列目、5件目から8件目までは4列目に出力
                if (contentCount < 4)
                {
                    inputRow = StartRowPosition + 2 + contentCount;
                    inputColumn = 1;
                }
                else
                {
                    inputRow = StartRowPosition + 3 + contentCount - 4;
                    inputColumn = 11;
                }
                myWorksheet.Cell(inputRow, inputColumn).Value = rae.Detail;//詳細を出力
                myWorksheet.Cell(inputRow, inputColumn + 1).Value = rae.Price;//金額を出力

                //伝票の総額、担当者、伝票作成日、勘定科目、コード、貸方部門を出力
                for (int i = 0; i <TotalPrice.ToString().Length; i++) myWorksheet.Cell(StartRowPosition + 10, 13 - i).Value = TotalPrice.ToString().Substring(TotalPrice.ToString().Length - 1 - i, 1);
                myWorksheet.Cell(StartRowPosition + 6, 20).Value = TextHelper.GetFirstName(OutputRep.Name);
                myWorksheet.Cell(StartRowPosition + 10, 1).Value = rae.AccountActivityDate.Year;
                myWorksheet.Cell(StartRowPosition + 10, 2).Value = rae.AccountActivityDate.Month;
                myWorksheet.Cell(StartRowPosition + 10, 3).Value = rae.AccountActivityDate.Day;
                myWorksheet.Cell(StartRowPosition + 9, 14).Value = $"{rae.Content.AccountingSubject.Subject} {rae.Content.AccountingSubject.SubjectCode}";
                myWorksheet.Cell(StartRowPosition + 9, 16).Value = rae.CreditAccount.Account;

                contentCount++;
            }
            ExcelOpen();
        }
        /// <summary>
        /// 文字列を比較して、同じならFalse、違えばTrueを返します
        /// </summary>
        /// <param name="Value1">文字列1</param>
        /// <param name="Value2">文字列2</param>
        /// <returns></returns>
        private bool IsStringEqualsReverse(string Value1, string Value2) => Value1 != Value2;

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
            myWorksheet.Style
                .Border.SetLeftBorder(XLBorderStyleValues.None)
                .Border.SetTopBorder(XLBorderStyleValues.None)
                .Border.SetRightBorder(XLBorderStyleValues.None)
                .Border.SetBottomBorder(XLBorderStyleValues.None);
        }

        protected override void SetCellsStyle()
        {
            MySheetCellRange(StartRowPosition + 1, 1, StartRowPosition + 5, 20).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            myWorksheet.Cell(StartRowPosition + 1, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            myWorksheet.Cell(StartRowPosition + 1, 16).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            for (int i = StartRowPosition + 2; i < 6; i++)
            {
                myWorksheet.Cell(i, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                myWorksheet.Cell(i, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                myWorksheet.Cell(i, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                myWorksheet.Cell(i, 15).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            }
            myWorksheet.Cell(StartRowPosition + 6, 20).Style
                .Alignment.SetTopToBottom(true)
                .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            MySheetCellRange(StartRowPosition + 9, 14, StartRowPosition + 9, 16).Style
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            MySheetCellRange(StartRowPosition + 10, 1, StartRowPosition + 10, 13).Style
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                .Alignment.SetVertical(XLAlignmentVerticalValues.Bottom);
        }

        protected override double[] SetColumnSizes() => new double[] { 4.71, 4.71, 5.14, 1.43, 1.29, 1.29, 1.43, 1.29, 1.29, 1.43, 1.29, 1.29, 1.43, 10.29, 10.14, 5.29, 0.83, 5.43, 0.83, 5.14 };

        protected override double[] SetRowSizes() => new double[] { 28.5, 17.25, 17.25, 17.25, 17.25, 17.25, 18.75, 18.75, 9, 30, 21 };

        protected override double SetMaeginsBottom() => ToInch(0);

        protected override double SetMaeginsLeft() => ToInch(2);

        protected override double SetMaeginsRight() => ToInch(0.5);

        protected override double SetMaeginsTop() => ToInch(0);

        protected override void SetMerge()
        {
            MySheetCellRange(StartRowPosition + 1, 1, StartRowPosition + 1, 15).Merge();
            for (int i = 2; i < 5; i++)
            {
                MySheetCellRange(StartRowPosition + i, 1, StartRowPosition + i, 3).Merge();
                MySheetCellRange(StartRowPosition + i, 4, StartRowPosition + i, 9).Merge();
                MySheetCellRange(StartRowPosition + i, 11, StartRowPosition + i, 14).Merge();
                MySheetCellRange(StartRowPosition + i, 16, StartRowPosition + i, 20).Merge();
            }
            MySheetCellRange(StartRowPosition + 6, 18, StartRowPosition + 7, 18).Merge();
            MySheetCellRange(StartRowPosition + 6, 20, StartRowPosition + 7, 20).Merge();
            MySheetCellRange(StartRowPosition + 9, 14, StartRowPosition + 9, 15).Merge();
            MySheetCellRange(StartRowPosition + 9, 16, StartRowPosition + 9, 19).Merge();
        }

        protected override void SetSheetFontStyle() => myWorksheet.Style.Font.FontSize = 11;

        protected override XLPaperSize SheetPaperSize() => XLPaperSize.A4Paper;
    }
}
