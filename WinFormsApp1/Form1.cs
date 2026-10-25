using System;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Windows.Forms;
using QuanLyChamCong;
namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnKiemTra_Click(object sender, EventArgs e)
        {
            try
            {
                using (TcpClient client = new TcpClient())
                {
                    client.Connect("127.0.0.1", 8888);

                    using (NetworkStream stream = client.GetStream())
                    using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8))
                    using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                    {
                        writer.AutoFlush = true;

                        writer.WriteLine("PING|");

                        string response = reader.ReadLine();

                        MessageBox.Show(response);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể kết nối Server: " + ex.Message);
            }
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            string maNV = txtMaNV.Text.Trim();
            string matKhau = txtMatKhau.Text;

            if (maNV == "" || matKhau == "")
            {
                MessageBox.Show("Vui lòng nhập mã nhân viên và mật khẩu!");
                return;
            }

            try
            {
                using (TcpClient client = new TcpClient())
                {
                    client.Connect("127.0.0.1", 8888);

                    using (NetworkStream stream = client.GetStream())
                    using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8))
                    using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                    {
                        writer.AutoFlush = true;

                        writer.WriteLine("LOGIN|" + maNV + "|" + matKhau);

                        string response = reader.ReadLine();

                        string[] parts = response.Split('|');

                        if (parts.Length >= 3 && parts[0] == "SUCCESS")
                        {
                            MessageBox.Show("Đăng nhập thành công!\n" +
                                            "Tên: " + parts[1] + "\n" +
                                            "Chức vụ: " + parts[2]);

                            string tenNV = parts[1];
                            string chucVu = parts[2];

                            FormChamCong form = new FormChamCong(
                                maNV, tenNV, chucVu);

                            this.Hide();
                            form.ShowDialog();
                            this.Show();
                        }
                        else
                        {
                            MessageBox.Show(
                                parts.Length >= 2
                                    ? parts[1]
                                    : "Đăng nhập thất bại!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối Server: " + ex.Message);
            }
        }

        private void btnDatLaiMatKhau_Click(object sender, EventArgs e)
        {
            try
            {
                DatabaseHelper db = new DatabaseHelper();

                bool ketQua = db.DatLaiMatKhau("NV002", "123456");

                MessageBox.Show(
                    ketQua ? "Đặt lại mật khẩu thành công!" :
                             "Không tìm thấy nhân viên NV002!"
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnMoDangKy_Click(object sender, EventArgs e)
        {
            FormDangKy form = new FormDangKy();
            form.ShowDialog();
        }
    }
}
