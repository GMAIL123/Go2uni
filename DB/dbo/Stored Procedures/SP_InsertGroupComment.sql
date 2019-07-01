
CREATE PROCEDURE SP_InsertGroupComment
(
	@comment	NVARCHAR(MAX),
	@groupid	BIGINT,
	@studentid	BIGINT
)
AS
BEGIN
	INSERT INTO studentinfo.Study_Group_Comment 
	VALUES(@comment,DEFAULT,@groupid,@studentid)
END