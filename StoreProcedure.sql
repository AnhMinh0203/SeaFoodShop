-- Sign In
alter PROCEDURE SignIn
    @phoneNumber VARCHAR(15)
AS
BEGIN
    DECLARE @sql NVARCHAR(MAX);
    SET @sql = N'SELECT Id, Password, Role, Status from users where PhoneNumber = @phoneNumber';
    EXEC sp_executesql @sql, N'@phoneNumber VARCHAR(15)', @phoneNumber;
END;

-- Sign Up
alter procedure SignUp 
	@dob date,
	@phoneNumber varchar(15),
	@password varchar(max),
	@fullName nvarchar(30),
	@result nvarchar(100) output
as
begin
if exists (select 1 from users where PhoneNumber = @phoneNumber)
	begin
		set @result = 'Phone number already exists';
	end
else
	begin
		declare @sql nvarchar(max)
		set @sql = N'insert into users(Id,Dob,PhoneNumber,Password,FullName,status) values (NewID(),@dob,@phoneNumber,@password,@fullName,0)';
		exec sp_executesql @sql, N'@dob date,@phoneNumber varchar(15),@password varchar(max),@fullName nvarchar(30)',@dob,@phoneNumber,@password,@fullName;
		set @result = 'Sign up successfully';
	end;
end;

-- Display Seafoods
ALTER PROCEDURE GetSeaFoods
    @PageNumber INT,
    @PageSize INT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

    SELECT Id, Name, Price, Unit, Status
    FROM SeaFoods
    ORDER BY Id
    OFFSET @Offset ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END;


-- Dispay seafood detail

alter procedure GetSeaFoodDetail
@Id int
as
begin
	SELECT 
	s.Id as Id,
	s.Name AS Name,
    s.Price AS Price,
    s.Unit AS Unit,
	s.Status as Status,
    t.NameType AS TypeName,
    d.Description as Description, 
    sf.Instruct as Instruct,
	sf.ExpirationDate as ExpirationDate,
	sf.Origin as Origin,
    (SELECT Image FROM Images i WHERE i.IdDescription = d.Id FOR JSON PATH) AS DescriptionImages,
    (SELECT Image FROM Images i WHERE i.IdSeaFoodDetail = sf.Id for json path) AS SeaFoodImages
	FROM 
		descriptions d
	INNER JOIN 
		SeaFoodDetail sf ON d.Id = sf.IdDescription
	INNER JOIN 
        SeaFoods s ON sf.Id = s.IdSeaFoodDetail
    INNER JOIN 
        Types t ON s.IdType = t.Id
	where d.Id = @Id
end;


-- Search seafood by name
create procedure SearchSeaFood
@NameSeaFood nvarchar(50)
as
begin
	select * from SeaFoods where Name like '%' + @NameSeaFood + '%'
end;

-- Search by type
create procedure SearchSeaFoodByType
@NameType nvarchar(50)
as
begin 
	select SeaFoods.*, Types.NameType from SeaFoods Join Types on SeaFoods.IdType = Types.Id where Types.NameType = @NameType
end;

-- Push comment
CREATE TYPE ImageTableType AS TABLE (
    Image nvarchar(max)
);

alter procedure PostComment
	@CommentText nvarchar(max),
	@Images ImageTableType readonly,
	@Like int ,
	@Dislike int,
	@IdFood int,
	@IdUser uniqueidentifier,
	@Stars int
as
begin 
	INSERT INTO Comments (Comment, LikeCount, Dislike, Status, IdFood, IdUser, Stars)
    VALUES (@CommentText, @Like, @Dislike, 'Post', @IdFood, @IdUser, @Stars);

    -- Get the Id of the newly inserted comment
    DECLARE @CommentId int;
    SET @CommentId = SCOPE_IDENTITY();

    -- Insert the image into the Images table with the corresponding comment Id
    INSERT INTO Images (IdComment, Image)
	SELECT @CommentId, Image
	FROM @Images;
end;

-- Update comment
alter PROCEDURE UpdateComment
    @CommentId int,
    @CommentText nvarchar(max),
    @Images ImageTableType readonly,
    @Stars int
