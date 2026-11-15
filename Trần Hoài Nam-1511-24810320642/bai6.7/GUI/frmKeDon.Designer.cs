using System.Drawing;
using System.Windows.Forms;

namespace GUI
{
    partial class frmKeDon
    {
        private System.ComponentModel.IContainer components = null;
        private GroupBox grpKham, grpChonThuoc;
        private Label lblHoSo, lblTrieuChung, lblChanDoan, lblTienKham, lblThuoc, lblSL, lblLieuDung, lblTongTamTinh;
        private ComboBox cboHoSoKham, cboThuoc;
        private TextBox txtTrieuChung, txtChanDoan, txtLieuDung;
        private NumericUpDown numTienKham, numSoLuong;
        private Button btnThemThuoc, btnXoaDong, btnLuuXuatDon;
        private DataGridView dgvChiTietDon;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            // Group 1: Thông tin khám & Chẩn đoán
            this.grpKham = new GroupBox { Text = "1. Thông tin Khám bệnh & Chẩn đoán", Location = new Point(15, 15), Size = new Size(840, 145) };

            this.lblHoSo = new Label { Text = "Hồ sơ khám:", Location = new Point(20, 30), AutoSize = true };
            this.cboHoSoKham = new ComboBox { Location = new Point(115, 27), Width = 340, DropDownStyle = ComboBoxStyle.DropDownList };

            this.lblTienKham = new Label { Text = "Tiền khám (VNĐ):", Location = new Point(485, 30), AutoSize = true };
            this.numTienKham = new NumericUpDown { Location = new Point(605, 27), Width = 150, Maximum = 50000000, Increment = 10000, Value = 100000 };

            this.lblTrieuChung = new Label { Text = "Triệu chứng:", Location = new Point(20, 68), AutoSize = true };
            this.txtTrieuChung = new TextBox { Location = new Point(115, 65), Width = 700, ReadOnly = true };

            this.lblChanDoan = new Label { Text = "Chẩn đoán:", Location = new Point(20, 105), AutoSize = true };
            this.txtChanDoan = new TextBox { Location = new Point(115, 102), Width = 700 };

            this.grpKham.Controls.AddRange(new Control[] {
                lblHoSo, cboHoSoKham, lblTienKham, numTienKham, lblTrieuChung, txtTrieuChung, lblChanDoan, txtChanDoan
            });

            // Group 2: Kê đơn thuốc
            this.grpChonThuoc = new GroupBox { Text = "2. Kê đơn thuốc (SqlTransaction & Kiểm tra tồn kho)", Location = new Point(15, 170), Size = new Size(840, 355) };

            this.lblThuoc = new Label { Text = "Chọn thuốc:", Location = new Point(20, 32), AutoSize = true };
            this.cboThuoc = new ComboBox { Location = new Point(115, 29), Width = 430, DropDownStyle = ComboBoxStyle.DropDownList };

            this.lblSL = new Label { Text = "Số lượng:", Location = new Point(565, 32), AutoSize = true };
            this.numSoLuong = new NumericUpDown { Location = new Point(635, 29), Width = 80, Minimum = 1, Maximum = 1000, Value = 1 };

            this.lblLieuDung = new Label { Text = "Liều dùng:", Location = new Point(20, 70), AutoSize = true };
            this.txtLieuDung = new TextBox { Location = new Point(115, 67), Width = 430, Text = "Sáng 1 viên, Chiều 1 viên sau ăn" };

            this.btnThemThuoc = new Button { Text = "+ Thêm vào đơn", Location = new Point(565, 65), Size = new Size(130, 30), BackColor = Color.Teal, ForeColor = Color.White };
            this.btnXoaDong = new Button { Text = "Xóa dòng", Location = new Point(705, 65), Size = new Size(110, 30) };

            this.dgvChiTietDon = new DataGridView
            {
                Location = new Point(20, 110),
                Size = new Size(795, 175),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ReadOnly = true,
                AllowUserToAddRows = false
            };

            this.lblTongTamTinh = new Label
            {
                Text = "Tổng chi phí dự kiến: 100,000 VNĐ",
                Location = new Point(20, 305),
                AutoSize = true,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.DarkRed
            };

            this.btnLuuXuatDon = new Button
            {
                Text = "LƯU && XUẤT ĐƠN THUỐC",
                Location = new Point(575, 295),
                Size = new Size(240, 45),
                BackColor = Color.DarkOrange,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };

            this.grpChonThuoc.Controls.AddRange(new Control[] {
                lblThuoc, cboThuoc, lblSL, numSoLuong, lblLieuDung, txtLieuDung,
                btnThemThuoc, btnXoaDong, dgvChiTietDon, lblTongTamTinh, btnLuuXuatDon
            });

            this.cboHoSoKham.SelectedIndexChanged += cboHoSoKham_SelectedIndexChanged;
            this.numTienKham.ValueChanged += (s, e) => CapNhatTongTien();
            this.btnThemThuoc.Click += btnThemThuoc_Click;
            this.btnXoaDong.Click += btnXoaDong_Click;
            this.btnLuuXuatDon.Click += btnLuuXuatDon_Click;
            this.Load += frmKeDon_Load;

            this.ClientSize = new Size(870, 540);
            this.Controls.AddRange(new Control[] { grpKham, grpChonThuoc });
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Khám bệnh & Kê Đơn thuốc";
        }
    }
}