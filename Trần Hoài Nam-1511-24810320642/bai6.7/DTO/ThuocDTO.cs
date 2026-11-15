namespace DTO
{
    public class ThuocDTO
    {
        public int MaThuoc { get; set; }
        public string TenThuoc { get; set; }
        public string DonViTinh { get; set; }
        public decimal DonGia { get; set; }
        public int SoLuongTon { get; set; }
        public string ThongTinHienThi => $"{TenThuoc} ({DonViTinh}) - Giá: {DonGia:N0}đ - Tồn: {SoLuongTon}";
    }
}