using System;
using System.Collections.Generic;
using DAL;
using DTO;

namespace BUS
{
    public class DonThuocBUS
    {
        private readonly DonThuocDAL dal = new DonThuocDAL();

        public int LuuVaXuatDonThuoc(int maHoSo, string chanDoan, decimal tienKham, List<ChiTietDonThuocDTO> dsThuoc)
        {
            if (maHoSo <= 0)
                throw new Exception("Vui lòng chọn hồ sơ bệnh nhân cần kê đơn!");
            if (string.IsNullOrWhiteSpace(chanDoan))
                throw new Exception("Vui lòng nhập chẩn đoán bệnh trước khi xuất đơn!");
            if (tienKham < 0)
                throw new Exception("Tiền khám không hợp lệ!");
            if (dsThuoc == null || dsThuoc.Count == 0)
                throw new Exception("Đơn thuốc phải có ít nhất 1 loại thuốc!");

            return dal.LuuVaXuatDonThuoc(maHoSo, chanDoan, tienKham, dsThuoc);
        }

        public List<DonThuocDTO> LayDanhSachDonThuoc(string trangThai) => dal.GetDanhSachDonThuoc(trangThai);

        public List<ChiTietDonThuocDTO> LayChiTietDon(int maDon) => dal.GetChiTietDonThuoc(maDon);

        public bool ThanhToan(int maDon, string trangThaiHienTai)
        {
            if (maDon <= 0)
                throw new Exception("Vui lòng chọn đơn thuốc cần thanh toán!");
            if (trangThaiHienTai == "Đã thanh toán")
                throw new Exception("Đơn thuốc này đã được thanh toán trước đó!");

            return dal.ThanhToan(maDon);
        }
    }
}