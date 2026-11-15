using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DTO;

namespace DAL
{
    public class DocGiaDAL
    {
        public List<DocGiaDTO> GetAll()
        {
            var list = new List<DocGiaDTO>();
            string sql = "SELECT MaDocGia, HoTen, Email, NgayHetHanThe FROM DocGia";

            using (SqlConnection conn = DBConnect.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        list.Add(new DocGiaDTO
                        {
                            MaDocGia = (int)rd["MaDocGia"],
                            HoTen = rd["HoTen"].ToString(),
                            Email = rd["Email"] == DBNull.Value ? "" : rd["Email"].ToString(),
                            NgayHetHanThe = (DateTime)rd["NgayHetHanThe"]
                        });
                    }
                }
            }
            return list;
        }

        public DocGiaDTO GetById(int maDocGia)
        {
            string sql = "SELECT MaDocGia, HoTen, Email, NgayHetHanThe FROM DocGia WHERE MaDocGia = @MaDocGia";
            using (SqlConnection conn = DBConnect.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@MaDocGia", maDocGia);
                conn.Open();
                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    if (rd.Read())
                    {
                        return new DocGiaDTO
                        {
                            MaDocGia = (int)rd["MaDocGia"],
                            HoTen = rd["HoTen"].ToString(),
                            Email = rd["Email"] == DBNull.Value ? "" : rd["Email"].ToString(),
                            NgayHetHanThe = (DateTime)rd["NgayHetHanThe"]
                        };
                    }
                }
            }
            return null;
        }

        public bool Insert(DocGiaDTO dg)
        {
            string sql = "INSERT INTO DocGia (HoTen, Email, NgayHetHanThe) VALUES (@HoTen, @Email, @NgayHetHanThe)";
            using (SqlConnection conn = DBConnect.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@HoTen", dg.HoTen);
                cmd.Parameters.AddWithValue("@Email", string.IsNullOrWhiteSpace(dg.Email) ? (object)DBNull.Value : dg.Email);
                cmd.Parameters.AddWithValue("@NgayHetHanThe", dg.NgayHetHanThe);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Update(DocGiaDTO dg)
        {
            string sql = "UPDATE DocGia SET HoTen = @HoTen, Email = @Email, NgayHetHanThe = @NgayHetHanThe WHERE MaDocGia = @MaDocGia";
            using (SqlConnection conn = DBConnect.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@MaDocGia", dg.MaDocGia);
                cmd.Parameters.AddWithValue("@HoTen", dg.HoTen);
                cmd.Parameters.AddWithValue("@Email", string.IsNullOrWhiteSpace(dg.Email) ? (object)DBNull.Value : dg.Email);
                cmd.Parameters.AddWithValue("@NgayHetHanThe", dg.NgayHetHanThe);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Delete(int maDocGia)
        {
            string sql = "DELETE FROM DocGia WHERE MaDocGia = @MaDocGia";
            using (SqlConnection conn = DBConnect.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@MaDocGia", maDocGia);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}