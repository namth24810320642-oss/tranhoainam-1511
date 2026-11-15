using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BUS;
using DTO;

namespace GUI
{
    public partial class frmSoDoBan : Form
    {
        private readonly BanBidaBUS banBus = new BanBidaBUS();
        private readonly MenuDichVuBUS dvBus = new MenuDichVuBUS();
        private readonly HoaDonBUS hdBus = new HoaDonBUS();

        private BanBidaDTO banDangChon = null;
        private HoaDonDTO hoaDonHienTai = null;
        private DataTable dtChiTiet = null; // Mô hình ngắt kết nối (Disconnected DataTable)

        public frmSoDoBan()
        {
            InitializeComponent();
        }

        private void frmSoDoBan_Load(object sender, EventArgs e)
        {
            LoadMenuDichVu();
            LoadSoDoBan();
        }

        private void LoadMenuDichVu()
        {
            cboDichVu.DataSource = dvBus.LayDanhSachDichVu();
            cboDichVu.DisplayMember = "ThongTinHienThi";
            cboDichVu.ValueMember = "MaDV";
        }

        // Sinh danh sách Button bàn động từ CSDL lên FlowLayoutPanel
        private void LoadSoDoBan()
        {
            flpBanBida.Controls.Clear();
            var danhSachBan = banBus.LayDanhSachBan();

            foreach (var ban in danhSachBan)
            {
                bool isTrong = ban.TrangThai == "Trống";
                Button btn = new Button
                {
                    Width = 132,
                    Height = 95,
                    Margin = new Padding(6),
                    Text = $"{ban.TenBan}\n[{ban.LoaiBan}]\n{ban.GiaTheoGio:N0}đ/giờ\n({ban.TrangThai})",
                    Tag = ban,
                    BackColor = isTrong ? Color.LightGreen : Color.OrangeRed,
                    ForeColor = isTrong ? Color.Black : Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 8.5F, FontStyle.Bold)
                };
                btn.Click += BtnBan_Click;
                flpBanBida.Controls.Add(btn);
            }
        }

