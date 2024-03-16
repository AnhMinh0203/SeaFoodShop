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
	IdDescription int foreign key references Descriptions(Id),
	Quantity int,
	Instruct nvarchar (255) null,
	ExpirationDate nvarchar (255) null,
	Origin nvarchar (30) null,
	PrimaryImage varchar(max) null,
	CreateDate date,
	CreateBy uniqueidentifier,
	ModifyDate date,
	ModifyBy uniqueidentifier
)
go

create table Descriptions (
	Id int identity(1,1) primary key,
	Description nvarchar(max)
)
go

create table SeaFoods (
	Id int identity(1,1) primary key,
	IdSeaFoodDetail int foreign key references SeaFoodDetail(Id),
	Name nvarchar (50),
	Price decimal (10,2),
	Unit nvarchar(10),
	IdType int foreign key references Types(Id),
	IdVoucher int foreign key references Vouchers(Id)
)
select * from SeaFoods
create table Images (
	Id int identity(1,1) primary key,
	IdFood int foreign key references SeaFoodDetail(Id),
	IdComment int foreign key references Comments(Id),
	IdBlog int foreign key references Blogs(Id),
	IdDescription int foreign key references Descriptions(Id),
	Image nvarchar(max)
)
go

create table Users (
	Id uniqueidentifier primary key,
	Dob date,
	PhoneNumber varchar (15),
	Password varchar(max),
	FullName nvarchar (30),
	Gender int null,
	Role int ,
	Status int 
)
go
create table FavoriteSeaFoods(
	IdUser uniqueidentifier foreign key references Users(Id),
	IdFood int foreign key references SeaFoods(Id)
)
go


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

create table Orders(
	Id int identity(1,1) primary key,
	PaymentMethod nvarchar(50),
	IdVoucher int foreign key references Vouchers(Id),
	IdFood int foreign key references SeaFoods(Id),
	IdUser uniqueidentifier foreign key references Users(Id)
)
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
go;

create table ShoppingCart(
	IdFood int foreign key references SeaFoods(Id),
	IdUser uniqueidentifier foreign key references Users(Id),
	Quantity int
)
go

create table Address(
	Id uniqueidentifier primary key,
	NameAddress nvarchar(max),
	PhoneNumber varchar(10),
	IsDefault int 
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


INSERT INTO Blogs (IdUser, Title, Content, PublishedDate, Thumbnail, Views, Likes)
VALUES 
    ('ED2A419B-2B52-4039-9A80-BCC9C3BC6AAB', N'TẠI SAO ỐC BULOT IRELAND ĐƯỢC ƯA CHUỘNG TRÊN BÀN TIỆC ?', N'1. Nguồn gốc xuất xứ 

Ốc bulot sinh sống ở các vùng biển trong lành tại khu vực Bắc Âu như Ireland, Pháp, Ireland, Bồ Đào Nha, Canada... Tuy nhiên, ốc bulot thường tập trung số lượng lớn và sinh trưởng tốt tại Ireland. 

Mặc dù cùng là loài ốc bulot nhưng tại các vùng biển khác nhau, chúng sẽ có một số điểm khác biệt về vẻ ngoài cũng như hương vị đặc trưng.

Ốc bulot Ireland có thịt dày, không lẫn cát đá, khi ăn cảm nhận được độ mềm mịn cùng hương vị tươi ngon. Chúng có vỏ màu kem nhạt được tô điểm bởi những vệt màu xanh lục, kích thước trung bình từ 6 - 10cm. Những con ốc bulot sinh sống ngoài tự nhiên có thể có tuổi thọ lên đến 10 năm.', '2024-03-15 10:00:00', 'path_to_thumbnail1.jpg', 100, 20);



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