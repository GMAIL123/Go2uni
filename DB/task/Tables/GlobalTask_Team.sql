CREATE TABLE [task].[GlobalTask_Team] (
    [GlobalTask_Team_ID]          BIGINT         IDENTITY (1000, 1) NOT NULL,
    [student_ID]                  BIGINT         CONSTRAINT [DF__GlobalTas__stude__787EE5A0] DEFAULT ('') NOT NULL,
    [GlobalTask_Team_Image]       NVARCHAR (MAX) CONSTRAINT [DF__GlobalTas__Globa__797309D9] DEFAULT ('') NOT NULL,
    [GlobalTask_Team_CreatedDate] DATETIME       CONSTRAINT [DF__GlobalTas__Globa__7A672E12] DEFAULT (CONVERT([varchar](10),getdate(),(103))) NOT NULL,
    [GlobalTask_Team_Vote]        BIGINT         CONSTRAINT [DF__GlobalTas__Globa__7B5B524B] DEFAULT ((0)) NOT NULL,
    [GlobalTask_ID]               BIGINT         NULL,
    [GlobalTask_Team_status]      BIT            CONSTRAINT [DF__GlobalTas__Globa__7C4F7684] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK__GlobalTa__34B03845E3C47898] PRIMARY KEY CLUSTERED ([GlobalTask_Team_ID] ASC),
    CONSTRAINT [FK_GlobalTask_Team_GlobalTask] FOREIGN KEY ([GlobalTask_ID]) REFERENCES [task].[GlobalTask] ([GlobalTask_ID]),
    CONSTRAINT [FK_GlobalTask_Team_Student_Basic] FOREIGN KEY ([student_ID]) REFERENCES [studentinfo].[Student_Basic] ([student_ID])
);

