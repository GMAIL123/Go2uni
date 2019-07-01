CREATE TABLE [studentinfo].[Study_Group_Member] (
    [Group_Member_ID] BIGINT   IDENTITY (500, 1) NOT NULL,
    [StudentID_FK]    BIGINT   CONSTRAINT [DF__Study_Gro__Group__72C60C4A] DEFAULT ('') NOT NULL,
    [IsPending]       BIT      CONSTRAINT [DF__Study_Gro__Group__73BA3083] DEFAULT ('') NOT NULL,
    [Group_Fk_ID]     BIGINT   NOT NULL,
    [CreatedDate]     DATETIME CONSTRAINT [DF_Study_Group_Member_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedDate]    DATETIME CONSTRAINT [DF_Study_Group_Member_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK__Study_Gr__91186C3EC8CF8074] PRIMARY KEY CLUSTERED ([Group_Member_ID] ASC),
    CONSTRAINT [FK__Study_Gro__Group__123EB7A3] FOREIGN KEY ([Group_Fk_ID]) REFERENCES [studentinfo].[Study_Group] ([Group_ID]),
    CONSTRAINT [FK_Study_Group_Member_Student_Basic] FOREIGN KEY ([StudentID_FK]) REFERENCES [studentinfo].[Student_Basic] ([student_ID])
);



