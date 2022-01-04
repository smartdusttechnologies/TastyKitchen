CREATE TABLE [dbo].[MenuCategoryWiseSaleReport]
(
	[Id] BIGINT IDENTITY (1, 1) NOT NULL,
	[Amount] [decimal](18, 0) NOT NULL,
	[Date] [datetime] NOT NULL,
	[IsDeleted] [bit] CONSTRAINT [D_MenuCategoryWiseSaleReport_IsDeleted] DEFAULT ((0)) NOT NULL,
	CONSTRAINT [PK_MenuCategoryWiseSaleReport] PRIMARY KEY CLUSTERED ([Id] ASC)
);
