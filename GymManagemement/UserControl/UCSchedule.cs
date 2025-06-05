// UCSchedule.cs (updated to use SetTrainerSchedules)
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using GymManagemement.Connection;
using GymManagemement.ModelControls;
using GymManagemement.Models;
using GymManagemement.Service;
using GymManagemement.Services;
using System.Diagnostics;
using GymManagemement._0._0;

namespace GymManagemement
{
    public partial class UCSchedule : UserControl
    {
        public UCSchedule()
        {
            InitializeComponent();
            ValueStart();
            this.Load += UCSchedule_Load;
        }

        private void UCSchedule_Load(object sender, EventArgs e)
        {
            LoadSchedule();
            LoadDataCombox();
            Thread.CurrentThread.CurrentCulture = new CultureInfo("vi-VN");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("vi-VN");
        }

        private void LoadSchedule()
        {
            flp_schedule.Controls.Clear();
            Load_Schedule loadSchedule = new Load_Schedule();
            List<Schedule> allSchedules = loadSchedule.GetSchedule();

            var groupedSchedules = allSchedules
                .GroupBy(s => s.trainerId)
                .ToDictionary(g => g.Key, g => g.ToList());

            foreach (var trainerSchedules in groupedSchedules)
            {
                UCLoad_Schedule ucLoadSchedule = new UCLoad_Schedule();
                ucLoadSchedule.SetTrainerSchedules(trainerSchedules.Key, trainerSchedules.Value);

                // Bắt sự kiện click từ UCLoad_Schedule
                ucLoadSchedule.RedSessionClicked += (s, session) =>
                {
                    Loadtrainer trainer = new Load_Trainer().GetDataTrainerbyID(session.trainerId);
                    lb_TrainerName.Text = trainer.Name;
                    lb_special.Text = trainer.Specialization;
                    lb_trainerPhone.Text = trainer.Phone;
                    lb_trainerEmail.Text = trainer.Email;
                    if (trainer.Image != null && trainer.Image.Length > 0)
                    {
                        using (var ms = new System.IO.MemoryStream(trainer.Image))
                        {
                            pb_trainer.Image = Image.FromStream(ms);
                        }
                    }
                    else
                    {
                        pb_trainer.Image = null; // hoặc gán ảnh mặc định: Image.FromFile("noimage.png")
                    }
                    pb_trainer.BringToFront();
                    btn_frame.UseTransparentBackground = true;
                    btn_frame.BringToFront();

                    lb_time.Text = $"{session.startTime:hh\\:mm} - {session.endTime:hh\\:mm}";
                    lb_memberName.Text = session.memberName;
                    lb_memberPhone.Text = session.phone;
                    lb_dayofweek.Text = TranslateDoW(session.dayofWeek);
                };

                flp_schedule.Controls.Add(ucLoadSchedule);
            }
        }
        private void LoadDataCombox()
        {
            // Thêm mục "None" vào đầu danh sách
            
            Load_Trainer loadTrainer = new Load_Trainer();
            List<Loadtrainer> trainers = loadTrainer.GetTrainer();
            Loadtrainer noneTrainer = new Loadtrainer { ID = 0, Name = "None" };
            trainers.Insert(0, noneTrainer);
            comboBoxTrainer.DataSource = trainers;
            comboBoxTrainer.DisplayMember = "Name";
            comboBoxTrainer.ValueMember = "ID";
            comboBoxTrainer.SelectedIndex = 0; // Chọn "None" mặc định

        }
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim().ToLower();
            foreach (UCLoad_Schedule uc in flp_schedule.Controls.OfType<UCLoad_Schedule>())
            {
                if (uc.trainerName.ToLower().Contains(keyword))
                {
                    uc.Visible = true;
                }
                else
                {
                    uc.Visible = false;
                }
            }
        }

        private string TranslateDoW(string engDoW)
        {
            switch (engDoW)
            {
                case "Monday":
                    return "Thứ hai";
                case "Tuesday":
                    return "Thứ ba";
                case "Wednesday":
                    return "Thứ tư";
                case "Thursday":
                    return "Thứ năm";
                case "Friday":
                    return "Thứ sáu";
                case "Saturday":
                    return "Thứ bảy";
                case "Sunday":
                    return "Chủ nhật";
                default:
                    return "Không rõ";
            } 
        }
        private void ValueStart()
        {
            ResetLabelsInPanel(guna2Panel7);
            pb_trainer.Image = null;
        }
        private void ResetLabelsInPanel(Control parent)
        {
            foreach (Control ctrl in parent.Controls)
            {
                if (ctrl is Guna2HtmlLabel && ctrl.Tag != "readonly")
                {
                    ctrl.Text = "";
                }

                if (ctrl.HasChildren)
                {
                    ResetLabelsInPanel(ctrl);
                }
            }
        }

        private MonthCalendar calendar;

        private void btn_schedule_Click(object sender, EventArgs e)
        {
            Form_Calender calendar = new Form_Calender();
            calendar.ShowDialog();
        }

        private void comboBoxTrainer_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedTrainerId = comboBoxTrainer.SelectedValue?.ToString();
            foreach (UCLoad_Schedule uc in flp_schedule.Controls.OfType<UCLoad_Schedule>())
            {
                if (string.IsNullOrEmpty(selectedTrainerId) || uc.TrainerId.ToString() == selectedTrainerId)
                {
                    uc.Visible = true;
                }
                else if (selectedTrainerId == "0") // Nếu chọn "None"
                {
                    uc.Visible = true;
                }
                else
                {
                    uc.Visible = false;
                }
            }
        }

        private void btn_reload_Click(object sender, EventArgs e)
        {
            LoadSchedule();
            LoadDataCombox();
            Thread.CurrentThread.CurrentCulture = new CultureInfo("vi-VN");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("vi-VN");
        }

        private void btnAddTrainer_Click(object sender, EventArgs e)
        {
            FrmAddSchedule frmAddSchedule = new FrmAddSchedule();
            if (frmAddSchedule.ShowDialog() == DialogResult.OK)
            {
                LoadSchedule();
                LoadDataCombox();
                Thread.CurrentThread.CurrentCulture = new CultureInfo("vi-VN");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("vi-VN");
            }
        }
    }
}
