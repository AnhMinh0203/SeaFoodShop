create database SeaFoodShop
go
use SeaFoodShop
go
create table Types (
	Id int identity(1,1) primary key,
	NameType nvarchar (20),
	CreateDate date,
	CreateBy uniqueidentifier,
	ModifyDate date,
	ModifyBy uniqueidentifier
)
go
create table SeaFoods (
	Id int identity(1,1) primary key,
	Name nvarchar (50),
	Description nvarchar(max),
	Price decimal (10,2),
	Unit nvarchar(10),
	Quantity int,
	IdType int foreign key references Types(Id),
	CreateDate date,
	CreateBy uniqueidentifier,
	ModifyDate date,
	ModifyBy uniqueidentifier
)
go

create table Images (
	IdFood int foreign key references SeaFoods(Id),
	Image nvarchar(max)
)
go

create table Users (
	Id uniqueidentifier primary key,
	Dob date,
	PhoneNumber varchar (15),
	Password varchar(max),
	FullName nvarchar (30),
	Role int ,
	Status int ,
)
go;

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
	Image nvarchar(max),
	LikeCount int,
	Dislike int,
	Status int,
	IdFood int foreign key references SeaFoods(Id),
	IdUser uniqueidentifier foreign key references Users(Id),
)
go

create table ShoppingCart(
	IdFood int foreign key references SeaFoods(Id),
	IdUser uniqueidentifier foreign key references Users(Id),
	Quantity int
)
go

create table Address(
	Id uniqueidentifier primary key,
	NameAddress nvarchar(max)
)
go

create table MapUserAndAddress(
	IdUser uniqueidentifier foreign key references Users(Id),
	IdAddress uniqueidentifier foreign key references Address(Id)
)
go