using System.Data.SqlClient;
using DTO;

namespace DAL
{
    public class BenhNhanDAL
    {
        // Tra cứu bệnh nhân theo SoDienThoai bằng SqlParameter
        public BenhNhanDTO GetByPhone(string sdt)
        {
            string sql = "SELECT MaBN, HoTen, SoDienThoai, NamSinh FROM BenhNhan WHERE SoDienThoai = @SDT";
            using (SqlConnection conn = DBConnect.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@SDT", sdt);
                conn.Open();
                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    if (rd.Read())
                    {
                        return new BenhNhanDTO
                        {
                            MaBN = (int)rd["MaBN"],
                            HoTen = rd["HoTen"].ToString(),
                            SoDienThoai = rd["SoDienThoai"].ToString(),
                            NamSinh = rd["NamSinh"] != System.DBNull.Value ? (int)rd["NamSinh"] : 0
                        };
                    }
                }
            }
            return null;
        }

        public int InsertAndGetId(BenhNhanDTO bn)
        {
            string sql = @"INSERT INTO BenhNhan (HoTen, SoDienThoai, NamSinh)
                           OUTPUT INSERTED.MaBN
                           VALUES (@HoTen, @SDT, @NamSinh)";
            using (SqlConnection conn = DBConnect.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@HoTen", bn.HoTen);
                cmd.Parameters.AddWithValue("@SDT", bn.SoDienThoai);
                cmd.Parameters.AddWithValue("@NamSinh", bn.NamSinh);
                conn.Open();
                return (int)cmd.ExecuteScalar();
            }
        }
    }
}