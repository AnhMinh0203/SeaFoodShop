-- Sign In
alter PROCEDURE SignIn
    @phoneNumber VARCHAR(15)

AS
BEGIN
    DECLARE @sql NVARCHAR(MAX);
    SET @sql = N'SELECT Id, Password, Status from users where PhoneNumber = @phoneNumber';
    EXEC sp_executesql @sql, N'@phoneNumber VARCHAR(15)', @phoneNumber;
END;
SignIn '0869819316'
select * from users
-- Sign Up
alter procedure SignUp 
	@dob date, 
	@phoneNumber varchar(15), 
	@password varchar(max), 
	@fullName nvarchar(30),
	@gender int,
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
		set @sql = N'insert into users(Id,Dob,PhoneNumber,Password,FullName,Gender,status) values (NewID(),@dob,@phoneNumber,@password,@fullName,@gender,0)';
		exec sp_executesql @sql, N'@dob date,@phoneNumber varchar(15),@password varchar(max),@fullName nvarchar(30),@gender int',@dob,@phoneNumber,@password,@fullName,@gender;
		set @result = 'Sign up successfully';
	end;
end;
select * from users
-- Display Seafoods
create PROCEDURE GetSeaFoods
    @PageNumber INT,
    @PageSize INT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

    SELECT Id, Name, Price, Unit
    FROM SeaFoods
    ORDER BY Id
    OFFSET @Offset ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END;


-- Dispay seafood detail

create procedure GetSeaFoodDetail
@Id int
as
begin
	SELECT 
	s.Id as Id,
	s.Name AS Name,
    s.Price AS Price,
    s.Unit AS Unit,
	--s.Status as Status,
	s.IdVoucher as IdVourcher,
    t.NameType AS TypeName,
    sf.Description as Description, 
    sf.Instruct as Instruct,
	sf.ExpirationDate as ExpirationDate,
	sf.Origin as Origin,
	(select Image from Images where IdSeaFood =@Id and Status = 1 ) as PrimaryImage,
	-- Status seafood = 0 -> Seafood image , status seafood = 1 -> primary image
	(SELECT Image as nameImage FROM Images i WHERE i.IdSeaFood = @Id and i.Status = 0 FOR JSON PATH) AS SeaFoodImagesJson,
	(SELECT Image as nameImage FROM Images i WHERE i.IdSeaFoodDetail = sf.Id for json path) AS DescriptionImagesJson
	FROM 
		SeaFoodDetail sf
	INNER JOIN 
        SeaFoods s ON sf.Id = s.IdSeaFoodDetail
    INNER JOIN 
        Types t ON s.IdType = t.Id
	where s.Id = @Id
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

create procedure PostComment
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
create PROCEDURE UpdateComment
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
create procedure DeleteComment
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
        C.Id = I.IdComment
	order by C.Id
	OFFSET @Offset rows
	FETCH next @PageSize rows only;
end;

-- Search comments

-- C1
--create procedure SearchComments
--	@CommentText nvarchar(max)
--as
--begin
--	SELECT Comment, Images.Image 
--	FROM Comments 
--	INNER JOIN Images ON Comments.Id = Images.IdComment
--	WHERE Comment LIKE '%' + @CommentText + '%';
--end;

--C2
create procedure SearchComments
	@CommentText nvarchar(max) 
as
begin
	select c.*, (select i.image as nameImage from images i where i.IdComment = c.Id for Json Path) as Pictures
	from Comments c 
	where c.Comment like '%'+ @CommentText +'%'
end

-- Update seafood quantity in shopping cart
create procedure UpdateShoppingCart
	@IdSeafood int,
	@IdUser uniqueidentifier,
	@Quantity int
as
begin
	update ShoppingCart set Quantity = @Quantity where IdFood = @IdSeafood and IdUser = @IdUser;
end;

