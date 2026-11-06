USE QuanLyChamCong;
GO

CREATE TABLE NhanVien
(
    MaNV VARCHAR(20) PRIMARY KEY,
    Ten NVARCHAR(100) NOT NULL,
    MatKhau VARCHAR(255) NOT NULL,
    ChucVu NVARCHAR(20) NOT NULL DEFAULT N'NhanVien',

    CONSTRAINT CK_NhanVien_ChucVu
        CHECK (ChucVu IN (N'NhanVien', N'Admin'))
);
GO

CREATE TABLE ChamCong
(
    ID INT IDENTITY(1,1) PRIMARY KEY,
    MaNV VARCHAR(20) NOT NULL,
    NgayChamCong DATE NOT NULL,
    GioVao DATETIME2 NULL,
    GioRa DATETIME2 NULL,
    IP VARCHAR(45) NULL,
    TrangThai NVARCHAR(30) NOT NULL DEFAULT N'DungGio',

    CONSTRAINT FK_ChamCong_NhanVien
        FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV),

    CONSTRAINT UQ_ChamCong_MaNV_Ngay
        UNIQUE (MaNV, NgayChamCong),

    CONSTRAINT CK_ChamCong_TrangThai
        CHECK (TrangThai IN
        (N'DungGio', N'DiMuon', N'ChuaCheckOut'))
);
GO