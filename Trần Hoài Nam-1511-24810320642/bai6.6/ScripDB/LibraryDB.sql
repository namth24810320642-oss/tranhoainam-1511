CREATE DATABASE LibraryDB;
GO
USE LibraryDB;
GO

CREATE TABLE DocGia (
    MaDocGia INT IDENTITY(1,1) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    Email VARCHAR(100) UNIQUE,
    NgayHetHanThe DATETIME NOT NULL
);

CREATE TABLE DauSach (
    MaSach INT IDENTITY(1,1) PRIMARY KEY,
    TenSach NVARCHAR(200) NOT NULL,
    TheLoai NVARCHAR(50),
    SoLuongCon INT NOT NULL CHECK (SoLuongCon >= 0)
);

CREATE TABLE PhieuMuon (
    MaPhieu INT IDENTITY(1,1) PRIMARY KEY,
    MaDocGia INT NOT NULL FOREIGN KEY REFERENCES DocGia(MaDocGia),
    NgayMuon DATETIME DEFAULT GETDATE(),
    HanTra DATETIME NOT NULL,
    TrangThai NVARCHAR(50) DEFAULT N'Đang mượn' -- 'Đang mượn', 'Đã trả', 'Quá hạn'
);

CREATE TABLE ChiTietMuon (
    MaPhieu INT NOT NULL FOREIGN KEY REFERENCES PhieuMuon(MaPhieu),
    MaSach INT NOT NULL FOREIGN KEY REFERENCES DauSach(MaSach),
    SoLuong INT NOT NULL CHECK (SoLuong > 0),
    PRIMARY KEY (MaPhieu, MaSach)
);


INSERT INTO DocGia (HoTen, Email, NgayHetHanThe) VALUES
(N'Nguyễn Văn An', 'an.nv@library.vn', '2027-12-31'),
(N'Trần Thị Bình', 'binh.tt@library.vn', '2027-06-30'),
(N'Lê Hoàng Cường', 'cuong.lh@library.vn', '2025-01-15'); -- Thẻ hết hạn

INSERT INTO DauSach (TenSach, TheLoai, SoLuongCon) VALUES
(N'Lập trình C# và ADO.NET', N'Công nghệ thông tin', 12),
(N'Cấu trúc dữ liệu và Giải thuật', N'Công nghệ thông tin', 8),
(N'Kinh tế học vĩ mô', N'Kinh tế', 10),
(N'Quản trị tài chính doanh nghiệp', N'Kinh tế', 6),
(N'Nhà giả kim', N'Văn học', 15),
(N'Tuổi trẻ đáng giá bao nhiêu', N'Văn học', 9);

INSERT INTO PhieuMuon (MaDocGia, NgayMuon, HanTra, TrangThai) VALUES
(1, DATEADD(DAY, -2, GETDATE()), DATEADD(DAY, 5, GETDATE()), N'Đang mượn'),   
(2, DATEADD(DAY, -15, GETDATE()), DATEADD(DAY, -3, GETDATE()), N'Quá hạn'),    
(1, DATEADD(DAY, -20, GETDATE()), DATEADD(DAY, -10, GETDATE()), N'Đã trả');   
