CREATE proc getstudentdatabyemail(@email nvarchar(60))
as
	begin
		select sb.*,sab.* from studentinfo.Student_Basic sb left join studentinfo.Student_Additional sab
		on sb.student_ID = sab.student_Fk_ID where sb.Student_Email = @email
	end