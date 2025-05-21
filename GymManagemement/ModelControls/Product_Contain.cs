using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GymManagemement.Connection;
using GymManagemement.Models;

namespace GymManagemement.ModelControls
{ 
    public partial class Product_Contain : UserControl
    {
        public event EventHandler ProductBought;
        public Product ProductData { get; private set; }
        ConnDB conndb = new ConnDB();
        public Product_Contain()
        {
            InitializeComponent();
        }
        public void SetData(Product product)
        {
            ProductData = product;
            lbName_Product.Text = product.Name;
            lbPrice.Text = product.Price.ToString("N0") + " VND";
            lbQuantity.Text = product.Quantity.ToString();
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
            Description.SetToolTip(this.PicProduct, product.Description);
        }

        private void btnBuy_Click(object sender, EventArgs e)
        {
            FrmBuyProduct buyProductForm = new FrmBuyProduct();
            buyProductForm.LoadData(ProductData);
            if (buyProductForm.ShowDialog() == DialogResult.OK)
            {
                ProductBought?.Invoke(this, EventArgs.Empty); // Gửi thông báo cho cha
            }
        }

        private void btnAddCart_Click(object sender, EventArgs e)
        {
            FrmAddToCart addToCartForm = new FrmAddToCart();
            addToCartForm.LoadData(ProductData);
            addToCartForm.ShowDialog();
        }
    }
}
