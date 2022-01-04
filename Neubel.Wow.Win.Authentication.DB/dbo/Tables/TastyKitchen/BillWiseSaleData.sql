CREATE TABLE [dbo].[BillWiseSaleData]
(
	[Id] BIGINT IDENTITY (1, 1) NOT NULL,
	[BillNumber] [varchar](256) NULL,
	[Amount] [decimal](18, 0) NOT NULL,
    [BillWiseSaleReportId] BIGINT NOT NULL,
	[IsDeleted] [bit] CONSTRAINT [D_BillWiseSaleData_IsDeleted] DEFAULT ((0)) NOT NULL,
	CONSTRAINT [PK_BillWiseSaleData] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_BillWiseSaleData_BillWiseSaleReport] FOREIGN KEY (BillWiseSaleReportId) REFERENCES [dbo].[BillWiseSaleReport] ([Id])
);
