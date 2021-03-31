GO
/****** Object:  Table [dbo].[Account]    Script Date: 2020-11-19 10:39:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Account](
	[ID] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](150) NOT NULL,
	[Balance] [decimal](12, 2) NOT NULL,
 CONSTRAINT [PK_Account] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Bill]    Script Date: 2020-11-19 10:39:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Bill](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[RefNo] [varchar](36) NOT NULL,
	[Status] [smallint] NOT NULL,
	[Type] [smallint] NOT NULL,
	[RequestType] [smallint] NULL,
	[AccountID] [uniqueidentifier] NOT NULL,
	[InvoiceNo] [varchar](36) NULL,
	[Amount] [decimal](12, 2) NOT NULL,
	[GSTAmount] [decimal](12, 2) NOT NULL,
	[GST] [decimal](12, 2) NOT NULL,
	[IssuedOn] [datetime2](0) NOT NULL,
	[DueOn] [datetime2](0) NULL,
	[RequestID] [bigint] NULL,
	[RefID] [varchar](36) NULL,
	[CreatedOn] [datetime2](0) NOT NULL,
	[ModifiedOn] [datetime2](0) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Bill] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BillLineItem]    Script Date: 2020-11-19 10:39:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BillLineItem](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[SectionIndex] [smallint] NOT NULL,
	[Section] [nvarchar](150) NOT NULL,
	[Index] [smallint] NOT NULL,
	[CodeID] [bigint] NULL,
	[Code] [varchar](15) NULL,
	[Descr] [nvarchar](255) NOT NULL,
	[Qty] [decimal](12, 2) NULL,
	[UnitPrice] [decimal](12, 2) NULL,
	[Amount] [decimal](12, 2) NOT NULL,
	[GSTAmount] [decimal](12, 2) NOT NULL,
	[GST] [decimal](12, 2) NOT NULL,
	[WillRecord] [bit] NOT NULL,
	[BillID] [bigint] NOT NULL,
	[CreatedOn] [datetime2](0) NOT NULL,
	[ModifiedOn] [datetime2](0) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_BillLineItem] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BillRequestTypeLookup]    Script Date: 2020-11-19 10:39:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BillRequestTypeLookup](
	[ID] [smallint] NOT NULL,
	[Text] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_BillRequestTypeLookup] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BillStatusLookup]    Script Date: 2020-11-19 10:39:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BillStatusLookup](
	[ID] [smallint] NOT NULL,
	[Text] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_BillStatusLookup] PRIMARY KEY CLUSTERED 
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
/****** Object:  Table [dbo].[Condition]    Script Date: 2020-11-19 10:39:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Condition](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Index] [smallint] NOT NULL,
	[Value] [nvarchar](255) NULL,
	[Field] [smallint] NOT NULL,
	[Operator] [smallint] NOT NULL,
	[Logical] [smallint] NOT NULL,
	[TransactionCodeID] [bigint] NOT NULL,
	[CreatedOn] [datetime2](0) NOT NULL,
	[ModifiedOn] [datetime2](0) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Condition] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FieldLookup]    Script Date: 2020-11-19 10:39:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FieldLookup](
	[ID] [smallint] NOT NULL,
	[Text] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_FieldLookup] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Locale]    Script Date: 2020-11-19 10:39:42 AM ******/
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
/****** Object:  Table [dbo].[Log]    Script Date: 2020-11-19 10:39:42 AM ******/
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
/****** Object:  Table [dbo].[LogicalLookup]    Script Date: 2020-11-19 10:39:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LogicalLookup](
	[ID] [smallint] NOT NULL,
	[Text] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_LogicalLookup] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LogTypeLookup]    Script Date: 2020-11-19 10:39:42 AM ******/
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
/****** Object:  Table [dbo].[Officer]    Script Date: 2020-11-19 10:39:42 AM ******/
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
/****** Object:  Table [dbo].[OperatorLookup]    Script Date: 2020-11-19 10:39:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OperatorLookup](
	[ID] [smallint] NOT NULL,
	[Text] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_OperatorLookup] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Payment]    Script Date: 2020-11-19 10:39:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Payment](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[AltID] [varchar](36) NULL,
	[RefNo] [varchar](36) NULL,
	[Status] [smallint] NOT NULL,
	[Mode] [smallint] NOT NULL,
	[Method] [smallint] NOT NULL,
	[AccountID] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](150) NOT NULL,
	[TransactionNo] [varchar](36) NOT NULL,
	[ReceiptNo] [varchar](36) NULL,
	[Amount] [decimal](12, 2) NOT NULL,
	[GSTAmount] [decimal](12, 2) NOT NULL,
	[GST] [decimal](12, 2) NOT NULL,
	[PaidOn] [datetime2](0) NOT NULL,
	[ProcessedBy] [uniqueidentifier] NULL,
	[ProcessedOn] [datetime2](0) NULL,
	[ContactPersonID] [UniqueIdentifier] NULL,
	[CreatedOn] [datetime2](0) NOT NULL,
	[ModifiedOn] [datetime2](0) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Payment] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PaymentBills]    Script Date: 2020-11-19 10:39:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PaymentBills](
	[PaymentID] [bigint] NOT NULL,
	[BillID] [bigint] NOT NULL,
 CONSTRAINT [PK_PaymentBills] PRIMARY KEY CLUSTERED 
(
	[PaymentID] ASC,
	[BillID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PaymentMethodLookup]    Script Date: 2020-11-19 10:39:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PaymentMethodLookup](
	[ID] [smallint] NOT NULL,
	[Text] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_PaymentMethodLookup] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PaymentModeLookup]    Script Date: 2020-11-19 10:39:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PaymentModeLookup](
	[ID] [smallint] NOT NULL,
	[Text] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_PaymentModeLookup] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PaymentStatusLookup]    Script Date: 2020-11-19 10:39:42 AM ******/
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
/****** Object:  Table [dbo].[Price]    Script Date: 2020-11-19 10:39:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Price](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Amount] [decimal](12, 2) NULL,
	[EffectiveFrom] [datetime2](0) NOT NULL,
	[TransactionCodeID] [bigint] NOT NULL,
	[CreatedOn] [datetime2](0) NOT NULL,
	[ModifiedOn] [datetime2](0) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Price] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Settings]    Script Date: 2020-11-19 10:39:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Settings](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Type] [smallint] NOT NULL,
	[Value] [nvarchar](4000) NULL,
	[Text] [nvarchar](255) NOT NULL,
	[DataType] [varchar](50) NOT NULL,
	[CreatedOn] [datetime2](0) NOT NULL,
	[ModifiedOn] [datetime2](0) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Settings] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SettingsLookup]    Script Date: 2020-11-19 10:39:42 AM ******/
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
/****** Object:  Table [dbo].[TransactionCode]    Script Date: 2020-11-19 10:39:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TransactionCode](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Code] [varchar](15) NOT NULL,
	[GLEntry] [varchar](36) NULL,
	[Text] [nvarchar](2000) NOT NULL,
	[Currency] [varchar](5) NULL,
	[IsBillable] [bit] NOT NULL,
	[CreatedOn] [datetime2](0) NOT NULL,
	[ModifiedOn] [datetime2](0) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_TransactionCode] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TransactionCodeLog]    Script Date: 2020-11-19 10:39:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TransactionCodeLog](
	[TransactionCodeID] [bigint] NOT NULL,
	[LogID] [bigint] NOT NULL,
 CONSTRAINT [PK_TransactionCodeLog] PRIMARY KEY CLUSTERED 
(
	[TransactionCodeID] ASC,
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Translations]    Script Date: 2020-11-19 10:39:42 AM ******/
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
/****** Object:  Table [dbo].[Notes]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notes](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Text] [nvarchar](4000) NOT NULL,
	[CreatedBy] [uniqueidentifier] NOT NULL,
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
/****** Object:  Table [dbo].[PaymentNotes]    Script Date: 12-02-2021 14:06:58 ******/
CREATE TABLE [dbo].[PaymentNotes](
	[PayID] [bigint] NOT NULL,
	[NoteID] [bigint] NOT NULL,
 CONSTRAINT [PK_PaymentNotes] PRIMARY KEY CLUSTERED 
(
	[PayID] ASC,
	[NoteID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
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
/****** Object:  Table [dbo].[PaymentLog]    Script Date: 2020-11-19 11:58:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PaymentLogs](
	[PaymentID] [bigint] NOT NULL,
	[LogID] [bigint] NOT NULL,
 CONSTRAINT [PK_PaymentLogs] PRIMARY KEY CLUSTERED 
(
	[PaymentID] ASC,
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
/****** Object:  Table [dbo].[Person]    Script Date: 2020-11-19 11:07:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Person](
	[ID] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](150) NULL,
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
/****** Object:  Table [dbo].[BillRequestTypeLookup]    Script Date: 2020-11-19 10:39:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DDAStatusLookup](
	[ID] [smallint] NOT NULL,
	[Text] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_DDAStatusLookup] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Bank]    Script Date: 2020-11-19 11:07:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Bank](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[AccountNo] [nvarchar](60) NULL,
	[AccountName] [nvarchar](150) NULL,
	[BranchCode] [nvarchar](15) NULL,
	[BankName] [nvarchar](150) NULL,
	[DDAStatus] [smallint] NOT NULL,
	[CreatedOn] [datetime2](0) NOT NULL,
	[ModifiedOn] [datetime2](0) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Bank] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PaymentBanks]    Script Date: 2020-11-19 11:07:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PaymentBanks](
	[PaymentID] [bigint] NOT NULL,
	[BankID] [bigint] NOT NULL,
 CONSTRAINT [PK_PaymentBanks] PRIMARY KEY CLUSTERED 
(
	[PaymentID] ASC,
	[BankID] ASC
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

ALTER TABLE [dbo].[Master] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Attachment] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Notes] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Bill] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Condition] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Log] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Payment] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Price] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[TransactionCode] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Bank] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO

ALTER TABLE [dbo].[PaymentBanks]  WITH CHECK ADD FOREIGN KEY([PaymentID])
REFERENCES [dbo].[Payment] ([ID])
GO
ALTER TABLE [dbo].[PaymentBanks]  WITH CHECK ADD FOREIGN KEY([BankID])
REFERENCES [dbo].[Bank] ([ID])
GO
ALTER TABLE [dbo].[Bank]  WITH CHECK ADD FOREIGN KEY([DDAStatus])
REFERENCES [dbo].[DDAStatusLookup] ([ID])
GO
ALTER TABLE [dbo].[PaymentLogs]  WITH CHECK ADD FOREIGN KEY([LogID])
REFERENCES [dbo].[Log] ([ID])
GO
ALTER TABLE [dbo].[PaymentLogs]  WITH CHECK ADD FOREIGN KEY([PaymentID])
REFERENCES [dbo].[Payment] ([ID])
GO
ALTER TABLE [dbo].[Translations]  WITH CHECK ADD FOREIGN KEY([Locale])
REFERENCES [dbo].[Locale] ([ID])
GO
ALTER TABLE [dbo].[Bill]  WITH CHECK ADD FOREIGN KEY([AccountID])
REFERENCES [dbo].[Account] ([ID])
GO
ALTER TABLE [dbo].[Bill]  WITH CHECK ADD FOREIGN KEY([RequestType])
REFERENCES [dbo].[BillRequestTypeLookup] ([ID])
GO
ALTER TABLE [dbo].[Bill]  WITH CHECK ADD FOREIGN KEY([Status])
REFERENCES [dbo].[BillStatusLookup] ([ID])
GO
ALTER TABLE [dbo].[Bill]  WITH CHECK ADD FOREIGN KEY([Type])
REFERENCES [dbo].[BillTypeLookup] ([ID])
GO
ALTER TABLE [dbo].[BillLineItem]  WITH CHECK ADD FOREIGN KEY([BillID])
REFERENCES [dbo].[Bill] ([ID])
GO
ALTER TABLE [dbo].[Condition]  WITH CHECK ADD FOREIGN KEY([Field])
REFERENCES [dbo].[FieldLookup] ([ID])
GO
ALTER TABLE [dbo].[Condition]  WITH CHECK ADD FOREIGN KEY([Logical])
REFERENCES [dbo].[LogicalLookup] ([ID])
GO
ALTER TABLE [dbo].[Condition]  WITH CHECK ADD FOREIGN KEY([Operator])
REFERENCES [dbo].[OperatorLookup] ([ID])
GO
ALTER TABLE [dbo].[Condition]  WITH CHECK ADD FOREIGN KEY([TransactionCodeID])
REFERENCES [dbo].[TransactionCode] ([ID])
GO
ALTER TABLE [dbo].[Log]  WITH CHECK ADD FOREIGN KEY([Type])
REFERENCES [dbo].[LogTypeLookup] ([ID])
GO
ALTER TABLE [dbo].[Payment]  WITH CHECK ADD FOREIGN KEY([AccountID])
REFERENCES [dbo].[Account] ([ID])
GO
ALTER TABLE [dbo].[Payment]  WITH CHECK ADD FOREIGN KEY([Mode])
REFERENCES [dbo].[PaymentModeLookup] ([ID])
GO
ALTER TABLE [dbo].[Payment]  WITH CHECK ADD FOREIGN KEY([Mode])
REFERENCES [dbo].[PaymentMethodLookup] ([ID])
GO
ALTER TABLE [dbo].[Payment]  WITH CHECK ADD FOREIGN KEY([ProcessedBy])
REFERENCES [dbo].[Officer] ([ID])
GO
ALTER TABLE [dbo].[Payment]  WITH CHECK ADD FOREIGN KEY([ProcessedBy])
REFERENCES [dbo].[Officer] ([ID])
GO
ALTER TABLE [dbo].[Payment]  WITH CHECK ADD FOREIGN KEY([Status])
REFERENCES [dbo].[PaymentStatusLookup] ([ID])
GO
ALTER TABLE [dbo].[PaymentBills]  WITH CHECK ADD FOREIGN KEY([BillID])
REFERENCES [dbo].[Bill] ([ID])
GO
ALTER TABLE [dbo].[PaymentBills]  WITH CHECK ADD FOREIGN KEY([PaymentID])
REFERENCES [dbo].[Payment] ([ID])
GO
ALTER TABLE [dbo].[Price]  WITH CHECK ADD FOREIGN KEY([TransactionCodeID])
REFERENCES [dbo].[TransactionCode] ([ID])
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
ALTER TABLE [dbo].[Translations]  WITH CHECK ADD FOREIGN KEY([Locale])
REFERENCES [dbo].[Locale] ([ID])
GO
ALTER TABLE [dbo].[NotesAttachments]  WITH CHECK ADD FOREIGN KEY([AttachmentID])
REFERENCES [dbo].[Attachment] ([ID])
GO
ALTER TABLE [dbo].[NotesAttachments]  WITH CHECK ADD FOREIGN KEY([NotesID])
REFERENCES [dbo].[Notes] ([ID])
GO
ALTER TABLE [dbo].[PaymentNotes]  WITH CHECK ADD FOREIGN KEY([PayID])
REFERENCES [dbo].[Payment] ([ID])
GO
ALTER TABLE [dbo].[PaymentNotes]  WITH CHECK ADD FOREIGN KEY([NoteID])
REFERENCES [dbo].[Notes] ([ID])
GO
ALTER TABLE [dbo].[Payment]  WITH CHECK ADD FOREIGN KEY([ContactPersonID])
REFERENCES [dbo].[Person] ([ID])
GO
ALTER TABLE [dbo].[PersonContacts]  WITH CHECK ADD FOREIGN KEY([ContactID])
REFERENCES [dbo].[ContactInfo] ([ID])
GO
ALTER TABLE [dbo].[PersonContacts]  WITH CHECK ADD FOREIGN KEY([PersonID])
REFERENCES [dbo].[Person] ([ID])
GO
ALTER TABLE [dbo].[ContactInfo]  WITH CHECK ADD FOREIGN KEY([Type])
REFERENCES [dbo].[ContactInfoTypeLookup] ([ID])
GO
ALTER TABLE [dbo].[Master]  WITH CHECK ADD FOREIGN KEY([Type])
REFERENCES [dbo].[MasterTypeLookup] ([ID])
GO