        private void BtnBan_Click(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.Tag is BanBidaDTO ban)
            {
                banDangChon = ban;
                HienThiThongTinBanDangChon();
            }
        }

        private void HienThiThongTinBanDangChon()
        {
            if (banDangChon == null) return;

            lblTenBanChon.Text = $"{banDangChon.TenBan} - {banDangChon.LoaiBan} ({banDangChon.GiaTheoGio:N0} VNĐ/giờ)";

            if (banDangChon.TrangThai == "Trống")
            {
                hoaDonHienTai = null;
                dtChiTiet = null;
                dgvChiTietHD.DataSource = null;

                lblThongTinGio.Text = "Trạng thái: Bàn Trống - Sẵn sàng phục vụ.";
                lblTienGio.Text = "Tiền giờ chơi (làm tròn 15p): 0 VNĐ";
                lblTienDV.Text = "Tiền dịch vụ / đồ uống: 0 VNĐ";
                lblTongCong.Text = "TỔNG THANH TOÁN: 0 VNĐ";

                btnMoBan.Enabled = true;
                btnThemMon.Enabled = false;
                btnGiamMon.Enabled = false;
                btnThanhToan.Enabled = false;
            }
            else
            {
                hoaDonHienTai = hdBus.LayHoaDonChuaThanhToan(banDangChon.MaBan);
                if (hoaDonHienTai != null)
                {
                    // Tải chi tiết hóa đơn vào DataTable ngắt kết nối
                    dtChiTiet = hdBus.LayChiTietHoaDonDataTable(hoaDonHienTai.MaHD);
                    dgvChiTietHD.DataSource = dtChiTiet;
                    DinhDangGridChiTiet();
                    CapNhatTinhTienTamTinh();
                }

                btnMoBan.Enabled = false;
                btnThemMon.Enabled = true;
                btnGiamMon.Enabled = true;
                btnThanhToan.Enabled = true;
            }
        }

        private void DinhDangGridChiTiet()
        {
            if (dgvChiTietHD.Columns.Count == 0) return;
            dgvChiTietHD.Columns["MaHD"].Visible = false;
            dgvChiTietHD.Columns["MaDV"].HeaderText = "Mã DV";
            dgvChiTietHD.Columns["TenDV"].HeaderText = "Tên Dịch Vụ / Món";
            dgvChiTietHD.Columns["SoLuong"].HeaderText = "Số Lượng";
            dgvChiTietHD.Columns["DonGia"].HeaderText = "Đơn Giá";
            dgvChiTietHD.Columns["DonGia"].DefaultCellStyle.Format = "N0";
            dgvChiTietHD.Columns["ThanhTien"].HeaderText = "Thành Tiền";
            dgvChiTietHD.Columns["ThanhTien"].DefaultCellStyle.Format = "N0";
        }

        private void CapNhatTinhTienTamTinh()
        {
            if (banDangChon == null || banDangChon.TrangThai != "Đang chơi" || hoaDonHienTai == null) return;

            DateTime hienTai = DateTime.Now;
            TimeSpan thoiGianChoi = hienTai - hoaDonHienTai.GioBatDau;
            int tongPhut = Math.Max(1, (int)thoiGianChoi.TotalMinutes);

            decimal tienGio = hdBus.TinhTienGioChoi(hoaDonHienTai.GioBatDau, hienTai, banDangChon.GiaTheoGio);
            decimal tienDV = TinhTongTienDichVuTuDataTable();
            decimal tongCong = tienGio + tienDV;

            lblThongTinGio.Text = $"Giờ bắt đầu: {hoaDonHienTai.GioBatDau:HH:mm dd/MM/yyyy} | Đã chơi: {tongPhut / 60} giờ {tongPhut % 60} phút";
            lblTienGio.Text = $"Tiền giờ chơi (làm tròn 15p): {tienGio:N0} VNĐ";
            lblTienDV.Text = $"Tiền dịch vụ / đồ uống: {tienDV:N0} VNĐ";
            lblTongCong.Text = $"TỔNG THANH TOÁN: {tongCong:N0} VNĐ";
        }

        private decimal TinhTongTienDichVuTuDataTable()
        {
            if (dtChiTiet == null) return 0;
            decimal sum = 0;
            foreach (DataRow row in dtChiTiet.Rows)
            {
                if (row.RowState != DataRowState.Deleted)
                {
                    sum += Convert.ToDecimal(row["ThanhTien"]);
                }
            }
            return sum;
        }

        // Mở bàn
        private void btnMoBan_Click(object sender, EventArgs e)
        {
            try
            {
                if (banDangChon == null) return;
                if (hdBus.MoBan(banDangChon.MaBan, banDangChon.TrangThai))
                {
                    MessageBox.Show($"Đã mở {banDangChon.TenBan}!", "Thông báo");
                    banDangChon.TrangThai = "Đang chơi";
                    LoadSoDoBan();
                    HienThiThongTinBanDangChon();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Thêm món / Tăng số lượng trực tiếp trên DataTable ngắt kết nối và đồng bộ xuống CSDL
        private void btnThemMon_Click(object sender, EventArgs e)
        {
            try
            {
                if (hoaDonHienTai == null || dtChiTiet == null) return;
                if (!(cboDichVu.SelectedItem is MenuDichVuDTO dv)) return;

                int slThem = (int)numSoLuong.Value;
                DataRow rowTonTai = dtChiTiet.AsEnumerable()
                    .FirstOrDefault(r => r.RowState != DataRowState.Deleted && (int)r["MaDV"] == dv.MaDV);

                int tongSLMoi = (rowTonTai != null ? (int)rowTonTai["SoLuong"] : 0) + slThem;

                if (dv.TonKho.HasValue && tongSLMoi > dv.TonKho.Value)
                {
                    MessageBox.Show($"Mặt hàng '{dv.TenDV}' chỉ còn {dv.TonKho.Value} trong kho!", "Cảnh báo tồn kho");
                    return;
                }

                if (rowTonTai != null)
                {
                    rowTonTai["SoLuong"] = tongSLMoi;
                    rowTonTai["ThanhTien"] = tongSLMoi * dv.DonGia;
                }
                else
                {
                    DataRow newRow = dtChiTiet.NewRow();
                    newRow["MaHD"] = hoaDonHienTai.MaHD;
                    newRow["MaDV"] = dv.MaDV;
                    newRow["TenDV"] = dv.TenDV;
                    newRow["SoLuong"] = slThem;
                    newRow["DonGia"] = dv.DonGia;
                    newRow["ThanhTien"] = slThem * dv.DonGia;
                    dtChiTiet.Rows.Add(newRow);
                }

                // Cập nhật DataTable xuống SQL Server bằng SqlDataAdapter
                hdBus.LuuChiTietHoaDon(dtChiTiet);
                CapNhatTinhTienTamTinh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật dịch vụ: " + ex.Message, "Lỗi");
            }
        }

        // Giảm số lượng hoặc xóa dòng trên DataTable
        private void btnGiamMon_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvChiTietHD.CurrentRow == null || dtChiTiet == null) return;
                DataRowView drv = dgvChiTietHD.CurrentRow.DataBoundItem as DataRowView;
                if (drv == null) return;

                DataRow row = drv.Row;
                int slHienTai = (int)row["SoLuong"];
                if (slHienTai > 1)
                {
                    row["SoLuong"] = slHienTai - 1;
                    row["ThanhTien"] = (slHienTai - 1) * Convert.ToDecimal(row["DonGia"]);
                }
                else
                {
                    row.Delete();
                }

                hdBus.LuuChiTietHoaDon(dtChiTiet);
                CapNhatTinhTienTamTinh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật dịch vụ: " + ex.Message, "Lỗi");
            }
        }

        // Thanh toán & In hóa đơn bằng SqlTransaction
        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            try
            {
                if (banDangChon == null || hoaDonHienTai == null) return;

                DateTime gioKetThuc = DateTime.Now;
                decimal tienGio = hdBus.TinhTienGioChoi(hoaDonHienTai.GioBatDau, gioKetThuc, banDangChon.GiaTheoGio);
                decimal tienDV = TinhTongTienDichVuTuDataTable();
                decimal tongTien = tienGio + tienDV;

                if (MessageBox.Show($"Xác nhận thanh toán cho {banDangChon.TenBan}?\nTổng tiền: {tongTien:N0} VNĐ",
                    "Xác nhận thanh toán", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // Tạo nội dung hóa đơn trước khi reset dữ liệu
                    string noiDungHoaDon = TaoNoiDungInHoaDon(hoaDonHienTai, banDangChon, gioKetThuc, tienGio, tienDV);

                    if (hdBus.ThanhToanHoaDon(hoaDonHienTai.MaHD, banDangChon.MaBan, gioKetThuc, tienGio, tienDV))
                    {
                        MessageBox.Show(noiDungHoaDon, "Thanh toán thành công - Hóa đơn Bida", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        banDangChon.TrangThai = "Trống";
                        LoadMenuDichVu(); // Tải lại tồn kho mới sau khi trừ kho
                        LoadSoDoBan();
                        HienThiThongTinBanDangChon();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Thanh toán thất bại (Transaction Rollback):\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string TaoNoiDungInHoaDon(HoaDonDTO hd, BanBidaDTO ban, DateTime gioKetThuc, decimal tienGio, decimal tienDV)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("==============================================");
            sb.AppendLine("           HÓA ĐƠN THANH TOÁN BIDA            ");
            sb.AppendLine("==============================================");
            sb.AppendLine($"Mã HĐ: #{hd.MaHD}   |   Bàn: {ban.TenBan} ({ban.LoaiBan})");
            sb.AppendLine($"Giờ vào : {hd.GioBatDau:dd/MM/yyyy HH:mm}");
            sb.AppendLine($"Giờ ra  : {gioKetThuc:dd/MM/yyyy HH:mm}");
            sb.AppendLine($"Đơn giá : {ban.GiaTheoGio:N0} VNĐ/giờ");
            sb.AppendLine("----------------------------------------------");
            sb.AppendLine("DỊCH VỤ ĐÃ SỬ DỤNG:");
            if (dtChiTiet != null && dtChiTiet.Rows.Count > 0)
            {
                foreach (DataRow r in dtChiTiet.Rows)
                {
                    if (r.RowState != DataRowState.Deleted)
                    {
                        sb.AppendLine($"- {r["TenDV"]} x{r["SoLuong"]} = {Convert.ToDecimal(r["ThanhTien"]):N0}đ");
                    }
                }
            }
            else
            {
                sb.AppendLine("- (Không gọi dịch vụ thêm)");
            }
            sb.AppendLine("----------------------------------------------");
            sb.AppendLine($"Tiền giờ chơi  : {tienGio,15:N0} VNĐ");
            sb.AppendLine($"Tiền dịch vụ   : {tienDV,15:N0} VNĐ");
            sb.AppendLine($"TỔNG THANH TOÁN: {(tienGio + tienDV),15:N0} VNĐ");
            sb.AppendLine("==============================================");
            return sb.ToString();
        }
    }
}