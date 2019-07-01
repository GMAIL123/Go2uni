CREATE TABLE [studentinfo].[Online_Community_Comment] (
    [Community_Comment_ID]            BIGINT         IDENTITY (500, 1) NOT NULL,
    [Community_Comment_Like]          BIGINT         CONSTRAINT [DF__Online_Co__Commu__5FB337D6] DEFAULT ((0)) NOT NULL,
    [Community_Comment_Dislike]       BIGINT         CONSTRAINT [DF__Online_Co__Commu__60A75C0F] DEFAULT ((0)) NOT NULL,
    [Community_Comment_Text]          NVARCHAR (MAX) CONSTRAINT [DF__Online_Co__Commu__619B8048] DEFAULT ('') NOT NULL,
    [Community_Comment_CommentedDate] DATETIME       CONSTRAINT [DF__Online_Co__Commu__628FA481] DEFAULT (CONVERT([varchar](10),getdate(),(103))) NOT NULL,
    [student_Fk_ID]                   BIGINT         NULL,
    [Community_Comment_status]        BIT            CONSTRAINT [DF__Online_Co__Commu__2739D489] DEFAULT ((1)) NOT NULL,
    [Topic_Fk_ID]                     BIGINT         NULL,
    CONSTRAINT [PK__Online_C__350AA9B0A517F64F] PRIMARY KEY CLUSTERED ([Community_Comment_ID] ASC),
    CONSTRAINT [FK__Online_Co__stude__0B91BA14] FOREIGN KEY ([student_Fk_ID]) REFERENCES [studentinfo].[Student_Basic] ([student_ID]),
    CONSTRAINT [FK_Online_Community_Topic] FOREIGN KEY ([Topic_Fk_ID]) REFERENCES [studentinfo].[Online_Community_Topic] ([Topic_ID])
);



