using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Globalization;
using System.Net.Sockets;
using System.Text;
using ClosedXML.Excel;

namespace QuanLyChamCong
{
    /// <summary>
    /// Màn hình quản trị dành cho Admin:
    /// - Lọc dữ liệu chấm công theo khoảng ngày, tìm kiếm theo Mã NV / Tên.
    /// - Tô màu dòng đi muộn, vẽ biểu đồ tròn GDI+ thống kê trạng thái.
    /// - Xuất Excel bằng ClosedXML.
    /// - Kiểm thử đồng thời 5 Client TCP tới Server (cổng 8888).
    /// </summary>
    public partial class FormAdmin : Form
    {
        #region Hằng số & cấu hình

        // Địa chỉ và cổng của Server chấm công
        private const string DiaChiServer = "127.0.0.1";
        private const int CongServer = 8888;

        // Mốc giờ quy định: vào sau 08:00 được tính là đi muộn
        private static readonly TimeSpan GioQuyDinh = new TimeSpan(8, 0, 0);

        // Mã phân loại trạng thái (dùng nội bộ để thống kê)
        private const string LoaiDungGio = "DungGio";
        private const string LoaiDiMuon = "DiMuon";
        private const string LoaiChuaCheckOut = "ChuaCheckOut";

        // Bảng màu giao diện
        private static readonly Color MauNenDiMuon = ColorTranslator.FromHtml("#FFCDD2");
        private static readonly Color MauChuDiMuon = ColorTranslator.FromHtml("#B71C1C");
        private static readonly Color MauChonDiMuon = ColorTranslator.FromHtml("#EF9A9A");
        private static readonly Color MauChuChuaCheckOut = ColorTranslator.FromHtml("#E65100");
        private static readonly Color MauChuDao = ColorTranslator.FromHtml("#1E3A5F");

        // Màu các lát biểu đồ tròn
        private static readonly Color MauBieuDoDungGio = ColorTranslator.FromHtml("#43A047");
        private static readonly Color MauBieuDoDiMuon = ColorTranslator.FromHtml("#E53935");
        private static readonly Color MauBieuDoChuaCheckOut = ColorTranslator.FromHtml("#FFB300");

        #endregion

        #region Trạng thái form

        private readonly string maAdmin;
        private readonly string tenAdmin;

        // Nguồn dữ liệu cho lưới, hỗ trợ lọc (Filter) và sắp xếp (Sort)
        private readonly BindingSource bsChamCong = new BindingSource();

        // Số liệu thống kê của dữ liệu đang hiển thị (sau khi lọc)
        private int soDungGio;
        private int soDiMuon;
        private int soChuaCheckOut;

        // Cờ báo đang có tác vụ chạy nền (tránh bấm trùng)
        private bool dangXuLy;

        // Font in đậm dùng chung cho ô "Chưa check-out" (tạo 1 lần, tránh rò rỉ GDI)
        private readonly Font fontOTrangThai = new Font("Segoe UI", 9.75F, FontStyle.Bold);

        #endregion

        /// <summary>
        /// Hàm khởi tạo mặc định (phục vụ trình thiết kế).
        /// </summary>
        public FormAdmin() : this("", "Quản trị viên")
        {
        }

        /// <summary>
        /// Khởi tạo màn hình quản trị với thông tin Admin đăng nhập.
        /// </summary>
        public FormAdmin(string maNV, string tenNV)
        {
            InitializeComponent();

            maAdmin = maNV ?? "";
            tenAdmin = string.IsNullOrWhiteSpace(tenNV) ? "Quản trị viên" : tenNV;

            CauHinhLuoiDuLieu();
        }

        #region Khởi tạo giao diện

        private async void FormAdmin_Load(object? sender, EventArgs e)
        {
            lblXinChao.Text = string.IsNullOrEmpty(maAdmin)
                ? "Xin chào, " + tenAdmin
                : "Xin chào, " + tenAdmin + " (" + maAdmin + ") — Quyền: Admin";

            // Mặc định: từ đầu tháng hiện tại đến hôm nay
            DateTime homNay = DateTime.Today;
            dtpTuNgay.Value = new DateTime(homNay.Year, homNay.Month, 1);
            dtpDenNgay.Value = homNay;

            // Tự động tải dữ liệu khi mở form
            await TaiDuLieuAsync(hienThongBaoLoi: false);
        }

