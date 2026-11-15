
# Hệ thống Quản lý Quán Bida (Billiards Club Management System) - Bài 6.8

Ứng dụng Windows Forms quản lý hoạt động kinh doanh câu lạc bộ Bida theo thời gian thực: theo dõi sơ đồ bàn trực quan, tính tiền giờ chơi tự động, gọi thêm đồ ăn/thức uống, chuyển bàn linh hoạt, thanh toán hóa đơn và thống kê doanh thu. Dự án được xây dựng bằng ngôn ngữ C# và hệ quản trị cơ sở dữ liệu SQL Server theo mô hình kiến trúc 3 lớp (3-Tier Architecture).

---

## 1. Cấu trúc Dự án (Kiến trúc 3 lớp)

```text
BilliardsManagement/
├── Database/
│   └── ScriptDB.sql          # Script khởi tạo CSDL QuanLyQuanBida, bảng và dữ liệu mẫu
├── DTO/                      # Data Transfer Object (Các đối tượng truyền dữ liệu)
│   ├── BanBidaDTO.cs
│   ├── DichVuDTO.cs
│   ├── HoaDonDTO.cs
│   └── ChiTietHoaDonDTO.cs
├── DAL/                      # Data Access Layer (Truy xuất CSDL & xử lý SqlTransaction)
│   ├── DBConnect.cs
│   ├── BanBidaDAL.cs
│   ├── DichVuDAL.cs
│   └── HoaDonDAL.cs
├── BUS/                      # Business Logic Layer (Xử lý quy tắc nghiệp vụ & tính toán)
│   ├── BanBidaBUS.cs
│   ├── DichVuBUS.cs
│   └── HoaDonBUS.cs
├── GUI/                      # Graphical User Interface (Giao diện Windows Forms)
│   ├── Program.cs
│   ├── frmMain.cs
│   ├── frmSoDoBan.cs
│   ├── frmQuanLyBanDichVu.cs
│   └── frmDoanhThu.cs
├── image/              # Hình ảnh chụp màn hình giao diện chạy thực tế
└── README.md                 # Tài liệu hướng dẫn và mô tả dự án

```

---

## 2. Công nghệ Sử dụng

* **Ngôn ngữ & Nền tảng:** C# (.NET Windows Forms)
* **Hệ quản trị Cơ sở dữ liệu:** Microsoft SQL Server
* **Thư viện truy xuất dữ liệu:** `System.Data.SqlClient` (ADO.NET thuần, sử dụng `SqlTransaction` đảm bảo tính toàn vẹn dữ liệu khi mở bàn, chuyển bàn, trừ tồn kho dịch vụ và thanh toán)
* **Công cụ phát triển:** Visual Studio 2022 / SQL Server Management Studio (SSMS)

---

## 3. Thiết kế Cơ sở dữ liệu (`QuanLyQuanBida`)

Hệ thống gồm **4 bảng chính** liên kết chặt chẽ thông qua khóa chính và khóa ngoại:

1. **`BanBida`**: Quản lý danh sách bàn chơi (`MaBan`, `TenBan`, `LoaiBan` — Lỗ / Phăng / VIP, `GiaGio`, `TrangThai` — Trống / Đang chơi).
2. **`DichVu`**: Quản lý menu đồ ăn, nước uống, phụ kiện (`MaDV`, `TenDV`, `DonViTinh`, `DonGia`, `SoLuongTon`).
3. **`HoaDon`**: Lưu thông tin phiên chơi của khách (`MaHD`, `MaBan`, `GioVao`, `GioRa`, `TienGio`, `TienDichVu`, `GiamGia`, `TongTien`, `TrangThai` — Chưa thanh toán / Đã thanh toán).
4. **`ChiTietHoaDon`**: Lưu chi tiết các dịch vụ khách đã gọi trong từng hóa đơn (`MaHD`, `MaDV`, `SoLuong`, `DonGiaBan`).

---

## 4. Các Chức năng Chính & Quy tắc Nghiệp vụ

1. **Sơ đồ Bàn Trực quan & Mở bàn (`frmSoDoBan`):**
* Hiển thị trực quan danh sách toàn bộ bàn Bida phân màu theo trạng thái (**Trống** / **Đang chơi**).
* Khi mở bàn, hệ thống thực thi `SqlTransaction`: khởi tạo hóa đơn mới ghi nhận mốc thời gian `GioVao` và chuyển trạng thái bàn sang **Đang chơi**.


