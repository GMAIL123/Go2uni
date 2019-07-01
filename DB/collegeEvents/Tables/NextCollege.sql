CREATE TABLE [collegeEvents].[NextCollege] (
    [NxtCollege_ID]        BIGINT         IDENTITY (10, 1) NOT NULL,
    [NxtCollege_Video]     NVARCHAR (MAX) CONSTRAINT [DF__NextColle__NxtCo__4F7CD00D] DEFAULT ('') NOT NULL,
    [NxtCollege_AddedDate] DATETIME       CONSTRAINT [DF__NextColle__NxtCo__5070F446] DEFAULT (CONVERT([varchar](10),getdate(),(103))) NOT NULL,
    [NxtCollege_Status]    BIT            CONSTRAINT [DF__NextColle__NxtCo__5165187F] DEFAULT ((1)) NULL,
    CONSTRAINT [PK__NextColl__59E291E4C1F41FB2] PRIMARY KEY CLUSTERED ([NxtCollege_ID] ASC)
);

