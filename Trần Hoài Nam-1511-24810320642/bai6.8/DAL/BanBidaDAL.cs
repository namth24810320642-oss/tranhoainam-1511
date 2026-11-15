using System.Collections.Generic;
using System.Data.SqlClient;
using DTO;

namespace DAL
{
    public class BanBidaDAL
    {
        public List<BanBidaDTO> GetAll()
        {
            var list = new List<BanBidaDTO>();
            string sql = "SELECT MaBan, TenBan, LoaiBan, GiaTheoGio, TrangThai FROM BanBida";

            using (SqlConnection conn = DBConnect.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        list.Add(new BanBidaDTO
                        {
                            MaBan = (int)rd["MaBan"],
                            TenBan = rd["TenBan"].ToString(),
                            LoaiBan = rd["LoaiBan"].ToString(),
                            GiaTheoGio = (decimal)rd["GiaTheoGio"],
                            TrangThai = rd["TrangThai"].ToString()
                        });
                    }
                }
            }
            return list;
        }

        public bool Insert(BanBidaDTO ban)
        {
            string sql = "INSERT INTO BanBida (TenBan, LoaiBan, GiaTheoGio, TrangThai) VALUES (@TenBan, @LoaiBan, @GiaTheoGio, N'Trống')";
            using (SqlConnection conn = DBConnect.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@TenBan", ban.TenBan);
                cmd.Parameters.AddWithValue("@LoaiBan", ban.LoaiBan);
                cmd.Parameters.AddWithValue("@GiaTheoGio", ban.GiaTheoGio);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Update(BanBidaDTO ban)
        {
            string sql = "UPDATE BanBida SET TenBan = @TenBan, LoaiBan = @LoaiBan, GiaTheoGio = @GiaTheoGio WHERE MaBan = @MaBan";
            using (SqlConnection conn = DBConnect.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@MaBan", ban.MaBan);
                cmd.Parameters.AddWithValue("@TenBan", ban.TenBan);
                cmd.Parameters.AddWithValue("@LoaiBan", ban.LoaiBan);
                cmd.Parameters.AddWithValue("@GiaTheoGio", ban.GiaTheoGio);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Delete(int maBan)
        {
            string sql = "DELETE FROM BanBida WHERE MaBan = @MaBan AND TrangThai = N'Trống'";
            using (SqlConnection conn = DBConnect.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@MaBan", maBan);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}