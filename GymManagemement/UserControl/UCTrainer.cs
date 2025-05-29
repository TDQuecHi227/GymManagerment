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
using GymManagemement.Connection;

namespace GymManagemement
{
    public partial class UCTrainer : UserControl
    {
        public UCTrainer()
        {
            InitializeComponent();
        }
        public void LoadDataTrainer()
        {
            Load_Trainer trainer = new Load_Trainer();
            List<Loadtrainer> trainers = trainer.GetTrainer();
            flp_trainer.Controls.Clear();
            try
            {
                flp_trainer.Controls.Clear();
                foreach(var item in trainers)
                {
                    var ctrl = new UCLoadtrainer();
                    ctrl.Setdata(item);
                    //ctrl.TrainerUpdated += () => LoadDataTrainer();
                    flp_trainer.Controls.Add(ctrl);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UCTrainer_Load(object sender, EventArgs e)
        {
            LoadDataTrainer();
            LoadSpecialization();
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            Addtrainer addtrainer = new Addtrainer();

            var result = addtrainer.ShowDialog();

            if (result == DialogResult.OK && addtrainer.NewTrainerData != null)
            {
                var newTrainer = addtrainer.NewTrainerData;
                var service = new Load_Trainer();
                string err = string.Empty;
                service.AddTrainer(newTrainer, ref err);

                if (string.IsNullOrEmpty(err))
                {
                    MessageBox.Show("Successfully!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDataTrainer();
                }
                else
                {
                    MessageBox.Show("Error: " + err, "Notification", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } 
        }
        private void cb_membership_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }
        private void tb_search_TextChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }
        private void ApplyFilters()
        {
            string selectedSpecial = cb_special.Text;
            string keyword = tb_search.Text.Trim().ToLower();

            foreach (Control ctrl in flp_trainer.Controls)
            {
                if (ctrl is UCLoadtrainer trainer)
                {
                    string special = trainer.currentTrainerData?.Specialization ?? "";
                    string name = trainer.currentTrainerData?.Name?.ToLower() ?? "";

                    bool matchSpecial = (cb_special.SelectedIndex == 0 || special.Contains(selectedSpecial));
                    bool matchName = string.IsNullOrEmpty(keyword) || name.Contains(keyword);

                    trainer.Visible = matchSpecial && matchName;
                }
            }
        }
        private void LoadSpecialization()
        {
            try
            {
                ConnDB db = new ConnDB();

                string query = @"SELECT DISTINCT specialization FROM trainers";

                DataSet ds = db.ExecuteQueryData(query, CommandType.Text);

                 if (ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];

                    DataRow row = dt.NewRow();
                    row["specialization"] = "None";
                    dt.Rows.InsertAt(row, 0);

                    cb_special.DataSource = dt;
                    cb_special.DisplayMember = "specialization";
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error load data: " + ex.Message, "Noti", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_reload_Click(object sender, EventArgs e)
        {
            LoadDataTrainer();
        }
    }
}
