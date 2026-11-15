using System;
using System.Collections.Generic;
using DAL;
using DTO;

namespace BUS
{
    public class DauSachBUS
    {
        private readonly DauSachDAL dal = new DauSachDAL();

        public List<DauSachDTO> LayDanhSachDauSach() => dal.GetAll();

        public List<DauSachDTO> TimKiemDauSach(string keyword) => dal.Search(keyword);

        public bool ThemDauSach(DauSachDTO ds)
        {
            if (string.IsNullOrWhiteSpace(ds.TenSach))
                throw new Exception("Tên sách không được để trống!");
            if (ds.SoLuongCon < 0)
                throw new Exception("Số lượng sách phải lớn hơn hoặc bằng 0!");
            return dal.Insert(ds);
        }

        public bool SuaDauSach(DauSachDTO ds)
        {
            if (ds.MaSach <= 0)
                throw new Exception("Vui lòng chọn đầu sách cần sửa!");
            if (string.IsNullOrWhiteSpace(ds.TenSach))
                throw new Exception("Tên sách không được để trống!");
            if (ds.SoLuongCon < 0)
                throw new Exception("Số lượng sách phải lớn hơn hoặc bằng 0!");
            return dal.Update(ds);
        }

        public bool XoaDauSach(int maSach)
        {
            if (maSach <= 0)
                throw new Exception("Vui lòng chọn đầu sách cần xóa!");
            return dal.Delete(maSach);
        }
    }
}