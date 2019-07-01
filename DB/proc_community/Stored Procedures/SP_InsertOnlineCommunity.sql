CREATE PROC [proc_community].[SP_InsertOnlineCommunity]
(
	@name	NVARCHAR(100),
	@about	NVARCHAR(max),
	@status BIT
)
AS
BEGIN
	INSERT INTO studentinfo.Online_Community values(@name,@about,default,@status)
END