CREATE TABLE [dbo].[UserValidationOTP] (
    [Id]                   BIGINT        IDENTITY (1, 1) NOT NULL,
    [UserId]               BIGINT        NOT NULL,
    [OTP]                  NVARCHAR (50) NOT NULL,
    [OTPGeneratedTime]     DATETIME      NOT NULL,
    [OTPAuthenticatedTime] DATETIME      NOT NULL,
    [Status]               SMALLINT      NOT NULL,
    [Type]                 NVARCHAR (50) NOT NULL,
    [OrgId]                INT           NOT NULL,
    CONSTRAINT [PK_UserValidationOTP] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_UserValidationOTP_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id])
);



