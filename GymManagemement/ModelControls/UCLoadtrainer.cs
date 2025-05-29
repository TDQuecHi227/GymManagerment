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
using GymManagemement.Service;

namespace GymManagemement
{
    public partial class UCLoadtrainer : UserControl
    {
        public event Action TrainerUpdated;
        public Loadtrainer currentTrainerData;
        public UCLoadtrainer()
        {
            InitializeComponent();
        }
        public void Setdata(Loadtrainer data)
        {
            currentTrainerData = data;
            txt_name.Text = data.Name;
            txt_Id.Text = data.ID.ToString();
            lb_email.Text = data.Email;
            lb_phone.Text = data.Phone;
            btn_special.Text = data.Specialization;
            if (data.Image != null)
            {
                using (MemoryStream ms = new MemoryStream(data.Image))
                {
                    //co sao lay vay
                    //pb_trainer.Image = Image.FromStream(ms);

                    //fix ti le anh
                    pb_trainer.Image = ResizeImage(Image.FromStream(ms), pb_trainer.Width, pb_trainer.Height);
                }
            }
            else
            {
                pb_trainer.Image = null; // hoặc ảnh mặc định
            }
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
        private void btn_delete_Click(object sender, EventArgs e)
        {
            deletetrainer();
        }
        private int getinttrainerid()
        {
            if (string.IsNullOrEmpty(txt_Id.Text))
                return -1;

            string numericPart = new string(txt_Id.Text.Where(char.IsDigit).ToArray());

            if(int.TryParse(numericPart, out int id)) return id;

            return -1;
        }
        private void deletetrainer()
        {
            int trainerID = getinttrainerid();

            if(trainerID == -1)
            {
                MessageBox.Show("ID is not valid", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var confirm = MessageBox.Show($"Are you sure to delete trainer #{trainerID}?",
                                           "Confirm",
                                           MessageBoxButtons.YesNo,
                                           MessageBoxIcon.Question);

            if(confirm == DialogResult.Yes)
            {
                string error = string.Empty; 
                Load_Trainer trainer = new Load_Trainer();

                Loadtrainer trainertoDelete = new Loadtrainer
                {
                    ID = trainerID,
                };
                if(trainer.DeleteTrainer(trainertoDelete,ref error))
                {
                    MessageBox.Show("Delete success!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Question);
                    this.Parent.Controls.Remove(this);
                }
                else  
                    MessageBox.Show("Error: " + error, "Information", MessageBoxButtons.OK, MessageBoxIcon.Error ); 
            }
        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
            edittrainer();
        }
        private void edittrainer()
        {
            var updateForm = new UpdateTrainer(currentTrainerData);
            if (updateForm.ShowDialog() == DialogResult.OK) 
            {
                var updated = updateForm.CurrentTrainerData;

                var service = new Load_Trainer();
                string err = null;
                if (service.UpdateTrainer(updated,ref err))
                {
                    Setdata(updated);
                    TrainerUpdated?.Invoke();
                }
                else
                {
                    MessageBox.Show("Lỗi update trainer: " + err + "Loi");
                }
                pb_trainer.BringToFront();
                pb_frame.UseTransparentBackground = true;
                pb_frame.BringToFront();
            }
        }
    }
}
