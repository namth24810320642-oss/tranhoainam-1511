using System;

namespace DTO
{
    public class HoSoKhamDTO
    {
        public int MaHoSo { get; set; }
        public int MaBN { get; set; }
        public string HoTenBN { get; set; }
        public string SoDienThoai { get; set; }
        public DateTime NgayKham { get; set; }
        public string TrieuChung { get; set; }
        public string ChanDoan { get; set; }
        public string ThongTinHienThi => $"HS#{MaHoSo} - {HoTenBN} ({SoDienThoai})";
    }
}