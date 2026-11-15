CREATE DATABASE ClinicDB;
GO
USE ClinicDB;
GO

CREATE TABLE BenhNhan (
    MaBN INT IDENTITY(1,1) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    SoDienThoai VARCHAR(15) UNIQUE NOT NULL,
    NamSinh INT
);

CREATE TABLE HoSoKham (
    MaHoSo INT IDENTITY(1,1) PRIMARY KEY,
    MaBN INT NOT NULL FOREIGN KEY REFERENCES BenhNhan(MaBN),
    NgayKham DATETIME DEFAULT GETDATE(),
    TrieuChung NVARCHAR(MAX),
    ChanDoan NVARCHAR(250)
);

CREATE TABLE Thuoc (
    MaThuoc INT IDENTITY(1,1) PRIMARY KEY,
    TenThuoc NVARCHAR(150) NOT NULL,
    DonViTinh NVARCHAR(20),
    DonGia DECIMAL(18,2) NOT NULL,
    SoLuongTon INT NOT NULL CHECK (SoLuongTon >= 0)
);

CREATE TABLE DonThuoc (
    MaDon INT IDENTITY(1,1) PRIMARY KEY,
    MaHoSo INT NOT NULL FOREIGN KEY REFERENCES HoSoKham(MaHoSo),
    TienKham DECIMAL(18,2) DEFAULT 0,
    TongTien DECIMAL(18,2) DEFAULT 0,
    TrangThai NVARCHAR(30) DEFAULT N'Chờ lấy thuốc' -- 'Chờ lấy thuốc', 'Đã thanh toán'
);

CREATE TABLE ChiTietDonThuoc (
    MaDon INT NOT NULL FOREIGN KEY REFERENCES DonThuoc(MaDon),
    MaThuoc INT NOT NULL FOREIGN KEY REFERENCES Thuoc(MaThuoc),
    SoLuong INT NOT NULL CHECK (SoLuong > 0),
    LieuDung NVARCHAR(100),
    PRIMARY KEY (MaDon, MaThuoc)
);


INSERT INTO Thuoc (TenThuoc, DonViTinh, DonGia, SoLuongTon) VALUES
(N'Paracetamol 500mg', N'Vỉ', 15000, 100),
(N'Amoxicillin 500mg', N'Viên', 3000, 200),
(N'Siro ho Bảo Thanh', N'Chai', 55000, 30);

INSERT INTO DonThuoc (MaHoSo, TienKham, TongTien, TrangThai) VALUES
(1, 100000, 160000, N'Chờ lấy thuốc');

INSERT INTO HoSoKham (MaBN, NgayKham, TrieuChung, ChanDoan) VALUES
(1, GETDATE(), N'Sốt nhẹ, đau họng, ho khan 2 ngày', N'Viêm họng cấp'),
(2, GETDATE(), N'Đau đầu, chóng mặt, mệt mỏi', N'Rối loạn tiền đình');