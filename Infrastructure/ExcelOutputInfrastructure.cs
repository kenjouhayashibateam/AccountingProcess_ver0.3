using System;
using Domain.Entities;
using Domain.Entities.ValueObjects;
using Domain.Repositories;
using ClosedXML.Excel;
using System.IO;
using Microsoft.Office.Interop;
using Microsoft.Office.Interop.Excel;
using Microsoft.VisualBasic;
using DocumentFormat.OpenXml.Spreadsheet;
using ClosedXML;

namespace Infrastructure
{
    public class ExcelOutputInfrastructure : IDataOutput
    {

        private XLWorkbook myWorkbook;
        private Workbooks myWorkbooks;
        private IXLWorksheet myWorksheet;
        private readonly ILogger Logger;
        private Application App;
        private readonly string openPath = System.IO.Path.GetFullPath(Properties.Resources.SaveFolderPath + Properties.Resources.SaveFile);

        public ExcelOutputInfrastructure(ILogger logger)
        {
            Logger = logger;
        }

        public ExcelOutputInfrastructure() : this(new LogFile()) { }

        private void ExcelOpen()
        {
            myWorkbooks.Open(openPath);
            App.Visible = true;
            //try
            //{
            //    myWorkbook = new XLWorkbook();

            //    bool setInstance = true;
            //    FileStream fs = new FileStream(openPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            //    myWorkbook = new XLWorkbook(fs, XLEventTracking.Disabled);
            //    foreach (IXLWorksheet worksheet in myWorkbook.Worksheets)
            //    {
            //        if (worksheet.Name == "test")
            //        {
            //            setInstance = false;
            //        }
            //    }
            //    if (setInstance)
            //    {
            //        myWorkbook.AddWorksheet("test");
            //    }
            //    myWorkbook.SaveAs(fs);
            //}
            //catch (InvalidCastException e)
            //{
            //    Logger.Log(ILogger.LogInfomation.ERROR, e.Message);
            //}
        }

        private void ExcelClose()
        {
            App = (Application)Interaction.GetObject(Class : "Excel.Application");

            myWorkbooks = App.Workbooks;

            foreach(Microsoft.Office.Interop.Excel.Workbook wb in myWorkbooks)
            {
                if (wb.Name==Properties.Resources.SaveFile)
                {
                    wb.Close(SaveChanges:false);
                }
            }

            if( myWorkbooks.Count==0)
            {
                App.Quit();
            }    
        }

