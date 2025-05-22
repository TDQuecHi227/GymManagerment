using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GymManagemement.Activities;
namespace GymManagemement
{
    public partial class ActivityItemControl : UserControl
    {
        public ActivityItemControl()
        {
            InitializeComponent();
        }
        public static string FormatTimeAgo(DateTime time)
        {
            TimeSpan diff = DateTime.Now - time;

            if (diff.TotalSeconds < 60)
                return $"{(int)diff.TotalSeconds} giây trước";
            if (diff.TotalMinutes < 60)
                return $"{(int)diff.TotalMinutes} phút trước";
            if (diff.TotalHours < 24)
                return $"{(int)diff.TotalHours} giờ trước";
            if (diff.TotalDays < 2)
                return "Hôm qua";
            return $"{(int)diff.TotalDays} ngày trước";
        }
        public void SetData(ActivityItem data)
        {
            lbDescription.Text = data.Description;
            lbTime.Text = FormatTimeAgo(data.TimeAgo);
        }
    }
}
