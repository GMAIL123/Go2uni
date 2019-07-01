
CREATE PROC [proc_community].[SP_InsertcommunityMember]
(
	@StudentID		BIGINT,
	@CommunityID	BIGINT
)
AS
BEGIN
	INSERT INTO studentinfo.Online_Community_Members VALUES(@StudentID,@CommunityID,DEFAULT,DEFAULT)
END