USE [PROG260FA23]
GO

/****** Object:  Table [dbo].[Produce]    Script Date: 10/25/2023 7:28:56 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Produce]') AND type in (N'U'))
DROP TABLE [dbo].[Produce]
GO

/****** Object:  Table [dbo].[Produce]    Script Date: 10/25/2023 7:28:56 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Produce](
	[Name] [nvarchar](50) NOT NULL,
	[Location] [nvarchar](50) NOT NULL,
	[Price] [float] NOT NULL,
	[UoM] [nvarchar](50) NOT NULL,
	[Sell_by_Date] [nvarchar](50) NOT NULL
) ON [PRIMARY]
GO

