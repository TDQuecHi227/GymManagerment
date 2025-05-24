using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GymManagemement.NewMembers
{
    public partial class NewMemControl : UserControl
    {
        public NewMemControl()
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
        public void SetData(NewMember data)
        {
            lbName.Text = data.Name;
            lbTime.Text = FormatTimeAgo(data.RegisteredAt);
            
        }
    }
}
