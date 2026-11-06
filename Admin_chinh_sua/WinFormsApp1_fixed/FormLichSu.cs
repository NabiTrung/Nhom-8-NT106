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
    public partial class FormLichSu : Form
    {
        [System.ComponentModel.DesignerSerializationVisibility(
    System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public string MaNV { get; set; }

        public FormLichSu()
        {
            InitializeComponent();
        }

        public FormLichSu(string maNV)
        {
            InitializeComponent();
            MaNV = maNV;
        }

        private void btnTaiLichSu_Click(object sender, EventArgs e)
        {
            try
            {
                using (TcpClient client = new TcpClient())
                {
                    client.Connect("127.0.0.1", 8888);

                    using (NetworkStream stream = client.GetStream())
                    using (StreamWriter writer =
                        new StreamWriter(stream, Encoding.UTF8))
                    using (StreamReader reader =
                        new StreamReader(stream, Encoding.UTF8))
                    {
                        writer.AutoFlush = true;
                        writer.WriteLine("HISTORY|" + MaNV);

                        string response = reader.ReadLine();

                        if (string.IsNullOrEmpty(response))
                        {
                            MessageBox.Show("Server không trả về dữ liệu.");
                            return;
                        }

                        string[] parts = response.Split('|');

                        if (parts[0] != "SUCCESS" || parts.Length < 2)
                        {
                            MessageBox.Show(parts.Length > 1
                                ? parts[1]
                                : "Không thể tải lịch sử.");
                            return;
                        }

                        DataTable dt = new DataTable();
                        dt.Columns.Add("Ngày chấm công");
                        dt.Columns.Add("Giờ vào");
                        dt.Columns.Add("Giờ ra");
                        dt.Columns.Add("Trạng thái");

                        string[] rows = parts[1].Split(';');

                        foreach (string row in rows)
                        {
                            string[] values = row.Split(',');

                            if (values.Length == 4)
                                dt.Rows.Add(values);
                        }

                        dgvLichSu.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối Server: " + ex.Message);
            }
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnTaiLichSu_Click_1(object sender, EventArgs e)
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
                        writer.WriteLine("HISTORY|" + MaNV);

                        string response = reader.ReadLine();

                        if (string.IsNullOrEmpty(response))
                        {
                            MessageBox.Show("Server không trả về dữ liệu.");
                            return;
                        }

                        string[] parts = response.Split('|');

                        if (parts[0] != "SUCCESS" || parts.Length < 2)
                        {
                            MessageBox.Show(parts.Length > 1
                                ? parts[1]
                                : "Không thể tải lịch sử.");
                            return;
                        }

                        DataTable dt = new DataTable();
                        dt.Columns.Add("Ngày chấm công");
                        dt.Columns.Add("Giờ vào");
                        dt.Columns.Add("Giờ ra");
                        dt.Columns.Add("Trạng thái");

                        string[] rows = parts[1].Split(';');

                        foreach (string row in rows)
                        {
                            string[] values = row.Split(',');

                            if (values.Length == 4)
                            {
                                dt.Rows.Add(values);
                            }
                        }

                        dgvLichSu.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối Server: " + ex.Message);
            }
        }

        private void btnDong_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}