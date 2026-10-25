namespace QuanLyChamCong
{
    partial class FormDangKy
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
            btnDangKy = new Button();
            lblTieuDe = new Label();
            lblMaNV = new Label();
            lblTen = new Label();
            lblMatKhau = new Label();
            lblXacNhanMatKhau = new Label();
            txtTen = new TextBox();
            txtXacNhanMatKhau = new TextBox();
            txtMaNV = new TextBox();
            txtMatKhau = new TextBox();
            SuspendLayout();
            // 
            // btnDangKy
            // 
            btnDangKy.Location = new Point(194, 296);
            btnDangKy.Name = "btnDangKy";
            btnDangKy.Size = new Size(94, 29);
            btnDangKy.TabIndex = 0;
            btnDangKy.Text = "Đăng ký";
            btnDangKy.UseVisualStyleBackColor = true;
            btnDangKy.Click += btnDangKy_Click;
            // 
            // lblTieuDe
            // 
            lblTieuDe.AutoSize = true;
            lblTieuDe.Location = new Point(149, 21);
            lblTieuDe.Name = "lblTieuDe";
            lblTieuDe.Size = new Size(155, 20);
            lblTieuDe.TabIndex = 1;
            lblTieuDe.Text = "ĐĂNG KÝ NHÂN VIÊN";
            lblTieuDe.Click += lblTieuDe_Click;
            // 
            // lblMaNV
            // 
            lblMaNV.AutoSize = true;
            lblMaNV.Location = new Point(56, 80);
            lblMaNV.Name = "lblMaNV";
            lblMaNV.Size = new Size(97, 20);
            lblMaNV.TabIndex = 2;
            lblMaNV.Text = "Mã nhân viên";
            // 
            // lblTen
            // 
            lblTen.AutoSize = true;
            lblTen.Location = new Point(56, 134);
            lblTen.Name = "lblTen";
            lblTen.Size = new Size(73, 20);
            lblTen.TabIndex = 3;
            lblTen.Text = "Họ và tên";
            // 
            // lblMatKhau
            // 
            lblMatKhau.AutoSize = true;
            lblMatKhau.Location = new Point(56, 186);
            lblMatKhau.Name = "lblMatKhau";
            lblMatKhau.Size = new Size(70, 20);
            lblMatKhau.TabIndex = 4;
            lblMatKhau.Text = "Mật khẩu";
            // 
            // lblXacNhanMatKhau
            // 
            lblXacNhanMatKhau.AutoSize = true;
            lblXacNhanMatKhau.Location = new Point(54, 238);
            lblXacNhanMatKhau.Name = "lblXacNhanMatKhau";
            lblXacNhanMatKhau.Size = new Size(134, 20);
            lblXacNhanMatKhau.TabIndex = 5;
            lblXacNhanMatKhau.Text = "Xác nhận mật khẩu";
            // 
            // txtTen
            // 
            txtTen.Location = new Point(194, 131);
            txtTen.Name = "txtTen";
            txtTen.Size = new Size(292, 27);
            txtTen.TabIndex = 6;
            // 
            // txtXacNhanMatKhau
            // 
            txtXacNhanMatKhau.Location = new Point(194, 231);
            txtXacNhanMatKhau.Name = "txtXacNhanMatKhau";
            txtXacNhanMatKhau.Size = new Size(292, 27);
            txtXacNhanMatKhau.TabIndex = 7;
            // 
            // txtMaNV
            // 
            txtMaNV.Location = new Point(194, 73);
            txtMaNV.Name = "txtMaNV";
            txtMaNV.Size = new Size(292, 27);
            txtMaNV.TabIndex = 8;
            // 
            // txtMatKhau
            // 
            txtMatKhau.Location = new Point(194, 179);
            txtMatKhau.Name = "txtMatKhau";
            txtMatKhau.Size = new Size(292, 27);
            txtMatKhau.TabIndex = 9;
            // 
            // FormDangKy
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(txtMatKhau);
            Controls.Add(txtMaNV);
            Controls.Add(txtXacNhanMatKhau);
            Controls.Add(txtTen);
            Controls.Add(lblXacNhanMatKhau);
            Controls.Add(lblMatKhau);
            Controls.Add(lblTen);
            Controls.Add(lblMaNV);
            Controls.Add(lblTieuDe);
            Controls.Add(btnDangKy);
            Name = "FormDangKy";
            Text = "FormDangKy";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnDangKy;
        private Label lblTieuDe;
        private Label lblMaNV;
        private Label lblTen;
        private Label lblMatKhau;
        private Label lblXacNhanMatKhau;
        private TextBox txtTen;
        private TextBox txtXacNhanMatKhau;
        private TextBox txtMaNV;
        private TextBox txtMatKhau;
    }
}