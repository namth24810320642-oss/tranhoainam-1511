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

        private void btnTiepNhan_Click(object sender, EventArgs e)
        {
            new frmTiepNhan().ShowDialog();
        }

        private void btnThuoc_Click(object sender, EventArgs e)
        {
            new frmThuoc().ShowDialog();
        }

        private void btnKeDon_Click(object sender, EventArgs e)
        {
            new frmKeDon().ShowDialog();
        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            new frmThanhToan().ShowDialog();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}