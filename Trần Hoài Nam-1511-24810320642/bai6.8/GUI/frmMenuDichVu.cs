using System;
using System.Windows.Forms;
using BUS;
using DTO;

namespace GUI
{
    public partial class frmMenuDichVu : Form
    {
        private readonly MenuDichVuBUS bus = new MenuDichVuBUS();

        public frmMenuDichVu()
        {
            InitializeComponent();
        }

        private void frmMenuDichVu_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            dgvDichVu.DataSource = bus.LayDanhSachDichVu();
            dgvDichVu.Columns["ThongTinHienThi"].Visible = false;
            dgvDichVu.Columns["MaDV"].HeaderText = "Mã DV";
            dgvDichVu.Columns["TenDV"].HeaderText = "Tên Dịch Vụ / Món";
            dgvDichVu.Columns["DonGia"].HeaderText = "Đơn Giá";
            dgvDichVu.Columns["DonGia"].DefaultCellStyle.Format = "N0";
            dgvDichVu.Columns["TonKho"].HeaderText = "Tồn Kho (Trống = Cho thuê)";
        }

        private void ClearInput()
        {
            txtMaDV.Clear();
            txtTenDV.Clear();
            numDonGia.Value = 0;
            numTonKho.Value = 0;
            chkDichVuThue.Checked = false;
        }

        private void dgvDichVu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvDichVu.Rows[e.RowIndex].DataBoundItem is MenuDichVuDTO dv)
            {
                txtMaDV.Text = dv.MaDV.ToString();
                txtTenDV.Text = dv.TenDV;
                numDonGia.Value = dv.DonGia;
                if (dv.TonKho.HasValue)
                {
                    chkDichVuThue.Checked = false;
                    numTonKho.Value = dv.TonKho.Value;
                }
                else
                {
                    chkDichVuThue.Checked = true;
                    numTonKho.Value = 0;
                }
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                MenuDichVuDTO dv = new MenuDichVuDTO
                {
                    TenDV = txtTenDV.Text.Trim(),
                    DonGia = numDonGia.Value,
                    TonKho = chkDichVuThue.Checked ? (int?)null : (int)numTonKho.Value
                };
                if (bus.ThemDichVu(dv))
                {
                    MessageBox.Show("Thêm dịch vụ mới thành công!", "Thông báo");
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
                int.TryParse(txtMaDV.Text, out int ma);
                MenuDichVuDTO dv = new MenuDichVuDTO
                {
                    MaDV = ma,
                    TenDV = txtTenDV.Text.Trim(),
                    DonGia = numDonGia.Value,
                    TonKho = chkDichVuThue.Checked ? (int?)null : (int)numTonKho.Value
                };
                if (bus.SuaDichVu(dv))
                {
                    MessageBox.Show("Cập nhật dịch vụ thành công!", "Thông báo");
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
                int.TryParse(txtMaDV.Text, out int ma);
                if (MessageBox.Show("Xác nhận xóa dịch vụ này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (bus.XoaDichVu(ma))
                    {
                        MessageBox.Show("Xóa thành công!", "Thông báo");
                        LoadData();
                        ClearInput();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể xóa (đã có trong hóa đơn): " + ex.Message, "Lỗi");
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ClearInput();
            LoadData();
        }
    }
}