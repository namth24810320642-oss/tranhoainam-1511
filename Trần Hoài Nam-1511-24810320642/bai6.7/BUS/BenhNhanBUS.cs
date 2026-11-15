using System;
using DAL;
using DTO;

namespace BUS
{
    public class BenhNhanBUS
    {
        private readonly BenhNhanDAL dal = new BenhNhanDAL();

        public BenhNhanDTO TraCuuTheoSDT(string sdt)
        {
            if (string.IsNullOrWhiteSpace(sdt))
                throw new Exception("Vui lòng nhập số điện thoại cần tra cứu!");
            return dal.GetByPhone(sdt.Trim());
        }

        public int ThemBenhNhanMoi(BenhNhanDTO bn)
        {
            if (string.IsNullOrWhiteSpace(bn.HoTen))
                throw new Exception("Họ tên bệnh nhân không được để trống!");
            if (string.IsNullOrWhiteSpace(bn.SoDienThoai))
                throw new Exception("Số điện thoại không được để trống!");
            if (bn.NamSinh < 1900 || bn.NamSinh > DateTime.Now.Year)
                throw new Exception("Năm sinh không hợp lệ!");

            return dal.InsertAndGetId(bn);
        }
    }
}