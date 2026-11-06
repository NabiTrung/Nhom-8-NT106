using System.Drawing;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using Font = System.Drawing.Font;

namespace ClientNhanVien
{
    partial class FormDangKy
    {
        private System.ComponentModel.IContainer? components = null;
        private TextBox txtMaNV = null!;
        private TextBox txtTen = null!;
        private TextBox txtMatKhau = null!;
        private TextBox txtXacNhan = null!;
        private Button btnDangKy = null!;
        private Button btnHuy = null!;

        protected override void Dispose(bool disposing)
        {
            if (disposing) components?.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            txtMaNV = new TextBox();
            txtTen = new TextBox();
            txtMatKhau = new TextBox();
            txtXacNhan = new TextBox();
            btnDangKy = new Button();
            btnHuy = new Button();
            bang = new TableLayoutPanel();
            tieuDe = new Label();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            bang.SuspendLayout();
            SuspendLayout();
            // 
            // txtMaNV
            // 
            txtMaNV.Dock = DockStyle.Fill;
            txtMaNV.Location = new Point(217, 15);
            txtMaNV.MaxLength = 50;
            txtMaNV.Name = "txtMaNV";
            txtMaNV.Size = new Size(602, 34);
            txtMaNV.TabIndex = 1;
            // 
            // txtTen
            // 
            txtTen.Dock = DockStyle.Fill;
            txtTen.Location = new Point(217, 70);
            txtTen.MaxLength = 100;
            txtTen.Name = "txtTen";
            txtTen.Size = new Size(602, 34);
            txtTen.TabIndex = 3;
            // 
            // txtMatKhau
            // 
            txtMatKhau.Dock = DockStyle.Fill;
            txtMatKhau.Location = new Point(217, 125);
            txtMatKhau.Name = "txtMatKhau";
            txtMatKhau.Size = new Size(602, 34);
            txtMatKhau.TabIndex = 5;
            txtMatKhau.UseSystemPasswordChar = true;
            // 
            // txtXacNhan
            // 
            txtXacNhan.Dock = DockStyle.Fill;
            txtXacNhan.Location = new Point(217, 180);
            txtXacNhan.Name = "txtXacNhan";
            txtXacNhan.Size = new Size(602, 34);
            txtXacNhan.TabIndex = 7;
            txtXacNhan.UseSystemPasswordChar = true;
            // 
            // btnDangKy
            // 
            btnDangKy.BackColor = Color.SeaGreen;
            btnDangKy.Dock = DockStyle.Fill;
            btnDangKy.ForeColor = Color.White;
            btnDangKy.Location = new Point(217, 235);
            btnDangKy.Name = "btnDangKy";
            btnDangKy.Size = new Size(602, 91);
            btnDangKy.TabIndex = 9;
            btnDangKy.Text = "Đăng ký";
            btnDangKy.UseVisualStyleBackColor = false;
            // 
            // btnHuy
            // 
            btnHuy.Dock = DockStyle.Fill;
            btnHuy.Location = new Point(27, 235);
            btnHuy.Name = "btnHuy";
            btnHuy.Size = new Size(184, 91);
            btnHuy.TabIndex = 8;
            btnHuy.Text = "Hủy";
            // 
            // bang
            // 
            bang.ColumnCount = 2;
            bang.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 190F));
            bang.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            bang.Controls.Add(txtMaNV, 1, 0);
            bang.Controls.Add(txtTen, 1, 1);
            bang.Controls.Add(txtMatKhau, 1, 2);
            bang.Controls.Add(txtXacNhan, 1, 3);
            bang.Controls.Add(btnHuy, 0, 4);
            bang.Controls.Add(btnDangKy, 1, 4);
            bang.Controls.Add(label1, 0, 0);
            bang.Controls.Add(label2, 0, 1);
            bang.Controls.Add(label3, 0, 2);
            bang.Controls.Add(label4, 0, 3);
            bang.Dock = DockStyle.Fill;
            bang.Location = new Point(0, 53);
            bang.Name = "bang";
            bang.Padding = new Padding(24, 12, 24, 18);
            bang.RowCount = 5;
            bang.RowStyles.Add(new RowStyle(SizeType.Absolute, 55F));
            bang.RowStyles.Add(new RowStyle(SizeType.Absolute, 55F));
            bang.RowStyles.Add(new RowStyle(SizeType.Absolute, 55F));
            bang.RowStyles.Add(new RowStyle(SizeType.Absolute, 55F));
            bang.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
            bang.Size = new Size(846, 347);
            bang.TabIndex = 0;
            // 
            // tieuDe
            // 
            tieuDe.Dock = DockStyle.Top;
            tieuDe.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            tieuDe.Location = new Point(0, 0);
            tieuDe.Name = "tieuDe";
            tieuDe.Size = new Size(846, 53);
            tieuDe.TabIndex = 1;
            tieuDe.Text = "ĐĂNG KÝ TÀI KHOẢN NHÂN VIÊN";
            tieuDe.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(27, 12);
            label1.Name = "label1";
            label1.Size = new Size(133, 28);
            label1.TabIndex = 10;
            label1.Text = "Mã nhân viên:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(27, 67);
            label2.Name = "label2";
            label2.Size = new Size(75, 28);
            label2.TabIndex = 11;
            label2.Text = "Họ tên:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(27, 122);
            label3.Name = "label3";
            label3.Size = new Size(98, 28);
            label3.TabIndex = 12;
            label3.Text = "Mật khẩu:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(27, 177);
            label4.Name = "label4";
            label4.Size = new Size(181, 28);
            label4.TabIndex = 13;
            label4.Text = "Xác nhận mật khẩu:";
            // 
            // FormDangKy
            // 
            ClientSize = new Size(846, 400);
            Controls.Add(bang);
            Controls.Add(tieuDe);
            Font = new Font("Segoe UI", 10F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "FormDangKy";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Đăng ký nhân viên";
            bang.ResumeLayout(false);
            bang.PerformLayout();
            ResumeLayout(false);
        }

        private TableLayoutPanel bang;
        private Label tieuDe;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
    }
}