AS
BEGIN
    UPDATE Comments
    SET Comment = @CommentText,
        Stars = @Stars,
		Status = 'Update'
    WHERE Id = @CommentId;

    -- Update all images for the comment
	Delete from Images where IdComment = @CommentId
	INSERT INTO Images (IdComment, Image)
	SELECT @CommentId, Image
	FROM @Images;
END;


-- Delete comment
alter procedure DeleteComment
	@Id int
as
begin
	delete from images where IdComment = @Id
	delete from Comments where Id = @Id
end;

-- Get comments
create procedure GetComments
	@PageNumber int,
	@PageSize int
as
begin
	set nocount on;
	declare @Offset int = (@PageNumber - 1) * @PageSize;

	SELECT
        C.Id AS CommentId,
        C.Comment,
        C.LikeCount,
        C.Dislike,
        C.Status,
        C.Stars,
        I.Image
    FROM
        Comments C
    INNER JOIN
        Images I ON C.Id = I.IdComment
    WHERE
        C.Id = @CommentId;
	order by Id
	OFFSET @Offset rows
	FETCH next @PageSize rows only;
end;

-- Search comments

-- C1
--alter procedure SearchComments
--	@CommentText nvarchar(max)
--as
--begin
--	SELECT Comment, Images.Image 
--	FROM Comments 
--	INNER JOIN Images ON Comments.Id = Images.IdComment
--	WHERE Comment LIKE '%' + @CommentText + '%';
--end;

--C2
alter procedure SearchComments
	@CommentText nvarchar(max) 
as
begin
	select c.*, (select i.image as nameImage from images i where i.IdComment = c.Id for Json Path) as Pictures
	from Comments c 
	where c.Comment like '%'+ @CommentText +'%'
end

-- Update seafood quantity in shopping cart
alter procedure UpdateShoppingCart
	@IdSeafood int,
	@IdUser uniqueidentifier,
	@Quantity int
as
begin
	update ShoppingCart set Quantity = @Quantity where IdFood = @IdSeafood and IdUser = @IdUser;
end;

-- Add seafood to shopping cart
alter PROCEDURE AddToShoppingCart
    @IdSeafood INT,
    @IdUser UNIQUEIDENTIFIER,
    @Quantity INT
AS
BEGIN
    MERGE INTO ShoppingCart AS Target
    USING (VALUES (@IdSeafood, @IdUser, @Quantity)) AS Source (IdFood, IdUser, Quantity)
    ON Target.IdFood = Source.IdFood AND Target.IdUser = Source.IdUser
    WHEN MATCHED THEN
        UPDATE SET Quantity = Target.Quantity + Source.Quantity
    WHEN NOT MATCHED THEN
        INSERT (IdFood, IdUser, Quantity) VALUES (Source.IdFood, Source.IdUser, Source.Quantity);
END;

-- Delete seafood in shopping cart
create procedure DeleteShoppingCart 
	@IdSeaFood int,
	@IdUser uniqueidentifier
as
begin 
	delete from ShoppingCart where IdFood = @IdSeaFood and IdUser = @IdUser;
end;

-- Search seafood in shopping cart
alter procedure SearchShoppingCart
	@IdUser uniqueidentifier,
	@NameSeaFood nvarchar(50)
as
begin
	if @NameSeaFood is not null
		select sf.*  from SeaFoods sf inner join ShoppingCart sc on sf.Id = sc.IdFood where sc.IdUser = @IdUser and sf.Name like '%' + @NameSeaFood+ '%'
	else
		select sf.*  from SeaFoods sf inner join ShoppingCart sc on sf.Id = sc.IdFood where sc.IdUser = @IdUser
end;

-- Add type
alter procedure AddSeaFoodType
	@nameType nvarchar(20),
	@result nvarchar(100) output
as
begin
	if exists (select 1 from Types where NameType like '%' + @nameType + '%')
	begin 
		set @result =  N'Tên loại đã tồn tại'
	end
	else
	begin
		insert into Types(NameType) values (@nameType);
		set @result =  N'Thêm loại mới thành công'
	end
end;

-- Delete type
alter procedure DeleteSeaFoodType
	@nameType nvarchar(20),
	@result nvarchar(100) output
