
# Hệ thống Quản lý Thư viện (Library Management System) - Bài 6.6

Ứng dụng Windows Forms quản lý toàn diện quy trình nghiệp vụ tại thư viện: quản lý thẻ độc giả, danh mục đầu sách, quản lý từng bản sao vật lý trên kệ, lập phiếu mượn sách có kiểm soát điều kiện và xử lý trả sách kèm tính phí phạt quá hạn. Dự án được xây dựng bằng ngôn ngữ C# và hệ quản trị cơ sở dữ liệu SQL Server theo mô hình kiến trúc 3 lớp (3-Tier Architecture).

---

## 1. Cấu trúc Dự án (Kiến trúc 3 lớp)

```text
LibraryManagement/
├── Database/
│   └── ScriptDB.sql          # Script khởi tạo CSDL QuanLyThuVien, bảng, khóa ngoại và dữ liệu mẫu
├── DTO/                      # Data Transfer Object (Các lớp đối tượng truyền dữ liệu)
│   ├── DocGiaDTO.cs
│   ├── DauSachDTO.cs
│   ├── BanSaoSachDTO.cs
│   └── PhieuMuonDTO.cs
├── DAL/                      # Data Access Layer (Truy xuất SQL Server & xử lý SqlTransaction)
│   ├── DBConnect.cs
│   ├── DocGiaDAL.cs
│   ├── SachDAL.cs
│   └── PhieuMuonDAL.cs
├── BUS/                      # Business Logic Layer (Xử lý quy tắc nghiệp vụ & kiểm tra hợp lệ)
│   ├── DocGiaBUS.cs
│   ├── SachBUS.cs
│   └── PhieuMuonBUS.cs
├── GUI/                      # Graphical User Interface (Giao diện Windows Forms)
│   ├── Program.cs
│   ├── frmMain.cs
│   ├── frmDocGia.cs
│   ├── frmSach.cs
│   ├── frmMuonSach.cs
│   └── frmTraSach.cs
├── image/              # Hình ảnh chụp màn hình thực tế các chức năng
└── README.md                 # Tài liệu hướng dẫn và mô tả dự án

```

---

## 2. Công nghệ Sử dụng

* **Ngôn ngữ & Nền tảng:** C# (.NET Windows Forms)
* **Hệ quản trị Cơ sở dữ liệu:** Microsoft SQL Server
* **Thư viện truy xuất dữ liệu:** `System.Data.SqlClient` (ADO.NET thuần, sử dụng `SqlTransaction` đảm bảo tính toàn vẹn dữ liệu khi mượn/trả sách và nhập bản sao)
* **Môi trường phát triển:** Visual Studio 2022 / SQL Server Management Studio (SSMS)

---

## 3. Thiết kế Cơ sở dữ liệu (`QuanLyThuVien`)

Hệ thống gồm **5 bảng chính** liên kết chặt chẽ thông qua khóa chính và khóa ngoại:

1. **`DocGia`**: Lưu thông tin độc giả (`MaDocGia`, `HoTen`, `SoDienThoai`, `NgayLapThe`, `NgayHetHan`, `TrangThai`).
2. **`DauSach`**: Lưu thông tin chung của tựa sách (`MaDauSach`, `TenSach`, `TacGia`, `TheLoai`, `NamXuatBan`, `SoLuongTong`).
3. **`BanSaoSach`**: Quản lý từng cuốn sách vật lý cụ thể (`MaBanSao`, `MaDauSach`, `TinhTrang`: *Sẵn sàng*, *Đang mượn*, *Hỏng*).
4. **`PhieuMuon`**: Lưu thông tin lượt mượn (`MaPhieuMuon`, `MaDocGia`, `NgayMuon`, `HanTra`, `TrangThai`).
5. **`ChiTietPhieuMuon`**: Lưu chi tiết từng bản sao trong phiếu mượn (`MaPhieuMuon`, `MaBanSao`, `NgayTraThucTe`, `TienPhat`).

---

## 4. Các Chức năng Chính & Quy tắc Nghiệp vụ

1. **Quản lý Độc giả (`frmDocGia`):**
* Thêm mới độc giả, cập nhật thông tin cá nhân, khóa/mở khóa thẻ và gia hạn ngày sử dụng thẻ thư viện.
* Kiểm tra ràng buộc mã độc giả duy nhất và ngày hết hạn phải lớn hơn ngày lập thẻ.


