using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using GymManagemement.Service;

namespace GymManagemement
{
    public partial class Addtrainer : Form
    {
        private byte[] selectedImageBytes;

        public Loadtrainer NewTrainerData { get; set; }
        public Addtrainer()
        {
            InitializeComponent();
            pb_trainer.Image = null;
        }

        private void Addtrainer_Load(object sender, EventArgs e)
        {
            var service = new Service.Load_Trainer();
            txt_ID.Text = service.GetNextTrainerId();
        }
        private void pb_frame_DoubleClick(object sender, EventArgs e)
        {
            uploadimage();
        }
        private void pb_upload_Click(object sender, EventArgs e)
        {
            uploadimage();
        }
        private Image ResizeImage(Image image, int width, int height)
        {
            Bitmap bmp = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(image, 0, 0, width, height);
            }
            return bmp;
        }
        private void uploadimage()
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Select an Image";
                ofd.Filter = "Image Files (*.jpg;*.jpeg;*.png;*.bmp;*.gif)|*.jpg;*.jpeg;*.png;*.bmp;*.gif";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    selectedImageBytes = File.ReadAllBytes(ofd.FileName);

                    using (MemoryStream ms = new MemoryStream(selectedImageBytes))
                    {
                        Image loadedImage = Image.FromStream(ms);
                        //fix anh
                        //pb_trainer.Image = ResizeImage(loadedImage, pb_trainer.Width, pb_trainer.Height);

                        //co sao lay vay
                        pb_trainer.Image = loadedImage;
                    }
                }
            }
            pb_trainer.BringToFront();
            pb_upload.UseTransparentBackground = true;
            pb_frame.BringToFront();
            pb_upload.BringToFront();
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            if (!ValidateFields()) return;

            if(!IsValidGmail(txt_email.Text))
            {
                MessageBox.Show("Vui lòng nhập email hợp lệ có dạng [abc]@[def].[ghk]", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_email.Focus();
                return;
            }

            NewTrainerData = new Loadtrainer
            {
                Name = txt_name.Text,
                Phone = txt_phone.Text,
                Email = txt_email.Text,
                Specialization = txt_special.Text,
                Image = selectedImageBytes,
            };

            DialogResult = DialogResult.OK;
            Close();
        }
        private bool ValidateFields()
        {
            txt_name.Text = txt_name.Text.Trim();
            if(string.IsNullOrEmpty(txt_name.Text))
            {
                MessageBox.Show("Please fill in a valid name (no blanks or spaces).");
                txt_name.Focus();
                return false;
            }

            txt_phone.Text = txt_phone.Text.Trim();
            if(string.IsNullOrEmpty(txt_phone.Text))
            {
                MessageBox.Show("Please fill in a valid phone number (no blanks or spaces).");
                txt_phone.Focus();
                return false;
            }

            txt_email.Text = txt_email.Text.Trim();
            if (string.IsNullOrEmpty(txt_email.Text))
            {
                MessageBox.Show("Please fill in a valid email (no blanks or spaces).");
                txt_email.Focus();
                return false;
            }

            txt_special.Text = txt_special.Text.Trim();
            if (string.IsNullOrEmpty(txt_special.Text))
            {
                MessageBox.Show("Please fill in a valid specialization (no blanks or spaces).");
                txt_special.Focus();
                return false;
            }
            return true;
        }
        private bool IsValidGmail(string email)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(
                email,
                @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"
            );
        }
        private void OnlyDigit_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true;
        }
        private void OnlyChar_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ' ')
            {
                e.Handled = true; // chặn ký tự không hợp lệ
            }
        }
        private void txt_phone_KeyPress(object sender, KeyPressEventArgs e)
        {
            OnlyDigit_KeyPress (sender, e);
        }

        private void txt_name_KeyPress(object sender, KeyPressEventArgs e)
        {
            OnlyChar_KeyPress (sender, e);
        }
        private void txt_special_KeyPress(object sender, KeyPressEventArgs e)
        {
            OnlyChar_KeyPress ((object)sender, e);
        }
        private void txt_email_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Cho phép chữ cái, số, dấu chấm, gạch dưới, @, và phím điều khiển (Backspace...)
            if (!char.IsControl(e.KeyChar) &&
                !char.IsLetterOrDigit(e.KeyChar) &&
                e.KeyChar != '.' && e.KeyChar != '_' && e.KeyChar != '@')
            {
                e.Handled = true; // Chặn ký tự không hợp lệ
            }
        }
    }
}
