create proc SP_GetallCommunity
as
	begin
		 select oc.Community_ID,oc.Community_Name,oc.Community_CreatedDate
		 from studentinfo.Online_Community oc 
	end