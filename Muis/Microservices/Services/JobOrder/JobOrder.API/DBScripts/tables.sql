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
/****** Object:  Table [dbo].[Attendee]    Script Date: 2020-11-19 11:26:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Attendee](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](150) NULL,
	[Designation] [nvarchar](150) NULL,
	[Start] [bit] NULL,
	[End] [bit] NULL,
	[JobID] [bigint] NOT NULL,
	[CreatedOn] [datetime2](0) NOT NULL,
	[ModifiedOn] [datetime2](0) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Attendee] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AttendeeSignature]    Script Date: 2020-11-19 11:26:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AttendeeSignature](
	[AttendeeID] [bigint] NOT NULL,
	[AttachmentID] [bigint] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 2020-11-19 11:26:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer](
	[ID] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](150) NULL,
	[Code] [varchar](30) NULL,
	[GroupCode] [varchar](30) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Findings]    Script Date: 2020-11-19 11:26:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Findings](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Remarks] [nvarchar](4000) NULL,
	[OfficerID] [uniqueidentifier] NULL,
	[JobID] [bigint] NOT NULL,
	[CreatedOn] [datetime2](0) NULL,
	[ModifiedOn] [datetime2](0) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Findings] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FindingsLineItem]    Script Date: 2020-11-19 11:26:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FindingsLineItem](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Scheme] [smallint] NULL,
	[SubScheme] [smallint] NULL,
	[Index] [smallint] NULL,
	[ChecklistCategoryID] [bigint] NOT NULL,
	[ChecklistCategoryText] [nvarchar](80) NOT NULL,
	[ChecklistItemID] [bigint] NOT NULL,
	[ChecklistItemText] [nvarchar](4000) NOT NULL,
	[Remarks] [nvarchar](4000) NULL,
	[Complied] [bit] NULL,
	[FindingsID] [bigint] NOT NULL,
	[CreatedOn] [datetime2](0) NULL,
	[ModifiedOn] [datetime2](0) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_FindingsLineItem] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FindingsLineItemAttachments]    Script Date: 2020-11-19 11:26:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FindingsLineItemAttachments](
	[LineItemID] [bigint] NULL,
	[AttachmentID] [bigint] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FindingsSignature ]    Script Date: 2020-11-19 11:26:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FindingsSignature ](
	[FindingsID] [bigint] NOT NULL,
	[AttachmentID] [bigint] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Invitees]    Script Date: 2020-11-19 11:26:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Invitees](
	[JobID] [bigint] NOT NULL,
	[OfficerID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Invitees] PRIMARY KEY CLUSTERED 
(
	[JobID] ASC,
	[OfficerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[JobOrder]    Script Date: 2020-11-19 11:26:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JobOrder](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[RefID] [bigint] NULL,
	[Type] [smallint] NOT NULL,
	[Status] [smallint] NOT NULL,
	[Notes] [nvarchar](4000) NULL,
	[CustomerID] [uniqueidentifier] NULL,
	[AssignedTo] [uniqueidentifier] NULL,
	[TargetDate] [datetime2](0) NULL,
	[ScheduledOn] [datetime2](0) NULL,
	[ScheduledOnTo] [datetime2](0) NULL,
	[CompletedOn] [datetime2](0) NULL,
	[CreatedOn] [datetime2](0) NULL,
	[ModifiedOn] [datetime2](0) NULL,
	[IsDeleted] [bit] NOT NULL,
	[ContactPersonID] [uniqueidentifier] NULL
 CONSTRAINT [PK_JobOrder] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[JobOrderLineItem]    Script Date: 2020-11-19 11:26:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JobOrderLineItem](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Scheme] [smallint] NULL,
	[SubScheme] [smallint] NULL,
	[ChecklistHistoryID] [bigint] NULL,
	[JobID] [bigint] NOT NULL,
	[CreatedOn] [datetime2](0) NULL,
	[ModifiedOn] [datetime2](0) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_JobOrderLineItem] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[JobOrderLineItemAttachments]    Script Date: 2020-11-19 11:26:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JobOrderLineItemAttachments](
	[LineItemID] [bigint] NOT NULL,
	[AttachmentID] [bigint] NOT NULL,
 CONSTRAINT [PK_JobOrderLineItemAttachments] PRIMARY KEY CLUSTERED 
(
	[LineItemID] ASC,
	[AttachmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[JobOrderLog]    Script Date: 2020-11-19 11:26:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JobOrderLog](
	[JobID] [bigint] NOT NULL,
	[LogID] [bigint] NOT NULL,
 CONSTRAINT [PK_RequestLog] PRIMARY KEY CLUSTERED 
(
	[JobID] ASC,
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[JobOrderPremises]    Script Date: 2020-11-19 11:26:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JobOrderPremises](
	[JobID] [bigint] NOT NULL,
	[PremiseID] [bigint] NOT NULL,
 CONSTRAINT [PK_JobOrderPremises] PRIMARY KEY CLUSTERED 
(
	[JobID] ASC,
	[PremiseID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[JobOrderStatusLookup]    Script Date: 2020-11-19 11:26:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JobOrderStatusLookup](
	[ID] [smallint] NOT NULL,
	[Text] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_JobOrderStatusLookup] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[JobOrderTypeLookup]    Script Date: 2020-11-19 11:26:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JobOrderTypeLookup](
	[ID] [smallint] NOT NULL,
	[Text] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_JobOrderTypeLookup] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Locale]    Script Date: 2020-11-19 11:26:33 AM ******/
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
/****** Object:  Table [dbo].[Log]    Script Date: 2020-11-19 11:26:33 AM ******/
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
/****** Object:  Table [dbo].[LogTypeLookup]    Script Date: 2020-11-19 11:26:33 AM ******/
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
/****** Object:  Table [dbo].[Master]    Script Date: 2020-11-19 11:26:33 AM ******/
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
/****** Object:  Table [dbo].[Premise]    Script Date: 2020-11-19 11:26:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Premise](
	[ID] [bigint] NOT NULL,
	[IsLocal] [bit] NOT NULL,
	[Name] [nvarchar](150) NULL,
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
	[CreatedOn] [datetime2](0) NOT NULL,
	[ModifiedOn] [datetime2](0) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[Grade] [smallint] NULL,
	[IsHighPriority] [bit] NULL,
 CONSTRAINT [PK_Premise] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RescheduleHistory]    Script Date: 2020-11-19 11:26:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RescheduleHistory](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[JobID] [bigint] NOT NULL,
	[MasterID] [uniqueidentifier] NOT NULL,
	[CreatedOn] [datetime2](0) NOT NULL,
	[Notes] [nvarchar](4000) NULL,
 CONSTRAINT [PK_RescheduleHistory] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SchemeLookup]    Script Date: 2020-11-19 11:26:33 AM ******/
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
/****** Object:  Table [dbo].[Settings]    Script Date: 2020-11-19 11:26:33 AM ******/
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
/****** Object:  Table [dbo].[SettingsLog]    Script Date: 2020-11-19 11:26:33 AM ******/
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
/****** Object:  Table [dbo].[SettingsLookup]    Script Date: 2020-11-19 11:26:33 AM ******/
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
/****** Object:  Table [dbo].[SubSchemeLookup]    Script Date: 2020-11-19 11:26:33 AM ******/
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
/****** Object:  Table [dbo].[Translations]    Script Date: 2020-11-19 11:26:33 AM ******/
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

