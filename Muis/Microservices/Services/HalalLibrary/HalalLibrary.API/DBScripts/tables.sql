/****** Object:  Table [dbo].[Ingredient]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ingredient](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Brand] [nvarchar](255) NULL,
	[RiskCategory] [smallint] NOT NULL DEFAULT(999),
	[Status] [smallint] NOT NULL DEFAULT(0),
	[SupplierID] [bigint] NULL,
	[CertifyingBodyID] [bigint] NULL,
	[VerifiedByID] [uniqueidentifier] NULL,
	[CreatedOn] [datetime2](0) NOT NULL,
	[ModifiedOn] [datetime2](0) NOT NULL,
	[IsDeleted] [bit] NOT NULL DEFAULT(0)
 CONSTRAINT [PK_Ingredient] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Supplier]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Supplier](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[CreatedOn] [datetime2](0) NOT NULL,
	[ModifiedOn] [datetime2](0) NOT NULL,
	[IsDeleted] [bit] NOT NULL DEFAULT(0)
 CONSTRAINT [PK_Supplier] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CertifyingBody]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CertifyingBody](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Status] [smallint] NOT NULL DEFAULT(0),
	[CreatedOn] [datetime2](0) NOT NULL,
	[ModifiedOn] [datetime2](0) NOT NULL,
	[IsDeleted] [bit] NOT NULL DEFAULT(0)
 CONSTRAINT [PK_CertifyingBody] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IngredientStatusLookup]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IngredientStatusLookup](
	[ID] [smallint] NOT NULL,
	[Text] [nvarchar](150) NULL,
 CONSTRAINT [PK_IngredientStatusLookup] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RiskCategoryLookup]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RiskCategoryLookup](
	[ID] [smallint] NOT NULL,
	[Text] [nvarchar](150) NULL,
 CONSTRAINT [PK_RiskCategoryLookup] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CertifyingBodyStatusLookup]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CertifyingBodyStatusLookup](
	[ID] [smallint] NOT NULL,
	[Text] [nvarchar](150) NULL,
 CONSTRAINT [PK_CertifyingBodyStatusLookup] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Officer]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Officer](
	[ID] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](150) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Officer] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Translations]    Script Date: 2020-11-19 11:07:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Translations](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Locale] [smallint] NOT NULL,
	[Key] [varchar](60) NOT NULL,
	[Text] [nvarchar](2000) NOT NULL,
 CONSTRAINT [PK_Translations] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

  /****** Object:  Table [dbo].[Locale]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Locale](
	[ID] [smallint] NOT NULL,
	[Text] [nvarchar](30) NULL,
	[Code] [varchar](5) NOT NULL,
 CONSTRAINT [PK_Locale] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[Ingredient]  WITH CHECK ADD FOREIGN KEY([RiskCategory])
REFERENCES [dbo].[RiskCategoryLookup] ([ID])
GO
ALTER TABLE [dbo].[Ingredient]  WITH CHECK ADD FOREIGN KEY([Status])
REFERENCES [dbo].[IngredientStatusLookup] ([ID])
GO
ALTER TABLE [dbo].[Ingredient]  WITH CHECK ADD FOREIGN KEY([SupplierID])
REFERENCES [dbo].[Supplier] ([ID])
GO
ALTER TABLE [dbo].[Ingredient]  WITH CHECK ADD FOREIGN KEY([CertifyingBodyID])
REFERENCES [dbo].[CertifyingBody] ([ID])
GO
ALTER TABLE [dbo].[Ingredient]  WITH CHECK ADD FOREIGN KEY([VerifiedByID])
REFERENCES [dbo].[Officer] ([ID])
GO

ALTER TABLE [dbo].[CertifyingBody]  WITH CHECK ADD FOREIGN KEY([Status])
REFERENCES [dbo].[CertifyingBodyStatusLookup] ([ID])
GO

ALTER TABLE [dbo].[Translations]  WITH CHECK ADD FOREIGN KEY([Locale])
REFERENCES [dbo].[Locale] ([ID])
GO