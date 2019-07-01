create proc [dbo].[SP_InsertCollegeDayReplyCooments](@comment nvarchar(max) , @id int,@commentid int)
as
	begin
		insert into collegeEvents.NextCollege_Comments_Reply values(@comment,default,@id,@commentid)
	end