using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace GymManagemement._0._0
{
    public partial class Form_Calender : Form
    {
        public Form_Calender()
        {
            InitializeComponent();
        }

        ////registry start with windows
        //public static void AddApplicationToStartup()
        //{
        //    try
        //    {
        //        using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
        //        {
        //            key.SetValue("ViCalendar_v1._0._0", "\"" + Application.ExecutablePath + "\"");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Không thể thêm vào khởi động cùng Windows: " + ex.Message);
        //    }
        //}

        lunarCalendar vcal = new lunarCalendar();
        lunarCalendar namam = new lunarCalendar();

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                // Button Tooltip
                ToolTip buttonToolTip = new ToolTip();
                buttonToolTip.ToolTipTitle = "Thông tin";
                buttonToolTip.UseFading = true;
                buttonToolTip.UseAnimation = true;
                buttonToolTip.IsBalloon = true;
                buttonToolTip.ShowAlways = true;
                buttonToolTip.AutoPopDelay = 5000;
                buttonToolTip.InitialDelay = 1000;
                buttonToolTip.ReshowDelay = 500;
                buttonToolTip.SetToolTip(viewButton, "Nhấn để xem lịch !");

                // Lấy ngày hiện tại
                DateTime now = DateTime.Now;
                int currentDay = now.Day;
                int currentMonth = now.Month;
                int currentYear = now.Year;
                int current_DaysOfMonth = DateTime.DaysInMonth(currentYear, currentMonth);

                // Hiển thị tháng năm
                currentMonthLB.Text = "THÁNG " + currentMonth + " NĂM " + currentYear;

                // Hiển thị năm âm lịch
                string namAm = namam.namduong(currentYear.ToString());
                currentMonthLB.Text += namAm;

                // Đổ dữ liệu combobox tháng
                monthCBox.Items.Clear();
                for (int comMonth = 1; comMonth < 13; comMonth++)
                {
                    monthCBox.Items.Add("Tháng " + comMonth);
                }
                monthCBox.SelectedIndex = currentMonth - 1;

                // Đổ dữ liệu combobox năm
                yearCBox.Items.Clear();
                for (int comYear = 1960; comYear < 2050; comYear++)
                {
                    yearCBox.Items.Add(comYear.ToString());
                }
                yearCBox.SelectedItem = currentYear.ToString();

                // Hiển thị ngày hiện tại
                mainDayLB.Text = currentDay.ToString();
                mainMonthAndYearLB.Text = "THÁNG " + currentMonth + " NĂM " + currentYear;

                // Chuyển đổi sang âm lịch
                int[] mainLunaArray = vcal.convertSolar2Lunar(currentDay, currentMonth, currentYear, 7);
                if (mainLunaArray != null && mainLunaArray.Length >= 3)
                {
                    mainLunaDayLB.Text = "NHẰM NGÀY " + mainLunaArray[0];
                    mainLunaMonthLB.Text = "THÁNG " + mainLunaArray[1] + "/" + mainLunaArray[2];
                }

                // Xác định thứ
                string[] vietnameseWeekdays = { "", "THỨ HAI", "THỨ BA", "THỨ TƯ", "THỨ NĂM", "THỨ SÁU", "THỨ BẢY", "CHỦ NHẬT" };
                int weekdayIndex = (int)now.DayOfWeek;
                if (weekdayIndex == 0) weekdayIndex = 7; // Chủ nhật
                mainWeekdayLB.Text = vietnameseWeekdays[weekdayIndex];

                // Hiển thị lịch
                DisplayCalendar(currentMonth, currentYear, currentDay);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi tải form: " + ex.Message);
            }
        }

        private void DisplayCalendar(int month, int year, int currentDay = 0)
        {
            try
            {
                // Reset tất cả các ô
                for (int i = 1; i < 43; i++)
                {
                    this.Controls["day" + i].Text = "";
                    this.Controls["luna" + i].Text = "";
                    this.Controls["day" + i].BackColor = Color.White;
                    this.Controls["luna" + i].BackColor = Color.White;
                }

                DateTime firstDayOfMonth = new DateTime(year, month, 1);
                int startDay = (int)firstDayOfMonth.DayOfWeek;
                if (startDay == 0) startDay = 7; // Chủ nhật

                int daysInMonth = DateTime.DaysInMonth(year, month);

                // Hiển thị các ngày trong tháng
                for (int day = 1; day <= daysInMonth; day++)
                {
                    int position = startDay + day - 2;
                    if (position < 42) // Chỉ hiển thị trong 42 ô (6 tuần)
                    {
                        this.Controls["day" + (position + 1)].Text = day.ToString();

                        // Hiển thị âm lịch
                        int[] lunaDate = vcal.convertSolar2Lunar(day, month, year, 7);
                        if (lunaDate != null && lunaDate.Length >= 2)
                        {
                            this.Controls["luna" + (position + 1)].Text = lunaDate[0] + "/" + lunaDate[1];
                        }

                        // Đánh dấu ngày hiện tại
                        if (day == currentDay && month == DateTime.Now.Month && year == DateTime.Now.Year)
                        {
                            this.Controls["day" + (position + 1)].BackColor = Color.Pink;
                            this.Controls["luna" + (position + 1)].BackColor = Color.Pink;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi hiển thị lịch: " + ex.Message);
            }
        }

        private void viewButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (monthCBox.SelectedIndex == -1 || yearCBox.SelectedIndex == -1)
                {
                    MessageBox.Show("Vui lòng chọn đầy đủ tháng và năm");
                    return;
                }

                int selectedMonth = monthCBox.SelectedIndex + 1;
                int selectedYear = int.Parse(yearCBox.SelectedItem.ToString());

                currentMonthLB.Text = "THÁNG " + selectedMonth + " NĂM " + selectedYear;

                // Hiển thị năm âm lịch
                string namAm = namam.namduong(selectedYear.ToString());
                currentMonthLB.Text += namAm;

                // Hiển thị lịch
                DisplayCalendar(selectedMonth, selectedYear);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message);
            }
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void defaultButton_Click(object sender, EventArgs e)
        {
            Form1_Load(null, EventArgs.Empty);
        }

        private void helpButton_Click(object sender, EventArgs e)
        {
            Help_form help_f = new Help_form();
            help_f.ShowDialog();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            digitalClockLB.Text = now.ToString("HH:mm:ss");
            buoiLB.Text = (now.Hour < 12) ? "Sáng" : "Chiều";
        }

        private void digitalClockLB_Click(object sender, EventArgs e)
        {
            // Không cần xử lý
        }
    }
}