using System;
using System.Data.SqlClient;
using System.Runtime.Serialization;

namespace SweetDessert.Model
{
    public static class SqlService
    {
        private static readonly string sqlconnection = System.Configuration.ConfigurationManager.ConnectionStrings["SweetDessertConnectionString"].ToString();

        public static SqlConnection OpenConnection
        {
            get
            {
                SqlConnection connection = new SqlConnection(sqlconnection);
                connection.Open();
                return connection;
            }
        }
    }

    [DataContract]
    public class KeyValueItem
    {
        public KeyValueItem() { }

        [DataMember]
        public Guid Key { get; set; }

        [DataMember]
        public string Value { get; set; }
    }
}