CREATE TABLE [dbo].[MenuItemWiseSaleData]
(
	[Id] BIGINT IDENTITY (1, 1) NOT NULL,
	[Name] [varchar](256) NULL,
	[Amount] [decimal](18, 0) NOT NULL,
	[Quantity] [decimal](18, 0) NOT NULL,
    [MenuItemWiseSaleReportId] BIGINT NOT NULL,
	[IsDeleted] [bit] CONSTRAINT [D_MenuItemWiseSaleData_IsDeleted] DEFAULT ((0)) NOT NULL,
	CONSTRAINT [PK_MenuItemWiseSaleData] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_MenuItemWiseSaleData_MenuItemWiseSaleReport] FOREIGN KEY (MenuItemWiseSaleReportId) REFERENCES [dbo].[MenuItemWiseSaleReport] ([Id])
);
