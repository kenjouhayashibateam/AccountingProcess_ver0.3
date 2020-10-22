using Domain.Entities.ValueObjects;

namespace Domain.Repositories
{
    /// <summary>
    /// データベースに接続します。
    /// </summary>
    public interface IDataBaseConnect
    {
        public void Registration(AccountingLocation accountingLocation);
    }
}