as
begin
	if not exists (select 1 from Types where NameType like '%' + @nameType + '%')
	begin 
		set @result =  N'Tên loại không tồn tại'
	end
	else
	begin
		delete from Types where NameType like '%' + @nameType + '%';
		set @result =  N'Xóa loại thành công'
	end
end;

-- Get all types
alter procedure GetSeaFoodType
as
begin
	select * from types 
end;

-- Update profile
alter procedure UpdateProfile
	@idUser uniqueidentifier,
	@fullName nvarchar(30),
	@dob date,
	@gender int,
	@phoneNumber nvarchar(15),
	@result nvarchar(100) output
as
begin
	update Users set FullName = @fullName,Dob = @dob, Gender = @gender, PhoneNumber = @phoneNumber where Id = @idUser
	set @result = N'Cập nhật thông tin thành công'
end;


-- Get user profile
alter procedure GetProfile
	@idUser uniqueidentifier
as
begin
	select Id,FullName, Dob, PhoneNumber, Gender from users where Id = @idUser
end

-- Change password
create procedure GetPassword
	@idUser uniqueidentifier
as
begin
	select password from Users where Id = @idUser
end

alter procedure ChangePassword
	@idUser uniqueidentifier,
	@newPassword varchar(max)
as
begin
	update Users set Password = @newPassword where Id = @idUser
end;

create procedure ChangePasswordAdmin
	@phoneNumber varchar(10),
	@newPassword varchar(max)
as
begin
	update Users set Password = @newPassword where PhoneNumber = @phoneNumber
end;


-- Get favorite seafoods
alter procedure GetFavoriteSeafoods
	@idUser uniqueidentifier
as
begin
	select sf.Id, sf.Name, sf.Price, sf.Unit,sf.Status, sf.IdVoucher from SeaFoods sf 
	inner join FavoriteSeaFoods fs on sf.Id = fs.IdFood 
	where fs.IdUser = @idUser
end

-- Add favorite seafoods
alter PROCEDURE AddFavoriteSeafood
    @IdSeafood INT,
    @IdUser UNIQUEIDENTIFIER
AS
BEGIN
    INSERT into FavoriteSeafoods(IdUser, IdFood) VALUES (@IdUser,@IdSeafood);
END;

-- Delete favorite seafoods
create procedure DeleteFavoriteSeafood
	@IdSeaFood int,
	@IdUser uniqueidentifier
as
begin 
	delete from FavoriteSeafoods where IdFood = @IdSeaFood and IdUser = @IdUser;
end;

--Add Address
alter procedure AddAddress
	@idUser uniqueidentifier,
	@idAddress uniqueidentifier,
	@nameAddress nvarchar(max),
	@phoneNumber varchar(10),
	@isDefault int,
	@result nvarchar(100) output
as 
begin
	INSERT INTO Address (Id, NameAddress, PhoneNumber, IsDefault) VALUES (@idAddress, @nameAddress, @phoneNumber, @isDefault);
	INSERT INTO MapUserAndAddress (IdAddress, IdUser,Action) VALUES (@idAddress, @idUser,1);
	set @result = N'Thêm mới địa chỉ thành công'
end

-- Get Address
alter procedure GetAddresses
	@idUser uniqueidentifier
as
begin
	select ad.* from Address ad
	inner join MapUserAndAddress mu on mu.IdAddress = ad.Id
	where mu.IdUser = @idUser and mu.Action = 1
end;

-- Delete Address
create procedure DeteleAddress
	@idUser uniqueidentifier,
	@idAddress uniqueidentifier
as
begin
	update MapUserAndAddress set Action = 0 where IdUser = @idUser and IdAddress = @idAddress
end

-- Read blog
alter PROCEDURE getBlogs
    @PageNumber INT,
    @PageSize INT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

    SELECT * from Blogs
    ORDER BY Id
    OFFSET @Offset ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END;

alter procedure getBlogDetail
	@idBlog int
as
begin
	select b.* , 
	(select Image from Images i where b.id = i.idBlog for json path ) as BlogImages 
	from Blogs b where b.id = @idBlog
end

-- Lock account
alter procedure lockAccount
	@phoneNumber varchar(10),
	@result nvarchar(100) output
