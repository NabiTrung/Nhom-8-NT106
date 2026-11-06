using System.Drawing;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using Font = System.Drawing.Font;

namespace ClientNhanVien
{
    partial class NhanVienForm
    {
        private System.ComponentModel.IContainer? components = null;
        private Label lblThongTin = null!;
        private Label lblDongHo = null!;
        private Button btnCheckIn = null!;
        private Button btnCheckOut = null!;
        private Button btnTaiLai = null!;
        private DataGridView dgvLichSu = null!;

        protected override void Dispose(bool disposing)
        {
            if (disposing) components?.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblThongTin = new Label();
            lblDongHo = new Label();
            btnCheckIn = new Button();
            btnCheckOut = new Button();
            btnTaiLai = new Button();
            dgvLichSu = new DataGridView();
            var bangNut = new TableLayoutPanel();
            var noiDung = new Panel();
            ((System.ComponentModel.ISupportInitialize)dgvLichSu).BeginInit();
            SuspendLayout();

            Text = "Chấm công nhân viên";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(900, 590);
            MinimumSize = new Size(740, 480);
            Font = new Font("Segoe UI", 10F);

            lblThongTin.Dock = DockStyle.Top;
            lblThongTin.Height = 44;
            lblThongTin.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblThongTin.TextAlign = ContentAlignment.MiddleCenter;
            lblThongTin.Text = "Thông tin nhân viên";

            lblDongHo.Dock = DockStyle.Top;
            lblDongHo.Height = 62;
            lblDongHo.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblDongHo.TextAlign = ContentAlignment.MiddleCenter;

            bangNut.Dock = DockStyle.Top;
            bangNut.Height = 96;
            bangNut.Padding = new Padding(12);
            bangNut.ColumnCount = 2;
            bangNut.RowCount = 1;
            bangNut.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            bangNut.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            bangNut.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            btnCheckIn.Text = "CHECK IN";
            btnCheckIn.BackColor = Color.SeaGreen;
            btnCheckIn.ForeColor = Color.White;
            btnCheckIn.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            btnCheckIn.Dock = DockStyle.Fill;
            btnCheckIn.FlatStyle = FlatStyle.Flat;

            btnCheckOut.Text = "CHECK OUT";
            btnCheckOut.BackColor = Color.Firebrick;
            btnCheckOut.ForeColor = Color.White;
            btnCheckOut.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            btnCheckOut.Dock = DockStyle.Fill;
            btnCheckOut.FlatStyle = FlatStyle.Flat;

            bangNut.Controls.Add(btnCheckIn, 0, 0);
            bangNut.Controls.Add(btnCheckOut, 1, 0);

            btnTaiLai.Dock = DockStyle.Top;
            btnTaiLai.Height = 38;
            btnTaiLai.Text = "Tải lại lịch sử chấm công";

            dgvLichSu.Dock = DockStyle.Fill;
            dgvLichSu.ReadOnly = true;
            dgvLichSu.AllowUserToAddRows = false;
            dgvLichSu.AllowUserToDeleteRows = false;
            dgvLichSu.AutoGenerateColumns = false;
            dgvLichSu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvLichSu.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLichSu.RowHeadersVisible = false;
            dgvLichSu.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "NgayChamCong", HeaderText = "Ngày", Name = "colNgay" });
            dgvLichSu.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "GioVao", HeaderText = "Giờ vào", Name = "colVao" });
            dgvLichSu.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "GioRa", HeaderText = "Giờ ra", Name = "colRa" });
            dgvLichSu.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "IP", HeaderText = "IP", Name = "colIP" });
            dgvLichSu.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TrangThai", HeaderText = "Trạng thái", Name = "colTrangThai" });

            noiDung.Dock = DockStyle.Fill;
            noiDung.Padding = new Padding(12);
            noiDung.Controls.Add(dgvLichSu);
            noiDung.Controls.Add(btnTaiLai);

            Controls.Add(noiDung);
            Controls.Add(bangNut);
            Controls.Add(lblDongHo);
            Controls.Add(lblThongTin);
            ((System.ComponentModel.ISupportInitialize)dgvLichSu).EndInit();
            ResumeLayout(false);
        }
    }
}
