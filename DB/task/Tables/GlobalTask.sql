CREATE TABLE [task].[GlobalTask] (
    [GlobalTask_ID]          BIGINT         IDENTITY (500, 1) NOT NULL,
    [GlobalTask_Name]        NVARCHAR (500) CONSTRAINT [DF__GlobalTas__Globa__74AE54BC] DEFAULT ('') NOT NULL,
    [GlobalTask_allTeamID]   NVARCHAR (500) CONSTRAINT [DF__GlobalTas__Globa__75A278F5] DEFAULT ('') NOT NULL,
    [GlobalTask_CreatedDate] DATETIME       CONSTRAINT [DF__GlobalTas__Globa__76969D2E] DEFAULT (CONVERT([varchar](10),getdate(),(103))) NOT NULL,
    [GlobalTask_status]      BIT            CONSTRAINT [DF__GlobalTas__Globa__778AC167] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK__GlobalTa__80E25431140AE16C] PRIMARY KEY CLUSTERED ([GlobalTask_ID] ASC)
);

