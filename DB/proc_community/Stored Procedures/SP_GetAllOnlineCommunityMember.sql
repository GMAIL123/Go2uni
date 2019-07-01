create proc proc_community.SP_GetAllOnlineCommunityMember(@groupid bigint)
as
	begin
		select cm.Community_Member_ID,cm.Community_Member_CreatedDate,sa.Student_Name,sa.student_ad_username,
		sa.student_ProfileImage from studentinfo.Online_Community_Members cm
		left join studentinfo.Student_Additional sa on sa.student_Fk_ID = cm.Community_Members where cm.Community_Member_Status = 1
	end