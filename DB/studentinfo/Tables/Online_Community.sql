CREATE TABLE [studentinfo].[Online_Community] (
    [Community_ID]          BIGINT         IDENTITY (10, 1) NOT NULL,
    [Community_Name]        NVARCHAR (100) CONSTRAINT [DF__Online_Co__Commu__5CD6CB2B] DEFAULT ('') NOT NULL,
    [Community_About]       NVARCHAR (MAX) CONSTRAINT [DF__Online_Co__Commu__5DCAEF64] DEFAULT ('') NOT NULL,
    [Community_CreatedDate] DATETIME       CONSTRAINT [DF__Online_Co__Commu__5EBF139D] DEFAULT (CONVERT([varchar](10),getdate(),(103))) NOT NULL,
    [Community_status]      BIT            NULL,
    CONSTRAINT [PK__Online_C__E7729A76EF54384B] PRIMARY KEY CLUSTERED ([Community_ID] ASC)
);

