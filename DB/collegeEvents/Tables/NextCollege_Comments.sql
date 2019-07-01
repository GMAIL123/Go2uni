CREATE TABLE [collegeEvents].[NextCollege_Comments] (
    [NxtCollege_Comments_ID]           BIGINT         IDENTITY (10, 1) NOT NULL,
    [NxtCollege_Comments_Text]         NVARCHAR (MAX) CONSTRAINT [DF__NextColle__NxtCo__52593CB8] DEFAULT ('') NOT NULL,
    [NxtCollege_Comments_InsertedDate] DATETIME       CONSTRAINT [DF__NextColle__NxtCo__534D60F1] DEFAULT (CONVERT([varchar](10),getdate(),(103))) NOT NULL,
    [student_Fk_ID]                    BIGINT         NULL,
    [NxtCollege_Fk_ID]                 BIGINT         NULL,
    CONSTRAINT [PK__NextColl__FB35BA9CD3E14835] PRIMARY KEY CLUSTERED ([NxtCollege_Comments_ID] ASC),
    CONSTRAINT [FK__NextColle__NxtCo__03F0984C] FOREIGN KEY ([NxtCollege_Fk_ID]) REFERENCES [collegeEvents].[NextCollege] ([NxtCollege_ID]),
    CONSTRAINT [FK__NextColle__stude__04E4BC85] FOREIGN KEY ([student_Fk_ID]) REFERENCES [studentinfo].[Student_Basic] ([student_ID])
);