2. **Quản lý Đầu sách & Bản sao Vật lý (`frmSach`):**
* Phân tách rõ giữa **Đầu sách** (tựa sách) và **Bản sao sách** (từng cuốn vật lý có mã vạch riêng).
* Khi nhập thêm bản sao mới, hệ thống sử dụng `SqlTransaction` để đồng thời lưu bản sao và tự động cộng dồn `SoLuongTong` cho đầu sách tương ứng.


3. **Lập Phiếu Mượn Sách (`frmMuonSach`):**
* Chỉ hiển thị danh sách các bản sao sách đang ở trạng thái **Sẵn sàng**.
* Kiểm tra tự động các quy tắc nghiệp vụ trước khi cho mượn:
* Thẻ độc giả phải ở trạng thái **Hoạt động** và **còn hạn sử dụng**.
* Độc giả không có sách mượn quá hạn chưa trả.
* Tổng số sách đang mượn (bao gồm sách cũ chưa trả + sách mượn mới) **không vượt quá 5 cuốn**.


* Thực thi `SqlTransaction`: tạo phiếu mượn, lưu chi tiết phiếu mượn và chuyển trạng thái các bản sao được chọn sang **Đang mượn**.


4. **Trả Sách & Tính Phí Phạt Quá Hạn (`frmTraSach`):**
* Hiển thị danh sách toàn bộ các cuốn sách đang được mượn.
* Tự động tính số ngày trễ hạn và tiền phạt theo định mức **`5.000 VNĐ / ngày / cuốn`** nếu trả sau ngày `HanTra`.
* Khi xác nhận trả sách, hệ thống cập nhật ngày trả thực tế, chuyển trạng thái bản sao về **Sẵn sàng** và tự động chuyển trạng thái phiếu mượn sang **Hoàn thành** nếu tất cả sách trong phiếu đã được trả đủ.



---

## 5. Hướng dẫn Cài đặt & Chạy Ứng dụng

1. **Bước 1 – Khởi tạo Cơ sở dữ liệu:**
* Mở **SQL Server Management Studio (SSMS)**.
* Mở file `Database/ScriptDB.sql` và nhấn **Execute (F5)** để tạo database `QuanLyThuVien` cùng dữ liệu mẫu.


2. **Bước 2 – Cấu hình Chuỗi kết nối:**
* Mở file `DAL/DBConnect.cs`.
* Chỉnh sửa tham số `Data Source` trong biến `connectionString` cho phù hợp với tên SQL Server trên máy của bạn (ví dụ: `Data Source=.;` hoặc `Data Source=localhost\SQLEXPRESS;`).


3. **Bước 3 – Cài đặt Thư viện & Chạy Ứng dụng:**
* Mở file Solution (`.sln` hoặc `.slnx`) bằng **Visual Studio**.
* Đảm bảo đã cài đặt gói NuGet `System.Data.SqlClient`.
* Nhấn **F5** (hoặc nút **Start**) để biên dịch và chạy chương trình.



---

## 6. Hình ảnh Minh họa Giao diện

*(Lưu ý: Đặt các ảnh chụp màn hình vào thư mục `Screenshots/` theo đúng tên bên dưới)*

| Giao diện Màn hình Chính | Quản lý Độc giả |

<img width="952" height="563" alt="bai6 6-1" src="https://github.com/user-attachments/assets/31c34578-4375-466c-ad07-7b58a098d102" />

<img width="908" height="562" alt="bai6 6-3" src="https://github.com/user-attachments/assets/62550f6e-ac8a-4ea4-b0e6-c387264d1c86" />

| Quản lý Đầu sách & Bản sao | Lập Phiếu Mượn Sách |

<img width="932" height="572" alt="bai6 6-2" src="https://github.com/user-attachments/assets/21635ca6-3145-4de8-b187-417845b8a5fb" />

<img width="1321" height="697" alt="bai6 6-4" src="https://github.com/user-attachments/assets/7c30c6f5-2d9d-497c-b0a2-612fd07c92e3" />

| Trả Sách & Tính Phí Phạt |

<img width="986" height="735" alt="bai6 6-5" src="https://github.com/user-attachments/assets/022fbfe0-7b25-4ce9-8321-961b93f3bf2a" />













```

```
