create database SeaFoodShop
go
use SeaFoodShop
go
create table Types (
	Id int identity(1,1) primary key,
	NameType nvarchar (20)
)
go

create table SeaFoodDetail(
	Id int identity(1,1) primary key,
	Quantity int,
	Instruct nvarchar (255) null,
	ExpirationDate nvarchar (255) null,
	Origin nvarchar (30) null,
	Description nvarchar(max),
	CreateDate date,
	CreateBy uniqueidentifier,
	ModifyDate date,
	ModifyBy uniqueidentifier
)
go

select * from SeaFoods

create table Vouchers(
	Id int identity(1,1) primary key,
	NameVoucher nvarchar(50),
	StartDate date,
	EndDate date,
	CreateDate date,
	CreateBy uniqueidentifier,
	ModifyDate date,
	ModifyBy uniqueidentifier
)
go


create table SeaFoods (
	Id int identity(1,1) primary key,
	IdSeaFoodDetail int foreign key references SeaFoodDetail(Id),
	NameProduct  nvarchar (50),
	Price decimal (10,2),
	Unit nvarchar(10),
	IdType int foreign key references Types(Id),
	IdVoucher int foreign key references Vouchers(Id)
)
go


create table Address(
	Id uniqueidentifier primary key,
	NameAddress nvarchar(max),
	PhoneNumber varchar(10),
	IsDefault int 
)
go

create table Users (
	Id uniqueidentifier primary key,
	Dob date,
	PhoneNumber varchar (15),
	Password varchar(max),
	FullName nvarchar (30),
	Gender int null,
	IdAddress uniqueidentifier foreign key references Address(Id),
	Avatar varchar(max) ,
	Status int 
)
go

create table MapUserAndAddress(
	IdUser uniqueidentifier foreign key references Users(Id),
	IdAddress uniqueidentifier foreign key references Address(Id),
	Action int
)
go

CREATE TABLE Blogs (
    Id INT PRIMARY KEY IDENTITY(1,1),
	IdUser uniqueidentifier foreign key references Users(Id),
    Title NVARCHAR(255) NOT NULL,
    Content NVARCHAR(MAX) NOT NULL,
    PublishedDate DATETIME NOT NULL,
    Thumbnail NVARCHAR(MAX),
    Views INT DEFAULT 0,
    Likes INT DEFAULT 0
);

go

create table Comments (
	Id int identity(1,1) primary key,
	Comment nvarchar(max),
	LikeCount int,
	Dislike int,
	Status varchar(10),
	Stars int,
	IdFood int foreign key references SeaFoods(Id),
	IdUser uniqueidentifier foreign key references Users(Id),
	IdBlog int foreign key references Blogs(Id)
)
go

create table Images (
	Id int identity(1,1) primary key,
	IdSeaFood int foreign key references SeaFoods(Id),
	IdSeaFoodDetail int foreign key references SeaFoodDetail(Id),
	IdComment int foreign key references Comments(Id),
	IdBlog int foreign key references Blogs(Id),
	Status bit,
	Image nvarchar(max)
)
go



create table FavoriteSeaFoods(
	IdUser uniqueidentifier foreign key references Users(Id),
	IdFood int foreign key references SeaFoods(Id)
)
go

create table Orders(
	Id int identity(1,1) primary key,
	PaymentMethod nvarchar(50),
	IdVoucher int foreign key references Vouchers(Id),
	IdFood int foreign key references SeaFoods(Id),
	IdUser uniqueidentifier foreign key references Users(Id)
)
go


create table ShoppingCart(
	IdFood int foreign key references SeaFoods(Id),
	IdUser uniqueidentifier foreign key references Users(Id),
	Quantity int
)
go

delete from Types
DBCC CHECKIDENT ('Types', RESEED, 0);
-- Insert Type
INSERT INTO Types (NameType) VALUES (N'Chả');
INSERT INTO Types (NameType) VALUES (N'Mực');
INSERT INTO Types (NameType) VALUES (N'Tôm');
INSERT INTO Types (NameType) VALUES (N'Cá');

select * from Types


delete from SeaFoodDetail
DBCC CHECKIDENT ('SeaFoodDetail', RESEED, 0);
-- Insert seafood detail chả mực
DECLARE @CreateDate date = GETDATE();
DECLARE @ModifyDate date = GETDATE();
DECLARE @CreateBy uniqueidentifier = '64A0B3AB-16A3-4B78-8FC8-A6ED74EC78F2';
DECLARE @ModifyBy uniqueidentifier = '64A0B3AB-16A3-4B78-8FC8-A6ED74EC78F2';

INSERT INTO SeaFoodDetail (Quantity, Instruct, ExpirationDate, Origin, Description, CreateDate, CreateBy, ModifyDate, ModifyBy)
VALUES (10, N'Cho nhiều dầu ăn, rán nhỏ lửa', N'12 tháng', N'Quảng Ninh', N'Chả mực là một trong những đặc sản...', @CreateDate, @CreateBy, @ModifyDate, @ModifyBy);

