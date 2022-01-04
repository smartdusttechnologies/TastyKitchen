CREATE TABLE [dbo].[DailyExpense]
(
	[Id] BIGINT IDENTITY (1, 1) NOT NULL,
	[Name] [varchar](200) NOT NULL,
	[Quantity] [decimal](18, 0) NULL,
	[Unit] [varchar](50) NULL,
	[Amount] [decimal](18, 0) NOT NULL,
	[Date] [datetime] NOT NULL,
	[IsDeleted] [bit] CONSTRAINT [D_DailyExpense_IsDeleted] DEFAULT ((0)) NOT NULL,
	CONSTRAINT [PK_DailyExpense] PRIMARY KEY CLUSTERED ([Id] ASC)
)
