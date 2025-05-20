using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GymManagemement.Services;

namespace GymManagemement
{
    public partial class UCPayment : UserControl
    {
        public UCPayment()
        {
            InitializeComponent();
        }
        private void loaddatapayment()
        {
            flp_payment.Controls.Clear();
            Load_payment payment = new Load_payment();
            List<payment> payments = payment.GetPayments();
            foreach (var item in payments)
            {
                var ctrl = new UCLoadpayment();
                ctrl.Setdata(item);
                flp_payment.Controls.Add(ctrl);
            }
        }
        private void UCPayment_Load(object sender, EventArgs e)
        {
            loaddatapayment();
            Load_payment payment = new Load_payment();
            lb_totalmoney.Text = payment.GetTotalPayments().ToString("N0") +" VND";
        }
    }
}
