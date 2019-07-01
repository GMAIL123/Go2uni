
CREATE PROCEDURE SP_GetCommentByGroupID 
(
	@GroupID BIGINT,
	@TopicID BIGINT
)
AS
BEGIN
	SELECT sgc.Group_Comment_Text,sgc.Group_Comment_CommentedDate,sa.Student_Name,sa.student_ProfileImage,
	sgc.Group_Fk_ID,sgc.TopicID_FK,sgc.student_Fk_ID,sgc.Group_Comment_ID
	FROM [studentinfo].[Study_Group_Comment] AS sgc
	LEFT JOIN [studentinfo].[Student_Additional] AS sa ON sa.student_Fk_ID=sgc.student_Fk_ID
	WHERE sgc.Group_Fk_ID=@GroupID AND sgc.TopicID_FK=@TopicID ORDER BY sgc.Group_Comment_CommentedDate DESC
END