-- Insert seafood detail chả tôm 
DECLARE @CreateDate date = GETDATE();
DECLARE @ModifyDate date = GETDATE();
DECLARE @CreateBy uniqueidentifier = '64A0B3AB-16A3-4B78-8FC8-A6ED74EC78F2';
DECLARE @ModifyBy uniqueidentifier = '64A0B3AB-16A3-4B78-8FC8-A6ED74EC78F2';

INSERT INTO SeaFoodDetail (Quantity, Instruct, ExpirationDate, Origin, Description, CreateDate, CreateBy, ModifyDate, ModifyBy)
VALUES (28, N'Dầu ăn cho vừa, rán nhỏ lửa', N'12 tháng', N'Quảng Ninh', N'Chả tôm là một trong những đặc sản...', @CreateDate, @CreateBy, @ModifyDate, @ModifyBy);

-- Insert seafood detail chả cá 
DECLARE @CreateDate date = GETDATE();
DECLARE @ModifyDate date = GETDATE();
DECLARE @CreateBy uniqueidentifier = '64A0B3AB-16A3-4B78-8FC8-A6ED74EC78F2';
DECLARE @ModifyBy uniqueidentifier = '64A0B3AB-16A3-4B78-8FC8-A6ED74EC78F2';

INSERT INTO SeaFoodDetail (Quantity, Instruct, ExpirationDate, Origin, Description, CreateDate, CreateBy, ModifyDate, ModifyBy)
VALUES (50, N'Dầu ăn cho vừa, rán nhỏ lửa', N'12 tháng', N'Quảng Ninh', N'Chả cá là một trong những đặc sản...', @CreateDate, @CreateBy, @ModifyDate, @ModifyBy);

-- Insert seafood detail chả mực loại 1
DECLARE @CreateDate date = GETDATE();
DECLARE @ModifyDate date = GETDATE();
DECLARE @CreateBy uniqueidentifier = '64A0B3AB-16A3-4B78-8FC8-A6ED74EC78F2';
DECLARE @ModifyBy uniqueidentifier = '64A0B3AB-16A3-4B78-8FC8-A6ED74EC78F2';

INSERT INTO SeaFoodDetail (Quantity, Instruct, ExpirationDate, Origin, Description, CreateDate, CreateBy, ModifyDate, ModifyBy)
VALUES (18, N'Cho nhiều dầu ăn, rán nhỏ lửa', N'12 tháng', N'Quảng Ninh', N'Chả mực là một trong những đặc sản...', @CreateDate, @CreateBy, @ModifyDate, @ModifyBy);

-- Insert seafood detail chả mực loại 2
DECLARE @CreateDate date = GETDATE();
DECLARE @ModifyDate date = GETDATE();
DECLARE @CreateBy uniqueidentifier = '64A0B3AB-16A3-4B78-8FC8-A6ED74EC78F2';
DECLARE @ModifyBy uniqueidentifier = '64A0B3AB-16A3-4B78-8FC8-A6ED74EC78F2';

INSERT INTO SeaFoodDetail (Quantity, Instruct, ExpirationDate, Origin, Description, CreateDate, CreateBy, ModifyDate, ModifyBy)
VALUES (18, N'Cho nhiều dầu ăn, rán nhỏ lửa', N'12 tháng', N'Quảng Ninh', N'Chả mực là một trong những đặc sản...', @CreateDate, @CreateBy, @ModifyDate, @ModifyBy);
-- Insert seafood detail chả mực loại 3
DECLARE @CreateDate date = GETDATE();
DECLARE @ModifyDate date = GETDATE();
DECLARE @CreateBy uniqueidentifier = '64A0B3AB-16A3-4B78-8FC8-A6ED74EC78F2';
DECLARE @ModifyBy uniqueidentifier = '64A0B3AB-16A3-4B78-8FC8-A6ED74EC78F2';

INSERT INTO SeaFoodDetail (Quantity, Instruct, ExpirationDate, Origin, Description, CreateDate, CreateBy, ModifyDate, ModifyBy)
VALUES (18, N'Cho nhiều dầu ăn, rán nhỏ lửa', N'12 tháng', N'Quảng Ninh', N'Chả mực là một trong những đặc sản...', @CreateDate, @CreateBy, @ModifyDate, @ModifyBy);

-- Insert seafood detail Cá song
DECLARE @CreateDate date = GETDATE();
DECLARE @ModifyDate date = GETDATE();
DECLARE @CreateBy uniqueidentifier = '64A0B3AB-16A3-4B78-8FC8-A6ED74EC78F2';
DECLARE @ModifyBy uniqueidentifier = '64A0B3AB-16A3-4B78-8FC8-A6ED74EC78F2';