-- Add seafood to shopping cart
create PROCEDURE AddToShoppingCart
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
create procedure SearchShoppingCart
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
create procedure AddSeaFoodType
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
create procedure DeleteSeaFoodType
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
create procedure GetSeaFoodType
as
begin
	select * from types 
end;

-- Update profile
create procedure UpdateProfile
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
GetProfile '8888888888'
alter procedure GetProfile
	@phoneNumber nvarchar(10)
as
begin
	select * from users where PhoneNumber = @phoneNumber
end

-- Change password
create procedure GetPassword
	@idUser uniqueidentifier
as
begin
	select password from Users where Id = @idUser
end

create procedure ChangePassword
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
create procedure GetFavoriteSeafoods
	@idUser uniqueidentifier
as
begin
	select sf.Id, sf.Name, sf.Price, sf.Unit, sf.IdVoucher from SeaFoods sf 
	inner join FavoriteSeaFoods fs on sf.Id = fs.IdFood 
	where fs.IdUser = @idUser
end

-- Add favorite seafoods
create PROCEDURE AddFavoriteSeafood
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
create procedure AddAddress
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
create procedure GetAddresses
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
create PROCEDURE getBlogs
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

create procedure getBlogDetail
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
			update Users set Status = -1 where PhoneNumber = @phoneNumber
			set @result =  N'Khóa tài khoản thành công'
		end
end

-- Unlock account
create procedure unLockAccount
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
create procedure searchAccount 
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
	update users set Status = -2 where PhoneNumber = @phoneNumber
	set @result = N'Xóa người dùng thành công'
end
declare @result nvarchar(100) 
exec deleteCustomer @phoneNumber ='6666666666', @result = @result OUTPUT
SELECT @result AS Result
-- Search customer
create proc getAdminInfor 
as begin
	select * from users where Status = 1
end

alter procedure searchCustomer
	@text varchar(30),
	@PageIndex INT,
	@PageSize INT,
	@TotalRecord INT OUTPUT
as
begin
	DECLARE @CurrentRow AS INT = (@PageIndex - 1) * @PageSize
	DECLARE @NextRow AS INT = @PageIndex * @PageSize
	
	;with result as (
		select u.*, row_number() over (order by Id) as RowNum from users u
		WHERE (u.PhoneNumber LIKE '%' + @text + '%' OR u.FullName LIKE '%' + @text + '%') and u.Status in (0,-1) AND u.Status != 1
	)
	select * from result where result.RowNum > @CurrentRow AND result.RowNum <= @NextRow

	SELECT @TotalRecord = COUNT(*)
    FROM Users u
    WHERE (u.PhoneNumber LIKE '%' + @text + '%' OR u.FullName LIKE '%' + @text + '%') and u.Status in (0,-1) AND u.Status != 1
end
select * from users
-- getAllCustomersAsync ----------------------
getTotalCustomersNumber 2,1
alter proc getTotalCustomersNumber 
	@Status INT,
	@Gender int
as 
begin
	IF @Status = 2 and @gender = 2 
		BEGIN
			select count(*) from Users where Status in (0,-1) and Gender In (0,1)
		END
	ELSE IF @Status = 2 AND @Gender <> 2 
		BEGIN
			SELECT COUNT(*) FROM Users WHERE Status in (0,-1) AND Gender = @Gender
		END
	ELSE IF @Status <> 2 AND @Gender = 2
		BEGIN
			SELECT COUNT(*) FROM Users WHERE Status = @Status AND Gender In (0,1)
		END
	else 
		BEGIN
			select count(*) from Users where Status = @Status and Gender = @gender
		end
end
select * from users u where u.Status IN (-1, 0) and u.Gender in (1, 0)
go

ALTER PROCEDURE getAllCustomers 
	-- Status = 0 -> Account bình thường
	-- Status = 1 -> Account của Admin
	-- Status = -1 -> Account bị khóa
	-- Status = 2 -> Cả Account bình thường và bị khóa
	@Status INT,
	@gender int,
	@PageIndex INT,
	@PageSize INT
