

CREATE PROC test.SP_InsertModules
(
	@name		NVARCHAR(500),
	@questions	NVARCHAR(500),
	@SubjectID	BIGINT)
AS
BEGIN
	INSERT INTO test.Online_Test_Modules 
	VALUES(@name,@questions,@SubjectID)
END