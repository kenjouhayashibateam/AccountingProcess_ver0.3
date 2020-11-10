using Domain.Entities.ValueObjects;
using Domain.Repositories;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure
{
    /// <summary>
    /// SQLServer接続クラス
    /// </summary>
    public class SQLServerConnectInfrastructure : IDataBaseConnect
    {
        private SqlConnection Cn;
        private SqlCommand Cmd;
        private SqlDataReader DataReader;

        /// <summary>
        /// ストアドプロシージャを実行するコマンドを生成します
        /// </summary>
        /// <param name="commandText">ストアドプロシージャ名</param>
        /// <param name="isSystemAdmin">saアカウントを使うかのチェック</param>
        private void ADO_NewInstance_StoredProc(string commandText,bool isSystemAdmin)
        {
            Cn = new SqlConnection();
            
            if(isSystemAdmin)
            {
                Cn.ConnectionString = Properties.Settings.Default.SystemAdminConnection;
            }
            else
            {
                Cn.ConnectionString = Properties.Settings.Default.AccountingProcessConnection;
            }
            
            Cmd = new SqlCommand()
            { 
                Connection = Cn,
                CommandType = CommandType.StoredProcedure,
                CommandText = commandText
            };
            Cn.Open();
        }
   
        public int Registration(Rep rep, Rep operationRep)
        {
            using (Cn) 
            {
                ADO_NewInstance_StoredProc("registration_rep",false);
                Cmd.Parameters.AddWithValue("@rep_name", rep.Name);
                Cmd.Parameters.AddWithValue("@password", rep.Password);
                Cmd.Parameters.AddWithValue("@validity", rep.IsValidity);
                Cmd.Parameters.AddWithValue("@is_permission", rep.IsAdminPermisson);
                return Cmd.ExecuteNonQuery();
            }
        }
       
        public int Update(Rep rep, Rep operationRep)
        {
            using(Cn)
            {
                ADO_NewInstance_StoredProc("update_rep", true);
                Cmd.Parameters.AddWithValue("@rep_id", rep.ID);
                Cmd.Parameters.AddWithValue("@password", rep.Password);
                Cmd.Parameters.AddWithValue("@is_validity", rep.IsValidity);
                Cmd.Parameters.AddWithValue("@is_permission", rep.IsAdminPermisson);
                Cmd.Parameters.AddWithValue("@operation_rep_id", operationRep.ID);
                return Cmd.ExecuteNonQuery();
            }
        }
       
        public ObservableCollection<Rep> ReferenceRep(string repName,bool isValidityTrueOnly)
        {
            Rep rep;
            ObservableCollection<Rep> reps = new ObservableCollection<Rep>();

            using (Cn)
            {
                ADO_NewInstance_StoredProc("reference_rep",false);
                Cmd.Parameters.Add(new SqlParameter("@rep_name", repName));
                Cmd.Parameters.Add(new SqlParameter("@true_only", isValidityTrueOnly));
                DataReader = Cmd.ExecuteReader();
                
                while (DataReader.Read())
                {
                    rep = new Rep((string)DataReader["rep_id"], (string)DataReader["name"], (string)DataReader["password"], (bool)DataReader["is_validity"],(bool)DataReader["is_permission"]);
                    reps.Add(rep);
                }
            }
            return reps;
        }
     
        public int Registration(AccountingSubject accountingSubject,Rep operationRep)
        {
            using(Cn)
            {
                ADO_NewInstance_StoredProc("registration_accounting_subject", false);
                Cmd.Parameters.AddWithValue("@subject_code", accountingSubject.SubjectCode);
                Cmd.Parameters.AddWithValue("@subject", accountingSubject.Subject);
                Cmd.Parameters.AddWithValue("@validity", accountingSubject.IsValidity);
                Cmd.Parameters.AddWithValue("@rep_id", operationRep.ID);
                return Cmd.ExecuteNonQuery();
            }
        }
    
        public ObservableCollection<AccountingSubject> ReferenceAccountingSubject(string subjectCode, string subject, bool isTrueOnly)
        {
            throw new System.NotImplementedException();
        }
 
        public int Update(AccountingSubject accountingSubject, Rep operationRep)
        {
            throw new System.NotImplementedException();
        }
  
        public int Registration(CreditAccount creditAccount, Rep operationRep)
        {
            throw new System.NotImplementedException();
        }
  
        public ObservableCollection<CreditAccount> ReferenceCreditAccount(string creditAccount, bool isValidityTrueOnly)
        {
            throw new System.NotImplementedException();
        }
     
        public int Update(CreditAccount creditAccount, Rep operationRep)
        {
            throw new System.NotImplementedException();
        }
   
        public int Registration(Content content, Rep operationRep)
        {
            throw new System.NotImplementedException();
        }
   
        public int Update(Content content, Rep operationRep)
        {
            throw new System.NotImplementedException();
        }
    
        public ObservableCollection<Content> ReferenceContent(string content, string accountingSubject, bool isValidityTrueOnly)
        {
            throw new System.NotImplementedException();
        }
    }
}
