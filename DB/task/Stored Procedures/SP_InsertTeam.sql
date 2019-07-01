
CREATE PROC task.SP_InsertTeam

(	

	@studentsid		NVARCHAR(500),

	@image			NVARCHAR(max),

	@status			BIT,

	@GlobalTask_ID	BIGINT

)

AS

BEGIN

	INSERT INTO task.GlobalTask_Team 

	VALUES(@studentsid,@image,DEFAULT,DEFAULT,@status,@GlobalTask_ID)

END