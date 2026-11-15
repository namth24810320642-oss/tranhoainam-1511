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

        private void btnDauSach_Click(object sender, EventArgs e)
        {
            new frmDauSach().ShowDialog();
        }

        private void btnDocGia_Click(object sender, EventArgs e)
        {
            new frmDocGia().ShowDialog();
        }

        private void btnMuonTra_Click(object sender, EventArgs e)
        {
            new frmMuonTra().ShowDialog();
        }

        private void btnThongKe_Click(object sender, EventArgs e)
        {
            new frmThongKe().ShowDialog();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}