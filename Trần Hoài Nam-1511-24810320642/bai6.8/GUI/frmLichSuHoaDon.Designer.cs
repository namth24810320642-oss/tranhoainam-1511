using System.Drawing;
using System.Windows.Forms;

namespace GUI
{
    partial class frmLichSuHoaDon
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblLoc, lblChiTiet;
        private ComboBox cboTrangThai;
        private DataGridView dgvHoaDon, dgvChiTiet;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblLoc = new Label { Text = "Lọc trạng thái hóa đơn:", Location = new Point(20, 22), AutoSize = true };
            this.cboTrangThai = new ComboBox { Location = new Point(165, 19), Width = 180, DropDownStyle = ComboBoxStyle.DropDownList };
            this.cboTrangThai.Items.AddRange(new object[] { "Tất cả", "Chưa thanh toán", "Đã thanh toán" });

            this.dgvHoaDon = new DataGridView
            {
                Location = new Point(20, 58),
                Size = new Size(800, 220),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ReadOnly = true,
                AllowUserToAddRows = false
            };

            this.lblChiTiet = new Label { Text = "Chi tiết dịch vụ của hóa đơn đang chọn:", Location = new Point(20, 292), AutoSize = true, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold) };

            this.dgvChiTiet = new DataGridView
            {
                Location = new Point(20, 318),
                Size = new Size(800, 175),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false
            };

            this.cboTrangThai.SelectedIndexChanged += (s, e) => LoadHoaDon();
            this.dgvHoaDon.CellClick += dgvHoaDon_CellClick;
            this.Load += frmLichSuHoaDon_Load;

            this.ClientSize = new Size(840, 515);
            this.Controls.AddRange(new Control[] { lblLoc, cboTrangThai, dgvHoaDon, lblChiTiet, dgvChiTiet });
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Lịch sử Hóa đơn CLB Bida";
        }
    }
}