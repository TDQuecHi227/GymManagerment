using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GymManagemement.Models;

namespace GymManagemement
{
    public partial class FrmAddToCart : Form
    {
        private int Sl = 1;
        private Product product = new Product();
        public FrmAddToCart()
        {
            InitializeComponent();
        }
        public void LoadData(Product product)
        {
            this.product = product;
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
            txtQuantity.Text = Sl.ToString();
        }

        private void btnPlus_Click(object sender, EventArgs e)
        {
            if (Sl <= product.Quantity)
            {
                Sl++;
            }
            txtQuantity.Text = Sl.ToString();
        }

        private void btnMinus_Click(object sender, EventArgs e)
        {
            if (Sl > 1)
            {
                Sl--;
            }
            txtQuantity.Text = Sl.ToString();
        }

        private void txtQuantity_TextChanged(object sender, EventArgs e)
        {
            if (txtQuantity.Text == "")
            {
                Sl = 1;
                txtQuantity.Text = Sl.ToString();      
            }
            else if (Convert.ToInt32(txtQuantity.Text) > product.Quantity)
            {
                Sl = product.Quantity;
                txtQuantity.Text = Sl.ToString();
            }
            else
            {
                Sl = Convert.ToInt32(txtQuantity.Text);
            }
        }

        private void btnAddCart_Click(object sender, EventArgs e)
        {
            Cart cart = new Cart
            {
                ProductId = product.Id,
                Name = product.Name,
                Quantity = Sl,
                Price = product.Price
            };
            CartManager.AddToCart(cart);
            this.Close();
        }
    }
}
