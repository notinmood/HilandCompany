using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace 麦克斯
{
    class DBHelper
    {
        string str = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

        private SqlConnection conn;

        public SqlConnection Conn
        {
            get 
            {
                if (conn == null)
                {
                    conn = new SqlConnection(str);
                }
                return conn;
            }
        }

        public void OpenConnection()
        {
            if (Conn.State == ConnectionState.Closed)
            {
                Conn.Open();
            }
            else if(Conn.State==ConnectionState.Broken)
            {
                Conn.Close();
                Conn.Open();
            }
        }

        public void CloseConnection()
        {
            if (Conn.State == ConnectionState.Broken || Conn.State == ConnectionState.Open)
            {
                Conn.Close();
            }
        }
    }
}
