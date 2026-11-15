using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using DTO;

namespace DAL
{
    public class HoaDonDAL
    {
        // 1. Mở bàn: Tạo HoaDon mới (GioBatDau = GetDate(), TrangThai = 0) & đổi BanBida sang 'Đang chơi'
        public bool MoBan(int maBan)
        {
            using (SqlConnection conn = DBConnect.GetConnection())
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();
                try
                {
                    string sqlHD = @"INSERT INTO HoaDon (MaBan, GioBatDau, TienGioChoi, TienDichVu, TrangThai)
                                     VALUES (@MaBan, GETDATE(), 0, 0, 0)";
                    SqlCommand cmdHD = new SqlCommand(sqlHD, conn, trans);
                    cmdHD.Parameters.AddWithValue("@MaBan", maBan);
                    cmdHD.ExecuteNonQuery();

                    string sqlBan = "UPDATE BanBida SET TrangThai = N'Đang chơi' WHERE MaBan = @MaBan AND TrangThai = N'Trống'";
                    SqlCommand cmdBan = new SqlCommand(sqlBan, conn, trans);
                    cmdBan.Parameters.AddWithValue("@MaBan", maBan);

                    if (cmdBan.ExecuteNonQuery() == 0)
                        throw new Exception("Bàn này đang có người chơi hoặc không tồn tại!");

                    trans.Commit();
                    return true;
                }
                catch
                {
                    trans.Rollback();
                    throw;
                }
            }
        }

