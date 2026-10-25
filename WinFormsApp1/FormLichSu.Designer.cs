namespace QuanLyChamCong
{
    partial class FormLichSu
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
            dgvLichSu = new DataGridView();
            btnDong = new Button();
            btnTaiLichSu = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvLichSu).BeginInit();
            SuspendLayout();
            // 
            // dgvLichSu
            // 
            dgvLichSu.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvLichSu.Location = new Point(27, 59);
            dgvLichSu.Name = "dgvLichSu";
            dgvLichSu.RowHeadersWidth = 51;
            dgvLichSu.Size = new Size(626, 229);
            dgvLichSu.TabIndex = 0;
            // 
            // btnDong
            // 
            btnDong.Location = new Point(479, 309);
            btnDong.Name = "btnDong";
            btnDong.Size = new Size(174, 51);
            btnDong.TabIndex = 1;
            btnDong.Text = "Đóng";
            btnDong.UseVisualStyleBackColor = true;
            btnDong.Click += btnDong_Click_1;
            // 
            // btnTaiLichSu
            // 
            btnTaiLichSu.Location = new Point(259, 309);
            btnTaiLichSu.Name = "btnTaiLichSu";
            btnTaiLichSu.Size = new Size(174, 51);
            btnTaiLichSu.TabIndex = 2;
            btnTaiLichSu.Text = "Tải Lịch Sử";
            btnTaiLichSu.UseVisualStyleBackColor = true;
            btnTaiLichSu.Click += btnTaiLichSu_Click_1;
            // 
            // FormLichSu
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(914, 489);
            Controls.Add(btnTaiLichSu);
            Controls.Add(btnDong);
            Controls.Add(dgvLichSu);
            Name = "FormLichSu";
            Text = "FormLichSu";
            ((System.ComponentModel.ISupportInitialize)dgvLichSu).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dgvLichSu;
        private Button btnDong;
        private Button btnTaiLichSu;
    }
}