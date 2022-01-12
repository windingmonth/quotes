if object_id('QuotesRecord') is null  
	 begin
	 CREATE TABLE [dbo].[QuotesRecord](
		[Id] [uniqueidentifier] NOT NULL,
		[Symbol] [nvarchar](64) NULL,
		[LatestPrice] [nvarchar](64) NULL,
		[OpeningPrice] [nvarchar](64) NULL,
		[HighestPrice] [nvarchar](64) NULL,
		[LowestPrice] [nvarchar](64) NULL,
		[YesterdayPrice] [nvarchar](64) NULL,
		[QuoteChange] [nvarchar](64) NULL,
		[CreatedOn] [datetime] NULL,
		[CreatedBy] [nvarchar](64) NULL,
		[UpdatedOn] [datetime] NULL,
		[UpdatedBy] [nvarchar](64) NULL,
	 CONSTRAINT [PK_QuotesRecord] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	end

	--索引部分
	 if not exists (select 1 from sys.indexes where name='IX_QuotesRecord_Symbol')
	 begin
	 CREATE NONCLUSTERED INDEX [IX_QuotesRecord_Symbol] ON [dbo].[QuotesRecord]
	(
		[Symbol] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	 end