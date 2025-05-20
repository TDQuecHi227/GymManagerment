using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagemement.Connection;

namespace GymManagemement.Services
{
    public class Load_payment
    {
        ConnDB conn = new ConnDB();
        public List<payment> GetPayments()
        {
            List<payment> payments = new List<payment>();
            string query = "SELECT * FROM transactions";
            var data = conn.ExecuteQueryData(query, CommandType.Text);
            foreach (DataRow item in data.Tables[0].Rows)
            {
                payments.Add(new payment
                {
                    Id = Convert.ToInt32(item["transaction_id"]),
                    Phone = item["phone"].ToString(),
                    Total_Amount = Convert.ToInt32(item["total_Amount"]),
                    Date = Convert.ToDateTime(item["transaction_date"]),
                    Method = item["payment_method"].ToString()
                });
            }
            return payments;
        }
        public int GetTotalPayments()
        {
            string query = "SELECT SUM(total_amount) FROM transactions";
            return conn.ExecuteScalar(query);
        }
    }
}
