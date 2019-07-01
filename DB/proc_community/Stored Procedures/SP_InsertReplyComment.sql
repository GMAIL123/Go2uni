CREATE PROC proc_community.SP_InsertReplyComment
(	
	@text NVARCHAR(MAX),
	@studentid BIGINT , 
	@commentid BIGINT,
	@status BIT
)
AS
BEGIN
	INSERT INTO studentinfo.Online_Community_Comment_Reply 
	VALUES(@text,DEFAULT,@studentid,@commentid,@status)
END