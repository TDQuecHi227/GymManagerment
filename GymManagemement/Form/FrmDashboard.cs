using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GymManagemement
{  
    public partial class FrmDashboard : Form
    {
        private UCNotification uCNotification;
        private Timer autoCloseTimer;
        public FrmDashboard()
        {
            InitializeComponent();
        }
        public void SetFormRoundedRegion(Form form, int cornerRadius)
        {
            Rectangle bounds = new Rectangle(0, 0, form.Width, form.Height);
            int diameter = cornerRadius * 2;
            GraphicsPath path = new GraphicsPath();

            path.StartFigure();
            path.AddArc(bounds.X, bounds.Y, diameter, diameter, 180, 90); // top-left
            path.AddArc(bounds.Right - diameter, bounds.Y, diameter, diameter, 270, 90); // top-right
            path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90); // bottom-right
            path.AddArc(bounds.X, bounds.Bottom - diameter, diameter, diameter, 90, 90); // bottom-left
            path.CloseFigure();

            form.Region = new Region(path);
        }
        protected override void WndProc(ref Message m)
        {
            const int WM_NCHITTEST = 0x84;
            const int HTCLIENT = 1;
            const int HTCAPTION = 2;

            if (m.Msg == WM_NCHITTEST)
            {
                m.Result = (IntPtr)HTCAPTION;
                return;
            }
            base.WndProc(ref m);
        }
        private void LoadUserControl(UserControl uc)
        {
            uc.Dock = DockStyle.Fill;
            plHome.Controls.Clear();
            plHome.Controls.Add(uc);
        }
        private void HomeFunction()
        {
            UCHome uCHome = new UCHome();
            uCHome.Dock = DockStyle.Fill;
            uCHome.MoreClicked += Home_MoreClicked;
            plHome.Controls.Clear();
            plHome.Controls.Add(uCHome);
        }
        private void Home_MoreClicked(object sender, EventArgs e)
        {
            LoadUserControl(new UCMember());
        }
        private void FrmDashboard_Load(object sender, EventArgs e)
        {
            HomeFunction();
            SetFormRoundedRegion(this, 20);    
        }
        public void GunaButton_Click(object sender, EventArgs e)
        {
            Guna.UI2.WinForms.Guna2ImageButton button = sender as Guna.UI2.WinForms.Guna2ImageButton;
            if (button != null)
            {
                // Perform action based on the button clicked
                switch (button.Name)
                {
                    case "btnNotifi":
                        if (uCNotification == null || uCNotification.IsDisposed)
                        {
                            uCNotification = new UCNotification();
                            Point location = btnNotifi.PointToScreen(Point.Empty);
                            location = this.PointToClient(location);
                            uCNotification.Location = new Point(location.X + btnNotifi.Width + 15, location.Y + btnNotifi.Height - 50);
                            this.Controls.Add(uCNotification);
                            uCNotification.BringToFront();
                        }
                        else
                        {
                            this.Controls.Remove(uCNotification);
                            uCNotification.Dispose();
                            uCNotification = null;
                        }
                        break;
                    case "btnHome":
                        HomeFunction();
                        break;
                    case "btnSchedule":
                        // Open Manage Payments form
                        break;
                    case "btnMember":
                        LoadUserControl(new UCMember());
                        break;
                    case "btnTrainer":
                        // Open Reports form
                        break;
                    case "btnPay":
                        LoadUserControl(new UCPayment());
                        break;
                    case "btnPackage":
                        // Open Settings form
                        break;
                    case "btnClass":
                        // Open Settings form
                        break;
                    case "btnLogOut":
                        this.Hide();
                        FrmLogin loginForm = new FrmLogin();
                        loginForm.FormClosed += (s, args) => this.Close();
                        loginForm.Show();
                        break;
                    case "btnSP":
                        LoadUserControl(new UCProduct());
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
