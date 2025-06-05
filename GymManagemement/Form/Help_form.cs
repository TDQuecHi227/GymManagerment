using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GymManagemement._0._0
{
    public partial class Help_form : Form
    {
        public Help_form()
        {
            InitializeComponent();
        }

        private void Help_form_Load(object sender, EventArgs e)
        {
            help_contentLB.Text = "Để xem lịch cho các tháng và năm. Ở Mục Chọn tháng xem bấm và chọn tháng cần xem, ở mục Chọn năm xem, chọn năm cần xem. Sau đó bấm chon XEM LỊCH. Để trở về thời gian lịch hiện hành, bấm MẶC ĐỊNH";
        }
    }
}
