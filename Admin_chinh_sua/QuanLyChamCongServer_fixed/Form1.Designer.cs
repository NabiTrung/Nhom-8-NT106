namespace QuanLyChamCongServer
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
            lblTieuDe = new Label();
            lblTrangThai = new Label();
            btnBatDau = new Button();
            btnDung = new Button();
            lstNhatKy = new ListBox();
            SuspendLayout();
            // 
            // lblTieuDe
            // 
            lblTieuDe.AutoSize = true;
            lblTieuDe.Location = new Point(163, 30);
            lblTieuDe.Name = "lblTieuDe";
            lblTieuDe.Size = new Size(216, 20);
            lblTieuDe.TabIndex = 0;
            lblTieuDe.Text = "SERVER QUẢN LÝ CHẤM CÔNG";
            // 
            // lblTrangThai
            // 
            lblTrangThai.AutoSize = true;
            lblTrangThai.Location = new Point(25, 128);
            lblTrangThai.Name = "lblTrangThai";
            lblTrangThai.Size = new Size(157, 20);
            lblTrangThai.TabIndex = 1;
            lblTrangThai.Text = "Trạng thái: Đang dừng";
            // 
            // btnBatDau
            // 
            btnBatDau.Location = new Point(137, 76);
            btnBatDau.Name = "btnBatDau";
            btnBatDau.Size = new Size(130, 29);
            btnBatDau.TabIndex = 2;
            btnBatDau.Text = "Bắt đầu Server";
            btnBatDau.UseVisualStyleBackColor = true;
            btnBatDau.Click += btnBatDau_Click;
            // 
            // btnDung
            // 
            btnDung.Location = new Point(302, 76);
            btnDung.Name = "btnDung";
            btnDung.Size = new Size(132, 29);
            btnDung.TabIndex = 3;
            btnDung.Text = "Dừng Server";
            btnDung.UseVisualStyleBackColor = true;
            btnDung.Click += btnDung_Click;
            // 
            // lstNhatKy
            // 
            lstNhatKy.FormattingEnabled = true;
            lstNhatKy.Location = new Point(25, 186);
            lstNhatKy.Name = "lstNhatKy";
            lstNhatKy.Size = new Size(242, 104);
            lstNhatKy.TabIndex = 4;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(lstNhatKy);
            Controls.Add(btnDung);
            Controls.Add(btnBatDau);
            Controls.Add(lblTrangThai);
            Controls.Add(lblTieuDe);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTieuDe;
        private Label lblTrangThai;
        private Button btnBatDau;
        private Button btnDung;
        private ListBox lstNhatKy;
    }
}