        /// <summary>
        /// Cấu hình DataGridView: kiểu dáng hiện đại và các cột hiển thị.
        /// </summary>
        private void CauHinhLuoiDuLieu()
        {
            DataGridView luoi = dgvChamCong;

            luoi.AutoGenerateColumns = false;
            luoi.ReadOnly = true;
            luoi.AllowUserToAddRows = false;
            luoi.AllowUserToDeleteRows = false;
            luoi.AllowUserToResizeRows = false;
            luoi.MultiSelect = false;
            luoi.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            luoi.RowHeadersVisible = false;
            luoi.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            luoi.BackgroundColor = Color.White;
            luoi.BorderStyle = BorderStyle.None;
            luoi.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            luoi.GridColor = ColorTranslator.FromHtml("#E0E0E0");
            luoi.RowTemplate.Height = 30;

            // Tiêu đề cột
            luoi.EnableHeadersVisualStyles = false;
            luoi.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            luoi.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            luoi.ColumnHeadersHeight = 38;
            luoi.ColumnHeadersDefaultCellStyle.BackColor = MauChuDao;
            luoi.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            luoi.ColumnHeadersDefaultCellStyle.SelectionBackColor = MauChuDao;
            luoi.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            luoi.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Ô dữ liệu
            luoi.DefaultCellStyle.Font = new Font("Segoe UI", 9.75F);
            luoi.DefaultCellStyle.ForeColor = ColorTranslator.FromHtml("#263238");
            luoi.DefaultCellStyle.SelectionBackColor = ColorTranslator.FromHtml("#BBDEFB");
            luoi.DefaultCellStyle.SelectionForeColor = ColorTranslator.FromHtml("#0D47A1");
            luoi.DefaultCellStyle.Padding = new Padding(4, 0, 4, 0);
            luoi.AlternatingRowsDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#F7F9FC");

            luoi.Columns.Clear();
            ThemCot("colID", "ID", "ID", 55, DataGridViewContentAlignment.MiddleCenter);
            ThemCot("colMaNV", "Mã NV", "MaNV", 90, DataGridViewContentAlignment.MiddleCenter);
            ThemCot("colTen", "Họ tên", "Ten", 180, DataGridViewContentAlignment.MiddleLeft);
            DataGridViewTextBoxColumn cotNgay =
                ThemCot("colNgay", "Ngày", "NgayChamCong", 100, DataGridViewContentAlignment.MiddleCenter);
            cotNgay.DefaultCellStyle.Format = "dd/MM/yyyy";
            ThemCot("colGioVao", "Giờ vào", "GioVao", 85, DataGridViewContentAlignment.MiddleCenter);
            ThemCot("colGioRa", "Giờ ra", "GioRa", 85, DataGridViewContentAlignment.MiddleCenter);
            ThemCot("colIP", "Địa chỉ IP", "IP", 110, DataGridViewContentAlignment.MiddleCenter);
            ThemCot("colTrangThai", "Trạng thái", "TrangThaiHienThi", 120, DataGridViewContentAlignment.MiddleCenter);

            luoi.DataSource = bsChamCong;
        }

        /// <summary>
        /// Thêm một cột văn bản vào lưới dữ liệu.
        /// </summary>
        private DataGridViewTextBoxColumn ThemCot(
            string ten, string tieuDe, string thuocTinh, int trongSo, DataGridViewContentAlignment canLe)
        {
            DataGridViewTextBoxColumn cot = new DataGridViewTextBoxColumn
            {
                Name = ten,
                HeaderText = tieuDe,
                DataPropertyName = thuocTinh,
                FillWeight = trongSo,
                MinimumWidth = 50,
                SortMode = DataGridViewColumnSortMode.Automatic
            };
            cot.DefaultCellStyle.Alignment = canLe;

            dgvChamCong.Columns.Add(cot);
            return cot;
        }

        #endregion

        #region Tải dữ liệu từ Server

        private async void btnTaiDuLieu_Click(object? sender, EventArgs e)
        {
            await TaiDuLieuAsync(hienThongBaoLoi: true);
        }

