namespace GymManagemement
{
    partial class UCLoadpayment
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.llbMore = new System.Windows.Forms.LinkLabel();
            this.lb_status = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lb_date = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lb_amount = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lb_phone = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lb_ID = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.SuspendLayout();
            // 
            // llbMore
            // 
            this.llbMore.AutoSize = true;
            this.llbMore.Location = new System.Drawing.Point(859, 9);
            this.llbMore.Name = "llbMore";
            this.llbMore.Size = new System.Drawing.Size(39, 13);
            this.llbMore.TabIndex = 70;
            this.llbMore.TabStop = true;
            this.llbMore.Text = "Chi tiết";
            this.llbMore.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llbMore_LinkClicked);
            // 
            // lb_status
            // 
            this.lb_status.BackColor = System.Drawing.Color.Transparent;
            this.lb_status.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_status.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lb_status.Location = new System.Drawing.Point(695, 8);
            this.lb_status.Margin = new System.Windows.Forms.Padding(2);
            this.lb_status.Name = "lb_status";
            this.lb_status.Size = new System.Drawing.Size(29, 17);
            this.lb_status.TabIndex = 69;
            this.lb_status.Text = "Bank";
            // 
            // lb_date
            // 
            this.lb_date.BackColor = System.Drawing.Color.Transparent;
            this.lb_date.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_date.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lb_date.Location = new System.Drawing.Point(524, 8);
            this.lb_date.Margin = new System.Windows.Forms.Padding(2);
            this.lb_date.Name = "lb_date";
            this.lb_date.Size = new System.Drawing.Size(69, 17);
            this.lb_date.TabIndex = 68;
            this.lb_date.Text = "22/05/2005";
            // 
            // lb_amount
            // 
            this.lb_amount.BackColor = System.Drawing.Color.Transparent;
            this.lb_amount.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_amount.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lb_amount.Location = new System.Drawing.Point(379, 8);
            this.lb_amount.Margin = new System.Windows.Forms.Padding(2);
            this.lb_amount.Name = "lb_amount";
            this.lb_amount.Size = new System.Drawing.Size(56, 17);
            this.lb_amount.TabIndex = 67;
            this.lb_amount.Text = "1.000.000";
            // 
            // lb_phone
            // 
            this.lb_phone.BackColor = System.Drawing.Color.Transparent;
            this.lb_phone.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_phone.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lb_phone.Location = new System.Drawing.Point(196, 8);
            this.lb_phone.Margin = new System.Windows.Forms.Padding(2);
            this.lb_phone.Name = "lb_phone";
            this.lb_phone.Size = new System.Drawing.Size(72, 17);
            this.lb_phone.TabIndex = 66;
            this.lb_phone.Text = "0769693509";
            // 
            // lb_ID
            // 
            this.lb_ID.BackColor = System.Drawing.Color.Transparent;
            this.lb_ID.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_ID.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lb_ID.Location = new System.Drawing.Point(21, 8);
            this.lb_ID.Margin = new System.Windows.Forms.Padding(2);
            this.lb_ID.Name = "lb_ID";
            this.lb_ID.Size = new System.Drawing.Size(8, 17);
            this.lb_ID.TabIndex = 65;
            this.lb_ID.Text = "1";
            // 
            // UCLoadpayment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.llbMore);
            this.Controls.Add(this.lb_status);
            this.Controls.Add(this.lb_date);
            this.Controls.Add(this.lb_amount);
            this.Controls.Add(this.lb_phone);
            this.Controls.Add(this.lb_ID);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "UCLoadpayment";
            this.Size = new System.Drawing.Size(921, 33);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel llbMore;
        private Guna.UI2.WinForms.Guna2HtmlLabel lb_status;
        private Guna.UI2.WinForms.Guna2HtmlLabel lb_date;
        private Guna.UI2.WinForms.Guna2HtmlLabel lb_amount;
        private Guna.UI2.WinForms.Guna2HtmlLabel lb_phone;
        private Guna.UI2.WinForms.Guna2HtmlLabel lb_ID;
    }
}
