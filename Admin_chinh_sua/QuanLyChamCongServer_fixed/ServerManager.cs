using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data;
namespace QuanLyChamCongServer
{
    public class ServerManager
    {
        private TcpListener listener;
        private CancellationTokenSource cts;

        public bool DangChay { get; private set; }

        public event Action<string> CoNhatKy;

        public async Task BatDauAsync()
        {
            if (DangChay)
                return;

            try
            {
                cts = new CancellationTokenSource();

                listener = new TcpListener(IPAddress.Any, 8888);
                listener.Start();

                DangChay = true;
                CoNhatKy?.Invoke("Server đang lắng nghe cổng 8888.");

                while (!cts.Token.IsCancellationRequested)
                {
                    TcpClient client =
                        await listener.AcceptTcpClientAsync();

                    _ = Task.Run(() => XuLyClient(client));
                }
            }
            catch (ObjectDisposedException)
            {
                // Listener đã được dừng.
            }
            catch (SocketException)
            {
                if (DangChay)
                    CoNhatKy?.Invoke("Lỗi socket khi nhận kết nối.");
            }
            catch (Exception ex)
            {
                CoNhatKy?.Invoke("Lỗi Server: " + ex.Message);
            }
            finally
            {
                DangChay = false;
                CoNhatKy?.Invoke("Server đã dừng.");
            }
        }