AS
BEGIN
	DECLARE @CurrentRow AS INT = (@PageIndex - 1) * @PageSize
	DECLARE @NextRow AS INT = @PageIndex * @PageSize

	DECLARE @result TABLE (
		Id UNIQUEIDENTIFIER PRIMARY KEY,
		Dob DATE,
		PhoneNumber VARCHAR(15),
		Password VARCHAR(MAX),
		FullName NVARCHAR(30),
		Gender INT NULL,
		IdAddress UNIQUEIDENTIFIER,
		Status INT,
		Avatar VARCHAR(MAX)
	)
	
	IF @Status = 2 and @gender = 2
	BEGIN
		INSERT INTO @result 
		SELECT u.* 
		FROM Users u 
		WHERE u.Status IN (-1, 0) and u.Gender In (0,1)
	END
	else if @Status = 2 AND @Gender <> 2 
	begin
		INSERT INTO @result 
		SELECT u.* 
		FROM Users u 
		WHERE u.Status IN (-1, 0) and u.Gender = @gender
	end
	else if @Status  <> 2  AND @Gender = 2 
	begin
		INSERT INTO @result 
		SELECT u.* 
		FROM Users u 
		WHERE u.Status = @Status and u.Gender in (0,1)
	end
	ELSE
	BEGIN
		INSERT INTO @result 
		SELECT u.* 
		FROM Users u 
		WHERE u.Status = @Status and u.Gender = @gender
	END

	SELECT *
	FROM (
		SELECT *,
			ROW_NUMBER() OVER (ORDER BY Id) AS RowNum
		FROM @result
	) AS Temp
	WHERE Temp.RowNum > @CurrentRow AND Temp.RowNum <= @NextRow
END






-- Add seafood --------------------------------------
create PROCEDURE addSeaFood
    @name nvarchar(50),--
    @price decimal(10,2),--
    @unit nvarchar(10),--
    @nameType nvarchar(20),--
    @status int,--
    @idVoucher int,--
    @instruct nvarchar(255),--
    @expirationDate nvarchar(255),--
    @origin nvarchar(30),--
	@description nvarchar(max),
    @primaryImage nvarchar(max), -- 
    @jsonImagesSeaFood NVARCHAR(MAX),
	@jsonImagesDescription NVARCHAR(MAX),--
	@result nvarchar(100) output
AS
BEGIN
	DECLARE @idSeaFoodDetail INT;
	DECLARE @idSeaFood INT;

    -- Chèn dữ liệu vào bảng SeaFoodDetail
    INSERT INTO SeaFoodDetail (Instruct, ExpirationDate, Origin,Description)
    VALUES (@instruct, @expirationDate, @origin, @description);
	-- Lấy ID của loại từ bảng SeaFoodDetail
	SELECT @idSeaFoodDetail = MAX(Id) FROM SeaFoodDetail

	 -- Lấy ID của loại từ bảng Types
    DECLARE @idType int;
    SELECT @idType = Id FROM Types WHERE NameType = @nameType;

	-- Chèn dữ liệu vào bảng Seafood
	INSERT INTO SeaFoods (IdSeaFoodDetail, Name, Price, Unit, IdType, Status, IdVoucher)
    VALUES (@idSeaFoodDetail, @name, @price, @unit, @idType, @status, @idVoucher);
	-- Lấy ID của loại từ bảng Seafood
	SELECT @idSeaFood = MAX(Id) FROM SeaFoods

	-- Insert images for description
    INSERT INTO Images (IdSeaFoodDetail, [Image])
    SELECT @idSeaFoodDetail, [Image]
    FROM OPENJSON(@jsonImagesDescription) WITH (
        [Image] NVARCHAR(255) '$.nameImage'
    );                                       

	 -- Insert primaryImage into Images table with Status = 1
    INSERT INTO Images ([IdSeaFood],[Image],[Status])
    VALUES (@idSeaFood, @primaryImage, 1);

   -- Insert images for seafood
    INSERT INTO Images (IdSeaFood, [Image], [Status])
    SELECT @idSeaFood, [Image], 0
    FROM OPENJSON(@jsonImagesSeaFood) WITH (
        [Image] NVARCHAR(255) '$.nameImage'
    );
	set @result = N'Thêm sản phẩm thành công'
