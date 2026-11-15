using System;
using System.Windows.Forms;
using BUS;
using DTO;

namespace GUI
{
    public partial class frmThuoc : Form
    {
        private readonly ThuocBUS bus = new ThuocBUS();

        public frmThuoc()
        {
            InitializeComponent();
        }

        private void frmThuoc_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            dgvThuoc.DataSource = bus.LayDanhSachThuoc();
            dgvThuoc.Columns["ThongTinHienThi"].Visible = false;
            dgvThuoc.Columns["MaThuoc"].HeaderText = "Mã Thuốc";
            dgvThuoc.Columns["TenThuoc"].HeaderText = "Tên Thuốc";
            dgvThuoc.Columns["DonViTinh"].HeaderText = "Đơn Vị Tính";
            dgvThuoc.Columns["DonGia"].HeaderText = "Đơn Giá";
            dgvThuoc.Columns["DonGia"].DefaultCellStyle.Format = "N0";
            dgvThuoc.Columns["SoLuongTon"].HeaderText = "Số Lượng Tồn";
        }

        private void ClearInput()
        {
            txtMaThuoc.Clear();
            txtTenThuoc.Clear();
            cboDonViTinh.SelectedIndex = -1;
            numDonGia.Value = 0;
            numTonHienTai.Value = 0;
            numTonHienTai.Enabled = true;
            numNhapThem.Value = 0;
        }

        private void dgvThuoc_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvThuoc.Rows[e.RowIndex];
                txtMaThuoc.Text = row.Cells["MaThuoc"].Value.ToString();
                txtTenThuoc.Text = row.Cells["TenThuoc"].Value.ToString();
                cboDonViTinh.Text = row.Cells["DonViTinh"].Value.ToString();
                numDonGia.Value = Convert.ToDecimal(row.Cells["DonGia"].Value);
                numTonHienTai.Value = Convert.ToDecimal(row.Cells["SoLuongTon"].Value);
                numTonHienTai.Enabled = false;
                numNhapThem.Value = 0;
            }
        }

        private void btnThemMoi_Click(object sender, EventArgs e)
        {
            try
            {
                ThuocDTO t = new ThuocDTO
                {
                    TenThuoc = txtTenThuoc.Text.Trim(),
                    DonViTinh = cboDonViTinh.Text.Trim(),
                    DonGia = numDonGia.Value,
                    SoLuongTon = (int)numTonHienTai.Value
                };

                if (bus.ThemThuocMoi(t))
                {
                    MessageBox.Show("Thêm thuốc mới thành công!", "Thông báo");
                    LoadData();
                    ClearInput();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnCapNhatKho_Click(object sender, EventArgs e)
        {
            try
            {
                int.TryParse(txtMaThuoc.Text, out int maThuoc);
                if (bus.CapNhatGiaVaNhapKho(maThuoc, txtTenThuoc.Text.Trim(), cboDonViTinh.Text.Trim(), numDonGia.Value, (int)numNhapThem.Value))
                {
                    MessageBox.Show("Cập nhật đơn giá và nhập thêm tồn kho thành công!", "Thông báo");
                    LoadData();
                    ClearInput();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ClearInput();
            LoadData();
        }
    }
}