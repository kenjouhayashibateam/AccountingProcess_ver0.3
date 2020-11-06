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
        public int Registration(Rep rep)
        {
            using (Cn) 
            {
                ADO_NewInstance_StoredProc("registration_rep",false);
                Cmd.Parameters.AddWithValue("@rep_name", rep.Name);
                Cmd.Parameters.AddWithValue("@password", rep.Password);
                Cmd.Parameters.AddWithValue("@validity", rep.IsValidity);
                return Cmd.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// 担当者データ更新
        /// </summary>
        /// <param name="rep">担当者</param>
        /// <returns>データ処理件数</returns>
        public int Update(Rep rep)
        {
            using(Cn)
            {
                ADO_NewInstance_StoredProc("update_rep", true);
                Cmd.Parameters.AddWithValue("@rep_id", rep.RepID);
                Cmd.Parameters.AddWithValue("@password", rep.Password);
                Cmd.Parameters.AddWithValue("@is_validity", rep.IsValidity);
                Cmd.Parameters.AddWithValue("@operation_rep_id", rep.RepID);
                return Cmd.ExecuteNonQuery();
            }
        }

        public ObservableCollection<Rep> ReferenceRep(string repName,bool isValidity)
        {
            Rep rep;
            ObservableCollection<Rep> reps = new ObservableCollection<Rep>();

            using (Cn)
            {
                ADO_NewInstance_StoredProc("reference_rep",false);
                Cmd.Parameters.Add(new SqlParameter("@rep_name", repName));
                Cmd.Parameters.Add(new SqlParameter("@true_only", isValidity));
                DataReader = Cmd.ExecuteReader();
                
                while (DataReader.Read())
                {
                    rep = new Rep((string)DataReader["rep_id"], (string)DataReader["name"], (string)DataReader["password"], (bool)DataReader["is_validity"],(bool)DataReader["is_permisson"]);
                    reps.Add(rep);
                }
            }
            return reps;
        }
    }
}
