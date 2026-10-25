using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.IO;
namespace QuanLyChamCong
{
    public partial class FormDangKy : Form
    {
        public FormDangKy()
        {
            InitializeComponent();
        }

        private void lblTieuDe_Click(object sender, EventArgs e)
        {

        }

        private void btnDangKy_Click(object sender, EventArgs e)
        {
            string maNV = txtMaNV.Text.Trim();
            string ten = txtTen.Text.Trim();
            string matKhau = txtMatKhau.Text;
            string xacNhan = txtXacNhanMatKhau.Text;

            if (maNV == "" || ten == "" ||
                matKhau == "" || xacNhan == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                return;
            }

            if (matKhau != xacNhan)
            {
                MessageBox.Show("Mật khẩu xác nhận không khớp!");
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

                        writer.WriteLine("REGISTER|" + maNV + "|" + ten + "|" + matKhau);

                        string response = reader.ReadLine();

                        string[] parts = response.Split('|');

                        if (parts.Length >= 2 && parts[0] == "SUCCESS")
                        {
                            MessageBox.Show("Đăng ký thành công!");
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show(
                                parts.Length >= 2
                                ? parts[1]
                                : "Đăng ký thất bại!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối Server: " + ex.Message);
            }
        }
    }
}
