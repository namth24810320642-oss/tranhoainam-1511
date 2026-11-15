using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DTO;

namespace DAL
{
    public class MenuDichVuDAL
    {
        public List<MenuDichVuDTO> GetAll()
        {
            var list = new List<MenuDichVuDTO>();
            string sql = "SELECT MaDV, TenDV, DonGia, TonKho FROM MenuDichVu";

            using (SqlConnection conn = DBConnect.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        list.Add(new MenuDichVuDTO
                        {
                            MaDV = (int)rd["MaDV"],
                            TenDV = rd["TenDV"].ToString(),
                            DonGia = (decimal)rd["DonGia"],
                            TonKho = rd["TonKho"] == DBNull.Value ? (int?)null : (int)rd["TonKho"]
                        });
                    }
                }
            }
            return list;
        }

        public bool Insert(MenuDichVuDTO dv)
        {
            string sql = "INSERT INTO MenuDichVu (TenDV, DonGia, TonKho) VALUES (@TenDV, @DonGia, @TonKho)";
            using (SqlConnection conn = DBConnect.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@TenDV", dv.TenDV);
                cmd.Parameters.AddWithValue("@DonGia", dv.DonGia);
                cmd.Parameters.AddWithValue("@TonKho", dv.TonKho.HasValue ? (object)dv.TonKho.Value : DBNull.Value);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Update(MenuDichVuDTO dv)
        {
            string sql = "UPDATE MenuDichVu SET TenDV = @TenDV, DonGia = @DonGia, TonKho = @TonKho WHERE MaDV = @MaDV";
            using (SqlConnection conn = DBConnect.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@MaDV", dv.MaDV);
                cmd.Parameters.AddWithValue("@TenDV", dv.TenDV);
                cmd.Parameters.AddWithValue("@DonGia", dv.DonGia);
                cmd.Parameters.AddWithValue("@TonKho", dv.TonKho.HasValue ? (object)dv.TonKho.Value : DBNull.Value);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Delete(int maDV)
        {
            string sql = "DELETE FROM MenuDichVu WHERE MaDV = @MaDV";
            using (SqlConnection conn = DBConnect.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@MaDV", maDV);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}