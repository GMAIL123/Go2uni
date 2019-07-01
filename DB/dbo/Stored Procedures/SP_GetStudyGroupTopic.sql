CREATE PROCEDURE SP_GetStudyGroupTopic  
(
	@GroupID	BIGINT, 
	@STudentID	BIGINT
)
AS 
BEGIN
	SELECT sgt.ID,sgt.Topic,sgt.GroupID_FK,sgt.CreatedDate,sa.Student_Name FROM [studentinfo].[Study_Group_Topic] AS sgt
	LEFT JOIN [studentinfo].[Student_Additional] AS sa ON sa.student_Fk_ID=sgt.StudentID_FK
	WHERE sgt.StudentID_FK=@StudentID AND sgt.GroupID_FK=@GroupID AND sgt.Status=1 
	ORDER BY sgt.CreatedDate DESC
END