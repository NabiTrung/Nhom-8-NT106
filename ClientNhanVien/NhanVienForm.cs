using ClientNhanVien;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientNhanVien
{
    public partial class NhanVienForm : Form
    {
        private readonly SocketClient? socket;
        private readonly string maNV;
        private readonly System.Windows.Forms.Timer dongHo = new() { Interval = 1000 };

        public NhanVienForm()
        {
            InitializeComponent();
            maNV = "";
            lblThongTin.Text = "Chế độ xem thử - chưa đăng nhập";
            KhoiTaoSuKien();
            btnCheckIn.Enabled = false;
            btnCheckOut.Enabled = false;
            btnTaiLai.Enabled = false;
        }

        public NhanVienForm(SocketClient socket, string maNV, string ten, string chucVu)
        {
            InitializeComponent();
            this.socket = socket ?? throw new ArgumentNullException(nameof(socket));
            this.maNV = maNV;
            lblThongTin.Text = $"Nhân viên: {ten} ({maNV}) - {chucVu}";
            KhoiTaoSuKien();
            Shown += async (_, _) => await TaiLichSuAsync();
        }

        private void KhoiTaoSuKien()
        {
            lblDongHo.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            dongHo.Tick += (_, _) => lblDongHo.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            dongHo.Start();
            btnCheckIn.Click += async (_, _) => await ChamCongAsync("CHECKIN");
            btnCheckOut.Click += async (_, _) => await ChamCongAsync("CHECKOUT");
            btnTaiLai.Click += async (_, _) => await TaiLichSuAsync();
        }

        private async Task ChamCongAsync(string lenh)
        {
            if (socket is null) return;
            btnCheckIn.Enabled = false;
            btnCheckOut.Enabled = false;
            try
            {
                string traLoi = await socket.SendAsync(lenh);
                string thongBao = traLoi switch
                {
                    "CHECKIN_OK" => "CHECK IN thành công!",
                    "CHECKOUT_OK" => "CHECK OUT thành công!",
                    "CHECKIN_FAIL" => "CHECK IN thất bại: có thể bạn đã chấm công hôm nay.",
                    "CHECKOUT_FAIL" => "CHECK OUT thất bại: chưa CHECK IN hoặc đã CHECK OUT.",
                    "NOT_LOGGED_IN" => "Bạn chưa đăng nhập hoặc phiên đăng nhập không hợp lệ.",
                    _ => "Server phản hồi: " + traLoi
                };
                MessageBox.Show(thongBao, "Chấm công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (traLoi is "CHECKIN_OK" or "CHECKOUT_OK")
                    await TaiLichSuAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể gửi yêu cầu đến Server: " + ex.Message);
            }
            finally
            {
                btnCheckIn.Enabled = true;
                btnCheckOut.Enabled = true;
            }
        }

        private async Task TaiLichSuAsync()
        {
            if (socket is null) return;
            btnTaiLai.Enabled = false;
            try
            {
                string traLoi = await socket.SendAsync("HISTORY");
                if (!traLoi.StartsWith("HISTORY_OK|", StringComparison.Ordinal))
                {
                    MessageBox.Show("Không thể tải lịch sử: " + traLoi);
                    return;
                }

                string json = SocketClient.Decode(traLoi["HISTORY_OK|".Length..]);
                List<DongChamCong> lichSu = JsonSerializer.Deserialize<List<DongChamCong>>(json) ?? new();
                dgvLichSu.DataSource = null;
                dgvLichSu.DataSource = lichSu;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải lịch sử: " + ex.Message);
            }
            finally
            {
                btnTaiLai.Enabled = true;
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            dongHo.Stop();
            dongHo.Dispose();
            base.OnFormClosed(e);
        }

        public class DongChamCong
        {
            public string NgayChamCong { get; set; } = "";
            public string GioVao { get; set; } = "";
            public string GioRa { get; set; } = "";
            public string IP { get; set; } = "";
            public string TrangThai { get; set; } = "";
        }
    }
}
