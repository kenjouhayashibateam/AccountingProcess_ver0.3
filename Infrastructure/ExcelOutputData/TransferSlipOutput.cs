using Domain.Entities;
using Domain.Entities.ValueObjects;
using Domain.Repositories;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using static Domain.Entities.Helpers.DataHelper;
using static Domain.Entities.Helpers.TextHelper;

namespace Infrastructure.ExcelOutputData
{
    /// <summary>
    /// 振替伝票出力クラス
    /// </summary>
    internal class TransferSlipOutput : SlipOutputBase
    {
        ObservableCollection<TransferReceiptsAndExpenditure> TransferReceiptsAndExpenditures;
        private readonly IDataBaseConnect DataBaseConnect =
            DefaultInfrastructure.GetDefaultDataBaseConnect();

        internal TransferSlipOutput(ObservableCollection<TransferReceiptsAndExpenditure> outputList) : base(outputList)
        {}
        private void SlipDataOutput()
        {
                AccountingSubject debitSubject = null;
                AccountingSubject creditSubject = null;
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

            foreach (TransferReceiptsAndExpenditure trae in TransferReceiptsAndExpenditures.OrderByDescending
                (r => r.CreditDept.Dept)
                .ThenBy(r => r.AccountActivityDate)
                .ThenBy(r => r.DebitAccount.ID)
                .ThenBy(r => r.DebitAccount.SubjectCode)
                .ThenBy(r => r.DebitAccount.Subject)
                .ThenBy(r => r.CreditAccount.ID)
                .ThenBy(r => r.CreditAccount.SubjectCode)
                .ThenBy(r => r.CreditAccount.Subject)
                .ThenBy(r => r.IsReducedTaxRate)
                .ThenBy(r => r.Location)
                )
            {
                //勘定科目の初期値を設定する
                if (debitSubject == null) { debitSubject = trae.DebitAccount; }
                if (creditSubject == null) { creditSubject = trae.CreditAccount; }
                //currentDateの初期値を設定する
                if (currentDate == DefaultDate) { currentDate = trae.AccountActivityDate; }
                if (string.IsNullOrEmpty(content)) { content = trae.ContentText; }//contentの初期値を設定する}
                if (string.IsNullOrEmpty(location)) { location = trae.Location; }
                if (string.IsNullOrEmpty(creditDept)) { creditDept = trae.CreditDept.Dept; }
                if (string.IsNullOrEmpty(clerk)) { clerk = trae.RegistrationRep.FirstName; }
                contentCount++;

                isNextSlip = IsSameData(trae, currentDate, creditDept, creditSubject, debitSubject,
                    content, location, isTaxRate);
                DateTime WizeCoreAccountingActivityDate = DefaultDate;

                if (isNextSlip)
                {
                    ItemIndex++;
                    TotalPrice += trae.Price;
                }
                else
                {
                    currentDate = trae.AccountActivityDate;//入出金日を代入
                    WizeCoreAccountingActivityDate = trae.AccountActivityDate;
                    creditSubject = trae.CreditAccount;
                    debitSubject = trae.DebitAccount;
                    content = trae.ContentText;
                    TotalPrice = trae.Price;
                    creditDept = trae.CreditDept.Dept;
                    location = trae.Location;
                    clerk = trae.RegistrationRep.FirstName;
                    isTaxRate = trae.IsReducedTaxRate;
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
                myWorksheet.Cell(inputRow, inputContentColumn).Value = trae.Price < 0
                    ? $@"{trae.ContentText} {trae.Detail} " +
                        $"▲\\{CommaDelimitedAmount(trae.Price * -1)}-" :
                        $@"{trae.ContentText} {trae.Detail} " +
                        $@"\{CommaDelimitedAmount(trae.Price)}-";
                //{rae.Content.Text}伝票の一番上の右の欄に入金日を出力                
                myWorksheet.Cell(StartRowPosition + 1, 16).Value =
                    $"{trae.AccountActivityDate:M/d}";
                //経理担当場所、伝票の総額、担当者、伝票作成日、勘定科目、コード、貸方部門を出力
                myWorksheet.Cell(StartRowPosition + 2, 16).Value = trae.Location;
                string s = trae.IsReducedTaxRate ? "※軽減税率" : string.Empty;
                myWorksheet.Cell(StartRowPosition + 3, 16).Value = s;
                for (int i = 0; i < TotalPrice.ToString().Length; i++)
                {
                    myWorksheet.Cell(StartRowPosition + 10, 13 - i).Value =
                        TotalPrice.ToString().Substring(TotalPrice.ToString().Length - 1 - i, 1);
                }
                DateTime OutputDate = DateTime.Today;
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
                //貸方勘定科目
                string css = $"{trae.CreditAccount.Subject}：{trae.CreditAccount.SubjectCode}";
                if (!string.IsNullOrEmpty(DataBaseConnect.GetBranchNumber(trae.CreditAccount)))
                { css += $"-{DataBaseConnect.GetBranchNumber(trae.CreditAccount)}"; }
                myWorksheet.Cell(StartRowPosition + 9, 14).Value = css;
                //借方勘定科目
                string dss = $"{trae.DebitAccount.Subject}：{trae.DebitAccount.SubjectCode}";
                if(!string.IsNullOrEmpty(DataBaseConnect.GetBranchNumber(trae.DebitAccount)))
                { dss += $"-{DataBaseConnect.GetBranchNumber(trae.DebitAccount)}"; }
                myWorksheet.Cell(StartRowPosition + 9, 4).Value = dss;

                s = trae.CreditDept.ID == "credit_dept3" ? string.Empty : trae.CreditDept.Dept;
                myWorksheet.Cell(StartRowPosition + 9, 16).Value = s;
            }
        }
        public override void Output()
        {
            SlipDataOutput();
            ExcelOpen();
        }

        protected override void SetList(IEnumerable outputList)
        { TransferReceiptsAndExpenditures = (ObservableCollection<TransferReceiptsAndExpenditure>)outputList; }
    }
}
