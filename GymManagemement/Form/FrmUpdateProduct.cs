using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GymManagemement.Models;
using GymManagemement.Services;

namespace GymManagemement
{
    public partial class FrmUpdateProduct : Form
    {
        public FrmUpdateProduct()
        {
            InitializeComponent();
        }
        public void  LoadData(Product product)
        {
            lbID.Text = product.Id.ToString();
            txtName.Text = product.Name;
            txtPrice.Text = product.Price.ToString("N0");
            txtQuantity.Text = product.Quantity.ToString();
            txtDescription.Text = product.Description;
            if (product.Image != null && product.Image.Length > 0)
            {
                using (var ms = new System.IO.MemoryStream(product.Image))
                {
                    PicProduct.Image = Image.FromStream(ms);
                }
            }
            else
            {
                PicProduct.Image = null; // hoặc gán ảnh mặc định: Image.FromFile("noimage.png")
            }
        }

        private void btnChange_Image_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                PicProduct.Image = Image.FromFile(ofd.FileName);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Load_Product load_Product = new Load_Product();
            Product product = new Product();
            product.Id = int.Parse(lbID.Text.Trim());
            product.Name = txtName.Text.Trim();
            product.Price = int.Parse(txtPrice.Text.Trim().Replace(".",""));
            product.Quantity = int.Parse(txtQuantity.Text.Trim());
            product.Description = txtDescription.Text.Trim();
            byte[] image = null;
            if (PicProduct.Image != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (Bitmap bmp = new Bitmap(PicProduct.Image))
                    {
                        bmp.Save(ms, ImageFormat.Png);
                        image = ms.ToArray();
                    }
                }
            }
            product.Image = image;
            bool result = load_Product.UpdateProduct(product);
            if (result)
            {
                MessageBox.Show("Cập nhật sản phẩm thành công!");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Cập nhật thất bại!");
            }
        }

        private void txtPrice_TextChanged(object sender, EventArgs e)
        {
            if (txtPrice.Text == "") return;
            string text = txtPrice.Text.Replace(".", "");
            if (!long.TryParse(text, out long value)) return;
            int selStart = txtPrice.SelectionStart;
            int lengthBefore = txtPrice.Text.Length;
            txtPrice.Text = value.ToString("N0", new System.Globalization.CultureInfo("vi-VN"));
            int lengthAfter = txtPrice.Text.Length;
            txtPrice.SelectionStart = selStart + (lengthAfter - lengthBefore);
        }
    }
}
