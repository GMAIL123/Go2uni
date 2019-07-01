
CREATE PROC SP_InsertDatatoStudyGroupMembers
(
	@StudentID	BIGINT,
	@Ispending	BIT,
	@groupid	BIGINT
)
AS
BEGIN
	INSERT INTO studentinfo.Study_Group_Member 
	VALUES(@StudentID,@Ispending,@groupid,DEFAULT,DEFAULT)
END