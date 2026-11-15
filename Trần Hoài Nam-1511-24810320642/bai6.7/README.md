
# Hệ thống Quản lý Phòng khám Tư nhân (Clinic Management System) - Bài 6.7

Ứng dụng Windows Forms quản lý toàn diện quy trình khám chữa bệnh tại phòng khám tư nhân: tiếp nhận bệnh nhân, tra cứu lịch sử hành chính, lập hồ sơ khám, quản lý kho dược phẩm, kê đơn thuốc tự động trừ tồn kho và thanh toán hóa đơn. Dự án được xây dựng bằng ngôn ngữ C# và hệ quản trị cơ sở dữ liệu SQL Server theo mô hình kiến trúc 3 lớp (3-Tier Architecture).

---

## 1. Cấu trúc Dự án (Kiến trúc 3 lớp)

```text
ClinicManagement/
├── Database/
│   └── ScriptDB.sql          # Script khởi tạo CSDL QuanLyPhongKham, bảng và dữ liệu mẫu
├── DTO/                      # Data Transfer Object (Các đối tượng truyền dữ liệu)
│   ├── BenhNhanDTO.cs
│   ├── HoSoKhamDTO.cs
│   ├── ThuocDTO.cs
│   ├── DonThuocDTO.cs
│   └── ChiTietDonThuocDTO.cs
├── DAL/                      # Data Access Layer (Truy xuất CSDL & xử lý SqlTransaction)
│   ├── DBConnect.cs
│   ├── BenhNhanDAL.cs
│   ├── HoSoKhamDAL.cs
│   ├── ThuocDAL.cs
│   └── DonThuocDAL.cs
├── BUS/                      # Business Logic Layer (Xử lý nghiệp vụ & kiểm tra ràng buộc)
│   ├── BenhNhanBUS.cs
│   ├── HoSoKhamBUS.cs
│   ├── ThuocBUS.cs
│   └── DonThuocBUS.cs
├── GUI/                      # Graphical User Interface (Giao diện Windows Forms)
│   ├── Program.cs
│   ├── frmMain.cs
│   ├── frmTiepNhan.cs
│   ├── frmThuoc.cs
│   ├── frmKeDon.cs
│   └── frmThanhToan.cs
├── image/                    # Thư mục chứa hình ảnh minh họa giao diện chạy thực tế
└── README.md                 # Tài liệu hướng dẫn và mô tả dự án

```

---

## 2. Công nghệ Sử dụng

* **Ngôn ngữ & Nền tảng:** C# (.NET Windows Forms)
* **Hệ quản trị Cơ sở dữ liệu:** Microsoft SQL Server
* **Thư viện truy xuất dữ liệu:** `System.Data.SqlClient` (ADO.NET thuần, sử dụng `SqlTransaction` đảm bảo toàn vẹn dữ liệu khi kê đơn và trừ kho thuốc)
* **Công cụ phát triển:** Visual Studio 2022 / SQL Server Management Studio (SSMS)

---

## 3. Thiết kế Cơ sở dữ liệu (`QuanLyPhongKham`)

Hệ thống gồm **5 bảng chính** liên kết chặt chẽ thông qua khóa chính và khóa ngoại:

1. **`BenhNhan`**: Lưu thông tin hành chính bệnh nhân (`MaBN`, `HoTen`, `SoDienThoai`, `NgaySinh`, `GioiTinh`, `DiaChi`).
2. **`HoSoKham`**: Lưu thông tin từng lượt khám (`MaHoSo`, `MaBN`, `NgayKham`, `TrieuChung`, `ChanDoan`, `TienKham`, `TrangThai`).
3. **`Thuoc`**: Quản lý danh mục dược phẩm (`MaThuoc`, `TenThuoc`, `DonViTinh`, `DonGia`, `SoLuongTon`).
4. **`DonThuoc`**: Lưu thông tin đơn thuốc gắn với hồ sơ khám (`MaDonThuoc`, `MaHoSo`, `NgayKeDon`, `TongTienThuoc`, `DaThanhToan`).
5. **`ChiTietDonThuoc`**: Chi tiết từng loại thuốc trong đơn (`MaDonThuoc`, `MaThuoc`, `SoLuong`, `DonGiaBan`, `LieuDung`).

