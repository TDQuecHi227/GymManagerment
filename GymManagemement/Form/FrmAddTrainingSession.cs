using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GymManagemement.Service;
using GymManagemement.Services;

namespace GymManagemement
{
    public partial class FrmAddTrainingSession : Form
    {
        public Schedule Schedule { get; set; }
        public int memberId { get; set; }
        private string err = "";
        public FrmAddTrainingSession(Schedule schedule)
        {
            InitializeComponent();
            Schedule = schedule;
        }

        private void FrmAddTrainingSession_Load(object sender, EventArgs e)
        {
            lbl_Time.Text = $"{Schedule.startTime:hh\\:mm} - {Schedule.endTime:hh\\:mm}";
        }

        private void btnEsc_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPhone.Text.Trim()))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại của khách hàng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (memberId == -1)
            {
                MessageBox.Show("Thành viên này không đăng ký gói PT.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            Load_Schedule load_Schedule = new Load_Schedule();
            if (load_Schedule.AddTrainingSession(memberId, Schedule) && load_Schedule.UpdateTrainerForMember(memberId, Schedule.trainerId))
            {
                MessageBox.Show("Thêm buổi tập thành công!", "Thông báo");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void txtPhone_TextChanged(object sender, EventArgs e)
        {
            string phone = txtPhone.Text.Trim();
            string err = "";
            Load_Member load_Member = new Load_Member();
            if (phone.Length == 10)
            {
                lbName_Mem.Text = load_Member.findMem(phone, ref err);
                memberId = load_Member.findMemId(phone, ref err);
            }
            else if (phone.Length < 10)
            {
                lbName_Mem.Text = ""; // Xóa tên khách nếu không còn số điện thoại
            }
        }
    }
}
