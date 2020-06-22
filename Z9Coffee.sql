

create database Z9TheCoffee

use Z9TheCoffee
--------------------------

create table ChiNhanh
(
	MaChiNhanh		int identity(1,1) primary key,
	TenChiNhanh		nvarchar(60),
	TrangThai		bit
)
--------------------------

create table QuanLy
(
	NgayVaoLam		datetime,
	MaNV			int identity(1,1) primary key,
	MaChiNhanh		int,	
	TenNhanVien		nvarchar(50),
	NamSinh			char(4),	
	SDT				char(11),
	DaNghi			bit,
	foreign key (MaChiNhanh) references ChiNhanh(MaChiNhanh) on update cascade,	
)
---------------------------

create table LoaiDoUong
(
	MaLoai	int identity(1,1) primary key,
	TenLoai	nvarchar(30),
	TrangThai	bit
)
---------------------------

Create table ChiTietDoUong
(
	NgayCapNhat		datetime,
	MaLoai			int,
	MaDoUong		int identity(1,1) primary key,	
	TenDoUong		nvarchar(50) not null,	
	GiaBan			decimal(18,0),	
	AnhBia			varchar(40),	
	TrangThai		bit,		
	foreign key(MaLoai) references LoaiDoUong(MaLoai) on update cascade
)
------------------------------

create table KhachHang
(
	MaKH			int identity(1,1) primary key,
	TenKH			nvarchar(50) ,
	Email			varchar(50) unique,
	MatKhau			nvarchar(50) ,		
)
--------------------------
create table  DatBan
(
	MaDatBan		int identity(1,1) primary key,
	MaChiNhanh		int,
	NgayGui			datetime,
	NgayDatBan		datetime,
	GioDatBan		varchar(5),
	SoLuongNguoi	int,
	TenKhachDatBan	nvarchar(70),
	DienThoaiDatBan	varchar(13),
	DaXong			bit,
	foreign key(MaChiNhanh) references ChiNhanh(MaChiNhanh) on update cascade
)
------------
create table QuanGiao
(
	MaQuan			int identity(1,1) primary key,	
	TenQuan			nvarchar(50),
	MaChiNhanh		int,
	TrangThai		bit,
	foreign key(MaChiNhanh) references ChiNhanh(MaChiNhanh) on update cascade
)
create table DonDatHang
(
	MaDonHang			int identity(1,1) primary key,
	MaNV				int,		
	NgayDat				datetime,
	GioDat				datetime,
	MaKH				int,--varchar(5),
	MaQuan				int,
	DiaChiGiao			nvarchar(70),
	DienThoai_KH		varchar(13),
	DaThanhToan			bit,
	foreign key(MaKH) references KhachHang(MaKH) on update cascade,
	foreign key(MaQuan) references QuanGiao(MaQuan) on update cascade	
)
-------------------------
------------

create table  ChiTietDonHang
(
	MaDonHang	int ,
	MaDoUong	int ,--varchar(5),
	primary key(MaDonHang,MaDoUong),
	SlMua		int,
	DonGia		decimal(18,0),
	foreign key (MaDoUong) references ChiTietDoUong(MaDoUong) on update cascade,
	foreign key (MaDonHang) references DonDatHang(MaDonHang) on update cascade
)

create table MayChu
(
	TaiKhoan_admin		nvarchar(50) primary key,
	MatKhau_admin		nvarchar(50)
)
Insert into MayChu values(N'admin','123')

-------------------------
Insert into ChiNhanh Values (N'Cơ sở Điện Biên Phủ',1)
Insert into ChiNhanh Values (N'Cơ sở Tô Ký',1)
-------------------------
SET dateformat dmy
Insert into QuanLy Values('2/10/2018',1,N'Đặng Ngọc Trầm','1997','0123',0)
Insert into QuanLy Values('30/10/2018',1,N'Đỗ Phạm Quang Khải','1997','',0)
Insert into QuanLy Values('31/10/2018',2,N'Phạm Ngọc Nhàn','1997','',0)
Insert into QuanLy Values('1/10/2018',2,N'Nguyễn Thành Luận','1997','4567',0)