---

## 4. Các Chức năng Chính & Quy tắc Nghiệp vụ

1. **Tiếp nhận Bệnh nhân & Khám bệnh (`frmTiepNhan`):**
* Tra cứu nhanh hồ sơ bệnh nhân cũ bằng **Số điện thoại**; tự động thêm mới bệnh nhân nếu đến khám lần đầu.
* Ghi nhận triệu chứng lâm sàng, chẩn đoán của bác sĩ và tiền công khám vào Hồ sơ khám.


2. **Quản lý Kho Thuốc (`frmThuoc`):**
* Thêm mới loại thuốc vào danh mục.
* Cập nhật đơn giá bán và nhập thêm số lượng thuốc mới vào kho (`SoLuongTon`).


3. **Bác sĩ Kê Đơn Thuốc & Trừ Kho Tự động (`frmKeDon`):**
* Chọn hồ sơ khám đang chờ kê đơn, thêm thuốc kèm số lượng và hướng dẫn liều dùng.
* Kiểm tra chặt chẽ điều kiện **số lượng kê không được vượt quá tồn kho hiện tại**.
* Khi lưu đơn thuốc, hệ thống thực thi `SqlTransaction`: tạo đơn thuốc, lưu chi tiết thuốc, **trừ trực tiếp số lượng tồn kho** trong bảng `Thuoc` và cập nhật trạng thái hồ sơ sang *Đã kê đơn*.


4. **Thanh toán Hóa đơn & In Phiếu (`frmThanhToan`):**
* Tự động tổng hợp **Tiền khám + Tiền thuốc** thành tổng số tiền bệnh nhân cần thanh toán.
* Hỗ trợ xác nhận thu tiền (cập nhật trạng thái `DaThanhToan`) và hiển thị/in phiếu hóa đơn chi tiết.



---

## 5. Hướng dẫn Cài đặt & Chạy Ứng dụng

1. **Bước 1 – Khởi tạo Cơ sở dữ liệu:**
* Mở **SQL Server Management Studio (SSMS)**.
* Mở file `Database/ScriptDB.sql` và nhấn **Execute (F5)** để khởi tạo database `QuanLyPhongKham` cùng dữ liệu mẫu.


2. **Bước 2 – Cấu hình Chuỗi kết nối:**
* Mở file `DAL/DBConnect.cs`.
* Cập nhật tham số `Data Source` trong chuỗi `connectionString` cho khớp với tên SQL Server trên máy của bạn (ví dụ: `Data Source=.;` hoặc `Data Source=localhost\SQLEXPRESS;`).


3. **Bước 3 – Biên dịch & Chạy Ứng dụng:**
* Mở file Solution (`.sln` hoặc `.slnx`) bằng **Visual Studio**.
* Đảm bảo đã cài đặt gói NuGet `System.Data.SqlClient`.
* Nhấn **F5** (hoặc **Start**) để chạy ứng dụng.



---

## 6. Hình ảnh Minh họa Giao diện

*(Lưu ý: Đặt các ảnh chụp màn hình vào thư mục `image/` và đổi tên khớp với các dòng bên dưới)*

| Giao diện Màn hình Chính | Tiếp nhận & Khám Bệnh |

<img width="953" height="565" alt="bai6 7-1" src="https://github.com/user-attachments/assets/bed10803-0ed3-4ee4-8dbf-30e3f5044721" />

<img width="992" height="700" alt="bai6 7-2" src="https://github.com/user-attachments/assets/61bcbb57-9b1c-4c3c-800b-82a7fe128cc2" />


| Quản lý Kho Thuốc | Bác sĩ Kê Đơn Thuốc |

<img width="968" height="587" alt="bai6 7-3" src="https://github.com/user-attachments/assets/8bb74d98-8921-415e-a37f-8bf59871f747" />

<img width="1090" height="712" alt="bai6 7-4" src="https://github.com/user-attachments/assets/cf07bf39-7d39-45e9-a1d1-2d4cc275d594" />

| Thanh toán & In Hóa đơn |

<img width="1052" height="702" alt="bai6 7-5" src="https://github.com/user-attachments/assets/c44b7c27-c5b7-4f31-a9be-b628d96f0aca" />

















```

```
