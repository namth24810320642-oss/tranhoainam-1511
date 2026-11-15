using System.Drawing;
using System.Windows.Forms;

namespace GUI
{
    partial class frmThongKe
    {
        private System.ComponentModel.IContainer components = null;
        private GroupBox grpQuaHan, grpThongKe;
        private Label lblCanhBao;
        private DataGridView dgvQuaHan, dgvThongKeTheLoai;
        private Button btnLamMoi;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            // GroupBox Cảnh báo quá hạn
            this.grpQuaHan = new GroupBox
            {
                Text = "Cảnh Báo Phiếu Mượn Quá Hạn (SqlDataReader & ExecuteScalar)",
                Location = new Point(15, 15),
                Size = new Size(760, 240),
                ForeColor = Color.DarkRed,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold)
            };

            this.lblCanhBao = new Label
            {
                Text = "Tổng số phiếu đang quá hạn: 0",
                Location = new Point(15, 28),
                AutoSize = true,
                ForeColor = Color.Red,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };

            this.dgvQuaHan = new DataGridView
            {
                Location = new Point(15, 58),
                Size = new Size(730, 165),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular)
            };

            this.grpQuaHan.Controls.AddRange(new Control[] { lblCanhBao, dgvQuaHan });

            // GroupBox Thống kê thể loại
            this.grpThongKe = new GroupBox
            {
                Text = "Thống Kê Lượt Mượn Theo Thể Loại Sách (GROUP BY)",
                Location = new Point(15, 270),
                Size = new Size(760, 230),
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold)
            };

            this.dgvThongKeTheLoai = new DataGridView
            {
                Location = new Point(15, 30),
                Size = new Size(730, 185),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular)
            };

            this.grpThongKe.Controls.Add(dgvThongKeTheLoai);

            this.btnLamMoi = new Button { Text = "Cập nhật dữ liệu", Location = new Point(625, 510), Size = new Size(150, 32) };
            this.btnLamMoi.Click += btnLamMoi_Click;
            this.Load += frmThongKe_Load;

            this.ClientSize = new Size(790, 555);
            this.Controls.AddRange(new Control[] { grpQuaHan, grpThongKe, btnLamMoi });
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Cảnh báo Quá hạn & Thống kê";
        }
    }
}