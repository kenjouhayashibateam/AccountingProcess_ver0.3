using System;
using Domain.Entities;
using Domain.Repositories;
using ClosedXML.Excel;
using Microsoft.Office.Interop.Excel;
using Microsoft.VisualBasic;
using static Domain.Entities.ValueObjects.MoneyCategory.Denomination;

namespace Infrastructure
{
    /// <summary>
    /// エクセル出力クラス
    /// </summary>
    public class ExcelOutputInfrastructure : IDataOutput
    {
        /// <summary>
        /// ClosedXML : ワークブック
        /// </summary>
        private XLWorkbook myWorkbook;
        /// <summary>
        /// Excel : ワークブック
        /// </summary>
        private Workbooks myWorkbooks;
        /// <summary>
        /// ClosedXML : ワークシート
        /// </summary>
        private IXLWorksheet myWorksheet;
        /// <summary>
        /// ログインフラストラクチャ
        /// </summary>
        private readonly ILogger Logger;
        /// <summary>
        /// エクセルアプリケーション
        /// </summary>
        private Application App;
        /// <summary>
        /// エクセルファイルを保存しているフォルダのFullPath
        /// </summary>
        private readonly string openPath = System.IO.Path.GetFullPath(Properties.Resources.SaveFolderPath + Properties.Resources.SaveFile);

        /// <summary>
        /// コンストラクタ　ログ保存のインフラストラクチャを設定します
        /// </summary>
        /// <param name="logger"></param>
        public ExcelOutputInfrastructure(ILogger logger)
        {
            Logger = logger;
        }

        public ExcelOutputInfrastructure() : this(new LogFileInfrastructure()) { }

