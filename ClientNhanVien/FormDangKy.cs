using ClientNhanVien;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientNhanVien
{
    public partial class FormDangKy : Form
    {
        private readonly string serverIP;

        public FormDangKy() : this("127.0.0.1") { }

        public FormDangKy(string serverIP)
        {
            InitializeComponent();
            this.serverIP = serverIP;
            btnDangKy.Click += async (_, _) => await DangKyAsync();
            btnHuy.Click += (_, _) => Close();
        }

        private async Task DangKyAsync()
        {
            string maNV = txtMaNV.Text.Trim();
            string ten = txtTen.Text.Trim();
            string matKhau = txtMatKhau.Text;
            string xacNhan = txtXacNhan.Text;

            if (string.IsNullOrWhiteSpace(maNV) || string.IsNullOrWhiteSpace(ten) ||
                string.IsNullOrEmpty(matKhau) || string.IsNullOrEmpty(xacNhan))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.");
                return;
            }
            if (matKhau != xacNhan)
            {
                MessageBox.Show("Xác nhận mật khẩu không khớp.");
                return;
            }
            if (matKhau.Length < 8)
            {
                MessageBox.Show("Mật khẩu phải có ít nhất 8 ký tự.");
                return;
            }

            btnDangKy.Enabled = false;
            try
            {
                using var socket = new SocketClient();
                await socket.ConnectAsync(serverIP);
                string yeuCau = $"REGISTER|{SocketClient.Encode(maNV)}|{SocketClient.Encode(ten)}|{SocketClient.Encode(matKhau)}";
                string traLoi = await socket.SendAsync(yeuCau);
                if (traLoi == "REGISTER_OK")
                {
                    MessageBox.Show("Đăng ký thành công!");
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else if (traLoi == "REGISTER_FAIL")
                    MessageBox.Show("Đăng ký thất bại. Có thể mã nhân viên đã tồn tại.");
                else
                    MessageBox.Show("Server phản hồi: " + traLoi);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể đăng ký: " + ex.Message);
            }
            finally
            {
                btnDangKy.Enabled = true;
            }
        }
    }
}
