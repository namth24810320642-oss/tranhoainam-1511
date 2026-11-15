using System;
using System.Collections.Generic;
using DAL;
using DTO;

namespace BUS
{
    public class BanBidaBUS
    {
        private readonly BanBidaDAL dal = new BanBidaDAL();

        public List<BanBidaDTO> LayDanhSachBan() => dal.GetAll();

        public bool ThemBan(BanBidaDTO ban)
        {
            if (string.IsNullOrWhiteSpace(ban.TenBan))
                throw new Exception("Tên bàn không được để trống!");
            if (ban.GiaTheoGio <= 0)
                throw new Exception("Giá theo giờ phải lớn hơn 0!");
            return dal.Insert(ban);
        }

        public bool SuaBan(BanBidaDTO ban)
        {
            if (ban.MaBan <= 0)
                throw new Exception("Vui lòng chọn bàn cần sửa!");
            if (string.IsNullOrWhiteSpace(ban.TenBan))
                throw new Exception("Tên bàn không được để trống!");
            if (ban.GiaTheoGio <= 0)
                throw new Exception("Giá theo giờ phải lớn hơn 0!");
            return dal.Update(ban);
        }

        public bool XoaBan(int maBan, string trangThai)
        {
            if (maBan <= 0)
                throw new Exception("Vui lòng chọn bàn cần xóa!");
            if (trangThai == "Đang chơi")
                throw new Exception("Không thể xóa bàn đang có khách chơi!");
            return dal.Delete(maBan);
        }
    }
}