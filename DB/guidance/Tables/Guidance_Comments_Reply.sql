CREATE TABLE [guidance].[Guidance_Comments_Reply] (
    [Guidance_Comments_Reply_ID]           BIGINT         IDENTITY (1000, 1) NOT NULL,
    [Guidance_Comments_Reply_Text]         NVARCHAR (MAX) CONSTRAINT [DF__Guidance___Guida__5AEE82B9] DEFAULT ('') NOT NULL,
    [Guidance_Comments_Reply_InsertedDate] DATETIME       CONSTRAINT [DF__Guidance___Guida__5BE2A6F2] DEFAULT (CONVERT([varchar](10),getdate(),(103))) NOT NULL,
    [student_Fk_ID]                        BIGINT         NULL,
    [Guidance_Comments_ID]                 BIGINT         NULL,
    CONSTRAINT [PK__Guidance__B1D4C4D74CC994E6] PRIMARY KEY CLUSTERED ([Guidance_Comments_Reply_ID] ASC),
    CONSTRAINT [FK__Guidance___Guida__08B54D69] FOREIGN KEY ([Guidance_Comments_ID]) REFERENCES [guidance].[Guidance_Comments] ([Guidance_Comments_ID]),
    CONSTRAINT [FK__Guidance___stude__09A971A2] FOREIGN KEY ([student_Fk_ID]) REFERENCES [studentinfo].[Student_Basic] ([student_ID])
);

