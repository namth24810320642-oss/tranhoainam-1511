namespace DTO
{
    public class MenuDichVuDTO
    {
        public int MaDV { get; set; }
        public string TenDV { get; set; }
        public decimal DonGia { get; set; }
        public int? TonKho { get; set; } // Null nếu là dịch vụ cho thuê
        public string ThongTinHienThi =>
            TonKho.HasValue
                ? $"{TenDV} - {DonGia:N0}đ (Tồn: {TonKho})"
                : $"{TenDV} - {DonGia:N0}đ (Dịch vụ thuê)";
    }
}