CREATE TABLE [test].[Online_Test_Modules] (
    [QuestionModules_ID]        BIGINT         IDENTITY (500, 1) NOT NULL,
    [QuestionModules_Name]      NVARCHAR (500) NULL,
    [QuestionModules_Questions] NVARCHAR (500) CONSTRAINT [DF__Online_Te__Quest__7D439ABD] DEFAULT ('') NOT NULL,
    [Online_Subject_ID]         BIGINT         NULL,
    CONSTRAINT [PK__Online_T__8950352BB77A8F0D] PRIMARY KEY CLUSTERED ([QuestionModules_ID] ASC),
    CONSTRAINT [FK__Online_Te__Onlin__46B27FE2] FOREIGN KEY ([Online_Subject_ID]) REFERENCES [test].[Online_Test_Subject] ([Online_Subject_ID])
);

