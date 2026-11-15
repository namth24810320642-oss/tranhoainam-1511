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
        private Button btnSoDoBan;
        private Button btnQuanLyBan;
        private Button btnMenuDichVu;
        private Button btnLichSuHD;
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
            this.btnSoDoBan = new Button();
            this.btnQuanLyBan = new Button();
            this.btnMenuDichVu = new Button();
            this.btnLichSuHD = new Button();
            this.btnThoat = new Button();
            this.pnlHeader.SuspendLayout();
            this.flpMenu.SuspendLayout();
            this.SuspendLayout();

            this.pnlHeader.BackColor = Color.FromArgb(20, 90, 50);
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Dock = DockStyle.Top;
            this.pnlHeader.Height = 80;

            this.lblTitle.Dock = DockStyle.Fill;
            this.lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            this.lblTitle.ForeColor = Color.White;
            this.lblTitle.Text = "HỆ THỐNG QUẢN LÝ BÀN BIDA && DỊCH VỤ GIẢI TRÍ";
            this.lblTitle.TextAlign = ContentAlignment.MiddleCenter;

            this.flpMenu.Dock = DockStyle.Fill;
            this.flpMenu.Padding = new Padding(45, 35, 20, 20);
            this.flpMenu.Controls.AddRange(new Control[] {
                this.btnSoDoBan, this.btnQuanLyBan, this.btnMenuDichVu, this.btnLichSuHD, this.btnThoat
            });

            ConfigButton(this.btnSoDoBan, "1. Sơ đồ Bàn && Tính tiền", Color.SeaGreen);
            ConfigButton(this.btnQuanLyBan, "2. Quản lý Danh mục Bàn", Color.SteelBlue);
            ConfigButton(this.btnMenuDichVu, "3. Quản lý Menu Dịch vụ", Color.DarkOrange);
            ConfigButton(this.btnLichSuHD, "4. Lịch sử Hóa đơn", Color.Teal);
            ConfigButton(this.btnThoat, "Thoát chương trình", Color.DimGray);

            this.btnSoDoBan.Click += btnSoDoBan_Click;
            this.btnQuanLyBan.Click += btnQuanLyBan_Click;
            this.btnMenuDichVu.Click += btnMenuDichVu_Click;
            this.btnLichSuHD.Click += btnLichSuHD_Click;
            this.btnThoat.Click += btnThoat_Click;

            this.ClientSize = new Size(760, 420);
            this.Controls.Add(this.flpMenu);
            this.Controls.Add(this.pnlHeader);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Quản lý CLB Bida - Bài 6.8";
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