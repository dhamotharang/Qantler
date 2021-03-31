GO
/****** Object:  Table [dbo].[ChecklistCategory]    Script Date: 2020-11-19 11:07:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChecklistCategory](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Index] [smallint] NOT NULL,
	[Text] [nvarchar](4000) NOT NULL,
	[HistoryID] [bigint] NOT NULL,
 CONSTRAINT [PK_ChecklistCategory] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ChecklistHistory]    Script Date: 2020-11-19 11:07:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChecklistHistory](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Scheme] [smallint] NULL,
	[Version] [smallint] NOT NULL,
	[CreatedBy] [uniqueidentifier] NOT NULL,
	[EffectiveFrom] [datetime2](0) NOT NULL,
	[CreatedOn] [datetime2](0) NOT NULL,
	[ModifiedOn] [datetime2](0) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_ChecklistHistory] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ChecklistItem]    Script Date: 2020-11-19 11:07:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChecklistItem](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Index] [smallint] NOT NULL,
	[Text] [nvarchar](4000) NOT NULL,
	[CategoryID] [bigint] NOT NULL,
	[Notes] [nvarchar](4000) NULL,
 CONSTRAINT [PK_ChecklistItem] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cluster]    Script Date: 1/2/2021 11:36:08 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Cluster](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[District] [nvarchar](15) NOT NULL,
	[Locations] [nvarchar](500) NOT NULL,
	[Color] [nvarchar](50) NULL,
	[CreatedOn] [datetime2](0) NOT NULL,
	[ModifiedOn] [datetime2](0) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Cluster] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ClusterLog]    Script Date: 2020-11-19 11:07:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ClusterLog](
	[ClusterID] [bigint] NOT NULL,
	[LogID] [bigint] NOT NULL,
 CONSTRAINT [PK_ClusterLog] PRIMARY KEY CLUSTERED 
(
	[ClusterID] ASC,
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ContactInfo]    Script Date: 2020-11-19 11:07:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContactInfo](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Type] [smallint] NOT NULL,
	[Value] [nvarchar](100) NOT NULL,
	[IsPrimary] [bit] NOT NULL,
	[CreatedOn] [datetime2](0) NOT NULL,
	[ModifiedOn] [datetime2](0) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_ContactInfo] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ContactInfoTypeLookup]    Script Date: 2020-11-19 11:07:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContactInfoTypeLookup](
	[ID] [smallint] NOT NULL,
	[Text] [nvarchar](150) NULL,
 CONSTRAINT [PK_ContactInfoTypeLookup] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Credential]    Script Date: 2020-11-19 11:07:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Credential](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ProviderID] [bigint] NOT NULL,
	[Key] [nvarchar](255) NOT NULL,
	[Secret] [nvarchar](1000) NULL,
	[ExpiresOn] [datetime2](0) NULL,
	[IdentityID] [uniqueidentifier] NOT NULL,
	[IsTemporary] [bit] NOT NULL,
	[CreatedOn] [datetime2](0) NOT NULL,
	[ModifiedOn] [datetime2](0) NOT NULL,
	[IsDeleted] [bit] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 2020-11-19 11:07:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer](
	[ID] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](150) NULL,
	[IDType] [smallint] NULL,
	[AltID] [nvarchar](30) NULL,
	[Code] [varchar](30) NULL,
	[GroupCode] [varchar](30) NULL,
	[Status] [smallint] NOT NULL,
	[ParentID] [uniqueidentifier] NULL,
	[CreatedOn] [datetime2](0) NOT NULL,
	[ModifiedOn] [datetime2](0) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CustomerContacts]    Script Date: 2020-11-19 11:07:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomerContacts](
	[CustomerID] [uniqueidentifier] NOT NULL,
	[ContactID] [bigint] NOT NULL,
 CONSTRAINT [PK_CustomerContacts] PRIMARY KEY CLUSTERED 
(
	[CustomerID] ASC,
	[ContactID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CustomerPremises]    Script Date: 2020-11-19 11:07:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomerPremises](
	[CustomerID] [uniqueidentifier] NOT NULL,
	[PremiseID] [bigint] NOT NULL,
 CONSTRAINT [PK_CustomerPremises] PRIMARY KEY CLUSTERED 
(
	[CustomerID] ASC,
	[PremiseID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CustomerStatusLookup]    Script Date: 2020-11-19 11:07:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomerStatusLookup](
	[ID] [smallint] NOT NULL,
	[Text] [nvarchar](150) NULL,
 CONSTRAINT [PK_CustomerStatusLookup] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmailTemplate]    Script Date: 2020-11-19 11:07:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmailTemplate](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Type] [smallint] NOT NULL,
	[From] [varchar](255) NULL,
	[Cc] [varchar](255) NULL,
	[Bcc] [varchar](255) NULL,
	[Title] [nvarchar](65) NULL,
	[Body] [nvarchar](max) NOT NULL,
	[IsBodyHtml] [bit] NOT NULL,
	[Keyword] [nvarchar](500) NULL,
	[CreatedOn] [datetime2](0) NOT NULL,
	[ModifiedOn] [datetime2](0) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_EmailTemplate] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmailTemplateTypeLookup]    Script Date: 2020-11-19 11:07:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmailTemplateTypeLookup](
	[ID] [smallint] NOT NULL,
	[Text] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_EmailTemplateTypeLookup] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Identity]    Script Date: 2020-11-19 11:07:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Identity](
	[ID] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](150) NOT NULL,
	[Designation] [nvarchar](150) NOT NULL,
	[Role] [smallint] NOT NULL,
	[Permissions] [varchar](255) NOT NULL,
	[Email] [varchar](100) NOT NULL,
	[Status] [smallint] NOT NULL,
	[AccessFailCount] [smallint] NULL,
	[LockoutEnabled] [bit] NULL,
	[LockoutEndOn] [datetime2](0) NULL,
	[Sequence] [smallint] NULL,
	[CreatedOn] [datetime2](0) NULL,
	[ModifiedOn] [datetime2](0) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Identity] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IdentityClusters]    Script Date: 2020-11-19 11:07:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IdentityClusters](
	[IdentityID] [uniqueidentifier] NOT NULL,
	[ClusterID] [bigint] NOT NULL,
 CONSTRAINT [PK_IdentityClusters] PRIMARY KEY CLUSTERED 
(
	[IdentityID] ASC,
	[ClusterID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IdentityLogs]    Script Date: 2020-11-19 11:07:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IdentityLogs](
	[IdentityID] [uniqueidentifier] NOT NULL,
	[LogID] [bigint] NOT NULL,
 CONSTRAINT [PK_IdentityLog] PRIMARY KEY CLUSTERED 
(
	[IdentityID] ASC,
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IdentityRequestTypes]    Script Date: 2020-11-19 11:07:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IdentityRequestTypes](
	[IdentityID] [uniqueidentifier] NOT NULL,
	[RequestType] [smallint] NOT NULL,
 CONSTRAINT [PK_IdentityRequestTypes] PRIMARY KEY CLUSTERED 
(
	[IdentityID] ASC,
	[RequestType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IdentityStatusLookup]    Script Date: 2020-11-19 11:07:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IdentityStatusLookup](
	[ID] [smallint] NOT NULL,
	[Text] [nvarchar](150) NULL,
 CONSTRAINT [PK_IdentityStatusLookup] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IDTypeLookup]    Script Date: 2020-11-19 11:07:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IDTypeLookup](
	[ID] [smallint] NOT NULL,
	[Text] [nvarchar](150) NULL,
 CONSTRAINT [PK_IDTypeLookup] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Locale]    Script Date: 2020-11-19 11:07:19 AM ******/
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
GO
/****** Object:  Table [dbo].[Log]    Script Date: 2020-11-19 11:07:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Log](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Type] [smallint] NULL,
	[RefID] [varchar](36) NULL,
	[Action] [nvarchar](2000) NOT NULL,
	[Raw] [nvarchar](2000) NOT NULL,
	[Notes] [nvarchar](4000) NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[CreatedOn] [datetime2](0) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Log] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LogTypeLookup]    Script Date: 2020-11-19 11:07:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LogTypeLookup](
	[ID] [smallint] NOT NULL,
	[Text] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_LogTypeLookup] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Nodes]    Script Date: 2020-11-19 11:07:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Nodes](
	[ClusterID] [bigint] NOT NULL,
	[Node] [varchar](2) NOT NULL,
 CONSTRAINT [PK_Nodes] PRIMARY KEY CLUSTERED 
(
	[ClusterID] ASC,
	[Node] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Person]    Script Date: 2020-11-19 11:07:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Person](
	[ID] [uniqueidentifier] NOT NULL,
	[Salutation] [varchar](10) NULL,
	[Name] [nvarchar](150) NULL,
	[Nationality] [nvarchar](60) NULL,
	[Gender] [varchar](10) NULL,
	[DOB] [datetime2](0) NULL,
	[Designation] [nvarchar](100) NULL,
	[DesignationOther] [nvarchar](100) NULL,
	[IDType] [smallint] NULL,
	[AltID] [varchar](30) NULL,
	[PassportIssuingCountry] [varchar](100) NULL,
	[CreatedOn] [datetime2](0) NOT NULL,
	[ModifiedOn] [datetime2](0) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Person] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PersonContacts]    Script Date: 2020-11-19 11:07:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PersonContacts](
	[PersonID] [uniqueidentifier] NOT NULL,
	[ContactID] [bigint] NOT NULL,
 CONSTRAINT [PK_PersonContacts] PRIMARY KEY CLUSTERED 
(
	[PersonID] ASC,
	[ContactID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Premise]    Script Date: 2020-11-19 11:07:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Premise](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[IsLocal] [bit] NOT NULL,
	[Name] [nvarchar](150) NOT NULL,
	[Type] [smallint] NOT NULL,
	[Area] [varchar](15) NULL,
	[Schedule] [varchar](50) NULL,
	[BlockNo] [varchar](15) NULL,
	[UnitNo] [varchar](5) NULL,
	[FloorNo] [varchar](5) NULL,
	[BuildingName] [nvarchar](100) NULL,
	[Address1] [nvarchar](150) NULL,
	[Address2] [nvarchar](150) NULL,
	[City] [nvarchar](100) NULL,
	[Province] [nvarchar](100) NULL,
	[Country] [nvarchar](100) NULL,
	[Postal] [varchar](20) NULL,
	[Longitude] [decimal](9, 6) NULL,
	[Latitude] [decimal](9, 6) NULL,
	[IsPrimary] [bit] NOT NULL,
	[ChangeType] [smallint] NOT NULL,
	[CreatedOn] [datetime2](0) NOT NULL,
	[ModifiedOn] [datetime2](0) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CustomerID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Premise] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PremiseTypeLookup]    Script Date: 2020-11-19 11:07:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PremiseTypeLookup](
	[ID] [smallint] NOT NULL,
	[Text] [nvarchar](150) NULL,
 CONSTRAINT [PK_PremiseTypeLookup] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SchemeLookup]    Script Date: 2020-11-19 11:07:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SchemeLookup](
	[ID] [smallint] NOT NULL,
	[Text] [nvarchar](150) NULL,
 CONSTRAINT [PK_SchemeLookup] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Settings]    Script Date: 2020-11-19 11:07:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Settings](
	[ID] [bigint] NOT NULL,
	[Type] [smallint] NOT NULL,
	[Value] [nvarchar](4000) NULL,
	[Text] [nvarchar](255) NOT NULL,
	[DataType] [smallint] NOT NULL,
	[CreatedOn] [datetime2](0) NOT NULL,
	[ModifiedOn] [datetime2](0) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Settings] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SettingsLog]    Script Date: 2020-11-19 11:07:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SettingsLog](
	[SettingsID] [bigint] NOT NULL,
	[LogID] [bigint] NOT NULL,
 CONSTRAINT [PK_SettingsLog] PRIMARY KEY CLUSTERED 
(
	[SettingsID] ASC,
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SettingsLookup]    Script Date: 2020-11-19 11:07:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SettingsLookup](
	[ID] [smallint] NOT NULL,
	[Text] [nvarchar](150) NULL,
 CONSTRAINT [PK_SettingsLookup] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SubSchemeLookup]    Script Date: 2020-11-19 11:07:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SubSchemeLookup](
	[ID] [smallint] NOT NULL,
	[Text] [nvarchar](150) NULL,
 CONSTRAINT [PK_SubSchemeLookup] PRIMARY KEY CLUSTERED 
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

ALTER TABLE [dbo].[ChecklistHistory] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Cluster] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[ContactInfo] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Credential] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Customer] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Identity] ADD  CONSTRAINT [DF__Identity__IsDele__30E33A54]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Premise] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO

ALTER TABLE [dbo].[ChecklistCategory]  WITH CHECK ADD FOREIGN KEY([HistoryID])
REFERENCES [dbo].[ChecklistHistory] ([ID])
GO
ALTER TABLE [dbo].[ChecklistHistory]  WITH CHECK ADD FOREIGN KEY([Scheme])
REFERENCES [dbo].[SchemeLookup] ([ID])
GO
ALTER TABLE [dbo].[ChecklistItem]  WITH CHECK ADD FOREIGN KEY([CategoryID])
REFERENCES [dbo].[ChecklistCategory] ([ID])
GO
ALTER TABLE [dbo].[ClusterLog]  WITH CHECK ADD FOREIGN KEY([ClusterID])
REFERENCES [dbo].[Cluster] ([ID])
GO
ALTER TABLE [dbo].[ClusterLog]  WITH CHECK ADD FOREIGN KEY([LogID])
REFERENCES [dbo].[Log] ([ID])
GO
ALTER TABLE [dbo].[ContactInfo]  WITH CHECK ADD FOREIGN KEY([Type])
REFERENCES [dbo].[ContactInfoTypeLookup] ([ID])
GO
ALTER TABLE [dbo].[Customer]  WITH CHECK ADD FOREIGN KEY([IDType])
REFERENCES [dbo].[IDTypeLookup] ([ID])
GO
ALTER TABLE [dbo].[Customer]  WITH CHECK ADD FOREIGN KEY([Status])
REFERENCES [dbo].[CustomerStatusLookup] ([ID])
GO
ALTER TABLE [dbo].[CustomerContacts]  WITH CHECK ADD FOREIGN KEY([ContactID])
REFERENCES [dbo].[ContactInfo] ([ID])
GO
ALTER TABLE [dbo].[CustomerContacts]  WITH CHECK ADD FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Person] ([ID])
GO
ALTER TABLE [dbo].[CustomerPremises]  WITH CHECK ADD FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customer] ([ID])
GO
ALTER TABLE [dbo].[CustomerPremises]  WITH CHECK ADD FOREIGN KEY([PremiseID])
REFERENCES [dbo].[Premise] ([ID])
GO
ALTER TABLE [dbo].[EmailTemplate]  WITH CHECK ADD FOREIGN KEY([Type])
REFERENCES [dbo].[EmailTemplateTypeLookup] ([ID])
GO
ALTER TABLE [dbo].[Identity]  WITH CHECK ADD  CONSTRAINT [FK__Identity__Status__31D75E8D] FOREIGN KEY([Status])
REFERENCES [dbo].[IdentityStatusLookup] ([ID])
GO
ALTER TABLE [dbo].[Identity] CHECK CONSTRAINT [FK__Identity__Status__31D75E8D]
GO
ALTER TABLE [dbo].[IdentityClusters]  WITH CHECK ADD FOREIGN KEY([ClusterID])
REFERENCES [dbo].[Cluster] ([ID])
GO
ALTER TABLE [dbo].[IdentityClusters]  WITH CHECK ADD  CONSTRAINT [FK__IdentityC__Ident__38845C1C] FOREIGN KEY([IdentityID])
REFERENCES [dbo].[Identity] ([ID])
GO
ALTER TABLE [dbo].[IdentityClusters] CHECK CONSTRAINT [FK__IdentityC__Ident__38845C1C]
GO
ALTER TABLE [dbo].[IdentityLogs]  WITH CHECK ADD  CONSTRAINT [FK__IdentityL__Ident__3E3D3572] FOREIGN KEY([IdentityID])
REFERENCES [dbo].[Identity] ([ID])
GO
ALTER TABLE [dbo].[IdentityLogs] CHECK CONSTRAINT [FK__IdentityL__Ident__3E3D3572]
GO
ALTER TABLE [dbo].[IdentityLogs]  WITH CHECK ADD FOREIGN KEY([LogID])
REFERENCES [dbo].[Log] ([ID])
GO
ALTER TABLE [dbo].[IdentityRequestTypes]  WITH CHECK ADD  CONSTRAINT [FK__IdentityR__Ident__34B3CB38] FOREIGN KEY([IdentityID])
REFERENCES [dbo].[Identity] ([ID])
GO
ALTER TABLE [dbo].[IdentityRequestTypes] CHECK CONSTRAINT [FK__IdentityR__Ident__34B3CB38]
GO
ALTER TABLE [dbo].[Log]  WITH CHECK ADD FOREIGN KEY([Type])
REFERENCES [dbo].[LogTypeLookup] ([ID])
GO
ALTER TABLE [dbo].[Log]  WITH CHECK ADD FOREIGN KEY([Type])
REFERENCES [dbo].[LogTypeLookup] ([ID])
GO
ALTER TABLE [dbo].[Nodes]  WITH CHECK ADD FOREIGN KEY([ClusterID])
REFERENCES [dbo].[Cluster] ([ID])
GO
ALTER TABLE [dbo].[Person]  WITH CHECK ADD FOREIGN KEY([IDType])
REFERENCES [dbo].[IDTypeLookup] ([ID])
GO
ALTER TABLE [dbo].[PersonContacts]  WITH CHECK ADD FOREIGN KEY([ContactID])
REFERENCES [dbo].[ContactInfo] ([ID])
GO
ALTER TABLE [dbo].[Premise]  WITH CHECK ADD FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customer] ([ID])
GO
ALTER TABLE [dbo].[Premise]  WITH CHECK ADD FOREIGN KEY([Type])
REFERENCES [dbo].[PremiseTypeLookup] ([ID])
GO
ALTER TABLE [dbo].[Premise]  WITH CHECK ADD FOREIGN KEY([Type])
REFERENCES [dbo].[PremiseTypeLookup] ([ID])
GO
ALTER TABLE [dbo].[Settings]  WITH CHECK ADD FOREIGN KEY([Type])
REFERENCES [dbo].[SettingsLookup] ([ID])
GO
ALTER TABLE [dbo].[Settings]  WITH CHECK ADD FOREIGN KEY([Type])
REFERENCES [dbo].[SettingsLookup] ([ID])
GO
ALTER TABLE [dbo].[Settings]  WITH CHECK ADD FOREIGN KEY([Type])
REFERENCES [dbo].[SettingsLookup] ([ID])
GO
ALTER TABLE [dbo].[SettingsLog]  WITH CHECK ADD FOREIGN KEY([LogID])
REFERENCES [dbo].[Log] ([ID])
GO
ALTER TABLE [dbo].[SettingsLog]  WITH CHECK ADD FOREIGN KEY([LogID])
REFERENCES [dbo].[Log] ([ID])
GO
ALTER TABLE [dbo].[SettingsLog]  WITH CHECK ADD FOREIGN KEY([LogID])
REFERENCES [dbo].[Log] ([ID])
GO
ALTER TABLE [dbo].[SettingsLog]  WITH CHECK ADD FOREIGN KEY([SettingsID])
REFERENCES [dbo].[Settings] ([ID])
GO
ALTER TABLE [dbo].[SettingsLog]  WITH CHECK ADD FOREIGN KEY([SettingsID])
REFERENCES [dbo].[Settings] ([ID])
GO
ALTER TABLE [dbo].[SettingsLog]  WITH CHECK ADD FOREIGN KEY([SettingsID])
REFERENCES [dbo].[Settings] ([ID])
GO
ALTER TABLE [dbo].[Translations]  WITH CHECK ADD FOREIGN KEY([Locale])
REFERENCES [dbo].[Locale] ([ID])
GO
