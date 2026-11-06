using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyChamCongServer
{
    public partial class Form1 : Form
    {
        private ServerManager server = new ServerManager();

        public Form1()
        {
            InitializeComponent();

            server.CoNhatKy += (noiDung) =>
            {
                if (IsDisposed)
                    return;

                if (InvokeRequired)
                {
                    BeginInvoke(new Action(() =>
                    {
                        lstNhatKy.Items.Add(noiDung);
                    }));
                }
                else
                {
                    lstNhatKy.Items.Add(noiDung);
                }
            };
        }

        private async void btnBatDau_Click(object sender, EventArgs e)
        {
            if (server.DangChay)
                return;

            btnBatDau.Enabled = false;
            lblTrangThai.Text = "Trạng thái: Đang chạy";
            lstNhatKy.Items.Add("Đang khởi động Server...");
            lstNhatKy.Items.Add("Đã nhấn nút Bắt đầu.");

            try
            {
                await Task.Run(() => server.BatDauAsync());
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể khởi động Server: " + ex.Message);
            }
            finally
            {
                btnBatDau.Enabled = true;
                lblTrangThai.Text = "Trạng thái: Đang dừng";
            }
        }

        private void btnDung_Click(object sender, EventArgs e)
        {
            server.DungServer();
            lblTrangThai.Text = "Trạng thái: Đang dừng";
            lstNhatKy.Items.Add("Đã gửi yêu cầu dừng Server.");
        }

        
    }
}