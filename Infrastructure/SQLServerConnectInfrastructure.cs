using Domain.Entities.ValueObjects;
using Domain.Repositories;
using System;
using System.Data;
using System.Data.SqlClient;


namespace Infrastructure
{
    public class SQLServerConnectInfrastructure : IDataBaseConnect
    {

        private readonly SqlConnection Cn = new SqlConnection();
        private SqlCommand Cmd;
        private SqlDataReader DataReader;

        public SQLServerConnectInfrastructure()
        {
            Cn.ConnectionString = Properties.Settings.Default.AccountingProcessConnection;
        }

        public void Registration(Rep rep)
        {
            ADO_NewInstance_StoredProc("registration_rep");
            
            Cmd.Parameters.Add(new SqlParameter("@rep_name", rep.Name));
            Cmd.Parameters.Add(new SqlParameter("@rep_password", rep.Password));
            Cmd.ExecuteNonQuery();
        }

        private void ADO_NewInstance_StoredProc(string commandText)
        {
            Cmd = new SqlCommand()
            {
                Connection = Cn,
                CommandType = CommandType.StoredProcedure
            };
        }




        //public string Registration(AccountingLocation accountingLocation)
        //{
        //    Cn.Open();
        //    Cmd.CommandText = "reference_location";
        //    Cmd.Parameters.Add(new SqlParameter("@location_name","理"));

        //    SqlDataReader dataReader=Cmd.ExecuteReader();

        //    while(dataReader.Read())
        //    {
        //        return dataReader["LocationName"].ToString();
        //    }
        //    return string.Empty;
        //}
    }
}
