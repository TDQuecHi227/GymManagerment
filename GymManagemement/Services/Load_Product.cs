using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using GymManagemement.Connection;
using GymManagemement.Models;

namespace GymManagemement.Services
{
    public class Load_Product
    {
       ConnDB conn = new ConnDB();
        public List<Product> GetProduct()
        {
            string query = "SELECT product_id, name, price, stock_quantity, description, image FROM products  ";          
            var ds = conn.ExecuteQueryData(query, CommandType.Text);
            List<Product> products = new List<Product>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                Product product = new Product
                {
                    Id = Convert.ToInt32(row["product_id"]),
                    Name = row["name"].ToString(),
                    Description = row["description"].ToString(),
                    Price = Convert.ToInt32(row["price"]),
                    Quantity = Convert.ToInt32(row["stock_quantity"]),
                    Image = (byte[])row["image"]
                };
                products.Add(product);
            }
            return products;
        }
        public bool InsertProduct(Product product)
        {
            string query = "INSERT INTO products (name, price, stock_quantity, description, image) VALUES (@name, @price, @quantity, @description, @image)";
            SqlCommand cmd = new SqlCommand(query);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@name", product.Name);
            cmd.Parameters.AddWithValue("@price", product.Price);
            cmd.Parameters.AddWithValue("@quantity", product.Quantity);
            cmd.Parameters.AddWithValue("@description", product.Description ?? (object)DBNull.Value);
            if (product.Image != null)
                cmd.Parameters.Add("@image", SqlDbType.VarBinary).Value = product.Image;
            else
                cmd.Parameters.Add("@image", SqlDbType.VarBinary).Value = DBNull.Value;
            string err = "";
            return conn.MyExecuteNonQuery(cmd, CommandType.Text, ref err);
        }
        public bool DeleteProduct(string id)
        {
            string query = "DELETE FROM products WHERE product_id = @id";
            SqlCommand cmd = new SqlCommand(query);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@id", id);
            string err = "";
            return conn.MyExecuteNonQuery(cmd, CommandType.Text, ref err);
        }
        
        public bool UpdateProduct(Product product)
        {
            string query = "UPDATE products SET name = @name, price = @price, stock_quantity = @quantity, description = @description, image = @image " +
                "WHERE product_id = @id";
            SqlCommand cmd = new SqlCommand(query);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@name", product.Name);
            cmd.Parameters.AddWithValue("@price", product.Price);
            cmd.Parameters.AddWithValue("@quantity", product.Quantity);
            cmd.Parameters.AddWithValue("@description", product.Description ?? (object)DBNull.Value);
            if (product.Image != null)
                cmd.Parameters.Add("@image", SqlDbType.VarBinary).Value = product.Image;
            else
                cmd.Parameters.Add("@image", SqlDbType.VarBinary).Value = DBNull.Value;
            cmd.Parameters.AddWithValue("@id", product.Id);
            string err = "";
            return conn.MyExecuteNonQuery(cmd, CommandType.Text, ref err);
        }
    }
}
