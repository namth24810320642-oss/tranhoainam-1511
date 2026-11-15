using System;

namespace DTO
{
    public class HoaDonDTO
    {
        public int MaHD { get; set; }
        public int MaBan { get; set; }
        public string TenBan { get; set; }
        public DateTime GioBatDau { get; set; }
        public DateTime? GioKetThuc { get; set; }
        public decimal TienGioChoi { get; set; }
        public decimal TienDichVu { get; set; }
        public decimal TongTien => TienGioChoi + TienDichVu;
        public bool TrangThai { get; set; } // false: Chưa thanh toán, true: Đã thanh toán
        public string TrangThaiText => TrangThai ? "Đã thanh toán" : "Chưa thanh toán";
    }
}