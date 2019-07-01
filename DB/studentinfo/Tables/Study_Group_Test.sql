CREATE TABLE [studentinfo].[Study_Group_Test] (
    [Group_Test_ID]    BIGINT         IDENTITY (700, 1) NOT NULL,
    [Group_Test_Date]  DATETIME       NOT NULL,
    [Group_Comment_ID] NVARCHAR (500) NULL,
    [Group_Fk_ID]      BIGINT         NULL,
    CONSTRAINT [PK__Study_Gr__72B80934C8AB481F] PRIMARY KEY CLUSTERED ([Group_Test_ID] ASC),
    CONSTRAINT [FK__Study_Gro__Group__1332DBDC] FOREIGN KEY ([Group_Fk_ID]) REFERENCES [studentinfo].[Study_Group] ([Group_ID])
);

