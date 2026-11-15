using System.Drawing;
using System.Windows.Forms;

namespace GUI
{
    partial class frmMuonTra
    {
        private System.ComponentModel.IContainer components = null;
        private GroupBox grpLapPhieu, grpDanhSach;
        private Label lblDG, lblHanTra, lblSach, lblSL, lblLocDG, lblLocTT, lblChiTiet;
        private ComboBox cboDocGiaMuon, cboSachMuon, cboLocDocGia, cboLocTrangThai;
        private DateTimePicker dtpHanTra;
        private NumericUpDown numSoLuongMuon;
        private Button btnThemSach, btnLapPhieu, btnLoc, btnTraSach;
        private DataGridView dgvGioSach, dgvPhieuMuon, dgvChiTietMuon;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            // GroupBox 1: Lập phiếu mượn
            this.grpLapPhieu = new GroupBox { Text = "Lập Phiếu Mượn Mới", Location = new Point(15, 15), Size = new Size(390, 500) };

            this.lblDG = new Label { Text = "Độc giả:", Location = new Point(15, 30), AutoSize = true };
            this.cboDocGiaMuon = new ComboBox { Location = new Point(105, 27), Width = 265, DropDownStyle = ComboBoxStyle.DropDownList };

            this.lblHanTra = new Label { Text = "Hạn trả:", Location = new Point(15, 68), AutoSize = true };
            this.dtpHanTra = new DateTimePicker { Location = new Point(105, 65), Width = 265, Format = DateTimePickerFormat.Short };

            this.lblSach = new Label { Text = "Chọn sách:", Location = new Point(15, 106), AutoSize = true };
            this.cboSachMuon = new ComboBox { Location = new Point(105, 103), Width = 265, DropDownStyle = ComboBoxStyle.DropDownList };

            this.lblSL = new Label { Text = "Số lượng:", Location = new Point(15, 144), AutoSize = true };
            this.numSoLuongMuon = new NumericUpDown { Location = new Point(105, 141), Width = 90, Minimum = 1, Maximum = 50, Value = 1 };
            this.btnThemSach = new Button { Text = "+ Thêm vào phiếu", Location = new Point(210, 139), Size = new Size(160, 30) };

            this.dgvGioSach = new DataGridView
            {
                Location = new Point(15, 185),
                Size = new Size(355, 245),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false
            };

            this.btnLapPhieu = new Button
            {
                Text = "XÁC NHẬN LẬP PHIẾU MƯỢN",
                Location = new Point(15, 445),
                Size = new Size(355, 40),
                BackColor = Color.SteelBlue,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold)
            };

            this.grpLapPhieu.Controls.AddRange(new Control[] {
                lblDG, cboDocGiaMuon, lblHanTra, dtpHanTra, lblSach, cboSachMuon,
                lblSL, numSoLuongMuon, btnThemSach, dgvGioSach, btnLapPhieu
            });

            // GroupBox 2: Lọc & Trả sách
            this.grpDanhSach = new GroupBox { Text = "Quản lý Phiếu Mượn & Trả Sách", Location = new Point(420, 15), Size = new Size(620, 500) };

            this.lblLocDG = new Label { Text = "Độc giả:", Location = new Point(15, 30), AutoSize = true };
            this.cboLocDocGia = new ComboBox { Location = new Point(75, 27), Width = 190, DropDownStyle = ComboBoxStyle.DropDownList };

            this.lblLocTT = new Label { Text = "Trạng thái:", Location = new Point(275, 30), AutoSize = true };
            this.cboLocTrangThai = new ComboBox { Location = new Point(350, 27), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
            this.cboLocTrangThai.Items.AddRange(new object[] { "Tất cả", "Đang mượn", "Đã trả", "Quá hạn" });

            this.btnLoc = new Button { Text = "Lọc", Location = new Point(515, 25), Size = new Size(85, 28) };

            this.dgvPhieuMuon = new DataGridView
            {
                Location = new Point(15, 65),
                Size = new Size(585, 220),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ReadOnly = true,
                AllowUserToAddRows = false
            };

            this.lblChiTiet = new Label { Text = "Chi tiết sách trong phiếu đang chọn:", Location = new Point(15, 295), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };

            this.dgvChiTietMuon = new DataGridView
            {
                Location = new Point(15, 318),
                Size = new Size(585, 115),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false
            };

            this.btnTraSach = new Button
            {
                Text = "TRẢ SÁCH (Cập nhật trạng thái & Cộng kho)",
                Location = new Point(15, 445),
                Size = new Size(585, 40),
                BackColor = Color.SeaGreen,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold)
            };

            this.grpDanhSach.Controls.AddRange(new Control[] {
                lblLocDG, cboLocDocGia, lblLocTT, cboLocTrangThai, btnLoc,
                dgvPhieuMuon, lblChiTiet, dgvChiTietMuon, btnTraSach
            });

            this.btnThemSach.Click += btnThemSach_Click;
            this.btnLapPhieu.Click += btnLapPhieu_Click;
            this.btnLoc.Click += btnLoc_Click;
            this.dgvPhieuMuon.CellClick += dgvPhieuMuon_CellClick;
            this.btnTraSach.Click += btnTraSach_Click;
            this.Load += frmMuonTra_Load;

            this.ClientSize = new Size(1055, 530);
            this.Controls.AddRange(new Control[] { grpLapPhieu, grpDanhSach });
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Quản lý Mượn / Trả Sách";
        }
    }
}