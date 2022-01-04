CREATE TABLE [dbo].[DailySale]
(
	[Id] BIGINT IDENTITY (1, 1) NOT NULL,
	[BillNumber] [varchar](256) NULL,
	[Type] [varchar](50) NOT NULL,
	[Amount] [decimal](18, 0) NOT NULL,
	[Date] [datetime] NOT NULL,
	[IsDeleted] [bit] CONSTRAINT [D_DailySale_IsDeleted] DEFAULT ((0)) NOT NULL,
	CONSTRAINT [PK_DailySale] PRIMARY KEY CLUSTERED ([Id] ASC)
);
