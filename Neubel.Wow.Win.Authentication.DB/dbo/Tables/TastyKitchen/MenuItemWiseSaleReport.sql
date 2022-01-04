CREATE TABLE [dbo].[MenuItemWiseSaleReport]
(
	[Id] BIGINT IDENTITY (1, 1) NOT NULL,
	[Amount] [decimal](18, 0) NOT NULL,
	[Date] [datetime] NOT NULL,
	[IsDeleted] [bit] CONSTRAINT [D_MenuItemWiseSaleReport_IsDeleted] DEFAULT ((0)) NOT NULL,
	CONSTRAINT [PK_MenuItemWiseSaleReport] PRIMARY KEY CLUSTERED ([Id] ASC)
);