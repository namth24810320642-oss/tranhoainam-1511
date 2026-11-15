using System;
using System.Collections.Generic;
using DAL;
using DTO;

namespace BUS
{
    public class PhieuMuonBUS
    {
        private readonly PhieuMuonDAL pmDal = new PhieuMuonDAL();
        private readonly DocGiaDAL dgDal = new DocGiaDAL();

        public List<PhieuMuonDTO> LocPhieuMuon(int maDocGia, string trangThai)
            => pmDal.LocPhieuMuon(maDocGia, trangThai);

        public List<ChiTietMuonDTO> LayChiTietPhieu(int maPhieu)
            => pmDal.GetChiTietByMaPhieu(maPhieu);

        public bool LapPhieuMuon(PhieuMuonDTO pm, List<ChiTietMuonDTO> danhSachMuon)
        {
            if (pm.MaDocGia <= 0)
                throw new Exception("Vui lòng chọn độc giả mượn sách!");
            if (danhSachMuon == null || danhSachMuon.Count == 0)
                throw new Exception("Vui lòng chọn ít nhất 1 đầu sách để mượn!");
            if (pm.HanTra.Date < DateTime.Now.Date)
                throw new Exception("Hạn trả phải lớn hơn hoặc bằng ngày hiện tại!");

            // Kiểm tra hạn thẻ độc giả (NgayHetHanThe >= GetDate())
            DocGiaDTO docGia = dgDal.GetById(pm.MaDocGia);
            if (docGia == null)
                throw new Exception("Không tìm thấy thông tin độc giả!");
            if (docGia.NgayHetHanThe.Date < DateTime.Now.Date)
                throw new Exception($"Thẻ độc giả '{docGia.HoTen}' đã hết hạn ngày {docGia.NgayHetHanThe:dd/MM/yyyy}. Không thể lập phiếu mượn!");

            return pmDal.LapPhieuMuon(pm, danhSachMuon);
        }

        public bool TraSach(int maPhieu, string trangThaiHienTai)
        {
            if (maPhieu <= 0)
                throw new Exception("Vui lòng chọn phiếu mượn cần trả!");
            if (trangThaiHienTai == "Đã trả")
                throw new Exception("Phiếu mượn này đã được trả rồi!");

            return pmDal.TraSach(maPhieu);
        }

        public List<PhieuMuonDTO> LayDanhSachQuaHan() => pmDal.GetDanhSachQuaHan();

        public int DemSoPhieuQuaHan() => pmDal.DemSoPhieuQuaHan();

        public List<ThongKeTheLoaiDTO> ThongKeTheoTheLoai() => pmDal.ThongKeTheoTheLoai();
    }
}