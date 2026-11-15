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
        private Button btnDauSach;
        private Button btnDocGia;
        private Button btnMuonTra;
        private Button btnThongKe;
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
            this.btnDauSach = new Button();
            this.btnDocGia = new Button();
            this.btnMuonTra = new Button();
            this.btnThongKe = new Button();
            this.btnThoat = new Button();
            this.pnlHeader.SuspendLayout();
            this.flpMenu.SuspendLayout();
            this.SuspendLayout();

            // pnlHeader
            this.pnlHeader.BackColor = Color.FromArgb(30, 80, 140);
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Dock = DockStyle.Top;
            this.pnlHeader.Height = 80;

            // lblTitle
            this.lblTitle.Dock = DockStyle.Fill;
            this.lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            this.lblTitle.ForeColor = Color.White;
            this.lblTitle.Text = "HỆ THỐNG QUẢN LÝ MƯỢN TRẢ SÁCH THƯ VIỆN";
            this.lblTitle.TextAlign = ContentAlignment.MiddleCenter;

            // flpMenu
            this.flpMenu.Dock = DockStyle.Fill;
            this.flpMenu.Padding = new Padding(45, 35, 20, 20);
            this.flpMenu.Controls.AddRange(new Control[] {
                this.btnDauSach, this.btnDocGia, this.btnMuonTra, this.btnThongKe, this.btnThoat
            });

            // Buttons
            ConfigButton(this.btnDauSach, "1. Quản lý Đầu sách", Color.SteelBlue);
            ConfigButton(this.btnDocGia, "2. Quản lý Độc giả", Color.Teal);
            ConfigButton(this.btnMuonTra, "3. Mượn / Trả sách", Color.DarkOrange);
            ConfigButton(this.btnThongKe, "4. Cảnh báo Quá hạn && Thống kê", Color.Crimson);
            ConfigButton(this.btnThoat, "Thoát chương trình", Color.DimGray);

            this.btnDauSach.Click += new System.EventHandler(this.btnDauSach_Click);
            this.btnDocGia.Click += new System.EventHandler(this.btnDocGia_Click);
            this.btnMuonTra.Click += new System.EventHandler(this.btnMuonTra_Click);
            this.btnThongKe.Click += new System.EventHandler(this.btnThongKe_Click);
            this.btnThoat.Click += new System.EventHandler(this.btnThoat_Click);

            // frmMain
            this.ClientSize = new Size(760, 420);
            this.Controls.Add(this.flpMenu);
            this.Controls.Add(this.pnlHeader);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Quản lý Thư viện - Bài 6.6";
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