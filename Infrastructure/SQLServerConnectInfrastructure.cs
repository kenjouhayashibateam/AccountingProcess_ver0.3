using Domain.Entities.ValueObjects;
using Domain.Repositories;
using System;
using System.Data.SqlClient;


namespace Infrastructure
{
    public class SQLServerConnectInfrastructure:IDataBaseConnect
    {

        private readonly SqlConnection Cn=new SqlConnection();
        private SqlCommand Cmd;
        
        public string Registration(AccountingLocation accountingLocation)
        {
            Cn.ConnectionString = Properties.Settings.Default.AccountingProcessConnection;
            Cn.Open();
            Cmd = new SqlCommand();
            Cmd.Connection = Cn;
            Cmd.CommandType = System.Data.CommandType.StoredProcedure;
            Cmd.CommandText = "reference_location";
            Cmd.Parameters.Add(new SqlParameter("@location_name","理"));

            SqlDataReader dataReader=Cmd.ExecuteReader();

            while(dataReader.Read())
            {
                return dataReader["LocationName"].ToString();
            }
            return string.Empty;
        }
    }
}
