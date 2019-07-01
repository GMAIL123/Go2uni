CREATE PROC [test].[SP_InsertSubject]
(
	@name		NVARCHAR(500),
	@modules	NVARCHAR(500)
)
AS
BEGIN
	INSERT INTO test.Online_Test_Subject 
	VALUES(@name,@modules)
END