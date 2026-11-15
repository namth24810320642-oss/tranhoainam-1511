using System.Drawing;
using System.Windows.Forms;

namespace GUI
{
    partial class frmThuoc
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblMa, lblTen, lblDVT, lblDonGia, lblTonHienTai, lblNhapThem;
        private TextBox txtMaThuoc, txtTenThuoc;
        private ComboBox cboDonViTinh;
        private NumericUpDown numDonGia, numTonHienTai, numNhapThem;
        private Button btnThemMoi, btnCapNhatKho, btnLamMoi;
        private DataGridView dgvThuoc;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblMa = new Label { Text = "Mã thuốc:", Location = new Point(20, 22), AutoSize = true };
            this.txtMaThuoc = new TextBox { Location = new Point(110, 19), Width = 90, ReadOnly = true };

            this.lblTen = new Label { Text = "Tên thuốc:", Location = new Point(220, 22), AutoSize = true };
            this.txtTenThuoc = new TextBox { Location = new Point(300, 19), Width = 240 };

            this.lblDVT = new Label { Text = "Đơn vị tính:", Location = new Point(560, 22), AutoSize = true };
            this.cboDonViTinh = new ComboBox { Location = new Point(645, 19), Width = 110 };
            this.cboDonViTinh.Items.AddRange(new object[] { "Viên", "Vỉ", "Hộp", "Chai", "Tuýp" });

            this.lblDonGia = new Label { Text = "Đơn giá (VNĐ):", Location = new Point(20, 62), AutoSize = true };
            this.numDonGia = new NumericUpDown { Location = new Point(125, 59), Width = 130, Maximum = 100000000, Increment = 1000 };

            this.lblTonHienTai = new Label { Text = "Tồn hiện tại:", Location = new Point(280, 62), AutoSize = true };
            this.numTonHienTai = new NumericUpDown { Location = new Point(370, 59), Width = 100, Maximum = 100000 };

            this.lblNhapThem = new Label { Text = "Nhập thêm kho:", Location = new Point(500, 62), AutoSize = true, ForeColor = Color.DarkGreen, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            this.numNhapThem = new NumericUpDown { Location = new Point(620, 59), Width = 100, Maximum = 100000 };

            this.btnThemMoi = new Button { Text = "Thêm thuốc mới", Location = new Point(20, 102), Size = new Size(140, 34), BackColor = Color.SteelBlue, ForeColor = Color.White };
            this.btnCapNhatKho = new Button { Text = "Cập nhật Giá && Nhập kho", Location = new Point(175, 102), Size = new Size(190, 34), BackColor = Color.SeaGreen, ForeColor = Color.White };
            this.btnLamMoi = new Button { Text = "Làm mới", Location = new Point(380, 102), Size = new Size(100, 34) };

            this.dgvThuoc = new DataGridView
            {
                Location = new Point(20, 150),
                Size = new Size(735, 270),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ReadOnly = true,
                AllowUserToAddRows = false
            };

            this.btnThemMoi.Click += btnThemMoi_Click;
            this.btnCapNhatKho.Click += btnCapNhatKho_Click;
            this.btnLamMoi.Click += btnLamMoi_Click;
            this.dgvThuoc.CellClick += dgvThuoc_CellClick;
            this.Load += frmThuoc_Load;

            this.ClientSize = new Size(775, 440);
            this.Controls.AddRange(new Control[] {
                lblMa, txtMaThuoc, lblTen, txtTenThuoc, lblDVT, cboDonViTinh,
                lblDonGia, numDonGia, lblTonHienTai, numTonHienTai, lblNhapThem, numNhapThem,
                btnThemMoi, btnCapNhatKho, btnLamMoi, dgvThuoc
            });
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Quản lý Danh mục Thuốc & Nhập kho";
        }
    }
}