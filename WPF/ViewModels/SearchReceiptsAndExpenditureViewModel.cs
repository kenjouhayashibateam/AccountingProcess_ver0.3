using Domain.Entities.ValueObjects;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.ViewModels.Datas;

namespace WPF.ViewModels
{
    public class SearchReceiptsAndExpenditureViewModel(IDataBaseConnect) : BaseViewModel
    {
        public override void SetRep(Rep rep)
        {
            throw new NotImplementedException();
        }

        public override void ValidationProperty(string propertyName, object value)
        {
            throw new NotImplementedException();
        }

        protected override void SetWindowDefaultTitle()
        {
            throw new NotImplementedException();
        }
    }
}
