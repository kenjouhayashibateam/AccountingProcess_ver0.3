using Domain.Entities.ValueObjects;
using Domain.Repositories;
using System.Collections.ObjectModel;

namespace Infrastructure
{
    public class LocalConectInfrastructure : IDataBaseConnect
    {
        public ObservableCollection<Rep> ReferenceRep(string repName, bool isValidity)
        {
            ObservableCollection<Rep> list = new ObservableCollection<Rep>
            {
                new Rep("rep1", "林飛 顕誠", "aaa", true,true),
                new Rep("rep2", "秋間 大樹", "bbb", false,false)
            };

            return list;
        }

        public int Registration(Rep rep)
        {
            return 1;
        }

        public int Update(Rep rep)
        {
            return 1;
        }
    }
}
