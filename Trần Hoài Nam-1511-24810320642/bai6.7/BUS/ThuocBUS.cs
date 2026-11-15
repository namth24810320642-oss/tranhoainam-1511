using System;
using System.Collections.Generic;
using DAL;
using DTO;

namespace BUS
{
    public class ThuocBUS
    {
        private readonly ThuocDAL dal = new ThuocDAL();

        public List<ThuocDTO> LayDanhSachThuoc() => dal.GetAll();

        public bool ThemThuocMoi(ThuocDTO t)
        {
            if (string.IsNullOrWhiteSpace(t.TenThuoc))
                throw new Exception("Tên thuốc không được để trống!");
            if (t.DonGia < 0)
                throw new Exception("Đơn giá không được âm!");
            if (t.SoLuongTon < 0)
                throw new Exception("Số lượng tồn không được âm!");

            return dal.Insert(t);
        }

        public bool CapNhatGiaVaNhapKho(int maThuoc, string tenThuoc, string donViTinh, decimal donGiaMoi, int soLuongNhapThem)
        {
            if (maThuoc <= 0)
                throw new Exception("Vui lòng chọn thuốc cần cập nhật!");
            if (string.IsNullOrWhiteSpace(tenThuoc))
                throw new Exception("Tên thuốc không được để trống!");
            if (donGiaMoi < 0)
                throw new Exception("Đơn giá mới không được âm!");
            if (soLuongNhapThem < 0)
                throw new Exception("Số lượng nhập thêm không được âm!");

            return dal.CapNhatGiaVaNhapKho(maThuoc, tenThuoc, donViTinh, donGiaMoi, soLuongNhapThem);
        }
    }
}