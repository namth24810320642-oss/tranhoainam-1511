CREATE DATABASE BilliardsDB;
GO
USE BilliardsDB;
GO

CREATE TABLE BanBida (
    MaBan INT IDENTITY(1,1) PRIMARY KEY,
    TenBan NVARCHAR(50) NOT NULL,
    LoaiBan NVARCHAR(30),
    GiaTheoGio DECIMAL(18,2) NOT NULL,
    TrangThai NVARCHAR(20) DEFAULT N'Trống' -- 'Trống', 'Đang chơi'
);

CREATE TABLE MenuDichVu (
    MaDV INT IDENTITY(1,1) PRIMARY KEY,
    TenDV NVARCHAR(100) NOT NULL,
    DonGia DECIMAL(18,2) NOT NULL,
    TonKho INT NULL -- NULL đối với dịch vụ cho thuê (vd: Thuê gậy) không cần trừ kho
);

CREATE TABLE HoaDon (
    MaHD INT IDENTITY(1,1) PRIMARY KEY,
    MaBan INT NOT NULL FOREIGN KEY REFERENCES BanBida(MaBan),
    GioBatDau DATETIME NOT NULL DEFAULT GETDATE(),
    GioKetThuc DATETIME NULL,
    TienGioChoi DECIMAL(18,2) DEFAULT 0,
    TienDichVu DECIMAL(18,2) DEFAULT 0,
    TrangThai BIT DEFAULT 0 -- 0: Chưa thanh toán, 1: Đã thanh toán
);

CREATE TABLE ChiTietHoaDon (
    MaHD INT NOT NULL FOREIGN KEY REFERENCES HoaDon(MaHD),
    MaDV INT NOT NULL FOREIGN KEY REFERENCES MenuDichVu(MaDV),
    SoLuong INT NOT NULL CHECK (SoLuong > 0),
    DonGia DECIMAL(18,2) NOT NULL,
    PRIMARY KEY (MaHD, MaDV)
);

INSERT INTO BanBida (TenBan, LoaiBan, GiaTheoGio, TrangThai) VALUES
(N'Bàn Lỗ 01', N'Pool (Lỗ)', 60000, N'Trống'),
(N'Bàn Lỗ 02', N'Pool (Lỗ)', 60000, N'Trống'),
(N'Bàn Carom 01', N'Carom (Phăng)', 50000, N'Trống'),
(N'Bàn Snooker VIP', N'Snooker', 100000, N'Trống');

INSERT INTO MenuDichVu (TenDV, DonGia, TonKho) VALUES
(N'Nước tăng lực Sting', 15000, 50),
(N'Mì xào bò', 35000, 20),
(N'Thuê gậy Carbon', 30000, NULL);

INSERT INTO HoaDon (MaBan, GioBatDau, GioKetThuc, TienGioChoi, TienDichVu, TrangThai) VALUES
(1, DATEADD(MINUTE, -45, GETDATE()), NULL, 0, 0, 0);