INSERT INTO SeaFoodDetail (Quantity, Instruct, ExpirationDate, Origin, Description, CreateDate, CreateBy, ModifyDate, ModifyBy)
VALUES (5, N'Có thể nấu diêu, rán, om, ...', N'12 tháng', N'Quảng Ninh', N'Cá song là 1 trong những loại cá ....', @CreateDate, @CreateBy, @ModifyDate, @ModifyBy);

select * from SeaFoodDetail

delete from Seafoods
DBCC CHECKIDENT ('SeaFoods', RESEED, 0);
-- Insert Seafood
INSERT INTO SeaFoods (IdSeaFoodDetail, Name, Price, Unit, IdType, IdVoucher)
VALUES (1, N'Chả mực', 225000, N'1 hộp/500g', 1, NULL);

INSERT INTO SeaFoods (IdSeaFoodDetail, Name, Price, Unit, IdType, IdVoucher)
VALUES (2, N'Chả tôm', 70000, N'1 hộp/500g', 1, NULL);

INSERT INTO SeaFoods (IdSeaFoodDetail, Name, Price, Unit, IdType, IdVoucher)
VALUES (3, N'Chả cá', 80000, N'1 hộp/500g', 1, NULL);

INSERT INTO SeaFoods (IdSeaFoodDetail, Name, Price, Unit, IdType, IdVoucher)
VALUES (4, N'Chả mực loại 1', 250000, N'1 hộp/500g', 1, NULL);

INSERT INTO SeaFoods (IdSeaFoodDetail, Name, Price, Unit, IdType, IdVoucher)
VALUES (5, N'Chả mực loại 2', 280000, N'1 hộp/500g', 1, NULL);

INSERT INTO SeaFoods (IdSeaFoodDetail, Name, Price, Unit, IdType, IdVoucher)
VALUES (5, N'Chả mực loại 3', 300000, N'1 hộp/500g', 1, NULL);

INSERT INTO SeaFoods (IdSeaFoodDetail, Name, Price, Unit, IdType, IdVoucher)
VALUES (7, N'Chả mực loại 3', 300000, N'1 hộp/500g', 4, NULL);
select * from SeaFoods





INSERT INTO Blogs (IdUser, Title, Content, PublishedDate, Thumbnail, Views, Likes)
VALUES 
    ('ED2A419B-2B52-4039-9A80-BCC9C3BC6AAB', N'TẠI SAO ỐC BULOT IRELAND ĐƯỢC ƯA CHUỘNG TRÊN BÀN TIỆC ?', N'1. Nguồn gốc xuất xứ 

Ốc bulot sinh sống ở các vùng biển trong lành tại khu vực Bắc Âu như Ireland, Pháp, Ireland, Bồ Đào Nha, Canada... Tuy nhiên, ốc bulot thường tập trung số lượng lớn và sinh trưởng tốt tại Ireland. 

Mặc dù cùng là loài ốc bulot nhưng tại các vùng biển khác nhau, chúng sẽ có một số điểm khác biệt về vẻ ngoài cũng như hương vị đặc trưng.

Ốc bulot Ireland có thịt dày, không lẫn cát đá, khi ăn cảm nhận được độ mềm mịn cùng hương vị tươi ngon. Chúng có vỏ màu kem nhạt được tô điểm bởi những vệt màu xanh lục, kích thước trung bình từ 6 - 10cm. Những con ốc bulot sinh sống ngoài tự nhiên có thể có tuổi thọ lên đến 10 năm.', '2024-03-15 10:00:00', 'path_to_thumbnail1.jpg', 100, 20);

select * from images

select * from blogs
delete from blogs

delete from Types
delete from Images
delete from seafooddetail
delete from seafoods
delete from ShoppingCart
delete  from Comments
delete from Descriptions
-- Types
insert into Types(NameType) values (N'Chả'),(N'Cá'),(N'Tôm'),(N'Mực')

-- Description
Insert into descriptions (Description) values (N'Chả mực Hạ Long là một đặc sản nổi tiếng của vùng biển Quảng Ninh, Việt Nam, nơi nổi tiếng với vị ngọt của mực và hương vị độc đáo của gia vị. Chả mực được làm từ những con mực tươi ngon, được tinh chế và chế biến theo quy trình truyền thống được lưu truyền từ đời này sang đời khác.')

-- SeaFoodsDetail
insert into SeaFoodDetail (Instruct, ExpirationDate, Origin, IdDescription)
values (N'- Khi rán nên để nhỏ lửa, tránh để lửa to', 'Ngày 13/09/2024', N'Quảng Ninh',1)

-- SeaFood 
insert into SeaFoods (IdSeaFoodDetail,Name,Price,Unit,IdType,Status)
values (1,N'Chả mực Hạ Long',500000,'Kg',1,0)


select * from users
select * from images
select * from comments
select * from ShoppingCart
select * from seafoods
select * from seafooddetail
select * from Comments
select * from ShoppingCart
select * from Types
select * from Descriptions

DBCC CHECKIDENT ('seafooddetail', RESEED, 0);