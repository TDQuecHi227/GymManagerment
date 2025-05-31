﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using GymManagemement.Activities;
using GymManagemement.Models;
using GymManagemement.Service;
using GymManagemement.Services;

namespace GymManagemement
{
    public partial class FrmBuyProduct : Form
    {
        private int Sl = 1;
        private int stock_quantity = 0;
        private int Tongtien = 0;
        private int Thua = 0;
        private string paymentMethod = "";
        public FrmBuyProduct()
        {
            InitializeComponent();  
        }
        public void LoadData(Product product)
        {
            lbID.Text = product.Id.ToString();
            lbName_Product.Text = product.Name;
            lbPrice.Text = product.Price.ToString("N0") + " VND";
            txtQuantity.Text = Sl.ToString();
            lbTotal.Text = (Sl * product.Price).ToString("N0");
            lbThua.Text = Thua.ToString("N0");
            stock_quantity = product.Quantity;
            if (product.Image != null && product.Image.Length > 0)
            {
                using (var ms = new System.IO.MemoryStream(product.Image))
                {
                    PicProduct.Image = Image.FromStream(ms);
                }
            }
            else
            {
                PicProduct.Image = null; // hoặc gán ảnh mặc định: Image.FromFile("noimage.png")
            }
        }
        private void btnPlus_Click(object sender, EventArgs e)
        {
            if (Sl <= stock_quantity)
            {
                Sl++;
            }
            txtQuantity.Text = Sl.ToString();
            Tongtien = (Sl * Convert.ToInt32(lbPrice.Text.Replace(" VND", "").Replace(".", "")));
            lbTotal.Text = Tongtien.ToString("N0");
        }

        private void btnMinus_Click(object sender, EventArgs e)
        {
            if (Sl > 1)
            {
                Sl--;
            }
            txtQuantity.Text = Sl.ToString();
            Tongtien = (Sl * Convert.ToInt32(lbPrice.Text.Replace(" VND", "").Replace(".", "")));
            lbTotal.Text = Tongtien.ToString("N0");
        }

        private void txtTra_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTra.Text))
            {
                lbThua.Text = "0";
                return;
            }

            string rawText = txtTra.Text.Replace(".", "").Replace(",", "");

            if (!long.TryParse(rawText, out long tienKhachTra))
                return;

            // Ghi nhớ vị trí con trỏ hiện tại
            int selStart = txtTra.SelectionStart;
            int oldLength = txtTra.Text.Length;

            // Format lại chuỗi nhập theo kiểu có dấu ngăn cách hàng nghìn (vi-VN)
            txtTra.Text = tienKhachTra.ToString("N0", new System.Globalization.CultureInfo("vi-VN"));

            // Đặt lại vị trí con trỏ cho đúng
            int newLength = txtTra.Text.Length;
            txtTra.SelectionStart = selStart + (newLength - oldLength);

            // Tính tiền thừa
            Thua = (int)(tienKhachTra - Tongtien);
            lbThua.Text = Thua >= 0
                ? Thua.ToString("N0", new System.Globalization.CultureInfo("vi-VN"))
                : "0";
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }          
            if (Convert.ToInt32(lbThua.Text.Replace(".", "")) < 0)
            {
                MessageBox.Show("Tiền trả không đủ.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtQuantity.Text) || txtQuantity.Text == "0")
            {
                MessageBox.Show("Vui lòng nhập số lượng sản phẩm.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(paymentMethod))
            {
                MessageBox.Show("Vui lòng chọn phương thức thanh toán.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            Product product = new Product();
            product.Id = Convert.ToInt32(lbID.Text);
            product.Name = lbName_Product.Text;
            product.Price = Convert.ToInt32(lbPrice.Text.Replace(" VND", "").Replace(".", ""));
            product.Quantity = Convert.ToInt32(txtQuantity.Text);
            string phone = txtPhone.Text;
            List<Product> products = new List<Product>();
            products.Add(product);
            Transaction transaction = new Transaction();
            if (transaction.transaction_product(products, phone, paymentMethod))
            {
                    ActivityList.activities.Insert(0, new ActivityItem
                    {
                        Description = $"{lbName_Mem.Text.Trim()} đã mua {product.Name} ({paymentMethod})",
                        TimeAgo = DateTime.Now
                    });
                MessageBox.Show("Thanh toán thành công");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Thanh toán thất bại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Cash_CheckedChanged(object sender, EventArgs e)
        {
            paymentMethod = "Tiền mặt";
            plCash.Visible = true;
            picBank.Visible = false;
        }

        private void Bank_CheckedChanged(object sender, EventArgs e)
        {
            paymentMethod = "Chuyển khoản";
            picBank.Visible = true;
            plCash.Visible = false;
        }

        private void txtQuantity_TextChanged(object sender, EventArgs e)
        {
            if (txtQuantity.Text == "")
            {
                Sl = 1;
                txtQuantity.Text = Sl.ToString();
                Tongtien = (Sl * Convert.ToInt32(lbPrice.Text.Replace(" VND", "").Replace(".", "")));
                lbTotal.Text = Tongtien.ToString("N0");
            }
            else if (Convert.ToInt32(txtQuantity.Text) > stock_quantity)
            {
                Sl = stock_quantity;
                txtQuantity.Text = Sl.ToString();
                Tongtien = (Sl * Convert.ToInt32(lbPrice.Text.Replace(" VND", "").Replace(".", "")));
                lbTotal.Text = Tongtien.ToString("N0");
            }
            else
            {
                Sl = Convert.ToInt32(txtQuantity.Text);
                Tongtien = (Sl * Convert.ToInt32(lbPrice.Text.Replace(" VND", "").Replace(".", "")));
                lbTotal.Text = Tongtien.ToString("N0");
            }
        }

        private void btnESC_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void txtOnlyNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
            if (!char.IsControl(e.KeyChar) && txtPhone.Text.Length >= 10)
            {
                e.Handled = true;
            }
        }
        private void txtOnlyNumber_KeyPress_Tra(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }


        private void txtPhone_TextChanged(object sender, EventArgs e)
        {
            string phone = txtPhone.Text.Trim();
            string err = "";
            Load_Member load_Member = new Load_Member();
            if (phone.Length == 10)
            {
                lbName_Mem.Text = load_Member.findMem(phone, ref err);
            }
            else if (phone.Length < 10)
            {
                lbName_Mem.Text = ""; // Xóa tên khách nếu không còn số điện thoại
            }
        }
    }
}
