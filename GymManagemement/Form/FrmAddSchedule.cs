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
using GymManagemement.Service;
using GymManagemement.Services;

namespace GymManagemement
{
    public partial class FrmAddSchedule : Form
    {
        private readonly Load_Schedule scheduleService = new Load_Schedule();

        public FrmAddSchedule()
        {
            InitializeComponent();
            // Load dữ liệu cho các combobox nếu cần
            LoadTrainerNames();
            LoadDayOfWeek();
        }

        private void LoadTrainerNames()
        {
            Load_Trainer loadTrainer = new Load_Trainer();
            List<Loadtrainer> trainers = loadTrainer.GetTrainer();
            comboBoxTrainer.DataSource = trainers;
            comboBoxTrainer.DisplayMember = "Name";
            comboBoxTrainer.ValueMember = "ID";
            comboBoxTrainer.SelectedIndex = -1; // Đặt giá trị mặc định là không chọn
        }
        private void LoadDayOfWeek()
        {
            string[] thuTiengViet = new string[]
            {
        "Thứ hai", "Thứ ba", "Thứ tư", "Thứ năm", "Thứ sáu", "Thứ bảy"
            };

            comboBoxDayOfWeek.Items.Clear();
            comboBoxDayOfWeek.Items.AddRange(thuTiengViet);

            comboBoxDayOfWeek1.Items.Clear();
            comboBoxDayOfWeek1.Items.AddRange(thuTiengViet);

            comboBoxDayOfWeek2.Items.Clear();
            comboBoxDayOfWeek2.Items.AddRange(thuTiengViet);
        }
        private string ConvertToEnglish(string thuTiengViet)
        {
            switch (thuTiengViet)
            {
                case "Thứ hai":
                    return "Monday";
                case "Thứ ba":
                    return "Tuesday";
                case "Thứ tư":
                    return "Wednesday";
                case "Thứ năm":
                    return "Thursday";
                case "Thứ sáu":
                    return "Friday";
                case "Thứ bảy":
                    return "Saturday";
                default:
                    return null;
            }
        }



        private void btnSave_Click(object sender, EventArgs e)
        {
            // Kiểm tra huấn luyện viên và thời gian
            if (comboBoxTrainer.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn huấn luyện viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtStartTime.Text) || string.IsNullOrWhiteSpace(txtEndTime.Text))
            {
                MessageBox.Show("Vui lòng nhập thời gian bắt đầu và kết thúc!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!TimeSpan.TryParse(txtStartTime.Text, out TimeSpan startTime) ||
                !TimeSpan.TryParse(txtEndTime.Text, out TimeSpan endTime))
            {
                MessageBox.Show("Vui lòng nhập đúng định dạng thời gian!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (startTime >= endTime)
            {
                MessageBox.Show("Thời gian bắt đầu phải trước thời gian kết thúc!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Danh sách các combobox cần kiểm tra
            var dayCombos = new List<ComboBox> { comboBoxDayOfWeek, comboBoxDayOfWeek1, comboBoxDayOfWeek2 };
            var trainerId = ((Loadtrainer)comboBoxTrainer.SelectedItem).ID;

            int savedCount = 0;

            foreach (var combo in dayCombos)
            {
                if (combo.SelectedItem != null)
                {
                    var schedule = new Schedule
                    {
                        trainerId = trainerId,
                        dayofWeek = ConvertToEnglish(combo.SelectedItem.ToString()),
                        startTime = startTime,
                        endTime = endTime
                    };

                    if (scheduleService.AddSchedule(schedule))
                    {
                        savedCount++;
                    }
                }
            }

            if (savedCount > 0)
            {
                MessageBox.Show($"Thêm thành công!", "Thông báo");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Không có ngày nào được chọn hoặc lưu không thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