END;

-- Update seafood
create PROCEDURE updateSeaFood
    @idSeaFood INT,
    @name nvarchar(50) = NULL,
    @price decimal(10,2) = NULL,
    @unit nvarchar(10) = NULL,
    @nameType nvarchar(20) = NULL,
    @status int = NULL,
    @idVoucher int = NULL,
    @instruct nvarchar(255) = NULL,
    @expirationDate nvarchar(255) = NULL,
    @origin nvarchar(30) = NULL,
    @description nvarchar(max) = NULL,
    @primaryImage nvarchar(max) = NULL,
    @jsonImagesSeaFood NVARCHAR(MAX) = NULL,
    @jsonImagesDescription NVARCHAR(MAX) = NULL,
    @result nvarchar(100) output
AS
BEGIN
    DECLARE @idSeafoodDetail int
    SELECT @idSeafoodDetail = IdSeaFoodDetail FROM SeaFoods WHERE Id = @idSeaFood

    -- Check if IdSeaFood exists
    IF NOT EXISTS (SELECT 1 FROM SeaFoods WHERE Id = @idSeaFood)
    BEGIN
        SET @result = N'IdSeaFood không tồn tại'
        RETURN
    END

    -- Update data in SeaFoodDetail table
    IF @instruct IS NOT NULL
        OR @expirationDate IS NOT NULL
        OR @origin IS NOT NULL
        OR @description IS NOT NULL
    BEGIN
        UPDATE SeaFoodDetail
        SET Instruct = ISNULL(@instruct, Instruct),
            ExpirationDate = ISNULL(@expirationDate, ExpirationDate),
            Origin = ISNULL(@origin, Origin),
            Description = ISNULL(@description, Description)
        WHERE Id = @idSeafoodDetail
    END

    -- Update data in Types table
    DECLARE @idType int;
    IF @nameType IS NOT NULL
    BEGIN
        SELECT @idType = Id FROM Types WHERE NameType = @nameType;
    END

    -- Update data in SeaFoods table
    UPDATE SeaFoods
    SET Name = ISNULL(@name, Name),
        Price = ISNULL(@price, Price),
        Unit = ISNULL(@unit, Unit),
        IdType = ISNULL(@idType, IdType),
        --Status = ISNULL(@status, Status),
        IdVoucher = ISNULL(@idVoucher, IdVoucher)
    WHERE Id = @idSeaFood

    -- Cập nhật Images cho mô tả
    IF @jsonImagesDescription IS NOT NULL
    BEGIN
        DELETE FROM Images WHERE IdSeaFoodDetail = @idSeafoodDetail;
        
        INSERT INTO Images (IdSeaFoodDetail, [Image])
        SELECT @idSeafoodDetail, [Image]
        FROM OPENJSON(@jsonImagesDescription) WITH (
            [Image] NVARCHAR(255) '$.nameImage'
        );
    END

     -- Cập nhật primaryImage
    IF @primaryImage IS NOT NULL
    BEGIN
        UPDATE Images
        SET 
            [Image] = @primaryImage,
            Status = 1
        WHERE IdSeaFood = @idSeaFood AND [Status] = 1;
    END

    -- Cập nhật Images cho SeaFood
    IF @jsonImagesSeaFood IS NOT NULL
    BEGIN
        DELETE FROM Images WHERE IdSeaFood = @idSeaFood AND [Status] = 0;

        INSERT INTO Images (IdSeaFood, [Image], [Status])
        SELECT @idSeaFood, [Image], 0
        FROM OPENJSON(@jsonImagesSeaFood) WITH (
            [Image] NVARCHAR(255) '$.nameImage'
        );
    END

    SET @result = N'Cập nhật sản phẩm thành công'
