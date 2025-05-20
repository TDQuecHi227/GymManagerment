using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagemement.Connection;

namespace GymManagemement.Services
{
    public class Load_Member_Home
    {
        ConnDB conn = new ConnDB();
        public List<MemActive> GetMemActive()
        {
            string sql = "SELECT * FROM members " +
                "JOIN memberships ON members.membership_id = memberships.membership_id " +
                "WHERE DATEADD(DAY, memberships.duration_days, members.join_date) > GETDATE()";
            var ds = conn.ExecuteQueryData(sql, CommandType.Text);
            List<MemActive> list = new List<MemActive>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                MemActive mem = new MemActive
                {
                    Name = row["full_name"].ToString(),
                    Phone = row["phone"].ToString(),
                    MemShip = row["name"].ToString()
                };
                list.Add(mem);
            }
            return list;
        }
        public List<MemExpired> GetMemExpired()
        {
            string sql = "SELECT * FROM members " +
                "JOIN memberships ON members.membership_id = memberships.membership_id " +
                "WHERE DATEADD(DAY, memberships.duration_days, members.join_date) < GETDATE()";
            var ds = conn.ExecuteQueryData(sql, CommandType.Text);
            List<MemExpired> list = new List<MemExpired>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                MemExpired mem = new MemExpired
                {
                    Name = row["full_name"].ToString(),
                    Phone = row["phone"].ToString(),
                    MemShip = row["name"].ToString()
                };
                list.Add(mem);
            }
            return list;
        }
    }
}
