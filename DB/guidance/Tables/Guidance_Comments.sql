CREATE TABLE [guidance].[Guidance_Comments] (
    [Guidance_Comments_ID]           BIGINT         IDENTITY (500, 1) NOT NULL,
    [Guidance_Comments_Text]         NVARCHAR (MAX) CONSTRAINT [DF__Guidance___Guida__59063A47] DEFAULT ('') NOT NULL,
    [Guidance_Comments_InsertedDate] DATETIME       CONSTRAINT [DF__Guidance___Guida__59FA5E80] DEFAULT (CONVERT([varchar](10),getdate(),(103))) NOT NULL,
    [Guidance_ID]                    BIGINT         NULL,
    CONSTRAINT [PK__Guidance__0A375B409F9D768F] PRIMARY KEY CLUSTERED ([Guidance_Comments_ID] ASC),
    CONSTRAINT [FK__Guidance___Guida__07C12930] FOREIGN KEY ([Guidance_ID]) REFERENCES [guidance].[Guidance] ([Guidance_ID])
);

