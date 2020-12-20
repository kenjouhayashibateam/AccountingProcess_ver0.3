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
            DateTime currentDate = DateTime.Parse("1900/01/01");

            foreach (ReceiptsAndExpenditure rae in ReceiptsAndExpenditures.OrderBy(r => r.AccountActivityDate)
                .ThenBy(r => r.IsPayment)
                .ThenBy(r => r.Content.AccountingSubject.SubjectCode)
                .ThenBy(r => r.Content.AccountingSubject.Subject))
            {
                if (!rae.IsPayment) continue;
                if (code == string.Empty) code = rae.Content.AccountingSubject.SubjectCode;

                if (code == rae.Content.AccountingSubject.SubjectCode) isGoNext = IsStringEqualsReverse(subject, rae.Content.AccountingSubject.Subject);
                else
                {
                    isGoNext = true;
                    code = rae.Content.AccountingSubject.SubjectCode;
                    subject = rae.Content.AccountingSubject.Subject;
                }
                if (!isGoNext) isGoNext = contentCount > 8;
                if (!isGoNext) isGoNext = currentDate == rae.AccountActivityDate;
                currentDate = rae.AccountActivityDate;
                if (isGoNext)
                {
                    for (int i = 0; i < rae.Price.ToString().Length; i++) myWorksheet.Cell(StartRowPosition + 11, 13 - i).Value = rae.Price.ToString().Substring(i, 1);
                    myWorksheet.Cell(StartRowPosition + 7, 20).Value = TextHelper.GetFirstName(OutputRep.Name);
                    myWorksheet.Cell(StartRowPosition + 11, 1).Value = rae.AccountActivityDate.Year;
                    myWorksheet.Cell(StartRowPosition + 11, 2).Value = rae.AccountActivityDate.Month;
                    myWorksheet.Cell(StartRowPosition + 11, 3).Value = rae.AccountActivityDate.Day;
                    myWorksheet.Cell(StartRowPosition + 10, 14).Value = $"{rae.Content.AccountingSubject.Subject} {rae.Content.AccountingSubject.SubjectCode}";
                    myWorksheet.Cell(StartRowPosition + 10, 16).Value = rae.CreditAccount.Account;
                    NextPage();
                    contentCount = 0;
                }
                else TotalPrice += rae.Price;
                myWorksheet.Cell(StartRowPosition + 2, 1).Value = $"{rae.AccountActivityDate:M/d} {rae.Content.Text}";
                if(contentCount<3)
                {
                    inputRow = StartRowPosition + 3 + contentCount;
                    inputColumn = 1;
                }
                else
                {
                    inputRow = StartRowPosition + 3 + contentCount - 4;
                    inputColumn = 11;
                }
                myWorksheet.Cell(inputRow, inputColumn).Value = rae.Detail;
                myWorksheet.Cell(inputRow, inputColumn + 1).Value = rae.Price;

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
        }

        protected override void SetBorderStyle() { }

        protected override void SetCellsStyle()
        {
            MySheetCellRange(StartRowPosition + 2, 1, StartRowPosition + 6, 20).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            myWorksheet.Cell(StartRowPosition + 2, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            myWorksheet.Cell(StartRowPosition + 2, 16).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            for (int i = StartRowPosition + 3; i < 7; i++)
            {
                myWorksheet.Cell(i, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                myWorksheet.Cell(i, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                myWorksheet.Cell(i, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                myWorksheet.Cell(i, 15).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            }
            myWorksheet.Cell(StartRowPosition + 7, 20).Style
                .Alignment.SetTopToBottom(true)
                .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            MySheetCellRange(StartRowPosition + 10, 14, StartRowPosition + 10, 16).Style
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            MySheetCellRange(StartRowPosition + 11, 1, StartRowPosition + 11, 13).Style
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                .Alignment.SetVertical(XLAlignmentVerticalValues.Bottom);
        }

        protected override double[] SetColumnSizes() => new double[] { 4.71, 4.71, 5.14, 1.43, 1.29, 1.29, 1.43, 1.29, 1.29, 1.43, 1.29, 1.29, 1.43, 10.29, 10.14, 5.29, 0.83, 5.43, 0.83, 5.43 };

        protected override double SetMaeginsBottom() => ToInch(0);

        protected override double SetMaeginsLeft() => ToInch(2);

        protected override double SetMaeginsRight() => ToInch(0.5);

        protected override double SetMaeginsTop() => ToInch(0);

        protected override void SetMerge()
        {
            for (int i = 1; i < 6; i++)
            {
                MySheetCellRange(StartRowPosition + i, 1, StartRowPosition + i, 3).Merge();
                MySheetCellRange(StartRowPosition + i, 4, StartRowPosition + i, 9).Merge();
                MySheetCellRange(StartRowPosition + i, 11, StartRowPosition + i, 14).Merge();
                MySheetCellRange(StartRowPosition + i, 16, StartRowPosition + i, 20).Merge();
            }
            MySheetCellRange(StartRowPosition + 7, 18, StartRowPosition + 8, 18).Merge();
            MySheetCellRange(StartRowPosition + 7, 20, StartRowPosition + 8, 20).Merge();
            MySheetCellRange(StartRowPosition + 10, 14, StartRowPosition + 10, 15).Merge();
            MySheetCellRange(StartRowPosition + 10, 16, StartRowPosition + 10, 20).Merge();
        }

        protected override double[] SetRowSizes() => new double[] { 28.5, 20.25, 20.25, 20.25, 20.25, 20.25, 18.75, 18.75, 21.75, 25.5, 21 };

        protected override void SetSheetFontStyle() => myWorksheet.Style.Font.FontSize = 11;

        protected override XLPaperSize SheetPaperSize() => XLPaperSize.A4Paper;
    }
}
