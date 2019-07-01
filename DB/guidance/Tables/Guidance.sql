CREATE TABLE [guidance].[Guidance] (
    [Guidance_ID]        BIGINT         IDENTITY (100, 1) NOT NULL,
    [Guidance_Video]     NVARCHAR (MAX) CONSTRAINT [DF__Guidance__Guidan__5629CD9C] DEFAULT ('') NOT NULL,
    [Guidance_AddedDate] DATETIME       CONSTRAINT [DF__Guidance__Guidan__571DF1D5] DEFAULT (CONVERT([varchar](10),getdate(),(103))) NOT NULL,
    [Guidance_Status]    BIT            CONSTRAINT [DF__Guidance__Guidan__5812160E] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK__Guidance__D7BD428054439957] PRIMARY KEY CLUSTERED ([Guidance_ID] ASC)
);