-------------------------
INSERT LoaiDoUong(TenLoai,TrangThai) VALUES (N'Cà Phê Thường',1)
INSERT LoaiDoUong(TenLoai,TrangThai) VALUES (N'Cà Phê Ý',1)
INSERT LoaiDoUong(TenLoai,TrangThai) VALUES (N'Trà',1)
--------------------------
INSERT QuanGiao(TenQuan,MaChiNhanh,TrangThai) VALUES (N'Bình Thạnh',1,1)
INSERT QuanGiao(TenQuan,MaChiNhanh,TrangThai) VALUES (N'Gò Vấp',2,1)
INSERT QuanGiao(TenQuan,MaChiNhanh,TrangThai) VALUES (N'Quận 12',2,1)
--------------------------

set dateformat dmy
INSERT ChiTietDoUong(NgayCapNhat,MaLoai,TenDoUong,GiaBan,AnhBia,TrangThai) VALUES('17/7/2018',1,N'Cà Phê Đen',15000,'cafe-den.png',1)
INSERT ChiTietDoUong(NgayCapNhat,MaLoai,TenDoUong,GiaBan,AnhBia,TrangThai) VALUES('17/7/2018',1,N'Cà Phê Sữa',15000,'cafe-sua.png',1)
INSERT ChiTietDoUong(NgayCapNhat,MaLoai,TenDoUong,GiaBan,AnhBia,TrangThai) VALUES('17/7/2018',1,N'Cà Phê Phin',10000,'cafe-phin.png',1)
INSERT ChiTietDoUong(NgayCapNhat,MaLoai,TenDoUong,GiaBan,AnhBia,TrangThai) VALUES('17/7/2018',1,N'Bạc Xỉu',18000,'bac-xiu.png',1)


INSERT ChiTietDoUong(NgayCapNhat,MaLoai,TenDoUong,GiaBan,AnhBia,TrangThai) VALUES('17/7/2018',2,N'Espresso',35000,'espresso.png',1)
INSERT ChiTietDoUong(NgayCapNhat,MaLoai,TenDoUong,GiaBan,AnhBia,TrangThai) VALUES('17/7/2018',2,N'Cappuccino',45000,'cappuccino.png',1)
INSERT ChiTietDoUong(NgayCapNhat,MaLoai,TenDoUong,GiaBan,AnhBia,TrangThai) VALUES('17/7/2018',2,N'Latte',45000,'latte.png',1)
INSERT ChiTietDoUong(NgayCapNhat,MaLoai,TenDoUong,GiaBan,AnhBia,TrangThai) VALUES('17/7/2018',2,N'Latte Macchiato',40000,'latte-macchiato.png',1)
INSERT ChiTietDoUong(NgayCapNhat,MaLoai,TenDoUong,GiaBan,AnhBia,TrangThai) VALUES('17/7/2018',2,N'Mocha',45000,'mocha.png',1)
INSERT ChiTietDoUong(NgayCapNhat,MaLoai,TenDoUong,GiaBan,AnhBia,TrangThai) VALUES('17/7/2018',2,N'Americano',35000,'americano.png',1)


INSERT ChiTietDoUong(NgayCapNhat,MaLoai,TenDoUong,GiaBan,AnhBia,TrangThai) VALUES('17/7/2018',3,N'Trà Chanh',20000,'tra-chanh.png',1)
INSERT ChiTietDoUong(NgayCapNhat,MaLoai,TenDoUong,GiaBan,AnhBia,TrangThai) VALUES('17/7/2018',3,N'Trà Đào',20000,'tra-dao.png',1)
INSERT ChiTietDoUong(NgayCapNhat,MaLoai,TenDoUong,GiaBan,AnhBia,TrangThai) VALUES('17/7/2018',3,N'Trà Chanh Sả',25000,'tra-chanh-sa.png',1)
INSERT ChiTietDoUong(NgayCapNhat,MaLoai,TenDoUong,GiaBan,AnhBia,TrangThai) VALUES('17/7/2018',3,N'Trà Gừng Chanh Sả',30000,'tra-gung-chanh-sa.png',1)
	
