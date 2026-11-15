using System.Drawing;
using System.Windows.Forms;

namespace GUI
{
    partial class frmMenuDichVu
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblMa, lblTen, lblGia, lblTon;
        private TextBox txtMaDV, txtTenDV;
        private NumericUpDown numDonGia, numTonKho;
        private CheckBox chkDichVuThue;
        private Button btnThem, btnSua, btnXoa, btnLamMoi;
        private DataGridView dgvDichVu;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblMa = new Label { Text = "Mã DV:", Location = new Point(20, 22), AutoSize = true };
            this.txtMaDV = new TextBox { Location = new Point(95, 19), Width = 85, ReadOnly = true };

            this.lblTen = new Label { Text = "Tên dịch vụ:", Location = new Point(200, 22), AutoSize = true };
            this.txtTenDV = new TextBox { Location = new Point(290, 19), Width = 390 };

            this.lblGia = new Label { Text = "Đơn giá:", Location = new Point(20, 60), AutoSize = true };
            this.numDonGia = new NumericUpDown { Location = new Point(95, 57), Width = 140, Maximum = 10000000, Increment = 1000 };

            this.lblTon = new Label { Text = "Tồn kho:", Location = new Point(260, 60), AutoSize = true };
            this.numTonKho = new NumericUpDown { Location = new Point(325, 57), Width = 100, Maximum = 100000 };

            this.chkDichVuThue = new CheckBox
            {
                Text = "Dịch vụ cho thuê (Không trừ kho)",
                Location = new Point(445, 59),
                AutoSize = true
            };

            this.btnThem = new Button { Text = "Thêm mới", Location = new Point(20, 100), Size = new Size(100, 32) };
            this.btnSua = new Button { Text = "Sửa", Location = new Point(130, 100), Size = new Size(90, 32) };
            this.btnXoa = new Button { Text = "Xóa", Location = new Point(230, 100), Size = new Size(90, 32) };
            this.btnLamMoi = new Button { Text = "Làm mới", Location = new Point(330, 100), Size = new Size(90, 32) };

            this.dgvDichVu = new DataGridView
            {
                Location = new Point(20, 148),
                Size = new Size(660, 250),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ReadOnly = true,
                AllowUserToAddRows = false
            };

            this.chkDichVuThue.CheckedChanged += (s, e) => numTonKho.Enabled = !chkDichVuThue.Checked;
            this.btnThem.Click += btnThem_Click;
            this.btnSua.Click += btnSua_Click;
            this.btnXoa.Click += btnXoa_Click;
            this.btnLamMoi.Click += btnLamMoi_Click;
            this.dgvDichVu.CellClick += dgvDichVu_CellClick;
            this.Load += frmMenuDichVu_Load;

            this.ClientSize = new Size(705, 420);
            this.Controls.AddRange(new Control[] {
                lblMa, txtMaDV, lblTen, txtTenDV, lblGia, numDonGia, lblTon, numTonKho,
                chkDichVuThue, btnThem, btnSua, btnXoa, btnLamMoi, dgvDichVu
            });
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Quản lý Menu Dịch vụ & Kho";
        }
    }
}