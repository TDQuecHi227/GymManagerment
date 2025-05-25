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
using GymManagemement.Models;
using GymManagemement.Services;


namespace GymManagemement
{
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
        }
        public void BoGocPanel(Panel panel, int radius)
        {
            Rectangle bounds = panel.ClientRectangle;
            GraphicsPath path = new GraphicsPath();

            int d = radius * 2;

            path.StartFigure();
            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90); // top-left
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90); // top-right
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90); // bottom-right
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90); // bottom-left
            path.CloseFigure();

            panel.Region = new Region(path);
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {
            BoGocPanel(this.plLogin, 20);
            this.plLogin.BackColor = Color.FromArgb(200, 255, 255, 255);
            this.txtUser.BackColor = Color.FromArgb(0, 255, 255, 255);
            this.txtPass.BackColor = Color.FromArgb(0, 255, 255, 255);
            this.picLogin.BackColor = Color.FromArgb(0, 255, 255, 255);
            this.picPass.BackColor = Color.FromArgb(0, 255, 255, 255);
            this.btnLogin.BackColor = Color.FromArgb(0, 255, 255, 255);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {   
            User user = new Load_User().GetUser();
            if (txtUser.Text.Trim() == user.UserName & txtPass.Text.Trim() == user.Password)
            {
                this.Hide();
                FrmDashboard main = new FrmDashboard();
                main.FormClosed += (s, args) => this.Close();
                main.Show();
            }
            else
            {
                MessageBox.Show("Tài khoản hoặc mật khẩu không đúng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUser.Focus();
            }

        }

        private void Visible_Click(object sender, EventArgs e)
        {
            txtPass.PasswordChar = '\0';
            Visible.Visible = false;
            Invisible.Visible = true;
        }

        private void Invisible_Click(object sender, EventArgs e)
        {
            txtPass.PasswordChar = '*';
            Visible.Visible = true;
            Invisible.Visible = false;
        }
    }
}
