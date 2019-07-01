CREATE TABLE [studentinfo].[Student_Additional] (
    [studnet_ad_ID]          BIGINT         IDENTITY (10000, 1) NOT NULL,
    [student_ad_username]    NVARCHAR (20)  CONSTRAINT [DF__Student_A__stude__66603565] DEFAULT ('') NOT NULL,
    [Student_Name]           NVARCHAR (100) NULL,
    [student_ad_joiningDate] DATE           CONSTRAINT [DF__Student_A__stude__6754599E] DEFAULT (CONVERT([varchar](10),getdate(),(103))) NULL,
    [student_mobile]         NVARCHAR (15)  NULL,
    [student_ProfileImage]   NVARCHAR (MAX) NULL,
    [student_Fk_ID]          BIGINT         NULL,
    CONSTRAINT [PK__Student___234D2356541C5167] PRIMARY KEY CLUSTERED ([studnet_ad_ID] ASC),
    CONSTRAINT [FK__Student_A__stude__0F624AF8] FOREIGN KEY ([student_Fk_ID]) REFERENCES [studentinfo].[Student_Basic] ([student_ID])
);



