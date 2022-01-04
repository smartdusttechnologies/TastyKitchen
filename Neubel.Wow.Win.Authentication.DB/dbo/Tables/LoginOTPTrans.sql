CREATE TABLE [dbo].[LoginOTPTrans] (
    [Id]                   BIGINT        IDENTITY (1, 1) NOT NULL,
    [OTP]                  NVARCHAR (10) NOT NULL,
    [LoginLogId]           BIGINT        NOT NULL,
    [OTPGeneratedTime]     DATETIME      NOT NULL,
    [OTPExpiryTime]        DATETIME      NOT NULL,
    [OTPAuthenticatedTime] DATETIME      NOT NULL,
    [Status]               SMALLINT      NOT NULL,
    [UserId]               BIGINT        NOT NULL,
    [OrgId]                INT           NOT NULL,
    CONSTRAINT [PK_OTPTrans] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_LoginOTPTrans_Organization] FOREIGN KEY ([OrgId]) REFERENCES [dbo].[Organization] ([Id])
);

