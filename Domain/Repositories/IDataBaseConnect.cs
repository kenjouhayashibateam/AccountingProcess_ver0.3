using Domain.Entities.ValueObjects;
using System.Collections.ObjectModel;

namespace Domain.Repositories
{
    /// <summary>
    /// データベース接続
    /// </summary>
    public interface IDataBaseConnect
    {
        public int Registration(Rep rep, Rep operationRep);
        public int Registration(AccountingSubject accountingSubject, Rep operationRep);
        public int Registration(CreditAccount creditAccount, Rep operationRep);
        public int Update(Rep rep, Rep opelationRep);
        public int Update(AccountingSubject accountingSubject, Rep operationRep);
        public int Update(CreditAccount creditAccount, Rep operationRep);
        public ObservableCollection<Rep> ReferenceRep(string repName,bool isValidityTrueOnly);
        public ObservableCollection<AccountingSubject> ReferenceAccountingSubject(string subjectCode, string subject, bool isValidityTrueOnly);
        public ObservableCollection<CreditAccount> ReferenceCreditAccount(string creditAccount, bool isValidityTrueOnly);
        
    }
}
