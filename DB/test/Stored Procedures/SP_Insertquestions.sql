CREATE PROC test.SP_Insertquestions
(
	@questions			NVARCHAR(MAX), 
	@answers			NVARCHAR(MAX), 
	@answer				NVARCHAR(1),
	@score				DECIMAL,
	@QuestionModules_ID BIGINT
)
AS
BEGIN
	INSERT INTO test.Online_Test_Question 
	VALUES(@questions,@answers,@answer,DEFAULT,@score,DEFAULT,@QuestionModules_ID)
END