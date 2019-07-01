
CREATE PROCEDURE SP_GetGroupNameByID 
(
	@StudentID BIGINT
)
AS
BEGIN
	SELECT sg.Group_ID,Group_Name FROM [studentinfo].[Study_Group_Member] AS sgm
	LEFT JOIN [studentinfo].[Study_Group] AS sg ON sgm.Group_FK_ID=sg.Group_ID
	WHERE sgm.StudentID_FK=@StudentID
END