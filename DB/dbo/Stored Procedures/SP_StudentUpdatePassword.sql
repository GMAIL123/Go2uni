CREATE PROCEDURE SP_StudentUpdatePassword
(
	@UserName NVARCHAR(100),
	@Password NVARCHAR(100)
)
AS 
BEGIN
	UPDATE	[studentinfo].[Student_Basic] 
	SET		Student_Password=@Password
	WHERE	Student_Email=@UserName
END