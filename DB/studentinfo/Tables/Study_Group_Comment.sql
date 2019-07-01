CREATE TABLE [studentinfo].[Study_Group_Comment] (
    [Group_Comment_ID]            BIGINT         IDENTITY (2000, 1) NOT NULL,
    [Group_Comment_Text]          NVARCHAR (MAX) CONSTRAINT [DF__Study_Gro__Group__70DDC3D8] DEFAULT ('') NOT NULL,
    [Group_Comment_CommentedDate] DATETIME       CONSTRAINT [DF__Study_Gro__Group__71D1E811] DEFAULT (getdate()) NOT NULL,
    [Group_Fk_ID]                 BIGINT         NOT NULL,
    [student_Fk_ID]               BIGINT         NOT NULL,
    [TopicID_FK]                  BIGINT         NOT NULL,
    CONSTRAINT [PK__Study_Gr__C75F0916B87B7FC8] PRIMARY KEY CLUSTERED ([Group_Comment_ID] ASC),
    CONSTRAINT [FK__Study_Gro__Group__10566F31] FOREIGN KEY ([Group_Fk_ID]) REFERENCES [studentinfo].[Study_Group] ([Group_ID]),
    CONSTRAINT [FK__Study_Gro__stude__114A936A] FOREIGN KEY ([student_Fk_ID]) REFERENCES [studentinfo].[Student_Basic] ([student_ID]),
    CONSTRAINT [FK_Study_Group_Comment_Study_Group_Topic] FOREIGN KEY ([TopicID_FK]) REFERENCES [studentinfo].[Study_Group_Topic] ([ID])
);



