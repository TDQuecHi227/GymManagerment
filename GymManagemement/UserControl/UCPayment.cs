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
using GymManagemement.ModelControls;
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
            ApplyFilters();
        }
        private void ApplyFilters()
        {
            DateTime selectedDate = dtpFillDate.Value.Date;
            string selectedMethod = cbMethod.SelectedItem?.ToString();
            string phoneKeyword = txtSearch.Text.Trim();

            int totalPayments = 0;
            decimal totalMoney = 0;

            foreach (Control ctrl in flp_payment.Controls)
            {
                if (ctrl is UCLoadpayment uCLoadpayment)
                {
                    var data = uCLoadpayment.ProductData;

                    bool matchDate = data?.Date.Date == selectedDate;

                    bool matchMethod = selectedMethod == "Tất cả" ||
                                       data?.Method.Equals(selectedMethod, StringComparison.OrdinalIgnoreCase) == true;

                    bool matchPhone = string.IsNullOrEmpty(phoneKeyword) ||
                                      (data?.Phone.Contains(phoneKeyword) == true);

                    bool isVisible = matchDate && matchMethod && matchPhone;
                    uCLoadpayment.Visible = isVisible;

                    if (isVisible)
                    {
                        totalPayments++;
                        totalMoney += data?.Total_Amount ?? 0;
                    }
                }
            }

            lb_totalmoney.Text = totalMoney.ToString("N0") + " VND";
            lb_totalpayment.Text = totalPayments.ToString();
        }

        private void dtpFillDate_ValueChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }
        private void cbMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void btn_reload_Click(object sender, EventArgs e)
        {
            loaddatapayment();
            ApplyFilters();
        }
    }
}
