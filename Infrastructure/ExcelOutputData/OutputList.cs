using Domain.Entities;
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
            (ReceiptsAndExpenditure validateReceiotsAndExpenditure, DateTime currentActivityDate, string currentDept,
                string currentSubjectCode, string currentSubject, string currentContent, string currentLocation, bool isTaxRate)
        {
            if(IndependentContent.Contains(currentContent))
            {
                return false;
            }
            return currentDept == validateReceiotsAndExpenditure.CreditDept.Dept &&
                currentSubjectCode == validateReceiotsAndExpenditure.Content.AccountingSubject.SubjectCode &&
                currentSubject == validateReceiotsAndExpenditure.Content.AccountingSubject.Subject &&
                currentActivityDate == validateReceiotsAndExpenditure.AccountActivityDate &&
                !IndependentContent.Contains(validateReceiotsAndExpenditure.Content.Text) &&
                ItemIndex < 10 && currentLocation == validateReceiotsAndExpenditure.Location &&
                isTaxRate == validateReceiotsAndExpenditure.IsReducedTaxRate;
        }
    }
}
