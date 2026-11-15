using System.Collections.Generic;
using System.Data.SqlClient;
using DTO;

namespace DAL
{
    public class DauSachDAL
    {
        public List<DauSachDTO> GetAll()
        {
            var list = new List<DauSachDTO>();
            string sql = "SELECT MaSach, TenSach, TheLoai, SoLuongCon FROM DauSach";

            using (SqlConnection conn = DBConnect.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        list.Add(new DauSachDTO
                        {
                            MaSach = (int)rd["MaSach"],
                            TenSach = rd["TenSach"].ToString(),
                            TheLoai = rd["TheLoai"].ToString(),
                            SoLuongCon = (int)rd["SoLuongCon"]
                        });
                    }
                }
            }
            return list;
        }

        public List<DauSachDTO> Search(string keyword)
        {
            var list = new List<DauSachDTO>();
            string sql = "SELECT MaSach, TenSach, TheLoai, SoLuongCon FROM DauSach WHERE TenSach LIKE @Kw OR TheLoai LIKE @Kw";

            using (SqlConnection conn = DBConnect.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Kw", "%" + keyword + "%");
                conn.Open();
                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        list.Add(new DauSachDTO
                        {
                            MaSach = (int)rd["MaSach"],
                            TenSach = rd["TenSach"].ToString(),
                            TheLoai = rd["TheLoai"].ToString(),
                            SoLuongCon = (int)rd["SoLuongCon"]
                        });
                    }
                }
            }
            return list;
        }

        public bool Insert(DauSachDTO ds)
        {
            string sql = "INSERT INTO DauSach (TenSach, TheLoai, SoLuongCon) VALUES (@TenSach, @TheLoai, @SoLuongCon)";
            using (SqlConnection conn = DBConnect.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@TenSach", ds.TenSach);
                cmd.Parameters.AddWithValue("@TheLoai", ds.TheLoai);
                cmd.Parameters.AddWithValue("@SoLuongCon", ds.SoLuongCon);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Update(DauSachDTO ds)
        {
            string sql = "UPDATE DauSach SET TenSach = @TenSach, TheLoai = @TheLoai, SoLuongCon = @SoLuongCon WHERE MaSach = @MaSach";
            using (SqlConnection conn = DBConnect.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@MaSach", ds.MaSach);
                cmd.Parameters.AddWithValue("@TenSach", ds.TenSach);
                cmd.Parameters.AddWithValue("@TheLoai", ds.TheLoai);
                cmd.Parameters.AddWithValue("@SoLuongCon", ds.SoLuongCon);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Delete(int maSach)
        {
            string sql = "DELETE FROM DauSach WHERE MaSach = @MaSach";
            using (SqlConnection conn = DBConnect.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@MaSach", maSach);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}