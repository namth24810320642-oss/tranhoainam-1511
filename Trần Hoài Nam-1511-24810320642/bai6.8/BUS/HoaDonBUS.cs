using System;
using System.Collections.Generic;
using System.Data;
using DAL;
using DTO;

namespace BUS
{
    public class HoaDonBUS
    {
        private readonly HoaDonDAL dal = new HoaDonDAL();

        public bool MoBan(int maBan, string trangThaiHienTai)
        {
            if (maBan <= 0)
                throw new Exception("Vui lòng chọn bàn cần mở!");
            if (trangThaiHienTai != "Trống")
                throw new Exception("Chỉ có thể mở bàn đang ở trạng thái Trống!");
            return dal.MoBan(maBan);
        }

        public HoaDonDTO LayHoaDonChuaThanhToan(int maBan) => dal.GetHoaDonChuaThanhToanByBan(maBan);

        public DataTable LayChiTietHoaDonDataTable(int maHD) => dal.GetChiTietHoaDonDataTable(maHD);

        public void LuuChiTietHoaDon(DataTable dt) => dal.LuuChiTietHoaDonDataTable(dt);

        // Tính tiền giờ chơi làm tròn theo block 15 phút (tối thiểu 1 block = 0.25 giờ)
        public decimal TinhTienGioChoi(DateTime gioBatDau, DateTime gioKetThuc, decimal giaTheoGio)
        {
            double tongPhut = (gioKetThuc - gioBatDau).TotalMinutes;
            if (tongPhut < 1) tongPhut = 1;
            double soBlock15Phut = Math.Ceiling(tongPhut / 15.0);
            double soGio = soBlock15Phut * 0.25;
            return Math.Round((decimal)soGio * giaTheoGio, 0);
        }

        public bool ThanhToanHoaDon(int maHD, int maBan, DateTime gioKetThuc, decimal tienGioChoi, decimal tienDichVu)
        {
            if (maHD <= 0)
                throw new Exception("Không tìm thấy hóa đơn cần thanh toán!");
            return dal.ThanhToanHoaDon(maHD, maBan, gioKetThuc, tienGioChoi, tienDichVu);
        }

        public List<HoaDonDTO> LayDanhSachHoaDon(int filter) => dal.GetAllHoaDon(filter);
    }
}