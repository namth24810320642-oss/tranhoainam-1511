using System.Collections.Generic;
using System.Data.SqlClient;
using DTO;

namespace DAL
{
    public class ThuocDAL
    {
        public List<ThuocDTO> GetAll()
        {
            var list = new List<ThuocDTO>();
            string sql = "SELECT MaThuoc, TenThuoc, DonViTinh, DonGia, SoLuongTon FROM Thuoc";

            using (SqlConnection conn = DBConnect.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        list.Add(new ThuocDTO
                        {
                            MaThuoc = (int)rd["MaThuoc"],
                            TenThuoc = rd["TenThuoc"].ToString(),
                            DonViTinh = rd["DonViTinh"].ToString(),
                            DonGia = (decimal)rd["DonGia"],
                            SoLuongTon = (int)rd["SoLuongTon"]
                        });
                    }
                }
            }
            return list;
        }

        public bool Insert(ThuocDTO t)
        {
            string sql = @"INSERT INTO Thuoc (TenThuoc, DonViTinh, DonGia, SoLuongTon)
                           VALUES (@TenThuoc, @DonViTinh, @DonGia, @SoLuongTon)";
            using (SqlConnection conn = DBConnect.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@TenThuoc", t.TenThuoc);
                cmd.Parameters.AddWithValue("@DonViTinh", t.DonViTinh);
                cmd.Parameters.AddWithValue("@DonGia", t.DonGia);
                cmd.Parameters.AddWithValue("@SoLuongTon", t.SoLuongTon);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // Cập nhật thông tin thuốc, đơn giá và cộng thêm số lượng nhập kho
        public bool CapNhatGiaVaNhapKho(int maThuoc, string tenThuoc, string donViTinh, decimal donGiaMoi, int soLuongNhapThem)
        {
            string sql = @"UPDATE Thuoc 
                           SET TenThuoc = @TenThuoc,
                               DonViTinh = @DonViTinh,
                               DonGia = @DonGia, 
                               SoLuongTon = SoLuongTon + @SoLuongNhapThem 
                           WHERE MaThuoc = @MaThuoc";
            using (SqlConnection conn = DBConnect.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@MaThuoc", maThuoc);
                cmd.Parameters.AddWithValue("@TenThuoc", tenThuoc);
                cmd.Parameters.AddWithValue("@DonViTinh", donViTinh);
                cmd.Parameters.AddWithValue("@DonGia", donGiaMoi);
                cmd.Parameters.AddWithValue("@SoLuongNhapThem", soLuongNhapThem);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}