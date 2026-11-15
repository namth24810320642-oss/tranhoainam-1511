using System;
using System.Drawing;
using System.Windows.Forms;
using BUS;

namespace GUI
{
    public partial class frmThongKe : Form
    {
        private readonly PhieuMuonBUS pmBus = new PhieuMuonBUS();

        public frmThongKe()
        {
            InitializeComponent();
        }

        private void frmThongKe_Load(object sender, EventArgs e)
        {
            LoadBaoCao();
        }

        private void LoadBaoCao()
        {
            // 1. Đếm số phiếu quá hạn (ExecuteScalar) & Danh sách quá hạn (SqlDataReader)
            int tongQuaHan = pmBus.DemSoPhieuQuaHan();
            lblCanhBao.Text = $"CẢNH BÁO: Hiện có {tongQuaHan} phiếu mượn đã vượt quá hạn trả!";

            dgvQuaHan.DataSource = pmBus.LayDanhSachQuaHan();
            dgvQuaHan.Columns["MaDocGia"].Visible = false;
            dgvQuaHan.Columns["MaPhieu"].HeaderText = "Mã Phiếu";
            dgvQuaHan.Columns["TenDocGia"].HeaderText = "Độc Giả";
            dgvQuaHan.Columns["NgayMuon"].HeaderText = "Ngày Mượn";
            dgvQuaHan.Columns["HanTra"].HeaderText = "Hạn Trả";
            dgvQuaHan.Columns["TrangThai"].HeaderText = "Trạng Thái";
            dgvQuaHan.DefaultCellStyle.ForeColor = Color.DarkRed;

            // 2. Thống kê theo thể loại (GROUP BY)
            dgvThongKeTheLoai.DataSource = pmBus.ThongKeTheoTheLoai();
            dgvThongKeTheLoai.Columns["TheLoai"].HeaderText = "Thể Loại Sách";
            dgvThongKeTheLoai.Columns["SoLuotMuon"].HeaderText = "Tổng Số Phiếu Mượn";
            dgvThongKeTheLoai.Columns["TongSachMuon"].HeaderText = "Tổng Số Cuốn Sách Được Mượn";
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LoadBaoCao();
        }
    }
}