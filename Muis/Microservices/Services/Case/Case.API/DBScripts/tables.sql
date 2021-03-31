/****** Object:  UserDefinedTableType [dbo].[BigIntType]    Script Date: 2/2/2021 9:50:19 AM ******/
CREATE TYPE [dbo].[BigIntType] AS TABLE(
	[Val] [bigint] NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[UniqueIdentifierType]    Script Date: 2/2/2021 9:50:19 AM ******/
CREATE TYPE [dbo].[UniqueIdentifierType] AS TABLE(
	[Val] [uniqueidentifier] NULL
)
GO
/****** Object:  Table [dbo].[Log]    Script Date: 2020-11-19 11:07:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Activity](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Type] [smallint] NULL,
	[RefID] [varchar](36) NULL,
	[Action] [nvarchar](2000) NOT NULL,
	[Notes] [nvarchar](4000) NULL,
    [CaseID] [bigint] NOT NULL,
	[UserId] [uniqueidentifier] NULL,
	[CreatedOn] [datetime2](0) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Activity] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Case]    Script Date: 2020-11-19 11:07:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Case](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[RefID] [nvarchar](36) NULL,
	[Source] [smallint] NOT NULL,
  [Status] [smallint] NOT NULL,
  [StatusMinor] [smallint] NULL,
  [OldStatus] [smallint] NULL,
	[OtherStatus] [smallint] NULL,
  [OtherStatusMinor] [smallint] NULL,
  [Type] [smallint] NULL,
  [Occurrence] [smallint] NOT NULL DEFAULT 0,
  [Background] [nvarchar](MAX) NOT NULL,
  [OffenderID] [uniqueidentifier] NULL,
  [ManagedByID] [uniqueidentifier] NULL,
  [AssignedToID] [uniqueidentifier] NULL,
  [ReportedByID] [uniqueidentifier] NULL,
  [ReportedOn] [datetime2](0) NOT NULL,
  [SuspendedFrom] [datetime2](0) NOT NULL,
  [CreatedByID] [uniqueidentifier] NOT NULL,
	[CreatedOn] [datetime2](0) NOT NULL,
	[ModifiedOn] [datetime2](0) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[Sanction] [smallint] NULL,
	[Title] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_Case] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Certificate]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Certificate](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Number] [varchar](60) NOT NULL,
	[Scheme] [smallint] NOT NULL,
	[SubScheme] [smallint] NULL,
	[IssuedOn] [datetime2](0) NULL,
	[StartsFrom] [datetime2](0) NULL,
	[ExpiresOn] [datetime2](0) NOT NULL,
	[SuspendedUntil] [datetime2](0) NULL,
	[SerialNo] [varchar](36) NULL,
	[Status] [smallint] NOT NULL,
	[PremiseID] [bigint] NOT NULL,
    [CaseID] [bigint] NOT NULL,
	[CreatedOn] [datetime2](0) NULL,
	[ModifiedOn] [datetime2](0) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Certificate] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
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
	[CreatedOn] [datetime2](0) NOT NULL,
	[ModifiedOn] [datetime2](0) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_ContactInfo] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Letter]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Letter](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
  [Type] [smallint] NOT NULL,
	[Status] [smallint] NOT NULL,
	[Body] [nvarchar](max) NULL,
  [EmailID] [bigint] NULL,
	[CreatedOn] [datetime2](0) NOT NULL,
	[ModifiedOn] [datetime2](0) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Letter] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LetterTemplate]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LetterTemplate](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
    [Type] [smallint] NOT NULL,
	[Body] [nvarchar](max) NULL,
    [Keyword] [nvarchar](500) NULL,
	[CreatedOn] [datetime2](0) NOT NULL,
	[ModifiedOn] [datetime2](0) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_LetterTemplate] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Master]    Script Date: 2020-11-19 11:26:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Master](
	[ID] [uniqueidentifier] NOT NULL,
	[Type] [smallint] NOT NULL,
	[Value] [nvarchar](255) NOT NULL,
	[ParentID] [uniqueidentifier] NULL,
	[CreatedOn] [datetime2](0) NOT NULL,
	[ModifiedOn] [datetime2](0) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Master] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Offender]    Script Date: 2020-11-19 11:26:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Offender](
	[ID] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](150) NULL
 CONSTRAINT [PK_Offender] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
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
	[IDType] [smallint] NULL,
	[AltID] [varchar](30) NULL,
	[CreatedOn] [datetime2](0) NOT NULL,
	[ModifiedOn] [datetime2](0) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Person] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Premise]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Premise](
	[ID] [bigint] NOT NULL,
	[IsLocal] [bit] NOT NULL,
	[Name] [nvarchar](150) NULL,
	[Type] [smallint] NOT NULL,
	[Area] [float] NULL,
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
	[CreatedOn] [datetime2](0) NOT NULL,
	[ModifiedOn] [datetime2](0) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Premise] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SanctionInfo]    Script Date: 2020-11-19 11:26:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SanctionInfo](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Type] [smallint] NOT NULL,
	[Sanction] [smallint] NOT NULL,
    [Value] [varchar](36) NULL,
	[CaseID] [bigint] NOT NULL,
	[CreatedOn] [datetime2](0) NOT NULL,
	[ModifiedOn] [datetime2](0) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_SanctionInfo] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Officer]    Script Date: 2020-11-19 11:26:33 AM ******/
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
/****** Object:  Table [dbo].[Attachment]    Script Date: 2020-11-19 11:26:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Attachment](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[FileID] [uniqueidentifier] NOT NULL,
	[FileName] [nvarchar](150) NOT NULL,
	[Extension] [nvarchar](30) NOT NULL,
	[Size] [bigint] NOT NULL,
	[CreatedOn] [datetime2](0) NOT NULL,
	[ModifiedOn] [datetime2](0) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Attachment] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Email]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Email](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[From] [varchar](255) NULL,
	[To] [varchar](255) NULL,
	[Cc] [varchar](255) NULL,
	[Bcc] [varchar](255) NULL,
	[Title] [nvarchar](65) NULL,
	[Body] [nvarchar](max) NULL,
	[IsBodyHtml] [bit] NOT NULL,
	[CreatedOn] [datetime2](0) NOT NULL,
	[ModifiedOn] [datetime2](0) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Email] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmailTemplate]    Script Date: 2020-11-19 11:58:07 AM ******/
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
/****** Object:  Table [dbo].[EmailTemplateTypeLookup]    Script Date: 2020-11-19 11:58:07 AM ******/
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
/****** Object:  Table [dbo].[CaseStatusLookup]    Script Date: 2020-11-19 11:07:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CaseStatusLookup](
	[ID] [smallint] NOT NULL,
	[Text] [nvarchar](150) NULL,
 CONSTRAINT [PK_CaseStatusLookup] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CaseStatusMinorLookup]    Script Date: 2020-11-19 11:07:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CaseStatusMinorLookup](
	[ID] [smallint] NOT NULL,
	[Text] [nvarchar](150) NULL,
 CONSTRAINT [PK_CaseStatusMinorLookup] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SourceLookup]    Script Date: 2020-11-19 11:07:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SourceLookup](
	[ID] [smallint] NOT NULL,
	[Text] [nvarchar](150) NULL,
 CONSTRAINT [PK_SourceLookup] PRIMARY KEY CLUSTERED 
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
/****** Object:  Table [dbo].[MasterTypeLookup]    Script Date: 2020-11-19 11:26:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MasterTypeLookup](
	[ID] [smallint] NOT NULL,
	[Text] [nvarchar](150) NULL,
 CONSTRAINT [PK_MasterTypeLookup] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SchemeLookup]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SchemeLookup](
	[ID] [smallint] NOT NULL,
	[Text] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_SchemeLookup] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SubSchemeLookup]    Script Date: 2020-11-19 11:58:07 AM ******/
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
/****** Object:  Table [dbo].[SubSchemeLookup]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SanctionLookup](
	[ID] [smallint] NOT NULL,
	[Text] [nvarchar](150) NULL,
 CONSTRAINT [PK_SanctionLookup] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SubSchemeLookup]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SanctionInfoTypeLookup](
	[ID] [smallint] NOT NULL,
	[Text] [nvarchar](150) NULL,
 CONSTRAINT [PK_SanctionInfoTypeLookup] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ActivityTypeLookup]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ActivityTypeLookup](
	[ID] [smallint] NOT NULL,
	[Text] [nvarchar](150) NULL,
 CONSTRAINT [PK_ActivityTypeLookup] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LetterTypeLookup]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LetterTypeLookup](
	[ID] [smallint] NOT NULL,
	[Text] [nvarchar](150) NULL,
 CONSTRAINT [PK_LetterTypeLookup] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LetterStatusLookup]    Script Date: 2021-02-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LetterStatusLookup](
	[ID] [smallint] NOT NULL,
	[Text] [nvarchar](150) NULL,
 CONSTRAINT [PK_LetterStatusLookup] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ActivityLetters]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ActivityLetters](
	[ActivityID] [bigint] NOT NULL,
	[LetterID] [bigint] NOT NULL,
 CONSTRAINT [PK_ActivityLetters] PRIMARY KEY CLUSTERED 
(
	[ActivityID] ASC,
    [LetterID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ActivityAttachments]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ActivityAttachments](
	[ActivityID] [bigint] NOT NULL,
	[AttachmentID] [bigint] NOT NULL,
 CONSTRAINT [PK_ActivityAttachments] PRIMARY KEY CLUSTERED 
(
	[ActivityID] ASC,
    [AttachmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ActivityEmails]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ActivityEmails](
	[ActivityID] [bigint] NOT NULL,
	[EmailID] [bigint] NOT NULL,
 CONSTRAINT [PK_ActivityEmails] PRIMARY KEY CLUSTERED 
(
	[ActivityID] ASC,
    [EmailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CaseBreachCategories]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CaseBreachCategories](
	[CaseID] [bigint] NOT NULL,
	[MasterID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_CaseBreachCategories] PRIMARY KEY CLUSTERED 
(
	[CaseID] ASC,
    [MasterID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CaseOffences]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CaseOffences](
	[CaseID] [bigint] NOT NULL,
	[MasterID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_CaseOffences] PRIMARY KEY CLUSTERED 
(
	[CaseID] ASC,
    [MasterID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CaseAttachments]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CaseAttachments](
	[CaseID] [bigint] NOT NULL,
	[AttachmentID] [bigint] NOT NULL,
 CONSTRAINT [PK_CaseAttachments] PRIMARY KEY CLUSTERED 
(
	[CaseID] ASC,
    [AttachmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CasePremises]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CasePremises](
	[CaseID] [bigint] NOT NULL,
	[PremiseID] [bigint] NOT NULL,
 CONSTRAINT [PK_CasePremises] PRIMARY KEY CLUSTERED 
(
	[CaseID] ASC,
    [PremiseID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CaseSanctionInfos]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CaseSanctionInfos](
	[CaseID] [bigint] NOT NULL,
	[SanctionInfoID] [bigint] NOT NULL,
 CONSTRAINT [PK_CaseSanctionInfos] PRIMARY KEY CLUSTERED 
(
	[CaseID] ASC,
    [SanctionInfoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OffenderContactInfos]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OffenderContactInfos](
	[OffenderID] [uniqueidentifier] NOT NULL,
	[ContactInfoID] [bigint] NOT NULL,
 CONSTRAINT [PK_OffenderContactInfos] PRIMARY KEY CLUSTERED 
(
	[OffenderID] ASC,
    [ContactInfoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PersonContactInfos]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PersonContactInfos](
	[PersonID] [uniqueidentifier] NOT NULL,
	[ContactInfoID] [bigint] NOT NULL,
 CONSTRAINT [PK_PersonContactInfos] PRIMARY KEY CLUSTERED 
(
	[PersonID] ASC,
    [ContactInfoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CaseTypeLookup]    Script Date: 2/2/2021 9:50:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CaseTypeLookup](
	[ID] [smallint] NOT NULL,
	[Text] [nvarchar](150) NULL,
 CONSTRAINT [PK_CaseTypeLookup] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
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
GO
/****** Object:  Table [dbo].[Translations]    Script Date: 2020-11-19 11:58:07 AM ******/
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
/****** Object:  Table [dbo].[CertificateStatusLookup]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CertificateStatusLookup](
	[ID] [smallint] NOT NULL,
	[Text] [nvarchar](150) NULL,
 CONSTRAINT [PK_CertificateStatusLookup] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PaymentStatusLookup]    Script Date: 2021-08-03 10:39:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PaymentStatusLookup](
	[ID] [smallint] NOT NULL,
	[Text] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_PaymentStatusLookup] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BillTypeLookup]    Script Date: 2020-11-19 10:39:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BillTypeLookup](
	[ID] [smallint] NOT NULL,
	[Text] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_BillTypeLookup] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Translations]  WITH CHECK ADD FOREIGN KEY([Locale])
REFERENCES [dbo].[Locale] ([ID])
GO
ALTER TABLE [dbo].[Activity]  WITH CHECK ADD FOREIGN KEY([Type])
REFERENCES [dbo].[ActivityTypeLookup] ([ID])
GO
ALTER TABLE [dbo].[Activity]  WITH CHECK ADD FOREIGN KEY([CaseID])
REFERENCES [dbo].[Case] ([ID])
GO
ALTER TABLE [dbo].[Activity]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[Officer] ([ID])
GO
ALTER TABLE [dbo].[ActivityLetters]  WITH CHECK ADD FOREIGN KEY([ActivityID])
REFERENCES [dbo].[Activity] ([ID])
GO
ALTER TABLE [dbo].[ActivityLetters]  WITH CHECK ADD FOREIGN KEY([LetterID])
REFERENCES [dbo].[Letter] ([ID])
GO
ALTER TABLE [dbo].[ActivityAttachments]  WITH CHECK ADD FOREIGN KEY([ActivityID])
REFERENCES [dbo].[Activity] ([ID])
GO
ALTER TABLE [dbo].[ActivityAttachments]  WITH CHECK ADD FOREIGN KEY([AttachmentID])
REFERENCES [dbo].[Attachment] ([ID])
GO
ALTER TABLE [dbo].[ActivityEmails]  WITH CHECK ADD FOREIGN KEY([ActivityID])
REFERENCES [dbo].[Activity] ([ID])
GO
ALTER TABLE [dbo].[ActivityEmails]  WITH CHECK ADD FOREIGN KEY([EmailID])
REFERENCES [dbo].[Email] ([ID])
GO
ALTER TABLE [dbo].[Case]  WITH CHECK ADD FOREIGN KEY([Source])
REFERENCES [dbo].[SourceLookup] ([ID])
GO
ALTER TABLE [dbo].[Case]  WITH CHECK ADD FOREIGN KEY([Status])
REFERENCES [dbo].[CaseStatusLookup] ([ID])
GO
ALTER TABLE [dbo].[Case]  WITH CHECK ADD FOREIGN KEY([OldStatus])
REFERENCES [dbo].[CaseStatusLookup] ([ID])
GO
ALTER TABLE [dbo].[Case]  WITH CHECK ADD FOREIGN KEY([StatusMinor])
REFERENCES [dbo].[CaseStatusMinorLookup] ([ID])
GO
ALTER TABLE [dbo].[Case]  WITH CHECK ADD FOREIGN KEY([OtherStatus])
REFERENCES [dbo].[CaseStatusLookup] ([ID])
GO
ALTER TABLE [dbo].[Case]  WITH CHECK ADD FOREIGN KEY([OtherStatusMinor])
REFERENCES [dbo].[CaseStatusMinorLookup] ([ID])
GO
ALTER TABLE [dbo].[Case]  WITH CHECK ADD FOREIGN KEY([OffenderID])
REFERENCES [dbo].[Offender] ([ID])
GO
ALTER TABLE [dbo].[Case]  WITH CHECK ADD FOREIGN KEY([ManagedByID])
REFERENCES [dbo].[Officer] ([ID])
GO
ALTER TABLE [dbo].[Case]  WITH CHECK ADD FOREIGN KEY([AssignedToID])
REFERENCES [dbo].[Officer] ([ID])
GO
ALTER TABLE [dbo].[Case]  WITH CHECK ADD FOREIGN KEY([CreatedByID])
REFERENCES [dbo].[Officer] ([ID])
GO
ALTER TABLE [dbo].[Case]  WITH CHECK ADD FOREIGN KEY([Sanction])
REFERENCES [dbo].[SanctionLookup] ([ID])
GO
ALTER TABLE [dbo].[CaseBreachCategories]  WITH CHECK ADD FOREIGN KEY([CaseID])
REFERENCES [dbo].[Case] ([ID])
GO
ALTER TABLE [dbo].[CaseBreachCategories]  WITH CHECK ADD FOREIGN KEY([MasterID])
REFERENCES [dbo].[Master] ([ID])
GO
ALTER TABLE [dbo].[CaseOffences]  WITH CHECK ADD FOREIGN KEY([CaseID])
REFERENCES [dbo].[Case] ([ID])
GO
ALTER TABLE [dbo].[CaseOffences]  WITH CHECK ADD FOREIGN KEY([MasterID])
REFERENCES [dbo].[Master] ([ID])
GO
ALTER TABLE [dbo].[CaseAttachments]  WITH CHECK ADD FOREIGN KEY([CaseID])
REFERENCES [dbo].[Case] ([ID])
GO
ALTER TABLE [dbo].[CaseAttachments]  WITH CHECK ADD FOREIGN KEY([AttachmentID])
REFERENCES [dbo].[Attachment] ([ID])
GO
ALTER TABLE [dbo].[CasePremises]  WITH CHECK ADD FOREIGN KEY([CaseID])
REFERENCES [dbo].[Case] ([ID])
GO
ALTER TABLE [dbo].[CasePremises]  WITH CHECK ADD FOREIGN KEY([PremiseID])
REFERENCES [dbo].[Premise] ([ID])
GO
ALTER TABLE [dbo].[CaseSanctionInfos]  WITH CHECK ADD FOREIGN KEY([CaseID])
REFERENCES [dbo].[Case] ([ID])
GO
ALTER TABLE [dbo].[CaseSanctionInfos]  WITH CHECK ADD FOREIGN KEY([SanctionInfoID])
REFERENCES [dbo].[SanctionInfo] ([ID])
GO
ALTER TABLE [dbo].[Certificate]  WITH CHECK ADD FOREIGN KEY([Scheme])
REFERENCES [dbo].[SchemeLookup] ([ID])
GO
ALTER TABLE [dbo].[Certificate]  WITH CHECK ADD FOREIGN KEY([SubScheme])
REFERENCES [dbo].[SubSchemeLookup] ([ID])
GO
ALTER TABLE [dbo].[Certificate]  WITH CHECK ADD FOREIGN KEY([PremiseID])
REFERENCES [dbo].[Premise] ([ID])
GO
ALTER TABLE [dbo].[Certificate]  WITH CHECK ADD FOREIGN KEY([CaseID])
REFERENCES [dbo].[Case] ([ID])
GO
ALTER TABLE [dbo].[ContactInfo]  WITH CHECK ADD FOREIGN KEY([Type])
REFERENCES [dbo].[ContactInfoTypeLookup] ([ID])
GO
ALTER TABLE [dbo].[EmailTemplate]  WITH CHECK ADD FOREIGN KEY([Type])
REFERENCES [dbo].[EmailTemplateTypeLookup] ([ID])
GO
ALTER TABLE [dbo].[Letter]  WITH CHECK ADD FOREIGN KEY([Type])
REFERENCES [dbo].[LetterTypeLookup] ([ID])
GO
ALTER TABLE [dbo].[Letter]  WITH CHECK ADD FOREIGN KEY([Status])
REFERENCES [dbo].[LetterStatusLookup] ([ID])
GO
ALTER TABLE [dbo].[LetterTemplate]  WITH CHECK ADD FOREIGN KEY([Type])
REFERENCES [dbo].[LetterTypeLookup] ([ID])
GO
ALTER TABLE [dbo].[Master]  WITH CHECK ADD FOREIGN KEY([Type])
REFERENCES [dbo].[MasterTypeLookup] ([ID])
GO
ALTER TABLE [dbo].[OffenderContactInfos]  WITH CHECK ADD FOREIGN KEY([OffenderID])
REFERENCES [dbo].[Offender] ([ID])
GO
ALTER TABLE [dbo].[OffenderContactInfos]  WITH CHECK ADD FOREIGN KEY([ContactInfoID])
REFERENCES [dbo].[ContactInfo] ([ID])
GO
ALTER TABLE [dbo].[PersonContactInfos]  WITH CHECK ADD FOREIGN KEY([PersonID])
REFERENCES [dbo].[Person] ([ID])
GO
ALTER TABLE [dbo].[PersonContactInfos]  WITH CHECK ADD FOREIGN KEY([ContactInfoID])
REFERENCES [dbo].[ContactInfo] ([ID])
GO
ALTER TABLE [dbo].[SanctionInfo]  WITH CHECK ADD FOREIGN KEY([Type])
REFERENCES [dbo].[SanctionInfoTypeLookup] ([ID])
GO
ALTER TABLE [dbo].[SanctionInfo]  WITH CHECK ADD FOREIGN KEY([Sanction])
REFERENCES [dbo].[SanctionLookup] ([ID])
GO
ALTER TABLE [dbo].[Case]  WITH CHECK ADD FOREIGN KEY([Type])
REFERENCES [dbo].[CaseTypeLookup] ([ID])
GO
ALTER TABLE [dbo].[Certificate]  WITH CHECK ADD FOREIGN KEY([Status])
REFERENCES [dbo].[CertificateStatusLookup] ([ID])
GO