        // Lấy hóa đơn chưa thanh toán của bàn đang chơi
        public HoaDonDTO GetHoaDonChuaThanhToanByBan(int maBan)
        {
            string sql = @"SELECT hd.MaHD, hd.MaBan, b.TenBan, hd.GioBatDau, hd.GioKetThuc,
                                  hd.TienGioChoi, hd.TienDichVu, hd.TrangThai
                           FROM HoaDon hd
                           JOIN BanBida b ON hd.MaBan = b.MaBan
                           WHERE hd.MaBan = @MaBan AND hd.TrangThai = 0";

            using (SqlConnection conn = DBConnect.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@MaBan", maBan);
                conn.Open();
                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    if (rd.Read())
                    {
                        return new HoaDonDTO
                        {
                            MaHD = (int)rd["MaHD"],
                            MaBan = (int)rd["MaBan"],
                            TenBan = rd["TenBan"].ToString(),
                            GioBatDau = (DateTime)rd["GioBatDau"],
                            GioKetThuc = rd["GioKetThuc"] == DBNull.Value ? (DateTime?)null : (DateTime)rd["GioKetThuc"],
                            TienGioChoi = (decimal)rd["TienGioChoi"],
                            TienDichVu = (decimal)rd["TienDichVu"],
                            TrangThai = (bool)rd["TrangThai"]
                        };
                    }
                }
            }
            return null;
        }

        // 2. Mô hình ngắt kết nối (SqlDataAdapter + DataTable): Tải chi tiết hóa đơn lên DataTable
        public DataTable GetChiTietHoaDonDataTable(int maHD)
        {
            string sql = @"SELECT ct.MaHD, ct.MaDV, dv.TenDV, ct.SoLuong, ct.DonGia,
                                  (ct.SoLuong * ct.DonGia) AS ThanhTien
                           FROM ChiTietHoaDon ct
                           JOIN MenuDichVu dv ON ct.MaDV = dv.MaDV
                           WHERE ct.MaHD = @MaHD";

            using (SqlConnection conn = DBConnect.GetConnection())
            using (SqlDataAdapter da = new SqlDataAdapter(sql, conn))
            {
                da.SelectCommand.Parameters.AddWithValue("@MaHD", maHD);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Cho phép chỉnh sửa cột tính toán trên DataTable ngắt kết nối
                if (dt.Columns.Contains("TenDV")) dt.Columns["TenDV"].ReadOnly = false;
                if (dt.Columns.Contains("ThanhTien")) dt.Columns["ThanhTien"].ReadOnly = false;
                return dt;
            }
        }

        // Đồng bộ thay đổi từ DataTable ngắt kết nối xuống bảng ChiTietHoaDon bằng SqlDataAdapter
        public void LuuChiTietHoaDonDataTable(DataTable dt)
        {
            using (SqlConnection conn = DBConnect.GetConnection())
            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                da.InsertCommand = new SqlCommand(
                    "INSERT INTO ChiTietHoaDon (MaHD, MaDV, SoLuong, DonGia) VALUES (@MaHD, @MaDV, @SoLuong, @DonGia)", conn);
                da.InsertCommand.Parameters.Add("@MaHD", SqlDbType.Int, 4, "MaHD");
                da.InsertCommand.Parameters.Add("@MaDV", SqlDbType.Int, 4, "MaDV");
                da.InsertCommand.Parameters.Add("@SoLuong", SqlDbType.Int, 4, "SoLuong");
                da.InsertCommand.Parameters.Add("@DonGia", SqlDbType.Decimal, 18, "DonGia");

                da.UpdateCommand = new SqlCommand(
                    "UPDATE ChiTietHoaDon SET SoLuong = @SoLuong, DonGia = @DonGia WHERE MaHD = @MaHD AND MaDV = @MaDV", conn);
                da.UpdateCommand.Parameters.Add("@SoLuong", SqlDbType.Int, 4, "SoLuong");
                da.UpdateCommand.Parameters.Add("@DonGia", SqlDbType.Decimal, 18, "DonGia");
                da.UpdateCommand.Parameters.Add("@MaHD", SqlDbType.Int, 4, "MaHD");
                da.UpdateCommand.Parameters.Add("@MaDV", SqlDbType.Int, 4, "MaDV");

                da.DeleteCommand = new SqlCommand(
                    "DELETE FROM ChiTietHoaDon WHERE MaHD = @MaHD AND MaDV = @MaDV", conn);
                da.DeleteCommand.Parameters.Add("@MaHD", SqlDbType.Int, 4, "MaHD");
                da.DeleteCommand.Parameters.Add("@MaDV", SqlDbType.Int, 4, "MaDV");

                da.Update(dt);
            }
        }

        // 3. Thanh toán hóa đơn bằng SqlTransaction
        public bool ThanhToanHoaDon(int maHD, int maBan, DateTime gioKetThuc, decimal tienGioChoi, decimal tienDichVu)
        {
            using (SqlConnection conn = DBConnect.GetConnection())
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();
                try
                {
                    // Kiểm tra tồn kho trước khi trừ (bỏ qua các dịch vụ thuê gậy có TonKho IS NULL)
                    string sqlCheckKho = @"SELECT dv.TenDV, dv.TonKho, ct.SoLuong
                                           FROM ChiTietHoaDon ct
                                           JOIN MenuDichVu dv ON ct.MaDV = dv.MaDV
                                           WHERE ct.MaHD = @MaHD AND dv.TonKho IS NOT NULL AND dv.TonKho < ct.SoLuong";
                    SqlCommand cmdCheck = new SqlCommand(sqlCheckKho, conn, trans);
                    cmdCheck.Parameters.AddWithValue("@MaHD", maHD);
                    using (SqlDataReader rd = cmdCheck.ExecuteReader())
                    {
                        if (rd.Read())
                        {
                            throw new Exception($"Mặt hàng '{rd["TenDV"]}' không đủ tồn kho (tồn: {rd["TonKho"]}, gọi: {rd["SoLuong"]})!");
                        }
                    }

                    // 1. Cập nhật HoaDon: GioKetThuc, TienGioChoi, TienDichVu, TrangThai = 1
                    string sqlHD = @"UPDATE HoaDon
                                     SET GioKetThuc = @GioKetThuc,
                                         TienGioChoi = @TienGioChoi,
                                         TienDichVu = @TienDichVu,
                                         TrangThai = 1
                                     WHERE MaHD = @MaHD";
                    SqlCommand cmdHD = new SqlCommand(sqlHD, conn, trans);
                    cmdHD.Parameters.AddWithValue("@GioKetThuc", gioKetThuc);
                    cmdHD.Parameters.AddWithValue("@TienGioChoi", tienGioChoi);
                    cmdHD.Parameters.AddWithValue("@TienDichVu", tienDichVu);
                    cmdHD.Parameters.AddWithValue("@MaHD", maHD);
                    cmdHD.ExecuteNonQuery();

                    // 2. Trừ TonKho tương ứng trong MenuDichVu (bỏ qua dịch vụ thuê gậy có TonKho IS NULL)
                    string sqlTruKho = @"UPDATE dv
                                         SET dv.TonKho = dv.TonKho - ct.SoLuong
                                         FROM MenuDichVu dv
                                         INNER JOIN ChiTietHoaDon ct ON dv.MaDV = ct.MaDV
                                         WHERE ct.MaHD = @MaHD AND dv.TonKho IS NOT NULL";
                    SqlCommand cmdKho = new SqlCommand(sqlTruKho, conn, trans);
                    cmdKho.Parameters.AddWithValue("@MaHD", maHD);
                    cmdKho.ExecuteNonQuery();

                    // 3. Cập nhật trạng thái BanBida về "Trống"
                    string sqlBan = "UPDATE BanBida SET TrangThai = N'Trống' WHERE MaBan = @MaBan";
                    SqlCommand cmdBan = new SqlCommand(sqlBan, conn, trans);
                    cmdBan.Parameters.AddWithValue("@MaBan", maBan);
                    cmdBan.ExecuteNonQuery();

                    // 4. Commit Transaction
                    trans.Commit();
                    return true;
                }
                catch
                {
                    trans.Rollback();
                    throw;
                }
            }
        }

        // Lấy danh sách lịch sử hóa đơn
        public List<HoaDonDTO> GetAllHoaDon(int trangThaiFilter) // -1: Tất cả, 0: Chưa TT, 1: Đã TT
        {
            var list = new List<HoaDonDTO>();
            string sql = @"SELECT hd.MaHD, hd.MaBan, b.TenBan, hd.GioBatDau, hd.GioKetThuc,
                                  hd.TienGioChoi, hd.TienDichVu, hd.TrangThai
                           FROM HoaDon hd
                           JOIN BanBida b ON hd.MaBan = b.MaBan
                           WHERE (@Filter = -1 OR hd.TrangThai = @Filter)
                           ORDER BY hd.MaHD DESC";

            using (SqlConnection conn = DBConnect.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Filter", trangThaiFilter);
                conn.Open();
                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        list.Add(new HoaDonDTO
                        {
                            MaHD = (int)rd["MaHD"],
                            MaBan = (int)rd["MaBan"],
                            TenBan = rd["TenBan"].ToString(),
                            GioBatDau = (DateTime)rd["GioBatDau"],
                            GioKetThuc = rd["GioKetThuc"] == DBNull.Value ? (DateTime?)null : (DateTime)rd["GioKetThuc"],
                            TienGioChoi = (decimal)rd["TienGioChoi"],
                            TienDichVu = (decimal)rd["TienDichVu"],
                            TrangThai = (bool)rd["TrangThai"]
                        });
                    }
                }
            }
            return list;
        }
    }
}