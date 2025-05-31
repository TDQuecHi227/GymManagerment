using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GymManagemement.Connection;
using GymManagemement.Models;
using GymManagemement.NewMembers;
using GymManagemement.Service;

namespace GymManagemement
{
    public partial class UCMember : UserControl
    {
        public UCMember()
        {
            InitializeComponent();
            dtp_date.Value = DateTime.Now; // Đặt giá trị mặc định cho DateTimePicker là ngày hiện tại
        }
        private void LoadDataMember()
        {
            Load_Member member = new Load_Member();
            List<Loadmember> members = member.Getmember();
            flp_member.Controls.Clear();
            try { 
                flp_member.Controls.Clear();
                foreach (var item in members)
                {
                    var ctrl = new UCLoadmember();
                    ctrl.Setdata(item);
                    ctrl.MemberUpdated += () => LoadDataMember(); // Đăng ký sự kiện cập nhật thành viên
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
            Loadmembershiptype();
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            Addmem addmem = new Addmem();
            if (addmem.ShowDialog() == DialogResult.OK)
            {
                LoadDataMember();
            }
        }

        private void btn_reload_Click(object sender, EventArgs e)
        {
            LoadDataMember();
        }
        private void Loadmembershiptype()
        {
            try
            {

                ConnDB db = new ConnDB();

                // 1. Load membership types
                string query = @"
                SELECT DISTINCT mbs.name AS namembs 
                FROM members mb 
                JOIN memberships mbs ON mb.membership_id = mbs.membership_id";

                DataSet ds = db.ExecuteQueryData(query, CommandType.Text);

                if (ds.Tables.Count > 0)
                {
                    DataTable table = ds.Tables[0];

                    DataRow row = table.NewRow();
                    row["namembs"] = "None";
                    table.Rows.InsertAt(row, 0);

                    cb_mbstype.DataSource = table;
                    cb_mbstype.DisplayMember = "namembs";
                    cb_mbstype.ValueMember = "namembs";
                }

                // 2. Load training types
                string query2 = "SELECT DISTINCT training_type FROM members";
                DataSet ds2 = db.ExecuteQueryData(query2, CommandType.Text);

                DataTable dt = ds2.Tables[0];

                DataRow newRow = dt.NewRow();
                newRow["training_type"] = "None";
                dt.Rows.InsertAt(newRow, 0);

                cb_traintype.DataSource = dt;
                cb_traintype.DisplayMember = "training_type";
                cb_traintype.ValueMember = "training_type";

                // 3. Load trainer list
                string query3 = @"
                SELECT DISTINCT mb.trainer_id AS id_trainer, t.full_name AS name_trainer
                FROM members mb 
                JOIN trainers t ON mb.trainer_id = t.trainer_id";

                DataSet ds3 = db.ExecuteQueryData(query3, CommandType.Text);

                if (ds3.Tables.Count > 0)
                {
                    DataTable dt3 = ds3.Tables[0];

                    DataRow row3 = dt3.NewRow();
                    row3["id_trainer"] = DBNull.Value; // hoặc "", nếu bạn dùng chuỗi rỗng
                    row3["name_trainer"] = "None";
                    dt3.Rows.InsertAt(row3, 0);

                    cb_trainer.DataSource = dt3;
                    cb_trainer.DisplayMember = "name_trainer";
                    cb_trainer.ValueMember = "id_trainer";
                }
            }
            catch(Exception e)
            {
                MessageBox.Show("Lỗi khi load dữ liệu lên cb_mbstype: " + e.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ApplyFilters()
        {
            string selectedMbsType = cb_mbstype.Text;
            string selectedTrainType = cb_traintype.Text;
            string selectedTrainer = cb_trainer.Text;

            foreach (Control ctrl in flp_member.Controls)
            {
                if (ctrl is UCLoadmember member)
                {
                    string mbsType = member.currentMemberData?.Membership ?? "";
                    string trainType = member.currentMemberData?.TrainingType ?? "";
                    string trainer = member.currentMemberData?.Trainer ?? "";

                    bool matchMbs = (cb_mbstype.SelectedIndex == 0 || mbsType.Contains(selectedMbsType));
                    bool matchTrain = (cb_traintype.SelectedIndex == 0 || trainType.Contains(selectedTrainType));
                    bool matchTrainer = (cb_trainer.SelectedIndex == 0 || trainer.Contains(selectedTrainer));

                    member.Visible = matchMbs && matchTrain && matchTrainer;
                }
            }
        }
        private void tb_search_TextChanged(object sender, EventArgs e)
        {
            string keyword = tb_search.Text.Trim().ToLower();

            foreach (Control ctrl in flp_member.Controls)
            {
                if(ctrl is UCLoadmember searchbyName)
                {
                    string membername = searchbyName.currentMemberData?.FullName?.ToLower() ?? "";

                    searchbyName.Visible = membername.Contains(keyword);
                }
            }
        }
        private void cb_mbstype_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void cb_traintype_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void cb_trainer_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }
    }
}
