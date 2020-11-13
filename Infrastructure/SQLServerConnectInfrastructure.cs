using DocumentFormat.OpenXml.Drawing;
using Domain.Entities.ValueObjects;
using Domain.Repositories;
using System;
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
        //private SqlCommand Cmd;
        private readonly LoginRep LoginRep=LoginRep.GetInstance();

        /// <summary>
        /// ストアドプロシージャを実行するコマンドを生成します
        /// </summary>
        /// <param name="commandText">ストアドプロシージャ名</param>
        private void ADO_NewInstance_StoredProc(SqlCommand cmd, string commandText)
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

            cmd.Connection = Cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = commandText;

            Cn.Open();
        }
   
        public int Registration(Rep rep, Rep operationRep)
        {
            SqlCommand Cmd = new SqlCommand();

            using (Cn) 
            {
                ADO_NewInstance_StoredProc(Cmd,"registration_rep");
                Cmd.Parameters.AddWithValue("@rep_name", rep.Name);
                Cmd.Parameters.AddWithValue("@password", rep.Password);
                Cmd.Parameters.AddWithValue("@validity", rep.IsValidity);
                Cmd.Parameters.AddWithValue("@is_permission", rep.IsAdminPermisson);
                return Cmd.ExecuteNonQuery();
            }
        }
       
        public int Update(Rep rep, Rep operationRep)
        {
            SqlCommand Cmd = new SqlCommand();

            using (Cn)
            {
                ADO_NewInstance_StoredProc(Cmd,"update_rep");
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
            SqlDataReader DataReader;
            SqlCommand Cmd = new SqlCommand();
            ObservableCollection<Rep> reps = new ObservableCollection<Rep>();

            using (Cn)
            {
                ADO_NewInstance_StoredProc(Cmd,"reference_rep");
                Cmd.Parameters.Add(new SqlParameter("@rep_name", repName));
                Cmd.Parameters.Add(new SqlParameter("@true_only", isValidityTrueOnly));
                DataReader = Cmd.ExecuteReader();
                
                while (DataReader.Read())
                {
                    reps.Add(new Rep((string)DataReader["rep_id"], (string)DataReader["name"], (string)DataReader["password"], (bool)DataReader["is_validity"], (bool)DataReader["is_permission"]));
                }
            }
            return reps;
        }
     
        public int Registration(AccountingSubject accountingSubject,Rep operationRep)
        {
            SqlCommand Cmd = new SqlCommand();

            using (Cn)
            {
                ADO_NewInstance_StoredProc(Cmd,"registration_accounting_subject");
                Cmd.Parameters.AddWithValue("@subject_code", accountingSubject.SubjectCode);
                Cmd.Parameters.AddWithValue("@subject", accountingSubject.Subject);
                Cmd.Parameters.AddWithValue("@validity", accountingSubject.IsValidity);
                Cmd.Parameters.AddWithValue("@rep_id", operationRep.ID);
                return Cmd.ExecuteNonQuery();
            }
        }

        public ObservableCollection<AccountingSubject> ReferenceAccountingSubject(string subjectCode, string subject, bool isTrueOnly)
        {
            SqlDataReader DataReader;
            ObservableCollection<AccountingSubject> accountingSubjects = new ObservableCollection<AccountingSubject>();
            SqlCommand Cmd = new SqlCommand();

            using (Cn)
            {
                ADO_NewInstance_StoredProc(Cmd,"reference_accounting_subject");
                Cmd.Parameters.AddWithValue("@subject_code", subjectCode);
                Cmd.Parameters.AddWithValue("@subject", subject);
                Cmd.Parameters.AddWithValue("@true_only", isTrueOnly);
                DataReader = Cmd.ExecuteReader();

                while (DataReader.Read())
                {
                    accountingSubjects.Add(new AccountingSubject((string)DataReader["accounting_subject_id"], (string)DataReader["subject_code"], (string)DataReader["subject"], (bool)DataReader["is_validity"]));
                }

                return accountingSubjects;
            }
        }
            
        public int Update(AccountingSubject accountingSubject, Rep operationRep)
        {
            SqlCommand Cmd = new SqlCommand();

            using (Cn)
            {
                ADO_NewInstance_StoredProc(Cmd,"update_accounting_subject");
                Cmd.Parameters.AddWithValue("@accounting_subject_id", accountingSubject.ID);
                Cmd.Parameters.AddWithValue("@is_validity", accountingSubject.IsValidity);
                Cmd.Parameters.AddWithValue("@operation_rep_id", operationRep.ID);
                return Cmd.ExecuteNonQuery();
            }
        }
  
        public int Registration(CreditAccount creditAccount, Rep operationRep)
        {
            SqlCommand Cmd = new SqlCommand();

            using (Cn)
            {
                ADO_NewInstance_StoredProc(Cmd,"registration_credit_account");
                Cmd.Parameters.AddWithValue("@account", creditAccount.Account);
                Cmd.Parameters.AddWithValue("@is_validity", creditAccount.IsValidity);
                Cmd.Parameters.AddWithValue("@rep_id", operationRep.ID);
                return Cmd.ExecuteNonQuery();
            }
        }
  
        public ObservableCollection<CreditAccount> ReferenceCreditAccount(string account, bool isValidityTrueOnly)
        {
            SqlDataReader DataReader;
            ObservableCollection<CreditAccount> creditAccounts = new ObservableCollection<CreditAccount>();
            SqlCommand Cmd = new SqlCommand();

            using (Cn)
            {
                ADO_NewInstance_StoredProc(Cmd,"reference_credit_account");
                Cmd.Parameters.AddWithValue("@account", account);
                Cmd.Parameters.AddWithValue("@true_only", isValidityTrueOnly);
                DataReader = Cmd.ExecuteReader();

                while (DataReader.Read())
                {
                    creditAccounts.Add(new CreditAccount((string)DataReader["credit_account_id"], (string)DataReader["account"], (bool)DataReader["is_validity"]));
                }
                return creditAccounts;
            }
        }
     
        public int Update(CreditAccount creditAccount, Rep operationRep)
        {
            SqlCommand Cmd = new SqlCommand();

            using (Cn)
            {
                ADO_NewInstance_StoredProc(Cmd,"update_credit_account");
                Cmd.Parameters.AddWithValue("@credit_account_id", creditAccount.ID);
                Cmd.Parameters.AddWithValue("@is_validity", creditAccount.IsValidity);
                Cmd.Parameters.AddWithValue("@operation_rep", operationRep);
                return Cmd.ExecuteNonQuery();
            }
        }
   
        public int Registration(Content content, Rep operationRep)
        {
            SqlCommand Cmd = new SqlCommand();

            using (Cn)
            {
                ADO_NewInstance_StoredProc(Cmd,"registration_content");
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
            SqlCommand Cmd = new SqlCommand();

            using (Cn)
            {
                ADO_NewInstance_StoredProc(Cmd,"update_content");
                Cmd.Parameters.AddWithValue("@content_id", content.ID);
                Cmd.Parameters.AddWithValue("@flat_rate", content.FlatRate);
                Cmd.Parameters.AddWithValue("@is_validity", content.IsValidity);
                Cmd.Parameters.AddWithValue("@operation_rep_id", operationRep.ID);
                return Cmd.ExecuteNonQuery();
            }
        }
    
        public ObservableCollection<Content> ReferenceContent(string contentText,string accountingSubjectCode, string accountingSubject, bool isValidityTrueOnly)
        {
            SqlDataReader DataReader;
            SqlDataAdapter sda;
            DataTable dt=new DataTable();
            ObservableCollection<Content> contents = new ObservableCollection<Content>();
            SqlCommand Cmd = new SqlCommand();

            using (Cn)
            {
                ADO_NewInstance_StoredProc(Cmd,"reference_content");
                Cmd.Parameters.AddWithValue("@content", contentText);
                Cmd.Parameters.AddWithValue("@subject_code", accountingSubjectCode);
                Cmd.Parameters.AddWithValue("@subject", accountingSubject);
                Cmd.Parameters.AddWithValue("@true_only", isValidityTrueOnly);
                //DataReader = Cmd.ExecuteReader();
                sda = new SqlDataAdapter(Cmd);

                sda.Fill(dt);

                //using(DataReader)
                //{
                //    while (DataReader.Read())
                //    {
                //        contents.Add(new Content((string)DataReader["content"], CallAccountingSubject((string)DataReader["accounting_subject_id"]), (int)DataReader["flat_rate"], (string)DataReader["text"], (bool)DataReader["is_validity"]));
                //    }
                //}
            }
            foreach(DataRow dr in dt.Rows)
            {
                contents.Add(new Content((string)dr["content_id"], CallAccountingSubject((string)dr["accounting_subject_id"]), (int)dr["flat_rate"], (string)dr["content"], (bool)dr["is_validity"]));
            }
            
            return contents;
        }

        public AccountingSubject CallAccountingSubject(string id)
        {
            SqlCommand Cmd = new SqlCommand();

            using (Cn)
            {
                using SqlDataReader DataReader = ReturnReaderCommandOneParameterStoredProc(Cmd, "call_accounting_subject", "@accounting_subject_id", id);
                while (DataReader.Read()) return new AccountingSubject((string)DataReader["accounting_subject_id"], (string)DataReader["subject_code"], (string)DataReader["subject"], (bool)DataReader["is_validity"]);
            }
            return null;
        }
        /// <summary>
        /// パラメータが一つのストアドプロシージャを実行します
        /// </summary>
        /// <param name="commandText">ストアドプロシージャ名</param>
        /// <param name="parameterName">パラメータ名</param>
        /// <param name="parameter">パラメータ</param>
        /// <returns></returns>
        private SqlDataReader ReturnReaderCommandOneParameterStoredProc(SqlCommand cmd, string commandText,string parameterName,string parameter)
        {
            ADO_NewInstance_StoredProc(cmd,commandText);
            cmd.Parameters.AddWithValue(parameterName, parameter);
            return cmd.ExecuteReader();
        }
    }
}
