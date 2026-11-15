using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DTO;

namespace DAL
{
    public class HoSoKhamDAL
    {
        public List<HoSoKhamDTO> GetAll()
        {
            var list = new List<HoSoKhamDTO>();
            string sql = @"SELECT hs.MaHoSo, hs.MaBN, bn.HoTen, bn.SoDienThoai, hs.NgayKham, hs.TrieuChung, hs.ChanDoan
                           FROM HoSoKham hs
                           JOIN BenhNhan bn ON hs.MaBN = bn.MaBN
                           ORDER BY hs.MaHoSo DESC";

            using (SqlConnection conn = DBConnect.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        list.Add(new HoSoKhamDTO
                        {
                            MaHoSo = (int)rd["MaHoSo"],
                            MaBN = (int)rd["MaBN"],
                            HoTenBN = rd["HoTen"].ToString(),
                            SoDienThoai = rd["SoDienThoai"].ToString(),
                            NgayKham = (DateTime)rd["NgayKham"],
                            TrieuChung = rd["TrieuChung"]?.ToString() ?? "",
                            ChanDoan = rd["ChanDoan"]?.ToString() ?? ""
                        });
                    }
                }
            }
            return list;
        }

        public bool Insert(HoSoKhamDTO hs)
        {
            string sql = @"INSERT INTO HoSoKham (MaBN, NgayKham, TrieuChung, ChanDoan)
                           VALUES (@MaBN, GETDATE(), @TrieuChung, @ChanDoan)";
            using (SqlConnection conn = DBConnect.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@MaBN", hs.MaBN);
                cmd.Parameters.AddWithValue("@TrieuChung", hs.TrieuChung ?? "");
                cmd.Parameters.AddWithValue("@ChanDoan", hs.ChanDoan ?? "");
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}