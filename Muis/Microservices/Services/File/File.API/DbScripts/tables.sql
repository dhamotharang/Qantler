GO
/****** Object:  Table [dbo].[File]    Script Date: 2020-11-19 10:33:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[File](
	[ID] [uniqueidentifier] NOT NULL,
	[FileName] [nvarchar](100) NOT NULL,
	[Directory] [nvarchar](255) NOT NULL,
	[Extension] [varchar](10) NOT NULL,
	[Size] [bigint] NOT NULL,
	[CreatedOn] [datetime2](0) NULL,
	[isDeleted] [bit] NOT NULL,
	[ModifiedOn] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[File] ADD  DEFAULT ((0)) FOR [isDeleted]
GO
