using System.Drawing;
using System.Windows.Forms;

namespace GUI
{
    partial class frmQuanLyBan
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblMa, lblTen, lblLoai, lblGia;
        private TextBox txtMaBan, txtTenBan;
        private ComboBox cboLoaiBan;
        private NumericUpDown numGiaGio;
        private Button btnThem, btnSua, btnXoa, btnLamMoi;
        private DataGridView dgvBanBida;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblMa = new Label { Text = "Mã bàn:", Location = new Point(20, 22), AutoSize = true };
            this.txtMaBan = new TextBox { Location = new Point(100, 19), Width = 90, ReadOnly = true };

            this.lblTen = new Label { Text = "Tên bàn:", Location = new Point(215, 22), AutoSize = true };
            this.txtTenBan = new TextBox { Location = new Point(285, 19), Width = 220 };

            this.lblLoai = new Label { Text = "Loại bàn:", Location = new Point(20, 60), AutoSize = true };
            this.cboLoaiBan = new ComboBox { Location = new Point(100, 57), Width = 180 };
            this.cboLoaiBan.Items.AddRange(new object[] { "Pool (Lỗ)", "Carom (Phăng)", "Snooker" });

            this.lblGia = new Label { Text = "Giá theo giờ (VNĐ):", Location = new Point(305, 60), AutoSize = true };
            this.numGiaGio = new NumericUpDown { Location = new Point(435, 57), Width = 140, Maximum = 10000000, Increment = 5000, Value = 60000 };

            this.btnThem = new Button { Text = "Thêm bàn", Location = new Point(20, 100), Size = new Size(100, 32) };
            this.btnSua = new Button { Text = "Sửa", Location = new Point(130, 100), Size = new Size(90, 32) };
            this.btnXoa = new Button { Text = "Xóa", Location = new Point(230, 100), Size = new Size(90, 32) };
            this.btnLamMoi = new Button { Text = "Làm mới", Location = new Point(330, 100), Size = new Size(90, 32) };

            this.dgvBanBida = new DataGridView
            {
                Location = new Point(20, 148),
                Size = new Size(680, 250),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ReadOnly = true,
                AllowUserToAddRows = false
            };

            this.btnThem.Click += btnThem_Click;
            this.btnSua.Click += btnSua_Click;
            this.btnXoa.Click += btnXoa_Click;
            this.btnLamMoi.Click += btnLamMoi_Click;
            this.dgvBanBida.CellClick += dgvBanBida_CellClick;
            this.Load += frmQuanLyBan_Load;

            this.ClientSize = new Size(725, 420);
            this.Controls.AddRange(new Control[] {
                lblMa, txtMaBan, txtTenBan, lblTen, lblLoai, cboLoaiBan,
                lblGia, numGiaGio, btnThem, btnSua, btnXoa, btnLamMoi, dgvBanBida
            });
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Quản lý Danh mục Bàn Bida";
        }
    }
}