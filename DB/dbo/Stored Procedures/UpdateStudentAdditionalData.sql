CREATE proc UpdateStudentAdditionalData(@username nvarchar(20),@phone nvarchar(15),@image nvarchar(max),@id bigint)
as
	begin
		update studentinfo.Student_Additional set student_ad_username = @username , student_mobile = @phone , 
		student_ProfileImage = @image where student_Fk_ID = @id

	end