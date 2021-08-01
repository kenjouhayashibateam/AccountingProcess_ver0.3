using ClosedXML.Excel;
using Domain.Entities;
using Domain.Entities.Helpers;
using Domain.Entities.ValueObjects;
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
        private readonly bool IsPayment;
        private readonly bool IsPreviousDay;

        public enum SlipType
        {
            Payment,
            Withdrawal,
            Transter
        }

        internal SlipOutput
            (ObservableCollection<ReceiptsAndExpenditure> outputDatas, Rep outputRep,
                SlipType slipType, bool isPreviousDay) : base(outputDatas)
        {
            OutputRep = outputRep;
            mySlipType = slipType;
            IsPayment = mySlipType == SlipType.Payment;
            IsPreviousDay = isPreviousDay;
        }

        protected void SlipDataOutput()
        {
            string code = string.Empty;
            string subject = string.Empty;
            string content = string.Empty ;
            string location = default;
            string creditDept = default;
            string clerk = default;
            bool isTaxRate = false;
            int contentCount = 0;
            int inputRow = 0;
            int inputContentColumn = 0;
            int TotalPrice = 0;
            DateTime currentDate = TextHelper.DefaultDate;

            //日付、入出金チェック、科目コード、勘定科目でソートして、伝票におこす
            foreach (ReceiptsAndExpenditure rae in ReceiptsAndExpenditures.OrderByDescending
                (r => r.IsPayment)
                .ThenBy(r => r.CreditDept.Dept)
                .ThenBy(r => r.Content.AccountingSubject.SubjectCode)
                .ThenBy(r => r.Content.AccountingSubject.Subject)
                .ThenBy(r => r.IsReducedTaxRate)
                .ThenBy(r => r.Location)
                )
            {
                if (rae.IsPayment != IsPayment) { continue; }
                //codeの初期値を設定する
                if (string.IsNullOrEmpty(code)) { code = rae.Content.AccountingSubject.SubjectCode; }
                //subjectの初期値を設定する
                if (string.IsNullOrEmpty(subject)) { subject = rae.Content.AccountingSubject.Subject; }
                //currentDateの初期値を設定する
                if (currentDate == TextHelper.DefaultDate) { currentDate = rae.AccountActivityDate; }
                if (string.IsNullOrEmpty(content)) { content = rae.Content.Text; }//contentの初期値を設定する}
                if (string.IsNullOrEmpty(location)) { location = rae.Location; }
                if (string.IsNullOrEmpty(creditDept)) { creditDept = rae.CreditDept.Dept; }
                if (string.IsNullOrEmpty(clerk)) { clerk = rae.RegistrationRep.FirstName; }
                contentCount++;

                if (IsSameData(rae, currentDate, creditDept, code, subject, content, location, isTaxRate))
                { TotalPrice += rae.Price; }
                else
                {
                    currentDate = rae.AccountActivityDate;//入出金日を代入
                    code = rae.Content.AccountingSubject.SubjectCode;
                    subject = rae.Content.AccountingSubject.Subject;
                    content = rae.Content.Text;
                    TotalPrice = rae.Price;
                    creditDept = rae.CreditDept.Dept;
                    location = rae.Location;
                    clerk = rae.RegistrationRep.FirstName;
                    isTaxRate = rae.IsReducedTaxRate;
                    contentCount = 1;
                    NextPage();//次のページへ
                    PageStyle();
                }

                //伝票1件目から5件目は一列目、6件目から10件目までは4列目に出力するので、
                //セルの場所を設定する
                if (contentCount <= 5)
                {
                    inputRow = StartRowPosition  + contentCount;
                    inputContentColumn = 1;
                }
                else
                {
                    inputRow = StartRowPosition  + contentCount - 5;
                    inputContentColumn = 11;
                }
                //伝票の詳細を設定したセルに出力する
                myWorksheet.Cell(inputRow, inputContentColumn).Value =
                    $@"{rae.Content.Text} {rae.Detail} \{TextHelper.CommaDelimitedAmount(rae.Price)}-";
                //{rae.Content.Text}伝票の一番上の右の欄に入金日を出力                
                myWorksheet.Cell(StartRowPosition + 1, 16).Value =
                    $"{rae.AccountActivityDate:M/d}";
                //経理担当場所、伝票の総額、担当者、伝票作成日、勘定科目、コード、貸方部門を出力
                myWorksheet.Cell(StartRowPosition + 2, 16).Value = rae.Location; 
                string s = rae.IsReducedTaxRate ? "※軽減税率" : string.Empty;
                myWorksheet.Cell(StartRowPosition + 3, 16).Value = s;
                for (int i = 0; i < TotalPrice.ToString().Length; i++)
                {
                    myWorksheet.Cell(StartRowPosition + 10, 13 - i).Value =
                        TotalPrice.ToString().Substring(TotalPrice.ToString().Length - 1 - i, 1);
                }
                //前日の日付にする場合の対応
                DateTime OutputDate = IsPreviousDay ? DateTime.Today.AddDays(-1) : DateTime.Today;
                //前日の青蓮堂の担当者と当日の伝票出力者（管理事務所の経理）が違う場合の対応
                if (OutputRep.FirstName == clerk)
                {
                    myWorksheet.Cell(StartRowPosition + 6, 20).Value = OutputRep.FirstName;
                }
                else
                {
                    myWorksheet.Cell(StartRowPosition + 6, 20).Value = clerk;
                    myWorksheet.Cell(StartRowPosition + 6, 18).Value =
                        OutputRep.FirstName;
                }
                //出力日
                myWorksheet.Cell(StartRowPosition + 10, 1).Value = OutputDate.Year;
                myWorksheet.Cell(StartRowPosition + 10, 2).Value = OutputDate.Month;
                myWorksheet.Cell(StartRowPosition + 10, 3).Value = OutputDate.Day;
               //勘定科目
                string ass =
                    $"{rae.Content.AccountingSubject.Subject} : " +
                    $"{rae.Content.AccountingSubject.SubjectCode}";
                switch (mySlipType)
                {
                    case SlipType.Payment:
                        myWorksheet.Cell(StartRowPosition + 9, 14).Value = ass;
                        break;
                    case SlipType.Withdrawal:
                        myWorksheet.Cell(StartRowPosition + 9, 4).Value = ass;
                        break;
                    case SlipType.Transter:
                        break;
                    default:
                        break;
                };
                s = rae.CreditDept.ID == "credit_dept3" ? string.Empty : rae.CreditDept.Dept;
                myWorksheet.Cell(StartRowPosition + 9, 16).Value = s;
            }
        }

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
            _ = myWorksheet.Style.Border.SetLeftBorder(XLBorderStyleValues.None).Border.SetTopBorder(XLBorderStyleValues.None).Border.SetRightBorder(XLBorderStyleValues.None).Border.SetBottomBorder(XLBorderStyleValues.None);
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

        protected override double[] SetRowSizes()
        {
            return new double[]
                { 24, 20.25, 20.25, 20.25, 20.25, 20.25, 18.75, 18.75, 9, 30, 30 };
        }

        protected override double SetMaeginsBottom() { return ToInch(0); }

        protected override double SetMaeginsLeft() { return ToInch(2); }

        protected override double SetMaeginsRight() { return ToInch(0.5); }

        protected override double SetMaeginsTop() { return ToInch(0); }

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

        protected override void SetSheetStyle() { myWorksheet.Style.Font.FontSize = 11; }

        protected override XLPaperSize SheetPaperSize() { return XLPaperSize.A4Paper; }

        protected override void SetList(IEnumerable outputList)
        {
            ReceiptsAndExpenditures = (ObservableCollection<ReceiptsAndExpenditure>)outputList;
        }

        protected override string SetSheetFontName() { return "ＭＳ 明朝"; }
    }
}
