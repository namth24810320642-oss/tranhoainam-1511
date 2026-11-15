using System.Drawing;
using System.Windows.Forms;

namespace GUI
{
    partial class frmThanhToan
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblLoc, lblChiTiet, lblTongKet;
        private ComboBox cboTrangThai;
        private DataGridView dgvDonThuoc, dgvChiTiet;
        private Button btnThanhToan, btnInHoaDon;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblLoc = new Label { Text = "Lọc trạng thái đơn:", Location = new Point(20, 22), AutoSize = true };
            this.cboTrangThai = new ComboBox { Location = new Point(145, 19), Width = 180, DropDownStyle = ComboBoxStyle.DropDownList };
            this.cboTrangThai.Items.AddRange(new object[] { "Tất cả", "Chờ lấy thuốc", "Đã thanh toán" });

            this.dgvDonThuoc = new DataGridView
            {
                Location = new Point(20, 58),
                Size = new Size(800, 210),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ReadOnly = true,
                AllowUserToAddRows = false
            };

            this.lblChiTiet = new Label { Text = "Chi tiết thuốc trong đơn:", Location = new Point(20, 282), AutoSize = true, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold) };

            this.dgvChiTiet = new DataGridView
            {
                Location = new Point(20, 308),
                Size = new Size(800, 150),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false
            };

            this.lblTongKet = new Label
            {
                Text = "Chọn một đơn thuốc để xem chi tiết thanh toán",
                Location = new Point(20, 475),
                AutoSize = true,
                Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                ForeColor = Color.DarkBlue
            };

            this.btnThanhToan = new Button
            {
                Text = "XÁC NHẬN THANH TOÁN",
                Location = new Point(465, 470),
                Size = new Size(195, 42),
                BackColor = Color.SeaGreen,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold)
            };

            this.btnInHoaDon = new Button
            {
                Text = "IN HÓA ĐƠN",
                Location = new Point(675, 470),
                Size = new Size(145, 42),
                BackColor = Color.SteelBlue,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold)
            };

            this.cboTrangThai.SelectedIndexChanged += (s, e) => LoadDanhSachDon();
            this.dgvDonThuoc.CellClick += dgvDonThuoc_CellClick;
            this.btnThanhToan.Click += btnThanhToan_Click;
            this.btnInHoaDon.Click += btnInHoaDon_Click;
            this.Load += frmThanhToan_Load;

            this.ClientSize = new Size(840, 530);
            this.Controls.AddRange(new Control[] {
                lblLoc, cboTrangThai, dgvDonThuoc, lblChiTiet, dgvChiTiet, lblTongKet, btnThanhToan, btnInHoaDon
            });
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Thanh toán Viện phí & In Hóa đơn";
        }
    }
}