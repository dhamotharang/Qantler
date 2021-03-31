/****** Object:  Table [dbo].[Attachment]    Script Date: 2020-11-19 11:58:07 AM ******/
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
/****** Object:  Table [dbo].[AttachmentTypeLookup]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AttachmentTypeLookup](
	[ID] [smallint] NOT NULL,
	[Text] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_AttachmentTypeLookup] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BatchCertificates]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BatchCertificates](
	[BatchID] [bigint] NOT NULL,
	[CertificateID] [bigint] NOT NULL,
 CONSTRAINT [PK_BatchCertificates] PRIMARY KEY CLUSTERED 
(
	[BatchID] ASC,
	[CertificateID] ASC
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
	[RequestType] [smallint] NOT NULL,
	[Number] [varchar](60) NOT NULL,
	[Status] [smallint] NOT NULL,
	[CodeID] [bigint] NOT NULL,
	[Template] [smallint] NOT NULL,
	[IsCertifiedTrueCopy] [bit] NOT NULL,
	[Scheme] [smallint] NOT NULL,
	[SubScheme] [smallint] NULL,
	[IssuedOn] [datetime2](0) NULL,
	[StartsFrom] [datetime2](0) NOT NULL,
	[ExpiresOn] [datetime2](0) NOT NULL,
	[SerialNo] [varchar](36) NULL,
	[CustomerID] [uniqueidentifier] NOT NULL,
	[CustomerName] [nvarchar](150) NOT NULL,
	[PremiseID] [bigint] NOT NULL,
	[MailingPremiseID] [bigint] NOT NULL,
	[Remarks] [nvarchar](500) NULL,
	[RequestID] [bigint] NULL,
	[CreatedOn] [datetime2](0) NULL,
	[ModifiedOn] [datetime2](0) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Certificate] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Certificate360]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Certificate360](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Number] [varchar](60) NOT NULL,
	[Status] [smallint] NOT NULL,
	[Template] [smallint] NOT NULL,
	[Scheme] [smallint] NOT NULL,
	[SubScheme] [smallint] NULL,
	[IssuedOn] [datetime2](0) NULL,
	[ExpiresOn] [datetime2](0) NULL,
	[SuspendedUntil] [datetime2](0) NULL,
	[SerialNo] [varchar](36) NULL,
	[CustomerID] [uniqueidentifier] NOT NULL,
	[CustomerName] [nvarchar](150) NOT NULL,
	[RequestorID] [uniqueidentifier] NULL,
	[RequestorName] [nvarchar](150) NULL,
	[AgentID] [uniqueidentifier] NULL,
	[AgentName] [nvarchar](150) NULL,
	[PremiseID] [bigint] NOT NULL,
	[CreatedOn] [datetime2](0) NULL,
	[ModifiedOn] [datetime2](0) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Certificate360] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Certificate360History]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Certificate360History](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[RequestID] [bigint] NOT NULL,
	[RefID] [varchar](36) NULL,
	[RequestorID] [uniqueidentifier] NULL,
	[RequestorName] [nvarchar](150) NULL,
	[AgentID] [uniqueidentifier] NULL,
	[AgentName] [nvarchar](150) NULL,
	[Duration] [smallint] NOT NULL,
	[IssuedOn] [datetime2](0) NULL,
	[ExpiresOn] [datetime2](0) NOT NULL,
	[SerialNo] [varchar](36) NULL,
	[ApprovedOn] [datetime2](0) NOT NULL,
	[ApprovedBy] [uniqueidentifier] NULL,
	[CertificateID] [bigint] NOT NULL,
	[CreatedOn] [datetime2](0) NOT NULL,
	[ModifiedOn] [datetime2](0) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Certificate360History] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Certificate360Ingredients]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Certificate360Ingredients](
	[CertificateID] [bigint] NOT NULL,
	[IngredientID] [bigint] NOT NULL,
 CONSTRAINT [PK_Certificate360Ingredients] PRIMARY KEY CLUSTERED 
(
	[CertificateID] ASC,
	[IngredientID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Certificate360Menus]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Certificate360Menus](
	[CertificateID] [bigint] NOT NULL,
	[MenuID] [bigint] NOT NULL,
 CONSTRAINT [PK_Certificate360Menus] PRIMARY KEY CLUSTERED 
(
	[CertificateID] ASC,
	[MenuID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Certificate360Teams]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Certificate360Teams](
	[CertificateID] [bigint] NOT NULL,
	[HalalTeamID] [bigint] NOT NULL,
 CONSTRAINT [PK_Certificate360Teams] PRIMARY KEY CLUSTERED 
(
	[CertificateID] ASC,
	[HalalTeamID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CertificateBatch]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CertificateBatch](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Code] [varchar](15) NOT NULL,
	[Description] [nvarchar](255) NOT NULL,
	[Status] [smallint] NOT NULL,
	[Template] [smallint] NOT NULL,
	[FileID] [uniqueidentifier] NULL,
	[AcknowledgedOn] [datetime2](0) NULL,
	[LastAction] [datetime2](0) NULL,
	[CreatedOn] [datetime2](0) NOT NULL,
	[ModifiedOn] [datetime2](0) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_CertificateBatch] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CertificateBatchStatusLookup]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CertificateBatchStatusLookup](
	[ID] [smallint] NOT NULL,
	[Text] [nvarchar](150) NULL,
 CONSTRAINT [PK_CertificateBatchStatusLookup] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CertificateMenus]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CertificateMenus](
	[CertificateID] [bigint] NOT NULL,
	[MenuID] [bigint] NOT NULL,
 CONSTRAINT [PK_CertificateMenus] PRIMARY KEY CLUSTERED 
(
	[CertificateID] ASC,
	[MenuID] ASC
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
/****** Object:  Table [dbo].[CertificateTemplateLookup]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CertificateTemplateLookup](
	[ID] [smallint] NOT NULL,
	[Text] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_CertificateTemplateLookup] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ChangeTypeLookup]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChangeTypeLookup](
	[ID] [smallint] NOT NULL,
	[Text] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_ChangeTypeLookup] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Characteristic]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Characteristic](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Type] [smallint] NOT NULL,
	[Value] [nvarchar](2000) NOT NULL,
	[CreatedOn] [datetime2](0) NOT NULL,
	[ModifiedOn] [datetime2](0) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Characteristic] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CharTypeLookup]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CharTypeLookup](
	[ID] [smallint] NOT NULL,
	[Text] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_CharTypeLookup] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Code]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Code](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Type] [smallint] NOT NULL,
	[Value] [varchar](36) NOT NULL,
	[Text] [nvarchar](255) NOT NULL,
	[BillingCycle] [varchar](4) NULL,
	[CertificateExpiry] [datetime2](0) NULL,
 CONSTRAINT [PK_Code] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CodeTypeLookup]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CodeTypeLookup](
	[ID] [smallint] NOT NULL,
	[Text] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_CodeTypeLookup] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Comment]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Comment](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Text] [nvarchar](500) NOT NULL,
	[UserID] [uniqueidentifier] NOT NULL,
	[BatchID] [bigint] NOT NULL,
	[CreatedOn] [datetime2](0) NOT NULL,
	[ModifiedOn] [datetime2](0) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Comment] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer](
	[ID] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](150) NULL,
	[IDType] [smallint] NULL,
	[AltID] [nvarchar](30) NULL,
	[CodeID] [bigint] NULL,
	[GroupCodeID] [bigint] NULL,
	[OfficerInCharge] UNIQUEIDENTIFIER NULL
 CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED 
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
	[Keyword] [nvarchar(500)] NULL,
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
/****** Object:  Table [dbo].[EscalateStatusLookup]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EscalateStatusLookup](
	[ID] [smallint] NOT NULL,
	[Text] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_EscalateStatusLookup] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HalalTeam]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HalalTeam](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[AltID] [nvarchar](36) NULL,
	[Name] [nvarchar](150) NOT NULL,
	[Designation] [nvarchar](100) NULL,
	[Role] [nvarchar](50) NULL,
	[IsCertified] [bit] NOT NULL,
	[JoinedOn] [datetime2](0) NULL,
	[ChangeType] [smallint] NOT NULL,
	[RequestID] [bigint] NOT NULL,
	[CreatedOn] [datetime2](0) NOT NULL,
	[ModifiedOn] [datetime2](0) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_HalalTeam] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Ingredient]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ingredient](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[RiskCategory] [smallint] NOT NULL,
	[Text] [nvarchar](500) NOT NULL,
	[SubText] [nvarchar](2000) NULL,
	[Approved] [bit] NULL,
	[Remarks] [nvarchar](2000) NULL,
	[ReviewedOn] [datetime2](0) NULL,
	[ChangeType] [smallint] NOT NULL,
	[SupplierName] [nvarchar](255) NULL,
	[BrandName] [nvarchar](255) NULL,
	[CertifyingBodyName] [nvarchar](255) NULL,
	[Status] [smallint] NULL,
	[CertifyingBodyStatus] [smallint] NULL,
	[RequestID] [bigint] NULL,
	[CreatedOn] [datetime2](0) NOT NULL,
	[ModifiedOn] [datetime2](0) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[ReviewedBy] [uniqueidentifier] NULL,
	[Undeclared] [bit] NULL,
 CONSTRAINT [PK_Ingredient] PRIMARY KEY CLUSTERED 
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
GO
/****** Object:  Table [dbo].[Log]    Script Date: 2020-11-19 11:58:07 AM ******/
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
	[UserId] [uniqueidentifier] NULL,
	[CreatedOn] [datetime2](0) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Log] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LogTypeLookup]    Script Date: 2020-11-19 11:58:07 AM ******/
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
/****** Object:  Table [dbo].[Master]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Master](
	[Type] [smallint] NOT NULL,
	[ID] [uniqueidentifier] NOT NULL,
	[Value] [nvarchar](255) NOT NULL,
	[ParentID] [uniqueidentifier] NULL,
	[CreatedOn] [datetime2](0) NOT NULL,
	[ModifiedOn] [datetime2](0) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Master] PRIMARY KEY CLUSTERED 
(
	[Type] ASC,
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MasterTypeLookup]    Script Date: 2020-11-19 11:58:07 AM ******/
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
/****** Object:  Table [dbo].[Menu]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Menu](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Scheme] [smallint] NOT NULL,
	[Text] [nvarchar](500) NOT NULL,
	[SubText] [nvarchar](2000) NULL,
	[Approved] [bit] NULL,
	[Remarks] [nvarchar](2000) NULL,
	[ReviewedOn] [datetime2](0) NULL,
	[ChangeType] [smallint] NOT NULL,
	[RequestID] [bigint] NULL,
	[CreatedOn] [datetime2](0) NOT NULL,
	[ModifiedOn] [datetime2](0) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[ReviewedBy] [uniqueidentifier] NULL,
	[Group] [smallint] NOT NULL,
	[Index] [smallint] NOT NULL,
	[Undeclared] [bit] NULL,
 CONSTRAINT [PK_Menu] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Notes]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notes](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Text] [nvarchar](4000) NOT NULL,
	[CreatedBy] [uniqueidentifier] NOT NULL,
	[RequestID] [bigint] NOT NULL,
	[CreatedOn] [datetime2](0) NOT NULL,
	[ModifiedOn] [datetime2](0) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Notes] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NotesAttachments]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NotesAttachments](
	[NotesID] [bigint] NOT NULL,
	[AttachmentID] [bigint] NOT NULL,
 CONSTRAINT [PK_NotesAttachments] PRIMARY KEY CLUSTERED 
(
	[NotesID] ASC,
	[AttachmentID] ASC
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
/****** Object:  Table [dbo].[Premise]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Premise](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
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
	[CustomerID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Premise] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PremiseTypeLookup]    Script Date: 2020-11-19 11:58:07 AM ******/
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
/****** Object:  Table [dbo].[Request]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Request](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Step] [smallint] NULL,
	[CustomerID] [uniqueidentifier] NULL,
	[CustomerName] [nvarchar](150) NULL,
	[CodeID] [bigint] NULL,
	[GroupCodeID] [bigint] NULL,
	[RequestorID] [uniqueidentifier] NOT NULL,
	[RequestorName] [nvarchar](150) NULL,
	[AgentID] [uniqueidentifier] NULL,
	[AgentName] [nvarchar](150) NULL,
	[Type] [smallint] NOT NULL,
	[Status] [smallint] NOT NULL,
	[StatusMinor] [smallint] NULL,
	[OldStatus] [smallint] NULL,
	[RefID] [varchar](36) NULL,
	[ParentID] [bigint] NULL,
	[Expedite] [bit] NOT NULL,
	[EscalateStatus] [smallint] NOT NULL,
	[AssignedTo] [uniqueidentifier] NULL,
	[JobID] [bigint] NULL,
	[SubmittedOn] [datetime2](0) NULL,
	[TargetCompletion] [datetime2](0) NULL,
	[DueOn] [datetime2](0) NULL,
	[LastAction] [datetime2](0) NULL,
	[RemindOn] [datetime2](0) NULL,
	[CreatedOn] [datetime2](0) NULL,
	[ModifiedOn] [datetime2](0) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Request] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RequestActionHistory]    Script Date: 28/1/2021 5:24:50 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RequestActionHistory](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[OfficerID] [uniqueidentifier] NOT NULL,
	[Action] [smallint] NOT NULL,
	[RequestID] [bigint] NOT NULL,
	[RefID] [varchar](36) NULL,
	[Remarks] [nvarchar](500) NULL,
	[CreatedOn] [datetime2](0) NOT NULL,
 CONSTRAINT [PK_RequestActionHistory] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RequestActionLookup]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RequestActionLookup](
	[ID] [smallint] NOT NULL,
	[Text] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_RequestActionLookup] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RequestAttachments]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RequestAttachments](
	[RequestID] [bigint] NOT NULL,
	[AttachmentID] [bigint] NOT NULL,
 CONSTRAINT [PK_RequestAttachments] PRIMARY KEY CLUSTERED 
(
	[RequestID] ASC,
	[AttachmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RequestCharacteristics]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RequestCharacteristics](
	[RequestID] [bigint] NOT NULL,
	[CharID] [bigint] NOT NULL,
 CONSTRAINT [PK_RequestCharacteristics] PRIMARY KEY CLUSTERED 
(
	[RequestID] ASC,
	[CharID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RequestLineItem]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RequestLineItem](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Scheme] [smallint] NULL,
	[SubScheme] [smallint] NULL,
	[RequestID] [bigint] NOT NULL,
	[CreatedOn] [datetime2](0) NULL,
	[ModifiedOn] [datetime2](0) NULL,
	[IsDeleted] [bit] NOT NULL,
	[ChecklistHistoryID] [bigint] NULL,
	[CertificateID] [bigint] NULL,
 CONSTRAINT [PK_RequestLineItem] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RequestLineItemCharacteristics]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RequestLineItemCharacteristics](
	[LineItemID] [bigint] NOT NULL,
	[CharID] [bigint] NOT NULL,
 CONSTRAINT [PK_RequestLineItemCharacteristics] PRIMARY KEY CLUSTERED 
(
	[LineItemID] ASC,
	[CharID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RequestLog]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RequestLog](
	[RequestID] [bigint] NOT NULL,
	[LogID] [bigint] NOT NULL,
 CONSTRAINT [PK_RequestLog] PRIMARY KEY CLUSTERED 
(
	[RequestID] ASC,
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RequestNotes]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RequestNotes](
	[RequestID] [bigint] NOT NULL,
	[NoteID] [bigint] NOT NULL,
 CONSTRAINT [PK_RequestNotes] PRIMARY KEY CLUSTERED 
(
	[RequestID] ASC,
	[NoteID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RequestPremise]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RequestPremise](
	[RequestID] [bigint] NOT NULL,
	[PremiseID] [bigint] NOT NULL,
	[IsPrimary] [bit] NOT NULL,
	[ChangeType] [smallint] NOT NULL,
 CONSTRAINT [PK_RequestPremise] PRIMARY KEY CLUSTERED 
(
	[RequestID] ASC,
	[PremiseID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RequestStatusLookup]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RequestStatusLookup](
	[ID] [smallint] NOT NULL,
	[Text] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_RequestStatusLookup] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RequestStatusMinorLookup]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RequestStatusMinorLookup](
	[ID] [smallint] NOT NULL,
	[Text] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_RequestStatusMinorLookup] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RequestTypeLookup]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RequestTypeLookup](
	[ID] [smallint] NOT NULL,
	[Text] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_RequestTypeLookup] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Review]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Review](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Step] [smallint] NULL,
	[ReviewerID] [uniqueidentifier] NOT NULL,
	[RequestID] [bigint] NOT NULL,
	[Grade] [smallint] NULL,
	[RefID] [bigint] NULL,
	[EmailID] [bigint] NULL,
	[CreatedOn] [datetime2](0) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Review] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ReviewLineItem]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReviewLineItem](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Scheme] [smallint] NOT NULL,
	[Remarks] [nvarchar](2000) NULL,
	[ReviewID] [bigint] NOT NULL,
	[Approved] [bit] NULL,
	[SubScheme] [smallint] NULL,
 CONSTRAINT [PK_ReviewLineItem] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RFA]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RFA](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Status] [smallint] NOT NULL,
	[RaisedBy] [uniqueidentifier] NOT NULL,
	[RequestID] [bigint] NOT NULL,
	[CreatedOn] [datetime2](0) NOT NULL,
	[ModifiedOn] [datetime2](0) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[DueOn] [datetime2](7) NULL,
 CONSTRAINT [PK_RFA] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RFAAttachments]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RFAAttachments](
	[RFAID] [bigint] NOT NULL,
	[AttachmentID] [bigint] NOT NULL,
 CONSTRAINT [PK_RFAAttachments] PRIMARY KEY CLUSTERED 
(
	[RFAID] ASC,
	[AttachmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RFALineItem]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RFALineItem](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Scheme] [smallint] NOT NULL,
	[Index] [smallint] NOT NULL,
	[Remarks] [nvarchar](2000) NULL,
	[RFAID] [bigint] NOT NULL,
	[CreatedOn] [datetime2](0) NOT NULL,
	[ModifiedOn] [datetime2](0) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[ChecklistCategoryID] [bigint] NOT NULL,
	[ChecklistCategoryText] [nvarchar](80) NOT NULL,
	[ChecklistID] [bigint] NOT NULL,
	[ChecklistText] [nvarchar](4000) NOT NULL,
 CONSTRAINT [PK_RFALineItem] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RFALineItemAttachments]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RFALineItemAttachments](
	[LineItemID] [bigint] NOT NULL,
	[AttachmentID] [bigint] NOT NULL,
 CONSTRAINT [PK_RFALineItemAttachments] PRIMARY KEY CLUSTERED 
(
	[LineItemID] ASC,
	[AttachmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RFALog]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RFALog](
	[RFAID] [bigint] NOT NULL,
	[LogID] [bigint] NOT NULL,
 CONSTRAINT [PK_RFALog] PRIMARY KEY CLUSTERED 
(
	[RFAID] ASC,
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RFAReply]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RFAReply](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Text] [nvarchar](2000) NOT NULL,
	[LineItemID] [bigint] NOT NULL,
	[CreatedOn] [datetime2](0) NOT NULL,
	[ModifiedOn] [datetime2](0) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_RFAReply] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RFAReplyAttachments]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RFAReplyAttachments](
	[ReplyID] [bigint] NOT NULL,
	[AttachmentID] [bigint] NOT NULL,
 CONSTRAINT [PK_RFAReplyAttachments] PRIMARY KEY CLUSTERED 
(
	[ReplyID] ASC,
	[AttachmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RFAStatusLookup]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RFAStatusLookup](
	[ID] [smallint] NOT NULL,
	[Text] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_RFAStatusLookup] PRIMARY KEY CLUSTERED 
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
	[Text] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_RiskCategoryLookup] PRIMARY KEY CLUSTERED 
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
/****** Object:  Table [dbo].[SerialNoMap]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SerialNoMap](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Key] [smallint] NOT NULL,
	[Val] [smallint] NOT NULL,
 CONSTRAINT [PK_SerialNoMap] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Settings]    Script Date: 2020-11-19 11:58:07 AM ******/
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
/****** Object:  Table [dbo].[SettingsLog]    Script Date: 2020-11-19 11:58:07 AM ******/
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
/****** Object:  Table [dbo].[SettingsLookup]    Script Date: 2020-11-19 11:58:07 AM ******/
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
/****** Object:  Table [dbo].[CertificateDeliveryStatusLookup]    Script Date: 2021-03-01 6:25:14 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CertificateDeliveryStatusLookup](
	[ID] [smallint] NOT NULL,
	[Text] [nvarchar](150) NOT NULL,
 CONSTRAINT [PK_CertificateDeliveryStatusLookup] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Statistics]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Statistics](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Key] [varchar](36) NULL,
	[Event] [varchar](30) NOT NULL,
	[RequestID] [bigint] NOT NULL,
	[Type] [smallint] NOT NULL,
	[CreatedOn] [datetime2](0) NOT NULL,
	[MonthYear] [date] NOT NULL,
	[Year] [smallint] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Statistics] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IDTypeLookup]    Script Date: 19/3/2021 12:58:09 PM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Attachment] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Certificate] ADD  DEFAULT ((0)) FOR [IsCertifiedTrueCopy]
GO
ALTER TABLE [dbo].[Certificate360] ADD  CONSTRAINT [DF__Certifica__IsDel__222BD200]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Characteristic] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Comment] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[HalalTeam] ADD  DEFAULT ((0)) FOR [ChangeType]
GO
ALTER TABLE [dbo].[HalalTeam] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Ingredient] ADD  DEFAULT ((0)) FOR [ChangeType]
GO
ALTER TABLE [dbo].[Ingredient] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Ingredient] ADD  DEFAULT ((0)) FOR [Undeclared]
GO
ALTER TABLE [dbo].[Master] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Menu] ADD  DEFAULT ((0)) FOR [ChangeType]
GO
ALTER TABLE [dbo].[Menu] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Menu] ADD  DEFAULT ((1)) FOR [Group]
GO
ALTER TABLE [dbo].[Menu] ADD  DEFAULT ((0)) FOR [Index]
GO
ALTER TABLE [dbo].[Menu] ADD  DEFAULT ((0)) FOR [Undeclared]
GO
ALTER TABLE [dbo].[Notes] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Premise] ADD  CONSTRAINT [DF__Premise__IsDelet__5C6D822E]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Request] ADD  DEFAULT ((0)) FOR [Step]
GO
ALTER TABLE [dbo].[Request] ADD  DEFAULT ((0)) FOR [EscalateStatus]
GO
ALTER TABLE [dbo].[Request] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[RequestPremise] ADD  DEFAULT ((0)) FOR [IsPrimary]
GO
ALTER TABLE [dbo].[RequestPremise] ADD  DEFAULT ((0)) FOR [ChangeType]
GO
ALTER TABLE [dbo].[RFA] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[RFALineItem] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[RFAReply] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Statistics] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO

ALTER TABLE [dbo].[BatchCertificates]  WITH CHECK ADD FOREIGN KEY([BatchID])
REFERENCES [dbo].[CertificateBatch] ([ID])
GO
ALTER TABLE [dbo].[BatchCertificates]  WITH CHECK ADD FOREIGN KEY([CertificateID])
REFERENCES [dbo].[Certificate] ([ID])
GO
ALTER TABLE [dbo].[Certificate]  WITH CHECK ADD FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customer] ([ID])
GO
ALTER TABLE [dbo].[Certificate]  WITH CHECK ADD FOREIGN KEY([MailingPremiseID])
REFERENCES [dbo].[Premise] ([ID])
GO
ALTER TABLE [dbo].[Certificate]  WITH CHECK ADD  CONSTRAINT [FK__Certifica__Premi__569F9A3F] FOREIGN KEY([PremiseID])
REFERENCES [dbo].[Premise] ([ID])
GO
ALTER TABLE [dbo].[Certificate] CHECK CONSTRAINT [FK__Certifica__Premi__569F9A3F]
GO
ALTER TABLE [dbo].[Certificate]  WITH CHECK ADD FOREIGN KEY([RequestID])
REFERENCES [dbo].[Request] ([ID])
GO
ALTER TABLE [dbo].[Certificate]  WITH CHECK ADD FOREIGN KEY([Scheme])
REFERENCES [dbo].[SchemeLookup] ([ID])
GO
ALTER TABLE [dbo].[Certificate]  WITH CHECK ADD FOREIGN KEY([SubScheme])
REFERENCES [dbo].[SubSchemeLookup] ([ID])
GO
ALTER TABLE [dbo].[Certificate]  WITH CHECK ADD FOREIGN KEY([Template])
REFERENCES [dbo].[CertificateTemplateLookup] ([ID])
GO
ALTER TABLE [dbo].[Certificate360]  WITH CHECK ADD  CONSTRAINT [FK__Certifica__Custo__1C72F8AA] FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customer] ([ID])
GO
ALTER TABLE [dbo].[Certificate360] CHECK CONSTRAINT [FK__Certifica__Custo__1C72F8AA]
GO
ALTER TABLE [dbo].[Certificate360]  WITH CHECK ADD  CONSTRAINT [FK__Certifica__Premi__1D671CE3] FOREIGN KEY([PremiseID])
REFERENCES [dbo].[Premise] ([ID])
GO
ALTER TABLE [dbo].[Certificate360] CHECK CONSTRAINT [FK__Certifica__Premi__1D671CE3]
GO
ALTER TABLE [dbo].[Certificate360]  WITH CHECK ADD  CONSTRAINT [FK__Certifica__Schem__1E5B411C] FOREIGN KEY([Scheme])
REFERENCES [dbo].[SchemeLookup] ([ID])
GO
ALTER TABLE [dbo].[Certificate360] CHECK CONSTRAINT [FK__Certifica__Schem__1E5B411C]
GO
ALTER TABLE [dbo].[Certificate360]  WITH CHECK ADD  CONSTRAINT [FK__Certifica__Statu__2137ADC7] FOREIGN KEY([Status])
REFERENCES [dbo].[CertificateStatusLookup] ([ID])
GO
ALTER TABLE [dbo].[Certificate360] CHECK CONSTRAINT [FK__Certifica__Statu__2137ADC7]
GO
ALTER TABLE [dbo].[Certificate360]  WITH CHECK ADD  CONSTRAINT [FK__Certifica__SubSc__1F4F6555] FOREIGN KEY([SubScheme])
REFERENCES [dbo].[SubSchemeLookup] ([ID])
GO
ALTER TABLE [dbo].[Certificate360] CHECK CONSTRAINT [FK__Certifica__SubSc__1F4F6555]
GO
ALTER TABLE [dbo].[Certificate360]  WITH CHECK ADD  CONSTRAINT [FK__Certifica__Templ__2043898E] FOREIGN KEY([Template])
REFERENCES [dbo].[CertificateTemplateLookup] ([ID])
GO
ALTER TABLE [dbo].[Certificate360] CHECK CONSTRAINT [FK__Certifica__Templ__2043898E]
GO
ALTER TABLE [dbo].[Certificate360History]  WITH CHECK ADD  CONSTRAINT [FK__Certifica__Certi__50E6C0E9] FOREIGN KEY([CertificateID])
REFERENCES [dbo].[Certificate360] ([ID])
GO
ALTER TABLE [dbo].[Certificate360History] CHECK CONSTRAINT [FK__Certifica__Certi__50E6C0E9]
GO
ALTER TABLE [dbo].[Certificate360Ingredients]  WITH CHECK ADD  CONSTRAINT [FK__Certifica__Certi__49459F21] FOREIGN KEY([CertificateID])
REFERENCES [dbo].[Certificate360] ([ID])
GO
ALTER TABLE [dbo].[Certificate360Ingredients] CHECK CONSTRAINT [FK__Certifica__Certi__49459F21]
GO
ALTER TABLE [dbo].[Certificate360Ingredients]  WITH CHECK ADD FOREIGN KEY([IngredientID])
REFERENCES [dbo].[Ingredient] ([ID])
GO
ALTER TABLE [dbo].[Certificate360Menus]  WITH CHECK ADD  CONSTRAINT [FK__Certifica__Certi__45750E3D] FOREIGN KEY([CertificateID])
REFERENCES [dbo].[Certificate360] ([ID])
GO
ALTER TABLE [dbo].[Certificate360Menus] CHECK CONSTRAINT [FK__Certifica__Certi__45750E3D]
GO
ALTER TABLE [dbo].[Certificate360Menus]  WITH CHECK ADD FOREIGN KEY([MenuID])
REFERENCES [dbo].[Menu] ([ID])
GO
ALTER TABLE [dbo].[Certificate360Teams]  WITH CHECK ADD  CONSTRAINT [FK__Certifica__Certi__4D163005] FOREIGN KEY([CertificateID])
REFERENCES [dbo].[Certificate360] ([ID])
GO
ALTER TABLE [dbo].[Certificate360Teams] CHECK CONSTRAINT [FK__Certifica__Certi__4D163005]
GO
ALTER TABLE [dbo].[Certificate360Teams]  WITH CHECK ADD FOREIGN KEY([HalalTeamID])
REFERENCES [dbo].[HalalTeam] ([ID])
GO
ALTER TABLE [dbo].[CertificateBatch]  WITH CHECK ADD FOREIGN KEY([Status])
REFERENCES [dbo].[CertificateBatchStatusLookup] ([ID])
GO
ALTER TABLE [dbo].[CertificateBatch]  WITH CHECK ADD FOREIGN KEY([Template])
REFERENCES [dbo].[CertificateTemplateLookup] ([ID])
GO
ALTER TABLE [dbo].[CertificateMenus]  WITH CHECK ADD FOREIGN KEY([CertificateID])
REFERENCES [dbo].[Certificate] ([ID])
GO
ALTER TABLE [dbo].[CertificateMenus]  WITH CHECK ADD FOREIGN KEY([MenuID])
REFERENCES [dbo].[Menu] ([ID])
GO
ALTER TABLE [dbo].[Characteristic]  WITH CHECK ADD FOREIGN KEY([Type])
REFERENCES [dbo].[CharTypeLookup] ([ID])
GO
ALTER TABLE [dbo].[Code]  WITH CHECK ADD FOREIGN KEY([Type])
REFERENCES [dbo].[CodeTypeLookup] ([ID])
GO
ALTER TABLE [dbo].[Comment]  WITH CHECK ADD FOREIGN KEY([BatchID])
REFERENCES [dbo].[CertificateBatch] ([ID])
GO
ALTER TABLE [dbo].[Comment]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[Officer] ([ID])
GO
ALTER TABLE [dbo].[Customer]  WITH CHECK ADD FOREIGN KEY([CodeID])
REFERENCES [dbo].[Code] ([ID])
GO
ALTER TABLE [dbo].[Customer]  WITH CHECK ADD FOREIGN KEY([GroupCodeID])
REFERENCES [dbo].[Code] ([ID])
GO
ALTER TABLE [dbo].[Customer]  WITH CHECK ADD  CONSTRAINT [FK_Customer_IDTypeLookup] FOREIGN KEY([IDType])
REFERENCES [dbo].[IDTypeLookup] ([ID])
GO
ALTER TABLE [dbo].[EmailTemplate]  WITH CHECK ADD FOREIGN KEY([Type])
REFERENCES [dbo].[EmailTemplateTypeLookup] ([ID])
GO
ALTER TABLE [dbo].[HalalTeam]  WITH CHECK ADD FOREIGN KEY([ChangeType])
REFERENCES [dbo].[ChangeTypeLookup] ([ID])
GO
ALTER TABLE [dbo].[HalalTeam]  WITH CHECK ADD FOREIGN KEY([RequestID])
REFERENCES [dbo].[Request] ([ID])
GO
ALTER TABLE [dbo].[Ingredient]  WITH CHECK ADD FOREIGN KEY([ChangeType])
REFERENCES [dbo].[ChangeTypeLookup] ([ID])
GO
ALTER TABLE [dbo].[Ingredient]  WITH CHECK ADD FOREIGN KEY([RequestID])
REFERENCES [dbo].[Request] ([ID])
GO
ALTER TABLE [dbo].[Ingredient]  WITH CHECK ADD FOREIGN KEY([RiskCategory])
REFERENCES [dbo].[RiskCategoryLookup] ([ID])
GO
ALTER TABLE [dbo].[Log]  WITH CHECK ADD FOREIGN KEY([Type])
REFERENCES [dbo].[LogTypeLookup] ([ID])
GO
ALTER TABLE [dbo].[Log]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[Officer] ([ID])
GO
ALTER TABLE [dbo].[Master]  WITH CHECK ADD FOREIGN KEY([Type])
REFERENCES [dbo].[MasterTypeLookup] ([ID])
GO
ALTER TABLE [dbo].[Menu]  WITH CHECK ADD FOREIGN KEY([ChangeType])
REFERENCES [dbo].[ChangeTypeLookup] ([ID])
GO
ALTER TABLE [dbo].[Menu]  WITH CHECK ADD FOREIGN KEY([RequestID])
REFERENCES [dbo].[Request] ([ID])
GO
ALTER TABLE [dbo].[Notes]  WITH CHECK ADD FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Officer] ([ID])
GO
ALTER TABLE [dbo].[Notes]  WITH CHECK ADD FOREIGN KEY([RequestID])
REFERENCES [dbo].[Request] ([ID])
GO
ALTER TABLE [dbo].[NotesAttachments]  WITH CHECK ADD FOREIGN KEY([AttachmentID])
REFERENCES [dbo].[Attachment] ([ID])
GO
ALTER TABLE [dbo].[NotesAttachments]  WITH CHECK ADD FOREIGN KEY([NotesID])
REFERENCES [dbo].[Notes] ([ID])
GO
ALTER TABLE [dbo].[Request]  WITH CHECK ADD FOREIGN KEY([AssignedTo])
REFERENCES [dbo].[Officer] ([ID])
GO
ALTER TABLE [dbo].[Request]  WITH CHECK ADD FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customer] ([ID])
GO
ALTER TABLE [dbo].[Request]  WITH CHECK ADD FOREIGN KEY([EscalateStatus])
REFERENCES [dbo].[EscalateStatusLookup] ([ID])
GO
ALTER TABLE [dbo].[Request]  WITH CHECK ADD FOREIGN KEY([OldStatus])
REFERENCES [dbo].[RequestStatusLookup] ([ID])
GO
ALTER TABLE [dbo].[Request]  WITH CHECK ADD FOREIGN KEY([ParentID])
REFERENCES [dbo].[Request] ([ID])
GO
ALTER TABLE [dbo].[Request]  WITH CHECK ADD FOREIGN KEY([Status])
REFERENCES [dbo].[RequestStatusLookup] ([ID])
GO
ALTER TABLE [dbo].[Request]  WITH CHECK ADD FOREIGN KEY([StatusMinor])
REFERENCES [dbo].[RequestStatusMinorLookup] ([ID])
GO
ALTER TABLE [dbo].[Request]  WITH CHECK ADD FOREIGN KEY([Type])
REFERENCES [dbo].[RequestTypeLookup] ([ID])
GO
ALTER TABLE [dbo].[RequestActionHistory]  WITH CHECK ADD FOREIGN KEY([Action])
REFERENCES [dbo].[RequestActionLookup] ([ID])
GO
ALTER TABLE [dbo].[RequestActionHistory]  WITH CHECK ADD FOREIGN KEY([OfficerID])
REFERENCES [dbo].[Officer] ([ID])
GO
ALTER TABLE [dbo].[RequestActionHistory]  WITH CHECK ADD FOREIGN KEY([RequestID])
REFERENCES [dbo].[Request] ([ID])
GO
ALTER TABLE [dbo].[RequestAttachments]  WITH CHECK ADD FOREIGN KEY([AttachmentID])
REFERENCES [dbo].[Attachment] ([ID])
GO
ALTER TABLE [dbo].[RequestAttachments]  WITH CHECK ADD FOREIGN KEY([RequestID])
REFERENCES [dbo].[Request] ([ID])
GO
ALTER TABLE [dbo].[RequestCharacteristics]  WITH CHECK ADD FOREIGN KEY([CharID])
REFERENCES [dbo].[Characteristic] ([ID])
GO
ALTER TABLE [dbo].[RequestCharacteristics]  WITH CHECK ADD FOREIGN KEY([RequestID])
REFERENCES [dbo].[Request] ([ID])
GO
ALTER TABLE [dbo].[RequestLineItem]  WITH CHECK ADD FOREIGN KEY([CertificateID])
REFERENCES [dbo].[Certificate] ([ID])
GO
ALTER TABLE [dbo].[RequestLineItem]  WITH CHECK ADD FOREIGN KEY([RequestID])
REFERENCES [dbo].[Request] ([ID])
GO
ALTER TABLE [dbo].[RequestLineItem]  WITH CHECK ADD FOREIGN KEY([Scheme])
REFERENCES [dbo].[SchemeLookup] ([ID])
GO
ALTER TABLE [dbo].[RequestLineItem]  WITH CHECK ADD FOREIGN KEY([SubScheme])
REFERENCES [dbo].[SubSchemeLookup] ([ID])
GO
ALTER TABLE [dbo].[RequestLineItemCharacteristics]  WITH CHECK ADD FOREIGN KEY([LineItemID])
REFERENCES [dbo].[RequestLineItem] ([ID])
GO
ALTER TABLE [dbo].[RequestLog]  WITH CHECK ADD FOREIGN KEY([LogID])
REFERENCES [dbo].[Log] ([ID])
GO
ALTER TABLE [dbo].[RequestLog]  WITH CHECK ADD FOREIGN KEY([RequestID])
REFERENCES [dbo].[Request] ([ID])
GO
ALTER TABLE [dbo].[RequestNotes]  WITH CHECK ADD FOREIGN KEY([NoteID])
REFERENCES [dbo].[Notes] ([ID])
GO
ALTER TABLE [dbo].[RequestNotes]  WITH CHECK ADD FOREIGN KEY([RequestID])
REFERENCES [dbo].[Request] ([ID])
GO
ALTER TABLE [dbo].[RequestPremise]  WITH CHECK ADD FOREIGN KEY([ChangeType])
REFERENCES [dbo].[ChangeTypeLookup] ([ID])
GO
ALTER TABLE [dbo].[RequestPremise]  WITH CHECK ADD  CONSTRAINT [FK__RequestPr__Premi__39AE55D6] FOREIGN KEY([PremiseID])
REFERENCES [dbo].[Premise] ([ID])
GO
ALTER TABLE [dbo].[RequestPremise] CHECK CONSTRAINT [FK__RequestPr__Premi__39AE55D6]
GO
ALTER TABLE [dbo].[RequestPremise]  WITH CHECK ADD  CONSTRAINT [FK__RequestPr__Premi__3AA27A0F] FOREIGN KEY([PremiseID])
REFERENCES [dbo].[Premise] ([ID])
GO
ALTER TABLE [dbo].[RequestPremise] CHECK CONSTRAINT [FK__RequestPr__Premi__3AA27A0F]
GO
ALTER TABLE [dbo].[RequestPremise]  WITH CHECK ADD  CONSTRAINT [FK__RequestPr__Premi__3D7EE6BA] FOREIGN KEY([PremiseID])
REFERENCES [dbo].[Premise] ([ID])
GO
ALTER TABLE [dbo].[RequestPremise] CHECK CONSTRAINT [FK__RequestPr__Premi__3D7EE6BA]
GO
ALTER TABLE [dbo].[RequestPremise]  WITH CHECK ADD  CONSTRAINT [FK__RequestPr__Premi__414F779E] FOREIGN KEY([PremiseID])
REFERENCES [dbo].[Premise] ([ID])
GO
ALTER TABLE [dbo].[RequestPremise] CHECK CONSTRAINT [FK__RequestPr__Premi__414F779E]
GO
ALTER TABLE [dbo].[RequestPremise]  WITH CHECK ADD FOREIGN KEY([RequestID])
REFERENCES [dbo].[Request] ([ID])
GO
ALTER TABLE [dbo].[RequestPremise]  WITH CHECK ADD FOREIGN KEY([RequestID])
REFERENCES [dbo].[Request] ([ID])
GO
ALTER TABLE [dbo].[RequestPremise]  WITH CHECK ADD FOREIGN KEY([RequestID])
REFERENCES [dbo].[Request] ([ID])
GO
ALTER TABLE [dbo].[Review]  WITH CHECK ADD FOREIGN KEY([EmailID])
REFERENCES [dbo].[Email] ([ID])
GO
ALTER TABLE [dbo].[Review]  WITH CHECK ADD FOREIGN KEY([RequestID])
REFERENCES [dbo].[Request] ([ID])
GO
ALTER TABLE [dbo].[ReviewLineItem]  WITH CHECK ADD FOREIGN KEY([ReviewID])
REFERENCES [dbo].[Review] ([ID])
GO
ALTER TABLE [dbo].[ReviewLineItem]  WITH CHECK ADD FOREIGN KEY([Scheme])
REFERENCES [dbo].[SchemeLookup] ([ID])
GO
ALTER TABLE [dbo].[ReviewLineItem]  WITH CHECK ADD FOREIGN KEY([SubScheme])
REFERENCES [dbo].[SubSchemeLookup] ([ID])
GO
ALTER TABLE [dbo].[ReviewLineItem]  WITH CHECK ADD FOREIGN KEY([SubScheme])
REFERENCES [dbo].[SubSchemeLookup] ([ID])
GO
ALTER TABLE [dbo].[ReviewLineItem]  WITH CHECK ADD FOREIGN KEY([SubScheme])
REFERENCES [dbo].[SubSchemeLookup] ([ID])
GO
ALTER TABLE [dbo].[RFA]  WITH CHECK ADD FOREIGN KEY([RaisedBy])
REFERENCES [dbo].[Officer] ([ID])
GO
ALTER TABLE [dbo].[RFA]  WITH CHECK ADD FOREIGN KEY([RequestID])
REFERENCES [dbo].[Request] ([ID])
GO
ALTER TABLE [dbo].[RFA]  WITH CHECK ADD FOREIGN KEY([Status])
REFERENCES [dbo].[RFAStatusLookup] ([ID])
GO
ALTER TABLE [dbo].[RFAAttachments]  WITH CHECK ADD FOREIGN KEY([AttachmentID])
REFERENCES [dbo].[Attachment] ([ID])
GO
ALTER TABLE [dbo].[RFAAttachments]  WITH CHECK ADD FOREIGN KEY([RFAID])
REFERENCES [dbo].[RFA] ([ID])
GO
ALTER TABLE [dbo].[RFALineItem]  WITH CHECK ADD FOREIGN KEY([RFAID])
REFERENCES [dbo].[RFA] ([ID])
GO
ALTER TABLE [dbo].[RFALineItem]  WITH CHECK ADD FOREIGN KEY([Scheme])
REFERENCES [dbo].[SchemeLookup] ([ID])
GO
ALTER TABLE [dbo].[RFALineItemAttachments]  WITH CHECK ADD FOREIGN KEY([AttachmentID])
REFERENCES [dbo].[Attachment] ([ID])
GO
ALTER TABLE [dbo].[RFALineItemAttachments]  WITH CHECK ADD FOREIGN KEY([LineItemID])
REFERENCES [dbo].[RFALineItem] ([ID])
GO
ALTER TABLE [dbo].[RFALog]  WITH CHECK ADD FOREIGN KEY([LogID])
REFERENCES [dbo].[Log] ([ID])
GO
ALTER TABLE [dbo].[RFALog]  WITH CHECK ADD FOREIGN KEY([RFAID])
REFERENCES [dbo].[RFA] ([ID])
GO
ALTER TABLE [dbo].[RFAReply]  WITH CHECK ADD FOREIGN KEY([LineItemID])
REFERENCES [dbo].[RFALineItem] ([ID])
GO
ALTER TABLE [dbo].[RFAReplyAttachments]  WITH CHECK ADD FOREIGN KEY([AttachmentID])
REFERENCES [dbo].[Attachment] ([ID])
GO
ALTER TABLE [dbo].[RFAReplyAttachments]  WITH CHECK ADD FOREIGN KEY([ReplyID])
REFERENCES [dbo].[RFAReply] ([ID])
GO
ALTER TABLE [dbo].[Translations]  WITH CHECK ADD FOREIGN KEY([Locale])
REFERENCES [dbo].[Locale] ([ID])
GO
ALTER TABLE [dbo].[Ingredient]  WITH CHECK ADD FOREIGN KEY([Status])
REFERENCES [dbo].[IngredientStatusLookup] ([ID])
GO
ALTER TABLE [dbo].[Ingredient]  WITH CHECK ADD FOREIGN KEY([CertifyingBodyStatus])
REFERENCES [dbo].[CertifyingBodyStatusLookup] ([ID])
GO
ALTER TABLE [dbo].[Certificate]  WITH CHECK ADD FOREIGN KEY([Status])
REFERENCES [dbo].[CertificateDeliveryStatusLookup] ([ID])
GO
ALTER TABLE [dbo].[Certificate]  WITH CHECK ADD  CONSTRAINT [FK_Certificate_Code] FOREIGN KEY([CodeID])
REFERENCES [dbo].[Code] ([ID])
GO
ALTER TABLE [dbo].[Certificate] CHECK CONSTRAINT [FK_Certificate_Code]
GO
ALTER TABLE [dbo].[Statistics]  WITH CHECK ADD FOREIGN KEY([RequestID])
REFERENCES [dbo].[Request] ([ID])
GO
ALTER TABLE [dbo].[Premise]  WITH CHECK ADD  CONSTRAINT [FK_Premise_Customer] FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customer] ([ID])
GO
ALTER TABLE [dbo].[Customer]  WITH CHECK ADD FOREIGN KEY([OfficerInCharge])
REFERENCES [dbo].[Officer] ([ID])
GO
