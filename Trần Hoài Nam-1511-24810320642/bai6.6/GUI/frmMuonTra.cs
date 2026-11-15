using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BUS;
using DTO;

namespace GUI
{
    public partial class frmMuonTra : Form
    {
        private readonly DocGiaBUS dgBus = new DocGiaBUS();
        private readonly DauSachBUS dsBus = new DauSachBUS();
        private readonly PhieuMuonBUS pmBus = new PhieuMuonBUS();
        private List<ChiTietMuonDTO> gioSach = new List<ChiTietMuonDTO>();

        public frmMuonTra()
        {
            InitializeComponent();
        }

        private void frmMuonTra_Load(object sender, EventArgs e)
        {
            dtpHanTra.Value = DateTime.Now.AddDays(7);
            LoadCombos();
            cboLocTrangThai.SelectedIndex = 0;
            LoadDanhSachPhieu();
        }

        private void LoadCombos()
        {
            var dsDocGia = dgBus.LayDanhSachDocGia();
            cboDocGiaMuon.DataSource = new List<DocGiaDTO>(dsDocGia);
            cboDocGiaMuon.DisplayMember = "HoTen";
            cboDocGiaMuon.ValueMember = "MaDocGia";

            var dsLocDG = new List<DocGiaDTO> { new DocGiaDTO { MaDocGia = 0, HoTen = "-- Tất cả độc giả --" } };
            dsLocDG.AddRange(dsDocGia);
            cboLocDocGia.DataSource = dsLocDG;
            cboLocDocGia.DisplayMember = "HoTen";
            cboLocDocGia.ValueMember = "MaDocGia";

            cboSachMuon.DataSource = dsBus.LayDanhSachDauSach();
            cboSachMuon.DisplayMember = "TenSach";
            cboSachMuon.ValueMember = "MaSach";
        }

        private void RefreshGioSach()
        {
            dgvGioSach.DataSource = null;
            dgvGioSach.DataSource = gioSach.Select(x => new { x.MaSach, x.TenSach, x.SoLuong }).ToList();
        }

        private void LoadDanhSachPhieu()
        {
            int maDG = cboLocDocGia.SelectedValue != null ? (int)cboLocDocGia.SelectedValue : 0;
            string tt = cboLocTrangThai.SelectedItem != null ? cboLocTrangThai.SelectedItem.ToString() : "Tất cả";

            dgvPhieuMuon.DataSource = pmBus.LocPhieuMuon(maDG, tt);
            dgvPhieuMuon.Columns["MaDocGia"].Visible = false;
            dgvPhieuMuon.Columns["MaPhieu"].HeaderText = "Mã Phiếu";
            dgvPhieuMuon.Columns["TenDocGia"].HeaderText = "Độc Giả";
            dgvPhieuMuon.Columns["NgayMuon"].HeaderText = "Ngày Mượn";
            dgvPhieuMuon.Columns["HanTra"].HeaderText = "Hạn Trả";
            dgvPhieuMuon.Columns["TrangThai"].HeaderText = "Trạng Thái";
        }

        private void btnThemSach_Click(object sender, EventArgs e)
        {
            if (cboSachMuon.SelectedItem is DauSachDTO sach)
            {
                int sl = (int)numSoLuongMuon.Value;
                var tonTai = gioSach.FirstOrDefault(x => x.MaSach == sach.MaSach);
                int tongSL = (tonTai?.SoLuong ?? 0) + sl;

                if (tongSL > sach.SoLuongCon)
                {
                    MessageBox.Show($"Sách '{sach.TenSach}' chỉ còn {sach.SoLuongCon} cuốn trong kho!", "Cảnh báo");
                    return;
                }

                if (tonTai != null) tonTai.SoLuong = tongSL;
                else gioSach.Add(new ChiTietMuonDTO { MaSach = sach.MaSach, TenSach = sach.TenSach, SoLuong = sl });

                RefreshGioSach();
            }
        }

        private void btnLapPhieu_Click(object sender, EventArgs e)
        {
            try
            {
                PhieuMuonDTO pm = new PhieuMuonDTO
                {
                    MaDocGia = (int)cboDocGiaMuon.SelectedValue,
                    HanTra = dtpHanTra.Value
                };

                if (pmBus.LapPhieuMuon(pm, gioSach))
                {
                    MessageBox.Show("Lập phiếu mượn thành công!", "Thông báo");
                    gioSach.Clear();
                    RefreshGioSach();
                    LoadCombos();
                    LoadDanhSachPhieu();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Không thể lập phiếu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnLoc_Click(object sender, EventArgs e)
        {
            LoadDanhSachPhieu();
        }

        private void dgvPhieuMuon_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int maPhieu = (int)dgvPhieuMuon.Rows[e.RowIndex].Cells["MaPhieu"].Value;
                dgvChiTietMuon.DataSource = pmBus.LayChiTietPhieu(maPhieu);
                dgvChiTietMuon.Columns["MaPhieu"].Visible = false;
                dgvChiTietMuon.Columns["MaSach"].HeaderText = "Mã Sách";
                dgvChiTietMuon.Columns["TenSach"].HeaderText = "Tên Sách";
                dgvChiTietMuon.Columns["SoLuong"].HeaderText = "Số Lượng Mượn";
            }
        }

        private void btnTraSach_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvPhieuMuon.CurrentRow == null) return;
                int maPhieu = (int)dgvPhieuMuon.CurrentRow.Cells["MaPhieu"].Value;
                string trangThai = dgvPhieuMuon.CurrentRow.Cells["TrangThai"].Value.ToString();

                if (pmBus.TraSach(maPhieu, trangThai))
                {
                    MessageBox.Show("Trả sách thành công! Đã cộng lại số lượng sách vào kho.", "Thông báo");
                    LoadCombos();
                    LoadDanhSachPhieu();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}