        /// <summary>
        /// Gửi lệnh ADMIN_GET_ALL tới Server và nạp kết quả vào lưới.
        /// </summary>
        private async Task TaiDuLieuAsync(bool hienThongBaoLoi)
        {
            if (dangXuLy)
                return;

            DateTime tuNgay = dtpTuNgay.Value.Date;
            DateTime denNgay = dtpDenNgay.Value.Date;

            if (tuNgay > denNgay)
            {
                MessageBox.Show(
                    "\"Từ ngày\" không được lớn hơn \"Đến ngày\". Vui lòng chọn lại khoảng thời gian!",
                    "Khoảng ngày không hợp lệ",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            DatTrangThaiXuLy(true, "Đang tải dữ liệu chấm công từ Server...");

            try
            {
                string lenh = TaoLenhLayDuLieu(tuNgay, denNgay);

                using CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(15));
                string phanHoi = await GuiLenhAsync(lenh, cts.Token);

                TachPhanHoi(phanHoi, out string maKetQua, out string noiDung);

                if (maKetQua != "SUCCESS")
                {
                    DatTrangThai("Server báo lỗi: " + noiDung);

                    if (hienThongBaoLoi)
                    {
                        MessageBox.Show(
                            "Không lấy được dữ liệu chấm công.\nServer phản hồi: " + noiDung,
                            "Lỗi từ Server",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                    return;
                }

                DataTable bang = PhanTichDuLieu(noiDung, out int soDongLoi);

                bsChamCong.DataSource = bang;
                bsChamCong.Sort = "NgayChamCong DESC, GioVao DESC";
                ApDungBoLoc();

                string thongBao = "Đã tải " + bang.Rows.Count + " bản ghi ("
                    + tuNgay.ToString("dd/MM/yyyy") + " → " + denNgay.ToString("dd/MM/yyyy") + ")"
                    + " lúc " + DateTime.Now.ToString("HH:mm:ss") + ".";

                if (soDongLoi > 0)
                    thongBao += " Bỏ qua " + soDongLoi + " dòng sai định dạng.";

                DatTrangThai(thongBao);
            }
            catch (OperationCanceledException)
            {
                XuLyLoiKetNoi("Hết thời gian chờ phản hồi từ Server.", hienThongBaoLoi);
            }
            catch (SocketException ex)
            {
                XuLyLoiKetNoi("Không thể kết nối tới Server (" + DiaChiServer + ":" + CongServer + "). "
                    + "Hãy chắc chắn Server đang chạy.\nChi tiết: " + ex.Message, hienThongBaoLoi);
            }
            catch (Exception ex)
            {
                XuLyLoiKetNoi("Lỗi khi tải dữ liệu: " + ex.Message, hienThongBaoLoi);
            }
            finally
            {
                DatTrangThaiXuLy(false);
            }
        }

        /// <summary>
        /// Tạo lệnh lấy dữ liệu theo giao thức: ADMIN_GET_ALL|yyyy-MM-dd|yyyy-MM-dd
        /// </summary>
        private static string TaoLenhLayDuLieu(DateTime tuNgay, DateTime denNgay)
        {
            return "ADMIN_GET_ALL|"
                + tuNgay.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + "|"
                + denNgay.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        private void XuLyLoiKetNoi(string thongDiep, bool hienThongBao)
        {
            DatTrangThai(thongDiep.Replace("\n", " "));

            if (hienThongBao)
            {
                MessageBox.Show(thongDiep, "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Phân tích chuỗi dữ liệu: ID,MaNV,Ten,NgayChamCong,GioVao,GioRa,IP,TrangThai;...
        /// </summary>
        private static DataTable PhanTichDuLieu(string noiDung, out int soDongLoi)
        {
            DataTable bang = new DataTable("ChamCong");
            bang.Columns.Add("ID", typeof(int));
            bang.Columns.Add("MaNV", typeof(string));
            bang.Columns.Add("Ten", typeof(string));
            bang.Columns.Add("NgayChamCong", typeof(DateTime));
            bang.Columns.Add("GioVao", typeof(string));
            bang.Columns.Add("GioRa", typeof(string));
            bang.Columns.Add("IP", typeof(string));
            bang.Columns.Add("TrangThai", typeof(string));
            bang.Columns.Add("PhanLoai", typeof(string));
            bang.Columns.Add("TrangThaiHienThi", typeof(string));
            bang.Columns.Add("LaDiMuon", typeof(bool));

            soDongLoi = 0;

            if (string.IsNullOrWhiteSpace(noiDung))
                return bang;

            string[] cacDong = noiDung.Split(';', StringSplitOptions.RemoveEmptyEntries);

            foreach (string dong in cacDong)
            {
                string[] truong = dong.Split(',');

                if (truong.Length != 8 ||
                    !int.TryParse(truong[0].Trim(), out int id) ||
                    !DateTime.TryParseExact(truong[3].Trim(), "yyyy-MM-dd",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime ngay))
                {
                    soDongLoi++;
                    continue;
                }

                string gioVao = truong[4].Trim();
                string gioRa = truong[5].Trim();
                string trangThai = truong[7].Trim();

                string phanLoai = PhanLoaiTrangThai(trangThai, gioVao, gioRa, out bool laDiMuon);

                bang.Rows.Add(
                    id,
                    truong[1].Trim(),
                    truong[2].Trim(),
                    ngay,
                    gioVao,
                    gioRa,
                    truong[6].Trim(),
                    trangThai,
                    phanLoai,
                    LayTenHienThi(phanLoai),
                    laDiMuon);
            }

            return bang;
        }

        /// <summary>
        /// Phân loại một bản ghi vào đúng 1 trong 3 nhóm (để tổng luôn bằng 100%).
        /// Thứ tự ưu tiên: Đi muộn → Chưa check-out → Đúng giờ.
        /// </summary>
        private static string PhanLoaiTrangThai(string trangThai, string gioVao, string gioRa, out bool laDiMuon)
        {
            // Đi muộn: Server đã đánh dấu DiMuon, hoặc giờ vào thực tế sau 08:00
            laDiMuon = trangThai == LoaiDiMuon ||
                       (TimeSpan.TryParse(gioVao, CultureInfo.InvariantCulture, out TimeSpan vao) &&
                        vao > GioQuyDinh);

            if (laDiMuon)
                return LoaiDiMuon;

            if (string.IsNullOrEmpty(gioRa) || trangThai == LoaiChuaCheckOut)
                return LoaiChuaCheckOut;

            return LoaiDungGio;
        }

        private static string LayTenHienThi(string phanLoai)
        {
            return phanLoai switch
            {
                LoaiDiMuon => "Đi muộn",
                LoaiChuaCheckOut => "Chưa check-out",
                _ => "Đúng giờ"
            };
        }

        #endregion

        #region Tìm kiếm / lọc

        private void txtTimKiem_TextChanged(object? sender, EventArgs e)
        {
            ApDungBoLoc();
        }

        private void txtTimKiem_KeyDown(object? sender, KeyEventArgs e)
        {
            // Nhấn Enter để tải lại dữ liệu, Esc để xoá từ khoá
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                btnTaiDuLieu.PerformClick();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                e.SuppressKeyPress = true;
                txtTimKiem.Clear();
            }
        }

        /// <summary>
        /// Lọc dữ liệu đang hiển thị theo Mã NV hoặc Tên (không phân biệt hoa thường).
        /// </summary>
        private void ApDungBoLoc()
        {
            if (bsChamCong.DataSource is not DataTable)
            {
                CapNhatThongKe();
                return;
            }

            string tuKhoa = txtTimKiem.Text.Trim();

            if (string.IsNullOrEmpty(tuKhoa))
            {
                bsChamCong.RemoveFilter();
            }
            else
            {
                string giaTri = ThoatKyTuLike(tuKhoa);
                bsChamCong.Filter = "MaNV LIKE '%" + giaTri + "%' OR Ten LIKE '%" + giaTri + "%'";
            }

            CapNhatThongKe();
        }

        /// <summary>
        /// Thoát các ký tự đặc biệt trong biểu thức LIKE của DataView để tránh lỗi cú pháp.
        /// </summary>
        private static string ThoatKyTuLike(string giaTri)
        {
            StringBuilder sb = new StringBuilder(giaTri.Length + 8);

            foreach (char c in giaTri)
            {
                switch (c)
                {
                    case '[':
                    case ']':
                    case '*':
                    case '%':
                        sb.Append('[').Append(c).Append(']');
                        break;
                    case '\'':
                        sb.Append("''");
                        break;
                    default:
                        sb.Append(c);
                        break;
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Đếm số bản ghi theo từng nhóm trạng thái (trên dữ liệu đã lọc) và vẽ lại biểu đồ.
        /// </summary>
        private void CapNhatThongKe()
        {
            soDungGio = 0;
            soDiMuon = 0;
            soChuaCheckOut = 0;

            foreach (object? item in bsChamCong.List)
            {
                if (item is not DataRowView drv)
                    continue;

                switch (drv["PhanLoai"] as string)
                {
                    case LoaiDiMuon:
                        soDiMuon++;
                        break;
                    case LoaiChuaCheckOut:
                        soChuaCheckOut++;
                        break;
                    default:
                        soDungGio++;
                        break;
                }
            }

            int tongGoc = (bsChamCong.DataSource as DataTable)?.Rows.Count ?? 0;
            int tongHienThi = bsChamCong.Count;

            lblSoBanGhi.Text = tongHienThi == tongGoc
                ? tongHienThi + " bản ghi"
                : "Hiển thị " + tongHienThi + " / " + tongGoc + " bản ghi";

            pnlChart.Invalidate();
        }

        #endregion

        #region Tô màu dòng trên lưới

        private void dgvChamCong_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.CellStyle is null)
                return;

            if (dgvChamCong.Rows[e.RowIndex].DataBoundItem is not DataRowView drv)
                return;

            bool laDiMuon = drv["LaDiMuon"] is bool b && b;

            if (laDiMuon)
            {
                // Dòng đi muộn (vào sau 08:00): nền đỏ nhạt, chữ đỏ đậm
                e.CellStyle.BackColor = MauNenDiMuon;
                e.CellStyle.ForeColor = MauChuDiMuon;
                e.CellStyle.SelectionBackColor = MauChonDiMuon;
                e.CellStyle.SelectionForeColor = MauChuDiMuon;
            }
            else if (dgvChamCong.Columns[e.ColumnIndex].Name == "colTrangThai" &&
                     (drv["PhanLoai"] as string) == LoaiChuaCheckOut)
            {
                // Nhấn mạnh nhẹ ô trạng thái "Chưa check-out"
                e.CellStyle.ForeColor = MauChuChuaCheckOut;
                e.CellStyle.Font = fontOTrangThai;
            }
        }

        /// <summary>
        /// Giải phóng font dùng chung khi đóng form.
        /// </summary>
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            fontOTrangThai.Dispose();
            base.OnFormClosed(e);
        }

        #endregion

        #region Biểu đồ tròn GDI+

        private void pnlChart_Paint(object? sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            g.Clear(pnlChart.BackColor);

            Rectangle vung = pnlChart.ClientRectangle;
            if (vung.Width < 60 || vung.Height < 60)
                return;

            (string Ten, int SoLuong, Color Mau)[] cacLat =
            {
                ("Đúng giờ", soDungGio, MauBieuDoDungGio),
                ("Đi muộn", soDiMuon, MauBieuDoDiMuon),
                ("Chưa check-out", soChuaCheckOut, MauBieuDoChuaCheckOut)
            };

            int tong = soDungGio + soDiMuon + soChuaCheckOut;

            const int le = 12;
            const int caoDongChuThich = 26;
            int caoChuThich = cacLat.Length * caoDongChuThich + 30;

            // Tính kích thước hình tròn sao cho vừa khung và chừa chỗ cho chú thích
            int duongKinh = Math.Min(vung.Width - 2 * le, vung.Height - caoChuThich - 2 * le);
            duongKinh = Math.Max(duongKinh, 40);

            Rectangle hinhTron = new Rectangle(
                vung.X + (vung.Width - duongKinh) / 2,
                vung.Y + le,
                duongKinh,
                duongKinh);

            using Font fontPhanTram = new Font("Segoe UI", 10F, FontStyle.Bold);
            using Font fontChuThich = new Font("Segoe UI", 9.75F, FontStyle.Regular);
            using Font fontTong = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            using StringFormat canGiua = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            // Bóng đổ nhẹ phía sau hình tròn
            Rectangle bong = hinhTron;
            bong.Offset(3, 4);
            using (SolidBrush coBong = new SolidBrush(Color.FromArgb(28, 0, 0, 0)))
            {
                g.FillEllipse(coBong, bong);
            }

            if (tong == 0)
            {
                // Chưa có dữ liệu: vẽ vòng tròn xám
                using SolidBrush coXam = new SolidBrush(ColorTranslator.FromHtml("#ECEFF1"));
                using SolidBrush coChuXam = new SolidBrush(ColorTranslator.FromHtml("#90A4AE"));
                g.FillEllipse(coXam, hinhTron);
                g.DrawString("Chưa có dữ liệu", fontTong, coChuXam, hinhTron, canGiua);
            }
            else
            {
                float gocBatDau = -90f;
                using Pen butVien = new Pen(Color.White, 2f);

                foreach (var lat in cacLat)
                {
                    if (lat.SoLuong <= 0)
                        continue;

                    float gocQuet = 360f * lat.SoLuong / tong;

                    using (SolidBrush co = new SolidBrush(lat.Mau))
                    {
                        if (gocQuet >= 359.99f)
                        {
                            // Chỉ có 1 nhóm → vẽ hình tròn đầy đủ (tránh đường kẻ thừa)
                            g.FillEllipse(co, hinhTron);
                            g.DrawEllipse(butVien, hinhTron);
                        }
                        else
                        {
                            g.FillPie(co, hinhTron, gocBatDau, gocQuet);
                            g.DrawPie(butVien, hinhTron, gocBatDau, gocQuet);
                        }
                    }

                    // Ghi phần trăm lên lát nếu đủ rộng
                    if (gocQuet >= 20f)
                    {
                        double gocGiua = (gocBatDau + gocQuet / 2f) * Math.PI / 180.0;
                        float banKinh = duongKinh / 2f;
                        float khoangCach = gocQuet >= 359.99f ? 0f : banKinh * 0.62f;

                        PointF tam = new PointF(
                            hinhTron.X + banKinh + (float)(Math.Cos(gocGiua) * khoangCach),
                            hinhTron.Y + banKinh + (float)(Math.Sin(gocGiua) * khoangCach));

                        string nhan = (100.0 * lat.SoLuong / tong).ToString("0.0") + "%";
                        SizeF kichThuoc = g.MeasureString(nhan, fontPhanTram);
                        RectangleF oNhan = new RectangleF(
                            tam.X - kichThuoc.Width / 2f, tam.Y - kichThuoc.Height / 2f,
                            kichThuoc.Width, kichThuoc.Height);

                        using SolidBrush coBongChu = new SolidBrush(Color.FromArgb(90, 0, 0, 0));
                        RectangleF oBong = oNhan;
                        oBong.Offset(1, 1);
                        g.DrawString(nhan, fontPhanTram, coBongChu, oBong, canGiua);
                        g.DrawString(nhan, fontPhanTram, Brushes.White, oNhan, canGiua);
                    }

                    gocBatDau += gocQuet;
                }
            }

            // ===== Chú thích (legend) =====
            int y = hinhTron.Bottom + 18;
            int xTrai = vung.X + le + 8;
            int xPhai = vung.Right - le - 8;

            using SolidBrush coChu = new SolidBrush(ColorTranslator.FromHtml("#37474F"));
            using StringFormat canPhai = new StringFormat
            {
                Alignment = StringAlignment.Far,
                LineAlignment = StringAlignment.Center
            };
            using StringFormat canTrai = new StringFormat
            {
                Alignment = StringAlignment.Near,
                LineAlignment = StringAlignment.Center
            };

            foreach (var lat in cacLat)
            {
                Rectangle oMau = new Rectangle(xTrai, y + (caoDongChuThich - 14) / 2, 14, 14);
                using (GraphicsPath duongBo = TaoHinhChuNhatBoGoc(oMau, 4))
                using (SolidBrush co = new SolidBrush(lat.Mau))
                {
                    g.FillPath(co, duongBo);
                }

                double phanTram = tong == 0 ? 0 : 100.0 * lat.SoLuong / tong;

                RectangleF dong = new RectangleF(xTrai + 22, y, xPhai - xTrai - 22, caoDongChuThich);
                g.DrawString(lat.Ten, fontChuThich, coChu, dong, canTrai);
                g.DrawString(lat.SoLuong + "  (" + phanTram.ToString("0.0") + "%)", fontTong, coChu, dong, canPhai);

                y += caoDongChuThich;
            }

            // Đường kẻ phân cách và dòng tổng
            using (Pen butKe = new Pen(ColorTranslator.FromHtml("#CFD8DC"), 1f))
            {
                g.DrawLine(butKe, xTrai, y + 4, xPhai, y + 4);
            }

            RectangleF dongTong = new RectangleF(xTrai, y + 6, xPhai - xTrai, caoDongChuThich);
            using SolidBrush coChuDao = new SolidBrush(MauChuDao);
            g.DrawString("Tổng cộng", fontTong, coChuDao, dongTong, canTrai);
            g.DrawString(tong + " bản ghi", fontTong, coChuDao, dongTong, canPhai);
        }

        /// <summary>
        /// Tạo đường viền hình chữ nhật bo góc (dùng cho ô màu chú thích).
        /// </summary>
        private static GraphicsPath TaoHinhChuNhatBoGoc(Rectangle hcn, int banKinh)
        {
            int d = banKinh * 2;
            GraphicsPath duong = new GraphicsPath();
            duong.AddArc(hcn.X, hcn.Y, d, d, 180, 90);
            duong.AddArc(hcn.Right - d, hcn.Y, d, d, 270, 90);
            duong.AddArc(hcn.Right - d, hcn.Bottom - d, d, d, 0, 90);
            duong.AddArc(hcn.X, hcn.Bottom - d, d, d, 90, 90);
            duong.CloseFigure();
            return duong;
        }

        #endregion

        #region Xuất Excel (ClosedXML)

        /// <summary>
        /// Một dòng dữ liệu được chụp lại để xuất Excel ở luồng nền.
        /// </summary>
        private sealed record DongXuatExcel(
            int Id, string MaNV, string Ten, DateTime Ngay,
            string GioVao, string GioRa, string IP, string TrangThai, bool LaDiMuon);

        private async void btnXuatExcel_Click(object? sender, EventArgs e)
        {
            if (dangXuLy)
                return;

            // Chụp dữ liệu đang hiển thị (đã lọc + sắp xếp) trên luồng giao diện
            List<DongXuatExcel> duLieu = new List<DongXuatExcel>();

            foreach (object? item in bsChamCong.List)
            {
                if (item is not DataRowView drv)
                    continue;

                duLieu.Add(new DongXuatExcel(
                    (int)drv["ID"],
                    drv["MaNV"] as string ?? "",
                    drv["Ten"] as string ?? "",
                    (DateTime)drv["NgayChamCong"],
                    drv["GioVao"] as string ?? "",
                    drv["GioRa"] as string ?? "",
                    drv["IP"] as string ?? "",
                    drv["TrangThaiHienThi"] as string ?? "",
                    drv["LaDiMuon"] is bool b && b));
            }

            if (duLieu.Count == 0)
            {
                MessageBox.Show(
                    "Không có dữ liệu để xuất. Vui lòng tải dữ liệu trước!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            DateTime tuNgay = dtpTuNgay.Value.Date;
            DateTime denNgay = dtpDenNgay.Value.Date;

            using SaveFileDialog hopThoai = new SaveFileDialog
            {
                Title = "Lưu báo cáo chấm công",
                Filter = "Bảng tính Excel (*.xlsx)|*.xlsx",
                DefaultExt = "xlsx",
                AddExtension = true,
                FileName = "BaoCaoChamCong_" + tuNgay.ToString("yyyyMMdd") + "_" + denNgay.ToString("yyyyMMdd") + ".xlsx"
            };

            if (hopThoai.ShowDialog(this) != DialogResult.OK)
                return;

            string duongDan = hopThoai.FileName;
            string tuKhoa = txtTimKiem.Text.Trim();

            DatTrangThaiXuLy(true, "Đang xuất file Excel...");

            try
            {
                await Task.Run(() => GhiFileExcel(duongDan, duLieu, tuNgay, denNgay, tuKhoa, tenAdmin));

                DatTrangThai("Đã xuất " + duLieu.Count + " bản ghi ra file: " + duongDan);

                DialogResult moFile = MessageBox.Show(
                    "Xuất file Excel thành công!\n" + duongDan + "\n\nBạn có muốn mở file ngay không?",
                    "Xuất Excel thành công",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information);

                if (moFile == DialogResult.Yes)
                {
                    Process.Start(new ProcessStartInfo(duongDan) { UseShellExecute = true });
                }
            }
            catch (IOException ex)
            {
                DatTrangThai("Xuất Excel thất bại.");
                MessageBox.Show(
                    "Không thể ghi file. Có thể file đang được mở bởi chương trình khác.\nChi tiết: " + ex.Message,
                    "Lỗi xuất Excel",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                DatTrangThai("Xuất Excel thất bại.");
                MessageBox.Show(
                    "Đã xảy ra lỗi khi xuất Excel: " + ex.Message,
                    "Lỗi xuất Excel",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                DatTrangThaiXuLy(false);
            }
        }

        /// <summary>
        /// Tạo workbook Excel có tiêu đề, header tô màu, kẻ viền, tô đỏ dòng đi muộn và dòng tổng hợp.
        /// </summary>
        private static void GhiFileExcel(
            string duongDan, List<DongXuatExcel> duLieu,
            DateTime tuNgay, DateTime denNgay, string tuKhoa, string nguoiXuat)
        {
            string[] tieuDeCot =
            {
                "STT", "ID", "Mã NV", "Họ tên", "Ngày", "Giờ vào", "Giờ ra", "Địa chỉ IP", "Trạng thái"
            };
            int soCot = tieuDeCot.Length;

            using XLWorkbook wb = new XLWorkbook();
            IXLWorksheet ws = wb.Worksheets.Add("ChamCong");

            // ----- Tiêu đề báo cáo -----
            IXLRange tieuDe = ws.Range(1, 1, 1, soCot);
            tieuDe.Merge();
            tieuDe.FirstCell().Value = "BÁO CÁO CHẤM CÔNG NHÂN VIÊN";
            tieuDe.Style.Font.Bold = true;
            tieuDe.Style.Font.FontSize = 16;
            tieuDe.Style.Font.FontColor = XLColor.FromHtml("#1E3A5F");
            tieuDe.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Row(1).Height = 28;

            IXLRange phuDe = ws.Range(2, 1, 2, soCot);
            phuDe.Merge();
            string moTa = "Từ ngày " + tuNgay.ToString("dd/MM/yyyy") + " đến ngày " + denNgay.ToString("dd/MM/yyyy")
                + (string.IsNullOrEmpty(tuKhoa) ? "" : "  |  Từ khoá lọc: \"" + tuKhoa + "\"")
                + "  |  Người xuất: " + nguoiXuat
                + "  |  Thời điểm xuất: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            phuDe.FirstCell().Value = moTa;
            phuDe.Style.Font.Italic = true;
            phuDe.Style.Font.FontColor = XLColor.FromHtml("#546E7A");
            phuDe.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            // ----- Dòng tiêu đề cột -----
            const int dongTieuDe = 4;
            for (int c = 0; c < soCot; c++)
            {
                ws.Cell(dongTieuDe, c + 1).Value = tieuDeCot[c];
            }

            IXLRange header = ws.Range(dongTieuDe, 1, dongTieuDe, soCot);
            header.Style.Font.Bold = true;
            header.Style.Font.FontColor = XLColor.White;
            header.Style.Fill.BackgroundColor = XLColor.FromHtml("#1E3A5F");
            header.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            header.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            ws.Row(dongTieuDe).Height = 22;

            // ----- Dữ liệu -----
            int dong = dongTieuDe + 1;
            int soDiMuon = 0, soChuaCheckOut = 0, soDungGio = 0;

            for (int i = 0; i < duLieu.Count; i++, dong++)
            {
                DongXuatExcel d = duLieu[i];

                ws.Cell(dong, 1).Value = i + 1;
                ws.Cell(dong, 2).Value = d.Id;
                ws.Cell(dong, 3).Value = d.MaNV;
                ws.Cell(dong, 4).Value = d.Ten;
                ws.Cell(dong, 5).Value = d.Ngay;
                ws.Cell(dong, 5).Style.DateFormat.Format = "dd/MM/yyyy";
                ws.Cell(dong, 6).Value = d.GioVao;
                ws.Cell(dong, 7).Value = d.GioRa;
                ws.Cell(dong, 8).Value = d.IP;
                ws.Cell(dong, 9).Value = d.TrangThai;

                IXLRange dongDuLieu = ws.Range(dong, 1, dong, soCot);

                if (d.LaDiMuon)
                {
                    soDiMuon++;
                    dongDuLieu.Style.Fill.BackgroundColor = XLColor.FromHtml("#FFCDD2");
                    dongDuLieu.Style.Font.FontColor = XLColor.FromHtml("#B71C1C");
                }
                else if (d.TrangThai == "Chưa check-out")
                {
                    soChuaCheckOut++;
                    ws.Cell(dong, 9).Style.Font.FontColor = XLColor.FromHtml("#E65100");
                    ws.Cell(dong, 9).Style.Font.Bold = true;
                }
                else
                {
                    soDungGio++;
                    if (i % 2 == 1)
                        dongDuLieu.Style.Fill.BackgroundColor = XLColor.FromHtml("#F7F9FC");
                }
            }

            int dongCuoi = dong - 1;

            // Căn giữa các cột ngắn
            ws.Range(dongTieuDe + 1, 1, dongCuoi, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Range(dongTieuDe + 1, 5, dongCuoi, soCot).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            // Kẻ viền toàn bộ bảng
            IXLRange bangDuLieu = ws.Range(dongTieuDe, 1, dongCuoi, soCot);
            bangDuLieu.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
            bangDuLieu.Style.Border.OutsideBorderColor = XLColor.FromHtml("#1E3A5F");
            bangDuLieu.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            bangDuLieu.Style.Border.InsideBorderColor = XLColor.FromHtml("#B0BEC5");

            // ----- Bảng tổng hợp -----
            int dongTongHop = dongCuoi + 2;
            (string Nhan, int GiaTri, string Mau)[] tongHop =
            {
                ("Tổng số bản ghi", duLieu.Count, "#1E3A5F"),
                ("Đúng giờ", soDungGio, "#2E7D32"),
                ("Đi muộn", soDiMuon, "#C62828"),
                ("Chưa check-out", soChuaCheckOut, "#E65100")
            };

            for (int i = 0; i < tongHop.Length; i++)
            {
                int r = dongTongHop + i;
                ws.Cell(r, 3).Value = tongHop[i].Nhan;
                ws.Cell(r, 4).Value = tongHop[i].GiaTri;
                ws.Cell(r, 3).Style.Font.Bold = true;
                ws.Cell(r, 4).Style.Font.Bold = true;
                ws.Cell(r, 4).Style.Font.FontColor = XLColor.FromHtml(tongHop[i].Mau);
                ws.Cell(r, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            }

            IXLRange bangTongHop = ws.Range(dongTongHop, 3, dongTongHop + tongHop.Length - 1, 4);
            bangTongHop.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            bangTongHop.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            bangTongHop.Style.Fill.BackgroundColor = XLColor.FromHtml("#ECEFF1");

            // Tự động co giãn độ rộng cột theo nội dung, cố định dòng tiêu đề cột
            ws.Columns(1, soCot).AdjustToContents();
            ws.SheetView.FreezeRows(dongTieuDe);
            ws.PageSetup.PageOrientation = XLPageOrientation.Landscape;
            ws.PageSetup.FitToPages(1, 0);

            wb.SaveAs(duongDan);
        }

        #endregion

        #region Kiểm thử đa Client (Stress Test)

        /// <summary>
        /// Kết quả của một Client trong bài kiểm thử đồng thời.
        /// </summary>
        private sealed record KetQuaKiemThu(
            int SoThuTu, string Lenh, bool ThanhCong, long ThoiGianMs, int CongCucBo, string PhanHoi);

        private async void btnTest5Client_Click(object? sender, EventArgs e)
        {
            if (dangXuLy)
                return;

            const int soClient = 5;

            DatTrangThaiXuLy(true, "Đang kiểm thử " + soClient + " Client song song...");
            lstNhatKy.Items.Clear();
            GhiNhatKy("▶ Bắt đầu kiểm thử " + soClient + " Client song song → " + DiaChiServer + ":" + CongServer);

            try
            {
                string lenhAdmin = TaoLenhLayDuLieu(dtpTuNgay.Value.Date, dtpDenNgay.Value.Date);

                // "Cổng xuất phát": tất cả Client cùng chờ tín hiệu rồi bắn đồng loạt
                TaskCompletionSource tinHieuXuatPhat =
                    new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

                Task<KetQuaKiemThu>[] cacTacVu = Enumerable.Range(1, soClient)
                    .Select(stt =>
                    {
                        // Client lẻ gửi PING (kiểm tra tầng mạng), Client chẵn gửi ADMIN_GET_ALL (có truy vấn CSDL)
                        string lenh = stt % 2 == 0 ? lenhAdmin : "PING|";
                        return Task.Run(async () =>
                        {
                            await tinHieuXuatPhat.Task.ConfigureAwait(false);
                            return await ChayMotClientAsync(stt, lenh).ConfigureAwait(false);
                        });
                    })
                    .ToArray();

                Stopwatch dongHoTong = Stopwatch.StartNew();
                tinHieuXuatPhat.SetResult();

                KetQuaKiemThu[] ketQua = await Task.WhenAll(cacTacVu);
                dongHoTong.Stop();

                foreach (KetQuaKiemThu kq in ketQua.OrderBy(k => k.SoThuTu))
                {
                    string tenLenh = kq.Lenh.Split('|')[0];
                    GhiNhatKy(
                        (kq.ThanhCong ? "✔" : "✘") +
                        " Client #" + kq.SoThuTu +
                        " [" + tenLenh + "]" +
                        (kq.CongCucBo > 0 ? " cổng " + kq.CongCucBo : "") +
                        " - " + kq.ThoiGianMs + " ms - " + RutGon(kq.PhanHoi, 60));
                }

                int soThanhCong = ketQua.Count(k => k.ThanhCong);
                double tbMs = ketQua.Average(k => k.ThoiGianMs);
                long maxMs = ketQua.Max(k => k.ThoiGianMs);

                string tongKet =
                    "Kết quả: " + soThanhCong + "/" + soClient + " Client thành công.\n" +
                    "Thời gian tổng (song song): " + dongHoTong.ElapsedMilliseconds + " ms\n" +
                    "Thời gian phản hồi trung bình: " + tbMs.ToString("0.0") + " ms\n" +
                    "Thời gian phản hồi lớn nhất: " + maxMs + " ms";

                GhiNhatKy("■ " + tongKet.Replace("\n", " | "));
                DatTrangThai("Kiểm thử đa Client hoàn tất: " + soThanhCong + "/" + soClient + " thành công.");

                MessageBox.Show(
                    tongKet + (soThanhCong == soClient
                        ? "\n\nServer xử lý đồng thời tốt!"
                        : "\n\nCó Client thất bại, xem chi tiết trong khung Nhật ký."),
                    "Kết quả kiểm thử đa Client",
                    MessageBoxButtons.OK,
                    soThanhCong == soClient ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                GhiNhatKy("✘ Lỗi kiểm thử: " + ex.Message);
                DatTrangThai("Kiểm thử đa Client thất bại.");
                MessageBox.Show("Lỗi khi kiểm thử đa Client: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                DatTrangThaiXuLy(false);
            }
        }

        /// <summary>
        /// Một Client độc lập: mở kết nối TCP riêng, gửi lệnh, đo thời gian phản hồi.
        /// Không ném ngoại lệ ra ngoài - mọi lỗi được ghi vào kết quả.
        /// </summary>
        private static async Task<KetQuaKiemThu> ChayMotClientAsync(int soThuTu, string lenh)
        {
            Stopwatch dongHo = Stopwatch.StartNew();

            try
            {
                using CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
                (string phanHoi, int congCucBo) = await GuiLenhChiTietAsync(lenh, cts.Token).ConfigureAwait(false);
                dongHo.Stop();

                bool thanhCong = phanHoi.StartsWith("SUCCESS", StringComparison.Ordinal);
                return new KetQuaKiemThu(soThuTu, lenh, thanhCong, dongHo.ElapsedMilliseconds, congCucBo, phanHoi);
            }
            catch (OperationCanceledException)
            {
                dongHo.Stop();
                return new KetQuaKiemThu(soThuTu, lenh, false, dongHo.ElapsedMilliseconds, 0, "Hết thời gian chờ");
            }
            catch (Exception ex)
            {
                dongHo.Stop();
                return new KetQuaKiemThu(soThuTu, lenh, false, dongHo.ElapsedMilliseconds, 0, "Lỗi: " + ex.Message);
            }
        }

        private void GhiNhatKy(string noiDung)
        {
            lstNhatKy.Items.Add("[" + DateTime.Now.ToString("HH:mm:ss.fff") + "] " + noiDung);
            lstNhatKy.TopIndex = Math.Max(0, lstNhatKy.Items.Count - 1);
        }

        private static string RutGon(string chuoi, int doDaiToiDa)
        {
            if (string.IsNullOrEmpty(chuoi) || chuoi.Length <= doDaiToiDa)
                return chuoi;

            return chuoi.Substring(0, doDaiToiDa) + "...";
        }

        #endregion

        #region Giao tiếp TCP

        /// <summary>
        /// Gửi một lệnh tới Server và trả về dòng phản hồi.
        /// </summary>
        private static async Task<string> GuiLenhAsync(string lenh, CancellationToken ct)
        {
            (string phanHoi, _) = await GuiLenhChiTietAsync(lenh, ct).ConfigureAwait(false);
            return phanHoi;
        }

        /// <summary>
        /// Gửi lệnh qua một kết nối TCP mới (mỗi lệnh 1 kết nối, đúng giao thức Server hiện tại).
        /// Trả về phản hồi và cổng cục bộ của kết nối (để phân biệt các Client).
        /// </summary>
        private static async Task<(string PhanHoi, int CongCucBo)> GuiLenhChiTietAsync(string lenh, CancellationToken ct)
        {
            using TcpClient client = new TcpClient();
            await client.ConnectAsync(DiaChiServer, CongServer, ct).ConfigureAwait(false);

            int congCucBo = (client.Client.LocalEndPoint as System.Net.IPEndPoint)?.Port ?? 0;

            using NetworkStream stream = client.GetStream();
            using StreamWriter writer = new StreamWriter(stream, new UTF8Encoding(false)) { AutoFlush = true };
            using StreamReader reader = new StreamReader(stream, Encoding.UTF8);

            await writer.WriteLineAsync(lenh.AsMemory(), ct).ConfigureAwait(false);

            string? phanHoi = await reader.ReadLineAsync(ct).ConfigureAwait(false);

            if (phanHoi == null)
                throw new IOException("Server đã đóng kết nối mà không phản hồi.");

            return (phanHoi, congCucBo);
        }

        /// <summary>
        /// Tách phản hồi "MA|NoiDung" thành mã kết quả và nội dung.
        /// </summary>
        private static void TachPhanHoi(string phanHoi, out string maKetQua, out string noiDung)
        {
            int viTri = phanHoi.IndexOf('|');

            if (viTri < 0)
            {
                maKetQua = phanHoi.Trim();
                noiDung = "";
                return;
            }

            maKetQua = phanHoi.Substring(0, viTri).Trim();
            noiDung = phanHoi.Substring(viTri + 1);
        }

        #endregion

        #region Tiện ích giao diện

        /// <summary>
        /// Bật/tắt trạng thái "đang xử lý": khoá các nút và đổi con trỏ chuột.
        /// </summary>
        private void DatTrangThaiXuLy(bool dangBan, string? thongDiep = null)
        {
            dangXuLy = dangBan;

            btnTaiDuLieu.Enabled = !dangBan;
            btnXuatExcel.Enabled = !dangBan;
            btnTest5Client.Enabled = !dangBan;
            dtpTuNgay.Enabled = !dangBan;
            dtpDenNgay.Enabled = !dangBan;

            UseWaitCursor = dangBan;

            if (thongDiep != null)
                DatTrangThai(thongDiep);
        }

        private void DatTrangThai(string thongDiep)
        {
            lblTrangThai.Text = thongDiep;
        }

        private void btnDangXuat_Click(object? sender, EventArgs e)
        {
            DialogResult xacNhan = MessageBox.Show(
                "Bạn có chắc chắn muốn đăng xuất khỏi trang quản trị?",
                "Xác nhận đăng xuất",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (xacNhan == DialogResult.Yes)
            {
                // Đóng form → quay về màn hình đăng nhập (Form1 đang chờ ShowDialog)
                Close();
            }
        }

        #endregion
    }

    /// <summary>
    /// Panel vẽ biểu đồ có bật Double Buffering để chống nhấp nháy
    /// và tự vẽ lại khi thay đổi kích thước.
    /// </summary>
    [DesignerCategory("Code")]
    public class BieuDoPanel : Panel
    {
        public BieuDoPanel()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw,
                true);
            UpdateStyles();
        }
    }
}
