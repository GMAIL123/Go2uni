create proc [dbo].[SP_InsertCollegeDay](@video nvarchar(max) , @status bit)
as
	begin
		insert into collegeEvents.NextCollege values(@video,default,@status)
	end