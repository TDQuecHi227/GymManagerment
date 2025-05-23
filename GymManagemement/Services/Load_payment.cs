using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagemement.Connection;
using GymManagemement.Models;

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
        public int GetTotalPaymentsCount()
        {
            string query = "SELECT count(*) FROM transactions";
            return conn.ExecuteScalar(query);
        }
        public List<ProductDetailView> GetTransactionDetails(int transactionId)
        {
            List<ProductDetailView> result = new List<ProductDetailView>();

            string query = $@"SELECT p.name, tp.quantity, tp.price_at_time, p.price
                             FROM transaction_products tp
                             JOIN products p ON tp.product_id = p.product_id
                             WHERE tp.transaction_id = {transactionId}";
            var data = conn.ExecuteQueryData(query, CommandType.Text);
            foreach (DataRow item in data.Tables[0].Rows)
            {
                result.Add(new ProductDetailView
                {
                    Name = item["name"].ToString(),
                    Quantity = Convert.ToInt32(item["quantity"]),
                    Price = Convert.ToInt32(item["price"]),
                    PriceAtTime = Convert.ToInt32(item["price_at_time"])
                });
            }

            return result;
        }

    }
}
