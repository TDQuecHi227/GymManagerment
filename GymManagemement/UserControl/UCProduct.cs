using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GymManagemement.ModelControls;
using GymManagemement.Models;
using GymManagemement.Services;

namespace GymManagemement
{
    public partial class UCProduct : UserControl
    {
        private Product selectedProduct = null;
        private Product_Contain selectedControl = null;
        public UCProduct()
        {
            InitializeComponent();
        }
        private void UCProduct_Load(object sender, EventArgs e)
        {
            LoadProducts();
        }
        private void LoadProducts()
        {
            flpProduct.Controls.Clear(); // Clear existing controls
            Load_Product loadProduct = new Load_Product();
            List<Product> products = loadProduct.GetProduct();
            foreach (var product in products)
            {
                Product_Contain productContain = new Product_Contain();
                productContain.SetData(product);
                productContain.Click += Product_Contain_Click;
                productContain.ProductBought += ProductContain_ProductBought;
                flpProduct.Controls.Add(productContain);
            }
        }
        private void ProductContain_ProductBought(object sender, EventArgs e)
        {
            LoadProducts();
        }
        private void Product_Contain_Click(object sender, EventArgs e)
        {
            var control = sender as Product_Contain;
            if (control != null)
            {
                selectedProduct = control.ProductData;
                selectedControl = control;

                // Đổi màu viền cho biết là đang chọn
                foreach (Product_Contain pc in flpProduct.Controls)
                    pc.BorderStyle = BorderStyle.None;

                control.BorderStyle = BorderStyle.FixedSingle;
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (selectedProduct == null)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm để xóa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa sản phẩm này không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
            {
                return;
            }
            Load_Product loadProduct = new Load_Product();
            bool isDeleted = loadProduct.DeleteProduct(selectedProduct.Id.ToString());
            if (isDeleted)
            {
                MessageBox.Show("Xóa sản phẩm thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                flpProduct.Controls.Remove(selectedControl);
                selectedControl = null;
                selectedProduct = null;
            }
            else
            {
                MessageBox.Show("Xóa sản phẩm thất bại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            FrmAddProduct formAdd = new FrmAddProduct();
            if (formAdd.ShowDialog() == DialogResult.OK)
            {
                LoadProducts();
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim().ToLower();

            foreach (Control ctrl in flpProduct.Controls)
            {
                if (ctrl is Product_Contain productContain)
                {
                    string productName = productContain.ProductData?.Name?.ToLower() ?? "";

                    // Ẩn nếu không khớp
                    productContain.Visible = productName.Contains(keyword);
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedProduct == null)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm để cập nhật", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            FrmUpdateProduct frmUpdate = new FrmUpdateProduct();
            frmUpdate.LoadData(selectedProduct);
            if (frmUpdate.ShowDialog() == DialogResult.OK)
            {
                LoadProducts();
            }

        }

        private void btn_reload_Click(object sender, EventArgs e)
        {
            LoadProducts();
        }

        private void btnCart_Click(object sender, EventArgs e)
        {
            FrmListCart cartForm = new FrmListCart();
            if (cartForm.ShowDialog() == DialogResult.OK)
            {
                LoadProducts();
            }
        }
    }
}
