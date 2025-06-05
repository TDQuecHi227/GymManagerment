// UCLoad_Schedule.cs (enhanced - hide only empty days per trainer)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using GymManagemement.Service;
using Guna.UI2.WinForms;
using GymManagemement.Services;
using System.Drawing;
using System.Data;

namespace GymManagemement
{
    public partial class UCLoad_Schedule : UserControl
    {
        public int TrainerId { get; private set; }
        private Dictionary<string, List<Guna2GradientButton>> scheduleButtons;
        private Dictionary<string, Control> dayPanels;
        private HashSet<string> daysUsed;
        public string trainerName;
        public event EventHandler<Session> RedSessionClicked;

        public UCLoad_Schedule()
        {
            InitializeComponent();
            InitializeScheduleButtonMap();

            // Gắn sự kiện click cho toàn bộ UserControl

        }

        private void InitializeScheduleButtonMap()
        {
            scheduleButtons = new Dictionary<string, List<Guna2GradientButton>>(StringComparer.OrdinalIgnoreCase)
            {
                ["Monday"] = new List<Guna2GradientButton> { btn_monday1, btn_monday2, btn_monday3 },
                ["Tuesday"] = new List<Guna2GradientButton> { btn_Tuesday1, btn_Tuesday2, btn_Tuesday3 },
                ["Wednesday"] = new List<Guna2GradientButton> { btn_Wednesday1, btn_Wednesday2, btn_Wednesday3 },
                ["Thursday"] = new List<Guna2GradientButton> { btn_Thursday1, btn_Thursday2, btn_Thursday3 },
                ["Friday"] = new List<Guna2GradientButton> { btn_Friday1, btn_Friday2, btn_Friday3 },
                ["Saturday"] = new List<Guna2GradientButton> { btn_Saturday1, btn_Saturday2, btn_Saturday3 },
            };

            dayPanels = new Dictionary<string, Control>(StringComparer.OrdinalIgnoreCase)
            {
                ["Monday"] = panelMonday,
                ["Tuesday"] = panelTuesday,
                ["Wednesday"] = panelWednesday,
                ["Thursday"] = panelThursday,
                ["Friday"] = panelFriday,
                ["Saturday"] = panelSaturday,
            };

            daysUsed = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        }

        public void SetTrainerSchedules(int trainerId, List<Schedule> schedules)
        {
            ClearPanels();
            TrainerId = trainerId;
            lb_TrainerName.Text = new Load_Trainer().findTrainerById(trainerId);
            trainerName = lb_TrainerName.Text;
            var sessions = new Load_Schedule().GetTrainerSessions(trainerId);
            foreach (var schedule in schedules)
            {
                if (!scheduleButtons.ContainsKey(schedule.dayofWeek)) continue;

                string timeText = $"{schedule.startTime:hh\\:mm} - {schedule.endTime:hh\\:mm}";
                var buttons = scheduleButtons[schedule.dayofWeek];
                foreach (var btn in buttons)
                {
                    if (string.IsNullOrWhiteSpace(btn.Text))
                    {
                        btn.Text = timeText;

                        var matchedSession = sessions.FirstOrDefault(s =>
                            s.dayofWeek.Equals(schedule.dayofWeek, StringComparison.OrdinalIgnoreCase) &&
                            s.startTime == schedule.startTime &&
                            s.endTime == schedule.endTime);

                        if (matchedSession != null)
                        {
                            btn.FillColor = Color.FromArgb(255, 200, 200);
                            btn.FillColor2 = Color.FromArgb(255, 120, 120);
                            btn.ForeColor = Color.FromArgb(64, 0, 0);
                            btn.Tag = matchedSession;
                        }
                        else
                        {
                            btn.FillColor = Color.FromArgb(192, 192, 255);
                            btn.FillColor2 = Color.FromArgb(255, 255, 192);
                            btn.ForeColor = Color.Blue;
                            btn.Tag = schedule;
                        }

                        btn.Click -= ScheduleButton_Click;
                        btn.Click += ScheduleButton_Click;

                        daysUsed.Add(schedule.dayofWeek);
                        break;
                    }
                }
            }

            foreach (var day in dayPanels.Keys)
            {
                dayPanels[day].Visible = daysUsed.Contains(day);
            }
        }
        private void ScheduleButton_Click(object sender, EventArgs e)
        {
            var btn = sender as Guna.UI2.WinForms.Guna2GradientButton;
            if (btn == null || btn.Tag == null) return;
            if (btn.FillColor == Color.FromArgb(255, 200, 200))
            {
                //session co id trainer, ten tv, sdt, thu ngay, start time, end time
                var session = btn.Tag as Session;
                if (session != null)
                {
                    // Gọi sự kiện ra ngoài
                    RedSessionClicked?.Invoke(this, session);
                }
            }
            else if (btn.FillColor == Color.FromArgb(192, 192, 255))
            {
                var schedule = btn.Tag as Schedule;
                var form = new FrmAddTrainingSession(schedule);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    var newSchedules = new Load_Schedule().GetSchedule();
                    var schedulesForThisTrainer = newSchedules.Where(s => s.trainerId == this.TrainerId).ToList();
                    SetTrainerSchedules(this.TrainerId, schedulesForThisTrainer);
                }
            }
        }
        public void ClearPanels()
        {
            daysUsed.Clear();
            foreach (var panel in dayPanels.Values)
                panel.Visible = false;

            foreach (var btnList in scheduleButtons.Values)
                foreach (var btn in btnList)
                    btn.Text = "";
        }
    }
}