        private void XuLyClient(TcpClient client)
        {
            string ip = "Không xác định";

            try
            {
                ip = ((IPEndPoint)client.Client.RemoteEndPoint)
                    .Address.ToString();

                CoNhatKy?.Invoke("Client kết nối: " + ip);

                using (client)
                using (NetworkStream stream = client.GetStream())
                using (StreamReader reader = new StreamReader(
                    stream, Encoding.UTF8))
                using (StreamWriter writer = new StreamWriter(
                    stream, Encoding.UTF8)
                { AutoFlush = true })
                {
                    string message = reader.ReadLine();

                    if (string.IsNullOrWhiteSpace(message))
                        return;

                    Packet packet = Packet.FromMessage(message);

                    if (packet == null)
                    {
                        writer.WriteLine("ERROR|Lenh khong hop le");
                        return;
                    }

                    CoNhatKy?.Invoke(
                        "Nhận lệnh: " + packet.Command + "|" + packet.Data);

                    switch (packet.Command)
                    {
                        case "PING":
                            writer.WriteLine("SUCCESS|Server dang hoat dong");
                            break;

                        case "LOGIN":
                            string[] data = packet.Data.Split('|');

                            if (data.Length != 2)
                            {
                                writer.WriteLine("ERROR|Du lieu dang nhap khong hop le");
                                break;
                            }

                            string maNV = data[0];
                            string matKhau = data[1];

                            try
                            {
                                QuanLyChamCong.DatabaseHelper db =
                                    new QuanLyChamCong.DatabaseHelper();

                                string ten;
                                string chucVu;

                                bool thanhCong = db.DangNhap(
                                    maNV,
                                    matKhau,
                                    out ten,
                                    out chucVu);

                                if (thanhCong)
                                {
                                    writer.WriteLine(
                                        "SUCCESS|" + ten + "|" + chucVu);
                                }
                                else
                                {
                                    writer.WriteLine(
                                        "ERROR|Sai ma nhan vien hoac mat khau");
                                }
                            }
                            catch (Exception ex)
                            {
                                CoNhatKy?.Invoke(
                                    "Loi dang nhap: " + ex.Message);

                                writer.WriteLine(
                                    "ERROR|Loi xu ly dang nhap");
                            }

                            break;

                        case "REGISTER":
                            string[] thongTin = packet.Data.Split('|');

                            if (thongTin.Length != 3)
                            {
                                writer.WriteLine("ERROR|Du lieu dang ky khong hop le");
                                break;
                            }

                            string maDangKy = thongTin[0];
                            string tenDangKy = thongTin[1];
                            string matKhauDangKy = thongTin[2];

                            if (string.IsNullOrWhiteSpace(maDangKy) ||
                                string.IsNullOrWhiteSpace(tenDangKy) ||
                                string.IsNullOrWhiteSpace(matKhauDangKy))
                            {
                                writer.WriteLine("ERROR|Vui long nhap day du thong tin");
                                break;
                            }

                            try
                            {
                                QuanLyChamCong.DatabaseHelper db =
                                    new QuanLyChamCong.DatabaseHelper();

                                string matKhauHash =
                                    QuanLyChamCong.PasswordHelper.BamMatKhau(matKhauDangKy);

                                bool dangKyThanhCong = db.DangKyNhanVien(
                                    maDangKy,
                                    tenDangKy,
                                    matKhauHash);

                                if (dangKyThanhCong)
                                {
                                    writer.WriteLine("SUCCESS|Dang ky thanh cong");
                                }
                                else
                                {
                                    writer.WriteLine(
                                        "ERROR|Dang ky that bai, co the ma nhan vien da ton tai");
                                }
                            }
                            catch (Exception ex)
                            {
                                CoNhatKy?.Invoke("Loi dang ky: " + ex.Message);
                                writer.WriteLine("ERROR|Loi xu ly dang ky");
                            }

                            break;

                        case "CHECKIN":
                            string[] thongTinCheckIn = packet.Data.Split('|');

                            if (thongTinCheckIn.Length != 1 ||
                                string.IsNullOrWhiteSpace(thongTinCheckIn[0]))
                            {
                                writer.WriteLine("ERROR|Ma nhan vien khong hop le");
                                break;
                            }

                            string maNVCheckIn = thongTinCheckIn[0];

                            try
                            {
                                QuanLyChamCong.DatabaseHelper db =
                                    new QuanLyChamCong.DatabaseHelper();

                                bool checkInThanhCong = db.CheckIn(maNVCheckIn, ip);

                                if (checkInThanhCong)
                                {
                                    writer.WriteLine("SUCCESS|Cham cong vao thanh cong");
                                }
                                else
                                {
                                    writer.WriteLine(
                                        "ERROR|Cham cong that bai, co the da cham cong hom nay");
                                }
                            }
                            catch (Exception ex)
                            {
                                CoNhatKy?.Invoke("Loi CHECKIN: " + ex.Message);
                                writer.WriteLine("ERROR|Loi xu ly cham cong vao");
                            }

                            break;

                        case "CHECKOUT":
                            string maNVCheckOut = packet.Data;

                            if (string.IsNullOrWhiteSpace(maNVCheckOut))
                            {
                                writer.WriteLine("ERROR|Ma nhan vien khong hop le");
                                break;
                            }

                            try
                            {
                                QuanLyChamCong.DatabaseHelper db =
                                    new QuanLyChamCong.DatabaseHelper();

                                bool checkOutThanhCong = db.CheckOut(maNVCheckOut);

                                if (checkOutThanhCong)
                                {
                                    writer.WriteLine("SUCCESS|Cham cong ra thanh cong");
                                }
                                else
                                {
                                    writer.WriteLine(
                                        "ERROR|Cham cong ra that bai, co the chua cham cong vao hoac da cham cong ra");
                                }
                            }
                            catch (Exception ex)
                            {
                                CoNhatKy?.Invoke("Loi CHECKOUT: " + ex.Message);
                                writer.WriteLine("ERROR|Loi xu ly cham cong ra");
                            }

                            break;

                        case "HISTORY":
                            {
                                string maNVHistory = packet.Data;

                                if (string.IsNullOrWhiteSpace(maNVHistory))
                                {
                                    writer.WriteLine("ERROR|Ma nhan vien khong hop le");
                                    break;
                                }

                                try
                                {
                                    QuanLyChamCong.DatabaseHelper db =
                                        new QuanLyChamCong.DatabaseHelper();

                                    DataTable dt = db.LayLichSuChamCong(maNVHistory);

                                    if (dt.Rows.Count == 0)
                                    {
                                        writer.WriteLine("ERROR|Chua co lich su cham cong");
                                        break;
                                    }

                                    List<string> rows = new List<string>();

                                    foreach (DataRow row in dt.Rows)
                                    {
                                        string ngay = Convert.ToDateTime(row["NgayChamCong"])
                                            .ToString("yyyy-MM-dd");

                                        string gioVao = row["GioVao"] == DBNull.Value
                                            ? ""
                                            : Convert.ToDateTime(row["GioVao"])
                                                .ToString("HH:mm:ss");

                                        string gioRa = row["GioRa"] == DBNull.Value
                                            ? ""
                                            : Convert.ToDateTime(row["GioRa"])
                                                .ToString("HH:mm:ss");

                                        string trangThai = row["TrangThai"] == DBNull.Value
                                            ? ""
                                            : row["TrangThai"].ToString();

                                        rows.Add(
                                            ngay + "," + gioVao + "," + gioRa + "," + trangThai);
                                    }

                                    writer.WriteLine("SUCCESS|" + string.Join(";", rows));
                                }
                                catch (Exception ex)
                                {
                                    CoNhatKy?.Invoke("Loi HISTORY: " + ex.Message);
                                    writer.WriteLine("ERROR|Loi xu ly lich su cham cong");
                                }

                                break;
                            }
                        case "ADMIN_GET_ALL":
                            {
                                // Dữ liệu dạng: tuNgay|denNgay (định dạng yyyy-MM-dd)
                                string[] khoangNgay = (packet.Data ?? "").Split('|');

                                if (khoangNgay.Length != 2 ||
                                    !DateTime.TryParseExact(khoangNgay[0].Trim(), "yyyy-MM-dd",
                                        System.Globalization.CultureInfo.InvariantCulture,
                                        System.Globalization.DateTimeStyles.None,
                                        out DateTime tuNgay) ||
                                    !DateTime.TryParseExact(khoangNgay[1].Trim(), "yyyy-MM-dd",
                                        System.Globalization.CultureInfo.InvariantCulture,
                                        System.Globalization.DateTimeStyles.None,
                                        out DateTime denNgay))
                                {
                                    writer.WriteLine("ERROR|Khoang ngay khong hop le (yyyy-MM-dd|yyyy-MM-dd)");
                                    break;
                                }

                                try
                                {
                                    QuanLyChamCong.DatabaseHelper db =
                                        new QuanLyChamCong.DatabaseHelper();

                                    DataTable dtAdmin = db.LayTatCaChamCongAdmin(tuNgay, denNgay);

                                    List<string> dongDuLieu = new List<string>();

                                    foreach (DataRow row in dtAdmin.Rows)
                                    {
                                        string ngay = row["NgayChamCong"] == DBNull.Value
                                            ? ""
                                            : Convert.ToDateTime(row["NgayChamCong"])
                                                .ToString("yyyy-MM-dd");

                                        string gioVao = row["GioVao"] == DBNull.Value
                                            ? ""
                                            : Convert.ToDateTime(row["GioVao"])
                                                .ToString("HH:mm:ss");

                                        string gioRa = row["GioRa"] == DBNull.Value
                                            ? ""
                                            : Convert.ToDateTime(row["GioRa"])
                                                .ToString("HH:mm:ss");

                                        dongDuLieu.Add(string.Join(",",
                                            LamSachTruong(row["ID"]),
                                            LamSachTruong(row["MaNV"]),
                                            LamSachTruong(row["Ten"]),
                                            ngay,
                                            gioVao,
                                            gioRa,
                                            LamSachTruong(row["IP"]),
                                            LamSachTruong(row["TrangThai"])));
                                    }

                                    // Trả về SUCCESS| (rỗng) nếu không có bản ghi để Client tự xử lý
                                    writer.WriteLine("SUCCESS|" + string.Join(";", dongDuLieu));
                                }
                                catch (Exception ex)
                                {
                                    CoNhatKy?.Invoke("Loi ADMIN_GET_ALL: " + ex.Message);
                                    writer.WriteLine("ERROR|Loi xu ly lay du lieu cham cong (Admin)");
                                }

                                break;
                            }
                        default:
                            writer.WriteLine("ERROR|Lenh chua duoc ho tro");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                CoNhatKy?.Invoke(
                    "Lỗi xử lý Client " + ip + ": " + ex.Message);
            }
        }

        /// <summary>
        /// Chuyển giá trị ô dữ liệu thành chuỗi an toàn cho giao thức:
        /// loại bỏ các ký tự phân tách ( , ; | ) và xuống dòng để không làm vỡ gói tin.
        /// </summary>
        private static string LamSachTruong(object? giaTri)
        {
            if (giaTri == null || giaTri == DBNull.Value)
                return "";

            string chuoi = giaTri.ToString() ?? "";

            return chuoi
                .Replace(",", " ")
                .Replace(";", " ")
                .Replace("|", " ")
                .Replace("\r", " ")
                .Replace("\n", " ")
                .Trim();
        }

        public void DungServer()
        {
            if (!DangChay)
                return;

            cts?.Cancel();
            listener?.Stop();

            DangChay = false;
            CoNhatKy?.Invoke("Đã gửi yêu cầu dừng Server.");
        }
    }
}