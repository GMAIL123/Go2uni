CREATE TABLE [collegeEvents].[NextCollege_Comments_Reply] (
    [NxtCollege_Comments_Reply_ID]           BIGINT         IDENTITY (100, 1) NOT NULL,
    [NxtCollege_Comments_Reply_Text]         NVARCHAR (MAX) CONSTRAINT [DF__NextColle__NxtCo__5441852A] DEFAULT ('') NOT NULL,
    [NxtCollege_Comments_Reply_InsertedDate] DATETIME       CONSTRAINT [DF__NextColle__NxtCo__5535A963] DEFAULT (CONVERT([varchar](10),getdate(),(103))) NOT NULL,
    [student_Fk_ID]                          BIGINT         NULL,
    [Comments_Fk_ID]                         BIGINT         NULL,
    CONSTRAINT [PK__NextColl__AD5B44A8E0B2DEA7] PRIMARY KEY CLUSTERED ([NxtCollege_Comments_Reply_ID] ASC),
    CONSTRAINT [FK__NextColle__Comme__05D8E0BE] FOREIGN KEY ([Comments_Fk_ID]) REFERENCES [collegeEvents].[NextCollege_Comments] ([NxtCollege_Comments_ID]),
    CONSTRAINT [FK__NextColle__stude__06CD04F7] FOREIGN KEY ([student_Fk_ID]) REFERENCES [studentinfo].[Student_Basic] ([student_ID])
);

