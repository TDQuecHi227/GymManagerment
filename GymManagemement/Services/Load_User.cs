using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagemement.Connection;
using GymManagemement.Models;

namespace GymManagemement.Services
{
    public class Load_User
    {
        ConnDB connDB = new ConnDB();
        public User GetUser()
        {
            string query = "SELECT * FROM users";
            var ds = connDB.ExecuteQueryData(query, System.Data.CommandType.Text);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                var row = ds.Tables[0].Rows[0];
                return new User
                {
                    UserName = row["userName"].ToString(),
                    Password = row["password_hash"].ToString()
                };
            }
            return null; // or throw an exception if no user found
        }
    }
}
