CREATE PROCEDURE SP_StudentLogIn
(
	@UserName NVARCHAR(100),
	@Password NVARCHAR(100)
)
AS 
BEGIN
	SELECT		ISNULL(CAST(sb.student_ID AS NVARCHAR(50))+'|'+sb.Student_Email+'|'+sa.student_ProfileImage+'|'
				+sa.student_ad_username+'|'+sa.Student_Name,'') AS StaticVariable
	FROM		[studentinfo].[Student_Basic] AS sb
	LEFT JOIN	[studentinfo].[Student_Additional] AS sa
	ON			sa.student_Fk_ID=sb.student_ID
	WHERE		sb.Student_Email=@UserName AND sb.Student_password=@Password AND sb.Student_status=1
END