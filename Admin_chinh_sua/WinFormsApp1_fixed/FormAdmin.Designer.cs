namespace QuanLyChamCong
{
    partial class FormAdmin
    {
        /// <summary>
        /// Biến bắt buộc của trình thiết kế.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Giải phóng tài nguyên đang sử dụng.
        /// </summary>
        /// <param name="disposing">true nếu cần giải phóng tài nguyên managed; ngược lại là false.</param>
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
        /// Phương thức bắt buộc cho trình thiết kế - không chỉnh sửa
        /// nội dung phương thức này bằng trình soạn thảo mã.
        /// </summary>
        private void InitializeComponent()
        {
            pnlHeader = new Panel();
            btnDangXuat = new Button();
            lblXinChao = new Label();
            lblTieuDe = new Label();
            flpBoLoc = new FlowLayoutPanel();
            lblTuNgay = new Label();
            dtpTuNgay = new DateTimePicker();
            lblDenNgay = new Label();
            dtpDenNgay = new DateTimePicker();
            lblTimKiem = new Label();
            txtTimKiem = new TextBox();
            btnTaiDuLieu = new Button();
            btnXuatExcel = new Button();
            btnTest5Client = new Button();
            tlpNoiDung = new TableLayoutPanel();
            dgvChamCong = new DataGridView();
            tlpThongKe = new TableLayoutPanel();
            grpBieuDo = new GroupBox();
            pnlChart = new BieuDoPanel();
            grpNhatKy = new GroupBox();
            lstNhatKy = new ListBox();
            ssTrangThai = new StatusStrip();
            lblTrangThai = new ToolStripStatusLabel();
            lblSoBanGhi = new ToolStripStatusLabel();
            pnlHeader.SuspendLayout();
            flpBoLoc.SuspendLayout();
            tlpNoiDung.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvChamCong).BeginInit();
            tlpThongKe.SuspendLayout();
            grpBieuDo.SuspendLayout();
            grpNhatKy.SuspendLayout();
            ssTrangThai.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(30, 58, 95);
            pnlHeader.Controls.Add(btnDangXuat);
            pnlHeader.Controls.Add(lblXinChao);
            pnlHeader.Controls.Add(lblTieuDe);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1280, 68);
            pnlHeader.TabIndex = 0;
            // 
            // btnDangXuat
            // 
            btnDangXuat.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnDangXuat.BackColor = Color.FromArgb(229, 57, 53);
            btnDangXuat.Cursor = Cursors.Hand;
            btnDangXuat.FlatAppearance.BorderSize = 0;
            btnDangXuat.FlatStyle = FlatStyle.Flat;
            btnDangXuat.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            btnDangXuat.ForeColor = Color.White;
            btnDangXuat.Location = new Point(1152, 17);
            btnDangXuat.Name = "btnDangXuat";
            btnDangXuat.Size = new Size(112, 34);
            btnDangXuat.TabIndex = 2;
            btnDangXuat.Text = "Đăng xuất";
            btnDangXuat.UseVisualStyleBackColor = false;
            btnDangXuat.Click += btnDangXuat_Click;
            // 
            // lblXinChao
            // 
            lblXinChao.AutoSize = true;
            lblXinChao.Font = new Font("Segoe UI", 9.75F);
            lblXinChao.ForeColor = Color.FromArgb(207, 216, 220);
            lblXinChao.Location = new Point(22, 40);
            lblXinChao.Name = "lblXinChao";
            lblXinChao.Size = new Size(160, 17);
            lblXinChao.TabIndex = 1;
            lblXinChao.Text = "Xin chào, Quản trị viên";
            // 
            // lblTieuDe
            // 
            lblTieuDe.AutoSize = true;
            lblTieuDe.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold);
            lblTieuDe.ForeColor = Color.White;
            lblTieuDe.Location = new Point(18, 8);
            lblTieuDe.Name = "lblTieuDe";
            lblTieuDe.Size = new Size(460, 30);
            lblTieuDe.TabIndex = 0;
            lblTieuDe.Text = "BẢNG ĐIỀU KHIỂN QUẢN TRỊ CHẤM CÔNG";
            // 
            // flpBoLoc
            // 
            flpBoLoc.AutoSize = true;
            flpBoLoc.BackColor = Color.White;
            flpBoLoc.Controls.Add(lblTuNgay);
            flpBoLoc.Controls.Add(dtpTuNgay);
            flpBoLoc.Controls.Add(lblDenNgay);
            flpBoLoc.Controls.Add(dtpDenNgay);
            flpBoLoc.Controls.Add(lblTimKiem);
            flpBoLoc.Controls.Add(txtTimKiem);
            flpBoLoc.Controls.Add(btnTaiDuLieu);
            flpBoLoc.Controls.Add(btnXuatExcel);
            flpBoLoc.Controls.Add(btnTest5Client);
            flpBoLoc.Dock = DockStyle.Top;
            flpBoLoc.Location = new Point(0, 68);
            flpBoLoc.Name = "flpBoLoc";
            flpBoLoc.Padding = new Padding(12, 10, 12, 10);
            flpBoLoc.Size = new Size(1280, 58);
            flpBoLoc.TabIndex = 1;
            // 
            // lblTuNgay
            // 
            lblTuNgay.AutoSize = true;
            lblTuNgay.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            lblTuNgay.ForeColor = Color.FromArgb(55, 71, 79);
            lblTuNgay.Margin = new Padding(3, 10, 3, 0);
            lblTuNgay.Name = "lblTuNgay";
            lblTuNgay.TabIndex = 0;
            lblTuNgay.Text = "Từ ngày:";
            // 
            // dtpTuNgay
            // 
            dtpTuNgay.CustomFormat = "dd/MM/yyyy";
            dtpTuNgay.Font = new Font("Segoe UI", 9.75F);
            dtpTuNgay.Format = DateTimePickerFormat.Custom;
            dtpTuNgay.Margin = new Padding(3, 6, 15, 3);
            dtpTuNgay.Name = "dtpTuNgay";
            dtpTuNgay.Size = new Size(125, 25);
            dtpTuNgay.TabIndex = 1;
            // 
            // lblDenNgay
            // 
            lblDenNgay.AutoSize = true;
            lblDenNgay.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            lblDenNgay.ForeColor = Color.FromArgb(55, 71, 79);
            lblDenNgay.Margin = new Padding(3, 10, 3, 0);
            lblDenNgay.Name = "lblDenNgay";
            lblDenNgay.TabIndex = 2;
            lblDenNgay.Text = "Đến ngày:";
            // 
            // dtpDenNgay
            // 
            dtpDenNgay.CustomFormat = "dd/MM/yyyy";
            dtpDenNgay.Font = new Font("Segoe UI", 9.75F);
            dtpDenNgay.Format = DateTimePickerFormat.Custom;
            dtpDenNgay.Margin = new Padding(3, 6, 15, 3);
            dtpDenNgay.Name = "dtpDenNgay";
            dtpDenNgay.Size = new Size(125, 25);
            dtpDenNgay.TabIndex = 3;
            // 
            // lblTimKiem
            // 
            lblTimKiem.AutoSize = true;
            lblTimKiem.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            lblTimKiem.ForeColor = Color.FromArgb(55, 71, 79);
            lblTimKiem.Margin = new Padding(3, 10, 3, 0);
            lblTimKiem.Name = "lblTimKiem";
            lblTimKiem.TabIndex = 4;
            lblTimKiem.Text = "Tìm kiếm:";
            // 
            // txtTimKiem
            // 
            txtTimKiem.Font = new Font("Segoe UI", 9.75F);
            txtTimKiem.Margin = new Padding(3, 6, 15, 3);
            txtTimKiem.Name = "txtTimKiem";
            txtTimKiem.PlaceholderText = "Nhập mã NV hoặc tên...";
            txtTimKiem.Size = new Size(220, 25);
            txtTimKiem.TabIndex = 5;
            txtTimKiem.TextChanged += txtTimKiem_TextChanged;
            txtTimKiem.KeyDown += txtTimKiem_KeyDown;
            // 
            // btnTaiDuLieu
            // 
            btnTaiDuLieu.BackColor = Color.FromArgb(25, 118, 210);
            btnTaiDuLieu.Cursor = Cursors.Hand;
            btnTaiDuLieu.FlatAppearance.BorderSize = 0;
            btnTaiDuLieu.FlatStyle = FlatStyle.Flat;
            btnTaiDuLieu.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            btnTaiDuLieu.ForeColor = Color.White;
            btnTaiDuLieu.Margin = new Padding(3, 3, 8, 3);
            btnTaiDuLieu.Name = "btnTaiDuLieu";
            btnTaiDuLieu.Size = new Size(120, 34);
            btnTaiDuLieu.TabIndex = 6;
            btnTaiDuLieu.Text = "⟳ Tải dữ liệu";
            btnTaiDuLieu.UseVisualStyleBackColor = false;
            btnTaiDuLieu.Click += btnTaiDuLieu_Click;
            // 
            // btnXuatExcel
            // 
            btnXuatExcel.BackColor = Color.FromArgb(46, 125, 50);
            btnXuatExcel.Cursor = Cursors.Hand;
            btnXuatExcel.FlatAppearance.BorderSize = 0;
            btnXuatExcel.FlatStyle = FlatStyle.Flat;
            btnXuatExcel.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            btnXuatExcel.ForeColor = Color.White;
            btnXuatExcel.Margin = new Padding(3, 3, 8, 3);
            btnXuatExcel.Name = "btnXuatExcel";
            btnXuatExcel.Size = new Size(120, 34);
            btnXuatExcel.TabIndex = 7;
            btnXuatExcel.Text = "Xuất Excel";
            btnXuatExcel.UseVisualStyleBackColor = false;
            btnXuatExcel.Click += btnXuatExcel_Click;
            // 
            // btnTest5Client
            // 
            btnTest5Client.BackColor = Color.FromArgb(106, 27, 154);
            btnTest5Client.Cursor = Cursors.Hand;
            btnTest5Client.FlatAppearance.BorderSize = 0;
            btnTest5Client.FlatStyle = FlatStyle.Flat;
            btnTest5Client.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            btnTest5Client.ForeColor = Color.White;
            btnTest5Client.Margin = new Padding(3, 3, 3, 3);
            btnTest5Client.Name = "btnTest5Client";
            btnTest5Client.Size = new Size(170, 34);
            btnTest5Client.TabIndex = 8;
            btnTest5Client.Text = "Kiểm thử 5 Client";
            btnTest5Client.UseVisualStyleBackColor = false;
            btnTest5Client.Click += btnTest5Client_Click;
            // 
            // tlpNoiDung
            // 
            tlpNoiDung.ColumnCount = 2;
            tlpNoiDung.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpNoiDung.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 380F));
            tlpNoiDung.Controls.Add(dgvChamCong, 0, 0);
            tlpNoiDung.Controls.Add(tlpThongKe, 1, 0);
            tlpNoiDung.Dock = DockStyle.Fill;
            tlpNoiDung.Location = new Point(0, 126);
            tlpNoiDung.Name = "tlpNoiDung";
            tlpNoiDung.Padding = new Padding(10);
            tlpNoiDung.RowCount = 1;
            tlpNoiDung.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpNoiDung.Size = new Size(1280, 612);
            tlpNoiDung.TabIndex = 2;
            // 
            // dgvChamCong
            // 
            dgvChamCong.Dock = DockStyle.Fill;
            dgvChamCong.Location = new Point(13, 13);
            dgvChamCong.Name = "dgvChamCong";
            dgvChamCong.Size = new Size(874, 586);
            dgvChamCong.TabIndex = 0;
            dgvChamCong.CellFormatting += dgvChamCong_CellFormatting;
            // 
            // tlpThongKe
            // 
            tlpThongKe.ColumnCount = 1;
            tlpThongKe.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpThongKe.Controls.Add(grpBieuDo, 0, 0);
            tlpThongKe.Controls.Add(grpNhatKy, 0, 1);
            tlpThongKe.Dock = DockStyle.Fill;
            tlpThongKe.Location = new Point(893, 13);
            tlpThongKe.Name = "tlpThongKe";
            tlpThongKe.RowCount = 2;
            tlpThongKe.RowStyles.Add(new RowStyle(SizeType.Percent, 62F));
            tlpThongKe.RowStyles.Add(new RowStyle(SizeType.Percent, 38F));
            tlpThongKe.Size = new Size(374, 586);
            tlpThongKe.TabIndex = 1;
            // 
            // grpBieuDo
            // 
            grpBieuDo.BackColor = Color.White;
            grpBieuDo.Controls.Add(pnlChart);
            grpBieuDo.Dock = DockStyle.Fill;
            grpBieuDo.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            grpBieuDo.ForeColor = Color.FromArgb(30, 58, 95);
            grpBieuDo.Name = "grpBieuDo";
            grpBieuDo.Padding = new Padding(8);
            grpBieuDo.TabIndex = 0;
            grpBieuDo.TabStop = false;
            grpBieuDo.Text = "Thống kê trạng thái chấm công";
            // 
            // pnlChart
            // 
            pnlChart.BackColor = Color.White;
            pnlChart.Dock = DockStyle.Fill;
            pnlChart.Name = "pnlChart";
            pnlChart.TabIndex = 0;
            pnlChart.Paint += pnlChart_Paint;
            // 
            // grpNhatKy
            // 
            grpNhatKy.BackColor = Color.White;
            grpNhatKy.Controls.Add(lstNhatKy);
            grpNhatKy.Dock = DockStyle.Fill;
            grpNhatKy.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            grpNhatKy.ForeColor = Color.FromArgb(30, 58, 95);
            grpNhatKy.Name = "grpNhatKy";
            grpNhatKy.Padding = new Padding(8);
            grpNhatKy.TabIndex = 1;
            grpNhatKy.TabStop = false;
            grpNhatKy.Text = "Nhật ký kiểm thử đa Client";
            // 
            // lstNhatKy
            // 
            lstNhatKy.BorderStyle = BorderStyle.None;
            lstNhatKy.Dock = DockStyle.Fill;
            lstNhatKy.Font = new Font("Consolas", 9F);
            lstNhatKy.ForeColor = Color.FromArgb(38, 50, 56);
            lstNhatKy.HorizontalScrollbar = true;
            lstNhatKy.IntegralHeight = false;
            lstNhatKy.Name = "lstNhatKy";
            lstNhatKy.TabIndex = 0;
            // 
            // ssTrangThai
            // 
            ssTrangThai.BackColor = Color.FromArgb(236, 239, 241);
            ssTrangThai.Items.AddRange(new ToolStripItem[] { lblTrangThai, lblSoBanGhi });
            ssTrangThai.Location = new Point(0, 738);
            ssTrangThai.Name = "ssTrangThai";
            ssTrangThai.Size = new Size(1280, 22);
            ssTrangThai.TabIndex = 3;
            // 
            // lblTrangThai
            // 
            lblTrangThai.Name = "lblTrangThai";
            lblTrangThai.Spring = true;
            lblTrangThai.Text = "Sẵn sàng.";
            lblTrangThai.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblSoBanGhi
            // 
            lblSoBanGhi.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblSoBanGhi.Name = "lblSoBanGhi";
            lblSoBanGhi.Text = "0 bản ghi";
            // 
            // FormAdmin
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(240, 242, 245);
            ClientSize = new Size(1280, 760);
            Controls.Add(tlpNoiDung);
            Controls.Add(flpBoLoc);
            Controls.Add(pnlHeader);
            Controls.Add(ssTrangThai);
            Font = new Font("Segoe UI", 9.75F);
            MinimumSize = new Size(1100, 640);
            Name = "FormAdmin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Quản trị chấm công - Admin";
            Load += FormAdmin_Load;
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            flpBoLoc.ResumeLayout(false);
            flpBoLoc.PerformLayout();
            tlpNoiDung.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvChamCong).EndInit();
            tlpThongKe.ResumeLayout(false);
            grpBieuDo.ResumeLayout(false);
            grpNhatKy.ResumeLayout(false);
            ssTrangThai.ResumeLayout(false);
            ssTrangThai.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel pnlHeader;
        private Label lblTieuDe;
        private Label lblXinChao;
        private Button btnDangXuat;
        private FlowLayoutPanel flpBoLoc;
        private Label lblTuNgay;
        private DateTimePicker dtpTuNgay;
        private Label lblDenNgay;
        private DateTimePicker dtpDenNgay;
        private Label lblTimKiem;
        private TextBox txtTimKiem;
        private Button btnTaiDuLieu;
        private Button btnXuatExcel;
        private Button btnTest5Client;
        private TableLayoutPanel tlpNoiDung;
        private DataGridView dgvChamCong;
        private TableLayoutPanel tlpThongKe;
        private GroupBox grpBieuDo;
        private BieuDoPanel pnlChart;
        private GroupBox grpNhatKy;
        private ListBox lstNhatKy;
        private StatusStrip ssTrangThai;
        private ToolStripStatusLabel lblTrangThai;
        private ToolStripStatusLabel lblSoBanGhi;
    }
}
