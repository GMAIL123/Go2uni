CREATE PROCEDURE SP_InsertStudent
(
	@Email NVARCHAR(100),
	@Password NVARCHAR(100)
)
AS 
BEGIN
	INSERT INTO	[studentinfo].[Student_Basic]
	VALUES(@Email,@Password,DEFAULT)
END