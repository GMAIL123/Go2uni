CREATE TABLE [studentinfo].[Online_Community_Members] (
    [Community_Member_ID]          BIGINT   IDENTITY (100, 1) NOT NULL,
    [Community_Members]            BIGINT   CONSTRAINT [DF__Online_Co__Commu__656C112C] DEFAULT ('') NOT NULL,
    [Community_FK_ID]              BIGINT   NULL,
    [Community_Member_CreatedDate] DATETIME CONSTRAINT [DF_Online_Community_Members_Community_Member_CreatedDate] DEFAULT (CONVERT([varchar](10),getdate(),(103))) NULL,
    [Community_Member_Status]      BIT      CONSTRAINT [DF_Online_Community_Members_Community_Member_Status] DEFAULT ((1)) NULL,
    CONSTRAINT [PK__Online_C__53A5173E8658DAF9] PRIMARY KEY CLUSTERED ([Community_Member_ID] ASC),
    CONSTRAINT [FK__Online_Co__Commu__0E6E26BF] FOREIGN KEY ([Community_FK_ID]) REFERENCES [studentinfo].[Online_Community] ([Community_ID]),
    CONSTRAINT [FK_Online_Community_Members_Student_Basic] FOREIGN KEY ([Community_Members]) REFERENCES [studentinfo].[Student_Basic] ([student_ID])
);

