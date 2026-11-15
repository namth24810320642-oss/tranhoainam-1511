using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using DTO;

namespace DAL
{
    public class DonThuocDAL
    {
        // Kê đơn thuốc & Xuất kho dược phẩm bằng SqlTransaction
        public int LuuVaXuatDonThuoc(int maHoSo, string chanDoan, decimal tienKham, List<ChiTietDonThuocDTO> dsThuoc)
        {
            using (SqlConnection conn = DBConnect.GetConnection())
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();
                try
                {
                    // Cập nhật chẩn đoán vào Hồ sơ khám
                    string sqlHS = "UPDATE HoSoKham SET ChanDoan = @ChanDoan WHERE MaHoSo = @MaHoSo";
                    SqlCommand cmdHS = new SqlCommand(sqlHS, conn, trans);
                    cmdHS.Parameters.AddWithValue("@ChanDoan", chanDoan);
                    cmdHS.Parameters.AddWithValue("@MaHoSo", maHoSo);
                    cmdHS.ExecuteNonQuery();

                    // Tính tổng tiền = Tiền khám + Tổng(SoLuong * DonGia)
                    decimal tongTienThuoc = dsThuoc.Sum(x => x.SoLuong * x.DonGia);
                    decimal tongTien = tienKham + tongTienThuoc;

                    // 1. Tạo bản ghi DonThuoc
                    string sqlDon = @"INSERT INTO DonThuoc (MaHoSo, TienKham, TongTien, TrangThai)
                                      OUTPUT INSERTED.MaDon
                                      VALUES (@MaHoSo, @TienKham, @TongTien, N'Chờ lấy thuốc')";
                    SqlCommand cmdDon = new SqlCommand(sqlDon, conn, trans);
                    cmdDon.Parameters.AddWithValue("@MaHoSo", maHoSo);
                    cmdDon.Parameters.AddWithValue("@TienKham", tienKham);
                    cmdDon.Parameters.AddWithValue("@TongTien", tongTien);
                    int maDon = (int)cmdDon.ExecuteScalar();

                    foreach (var item in dsThuoc)
                    {
                        // 2. Tạo bản ghi ChiTietDonThuoc
                        string sqlCT = @"INSERT INTO ChiTietDonThuoc (MaDon, MaThuoc, SoLuong, LieuDung)
                                         VALUES (@MaDon, @MaThuoc, @SoLuong, @LieuDung)";
                        SqlCommand cmdCT = new SqlCommand(sqlCT, conn, trans);
                        cmdCT.Parameters.AddWithValue("@MaDon", maDon);
                        cmdCT.Parameters.AddWithValue("@MaThuoc", item.MaThuoc);
                        cmdCT.Parameters.AddWithValue("@SoLuong", item.SoLuong);
                        cmdCT.Parameters.AddWithValue("@LieuDung", item.LieuDung ?? "");
                        cmdCT.ExecuteNonQuery();

                        // 3. Trừ số lượng tồn trong bảng Thuoc (chỉ cho phép trừ khi SoLuongTon >= SoLuong)
                        string sqlTruKho = @"UPDATE Thuoc 
                                             SET SoLuongTon = SoLuongTon - @SoLuong 
                                             WHERE MaThuoc = @MaThuoc AND SoLuongTon >= @SoLuong";
                        SqlCommand cmdKho = new SqlCommand(sqlTruKho, conn, trans);
                        cmdKho.Parameters.AddWithValue("@SoLuong", item.SoLuong);
                        cmdKho.Parameters.AddWithValue("@MaThuoc", item.MaThuoc);

                        int affected = cmdKho.ExecuteNonQuery();
                        if (affected == 0)
                        {
                            // 4. Không đủ tồn kho -> Ném ngoại lệ để gọi trans.Rollback()
                            throw new Exception($"Thuốc '{item.TenThuoc}' không đủ số lượng tồn trong kho (yêu cầu {item.SoLuong})!");
                        }
                    }

                    trans.Commit();
                    return maDon;
                }
                catch
                {
                    trans.Rollback();
                    throw;
                }
            }
        }

        public List<DonThuocDTO> GetDanhSachDonThuoc(string trangThai)
        {
            var list = new List<DonThuocDTO>();
            string sql = @"SELECT dt.MaDon, dt.MaHoSo, bn.HoTen, bn.SoDienThoai, hs.NgayKham, hs.ChanDoan,
                                  dt.TienKham, dt.TongTien, dt.TrangThai
                           FROM DonThuoc dt
                           JOIN HoSoKham hs ON dt.MaHoSo = hs.MaHoSo
                           JOIN BenhNhan bn ON hs.MaBN = bn.MaBN
                           WHERE (@TrangThai = N'Tất cả' OR dt.TrangThai = @TrangThai)
                           ORDER BY dt.MaDon DESC";

            using (SqlConnection conn = DBConnect.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@TrangThai", trangThai);
                conn.Open();
                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        list.Add(new DonThuocDTO
                        {
                            MaDon = (int)rd["MaDon"],
                            MaHoSo = (int)rd["MaHoSo"],
                            HoTenBN = rd["HoTen"].ToString(),
                            SoDienThoai = rd["SoDienThoai"].ToString(),
                            NgayKham = (DateTime)rd["NgayKham"],
                            ChanDoan = rd["ChanDoan"]?.ToString() ?? "",
                            TienKham = (decimal)rd["TienKham"],
                            TongTien = (decimal)rd["TongTien"],
                            TrangThai = rd["TrangThai"].ToString()
                        });
                    }
                }
            }
            return list;
        }

        public List<ChiTietDonThuocDTO> GetChiTietDonThuoc(int maDon)
        {
            var list = new List<ChiTietDonThuocDTO>();
            string sql = @"SELECT ct.MaDon, ct.MaThuoc, t.TenThuoc, t.DonViTinh, ct.SoLuong, t.DonGia, ct.LieuDung
                           FROM ChiTietDonThuoc ct
                           JOIN Thuoc t ON ct.MaThuoc = t.MaThuoc
                           WHERE ct.MaDon = @MaDon";

            using (SqlConnection conn = DBConnect.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@MaDon", maDon);
                conn.Open();
                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        list.Add(new ChiTietDonThuocDTO
                        {
                            MaDon = (int)rd["MaDon"],
                            MaThuoc = (int)rd["MaThuoc"],
                            TenThuoc = rd["TenThuoc"].ToString(),
                            DonViTinh = rd["DonViTinh"].ToString(),
                            SoLuong = (int)rd["SoLuong"],
                            DonGia = (decimal)rd["DonGia"],
                            LieuDung = rd["LieuDung"]?.ToString() ?? ""
                        });
                    }
                }
            }
            return list;
        }

        public bool ThanhToan(int maDon)
        {
            string sql = "UPDATE DonThuoc SET TrangThai = N'Đã thanh toán' WHERE MaDon = @MaDon";
            using (SqlConnection conn = DBConnect.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@MaDon", maDon);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}