-- phpMyAdmin SQL Dump
-- version 4.8.5
-- https://www.phpmyadmin.net/
--
-- Máy chủ: 127.0.0.1
-- Thời gian đã tạo: Th5 01, 2019 lúc 12:07 PM
-- Phiên bản máy phục vụ: 10.1.38-MariaDB
-- Phiên bản PHP: 7.3.4

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Cơ sở dữ liệu: `comdb`
--

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `chitiethd`
--

CREATE TABLE `chitiethd` (
  `idHD` int(11) NOT NULL,
  `idSp` int(11) NOT NULL,
  `soluong` varchar(45) DEFAULT NULL,
  `giaSP` float DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `danhmuc`
--

CREATE TABLE `danhmuc` (
  `iddanhMuc` int(11) NOT NULL,
  `tenDM` varchar(100) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Đang đổ dữ liệu cho bảng `danhmuc`
--

INSERT INTO `danhmuc` (`iddanhMuc`, `tenDM`) VALUES
(1, 'gà'),
(2, 'cơm'),
(3, 'mì xào, nuôi xào'),
(4, 'canh'),
(5, 'đồ chiên');

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `hoadon`
--

CREATE TABLE `hoadon` (
  `idhoadon` int(11) NOT NULL,
  `idNguoimua` int(11) NOT NULL,
  `idchiTiet` int(11) NOT NULL,
  `tongTien` float DEFAULT NULL,
  `soLuong` int(11) DEFAULT NULL,
  `ngayTao` date DEFAULT NULL,
  `tinhTrang` varchar(45) COLLATE utf8_unicode_ci DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `khachhang`
--

CREATE TABLE `khachhang` (
  `idKhachHang` int(11) NOT NULL,
  `nameKH` varchar(100) DEFAULT NULL,
  `email` varchar(100) NOT NULL,
  `password` varchar(500) NOT NULL,
  `address` varchar(500) DEFAULT NULL,
  `soDiethoai` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Đang đổ dữ liệu cho bảng `khachhang`
--

INSERT INTO `khachhang` (`idKhachHang`, `nameKH`, `email`, `password`, `address`, `soDiethoai`) VALUES
(22222, 'qweqwe', 'qweqweqwe', 'qweqweqwe', '2222', 33333),
(123123, 'sadasd', 'meo@gmail.com', '123123', '123123', 123123123);

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `sanpham`
--

CREATE TABLE `sanpham` (
  `idsanPham` int(11) NOT NULL,
  `tenSP` varchar(50) DEFAULT NULL,
  `giaSP` float DEFAULT NULL,
  `hinh1` varchar(100) DEFAULT NULL,
  `hinh2` varchar(100) DEFAULT NULL,
  `hinh3` varchar(100) DEFAULT NULL,
  `hinh4` varchar(100) DEFAULT NULL,
  `moTa` varchar(1000) DEFAULT NULL,
  `danhMuc` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Đang đổ dữ liệu cho bảng `sanpham`
--

INSERT INTO `sanpham` (`idsanPham`, `tenSP`, `giaSP`, `hinh1`, `hinh2`, `hinh3`, `hinh4`, `moTa`, `danhMuc`) VALUES
(1, 'cơm gà', 50000, NULL, NULL, NULL, NULL, 'ngon đó', 2);

--
-- Chỉ mục cho các bảng đã đổ
--

--
-- Chỉ mục cho bảng `chitiethd`
--
ALTER TABLE `chitiethd`
  ADD PRIMARY KEY (`idSp`,`idHD`),
  ADD KEY `hoadonFK_idx` (`idHD`);

--
-- Chỉ mục cho bảng `danhmuc`
--
ALTER TABLE `danhmuc`
  ADD PRIMARY KEY (`iddanhMuc`);

--
-- Chỉ mục cho bảng `hoadon`
--
ALTER TABLE `hoadon`
  ADD PRIMARY KEY (`idhoadon`),
  ADD KEY `nguoiMua_idx` (`idNguoimua`);

--
-- Chỉ mục cho bảng `khachhang`
--
ALTER TABLE `khachhang`
  ADD PRIMARY KEY (`idKhachHang`),
  ADD UNIQUE KEY `email_UNIQUE` (`email`);

--
-- Chỉ mục cho bảng `sanpham`
--
ALTER TABLE `sanpham`
  ADD PRIMARY KEY (`idsanPham`),
  ADD KEY `danhMucFK_idx` (`danhMuc`);

--
-- AUTO_INCREMENT cho các bảng đã đổ
--

--
-- AUTO_INCREMENT cho bảng `danhmuc`
--
ALTER TABLE `danhmuc`
  MODIFY `iddanhMuc` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT cho bảng `sanpham`
--
ALTER TABLE `sanpham`
  MODIFY `idsanPham` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- Các ràng buộc cho các bảng đã đổ
--

--
-- Các ràng buộc cho bảng `chitiethd`
--
ALTER TABLE `chitiethd`
  ADD CONSTRAINT `hoadonFK` FOREIGN KEY (`idHD`) REFERENCES `hoadon` (`idhoadon`),
  ADD CONSTRAINT `sanphamFK` FOREIGN KEY (`idSp`) REFERENCES `sanpham` (`idsanPham`);

--
-- Các ràng buộc cho bảng `hoadon`
--
ALTER TABLE `hoadon`
  ADD CONSTRAINT `nguoiMua` FOREIGN KEY (`idNguoimua`) REFERENCES `khachhang` (`idKhachHang`);

--
-- Các ràng buộc cho bảng `sanpham`
--
ALTER TABLE `sanpham`
  ADD CONSTRAINT `danhMucFK` FOREIGN KEY (`danhMuc`) REFERENCES `danhmuc` (`iddanhMuc`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
