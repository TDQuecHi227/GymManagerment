using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GymManagemement.Service;

namespace GymManagemement
{
    public partial class UCLoadmember : UserControl
    {
        public UCLoadmember()
        {
            InitializeComponent();
        }
        private void LoadDataMember()
        {
            Load_Member member = new Load_Member();
            List<Loadmember> members = member.Getmember();
            //flp_member.Controls.Clear();
            try
            {
                foreach (var item in members)
                {
                    var ctrl = new UCLoadmember();
                    ctrl.Setdata(item);
                    //flp_member.Controls.Add(ctrl);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Setdata(Loadmember data)
        {
            lb_ID.Text = data.Id;
            lb_FULLNAME.Text = data.FullName;
            lb_PHONE.Text = data.Phone;
            lb_EMAIL.Text = data.Email;
            lb_GENDER.Text = data.Gender;
            lb_DOB.Text = data.DateOfBirth.ToString("dd/MM/yyyy");
            lb_JOINDATE.Text = data.JoinDate.ToString("dd/MM/yyyy");
            lb_MEMBERSHIP.Text = data.Membership;
            lb_TRAININGTYPE.Text = data.TrainingType;
            lb_TRAINER.Text = data.Trainer;
        }

        private void UCLoadmember_MouseLeave(object sender, EventArgs e)
        {
            if (!this.ClientRectangle.Contains(this.PointToClient(Cursor.Position)))
            {
                btn_delete.Visible = false;
                btn_edit.Visible = false;
            }
        }

        private void UCLoadmember_MouseEnter(object sender, EventArgs e)
        {
            btn_delete.Visible = true;
            btn_edit.Visible = true;
        }
        private void btn_delete_Click(object sender, EventArgs e)
        {
            deletemem();
        }
        private void deletemem()
        {
            string id = lb_ID.Text;
            var result = MessageBox.Show("Bạn có chắc chắn muốn xóa thành viên này không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Load_Member member = new Load_Member();
                string err = string.Empty;

                // Create a Loadmember object with the ID to delete
                Loadmember memberToDelete = new Loadmember { Id = id };

                if (member.DeleteMember(memberToDelete, ref err))
                {
                    MessageBox.Show("Xóa thành viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Parent.Controls.Remove(this);
                }
                else
                {
                    MessageBox.Show("Lỗi: " + err, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
