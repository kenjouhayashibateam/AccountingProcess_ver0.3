using System;
using Domain.Entities;
using Domain.Repositories;
using ClosedXML.Excel;
using System.IO;
using Microsoft.Office.Interop;
using Microsoft.Office.Interop.Excel;
using Microsoft.VisualBasic;

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

            foreach(Workbook wb in myWorkbooks)
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
            myWorksheet.Cells().Clear();
            myWorksheet.Cells().Style.NumberFormat.Format = "@";
            myWorkbook.SaveAs(openPath);
            ExcelOpen();
        }
    }
}
