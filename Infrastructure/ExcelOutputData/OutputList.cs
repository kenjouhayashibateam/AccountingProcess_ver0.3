using Domain.Entities;
using Domain.Entities.ValueObjects;
using Domain.Repositories;
using System;
using System.Collections;
using System.Linq;

namespace Infrastructure.ExcelOutputData
{
    /// <summary>
    /// リスト出力
    /// </summary>
    internal abstract class OutputList : ExcelApp
    {
        /// <summary>
        /// データリストのインデックス
        /// </summary>
        protected int ItemIndex;
        /// <summary>
        /// データ出力のページ数
        /// </summary>
        protected int PageCount;
        /// <summary>
        /// データを出力するページの一番上のRow
        /// </summary>
        protected int StartRowPosition;
        /// <summary>
        /// 強制的に伝票や出納帳の項目を単独にする文字列配列
        /// </summary>
        protected string[] IndependentContent => new string[] { "違算金" };

        protected OutputList(IEnumerable outputList)
        {
            SetList(outputList);
            ItemIndex = 0;
            PageCount = 0;
            StartRowPosition = 1;
            myWorksheet.PageSetup.PaperSize = SheetPaperSize();
            myWorksheet.Style.Font.FontName = SetSheetFontName();
            NextPage();
            PageStyle();
        }
        /// <summary>
        /// 出力するリストを保持します
        /// </summary>
        /// <param name="outputList">出力リスト</param>
        protected abstract void SetList(IEnumerable outputList);
        /// <summary>
        /// データリストを出力します
        /// </summary>
        public abstract void Output();
        /// <summary>
        /// エクセルシートの用紙1ページごとのStyleを設定します
        /// </summary>
        protected abstract void PageStyle();
        /// <summary>
        /// 新しいページのStyleを設定します
        /// </summary>
        protected void NextPage()
        {
            PageCount++;
            StartRowPosition = (SetRowSizes().Length * (PageCount - 1)) + 1;
            for (int i = 0; i < SetRowSizes().Length; i++)
            { myWorksheet.Row(StartRowPosition + i).Height = SetRowSizes()[i]; }
            for (int i = 0; i < SetColumnSizes().Length; i++)
            { myWorksheet.Column(i + 1).Width = SetColumnSizes()[i]; }
        }
        /// <summary>
        /// 出納データを同一の要素に出力するかを返します
        /// </summary>
        /// <returns></returns>
        protected bool IsSameData
            (ReceiptsAndExpenditure validateReceiotsAndExpenditure, DateTime currentActivityDate,
                string currentDept, AccountingSubject currentSubject, string currentContent,
                string currentLocation, bool isTaxRate)
        {
            //出力ページの出納データが強制的に単独にする内容文字列配列に入っているものでPageCountが1なら
            //True。あるいは、出力ページの出納データが強制的に単独にする内容文字列配列に入っていない、
            //出力ページが比較するデータと、貸方部門が同じ、勘定科目コードが同じ、勘定科目が同じ、
            //入出金日が同じ、比較する出納データが強制的に単独にする内容文字列配列に含まれていない
            //、出力ページが10を超えていない、経理担当場所が同じ、出力ページの軽減税率チェックが比較する
            //出納データと同じ場合にTrueを返す
            bool b = (IndependentContent.Contains(currentContent) &&
                IndependentContent.Contains(validateReceiotsAndExpenditure.Content.Text) && PageCount == 1);
            if (!b)
            {
                b = (!IndependentContent.Contains(currentContent) &&
                    currentDept == validateReceiotsAndExpenditure.CreditDept.Dept &&
                    currentSubject.Equals(validateReceiotsAndExpenditure.Content.AccountingSubject) &&
                    currentActivityDate == validateReceiotsAndExpenditure.AccountActivityDate &&
                    !IndependentContent.Contains(validateReceiotsAndExpenditure.Content.Text) &&
                    ItemIndex < 10 && currentLocation == validateReceiotsAndExpenditure.Location &&
                    isTaxRate == validateReceiotsAndExpenditure.IsReducedTaxRate);
            }
            return b;
        }
        /// <summary>
        /// 振替データを同一の要素に出力するかを返します
        /// </summary>
        /// <returns></returns>
        protected bool IsSameData(TransferReceiptsAndExpenditure validateTransferReceiptsAndExpenditure,
            DateTime currentActivityDate, string currentDept,AccountingSubject currentCreditAccount,
            AccountingSubject currentDebitAccount, string currentContent, string currentLocation, bool isTaxRate)
        {
            //出力ページの振替データが強制的に単独にする内容文字列配列に入っているものでPageCountが1なら
            //True。あるいは、出力ページの振替データが強制的に単独にする内容文字列配列に入っていない、
            //出力ページが比較するデータと、貸方部門が同じ、貸方勘定科目コードが同じ、貸方勘定科目が同じ、
            //借方勘定科目コードが同じ、借方勘定科目が同じ、入出金日が同じ、比較する出納データが強制的に単独にする
            //内容文字列配列に含まれていない、出力ページが10を超えていない、経理担当場所が同じ、
            //出力ページの軽減税率チェックが比較する出納データと同じ場合にTrueを返す
            bool b = (IndependentContent.Contains(currentContent) &&
                IndependentContent.Contains(validateTransferReceiptsAndExpenditure.ContentText) && PageCount == 1);
            if (!b)
            {
                b = (!IndependentContent.Contains(currentContent) &&
                    currentDept == validateTransferReceiptsAndExpenditure.CreditDept.Dept &&
                    currentCreditAccount.Equals(validateTransferReceiptsAndExpenditure.CreditAccount) &&
                    currentDebitAccount.Equals(validateTransferReceiptsAndExpenditure.DebitAccount)&&
                    currentActivityDate == validateTransferReceiptsAndExpenditure.AccountActivityDate &&
                    !IndependentContent.Contains(validateTransferReceiptsAndExpenditure.ContentText) &&
                    ItemIndex < 10 && currentLocation == validateTransferReceiptsAndExpenditure.Location &&
                    isTaxRate == validateTransferReceiptsAndExpenditure.IsReducedTaxRate);
            }
            return b;
        }
        /// <summary>
        /// 提出書類に表記する内容を返します
        /// </summary>
        /// <param name="rae"></param>
        /// <returns></returns>
        protected static string ReturnProvisoContent(ReceiptsAndExpenditure rae)
        {
            IDataBaseConnect dbc = DefaultInfrastructure.GetDefaultDataBaseConnect();
            string s = dbc.CallContentConvertText(rae.Content.ID) ?? rae.Content.Text;
            return s == rae.Content.AccountingSubject.Subject ? string.Empty : s;
        }
    }
}