--------------------------
Insert into KhachHang values ('Doraemon','doraemon@gmail.com',123)
Insert into KhachHang values ('Nobita','nobita@gmail.com',123)
Insert into KhachHang values ('Conan','conan@gmail.com',123)
Insert into KhachHang values (N'Nguyễn Thành Luận','thanhluan0932777503@gmail.com',123)
----------------------------
--set dateformat dmy
--Insert into DonDatHang values('12/07/2018','12:30',1,1,'1/2/3','123',0)
--Insert into DonDatHang values('13/07/2018','13:30',2,2,'2/3/4','234',0)
--Insert into DonDatHang values('14/07/2018','14:30',3,2,'3/4/5','345',0)
--------------------------------------
--Insert into NhanVienNhanDonHang values(1,1)


---------------------------------------------------------
--DBcC checkident ('ThuongHieuDan', reseed, 0)
--DBcC checkident ('TheLoaiDan', reseed, 0)
--DBcC checkident ('KhachHang', reseed, 3)


--drop table ChiTietDonHang
--drop  table QuanLy
--drop  table ChiTietDoUong
--drop table LoaiDoUong

--drop table DonDatHang
--drop table KhachHang
--drop  table QuanGiao
--drop  table ChiNhanh








----------------
select NgayCapNhat,MaDoUong,TenLoai,TenDoUong,GiaBan,AnhBia,TrangThai from LoaiDoUong,ChiTietDoUong where LoaiDoUong.MaLoai=ChiTietDoUong.MaLoai
select * from ChiTietDoUong
select * from LoaiDoUong
DBcC checkident ('KhachHang', reseed, 0)

select * from ChiNhanh
select * from QuanLy where MaChiNhanh=1
select * from ChiTietDoUong where TrangThai=''
INSERT ChiTietDoUong(NgayCapNhat,MaLoai,TenDoUong,GiaBan,AnhBia,TrangThai) VALUES('17/7/2018',1,N'Nước Mía',18000,'bac-xiu.png',N'Hết Phục Vụ')
select * from LoaiDoUong

select * from KhachHang
select * from QuanGiao

-- đơn dặt hàng đã giao
select MaDonHang,convert(varchar(10),NgayDat,103) as [NgayDat],convert(varchar(5),GioDat,108) as [GioDat],TenNhanVien as [TenNhanVienGiao],TenKH,TenQuan,DiaChiGiao,DienThoai_KH,DaThanhToan
from DonDatHang
	left join QuanLy 
		on DonDatHang.MaNV = QuanLy.MaNV 
left join KhachHang
on  DonDatHang.MaKH = KhachHang.MaKH
left join QuanGiao
on DonDatHang.MaQuan = QuanGiao.MaQuan
left join ChiNhanh
on QuanGiao.MaChiNhanh = ChiNhanh.MaChiNhanh
where DaThanhToan='true' and ChiNhanh.MaChiNhanh = 2

select * from DonDatHang

select MaDonHang,convert(varchar(10),NgayDat,103) as [NgayDat],convert(varchar(5),GioDat,108) as [GioDat],TenNhanVien as [TenNhanVienGiao],TenKH,TenQuan,DiaChiGiao,DienThoai_KH,DaThanhToan
from DonDatHang,QuanLy,KhachHang,QuanGiao
where DonDatHang.MaNV=QuanLy.MaNV
group by MaDonHang,NgayDat,GioDat,TenNhanVien,TenKH,TenQuan,DiaChiGiao,DienThoai_KH,DaThanhToan

select * from QuanLy

-- Đơn Đặt Hàng chưa giao
select MaDonHang,NgayDat,TenKH,TenQuan,DiaChiGiao,DaThanhToan
from DonDatHang,QuanGiao ,ChiNhanh,KhachHang,QuanLy
where QuanGiao.MaQuan = DonDatHang.MaQuan 
and ChiNhanh.MaChiNhanh=QuanGiao.MaChiNhanh 
and KhachHang.MaKH = DonDatHang.MaKH
--and QuanLy.MaNV = DonDatHang.MaNV
and ChiNhanh.MaChiNhanh =2
and DaThanhToan = 'false'
group by MaDonHang,NgayDat,TenKH,TenQuan,DiaChiGiao,DaThanhToan



select * from ChiTietDonHang

