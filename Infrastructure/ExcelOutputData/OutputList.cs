using Domain.Entities;
using System.Collections.ObjectModel;

namespace Infrastructure.ExcelOutputData
{
    internal abstract class OutputList : OutputData
    {
        private ObservableCollection<ReceiptsAndExpenditure> ReceiptsAndExpenditureOutputs;

        protected OutputList(ObservableCollection<ReceiptsAndExpenditure> receiptsAndExpenditures)
        {
            ReceiptsAndExpenditureOutputs = receiptsAndExpenditures;
        }
    }
}
