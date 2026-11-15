using System;
using System.Drawing;
using System.Windows.Forms;
using BUS;
using DTO;

namespace GUI
{
    public partial class frmTiepNhan : Form
    {
        private readonly BenhNhanBUS bnBus = new BenhNhanBUS();
        private readonly HoSoKhamBUS hsBus = new HoSoKhamBUS();

        public frmTiepNhan()
        {
            InitializeComponent();
        }

        private void frmTiepNhan_Load(object sender, EventArgs e)
        {
            LoadDanhSachHoSo();
        }

        private void LoadDanhSachHoSo()
        {
            dgvHoSoKham.DataSource = hsBus.LayDanhSachHoSo();
            dgvHoSoKham.Columns["MaBN"].Visible = false;
            dgvHoSoKham.Columns["ThongTinHienThi"].Visible = false;
            dgvHoSoKham.Columns["MaHoSo"].HeaderText = "Mã Hồ Sơ";
            dgvHoSoKham.Columns["HoTenBN"].HeaderText = "Bệnh Nhân";
            dgvHoSoKham.Columns["SoDienThoai"].HeaderText = "Số Điện Thoại";
            dgvHoSoKham.Columns["NgayKham"].HeaderText = "Ngày Khám";
            dgvHoSoKham.Columns["TrieuChung"].HeaderText = "Triệu Chứng";
            dgvHoSoKham.Columns["ChanDoan"].HeaderText = "Chẩn Đoán";
        }

        private void btnTraCuu_Click(object sender, EventArgs e)
        {
            try
            {
                BenhNhanDTO bn = bnBus.TraCuuTheoSDT(txtSDT.Text);
                if (bn != null)
                {
                    txtMaBN.Text = bn.MaBN.ToString();
                    txtHoTen.Text = bn.HoTen;
                    numNamSinh.Value = bn.NamSinh;
                    txtHoTen.ReadOnly = true;
                    numNamSinh.Enabled = false;
                    lblTrangThaiBN.Text = "✔ Bệnh nhân cũ - Đã tải thông tin!";
                    lblTrangThaiBN.ForeColor = Color.Green;
                }
                else
                {
                    txtMaBN.Clear();
                    txtHoTen.Clear();
                    txtHoTen.ReadOnly = false;
                    numNamSinh.Enabled = true;
                    lblTrangThaiBN.Text = "✚ Bệnh nhân mới - Vui lòng nhập Họ tên & Năm sinh!";
                    lblTrangThaiBN.ForeColor = Color.DarkOrange;
                    txtHoTen.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo");
            }
        }

        private void btnTaoHoSo_Click(object sender, EventArgs e)
        {
            try
            {
                int maBN;
                if (string.IsNullOrWhiteSpace(txtMaBN.Text))
                {
                    // Bệnh nhân mới -> Thêm mới BN trước rồi lấy MaBN
                    BenhNhanDTO bnMoi = new BenhNhanDTO
                    {
                        HoTen = txtHoTen.Text.Trim(),
                        SoDienThoai = txtSDT.Text.Trim(),
                        NamSinh = (int)numNamSinh.Value
                    };
                    maBN = bnBus.ThemBenhNhanMoi(bnMoi);
                    txtMaBN.Text = maBN.ToString();
                }
                else
                {
                    maBN = int.Parse(txtMaBN.Text);
                }

                HoSoKhamDTO hs = new HoSoKhamDTO
                {
                    MaBN = maBN,
                    TrieuChung = txtTrieuChung.Text.Trim(),
                    ChanDoan = ""
                };

                if (hsBus.TaoHoSoKham(hs))
                {
                    MessageBox.Show("Tiếp nhận bệnh nhân và tạo hồ sơ khám thành công!", "Thông báo");
                    txtTrieuChung.Clear();
                    LoadDanhSachHoSo();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtSDT.Clear();
            txtMaBN.Clear();
            txtHoTen.Clear();
            txtTrieuChung.Clear();
            txtHoTen.ReadOnly = false;
            numNamSinh.Enabled = true;
            lblTrangThaiBN.Text = "Nhập SĐT và bấm Tra cứu";
            lblTrangThaiBN.ForeColor = Color.Blue;
        }
    }
}