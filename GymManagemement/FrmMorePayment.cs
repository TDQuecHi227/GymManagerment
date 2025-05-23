using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GymManagemement.Models;

namespace GymManagemement
{
    public partial class FrmMorePayment : Form
    {
        public FrmMorePayment(List<ProductDetailView> chiTiet)
        {
            InitializeComponent();
            dgvDetails.DataSource = chiTiet;
            dgvDetails.Columns["Name"].HeaderText = "Tên Sản Phẩm";
            dgvDetails.Columns["Price"].HeaderText = "Đơn Giá";
            dgvDetails.Columns["PriceAtTime"].HeaderText = "Thành Tiền";
            dgvDetails.Columns["Quantity"].HeaderText = "Số Lượng";

        }

        private void FrmMorePayment_Load(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
