CREATE TABLE [test].[Online_Test_Question] (
    [Question_ID]            BIGINT          IDENTITY (1000, 1) NOT NULL,
    [Question_Question]      NVARCHAR (MAX)  CONSTRAINT [DF__Online_Te__Quest__40F9A68C] DEFAULT ('') NOT NULL,
    [Question_Answers]       NVARCHAR (MAX)  NULL,
    [Question_CorrectAnswer] NVARCHAR (1)    NULL,
    [Question_CreatedDate]   DATETIME        CONSTRAINT [DF__Online_Te__Quest__41EDCAC5] DEFAULT (CONVERT([varchar](10),getdate(),(103))) NOT NULL,
    [Question_ScoreMarks]    DECIMAL (10, 3) CONSTRAINT [DF__Online_Te__Quest__42E1EEFE] DEFAULT ((0)) NOT NULL,
    [Question_Status]        BIT             CONSTRAINT [DF__Online_Te__Quest__43D61337] DEFAULT ((1)) NULL,
    [QuestionModules_ID]     BIGINT          NULL,
    CONSTRAINT [FK__Online_Te__Quest__44CA3770] FOREIGN KEY ([QuestionModules_ID]) REFERENCES [test].[Online_Test_Modules] ([QuestionModules_ID])
);

