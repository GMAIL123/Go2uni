CREATE proc getallCommentsbyGroupID(@groupid bigint)
as
	begin
		select gc.*,sa.student_ProfileImage,sa.student_ad_username from studentinfo.Study_Group_Comment gc
		left join studentinfo.Student_Additional sa on gc.student_Fk_ID = sa.student_Fk_ID where gc.Group_Fk_ID=@groupid
	end