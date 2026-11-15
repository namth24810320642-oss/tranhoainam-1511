using System.Drawing;
using System.Windows.Forms;

namespace GUI
{
    partial class frmDauSach
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblMa, lblTen, lblTheLoai, lblSoLuong, lblTim;
        private TextBox txtMaSach, txtTenSach, txtTimKiem;
        private ComboBox cboTheLoai;
        private NumericUpDown numSoLuong;
        private Button btnThem, btnSua, btnXoa, btnLamMoi, btnTimKiem;
        private DataGridView dgvDauSach;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblMa = new Label { Text = "Mã sách:", Location = new Point(20, 22), AutoSize = true };
            this.txtMaSach = new TextBox { Location = new Point(110, 19), Width = 120, ReadOnly = true };

            this.lblTen = new Label { Text = "Tên sách:", Location = new Point(260, 22), AutoSize = true };
            this.txtTenSach = new TextBox { Location = new Point(340, 19), Width = 380 };

            this.lblTheLoai = new Label { Text = "Thể loại:", Location = new Point(20, 60), AutoSize = true };
            this.cboTheLoai = new ComboBox { Location = new Point(110, 57), Width = 200, DropDownStyle = ComboBoxStyle.DropDown };
            this.cboTheLoai.Items.AddRange(new object[] { "Công nghệ thông tin", "Kinh tế", "Văn học", "Khoa học", "Ngoại ngữ" });

            this.lblSoLuong = new Label { Text = "Số lượng còn:", Location = new Point(340, 60), AutoSize = true };
            this.numSoLuong = new NumericUpDown { Location = new Point(440, 58), Width = 100, Minimum = 0, Maximum = 10000 };

            this.btnThem = new Button { Text = "Thêm", Location = new Point(20, 100), Size = new Size(90, 32) };
            this.btnSua = new Button { Text = "Sửa", Location = new Point(120, 100), Size = new Size(90, 32) };
            this.btnXoa = new Button { Text = "Xóa", Location = new Point(220, 100), Size = new Size(90, 32) };
            this.btnLamMoi = new Button { Text = "Làm mới", Location = new Point(320, 100), Size = new Size(90, 32) };

            this.lblTim = new Label { Text = "Tìm kiếm:", Location = new Point(430, 108), AutoSize = true };
            this.txtTimKiem = new TextBox { Location = new Point(500, 105), Width = 140 };
            this.btnTimKiem = new Button { Text = "Tìm", Location = new Point(650, 102), Size = new Size(70, 30) };

            this.dgvDauSach = new DataGridView
            {
                Location = new Point(20, 148),
                Size = new Size(700, 260),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ReadOnly = true,
                AllowUserToAddRows = false
            };

            this.btnThem.Click += btnThem_Click;
            this.btnSua.Click += btnSua_Click;
            this.btnXoa.Click += btnXoa_Click;
            this.btnLamMoi.Click += btnLamMoi_Click;
            this.btnTimKiem.Click += btnTimKiem_Click;
            this.dgvDauSach.CellClick += dgvDauSach_CellClick;
            this.Load += frmDauSach_Load;

            this.ClientSize = new Size(745, 430);
            this.Controls.AddRange(new Control[] {
                lblMa, txtMaSach, lblTen, txtTenSach, lblTheLoai, cboTheLoai,
                lblSoLuong, numSoLuong, btnThem, btnSua, btnXoa, btnLamMoi,
                lblTim, txtTimKiem, btnTimKiem, dgvDauSach
            });
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Quản lý Đầu sách";
        }
    }
}