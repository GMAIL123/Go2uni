CREATE PROC [guidance].[SP_InsertGuidanceComments]
(
	@text		NVARCHAR(MAX),
	@guideID	BIGINT
)
AS
BEGIN
	INSERT INTO guidance.Guidance 
	VALUES(@text,DEFAULT,@guideID)
END