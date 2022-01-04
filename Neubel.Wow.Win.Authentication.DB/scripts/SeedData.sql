
IF NOT EXISTS (SELECT 1 FROM [Organization] WHERE Id = 0)
BEGIN
    SET IDENTITY_INSERT [dbo].[Organization]  ON

    INSERT INTO [dbo].[Organization]
               ([Id]
		       ,[OrgCode]
               ,[OrgName]
               ,[IsDeleted])
         VALUES
               (0,'SYSORG','SYSORG', 0)

    SET IDENTITY_INSERT [dbo].[Organization]  OFF
END
GO

IF NOT EXISTS (SELECT 1 FROM [Role] WHERE Id = 0)
BEGIN
    SET IDENTITY_INSERT [dbo].[Role]  ON

    INSERT INTO [dbo].[Role]
               ([Id]
		       ,[Name]
               ,[Level]
               ,[IsDeleted])
         VALUES
               (0, 'Sysadmin', 0 , 0),
		       (1, 'Admin', 1 , 0),
		       (2, 'ApplicationAdmin', 2 , 0),
		       (2, 'Manager', 3 , 0),
		       (3, 'GeneralUser', 6 , 0)

    SET IDENTITY_INSERT [dbo].[Role]  OFF
END
GO

IF NOT EXISTS (SELECT 1 FROM [User] WHERE Id = 0)
BEGIN
    SET IDENTITY_INSERT [dbo].[User]  ON

    INSERT INTO [dbo].[User]
               ([Id]
		       ,[UserName]
               ,[FirstName]
               ,[LastName]
               ,[Email]
               ,[Mobile]
               ,[Country]
               ,[ISDCode]
               ,[TwoFactor]
               ,[Locked]
               ,[IsActive]
               ,[EmailValidationStatus]
               ,[MobileValidationStatus]
               ,[OrgId]
               ,[AdminLevel]
               ,[IsDeleted])
         VALUES
               (0, 'sysadmin','sysadmin','sysadmin','sysadmin@gmail.com','1234567899','INDIA','91', 0, 0, 1, 0, 0, 0, 0, 0)

    SET IDENTITY_INSERT [dbo].[User]  OFF
END
GO

IF NOT EXISTS (SELECT 1 FROM [PasswordPolicy] WHERE Id = 0)
BEGIN
    SET IDENTITY_INSERT [dbo].[PasswordPolicy]  ON

    INSERT INTO [dbo].[PasswordPolicy]
               ([Id]
		       ,[MinCaps]
               ,[MinSmallChars]
               ,[MinSpecialChars]
               ,[MinNumber]
               ,[MinLength]
               ,[AllowUserName]
               ,[DisAllPastPassword]
               ,[DisAllowedChars]
               ,[ChangeIntervalDays]
               ,[OrgId]
               ,[IsDeleted])
         VALUES
               (0, 1, 1, 1, 1, 8, 1, 0 , null, 30, 0, 0)

    SET IDENTITY_INSERT [dbo].[PasswordPolicy]  OFF
END
GO

IF NOT EXISTS (SELECT 1 FROM [PasswordLogin] WHERE Id = 0)
BEGIN
    SET IDENTITY_INSERT [dbo].[PasswordLogin]  ON

    INSERT INTO [dbo].[PasswordLogin]
               ([Id]
		       ,[PasswordHash]
               ,[PasswordSalt]
               ,[UserId]
               ,[ChangeDate])
         VALUES
               --(0, 'Fz3VkUWQRP+QLYvmMilCjoz5FMRHQM8OEJxzvxwpycI=', '4XgQuK7k1tMBmh3X46N9qQ==', 0, GetDate())
			   (0, 'qnVDMZYlsGjs4chNs1/qPidI70eDUZ1fzUF5EdCqdl0=', 'NDlzcm0GY1GqMgn+urXX9Q==', 0, GetDate())

    SET IDENTITY_INSERT [dbo].[PasswordLogin]  OFF
END
GO

IF NOT EXISTS (SELECT 1 FROM [UserRole] WHERE Id = 0)
BEGIN
    SET IDENTITY_INSERT [dbo].[UserRole]  ON

    INSERT INTO [dbo].[UserRole]
               ([Id]
		       ,[UserId]
               ,[RoleId]
               ,[IsDeleted])
         VALUES
               (0, 0, 0, 0)

    SET IDENTITY_INSERT [dbo].[UserRole]  OFF
END
GO



