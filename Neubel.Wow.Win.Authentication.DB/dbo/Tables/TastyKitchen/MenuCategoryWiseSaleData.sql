CREATE TABLE [dbo].[MenuCategoryWiseSaleData]
(
	[Id] BIGINT IDENTITY (1, 1) NOT NULL,
	[Name] [varchar](256) NULL,
	[Quantity] [decimal](18, 0) NOT NULL,
	[Amount] [decimal](18, 0) NOT NULL,
    [MenuCategoryWiseSaleReportId] BIGINT NOT NULL,
	[IsDeleted] [bit] CONSTRAINT [D_MenuCategoryWiseSaleData_IsDeleted] DEFAULT ((0)) NOT NULL,
	CONSTRAINT [PK_MenuCategoryWiseSaleData] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_MenuCategoryWiseSaleData_MenuCategoryWiseSaleReport] FOREIGN KEY (MenuCategoryWiseSaleReportId) REFERENCES [dbo].[MenuCategoryWiseSaleReport] ([Id])
);