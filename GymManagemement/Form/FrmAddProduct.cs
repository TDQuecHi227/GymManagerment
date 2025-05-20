﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GymManagemement.Models;
using GymManagemement.Services;

namespace GymManagemement
{
    public partial class FrmAddProduct : Form
    {
        public FrmAddProduct()
        {
            InitializeComponent();
        }

        private void btnAdd_Image_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                PicProduct.Image = Image.FromFile(ofd.FileName);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Load_Product load_Product = new Load_Product();
            Product product = new Product();
            product.Name = txtName.Text.Trim();
            product.Price = int.Parse(txtPrice.Text.Trim());
            product.Quantity = int.Parse(txtQuantity.Text.Trim());
            product.Description = txtDescription.Text.Trim();
            byte[] image = null;
            if (PicProduct.Image != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    PicProduct.Image.Save(ms, PicProduct.Image.RawFormat);
                    image = ms.ToArray();
                }
            }
            product.Image = image;
            bool result = load_Product.InsertProduct(product);
            if (result)
            {
                MessageBox.Show("Thêm sản phẩm thành công!");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Thêm thất bại!");
            }
        }
    }
}
