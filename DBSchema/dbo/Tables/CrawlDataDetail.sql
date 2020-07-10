CREATE TABLE [dbo].[CrawlDataDetail] (
    [Seq]        UNIQUEIDENTIFIER NULL,
    [DetailData] NVARCHAR (MAX)   NULL,
    [JobStatus]  NVARCHAR (100)   NULL,
    [EndTime]    DATETIME2 (7)    CONSTRAINT [DF_CrawlDataDetail_EndTime] DEFAULT (getutcdate()) NULL
);

