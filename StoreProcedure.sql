-- Sign In
CREATE PROCEDURE SignIn
    @phoneNumber VARCHAR(15)
AS
BEGIN
    DECLARE @sql NVARCHAR(MAX);
    SET @sql = N'SELECT Password, Role from users where PhoneNumber = @phoneNumber';
    EXEC sp_executesql @sql, N'@phoneNumber VARCHAR(15)', @phoneNumber;
END;

-- Sign Up
create procedure SignUp 
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
		set @sql = N'insert into users(Id,Dob,PhoneNumber,Password,FullName) values (NewID(),@dob,@phoneNumber,@password,@fullName)';
		exec sp_executesql @sql, N'@dob date,@phoneNumber varchar(15),@password varchar(max),@fullName nvarchar(30)',@dob,@phoneNumber,@password,@fullName;
		set @result = 'Sign up successfully';
	end;
end;



--DECLARE @result NVARCHAR(100);
--EXECUTE SignUp '2003-03-02', '0869819316', 'MinhLe123', N'Lê Anh Minh', @result OUTPUT;
--print @result;
execute SignIn '0869819316';
select * from users;