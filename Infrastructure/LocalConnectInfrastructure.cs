using Domain.Entities.ValueObjects;
using Domain.Repositories;
using System.Collections.ObjectModel;

namespace Infrastructure
{
    public class LocalConnectInfrastructure : IDataBaseConnect
    {
        public ObservableCollection<AccountingSubject> ReferenceAccountingSubject(string subjectCode, string subject, bool isTrueOnly)
        {
            ObservableCollection<AccountingSubject> list = new ObservableCollection<AccountingSubject>
            {
                new AccountingSubject("accounitng_subject1","000","法事冥加",true),
                new AccountingSubject("accounting_subject2","000","葬儀冥加",true),
                new AccountingSubject("accounting_subject3","222","その他",false)
            };
            return list;
        }

        public ObservableCollection<Rep> ReferenceRep(string repName, bool isValidity)
        {
            ObservableCollection<Rep> list = new ObservableCollection<Rep>
            {
                new Rep("rep1", "林飛 顕誠", "aaa", true,true),
                new Rep("rep2", "秋間 大樹", "bbb", false,false)
            };

            return list;
        }

        public int Registration(Rep rep,Rep operationRep)
        {
            return 1;
        }

        public int Registration(AccountingSubject accountingSubject,Rep operationRep)
        {
            return 1;
        }

        public int Update(Rep rep, Rep operationRep)
        {
            return 1;
        }

        public int Update(AccountingSubject accountingSubject, Rep operationRep)
        {
            return 1;
        }
    }
}
