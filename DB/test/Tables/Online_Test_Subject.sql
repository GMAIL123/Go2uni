CREATE TABLE [test].[Online_Test_Subject] (
    [Online_Subject_ID]         BIGINT         IDENTITY (100, 1) NOT NULL,
    [Online_Subject_Name]       NVARCHAR (500) CONSTRAINT [DF__Online_Te__Onlin__02084FDA] DEFAULT ('') NOT NULL,
    [Online_Subject_ModulesSet] NVARCHAR (500) CONSTRAINT [DF__Online_Te__Onlin__02FC7413] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK__Online_T__250B4A3460FE0CB3] PRIMARY KEY CLUSTERED ([Online_Subject_ID] ASC)
);

