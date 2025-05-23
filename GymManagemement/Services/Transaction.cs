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
        public bool transaction_product(List<Product> products, string phone, string paymentmethod)
        {
            string err = "";
            int transactionIdValue = 0;
            List<SqlCommand> commands = new List<SqlCommand>();

            // Tính tổng tiền của toàn bộ sản phẩm
            decimal totalAmount = products.Sum(p => p.Price * p.Quantity);

            // 1. Thêm vào bảng transactions
            string insertTransaction = @"INSERT INTO transactions (phone, total_amount, transaction_date, payment_method)
                                 OUTPUT INSERTED.transaction_id
                                 VALUES (@phone, @total, @date, @method)";
            SqlCommand cmd = new SqlCommand(insertTransaction);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@phone", phone);
            cmd.Parameters.AddWithValue("@total", totalAmount);
            cmd.Parameters.AddWithValue("@date", DateTime.Now);
            cmd.Parameters.AddWithValue("@method", paymentmethod);

            int result = conn.ExecuteScalar(cmd);
            transactionIdValue = Convert.ToInt32(result);

            // 2. Lặp qua từng sản phẩm để cập nhật và thêm chi tiết
            foreach (Product product in products)
            {
                // a. Cập nhật số lượng trong bảng products
                string updateProduct = @"UPDATE products SET stock_quantity = stock_quantity - @quantity WHERE product_id = @product_id";
                SqlCommand cmdUpdate = new SqlCommand(updateProduct);
                cmdUpdate.CommandType = CommandType.Text;
                cmdUpdate.Parameters.AddWithValue("@product_id", product.Id);
                cmdUpdate.Parameters.AddWithValue("@quantity", product.Quantity);
                commands.Add(cmdUpdate);

                // b. Thêm chi tiết vào bảng transaction_products
                string insertTransactionProduct = @"INSERT INTO transaction_products (transaction_id, product_id, quantity, price_at_time)
                                            VALUES (@transaction_id, @product_id, @quantity, @price)";
                SqlCommand cmdInsert = new SqlCommand(insertTransactionProduct);
                cmdInsert.CommandType = CommandType.Text;
                cmdInsert.Parameters.AddWithValue("@transaction_id", transactionIdValue);
                cmdInsert.Parameters.AddWithValue("@product_id", product.Id);
                cmdInsert.Parameters.AddWithValue("@quantity", product.Quantity);
                cmdInsert.Parameters.AddWithValue("@price", product.Price * product.Quantity); // Giá tổng của sản phẩm tại thời điểm mua
                commands.Add(cmdInsert);
            }

            // 3. Thực hiện transaction
            return conn.ExecuteTransaction(commands, ref err);
        }

    }
    
}
