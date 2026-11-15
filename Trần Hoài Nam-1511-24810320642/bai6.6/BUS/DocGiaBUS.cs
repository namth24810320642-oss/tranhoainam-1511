using System;
using System.Collections.Generic;
using DAL;
using DTO;

namespace BUS
{
    public class DocGiaBUS
    {
        private readonly DocGiaDAL dal = new DocGiaDAL();

        public List<DocGiaDTO> LayDanhSachDocGia() => dal.GetAll();

        public bool ThemDocGia(DocGiaDTO dg)
        {
            if (string.IsNullOrWhiteSpace(dg.HoTen))
                throw new Exception("Họ tên độc giả không được để trống!");
            return dal.Insert(dg);
        }

        public bool SuaDocGia(DocGiaDTO dg)
        {
            if (dg.MaDocGia <= 0)
                throw new Exception("Vui lòng chọn độc giả cần sửa!");
            if (string.IsNullOrWhiteSpace(dg.HoTen))
                throw new Exception("Họ tên độc giả không được để trống!");
            return dal.Update(dg);
        }

        public bool XoaDocGia(int maDocGia)
        {
            if (maDocGia <= 0)
                throw new Exception("Vui lòng chọn độc giả cần xóa!");
            return dal.Delete(maDocGia);
        }
    }
}