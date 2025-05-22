using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using GymManagemement.Models;
using GymManagemement.Service;
using GymManagemement.Services;
using TheArtOfDevHtmlRenderer.Core.Entities;

namespace GymManagemement
{
    public partial class FrmListCart : Form
    {
        private string paymentMethod = "";
        private int Tongtien = 0;
        private int Thua = 0;
        public FrmListCart()
        {
            InitializeComponent();
        }

        private void FrmListCart_Load(object sender, EventArgs e)
        {
            LoadData();
        }
        private void LoadData()
        {
            dgvCart.DataSource = null;
            var dataSource = CartManager.CartList.Select(c => new
            {
                ID_SP = c.ProductId,
                Tên = c.Name,
                Số_Lượng = c.Quantity,
                Đơn_giá = c.Price,
                Thành_tiền = c.Price * c.Quantity
            }).ToList();
            dgvCart.DataSource = dataSource;
            dgvCart.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            Tongtien = dataSource.Sum(item => item.Thành_tiền);
            lbTotal.Text = $"{Tongtien:N0} VND";
        }

        private void Cash_CheckedChanged(object sender, EventArgs e)
        {
            paymentMethod = "Tiền mặt";
            plCash.Visible = true;
            picBank.Visible = false;
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

        private void Bank_CheckedChanged(object sender, EventArgs e)
        {
            paymentMethod = "Chuyển khoản";
            plCash.Visible = false;
            picBank.Visible = true;
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
            List<Cart> cartList = CartManager.CartList;
            string phone = txtPhone.Text;
            Transaction transaction = new Transaction();
            bool isSuccess = false;
            foreach (var item in cartList)
            {
                Product product = new Product
                {
                    Id = item.ProductId,
                    Name = item.Name,
                    Price = item.Price,
                    Quantity = item.Quantity
                };
                isSuccess = transaction.transaction_product(product, phone, paymentMethod);
            }
            if (isSuccess)
            {
                MessageBox.Show("Thanh toán thành công!");
                this.DialogResult = DialogResult.OK;
                CartManager.ClearCart();
                this.Close();
            }
            else
            {
                MessageBox.Show("Thanh toán thất bại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnESC_Click(object sender, EventArgs e)
        {
            CartManager.ClearCart();
            this.Close();
        }
        private void dgvCart_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var hitTestInfo = dgvCart.HitTest(e.X, e.Y);
                if (hitTestInfo.RowIndex >= 0)
                {
                    dgvCart.ClearSelection();
                    dgvCart.Rows[hitTestInfo.RowIndex].Selected = true;
                    dgvCart.CurrentCell = dgvCart.Rows[hitTestInfo.RowIndex].Cells[0];
                }
            }
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            if(dgvCart.SelectedRows.Count > 0)
            {
                int productId = Convert.ToInt32(dgvCart.SelectedRows[0].Cells["ID_SP"].Value);
                CartManager.RemoveFromCart(productId);
                LoadData();
            }
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
        private void txtPhone_TextChanged(object sender, EventArgs e)
        {
            string phone = txtPhone.Text.Trim();
            string err = "";
            Load_Member load_Member = new Load_Member();
            if (phone.Length == 10)
            {
                lbName_Mem.Text = load_Member.findMem_Product(phone, ref err);
            }
        }
    }
}
