using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using GymManagemement.Connection;

namespace GymManagemement.Service
{
    public class Load_Trainer
    {
        ConnDB conn = new ConnDB();
        private void ResetIdentity(ref string err)
        {
            string sql = @"DECLARE @maxId INT;
                         SELECT @maxID = ISNULL(MAX(CAST(trainer_id AS INT)), 0) FROM trainers;
                         DBCC CHECKIDENT('trainers', RESEED, @maxId);";
            using (var cmd = new SqlCommand(sql))
            {
                try
                {
                    conn.MyExecuteNonQuery(cmd, System.Data.CommandType.Text, ref err);
                }
                catch(SqlException ex)
                {
                    err = ex.Message;
                }
            }
        }
        public string GetNextTrainerId()
        {
            string query = "SELECT MAX(CAST(trainer_id AS INT)) FROM trainers";
            var ds = conn.ExecuteQueryData(query, System.Data.CommandType.Text);
            if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0][0] != DBNull.Value)
            {
                int maxId = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                return (maxId + 1).ToString();
            }
            else
            {
                return "1";
            }
        }
        public List<Loadtrainer> GetTrainer()
        {
            string query = "SELECT * FROM trainers";

            var ds = conn.ExecuteQueryData(query,System.Data.CommandType.Text);
            List<Loadtrainer> list = new List<Loadtrainer>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Loadtrainer trainer = new Loadtrainer
                {
                    ID = Convert.ToInt32(dr["trainer_id"]),
                    Name = dr["full_name"].ToString(),
                    Phone = dr["phone"].ToString(),
                    Email = dr["email"].ToString(),
                    Specialization = dr["specialization"].ToString(),
                    Image = dr["image"] != DBNull.Value ? (byte[])dr["image"] : null
                };
                list.Add(trainer);
            }
            return list;
        }
        public bool AddTrainer(Loadtrainer trainer, ref string err)
        {
            string sql = "INSERT INTO trainers (full_name, phone, email, specialization, image) VALUES (@full_name, @phone, @email, @specialization, @image)";
            using (var cmd = new SqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@full_name", trainer.Name);
                cmd.Parameters.AddWithValue("@phone", trainer.Phone);
                cmd.Parameters.AddWithValue("@email", trainer.Email);
                cmd.Parameters.AddWithValue("@specialization", trainer.Specialization);
                cmd.Parameters.AddWithValue("@image", trainer.Image);
                try
                {
                    conn.MyExecuteNonQuery(cmd, CommandType.Text, ref err);
                    return true;
                }
                catch (SqlException ex)
                {
                    err = ex.Message;
                    return false;
                }
            }
        }
        public bool DeleteTrainer(Loadtrainer trainerId, ref string err)
        {
            string deletesql = "DELETE FROM trainers WHERE trainer_id = @trainer_id";
            using (var cmd = new SqlCommand(deletesql))
            {
                cmd.Parameters.AddWithValue("@trainer_id", trainerId.ID);
                bool delete = conn.MyExecuteNonQuery(cmd, CommandType.Text, ref err);
                if (!delete)
                {
                    return false;
                }
            }
            ResetIdentity(ref err);

            return string.IsNullOrEmpty(err);
        }
        public bool UpdateTrainer(Loadtrainer trainer, ref string err)
        {
            string query = @"UPDATE trainers SET
                             full_name = @full_name,
                             phone = @phone,
                             email = @email,
                             specialization = @specialization,
                             image = @image
                             WHERE trainer_id = @trainer_id";

            SqlCommand cmd = new SqlCommand(@query);
            cmd.Parameters.AddWithValue("@trainer_id", trainer.ID);
            cmd.Parameters.AddWithValue("@full_name", trainer.Name);
            cmd.Parameters.AddWithValue("@phone", trainer.Phone);
            cmd.Parameters.AddWithValue("@email", trainer.Email);
            cmd.Parameters.AddWithValue("@specialization", trainer.Specialization);
            cmd.Parameters.AddWithValue("@image", trainer.Image);

            return conn.MyExecuteNonQuery(cmd, CommandType.Text, ref err);
        }
    }
}
