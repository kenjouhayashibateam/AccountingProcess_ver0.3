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
        /// <summary>
        /// 担当者登録
        /// </summary>
        /// <param name="rep">担当者</param>
        /// <returns>データ処理件数</returns>
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
        /// <summary>
        /// 担当者データ更新
        /// </summary>
        /// <param name="rep">担当者</param>
        /// <returns>データ処理件数</returns>
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
        /// <summary>
        /// 担当者検索
        /// </summary>
        /// <param name="repName">担当者名</param>
        /// <param name="isValidityTrueOnly">有効なデータのみ検索</param>
        /// <returns></returns>
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
        /// <summary>
        /// 勘定科目登録
        /// </summary>
        /// <param name="accountingSubject">勘定科目</param>
        /// <param name="operationRep">登録担当者</param>
        /// <returns>データ処理件数</returns>
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
    }
}
