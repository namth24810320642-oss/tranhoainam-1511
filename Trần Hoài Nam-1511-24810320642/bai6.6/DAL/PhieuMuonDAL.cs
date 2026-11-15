using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DTO;

namespace DAL
{
    public class PhieuMuonDAL
    {
        // Lọc phiếu mượn theo Độc giả và Trạng thái
        public List<PhieuMuonDTO> LocPhieuMuon(int maDocGia, string trangThai)
        {
            var list = new List<PhieuMuonDTO>();
            string sql = @"SELECT pm.MaPhieu, pm.MaDocGia, dg.HoTen, pm.NgayMuon, pm.HanTra, pm.TrangThai
                           FROM PhieuMuon pm
                           JOIN DocGia dg ON pm.MaDocGia = dg.MaDocGia
                           WHERE (@MaDocGia = 0 OR pm.MaDocGia = @MaDocGia)
                             AND (@TrangThai = N'Tất cả' OR pm.TrangThai = @TrangThai)
                           ORDER BY pm.MaPhieu DESC";

            using (SqlConnection conn = DBConnect.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@MaDocGia", maDocGia);
                cmd.Parameters.AddWithValue("@TrangThai", trangThai);
                conn.Open();
                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        list.Add(new PhieuMuonDTO
                        {
                            MaPhieu = (int)rd["MaPhieu"],
                            MaDocGia = (int)rd["MaDocGia"],
                            TenDocGia = rd["HoTen"].ToString(),
                            NgayMuon = (DateTime)rd["NgayMuon"],
                            HanTra = (DateTime)rd["HanTra"],
                            TrangThai = rd["TrangThai"].ToString()
                        });
                    }
                }
            }
            return list;
        }

        public List<ChiTietMuonDTO> GetChiTietByMaPhieu(int maPhieu)
        {
            var list = new List<ChiTietMuonDTO>();
            string sql = @"SELECT ct.MaPhieu, ct.MaSach, ds.TenSach, ct.SoLuong
                           FROM ChiTietMuon ct
                           JOIN DauSach ds ON ct.MaSach = ds.MaSach
                           WHERE ct.MaPhieu = @MaPhieu";

            using (SqlConnection conn = DBConnect.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@MaPhieu", maPhieu);
                conn.Open();
                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        list.Add(new ChiTietMuonDTO
                        {
                            MaPhieu = (int)rd["MaPhieu"],
                            MaSach = (int)rd["MaSach"],
                            TenSach = rd["TenSach"].ToString(),
                            SoLuong = (int)rd["SoLuong"]
                        });
                    }
                }
            }
            return list;
        }

        // Lập phiếu mượn mới và trừ số lượng sách tồn kho
        public bool LapPhieuMuon(PhieuMuonDTO pm, List<ChiTietMuonDTO> danhSachMuon)
        {
            using (SqlConnection conn = DBConnect.GetConnection())
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();
                try
                {
                    string sqlPM = @"INSERT INTO PhieuMuon (MaDocGia, NgayMuon, HanTra, TrangThai)
                                     OUTPUT INSERTED.MaPhieu
                                     VALUES (@MaDocGia, GETDATE(), @HanTra, N'Đang mượn')";
                    SqlCommand cmdPM = new SqlCommand(sqlPM, conn, trans);
                    cmdPM.Parameters.AddWithValue("@MaDocGia", pm.MaDocGia);
                    cmdPM.Parameters.AddWithValue("@HanTra", pm.HanTra);
                    int maPhieuMoi = (int)cmdPM.ExecuteScalar();

                    foreach (var item in danhSachMuon)
                    {
                        string sqlTruSach = @"UPDATE DauSach 
                                              SET SoLuongCon = SoLuongCon - @SoLuong 
                                              WHERE MaSach = @MaSach AND SoLuongCon >= @SoLuong";
                        SqlCommand cmdTru = new SqlCommand(sqlTruSach, conn, trans);
                        cmdTru.Parameters.AddWithValue("@SoLuong", item.SoLuong);
                        cmdTru.Parameters.AddWithValue("@MaSach", item.MaSach);

                        if (cmdTru.ExecuteNonQuery() == 0)
                            throw new Exception($"Sách '{item.TenSach}' không đủ số lượng trong kho!");

                        string sqlCT = @"INSERT INTO ChiTietMuon (MaPhieu, MaSach, SoLuong)
                                         VALUES (@MaPhieu, @MaSach, @SoLuong)";
                        SqlCommand cmdCT = new SqlCommand(sqlCT, conn, trans);
                        cmdCT.Parameters.AddWithValue("@MaPhieu", maPhieuMoi);
                        cmdCT.Parameters.AddWithValue("@MaSach", item.MaSach);
                        cmdCT.Parameters.AddWithValue("@SoLuong", item.SoLuong);
                        cmdCT.ExecuteNonQuery();
                    }

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

        // Trả sách: Parameterized SQL UPDATE chuyển trạng thái sang 'Đã trả' và cộng lại SoLuongCon
        public bool TraSach(int maPhieu)
        {
            using (SqlConnection conn = DBConnect.GetConnection())
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();
                try
                {
                    string sqlCongSach = @"UPDATE ds
                                           SET ds.SoLuongCon = ds.SoLuongCon + ct.SoLuong
                                           FROM DauSach ds
                                           INNER JOIN ChiTietMuon ct ON ds.MaSach = ct.MaSach
                                           WHERE ct.MaPhieu = @MaPhieu";
                    SqlCommand cmdCong = new SqlCommand(sqlCongSach, conn, trans);
                    cmdCong.Parameters.AddWithValue("@MaPhieu", maPhieu);
                    cmdCong.ExecuteNonQuery();

                    string sqlUpdatePM = @"UPDATE PhieuMuon 
                                           SET TrangThai = N'Đã trả' 
                                           WHERE MaPhieu = @MaPhieu AND TrangThai != N'Đã trả'";
                    SqlCommand cmdPM = new SqlCommand(sqlUpdatePM, conn, trans);
                    cmdPM.Parameters.AddWithValue("@MaPhieu", maPhieu);
                    int rows = cmdPM.ExecuteNonQuery();

                    if (rows == 0)
                        throw new Exception("Phiếu mượn này đã được trả trước đó hoặc không tồn tại!");

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

        // Cảnh báo sách quá hạn bằng SqlDataReader
        public List<PhieuMuonDTO> GetDanhSachQuaHan()
        {
            var list = new List<PhieuMuonDTO>();
            string sql = @"SELECT pm.MaPhieu, pm.MaDocGia, dg.HoTen, pm.NgayMuon, pm.HanTra, pm.TrangThai
                           FROM PhieuMuon pm
                           JOIN DocGia dg ON pm.MaDocGia = dg.MaDocGia
                           WHERE pm.HanTra < GETDATE() AND pm.TrangThai != N'Đã trả'
                           ORDER BY pm.HanTra ASC";

            using (SqlConnection conn = DBConnect.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        list.Add(new PhieuMuonDTO
                        {
                            MaPhieu = (int)rd["MaPhieu"],
                            MaDocGia = (int)rd["MaDocGia"],
                            TenDocGia = rd["HoTen"].ToString(),
                            NgayMuon = (DateTime)rd["NgayMuon"],
                            HanTra = (DateTime)rd["HanTra"],
                            TrangThai = rd["TrangThai"].ToString()
                        });
                    }
                }
            }
            return list;
        }

        // Đếm tổng số phiếu quá hạn bằng ExecuteScalar
        public int DemSoPhieuQuaHan()
        {
            string sql = "SELECT COUNT(*) FROM PhieuMuon WHERE HanTra < GETDATE() AND TrangThai != N'Đã trả'";
            using (SqlConnection conn = DBConnect.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                object result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 0;
            }
        }

        // Thống kê tổng số lượt mượn theo từng Thể loại bằng GROUP BY
        public List<ThongKeTheLoaiDTO> ThongKeTheoTheLoai()
        {
            var list = new List<ThongKeTheLoaiDTO>();
            string sql = @"SELECT ds.TheLoai, 
                                  COUNT(DISTINCT ct.MaPhieu) AS SoLuotMuon,
                                  ISNULL(SUM(ct.SoLuong), 0) AS TongSachMuon
                           FROM DauSach ds
                           LEFT JOIN ChiTietMuon ct ON ds.MaSach = ct.MaSach
                           GROUP BY ds.TheLoai
                           ORDER BY SoLuotMuon DESC";

            using (SqlConnection conn = DBConnect.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        list.Add(new ThongKeTheLoaiDTO
                        {
                            TheLoai = rd["TheLoai"].ToString(),
                            SoLuotMuon = (int)rd["SoLuotMuon"],
                            TongSachMuon = (int)rd["TongSachMuon"]
                        });
                    }
                }
            }
            return list;
        }
    }
}