using System;
using System.Windows.Forms;
using BUS;

namespace GUI
{
    public partial class frmLichSuHoaDon : Form
    {
        private readonly HoaDonBUS bus = new HoaDonBUS();

        public frmLichSuHoaDon()
        {
            InitializeComponent();
        }

        private void frmLichSuHoaDon_Load(object sender, EventArgs e)
        {
            cboTrangThai.SelectedIndex = 0;
        }

        private void LoadHoaDon()
        {
            int filter = cboTrangThai.SelectedIndex == 1 ? 0 : (cboTrangThai.SelectedIndex == 2 ? 1 : -1);
            dgvHoaDon.DataSource = bus.LayDanhSachHoaDon(filter);

            dgvHoaDon.Columns["MaBan"].Visible = false;
            dgvHoaDon.Columns["TrangThai"].Visible = false;
            dgvHoaDon.Columns["MaHD"].HeaderText = "Mã HĐ";
            dgvHoaDon.Columns["TenBan"].HeaderText = "Bàn";
            dgvHoaDon.Columns["GioBatDau"].HeaderText = "Giờ Bắt Đầu";
            dgvHoaDon.Columns["GioKetThuc"].HeaderText = "Giờ Kết Thúc";
            dgvHoaDon.Columns["TienGioChoi"].HeaderText = "Tiền Giờ";
            dgvHoaDon.Columns["TienGioChoi"].DefaultCellStyle.Format = "N0";
            dgvHoaDon.Columns["TienDichVu"].HeaderText = "Tiền Dịch Vụ";
            dgvHoaDon.Columns["TienDichVu"].DefaultCellStyle.Format = "N0";
            dgvHoaDon.Columns["TongTien"].HeaderText = "Tổng Tiền";
            dgvHoaDon.Columns["TongTien"].DefaultCellStyle.Format = "N0";
            dgvHoaDon.Columns["TrangThaiText"].HeaderText = "Trạng Thái";
        }

        private void dgvHoaDon_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int maHD = (int)dgvHoaDon.Rows[e.RowIndex].Cells["MaHD"].Value;
                dgvChiTiet.DataSource = bus.LayChiTietHoaDonDataTable(maHD);
                dgvChiTiet.Columns["MaHD"].Visible = false;
                dgvChiTiet.Columns["MaDV"].HeaderText = "Mã DV";
                dgvChiTiet.Columns["TenDV"].HeaderText = "Tên Dịch Vụ";
                dgvChiTiet.Columns["SoLuong"].HeaderText = "Số Lượng";
                dgvChiTiet.Columns["DonGia"].HeaderText = "Đơn Giá";
                dgvChiTiet.Columns["DonGia"].DefaultCellStyle.Format = "N0";
                dgvChiTiet.Columns["ThanhTien"].HeaderText = "Thành Tiền";
                dgvChiTiet.Columns["ThanhTien"].DefaultCellStyle.Format = "N0";
            }
        }
    }
}