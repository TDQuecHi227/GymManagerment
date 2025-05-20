using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GymManagemement.Connection;
using GymManagemement.Models;

namespace GymManagemement.Services
{
    public class Transaction
    {
        ConnDB conn = new ConnDB();
        public bool transaction_product(Product product, string phone, string paymentmethod)
        {
            string err = "";
            int transactionIdValue = 0;
            List<SqlCommand> commands = new List<SqlCommand>();
            string insertTransacton = @"INSERT INTO transactions (phone, total_amount, transaction_date, payment_method)
                                        OUTPUT INSERTED.transaction_id
                                        VALUES (@phone, @total, @date, @method)";
            SqlCommand cmd = new SqlCommand(insertTransacton);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@phone", phone);
            cmd.Parameters.AddWithValue("@total", product.Price * product.Quantity);
            cmd.Parameters.AddWithValue("@date", DateTime.Now);
            cmd.Parameters.AddWithValue("@method", paymentmethod);
            int result = conn.ExecuteScalar(cmd);
            transactionIdValue = Convert.ToInt32(result);
            string updateProduct = "UPDATE products SET stock_quantity = stock_quantity - @quantity WHERE product_id = @product_id";
            SqlCommand cmd2 = new SqlCommand(updateProduct);
            cmd2.CommandType = CommandType.Text;
            cmd2.Parameters.AddWithValue("@product_id", product.Id);
            cmd2.Parameters.AddWithValue("@quantity", product.Quantity);
            commands.Add(cmd2);
            string insertTransaction_Product = "INSERT INTO transaction_products (transaction_id, product_id, quantity, price_at_time) VALUES (@transaction_id, @product_id, @quantity, @price)";
            SqlCommand cmd3 = new SqlCommand(insertTransaction_Product);
            cmd3.CommandType = CommandType.Text;
            cmd3.Parameters.AddWithValue("@transaction_id", transactionIdValue);
            cmd3.Parameters.AddWithValue("@product_id", product.Id);
            cmd3.Parameters.AddWithValue("@quantity", product.Quantity);
            cmd3.Parameters.AddWithValue("@price", product.Price * product.Quantity);
            commands.Add(cmd3);
            return conn.ExecuteTransaction(commands, ref err);

        }
    }
    
}
