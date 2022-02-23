using ClosedXML.Excel;
using Domain.Entities;
using Domain.Entities.Helpers;
using Domain.Entities.ValueObjects;
using Domain.Repositories;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using static Domain.Entities.Helpers.TextHelper;
using static Domain.Entities.Helpers.DataHelper;

namespace Infrastructure.ExcelOutputData
{
    /// <summary>
    /// 伝票出力クラス
    /// </summary>
    internal class SlipOutput : SlipOutputBase
    {
        protected ObservableCollection<ReceiptsAndExpenditure> ReceiptsAndExpenditures;
        private readonly SlipType mySlipType;
        private readonly bool IsPayment;
        private readonly bool IsPreviousDay;
        private readonly IDataBaseConnect DataBaseConnect = 
            DefaultInfrastructure.GetDefaultDataBaseConnect();
        /// <summary>
        /// 伝票の種類
        /// </summary>
        public enum SlipType
        {
            /// <summary>
            /// 入金伝票
            /// </summary>
            Payment,
            /// <summary>
            /// 出金伝票
            /// </summary>
            Withdrawal,
            /// <summary>
            /// 社内振替伝票
            /// </summary>
            Transfer
        }

        internal SlipOutput
            (ObservableCollection<ReceiptsAndExpenditure> outputDatas, SlipType slipType, bool isPreviousDay) :
                base(outputDatas)
        {
            mySlipType = slipType;
            IsPayment = mySlipType == SlipType.Payment;
            IsPreviousDay = isPreviousDay;
        }

        protected void SlipDataOutput()
        {
            AccountingSubject subject = null;
            DateTime currentDate = DefaultDate;
            string content = string.Empty;
            string location = default;
            string creditDept = default;
            string clerk = default;
            int contentCount = 0;
            int TotalPrice = 0;
            bool isTaxRate = false;
            bool isNextSlip;
            int inputRow = 0;
            int inputContentColumn = 0;

            //日付、入出金チェック、科目コード、勘定科目でソートして、伝票におこす
            foreach (ReceiptsAndExpenditure rae in ReceiptsAndExpenditures.OrderByDescending
                (r => r.IsPayment)
                .ThenBy(r => r.CreditDept.Dept)
                .ThenBy(r => r.AccountActivityDate)
                .ThenBy(r => r.Content.AccountingSubject.ID)
                .ThenBy(r => r.Content.AccountingSubject.SubjectCode)
                .ThenBy(r => r.Content.AccountingSubject.Subject)
                .ThenBy(r => r.IsReducedTaxRate)
                .ThenBy(r => r.Location)
                )
            {
                if (rae.IsPayment != IsPayment) { continue; }
                //subjectの初期値を設定する
                if (subject == null) { subject = rae.Content.AccountingSubject; }
                //currentDateの初期値を設定する
                if (currentDate == DefaultDate) { currentDate = rae.AccountActivityDate; }
                if (string.IsNullOrEmpty(content)) { content = rae.Content.Text; }//contentの初期値を設定する}
                if (string.IsNullOrEmpty(location)) { location = rae.Location; }
                if (string.IsNullOrEmpty(creditDept)) { creditDept = rae.CreditDept.Dept; }
                if (string.IsNullOrEmpty(clerk)) { clerk = rae.RegistrationRep.FirstName; }
                contentCount++;

                string ass =
                    $"{rae.Content.AccountingSubject.Subject} : " +
                    $"{rae.Content.AccountingSubject.SubjectCode}";
                if (!string.IsNullOrEmpty(DataBaseConnect.GetBranchNumber
                    (rae.Content.AccountingSubject)))
                {
                    ass += $"-{DataBaseConnect.GetBranchNumber(rae.Content.AccountingSubject)}";
                }

                isNextSlip = IsSameData(rae, currentDate, creditDept, subject, content, location, isTaxRate);
                DateTime WizeCoreAccountingActivityDate = DefaultDate;

                if (isNextSlip)
                {
                    ItemIndex++;
                    TotalPrice += rae.Price;
                }
                else
                {
                    currentDate = rae.AccountActivityDate;//入出金日を代入
                    WizeCoreAccountingActivityDate = rae.AccountActivityDate;
                    subject = rae.Content.AccountingSubject;
                    content = rae.Content.Text;
                    TotalPrice = rae.Price;
                    creditDept = rae.CreditDept.Dept;
                    location = rae.Location;
                    clerk = rae.RegistrationRep.FirstName;
                    isTaxRate = rae.IsReducedTaxRate;
                    contentCount = 1;
                    ItemIndex = 1;
                    NextPage();//次のページへ
                    PageStyle();
                }

                //伝票1件目から5件目は一列目、6件目から10件目までは4列目に出力するので、
                //セルの場所を設定する
                if (contentCount <= 5)
                {
                    inputRow = StartRowPosition + contentCount;
                    inputContentColumn = 1;
                }
                else
                {
                    inputRow = StartRowPosition + contentCount - 5;
                    inputContentColumn = 11;
                }
                if (inputRow == 18)
                { return; }
                //伝票の詳細を設定したセルに出力する
                myWorksheet.Cell(inputRow, inputContentColumn).Value = rae.Price < 0
                    ? $@"{ReturnProvisoContent(rae)} {rae.Detail} " +
                        $"▲\\{CommaDelimitedAmount(rae.Price * -1)}-"
                    : $@"{ReturnProvisoContent(rae)} {rae.Detail} " +
                        $@"\{CommaDelimitedAmount(rae.Price)}-";
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
                if (LoginRep.GetInstance().Rep.FirstName == clerk)
                { myWorksheet.Cell(StartRowPosition + 6, 20).Value = LoginRep.GetInstance().Rep.FirstName; }
                else
                {
                    myWorksheet.Cell(StartRowPosition + 6, 20).Value = clerk;
                    myWorksheet.Cell(StartRowPosition + 6, 18).Value = LoginRep.GetInstance().Rep.FirstName;
                }
                //出力日
                myWorksheet.Cell(StartRowPosition + 10, 1).Value = OutputDate.Year;
                myWorksheet.Cell(StartRowPosition + 10, 2).Value = OutputDate.Month;
                myWorksheet.Cell(StartRowPosition + 10, 3).Value = OutputDate.Day;
                //勘定科目
                switch (mySlipType)
                {
                    case SlipType.Payment:
                        myWorksheet.Cell(StartRowPosition + 9, 14).Value = ass;
                        break;
                    case SlipType.Withdrawal:
                        myWorksheet.Cell(StartRowPosition + 9, 4).Value = ass;
                        break;
                    case SlipType.Transfer:
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

        protected override void SetList(IEnumerable outputList)
        { ReceiptsAndExpenditures = (ObservableCollection<ReceiptsAndExpenditure>)outputList; }
    }
}
