using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BUS;
using DTO;

namespace GUI
{
    public partial class frmKeDon : Form
    {
        private readonly HoSoKhamBUS hsBus = new HoSoKhamBUS();
        private readonly ThuocBUS thuocBus = new ThuocBUS();
        private readonly DonThuocBUS donBus = new DonThuocBUS();
        private List<ChiTietDonThuocDTO> danhSachKeDon = new List<ChiTietDonThuocDTO>();

        public frmKeDon()
        {
            InitializeComponent();
        }

        private void frmKeDon_Load(object sender, EventArgs e)
        {
            LoadCombos();
            RefreshGrid();
        }

        private void LoadCombos()
        {
            cboHoSoKham.DataSource = hsBus.LayDanhSachHoSo();
            cboHoSoKham.DisplayMember = "ThongTinHienThi";
            cboHoSoKham.ValueMember = "MaHoSo";

            cboThuoc.DataSource = thuocBus.LayDanhSachThuoc();
            cboThuoc.DisplayMember = "ThongTinHienThi";
            cboThuoc.ValueMember = "MaThuoc";
        }

        private void cboHoSoKham_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboHoSoKham.SelectedItem is HoSoKhamDTO hs)
            {
                txtTrieuChung.Text = hs.TrieuChung;
                txtChanDoan.Text = hs.ChanDoan;
            }
        }

        private void RefreshGrid()
        {
            dgvChiTietDon.DataSource = null;
            dgvChiTietDon.DataSource = danhSachKeDon.Select(x => new
            {
                x.MaThuoc,
                x.TenThuoc,
                x.DonViTinh,
                x.SoLuong,
                DonGia = x.DonGia.ToString("N0"),
                ThanhTien = x.ThanhTien.ToString("N0"),
                x.LieuDung
            }).ToList();

            CapNhatTongTien();
        }

        private void CapNhatTongTien()
        {
            decimal tienThuoc = danhSachKeDon.Sum(x => x.ThanhTien);
            decimal tong = numTienKham.Value + tienThuoc;
            lblTongTamTinh.Text = $"Tổng chi phí dự kiến: {tong:N0} VNĐ (Khám: {numTienKham.Value:N0}đ + Thuốc: {tienThuoc:N0}đ)";
        }

        private void btnThemThuoc_Click(object sender, EventArgs e)
        {
            if (cboThuoc.SelectedItem is ThuocDTO t)
            {
                int sl = (int)numSoLuong.Value;
                var tonTai = danhSachKeDon.FirstOrDefault(x => x.MaThuoc == t.MaThuoc);
                if (tonTai != null)
                {
                    tonTai.SoLuong += sl;
                    tonTai.LieuDung = txtLieuDung.Text.Trim();
                }
                else
                {
                    danhSachKeDon.Add(new ChiTietDonThuocDTO
                    {
                        MaThuoc = t.MaThuoc,
                        TenThuoc = t.TenThuoc,
                        DonViTinh = t.DonViTinh,
                        SoLuong = sl,
                        DonGia = t.DonGia,
                        LieuDung = txtLieuDung.Text.Trim()
                    });
                }
                RefreshGrid();
            }
        }

        private void btnXoaDong_Click(object sender, EventArgs e)
        {
            if (dgvChiTietDon.CurrentRow != null)
            {
                int maThuoc = (int)dgvChiTietDon.CurrentRow.Cells["MaThuoc"].Value;
                danhSachKeDon.RemoveAll(x => x.MaThuoc == maThuoc);
                RefreshGrid();
            }
        }

        private void btnLuuXuatDon_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboHoSoKham.SelectedValue == null) return;
                int maHoSo = (int)cboHoSoKham.SelectedValue;

                int maDonMoi = donBus.LuuVaXuatDonThuoc(maHoSo, txtChanDoan.Text.Trim(), numTienKham.Value, danhSachKeDon);

                MessageBox.Show($"Lưu & xuất đơn thuốc #{maDonMoi} thành công! Đã trừ tồn kho dược phẩm.", "Thành công");
                danhSachKeDon.Clear();
                RefreshGrid();
                LoadCombos(); // Tải lại tồn kho mới trên ComboBox
            }
            catch (Exception ex)
            {
                // Hiển thị cảnh báo khi SqlTransaction thực hiện Rollback do hết thuốc trong kho
                MessageBox.Show("Giao dịch đã được hoàn tác (Rollback):\n" + ex.Message, "Cảnh báo Tồn kho", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}