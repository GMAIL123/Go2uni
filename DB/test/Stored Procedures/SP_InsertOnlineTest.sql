CREATE PROCEDURE test.SP_InsertOnlineTest
(
	@Test_Time			TIME,
	@Online_Subjects	NVARCHAR(500),
	@Test_Name			NVARCHAR(100),
	@Online_Subject_ID	BIGINT
)
AS 
BEGIN
	INSERT INTO test.Online_Test(Test_Time,Online_Subjects,Test_Name,Online_Subject_ID) 
	VALUES(@Test_Time,@Online_Subjects,@Test_Name,@Online_Subject_ID)
END