using System.Drawing;
using System.Windows.Forms;

namespace GUI
{
    partial class frmDocGia
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblMa, lblHoTen, lblEmail, lblHetHan;
        private TextBox txtMaDocGia, txtHoTen, txtEmail;
        private DateTimePicker dtpNgayHetHan;
        private Button btnThem, btnSua, btnXoa, btnLamMoi;
        private DataGridView dgvDocGia;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblMa = new Label { Text = "Mã độc giả:", Location = new Point(20, 22), AutoSize = true };
            this.txtMaDocGia = new TextBox { Location = new Point(125, 19), Width = 110, ReadOnly = true };

            this.lblHoTen = new Label { Text = "Họ và tên:", Location = new Point(260, 22), AutoSize = true };
            this.txtHoTen = new TextBox { Location = new Point(340, 19), Width = 360 };

            this.lblEmail = new Label { Text = "Email:", Location = new Point(20, 60), AutoSize = true };
            this.txtEmail = new TextBox { Location = new Point(125, 57), Width = 260 };

            this.lblHetHan = new Label { Text = "Ngày hết hạn thẻ:", Location = new Point(405, 60), AutoSize = true };
            this.dtpNgayHetHan = new DateTimePicker { Location = new Point(530, 57), Width = 170, Format = DateTimePickerFormat.Short };

            this.btnThem = new Button { Text = "Thêm", Location = new Point(20, 100), Size = new Size(95, 32) };
            this.btnSua = new Button { Text = "Sửa", Location = new Point(125, 100), Size = new Size(95, 32) };
            this.btnXoa = new Button { Text = "Xóa", Location = new Point(230, 100), Size = new Size(95, 32) };
            this.btnLamMoi = new Button { Text = "Làm mới", Location = new Point(335, 100), Size = new Size(95, 32) };

            this.dgvDocGia = new DataGridView
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
            this.dgvDocGia.CellClick += dgvDocGia_CellClick;
            this.Load += frmDocGia_Load;

            this.ClientSize = new Size(725, 420);
            this.Controls.AddRange(new Control[] {
                lblMa, txtMaDocGia, lblHoTen, txtHoTen, lblEmail, txtEmail,
                lblHetHan, dtpNgayHetHan, btnThem, btnSua, btnXoa, btnLamMoi, dgvDocGia
            });
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Quản lý Độc giả";
        }
    }
}