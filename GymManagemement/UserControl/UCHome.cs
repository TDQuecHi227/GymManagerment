using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using GymManagemement.Activities;
using GymManagemement.Models;
using GymManagemement.NewMembers;
using GymManagemement.Services;

namespace GymManagemement
{
    public partial class UCHome : UserControl
    {
        public event EventHandler MoreClicked;
        public UCHome()
        {
            InitializeComponent();
        }
        private void GunaButton_Click(object sender, EventArgs e)
        {
            Guna.UI2.WinForms.Guna2Button button = sender as Guna.UI2.WinForms.Guna2Button;
            if (button != null)
            {
                string buttonName = button.Name;
                switch(buttonName)
                {
                    case "btnConHan":
                        btnConHan.FillColor = Color.DodgerBlue;
                        btnConHan.ForeColor = Color.White;
                        btnHetHan.FillColor = Color.Gainsboro;
                        btnHetHan.ForeColor = Color.Gray;
                        LoadDataMemActive();
                        break;
                    case "btnHetHan":
                        btnHetHan.FillColor = Color.DodgerBlue;
                        btnHetHan.ForeColor = Color.White;
                        btnConHan.FillColor = Color.Gainsboro;
                        btnConHan.ForeColor = Color.Gray;
                        LoadDataMemExpired();
                        break;                   
                    default:
                        break;
                }
            }
        }
        private void LoadDataActivity()
        {
            flpActivities.Controls.Clear();
            foreach (var item in ActivityList.activities)
            {
                var ctrl = new ActivityItemControl();
                ctrl.SetData(item);
                flpActivities.Controls.Add(ctrl);
            }
        }
        private void LoadDataNewMember()
        {
            List<NewMember> members = new List<NewMember>
            {
                new NewMember { Name = "Nguyễn Văn D", RegisteredAt = "10 phút trước" },
                new NewMember { Name = "Phạm Thị E", RegisteredAt = "3 giờ trước" },
                new NewMember { Name = "Lê Văn F", RegisteredAt = "5 giờ trước" }
            };
            foreach (var mem in members)
            {
                var memCtrl = new NewMemControl();
                memCtrl.SetData(mem);
                flpNewMembers.Controls.Add(memCtrl);
            }

        }
        private void LoadDataMemActive()
        {
            Load_Member_Home memberRepository = new Load_Member_Home();
            List<MemActive> members = memberRepository.GetMemActive();
            flpMem.Controls.Clear();
            
            foreach (var item in members)
            {
                var ctrl = new UCMemActive();
                ctrl.SetData(item);
                flpMem.Controls.Add(ctrl);
            }
        }
        private void LoadDataMemExpired()
        {
            Load_Member_Home memberRepository = new Load_Member_Home();
            List<MemExpired> members = memberRepository.GetMemExpired();
            flpMem.Controls.Clear();

            foreach (var item in members)
            {
                var ctrl = new UCMemExpired();
                ctrl.SetData(item);
                flpMem.Controls.Add(ctrl);
            }
        }
        private void LoadDataSumAndRatio()
        {
            Load_SumAndRatio_Home sumAndRatioRepository = new Load_SumAndRatio_Home();
            lbTotalMem.Text = sumAndRatioRepository.GetTotalMembers().ToString("N0");
            lbTongTV.Text = sumAndRatioRepository.GetTotalMembers().ToString();
            lbTotalTrainer.Text = sumAndRatioRepository.GetTotalTrainers().ToString();
            lbTotalRevenue.Text = sumAndRatioRepository.GetTotalRevenue().ToString("N0") + " VND";
            string ratiomember = sumAndRatioRepository.GetRatioMembers();
            //string ratiotrainer = sumAndRatioRepository.GetRatioTrainers();
            //string ratiorevenue = sumAndRatioRepository.GetRatioRevenue();
            lbRatioMem.Text = ratiomember;
            if(ratiomember.Contains("↑"))
            {
                lbRatioMem.ForeColor = Color.Green;
            }
            else if (ratiomember.Contains("↓"))
            {
                lbRatioMem.ForeColor = Color.Red;
            }
            else
            {
                lbRatioMem.ForeColor = Color.Gray;
            }
            //lbRatioTrainer.Text = ratiotrainer;
            //if (ratiotrainer.Contains("↑"))
            //{
            //    lbRatioTrainer.ForeColor = Color.Green;
            //}
            //else if (ratiotrainer.Contains("↓"))
            //{
            //    lbRatioTrainer.ForeColor = Color.Red;
            //}
            //else
            //{
            //    lbRatioTrainer.ForeColor = Color.Gray;
            //}
            //lbRatioRevenue.Text = ratiorevenue;
            //if (ratiorevenue.Contains("↑"))
            //{
            //    lbRatioRevenue.ForeColor = Color.Green;
            //}
            //else if (ratiorevenue.Contains("↓"))
            //{
            //    lbRatioRevenue.ForeColor = Color.Red;
            //}
            //else
            //{
            //    lbRatioRevenue.ForeColor = Color.Gray;
            //}
        }
        private void UpdateChart_Monthly()
        {
            Doanhthu.Series[0].Points.Clear();

            string[] months = { "Tháng 1", "Tháng 2", "Tháng 3", "Tháng 4", "Tháng 5", "Tháng 6",
                        "Tháng 7", "Tháng 8", "Tháng 9", "Tháng 10", "Tháng 11", "Tháng 12" };
            int[] values = { 5, 10, 3, 8, 2, 6, 4, 9, 7, 11, 5, 6 };

            for (int i = 0; i < months.Length; i++)
            {
                Doanhthu.Series[0].Points.AddXY(months[i], values[i]);
            }
        }
        private void UpdateChart_Yearly()
        {
            Doanhthu.Series[0].Points.Clear(); // Xóa dữ liệu cũ
            string[] years = { "2020", "2021", "2022", "2023" };
            int[] values = { 100, 200, 150, 300 };
            for (int i = 0; i < years.Length; i++)
            {
                Doanhthu.Series[0].Points.AddXY(years[i], values[i]);
            }
        }
        private void UpdateChart_Dayly()
        {
            Doanhthu.Series[0].Points.Clear(); // Xóa dữ liệu cũ
            string[] days = { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };
            int[] values = { 20, 30, 25, 40, 35, 50, 45 };
            for (int i = 0; i < days.Length; i++)
            {
                Doanhthu.Series[0].Points.AddXY(days[i], values[i]);
            }
        }      
        
        private void UCHome_Load(object sender, EventArgs e)
        {
            LoadDataActivity();
            LoadDataNewMember();
            LoadDataMemActive();
            LoadDataSumAndRatio();
        }

        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (guna2ComboBox1.SelectedItem.ToString() == "Tháng")
            {
                UpdateChart_Monthly();
            }
            else if (guna2ComboBox1.SelectedItem.ToString() == "Năm")
            {
                UpdateChart_Yearly();
            }
            else if (guna2ComboBox1.SelectedItem.ToString() == "Ngày")
            {
                UpdateChart_Dayly();
            }
        }

        private void llbMore_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MoreClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