        public void CashBoxDataOutput(Cashbox cashbox)
        {
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

            myWorksheet.Range(myWorksheet.Cell(15, 5), myWorksheet.Cell(15, 7)).Merge();
            myWorksheet.Range(myWorksheet.Cell(16, 5), myWorksheet.Cell(16, 7)).Merge();
            myWorksheet.Range(myWorksheet.Cell(17, 5), myWorksheet.Cell(17, 7)).Merge();
            myWorksheet.Range(myWorksheet.Cell(18, 5), myWorksheet.Cell(18, 7)).Merge();
            myWorksheet.Range(myWorksheet.Cell(20, 1), myWorksheet.Cell(20, 7)).Merge();

            Double[] RowSizes = new Double[] { 18.75, 41.25, 18.75, 18.75, 18.75, 18.75, 18.75, 18.75, 18.75, 18.75, 18.75, 18.75, 18.75, 18.75, 18.75, 18.75, 18.75, 18.75, 18.75, 32.25 };
            Double[] ColumnSizes =new Double[] { 10.38, 6.88, 14.38, 10.38, 6.75, 6.75, 6.75 };

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

            myWorksheet.Range(myWorksheet.Cell(15,1),myWorksheet.Cell(18,7)).Style
                .Border.SetBottomBorder(XLBorderStyleValues.Thin)
                .Border.SetTopBorder(XLBorderStyleValues.Thin)
                .Border.SetLeftBorder(XLBorderStyleValues.Thin)
                .Border.SetRightBorder(XLBorderStyleValues.Thin);

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

            myWorksheet.Cell(6, 4).Value = "束金種";
            myWorksheet.Cell(6, 5).Value = "数量";
            myWorksheet.Cell(6, 6).Value = "金額";

            myWorksheet.Range(myWorksheet.Cell(7, 4), myWorksheet.Cell(12, 4)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            myWorksheet.Range(myWorksheet.Cell(7, 4), myWorksheet.Cell(12, 4)).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            myWorksheet.Cell(7, 4).Value = "500円束";
            myWorksheet.Cell(8, 4).Value = "100円束";
            myWorksheet.Cell(9, 4).Value = "50円束";
            myWorksheet.Cell(10, 4).Value = "10円束";
            myWorksheet.Cell(11, 4).Value = "5円束";
            myWorksheet.Cell(12, 4).Value = "1円束";

            myWorksheet.Range(myWorksheet.Cell(4, 2), myWorksheet.Cell(12, 2)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            myWorksheet.Range(myWorksheet.Cell(4, 2), myWorksheet.Cell(12, 2)).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            myWorksheet.Cell(4, 2).Value = cashbox.MoneyCategorys[MoneyCategory.Denomination.TenThousandYen].Count;
            myWorksheet.Cell(5, 2).Value = cashbox.MoneyCategorys[MoneyCategory.Denomination.FiveThousandYen].Count;
            myWorksheet.Cell(6, 2).Value = cashbox.MoneyCategorys[MoneyCategory.Denomination.OneThousandYen].Count;
            myWorksheet.Cell(7, 2).Value = cashbox.MoneyCategorys[MoneyCategory.Denomination.FiveHundredYen].Count;
            myWorksheet.Cell(8, 2).Value = cashbox.MoneyCategorys[MoneyCategory.Denomination.OneHundredYen].Count;
            myWorksheet.Cell(9, 2).Value = cashbox.MoneyCategorys[MoneyCategory.Denomination.FiftyYen].Count;
            myWorksheet.Cell(10, 2).Value = cashbox.MoneyCategorys[MoneyCategory.Denomination.TenYen].Count;
            myWorksheet.Cell(11, 2).Value = cashbox.MoneyCategorys[MoneyCategory.Denomination.FiveYen].Count;
            myWorksheet.Cell(12, 2).Value = cashbox.MoneyCategorys[MoneyCategory.Denomination.OneYen].Count;

            myWorksheet.Range(myWorksheet.Cell(4, 3), myWorksheet.Cell(12, 3)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            myWorksheet.Range(myWorksheet.Cell(4, 3), myWorksheet.Cell(12, 3)).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            myWorksheet.Cell(4, 3).Value = cashbox.MoneyCategorys[MoneyCategory.Denomination.TenThousandYen].AmountWithUnit();
            myWorksheet.Cell(5, 3).Value = cashbox.MoneyCategorys[MoneyCategory.Denomination.FiveThousandYen].AmountWithUnit();
            myWorksheet.Cell(6, 3).Value = cashbox.MoneyCategorys[MoneyCategory.Denomination.OneThousandYen].AmountWithUnit();
            myWorksheet.Cell(7, 3).Value = cashbox.MoneyCategorys[MoneyCategory.Denomination.FiveHundredYen].AmountWithUnit();
            myWorksheet.Cell(8, 3).Value = cashbox.MoneyCategorys[MoneyCategory.Denomination.OneHundredYen].AmountWithUnit();
            myWorksheet.Cell(9, 3).Value = cashbox.MoneyCategorys[MoneyCategory.Denomination.FiftyYen].AmountWithUnit();
            myWorksheet.Cell(10, 3).Value = cashbox.MoneyCategorys[MoneyCategory.Denomination.TenYen].AmountWithUnit();
            myWorksheet.Cell(11, 3).Value = cashbox.MoneyCategorys[MoneyCategory.Denomination.FiveYen].AmountWithUnit();
            myWorksheet.Cell(12, 3).Value = cashbox.MoneyCategorys[MoneyCategory.Denomination.OneYen].AmountWithUnit();

            myWorksheet.Range(myWorksheet.Cell(7, 5), myWorksheet.Cell(7, 12)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            myWorksheet.Range(myWorksheet.Cell(7, 5), myWorksheet.Cell(7, 12)).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            myWorksheet.Cell(7, 5).Value = cashbox.MoneyCategorys[MoneyCategory.Denomination.FiveHundredYenBundle].Count;
            myWorksheet.Cell(8, 5).Value = cashbox.MoneyCategorys[MoneyCategory.Denomination.OneHundredYenBundle].Count;
            myWorksheet.Cell(9, 5).Value = cashbox.MoneyCategorys[MoneyCategory.Denomination.FiftyYenBundle].Count;
            myWorksheet.Cell(10, 5).Value = cashbox.MoneyCategorys[MoneyCategory.Denomination.TenYenBundle].Count;
            myWorksheet.Cell(11, 5).Value = cashbox.MoneyCategorys[MoneyCategory.Denomination.FiveYenBundle].Count;
            myWorksheet.Cell(12, 5).Value = cashbox.MoneyCategorys[MoneyCategory.Denomination.OneYenBundle].Count;

            myWorksheet.Range(myWorksheet.Cell(7, 6), myWorksheet.Cell(12, 6)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            myWorksheet.Range(myWorksheet.Cell(7, 6), myWorksheet.Cell(12, 6)).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            myWorksheet.Cell(7, 6).Value = cashbox.MoneyCategorys[MoneyCategory.Denomination.FiveHundredYenBundle].AmountWithUnit();
            myWorksheet.Cell(8, 6).Value = cashbox.MoneyCategorys[MoneyCategory.Denomination.OneHundredYenBundle].AmountWithUnit();
            myWorksheet.Cell(9, 6).Value = cashbox.MoneyCategorys[MoneyCategory.Denomination.FiftyYenBundle].AmountWithUnit();
            myWorksheet.Cell(10, 6).Value = cashbox.MoneyCategorys[MoneyCategory.Denomination.TenYenBundle].AmountWithUnit();
            myWorksheet.Cell(11, 6).Value = cashbox.MoneyCategorys[MoneyCategory.Denomination.FiveYenBundle].AmountWithUnit();
            myWorksheet.Cell(12, 6).Value = cashbox.MoneyCategorys[MoneyCategory.Denomination.OneYenBundle].AmountWithUnit();

            myWorksheet.Cell(14, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            myWorksheet.Cell(14, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            myWorksheet.Cell(14, 1).Value = "金庫等";

            for(int i=0;i<cashbox.OtherMoneys.Length;i++)
            {

            }

            myWorksheet.Cell(20, 1).Style.Font.Bold = true;
            myWorksheet.Cell(20, 1).Style.Font.FontSize = 20;
            myWorksheet.Cell(20, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            myWorksheet.Cell(20, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            myWorksheet.Cell(20, 1).Value = $"合計　{cashbox.GetTotalAmountWithUnit()}";

            myWorkbook.SaveAs(openPath);
            ExcelOpen();
        }
        private double ToInch(double x)
        {
            return x * 0.39370;
        }
    }
}
