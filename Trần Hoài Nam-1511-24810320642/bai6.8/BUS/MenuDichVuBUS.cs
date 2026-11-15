using System;
using System.Collections.Generic;
using DAL;
using DTO;

namespace BUS
{
    public class MenuDichVuBUS
    {
        private readonly MenuDichVuDAL dal = new MenuDichVuDAL();

        public List<MenuDichVuDTO> LayDanhSachDichVu() => dal.GetAll();

        public bool ThemDichVu(MenuDichVuDTO dv)
        {
            if (string.IsNullOrWhiteSpace(dv.TenDV))
                throw new Exception("Tên dịch vụ / món không được để trống!");
            if (dv.DonGia < 0)
                throw new Exception("Đơn giá không được âm!");
            if (dv.TonKho.HasValue && dv.TonKho.Value < 0)
                throw new Exception("Số lượng tồn kho không được âm!");
            return dal.Insert(dv);
        }

        public bool SuaDichVu(MenuDichVuDTO dv)
        {
            if (dv.MaDV <= 0)
                throw new Exception("Vui lòng chọn dịch vụ cần sửa!");
            if (string.IsNullOrWhiteSpace(dv.TenDV))
                throw new Exception("Tên dịch vụ / món không được để trống!");
            if (dv.DonGia < 0)
                throw new Exception("Đơn giá không được âm!");
            if (dv.TonKho.HasValue && dv.TonKho.Value < 0)
                throw new Exception("Số lượng tồn kho không được âm!");
            return dal.Update(dv);
        }

        public bool XoaDichVu(int maDV)
        {
            if (maDV <= 0)
                throw new Exception("Vui lòng chọn dịch vụ cần xóa!");
            return dal.Delete(maDV);
        }
    }
}