create proc [dbo].[SP_InsertDatatoStudyGroup](@name nvarchar(50),@notes nvarchar(max),@about nvarchar(max))
as
	begin
		insert into studentinfo.Study_Group values(@name,@notes,@about,default,default,default)
	end