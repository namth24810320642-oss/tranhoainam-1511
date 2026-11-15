using System;
using System.Windows.Forms;

namespace GUI
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnSoDoBan_Click(object sender, EventArgs e)
        {
            new frmSoDoBan().ShowDialog();
        }

        private void btnQuanLyBan_Click(object sender, EventArgs e)
        {
            new frmQuanLyBan().ShowDialog();
        }

        private void btnMenuDichVu_Click(object sender, EventArgs e)
        {
            new frmMenuDichVu().ShowDialog();
        }

        private void btnLichSuHD_Click(object sender, EventArgs e)
        {
            new frmLichSuHoaDon().ShowDialog();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}