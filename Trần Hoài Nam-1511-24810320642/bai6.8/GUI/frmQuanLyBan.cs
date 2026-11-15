using System;
using System.Windows.Forms;
using BUS;
using DTO;

namespace GUI
{
    public partial class frmQuanLyBan : Form
    {
        private readonly BanBidaBUS bus = new BanBidaBUS();

        public frmQuanLyBan()
        {
            InitializeComponent();
        }

        private void frmQuanLyBan_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            dgvBanBida.DataSource = bus.LayDanhSachBan();
            dgvBanBida.Columns["MaBan"].HeaderText = "Mã Bàn";
            dgvBanBida.Columns["TenBan"].HeaderText = "Tên Bàn";
            dgvBanBida.Columns["LoaiBan"].HeaderText = "Loại Bàn";
            dgvBanBida.Columns["GiaTheoGio"].HeaderText = "Giá Theo Giờ";
            dgvBanBida.Columns["GiaTheoGio"].DefaultCellStyle.Format = "N0";
            dgvBanBida.Columns["TrangThai"].HeaderText = "Trạng Thái";
        }

        private void ClearInput()
        {
            txtMaBan.Clear();
            txtTenBan.Clear();
            cboLoaiBan.SelectedIndex = 0;
            numGiaGio.Value = 60000;
        }

        private void dgvBanBida_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvBanBida.Rows[e.RowIndex];
                txtMaBan.Text = row.Cells["MaBan"].Value.ToString();
                txtTenBan.Text = row.Cells["TenBan"].Value.ToString();
                cboLoaiBan.Text = row.Cells["LoaiBan"].Value.ToString();
                numGiaGio.Value = Convert.ToDecimal(row.Cells["GiaTheoGio"].Value);
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                BanBidaDTO ban = new BanBidaDTO
                {
                    TenBan = txtTenBan.Text.Trim(),
                    LoaiBan = cboLoaiBan.Text.Trim(),
                    GiaTheoGio = numGiaGio.Value
                };
                if (bus.ThemBan(ban))
                {
                    MessageBox.Show("Thêm bàn mới thành công!", "Thông báo");
                    LoadData();
                    ClearInput();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                int.TryParse(txtMaBan.Text, out int ma);
                BanBidaDTO ban = new BanBidaDTO
                {
                    MaBan = ma,
                    TenBan = txtTenBan.Text.Trim(),
                    LoaiBan = cboLoaiBan.Text.Trim(),
                    GiaTheoGio = numGiaGio.Value
                };
                if (bus.SuaBan(ban))
                {
                    MessageBox.Show("Cập nhật bàn thành công!", "Thông báo");
                    LoadData();
                    ClearInput();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvBanBida.CurrentRow == null) return;
                int.TryParse(txtMaBan.Text, out int ma);
                string trangThai = dgvBanBida.CurrentRow.Cells["TrangThai"].Value.ToString();

                if (MessageBox.Show("Xác nhận xóa bàn này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (bus.XoaBan(ma, trangThai))
                    {
                        MessageBox.Show("Xóa bàn thành công!", "Thông báo");
                        LoadData();
                        ClearInput();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể xóa bàn: " + ex.Message, "Lỗi");
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ClearInput();
            LoadData();
        }
    }
}