using ClosedXML.Excel;
using Domain.Entities;
using Domain.Entities.Helpers;
using Domain.Entities.ValueObjects;
using Domain.Repositories;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;

namespace Infrastructure.ExcelOutputData
{
    /// <summary>
    /// 伝票出力クラス
    /// </summary>
    internal class SlipOutput : OutputList
    {
        protected ObservableCollection<ReceiptsAndExpenditure> ReceiptsAndExpenditures;
        protected Rep OutputRep;
        private readonly SlipType mySlipType;
        private readonly bool IsPayment;//振替伝票出力機能を作成する際に使用する。現状は入出金の確認だけの機能
        public enum SlipType
        {
            Payment,
            Withdrawal,
            Transter
        }

        internal SlipOutput(ObservableCollection<ReceiptsAndExpenditure> outputDatas, Rep outputRep,SlipType slipType) : base(outputDatas)
        {
            OutputRep = outputRep;
            mySlipType = slipType;
            IsPayment = mySlipType == SlipType.Payment;
        }

        protected void SlipDataOutput()
        {
            string code = string.Empty;
            string subject = string.Empty;
            string content = string.Empty ;
            string location = default;

            bool isGoNext = false;
            int contentCount = 0;
            int inputRow = 0;
            int inputContentColumn = 0;
            int TotalPrice = 0;
            DateTime currentDate = TextHelper.DefaultDate;
            //日付、入出金チェック、科目コード、勘定科目でソートして、伝票におこす
            //foreach (ReceiptsAndExpenditure rae in ReceiptsAndExpenditures.OrderBy(r => r.AccountActivityDate)
            //    .ThenBy(r => r.IsPayment)
            foreach (ReceiptsAndExpenditure rae in ReceiptsAndExpenditures.OrderByDescending(r => r.IsPayment)
                .ThenBy(r => r.Content.AccountingSubject.SubjectCode)
                .ThenBy(r=>r.Content.AccountingSubject.Subject)
                .ThenBy(r=>r.Location))
            {
                if (rae.IsPayment!=IsPayment) continue;
                if (code == string.Empty) code = rae.Content.AccountingSubject.SubjectCode;//codeの初期値を設定する
                if (subject == string.Empty) subject = rae.Content.AccountingSubject.Subject;//subjectの初期値を設定する
                if (currentDate == TextHelper.DefaultDate) currentDate = rae.AccountActivityDate;//currentDateの初期値を設定する
                if (content == string.Empty) content = rae.Content.Text;//contentの初期値を設定する
                if (location == string.Empty) location = rae.Location;
                contentCount++;

                isGoNext = location != rae.Location;//伝票の作成場所が違えば次の伝票へ移動する

                //codeが同じならisGoNextにsubjectの比較結果を代入する
                if (code == rae.Content.AccountingSubject.SubjectCode | isGoNext == false)
                {
                    isGoNext = IsStringEqualsReverse(subject, rae.Content.AccountingSubject.Subject);
                    if (!isGoNext) isGoNext = currentDate != rae.AccountActivityDate;//入出金日比較
                    //if (!isGoNext) isGoNext = IsStringEqualsReverse(content, rae.Content.Text); //伝票内容比較
                }
                else isGoNext = true;

                currentDate = rae.AccountActivityDate;//入出金日を代入
                if (!isGoNext) isGoNext = contentCount > 8;//9件以上は次のページに出力
                //頁移動の有無による動作
                if (isGoNext)
                {
                    code = rae.Content.AccountingSubject.SubjectCode;
                    subject = rae.Content.AccountingSubject.Subject;
                    content = rae.Content.Text;                    
                    TotalPrice = rae.Price;
                    contentCount = 1;
                    NextPage();//次のページへ
                }
                else TotalPrice += rae.Price;//ページ移動がなければ、総額に現在のデータのPriceを加算
                myWorksheet.Cell(StartRowPosition + 1, 1).Value = $"{rae.AccountActivityDate:M/d}";//{rae.Content.Text}伝票の一番上にタイトルとして入金日、Contentを出力                
                //伝票1件目から4件目は一列目、5件目から8件目までは4列目に出力
                if (contentCount <= 4)
                {
                    inputRow = StartRowPosition + 1 + contentCount;
                    inputContentColumn = 1;
                }
                else
                {
                    inputRow = StartRowPosition + 1 + contentCount - 4;
                    inputContentColumn = 11;
                }
                myWorksheet.Cell(inputRow, inputContentColumn).Value = $"{rae.Content.Text} {rae.Detail} \\{rae.Price}-";

                //経理担当場所、伝票の総額、担当者、伝票作成日、勘定科目、コード、貸方部門を出力
                myWorksheet.Cell(StartRowPosition + 1, 16).Value = rae.Location;
                string s;
                if (rae.IsReducedTaxRate) s = "※軽減税率";
                else s = string.Empty;
                myWorksheet.Cell(StartRowPosition + 2, 16).Value = s;
                for (int i = 0; i < TotalPrice.ToString().Length; i++) myWorksheet.Cell(StartRowPosition + 10, 13 - i).Value = TotalPrice.ToString().Substring(TotalPrice.ToString().Length - 1 - i, 1);
                myWorksheet.Cell(StartRowPosition + 6, 20).Value = TextHelper.GetFirstName(OutputRep.Name);
                myWorksheet.Cell(StartRowPosition + 10, 1).Value = rae.AccountActivityDate.Year;
                myWorksheet.Cell(StartRowPosition + 10, 2).Value = rae.AccountActivityDate.Month;
                myWorksheet.Cell(StartRowPosition + 10, 3).Value = rae.AccountActivityDate.Day;
                
                string ass = $"{rae.Content.AccountingSubject.SubjectCode} : {rae.Content.AccountingSubject.Subject}";
                switch(mySlipType)
                {
                    case SlipType.Payment:
                        myWorksheet.Cell(StartRowPosition + 9, 14).Value = ass;
                        break;
                    case SlipType.Withdrawal:
                        myWorksheet.Cell(StartRowPosition + 9, 4).Value = ass;
                        break;
                    default:
                        break;
                };
                myWorksheet.Cell(StartRowPosition + 9, 16).Value = rae.CreditAccount.Account;
            }
        }
        /// <summary>
        /// 文字列を比較して、同じならFalse、違えばTrueを返します
        /// </summary>
        /// <param name="Value1">文字列1</param>
        /// <param name="Value2">文字列2</param>
        /// <returns></returns>
        private bool IsStringEqualsReverse(string Value1, string Value2) => Value1 != Value2;

