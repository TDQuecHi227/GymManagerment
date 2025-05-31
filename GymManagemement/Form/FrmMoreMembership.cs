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
    public partial class FrmMoreMembership : Form
    {
        public FrmMoreMembership(List<MembershipDetailView> membership)
        {
            InitializeComponent();
            dgvDetails.DataSource = membership;
            dgvDetails.Columns["Name"].HeaderText = "Tên Gói Tập";
            dgvDetails.Columns["PriceAtTime"].HeaderText = "Giá Gói Tập";
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
