CREATE TABLE [dbo].[PasswordLog] (
    [Id]           BIGINT        IDENTITY (1, 1) NOT NULL,
    [UserId]       BIGINT        NOT NULL,
    [PasswordHash] NVARCHAR (50) NOT NULL,
    [PasswordSalt] NVARCHAR (50) NOT NULL,
    [ChangeDate]   DATETIME      NOT NULL,
    CONSTRAINT [PK_PasswordLog] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_PasswordLog_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id]),
    CONSTRAINT [FK_PasswordLog_User1] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id])
);



