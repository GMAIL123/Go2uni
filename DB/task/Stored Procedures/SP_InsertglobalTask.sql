CREATE PROC [task].[SP_InsertglobalTask]
(
	@taskName	NVARCHAR(500),
	@allTeam	NVARCHAR(500),
	@status		BIT
)
AS
BEGIN
	INSERT INTO task.GlobalTask 
	VALUES(@taskName,@allTeam,DEFAULT,@status)
END