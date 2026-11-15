using System;
using System.Windows.Forms;
using BUS;
using DTO;

namespace GUI
{
    public partial class frmDocGia : Form
    {
        private readonly DocGiaBUS bus = new DocGiaBUS();

        public frmDocGia()
        {
            InitializeComponent();
        }

        private void frmDocGia_Load(object sender, EventArgs e)
        {
            LoadData();
            dtpNgayHetHan.Value = DateTime.Now.AddYears(1);
        }

        private void LoadData()
        {
            dgvDocGia.DataSource = bus.LayDanhSachDocGia();
            dgvDocGia.Columns["MaDocGia"].HeaderText = "Mã Độc Giả";
            dgvDocGia.Columns["HoTen"].HeaderText = "Họ Tên";
            dgvDocGia.Columns["Email"].HeaderText = "Email";
            dgvDocGia.Columns["NgayHetHanThe"].HeaderText = "Ngày Hết Hạn Thẻ";
            dgvDocGia.Columns["NgayHetHanThe"].DefaultCellStyle.Format = "dd/MM/yyyy";
        }

        private void ClearInput()
        {
            txtMaDocGia.Clear();
            txtHoTen.Clear();
            txtEmail.Clear();
            dtpNgayHetHan.Value = DateTime.Now.AddYears(1);
        }

        private void dgvDocGia_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvDocGia.Rows[e.RowIndex];
                txtMaDocGia.Text = row.Cells["MaDocGia"].Value.ToString();
                txtHoTen.Text = row.Cells["HoTen"].Value.ToString();
                txtEmail.Text = row.Cells["Email"].Value.ToString();
                dtpNgayHetHan.Value = Convert.ToDateTime(row.Cells["NgayHetHanThe"].Value);
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                DocGiaDTO dg = new DocGiaDTO
                {
                    HoTen = txtHoTen.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    NgayHetHanThe = dtpNgayHetHan.Value
                };
                if (bus.ThemDocGia(dg))
                {
                    MessageBox.Show("Thêm độc giả thành công!", "Thông báo");
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
                int.TryParse(txtMaDocGia.Text, out int ma);
                DocGiaDTO dg = new DocGiaDTO
                {
                    MaDocGia = ma,
                    HoTen = txtHoTen.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    NgayHetHanThe = dtpNgayHetHan.Value
                };
                if (bus.SuaDocGia(dg))
                {
                    MessageBox.Show("Cập nhật độc giả thành công!", "Thông báo");
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
                int.TryParse(txtMaDocGia.Text, out int ma);
                if (MessageBox.Show("Xác nhận xóa độc giả này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (bus.XoaDocGia(ma))
                    {
                        MessageBox.Show("Xóa thành công!", "Thông báo");
                        LoadData();
                        ClearInput();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể xóa (độc giả đã có phiếu mượn): " + ex.Message, "Lỗi");
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ClearInput();
            LoadData();
        }
    }
}