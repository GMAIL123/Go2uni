CREATE TABLE [studentinfo].[Study_Group_Topic] (
    [ID]           BIGINT         IDENTITY (1, 1) NOT NULL,
    [Topic]        NVARCHAR (500) NOT NULL,
    [GroupID_FK]   BIGINT         NOT NULL,
    [StudentID_FK] BIGINT         NOT NULL,
    [CreatedDate]  DATETIME       CONSTRAINT [DF_Study_Group_Topic_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [Status]       BIT            CONSTRAINT [DF_Study_Group_Topic_Status] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Study_Group_Topic] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Study_Group_Topic_Student_Basic] FOREIGN KEY ([StudentID_FK]) REFERENCES [studentinfo].[Student_Basic] ([student_ID]),
    CONSTRAINT [FK_Study_Group_Topic_Study_Group] FOREIGN KEY ([GroupID_FK]) REFERENCES [studentinfo].[Study_Group] ([Group_ID])
);

