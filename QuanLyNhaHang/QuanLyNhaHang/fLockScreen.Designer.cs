namespace QuanLyNhaHang
{
    partial class fLockScreen
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fLockScreen));
            this.label1 = new System.Windows.Forms.Label();
            this.tbxInput = new Bunifu.Framework.UI.BunifuMaterialTextbox();
            this.btnOK = new Bunifu.Framework.UI.BunifuThinButton2();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Nhập mật khẩu:";
            // 
            // tbxInput
            // 
            this.tbxInput.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tbxInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.tbxInput.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tbxInput.HintForeColor = System.Drawing.Color.Empty;
            this.tbxInput.HintText = "";
            this.tbxInput.isPassword = false;
            this.tbxInput.LineFocusedColor = System.Drawing.Color.Blue;
            this.tbxInput.LineIdleColor = System.Drawing.Color.Gray;
            this.tbxInput.LineMouseHoverColor = System.Drawing.Color.Blue;
            this.tbxInput.LineThickness = 3;
            this.tbxInput.Location = new System.Drawing.Point(14, 28);
            this.tbxInput.Margin = new System.Windows.Forms.Padding(4);
            this.tbxInput.Name = "tbxInput";
            this.tbxInput.Size = new System.Drawing.Size(254, 44);
            this.tbxInput.TabIndex = 5;
            this.tbxInput.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbxInput.Enter += new System.EventHandler(this.tbxInput_Enter);
            // 
            // btnOK
            // 
            this.btnOK.ActiveBorderThickness = 1;
            this.btnOK.ActiveCornerRadius = 20;
            this.btnOK.ActiveFillColor = System.Drawing.Color.SeaGreen;
            this.btnOK.ActiveForecolor = System.Drawing.Color.White;
            this.btnOK.ActiveLineColor = System.Drawing.Color.SeaGreen;
            this.btnOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.btnOK.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnOK.BackgroundImage")));
            this.btnOK.ButtonText = "Mở khóa";
            this.btnOK.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.ForeColor = System.Drawing.Color.SeaGreen;
            this.btnOK.IdleBorderThickness = 1;
            this.btnOK.IdleCornerRadius = 20;
            this.btnOK.IdleFillColor = System.Drawing.Color.White;
            this.btnOK.IdleForecolor = System.Drawing.Color.SeaGreen;
            this.btnOK.IdleLineColor = System.Drawing.Color.SeaGreen;
            this.btnOK.Location = new System.Drawing.Point(85, 95);
            this.btnOK.Margin = new System.Windows.Forms.Padding(5);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(115, 41);
            this.btnOK.TabIndex = 7;
            this.btnOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // fLockScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(280, 150);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbxInput);
            this.FormBorderEffect = DevExpress.XtraEditors.FormBorderEffect.Glow;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "fLockScreen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "fLockScreen";
            this.Load += new System.EventHandler(this.fLockScreen_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Bunifu.Framework.UI.BunifuThinButton2 btnOK;
        private System.Windows.Forms.Label label1;
        private Bunifu.Framework.UI.BunifuMaterialTextbox tbxInput;
    }
}