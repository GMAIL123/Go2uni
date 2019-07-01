CREATE proc SP_GetallDatafromStudentsByID(@id bigint)
as
		begin
			select sb.*,sab.* from studentinfo.Student_Basic sb left join studentinfo.Student_Additional sab
			on sb.student_ID = sab.student_Fk_ID where sb.student_ID = @id
		end