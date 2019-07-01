Create proc SP_GetallTopicDetails(@TopicID bigint)
	as 
	begin
	select  ct.Topic_ID,ct.studentCreatedBy_Fk_ID,ct.Community_Fk_ID,ct.Topic_Discussion,ct.Topic_CreatedDate
	 ,sa.Student_Name as CreatedBy ,sa.student_ProfileImage,cc.Community_Name as CommunityName , occ.Community_Comment_ID,occ.Community_Comment_Like
	 ,occ.Community_Comment_Dislike,occ.Community_Comment_Text,
	occ.Community_Comment_CommentedDate,occ.Community_Comment_status from 
	studentinfo.Online_Community_Comment as occ 
	inner join studentinfo.Online_Community_Topic as ct on ct.Topic_ID=occ.Topic_Fk_ID 
	inner join studentinfo.Online_Community as cc on cc.Community_ID=ct.Community_Fk_ID
	inner join studentinfo.Student_Additional as sa on sa.student_Fk_ID=ct.studentCreatedBy_Fk_ID 
	where ct.Topic_ID=@TopicID
	end