as
begin
	if not exists (select 1 from Users where PhoneNumber = @phoneNumber)
		begin 
			set @result =  N'Tài khoản không tồn tại'
		end
	else
		begin
			update Users set Status = 1 where PhoneNumber = @phoneNumber
			set @result =  N'Khóa tài khoản thành công'
		end
end

-- Unlock account
alter procedure unLockAccount
	@phoneNumber varchar(10),
	@result nvarchar(100) output
as
begin
if not exists (select 1 from Users where PhoneNumber = @phoneNumber)
	begin 
		set @result =  N'Tài khoản không tồn tại'
	end
else
	begin
		update Users set Status = 0 where PhoneNumber = @phoneNumber
		set @result =  N'Mở khóa tài khoản thành công'
	end
end

-- Search account
alter procedure searchAccount 
	@phoneNumber varchar(10) null,
	@status int null
as
begin
	IF @phoneNumber is not null and @status is null
		select Id,PhoneNumber, FullName,Dob, Gender, Status from users where PhoneNumber like '%' + @phoneNumber + '%'  
	else if @status = 1
		if @phoneNumber is not null
			select Id,PhoneNumber, FullName,Dob, Gender, Status from users where PhoneNumber like '%' + @phoneNumber + '%' and Status = 1
		else
			select Id,PhoneNumber, FullName,Dob, Gender, Status from users where Status = 1
	else
		select Id,PhoneNumber, FullName,Dob, Gender, Status from users
end

-- Delete customer
alter procedure deleteCustomer
	@phoneNumber varchar(10),
	@result nvarchar(100) output
as
begin
	update users set Status = -1 where PhoneNumber = @phoneNumber
	set @result = N'Xóa người dùng thành công'
end

-- Search customer

create procedure searchCustomer
	@phoneNumber varchar(10),
	@status int 
as
begin
	IF @phoneNumber is not null and @status = 0
		select u.Id,u.PhoneNumber, u.FullName,Dob, u.Gender, a.NameAddress, Status from users u
		inner join MapUserAndAddress mua on u.IdAddress = mua.IdAddress
		inner join Address a on mua.IdAddress = a.Id
		where u.PhoneNumber like '%' + @phoneNumber + '%'  
	else if @status = 1
		if @phoneNumber is not null
			select Id,PhoneNumber, FullName,Dob, Gender, Status from users where PhoneNumber like '%' + @phoneNumber + '%' and Status = 1
		else
			select Id,PhoneNumber, FullName,Dob, Gender, Status from users where Status = 1
	else
		select u.Id,u.PhoneNumber, u.FullName,u.Dob, u.Gender, a.NameAddress, Status from users u
		inner join MapUserAndAddress mua on mua.IdAddress = mua.IdAddress
		inner join Address a on mua.IdAddress = a.Id
end

-- Add seafood
alter PROCEDURE addSeaFood
    @name nvarchar(50),--
    @price decimal(10,2),--
    @unit nvarchar(10),--
    @nameType nvarchar(20),--
    @status int,--
    @idVoucher int,--
    @instruct nvarchar(255),--
    @expirationDate nvarchar(255),--
    @origin nvarchar(30),--
    @primaryImage nvarchar(max), -- 
    @jsonImagesSeaFood NVARCHAR(MAX),
	@jsonImagesDescription NVARCHAR(MAX),--
    @description nvarchar(max), --
	@result nvarchar(100) output
