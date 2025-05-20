using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagemement.Connection
{
    class ConnDB
    {
        string ConnString = "Data Source=(local);Initial Catalog=Gym;User ID=sa;Password=227985";
        SqlConnection conn = null;
        SqlCommand comm = null;
        SqlDataAdapter da = null;

        public ConnDB()
        {
            conn = new SqlConnection(ConnString);
            comm = conn.CreateCommand();
        }
        public DataSet ExecuteQueryData(string sql, CommandType ct)
        {
            if (conn.State == ConnectionState.Open) conn.Close();
            conn.Open();
            comm.CommandText = sql;
            comm.CommandType = ct;
            da = new SqlDataAdapter(comm);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }
        public bool MyExecuteNonQuery(SqlCommand cmd, CommandType ct, ref string error)
        {
            bool f = false;
            try
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();

                conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = ct;
                cmd.ExecuteNonQuery();
                f = true;
            }
            catch (SqlException ex)
            {
                error = ex.Message;
            }
            finally
            {
                conn.Close();
            }
            return f;
        }
        public int ExecuteScalar(string sql)
        {
            if (conn.State == ConnectionState.Open) conn.Close();
            conn.Open();
            comm.CommandText = sql;
            int result = Convert.ToInt32(comm.ExecuteScalar());
            conn.Close();
            return result;
        }
        public int ExecuteScalar(SqlCommand cmd)
        {
            if (conn.State == ConnectionState.Open) conn.Close();
            conn.Open();
            cmd.Connection = conn;
            int result = Convert.ToInt32(cmd.ExecuteScalar());
            conn.Close();
            return result;
        }
        public bool ExecuteTransaction(List<SqlCommand> commands, ref string error)
        {
            bool isSuccess = false;
            if (conn.State == ConnectionState.Open)
                conn.Close();

            conn.Open();
            SqlTransaction transaction = conn.BeginTransaction();

            try
            {
                foreach (SqlCommand cmd in commands)
                {
                    cmd.Connection = conn;
                    cmd.Transaction = transaction;
                    cmd.ExecuteNonQuery();
                }

                transaction.Commit();
                isSuccess = true;
            }
            catch (SqlException ex)
            {
                transaction.Rollback();
                error = ex.Message;
            }
            finally
            {
                conn.Close();
            }

            return isSuccess;
        }

    }
}
