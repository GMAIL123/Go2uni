CREATE TABLE [studentinfo].[Online_Community_Comment_Reply] (
    [Community_Comment_Reply_ID]            BIGINT         IDENTITY (1000, 1) NOT NULL,
    [Community_Comment_Reply_Text]          NVARCHAR (MAX) CONSTRAINT [DF__Online_Co__Commu__6383C8BA] DEFAULT ('') NOT NULL,
    [Community_Comment_Reply_CommentedDate] DATETIME       CONSTRAINT [DF__Online_Co__Commu__6477ECF3] DEFAULT (CONVERT([varchar](10),getdate(),(103))) NOT NULL,
    [student_Fk_ID]                         BIGINT         NULL,
    [Community_Comment_FK_ID]               BIGINT         NULL,
    [Community_Comment_Reply_status]        BIT            NULL,
    CONSTRAINT [PK__Online_C__CFE310AED6E4F37A] PRIMARY KEY CLUSTERED ([Community_Comment_Reply_ID] ASC),
    CONSTRAINT [FK__Online_Co__Commu__0C85DE4D] FOREIGN KEY ([Community_Comment_FK_ID]) REFERENCES [studentinfo].[Online_Community_Comment] ([Community_Comment_ID]),
    CONSTRAINT [FK__Online_Co__stude__0D7A0286] FOREIGN KEY ([student_Fk_ID]) REFERENCES [studentinfo].[Student_Basic] ([student_ID])
);

