CREATE TABLE [dbo].[CrawlDataJobList] (
    [Seq]          UNIQUEIDENTIFIER CONSTRAINT [DF_CrawlDataJobList_Seq] DEFAULT (newid()) NOT NULL,
    [JobInfo]      NVARCHAR (MAX)   NULL,
    [RegisterTime] DATETIME2 (7)    CONSTRAINT [DF_CrawlDataJobList_RegisterTime] DEFAULT (getutcdate()) NULL,
    [JobStatus]    NVARCHAR (100)   CONSTRAINT [DF_CrawlDataJobList_JobStatus] DEFAULT (N'not start') NULL,
    [StartTime]    DATETIME2 (7)    NULL,
    [EndTime]      DATETIME2 (7)    NULL
);