2. **Gọi Món / Dịch vụ & Trừ Tồn Kho Tự động:**
* Cho phép thêm nước uống, đồ ăn vào hóa đơn của bàn đang hoạt động; tự động cộng dồn số lượng nếu khách gọi trùng món đã có trong hóa đơn.
* Ràng buộc kiểm tra số lượng gọi không vượt quá `SoLuongTon` và trừ trực tiếp tồn kho dịch vụ thông qua `SqlTransaction`.


3. **Chuyển Bàn Linh hoạt:**
* Chuyển toàn bộ phiên chơi (`HoaDon` hiện tại, giờ bắt đầu và danh sách dịch vụ đã gọi) từ bàn đang chơi sang một bàn **Trống** khác.
* Sử dụng `SqlTransaction` để đồng thời cập nhật `MaBan` trong hóa đơn, trả bàn cũ về trạng thái **Trống** và chuyển bàn mới sang **Đang chơi**.


4. **Tính Tiền Giờ Tự động & Thanh toán Hóa đơn:**
* Tự động tính tiền giờ dựa trên thời gian thực tế (`GioRa - GioVao`) nhân với `GiaGio` của từng loại bàn.
* Tổng hợp **Tiền giờ + Tiền dịch vụ**, hỗ trợ áp dụng mức giảm giá (theo %) và xuất hóa đơn chi tiết.
* Giải phóng bàn về trạng thái **Trống** ngay sau khi xác nhận thanh toán.


5. **Quản lý Danh mục & Thống kê Doanh thu (`frmQuanLyBanDichVu`, `frmDoanhThu`):**
* Thêm/cập nhật bàn Bida, điều chỉnh đơn giá giờ chơi, quản lý menu và nhập thêm hàng tồn kho.
* Thống kê tổng doanh thu theo khoảng thời gian tùy chọn (Từ ngày – Đến ngày), tách bạch rõ doanh thu từ tiền giờ và tiền dịch vụ.



---

## 5. Hướng dẫn Cài đặt & Chạy Ứng dụng

1. **Bước 1 – Khởi tạo Cơ sở dữ liệu:**
* Mở **SQL Server Management Studio (SSMS)**.
* Mở file `Database/ScriptDB.sql` và nhấn **Execute (F5)** để khởi tạo database `QuanLyQuanBida` cùng dữ liệu mẫu.


2. **Bước 2 – Cấu hình Chuỗi kết nối:**
* Mở file `DAL/DBConnect.cs`.
* Cập nhật tham số `Data Source` trong chuỗi `connectionString` cho khớp với tên SQL Server trên máy của bạn (ví dụ: `Data Source=.;` hoặc `Data Source=localhost\SQLEXPRESS;`).


3. **Bước 3 – Biên dịch & Chạy Ứng dụng:**
* Mở file Solution (`.sln` hoặc `.slnx`) bằng **Visual Studio**.
* Đảm bảo đã cài đặt gói NuGet `System.Data.SqlClient`.
* Nhấn **F5** (hoặc **Start**) để chạy ứng dụng.



---

## 6. Hình ảnh Minh họa Giao diện

*(Lưu ý: Đặt các ảnh chụp màn hình vào thư mục `Screenshots/` và đổi tên khớp với các dòng bên dưới)*

| Giao diện Màn hình Chính | Sơ đồ Bàn & Gọi Dịch vụ |

<img width="952" height="562" alt="bai6 8-1" src="https://github.com/user-attachments/assets/fa460842-6903-4479-8d47-0ec20870ee4c" />

<img width="1365" height="726" alt="bai6 8-2" src="https://github.com/user-attachments/assets/7b3af785-e6ee-4bd7-8256-2a1eacc6cb6e" />

| Chức năng Chuyển Bàn | Thanh toán Hóa đơn |

<img width="905" height="561" alt="bai6 8-3" src="https://github.com/user-attachments/assets/7eda4906-f098-4601-b3a0-7a12ae386b47" />

<img width="882" height="566" alt="bai6 8-4" src="https://github.com/user-attachments/assets/3d44637a-8bb8-4620-9ad3-7edd320e6dd5" />

| Quản lý Bàn/Dịch vụ & Thống kê Doanh thu |

<img width="1055" height="680" alt="bai6 8-5" src="https://github.com/user-attachments/assets/403b9510-1563-4526-adab-774b53e05737" />















```

```
