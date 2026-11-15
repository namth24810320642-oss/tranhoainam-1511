using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BUS;
using DTO;

namespace GUI
{
    public partial class frmThanhToan : Form
    {
        private readonly DonThuocBUS bus = new DonThuocBUS();
        private DonThuocDTO donDangChon = null;
        private List<ChiTietDonThuocDTO> chiTietDangChon = new List<ChiTietDonThuocDTO>();

        public frmThanhToan()
        {
            InitializeComponent();
        }

        private void frmThanhToan_Load(object sender, EventArgs e)
        {
            cboTrangThai.SelectedIndex = 0;
        }

        private void LoadDanhSachDon()
        {
            string tt = cboTrangThai.SelectedItem?.ToString() ?? "Tất cả";
            dgvDonThuoc.DataSource = bus.LayDanhSachDonThuoc(tt);

            dgvDonThuoc.Columns["MaDon"].HeaderText = "Mã Đơn";
            dgvDonThuoc.Columns["MaHoSo"].HeaderText = "Mã HS";
            dgvDonThuoc.Columns["HoTenBN"].HeaderText = "Bệnh Nhân";
            dgvDonThuoc.Columns["SoDienThoai"].HeaderText = "SĐT";
            dgvDonThuoc.Columns["NgayKham"].HeaderText = "Ngày Khám";
            dgvDonThuoc.Columns["ChanDoan"].HeaderText = "Chẩn Đoán";
            dgvDonThuoc.Columns["TienKham"].HeaderText = "Tiền Khám";
            dgvDonThuoc.Columns["TienKham"].DefaultCellStyle.Format = "N0";
            dgvDonThuoc.Columns["TongTien"].HeaderText = "Tổng Tiền";
            dgvDonThuoc.Columns["TongTien"].DefaultCellStyle.Format = "N0";
            dgvDonThuoc.Columns["TrangThai"].HeaderText = "Trạng Thái";
        }

        private void dgvDonThuoc_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvDonThuoc.Rows[e.RowIndex].DataBoundItem is DonThuocDTO don)
            {
                donDangChon = don;
                chiTietDangChon = bus.LayChiTietDon(don.MaDon);

                dgvChiTiet.DataSource = chiTietDangChon;
                dgvChiTiet.Columns["MaDon"].Visible = false;
                dgvChiTiet.Columns["MaThuoc"].HeaderText = "Mã Thuốc";
                dgvChiTiet.Columns["TenThuoc"].HeaderText = "Tên Thuốc";
                dgvChiTiet.Columns["DonViTinh"].HeaderText = "ĐVT";
                dgvChiTiet.Columns["SoLuong"].HeaderText = "Số Lượng";
                dgvChiTiet.Columns["DonGia"].HeaderText = "Đơn Giá";
                dgvChiTiet.Columns["DonGia"].DefaultCellStyle.Format = "N0";
                dgvChiTiet.Columns["ThanhTien"].HeaderText = "Thành Tiền";
                dgvChiTiet.Columns["ThanhTien"].DefaultCellStyle.Format = "N0";
                dgvChiTiet.Columns["LieuDung"].HeaderText = "Liều Dùng";

                decimal tienThuoc = chiTietDangChon.Sum(x => x.ThanhTien);
                lblTongKet.Text = $"Tiền khám: {don.TienKham:N0}đ | Tiền thuốc: {tienThuoc:N0}đ | TỔNG CỘNG: {don.TongTien:N0} VNĐ";
            }
        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            try
            {
                if (donDangChon == null)
                {
                    MessageBox.Show("Vui lòng chọn đơn thuốc cần thanh toán!", "Thông báo");
                    return;
                }

                if (bus.ThanhToan(donDangChon.MaDon, donDangChon.TrangThai))
                {
                    MessageBox.Show($"Đã thanh toán thành công đơn thuốc #{donDangChon.MaDon}!", "Thông báo");
                    donDangChon.TrangThai = "Đã thanh toán";
                    LoadDanhSachDon();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnInHoaDon_Click(object sender, EventArgs e)
        {
            if (donDangChon == null)
            {
                MessageBox.Show("Vui lòng chọn đơn thuốc cần in hóa đơn!", "Thông báo");
                return;
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("==================================================");
            sb.AppendLine("          HÓA ĐƠN KHÁM BỆNH & ĐƠN THUỐC           ");
            sb.AppendLine("==================================================");
            sb.AppendLine($"Mã Đơn: #{donDangChon.MaDon}   |   Ngày khám: {donDangChon.NgayKham:dd/MM/yyyy HH:mm}");
            sb.AppendLine($"Bệnh nhân: {donDangChon.HoTenBN} (SĐT: {donDangChon.SoDienThoai})");
            sb.AppendLine($"Chẩn đoán: {donDangChon.ChanDoan}");
            sb.AppendLine($"Trạng thái: {donDangChon.TrangThai}");
            sb.AppendLine("--------------------------------------------------");
            sb.AppendLine("CHI TIẾT ĐƠN THUỐC:");
            foreach (var item in chiTietDangChon)
            {
                sb.AppendLine($"- {item.TenThuoc} ({item.SoLuong} {item.DonViTinh} x {item.DonGia:N0}đ) = {item.ThanhTien:N0}đ");
                sb.AppendLine($"  Cách dùng: {item.LieuDung}");
            }
            sb.AppendLine("--------------------------------------------------");
            decimal tienThuoc = chiTietDangChon.Sum(x => x.ThanhTien);
            sb.AppendLine($"Tiền khám bệnh : {donDangChon.TienKham,15:N0} VNĐ");
            sb.AppendLine($"Tổng tiền thuốc: {tienThuoc,15:N0} VNĐ");
            sb.AppendLine($"TỔNG THANH TOÁN: {donDangChon.TongTien,15:N0} VNĐ");
            sb.AppendLine("==================================================");

            MessageBox.Show(sb.ToString(), "Bản In Hóa Đơn Viện Phí", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}