        /// <summary>
        /// デストラクタ　エクセルプロセスを開放します
        /// </summary>
        ~ExcelOutputInfrastructure()
        {
            if(App==null)
            {
                return;
            }
            if(App.Workbooks.Count==0)
            {
                App.Quit();
            }
        }
        /// <summary>
        /// データ出力エクセルファイルを開きます
        /// </summary>
        private void ExcelOpen()
        {
            myWorkbooks.Open(Filename: openPath, ReadOnly: true);
            App.Visible = true;
        }
        /// <summary>
        /// エクセルアプリケーションを呼び出します
        /// </summary>
        private void CallExcelApplication()
        {
            try
            {
                App = (Application)Interaction.GetObject(Class : "Excel.Application");
            }
            catch
            {
                App = new Application();
            }
        }
        /// <summary>
        /// データ出力エクセルファイルを閉じます
        /// </summary>
        private void ExcelClose()
        {
            CallExcelApplication();

            myWorkbooks = App.Workbooks;
            //出力ファイルを検出して閉じる
            foreach(Microsoft.Office.Interop.Excel.Workbook wb in myWorkbooks)
            {
                if (wb.Name==Properties.Resources.SaveFile)
                {
                    wb.Close(SaveChanges:false);
                }
            }
            //開いているワークブックがなければエクセルアプリケーションを終了する
            if( myWorkbooks.Count==0)
            {
                App.Quit();
            }    
        }
        /// <summary>
        /// 金庫データを出力します※テンプレートパターンを使ってリファクタリングする
        /// </summary>
        public void CashBoxDataOutput()
        {
            Cashbox myCashbox = Cashbox.GetInstance();
           
            ExcelClose();
            myWorkbook = new XLWorkbook();

            myWorksheet = myWorkbook.AddWorksheet(Properties.Resources.SheetName);

            myWorksheet .Style.Font.FontName = "ＭＳ ゴシック";

            myWorksheet.Range(myWorksheet.Cell(1, 1), myWorksheet.Cell(20, 7)).Style.NumberFormat.Format = "@";

            myWorksheet.PageSetup.PaperSize = XLPaperSize.B5Paper;

            myWorksheet.PageSetup.Margins.Top = ToInch(1.91);
            myWorksheet.PageSetup.Margins.Left = ToInch(1.78);
            myWorksheet.PageSetup.Margins.Right = ToInch(1.78);
            myWorksheet.PageSetup.Margins.Bottom = ToInch(1.91);

            myWorksheet.Range(myWorksheet.Cell(1, 1), myWorksheet.Cell(1, 3)).Merge();
            myWorksheet.Range(myWorksheet.Cell(6, 6), myWorksheet.Cell(6, 7)).Merge();
            myWorksheet.Range(myWorksheet.Cell(7, 6), myWorksheet.Cell(7, 7)).Merge();
            myWorksheet.Range(myWorksheet.Cell(8, 6), myWorksheet.Cell(8, 7)).Merge();
            myWorksheet.Range(myWorksheet.Cell(9, 6), myWorksheet.Cell(9, 7)).Merge();
            myWorksheet.Range(myWorksheet.Cell(10, 6), myWorksheet.Cell(10, 7)).Merge();
            myWorksheet.Range(myWorksheet.Cell(11, 6), myWorksheet.Cell(11, 7)).Merge();
            myWorksheet.Range(myWorksheet.Cell(12, 6), myWorksheet.Cell(12, 7)).Merge();

            myWorksheet.Range(myWorksheet.Cell(15, 2), myWorksheet.Cell(15, 3)).Merge();
            myWorksheet.Range(myWorksheet.Cell(16, 2), myWorksheet.Cell(16, 3)).Merge();
            myWorksheet.Range(myWorksheet.Cell(17, 2), myWorksheet.Cell(17, 3)).Merge();
            myWorksheet.Range(myWorksheet.Cell(18, 2), myWorksheet.Cell(18, 3)).Merge();
            myWorksheet.Range(myWorksheet.Cell(19, 2), myWorksheet.Cell(19, 3)).Merge();

            myWorksheet.Range(myWorksheet.Cell(15, 5), myWorksheet.Cell(15, 7)).Merge();
            myWorksheet.Range(myWorksheet.Cell(16, 5), myWorksheet.Cell(16, 7)).Merge();
            myWorksheet.Range(myWorksheet.Cell(17, 5), myWorksheet.Cell(17, 7)).Merge();
            myWorksheet.Range(myWorksheet.Cell(18, 5), myWorksheet.Cell(18, 7)).Merge();
            myWorksheet.Range(myWorksheet.Cell(19, 5), myWorksheet.Cell(19, 7)).Merge();
            myWorksheet.Range(myWorksheet.Cell(21, 1), myWorksheet.Cell(21, 7)).Merge();

            Double[] RowSizes = new Double[] { 18.75, 41.25, 18.75, 18.75, 18.75, 18.75, 18.75, 18.75, 18.75, 18.75, 18.75, 18.75, 18.75, 18.75, 18.75, 18.75, 18.75, 18.75, 18.75,18.75, 32.25 };
            Double[] ColumnSizes =new Double[] { 12.86, 6.88, 14.38, 12.86, 6.75, 6.75, 6.75 };

            for(int i=0;i<RowSizes.Length;i++)
            {
                myWorksheet.Rows((i+1).ToString()).Height = RowSizes[i];
            }

            for(int i=0;i<ColumnSizes.Length;i++)
            {
                myWorksheet.Columns((i+1).ToString()).Width = ColumnSizes[i];
            }

            myWorksheet.Range(myWorksheet.Cell(1, 5), myWorksheet.Cell(2, 7)).Style
                .Border.SetBottomBorder(XLBorderStyleValues.Thin)
                .Border.SetTopBorder(XLBorderStyleValues.Thin)
                .Border.SetLeftBorder(XLBorderStyleValues.Thin)
                .Border.SetRightBorder(XLBorderStyleValues.Thin);

            myWorksheet.Range(myWorksheet.Cell(3,1),myWorksheet.Cell(12,3)).Style
                .Border.SetBottomBorder(XLBorderStyleValues.Thin)
                .Border.SetTopBorder(XLBorderStyleValues.Thin)
                .Border.SetLeftBorder(XLBorderStyleValues.Thin)
                .Border.SetRightBorder(XLBorderStyleValues.Thin);

            myWorksheet.Range(myWorksheet.Cell(6,4),myWorksheet.Cell(12,7)).Style
                .Border.SetBottomBorder(XLBorderStyleValues.Thin)
                .Border.SetTopBorder(XLBorderStyleValues.Thin)
                .Border.SetLeftBorder(XLBorderStyleValues.Thin)
                .Border.SetRightBorder(XLBorderStyleValues.Thin);

            myWorksheet.Range(myWorksheet.Cell(15,1),myWorksheet.Cell(19,7)).Style
                .Border.SetBottomBorder(XLBorderStyleValues.Thin)
                .Border.SetTopBorder(XLBorderStyleValues.Thin)
                .Border.SetLeftBorder(XLBorderStyleValues.Thin)
                .Border.SetRightBorder(XLBorderStyleValues.Thin);
            myWorksheet.Range(myWorksheet.Cell(15, 4), myWorksheet.Cell(19, 4)).Style.Border.SetLeftBorder(XLBorderStyleValues.Double);

            DateTime Now = DateTime.Now;

            myWorksheet.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            myWorksheet.Cell(1, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            myWorksheet.Cell(1, 1).Value =Now.ToString("yyyy年MM月dd日");

            myWorksheet.Range(myWorksheet.Cell(1, 5), myWorksheet.Cell(1, 7)).Style.Alignment.Horizontal=XLAlignmentHorizontalValues.Center;
            myWorksheet.Range(myWorksheet.Cell(1, 5), myWorksheet.Cell(1, 7)).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            myWorksheet.Cell(1, 5).Value = "本部長";
            myWorksheet.Cell(1, 6).Value = "副住職";
            myWorksheet.Cell(1, 7).Value = "係";

            myWorksheet.Range(myWorksheet.Cell(3, 1), myWorksheet.Cell(3, 3)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            myWorksheet.Range(myWorksheet.Cell(3, 1), myWorksheet.Cell(3, 3)).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            myWorksheet.Cell(3, 1).Value = "金種";
            myWorksheet.Cell(3, 2).Value = "数量";
            myWorksheet.Cell(3, 3).Value = "金額";

            myWorksheet.Range(myWorksheet.Cell(4, 1), myWorksheet.Cell(12, 1)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            myWorksheet.Range(myWorksheet.Cell(4, 1), myWorksheet.Cell(12, 1)).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            myWorksheet.Cell(4, 1).Value = "1万円";
            myWorksheet.Cell(5, 1).Value = "5千円";
            myWorksheet.Cell(6, 1).Value = "1千円";
            myWorksheet.Cell(7, 1).Value = "500円";
            myWorksheet.Cell(8, 1).Value = "100円";
            myWorksheet.Cell(9, 1).Value = "50円";
            myWorksheet.Cell(10, 1).Value = "10円";
            myWorksheet.Cell(11, 1).Value = "5円";
            myWorksheet.Cell(12, 1).Value = "1円";

            myWorksheet.Range(myWorksheet.Cell(6, 4), myWorksheet.Cell(6, 6)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            myWorksheet.Range(myWorksheet.Cell(6, 4), myWorksheet.Cell(6, 6)).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            myWorksheet.Cell(6, 4).Value = $"{Cashbox.BundleCount}枚束";
            myWorksheet.Cell(6, 5).Value = "数量";
            myWorksheet.Cell(6, 6).Value = "金額";

            myWorksheet.Range(myWorksheet.Cell(7, 4), myWorksheet.Cell(12, 4)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            myWorksheet.Range(myWorksheet.Cell(7, 4), myWorksheet.Cell(12, 4)).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            myWorksheet.Cell(7, 4).Value = "500円";
            myWorksheet.Cell(8, 4).Value = "100円";
            myWorksheet.Cell(9, 4).Value = "50円";
            myWorksheet.Cell(10, 4).Value = "10円";
            myWorksheet.Cell(11, 4).Value = "5円";
            myWorksheet.Cell(12, 4).Value = "1円";

            myWorksheet.Range(myWorksheet.Cell(4, 2), myWorksheet.Cell(12, 2)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            myWorksheet.Range(myWorksheet.Cell(4, 2), myWorksheet.Cell(12, 2)).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            myWorksheet.Cell(4, 2).Value = myCashbox.MoneyCategorys[TenThousandYen].Count;
            myWorksheet.Cell(5, 2).Value = myCashbox.MoneyCategorys[FiveThousandYen].Count;
            myWorksheet.Cell(6, 2).Value = myCashbox.MoneyCategorys[OneThousandYen].Count;
            myWorksheet.Cell(7, 2).Value = myCashbox.MoneyCategorys[FiveHundredYen].Count;
            myWorksheet.Cell(8, 2).Value = myCashbox.MoneyCategorys[OneHundredYen].Count;
            myWorksheet.Cell(9, 2).Value = myCashbox.MoneyCategorys[FiftyYen].Count;
            myWorksheet.Cell(10, 2).Value = myCashbox.MoneyCategorys[TenYen].Count;
            myWorksheet.Cell(11, 2).Value = myCashbox.MoneyCategorys[FiveYen].Count;
            myWorksheet.Cell(12, 2).Value = myCashbox.MoneyCategorys[OneYen].Count;

            myWorksheet.Range(myWorksheet.Cell(4, 3), myWorksheet.Cell(12, 3)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            myWorksheet.Range(myWorksheet.Cell(4, 3), myWorksheet.Cell(12, 3)).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            myWorksheet.Cell(4, 3).Value = myCashbox.MoneyCategorys[TenThousandYen].AmountWithUnit();
            myWorksheet.Cell(5, 3).Value = myCashbox.MoneyCategorys[FiveThousandYen].AmountWithUnit();
            myWorksheet.Cell(6, 3).Value = myCashbox.MoneyCategorys[OneThousandYen].AmountWithUnit();
            myWorksheet.Cell(7, 3).Value = myCashbox.MoneyCategorys[FiveHundredYen].AmountWithUnit();
            myWorksheet.Cell(8, 3).Value = myCashbox.MoneyCategorys[OneHundredYen].AmountWithUnit();
            myWorksheet.Cell(9, 3).Value = myCashbox.MoneyCategorys[FiftyYen].AmountWithUnit();
            myWorksheet.Cell(10, 3).Value = myCashbox.MoneyCategorys[TenYen].AmountWithUnit();
            myWorksheet.Cell(11, 3).Value = myCashbox.MoneyCategorys[FiveYen].AmountWithUnit();
            myWorksheet.Cell(12, 3).Value = myCashbox.MoneyCategorys[OneYen].AmountWithUnit();

            myWorksheet.Range(myWorksheet.Cell(7, 5), myWorksheet.Cell(12, 12)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            myWorksheet.Range(myWorksheet.Cell(7, 5), myWorksheet.Cell(12, 12)).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            myWorksheet.Cell(7, 5).Value = myCashbox.MoneyCategorys[FiveHundredYenBundle].Count;
            myWorksheet.Cell(8, 5).Value = myCashbox.MoneyCategorys[OneHundredYenBundle].Count;
            myWorksheet.Cell(9, 5).Value = myCashbox.MoneyCategorys[FiftyYenBundle].Count;
            myWorksheet.Cell(10, 5).Value = myCashbox.MoneyCategorys[TenYenBundle].Count;
            myWorksheet.Cell(11, 5).Value = myCashbox.MoneyCategorys[FiveYenBundle].Count;
            myWorksheet.Cell(12, 5).Value = myCashbox.MoneyCategorys[OneYenBundle].Count;

            myWorksheet.Range(myWorksheet.Cell(7, 6), myWorksheet.Cell(12, 6)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            myWorksheet.Range(myWorksheet.Cell(7, 6), myWorksheet.Cell(12, 6)).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            myWorksheet.Cell(7, 6).Value = myCashbox.MoneyCategorys[FiveHundredYenBundle].AmountWithUnit();
            myWorksheet.Cell(8, 6).Value = myCashbox.MoneyCategorys[OneHundredYenBundle].AmountWithUnit();
            myWorksheet.Cell(9, 6).Value = myCashbox.MoneyCategorys[FiftyYenBundle].AmountWithUnit();
            myWorksheet.Cell(10, 6).Value = myCashbox.MoneyCategorys[TenYenBundle].AmountWithUnit();
            myWorksheet.Cell(11, 6).Value = myCashbox.MoneyCategorys[FiveYenBundle].AmountWithUnit();
            myWorksheet.Cell(12, 6).Value = myCashbox.MoneyCategorys[OneYenBundle].AmountWithUnit();

            myWorksheet.Cell(14, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            myWorksheet.Cell(14, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            myWorksheet.Range(myWorksheet.Cell(15, 1), myWorksheet.Cell(19, 7)).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            myWorksheet.Range(myWorksheet.Cell(15, 1), myWorksheet.Cell(15, 7)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            myWorksheet.Range(myWorksheet.Cell(16, 1), myWorksheet.Cell(19, 1)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            myWorksheet.Range(myWorksheet.Cell(16, 2), myWorksheet.Cell(19, 2)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            myWorksheet.Range(myWorksheet.Cell(16, 4), myWorksheet.Cell(19, 4)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            myWorksheet.Range(myWorksheet.Cell(16, 5), myWorksheet.Cell(19, 5)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

            myWorksheet.Cell(14, 1).Value = "釣り銭等";
            myWorksheet.Cell(15, 1).Value = "内容";
            myWorksheet.Cell(15, 2).Value = "金額";
            myWorksheet.Cell(15, 4).Value = "内容";
            myWorksheet.Cell(15, 5).Value = "金額";

            for (int i = 0; i < myCashbox.OtherMoneys.Length; i++)
            {
                if(i<4)
                {
                    myWorksheet.Cell(16 + i, 1).Value = myCashbox.OtherMoneys[i].Title;
                    myWorksheet.Cell(16 + i, 2).Value = myCashbox.OtherMoneys[i].AmountWithUnit();
                }
                else
                {
                    myWorksheet.Cell(16 + (i - 4), 4).Value = myCashbox.OtherMoneys[i].Title;
                    myWorksheet.Cell(16 + (i - 4), 5).Value = myCashbox.OtherMoneys[i].AmountWithUnit();
                }
            }

            myWorksheet.Cell(21, 1).Style.Font.Bold = true;
            myWorksheet.Cell(21, 1).Style.Font.FontSize = 20;
            myWorksheet.Cell(21, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            myWorksheet.Cell(21, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            myWorksheet.Cell(21, 1).Value = $"合計　{myCashbox.GetTotalAmountWithUnit()}";

            myWorkbook.SaveAs(openPath);
            ExcelOpen();
        }
        /// <summary>
        /// メートル法の数字をインチ法で返します
        /// </summary>
        /// <param name="x">メートル法での長さ</param>
        /// <returns>インチ法での長さ</returns>
        private double ToInch(double x)
        {
            return x * 0.39370;
        }
    }
}
