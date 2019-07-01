CREATE TABLE [studentinfo].[Student_Basic] (
    [student_ID]       BIGINT        IDENTITY (1000, 1) NOT NULL,
    [Student_Email]    NVARCHAR (60) CONSTRAINT [DF__Student_B__studn__68487DD7] DEFAULT ('') NOT NULL,
    [Student_password] NVARCHAR (20) CONSTRAINT [DF__Student_B__stude__693CA210] DEFAULT ('123456') NOT NULL,
    [Student_Status]   BIT           CONSTRAINT [DF__Student_B__stude__6A30C649] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK__Student___2A082B2266903251] PRIMARY KEY CLUSTERED ([student_ID] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [NonClusteredIndex-20180704-151505]
    ON [studentinfo].[Student_Basic]([Student_Email] ASC);


GO
create trigger [studentinfo].NewStudentRegistration on studentinfo.Student_Basic
for insert
	as
	begin
		set NOCOUNT ON;
		declare @studentid int;
		set @studentid = IDENT_CURRENT('studentinfo.Student_Basic')
		insert into studentinfo.Student_Additional values('student','',default,'','',@studentid)
	end