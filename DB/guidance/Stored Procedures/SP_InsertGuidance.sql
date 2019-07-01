CREATE PROC [guidance].[SP_InsertGuidance]
(
	@video	NVARCHAR(MAX),
	@status BIT
)
AS
BEGIN
	INSERT INTO guidance.Guidance 
	VALUES(@video,DEFAULT,@status)
END