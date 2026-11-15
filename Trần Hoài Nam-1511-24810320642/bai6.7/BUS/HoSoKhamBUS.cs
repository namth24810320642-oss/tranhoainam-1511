using System;
using System.Collections.Generic;
using DAL;
using DTO;

namespace BUS
{
    public class HoSoKhamBUS
    {
        private readonly HoSoKhamDAL dal = new HoSoKhamDAL();

        public List<HoSoKhamDTO> LayDanhSachHoSo() => dal.GetAll();

        public bool TaoHoSoKham(HoSoKhamDTO hs)
        {
            if (hs.MaBN <= 0)
                throw new Exception("Chưa xác định bệnh nhân cho hồ sơ khám!");
            if (string.IsNullOrWhiteSpace(hs.TrieuChung))
                throw new Exception("Vui lòng nhập triệu chứng lâm sàng!");

            return dal.Insert(hs);
        }
    }
}