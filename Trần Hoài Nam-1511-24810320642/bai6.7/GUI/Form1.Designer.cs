using System.Drawing;
using System.Windows.Forms;

namespace GUI
{
    partial class frmMain
    {
        private System.ComponentModel.IContainer components = null;
        private Panel pnlHeader;
        private Label lblTitle;
        private FlowLayoutPanel flpMenu;
        private Button btnTiepNhan;
        private Button btnThuoc;
        private Button btnKeDon;
        private Button btnThanhToan;
        private Button btnThoat;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlHeader = new Panel();
            this.lblTitle = new Label();
            this.flpMenu = new FlowLayoutPanel();
            this.btnTiepNhan = new Button();
            this.btnThuoc = new Button();
            this.btnKeDon = new Button();
            this.btnThanhToan = new Button();
            this.btnThoat = new Button();
            this.pnlHeader.SuspendLayout();
            this.flpMenu.SuspendLayout();
            this.SuspendLayout();

            this.pnlHeader.BackColor = Color.FromArgb(0, 102, 102);
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Dock = DockStyle.Top;
            this.pnlHeader.Height = 80;

            this.lblTitle.Dock = DockStyle.Fill;
            this.lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            this.lblTitle.ForeColor = Color.White;
            this.lblTitle.Text = "HỆ THỐNG QUẢN LÝ PHÒNG KHÁM && KÊ ĐƠN THUỐC";
            this.lblTitle.TextAlign = ContentAlignment.MiddleCenter;

            this.flpMenu.Dock = DockStyle.Fill;
            this.flpMenu.Padding = new Padding(45, 35, 20, 20);
            this.flpMenu.Controls.AddRange(new Control[] {
                this.btnTiepNhan, this.btnThuoc, this.btnKeDon, this.btnThanhToan, this.btnThoat
            });

            ConfigButton(this.btnTiepNhan, "1. Tiếp nhận Bệnh nhân", Color.Teal);
            ConfigButton(this.btnThuoc, "2. Quản lý Danh mục Thuốc", Color.SteelBlue);
            ConfigButton(this.btnKeDon, "3. Khám bệnh && Kê Đơn", Color.DarkOrange);
            ConfigButton(this.btnThanhToan, "4. Thanh toán && In Hóa đơn", Color.SeaGreen);
            ConfigButton(this.btnThoat, "Thoát chương trình", Color.DimGray);

            this.btnTiepNhan.Click += btnTiepNhan_Click;
            this.btnThuoc.Click += btnThuoc_Click;
            this.btnKeDon.Click += btnKeDon_Click;
            this.btnThanhToan.Click += btnThanhToan_Click;
            this.btnThoat.Click += btnThoat_Click;

            this.ClientSize = new Size(760, 420);
            this.Controls.Add(this.flpMenu);
            this.Controls.Add(this.pnlHeader);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Quản lý Phòng khám - Bài 6.7";
            this.pnlHeader.ResumeLayout(false);
            this.flpMenu.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private void ConfigButton(Button btn, string text, Color bg)
        {
            btn.Size = new Size(210, 95);
            btn.Margin = new Padding(10);
            btn.Text = text;
            btn.BackColor = bg;
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        }
    }
}