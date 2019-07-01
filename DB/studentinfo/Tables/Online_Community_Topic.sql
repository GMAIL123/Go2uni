CREATE TABLE [studentinfo].[Online_Community_Topic] (
    [Topic_ID]               BIGINT         IDENTITY (500, 1) NOT NULL,
    [studentCreatedBy_Fk_ID] BIGINT         NULL,
    [Community_Fk_ID]        BIGINT         NULL,
    [Topic_Discussion]       NVARCHAR (MAX) CONSTRAINT [DF__Online_Co__Topic__08162EEB] DEFAULT ('') NOT NULL,
    [Topic_CreatedDate]      DATETIME       CONSTRAINT [DF__Online_Co__Topic__090A5324] DEFAULT (getdate()) NOT NULL,
    [Topic_status]           BIT            CONSTRAINT [DF__Online_Co__Topic__09FE775D] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK__Online_C__8DEAA425307346C5] PRIMARY KEY CLUSTERED ([Topic_ID] ASC),
    CONSTRAINT [FK__Online_Co__Commu__07220AB2] FOREIGN KEY ([Community_Fk_ID]) REFERENCES [studentinfo].[Online_Community] ([Community_ID]),
    CONSTRAINT [FK__Online_Co__stude__062DE679] FOREIGN KEY ([studentCreatedBy_Fk_ID]) REFERENCES [studentinfo].[Student_Basic] ([student_ID])
);

