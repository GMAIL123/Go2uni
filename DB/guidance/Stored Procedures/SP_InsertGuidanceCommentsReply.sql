CREATE PROCEDURE [guidance].[SP_InsertGuidanceCommentsReply]
(
	@text NVARCHAR(MAX),
	@studentid BIGINT,
	@comment BIGINT
)
AS
BEGIN
	INSERT INTO guidance.Guidance_Comments_Reply 
	VALUES(@text,DEFAULT,@studentid,@comment)
END