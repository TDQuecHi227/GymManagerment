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
using GymManagemement.Services;

namespace GymManagemement
{
    public partial class UCLoadpayment : UserControl
    {
        public payment ProductData { get; private set; }
        public UCLoadpayment()
        {
            InitializeComponent();
        }
        public void Setdata(payment data)
        {
            ProductData = data;
            lb_ID.Text = data.Id.ToString();
            lb_phone.Text = data.Phone;
            lb_amount.Text = data.Total_Amount.ToString("N0");
            lb_date.Text = data.Date.ToString("yyyy-MM-dd");
            lb_status.Text = data.Method;
        }

        private void llbMore_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (ProductData != null)
            {
                List<ProductDetailView> chiTiet = new Load_payment().GetTransactionDetails(ProductData.Id);
                FrmMorePayment form = new FrmMorePayment(chiTiet);
                form.ShowDialog();
            }
        }
    }
}