AS
BEGIN
	-- Declare two json table
    DECLARE @jsonSeaFoodTable TABLE (
        [Image] VARCHAR(max)
    )
	DECLARE @jsonDescription TABLE (
        [Image] VARCHAR(max)
    )
	
	DECLARE @idSeaFoodDetail INT;
	DECLARE @idDescription INT;

    INSERT INTO Descriptions (Description) VALUES (@description);
	SELECT @idDescription = MAX(Id) FROM Descriptions

	INSERT INTO @jsonDescription ([Image])
    SELECT [Image]
    FROM OPENJSON(@jsonImagesDescription) WITH (
        [Image] NVARCHAR(100) '$.Image')

	-- Insert images into Images table for description
	INSERT INTO Images ([IdDescription],[Image])
    SELECT @idDescription, [Image]
    FROM @jsonDescription

    -- Chèn dữ liệu vào bảng SeaFoodDetail
    INSERT INTO SeaFoodDetail (IdDescription, Instruct, ExpirationDate, Origin, PrimaryImage)
    VALUES (@idDescription, @instruct, @expirationDate, @origin, @primaryImage);

	-- Chèn dữ liệu vào bảng @jsonSeaFoodTable
	INSERT INTO @jsonSeaFoodTable ([Image])
    SELECT [Image]
    FROM OPENJSON(@jsonImagesSeaFood) WITH (
        [Image] NVARCHAR(100) '$.Image')

	SELECT @idSeaFoodDetail = MAX(Id) FROM SeafoodDetail

	-- Insert images into Images table for seafood
    INSERT INTO Images ([IdSeaFoodDetail],[Image])
    SELECT @idSeaFoodDetail, [Image]
    FROM @jsonSeaFoodTable
    -- Lấy ID của loại từ bảng Types
    DECLARE @idType int;
    SELECT @idType = Id FROM Types WHERE NameType = @nameType;

    -- Chèn dữ liệu vào bảng SeaFoods
    INSERT INTO SeaFoods (IdSeaFoodDetail, Name, Price, Unit, IdType, Status, IdVoucher)
    VALUES (@idSeaFoodDetail, @name, @price, @unit, @idType, @status, @idVoucher);
	set @result = N'Thêm sản phẩm thành công'
END;

    
-- draft

EXEC sp_rename 'DeleteTypeSeaFood', 'DeleteSeaFoodType'
	SELECT Comment, Images.Image 
	FROM Comments 
	INNER JOIN Images ON Comments.Id = Images.IdComment
	WHERE Comment LIKE '%' + 'C' + '%';


update SeaFoodDetail set IdDescription = 1 where Id = 1
select * from SeaFoodDetail
-- Thực thi thủ tục PushComment
EXEC PushComment N'Nội dung bình luận', @Images, 10, 5, 1, 1, 'ED2A419B-2B52-4039-9A80-BCC9C3BC6AAB', 4;
AddTypeSeaFood 'Type5'

DECLARE @Images ImageTableType;
INSERT INTO @Images (Image) VALUES ('Image1New.jpg');

EXEC UpdateComment
    @CommentId = 10,
    @CommentText = 'ahihidsfdfdfgdfhihi',
    @Images = @Images,
    @Stars = 4;

SearchShoppingCart 'ED2A419B-2B52-4039-9A80-BCC9C3BC6AAB', '1'
;with abc as 
(
	select 
		c.*, 
		(select i.Image from Images i where i.IdComment = c.Id for Json Path ) as Anh
	from Comments c where  c.Comment  like '%C%'
)

select c.* , (select i.Image as nameImage from Images i where i.IdComment = c.Id for Json Path ) as Pictures from Comments c where c.Comment like '%a%'
select * from ShoppingCart

select * from Vouchers

INSERT INTO Vouchers (NameVoucher, StartDate, EndDate, CreateDate)
VALUES 
    ('Voucher A', '2024-04-01', '2024-04-30', '2024-03-19')

EXEC addSeaFood 
    @name = N'Chả cá Cẩm Phả',
    @price = 80000, -- Thay giá trị tùy theo giá
    @unit = N'hộp/500g',
    @nameType = N'Chả',
    @status = 1, -- Thay giá trị tùy theo trạng thái
    @idVoucher = 1, -- Thay giá trị tùy theo idVoucher
    @instruct = N'Rán với lửa vừa và nhỏ , không cần cho quá nhiều dầu ăn',
    @expirationDate = N'12 tháng kể từ ngày sản xuất',
    @origin = N'Quảng Ninh',
    @primaryImage = N'Đường dẫn ảnh chính',
    @jsonImages = N'[{"Image": "link_anh_1.jpg"}, {"Image": "link_anh_2.jpg"}]', -- Chuỗi JSON chứa danh sách hình ảnh
    @description = N'Một trong những đặc sản nổi tiếng ở Qn không thể không nhắc đến chả cá Cẩm Phả';

select * from SeaFoods
select * from SeaFoodDetail
select * from Images
select * from Descriptions
select * from Vouchers
delete Descriptions where id = 16





