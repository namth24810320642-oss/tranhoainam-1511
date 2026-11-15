using System;
using System.Windows.Forms;
using BUS;
using DTO;

namespace GUI
{
    public partial class frmDauSach : Form
    {
        private readonly DauSachBUS bus = new DauSachBUS();

        public frmDauSach()
        {
            InitializeComponent();
        }

        private void frmDauSach_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            dgvDauSach.DataSource = bus.LayDanhSachDauSach();
            dgvDauSach.Columns["MaSach"].HeaderText = "Mã Sách";
            dgvDauSach.Columns["TenSach"].HeaderText = "Tên Sách";
            dgvDauSach.Columns["TheLoai"].HeaderText = "Thể Loại";
            dgvDauSach.Columns["SoLuongCon"].HeaderText = "Số Lượng Còn";
        }

        private void ClearInput()
        {
            txtMaSach.Clear();
            txtTenSach.Clear();
            cboTheLoai.SelectedIndex = -1;
            numSoLuong.Value = 0;
        }

        private void dgvDauSach_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvDauSach.Rows[e.RowIndex];
                txtMaSach.Text = row.Cells["MaSach"].Value.ToString();
                txtTenSach.Text = row.Cells["TenSach"].Value.ToString();
                cboTheLoai.Text = row.Cells["TheLoai"].Value.ToString();
                numSoLuong.Value = Convert.ToDecimal(row.Cells["SoLuongCon"].Value);
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                DauSachDTO ds = new DauSachDTO
                {
                    TenSach = txtTenSach.Text.Trim(),
                    TheLoai = cboTheLoai.Text.Trim(),
                    SoLuongCon = (int)numSoLuong.Value
                };
                if (bus.ThemDauSach(ds))
                {
                    MessageBox.Show("Thêm đầu sách thành công!", "Thông báo");
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
                int.TryParse(txtMaSach.Text, out int ma);
                DauSachDTO ds = new DauSachDTO
                {
                    MaSach = ma,
                    TenSach = txtTenSach.Text.Trim(),
                    TheLoai = cboTheLoai.Text.Trim(),
                    SoLuongCon = (int)numSoLuong.Value
                };
                if (bus.SuaDauSach(ds))
                {
                    MessageBox.Show("Cập nhật đầu sách thành công!", "Thông báo");
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
                int.TryParse(txtMaSach.Text, out int ma);
                if (MessageBox.Show("Bạn có chắc muốn xóa đầu sách này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (bus.XoaDauSach(ma))
                    {
                        MessageBox.Show("Xóa thành công!", "Thông báo");
                        LoadData();
                        ClearInput();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể xóa (sách đang có dữ liệu mượn): " + ex.Message, "Lỗi");
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ClearInput();
            LoadData();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            dgvDauSach.DataSource = bus.TimKiemDauSach(txtTimKiem.Text.Trim());
        }
    }
}