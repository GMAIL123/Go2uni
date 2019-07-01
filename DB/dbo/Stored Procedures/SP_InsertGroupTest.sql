CREATE proc SP_InsertGroupTest(@testdate date, @comments nvarchar(500),@id bigint)
as
	begin
		insert into studentinfo.Study_Group_Test values(@testdate,@comments,@id)
	end