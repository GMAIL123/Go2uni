CREATE proc getGroupinfoByID(@groupid bigint)
as	
	begin
		select * from  studentinfo.Study_Group where Group_ID=@groupid and Group_status=1
	end