using System.Drawing;
using System.Windows.Forms;

namespace GUI
{
    partial class frmTiepNhan
    {
        private System.ComponentModel.IContainer components = null;
        private GroupBox grpBenhNhan, grpHoSo;
        private Label lblSDT, lblMaBN, lblHoTen, lblNamSinh, lblTrangThaiBN, lblTrieuChung;
        private TextBox txtSDT, txtMaBN, txtHoTen, txtTrieuChung;
        private NumericUpDown numNamSinh;
        private Button btnTraCuu, btnTaoHoSo, btnLamMoi;
        private DataGridView dgvHoSoKham;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.grpBenhNhan = new GroupBox { Text = "1. Tra cứu & Thông tin Bệnh nhân", Location = new Point(15, 15), Size = new Size(760, 145) };

            this.lblSDT = new Label { Text = "Số điện thoại:", Location = new Point(20, 32), AutoSize = true };
            this.txtSDT = new TextBox { Location = new Point(120, 29), Width = 180 };
            this.btnTraCuu = new Button { Text = "Tra cứu SĐT", Location = new Point(315, 27), Size = new Size(110, 28), BackColor = Color.Teal, ForeColor = Color.White };

            this.lblTrangThaiBN = new Label { Text = "Nhập SĐT và bấm Tra cứu", Location = new Point(440, 32), AutoSize = true, ForeColor = Color.Blue, Font = new Font("Segoe UI", 9F, FontStyle.Italic) };

            this.lblMaBN = new Label { Text = "Mã BN:", Location = new Point(20, 72), AutoSize = true };
            this.txtMaBN = new TextBox { Location = new Point(120, 69), Width = 80, ReadOnly = true };

            this.lblHoTen = new Label { Text = "Họ và tên:", Location = new Point(220, 72), AutoSize = true };
            this.txtHoTen = new TextBox { Location = new Point(295, 69), Width = 240 };

            this.lblNamSinh = new Label { Text = "Năm sinh:", Location = new Point(555, 72), AutoSize = true };
            this.numNamSinh = new NumericUpDown { Location = new Point(630, 69), Width = 100, Minimum = 1900, Maximum = 2030, Value = 1995 };

            this.btnLamMoi = new Button { Text = "Nhập bệnh nhân khác", Location = new Point(580, 105), Size = new Size(150, 28) };

            this.grpBenhNhan.Controls.AddRange(new Control[] {
                lblSDT, txtSDT, btnTraCuu, lblTrangThaiBN, lblMaBN, txtMaBN, lblHoTen, txtHoTen, lblNamSinh, numNamSinh, btnLamMoi
            });

            this.grpHoSo = new GroupBox { Text = "2. Lập Hồ sơ Tiếp nhận Khám", Location = new Point(15, 170), Size = new Size(760, 110) };
            this.lblTrieuChung = new Label { Text = "Triệu chứng:", Location = new Point(20, 32), AutoSize = true };
            this.txtTrieuChung = new TextBox { Location = new Point(120, 29), Width = 440, Height = 55, Multiline = true };
            this.btnTaoHoSo = new Button
            {
                Text = "LƯU & TẠO HỒ SƠ KHÁM",
                Location = new Point(575, 29),
                Size = new Size(165, 55),
                BackColor = Color.SteelBlue,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };

            this.grpHoSo.Controls.AddRange(new Control[] { lblTrieuChung, txtTrieuChung, btnTaoHoSo });

            this.dgvHoSoKham = new DataGridView
            {
                Location = new Point(15, 295),
                Size = new Size(760, 220),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false
            };

            this.btnTraCuu.Click += btnTraCuu_Click;
            this.btnTaoHoSo.Click += btnTaoHoSo_Click;
            this.btnLamMoi.Click += btnLamMoi_Click;
            this.Load += frmTiepNhan_Load;

            this.ClientSize = new Size(790, 530);
            this.Controls.AddRange(new Control[] { grpBenhNhan, grpHoSo, dgvHoSoKham });
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Tiếp nhận Bệnh nhân";
        }
    }
}