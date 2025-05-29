using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GymManagemement
{
    public partial class UpdateTrainer : Form
    {
        private byte[] selectedImageBytes;
        public Loadtrainer CurrentTrainerData {  get; set; }

        public UpdateTrainer(Loadtrainer trainerToEdit) : this()
        {
            CurrentTrainerData = trainerToEdit;
        }
        public UpdateTrainer()
        {
            InitializeComponent();
            this.AcceptButton = btn_update;
        }

        private void UpdateTrainer_Load(object sender, EventArgs e)
        {
            if (CurrentTrainerData == null)
            {
                MessageBox.Show("Dữ liệu huấn luyện viên chưa được truyền vào.");
                return;
            }

            txt_id.Text = CurrentTrainerData.ID.ToString();
            txt_name.Text = CurrentTrainerData.Name;
            txt_phone.Text = CurrentTrainerData.Phone;
            txt_email.Text = CurrentTrainerData.Email;
            txt_special.Text = CurrentTrainerData.Specialization;

            // Nếu bạn muốn hiển thị ảnh (nếu có):
            if (CurrentTrainerData.Image != null)
            {
                using (MemoryStream ms = new MemoryStream(CurrentTrainerData.Image))
                {
                    Image img = Image.FromStream(ms);
                    pb_trainer.Image = ResizeImage(img, pb_trainer.Width, pb_trainer.Height);
                    preview_avt.Image = pb_trainer.Image;
                }
            }
            selectedImageBytes = CurrentTrainerData.Image;
        }
        private void btn_upload_Click(object sender, EventArgs e)
        {
            uploadimage();
            btn_upload.ForeColor = Color.Black;
        }
        private void btn_remove_Click(object sender, EventArgs e)
        {
            resetImage();
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
                        preview_avt.Image = pb_trainer.Image = ResizeImage(loadedImage, pb_trainer.Width, pb_trainer.Height);
                        

                        //co sao lay vay
                        //preview_avt.Image = pb_trainer.Image = loadedImage;
                    }
                }
            }
            preview_avt.BringToFront();
            preview_frame.UseTransparentBackground = true;
            preview_frame.BringToFront();

            pb_trainer.BringToFront();
            pb_camera.UseTransparentBackground = true;
            pb_frame.BringToFront();
            pb_camera.BringToFront();
        }
        private void resetImage()
        {
            pb_trainer.Image = null;
            pb_trainer.BringToFront();
            pb_camera.UseTransparentBackground = true;
            pb_frame.BringToFront();
            pb_camera.BringToFront();
            btn_remove.ForeColor = Color.Black;

            preview_avt.Image = null;
            preview_avt.BringToFront();
            preview_frame.UseTransparentBackground = true;
            preview_frame.BringToFront();
        }
        private void TxtFields_TextChanged(object sender, EventArgs e)
        {
            if (sender == txt_id) preview_ID.Text = txt_id.Text;
            else if (sender == txt_name) preview_name.Text = txt_name.Text;
            else if (sender == txt_phone) preview_phone.Text = txt_phone.Text;
            else if (sender == txt_email) preview_email.Text = txt_email.Text;
            else if (sender == txt_special) preview_special.Text = txt_special.Text;
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            if (!ValidateFields()) return;

            if (!IsValidGmail(txt_email.Text))
            {
                MessageBox.Show("Vui lòng nhập email hợp lệ có dạng [abc]@[def].[ghk]", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_email.Focus();
                return;
            }

            CurrentTrainerData.Name = txt_name.Text.Trim();
            CurrentTrainerData.Email = txt_email.Text.Trim();
            CurrentTrainerData.Phone = txt_phone.Text.Trim();
            CurrentTrainerData.Specialization = txt_special.Text.Trim();
            CurrentTrainerData.Image = selectedImageBytes;

            if (selectedImageBytes != null)
            {
                CurrentTrainerData.Image = selectedImageBytes;
            }
            else if (CurrentTrainerData.Image == null)
            {
                // Nếu chưa có ảnh nào, đặt là DBNull hoặc null tùy xử lý bên dưới
                CurrentTrainerData.Image = null;
            }

            string err = "";
            var service = new Service.Load_Trainer();
            bool ok = service.UpdateTrainer(CurrentTrainerData, ref err);

            if(ok)
            {
                MessageBox.Show("Successfully!", "Noti", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("update error: " + err + "Error: ");
            }
        }

        private void btn_reset_Click(object sender, EventArgs e)
        {
            txt_name.Clear();
            txt_phone.Clear();
            txt_email.Clear();
            txt_special.Clear();
            txt_des.Clear();

            resetImage();
        }
        private bool ValidateFields()
        {
            txt_name.Text = txt_name.Text.Trim();
            if (string.IsNullOrEmpty(txt_name.Text))
            {
                MessageBox.Show("Please fill in a valid name (no blanks or spaces).");
                txt_name.Focus();
                return false;
            }

            txt_phone.Text = txt_phone.Text.Trim();
            if (string.IsNullOrEmpty(txt_phone.Text))
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

        private void txt_name_KeyPress(object sender, KeyPressEventArgs e)
        {
            OnlyChar_KeyPress (sender, e);
        }

        private void txt_phone_KeyPress(object sender, KeyPressEventArgs e)
        {
            OnlyDigit_KeyPress (sender, e);
        }

        private void txt_email_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) &&
            !char.IsLetterOrDigit(e.KeyChar) &&
            e.KeyChar != '.' && e.KeyChar != '_' && e.KeyChar != '@')
            {
                e.Handled = true; // Chặn ký tự không hợp lệ
            }
        }

        private void txt_special_KeyPress(object sender, KeyPressEventArgs e)
        {
            OnlyChar_KeyPress (sender, e);
        }
    }
}
