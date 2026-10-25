using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using WinFormsApp1;
namespace QuanLyChamCong
{
    public partial class FormChamCong : Form
    {
        [System.ComponentModel.DesignerSerializationVisibility(
    System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public string MaNV { get; set; }

        [System.ComponentModel.DesignerSerializationVisibility(
            System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public string TenNV { get; set; }

        [System.ComponentModel.DesignerSerializationVisibility(
            System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public string ChucVu { get; set; }
        public FormChamCong()
        {
            InitializeComponent();
        }

        public FormChamCong(string maNV, string tenNV, string chucVu)
        {
            InitializeComponent();

            MaNV = maNV;
            TenNV = tenNV;
            ChucVu = chucVu;
        }

        private void FormChamCong_Load(object sender, EventArgs e)
        {
            lblMaNV.Text = "Mã nhân viên: " + MaNV;
            lblTenNV.Text = "Nhân viên: " + TenNV;
            lblChucVu.Text = "Chức vụ: " + ChucVu;
        }

        private void btnCheckIn_Click(object sender, EventArgs e)
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

                        writer.WriteLine("CHECKIN|" + MaNV);

                        string response = reader.ReadLine();

                        string[] parts = response.Split('|');

                        if (parts.Length >= 2 && parts[0] == "SUCCESS")
                        {
                            MessageBox.Show("Chấm công vào thành công!");
                        }
                        else
                        {
                            MessageBox.Show(
                                parts.Length >= 2
                                ? parts[1]
                                : "Chấm công vào thất bại!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối Server: " + ex.Message);
            }
        }

        private void btnCheckOut_Click(object sender, EventArgs e)
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

                        writer.WriteLine("CHECKOUT|" + MaNV);

                        string response = reader.ReadLine();
                        string[] parts = response.Split('|');

                        if (parts.Length >= 2 && parts[0] == "SUCCESS")
                        {
                            MessageBox.Show("Chấm công ra thành công!");
                        }
                        else
                        {
                            MessageBox.Show(
                                parts.Length >= 2
                                ? parts[1]
                                : "Chấm công ra thất bại!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối Server: " + ex.Message);
            }
        }

        private void btnLichSu_Click(object sender, EventArgs e)
        {
            FormLichSu form = new FormLichSu(MaNV);
            form.ShowDialog();
        }
        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            Form1 form = new Form1();
            form.Show();

            this.Hide();
        }
    }
}