-- Chi Tiết Đơn Đặt Hàng
select ChiTietDonHang.MaDonHang,DonDatHang.MaNV,convert(varchar(10),NgayDat,103) as [NgayDat],convert(varchar(5),GioDat,108) as [GioDat],TenKH,TenQuan,DiaChiGiao,DienThoai_KH,TenDoUong,SlMua,DonGia,SlMua*DonGia as [ThanhTien],DaThanhToan 
from DonDatHang,KhachHang,QuanGiao,ChiNhanh,ChiTietDonHang,ChiTietDoUong
where DonDatHang.MaKH= KhachHang.MaKH 
and QuanGiao.MaQuan = DonDatHang.MaQuan 
and ChiNhanh.MaChiNhanh = QuanGiao.MaChiNhanh 
and DonDatHang.MaDonHang = ChiTietDonHang.MaDonHang
and ChiTietDoUong.MaDoUong = ChiTietDonHang.MaDoUong
--and NhanVienGiaoHang.MaChiNhanh='2'
and DonDatHang.MaDonHang = 2
group by ChiTietDonHang.MaDonHang,DonDatHang.MaNV,NgayDat,GioDat,TenKH,TenQuan,DiaChiGiao,DienThoai_KH,TenChiNhanh,TenDoUong,SlMua,DonGia,DaThanhToan 

insert into NhanVienNhanDonHang values (3,1)
select NhanVienNhanDonHang.MaDonHang,NhanVienNhanDonHang.MaNV,convert(varchar(10),NgayDat,103) as [NgayDat],convert(varchar(5),GioDat,108) as [GioDat],TenKH,TenQuan,TenChiNhanh,DiaChiGiao,DienThoai_KH,DaThanhToan 
from DonDatHang,KhachHang,QuanGiao,ChiNhanh,NhanVienGiaoHang,NhanVienNhanDonHang
where DonDatHang.MaKH= KhachHang.MaKH 
and QuanGiao.MaQuan = DonDatHang.MaQuan 
and ChiNhanh.MaChiNhanh = QuanGiao.MaChiNhanh 
and ChiNhanh.MaChiNhanh= NhanVienGiaoHang.MaChiNhanh 
and DonDatHang.MaDonHang = NhanVienNhanDonHang.MaDonHang
and NhanVienNhanDonHang.MaNV='1'

select MaNV,TenNhanVien,NhanVienGiaoHang.MaChiNhanh,TenChiNhanh,SDT from NhanVienGiaoHang,ChiNhanh where ChiNhanh.MaChiNhanh=NhanVienGiaoHang.MaChiNhanh

select * from ChiNhanh

select MaDatBan,TenChiNhanh,NgayGui,NgayDatBan,GioDatBan,SoLuongNguoi,TenKhachDatBan,DienThoaiDatBan from DatBan,ChiNhanh where DatBan.MaChiNhanh=ChiNhanh.MaChiNhanh and DatBan.MaChiNhanh='2'

select * from QuanLy
select * from DonDatHang where DaThanhToan = 1
select * from ChiTietDonHang 
select * from DatBan where DaXong = 1
select * from QuanLy



set dateformat dmy
select MaDonHang,DonDatHang.MaQuan,ChiNhanh.MaChiNhanh,convert(varchar(10),NgayDat,103) as [Ngày đặt] 
from DonDatHang,ChiNhanh,QuanGiao where DonDatHang.MaQuan = QuanGiao.MaQuan and QuanGiao.MaChiNhanh = ChiNhanh.MaChiNhanh and QuanGiao.MaChiNhanh='1'
and   NgayDat >= '20/09/2018' and  NgayDat <='28/10/2018 23:59:59.999'  


--

select MONTH(NgayDat),COUNT(MaDonHang)
from DonDatHang, ChiNhanh,QuanGiao
where DonDatHang.MaQuan = QuanGiao.MaQuan and QuanGiao.MaChiNhanh = ChiNhanh.MaChiNhanh and Year(NgayDat) ='2014'
and QuanGiao.MaChiNhanh='2' and DaThanhToan = 1
group by MONTH(NgayDat)


select MONTH(NgayDatBan) as [Tháng],COUNT(MaDatBan) as [Số đơn đặt Bàn]
from DatBan, ChiNhanh
where DatBan.MaChiNhanh = ChiNhanh.MaChiNhanh and DatBan.MaChiNhanh='2' and DaXong= 1  
group by MONTH(NgayDatBan)

select * from ChiTietDoUong
select * from KhachHang
select * from LoaiDoUong
select * from ChiNhanh
DBCC checkident ('DatBan', reseed, 0)