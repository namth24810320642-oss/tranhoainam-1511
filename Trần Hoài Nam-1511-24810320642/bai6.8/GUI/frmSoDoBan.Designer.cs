using System.Drawing;
using System.Windows.Forms;

namespace GUI
{
    partial class frmSoDoBan
    {
        private System.ComponentModel.IContainer components = null;
        private GroupBox grpSoDoBan, grpChiTiet;
        private FlowLayoutPanel flpBanBida;
        private Label lblChuThich, lblTenBanChon, lblThongTinGio, lblChonDV, lblSL, lblTienGio, lblTienDV, lblTongCong;
        private Button btnMoBan, btnThemMon, btnGiamMon, btnThanhToan;
        private ComboBox cboDichVu;
        private NumericUpDown numSoLuong;
        private DataGridView dgvChiTietHD;
        private System.Windows.Forms.Timer tmrTinhGio;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            // GroupBox 1: Sơ đồ bàn trực quan (Visual Grid Layout)
            this.grpSoDoBan = new GroupBox { Text = "1. Sơ đồ Bàn Bida Trực quan", Location = new Point(15, 15), Size = new Size(470, 520) };

            this.flpBanBida = new FlowLayoutPanel
            {
                Location = new Point(15, 28),
                Size = new Size(440, 415),
                AutoScroll = true,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.WhiteSmoke
            };

            this.lblChuThich = new Label
            {
                Text = "🟢 Xanh lá: Bàn Trống   |   🔴 Cam/Đỏ: Bàn Đang chơi",
                Location = new Point(15, 452),
                AutoSize = true,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };

            this.btnMoBan = new Button
            {
                Text = "MỞ BÀN (Bắt đầu tính giờ)",
                Location = new Point(15, 476),
                Size = new Size(440, 34),
                BackColor = Color.SeaGreen,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Enabled = false
            };

            this.grpSoDoBan.Controls.AddRange(new Control[] { flpBanBida, lblChuThich, btnMoBan });

            // GroupBox 2: Dịch vụ & Thanh toán (Disconnected DataTable + SqlTransaction)
            this.grpChiTiet = new GroupBox { Text = "2. Gọi Dịch vụ (DataTable) & Thanh toán", Location = new Point(500, 15), Size = new Size(575, 520) };

            this.lblTenBanChon = new Label
            {
                Text = "Chưa chọn bàn",
                Location = new Point(15, 26),
                AutoSize = true,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.DarkBlue
            };

            this.lblThongTinGio = new Label
            {
                Text = "Bấm vào một bàn trên sơ đồ để xem trạng thái...",
                Location = new Point(15, 54),
                AutoSize = true,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Regular)
            };

            this.lblChonDV = new Label { Text = "Dịch vụ:", Location = new Point(15, 88), AutoSize = true };
            this.cboDichVu = new ComboBox { Location = new Point(75, 85), Width = 290, DropDownStyle = ComboBoxStyle.DropDownList };

            this.lblSL = new Label { Text = "SL:", Location = new Point(375, 88), AutoSize = true };
            this.numSoLuong = new NumericUpDown { Location = new Point(405, 85), Width = 60, Minimum = 1, Maximum = 100, Value = 1 };

            this.btnThemMon = new Button
            {
                Text = "+ Thêm món",
                Location = new Point(475, 83),
                Size = new Size(85, 28),
                BackColor = Color.SteelBlue,
                ForeColor = Color.White
            };

            this.btnGiamMon = new Button
            {
                Text = "- Giảm / Xóa món đang chọn",
                Location = new Point(375, 118),
                Size = new Size(185, 26)
            };

            this.dgvChiTietHD = new DataGridView
            {
                Location = new Point(15, 150),
                Size = new Size(545, 225),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ReadOnly = true,
                AllowUserToAddRows = false
            };

            this.lblTienGio = new Label { Text = "Tiền giờ chơi (làm tròn 15p): 0 VNĐ", Location = new Point(15, 388), AutoSize = true, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold) };
            this.lblTienDV = new Label { Text = "Tiền dịch vụ / đồ uống: 0 VNĐ", Location = new Point(15, 412), AutoSize = true, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold) };
            this.lblTongCong = new Label
            {
                Text = "TỔNG THANH TOÁN: 0 VNĐ",
                Location = new Point(15, 438),
                AutoSize = true,
                Font = new Font("Segoe UI", 11.5F, FontStyle.Bold),
                ForeColor = Color.Crimson
            };

            this.btnThanhToan = new Button
            {
                Text = "THANH TOÁN && IN HÓA ĐƠN",
                Location = new Point(15, 470),
                Size = new Size(545, 40),
                BackColor = Color.DarkOrange,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Enabled = false
            };

            this.grpChiTiet.Controls.AddRange(new Control[] {
                lblTenBanChon, lblThongTinGio, lblChonDV, cboDichVu, lblSL, numSoLuong,
                btnThemMon, btnGiamMon, dgvChiTietHD, lblTienGio, lblTienDV, lblTongCong, btnThanhToan
            });

            // Timer cập nhật thời gian chơi thực tế mỗi 5 giây
            this.tmrTinhGio = new System.Windows.Forms.Timer(this.components) { Interval = 5000, Enabled = true };
            this.tmrTinhGio.Tick += (s, e) => CapNhatTinhTienTamTinh();

            this.btnMoBan.Click += btnMoBan_Click;
            this.btnThemMon.Click += btnThemMon_Click;
            this.btnGiamMon.Click += btnGiamMon_Click;
            this.btnThanhToan.Click += btnThanhToan_Click;
            this.Load += frmSoDoBan_Load;

            this.ClientSize = new Size(1090, 550);
            this.Controls.AddRange(new Control[] { grpSoDoBan, grpChiTiet });
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Sơ đồ Bàn Bida & Quản lý Tính tiền";
        }
    }
}