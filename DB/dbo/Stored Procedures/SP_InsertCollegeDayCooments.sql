CREATE proc SP_InsertCollegeDayCooments(@comment nvarchar(max) , @id bigint,@nxtdayid int)
as
	begin
		insert into collegeEvents.NextCollege_Comments values(@comment,default,@id,@nxtdayid)
	end