using Domain.Entities.ValueObjects;
using Domain.Repositories;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using WPF.Views.Datas;

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
        private LoginRep LoginRep=LoginRep.GetInstance();

        /// <summary>
        /// ストアドプロシージャを実行するコマンドを生成します
        /// </summary>
        /// <param name="commandText">ストアドプロシージャ名</param>
        private void ADO_NewInstance_StoredProc(string commandText)
        {
            Cn = new SqlConnection();
            
            if(LoginRep.Rep.IsAdminPermisson)
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
                ADO_NewInstance_StoredProc("registration_rep");
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
                ADO_NewInstance_StoredProc("update_rep");
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
                ADO_NewInstance_StoredProc("reference_rep");
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
                ADO_NewInstance_StoredProc("registration_accounting_subject");
                Cmd.Parameters.AddWithValue("@subject_code", accountingSubject.SubjectCode);
                Cmd.Parameters.AddWithValue("@subject", accountingSubject.Subject);
                Cmd.Parameters.AddWithValue("@validity", accountingSubject.IsValidity);
                Cmd.Parameters.AddWithValue("@rep_id", operationRep.ID);
                return Cmd.ExecuteNonQuery();
            }
        }

        public ObservableCollection<AccountingSubject> ReferenceAccountingSubject(string subjectCode, string subject, bool isTrueOnly)
        {
            AccountingSubject accountingSubject;
            ObservableCollection<AccountingSubject> accountingSubjects = new ObservableCollection<AccountingSubject>();

            using (Cn)
            {
                ADO_NewInstance_StoredProc("reference_accounting_subject");
                Cmd.Parameters.AddWithValue("@subject_code", subjectCode);
                Cmd.Parameters.AddWithValue("@subject", subject);
                Cmd.Parameters.AddWithValue("@true_only", isTrueOnly);
                DataReader = Cmd.ExecuteReader();

                while (DataReader.Read())
                {
                    accountingSubject = new AccountingSubject((string)DataReader["subject_id"], (string)DataReader["subject_code"], (string)DataReader["subject"], (bool)DataReader["is_validity"]);
                    accountingSubjects.Add(accountingSubject);
                }

                return accountingSubjects;
            }
        }
            
        public int Update(AccountingSubject accountingSubject, Rep operationRep)
        {
            using(Cn)
            {
                ADO_NewInstance_StoredProc("update_accounting_subject");
                Cmd.Parameters.AddWithValue("@accounting_subject_id", accountingSubject.ID);
                Cmd.Parameters.AddWithValue("@is_validity", accountingSubject.IsValidity);
                Cmd.Parameters.AddWithValue("@operation_rep_id", operationRep.ID);
                return Cmd.ExecuteNonQuery();
            }
        }
  
        public int Registration(CreditAccount creditAccount, Rep operationRep)
        {
            using(Cn)
            {
                ADO_NewInstance_StoredProc("registration_credit_account");
                Cmd.Parameters.AddWithValue("@account", creditAccount.Account);
                Cmd.Parameters.AddWithValue("@is_validity", creditAccount.IsValidity);
                Cmd.Parameters.AddWithValue("@rep_id", operationRep.ID);
                return Cmd.ExecuteNonQuery();
            }
        }
  
        public ObservableCollection<CreditAccount> ReferenceCreditAccount(string account, bool isValidityTrueOnly)
        {
            CreditAccount creditAccount;
            ObservableCollection<CreditAccount> creditAccounts = new ObservableCollection<CreditAccount>();

            using(Cn)
            {
                ADO_NewInstance_StoredProc("reference_credit_account");
                Cmd.Parameters.AddWithValue("@account", account);
                Cmd.Parameters.AddWithValue("@true_only", isValidityTrueOnly);
                DataReader = Cmd.ExecuteReader();

                while (DataReader.Read())
                {
                    creditAccount = new CreditAccount((string)DataReader["credit_account_id"], (string)DataReader["account"], (bool)DataReader["is_validity"]);
                    creditAccounts.Add(creditAccount);
                }
                return creditAccounts;
            }
        }
     
        public int Update(CreditAccount creditAccount, Rep operationRep)
        {
            using(Cn)
            {
                ADO_NewInstance_StoredProc("update_credit_account");
                Cmd.Parameters.AddWithValue("@credit_account_id", creditAccount.ID);
                Cmd.Parameters.AddWithValue("@is_validity", creditAccount.IsValidity);
                Cmd.Parameters.AddWithValue("@operation_rep", operationRep);
                return Cmd.ExecuteNonQuery();
            }
        }
   
        public int Registration(Content content, Rep operationRep)
        {
            using(Cn)
            {
                ADO_NewInstance_StoredProc("registration_content");
                Cmd.Parameters.AddWithValue("@account_subject_id", content.AccountingSubject.ID);
                Cmd.Parameters.AddWithValue("@content", content.Text);
                Cmd.Parameters.AddWithValue("@flat_rate", content.FlatRate);
                Cmd.Parameters.AddWithValue("@is_validity", content.IsValidity);
                Cmd.Parameters.AddWithValue("@rep_id", operationRep.ID);
                return Cmd.ExecuteNonQuery();
            }
        }
   
        public int Update(Content content, Rep operationRep)
        {
            using(Cn)
            {
                ADO_NewInstance_StoredProc("update_content");
                Cmd.Parameters.AddWithValue("@content_id", content.ID);
                Cmd.Parameters.AddWithValue("@flat_rate", content.FlatRate);
                Cmd.Parameters.AddWithValue("@is_validity", content.IsValidity);
                Cmd.Parameters.AddWithValue("@operation_rep_id", operationRep.ID);
                return Cmd.ExecuteNonQuery();
            }
        }
    
        public ObservableCollection<Content> ReferenceContent(string contentText, string accountingSubject, bool isValidityTrueOnly)
        {
            Content content;
            ObservableCollection<Content> contents = new ObservableCollection<Content>();

            using(Cn)
            {
                ADO_NewInstance_StoredProc("reference_content");

            }
            return contents;
        }
    }
}
