namespace WinFormsApp1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnKiemTra = new Button();
            btnDangNhap = new Button();
            lblMaNV = new Label();
            lblMatKhau = new Label();
            txtMaNV = new TextBox();
            txtMatKhau = new TextBox();
            label1 = new Label();
            btnDatLaiMatKhau = new Button();
            btnMoDangKy = new Button();
            SuspendLayout();
            // 
            // btnKiemTra
            // 
            btnKiemTra.Location = new Point(60, 403);
            btnKiemTra.Name = "btnKiemTra";
            btnKiemTra.Size = new Size(420, 43);
            btnKiemTra.TabIndex = 0;
            btnKiemTra.Text = "Kiểm tra kết nối";
            btnKiemTra.UseVisualStyleBackColor = true;
            btnKiemTra.Click += btnKiemTra_Click;
            // 
            // btnDangNhap
            // 
            btnDangNhap.Location = new Point(57, 267);
            btnDangNhap.Name = "btnDangNhap";
            btnDangNhap.Size = new Size(420, 40);
            btnDangNhap.TabIndex = 1;
            btnDangNhap.Text = "Đăng nhập";
            btnDangNhap.UseVisualStyleBackColor = true;
            btnDangNhap.Click += btnDangNhap_Click;
            // 
            // lblMaNV
            // 
            lblMaNV.AutoSize = true;
            lblMaNV.Location = new Point(57, 95);
            lblMaNV.Name = "lblMaNV";
            lblMaNV.Size = new Size(97, 20);
            lblMaNV.TabIndex = 2;
            lblMaNV.Text = "Mã nhân viên";
            // 
            // lblMatKhau
            // 
            lblMatKhau.AutoSize = true;
            lblMatKhau.Location = new Point(57, 188);
            lblMatKhau.Name = "lblMatKhau";
            lblMatKhau.Size = new Size(70, 20);
            lblMatKhau.TabIndex = 3;
            lblMatKhau.Text = "Mật khẩu";
            // 
            // txtMaNV
            // 
            txtMaNV.Location = new Point(57, 129);
            txtMaNV.Name = "txtMaNV";
            txtMaNV.Size = new Size(294, 27);
            txtMaNV.TabIndex = 4;
            // 
            // txtMatKhau
            // 
            txtMatKhau.Location = new Point(57, 221);
            txtMatKhau.Name = "txtMatKhau";
            txtMatKhau.PasswordChar = '*';
            txtMatKhau.Size = new Size(294, 27);
            txtMatKhau.TabIndex = 5;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(120, 31);
            label1.Name = "label1";
            label1.Size = new Size(161, 20);
            label1.TabIndex = 6;
            label1.Text = "QUẢN LÝ CHẤM CÔNG";
            // 
            // btnDatLaiMatKhau
            // 
            btnDatLaiMatKhau.Location = new Point(57, 313);
            btnDatLaiMatKhau.Name = "btnDatLaiMatKhau";
            btnDatLaiMatKhau.Size = new Size(420, 39);
            btnDatLaiMatKhau.TabIndex = 7;
            btnDatLaiMatKhau.Text = "Đặt lại mật khẩu";
            btnDatLaiMatKhau.UseVisualStyleBackColor = true;
            btnDatLaiMatKhau.Click += btnDatLaiMatKhau_Click;
            // 
            // btnMoDangKy
            // 
            btnMoDangKy.Location = new Point(60, 358);
            btnMoDangKy.Name = "btnMoDangKy";
            btnMoDangKy.Size = new Size(417, 39);
            btnMoDangKy.TabIndex = 8;
            btnMoDangKy.Text = "Đăng ký";
            btnMoDangKy.UseVisualStyleBackColor = true;
            btnMoDangKy.Click += btnMoDangKy_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnMoDangKy);
            Controls.Add(btnDatLaiMatKhau);
            Controls.Add(label1);
            Controls.Add(txtMatKhau);
            Controls.Add(txtMaNV);
            Controls.Add(lblMatKhau);
            Controls.Add(lblMaNV);
            Controls.Add(btnDangNhap);
            Controls.Add(btnKiemTra);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnKiemTra;
        private Button btnDangNhap;
        private Label lblMaNV;
        private Label lblMatKhau;
        private TextBox txtMaNV;
        private TextBox txtMatKhau;
        private Label label1;
        private Button btnDatLaiMatKhau;
        private Button btnMoDangKy;
    }
}