/****** Object:  Table [dbo].[Notes]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notes](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Text] [nvarchar](4000) NOT NULL,
	[CreatedBy] [uniqueidentifier] NOT NULL,
	[JobOrderID] [bigint] NOT NULL,
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
/****** Object:  Table [dbo].[PeriodicSchedulerStatusLookup]    Script Date: 2021-01-22 09:35:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PeriodicSchedulerStatusLookup](
	[ID] [smallint] NOT NULL,
	[Text] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_PeriodicSchedulerStatusLookup] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CertificateStatusLookup]    Script Date: 2021-01-22 09:35:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CertificateStatusLookup](
	[ID] [smallint] NOT NULL,
	[Text] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_CertificateStatusLookup] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PeriodicScheduler]    Script Date: 2021-01-22 09:44:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PeriodicScheduler](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[PremiseID] [bigint] NOT NULL,
	[LastJobID] [bigint] NULL,
	[LastScheduledOn] [datetime2](0) NULL,
	[NextTargetInspection] [datetime2](0) NULL,
	[Status] [smallint] NOT NULL
 CONSTRAINT [PK_PeriodicScheduler] PRIMARY KEY CLUSTERED 
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
	[Status] [smallint] NOT NULL,
	[Scheme] [smallint] NOT NULL,
	[SubScheme] [smallint] NULL,
	[IssuedOn] [datetime2](0) NULL,
	[StartsFrom] [datetime2](0) NULL,
	[ExpiresOn] [datetime2](0) NULL,
	[CustomerID] [uniqueidentifier] NOT NULL,	
	[PremiseID] [bigint] NOT NULL,
	[CreatedOn] [datetime2](0) NULL,
	[ModifiedOn] [datetime2](0) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Certificate] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Email]    Script Date: 2021-02-12 11:58:07 AM ******/
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
/****** Object:  Table [dbo].[EmailTemplate]    Script Date: Script Date: 2021-02-12 11:58:07 AM ******/
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
	[Keyword] nvarchar(500) NULL,
	[CreatedOn] [datetime2](0) NOT NULL,
	[ModifiedOn] [datetime2](0) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_EmailTemplate] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmailTemplateTypeLookup]    Script Date: Script Date: 2021-02-12 11:58:07 AM ******/
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
/****** Object:  Table [dbo].[JobOrderEmails]    Script Date: 12-02-2021 14:06:58 ******/
CREATE TABLE [dbo].[JobOrderEmails](
	[JobID] [bigint] NOT NULL,
	[EmailID] [bigint] NOT NULL,
 CONSTRAINT [PK_JobOrderEmails] PRIMARY KEY CLUSTERED 
(
	[JobID] ASC,
	[EmailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
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
/****** Object:  Table [dbo].[JobOrderActionHistory]    Script Date: 28/1/2021 5:24:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JobOrderActionHistory](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[OfficerID] [uniqueidentifier] NOT NULL,
	[Action] [smallint] NOT NULL,
	[JobID] [bigint] NOT NULL,
	[Remarks] [nvarchar](500) NULL,
	[CreatedOn] [datetime2](0) NOT NULL,
 CONSTRAINT [PK_JobOrderActionHistory] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[JobOrderActionLookup]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JobOrderActionLookup](
	[ID] [smallint] NOT NULL,
	[Text] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_JobOrderActionLookup] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Attachment] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Customer] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Findings] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[JobOrder] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[JobOrderLineItem] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Master] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Officer] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Certificate] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Premise] ADD  DEFAULT ((0)) FOR [IsHighPriority]
GO
ALTER TABLE [dbo].[ContactInfo] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO

ALTER TABLE [dbo].[Person]  WITH CHECK ADD FOREIGN KEY([IDType])
REFERENCES [dbo].[IDTypeLookup] ([ID])
GO
ALTER TABLE [dbo].[AttendeeSignature]  WITH CHECK ADD FOREIGN KEY([AttachmentID])
REFERENCES [dbo].[Attachment] ([ID])
GO
ALTER TABLE [dbo].[AttendeeSignature]  WITH CHECK ADD FOREIGN KEY([AttendeeID])
REFERENCES [dbo].[Attendee] ([ID])
GO
ALTER TABLE [dbo].[Findings]  WITH CHECK ADD FOREIGN KEY([JobID])
REFERENCES [dbo].[JobOrder] ([ID])
GO
ALTER TABLE [dbo].[Findings]  WITH CHECK ADD FOREIGN KEY([OfficerID])
REFERENCES [dbo].[Officer] ([ID])
GO
ALTER TABLE [dbo].[FindingsLineItem]  WITH CHECK ADD FOREIGN KEY([FindingsID])
REFERENCES [dbo].[Findings] ([ID])
GO
ALTER TABLE [dbo].[FindingsLineItem]  WITH CHECK ADD FOREIGN KEY([Scheme])
REFERENCES [dbo].[SchemeLookup] ([ID])
GO
ALTER TABLE [dbo].[FindingsLineItem]  WITH CHECK ADD FOREIGN KEY([SubScheme])
REFERENCES [dbo].[SubSchemeLookup] ([ID])
GO
ALTER TABLE [dbo].[FindingsSignature ]  WITH CHECK ADD FOREIGN KEY([AttachmentID])
REFERENCES [dbo].[Attachment] ([ID])
GO
ALTER TABLE [dbo].[FindingsSignature ]  WITH CHECK ADD FOREIGN KEY([FindingsID])
REFERENCES [dbo].[Findings] ([ID])
GO
ALTER TABLE [dbo].[Invitees]  WITH CHECK ADD FOREIGN KEY([JobID])
REFERENCES [dbo].[JobOrder] ([ID])
GO
ALTER TABLE [dbo].[Invitees]  WITH CHECK ADD FOREIGN KEY([OfficerID])
REFERENCES [dbo].[Officer] ([ID])
GO
ALTER TABLE [dbo].[JobOrder]  WITH CHECK ADD FOREIGN KEY([AssignedTo])
REFERENCES [dbo].[Officer] ([ID])
GO
ALTER TABLE [dbo].[JobOrder]  WITH CHECK ADD FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customer] ([ID])
GO
ALTER TABLE [dbo].[JobOrder]  WITH CHECK ADD FOREIGN KEY([Status])
REFERENCES [dbo].[JobOrderStatusLookup] ([ID])
GO
ALTER TABLE [dbo].[JobOrder]  WITH CHECK ADD FOREIGN KEY([Type])
REFERENCES [dbo].[JobOrderTypeLookup] ([ID])
GO
ALTER TABLE [dbo].[JobOrderLineItem]  WITH CHECK ADD FOREIGN KEY([JobID])
REFERENCES [dbo].[JobOrder] ([ID])
GO
ALTER TABLE [dbo].[JobOrderLineItem]  WITH CHECK ADD FOREIGN KEY([Scheme])
REFERENCES [dbo].[SchemeLookup] ([ID])
GO
ALTER TABLE [dbo].[JobOrderLineItem]  WITH CHECK ADD FOREIGN KEY([SubScheme])
REFERENCES [dbo].[SubSchemeLookup] ([ID])
GO
ALTER TABLE [dbo].[JobOrderLineItemAttachments]  WITH CHECK ADD FOREIGN KEY([AttachmentID])
REFERENCES [dbo].[Attachment] ([ID])
GO
ALTER TABLE [dbo].[JobOrderLineItemAttachments]  WITH CHECK ADD FOREIGN KEY([LineItemID])
REFERENCES [dbo].[JobOrderLineItem] ([ID])
GO
ALTER TABLE [dbo].[JobOrderLog]  WITH CHECK ADD FOREIGN KEY([JobID])
REFERENCES [dbo].[JobOrder] ([ID])
GO
ALTER TABLE [dbo].[JobOrderLog]  WITH CHECK ADD FOREIGN KEY([LogID])
REFERENCES [dbo].[Log] ([ID])
GO
ALTER TABLE [dbo].[JobOrderPremises]  WITH CHECK ADD FOREIGN KEY([JobID])
REFERENCES [dbo].[JobOrder] ([ID])
GO
ALTER TABLE [dbo].[JobOrderPremises]  WITH CHECK ADD FOREIGN KEY([PremiseID])
REFERENCES [dbo].[Premise] ([ID])
GO
ALTER TABLE [dbo].[Master]  WITH CHECK ADD FOREIGN KEY([Type])
REFERENCES [dbo].[MasterTypeLookup] ([ID])
GO
ALTER TABLE [dbo].[RescheduleHistory]  WITH CHECK ADD FOREIGN KEY([JobID])
REFERENCES [dbo].[JobOrder] ([ID])
GO
ALTER TABLE [dbo].[Settings]  WITH CHECK ADD FOREIGN KEY([Type])
REFERENCES [dbo].[SettingsLookup] ([ID])
GO
ALTER TABLE [dbo].[SettingsLog]  WITH CHECK ADD FOREIGN KEY([LogID])
REFERENCES [dbo].[Log] ([ID])
GO
ALTER TABLE [dbo].[SettingsLog]  WITH CHECK ADD FOREIGN KEY([SettingsID])
REFERENCES [dbo].[Settings] ([ID])
GO
ALTER TABLE [dbo].[Translations]  WITH CHECK ADD FOREIGN KEY([Locale])
REFERENCES [dbo].[Locale] ([ID])
GO
ALTER TABLE [dbo].[Notes] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Notes]  WITH CHECK ADD FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Officer] ([ID])
GO
ALTER TABLE [dbo].[Notes]  WITH CHECK ADD FOREIGN KEY([JobOrderID])
REFERENCES [dbo].[JobOrder] ([ID])
GO
ALTER TABLE [dbo].[NotesAttachments]  WITH CHECK ADD FOREIGN KEY([AttachmentID])
REFERENCES [dbo].[Attachment] ([ID])
GO
ALTER TABLE [dbo].[NotesAttachments]  WITH CHECK ADD FOREIGN KEY([NotesID])
REFERENCES [dbo].[Notes] ([ID])
GO
ALTER TABLE [dbo].[PeriodicScheduler]  WITH CHECK ADD FOREIGN KEY([Status])
REFERENCES [dbo].[PeriodicSchedulerStatusLookup] ([ID])
GO
ALTER TABLE [dbo].[Certificate]  WITH CHECK ADD  FOREIGN KEY([PremiseID])
REFERENCES [dbo].[Premise] ([ID])
GO
ALTER TABLE [dbo].[Certificate]  WITH CHECK ADD  FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customer] ([ID])
GO
ALTER TABLE [dbo].[Certificate]  WITH CHECK ADD  FOREIGN KEY([Status])
REFERENCES [dbo].[CertificateStatusLookup] ([ID])
GO
ALTER TABLE [dbo].[EmailTemplate]  WITH CHECK ADD FOREIGN KEY([Type])
REFERENCES [dbo].[EmailTemplateTypeLookup] ([ID])
GO
ALTER TABLE [dbo].[JobOrderEmails]  WITH CHECK ADD FOREIGN KEY([JobID])
REFERENCES [dbo].[JobOrder] ([ID])
GO
ALTER TABLE [dbo].[JobOrderEmails]  WITH CHECK ADD FOREIGN KEY([EmailID])
REFERENCES [dbo].[Email] ([ID])
GO
ALTER TABLE [dbo].[JobOrder] ADD FOREIGN KEY([ContactPersonID])
REFERENCES [dbo].[Person] ([ID])
GO
ALTER TABLE [dbo].[PersonContacts]  WITH CHECK ADD FOREIGN KEY([ContactID])
REFERENCES [dbo].[ContactInfo] ([ID])
GO
ALTER TABLE [dbo].[ContactInfo]  WITH CHECK ADD FOREIGN KEY([Type])
REFERENCES [dbo].[ContactInfoTypeLookup] ([ID])
GO
ALTER TABLE [dbo].[PersonContacts]  WITH CHECK ADD FOREIGN KEY([ContactID])
REFERENCES [dbo].[ContactInfo] ([ID])
GO
ALTER TABLE [dbo].[JobOrderActionHistory]  WITH CHECK ADD FOREIGN KEY([Action])
REFERENCES [dbo].[JobOrderActionLookup] ([ID])
GO
ALTER TABLE [dbo].[JobOrderActionHistory]  WITH CHECK ADD FOREIGN KEY([OfficerID])
REFERENCES [dbo].[Officer] ([ID])
GO
ALTER TABLE [dbo].[JobOrderActionHistory]  WITH CHECK ADD FOREIGN KEY([JobID])
REFERENCES [dbo].[JobOrder] ([ID])
GO