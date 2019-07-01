	Create proc SP_GetallCommunityTopic(@CommunityID bigint)
	as 
	begin
	select ct.Topic_ID,ct.studentCreatedBy_Fk_ID,ct.Community_Fk_ID,ct.Topic_Discussion,ct.Topic_CreatedDate
	 ,sa.Student_Name as CreatedBy ,sa.student_ProfileImage,cc.Community_Name as CommunityName 
	from  studentinfo.Online_Community_Topic as ct 
	inner join studentinfo.Student_Additional as sa on sa.student_Fk_ID=ct.studentCreatedBy_Fk_ID 
	inner join studentinfo.Online_Community as cc on cc.Community_ID=ct.Community_Fk_ID 
	where ct.Community_Fk_ID=@CommunityID
	end