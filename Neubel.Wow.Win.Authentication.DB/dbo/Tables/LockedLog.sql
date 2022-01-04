CREATE TABLE [dbo].[LockedLog] (
    [Id]         INT      IDENTITY (1, 1) NOT NULL,
    [LockedDate] DATETIME NOT NULL,
    [UserId]     BIGINT   NOT NULL,
    CONSTRAINT [PK_LockedLog] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_LockedLog_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id])
);



