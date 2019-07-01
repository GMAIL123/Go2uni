CREATE TABLE [studentinfo].[Study_Group] (
    [Group_ID]          BIGINT         IDENTITY (100, 1) NOT NULL,
    [Group_Name]        NVARCHAR (50)  CONSTRAINT [DF__Study_Gro__Group__6B24EA82] DEFAULT ('') NOT NULL,
    [Group_Notes]       NVARCHAR (MAX) CONSTRAINT [DF__Study_Gro__Group__6C190EBB] DEFAULT ('') NOT NULL,
    [Group_About]       NVARCHAR (MAX) CONSTRAINT [DF__Study_Gro__Group__6D0D32F4] DEFAULT ('') NOT NULL,
    [Group_Comment_ID]  NVARCHAR (MAX) CONSTRAINT [DF__Study_Gro__Group__6E01572D] DEFAULT ('') NOT NULL,
    [Group_CreatedDate] DATETIME       CONSTRAINT [DF__Study_Gro__Group__6EF57B66] DEFAULT (getdate()) NOT NULL,
    [Group_status]      BIT            CONSTRAINT [DF__Study_Gro__Group__6FE99F9F] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK__Study_Gr__31981269C709E091] PRIMARY KEY CLUSTERED ([Group_ID] ASC)
);



