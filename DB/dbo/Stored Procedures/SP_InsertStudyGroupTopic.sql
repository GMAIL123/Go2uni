CREATE PROCEDURE SP_InsertStudyGroupTopic
(
	@Topic		NVARCHAR(500),
	@GroupID	BIGINT, 
	@STudentID	BIGINT
)
AS 
BEGIN
	INSERT INTO [studentinfo].[Study_Group_Topic](Topic,GroupID_FK,STudentID_FK) 
	VALUES(@Topic,@GroupID,@STudentID)
END