using Domain.Entities.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.ValueObjects
{
    public class OtherMoney
    {
        public string Title;
        public int Amount;

        public string AmountWithUnit()
        {
            if(Amount<1)
            {
                return string.Empty;
            }
            else
            {
                return AmountHelper.AmountWithUnit(Amount);
            }
        }
    }
}
