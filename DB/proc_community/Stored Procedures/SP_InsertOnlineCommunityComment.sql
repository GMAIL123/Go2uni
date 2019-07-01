
CREATE PROC proc_community.SP_InsertOnlineCommunityComment
(	
	@like		BIGINT,
	@dislike	BIGINT,
	@text		NVARCHAR(MAX),
	@studentid	BIGINT,
	@comID		BIGINT,
	@status		BIT
)
AS
BEGIN
	INSERT INTO studentinfo.Online_Community_Comment 
	VALUES(@like,@dislike,@text,DEFAULT,@studentid,@comID,@status)
END