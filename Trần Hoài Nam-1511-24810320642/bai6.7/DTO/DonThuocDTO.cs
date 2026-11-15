using System;

namespace DTO
{
    public class DonThuocDTO
    {
        public int MaDon { get; set; }
        public int MaHoSo { get; set; }
        public string HoTenBN { get; set; }
        public string SoDienThoai { get; set; }
        public DateTime NgayKham { get; set; }
        public string ChanDoan { get; set; }
        public decimal TienKham { get; set; }
        public decimal TongTien { get; set; }
        public string TrangThai { get; set; }
    }
}