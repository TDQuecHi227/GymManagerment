using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace GymManagemement
{
    public partial class Addmem : Form
    {
        private string selectedGender = "";
        private string selectedMembership = "";
        private string selectedTrainingType = "";
        //private string selectedTrainer = "";
        public Loadmember NewMemberData { get; private set; }
        public Addmem()
        {
            InitializeComponent();
        }
        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txt_phone_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Chỉ cho phép số và phím điều khiển như Backspace
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Chặn ký tự không hợp lệ
            }

            // Giới hạn tối đa 10 chữ số
            if (!char.IsControl(e.KeyChar) && txt_phone.Text.Length >= 10)
            {
                e.Handled = true; // Chặn nếu đã đủ 10 ký tự
            }
        }
        private void txt_email_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Cho phép chữ cái, số, dấu chấm, gạch dưới, @, và phím điều khiển (Backspace...)
            if (!char.IsControl(e.KeyChar) &&
                !char.IsLetterOrDigit(e.KeyChar) &&
                e.KeyChar != '.' && e.KeyChar != '_' && e.KeyChar != '@')
            {
                e.Handled = true; // Chặn ký tự không hợp lệ
            }
        }
        private bool IsValidGmail(string email)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(
                email,
                @"^[a-zA-Z0-9._%+-]+@gmail\.com$"
            );
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            if (!IsValidGmail(txt_email.Text))
            {
                MessageBox.Show("Vui lòng nhập email hợp lệ có đuôi @gmail.com", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_email.Focus();
                return;
            }
            if (string.IsNullOrEmpty(selectedGender))
            {
                MessageBox.Show("Vui lòng chọn giới tính", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txt_fullname.Text) ||
                string.IsNullOrWhiteSpace(txt_email.Text) ||
                string.IsNullOrWhiteSpace(txt_phone.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            NewMemberData = new Loadmember
            {
                FullName = txt_fullname.Text.Trim(),
                Email = txt_email.Text.Trim(),
                Phone = txt_phone.Text.Trim(),
                Gender = selectedGender,
                Membership = selectedMembership,
                TrainingType = selectedTrainingType,

                DateOfBirth = dtp_DoB.Value,
                JoinDate = dtp_joindate.Value,
                Trainer = null,
            };
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btn_clearname_Click(object sender, EventArgs e)
        {
            txt_fullname.Clear();
        }

        private void Addmem_Load(object sender, EventArgs e)
        {
            var service = new Service.Load_Member();
            txt_id.Text = service.GetNextMemberId();
            txt_id.ReadOnly = true;
        }
        #region ----UI GENDER----
        private void btn_male_Click(object sender, EventArgs e)
        {
            selectedGender = "Nam";
            HighlightGenderButton(btn_male);
        }
        private void btn_female_Click(object sender, EventArgs e)
        {
            selectedGender = "Nữ";
            HighlightGenderButton(btn_female);
        }
        private void HighlightGenderButton(Guna.UI2.WinForms.Guna2Button selectedBtn)
        {
            // Reset màu tất cả nút
            btn_male.FillColor = SystemColors.Control;
            btn_female.FillColor = SystemColors.Control;

            selectedBtn.FillColor = Color.FromArgb(128, 255, 255);
        }
        #endregion

        #region ----UI MEMBERSHIP----
        private void btn_basic_Click(object sender, EventArgs e)
        {
            selectedMembership = "1";
            HighlightMembershipButton(btn_basic);
        }
        private void btn_premium_Click(object sender, EventArgs e)
        {
            selectedMembership = "2";
            HighlightMembershipButton(btn_premium);
        }
        private void btn_VIP_Click(object sender, EventArgs e)
        {
            selectedMembership = "3";
            HighlightMembershipButton(btn_VIP);
        }
        private void btn_yearpass_Click(object sender, EventArgs e)
        {
            selectedMembership = "4";
            HighlightMembershipButton(btn_yearpass);
        }
        private void HighlightMembershipButton(Guna.UI2.WinForms.Guna2Button selectedBtn)
        {
            var membershipButtons = new List<Guna2Button> {
                btn_basic,
                btn_premium,
                btn_VIP,
                btn_yearpass
            };
            foreach (var btn in membershipButtons)
            {
                btn.FillColor = SystemColors.Control;
                if (btn.Name == "btn_yearpass") btn.ForeColor = Color.Black;
            }
            foreach (var btn in membershipButtons)
            {
                if (selectedBtn.Name == "btn_basic") selectedBtn.FillColor = Color.FromArgb(200, 230, 201);
                if (selectedBtn.Name == "btn_premium") selectedBtn.FillColor = Color.FromArgb(144, 202, 249);
                if (selectedBtn.Name == "btn_VIP") selectedBtn.FillColor = Color.FromArgb(255, 179, 0);
                if (selectedBtn.Name == "btn_yearpass")
                {
                    selectedBtn.FillColor = Color.FromArgb(186, 104, 200);
                    selectedBtn.ForeColor = Color.White;
                }
            }
        }
            #endregion

        #region ----UI TRAINING TYPE----
        private void btn_none_Click(object sender, EventArgs e)
        {
            selectedTrainingType = "Solo";
            HighlightTrainingTypeButton(btn_none);
            btn_choosetrainer.FillColor = SystemColors.Control;
            btn_choosetrainer.Enabled = false;
        }

        private void btn_pt_Click(object sender, EventArgs e)
        {
            selectedTrainingType = "PT";
            HighlightTrainingTypeButton(btn_pt);
            btn_choosetrainer.FillColor = Color.RoyalBlue;
            btn_choosetrainer.Enabled = true;
        }
        private void HighlightTrainingTypeButton(Guna.UI2.WinForms.Guna2Button selectedBtn)
        {
            // Reset màu tất cả nút
            btn_none.FillColor = SystemColors.Control;
            btn_pt.FillColor = SystemColors.Control;
            selectedBtn.FillColor = Color.LightPink;
        }
        #endregion
    }
}
