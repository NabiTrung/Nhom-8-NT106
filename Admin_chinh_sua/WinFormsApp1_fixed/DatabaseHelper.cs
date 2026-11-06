using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace QuanLyChamCong
{
    public class DatabaseHelper
    {
        private string connectionString =
    @"Data Source=AWGUSTORYA\MSSQLSERVER01;
      Initial Catalog=QuanLyChamCong;
      Integrated Security=True;
      TrustServerCertificate=True;";

        public SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        public bool KiemTraKetNoi()
        {
            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool DangKyNhanVien(
    string maNV,
    string ten,
    string matKhauHash)
        {
            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();

                    string sql = @"INSERT INTO NhanVien
                           (MaNV, Ten, MatKhau, ChucVu)
                           VALUES
                           (@MaNV, @Ten, @MatKhau, @ChucVu)";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaNV", maNV);
                        cmd.Parameters.AddWithValue("@Ten", ten);
                        cmd.Parameters.AddWithValue("@MatKhau", matKhauHash);
                        cmd.Parameters.AddWithValue("@ChucVu", "NhanVien");

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (SqlException)
            {
                return false;
            }
        }
        public bool DangNhap(
    string maNV,
    string matKhau,
    out string ten,
    out string chucVu)
        {
            ten = "";
            chucVu = "";

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                string sql = @"
            SELECT Ten, MatKhau, ChucVu
            FROM NhanVien
            WHERE MaNV = @MaNV";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@MaNV", maNV);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read())
                            return false;

                        string matKhauDaBam =
                            reader["MatKhau"].ToString();

                        if (!PasswordHelper.KiemTraMatKhau(
                            matKhau, matKhauDaBam))
                            return false;

                        ten = reader["Ten"].ToString();
                        chucVu = reader["ChucVu"].ToString();

                        return true;
                    }
                }
            }
        }
        public string ThuDangKy()
        {
            string matKhau = "123456";

            string matKhauHash =
                PasswordHelper.BamMatKhau(matKhau);

            bool ketQua = DangKyNhanVien(
                "NV002",
                "Nguyen Van B",
                matKhauHash
            );

            if (ketQua)
                return "Thêm nhân viên thành công!";

            return "Thêm thất bại! Có thể mã nhân viên đã tồn tại.";
        }
        public bool CheckIn(string maNV, string ip)
        {
            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();

                    // Kiểm tra nhân viên đã chấm công hôm nay chưa
                    string sqlKiemTra = @"
                SELECT COUNT(*)
                FROM ChamCong
                WHERE MaNV = @MaNV
                  AND NgayChamCong = @NgayChamCong";

                    using (SqlCommand cmdKiemTra = new SqlCommand(sqlKiemTra, conn))
                    {
                        cmdKiemTra.Parameters.AddWithValue("@MaNV", maNV);
                        cmdKiemTra.Parameters.AddWithValue(
                            "@NgayChamCong", DateTime.Today);

                        int soBanGhi = (int)cmdKiemTra.ExecuteScalar();

                        if (soBanGhi > 0)
                        {
                            return false;
                        }
                    }

                    // Chưa chấm công hôm nay thì tạo bản ghi mới
                    DateTime gioVao = DateTime.Now;

                    string trangThai =
                        gioVao.TimeOfDay > new TimeSpan(8, 0, 0)
                        ? "DiMuon"
                        : "DungGio";

                    string sqlThem = @"
                INSERT INTO ChamCong
                    (MaNV, NgayChamCong, GioVao, IP, TrangThai)
                VALUES
                    (@MaNV, @NgayChamCong, @GioVao, @IP, @TrangThai)";

                    using (SqlCommand cmdThem = new SqlCommand(sqlThem, conn))
                    {
                        cmdThem.Parameters.AddWithValue("@MaNV", maNV);
                        cmdThem.Parameters.AddWithValue(
                            "@NgayChamCong", DateTime.Today);
                        cmdThem.Parameters.AddWithValue("@GioVao", gioVao);
                        cmdThem.Parameters.AddWithValue("@IP", ip);
                        cmdThem.Parameters.AddWithValue("@TrangThai", trangThai);

                        return cmdThem.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (SqlException)
            {
                return false;
            }
        }
        public bool CheckOut(string maNV)
        {
            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();

                    string sql = @"
                UPDATE ChamCong
                SET GioRa = @GioRa
                WHERE MaNV = @MaNV
                  AND NgayChamCong = @NgayChamCong
                  AND GioVao IS NOT NULL
                  AND GioRa IS NULL";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaNV", maNV);
                        cmd.Parameters.AddWithValue("@NgayChamCong", DateTime.Today);
                        cmd.Parameters.AddWithValue("@GioRa", DateTime.Now);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (SqlException)
            {
                return false;
            }
        }
        public DataTable LayLichSuChamCong(string maNV)
        {
            DataTable bangDuLieu = new DataTable();

            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();

                    string sql = @"
                SELECT NgayChamCong, GioVao, GioRa, IP, TrangThai
                FROM ChamCong
                WHERE MaNV = @MaNV
                ORDER BY NgayChamCong DESC";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaNV", maNV);

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(bangDuLieu);
                        }
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }

            return bangDuLieu;
        }
        public string ThuCheckIn()
        {
            bool ketQua = CheckIn("NV001", "127.0.0.1");

            if (ketQua)
                return "Chấm công vào thành công!";

            return "Chấm công thất bại! Có thể nhân viên đã chấm công hôm nay.";
        }
        public string ThuCheckOut()
        {
            bool ketQua = CheckOut("NV001");

            if (ketQua)
                return "Chấm công ra thành công!";

            return "Chấm công ra thất bại! Có thể chưa chấm công vào hoặc đã chấm công ra.";
        }
        public string ThuLichSuChamCong()
        {
            DataTable bangDuLieu = LayLichSuChamCong("NV001");

            return "Số bản ghi chấm công: " + bangDuLieu.Rows.Count;
        }
        public bool DatLaiMatKhau(string maNV, string matKhauMoi)
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                string sql = @"UPDATE NhanVien
                       SET MatKhau = @MatKhau
                       WHERE MaNV = @MaNV";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue(
                        "@MatKhau",
                        PasswordHelper.BamMatKhau(matKhauMoi)
                    );

                    cmd.Parameters.AddWithValue("@MaNV", maNV);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        /// <summary>
        /// Lấy toàn bộ dữ liệu chấm công (kèm tên nhân viên) trong khoảng ngày
        /// dành cho màn hình quản trị (Admin).
        /// </summary>
        /// <param name="tuNgay">Ngày bắt đầu (lấy phần ngày, bao gồm).</param>
        /// <param name="denNgay">Ngày kết thúc (lấy phần ngày, bao gồm).</param>
        /// <returns>Bảng gồm các cột: ID, MaNV, Ten, NgayChamCong, GioVao, GioRa, IP, TrangThai.</returns>
        public DataTable LayTatCaChamCongAdmin(DateTime tuNgay, DateTime denNgay)
        {
            DataTable bangDuLieu = new DataTable();

            // Đảm bảo tuNgay <= denNgay, nếu người dùng chọn ngược thì đảo lại
            DateTime batDau = tuNgay.Date;
            DateTime ketThuc = denNgay.Date;

            if (batDau > ketThuc)
            {
                (batDau, ketThuc) = (ketThuc, batDau);
            }

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                string sql = @"
                SELECT  cc.ID,
                        cc.MaNV,
                        nv.Ten,
                        cc.NgayChamCong,
                        cc.GioVao,
                        cc.GioRa,
                        cc.IP,
                        cc.TrangThai
                FROM ChamCong cc
                INNER JOIN NhanVien nv ON nv.MaNV = cc.MaNV
                WHERE cc.NgayChamCong BETWEEN @TuNgay AND @DenNgay
                ORDER BY cc.NgayChamCong DESC, cc.GioVao DESC";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    // Dùng kiểu DATE tường minh để khớp với cột NgayChamCong
                    cmd.Parameters.Add("@TuNgay", SqlDbType.Date).Value = batDau;
                    cmd.Parameters.Add("@DenNgay", SqlDbType.Date).Value = ketThuc;

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(bangDuLieu);
                    }
                }
            }

            return bangDuLieu;
        }
    }
}