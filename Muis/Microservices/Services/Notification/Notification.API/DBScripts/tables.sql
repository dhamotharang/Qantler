GO
/****** Object:  Table [dbo].[CategoryLookup]    Script Date: 2020-11-19 11:53:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CategoryLookup](
	[ID] [smallint] NOT NULL,
	[Text] [nvarchar](100) NULL,
 CONSTRAINT [PK_CategoryLookup] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ContentTypeLookup]    Script Date: 2020-11-19 11:53:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContentTypeLookup](
	[ID] [smallint] NOT NULL,
	[Text] [nvarchar](100) NULL,
 CONSTRAINT [PK_ContentTypeLookup] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LevelLookup]    Script Date: 2020-11-19 11:53:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LevelLookup](
	[ID] [smallint] NOT NULL,
	[Text] [nvarchar](100) NULL,
 CONSTRAINT [PK_LevelLookup] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Notification]    Script Date: 2020-11-19 11:53:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notification](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](100) NULL,
	[Preview] [nvarchar](1024) NULL,
	[Body] [nvarchar](2000) NULL,
	[RefID] [nvarchar](36) NULL,
	[Module] [nvarchar](36) NULL,
	[Category] [smallint] NOT NULL,
	[Level] [smallint] NOT NULL,
	[ContentType] [smallint] NOT NULL,
	[CreatedOn] [datetime2](0) NOT NULL,
 CONSTRAINT [PK_Notification] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PushToken]    Script Date: 2020-11-19 11:53:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PushToken](
	[Token] [varchar](255) NOT NULL,
	[DeviceID] [varchar](36) NOT NULL,
	[UserID] [uniqueidentifier] NOT NULL,
	[CreatedOn] [datetime2](0) NULL,
 CONSTRAINT [PK_PushToken] PRIMARY KEY CLUSTERED 
(
	[DeviceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StateLookup]    Script Date: 2020-11-19 11:53:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StateLookup](
	[ID] [smallint] NOT NULL,
	[Text] [nvarchar](100) NULL,
 CONSTRAINT [PK_StateLookup] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserNotification]    Script Date: 2020-11-19 11:53:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserNotification](
	[UserID] [uniqueidentifier] NOT NULL,
	[NotificationID] [bigint] NOT NULL,
	[State] [smallint] NOT NULL,
	[ModifiedOn] [datetime2](0) NULL,
 CONSTRAINT [PK_UserNotification] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC,
	[NotificationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Notification]  WITH CHECK ADD FOREIGN KEY([Category])
REFERENCES [dbo].[CategoryLookup] ([ID])
GO
ALTER TABLE [dbo].[Notification]  WITH CHECK ADD FOREIGN KEY([ContentType])
REFERENCES [dbo].[ContentTypeLookup] ([ID])
GO
ALTER TABLE [dbo].[Notification]  WITH CHECK ADD FOREIGN KEY([Level])
REFERENCES [dbo].[LevelLookup] ([ID])
GO
ALTER TABLE [dbo].[UserNotification]  WITH CHECK ADD FOREIGN KEY([State])
REFERENCES [dbo].[StateLookup] ([ID])
GO