END;


-- Delete seafood
create PROCEDURE deleteSeaFood
    @idSeaFood int,
    @result nvarchar(100) OUTPUT
AS
BEGIN
	declare @idSeafoodDetail int
	select @idSeafoodDetail = IdSeaFoodDetail from SeaFoods where Id = @idSeaFood
    -- Xóa các hình ảnh từ bảng Images
    DELETE FROM Images WHERE IdSeaFoodDetail = (SELECT IdSeaFoodDetail FROM SeaFoods WHERE Id = @idSeaFood);
    DELETE FROM Images WHERE IdSeaFood = @idSeaFood;

    -- Xóa dữ liệu từ bảng SeaFoods
    DELETE FROM SeaFoods WHERE Id = @idSeaFood;

    -- Xóa dữ liệu từ bảng SeaFoodDetail
	
    DELETE FROM SeaFoodDetail WHERE Id = @idSeafoodDetail
	
    SET @result = N'Xóa sản phẩm thành công';
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
select * from Images
select * from SeaFoodDetail
select * from Types

-- Thực thi stored addSeaFood với dữ liệu mẫu mới
DECLARE @resultMessage nvarchar(100);
EXEC addSeaFood 
    @name = N'Chả mực',
    @price = 200000,
    @unit = N'1 hộp/500g',
    @nameType = N'Chả',
    @status = 1,
    @idVoucher = 2,
    @instruct = N'Rán nhỏ lửa, nhiều dầu',
    @expirationDate = N'12 tháng kể từ ngày sản xuất',
    @origin = N'Quảng Ninh',
    @description = N'Chả mực Hạ Long là 1 trong những đặc sản nổi tiếng của QN',
    @primaryImage = 'Primary-Img-Chamuc',
    @jsonImagesSeaFood = '[
        {"Image": "seaf-img-1"},
        {"Image": "seaf-img-2"}
    ]',
    @jsonImagesDescription = '[
        {"Image": "desc-img-chamuc1"},
        {"Image": "desc-img-chamuc2"}
    ]',
    @result = @resultMessage OUTPUT;

PRINT @resultMessage;

-- Thực thi stored deleteSeaFood
DECLARE @resultMessage nvarchar(100);
EXEC deleteSeaFood 
    @idSeaFood = 39,
    @result = @resultMessage OUTPUT;

PRINT @resultMessage;



-- test Update
DECLARE @result nvarchar(100)
DECLARE @idSeaFood INT = 40
DECLARE @name nvarchar(50) = NULL
DECLARE @price decimal(10,2) = null
DECLARE @unit nvarchar(10) = NULL
DECLARE @nameType nvarchar(20) = NULL
DECLARE @status int = NULL
DECLARE @idVoucher int = NULL
DECLARE @instruct nvarchar(255) = NULL
DECLARE @expirationDate nvarchar(255) = N'10 tháng kể từ ngày sản xuất'
DECLARE @origin nvarchar(30) = NULL
DECLARE @description nvarchar(max) = NULL
DECLARE @primaryImage nvarchar(max) = NULL
DECLARE @jsonImagesSeaFood NVARCHAR(MAX) = NULL
DECLARE @jsonImagesDescription NVARCHAR(MAX) = NULL

EXEC updateSeaFood 
    @idSeaFood,
    @name,
    @price,
    @unit,
    @nameType,
    @status,
    @idVoucher,
    @instruct,
    @expirationDate,
    @origin,
    @description,
    @primaryImage,
    @jsonImagesSeaFood,
    @jsonImagesDescription,
    @result OUTPUT

SELECT @result AS 'Result'

