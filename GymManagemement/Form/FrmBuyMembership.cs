using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GymManagemement.Activities;
using GymManagemement.Models;
using GymManagemement.Service;
using GymManagemement.Services;

namespace GymManagemement
{
    public partial class FrmBuyMembership : Form
    {
        private int Tongtien = 0;
        private int Thua = 0;
        private string paymentMethod = "";
        private string Phone = "";
        private string err = "";
        public Loadmembership CurrentMembershipData { get; set; }
        public FrmBuyMembership(Loadmembership data, string phone)
        {
            CurrentMembershipData = data;
            Phone = phone;
            InitializeComponent();
        }

        private void FrmBuyMembership_Load(object sender, EventArgs e)
        {
            lbTenSP.Text = CurrentMembershipData.Name;
            lbGia.Text = Convert.ToInt32(CurrentMembershipData.Price).ToString("N0") + " VNĐ";
            lbTotal.Text = Convert.ToInt32(CurrentMembershipData.Price).ToString("N0") + " VNĐ";
            lbThua.Text = Thua.ToString("N0", new System.Globalization.CultureInfo("vi-VN"));
            lbPhone.Text = Phone;
            Load_Member loadMember = new Load_Member();
            lbName_Mem.Text = loadMember.findMem(Phone, ref err);

            // 3. Hiển thị UCLoadmembership trong panel
            var ucload = new UCLoadmembership();
            var loadmembership = new Load_Membership();
            CurrentMembershipData.status = "Hoạt động";
            CurrentMembershipData.Durations += " Ngày";
            int id = Convert.ToInt32(CurrentMembershipData.Id);
            CurrentMembershipData.Quantity = loadmembership.SumMember(id).ToString() + " Người đăng ký";
            ucload.Setdata(CurrentMembershipData); // Gán dữ liệu vào control

            ucload.Dock = DockStyle.Fill;
            plload.Controls.Clear();
            plload.Controls.Add(ucload);
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
            if (Convert.ToInt32(lbThua.Text.Replace(".", "")) < 0)
            {
                MessageBox.Show("Tiền trả không đủ.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(paymentMethod))
            {
                MessageBox.Show("Vui lòng chọn phương thức thanh toán.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            Transaction transaction = new Transaction();
            if (transaction.transaction_membership(CurrentMembershipData, Phone, paymentMethod))
            {
                ActivityList.activities.Insert(0, new ActivityItem
                {
                    Description = $"{lbName_Mem.Text.Trim()} đã mua gói {lbTenSP.Text.Trim()} ({paymentMethod})",
                    TimeAgo = DateTime.Now
                });
                MessageBox.Show("Thanh toán thành công");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Giao dịch thất bại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        private void txtOnlyNumber_KeyPress_Tra(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnESC_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