        public override void Output()
        {
            SlipDataOutput();
            ExcelOpen();
        }

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
            myWorksheet.Cell(StartRowPosition + 2, 16).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            for (int i = StartRowPosition + 2; i < StartRowPosition + 7; i++)
            {
                myWorksheet.Cell(i, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                myWorksheet.Cell(i, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                myWorksheet.Cell(i, 1).Style.Alignment.SetShrinkToFit(true);
                myWorksheet.Cell(i, 11).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                myWorksheet.Cell(i, 11).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                myWorksheet.Cell(i, 11).Style.Alignment.SetShrinkToFit(true);
            }
            myWorksheet.Cell(StartRowPosition + 6, 20).Style
                //.Alignment.SetTopToBottom(true)
                .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            MySheetCellRange(StartRowPosition + 9, 4, StartRowPosition + 9, 16).Style
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                .Alignment.SetVertical(XLAlignmentVerticalValues.Bottom)
                .Alignment.SetShrinkToFit(true);
            MySheetCellRange(StartRowPosition + 10, 1, StartRowPosition + 10, 13).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Bottom);
            myWorksheet.Cell(StartRowPosition + 10, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            MySheetCellRange(StartRowPosition + 10, 2, StartRowPosition + 10, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            MySheetCellRange(StartRowPosition + 10, 4, StartRowPosition + 10, 13).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
        }

        protected override double[] SetColumnSizes() => new double[] { 4.71, 4.71, 5.14, 1.43, 1.29, 1.29, 1.43, 1.29, 1.29, 1.43, 1.29, 1.29, 1.43, 10.29, 10.14, 5.29, 0.92, 5.43, 0.92, 5.14 };

        protected override double[] SetRowSizes() => new double[] { 28.5, 20.25, 20.25, 20.25, 20.25, 20.25, 18.75, 18.75, 9, 30, 29.25 };

        protected override double SetMaeginsBottom() => ToInch(0);

        protected override double SetMaeginsLeft() => ToInch(2);

        protected override double SetMaeginsRight() => ToInch(0.5);

        protected override double SetMaeginsTop() => ToInch(0);

        protected override void SetMerge()
        {
            MySheetCellRange(StartRowPosition + 1, 1, StartRowPosition + 1, 15).Merge();
            MySheetCellRange(StartRowPosition + 1, 16, StartRowPosition + 1, 20).Merge();
            for (int i = 2; i < 6; i++)
            {
                MySheetCellRange(StartRowPosition + i, 1, StartRowPosition + i, 9).Merge();
                MySheetCellRange(StartRowPosition + i, 11, StartRowPosition + i, 15).Merge();
            }
            MySheetCellRange(StartRowPosition + 2, 16, StartRowPosition + 2, 20).Merge();
            MySheetCellRange(StartRowPosition + 6, 18, StartRowPosition + 7, 18).Merge();
            MySheetCellRange(StartRowPosition + 6, 20, StartRowPosition + 7, 20).Merge();
            MySheetCellRange(StartRowPosition + 9, 4, StartRowPosition + 9, 13).Merge();
            MySheetCellRange(StartRowPosition + 9, 14, StartRowPosition + 9, 15).Merge();
            MySheetCellRange(StartRowPosition + 9, 16, StartRowPosition + 9, 19).Merge();
        }

        protected override void SetSheetFontStyle() => myWorksheet.Style.Font.FontSize = 11;

        protected override XLPaperSize SheetPaperSize() => XLPaperSize.A4Paper;

        protected override void SetList(IEnumerable outputList) => ReceiptsAndExpenditures = (ObservableCollection<ReceiptsAndExpenditure>)outputList;

        protected override string SetSheetFontName() => "ＭＳ 明朝";
    }
}
