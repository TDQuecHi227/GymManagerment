using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GymManagemement.Service;

namespace GymManagemement
{
    public partial class UCMember : UserControl
    {
        public UCMember()
        {
            InitializeComponent();
        }
        private void LoadDataMember()
        {
            Load_Member member = new Load_Member();
            List<Loadmember> members = member.Getmember();
            flp_member.Controls.Clear();
            try { 
                foreach (var item in members)
                {
                    var ctrl = new UCLoadmember();
                    ctrl.Setdata(item);
                    flp_member.Controls.Add(ctrl);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UCMember_Load(object sender, EventArgs e)
        {
            LoadDataMember();
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            Addmem addmem = new Addmem();

            var result = addmem.ShowDialog(); // Hiện form Addmem  

            if (result == DialogResult.OK && addmem.NewMemberData != null)
            {
                var newMember = addmem.NewMemberData;

                // Thêm vào cơ sở dữ liệu hoặc danh sách (nếu bạn đã có)  
                var service = new Load_Member();
                string err = string.Empty; // Declare and initialize the 'err' variable  
                service.AddMember(newMember, ref err); // đảm bảo bạn có hàm AddMember()  

                if (!string.IsNullOrEmpty(err))
                {
                    MessageBox.Show("Error: " + err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                // Load lại danh sách để hiển thị  
                LoadDataMember();
            }
        }

        private void btn_reload_Click(object sender, EventArgs e)
        {
            LoadDataMember();
        }
    }
}
