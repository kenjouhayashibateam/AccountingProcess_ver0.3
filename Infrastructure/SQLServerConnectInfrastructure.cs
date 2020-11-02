using Domain.Entities.ValueObjects;
using Domain.Repositories;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure
{
    /// <summary>
    /// SQLServer接続クラス
    /// </summary>
    public class SQLServerConnectInfrastructure : IDataBaseConnect
    {
        private readonly SqlConnection Cn = new SqlConnection();
        private SqlCommand Cmd;
        private readonly SqlDataReader DataReader;

        /// <summary>
        /// デストラクタ　SQLServerから切断します
        /// </summary>
        ~SQLServerConnectInfrastructure()
        {
            Cn.Close();
        }
        /// <summary>
        /// コンストラクタ　SQLServerにログインする接続文字列を設定します
        /// </summary>
        public SQLServerConnectInfrastructure()
        {
            Cn.ConnectionString = Properties.Settings.Default.AccountingProcessConnection;
        }
        /// <summary>
        /// ストアドプロシージャを実行するコマンドを生成します
        /// </summary>
        /// <param name="commandText">ストアドプロシージャ名</param>
        private void ADO_NewInstance_StoredProc(string commandText)
        {
            Cmd = new SqlCommand()
            {
                Connection = Cn,
                CommandType = CommandType.StoredProcedure,
                CommandText = commandText
            };
        }
        /// <summary>
        /// 担当者登録
        /// </summary>
        /// <param name="rep">担当者</param>
        /// <returns>データ処理件数</returns>
        public int Registration(Rep rep)
        {
            ADO_NewInstance_StoredProc("registration_rep");
            
            Cmd.Parameters.Add(new SqlParameter("@rep_name", rep.Name));
            Cmd.Parameters.Add(new SqlParameter("@rep_password", rep.Password));
            Cmd.Parameters.Add(new SqlParameter("@is_validity", rep.IsValidity));
            return Cmd.ExecuteNonQuery();
        }
        /// <summary>
        /// 担当者データ更新
        /// </summary>
        /// <param name="rep">担当者</param>
        /// <returns>データ処理件数</returns>
        public int Update(Rep rep)
        {
            return 0;
        }

        public void ReferenceRep(string repName)
        {
            throw new System.NotImplementedException();
        }
    }
}
