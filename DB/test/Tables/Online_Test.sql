CREATE TABLE [test].[Online_Test] (
    [Test_ID]           BIGINT         IDENTITY (10, 1) NOT NULL,
    [Test_Time]         TIME (7)       NULL,
    [Online_Subjects]   NVARCHAR (500) NULL,
    [Test_Name]         NVARCHAR (100) NULL,
    [Online_Subject_ID] BIGINT         NULL,
    CONSTRAINT [PK__Online_T__B502D0022E15FD4A] PRIMARY KEY CLUSTERED ([Test_ID] ASC),
    CONSTRAINT [FK__Online_Te__Onlin__47A6A41B] FOREIGN KEY ([Online_Subject_ID]) REFERENCES [test].[Online_Test_Subject] ([Online_Subject_ID])
);

