USE [master]
GO

CREATE DATABASE oerdevdb
GO

USE oerdevdb
GO

/****** Object:  UserDefinedTableType [dbo].[UT_AnswerOptions]    Script Date: 09-01-2020 11:30:54 ******/
CREATE TYPE [dbo].[UT_AnswerOptions] AS TABLE(
	[QuestionId] [int] NOT NULL,
	[AnswerId] [int] NOT NULL,
	[OptionText] [nvarchar](max) NULL,
	[CorrectOption] [bit] NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[UT_QRCUsers]    Script Date: 09-01-2020 11:30:54 ******/
CREATE TYPE [dbo].[UT_QRCUsers] AS TABLE(
	[QRCId] [int] NULL,
	[CategoryId] [int] NULL,
	[UserId] [int] NULL,
	[CreatedBy] [int] NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[UT_Ques]    Script Date: 09-01-2020 11:30:54 ******/
CREATE TYPE [dbo].[UT_Ques] AS TABLE(
	[QuestionId] [int] NOT NULL,
	[QuestionText] [nvarchar](max) NULL,
	[Nedia] [varchar](400) NULL,
	[FileName] [varchar](400) NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[UT_Question]    Script Date: 09-01-2020 11:30:54 ******/
CREATE TYPE [dbo].[UT_Question] AS TABLE(
	[QuestionId] [int] NOT NULL,
	[QuestionText] [nvarchar](max) NULL,
	[Media] [varchar](400) NULL,
	[FileName] [varchar](400) NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[UT_Questions]    Script Date: 09-01-2020 11:30:54 ******/
CREATE TYPE [dbo].[UT_Questions] AS TABLE(
	[QuestionId] [int] NOT NULL,
	[QuestionText] [nvarchar](max) NULL,
	[Media] [nvarchar](200) NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[UT_QuestionsForContent]    Script Date: 09-01-2020 11:30:54 ******/
CREATE TYPE [dbo].[UT_QuestionsForContent] AS TABLE(
	[QuestionId] [int] NOT NULL,
	[QuestionText] [nvarchar](max) NULL,
	[Media] [nvarchar](400) NULL,
	[FileName] [nvarchar](400) NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[UT_Resource]    Script Date: 09-01-2020 11:30:54 ******/
CREATE TYPE [dbo].[UT_Resource] AS TABLE(
	[Id] [int] NOT NULL,
	[ResourceId] [int] NOT NULL,
	[SectionId] [int] NOT NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[UT_Sections]    Script Date: 09-01-2020 11:30:54 ******/
CREATE TYPE [dbo].[UT_Sections] AS TABLE(
	[Id] [int] NOT NULL,
	[Name] [nvarchar](100) NULL
)
GO
/****** Object:  UserDefinedFunction [dbo].[StringSplit]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[StringSplit]
(
    @String  VARCHAR(MAX), @Separator CHAR(1)
)
RETURNS @RESULT TABLE(Value VARCHAR(MAX))
AS
BEGIN     
 DECLARE @SeparatorPosition INT = CHARINDEX(@Separator, @String ),
        @Value VARCHAR(MAX), @StartPosition INT = 1
 
 IF @SeparatorPosition = 0  
  BEGIN
   INSERT INTO @RESULT VALUES(@String)
   RETURN
  END
     
 SET @String = @String + @Separator
 WHILE @SeparatorPosition > 0
  BEGIN
   SET @Value = SUBSTRING(@String , @StartPosition, @SeparatorPosition- @StartPosition)
 
   IF( @Value <> ''  ) 
    INSERT INTO @RESULT VALUES(@Value)
   
   SET @StartPosition = @SeparatorPosition + 1
   SET @SeparatorPosition = CHARINDEX(@Separator, @String , @StartPosition)
  END    
     
 RETURN
END
GO
/****** Object:  Table [dbo].[UserMaster]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserMaster](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TitleId] [int] NULL,
	[FirstName] [nvarchar](250) NOT NULL,
	[MiddleName] [nvarchar](250) NULL,
	[LastName] [nvarchar](250) NULL,
	[CountryId] [int] NULL,
	[StateId] [int] NULL,
	[Gender] [int] NULL,
	[Email] [nvarchar](250) NOT NULL,
	[PortalLanguageId] [int] NULL,
	[DepartmentId] [int] NULL,
	[DesignationId] [int] NULL,
	[DateOfBirth] [date] NULL,
	[Photo] [nvarchar](200) NULL,
	[ProfileDescription] [nvarchar](4000) NULL,
	[SubjectsInterested] [nvarchar](max) NULL,
	[ApprovalStatus] [bit] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[UpdatedOn] [datetime] NULL,
	[Active] [bit] NOT NULL,
	[IsContributor] [bit] NULL,
	[IsAdmin] [bit] NULL,
	[IsEmailNotification] [bit] NULL,
	[LastLogin] [datetime] NULL,
	[Theme] [nvarchar](50) NOT NULL,
	[IsVerifier] [bit] NULL,
 CONSTRAINT [PK_AdminUser] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CategoryMaster]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CategoryMaster](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[UpdatedBy] [int] NOT NULL,
	[UpdatedOn] [datetime] NOT NULL,
	[Active] [bit] NOT NULL,
	[Name_Ar] [nvarchar](500) NULL,
	[Status] [bit] NULL,
 CONSTRAINT [PK_CategoryMaster] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CourseMaster]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CourseMaster](
	[Id] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](250) NOT NULL,
	[CategoryId] [int] NOT NULL,
	[SubCategoryId] [int] NULL,
	[Thumbnail] [nvarchar](200) NULL,
	[CourseDescription] [nvarchar](2000) NULL,
	[Keywords] [nvarchar](1500) NULL,
	[CourseContent] [ntext] NULL,
	[CopyRightId] [int] NULL,
	[IsDraft] [bit] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[IsApproved] [bit] NULL,
	[Rating] [float] NOT NULL,
	[ReportAbuseCount] [int] NOT NULL,
	[EducationId] [int] NULL,
	[ProfessionId] [int] NULL,
	[ViewCount] [int] NULL,
	[AverageReadingTime] [int] NULL,
	[DownloadCount] [int] NULL,
	[SharedCount] [int] NULL,
	[ReadingTime] [int] NULL,
	[LastView] [datetime] NULL,
	[LevelId] [int] NULL,
	[EducationalStandardId] [int] NULL,
	[EducationalUseId] [int] NULL,
	[CommunityBadge] [bit] NULL,
	[MoEBadge] [bit] NULL,
	[IsApprovedSensory] [bit] NULL,
 CONSTRAINT [PK_CourseMaster] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CourseRating]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CourseRating](
	[Id] [decimal](18, 0) IDENTITY(1,1) NOT NULL,
	[CourseId] [decimal](18, 0) NULL,
	[RatedBy] [int] NULL,
	[Rating] [float] NULL,
	[RatedOn] [datetime] NULL,
	[Comments] [nvarchar](2000) NULL,
 CONSTRAINT [PK_CourseRating] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CopyrightMaster]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CopyrightMaster](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](100) NOT NULL,
	[Description] [text] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[UpdatedBy] [int] NOT NULL,
	[UpdatedOn] [datetime] NOT NULL,
	[Active] [bit] NOT NULL,
	[Media] [nvarchar](100) NULL,
	[Title_Ar] [nvarchar](200) NULL,
	[Description_Ar] [nvarchar](200) NULL,
	[Protected] [bit] NOT NULL,
	[IsResourceProtect] [bit] NOT NULL,
	[Status] [bit] NULL,
 CONSTRAINT [PK_CopyrightMaster] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CourseEnrollment]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CourseEnrollment](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CourseId] [numeric](18, 0) NULL,
	[UserId] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[UpdatedOn] [datetime] NULL,
	[Active] [bit] NULL,
 CONSTRAINT [PK_CourseEnrollment] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[VCourseMaster]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE  VIEW [dbo].[VCourseMaster]  
AS  
select cm.Id,
cm.Title,
cm.CategoryId,
cat.Name as Category,
cm.SubCategoryId,
cm.Thumbnail,
cm.CourseDescription,
cm.Keywords,
cm.CourseContent,
cm.CopyRightId,
(SELECT Title FROM CopyrightMaster WHERE Id = cm.CopyRightId) As CopyrightTitle,
(SELECT Title_Ar FROM CopyrightMaster WHERE Id = cm.CopyRightId) As CopyrightTitle_Ar,
(SELECT Media FROM CopyrightMaster WHERE Id = cm.CopyRightId) As CopyrightMedia,
(SELECT [Description] FROM CopyrightMaster WHERE Id = cm.CopyRightId) As CopyrightDescription,
(SELECT Description_Ar FROM CopyrightMaster WHERE Id = cm.CopyRightId) As CopyrightDescription_Ar,
cm.IsDraft,
u.FirstName +' '+ u.LastName as CreatedByName,
cm.CreatedBy,
cm.CreatedOn,
cm.IsApproved,
cm.Rating,
cm.ReportAbuseCount,
cm.EducationId,
cm.ProfessionId,
cm.ViewCount,
cm.AverageReadingTime,
cm.DownloadCount,
cm.SharedCount,
cm.ReadingTime,
cm.LastView,
cm.LevelId,
cat.Name_Ar,
cat.Name,
(SELECT Count(CourseId) from CourseEnrollment WHERE Courseid = cm.Id and Active = 1) As enrollmentcount
,(SELECT CONVERT(INT,Rating) AS Star,COUNT(RatedBy) As NoOfUsers
from [CourseRating] where courseid = cm.Id
GROUP BY Rating FOR JSON AUTO) AS AllRating,
cm.EducationalStandardId,
cm.EducationalUseId,
cm.CommunityBadge,
cm.MoEBadge from CourseMaster cm
INNER JOIN UserMaster u ON cm.CreatedBy = u.id
INNER JOIN CategoryMaster cat ON cm.CategoryId = cat.Id
GO
/****** Object:  Table [dbo].[ResourceRating]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ResourceRating](
	[Id] [decimal](18, 0) IDENTITY(1,1) NOT NULL,
	[ResourceId] [decimal](18, 0) NULL,
	[RatedBy] [int] NULL,
	[Rating] [float] NULL,
	[RatedOn] [datetime] NULL,
	[Comments] [nvarchar](2000) NULL,
 CONSTRAINT [PK_ResourceRating] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ResourceMaster]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ResourceMaster](
	[Id] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](250) NOT NULL,
	[CategoryId] [int] NOT NULL,
	[SubCategoryId] [int] NULL,
	[Thumbnail] [nvarchar](200) NULL,
	[ResourceDescription] [nvarchar](2000) NULL,
	[Keywords] [nvarchar](1500) NULL,
	[ResourceContent] [ntext] NULL,
	[MaterialTypeId] [int] NULL,
	[CopyRightId] [int] NULL,
	[IsDraft] [bit] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[IsApproved] [bit] NULL,
	[Rating] [float] NOT NULL,
	[AlignmentRating] [float] NOT NULL,
	[ReportAbuseCount] [int] NOT NULL,
	[UrlSegment] [nvarchar](500) NULL,
	[ViewCount] [int] NULL,
	[AverageReadingTime] [int] NULL,
	[DownloadCount] [int] NULL,
	[SharedCount] [int] NULL,
	[ReadingTime] [int] NULL,
	[Objective] [nvarchar](2000) NULL,
	[LevelId] [int] NULL,
	[EducationalStandardId] [int] NULL,
	[EducationalUseId] [int] NULL,
	[Format] [nvarchar](48) NULL,
	[LastView] [datetime] NULL,
	[CommunityBadge] [bit] NULL,
	[MoEBadge] [bit] NULL,
	[IsApprovedSensory] [bit] NULL,
 CONSTRAINT [PK_ResourceMaster] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  View [dbo].[VResourceMaster]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[VResourceMaster]  
AS  

SELECT 
rm.Id,
rm.Title,
rm.CategoryId,
rm.SubCategoryId,
rm.Thumbnail,
rm.ResourceDescription,
rm.Keywords,
rm.ResourceContent,
rm.MaterialTypeId,
rm.CopyRightId,
(SELECT Title FROM CopyrightMaster WHERE Id = rm.CopyRightId) As CopyrightTitle,
(SELECT Title_Ar FROM CopyrightMaster WHERE Id = rm.CopyRightId) As CopyrightTitle_Ar,
(SELECT Media FROM CopyrightMaster WHERE Id = rm.CopyRightId) As CopyrightMedia,
(SELECT [Description] FROM CopyrightMaster WHERE Id = rm.CopyRightId) As CopyrightDescription,
(SELECT Description_Ar FROM CopyrightMaster WHERE Id = rm.CopyRightId) As CopyrightDescription_Ar,
rm.IsDraft,
rm.CreatedBy,
rm.CreatedOn,
rm.IsApproved,
rm.Rating,
rm.AlignmentRating,
rm.ReportAbuseCount,
rm.UrlSegment,
rm.ViewCount,
rm.AverageReadingTime,
rm.DownloadCount,
rm.SharedCount,
rm.ReadingTime,
rm.Objective,
rm.LevelId,
cat.Name_Ar,
cat.Name,
(SELECT CONVERT(INT,Rating) AS Star,COUNT(RatedBy) As NoOfUsers
from [ResourceRating] where resourceid = rm.Id
GROUP BY Rating FOR JSON AUTO) AS AllRating,
rm.EducationalStandardId,
rm.EducationalUseId,
rm.[Format],
rm.LastView,
cat.Name as Category,
u.FirstName +' '+ u.LastName as CreatedByName,
rm.MoEBadge,
rm.CommunityBadge from ResourceMaster rm
INNER JOIN UserMaster u ON rm.CreatedBy = u.id
INNER JOIN CategoryMaster cat ON rm.CategoryId = cat.Id

GO
/****** Object:  Table [dbo].[Announcements]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Announcements](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Text] [nvarchar](max) NULL,
	[CreatedBy] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedOn] [datetime] NULL,
	[Active] [bit] NULL,
	[Text_Ar] [nvarchar](max) NULL,
 CONSTRAINT [PK_Announcements] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AnswerOptions]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AnswerOptions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[QuestionId] [int] NULL,
	[AnswerOption] [nvarchar](200) NULL,
	[CorrectOption] [bit] NULL,
 CONSTRAINT [PK_AnswerOptions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CommunityApproveRejectCount]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CommunityApproveRejectCount](
	[ApproveCount] [int] NULL,
	[RejectCount] [int] NULL,
	[LastUpdatedBy] [int] NULL,
	[LastUpdatedOn] [datetime] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CommunityCheckMaster]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CommunityCheckMaster](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[ContentId] [int] NULL,
	[ContentType] [int] NULL,
	[Status] [bit] NULL,
	[Comments] [nvarchar](max) NULL,
	[IsCurrent] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ContactUs]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContactUs](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](100) NULL,
	[LastName] [nvarchar](100) NULL,
	[Email] [nvarchar](150) NULL,
	[Telephone] [nvarchar](50) NULL,
	[Subject] [nvarchar](100) NULL,
	[Message] [nvarchar](max) NULL,
	[CreatedOn] [datetime] NULL,
	[IsReplied] [bit] NULL,
	[RepliedBy] [int] NULL,
	[RepliedOn] [datetime] NULL,
	[RepliedText] [nvarchar](max) NULL,
 CONSTRAINT [PK_ContactUs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ContentApproval]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContentApproval](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ContentId] [int] NULL,
	[ContentType] [int] NULL,
	[Status] [int] NULL,
	[Comment] [nvarchar](500) NULL,
	[CreatedBy] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedOn] [datetime] NULL,
	[AssignedTo] [int] NULL,
	[ApprovedBy] [int] NULL,
	[ApprovedDate] [datetime] NULL,
	[AssignedDate] [datetime] NULL,
	[QrcId] [int] NULL,
 CONSTRAINT [PK_ContentApproval] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ContentDownloadInfo]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContentDownloadInfo](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ContentId] [int] NULL,
	[ContentTypeId] [int] NULL,
	[DownloadedBy] [int] NULL,
	[DownloadedOn] [datetime] NULL,
 CONSTRAINT [PK_ContentDownloadInfo] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ContentSharedInfo]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContentSharedInfo](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ContentId] [int] NULL,
	[ContentTypeId] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[SocialMediaName] [varchar](max) NULL,
 CONSTRAINT [PK_ContentSharedInfo] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CountryMaster]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CountryMaster](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](15) NULL,
	[Name] [nvarchar](250) NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[UpdatedBy] [int] NOT NULL,
	[UpdatedOn] [datetime] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[Name_Ar] [nvarchar](500) NULL,
 CONSTRAINT [PK_CountryMaster] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CourseAbuseReports]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CourseAbuseReports](
	[Id] [decimal](18, 0) IDENTITY(1,1) NOT NULL,
	[CourseId] [decimal](18, 0) NULL,
	[ReportedBy] [int] NULL,
	[ReportReasons] [nvarchar](50) NULL,
	[Comments] [nvarchar](2000) NULL,
	[IsHidden] [bit] NULL,
	[Reason] [nvarchar](500) NULL,
	[CreatedDate] [datetime] NULL,
	[UpdateDate] [datetime] NULL,
 CONSTRAINT [PK_CourseAbuseReports] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CourseApprovals]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CourseApprovals](
	[Id] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[CourseId] [numeric](18, 0) NOT NULL,
	[ApprovedBy] [int] NOT NULL,
	[ApprovedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_CourseApprovals] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CourseAssociatedFiles]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CourseAssociatedFiles](
	[Id] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[CourseId] [numeric](18, 0) NOT NULL,
	[AssociatedFile] [nvarchar](200) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[FileName] [nvarchar](500) NULL,
	[IsInclude] [bit] NOT NULL,
 CONSTRAINT [PK_CourseAssociatedFiles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CourseCommentAbuseReports]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CourseCommentAbuseReports](
	[Id] [decimal](18, 0) IDENTITY(1,1) NOT NULL,
	[CourseCommentId] [decimal](18, 0) NULL,
	[ReportedBy] [int] NULL,
	[ReportReasons] [nvarchar](50) NULL,
	[Comments] [nvarchar](2000) NULL,
 CONSTRAINT [PK_CourseCommentAbuseReports] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CourseComments]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CourseComments](
	[Id] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[CourseId] [numeric](18, 0) NOT NULL,
	[Comments] [nvarchar](2000) NOT NULL,
	[UserId] [int] NOT NULL,
	[CommentDate] [datetime] NOT NULL,
	[ReportAbuseCount] [int] NOT NULL,
	[IsHidden] [bit] NULL,
	[Reason] [nvarchar](500) NULL,
	[UpdateDate] [datetime] NULL,
 CONSTRAINT [PK_CourseComments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CourseResourceMapping]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CourseResourceMapping](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CourseId] [numeric](18, 0) NULL,
	[ResourcesId] [numeric](18, 0) NULL,
 CONSTRAINT [PK_CourseResourceMapping] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CourseSections]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CourseSections](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NULL,
	[CourseId] [numeric](18, 0) NULL,
 CONSTRAINT [PK_CourseSections] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CourseURLReferences]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CourseURLReferences](
	[Id] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[CourseId] [numeric](18, 0) NOT NULL,
	[URLReferenceId] [numeric](18, 0) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_CourseURLReferences] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DepartmentMaster]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DepartmentMaster](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[UpdatedBy] [int] NOT NULL,
	[UpdatedOn] [datetime] NOT NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_DepartmentMaster] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DesignationMaster]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DesignationMaster](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[UpdatedBy] [int] NOT NULL,
	[UpdatedOn] [datetime] NOT NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_DesignationMaster] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EducationMaster]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EducationMaster](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](150) NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[UpdatedBy] [int] NOT NULL,
	[UpdatedOn] [datetime] NOT NULL,
	[Active] [bit] NOT NULL,
	[Name_Ar] [nvarchar](400) NULL,
	[Status] [bit] NULL,
 CONSTRAINT [PK_EducationMaster] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GradeMaster]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GradeMaster](
	[Id] [int] NULL,
	[Code] [nvarchar](20) NULL,
	[Description] [nvarchar](200) NULL,
	[Active] [bit] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InstitutionMaster]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InstitutionMaster](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](150) NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[UpdatedBy] [int] NOT NULL,
	[UpdatedOn] [datetime] NOT NULL,
	[Active] [bit] NOT NULL,
	[Name_Ar] [nvarchar](400) NULL,
 CONSTRAINT [PK_InstitutionMaster] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LanguageMaster]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LanguageMaster](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[UpdatedBy] [int] NOT NULL,
	[UpdatedOn] [datetime] NOT NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_LanguageMaster] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LogAction]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LogAction](
	[Id] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[LogModuleId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[ActionId] [int] NOT NULL,
	[ActionDate] [datetime] NOT NULL,
	[ActionDetail] [nvarchar](250) NULL,
 CONSTRAINT [PK_LogAction] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LogActionMaster]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LogActionMaster](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](25) NOT NULL,
 CONSTRAINT [PK_LogActionMaster] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LogError]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LogError](
	[Id] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[LogModuleId] [int] NOT NULL,
	[FunctionName] [nvarchar](100) NULL,
	[Message] [nvarchar](4000) NULL,
	[MessageDate] [datetime] NOT NULL,
 CONSTRAINT [PK_LogError] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LogModuleMaster]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LogModuleMaster](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_LogModuleMaster] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[lu_Educational_Standard]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[lu_Educational_Standard](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Standard] [nvarchar](200) NULL,
	[EducationalUse_Ar] [nvarchar](200) NULL,
	[Active] [bit] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedBy] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[UpdatedOn] [datetime] NULL,
	[Standard_Ar] [nvarchar](200) NULL,
	[Status] [bit] NULL,
 CONSTRAINT [PK_lu_Educational_Standard] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[lu_Educational_Use]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[lu_Educational_Use](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EducationalUse] [nvarchar](200) NULL,
	[EducationalUse_Ar] [nvarchar](200) NULL,
	[CreatedBy] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[UpdatedOn] [datetime] NULL,
	[UpdatedBy] [int] NULL,
	[Active] [bit] NULL,
	[Status] [bit] NULL,
 CONSTRAINT [PK_lu_Educational_Use] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[lu_Level]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[lu_Level](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Level] [nvarchar](100) NULL,
	[Level_Ar] [nvarchar](200) NULL,
	[CreatedBy] [int] NULL,
	[UpdatedOn] [datetime] NULL,
	[CreatedOn] [datetime] NULL,
	[Active] [bit] NULL,
	[UpdatedBy] [int] NULL,
	[Status] [bit] NULL,
 CONSTRAINT [PK_lu_Level] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MaterialTypeMaster]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MaterialTypeMaster](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[UpdatedBy] [int] NOT NULL,
	[UpdatedOn] [datetime] NOT NULL,
	[Active] [bit] NOT NULL,
	[Name_Ar] [nvarchar](200) NULL,
	[Status] [bit] NULL,
 CONSTRAINT [PK_MaterialTypeMaster] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MessageType]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MessageType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Type] [nvarchar](200) NULL,
 CONSTRAINT [PK_MessageType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MoECheckMaster]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MoECheckMaster](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[ContentId] [int] NULL,
	[ContentType] [int] NULL,
	[Status] [bit] NULL,
	[Comments] [nvarchar](max) NULL,
	[IsCurrent] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Notifications]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notifications](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ReferenceId] [int] NULL,
	[ReferenceTypeId] [int] NULL,
	[Subject] [nvarchar](200) NULL,
	[Content] [nvarchar](max) NULL,
	[MessageTypeId] [int] NULL,
	[IsApproved] [bit] NULL,
	[CreatedDate] [datetime] NULL,
	[ReadDate] [datetime] NULL,
	[DeletedDate] [datetime] NULL,
	[IsRead] [bit] NULL,
	[IsDelete] [bit] NULL,
	[UserId] [int] NULL,
	[Reviewer] [int] NULL,
	[Comment] [nvarchar](max) NULL,
	[URL] [nvarchar](400) NULL,
	[Status] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OerConfig]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OerConfig](
	[key] [nvarchar](255) NULL,
	[value] [nvarchar](255) NULL,
	[ConfigType] [nvarchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProfessionMaster]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProfessionMaster](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[UpdatedBy] [int] NOT NULL,
	[UpdatedOn] [datetime] NOT NULL,
	[Active] [bit] NOT NULL,
	[Name_Ar] [nvarchar](400) NULL,
	[Status] [bit] NULL,
 CONSTRAINT [PK_ProfessionMaster] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[QRCCategory]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[QRCCategory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[QRCId] [int] NULL,
	[CategoryId] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[IsAvailable] [bit] NOT NULL,
 CONSTRAINT [PK_QRCCategory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[QRCMaster]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[QRCMaster](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](150) NOT NULL,
	[Description] [nvarchar](2000) NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[UpdatedBy] [int] NOT NULL,
	[UpdatedOn] [datetime] NOT NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_QRCMaster] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[QRCMasterCategory]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[QRCMasterCategory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[QrcId] [int] NOT NULL,
	[CategoryId] [int] NOT NULL,
 CONSTRAINT [PK_QRCMasterCategory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[QRCUserMapping]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[QRCUserMapping](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[QRCId] [int] NULL,
	[CategoryId] [int] NULL,
	[UserId] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[UpdatedOn] [datetime] NULL,
	[Active] [bit] NULL,
 CONSTRAINT [PK_QRCUserMapping] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Questions]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Questions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[QuestionText] [nvarchar](max) NULL,
	[TestId] [int] NULL,
	[Media] [nvarchar](200) NULL,
	[FileName] [nvarchar](400) NULL,
 CONSTRAINT [PK_Questions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ResourceAbuseReports]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ResourceAbuseReports](
	[Id] [decimal](18, 0) IDENTITY(1,1) NOT NULL,
	[ResourceId] [decimal](18, 0) NULL,
	[ReportedBy] [int] NULL,
	[ReportReasons] [nvarchar](50) NULL,
	[Comments] [nvarchar](2000) NULL,
	[IsHidden] [bit] NULL,
	[Reason] [nvarchar](500) NULL,
	[CreatedDate] [datetime] NULL,
	[UpdateDate] [datetime] NULL,
 CONSTRAINT [PK_ResourceAbuseReports] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ResourceAlignmentRating]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ResourceAlignmentRating](
	[Id] [decimal](18, 0) IDENTITY(1,1) NOT NULL,
	[ResourceId] [decimal](18, 0) NOT NULL,
	[CategoryId] [int] NULL,
	[Grade] [nvarchar](50) NULL,
	[RatedBy] [int] NOT NULL,
	[Rating] [float] NOT NULL,
	[RatedOn] [datetime] NOT NULL,
	[LevelId] [int] NULL,
 CONSTRAINT [PK_ResourceAlignmentRating] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ResourceApprovals]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ResourceApprovals](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ResourceId] [numeric](18, 0) NOT NULL,
	[ApprovedBy] [int] NOT NULL,
	[ApprovedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_ResourceApprovals_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ResourceAssociatedFiles]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ResourceAssociatedFiles](
	[Id] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[ResourceId] [numeric](18, 0) NOT NULL,
	[AssociatedFile] [nvarchar](max) NULL,
	[UploadedDate] [datetime] NOT NULL,
	[FileName] [nvarchar](500) NULL,
	[IsInclude] [bit] NOT NULL,
 CONSTRAINT [PK_ResourceAssociatedFiles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ResourceComments]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ResourceComments](
	[Id] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[ResourceId] [numeric](18, 0) NOT NULL,
	[Comments] [nvarchar](2000) NOT NULL,
	[UserId] [int] NOT NULL,
	[CommentDate] [datetime] NOT NULL,
	[ReportAbuseCount] [int] NOT NULL,
	[IsHidden] [bit] NULL,
	[Reason] [nvarchar](500) NULL,
	[UpdateDate] [datetime] NULL,
 CONSTRAINT [PK_ResourceComments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ResourceCommentsAbuseReports]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ResourceCommentsAbuseReports](
	[Id] [decimal](18, 0) IDENTITY(1,1) NOT NULL,
	[ResourceCommentId] [decimal](18, 0) NULL,
	[ReportedBy] [int] NULL,
	[ReportReasons] [nvarchar](50) NULL,
	[Comments] [nvarchar](2000) NULL,
 CONSTRAINT [PK_ResourceCommentAbuseReports] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ResourceRemixHistory]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ResourceRemixHistory](
	[Id] [decimal](18, 0) IDENTITY(1,1) NOT NULL,
	[ResourceSourceID] [decimal](18, 0) NULL,
	[ResourceRemixedID] [decimal](18, 0) NULL,
	[Version] [int] NULL,
	[CreatedOn] [datetime] NULL,
 CONSTRAINT [PK_ResourceRemixHistory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ResourceURLReferences]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ResourceURLReferences](
	[Id] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[ResourceId] [numeric](18, 0) NOT NULL,
	[URLReferenceId] [numeric](18, 0) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_ResourceURLReferences] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SectionResource]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SectionResource](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SectionId] [int] NULL,
	[ResourceId] [numeric](18, 0) NULL,
 CONSTRAINT [PK_SectionResource] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SensoryCheckMaster]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SensoryCheckMaster](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[ContentId] [int] NULL,
	[ContentType] [int] NULL,
	[Status] [bit] NULL,
	[Comments] [nvarchar](max) NULL,
	[IsCurrent] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SocialMediaMaster]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SocialMediaMaster](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedOn] [datetime] NULL,
	[Name_Ar] [nvarchar](max) NULL,
 CONSTRAINT [PK_SocialMediaMaster] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StateMaster]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StateMaster](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CountryId] [int] NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[UpdatedBy] [int] NOT NULL,
	[UpdatedOn] [datetime] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[Name_Ar] [nvarchar](200) NULL,
 CONSTRAINT [PK_StateMaster] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StreamMaster]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StreamMaster](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[UpdatedBy] [int] NOT NULL,
	[UpdatedOn] [datetime] NOT NULL,
	[Active] [bit] NOT NULL,
	[Name_Ar] [nvarchar](500) NULL,
 CONSTRAINT [PK_StreamMaster] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SubCategoryMaster]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SubCategoryMaster](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CategoryId] [int] NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[UpdatedBy] [int] NOT NULL,
	[UpdatedOn] [datetime] NOT NULL,
	[Active] [bit] NOT NULL,
	[Name_Ar] [nvarchar](500) NULL,
	[Status] [bit] NULL,
 CONSTRAINT [PK_SubCategoryMaster] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tests]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tests](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TestName] [nvarchar](100) NULL,
	[CreatedBy] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedOn] [datetime] NULL,
	[CourseId] [numeric](18, 0) NULL,
 CONSTRAINT [PK_Tests] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TitleMaster]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TitleMaster](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[UpdatedBy] [int] NOT NULL,
	[UpdatedOn] [datetime] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[Name_Ar] [nvarchar](200) NULL,
 CONSTRAINT [PK_TitleMaster] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Titles]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Titles](
	[Id] [int] NULL,
	[Name] [nvarchar](20) NULL,
	[Active] [bit] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserBookmarks]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserBookmarks](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[ContentId] [int] NULL,
	[ContentType] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserCertification]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserCertification](
	[Id] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[CertificationName] [nvarchar](250) NOT NULL,
	[Year] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_UserCertification] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserCourseTests]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserCourseTests](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[QuestionId] [int] NULL,
	[AnswerId] [int] NULL,
	[UserId] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[CourseId] [numeric](18, 0) NULL,
 CONSTRAINT [PK_UserCourseTests] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserEducation]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserEducation](
	[Id] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[UniversitySchool] [nvarchar](250) NOT NULL,
	[Major] [nvarchar](100) NULL,
	[Grade] [nvarchar](10) NULL,
	[FromDate] [date] NULL,
	[ToDate] [date] NULL,
	[CreatedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_UserEducation] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserExperiences]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserExperiences](
	[Id] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[OrganizationName] [nvarchar](250) NOT NULL,
	[Designation] [nvarchar](250) NOT NULL,
	[FromDate] [date] NULL,
	[ToDate] [date] NULL,
	[CreatedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_UserExperiences] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserLanguages]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserLanguages](
	[Id] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[LanguageId] [int] NOT NULL,
	[IsSpeak] [bit] NOT NULL,
	[IsRead] [bit] NOT NULL,
	[IsWrite] [bit] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_UserLanguages] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserSocialMedia]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserSocialMedia](
	[Id] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[SocialMediaId] [int] NOT NULL,
	[URL] [nvarchar](100) NULL,
	[CreatedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_UserSocialMedia] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Visiters]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Visiters](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[CreatedOn] [datetime] NULL,
 CONSTRAINT [PK_Visiters] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WebContentPages]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WebContentPages](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PageName] [nvarchar](50) NULL,
	[PageName_Ar] [nvarchar](100) NULL,
 CONSTRAINT [PK_WebContentPages] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WebPageContent]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WebPageContent](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PageID] [int] NULL,
	[PageContent] [nvarchar](max) NULL,
	[CreatedBy] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedOn] [datetime] NULL,
	[PageContent_Ar] [nvarchar](max) NULL,
 CONSTRAINT [PK_WebPageContent] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WhiteListingURLs]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WhiteListingURLs](
	[Id] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[RequestedBy] [int] NOT NULL,
	[VerifiedBy] [int] NULL,
	[URL] [nvarchar](2000) NOT NULL,
	[IsApproved] [bit] NOT NULL,
	[RequestedOn] [date] NOT NULL,
	[VerifiedOn] [date] NULL,
	[RejectedReason] [nvarchar](100) NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_URLWhiteListing] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[CategoryMaster] ON 

INSERT [dbo].[CategoryMaster] ([Id], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [Active], [Name_Ar], [Status]) VALUES (1, N'test', 2, CAST(N'2019-12-26T10:54:46.713' AS DateTime), 2, CAST(N'2019-12-26T10:54:46.713' AS DateTime), 1, N'test', 1)
SET IDENTITY_INSERT [dbo].[CategoryMaster] OFF
SET IDENTITY_INSERT [dbo].[CopyrightMaster] ON 

INSERT [dbo].[CopyrightMaster] ([Id], [Title], [Description], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [Active], [Media], [Title_Ar], [Description_Ar], [Protected], [IsResourceProtect], [Status]) VALUES (1, N'test', N'test', 2, CAST(N'2019-12-26T11:27:31.360' AS DateTime), 2, CAST(N'2019-12-26T11:27:31.360' AS DateTime), 1, N'http://182.72.164.238:9000/oer-bucket/copyright/1577339732686rsim5Ioq9Q.png', N'test', N'test', 0, 0, 1)
SET IDENTITY_INSERT [dbo].[CopyrightMaster] OFF
SET IDENTITY_INSERT [dbo].[CountryMaster] ON 

INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (4, N'AE', N'United Arab Emirates', 1, CAST(N'2019-06-10T07:06:07.680' AS DateTime), 1, CAST(N'2019-06-10T07:06:07.680' AS DateTime), 1, N'الإمارات العربية المتحدة')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (5, NULL, N'Afghanistan', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'أفغانستان')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (6, NULL, N'Albania', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'ألبانيا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (7, NULL, N'Algeria', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'الجزائر')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (8, NULL, N'American Samoa', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'ساموا-الأمريكي')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (9, NULL, N'Andorra', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'أندورا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (10, NULL, N'Angola', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'أنغولا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (11, NULL, N'Anguilla', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'أنغويلا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (12, NULL, N'Antarctica', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'أنتاركتيكا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (13, NULL, N'Antigua and Barbuda', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'أنتيغوا وبربودا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (14, NULL, N'Argentina', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'الأرجنتين')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (15, NULL, N'Armenia', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'أرمينيا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (16, NULL, N'Aruba', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'أروبه')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (17, NULL, N'Australia', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'أستراليا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (18, NULL, N'Austria', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'النمسا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (19, NULL, N'Azerbaijan', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'أذربيجان')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (20, NULL, N'Bahamas', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'الباهاماس')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (21, NULL, N'Bahrain', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'البحرين')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (22, NULL, N'Bangladesh', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'بنغلاديش')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (23, NULL, N'Barbados', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'بربادوس')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (24, NULL, N'Belarus', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'روسيا البيضاء')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (25, NULL, N'Belgium', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'بلجيكا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (26, NULL, N'Belize', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'بيليز')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (27, NULL, N'Benin', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'بنين')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (28, NULL, N'Bermuda', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'جزر برمودا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (29, NULL, N'Bhutan', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'بوتان')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (30, NULL, N'Bolivia', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'بوليفيا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (31, NULL, N'Bosnia and Herzegovina', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'البوسنة و الهرسك')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (32, NULL, N'Botswana', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'بوتسوانا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (33, NULL, N'Brazil', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'البرازيل')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (34, NULL, N'Brunei Darussalam', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'بروني')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (35, NULL, N'Bulgaria', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'بلغاريا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (36, NULL, N'Burkina Faso', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'بوركينا فاسو')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (37, NULL, N'Burundi', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'بوروندي')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (38, NULL, N'Cambodia', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'كمبوديا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (39, NULL, N'Cameroon', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'كاميرون')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (40, NULL, N'Canada', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'كندا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (41, NULL, N'Cape Verde', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'الرأس الأخضر')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (42, NULL, N'Central African Republic', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'جمهورية أفريقيا الوسطى')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (43, NULL, N'Chad', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'تشاد')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (44, NULL, N'Chile', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'شيلي')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (45, NULL, N'China', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'جمهورية الصين الشعبية')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (46, NULL, N'Colombia', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'كولومبيا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (47, NULL, N'Comoros', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'جزر القمر')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (48, NULL, N'Democratic Republic', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'جمهورية الكونغو الديمقراطية')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (49, NULL, N'Congo, Republic of (Brazzaville)', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'جمهورية الكونغو')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (50, NULL, N'Cook Islands', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'جزر كوك')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (51, NULL, N'Costa Rica', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'كوستاريكا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (52, NULL, N'Cote d”Ivoire', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'ساحل العاج')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (53, NULL, N'Croatia', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'كرواتيا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (54, NULL, N'Cuba', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'كوبا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (55, NULL, N'Cyprus', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'قبرص')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (56, NULL, N'Czech Republic', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'الجمهورية التشيكية')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (57, NULL, N'Denmark', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'الدانمارك')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (58, NULL, N'Djibouti', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'جيبوتي')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (59, NULL, N'Dominica', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'دومينيكا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (60, NULL, N'Dominican Republic', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'الجمهورية الدومينيكية')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (61, NULL, N'East Timor Timor-Leste', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'تيمور الشرقية')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (62, NULL, N'Ecuador', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'إكوادور')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (63, NULL, N'Egypt', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'مصر')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (64, NULL, N'El Salvador', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'إلسلفادور')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (65, NULL, N'Equatorial Guinea', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'غينيا الاستوائي')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (66, NULL, N'Eritrea', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'إريتريا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (67, NULL, N'Estonia', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'استونيا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (68, NULL, N'Ethiopia', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'أثيوبيا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (69, NULL, N'Faroe Islands', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'جزر فارو')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (70, NULL, N'Fiji', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'فيجي')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (71, NULL, N'Finland', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'فنلندا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (72, NULL, N'France', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'فرنسا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (73, NULL, N'French Guiana', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'غويانا الفرنسية')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (74, NULL, N'French Polynesia', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'بولينيزيا الفرنسية')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (75, NULL, N'Gabon', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'الغابون')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (76, NULL, N'Gambia', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'غامبيا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (77, NULL, N'Georgia', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'جيورجيا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (78, NULL, N'Germany', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'ألمانيا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (79, NULL, N'Ghana', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'غانا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (80, NULL, N'Gibraltar', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'جبل طارق')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (81, NULL, N'Greece', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'اليونان')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (82, NULL, N'Greenland', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'جرينلاند')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (83, NULL, N'Grenada', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'غرينادا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (84, NULL, N'Guadeloupe', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'جزر جوادلوب')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (85, NULL, N'Guam', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'جوام')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (86, NULL, N'Guatemala', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'غواتيمال')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (87, NULL, N'Guinea', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'غينيا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (88, NULL, N'Guinea-Bissau', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'غينيا-بيساو')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (89, NULL, N'Guyana', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'غيانا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (90, NULL, N'Haiti', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'هايتي')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (91, NULL, N'Honduras', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'هندوراس')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (92, NULL, N'Hong Kong', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'هونغ كونغ')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (93, NULL, N'Hungary', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'المجر')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (94, NULL, N'Iceland', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'آيسلندا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (95, NULL, N'India', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'الهند')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (96, NULL, N'Indonesia', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'أندونيسيا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (97, NULL, N'Iran', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'إيران')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (98, NULL, N'Iraq', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'العراق')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (99, NULL, N'Ireland', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'إيرلندا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (100, NULL, N'Italy', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'إيطاليا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (101, NULL, N'Jamaica', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'جمايكا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (102, NULL, N'Japan', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'اليابان')
GO
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (103, NULL, N'Jordan', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'الأردن')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (104, NULL, N'Kazakhstan', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'كازاخستان')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (105, NULL, N'Kenya', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'كينيا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (106, NULL, N'Kiribati', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'كيريباتي')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (107, NULL, N'Korea, (North Korea)', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'كوريا الشمالية')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (108, NULL, N'Korea, (South Korea)', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'كوريا الجنوبية')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (109, NULL, N'Kuwait', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'الكويت')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (110, NULL, N'Kyrgyzstan', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'قيرغيزستان')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (111, NULL, N'Lao, PDR', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'لاوس')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (112, NULL, N'Latvia', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'لاتفيا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (113, NULL, N'Lebanon', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'لبنان')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (114, NULL, N'Lesotho', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'ليسوتو')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (115, NULL, N'Liberia', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'ليبيريا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (116, NULL, N'Libya', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'ليبيا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (117, NULL, N'Liechtenstein', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'ليختنشتين')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (118, NULL, N'Lithuania', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'لتوانيا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (119, NULL, N'Luxembourg', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'لوكسمبورغ')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (120, NULL, N'Macao', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'ماكاو')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (121, NULL, N'Macedonia, Rep. of', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'مقدونيا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (122, NULL, N'Madagascar', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'مدغشقر')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (123, NULL, N'Malawi', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'مالاوي')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (124, NULL, N'Malaysia', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'ماليزيا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (125, NULL, N'Maldives', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'المالديف')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (126, NULL, N'Mali', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'مالي')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (127, NULL, N'Malta', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'مالطا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (128, NULL, N'Marshall Islands', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'جزر مارشال')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (129, NULL, N'Martinique', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'مارتينيك')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (130, NULL, N'Mauritania', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'موريتانيا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (131, NULL, N'Mauritius', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'موريشيوس')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (132, NULL, N'Mexico', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'المكسيك')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (133, NULL, N'Micronesia', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'مايكرونيزيا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (134, NULL, N'Moldova', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'مولدافيا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (135, NULL, N'Monaco', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'موناكو')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (136, NULL, N'Mongolia', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'منغوليا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (137, NULL, N'Montenegro', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'الجبل الأسو')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (138, NULL, N'Montserrat', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'مونتسيرات')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (139, NULL, N'Morocco', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'المغرب')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (140, NULL, N'Mozambique', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'موزمبيق')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (141, NULL, N'Myanmar, Burma', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'ميانمار')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (142, NULL, N'Namibia', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'ناميبيا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (143, NULL, N'Nauru', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'نورو')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (144, NULL, N'Nepal', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'نيبال')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (145, NULL, N'Netherlands', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'هولندا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (146, NULL, N'Netherlands Antilles', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'جزر الأنتيل الهولندي')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (147, NULL, N'New Caledonia', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'كاليدونيا الجديدة')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (148, NULL, N'New Zealand', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'نيوزيلندا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (149, NULL, N'Nicaragua', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'نيكاراجوا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (150, NULL, N'Niger', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'النيجر')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (151, NULL, N'Nigeria', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'نيجيريا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (152, NULL, N'Niue', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'ني')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (153, NULL, N'Northern Mariana Islands', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'جزر ماريانا الشمالية')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (154, NULL, N'Norway', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'النرويج')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (155, NULL, N'Oman', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'عُمان')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (156, NULL, N'Pakistan', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'باكستان')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (157, NULL, N'Palau', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'بالاو')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (158, NULL, N'Palestine', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'فلسطين')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (159, NULL, N'Panama', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'بنما')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (160, NULL, N'Papua New Guinea', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'بابوا غينيا الجديدة')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (161, NULL, N'Paraguay', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'باراغواي')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (162, NULL, N'Peru', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'بيرو')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (163, NULL, N'Philippines', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'الفليبين')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (164, NULL, N'Poland', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'بولونيا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (165, NULL, N'Portugal', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'البرتغال')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (166, NULL, N'Puerto Rico', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'بورتو ريكو')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (167, NULL, N'Qatar', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'قطر')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (168, NULL, N'Reunion Island', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'ريونيون')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (169, NULL, N'Romania', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'رومانيا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (170, NULL, N'Russia', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'روسيا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (171, NULL, N'Rwanda', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'رواندا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (172, NULL, N'Saint Kitts and Nevis', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'سانت كيتس ونيفس')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (173, NULL, N'Saint Lucia', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'سانت لوسيا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (174, NULL, N'Saint Vincent and the', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'سانت فنسنت وجزر غرينادين')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (175, NULL, N'Samoa', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'المناطق')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (176, NULL, N'San Marino', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'سان مارينو')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (177, NULL, N'Sao Tome and Príncipe', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'ساو تومي وبرينسيبي')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (178, NULL, N'Saudi Arabia', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'المملكة العربية السعودية')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (179, NULL, N'Senegal', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'السنغال')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (180, NULL, N'Serbia', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'جمهورية صربيا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (181, NULL, N'Seychelles', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'سيشيل')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (182, NULL, N'Sierra Leone', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'سيراليون')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (183, NULL, N'Singapore', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'سنغافورة')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (184, NULL, N'Slovakia', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'سلوفاكيا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (185, NULL, N'Slovenia', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'سلوفينيا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (186, NULL, N'Solomon Islands', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'جزر سليمان')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (187, NULL, N'Somalia', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'الصومال')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (188, NULL, N'South Africa', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'جنوب أفريقيا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (189, NULL, N'Spain', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'إسبانيا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (190, NULL, N'Sri Lanka', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'سريلانكا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (191, NULL, N'Sudan', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'السودان')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (192, NULL, N'Suriname', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'سورينام')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (193, NULL, N'Swaziland', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'سوازيلند')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (194, NULL, N'Sweden', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'السويد')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (195, NULL, N'Switzerland', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'سويسرا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (196, NULL, N'Syria', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'سوريا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (197, NULL, N'Taiwan', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'تايوان')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (198, NULL, N'Tajikistan', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'طاجيكستان')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (199, NULL, N'Tanzania', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'تنزانيا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (200, NULL, N'Thailand', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'تايلندا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (201, NULL, N'Tibet', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'تبت')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (202, NULL, N'Timor-Leste (East Timor)', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'تيمور الشرقية')
GO
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (203, NULL, N'Togo', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'توغو')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (204, NULL, N'Tonga', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'تونغا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (205, NULL, N'Trinidad and Tobago', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'ترينيداد وتوباغو')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (206, NULL, N'Tunisia', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'تونس')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (207, NULL, N'Turkey', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'تركيا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (208, NULL, N'Turkmenistan', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'تركمانستان')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (209, NULL, N'Tuvalu', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'توفالو')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (210, NULL, N'Uganda', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'أوغندا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (211, NULL, N'Ukraine', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'أوكرانيا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (212, NULL, N'United Kingdom', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'المملكة المتحدة')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (213, NULL, N'United States', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'الولايات المتحدة')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (214, NULL, N'Uruguay', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'أورغواي')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (215, NULL, N'Uzbekistan', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'أوزباكستان')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (216, NULL, N'Vanuatu', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'فانواتو')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (217, NULL, N'Vatican City State', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'الفاتيكان')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (218, NULL, N'Venezuela', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'فنزويلا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (219, NULL, N'Vietnam', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'فيتنام')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (220, NULL, N'Virgin Islands (British)', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'الجزر العذراء البريطانية')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (221, NULL, N'Virgin Islands (U.S.)', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'الجزر العذراء الأمريكي')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (222, NULL, N'Wallis and ', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'والس وفوتونا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (223, NULL, N'Western Sahara', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'الصحراء الغربية')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (224, NULL, N'Yemen', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'اليمن')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (225, NULL, N'Zambia', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'زامبيا')
INSERT [dbo].[CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (226, NULL, N'Zimbabwe', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'زمبابوي')
SET IDENTITY_INSERT [dbo].[CountryMaster] OFF
SET IDENTITY_INSERT [dbo].[CourseAssociatedFiles] ON 

INSERT [dbo].[CourseAssociatedFiles] ([Id], [CourseId], [AssociatedFile], [CreatedOn], [FileName], [IsInclude]) VALUES (CAST(3 AS Numeric(18, 0)), CAST(1 AS Numeric(18, 0)), N'http://182.72.164.238:9000/oer-bucket/courses/1578205308073IQneRZ45AG.pdf', CAST(N'2020-01-05T11:59:59.007' AS DateTime), N'test2.pdf', 1)
INSERT [dbo].[CourseAssociatedFiles] ([Id], [CourseId], [AssociatedFile], [CreatedOn], [FileName], [IsInclude]) VALUES (CAST(4 AS Numeric(18, 0)), CAST(1 AS Numeric(18, 0)), N'http://182.72.164.238:9000/oer-bucket/courses/1578206184566gVocyMrG98.pdf', CAST(N'2020-01-05T12:06:38.987' AS DateTime), N'TestDoc.pdf', 1)
INSERT [dbo].[CourseAssociatedFiles] ([Id], [CourseId], [AssociatedFile], [CreatedOn], [FileName], [IsInclude]) VALUES (CAST(5 AS Numeric(18, 0)), CAST(1 AS Numeric(18, 0)), N'http://182.72.164.238:9000/oer-bucket/courses/1578206283822yRoUVw3Kgt.avi', CAST(N'2020-01-05T12:08:15.003' AS DateTime), N'flame.avi', 1)
INSERT [dbo].[CourseAssociatedFiles] ([Id], [CourseId], [AssociatedFile], [CreatedOn], [FileName], [IsInclude]) VALUES (CAST(6 AS Numeric(18, 0)), CAST(1 AS Numeric(18, 0)), N'http://182.72.164.238:9000/oer-bucket/courses/1578415278056JnzIhXYNBe.mp4', CAST(N'2020-01-07T22:39:20.947' AS DateTime), N'1GB File.mp4', 1)
SET IDENTITY_INSERT [dbo].[CourseAssociatedFiles] OFF
SET IDENTITY_INSERT [dbo].[CourseMaster] ON 

INSERT [dbo].[CourseMaster] ([Id], [Title], [CategoryId], [SubCategoryId], [Thumbnail], [CourseDescription], [Keywords], [CourseContent], [CopyRightId], [IsDraft], [CreatedBy], [CreatedOn], [IsApproved], [Rating], [ReportAbuseCount], [EducationId], [ProfessionId], [ViewCount], [AverageReadingTime], [DownloadCount], [SharedCount], [ReadingTime], [LastView], [LevelId], [EducationalStandardId], [EducationalUseId], [CommunityBadge], [MoEBadge], [IsApprovedSensory]) VALUES (CAST(1 AS Numeric(18, 0)), N'Test large file size upload', 1, 1, N'http://182.72.164.238:9000/oer-bucket/thumbs/1578166844947cUazfvNcxN.png', N'Test large file size upload', N'', N'<!DOCTYPE html>
<html>
<head>
</head>
<body>
<p>Test large file size upload</p>
</body>
</html>', 1, 0, 9, CAST(N'2020-01-05T01:10:52.293' AS DateTime), NULL, 0, 0, 1, 1, 1, NULL, NULL, NULL, 20, CAST(N'2020-01-07T22:39:38.643' AS DateTime), 1, 1, 1, 0, 0, 0)
SET IDENTITY_INSERT [dbo].[CourseMaster] OFF
SET IDENTITY_INSERT [dbo].[EducationMaster] ON 

INSERT [dbo].[EducationMaster] ([Id], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [Active], [Name_Ar], [Status]) VALUES (1, N'test', 2, CAST(N'2019-12-26T11:27:09.123' AS DateTime), 2, CAST(N'2019-12-26T11:27:09.123' AS DateTime), 1, N'test', 1)
SET IDENTITY_INSERT [dbo].[EducationMaster] OFF
SET IDENTITY_INSERT [dbo].[LogAction] ON 

INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(1 AS Numeric(18, 0)), 18, 2, 4, CAST(N'2019-12-26T10:53:47.720' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(2 AS Numeric(18, 0)), 18, 2, 4, CAST(N'2019-12-26T10:54:37.420' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(3 AS Numeric(18, 0)), 1, 2, 1, CAST(N'2019-12-26T10:54:46.717' AS DateTime), N'')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(4 AS Numeric(18, 0)), 18, 2, 4, CAST(N'2019-12-26T10:54:54.993' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(5 AS Numeric(18, 0)), 18, 2, 4, CAST(N'2019-12-26T10:54:55.310' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(6 AS Numeric(18, 0)), 18, 2, 4, CAST(N'2019-12-26T10:55:27.547' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(7 AS Numeric(18, 0)), 18, 2, 4, CAST(N'2019-12-26T11:26:46.777' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(8 AS Numeric(18, 0)), 18, 2, 4, CAST(N'2019-12-26T12:06:18.327' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(9 AS Numeric(18, 0)), 18, 3, 4, CAST(N'2019-12-26T18:18:19.357' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(10 AS Numeric(18, 0)), 18, 3, 4, CAST(N'2019-12-26T18:23:52.597' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(11 AS Numeric(18, 0)), 18, 4, 4, CAST(N'2019-12-30T10:15:19.323' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(12 AS Numeric(18, 0)), 18, 5, 4, CAST(N'2019-12-30T10:16:03.427' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(13 AS Numeric(18, 0)), 18, 6, 4, CAST(N'2019-12-30T10:18:18.750' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(14 AS Numeric(18, 0)), 18, 7, 4, CAST(N'2019-12-30T10:18:55.150' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(15 AS Numeric(18, 0)), 18, 7, 4, CAST(N'2019-12-30T10:20:58.583' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(16 AS Numeric(18, 0)), 18, 8, 4, CAST(N'2020-01-04T14:46:13.913' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(17 AS Numeric(18, 0)), 18, 8, 4, CAST(N'2020-01-04T14:48:02.303' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(18 AS Numeric(18, 0)), 18, 8, 4, CAST(N'2020-01-04T15:57:35.840' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(19 AS Numeric(18, 0)), 18, 9, 4, CAST(N'2020-01-04T16:02:47.183' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(20 AS Numeric(18, 0)), 18, 9, 4, CAST(N'2020-01-04T16:06:55.890' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(21 AS Numeric(18, 0)), 18, 9, 4, CAST(N'2020-01-04T17:44:54.913' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(22 AS Numeric(18, 0)), 18, 9, 4, CAST(N'2020-01-04T17:45:51.847' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(23 AS Numeric(18, 0)), 18, 9, 4, CAST(N'2020-01-04T17:52:49.103' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(24 AS Numeric(18, 0)), 18, 9, 4, CAST(N'2020-01-04T17:52:49.150' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(25 AS Numeric(18, 0)), 18, 9, 4, CAST(N'2020-01-04T17:59:20.233' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(26 AS Numeric(18, 0)), 18, 9, 4, CAST(N'2020-01-04T18:11:32.303' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(27 AS Numeric(18, 0)), 18, 9, 4, CAST(N'2020-01-04T18:11:32.380' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(28 AS Numeric(18, 0)), 18, 9, 4, CAST(N'2020-01-04T18:12:13.210' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(29 AS Numeric(18, 0)), 18, 9, 4, CAST(N'2020-01-04T18:12:13.267' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(30 AS Numeric(18, 0)), 18, 9, 4, CAST(N'2020-01-05T00:55:20.697' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(31 AS Numeric(18, 0)), 18, 9, 4, CAST(N'2020-01-05T00:57:43.260' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(32 AS Numeric(18, 0)), 18, 9, 4, CAST(N'2020-01-05T01:01:09.563' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(33 AS Numeric(18, 0)), 18, 9, 4, CAST(N'2020-01-05T01:10:57.967' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(34 AS Numeric(18, 0)), 18, 9, 4, CAST(N'2020-01-05T01:10:58.000' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(35 AS Numeric(18, 0)), 18, 9, 4, CAST(N'2020-01-05T01:11:22.577' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(36 AS Numeric(18, 0)), 18, 9, 4, CAST(N'2020-01-05T01:11:22.613' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(37 AS Numeric(18, 0)), 18, 9, 4, CAST(N'2020-01-05T01:13:35.023' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(38 AS Numeric(18, 0)), 18, 9, 4, CAST(N'2020-01-05T02:02:05.393' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(39 AS Numeric(18, 0)), 18, 9, 4, CAST(N'2020-01-05T02:02:40.217' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(40 AS Numeric(18, 0)), 18, 9, 4, CAST(N'2020-01-05T02:02:40.257' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(41 AS Numeric(18, 0)), 18, 9, 4, CAST(N'2020-01-05T03:25:03.767' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(42 AS Numeric(18, 0)), 18, 9, 4, CAST(N'2020-01-05T03:26:05.973' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(43 AS Numeric(18, 0)), 18, 9, 4, CAST(N'2020-01-05T03:26:18.483' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(44 AS Numeric(18, 0)), 18, 6, 4, CAST(N'2020-01-05T07:55:48.450' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(45 AS Numeric(18, 0)), 18, 6, 4, CAST(N'2020-01-05T08:00:32.480' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(46 AS Numeric(18, 0)), 18, 6, 4, CAST(N'2020-01-05T08:00:51.513' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(47 AS Numeric(18, 0)), 18, 6, 4, CAST(N'2020-01-05T08:00:51.620' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(48 AS Numeric(18, 0)), 18, 6, 4, CAST(N'2020-01-05T08:36:02.337' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(49 AS Numeric(18, 0)), 18, 6, 4, CAST(N'2020-01-05T08:36:02.520' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(50 AS Numeric(18, 0)), 18, 8, 4, CAST(N'2020-01-05T09:24:58.217' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(51 AS Numeric(18, 0)), 18, 8, 4, CAST(N'2020-01-05T09:30:09.050' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(52 AS Numeric(18, 0)), 18, 4, 4, CAST(N'2020-01-05T09:59:54.610' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(53 AS Numeric(18, 0)), 18, 4, 4, CAST(N'2020-01-05T10:01:28.593' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(54 AS Numeric(18, 0)), 18, 6, 4, CAST(N'2020-01-05T10:04:27.007' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(55 AS Numeric(18, 0)), 18, 6, 4, CAST(N'2020-01-05T10:27:47.613' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(56 AS Numeric(18, 0)), 18, 6, 4, CAST(N'2020-01-05T10:29:15.407' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(57 AS Numeric(18, 0)), 18, 4, 4, CAST(N'2020-01-05T10:30:00.733' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(58 AS Numeric(18, 0)), 18, 4, 4, CAST(N'2020-01-05T10:30:12.297' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(59 AS Numeric(18, 0)), 18, 4, 4, CAST(N'2020-01-05T10:30:12.480' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(60 AS Numeric(18, 0)), 18, 6, 4, CAST(N'2020-01-05T10:34:43.477' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(61 AS Numeric(18, 0)), 18, 8, 4, CAST(N'2020-01-05T10:42:56.320' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(62 AS Numeric(18, 0)), 18, 8, 4, CAST(N'2020-01-05T10:44:05.223' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(63 AS Numeric(18, 0)), 18, 8, 4, CAST(N'2020-01-05T10:44:05.597' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(64 AS Numeric(18, 0)), 18, 4, 4, CAST(N'2020-01-05T10:58:08.423' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(65 AS Numeric(18, 0)), 18, 4, 4, CAST(N'2020-01-05T10:58:09.207' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(66 AS Numeric(18, 0)), 18, 4, 4, CAST(N'2020-01-05T11:16:44.210' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(67 AS Numeric(18, 0)), 18, 4, 4, CAST(N'2020-01-05T11:16:44.430' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(68 AS Numeric(18, 0)), 18, 6, 4, CAST(N'2020-01-05T11:20:36.430' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(69 AS Numeric(18, 0)), 18, 9, 4, CAST(N'2020-01-05T11:35:08.587' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(70 AS Numeric(18, 0)), 18, 9, 4, CAST(N'2020-01-05T11:35:38.393' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(71 AS Numeric(18, 0)), 18, 9, 4, CAST(N'2020-01-05T11:35:38.437' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(72 AS Numeric(18, 0)), 18, 4, 4, CAST(N'2020-01-05T11:46:31.083' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(73 AS Numeric(18, 0)), 18, 4, 4, CAST(N'2020-01-05T11:46:31.260' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(74 AS Numeric(18, 0)), 18, 9, 4, CAST(N'2020-01-05T11:51:31.237' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(75 AS Numeric(18, 0)), 18, 9, 4, CAST(N'2020-01-05T11:51:31.300' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(76 AS Numeric(18, 0)), 18, 9, 4, CAST(N'2020-01-05T12:00:04.750' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(77 AS Numeric(18, 0)), 18, 9, 4, CAST(N'2020-01-05T12:00:04.790' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(78 AS Numeric(18, 0)), 18, 9, 4, CAST(N'2020-01-05T12:02:43.817' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(79 AS Numeric(18, 0)), 18, 9, 4, CAST(N'2020-01-05T12:06:48.933' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(80 AS Numeric(18, 0)), 18, 9, 4, CAST(N'2020-01-05T12:06:48.973' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(81 AS Numeric(18, 0)), 18, 9, 4, CAST(N'2020-01-05T12:08:21.663' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(82 AS Numeric(18, 0)), 18, 9, 4, CAST(N'2020-01-05T12:08:21.700' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(83 AS Numeric(18, 0)), 18, 9, 4, CAST(N'2020-01-05T12:42:24.997' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(84 AS Numeric(18, 0)), 18, 8, 4, CAST(N'2020-01-05T12:51:00.377' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(85 AS Numeric(18, 0)), 18, 8, 4, CAST(N'2020-01-05T12:51:00.403' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(86 AS Numeric(18, 0)), 18, 9, 4, CAST(N'2020-01-05T12:53:40.933' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(87 AS Numeric(18, 0)), 18, 9, 4, CAST(N'2020-01-05T12:53:40.973' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(88 AS Numeric(18, 0)), 18, 8, 4, CAST(N'2020-01-05T13:03:40.143' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(89 AS Numeric(18, 0)), 18, 8, 4, CAST(N'2020-01-05T13:03:40.180' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(90 AS Numeric(18, 0)), 18, 9, 4, CAST(N'2020-01-05T13:12:44.293' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(91 AS Numeric(18, 0)), 18, 9, 4, CAST(N'2020-01-05T13:14:44.733' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(92 AS Numeric(18, 0)), 18, 9, 4, CAST(N'2020-01-05T13:16:54.427' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(93 AS Numeric(18, 0)), 18, 9, 4, CAST(N'2020-01-05T13:38:26.400' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(94 AS Numeric(18, 0)), 18, 9, 4, CAST(N'2020-01-05T13:38:32.843' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(95 AS Numeric(18, 0)), 18, 9, 4, CAST(N'2020-01-05T13:38:32.877' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(96 AS Numeric(18, 0)), 18, 4, 4, CAST(N'2020-01-05T16:47:04.360' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(97 AS Numeric(18, 0)), 18, 4, 4, CAST(N'2020-01-05T16:58:15.463' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(98 AS Numeric(18, 0)), 18, 4, 4, CAST(N'2020-01-05T16:58:15.577' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(99 AS Numeric(18, 0)), 18, 4, 4, CAST(N'2020-01-05T17:01:02.277' AS DateTime), N'Fetching UserProfile')
GO
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(100 AS Numeric(18, 0)), 18, 4, 4, CAST(N'2020-01-05T17:01:02.390' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(101 AS Numeric(18, 0)), 18, 9, 4, CAST(N'2020-01-06T10:15:51.987' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(102 AS Numeric(18, 0)), 18, 9, 4, CAST(N'2020-01-06T16:45:24.880' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(103 AS Numeric(18, 0)), 18, 9, 4, CAST(N'2020-01-06T20:24:57.837' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(104 AS Numeric(18, 0)), 18, 9, 4, CAST(N'2020-01-06T21:08:09.893' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(105 AS Numeric(18, 0)), 18, 9, 4, CAST(N'2020-01-06T21:12:45.043' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(106 AS Numeric(18, 0)), 18, 9, 4, CAST(N'2020-01-07T00:03:06.567' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(107 AS Numeric(18, 0)), 18, 9, 4, CAST(N'2020-01-07T00:27:36.807' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(108 AS Numeric(18, 0)), 18, 9, 4, CAST(N'2020-01-07T00:50:06.323' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(109 AS Numeric(18, 0)), 18, 9, 4, CAST(N'2020-01-07T01:25:58.320' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(110 AS Numeric(18, 0)), 18, 9, 4, CAST(N'2020-01-07T01:51:41.890' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(111 AS Numeric(18, 0)), 18, 9, 4, CAST(N'2020-01-07T01:52:35.063' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(112 AS Numeric(18, 0)), 18, 9, 4, CAST(N'2020-01-07T01:52:35.100' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(113 AS Numeric(18, 0)), 18, 9, 4, CAST(N'2020-01-07T02:22:28.940' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(114 AS Numeric(18, 0)), 18, 9, 4, CAST(N'2020-01-07T22:11:00.720' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(115 AS Numeric(18, 0)), 18, 9, 4, CAST(N'2020-01-07T22:39:38.400' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(116 AS Numeric(18, 0)), 18, 9, 4, CAST(N'2020-01-07T22:39:38.437' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(117 AS Numeric(18, 0)), 18, 9, 4, CAST(N'2020-01-08T17:21:57.337' AS DateTime), N'Fetching UserProfile')
INSERT [dbo].[LogAction] ([Id], [LogModuleId], [UserId], [ActionId], [ActionDate], [ActionDetail]) VALUES (CAST(118 AS Numeric(18, 0)), 18, 6, 4, CAST(N'2020-01-08T18:17:50.353' AS DateTime), N'Fetching UserProfile')
SET IDENTITY_INSERT [dbo].[LogAction] OFF
SET IDENTITY_INSERT [dbo].[LogActionMaster] ON 

INSERT [dbo].[LogActionMaster] ([Id], [Name]) VALUES (1, N'Create')
INSERT [dbo].[LogActionMaster] ([Id], [Name]) VALUES (2, N'Update')
INSERT [dbo].[LogActionMaster] ([Id], [Name]) VALUES (3, N'Delete')
INSERT [dbo].[LogActionMaster] ([Id], [Name]) VALUES (4, N'View')
SET IDENTITY_INSERT [dbo].[LogActionMaster] OFF
SET IDENTITY_INSERT [dbo].[LogModuleMaster] ON 

INSERT [dbo].[LogModuleMaster] ([Id], [Name]) VALUES (1, N'CategoryMaster')
INSERT [dbo].[LogModuleMaster] ([Id], [Name]) VALUES (2, N'CopyRightMaster')
INSERT [dbo].[LogModuleMaster] ([Id], [Name]) VALUES (3, N'CountryMaster')
INSERT [dbo].[LogModuleMaster] ([Id], [Name]) VALUES (4, N'CourseMaster')
INSERT [dbo].[LogModuleMaster] ([Id], [Name]) VALUES (5, N'DepartmentMaster')
INSERT [dbo].[LogModuleMaster] ([Id], [Name]) VALUES (6, N'DesignationMaster')
INSERT [dbo].[LogModuleMaster] ([Id], [Name]) VALUES (7, N'EducationMaster')
INSERT [dbo].[LogModuleMaster] ([Id], [Name]) VALUES (8, N'InstitutionMaster')
INSERT [dbo].[LogModuleMaster] ([Id], [Name]) VALUES (9, N'LanguageMaster')
INSERT [dbo].[LogModuleMaster] ([Id], [Name]) VALUES (10, N'MaterialTypeMaster')
INSERT [dbo].[LogModuleMaster] ([Id], [Name]) VALUES (11, N'ProfessionMaster')
INSERT [dbo].[LogModuleMaster] ([Id], [Name]) VALUES (12, N'QRCMaster')
INSERT [dbo].[LogModuleMaster] ([Id], [Name]) VALUES (13, N'ResourceMaster')
INSERT [dbo].[LogModuleMaster] ([Id], [Name]) VALUES (14, N'SocialMediaMaster')
INSERT [dbo].[LogModuleMaster] ([Id], [Name]) VALUES (15, N'StateMaster')
INSERT [dbo].[LogModuleMaster] ([Id], [Name]) VALUES (16, N'StreamMaster')
INSERT [dbo].[LogModuleMaster] ([Id], [Name]) VALUES (17, N'SubCategoryMaster')
INSERT [dbo].[LogModuleMaster] ([Id], [Name]) VALUES (18, N'UserMaster')
SET IDENTITY_INSERT [dbo].[LogModuleMaster] OFF
SET IDENTITY_INSERT [dbo].[lu_Educational_Standard] ON 

INSERT [dbo].[lu_Educational_Standard] ([Id], [Standard], [EducationalUse_Ar], [Active], [CreatedBy], [UpdatedBy], [CreatedOn], [UpdatedOn], [Standard_Ar], [Status]) VALUES (1, N'test', NULL, 1, 2, 2, CAST(N'2019-12-26T11:27:38.240' AS DateTime), CAST(N'2019-12-26T11:27:38.240' AS DateTime), N'test', 1)
SET IDENTITY_INSERT [dbo].[lu_Educational_Standard] OFF
SET IDENTITY_INSERT [dbo].[lu_Educational_Use] ON 

INSERT [dbo].[lu_Educational_Use] ([Id], [EducationalUse], [EducationalUse_Ar], [CreatedBy], [CreatedOn], [UpdatedOn], [UpdatedBy], [Active], [Status]) VALUES (1, N'test', N'test', 2, CAST(N'2019-12-26T11:27:50.740' AS DateTime), CAST(N'2019-12-26T11:27:50.740' AS DateTime), 2, 1, 1)
SET IDENTITY_INSERT [dbo].[lu_Educational_Use] OFF
SET IDENTITY_INSERT [dbo].[lu_Level] ON 

INSERT [dbo].[lu_Level] ([Id], [Level], [Level_Ar], [CreatedBy], [UpdatedOn], [CreatedOn], [Active], [UpdatedBy], [Status]) VALUES (1, N'test', N'test', 2, CAST(N'2019-12-26T11:27:58.163' AS DateTime), CAST(N'2019-12-26T11:27:58.163' AS DateTime), 1, 2, 1)
SET IDENTITY_INSERT [dbo].[lu_Level] OFF
SET IDENTITY_INSERT [dbo].[MaterialTypeMaster] ON 

INSERT [dbo].[MaterialTypeMaster] ([Id], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [Active], [Name_Ar], [Status]) VALUES (1, N'test', 2, CAST(N'2019-12-26T11:27:44.657' AS DateTime), 2, CAST(N'2019-12-26T11:27:44.657' AS DateTime), 1, N'test', 1)
SET IDENTITY_INSERT [dbo].[MaterialTypeMaster] OFF
SET IDENTITY_INSERT [dbo].[MessageType] ON 

INSERT [dbo].[MessageType] ([Id], [Type]) VALUES (1, N'Course Approval')
INSERT [dbo].[MessageType] ([Id], [Type]) VALUES (2, N'Course Rejection')
INSERT [dbo].[MessageType] ([Id], [Type]) VALUES (3, N'Course Promotion')
INSERT [dbo].[MessageType] ([Id], [Type]) VALUES (4, N'Resource Approval')
INSERT [dbo].[MessageType] ([Id], [Type]) VALUES (5, N'Resource Rejection')
INSERT [dbo].[MessageType] ([Id], [Type]) VALUES (6, N'Resource Promotion')
INSERT [dbo].[MessageType] ([Id], [Type]) VALUES (7, N'Contributor Access Approved')
INSERT [dbo].[MessageType] ([Id], [Type]) VALUES (8, N'Contributor Access Rejected')
INSERT [dbo].[MessageType] ([Id], [Type]) VALUES (9, N'URL Whitelist Approval')
INSERT [dbo].[MessageType] ([Id], [Type]) VALUES (10, N'URL Whitelist Rejection')
INSERT [dbo].[MessageType] ([Id], [Type]) VALUES (11, N'QRC Review')
SET IDENTITY_INSERT [dbo].[MessageType] OFF
SET IDENTITY_INSERT [dbo].[Notifications] ON 

INSERT [dbo].[Notifications] ([Id], [ReferenceId], [ReferenceTypeId], [Subject], [Content], [MessageTypeId], [IsApproved], [CreatedDate], [ReadDate], [DeletedDate], [IsRead], [IsDelete], [UserId], [Reviewer], [Comment], [URL], [Status]) VALUES (1, 7, 3, N'User Contributor', N'User Contributor Access Status', 7, 1, CAST(N'2019-12-30T10:20:50.730' AS DateTime), NULL, NULL, 0, 0, 7, NULL, NULL, N'http://182.72.164.238:8000/dashboard/community-check', 1)
INSERT [dbo].[Notifications] ([Id], [ReferenceId], [ReferenceTypeId], [Subject], [Content], [MessageTypeId], [IsApproved], [CreatedDate], [ReadDate], [DeletedDate], [IsRead], [IsDelete], [UserId], [Reviewer], [Comment], [URL], [Status]) VALUES (2, 8, 3, N'User Contributor', N'User Contributor Access Status', 7, 1, CAST(N'2020-01-04T14:47:55.550' AS DateTime), NULL, NULL, 0, 0, 8, NULL, NULL, N'http://182.72.164.238:8000/dashboard/community-check', 1)
INSERT [dbo].[Notifications] ([Id], [ReferenceId], [ReferenceTypeId], [Subject], [Content], [MessageTypeId], [IsApproved], [CreatedDate], [ReadDate], [DeletedDate], [IsRead], [IsDelete], [UserId], [Reviewer], [Comment], [URL], [Status]) VALUES (3, 9, 3, N'User Contributor', N'User Contributor Access Status', 7, 1, CAST(N'2020-01-04T16:06:47.897' AS DateTime), NULL, NULL, 0, 0, 9, NULL, NULL, N'http://182.72.164.238:8000/dashboard/community-check', 1)
INSERT [dbo].[Notifications] ([Id], [ReferenceId], [ReferenceTypeId], [Subject], [Content], [MessageTypeId], [IsApproved], [CreatedDate], [ReadDate], [DeletedDate], [IsRead], [IsDelete], [UserId], [Reviewer], [Comment], [URL], [Status]) VALUES (4, 4, 3, N'User Contributor', N'User Contributor Access Status', 7, 1, CAST(N'2020-01-05T10:01:25.557' AS DateTime), NULL, NULL, 0, 0, 4, NULL, NULL, N'http://182.72.164.238:8000/dashboard/community-check', 1)
SET IDENTITY_INSERT [dbo].[Notifications] OFF
INSERT [dbo].[OerConfig] ([key], [value], [ConfigType]) VALUES (N'AWSEndPoint', N'http://oer-admin-bucket.s3.amazonaws.com/oer-admin-bucket/', N'AWS')
INSERT [dbo].[OerConfig] ([key], [value], [ConfigType]) VALUES (N'AWSAccessKey', N'AKIAXFRZTMWJNBH5N2VC', N'AWS')
INSERT [dbo].[OerConfig] ([key], [value], [ConfigType]) VALUES (N'AWSSecretKey', N'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', N'AWS')
INSERT [dbo].[OerConfig] ([key], [value], [ConfigType]) VALUES (N'AWSUser', N'us-east-2', N'AWS')
INSERT [dbo].[OerConfig] ([key], [value], [ConfigType]) VALUES (N'AWSBucketName', N'oer-admin-bucket', N'AWS')
SET IDENTITY_INSERT [dbo].[ProfessionMaster] ON 

INSERT [dbo].[ProfessionMaster] ([Id], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [Active], [Name_Ar], [Status]) VALUES (1, N'test', 2, CAST(N'2019-12-26T11:27:15.373' AS DateTime), 2, CAST(N'2019-12-26T11:27:15.373' AS DateTime), 1, N'test', 1)
SET IDENTITY_INSERT [dbo].[ProfessionMaster] OFF
SET IDENTITY_INSERT [dbo].[ResourceAssociatedFiles] ON 

INSERT [dbo].[ResourceAssociatedFiles] ([Id], [ResourceId], [AssociatedFile], [UploadedDate], [FileName], [IsInclude]) VALUES (CAST(1 AS Numeric(18, 0)), CAST(2 AS Numeric(18, 0)), N'http://182.72.164.238:9000/oer-bucket/Treadmill Walking 300fps.avi', CAST(N'2020-01-05T10:26:37.777' AS DateTime), N'Treadmill Walking 300fps.avi', 1)
INSERT [dbo].[ResourceAssociatedFiles] ([Id], [ResourceId], [AssociatedFile], [UploadedDate], [FileName], [IsInclude]) VALUES (CAST(2 AS Numeric(18, 0)), CAST(3 AS Numeric(18, 0)), N'http://182.72.164.238:9000/oer-bucket/Golf_Putting_AutoTracking.avi', CAST(N'2020-01-05T11:16:35.550' AS DateTime), N'Golf_Putting_AutoTracking.avi', 1)
INSERT [dbo].[ResourceAssociatedFiles] ([Id], [ResourceId], [AssociatedFile], [UploadedDate], [FileName], [IsInclude]) VALUES (CAST(3 AS Numeric(18, 0)), CAST(5 AS Numeric(18, 0)), N'http://182.72.164.238:9000/oer-bucket/resources/1578223128630ppMctQLh4E.avi', CAST(N'2020-01-05T16:58:08.797' AS DateTime), N'Treadmill Walking 300fps.avi', 1)
INSERT [dbo].[ResourceAssociatedFiles] ([Id], [ResourceId], [AssociatedFile], [UploadedDate], [FileName], [IsInclude]) VALUES (CAST(4 AS Numeric(18, 0)), CAST(6 AS Numeric(18, 0)), N'http://182.72.164.238:9000/oer-bucket/resources/1578223823597KaFZYvqS9r.avi', CAST(N'2020-01-05T17:00:57.110' AS DateTime), N'Golf_Putting_AutoTracking.avi', 1)
SET IDENTITY_INSERT [dbo].[ResourceAssociatedFiles] OFF
SET IDENTITY_INSERT [dbo].[ResourceMaster] ON 

INSERT [dbo].[ResourceMaster] ([Id], [Title], [CategoryId], [SubCategoryId], [Thumbnail], [ResourceDescription], [Keywords], [ResourceContent], [MaterialTypeId], [CopyRightId], [IsDraft], [CreatedBy], [CreatedOn], [IsApproved], [Rating], [AlignmentRating], [ReportAbuseCount], [UrlSegment], [ViewCount], [AverageReadingTime], [DownloadCount], [SharedCount], [ReadingTime], [Objective], [LevelId], [EducationalStandardId], [EducationalUseId], [Format], [LastView], [CommunityBadge], [MoEBadge], [IsApprovedSensory]) VALUES (CAST(1 AS Numeric(18, 0)), N'100 mb file test', 1, 1, NULL, N'snmlsnlksndfd
sndnsfdkjnfdks
nnsdlkjskjfnkjnfdskjnfds
nsdkskfd', N'', N'<!DOCTYPE html>
<html>
<head>
</head>
<body>
<p>regfergfegm mlkmgrlkmlk nlnmlklkgrmlkke kjnlknrgnlkre</p>
</body>
</html>', 1, NULL, 1, 8, CAST(N'2020-01-04T14:50:21.867' AS DateTime), NULL, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, N'feefne  jknewnfkjwenfw  knkjwnfek nknfewjknwefkjfew', 1, 1, 1, NULL, CAST(N'2020-01-04T14:50:24.973' AS DateTime), 0, 0, 0)
INSERT [dbo].[ResourceMaster] ([Id], [Title], [CategoryId], [SubCategoryId], [Thumbnail], [ResourceDescription], [Keywords], [ResourceContent], [MaterialTypeId], [CopyRightId], [IsDraft], [CreatedBy], [CreatedOn], [IsApproved], [Rating], [AlignmentRating], [ReportAbuseCount], [UrlSegment], [ViewCount], [AverageReadingTime], [DownloadCount], [SharedCount], [ReadingTime], [Objective], [LevelId], [EducationalStandardId], [EducationalUseId], [Format], [LastView], [CommunityBadge], [MoEBadge], [IsApprovedSensory]) VALUES (CAST(2 AS Numeric(18, 0)), N'Falling Tiles', 1, 1, N'http://182.72.164.238:9000/oer-bucket/thumbs/1578198765929RVa2wKlv2u.jpg', N'Falling tiles is a test resource ', N'jingle', N'<!DOCTYPE html>
<html>
<head>
</head>
<body>
<p>falling tiles is a must read</p>
</body>
</html>', 1, 1, 0, 4, CAST(N'2020-01-05T10:26:37.777' AS DateTime), NULL, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, 120, N'this is just to check the speed of the falling tiles, nah, just kidding, the speed of file upload.', 1, 1, 1, NULL, CAST(N'2020-01-05T10:58:20.470' AS DateTime), 0, 0, 0)
INSERT [dbo].[ResourceMaster] ([Id], [Title], [CategoryId], [SubCategoryId], [Thumbnail], [ResourceDescription], [Keywords], [ResourceContent], [MaterialTypeId], [CopyRightId], [IsDraft], [CreatedBy], [CreatedOn], [IsApproved], [Rating], [AlignmentRating], [ReportAbuseCount], [UrlSegment], [ViewCount], [AverageReadingTime], [DownloadCount], [SharedCount], [ReadingTime], [Objective], [LevelId], [EducationalStandardId], [EducationalUseId], [Format], [LastView], [CommunityBadge], [MoEBadge], [IsApprovedSensory]) VALUES (CAST(3 AS Numeric(18, 0)), N'years together', 1, 1, N'http://182.72.164.238:9000/oer-bucket/thumbs/1578203189066A01QcoPSgj.jpg', N'test resource', N'', N'<!DOCTYPE html>
<html>
<head>
</head>
<body>
<p>test</p>
</body>
</html>', 1, 1, 0, 4, CAST(N'2020-01-05T11:16:35.550' AS DateTime), NULL, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, 47, N'Test', 1, 1, 1, NULL, CAST(N'2020-01-05T11:16:44.970' AS DateTime), 0, 0, 0)
INSERT [dbo].[ResourceMaster] ([Id], [Title], [CategoryId], [SubCategoryId], [Thumbnail], [ResourceDescription], [Keywords], [ResourceContent], [MaterialTypeId], [CopyRightId], [IsDraft], [CreatedBy], [CreatedOn], [IsApproved], [Rating], [AlignmentRating], [ReportAbuseCount], [UrlSegment], [ViewCount], [AverageReadingTime], [DownloadCount], [SharedCount], [ReadingTime], [Objective], [LevelId], [EducationalStandardId], [EducationalUseId], [Format], [LastView], [CommunityBadge], [MoEBadge], [IsApprovedSensory]) VALUES (CAST(4 AS Numeric(18, 0)), N'mr', 1, 1, N'http://182.72.164.238:9000/oer-bucket/thumbs/1578209606214UGmy62h0EW.jpg', N'nmbnmb  bknknkn nnkjnknkn knkknj', N'', N'<!DOCTYPE html>
<html>
<head>
</head>
<body>
<p>nmbbnmbnbmbmb nbmnbnmbnmbmbnm</p>
</body>
</html>', 1, 1, 0, 8, CAST(N'2020-01-05T13:03:34.727' AS DateTime), NULL, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, 14, N'bjhbhjbjhb bbjbjhbjbj hbhjbj j jbj bj', 1, 1, 1, NULL, CAST(N'2020-01-05T13:03:40.487' AS DateTime), 0, 0, 0)
INSERT [dbo].[ResourceMaster] ([Id], [Title], [CategoryId], [SubCategoryId], [Thumbnail], [ResourceDescription], [Keywords], [ResourceContent], [MaterialTypeId], [CopyRightId], [IsDraft], [CreatedBy], [CreatedOn], [IsApproved], [Rating], [AlignmentRating], [ReportAbuseCount], [UrlSegment], [ViewCount], [AverageReadingTime], [DownloadCount], [SharedCount], [ReadingTime], [Objective], [LevelId], [EducationalStandardId], [EducationalUseId], [Format], [LastView], [CommunityBadge], [MoEBadge], [IsApprovedSensory]) VALUES (CAST(5 AS Numeric(18, 0)), N'Ratatouille', 1, 1, N'http://182.72.164.238:9000/oer-bucket/thumbs/1578223066084I3BKN7ATzv.jpg', N'test', N'', N'<!DOCTYPE html>
<html>
<head>
</head>
<body>
<p>test</p>
</body>
</html>', 1, 1, 0, 4, CAST(N'2020-01-05T16:58:08.797' AS DateTime), NULL, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, 60, N'test', 1, 1, 1, NULL, CAST(N'2020-01-05T16:58:15.867' AS DateTime), 0, 0, 0)
INSERT [dbo].[ResourceMaster] ([Id], [Title], [CategoryId], [SubCategoryId], [Thumbnail], [ResourceDescription], [Keywords], [ResourceContent], [MaterialTypeId], [CopyRightId], [IsDraft], [CreatedBy], [CreatedOn], [IsApproved], [Rating], [AlignmentRating], [ReportAbuseCount], [UrlSegment], [ViewCount], [AverageReadingTime], [DownloadCount], [SharedCount], [ReadingTime], [Objective], [LevelId], [EducationalStandardId], [EducationalUseId], [Format], [LastView], [CommunityBadge], [MoEBadge], [IsApprovedSensory]) VALUES (CAST(6 AS Numeric(18, 0)), N'Nanny''s Day Out', 1, 1, N'http://182.72.164.238:9000/oer-bucket/thumbs/1578223797905aIftw945tK.jpg', N'Test', N'', N'<!DOCTYPE html>
<html>
<head>
</head>
<body>
<p>test</p>
</body>
</html>', 1, 1, 0, 4, CAST(N'2020-01-05T17:00:57.110' AS DateTime), NULL, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, 146, N'test', 1, 1, 1, NULL, CAST(N'2020-01-05T17:01:02.730' AS DateTime), 0, 0, 0)
SET IDENTITY_INSERT [dbo].[ResourceMaster] OFF
SET IDENTITY_INSERT [dbo].[SocialMediaMaster] ON 

INSERT [dbo].[SocialMediaMaster] ([Id], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [Name_Ar]) VALUES (2, N'Facebook', 1, CAST(N'2019-04-07T09:37:51.167' AS DateTime), 1, CAST(N'2019-04-07T09:37:51.167' AS DateTime), N'فيسبوك')
INSERT [dbo].[SocialMediaMaster] ([Id], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [Name_Ar]) VALUES (3, N'Twitter', 1, CAST(N'2019-04-07T09:38:18.630' AS DateTime), 1, CAST(N'2019-04-07T09:38:18.630' AS DateTime), N'تويتر')
INSERT [dbo].[SocialMediaMaster] ([Id], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [Name_Ar]) VALUES (4, N'LinkedIn', 1, CAST(N'2019-04-07T09:38:28.383' AS DateTime), 1, CAST(N'2019-04-07T09:38:28.383' AS DateTime), N'لينكدإن')
INSERT [dbo].[SocialMediaMaster] ([Id], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [Name_Ar]) VALUES (5, N'Google Plus', 1, CAST(N'2019-04-07T09:38:48.760' AS DateTime), 1, CAST(N'2019-04-07T09:38:48.760' AS DateTime), N'جووجل بلس')
INSERT [dbo].[SocialMediaMaster] ([Id], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [Name_Ar]) VALUES (6, N'Whats App', 1, CAST(N'2019-04-07T09:39:07.020' AS DateTime), 1, CAST(N'2019-04-07T09:39:07.020' AS DateTime), N'واتساب')
SET IDENTITY_INSERT [dbo].[SocialMediaMaster] OFF
SET IDENTITY_INSERT [dbo].[StateMaster] ON 

INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (8, 4, N'Abu Dhabi', 1, CAST(N'2019-06-10T07:16:58.653' AS DateTime), 1, CAST(N'2019-06-10T07:16:58.653' AS DateTime), 1, N'أبو ظبي')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (9, 4, N'Ajman', 1, CAST(N'2019-06-10T07:17:24.557' AS DateTime), 1, CAST(N'2019-06-10T07:17:24.557' AS DateTime), 1, N'عجمان')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (10, 4, N'Dubai', 1, CAST(N'2019-06-10T07:17:54.750' AS DateTime), 1, CAST(N'2019-06-10T07:17:54.750' AS DateTime), 1, N'دبي')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (11, 4, N'Fujairah', 1, CAST(N'2019-06-10T07:18:20.653' AS DateTime), 1, CAST(N'2019-06-10T07:18:20.653' AS DateTime), 1, N'الفجيرة')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (12, 4, N'Ras Al Khaimah', 1, CAST(N'2019-06-10T07:18:48.653' AS DateTime), 1, CAST(N'2019-06-10T07:18:48.653' AS DateTime), 1, N'رأس الخيمة')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (13, 4, N'Sharjah', 1, CAST(N'2019-06-10T07:19:55.980' AS DateTime), 1, CAST(N'2019-06-10T07:19:55.980' AS DateTime), 1, N'الشارقة')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (14, 4, N'Umm Al Quwain', 1, CAST(N'2019-06-10T07:20:37.043' AS DateTime), 1, CAST(N'2019-06-10T07:20:37.043' AS DateTime), 1, N'أم القيوين')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (15, 5, N'N/A', 1, CAST(N'2019-10-13T23:56:55.900' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.900' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (16, 6, N'N/A', 1, CAST(N'2019-10-13T23:56:55.900' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.900' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (17, 7, N'N/A', 1, CAST(N'2019-10-13T23:56:55.903' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.903' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (18, 8, N'N/A', 1, CAST(N'2019-10-13T23:56:55.903' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.903' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (19, 9, N'N/A', 1, CAST(N'2019-10-13T23:56:55.907' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.907' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (20, 10, N'N/A', 1, CAST(N'2019-10-13T23:56:55.907' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.907' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (21, 11, N'N/A', 1, CAST(N'2019-10-13T23:56:55.910' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.910' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (22, 12, N'N/A', 1, CAST(N'2019-10-13T23:56:55.910' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.910' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (23, 13, N'N/A', 1, CAST(N'2019-10-13T23:56:55.910' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.910' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (24, 14, N'N/A', 1, CAST(N'2019-10-13T23:56:55.910' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.910' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (25, 15, N'N/A', 1, CAST(N'2019-10-13T23:56:55.913' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.913' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (26, 16, N'N/A', 1, CAST(N'2019-10-13T23:56:55.913' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.913' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (27, 17, N'N/A', 1, CAST(N'2019-10-13T23:56:55.913' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.913' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (28, 18, N'N/A', 1, CAST(N'2019-10-13T23:56:55.917' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.917' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (29, 19, N'N/A', 1, CAST(N'2019-10-13T23:56:55.917' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.917' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (30, 20, N'N/A', 1, CAST(N'2019-10-13T23:56:55.917' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.917' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (31, 21, N'N/A', 1, CAST(N'2019-10-13T23:56:55.917' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.917' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (32, 22, N'N/A', 1, CAST(N'2019-10-13T23:56:55.920' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.920' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (33, 23, N'N/A', 1, CAST(N'2019-10-13T23:56:55.920' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.920' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (34, 24, N'N/A', 1, CAST(N'2019-10-13T23:56:55.923' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.923' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (35, 25, N'N/A', 1, CAST(N'2019-10-13T23:56:55.923' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.923' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (36, 26, N'N/A', 1, CAST(N'2019-10-13T23:56:55.927' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.927' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (37, 27, N'N/A', 1, CAST(N'2019-10-13T23:56:55.927' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.927' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (38, 28, N'N/A', 1, CAST(N'2019-10-13T23:56:55.930' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.930' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (39, 29, N'N/A', 1, CAST(N'2019-10-13T23:56:55.930' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.930' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (40, 30, N'N/A', 1, CAST(N'2019-10-13T23:56:55.930' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.930' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (41, 31, N'N/A', 1, CAST(N'2019-10-13T23:56:55.930' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.930' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (42, 32, N'N/A', 1, CAST(N'2019-10-13T23:56:55.930' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.930' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (43, 33, N'N/A', 1, CAST(N'2019-10-13T23:56:55.930' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.930' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (44, 34, N'N/A', 1, CAST(N'2019-10-13T23:56:55.933' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.933' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (45, 35, N'N/A', 1, CAST(N'2019-10-13T23:56:55.933' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.933' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (46, 36, N'N/A', 1, CAST(N'2019-10-13T23:56:55.940' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.940' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (47, 37, N'N/A', 1, CAST(N'2019-10-13T23:56:55.940' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.940' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (48, 38, N'N/A', 1, CAST(N'2019-10-13T23:56:55.940' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.940' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (49, 39, N'N/A', 1, CAST(N'2019-10-13T23:56:55.943' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.943' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (50, 40, N'N/A', 1, CAST(N'2019-10-13T23:56:55.943' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.943' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (51, 41, N'N/A', 1, CAST(N'2019-10-13T23:56:55.943' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.943' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (52, 42, N'N/A', 1, CAST(N'2019-10-13T23:56:55.943' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.943' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (53, 43, N'N/A', 1, CAST(N'2019-10-13T23:56:55.947' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.947' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (54, 44, N'N/A', 1, CAST(N'2019-10-13T23:56:55.947' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.947' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (55, 45, N'N/A', 1, CAST(N'2019-10-13T23:56:55.947' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.947' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (56, 46, N'N/A', 1, CAST(N'2019-10-13T23:56:55.947' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.947' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (57, 47, N'N/A', 1, CAST(N'2019-10-13T23:56:55.947' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.947' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (58, 48, N'N/A', 1, CAST(N'2019-10-13T23:56:55.950' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.950' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (59, 49, N'N/A', 1, CAST(N'2019-10-13T23:56:55.950' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.950' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (60, 50, N'N/A', 1, CAST(N'2019-10-13T23:56:55.953' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.953' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (61, 51, N'N/A', 1, CAST(N'2019-10-13T23:56:55.953' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.953' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (62, 52, N'N/A', 1, CAST(N'2019-10-13T23:56:55.953' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.953' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (63, 53, N'N/A', 1, CAST(N'2019-10-13T23:56:55.953' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.953' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (64, 54, N'N/A', 1, CAST(N'2019-10-13T23:56:55.957' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.957' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (65, 55, N'N/A', 1, CAST(N'2019-10-13T23:56:55.957' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.957' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (66, 56, N'N/A', 1, CAST(N'2019-10-13T23:56:55.960' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.960' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (67, 57, N'N/A', 1, CAST(N'2019-10-13T23:56:55.960' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.960' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (68, 58, N'N/A', 1, CAST(N'2019-10-13T23:56:55.960' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.960' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (69, 59, N'N/A', 1, CAST(N'2019-10-13T23:56:55.960' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.960' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (70, 60, N'N/A', 1, CAST(N'2019-10-13T23:56:55.960' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.960' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (71, 61, N'N/A', 1, CAST(N'2019-10-13T23:56:55.960' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.960' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (72, 62, N'N/A', 1, CAST(N'2019-10-13T23:56:55.963' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.963' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (73, 63, N'N/A', 1, CAST(N'2019-10-13T23:56:55.963' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.963' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (74, 64, N'N/A', 1, CAST(N'2019-10-13T23:56:55.963' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.963' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (75, 65, N'N/A', 1, CAST(N'2019-10-13T23:56:55.963' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.963' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (76, 66, N'N/A', 1, CAST(N'2019-10-13T23:56:55.967' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.967' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (77, 67, N'N/A', 1, CAST(N'2019-10-13T23:56:55.967' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.967' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (78, 68, N'N/A', 1, CAST(N'2019-10-13T23:56:55.970' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.970' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (79, 69, N'N/A', 1, CAST(N'2019-10-13T23:56:55.970' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.970' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (80, 70, N'N/A', 1, CAST(N'2019-10-13T23:56:55.970' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.970' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (81, 71, N'N/A', 1, CAST(N'2019-10-13T23:56:55.973' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.973' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (82, 72, N'N/A', 1, CAST(N'2019-10-13T23:56:55.973' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.973' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (83, 73, N'N/A', 1, CAST(N'2019-10-13T23:56:55.973' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.973' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (84, 74, N'N/A', 1, CAST(N'2019-10-13T23:56:55.973' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.973' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (85, 75, N'N/A', 1, CAST(N'2019-10-13T23:56:55.977' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.977' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (86, 76, N'N/A', 1, CAST(N'2019-10-13T23:56:55.977' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.977' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (87, 77, N'N/A', 1, CAST(N'2019-10-13T23:56:55.977' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.977' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (88, 78, N'N/A', 1, CAST(N'2019-10-13T23:56:55.977' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.977' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (89, 79, N'N/A', 1, CAST(N'2019-10-13T23:56:55.980' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.980' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (90, 80, N'N/A', 1, CAST(N'2019-10-13T23:56:55.980' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.980' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (91, 81, N'N/A', 1, CAST(N'2019-10-13T23:56:55.980' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.980' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (92, 82, N'N/A', 1, CAST(N'2019-10-13T23:56:55.980' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.980' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (93, 83, N'N/A', 1, CAST(N'2019-10-13T23:56:55.980' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.980' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (94, 84, N'N/A', 1, CAST(N'2019-10-13T23:56:55.983' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.983' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (95, 85, N'N/A', 1, CAST(N'2019-10-13T23:56:55.983' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.983' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (96, 86, N'N/A', 1, CAST(N'2019-10-13T23:56:55.987' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.987' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (97, 87, N'N/A', 1, CAST(N'2019-10-13T23:56:55.987' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.987' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (98, 88, N'N/A', 1, CAST(N'2019-10-13T23:56:55.990' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.990' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (99, 89, N'N/A', 1, CAST(N'2019-10-13T23:56:55.990' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.990' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (100, 90, N'N/A', 1, CAST(N'2019-10-13T23:56:55.990' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.990' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (101, 91, N'N/A', 1, CAST(N'2019-10-13T23:56:55.990' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.990' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (102, 92, N'N/A', 1, CAST(N'2019-10-13T23:56:55.990' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.990' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (103, 93, N'N/A', 1, CAST(N'2019-10-13T23:56:55.990' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.990' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (104, 94, N'N/A', 1, CAST(N'2019-10-13T23:56:55.993' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.993' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (105, 95, N'N/A', 1, CAST(N'2019-10-13T23:56:55.993' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.993' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (106, 96, N'N/A', 1, CAST(N'2019-10-13T23:56:55.993' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.993' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (107, 97, N'N/A', 1, CAST(N'2019-10-13T23:56:55.993' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.993' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (108, 98, N'N/A', 1, CAST(N'2019-10-13T23:56:55.997' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.997' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (109, 99, N'N/A', 1, CAST(N'2019-10-13T23:56:56.000' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.000' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (110, 100, N'N/A', 1, CAST(N'2019-10-13T23:56:56.000' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.000' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (111, 101, N'N/A', 1, CAST(N'2019-10-13T23:56:56.003' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.003' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (112, 102, N'N/A', 1, CAST(N'2019-10-13T23:56:56.007' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.007' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (113, 103, N'N/A', 1, CAST(N'2019-10-13T23:56:56.010' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.010' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (114, 104, N'N/A', 1, CAST(N'2019-10-13T23:56:56.010' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.010' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (115, 105, N'N/A', 1, CAST(N'2019-10-13T23:56:56.010' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.010' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (116, 106, N'N/A', 1, CAST(N'2019-10-13T23:56:56.013' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.013' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (117, 107, N'N/A', 1, CAST(N'2019-10-13T23:56:56.017' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.017' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (118, 108, N'N/A', 1, CAST(N'2019-10-13T23:56:56.017' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.017' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (119, 109, N'N/A', 1, CAST(N'2019-10-13T23:56:56.017' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.017' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (120, 110, N'N/A', 1, CAST(N'2019-10-13T23:56:56.020' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.020' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (121, 111, N'N/A', 1, CAST(N'2019-10-13T23:56:56.020' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.020' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (122, 112, N'N/A', 1, CAST(N'2019-10-13T23:56:56.020' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.020' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (123, 113, N'N/A', 1, CAST(N'2019-10-13T23:56:56.020' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.020' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (124, 114, N'N/A', 1, CAST(N'2019-10-13T23:56:56.020' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.020' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (125, 115, N'N/A', 1, CAST(N'2019-10-13T23:56:56.020' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.020' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (126, 116, N'N/A', 1, CAST(N'2019-10-13T23:56:56.020' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.020' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (127, 117, N'N/A', 1, CAST(N'2019-10-13T23:56:56.023' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.023' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (128, 118, N'N/A', 1, CAST(N'2019-10-13T23:56:56.023' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.023' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (129, 119, N'N/A', 1, CAST(N'2019-10-13T23:56:56.023' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.023' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (130, 120, N'N/A', 1, CAST(N'2019-10-13T23:56:56.023' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.023' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (131, 121, N'N/A', 1, CAST(N'2019-10-13T23:56:56.027' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.027' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (132, 122, N'N/A', 1, CAST(N'2019-10-13T23:56:56.027' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.027' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (133, 123, N'N/A', 1, CAST(N'2019-10-13T23:56:56.027' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.027' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (134, 124, N'N/A', 1, CAST(N'2019-10-13T23:56:56.030' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.030' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (135, 125, N'N/A', 1, CAST(N'2019-10-13T23:56:56.030' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.030' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (136, 126, N'N/A', 1, CAST(N'2019-10-13T23:56:56.030' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.030' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (137, 127, N'N/A', 1, CAST(N'2019-10-13T23:56:56.030' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.030' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (138, 128, N'N/A', 1, CAST(N'2019-10-13T23:56:56.033' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.033' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (139, 129, N'N/A', 1, CAST(N'2019-10-13T23:56:56.033' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.033' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (140, 130, N'N/A', 1, CAST(N'2019-10-13T23:56:56.033' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.033' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (141, 131, N'N/A', 1, CAST(N'2019-10-13T23:56:56.033' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.033' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (142, 132, N'N/A', 1, CAST(N'2019-10-13T23:56:56.037' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.037' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (143, 133, N'N/A', 1, CAST(N'2019-10-13T23:56:56.037' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.037' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (144, 134, N'N/A', 1, CAST(N'2019-10-13T23:56:56.037' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.037' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (145, 135, N'N/A', 1, CAST(N'2019-10-13T23:56:56.040' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.040' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (146, 136, N'N/A', 1, CAST(N'2019-10-13T23:56:56.040' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.040' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (147, 137, N'N/A', 1, CAST(N'2019-10-13T23:56:56.040' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.040' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (148, 138, N'N/A', 1, CAST(N'2019-10-13T23:56:56.040' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.040' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (149, 139, N'N/A', 1, CAST(N'2019-10-13T23:56:56.040' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.040' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (150, 140, N'N/A', 1, CAST(N'2019-10-13T23:56:56.040' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.040' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (151, 141, N'N/A', 1, CAST(N'2019-10-13T23:56:56.043' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.043' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (152, 142, N'N/A', 1, CAST(N'2019-10-13T23:56:56.047' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.047' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (153, 143, N'N/A', 1, CAST(N'2019-10-13T23:56:56.047' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.047' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (154, 144, N'N/A', 1, CAST(N'2019-10-13T23:56:56.050' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.050' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (155, 145, N'N/A', 1, CAST(N'2019-10-13T23:56:56.050' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.050' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (156, 146, N'N/A', 1, CAST(N'2019-10-13T23:56:56.053' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.053' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (157, 147, N'N/A', 1, CAST(N'2019-10-13T23:56:56.053' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.053' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (158, 148, N'N/A', 1, CAST(N'2019-10-13T23:56:56.053' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.053' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (159, 149, N'N/A', 1, CAST(N'2019-10-13T23:56:56.057' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.057' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (160, 150, N'N/A', 1, CAST(N'2019-10-13T23:56:56.060' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.060' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (161, 151, N'N/A', 1, CAST(N'2019-10-13T23:56:56.060' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.060' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (162, 152, N'N/A', 1, CAST(N'2019-10-13T23:56:56.060' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.060' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (163, 153, N'N/A', 1, CAST(N'2019-10-13T23:56:56.063' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.063' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (164, 154, N'N/A', 1, CAST(N'2019-10-13T23:56:56.067' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.067' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (165, 155, N'N/A', 1, CAST(N'2019-10-13T23:56:56.070' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.070' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (166, 156, N'N/A', 1, CAST(N'2019-10-13T23:56:56.070' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.070' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (167, 157, N'N/A', 1, CAST(N'2019-10-13T23:56:56.073' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.073' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (168, 158, N'N/A', 1, CAST(N'2019-10-13T23:56:56.073' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.073' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (169, 159, N'N/A', 1, CAST(N'2019-10-13T23:56:56.077' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.077' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (170, 160, N'N/A', 1, CAST(N'2019-10-13T23:56:56.077' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.077' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (171, 161, N'N/A', 1, CAST(N'2019-10-13T23:56:56.080' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.080' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (172, 162, N'N/A', 1, CAST(N'2019-10-13T23:56:56.080' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.080' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (173, 163, N'N/A', 1, CAST(N'2019-10-13T23:56:56.080' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.080' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (174, 164, N'N/A', 1, CAST(N'2019-10-13T23:56:56.080' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.080' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (175, 165, N'N/A', 1, CAST(N'2019-10-13T23:56:56.083' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.083' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (176, 166, N'N/A', 1, CAST(N'2019-10-13T23:56:56.083' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.083' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (177, 167, N'N/A', 1, CAST(N'2019-10-13T23:56:56.083' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.083' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (178, 168, N'N/A', 1, CAST(N'2019-10-13T23:56:56.087' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.087' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (179, 169, N'N/A', 1, CAST(N'2019-10-13T23:56:56.090' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.090' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (180, 170, N'N/A', 1, CAST(N'2019-10-13T23:56:56.093' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.093' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (181, 171, N'N/A', 1, CAST(N'2019-10-13T23:56:56.093' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.093' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (182, 172, N'N/A', 1, CAST(N'2019-10-13T23:56:56.097' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.097' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (183, 173, N'N/A', 1, CAST(N'2019-10-13T23:56:56.097' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.097' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (184, 174, N'N/A', 1, CAST(N'2019-10-13T23:56:56.100' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.100' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (185, 175, N'N/A', 1, CAST(N'2019-10-13T23:56:56.100' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.100' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (186, 176, N'N/A', 1, CAST(N'2019-10-13T23:56:56.100' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.100' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (187, 177, N'N/A', 1, CAST(N'2019-10-13T23:56:56.100' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.100' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (188, 178, N'N/A', 1, CAST(N'2019-10-13T23:56:56.103' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.103' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (189, 179, N'N/A', 1, CAST(N'2019-10-13T23:56:56.103' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.103' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (190, 180, N'N/A', 1, CAST(N'2019-10-13T23:56:56.103' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.103' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (191, 181, N'N/A', 1, CAST(N'2019-10-13T23:56:56.107' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.107' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (192, 182, N'N/A', 1, CAST(N'2019-10-13T23:56:56.110' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.110' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (193, 183, N'N/A', 1, CAST(N'2019-10-13T23:56:56.110' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.110' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (194, 184, N'N/A', 1, CAST(N'2019-10-13T23:56:56.110' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.110' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (195, 185, N'N/A', 1, CAST(N'2019-10-13T23:56:56.110' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.110' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (196, 186, N'N/A', 1, CAST(N'2019-10-13T23:56:56.113' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.113' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (197, 187, N'N/A', 1, CAST(N'2019-10-13T23:56:56.113' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.113' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (198, 188, N'N/A', 1, CAST(N'2019-10-13T23:56:56.113' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.113' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (199, 189, N'N/A', 1, CAST(N'2019-10-13T23:56:56.117' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.117' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (200, 190, N'N/A', 1, CAST(N'2019-10-13T23:56:56.120' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.120' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (201, 191, N'N/A', 1, CAST(N'2019-10-13T23:56:56.120' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.120' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (202, 192, N'N/A', 1, CAST(N'2019-10-13T23:56:56.120' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.120' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (203, 193, N'N/A', 1, CAST(N'2019-10-13T23:56:56.120' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.120' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (204, 194, N'N/A', 1, CAST(N'2019-10-13T23:56:56.123' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.123' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (205, 195, N'N/A', 1, CAST(N'2019-10-13T23:56:56.127' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.127' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (206, 196, N'N/A', 1, CAST(N'2019-10-13T23:56:56.127' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.127' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (207, 197, N'N/A', 1, CAST(N'2019-10-13T23:56:56.130' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.130' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (208, 198, N'N/A', 1, CAST(N'2019-10-13T23:56:56.130' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.130' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (209, 199, N'N/A', 1, CAST(N'2019-10-13T23:56:56.130' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.130' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (210, 200, N'N/A', 1, CAST(N'2019-10-13T23:56:56.130' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.130' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (211, 201, N'N/A', 1, CAST(N'2019-10-13T23:56:56.130' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.130' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (212, 202, N'N/A', 1, CAST(N'2019-10-13T23:56:56.133' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.133' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (213, 203, N'N/A', 1, CAST(N'2019-10-13T23:56:56.133' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.133' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (214, 204, N'N/A', 1, CAST(N'2019-10-13T23:56:56.133' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.133' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (215, 205, N'N/A', 1, CAST(N'2019-10-13T23:56:56.137' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.137' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (216, 206, N'N/A', 1, CAST(N'2019-10-13T23:56:56.137' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.137' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (217, 207, N'N/A', 1, CAST(N'2019-10-13T23:56:56.140' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.140' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (218, 208, N'N/A', 1, CAST(N'2019-10-13T23:56:56.140' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.140' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (219, 209, N'N/A', 1, CAST(N'2019-10-13T23:56:56.147' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.147' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (220, 210, N'N/A', 1, CAST(N'2019-10-13T23:56:56.147' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.147' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (221, 211, N'N/A', 1, CAST(N'2019-10-13T23:56:56.147' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.147' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (222, 212, N'N/A', 1, CAST(N'2019-10-13T23:56:56.150' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.150' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (223, 213, N'N/A', 1, CAST(N'2019-10-13T23:56:56.150' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.150' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (224, 214, N'N/A', 1, CAST(N'2019-10-13T23:56:56.150' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.150' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (225, 215, N'N/A', 1, CAST(N'2019-10-13T23:56:56.150' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.150' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (226, 216, N'N/A', 1, CAST(N'2019-10-13T23:56:56.153' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.153' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (227, 217, N'N/A', 1, CAST(N'2019-10-13T23:56:56.157' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.157' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (228, 218, N'N/A', 1, CAST(N'2019-10-13T23:56:56.160' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.160' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (229, 219, N'N/A', 1, CAST(N'2019-10-13T23:56:56.163' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.163' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (230, 220, N'N/A', 1, CAST(N'2019-10-13T23:56:56.167' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.167' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (231, 221, N'N/A', 1, CAST(N'2019-10-13T23:56:56.170' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.170' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (232, 222, N'N/A', 1, CAST(N'2019-10-13T23:56:56.173' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.173' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (233, 223, N'N/A', 1, CAST(N'2019-10-14T00:00:58.423' AS DateTime), 1, CAST(N'2019-10-14T00:00:58.423' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (234, 224, N'N/A', 1, CAST(N'2019-10-14T00:00:58.427' AS DateTime), 1, CAST(N'2019-10-14T00:00:58.427' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (235, 225, N'N/A', 1, CAST(N'2019-10-14T00:00:58.430' AS DateTime), 1, CAST(N'2019-10-14T00:00:58.430' AS DateTime), 1, N'غير قابل للتطبيق')
INSERT [dbo].[StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (236, 226, N'N/A', 1, CAST(N'2019-10-14T00:00:58.430' AS DateTime), 1, CAST(N'2019-10-14T00:00:58.430' AS DateTime), 1, N'غير قابل للتطبيق')
SET IDENTITY_INSERT [dbo].[StateMaster] OFF
SET IDENTITY_INSERT [dbo].[SubCategoryMaster] ON 

INSERT [dbo].[SubCategoryMaster] ([Id], [CategoryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [Active], [Name_Ar], [Status]) VALUES (1, 1, N'test', 2, CAST(N'2019-12-26T11:27:02.190' AS DateTime), 2, CAST(N'2019-12-26T11:27:02.190' AS DateTime), 1, N'test', 1)
SET IDENTITY_INSERT [dbo].[SubCategoryMaster] OFF
SET IDENTITY_INSERT [dbo].[TitleMaster] ON 

INSERT [dbo].[TitleMaster] ([Id], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (1, N'Mr.', 1, CAST(N'2019-04-06T10:13:52.433' AS DateTime), 1, CAST(N'2019-04-06T10:13:52.433' AS DateTime), 1, N'السيد.')
INSERT [dbo].[TitleMaster] ([Id], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (2, N'Mrs.', 1, CAST(N'2019-04-06T10:18:47.737' AS DateTime), 1, CAST(N'2019-04-06T10:18:47.737' AS DateTime), 1, N'السيدة.')
INSERT [dbo].[TitleMaster] ([Id], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (3, N'Miss.', 1, CAST(N'2019-04-06T10:30:06.090' AS DateTime), 1, CAST(N'2019-04-06T10:30:06.090' AS DateTime), 1, N'الاّنسة')
INSERT [dbo].[TitleMaster] ([Id], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (4, N'Ms.', 1, CAST(N'2019-04-06T10:30:22.400' AS DateTime), 1, CAST(N'2019-04-06T10:30:22.400' AS DateTime), 0, N'الآنسة.')
INSERT [dbo].[TitleMaster] ([Id], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (5, N'Dr.', 1, CAST(N'2019-04-06T10:30:32.643' AS DateTime), 1, CAST(N'2019-04-06T10:30:32.643' AS DateTime), 1, N'الدكتور.')
INSERT [dbo].[TitleMaster] ([Id], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (6, N'H.E', 1, CAST(N'2019-04-06T10:30:41.863' AS DateTime), 1, CAST(N'2019-04-06T10:30:41.863' AS DateTime), 1, N'سعادة.')
SET IDENTITY_INSERT [dbo].[TitleMaster] OFF
SET IDENTITY_INSERT [dbo].[UserMaster] ON 

INSERT [dbo].[UserMaster] ([Id], [TitleId], [FirstName], [MiddleName], [LastName], [CountryId], [StateId], [Gender], [Email], [PortalLanguageId], [DepartmentId], [DesignationId], [DateOfBirth], [Photo], [ProfileDescription], [SubjectsInterested], [ApprovalStatus], [CreatedOn], [UpdatedOn], [Active], [IsContributor], [IsAdmin], [IsEmailNotification], [LastLogin], [Theme], [IsVerifier]) VALUES (1, NULL, N'Admin', NULL, N'User', NULL, NULL, NULL, N'admin@oer.com', NULL, NULL, NULL, CAST(N'2018-12-30' AS Date), NULL, N'DO NOT DELETE', N'', 1, CAST(N'2019-04-06T10:11:34.780' AS DateTime), CAST(N'2019-06-06T19:38:05.543' AS DateTime), 1, 1, 1, 0, NULL, N'Gold', NULL)
INSERT [dbo].[UserMaster] ([Id], [TitleId], [FirstName], [MiddleName], [LastName], [CountryId], [StateId], [Gender], [Email], [PortalLanguageId], [DepartmentId], [DesignationId], [DateOfBirth], [Photo], [ProfileDescription], [SubjectsInterested], [ApprovalStatus], [CreatedOn], [UpdatedOn], [Active], [IsContributor], [IsAdmin], [IsEmailNotification], [LastLogin], [Theme], [IsVerifier]) VALUES (2, 1, N'Kumardev', NULL, N'T', 5, 15, 1, N'tbkumardev@mailinator.com', 2, NULL, NULL, CAST(N'2001-12-01' AS Date), NULL, NULL, N'test', 0, CAST(N'2019-12-26T10:53:47.710' AS DateTime), CAST(N'2019-12-26T10:55:27.537' AS DateTime), 1, 1, 1, 1, CAST(N'2019-12-26T10:55:42.770' AS DateTime), N'Blue', NULL)
INSERT [dbo].[UserMaster] ([Id], [TitleId], [FirstName], [MiddleName], [LastName], [CountryId], [StateId], [Gender], [Email], [PortalLanguageId], [DepartmentId], [DesignationId], [DateOfBirth], [Photo], [ProfileDescription], [SubjectsInterested], [ApprovalStatus], [CreatedOn], [UpdatedOn], [Active], [IsContributor], [IsAdmin], [IsEmailNotification], [LastLogin], [Theme], [IsVerifier]) VALUES (3, NULL, N'Kumardev', NULL, N'TB', NULL, NULL, 0, N'tbkumardev@gmail.com', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, CAST(N'2019-12-26T18:18:19.353' AS DateTime), CAST(N'2019-12-26T18:18:19.353' AS DateTime), 1, 1, 1, 1, NULL, N'Blue', NULL)
INSERT [dbo].[UserMaster] ([Id], [TitleId], [FirstName], [MiddleName], [LastName], [CountryId], [StateId], [Gender], [Email], [PortalLanguageId], [DepartmentId], [DesignationId], [DateOfBirth], [Photo], [ProfileDescription], [SubjectsInterested], [ApprovalStatus], [CreatedOn], [UpdatedOn], [Active], [IsContributor], [IsAdmin], [IsEmailNotification], [LastLogin], [Theme], [IsVerifier]) VALUES (4, 2, N'Serah', NULL, N'George', 4, 14, 2, N'serahgeorge@yopmail.com', 2, NULL, NULL, CAST(N'2000-01-01' AS Date), NULL, NULL, N'test', 0, CAST(N'2019-12-30T10:15:19.310' AS DateTime), CAST(N'2020-01-05T10:01:28.587' AS DateTime), 1, 1, 0, 1, NULL, N'Blue', NULL)
INSERT [dbo].[UserMaster] ([Id], [TitleId], [FirstName], [MiddleName], [LastName], [CountryId], [StateId], [Gender], [Email], [PortalLanguageId], [DepartmentId], [DesignationId], [DateOfBirth], [Photo], [ProfileDescription], [SubjectsInterested], [ApprovalStatus], [CreatedOn], [UpdatedOn], [Active], [IsContributor], [IsAdmin], [IsEmailNotification], [LastLogin], [Theme], [IsVerifier]) VALUES (5, NULL, N'Hashim', NULL, N'Haleem', NULL, NULL, 0, N'hashimh@gmail.com', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, CAST(N'2019-12-30T10:16:03.420' AS DateTime), CAST(N'2019-12-30T10:16:03.420' AS DateTime), 1, 0, 0, 1, NULL, N'Blue', NULL)
INSERT [dbo].[UserMaster] ([Id], [TitleId], [FirstName], [MiddleName], [LastName], [CountryId], [StateId], [Gender], [Email], [PortalLanguageId], [DepartmentId], [DesignationId], [DateOfBirth], [Photo], [ProfileDescription], [SubjectsInterested], [ApprovalStatus], [CreatedOn], [UpdatedOn], [Active], [IsContributor], [IsAdmin], [IsEmailNotification], [LastLogin], [Theme], [IsVerifier]) VALUES (6, 3, N'Hashim', NULL, N'Haleem', 4, 10, 2, N'hashim.haleem@verbat.com', 2, NULL, NULL, CAST(N'2000-01-01' AS Date), NULL, NULL, N'test', 0, CAST(N'2019-12-30T10:18:18.747' AS DateTime), CAST(N'2020-01-05T10:29:15.393' AS DateTime), 1, 1, 1, 1, NULL, N'Blue', NULL)
INSERT [dbo].[UserMaster] ([Id], [TitleId], [FirstName], [MiddleName], [LastName], [CountryId], [StateId], [Gender], [Email], [PortalLanguageId], [DepartmentId], [DesignationId], [DateOfBirth], [Photo], [ProfileDescription], [SubjectsInterested], [ApprovalStatus], [CreatedOn], [UpdatedOn], [Active], [IsContributor], [IsAdmin], [IsEmailNotification], [LastLogin], [Theme], [IsVerifier]) VALUES (7, 2, N'Megha', NULL, N'M', 4, 13, 2, N'megha@yopmail.com', 2, NULL, NULL, CAST(N'1994-12-01' AS Date), NULL, NULL, N'test', 0, CAST(N'2019-12-30T10:18:55.140' AS DateTime), CAST(N'2019-12-30T10:20:50.730' AS DateTime), 1, 1, 0, 1, NULL, N'Blue', NULL)
INSERT [dbo].[UserMaster] ([Id], [TitleId], [FirstName], [MiddleName], [LastName], [CountryId], [StateId], [Gender], [Email], [PortalLanguageId], [DepartmentId], [DesignationId], [DateOfBirth], [Photo], [ProfileDescription], [SubjectsInterested], [ApprovalStatus], [CreatedOn], [UpdatedOn], [Active], [IsContributor], [IsAdmin], [IsEmailNotification], [LastLogin], [Theme], [IsVerifier]) VALUES (8, 1, N'prashant', NULL, N'thomas', 17, 27, 1, N'prashant.thomas@verbat.com', 2, NULL, NULL, CAST(N'1917-01-24' AS Date), NULL, NULL, N'test', 0, CAST(N'2020-01-04T14:46:13.897' AS DateTime), CAST(N'2020-01-04T14:47:55.550' AS DateTime), 1, 1, 0, 1, NULL, N'Blue', NULL)
INSERT [dbo].[UserMaster] ([Id], [TitleId], [FirstName], [MiddleName], [LastName], [CountryId], [StateId], [Gender], [Email], [PortalLanguageId], [DepartmentId], [DesignationId], [DateOfBirth], [Photo], [ProfileDescription], [SubjectsInterested], [ApprovalStatus], [CreatedOn], [UpdatedOn], [Active], [IsContributor], [IsAdmin], [IsEmailNotification], [LastLogin], [Theme], [IsVerifier]) VALUES (9, 1, N'Syed', NULL, N'Shabeer', 95, 105, 1, N'syed.shabeer@qantler.com', 2, NULL, NULL, CAST(N'1978-05-13' AS Date), N'http://182.72.164.238:9000/oer-bucket/profile/1578205954415bVbjvgWe2q.jpg', NULL, N'test', 0, CAST(N'2020-01-04T16:02:47.180' AS DateTime), CAST(N'2020-01-05T12:02:43.807' AS DateTime), 1, 1, 0, 1, NULL, N'Blue', NULL)
SET IDENTITY_INSERT [dbo].[UserMaster] OFF
SET IDENTITY_INSERT [dbo].[Visiters] ON 

INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (1, NULL, CAST(N'2019-12-26T10:53:38.990' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (2, NULL, CAST(N'2019-12-26T10:54:55.890' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (3, NULL, CAST(N'2019-12-26T12:06:04.953' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (4, NULL, CAST(N'2019-12-26T12:06:18.500' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (5, NULL, CAST(N'2019-12-26T18:17:47.093' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (6, NULL, CAST(N'2019-12-26T18:23:52.737' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (7, NULL, CAST(N'2019-12-27T18:35:01.057' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (8, NULL, CAST(N'2019-12-30T10:09:02.793' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (9, NULL, CAST(N'2019-12-30T10:12:55.110' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (10, NULL, CAST(N'2019-12-30T10:15:47.790' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (11, NULL, CAST(N'2019-12-30T10:18:01.373' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (12, NULL, CAST(N'2019-12-30T10:18:38.033' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (13, NULL, CAST(N'2020-01-04T14:42:21.003' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (14, NULL, CAST(N'2020-01-04T15:53:54.667' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (15, NULL, CAST(N'2020-01-04T16:02:31.007' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (16, NULL, CAST(N'2020-01-04T17:44:55.077' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (17, NULL, CAST(N'2020-01-04T17:52:50.000' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (18, NULL, CAST(N'2020-01-04T17:59:20.367' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (19, NULL, CAST(N'2020-01-04T18:11:32.877' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (20, NULL, CAST(N'2020-01-04T18:12:14.013' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (21, NULL, CAST(N'2020-01-05T00:54:51.030' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (22, NULL, CAST(N'2020-01-05T00:55:20.817' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (23, NULL, CAST(N'2020-01-05T00:57:43.400' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (24, NULL, CAST(N'2020-01-05T01:01:01.213' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (25, NULL, CAST(N'2020-01-05T01:01:09.663' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (26, NULL, CAST(N'2020-01-05T01:10:58.210' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (27, NULL, CAST(N'2020-01-05T01:11:22.823' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (28, NULL, CAST(N'2020-01-05T01:13:35.163' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (29, NULL, CAST(N'2020-01-05T02:01:57.460' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (30, NULL, CAST(N'2020-01-05T02:02:05.507' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (31, NULL, CAST(N'2020-01-05T02:02:41.003' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (32, NULL, CAST(N'2020-01-05T03:24:11.820' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (33, NULL, CAST(N'2020-01-05T03:25:03.910' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (34, NULL, CAST(N'2020-01-05T03:26:18.887' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (35, NULL, CAST(N'2020-01-05T07:55:19.333' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (36, NULL, CAST(N'2020-01-05T07:55:48.737' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (37, NULL, CAST(N'2020-01-05T08:00:03.537' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (38, NULL, CAST(N'2020-01-05T08:00:32.750' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (39, NULL, CAST(N'2020-01-05T08:00:53.193' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (40, NULL, CAST(N'2020-01-05T08:36:03.507' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (41, NULL, CAST(N'2020-01-05T09:24:42.980' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (42, NULL, CAST(N'2020-01-05T09:24:58.253' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (43, NULL, CAST(N'2020-01-05T09:27:14.540' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (44, NULL, CAST(N'2020-01-05T09:30:09.250' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (45, NULL, CAST(N'2020-01-05T09:54:51.293' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (46, NULL, CAST(N'2020-01-05T09:59:04.003' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (47, NULL, CAST(N'2020-01-05T09:59:55.033' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (48, NULL, CAST(N'2020-01-05T10:04:27.757' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (49, NULL, CAST(N'2020-01-05T10:26:47.887' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (50, NULL, CAST(N'2020-01-05T10:27:48.047' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (51, NULL, CAST(N'2020-01-05T10:29:44.303' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (52, NULL, CAST(N'2020-01-05T10:30:01.170' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (53, NULL, CAST(N'2020-01-05T10:30:12.960' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (54, NULL, CAST(N'2020-01-05T10:33:30.907' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (55, NULL, CAST(N'2020-01-05T10:34:43.930' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (56, NULL, CAST(N'2020-01-05T10:42:00.250' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (57, NULL, CAST(N'2020-01-05T10:42:57.090' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (58, NULL, CAST(N'2020-01-05T10:44:06.187' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (59, NULL, CAST(N'2020-01-05T10:58:21.313' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (60, NULL, CAST(N'2020-01-05T11:16:44.703' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (61, NULL, CAST(N'2020-01-05T11:20:36.877' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (62, NULL, CAST(N'2020-01-05T11:35:00.897' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (63, NULL, CAST(N'2020-01-05T11:35:08.700' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (64, NULL, CAST(N'2020-01-05T11:35:38.653' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (65, NULL, CAST(N'2020-01-05T11:46:33.323' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (66, NULL, CAST(N'2020-01-05T11:51:31.557' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (67, NULL, CAST(N'2020-01-05T12:00:05.070' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (68, NULL, CAST(N'2020-01-05T12:06:49.327' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (69, NULL, CAST(N'2020-01-05T12:08:22.003' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (70, NULL, CAST(N'2020-01-05T12:42:25.230' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (71, NULL, CAST(N'2020-01-05T12:51:00.457' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (72, NULL, CAST(N'2020-01-05T12:53:41.250' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (73, NULL, CAST(N'2020-01-05T13:03:40.523' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (74, NULL, CAST(N'2020-01-05T13:12:33.237' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (75, NULL, CAST(N'2020-01-05T13:12:44.450' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (76, NULL, CAST(N'2020-01-05T13:14:35.137' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (77, NULL, CAST(N'2020-01-05T13:14:44.890' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (78, NULL, CAST(N'2020-01-05T13:16:35.153' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (79, NULL, CAST(N'2020-01-05T13:16:54.823' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (80, NULL, CAST(N'2020-01-05T13:38:17.003' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (81, NULL, CAST(N'2020-01-05T13:38:26.577' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (82, NULL, CAST(N'2020-01-05T13:38:33.647' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (83, NULL, CAST(N'2020-01-05T16:46:38.193' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (84, NULL, CAST(N'2020-01-05T16:47:04.613' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (85, NULL, CAST(N'2020-01-05T16:58:15.917' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (86, NULL, CAST(N'2020-01-05T17:01:02.743' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (87, NULL, CAST(N'2020-01-05T20:43:02.707' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (88, NULL, CAST(N'2020-01-06T10:15:35.873' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (89, NULL, CAST(N'2020-01-06T10:15:52.100' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (90, NULL, CAST(N'2020-01-06T16:45:10.103' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (91, NULL, CAST(N'2020-01-06T16:45:25.013' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (92, NULL, CAST(N'2020-01-06T20:24:43.517' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (93, NULL, CAST(N'2020-01-06T20:24:49.983' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (94, NULL, CAST(N'2020-01-06T20:24:57.963' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (95, NULL, CAST(N'2020-01-06T21:08:11.120' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (96, NULL, CAST(N'2020-01-06T21:12:36.743' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (97, NULL, CAST(N'2020-01-06T21:12:45.133' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (98, NULL, CAST(N'2020-01-07T00:02:58.023' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (99, NULL, CAST(N'2020-01-07T00:03:06.680' AS DateTime))
GO
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (100, NULL, CAST(N'2020-01-07T00:27:29.000' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (101, NULL, CAST(N'2020-01-07T00:27:36.923' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (102, NULL, CAST(N'2020-01-07T00:49:54.907' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (103, NULL, CAST(N'2020-01-07T00:50:06.427' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (104, NULL, CAST(N'2020-01-07T01:25:43.507' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (105, NULL, CAST(N'2020-01-07T01:25:58.423' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (106, NULL, CAST(N'2020-01-07T01:51:33.747' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (107, NULL, CAST(N'2020-01-07T01:51:42.013' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (108, NULL, CAST(N'2020-01-07T01:52:36.323' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (109, NULL, CAST(N'2020-01-07T02:22:19.423' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (110, NULL, CAST(N'2020-01-07T02:22:29.370' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (111, NULL, CAST(N'2020-01-07T22:10:52.493' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (112, NULL, CAST(N'2020-01-07T22:11:00.837' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (113, NULL, CAST(N'2020-01-07T22:39:38.660' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (114, NULL, CAST(N'2020-01-08T16:12:18.220' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (115, NULL, CAST(N'2020-01-08T18:17:50.707' AS DateTime))
INSERT [dbo].[Visiters] ([Id], [UserId], [CreatedOn]) VALUES (116, NULL, CAST(N'2020-01-09T10:30:04.467' AS DateTime))
SET IDENTITY_INSERT [dbo].[Visiters] OFF
SET IDENTITY_INSERT [dbo].[WebContentPages] ON 

INSERT [dbo].[WebContentPages] ([Id], [PageName], [PageName_Ar]) VALUES (1, N'About Organisation', N'?? ???????')
INSERT [dbo].[WebContentPages] ([Id], [PageName], [PageName_Ar]) VALUES (2, N'Help Center', N'???? ????????')
INSERT [dbo].[WebContentPages] ([Id], [PageName], [PageName_Ar]) VALUES (3, N'Contact Us', N'???? ???')
INSERT [dbo].[WebContentPages] ([Id], [PageName], [PageName_Ar]) VALUES (4, N'Terms of Service', N'???? ??????')
INSERT [dbo].[WebContentPages] ([Id], [PageName], [PageName_Ar]) VALUES (5, N'Privacy Policy', N'????? ????')
INSERT [dbo].[WebContentPages] ([Id], [PageName], [PageName_Ar]) VALUES (6, N'How it Works', N'??? ????')
SET IDENTITY_INSERT [dbo].[WebContentPages] OFF
SET IDENTITY_INSERT [dbo].[WebPageContent] ON 

INSERT [dbo].[WebPageContent] ([Id], [PageID], [PageContent], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [PageContent_Ar]) VALUES (1, 2, N'<!DOCTYPE html>
<html>
<head>
</head>
<body>
<section class="cs-g article-list" style="box-sizing: border-box; margin-bottom: 10px; float: left; width: 302.156px; margin-right: 30.8906px; clear: left; color: #474f60; font-family: ''Open Sans'', Helvetica, Arial, sans-serif; font-size: 14px; background-color: #ffffff;">
<div class="list-lead" style="box-sizing: border-box; line-height: 28.4375px; font-size: 18px; color: inherit; font-family: Arvo, Helvetica, sans-serif; margin: 10px 0px;"><a style="box-sizing: border-box; color: #474f60; text-decoration-line: none; font-size: 16px; font-weight: 600; letter-spacing: 0.2px;" title="About" href="https://help.oercommons.org/support/solutions/folders/42000070344">About&nbsp;<span class="item-count" style="box-sizing: border-box; display: inline-block; color: #9aa1a6; font-weight: normal; font-family: ''Open Sans'', Helvetica, Arial, sans-serif;">3</span></a></div>
<ul style="box-sizing: border-box; padding: 0px; margin: 10px 0px 0px; list-style-position: initial; list-style-image: initial;">
<li style="box-sizing: border-box; line-height: 21.875px; list-style: none; position: relative; padding-left: 30px; margin-bottom: 10px; background-image: none;">
<div class="ellipsis" style="box-sizing: border-box; white-space: nowrap; overflow: hidden; text-overflow: ellipsis;"><a style="box-sizing: border-box; color: #0b6a8e; text-decoration-line: none; line-height: 1.6;" href="https://help.oercommons.org/support/solutions/articles/42000014831-about-oer-commons">About OER Commons</a></div>
</li>
<li style="box-sizing: border-box; line-height: 21.875px; list-style: none; position: relative; padding-left: 30px; margin-bottom: 10px; background-image: none;">
<div class="ellipsis" style="box-sizing: border-box; white-space: nowrap; overflow: hidden; text-overflow: ellipsis;"><a style="box-sizing: border-box; color: #0b6a8e; text-decoration-line: none; line-height: 1.6;" href="https://help.oercommons.org/support/solutions/articles/42000014832-oer-commons-partners">OER Commons Partners</a></div>
</li>
<li style="box-sizing: border-box; line-height: 21.875px; list-style: none; position: relative; padding-left: 30px; margin-bottom: 10px; background-image: none;">
<div class="ellipsis" style="box-sizing: border-box; white-space: nowrap; overflow: hidden; text-overflow: ellipsis;"><a style="box-sizing: border-box; color: #0b6a8e; text-decoration-line: none; line-height: 1.6;" href="https://help.oercommons.org/support/solutions/articles/42000014835-what-are-open-educational-resources-">What are Open Educational Resources?</a></div>
</li>
</ul>
</section>
<section class="cs-g article-list" style="box-sizing: border-box; margin-bottom: 10px; float: left; width: 302.156px; margin-right: 0px; clear: right; color: #474f60; font-family: ''Open Sans'', Helvetica, Arial, sans-serif; font-size: 14px; background-color: #ffffff;">
<div class="list-lead" style="box-sizing: border-box; line-height: 28.4375px; font-size: 18px; color: inherit; font-family: Arvo, Helvetica, sans-serif; margin: 10px 0px;"><a style="box-sizing: border-box; color: #474f60; text-decoration-line: none; font-size: 16px; font-weight: 600; letter-spacing: 0.2px;" title="FAQ" href="https://help.oercommons.org/support/solutions/folders/42000069914">FAQ&nbsp;<span class="item-count" style="box-sizing: border-box; display: inline-block; color: #9aa1a6; font-weight: normal; font-family: ''Open Sans'', Helvetica, Arial, sans-serif;">1</span></a></div>
<ul style="box-sizing: border-box; padding: 0px; margin: 10px 0px 0px; list-style-position: initial; list-style-image: initial;">
<li style="box-sizing: border-box; line-height: 21.875px; list-style: none; position: relative; padding-left: 30px; margin-bottom: 10px; background-image: none;">
<div class="ellipsis" style="box-sizing: border-box; white-space: nowrap; overflow: hidden; text-overflow: ellipsis;"><a style="box-sizing: border-box; color: #0b6a8e; text-decoration-line: none; line-height: 1.6;" href="https://help.oercommons.org/support/solutions/articles/42000014826-how-do-i-publish-my-content-in-other-languages-">How do I publish my content in other languages?</a></div>
</li>
</ul>
</section>
<section class="cs-g article-list" style="box-sizing: border-box; margin-bottom: 10px; float: left; width: 302.156px; margin-right: 30.8906px; clear: left; color: #474f60; font-family: ''Open Sans'', Helvetica, Arial, sans-serif; font-size: 14px; background-color: #ffffff;">
<div class="list-lead" style="box-sizing: border-box; line-height: 28.4375px; font-size: 18px; color: inherit; font-family: Arvo, Helvetica, sans-serif; margin: 10px 0px;"><a style="box-sizing: border-box; color: #474f60; text-decoration-line: none; font-size: 16px; font-weight: 600; letter-spacing: 0.2px;" title="OER Commons Help" href="https://help.oercommons.org/support/solutions/folders/42000070354">OER Commons Help&nbsp;<span class="item-count" style="box-sizing: border-box; display: inline-block; color: #9aa1a6; font-weight: normal; font-family: ''Open Sans'', Helvetica, Arial, sans-serif;">3</span></a></div>
<ul style="box-sizing: border-box; padding: 0px; margin: 10px 0px 0px; list-style-position: initial; list-style-image: initial;">
<li style="box-sizing: border-box; line-height: 21.875px; list-style: none; position: relative; padding-left: 30px; margin-bottom: 10px; background-image: none;">
<div class="ellipsis" style="box-sizing: border-box; white-space: nowrap; overflow: hidden; text-overflow: ellipsis;"><a style="box-sizing: border-box; color: #0b6a8e; text-decoration-line: none; line-height: 1.6;" href="https://help.oercommons.org/support/solutions/articles/42000014833-how-resources-are-organized">How Resources Are Organized</a></div>
</li>
<li style="box-sizing: border-box; line-height: 21.875px; list-style: none; position: relative; padding-left: 30px; margin-bottom: 10px; background-image: none;">
<div class="ellipsis" style="box-sizing: border-box; white-space: nowrap; overflow: hidden; text-overflow: ellipsis;"><a style="box-sizing: border-box; color: #0b6a8e; text-decoration-line: none; line-height: 1.6;" href="https://help.oercommons.org/support/solutions/articles/42000014834-finding-resources">Finding Resources</a></div>
</li>
<li style="box-sizing: border-box; line-height: 21.875px; list-style: none; position: relative; padding-left: 30px; margin-bottom: 10px; background-image: none;">
<div class="ellipsis" style="box-sizing: border-box; white-space: nowrap; overflow: hidden; text-overflow: ellipsis;"><a style="box-sizing: border-box; color: #b2d46f; text-decoration-line: none; outline: 0px; outline-offset: -2px; line-height: 1.6;" href="https://help.oercommons.org/support/solutions/articles/42000014836-member-tools">Member Tools</a>&nbsp; &nbsp;&nbsp;</div>
</li>
</ul>
</section>
</body>
</html>', 1, CAST(N'2019-05-27T12:32:39.000' AS DateTime), 1, CAST(N'2019-11-21T14:35:52.337' AS DateTime), N'<!DOCTYPE html>
<html>
<head>
</head>
<body>
<p dir="RTL" style="margin-top: 0in; text-align: right; background: #F0F0F0; direction: rtl; unicode-bidi: embed;"><span lang="AR-SA" style="font-size: 14.0pt; mso-ansi-font-size: 13.5pt; mso-ascii-font-family: DroidArabicKufi; mso-hansi-font-family: DroidArabicKufi; color: #121212;">نبذة عن منارة</span></p>
<p dir="RTL" style="margin-top: 0in; text-align: justify; background: #F0F0F0; direction: rtl; unicode-bidi: embed;"><span lang="AR-SA" style="font-size: 14.0pt; mso-ansi-font-size: 13.5pt; mso-ascii-font-family: DroidArabicKufi; mso-hansi-font-family: DroidArabicKufi; color: #121212;">الأسئلة الشّائعة حول منصّة "منارة".</span></p>
<h1 dir="RTL" style="margin: 15.0pt 0in 7.5pt 0in;"><span lang="AR-SA" style="font-size: 14.0pt; mso-ansi-font-size: 13.5pt; line-height: 107%; font-family: ''Times New Roman'',serif; mso-ascii-font-family: DroidArabicKufi; mso-hansi-font-family: DroidArabicKufi; mso-bidi-font-family: ''Times New Roman''; mso-bidi-theme-font: major-bidi; color: #121212;">ما هي الموارد التعليمية المفتوحة؟؟؟</span></h1>
<p style="padding-left: 40px;"><span lang="AR-SA" style="font-size: 14.0pt; mso-ansi-font-size: 13.5pt; line-height: 107%; font-family: ''Times New Roman'',serif; mso-ascii-font-family: DroidArabicKufi; mso-hansi-font-family: DroidArabicKufi; mso-bidi-font-family: ''Times New Roman''; mso-bidi-theme-font: major-bidi; color: #121212;">الشرط الأول </span></p>
</body>
</html>')
INSERT [dbo].[WebPageContent] ([Id], [PageID], [PageContent], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [PageContent_Ar]) VALUES (2, 1, N'<!DOCTYPE html>
<html>
<head>
</head>
<body>
<p><span style="font-size: 10.0pt; line-height: 107%; font-family: ''Book Antiqua'',serif; mso-fareast-font-family: Calibri; mso-fareast-theme-font: minor-latin; mso-bidi-font-family: ''Simplified Arabic''; color: #121212; mso-ansi-language: EN-US; mso-fareast-language: EN-US; mso-bidi-language: AR-SA;">The UAE Ministry of Education launched (Manara) platform for the open educational sources, in order to support the learning and teaching all over the world, promote the culture of participation in the open learning, and provide a platform for the reliable Arab educational and learning sources, based in&nbsp;</span><span style="color: #121212; font-family: ''Book Antiqua'', serif; font-size: 10pt;">United Arab Emirates across the region and the world. These resources are &ldquo;open&rdquo; for all of us, and available for use in the learning and teaching. In addition, the resources and pathways available on the (Manara) platform contain licenses and copyrights to legitimize the use of these various educational resources and pathways.&nbsp;</span></p>
<p><span style="font-size: 10.0pt; line-height: 107%; font-family: ''Book Antiqua'',serif; mso-fareast-font-family: Calibri; mso-fareast-theme-font: minor-latin; mso-bidi-font-family: ''Simplified Arabic''; color: #121212; mso-ansi-language: EN-US; mso-fareast-language: EN-US; mso-bidi-language: AR-SA;">The platform engages community with rich educational resources which can support and develop the high quality educational curricula&nbsp; and meaningful learning, and enable students access to the educational sources and educational pathways which lead them to learn. Moreover, platform provides the basic system and tools required for users to effectively use it, create, share, amend and review of the open educational resources, as well as collaborate with others to create and discuss the open educational resources. The resources available include a wide range of the open educational resources, which will contribute to promote the resources based on Arabic language in the United Arab Emirates and region.</span></p>
<p>&nbsp;</p>
</body>
</html>', 1, CAST(N'2019-05-27T07:27:34.790' AS DateTime), 1, CAST(N'2019-11-22T08:53:39.487' AS DateTime), N'<!DOCTYPE html>
<html>
<head>
</head>
<body>
<p class="MsoNormal" style="text-align: right;" align="right"><strong><span dir="RTL" lang="AR-AE" style="font-size: 12.0pt; line-height: 107%; font-family: ''Sakkal Majalla''; mso-hansi-font-family: ''Sakkal Majalla''; mso-bidi-language: AR-AE;">حول منصة "منارة"</span></strong></p>
<p class="MsoNormal" style="mso-margin-bottom-alt: auto; text-align: right; mso-outline-level: 5;" align="right"><span dir="RTL" lang="AR">&nbsp;</span><span dir="RTL" lang="AR">دشّنت</span><span dir="RTL" lang="AR"> وزارة التّربية والتّعليم في دولة الإمارات العربية المتحدة منصّة (منارة) </span><span dir="RTL" lang="AR-AE">للمصادر التّعليمية المفتوحة</span><span dir="RTL" lang="AR-AE">؛</span> <span dir="RTL" lang="AR">من أجل دعم التّعليم والتعلّم على مستوى العالم، وكذلك من أجل تعزيز ثقافة المشاركة في التعليم المفتوح، وتوفير منصة للمصادر التربوية والتعليمية العربية الموثوقة، تكون قاعدتها &nbsp;دولة الإمارات العربية المتحدة على مستوى المنطقة والعالم. وهذه الموارد "مفتوحة" لنا جميعاً، ومتاحة للاستخدام في التعليم والتعلم. وبالإضافة إلى ذلك تحتوي الموارد والمساقات الموجودة على منصة (منارة) على تراخيص وحقوق نشر لإضفاء الشرعية على استخدام هذه الموارد والمساقات التعليمية المختلفة.</span></p>
<p id="tw-target-text" dir="rtl" data-placeholder="Translation"><span lang="ar" tabindex="0">وتعمل المنصة على إشراك المجتمع بموارد تعليمية غنية يمكنها دعم وتطوير المناهج التعليمية العالية الجودة، والتّعليم الهادف، وتمكين وصول الطلاب إلى المصادر التعليمية والمساقات التربوية التي تقودهم للتعلّم. بالإضافة إلى ذلك، توفر المنصة النظام الأساسي والأدوات اللازمة للمستخدمين لاستخدامها بفعالية، وإنشاء ومشاركة وتعديل ومراجعة الموارد التعليمية المفتوحة، بالإضافة إلى التعاون مع الآخرين؛ لإنشاء ومناقشة الموارد التعليمية المفتوحة. وتشمل</span>الموارد المتوفرة مجموعة واسعة من الموارد التعليمية المفتوحة، والتي من شأنها أن تسهم في تعزيز الموارد القائمة على اللغة العربية في دولة الإمارات العربية المتحدة والمنطقة.</p>
</body>
</html>')
INSERT [dbo].[WebPageContent] ([Id], [PageID], [PageContent], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [PageContent_Ar]) VALUES (3, 5, N'<!DOCTYPE html>
<html>
<head>
</head>
<body>
<h3 style="box-sizing: border-box; margin: 0px 0px 10px; padding: 0px; border: 0px; outline: 0px; font-variant-numeric: normal; font-variant-east-asian: normal; font-weight: 400; font-stretch: normal; font-size: 24px; line-height: 1.4; font-family: Arvo, serif; vertical-align: baseline; background: #f1f2f2; color: #474f60;">The Movement</h3>
<p style="box-sizing: border-box; margin: 0px 0px 10px; padding: 0px; border: 0px; outline: 0px; font-size: 16px; vertical-align: baseline; background: #f1f2f2; color: #474f60; font-family: ''Open Sans'', sans-serif;">The worldwide OER movement is rooted in the human right to access high-quality education. This shift in educational practice is not just about cost savings and easy access to openly licensed content; it&rsquo;s about participation and co-creation. Open Educational Resources (OER) offer opportunities for systemic change in teaching and learning content through engaging educators in new participatory processes and effective technologies for engaging with learning.</p>
<h3 style="box-sizing: border-box; margin: 0px 0px 10px; padding: 0px; border: 0px; outline: 0px; font-variant-numeric: normal; font-variant-east-asian: normal; font-weight: 400; font-stretch: normal; font-size: 24px; line-height: 1.4; font-family: Arvo, serif; vertical-align: baseline; background: #ffffff; color: #474f60;">Open Education Practice</h3>
<p style="box-sizing: border-box; margin: 0px 0px 10px; padding: 0px; border: 0px; outline: 0px; font-size: 16px; vertical-align: baseline; background: #ffffff; color: #474f60; font-family: ''Open Sans'', sans-serif;">The move to open education practice (OEP) is more than a shift in content, it is an immersive experience in collaborative teaching and learning. OEP leverages open education resources (OER) to expand the role of educators, allowing teachers to become curators, curriculum designers, and content creators. In sharing teaching tools and strategies, educators network their strengths and improve the quality of education for their students. </p>
<p style="box-sizing: border-box; margin: 0px; padding: 0px; border: 0px; outline: 0px; font-size: 16px; vertical-align: baseline; background: #ffffff; color: #474f60; font-family: ''Open Sans'', sans-serif;">With an open practice, educators are able to adjust their content, pedagogies, and approach based on their learners, without the limitations of &ldquo;all rights reserved&rdquo;.</p>
</body>
</html>', 1, CAST(N'2019-05-27T13:39:18.067' AS DateTime), 1, CAST(N'2019-10-26T10:17:54.703' AS DateTime), N'<!DOCTYPE html>
<html>
<head>
</head>
<body>
<p>الحركة</p>
<p>تتجذر حركة الموارد التعليمية المفتوحة في جميع أنحاء العالم في حق الإنسان في الحصول على تعليم عالي الجودة. هذا التحول في الممارسة التعليمية لا يقتصر فقط على توفير التكاليف وسهولة الوصول إلى المحتوى المرخّص بشكل مفتوح ؛ إنه يتعلق بالمشاركة والإبداع المشترك. توفر المصادر التعليمية المفتوحة (OER) فرصًا للتغيير المنهجي في محتوى التعليم والتعلم من خلال إشراك المعلمين في العمليات التشاركية الجديدة والتقنيات الفعالة للمشاركة في التعلم.</p>
<p>&nbsp;</p>
<p>ممارسة التعليم المفتوح</p>
<p>الانتقال إلى ممارسة التعليم المفتوح (OEP) هو أكثر من مجرد تحول في المحتوى ، بل هو تجربة غامرة في التعليم والتعلم التعاوني. تعزز OEP موارد التعليم المفتوح (OER) لتوسيع دور المعلمين ، مما يسمح للمعلمين بأن يصبحوا منسقين ، ومصممي المناهج ، ومنشئي المحتوى. عند مشاركة الأدوات والاستراتيجيات التعليمية ، يقوم المعلمون بربط نقاط قوتهم وتحسين جودة التعليم لطلابهم.</p>
<p>&nbsp;</p>
<p>من خلال الممارسة المفتوحة ، يكون المعلمون قادرين على ضبط محتواهم وتعليمهم ونهجهم استنادًا إلى متعلميهم ، دون قيود "جميع الحقوق محفوظة".</p>
</body>
</html>')
INSERT [dbo].[WebPageContent] ([Id], [PageID], [PageContent], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [PageContent_Ar]) VALUES (4, 4, N'<!DOCTYPE html>
<html>
<head>
</head>
<body>
<p><span style="font-size: 10pt; line-height: 107%; font-family: ''Book Antiqua'', serif;">The terms and conditions in the &ldquo;terms and conditions&rdquo; document are intended to technically and morally regulate the services, in order to provide the best level of service for all customers. This agreement is for kind information purposes only. Manara platform reserves the right to change&nbsp; any paragraph or/ and paragraphs contained in this document without obligation to notify the customer.</span></p>
<p><span style="font-size: 10pt; line-height: 107%; font-family: ''Book Antiqua'', serif;"><span style="font-size: 10pt; line-height: 107%;">You must read this document carefully before requesting any of the (Manara) services.</span></span></p>
<p><span style="font-size: 10pt; line-height: 107%; font-family: ''Book Antiqua'', serif;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;">The provision of service by us is based primarily on your full consent to all what contained in the terms and conditions document.</span></span></span></p>
<p><span style="font-size: 10pt; line-height: 107%; font-family: ''Book Antiqua'', serif;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;">General conditions</span></span></span></span></p>
<p><span style="font-size: 10pt; line-height: 107%; font-family: ''Book Antiqua'', serif;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;">1- Manara reserves the right to reject and / or stop any service provided by Manara team to any person or entity without obligation to give reasons.</span></span></span></span></span></p>
<p><span style="font-size: 10pt; line-height: 107%; font-family: ''Book Antiqua'', serif;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;">2- Manara is committed to being clear and transparent with its customers in all stages of service provision.</span></span></span></span></span></span></p>
<p><span style="font-size: 10pt; line-height: 107%; font-family: ''Book Antiqua'', serif;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;">3- The circumvention in the use of any services shall be deemed a breach of the agreement between the customer and Manara platform.</span></span></span></span></span></span></span></p>
<p><span style="font-size: 10pt; line-height: 107%; font-family: ''Book Antiqua'', serif;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;">The fair usage</span></span></span></span></span></span></span></span></p>
<p><span style="font-size: 10pt; line-height: 107%; font-family: ''Book Antiqua'', serif;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;">- </span><span style="font-size: 10pt; line-height: 107%;">Manara has the fair usage policy in the use of resources and services provided by us, and reserves its right to take what it sees fit for the services that misuse the resources, which may affect the level of quality provided.</span></span></span></span></span></span></span></span></span></p>
<p><span style="font-size: 10pt; line-height: 107%; font-family: ''Book Antiqua'', serif;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;">-Consumption of the processors and memory on the servers is measured to ensure they dont reach the maximum per package.</span></span></span></span></span></span></span></span></span></span></p>
<p><span style="font-size: 10pt; line-height: 107%; font-family: ''Book Antiqua'', serif;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;">-The fair usage policy applies to all pathways and resources, which are uploaded on the Manara platform</span></span></span></span></span></span></span></span></span></span></p>
<p><span style="font-size: 10pt; line-height: 107%; font-family: ''Book Antiqua'', serif;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><strong><span style="font-size: 10pt; line-height: 107%;">Reactivation of the customer&rsquo;s account</span></strong></span></span></span></span></span></span></span></span></span></span></p>
<p><span style="font-size: 10pt; line-height: 107%; font-family: ''Book Antiqua'', serif;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;">In case of closure of the customer&rsquo;s account and his desire to reopen it, contact the technical support team.</span></span></span></span></span></span></span></span></span></span></span></p>
<p><strong><span style="font-size: 10pt; line-height: 107%; font-family: ''Book Antiqua'', serif;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;">Cancellation of services</span></span></span></span></span></span></span></span></span></span></span></span></strong></p>
<p><span style="font-size: 10pt; line-height: 107%; font-family: ''Book Antiqua'', serif;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;">Manara reserves the right to stop&nbsp; provision of technical support for the accounts in violation of the terms and conditions of services.</span></span></span></span></span></span></span></span></span></span></span></span></span></p>
<p><span style="font-size: 10pt; line-height: 107%; font-family: ''Book Antiqua'', serif;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;">In case the customer requests to cancel the service, this is checked with the customer, in order to cancel the service by (Manara)</span></span></span></span></span></span></span></span></span></span></span></span></span></span></p>
<p><strong><span style="font-size: 10pt; line-height: 107%; font-family: ''Book Antiqua'', serif;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;">Responsibility of (Manara)</span></span></span></span></span></span></span></span></span></span></span></span></span></span></span></strong></p>
<p><span style="font-size: 10pt; line-height: 107%; font-family: ''Book Antiqua'', serif;">Manara is not responsible for the content of sites it hosts or the opinions contained herein. Bust the customer bears the full responsibility before the legal and governmental entities.</span></p>
<p><span style="font-size: 10pt; line-height: 107%; font-family: ''Book Antiqua'', serif;"><span style="font-size: 10pt; line-height: 107%;">Manara is not responsible for any damages caused to <span style="background: white; mso-shading-themecolor: background1;">the customers</span> due to interruption of any service and exerts every efforts to provide the best possible performance of service.</span></span></p>
<p><span style="font-size: 10pt; line-height: 107%; font-family: ''Book Antiqua'', serif;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;">Manara is solely responsible for the technical aspect. The technical estimates are exclusively valued by the technical team of the platform.</span></span></span></p>
<p><span style="font-size: 10pt; line-height: 107%; font-family: ''Book Antiqua'', serif;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;">The customers is fully responsible for his account and contents associated with it in any Manara services. It may be also influenced by the external conditions, and we exert our best to ensure the best result.</span></span></span></span></p>
<p><span style="font-size: 10pt; line-height: 107%; font-family: ''Book Antiqua'', serif;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;">Licenses of programs and systems</span></span></span></span></span></p>
<p><span style="font-size: 10pt; line-height: 107%; font-family: ''Book Antiqua'', serif;">Manara commits to provide a licensed and legal services for the programs and systems used to deliver the services through it or by its respective team.</span></p>
<p><span style="font-size: 10pt; line-height: 107%; font-family: ''Book Antiqua'', serif;"><span style="font-size: 10pt; line-height: 107%;">Prohibited materials</span></span></p>
<p><span style="font-size: 10pt; line-height: 107%; font-family: ''Book Antiqua'', serif;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%; color: #333333;">1.&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span style="font-size: 10pt; line-height: 107%; color: #333333;">Using the site in order to commit an offense or to encourage others to involve in any actions which may be considered a crime or involve a civil liability.</span></span></span></p>
<p><span style="font-size: 10pt; line-height: 107%; font-family: ''Book Antiqua'', serif;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%; color: #333333;"><span style="font-size: 10pt; line-height: 107%;">2.&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span style="font-size: 10pt; line-height: 107%;">Including or publishing any unlawful contents which include discrimination, defamation, abuse or inappropriate materials.</span></span></span></span></p>
<p><span style="font-size: 10pt; line-height: 107%; font-family: ''Book Antiqua'', serif;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%; color: #333333;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;">3.&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span style="font-size: 10pt; line-height: 107%;">Using the site in order to impersonate other persons or parties.</span></span></span></span></span></p>
<p><span style="font-size: 10pt; line-height: 107%; font-family: ''Book Antiqua'', serif;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%; color: #333333;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;">4.&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span style="font-size: 10pt; line-height: 107%;">Using the site in order to upload any material has programs containing viruses, Trujan horses, and any computer codes, files and programs&nbsp; that may alter, damage or impede the site, any device or program belonging to any person accesses the site.</span></span></span></span></span></span></p>
</body>
</html>', 1, CAST(N'2019-05-27T15:42:29.723' AS DateTime), 1, CAST(N'2019-11-23T08:00:51.510' AS DateTime), N'<!DOCTYPE html>
<html>
<head>
</head>
<body>
<p>هدف</p>
<p>يتم توفير محتويات وخدمات ACU-OER من قبل أمانة ACU في دائرة المعلومات التعليمية والبحثية الكورية (KERIS). يرجى قراءة شروط الاستخدام هذه ("الشروط") وسياسة خصوصية ACU-OER قبل التسجيل في www.aseanoer.net أو استخدام أي جزء من موقع ACU-OER ("الموقع الإلكتروني" ، الذي يتكون من جميع المحتويات و الصفحات الموجودة داخل نطاق الويب www.aseanoer.net). يمكنك الوصول إلى موقع الويب ، بما في ذلك الوصول إلى أي مواد تعليمية أو لوحات إعلانات أو خدمات إلكترونية أخرى ("الخدمات"). نظرًا لأن بعض خدماتنا قد تكون برنامجًا يتم تنزيله على أجهزة الكمبيوتر أو الهواتف أو الأجهزة اللوحية أو الأجهزة الأخرى ، فإنك توافق على أننا قد نقوم بتحديث هذا البرنامج تلقائيًا ، وأن هذه الشروط سوف تنطبق على هذه التحديثات. هذه الشروط وسياسة الخصوصية هي اتفاقيات ("الاتفاقيات") بينك وبين ACU-OER. باستخدام خدماتنا ، فإنك توافق على الالتزام بهذه الشروط ، بما في ذلك سياسة الخصوصية ، سواء كنت مستخدماً مسجلاً أم لا. إذا كنت لا تفهم أو لا توافق على الالتزام بشروط الاتفاقيات ، فيجب ألا تستخدم موقع الويب.</p>
<p>تأثير وتعديل الشروط</p>
<p>أ. الخدمات جميع الخدمات المتعلقة بالتعلم عبر الإنترنت التي تقدمها ACU-OER.</p>
<p>ب. ACU-OER لموقع الويب وصفحات LMS الرئيسية و CMS ، والتي تتكون من جميع المحتوى والصفحات الموجودة داخل نطاق الويب www.aseanoer.net</p>
<p>ج. المستخدم أي شخص يوافق على هذه الشروط من خلال تسجيل الدخول إلى الموقع الإلكتروني ويسجل حساب المستخدم الخاص به / لها في موقع ACU-OER من خلال تقديم معلوماته الشخصية ومصرح له باستخدام الخدمات التي يوفرها ACT</p>
<p>د. اتفاقيات استخدام الخدمة - جميع الاتفاقيات ، بما في ذلك هذه الشروط ، المبرمة بين ACU-OER وأعضائها فيما يتعلق باستخدام الخدمات</p>
<p>ه. كلمة المرور هي مجموعة من الأحرف والأرقام الفريدة التي يعدها العضو لمصادقة المستخدم</p>
<p>F. الانسحاب من اتفاقات استخدام الخدمة من قبل ACU-OER أو من قبل العضو</p>
<p>يجوز لأي مصطلح لم يتم تعريفه في ما سبق من بين المصطلحات المستخدمة في هذه الشروط اتباع القوانين والأدلة ذات الصلة على كل موقع ، أو الامتثال بطريقة أخرى للممارسات العرفية.</p>
<p>&nbsp;</p>
<p>&nbsp;</p>
</body>
</html>')
INSERT [dbo].[WebPageContent] ([Id], [PageID], [PageContent], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [PageContent_Ar]) VALUES (5, 3, N'<!DOCTYPE html>
<html>
<head>
</head>
<body>
<p>This is a text message for contact us. This is a text message for contact us. This is a text message for contact us. This is a text message for contact us. This is a text message for contact us. This is a text message for contact us. This is a text message for contact us. This is a text message for contact us. This is a text message for contact us. This is a text message for contact us. This is a text message for contact us. This is a text message for contact us. This is a text message for contact us. This is a text message for contact us. This is a text message for contact us. This is a text message for contact us. This is a text message for contact us. This is a text message for contact us. This is a text message for contact us. This is a text message for contact us.&nbsp;</p>
<p>&nbsp;</p>
</body>
</html>', 1, CAST(N'2019-05-29T06:43:34.720' AS DateTime), 1, CAST(N'2019-10-26T10:20:40.203' AS DateTime), N'<!DOCTYPE html>
<html>
<head>
</head>
<body>
<p>هذه رسالة نصية للاتصال بنا. هذه رسالة نصية للاتصال بنا. هذه رسالة نصية للاتصال بنا. هذه رسالة نصية للاتصال بنا. هذه رسالة نصية للاتصال بنا. هذه رسالة نصية للاتصال بنا. هذه رسالة نصية للاتصال بنا. هذه رسالة نصية للاتصال بنا. هذه رسالة نصية للاتصال بنا. هذه رسالة نصية للاتصال بنا. هذه رسالة نصية للاتصال بنا. هذه رسالة نصية للاتصال بنا. هذه رسالة نصية للاتصال بنا. هذه رسالة نصية للاتصال بنا. هذه رسالة نصية للاتصال بنا. هذه رسالة نصية للاتصال بنا. هذه رسالة نصية للاتصال بنا. هذه رسالة نصية للاتصال بنا. هذه رسالة نصية للاتصال بنا. هذه رسالة نصية للاتصال بنا.</p>
</body>
</html>')
INSERT [dbo].[WebPageContent] ([Id], [PageID], [PageContent], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [PageContent_Ar]) VALUES (8, 6, N'<!DOCTYPE html>
<html>
<head>
</head>
<body>
<p>This is how it works!!!!&nbsp;&nbsp;</p>
</body>
</html>', 1, CAST(N'2019-06-06T10:11:17.627' AS DateTime), 1, CAST(N'2019-10-26T10:18:13.100' AS DateTime), N'<!DOCTYPE html>
<html>
<head>
</head>
<body>
<p>هذه بيانات اختبار لكيفية عملها</p>
</body>
</html>')
SET IDENTITY_INSERT [dbo].[WebPageContent] OFF
SET IDENTITY_INSERT [dbo].[WhiteListingURLs] ON 

INSERT [dbo].[WhiteListingURLs] ([Id], [RequestedBy], [VerifiedBy], [URL], [IsApproved], [RequestedOn], [VerifiedOn], [RejectedReason], [IsActive]) VALUES (CAST(1 AS Numeric(18, 0)), 8, NULL, N'http://test.com', 0, CAST(N'2020-01-05' AS Date), NULL, NULL, 1)
SET IDENTITY_INSERT [dbo].[WhiteListingURLs] OFF
ALTER TABLE [dbo].[CategoryMaster] ADD  CONSTRAINT [DF_CategoryMaster_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[CategoryMaster] ADD  CONSTRAINT [DF_CategoryMaster_UpdatedOn]  DEFAULT (getdate()) FOR [UpdatedOn]
GO
ALTER TABLE [dbo].[CategoryMaster] ADD  CONSTRAINT [DF_CategoryMaster_Active]  DEFAULT ((0)) FOR [Active]
GO
ALTER TABLE [dbo].[CategoryMaster] ADD  CONSTRAINT [DF_CategoryMaster_Status]  DEFAULT ((1)) FOR [Status]
GO
ALTER TABLE [dbo].[CommunityCheckMaster] ADD  DEFAULT ((1)) FOR [IsCurrent]
GO
ALTER TABLE [dbo].[CopyrightMaster] ADD  CONSTRAINT [DF_CopyrightMaster_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[CopyrightMaster] ADD  CONSTRAINT [DF_CopyrightMaster_UpdatedOn]  DEFAULT (getdate()) FOR [UpdatedOn]
GO
ALTER TABLE [dbo].[CopyrightMaster] ADD  CONSTRAINT [DF_CopyrightMaster_Active]  DEFAULT ((0)) FOR [Active]
GO
ALTER TABLE [dbo].[CopyrightMaster] ADD  DEFAULT ((0)) FOR [Protected]
GO
ALTER TABLE [dbo].[CopyrightMaster] ADD  DEFAULT ((0)) FOR [IsResourceProtect]
GO
ALTER TABLE [dbo].[CopyrightMaster] ADD  CONSTRAINT [DF_CopyrightMaster_Status]  DEFAULT ((1)) FOR [Status]
GO
ALTER TABLE [dbo].[CountryMaster] ADD  CONSTRAINT [DF_CountryMaster_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[CountryMaster] ADD  CONSTRAINT [DF_CountryMaster_UpdatedOn]  DEFAULT (getdate()) FOR [UpdatedOn]
GO
ALTER TABLE [dbo].[CountryMaster] ADD  CONSTRAINT [DF_CountryMaster_IsActive]  DEFAULT ((0)) FOR [IsActive]
GO
ALTER TABLE [dbo].[CourseApprovals] ADD  CONSTRAINT [DF_CourseApprovals_ApprovedOn]  DEFAULT (getdate()) FOR [ApprovedOn]
GO
ALTER TABLE [dbo].[CourseAssociatedFiles] ADD  CONSTRAINT [DF_CourseAssociatedFiles_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[CourseAssociatedFiles] ADD  DEFAULT ((1)) FOR [IsInclude]
GO
ALTER TABLE [dbo].[CourseComments] ADD  CONSTRAINT [DF_CourseComments_CommentDate]  DEFAULT (getdate()) FOR [CommentDate]
GO
ALTER TABLE [dbo].[CourseComments] ADD  CONSTRAINT [DF_CourseComments_ReportAbuseCount]  DEFAULT ((0)) FOR [ReportAbuseCount]
GO
ALTER TABLE [dbo].[CourseComments] ADD  CONSTRAINT [DF_CourseComments_IsHidden]  DEFAULT ((0)) FOR [IsHidden]
GO
ALTER TABLE [dbo].[CourseMaster] ADD  CONSTRAINT [DF_CourseMaster_IsDraft]  DEFAULT ((0)) FOR [IsDraft]
GO
ALTER TABLE [dbo].[CourseMaster] ADD  CONSTRAINT [DF_CourseMaster_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[CourseMaster] ADD  CONSTRAINT [DF_CourseMaster_IsApproved]  DEFAULT ((0)) FOR [IsApproved]
GO
ALTER TABLE [dbo].[CourseMaster] ADD  CONSTRAINT [DF_CourseMaster_Rating]  DEFAULT ((0)) FOR [Rating]
GO
ALTER TABLE [dbo].[CourseMaster] ADD  CONSTRAINT [DF_CourseMaster_ReportAbuseCount]  DEFAULT ((0)) FOR [ReportAbuseCount]
GO
ALTER TABLE [dbo].[CourseMaster] ADD  DEFAULT ((0)) FOR [CommunityBadge]
GO
ALTER TABLE [dbo].[CourseMaster] ADD  DEFAULT ((0)) FOR [MoEBadge]
GO
ALTER TABLE [dbo].[CourseMaster] ADD  DEFAULT ((0)) FOR [IsApprovedSensory]
GO
ALTER TABLE [dbo].[CourseRating] ADD  CONSTRAINT [DF_CourseRating_Rating]  DEFAULT ((0)) FOR [Rating]
GO
ALTER TABLE [dbo].[CourseRating] ADD  CONSTRAINT [DF_CourseRating_RatedOn]  DEFAULT (getdate()) FOR [RatedOn]
GO
ALTER TABLE [dbo].[CourseURLReferences] ADD  CONSTRAINT [DF_CourseURLReferences_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[DepartmentMaster] ADD  CONSTRAINT [DF_DepartmentMaster_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[DepartmentMaster] ADD  CONSTRAINT [DF_DepartmentMaster_UpdatedOn]  DEFAULT (getdate()) FOR [UpdatedOn]
GO
ALTER TABLE [dbo].[DepartmentMaster] ADD  CONSTRAINT [DF_DepartmentMaster_Active]  DEFAULT ((0)) FOR [Active]
GO
ALTER TABLE [dbo].[DesignationMaster] ADD  CONSTRAINT [DF_DesignationMaster_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[DesignationMaster] ADD  CONSTRAINT [DF_DesignationMaster_UpdatedOn]  DEFAULT (getdate()) FOR [UpdatedOn]
GO
ALTER TABLE [dbo].[DesignationMaster] ADD  CONSTRAINT [DF_DesignationMaster_Active]  DEFAULT ((0)) FOR [Active]
GO
ALTER TABLE [dbo].[EducationMaster] ADD  CONSTRAINT [DF_EducationMaster_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[EducationMaster] ADD  CONSTRAINT [DF_EducationMaster_UpdatedOn]  DEFAULT (getdate()) FOR [UpdatedOn]
GO
ALTER TABLE [dbo].[EducationMaster] ADD  CONSTRAINT [DF_EducationMaster_Active]  DEFAULT ((0)) FOR [Active]
GO
ALTER TABLE [dbo].[EducationMaster] ADD  CONSTRAINT [DF_EducationMaster_Status]  DEFAULT ((1)) FOR [Status]
GO
ALTER TABLE [dbo].[GradeMaster] ADD  CONSTRAINT [DF_GradeMaster_Active]  DEFAULT ((1)) FOR [Active]
GO
ALTER TABLE [dbo].[InstitutionMaster] ADD  CONSTRAINT [DF_InstitutionMaster_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[InstitutionMaster] ADD  CONSTRAINT [DF_InstitutionMaster_UpdatedOn]  DEFAULT (getdate()) FOR [UpdatedOn]
GO
ALTER TABLE [dbo].[InstitutionMaster] ADD  CONSTRAINT [DF_InstitutionMaster_Active]  DEFAULT ((0)) FOR [Active]
GO
ALTER TABLE [dbo].[LanguageMaster] ADD  CONSTRAINT [DF_LanguageMaster_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[LanguageMaster] ADD  CONSTRAINT [DF_LanguageMaster_UpdatedOn]  DEFAULT (getdate()) FOR [UpdatedOn]
GO
ALTER TABLE [dbo].[LanguageMaster] ADD  CONSTRAINT [DF_LanguageMaster_Active]  DEFAULT ((0)) FOR [Active]
GO
ALTER TABLE [dbo].[LogAction] ADD  CONSTRAINT [DF_LogAction_ActionDate]  DEFAULT (getdate()) FOR [ActionDate]
GO
ALTER TABLE [dbo].[LogError] ADD  CONSTRAINT [DF_LogError_MessageDate]  DEFAULT (getdate()) FOR [MessageDate]
GO
ALTER TABLE [dbo].[lu_Educational_Standard] ADD  CONSTRAINT [DF_lu_Educational_Standard_Status]  DEFAULT ((1)) FOR [Status]
GO
ALTER TABLE [dbo].[lu_Educational_Use] ADD  CONSTRAINT [DF_lu_Educational_Use_Status]  DEFAULT ((1)) FOR [Status]
GO
ALTER TABLE [dbo].[lu_Level] ADD  CONSTRAINT [DF_lu_Level_Status]  DEFAULT ((1)) FOR [Status]
GO
ALTER TABLE [dbo].[MaterialTypeMaster] ADD  CONSTRAINT [DF_MaterialTypeMaster_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[MaterialTypeMaster] ADD  CONSTRAINT [DF_MaterialTypeMaster_UpdatedOn]  DEFAULT (getdate()) FOR [UpdatedOn]
GO
ALTER TABLE [dbo].[MaterialTypeMaster] ADD  CONSTRAINT [DF_MaterialTypeMaster_Active]  DEFAULT ((0)) FOR [Active]
GO
ALTER TABLE [dbo].[MaterialTypeMaster] ADD  CONSTRAINT [DF_MaterialTypeMaster_Status]  DEFAULT ((1)) FOR [Status]
GO
ALTER TABLE [dbo].[MoECheckMaster] ADD  DEFAULT ((1)) FOR [IsCurrent]
GO
ALTER TABLE [dbo].[Notifications] ADD  CONSTRAINT [DF_Notifications_Status]  DEFAULT ((1)) FOR [Status]
GO
ALTER TABLE [dbo].[ProfessionMaster] ADD  CONSTRAINT [DF_ProfessionMaster_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[ProfessionMaster] ADD  CONSTRAINT [DF_ProfessionMaster_UpdatedOn]  DEFAULT (getdate()) FOR [UpdatedOn]
GO
ALTER TABLE [dbo].[ProfessionMaster] ADD  CONSTRAINT [DF_ProfessionMaster_Active]  DEFAULT ((0)) FOR [Active]
GO
ALTER TABLE [dbo].[ProfessionMaster] ADD  CONSTRAINT [DF_ProfessionMaster_Status]  DEFAULT ((1)) FOR [Status]
GO
ALTER TABLE [dbo].[QRCCategory] ADD  DEFAULT ((0)) FOR [IsAvailable]
GO
ALTER TABLE [dbo].[QRCMaster] ADD  CONSTRAINT [DF_QRCMaster_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[QRCMaster] ADD  CONSTRAINT [DF_QRCMaster_UpdatedOn]  DEFAULT (getdate()) FOR [UpdatedOn]
GO
ALTER TABLE [dbo].[QRCMaster] ADD  CONSTRAINT [DF_QRCMaster_Active]  DEFAULT ((0)) FOR [Active]
GO
ALTER TABLE [dbo].[ResourceAlignmentRating] ADD  CONSTRAINT [DF_ResourceAlignmentRating_Rating]  DEFAULT ((0)) FOR [Rating]
GO
ALTER TABLE [dbo].[ResourceAlignmentRating] ADD  CONSTRAINT [DF_ResourceAlignmentRating_RatedOn]  DEFAULT (getdate()) FOR [RatedOn]
GO
ALTER TABLE [dbo].[ResourceApprovals] ADD  CONSTRAINT [DF_ResourceApprovals_ApprovedOn]  DEFAULT (getdate()) FOR [ApprovedOn]
GO
ALTER TABLE [dbo].[ResourceAssociatedFiles] ADD  CONSTRAINT [DF_ResourceAssociatedFiles_UploadedDate]  DEFAULT (getdate()) FOR [UploadedDate]
GO
ALTER TABLE [dbo].[ResourceAssociatedFiles] ADD  DEFAULT ((1)) FOR [IsInclude]
GO
ALTER TABLE [dbo].[ResourceComments] ADD  CONSTRAINT [DF_ResourceComments_CommentDate]  DEFAULT (getdate()) FOR [CommentDate]
GO
ALTER TABLE [dbo].[ResourceComments] ADD  CONSTRAINT [DF_ResourceComments_ReportAbuseCount]  DEFAULT ((0)) FOR [ReportAbuseCount]
GO
ALTER TABLE [dbo].[ResourceComments] ADD  CONSTRAINT [DF_ResourceComments_IsHidden]  DEFAULT ((0)) FOR [IsHidden]
GO
ALTER TABLE [dbo].[ResourceMaster] ADD  CONSTRAINT [DF_ResourceMaster_IsDraft]  DEFAULT ((0)) FOR [IsDraft]
GO
ALTER TABLE [dbo].[ResourceMaster] ADD  CONSTRAINT [DF_ResourceMaster_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[ResourceMaster] ADD  CONSTRAINT [DF_ResourceMaster_IsApproved]  DEFAULT ((0)) FOR [IsApproved]
GO
ALTER TABLE [dbo].[ResourceMaster] ADD  CONSTRAINT [DF_ResourceMaster_Rating]  DEFAULT ((0)) FOR [Rating]
GO
ALTER TABLE [dbo].[ResourceMaster] ADD  CONSTRAINT [DF_ResourceMaster_AlignmentRating]  DEFAULT ((0)) FOR [AlignmentRating]
GO
ALTER TABLE [dbo].[ResourceMaster] ADD  CONSTRAINT [DF_ResourceMaster_ReportAbuseCount]  DEFAULT ((0)) FOR [ReportAbuseCount]
GO
ALTER TABLE [dbo].[ResourceMaster] ADD  DEFAULT ((0)) FOR [CommunityBadge]
GO
ALTER TABLE [dbo].[ResourceMaster] ADD  DEFAULT ((0)) FOR [MoEBadge]
GO
ALTER TABLE [dbo].[ResourceMaster] ADD  DEFAULT ((0)) FOR [IsApprovedSensory]
GO
ALTER TABLE [dbo].[ResourceRating] ADD  CONSTRAINT [DF_ResourceRating_Rating]  DEFAULT ((0)) FOR [Rating]
GO
ALTER TABLE [dbo].[ResourceRating] ADD  CONSTRAINT [DF_ResourceRating_RatedOn]  DEFAULT (getdate()) FOR [RatedOn]
GO
ALTER TABLE [dbo].[ResourceURLReferences] ADD  CONSTRAINT [DF_ResourceURLReferences_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[SensoryCheckMaster] ADD  DEFAULT ((1)) FOR [IsCurrent]
GO
ALTER TABLE [dbo].[SocialMediaMaster] ADD  CONSTRAINT [DF_SocialMediaMaster_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[StateMaster] ADD  CONSTRAINT [DF_StateMaster_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[StateMaster] ADD  CONSTRAINT [DF_StateMaster_UpdatedOn]  DEFAULT (getdate()) FOR [UpdatedOn]
GO
ALTER TABLE [dbo].[StateMaster] ADD  CONSTRAINT [DF_StateMaster_IsActive]  DEFAULT ((0)) FOR [IsActive]
GO
ALTER TABLE [dbo].[StreamMaster] ADD  CONSTRAINT [DF_StreamMaster_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[StreamMaster] ADD  CONSTRAINT [DF_StreamMaster_UpdatedOn]  DEFAULT (getdate()) FOR [UpdatedOn]
GO
ALTER TABLE [dbo].[StreamMaster] ADD  CONSTRAINT [DF_StreamMaster_Active]  DEFAULT ((0)) FOR [Active]
GO
ALTER TABLE [dbo].[SubCategoryMaster] ADD  CONSTRAINT [DF_SubCategoryMaster_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[SubCategoryMaster] ADD  CONSTRAINT [DF_SubCategoryMaster_UpdatedOn]  DEFAULT (getdate()) FOR [UpdatedOn]
GO
ALTER TABLE [dbo].[SubCategoryMaster] ADD  CONSTRAINT [DF_SubCategoryMaster_Active]  DEFAULT ((0)) FOR [Active]
GO
ALTER TABLE [dbo].[SubCategoryMaster] ADD  CONSTRAINT [DF_SubCategoryMaster_Status]  DEFAULT ((1)) FOR [Status]
GO
ALTER TABLE [dbo].[TitleMaster] ADD  CONSTRAINT [DF_TitleMaster_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[TitleMaster] ADD  CONSTRAINT [DF_TitleMaster_UpdatedOn]  DEFAULT (getdate()) FOR [UpdatedOn]
GO
ALTER TABLE [dbo].[TitleMaster] ADD  CONSTRAINT [DF_TitleMaster_IsActive]  DEFAULT ((0)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Titles] ADD  CONSTRAINT [DF_Titles_Active]  DEFAULT ((1)) FOR [Active]
GO
ALTER TABLE [dbo].[UserCertification] ADD  CONSTRAINT [DF_UserCertification_Year]  DEFAULT (datepart(year,getdate())) FOR [Year]
GO
ALTER TABLE [dbo].[UserCertification] ADD  CONSTRAINT [DF_UserCertification_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[UserEducation] ADD  CONSTRAINT [DF_UserEducation_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[UserExperiences] ADD  CONSTRAINT [DF_UserExperiences_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[UserLanguages] ADD  CONSTRAINT [DF_Table1_Speak]  DEFAULT ((0)) FOR [IsSpeak]
GO
ALTER TABLE [dbo].[UserLanguages] ADD  CONSTRAINT [DF_Table1_Read]  DEFAULT ((0)) FOR [IsRead]
GO
ALTER TABLE [dbo].[UserLanguages] ADD  CONSTRAINT [DF_Table1_Write]  DEFAULT ((0)) FOR [IsWrite]
GO
ALTER TABLE [dbo].[UserLanguages] ADD  CONSTRAINT [DF_UserLanguages_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[UserMaster] ADD  CONSTRAINT [DF_UserMaster_Gender]  DEFAULT ((0)) FOR [Gender]
GO
ALTER TABLE [dbo].[UserMaster] ADD  CONSTRAINT [DF_AdminUser_ApprovalStatus]  DEFAULT ((0)) FOR [ApprovalStatus]
GO
ALTER TABLE [dbo].[UserMaster] ADD  CONSTRAINT [DF_AdminUser_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[UserMaster] ADD  CONSTRAINT [DF_AdminUser_UpdatedOn]  DEFAULT (getdate()) FOR [UpdatedOn]
GO
ALTER TABLE [dbo].[UserMaster] ADD  CONSTRAINT [DF_AdminUser_Active]  DEFAULT ((0)) FOR [Active]
GO
ALTER TABLE [dbo].[UserMaster] ADD  CONSTRAINT [DF_UserMaster_IsContributor]  DEFAULT ((0)) FOR [IsContributor]
GO
ALTER TABLE [dbo].[UserMaster] ADD  CONSTRAINT [DF_UserMaster_IsAdmin]  DEFAULT ((0)) FOR [IsAdmin]
GO
ALTER TABLE [dbo].[UserMaster] ADD  CONSTRAINT [D_usermaster_IsEmailNotification]  DEFAULT ((1)) FOR [IsEmailNotification]
GO
ALTER TABLE [dbo].[UserMaster] ADD  DEFAULT ('Blue') FOR [Theme]
GO
ALTER TABLE [dbo].[UserSocialMedia] ADD  CONSTRAINT [DF_UserSocialMedia_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[WhiteListingURLs] ADD  CONSTRAINT [DF_URLWhiteListing_IsApproved]  DEFAULT ((0)) FOR [IsApproved]
GO
ALTER TABLE [dbo].[WhiteListingURLs] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Announcements]  WITH CHECK ADD  CONSTRAINT [FK_Announcements_UserMaster] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[UserMaster] ([Id])
GO
ALTER TABLE [dbo].[Announcements] CHECK CONSTRAINT [FK_Announcements_UserMaster]
GO
ALTER TABLE [dbo].[Announcements]  WITH CHECK ADD  CONSTRAINT [FK_Announcements_UserMaster1] FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[UserMaster] ([Id])
GO
ALTER TABLE [dbo].[Announcements] CHECK CONSTRAINT [FK_Announcements_UserMaster1]
GO
ALTER TABLE [dbo].[AnswerOptions]  WITH CHECK ADD  CONSTRAINT [FK_AnswerOptions_Questions] FOREIGN KEY([QuestionId])
REFERENCES [dbo].[Questions] ([Id])
GO
ALTER TABLE [dbo].[AnswerOptions] CHECK CONSTRAINT [FK_AnswerOptions_Questions]
GO
ALTER TABLE [dbo].[CategoryMaster]  WITH CHECK ADD  CONSTRAINT [FK_CategoryMaster_UserMaster] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[UserMaster] ([Id])
GO
ALTER TABLE [dbo].[CategoryMaster] CHECK CONSTRAINT [FK_CategoryMaster_UserMaster]
GO
ALTER TABLE [dbo].[CategoryMaster]  WITH CHECK ADD  CONSTRAINT [FK_CategoryMaster_UserMaster1] FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[UserMaster] ([Id])
GO
ALTER TABLE [dbo].[CategoryMaster] CHECK CONSTRAINT [FK_CategoryMaster_UserMaster1]
GO
ALTER TABLE [dbo].[CommunityCheckMaster]  WITH CHECK ADD  CONSTRAINT [fk_communitycheckuser] FOREIGN KEY([UserId])
REFERENCES [dbo].[UserMaster] ([Id])
GO
ALTER TABLE [dbo].[CommunityCheckMaster] CHECK CONSTRAINT [fk_communitycheckuser]
GO
ALTER TABLE [dbo].[ContactUs]  WITH CHECK ADD  CONSTRAINT [FK_ContactUs_UserMaster] FOREIGN KEY([RepliedBy])
REFERENCES [dbo].[UserMaster] ([Id])
GO
ALTER TABLE [dbo].[ContactUs] CHECK CONSTRAINT [FK_ContactUs_UserMaster]
GO
ALTER TABLE [dbo].[ContentApproval]  WITH CHECK ADD  CONSTRAINT [FK_ContentApproval_UserMaster] FOREIGN KEY([QrcId])
REFERENCES [dbo].[QRCMaster] ([Id])
GO
ALTER TABLE [dbo].[ContentApproval] CHECK CONSTRAINT [FK_ContentApproval_UserMaster]
GO
ALTER TABLE [dbo].[ContentApproval]  WITH CHECK ADD  CONSTRAINT [FK_ContentApproval_UserMaster1] FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[UserMaster] ([Id])
GO
ALTER TABLE [dbo].[ContentApproval] CHECK CONSTRAINT [FK_ContentApproval_UserMaster1]
GO
ALTER TABLE [dbo].[ContentApproval]  WITH CHECK ADD  CONSTRAINT [FK_ContentApproval_UserMaster2] FOREIGN KEY([AssignedTo])
REFERENCES [dbo].[UserMaster] ([Id])
GO
ALTER TABLE [dbo].[ContentApproval] CHECK CONSTRAINT [FK_ContentApproval_UserMaster2]
GO
ALTER TABLE [dbo].[ContentApproval]  WITH CHECK ADD  CONSTRAINT [FK_ContentApproval_UserMaster3] FOREIGN KEY([ApprovedBy])
REFERENCES [dbo].[UserMaster] ([Id])
GO
ALTER TABLE [dbo].[ContentApproval] CHECK CONSTRAINT [FK_ContentApproval_UserMaster3]
GO
ALTER TABLE [dbo].[ContentDownloadInfo]  WITH CHECK ADD  CONSTRAINT [FK_ContentDownloadInfo_UserMaster] FOREIGN KEY([DownloadedBy])
REFERENCES [dbo].[UserMaster] ([Id])
GO
ALTER TABLE [dbo].[ContentDownloadInfo] CHECK CONSTRAINT [FK_ContentDownloadInfo_UserMaster]
GO
ALTER TABLE [dbo].[ContentSharedInfo]  WITH CHECK ADD  CONSTRAINT [FK_ContentSharedInfo_UserMaster] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[UserMaster] ([Id])
GO
ALTER TABLE [dbo].[ContentSharedInfo] CHECK CONSTRAINT [FK_ContentSharedInfo_UserMaster]
GO
ALTER TABLE [dbo].[CopyrightMaster]  WITH CHECK ADD  CONSTRAINT [FK_CopyrightMaster_UserMaster] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[UserMaster] ([Id])
GO
ALTER TABLE [dbo].[CopyrightMaster] CHECK CONSTRAINT [FK_CopyrightMaster_UserMaster]
GO
ALTER TABLE [dbo].[CopyrightMaster]  WITH CHECK ADD  CONSTRAINT [FK_CopyrightMaster_UserMaster1] FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[UserMaster] ([Id])
GO
ALTER TABLE [dbo].[CopyrightMaster] CHECK CONSTRAINT [FK_CopyrightMaster_UserMaster1]
GO
ALTER TABLE [dbo].[CountryMaster]  WITH CHECK ADD  CONSTRAINT [FK_CountryMaster_UserMaster] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[UserMaster] ([Id])
GO
ALTER TABLE [dbo].[CountryMaster] CHECK CONSTRAINT [FK_CountryMaster_UserMaster]
GO
ALTER TABLE [dbo].[CountryMaster]  WITH CHECK ADD  CONSTRAINT [FK_CountryMaster_UserMaster1] FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[UserMaster] ([Id])
GO
ALTER TABLE [dbo].[CountryMaster] CHECK CONSTRAINT [FK_CountryMaster_UserMaster1]
GO
ALTER TABLE [dbo].[CourseApprovals]  WITH CHECK ADD  CONSTRAINT [FK_CourseApprovals_CourseMaster] FOREIGN KEY([CourseId])
REFERENCES [dbo].[CourseMaster] ([Id])
GO
ALTER TABLE [dbo].[CourseApprovals] CHECK CONSTRAINT [FK_CourseApprovals_CourseMaster]
GO
ALTER TABLE [dbo].[CourseApprovals]  WITH CHECK ADD  CONSTRAINT [FK_CourseApprovals_UserMaster] FOREIGN KEY([ApprovedBy])
REFERENCES [dbo].[UserMaster] ([Id])
GO
ALTER TABLE [dbo].[CourseApprovals] CHECK CONSTRAINT [FK_CourseApprovals_UserMaster]
GO
ALTER TABLE [dbo].[CourseAssociatedFiles]  WITH CHECK ADD  CONSTRAINT [FK_CourseAssociatedFiles_CourseMaster] FOREIGN KEY([CourseId])
REFERENCES [dbo].[CourseMaster] ([Id])
GO
ALTER TABLE [dbo].[CourseAssociatedFiles] CHECK CONSTRAINT [FK_CourseAssociatedFiles_CourseMaster]
GO
ALTER TABLE [dbo].[CourseComments]  WITH CHECK ADD  CONSTRAINT [FK_CourseComments_CourseMaster] FOREIGN KEY([CourseId])
REFERENCES [dbo].[CourseMaster] ([Id])
GO
ALTER TABLE [dbo].[CourseComments] CHECK CONSTRAINT [FK_CourseComments_CourseMaster]
GO
ALTER TABLE [dbo].[CourseComments]  WITH CHECK ADD  CONSTRAINT [FK_CourseComments_UserMaster] FOREIGN KEY([UserId])
REFERENCES [dbo].[UserMaster] ([Id])
GO
ALTER TABLE [dbo].[CourseComments] CHECK CONSTRAINT [FK_CourseComments_UserMaster]
GO
ALTER TABLE [dbo].[CourseEnrollment]  WITH CHECK ADD  CONSTRAINT [FK_CourseEnrollment_CourseEnrollment] FOREIGN KEY([CourseId])
REFERENCES [dbo].[CourseMaster] ([Id])
GO
ALTER TABLE [dbo].[CourseEnrollment] CHECK CONSTRAINT [FK_CourseEnrollment_CourseEnrollment]
GO
ALTER TABLE [dbo].[CourseEnrollment]  WITH CHECK ADD  CONSTRAINT [FK_CourseEnrollment_UserMaster] FOREIGN KEY([UserId])
REFERENCES [dbo].[UserMaster] ([Id])
GO
ALTER TABLE [dbo].[CourseEnrollment] CHECK CONSTRAINT [FK_CourseEnrollment_UserMaster]
GO
ALTER TABLE [dbo].[CourseMaster]  WITH CHECK ADD  CONSTRAINT [FK_CourseMaster_CategoryMaster] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[CategoryMaster] ([Id])
GO
ALTER TABLE [dbo].[CourseMaster] CHECK CONSTRAINT [FK_CourseMaster_CategoryMaster]
GO
ALTER TABLE [dbo].[CourseMaster]  WITH CHECK ADD  CONSTRAINT [FK_CourseMaster_UserMaster] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[UserMaster] ([Id])
GO
ALTER TABLE [dbo].[CourseMaster] CHECK CONSTRAINT [FK_CourseMaster_UserMaster]
GO
ALTER TABLE [dbo].[CourseResourceMapping]  WITH CHECK ADD  CONSTRAINT [FK_CourseResourceMapping_CourseResourceMapping] FOREIGN KEY([CourseId])
REFERENCES [dbo].[CourseMaster] ([Id])
GO
ALTER TABLE [dbo].[CourseResourceMapping] CHECK CONSTRAINT [FK_CourseResourceMapping_CourseResourceMapping]
GO
ALTER TABLE [dbo].[CourseResourceMapping]  WITH CHECK ADD  CONSTRAINT [FK_CourseResourceMapping_ResourceMaster] FOREIGN KEY([ResourcesId])
REFERENCES [dbo].[ResourceMaster] ([Id])
GO
ALTER TABLE [dbo].[CourseResourceMapping] CHECK CONSTRAINT [FK_CourseResourceMapping_ResourceMaster]
GO
ALTER TABLE [dbo].[CourseURLReferences]  WITH CHECK ADD  CONSTRAINT [FK_CourseURLReferences_CourseMaster] FOREIGN KEY([CourseId])
REFERENCES [dbo].[CourseMaster] ([Id])
GO
ALTER TABLE [dbo].[CourseURLReferences] CHECK CONSTRAINT [FK_CourseURLReferences_CourseMaster]
GO
ALTER TABLE [dbo].[CourseURLReferences]  WITH CHECK ADD  CONSTRAINT [FK_CourseURLReferences_WhiteListingURLs] FOREIGN KEY([URLReferenceId])
REFERENCES [dbo].[WhiteListingURLs] ([Id])
GO
ALTER TABLE [dbo].[CourseURLReferences] CHECK CONSTRAINT [FK_CourseURLReferences_WhiteListingURLs]
GO
ALTER TABLE [dbo].[DepartmentMaster]  WITH CHECK ADD  CONSTRAINT [FK_DepartmentMaster_UserMaster] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[UserMaster] ([Id])
GO
ALTER TABLE [dbo].[DepartmentMaster] CHECK CONSTRAINT [FK_DepartmentMaster_UserMaster]
GO
ALTER TABLE [dbo].[DepartmentMaster]  WITH CHECK ADD  CONSTRAINT [FK_DepartmentMaster_UserMaster1] FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[UserMaster] ([Id])
GO
ALTER TABLE [dbo].[DepartmentMaster] CHECK CONSTRAINT [FK_DepartmentMaster_UserMaster1]
GO
ALTER TABLE [dbo].[DesignationMaster]  WITH CHECK ADD  CONSTRAINT [FK_DesignationMaster_UserMaster] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[UserMaster] ([Id])
GO
ALTER TABLE [dbo].[DesignationMaster] CHECK CONSTRAINT [FK_DesignationMaster_UserMaster]
GO
ALTER TABLE [dbo].[DesignationMaster]  WITH CHECK ADD  CONSTRAINT [FK_DesignationMaster_UserMaster1] FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[UserMaster] ([Id])
GO
ALTER TABLE [dbo].[DesignationMaster] CHECK CONSTRAINT [FK_DesignationMaster_UserMaster1]
GO
ALTER TABLE [dbo].[EducationMaster]  WITH CHECK ADD  CONSTRAINT [FK_EducationMaster_UserMaster] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[UserMaster] ([Id])
GO
ALTER TABLE [dbo].[EducationMaster] CHECK CONSTRAINT [FK_EducationMaster_UserMaster]
GO
ALTER TABLE [dbo].[EducationMaster]  WITH CHECK ADD  CONSTRAINT [FK_EducationMaster_UserMaster1] FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[UserMaster] ([Id])
GO
ALTER TABLE [dbo].[EducationMaster] CHECK CONSTRAINT [FK_EducationMaster_UserMaster1]
GO
ALTER TABLE [dbo].[InstitutionMaster]  WITH CHECK ADD  CONSTRAINT [FK_InstitutionMaster_UserMaster] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[UserMaster] ([Id])
GO
ALTER TABLE [dbo].[InstitutionMaster] CHECK CONSTRAINT [FK_InstitutionMaster_UserMaster]
GO
ALTER TABLE [dbo].[InstitutionMaster]  WITH CHECK ADD  CONSTRAINT [FK_InstitutionMaster_UserMaster1] FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[UserMaster] ([Id])
GO
ALTER TABLE [dbo].[InstitutionMaster] CHECK CONSTRAINT [FK_InstitutionMaster_UserMaster1]
GO
ALTER TABLE [dbo].[LanguageMaster]  WITH CHECK ADD  CONSTRAINT [FK_LanguageMaster_UserMaster] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[UserMaster] ([Id])
GO
ALTER TABLE [dbo].[LanguageMaster] CHECK CONSTRAINT [FK_LanguageMaster_UserMaster]
GO
ALTER TABLE [dbo].[LanguageMaster]  WITH CHECK ADD  CONSTRAINT [FK_LanguageMaster_UserMaster1] FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[UserMaster] ([Id])
GO
ALTER TABLE [dbo].[LanguageMaster] CHECK CONSTRAINT [FK_LanguageMaster_UserMaster1]
GO
ALTER TABLE [dbo].[LogAction]  WITH CHECK ADD  CONSTRAINT [FK_LogAction_LogActionMaster] FOREIGN KEY([ActionId])
REFERENCES [dbo].[LogActionMaster] ([Id])
GO
ALTER TABLE [dbo].[LogAction] CHECK CONSTRAINT [FK_LogAction_LogActionMaster]
GO
ALTER TABLE [dbo].[LogAction]  WITH CHECK ADD  CONSTRAINT [FK_LogAction_LogModuleMaster] FOREIGN KEY([LogModuleId])
REFERENCES [dbo].[LogModuleMaster] ([Id])
GO
ALTER TABLE [dbo].[LogAction] CHECK CONSTRAINT [FK_LogAction_LogModuleMaster]
GO
ALTER TABLE [dbo].[LogAction]  WITH CHECK ADD  CONSTRAINT [FK_LogAction_UserMaster] FOREIGN KEY([UserId])
REFERENCES [dbo].[UserMaster] ([Id])
GO
ALTER TABLE [dbo].[LogAction] CHECK CONSTRAINT [FK_LogAction_UserMaster]
GO
ALTER TABLE [dbo].[LogError]  WITH CHECK ADD  CONSTRAINT [FK_LogError_LogModuleMaster] FOREIGN KEY([LogModuleId])
REFERENCES [dbo].[LogModuleMaster] ([Id])
GO
ALTER TABLE [dbo].[LogError] CHECK CONSTRAINT [FK_LogError_LogModuleMaster]
GO
ALTER TABLE [dbo].[LogError]  WITH CHECK ADD  CONSTRAINT [FK_LogError_UserMaster] FOREIGN KEY([UserId])
REFERENCES [dbo].[UserMaster] ([Id])
GO
ALTER TABLE [dbo].[LogError] CHECK CONSTRAINT [FK_LogError_UserMaster]
GO
ALTER TABLE [dbo].[MaterialTypeMaster]  WITH CHECK ADD  CONSTRAINT [FK_MaterialTypeMaster_UserMaster] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[UserMaster] ([Id])
GO
ALTER TABLE [dbo].[MaterialTypeMaster] CHECK CONSTRAINT [FK_MaterialTypeMaster_UserMaster]
GO
ALTER TABLE [dbo].[MaterialTypeMaster]  WITH CHECK ADD  CONSTRAINT [FK_MaterialTypeMaster_UserMaster1] FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[UserMaster] ([Id])
GO
ALTER TABLE [dbo].[MaterialTypeMaster] CHECK CONSTRAINT [FK_MaterialTypeMaster_UserMaster1]
GO
ALTER TABLE [dbo].[MoECheckMaster]  WITH CHECK ADD  CONSTRAINT [fk_moecheckuser] FOREIGN KEY([UserId])
REFERENCES [dbo].[UserMaster] ([Id])
GO
ALTER TABLE [dbo].[MoECheckMaster] CHECK CONSTRAINT [fk_moecheckuser]
GO
ALTER TABLE [dbo].[Notifications]  WITH CHECK ADD  CONSTRAINT [FK_Notifications_MessageType] FOREIGN KEY([MessageTypeId])
REFERENCES [dbo].[MessageType] ([Id])
GO
ALTER TABLE [dbo].[Notifications] CHECK CONSTRAINT [FK_Notifications_MessageType]
GO
ALTER TABLE [dbo].[Notifications]  WITH CHECK ADD  CONSTRAINT [FK_Notifications_UserMaster] FOREIGN KEY([UserId])
REFERENCES [dbo].[UserMaster] ([Id])
GO
ALTER TABLE [dbo].[Notifications] CHECK CONSTRAINT [FK_Notifications_UserMaster]
GO
ALTER TABLE [dbo].[ProfessionMaster]  WITH CHECK ADD  CONSTRAINT [FK_ProfessionMaster_UserMaster] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[UserMaster] ([Id])
GO
ALTER TABLE [dbo].[ProfessionMaster] CHECK CONSTRAINT [FK_ProfessionMaster_UserMaster]
GO
ALTER TABLE [dbo].[ProfessionMaster]  WITH CHECK ADD  CONSTRAINT [FK_ProfessionMaster_UserMaster1] FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[UserMaster] ([Id])
GO
ALTER TABLE [dbo].[ProfessionMaster] CHECK CONSTRAINT [FK_ProfessionMaster_UserMaster1]
GO
ALTER TABLE [dbo].[QRCCategory]  WITH CHECK ADD  CONSTRAINT [FK_QRCCategory_CategoryMaster] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[CategoryMaster] ([Id])
GO
ALTER TABLE [dbo].[QRCCategory] CHECK CONSTRAINT [FK_QRCCategory_CategoryMaster]
GO
ALTER TABLE [dbo].[QRCCategory]  WITH CHECK ADD  CONSTRAINT [FK_QRCCategory_QRCMaster] FOREIGN KEY([QRCId])
REFERENCES [dbo].[QRCMaster] ([Id])
GO
ALTER TABLE [dbo].[QRCCategory] CHECK CONSTRAINT [FK_QRCCategory_QRCMaster]
GO
ALTER TABLE [dbo].[QRCMaster]  WITH CHECK ADD  CONSTRAINT [FK_QRCMaster_UserMaster] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[UserMaster] ([Id])
GO
ALTER TABLE [dbo].[QRCMaster] CHECK CONSTRAINT [FK_QRCMaster_UserMaster]
GO
ALTER TABLE [dbo].[QRCMaster]  WITH CHECK ADD  CONSTRAINT [FK_QRCMaster_UserMaster1] FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[UserMaster] ([Id])
GO
ALTER TABLE [dbo].[QRCMaster] CHECK CONSTRAINT [FK_QRCMaster_UserMaster1]
GO
ALTER TABLE [dbo].[QRCMasterCategory]  WITH CHECK ADD  CONSTRAINT [FK_QRCMasterCategory_QRCMaster] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[QRCMaster] ([Id])
GO
ALTER TABLE [dbo].[QRCMasterCategory] CHECK CONSTRAINT [FK_QRCMasterCategory_QRCMaster]
GO
ALTER TABLE [dbo].[QRCUserMapping]  WITH CHECK ADD  CONSTRAINT [FK_QRCUserMapping_CategoryMaster] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[CategoryMaster] ([Id])
GO
ALTER TABLE [dbo].[QRCUserMapping] CHECK CONSTRAINT [FK_QRCUserMapping_CategoryMaster]
GO
ALTER TABLE [dbo].[QRCUserMapping]  WITH CHECK ADD  CONSTRAINT [FK_QRCUserMapping_QRCMaster] FOREIGN KEY([QRCId])
REFERENCES [dbo].[QRCMaster] ([Id])
GO
ALTER TABLE [dbo].[QRCUserMapping] CHECK CONSTRAINT [FK_QRCUserMapping_QRCMaster]
GO
ALTER TABLE [dbo].[QRCUserMapping]  WITH CHECK ADD  CONSTRAINT [FK_QRCUserMapping_UserMaster] FOREIGN KEY([UserId])
REFERENCES [dbo].[UserMaster] ([Id])
GO
ALTER TABLE [dbo].[QRCUserMapping] CHECK CONSTRAINT [FK_QRCUserMapping_UserMaster]
GO
ALTER TABLE [dbo].[QRCUserMapping]  WITH CHECK ADD  CONSTRAINT [FK_QRCUserMapping_UserMaster1] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[UserMaster] ([Id])
GO
ALTER TABLE [dbo].[QRCUserMapping] CHECK CONSTRAINT [FK_QRCUserMapping_UserMaster1]
GO
ALTER TABLE [dbo].[Questions]  WITH CHECK ADD  CONSTRAINT [FK_Questions_Tests] FOREIGN KEY([TestId])
REFERENCES [dbo].[Tests] ([Id])
GO
ALTER TABLE [dbo].[Questions] CHECK CONSTRAINT [FK_Questions_Tests]
GO
ALTER TABLE [dbo].[ResourceApprovals]  WITH CHECK ADD  CONSTRAINT [FK_ResourceApprovals_ResourceMaster] FOREIGN KEY([ResourceId])
REFERENCES [dbo].[ResourceMaster] ([Id])
GO
ALTER TABLE [dbo].[ResourceApprovals] CHECK CONSTRAINT [FK_ResourceApprovals_ResourceMaster]
GO
ALTER TABLE [dbo].[ResourceApprovals]  WITH CHECK ADD  CONSTRAINT [FK_ResourceApprovals_UserMaster] FOREIGN KEY([ApprovedBy])
REFERENCES [dbo].[UserMaster] ([Id])
GO
ALTER TABLE [dbo].[ResourceApprovals] CHECK CONSTRAINT [FK_ResourceApprovals_UserMaster]
GO
ALTER TABLE [dbo].[ResourceAssociatedFiles]  WITH CHECK ADD  CONSTRAINT [FK_ResourceAssociatedFiles_ResourceMaster] FOREIGN KEY([ResourceId])
REFERENCES [dbo].[ResourceMaster] ([Id])
GO
ALTER TABLE [dbo].[ResourceAssociatedFiles] CHECK CONSTRAINT [FK_ResourceAssociatedFiles_ResourceMaster]
GO
ALTER TABLE [dbo].[ResourceComments]  WITH CHECK ADD  CONSTRAINT [FK_ResourceComments_ResourceMaster] FOREIGN KEY([ResourceId])
REFERENCES [dbo].[ResourceMaster] ([Id])
GO
ALTER TABLE [dbo].[ResourceComments] CHECK CONSTRAINT [FK_ResourceComments_ResourceMaster]
GO
ALTER TABLE [dbo].[ResourceComments]  WITH CHECK ADD  CONSTRAINT [FK_ResourceComments_UserMaster] FOREIGN KEY([UserId])
REFERENCES [dbo].[UserMaster] ([Id])
GO
ALTER TABLE [dbo].[ResourceComments] CHECK CONSTRAINT [FK_ResourceComments_UserMaster]
GO
ALTER TABLE [dbo].[ResourceMaster]  WITH CHECK ADD  CONSTRAINT [FK_ResourceMaster_CategoryMaster] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[CategoryMaster] ([Id])
GO
ALTER TABLE [dbo].[ResourceMaster] CHECK CONSTRAINT [FK_ResourceMaster_CategoryMaster]
GO
ALTER TABLE [dbo].[ResourceMaster]  WITH CHECK ADD  CONSTRAINT [FK_ResourceMaster_CopyrightMaster] FOREIGN KEY([CopyRightId])
REFERENCES [dbo].[CopyrightMaster] ([Id])
GO
ALTER TABLE [dbo].[ResourceMaster] CHECK CONSTRAINT [FK_ResourceMaster_CopyrightMaster]
GO
ALTER TABLE [dbo].[ResourceMaster]  WITH CHECK ADD  CONSTRAINT [FK_ResourceMaster_lu_Educational_Standard] FOREIGN KEY([EducationalStandardId])
REFERENCES [dbo].[lu_Educational_Standard] ([Id])
GO
ALTER TABLE [dbo].[ResourceMaster] CHECK CONSTRAINT [FK_ResourceMaster_lu_Educational_Standard]
GO
ALTER TABLE [dbo].[ResourceMaster]  WITH CHECK ADD  CONSTRAINT [FK_ResourceMaster_lu_Educational_Use] FOREIGN KEY([EducationalUseId])
REFERENCES [dbo].[lu_Educational_Use] ([Id])
GO
ALTER TABLE [dbo].[ResourceMaster] CHECK CONSTRAINT [FK_ResourceMaster_lu_Educational_Use]
GO
ALTER TABLE [dbo].[ResourceMaster]  WITH CHECK ADD  CONSTRAINT [FK_ResourceMaster_lu_Level] FOREIGN KEY([LevelId])
REFERENCES [dbo].[lu_Level] ([Id])
GO
ALTER TABLE [dbo].[ResourceMaster] CHECK CONSTRAINT [FK_ResourceMaster_lu_Level]
GO
ALTER TABLE [dbo].[ResourceMaster]  WITH CHECK ADD  CONSTRAINT [FK_ResourceMaster_MaterialTypeMaster] FOREIGN KEY([MaterialTypeId])
REFERENCES [dbo].[MaterialTypeMaster] ([Id])
GO
ALTER TABLE [dbo].[ResourceMaster] CHECK CONSTRAINT [FK_ResourceMaster_MaterialTypeMaster]
GO
ALTER TABLE [dbo].[ResourceMaster]  WITH CHECK ADD  CONSTRAINT [FK_ResourceMaster_UserMaster] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[UserMaster] ([Id])
GO
ALTER TABLE [dbo].[ResourceMaster] CHECK CONSTRAINT [FK_ResourceMaster_UserMaster]
GO
ALTER TABLE [dbo].[ResourceURLReferences]  WITH CHECK ADD  CONSTRAINT [FK_ResourceURLReferences_ResourceMaster] FOREIGN KEY([ResourceId])
REFERENCES [dbo].[ResourceMaster] ([Id])
GO
ALTER TABLE [dbo].[ResourceURLReferences] CHECK CONSTRAINT [FK_ResourceURLReferences_ResourceMaster]
GO
ALTER TABLE [dbo].[ResourceURLReferences]  WITH CHECK ADD  CONSTRAINT [FK_ResourceURLReferences_WhiteListingURLs] FOREIGN KEY([URLReferenceId])
REFERENCES [dbo].[WhiteListingURLs] ([Id])
GO
ALTER TABLE [dbo].[ResourceURLReferences] CHECK CONSTRAINT [FK_ResourceURLReferences_WhiteListingURLs]
GO
ALTER TABLE [dbo].[SectionResource]  WITH CHECK ADD  CONSTRAINT [FK_SectionResource_CourseSections] FOREIGN KEY([SectionId])
REFERENCES [dbo].[CourseSections] ([Id])
GO
ALTER TABLE [dbo].[SectionResource] CHECK CONSTRAINT [FK_SectionResource_CourseSections]
GO
ALTER TABLE [dbo].[SectionResource]  WITH CHECK ADD  CONSTRAINT [FK_SectionResource_SectionResource] FOREIGN KEY([ResourceId])
REFERENCES [dbo].[ResourceMaster] ([Id])
GO
ALTER TABLE [dbo].[SectionResource] CHECK CONSTRAINT [FK_SectionResource_SectionResource]
GO
ALTER TABLE [dbo].[SensoryCheckMaster]  WITH CHECK ADD  CONSTRAINT [fk_sensorycheckuser] FOREIGN KEY([UserId])
REFERENCES [dbo].[UserMaster] ([Id])
GO
ALTER TABLE [dbo].[SensoryCheckMaster] CHECK CONSTRAINT [fk_sensorycheckuser]
GO
ALTER TABLE [dbo].[SocialMediaMaster]  WITH CHECK ADD  CONSTRAINT [FK_SocialMediaMaster_UserMaster] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[UserMaster] ([Id])
GO
ALTER TABLE [dbo].[SocialMediaMaster] CHECK CONSTRAINT [FK_SocialMediaMaster_UserMaster]
GO
ALTER TABLE [dbo].[SocialMediaMaster]  WITH CHECK ADD  CONSTRAINT [FK_SocialMediaMaster_UserMaster1] FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[UserMaster] ([Id])
GO
ALTER TABLE [dbo].[SocialMediaMaster] CHECK CONSTRAINT [FK_SocialMediaMaster_UserMaster1]
GO
ALTER TABLE [dbo].[StateMaster]  WITH CHECK ADD  CONSTRAINT [FK_StateMaster_CountryMaster] FOREIGN KEY([CountryId])
REFERENCES [dbo].[CountryMaster] ([Id])
GO
ALTER TABLE [dbo].[StateMaster] CHECK CONSTRAINT [FK_StateMaster_CountryMaster]
GO
ALTER TABLE [dbo].[StateMaster]  WITH CHECK ADD  CONSTRAINT [FK_StateMaster_UserMaster] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[UserMaster] ([Id])
GO
ALTER TABLE [dbo].[StateMaster] CHECK CONSTRAINT [FK_StateMaster_UserMaster]
GO
ALTER TABLE [dbo].[StateMaster]  WITH CHECK ADD  CONSTRAINT [FK_StateMaster_UserMaster1] FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[UserMaster] ([Id])
GO
ALTER TABLE [dbo].[StateMaster] CHECK CONSTRAINT [FK_StateMaster_UserMaster1]
GO
ALTER TABLE [dbo].[StreamMaster]  WITH CHECK ADD  CONSTRAINT [FK_StreamMaster_UserMaster] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[UserMaster] ([Id])
GO
ALTER TABLE [dbo].[StreamMaster] CHECK CONSTRAINT [FK_StreamMaster_UserMaster]
GO
ALTER TABLE [dbo].[StreamMaster]  WITH CHECK ADD  CONSTRAINT [FK_StreamMaster_UserMaster1] FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[UserMaster] ([Id])
GO
ALTER TABLE [dbo].[StreamMaster] CHECK CONSTRAINT [FK_StreamMaster_UserMaster1]
GO
ALTER TABLE [dbo].[SubCategoryMaster]  WITH CHECK ADD  CONSTRAINT [FK_SubCategoryMaster_CategoryMaster] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[CategoryMaster] ([Id])
GO
ALTER TABLE [dbo].[SubCategoryMaster] CHECK CONSTRAINT [FK_SubCategoryMaster_CategoryMaster]
GO
ALTER TABLE [dbo].[SubCategoryMaster]  WITH CHECK ADD  CONSTRAINT [FK_SubCategoryMaster_UserMaster] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[UserMaster] ([Id])
GO
ALTER TABLE [dbo].[SubCategoryMaster] CHECK CONSTRAINT [FK_SubCategoryMaster_UserMaster]
GO
ALTER TABLE [dbo].[SubCategoryMaster]  WITH CHECK ADD  CONSTRAINT [FK_SubCategoryMaster_UserMaster1] FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[UserMaster] ([Id])
GO
ALTER TABLE [dbo].[SubCategoryMaster] CHECK CONSTRAINT [FK_SubCategoryMaster_UserMaster1]
GO
ALTER TABLE [dbo].[Tests]  WITH CHECK ADD  CONSTRAINT [FK_Tests_Tests] FOREIGN KEY([CourseId])
REFERENCES [dbo].[CourseMaster] ([Id])
GO
ALTER TABLE [dbo].[Tests] CHECK CONSTRAINT [FK_Tests_Tests]
GO
ALTER TABLE [dbo].[Tests]  WITH CHECK ADD  CONSTRAINT [FK_Tests_UserMaster] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[UserMaster] ([Id])
GO
ALTER TABLE [dbo].[Tests] CHECK CONSTRAINT [FK_Tests_UserMaster]
GO
ALTER TABLE [dbo].[Tests]  WITH CHECK ADD  CONSTRAINT [FK_Tests_UserMaster1] FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[UserMaster] ([Id])
GO
ALTER TABLE [dbo].[Tests] CHECK CONSTRAINT [FK_Tests_UserMaster1]
GO
ALTER TABLE [dbo].[TitleMaster]  WITH CHECK ADD  CONSTRAINT [FK_TitleMaster_UserMaster] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[UserMaster] ([Id])
GO
ALTER TABLE [dbo].[TitleMaster] CHECK CONSTRAINT [FK_TitleMaster_UserMaster]
GO
ALTER TABLE [dbo].[TitleMaster]  WITH CHECK ADD  CONSTRAINT [FK_TitleMaster_UserMaster1] FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[UserMaster] ([Id])
GO
ALTER TABLE [dbo].[TitleMaster] CHECK CONSTRAINT [FK_TitleMaster_UserMaster1]
GO
ALTER TABLE [dbo].[UserBookmarks]  WITH CHECK ADD  CONSTRAINT [FK_UserMaster_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[UserMaster] ([Id])
GO
ALTER TABLE [dbo].[UserBookmarks] CHECK CONSTRAINT [FK_UserMaster_UserId]
GO
ALTER TABLE [dbo].[UserCertification]  WITH CHECK ADD  CONSTRAINT [FK_UserCertification_UserMaster] FOREIGN KEY([UserId])
REFERENCES [dbo].[UserMaster] ([Id])
GO
ALTER TABLE [dbo].[UserCertification] CHECK CONSTRAINT [FK_UserCertification_UserMaster]
GO
ALTER TABLE [dbo].[UserCourseTests]  WITH CHECK ADD  CONSTRAINT [FK_UserCourseTests_AnswerOptions1] FOREIGN KEY([AnswerId])
REFERENCES [dbo].[AnswerOptions] ([Id])
GO
ALTER TABLE [dbo].[UserCourseTests] CHECK CONSTRAINT [FK_UserCourseTests_AnswerOptions1]
GO
ALTER TABLE [dbo].[UserCourseTests]  WITH CHECK ADD  CONSTRAINT [FK_UserCourseTests_CourseMaster] FOREIGN KEY([CourseId])
REFERENCES [dbo].[CourseMaster] ([Id])
GO
ALTER TABLE [dbo].[UserCourseTests] CHECK CONSTRAINT [FK_UserCourseTests_CourseMaster]
GO
ALTER TABLE [dbo].[UserCourseTests]  WITH CHECK ADD  CONSTRAINT [FK_UserCourseTests_Questions1] FOREIGN KEY([QuestionId])
REFERENCES [dbo].[Questions] ([Id])
GO
ALTER TABLE [dbo].[UserCourseTests] CHECK CONSTRAINT [FK_UserCourseTests_Questions1]
GO
ALTER TABLE [dbo].[UserCourseTests]  WITH CHECK ADD  CONSTRAINT [FK_UserCourseTests_UserMaster1] FOREIGN KEY([UserId])
REFERENCES [dbo].[UserMaster] ([Id])
GO
ALTER TABLE [dbo].[UserCourseTests] CHECK CONSTRAINT [FK_UserCourseTests_UserMaster1]
GO
ALTER TABLE [dbo].[UserEducation]  WITH CHECK ADD  CONSTRAINT [FK_UserEducation_UserMaster] FOREIGN KEY([UserId])
REFERENCES [dbo].[UserMaster] ([Id])
GO
ALTER TABLE [dbo].[UserEducation] CHECK CONSTRAINT [FK_UserEducation_UserMaster]
GO
ALTER TABLE [dbo].[UserExperiences]  WITH CHECK ADD  CONSTRAINT [FK_UserExperiences_UserMaster] FOREIGN KEY([UserId])
REFERENCES [dbo].[UserMaster] ([Id])
GO
ALTER TABLE [dbo].[UserExperiences] CHECK CONSTRAINT [FK_UserExperiences_UserMaster]
GO
ALTER TABLE [dbo].[UserLanguages]  WITH CHECK ADD  CONSTRAINT [FK_UserLanguages_LanguageMaster] FOREIGN KEY([LanguageId])
REFERENCES [dbo].[LanguageMaster] ([Id])
GO
ALTER TABLE [dbo].[UserLanguages] CHECK CONSTRAINT [FK_UserLanguages_LanguageMaster]
GO
ALTER TABLE [dbo].[UserLanguages]  WITH CHECK ADD  CONSTRAINT [FK_UserLanguages_UserMaster] FOREIGN KEY([UserId])
REFERENCES [dbo].[UserMaster] ([Id])
GO
ALTER TABLE [dbo].[UserLanguages] CHECK CONSTRAINT [FK_UserLanguages_UserMaster]
GO
ALTER TABLE [dbo].[UserMaster]  WITH CHECK ADD  CONSTRAINT [FK_UserMaster_TitleMaster] FOREIGN KEY([TitleId])
REFERENCES [dbo].[TitleMaster] ([Id])
GO
ALTER TABLE [dbo].[UserMaster] CHECK CONSTRAINT [FK_UserMaster_TitleMaster]
GO
ALTER TABLE [dbo].[UserSocialMedia]  WITH CHECK ADD  CONSTRAINT [FK_UserSocialMedia_SocialMediaMaster] FOREIGN KEY([SocialMediaId])
REFERENCES [dbo].[SocialMediaMaster] ([Id])
GO
ALTER TABLE [dbo].[UserSocialMedia] CHECK CONSTRAINT [FK_UserSocialMedia_SocialMediaMaster]
GO
ALTER TABLE [dbo].[UserSocialMedia]  WITH CHECK ADD  CONSTRAINT [FK_UserSocialMedia_UserMaster] FOREIGN KEY([UserId])
REFERENCES [dbo].[UserMaster] ([Id])
GO
ALTER TABLE [dbo].[UserSocialMedia] CHECK CONSTRAINT [FK_UserSocialMedia_UserMaster]
GO
ALTER TABLE [dbo].[Visiters]  WITH CHECK ADD  CONSTRAINT [FK_Visiters_UserMaster] FOREIGN KEY([UserId])
REFERENCES [dbo].[UserMaster] ([Id])
GO
ALTER TABLE [dbo].[Visiters] CHECK CONSTRAINT [FK_Visiters_UserMaster]
GO
ALTER TABLE [dbo].[WebPageContent]  WITH CHECK ADD  CONSTRAINT [FK_WebPageContent_WebContentPages] FOREIGN KEY([PageID])
REFERENCES [dbo].[WebContentPages] ([Id])
GO
ALTER TABLE [dbo].[WebPageContent] CHECK CONSTRAINT [FK_WebPageContent_WebContentPages]
GO
ALTER TABLE [dbo].[WhiteListingURLs]  WITH CHECK ADD  CONSTRAINT [FK_URLWhiteListing_UserMaster] FOREIGN KEY([VerifiedBy])
REFERENCES [dbo].[UserMaster] ([Id])
GO
ALTER TABLE [dbo].[WhiteListingURLs] CHECK CONSTRAINT [FK_URLWhiteListing_UserMaster]
GO
ALTER TABLE [dbo].[WhiteListingURLs]  WITH CHECK ADD  CONSTRAINT [FK_WhiteListingURLs_UserMaster] FOREIGN KEY([RequestedBy])
REFERENCES [dbo].[UserMaster] ([Id])
GO
ALTER TABLE [dbo].[WhiteListingURLs] CHECK CONSTRAINT [FK_WhiteListingURLs_UserMaster]
GO
/****** Object:  StoredProcedure [dbo].[ApproveCourse]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ApproveCourse @CourseId=3,@CreatedBy=36
CREATE PROCEDURE [dbo].[ApproveCourse] 
	@CourseId Numeric(18,0),
	@CreatedBy INT
AS
BEGIN


CREATE table #tempData
(
UserId INT
)

INSERT INTO #tempData
SELECT UserID from QRCUserMapping WHERE QRCId in (
SELECT QRCId FROM QRCCategory WHERE CategoryID IN(SELECT CategoryID FROM CourseMaster WHERE ID = @CourseId))


Insert into ContentApproval(
ContentId,
ContentType,--1 means course
[Status],
CreatedBy,
CreatedOn,
AssignedTo,
AssignedDate
)
SELECT @CourseId,1,NULL,@CreatedBy,GETDATE(),UserId,GETDATE() FROM #tempData

--VALUES(@CourseId,1,NULL,@CreatedBy,GETDATE(),@ApproveBy,GETDATE())

RETURN 115 -- APPROVED
END
--ELSE
--RETURN  116-- ALREADY APPROVED
--END


GO
/****** Object:  StoredProcedure [dbo].[ApproveResource]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ApproveResource @resourceId=33,@CreatedBy =36
CREATE PROCEDURE [dbo].[ApproveResource] 
	@ResourceId Numeric(18,0),
	@CreatedBy INT
AS
BEGIN

CREATE table #tempData
(
UserId INT
)

INSERT INTO #tempData
SELECT UserID from QRCUserMapping WHERE QRCId in (
SELECT QRCId FROM QRCCategory WHERE CategoryID IN(SELECT CategoryID FROM ResourceMaster WHERE ID = @ResourceId))


Insert into ContentApproval(
ContentId,
ContentType,--2 means reasource
[Status],
CreatedBy,
CreatedOn,
AssignedTo,
AssignedDate
)
SELECT @ResourceId,2,NULL,@CreatedBy,GETDATE(),UserId,GETDATE() FROM #tempData

RETURN 115 -- APPROVED
END


GO
/****** Object:  StoredProcedure [dbo].[CommentOnCourse]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CommentOnCourse]      
     @CourseId INT,  
     @Comments nvarchar(2000),  
     @CommentedBy int      
AS  
BEGIN  
Declare @return INT  
  
  INSERT INTO CourseComments(CourseId,Comments,UserId, CommentDate,IsHidden,UpdateDate)  
  VALUES (@CourseId,@Comments,@CommentedBy, GETDATE(),0,GETDATE())   
  
  IF @@IDENTITY>0  
  SET @return =100 -- creation success  
  
  ELSE  
  SET @return =107  
    
  
  
  RETURN @return  
END  
  
GO
/****** Object:  StoredProcedure [dbo].[CommentOnResource]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CommentOnResource]        
     @ResourceId INT,    
     @Comments nvarchar(2000),    
     @CommentedBy int        
AS    
BEGIN    
Declare @return INT    
    
  INSERT INTO ResourceComments(ResourceId,Comments,UserId, CommentDate,IsHidden,UpdateDate)    
  VALUES (@ResourceId,@Comments,@CommentedBy, GETDATE(),0,GETDATE())     
    
  IF @@IDENTITY>0    
  SET @return =100 -- creation success    
    
  ELSE    
  SET @return =107    
      
    
    
  RETURN @return    
END    
GO
/****** Object:  StoredProcedure [dbo].[CreateCategory]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[CreateCategory] 
	@Name NVARCHAR(150),
	@Name_Ar NVARCHAR(350),
	@CreatedBy INT
AS
BEGIN
Declare @return INT
IF NOT EXISTS (SELECT * FROM CategoryMaster WHERE Name=@Name and Status = 1)
BEGIN	
		INSERT INTO CategoryMaster (Name,CreatedBy,CreatedOn, UpdatedBy,UpdatedOn,Name_Ar,Active,Status)
		VALUES (@Name,@CreatedBy,GETDATE(),@CreatedBy,GETDATE(),@Name_Ar,1,1)
		
		exec CreateLogEntry @LogModuleId=1,@UserId=@CreatedBy,@ActionId=1,@ActionDetail=''

		SET @return =100 -- creation success
END

ELSE
	BEGIN	
		SET @return = 105 -- Record exists
	END

	SELECT	cm.Id,
				cm.Name,
				cm.CreatedOn,				
			 CONCAT(c.FirstName, ' ', c.LastName) as CreatedBy,
				CONCAT(l.FirstName, ' ', l.LastName) as UpdatedBy,
				cm.UpdatedOn, cm.Active, cm.Name_Ar
		 from CategoryMaster cm 
		 inner join UserMaster c  on cm.CreatedBy= c.Id
		 inner join UserMaster l on cm.UpdatedBy =l.Id and cm.Status = 1 
		 --and c.Active = 1 and l.Active = 1
		 order by cm.Id desc 	
		 RETURN @return
END
GO
/****** Object:  StoredProcedure [dbo].[CreateCopyright]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[CreateCopyright]     
 @Title NVARCHAR(100),    
 @Description Text,    
 @Title_Ar NVARCHAR(100),    
 @Description_Ar NVARCHAR(MAX),    
 @CreatedBy INT,    
 @Media NVARCHAR(100)=NULL ,  
 @Protected BIT = 0,
 @IsResourceProtect BIT = 0
AS    
BEGIN    
Declare @return INT    
  
IF NOT EXISTS (SELECT * FROM CopyrightMaster WHERE Title=@Title and Status = 1)    
BEGIN     
    
  INSERT INTO CopyrightMaster (Title,Description,Title_Ar,Description_Ar, CreatedBy,CreatedOn, UpdatedBy,UpdatedOn,Media,Protected,IsResourceProtect,Active,Status)    
  VALUES (@Title, @Description,@Title_Ar,@Description_Ar,@CreatedBy,GETDATE(),@CreatedBy,GETDATE(),@Media,@Protected,@IsResourceProtect,1,1)    
      
  -- do log entry here     
    
  SET @return = 100 -- creation success    
END    
    
ELSE    
 BEGIN    
  SET @return = 105 -- Record exists    
 END    
    
  SELECT cr.Id,    
    cr.Title,    
    Cr.Description,    
    cr.Title_Ar,    
    cr.Description_Ar,    
    cr.CreatedOn,    
    CONCAT(c.FirstName, ' ', c.LastName) as CreatedBy,    
    CONCAT(l.FirstName, ' ', l.LastName) as UpdatedBy, cr.Active, cr.UpdatedOn,Media,
	cr.IsResourceProtect,
 cr.Protected  
   from CopyrightMaster cr     
   inner join UserMaster c  on cr.CreatedBy= c.Id    
   inner join UserMaster l on cr.UpdatedBy =l.Id 
   where cr.Status = 1 --and c.Active = 1 and l.Active = 1
   order by cr.Id desc     
    
   RETURN  @return    
    
END    
GO
/****** Object:  StoredProcedure [dbo].[CreateCourse]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CreateCourse]         
(        
     @Title NVARCHAR(250),        
           @CategoryId INT,        
           @SubCategoryId INT=NULL,        
           @Thumbnail  NVARCHAR(MAX)=NULL,        
           @CourseDescription  NVARCHAR(2000)=NULL,        
           @Keywords  NVARCHAR(1500)=NULL,        
           @CourseContent NTEXT=NULL,                  
           @CopyRightId INT=NULL,        
           @IsDraft BIT=NULL,        
     @CreatedBy INT=NULL,        
     @EducationId int= NULL,        
           @ProfessionId int=NULL,          
   @References NVARCHAR(MAX)=null,          
   @CourseFiles NVARCHAR(MAX)=null,      
   @ReadingTime INT = NULL,        
   @UT_Sections [UT_Sections] Readonly,        
   @UT_Resource [UT_Resource] Readonly,      
   @LevelId INT = NULL,      
   @EducationalStandardId INT = NULL,      
   @EducationalUseId INT = NULL      
     )        
AS        
BEGIN        
        
DECLARE @Id INT        
Declare @return INT        
DECLARE @SectionsCount INT;    
DECLARE @OutCourseId INT   
DECLARE @MessageTypeId Int;   
        
INSERT INTO [dbo].[CourseMaster]        
           (Title        
           ,CategoryId        
           ,SubCategoryId        
           ,Thumbnail        
           ,CourseDescription        
           ,Keywords        
           ,CourseContent        
           ,CopyRightId        
           ,IsDraft        
           ,CreatedBy         
           ,EducationId        
           ,ProfessionId        
   ,CreatedOn,        
   ReadingTime,      
   LevelId,      
   EducationalStandardId,      
   EducationalUseId,      
   IsApproved
   )        
     VALUES        
           (@Title,        
           @CategoryId,        
           @SubCategoryId,        
           @Thumbnail,         
           @CourseDescription,         
           @Keywords,         
           @CourseContent,                
           @CopyRightId,        
           @IsDraft,         
     @CreatedBy,        
           @EducationId,        
           @ProfessionId,        
     GETDATE(),        
     @ReadingTime,      
     @LevelId ,      
     @EducationalStandardId ,      
     @EducationalUseId,
	 NULL
     )        
        
 SET @Id=SCOPE_IDENTITY();    
  SET @OutCourseId =      @Id  
  
  
        
 IF @Id>0        
 BEGIN      
   
 SET @return = 100 -- creation success        
         
IF(@IsDraft = 0)  
BEGIN         
DECLARE @TotalCount INT;        
DECLARE @QrcID INT;        
DECLARE @RecordId INT;        
DECLARE @SectionId INT;        
DECLARE @i INT; 

SET @i = 1;        
   
   Declare @Date Datetime
   SET @Date = getdate() 
   SELECT @MessageTypeId = ID FROM MessageType WHERE [Type] = 'Course Approval'
   --SELECT @UserIdToNotify = UserId FROm @QRCUsers WHERE Id= @ict
--exec spi_Notifications @Id,1,@Title,@CourseDescription,@MessageTypeId,0,@Date,0,0,@CreatedBy,NULL,NULL,NULL
--exec spi_Notifications @Id,1,@Title,@CourseDescription,@MessageTypeId,0,@Date,0,0,@UserIdToNotify,NULL,NULL,NULL
        
select @TotalCount = count(*) from QRCCategory WHERE CategoryID = @CategoryID AND IsAvailable =0 --order by CreatedOn asc        
        
IF(@TotalCount>0)        
BEGIN        
   select TOP 1 @RecordId=ID,@QrcID=QRCId from QRCCategory WHERE CategoryID = @CategoryID AND IsAvailable =0   
   AND QRCID IN (SELECT DISTINCT QRCID FROM QRCUserMapping  
   EXCEPT (SELECT DISTINCT QRCID FROM QRCUserMapping WHERE UserId = @CreatedBy AND Active = 1))  
   order by CreatedOn asc        
        
   Update QRCCategory SET IsAvailable = 1 WHERE ID = @RecordId        
        
   IF EXISTS(        
   SELECT         
     TOP 1 1        
     from QRCusermapping  where QRCID =@QrcID and active = 1 and CategoryId = @CategoryId)        
     BEGIN        
       INSERT INTO ContentApproval(ContentId,        
       ContentType,        
       CreatedBy,        
       CreatedOn,        
       AssignedTo,        
       AssignedDate,  
    QrcId)        
        
       SELECT         
 @Id,        
       1, -- Course        
       @CreatedBy,        
       GETDATE(),        
       userid,        
       GETDATE() ,  
    @QrcID  
       from QRCusermapping  where QRCID =@QrcID and active = 1   
    and QRCusermapping.CategoryId = @CategoryId     
    UPDATE CourseMaster SET IsDraft = 0 WHERE Id = @Id      
      END        
END        
IF(@TotalCount=1)          
BEGIN          
Update QRCCategory SET IsAvailable = 0  WHERE CategoryID = @CategoryID           
END    
  
--IF NOT EXISTS(SELECT ContentId FROm ContentApproval WHERE ContentId = @id and ContentType = 1)  
-- BEGIN  
-- UPDATE CourseMaster SET IsDraft = 1 WHERE Id = @Id  
-- END    
END        
SET @SectionsCount = (SELECT Count(*) FROM @UT_Sections)        
        
WHILE @i <= @SectionsCount        
        
BEGIN        
        
INSERT INTO CourseSections        
(        
Name,        
CourseId        
)        
        
SELECT Name,@Id FROM @UT_Sections WHERE Id = @i        
        
SET @SectionId = SCOPE_IDENTITY();        
        
INSERT INTO [dbo].[SectionResource](ResourceId,        
SectionId)        
SELECT         
ResourceId,        
@SectionId        
FROM @UT_Resource WHERE SectionId in (SELECT distinct  ID FROM @UT_Sections WHERE Id = @i)        
        
SET @i= @i+1;        
END;        
        
--IF (@ResourcesIds <> NULL)        
--BEGIN        
--CREATE TABLE #temp        
--(        
--CourseId int,        
--ResourceId int        
--)        
--INSERT INTO #temp (        
--CourseId,        
--ResourceId        
--)        
--SELECT         
--@Id,        
--value      
--FROM StringSplit(@ResourcesIds, ',')          
        
--DECLARE @FirstResourceId INT;        
--SET @FirstResourceId =(SELECT top 1 ResourceID FROM #temp)        
--IF(@FirstResourceId>0)        
--BEGIN        
        
--IF NOT EXISTS(SELECT TOP 1 1 FROM CourseResourceMapping WHERE CourseID =@Id )        
--   INSERT INTO CourseResourceMapping(CourseId,ResourcesId)        
--   SELECT CourseId,ResourceId FROM #temp        
        
--END;        
--END;        
 IF @References IS NOT NULL        
 BEGIN        
 -- INSERT Resource URL References FROM JSON        
         
  INSERT INTO CourseURLReferences        
            
  SELECT @Id,URLReferenceId,GETDATE() FROM          
   OPENJSON ( @References )          
  WITH (           
URLReferenceId   int '$.URLReferenceId'         
  )         
        
 END        
        
 IF @CourseFiles IS NOT NULL        
 BEGIN        
 -- INSERT Resource Associated Files FROM JSON        
         
  INSERT INTO CourseAssociatedFiles        
            
  SELECT @Id,AssociatedFile,GETDATE(),FileName,1 FROM          
   OPENJSON ( @CourseFiles )          
  WITH (           
              AssociatedFile   varchar(200) '$.AssociatedFile' ,    
     FileName   nvarchar(200) '$.FileName'     
  )         
        
 END        
  
        
        
  SELECT Id,        
TitleId,        
FirstName +' ' +LastName as UserName,        
Email FROM UserMaster WHERE ID in (        
 SELECT         
userid        
from QRCusermapping  where QRCID = @QrcID and active = 1) AND         
IsEmailNotification = 1 

DECLARE @QRCUsers TABLE(ID INT IDENTITY(1,1),UserId INT)
INSERT INTO @QRCUsers  
SELECT Id FROM UserMaster WHERE ID in (SELECT userid from QRCusermapping  where QRCID = @QrcID and active = 1) AND IsEmailNotification = 1 
DECLARE @TotalRows INT
DECLARE @ict INT = 1
SELECT @TotalRows = COUNT(*) FROM @QRCUsers
WHILE @ict <= @TotalRows
BEGIN
	SELECT @MessageTypeId = ID FROM MessageType WHERE [Type] = 'QRC Review'
	DECLARE @UserIdToNotify INT 
	SELECT @UserIdToNotify = UserId FROm @QRCUsers WHERE Id= @ict
	exec spi_Notifications @Id,1,@Title,@CourseDescription,@MessageTypeId,0,@Date,0,0,@UserIdToNotify,NULL,NULL,NULL
SET @ict = @ict + 1
END             
        
--SELECT * from Coursesections WHERE CourseId = @Id        
        
--SELECT rm.id,rm.title,sr.SectionId FROM SectionResource sr        
--INNER JOIN ResourceMaster rm        
--ON sr.resourceid = rm.id        
        
--WHERE sr.SectionId in (SELECT ID from Coursesections WHERE CourseId = @Id)        
        
        
 DECLARE @SharedCount INT         
DECLARE @DownloadCount INT       
DECLARE @VisitCount INT    
          
IF Exists(select TOP 1 1 from CourseMaster WHERE Id=@Id)               
 BEGIN              
           
 SET @SharedCount = (select count(Id) from ContentSharedInfo WHERE ContentId = @Id AND ContentTypeId = 1)          
 SET @DownloadCount = (select count(Id) from ContentDownloadInfo WHERE ContentId = @Id AND ContentTypeId = 1)          
    
    
 SET @VisitCount = (SELECT ViewCount FROM CourseMaster WHERE ID = @Id)    
     
 IF(@VisitCount IS NULL)    
 BEGIN    
 Update CourseMaster SET ViewCount =  1 WHERE ID = @Id     
    
 END    
    
 ELSE     
 BEGIN    
  Update CourseMaster SET ViewCount = @VisitCount+ 1 WHERE ID = @Id     
 END;    
    
    
 SELECT r.Id              
      ,r.Title              
      ,r.CategoryId, c.Name as CategoryName              
      ,SubCategoryId, sc.Name as SubCategoryName              
      ,Thumbnail              
      ,CourseDescription              
      ,Keywords              
      ,CourseContent                 
      ,CopyRightId, cm.Title as CopyrightTitle, cm.Description as CopyrightDescription    
      ,IsDraft      
   , EducationId, edm.Name as EducationName              
   , ProfessionId, pm.Name as ProfessionName              
      , CONCAT(um.FirstName, '', um.LastName) as CreatedBy, um.Id as CreatedById              
      ,r.CreatedOn              
      ,IsApproved              
      ,Rating                   
      ,ReportAbuseCount,ViewCount,AverageReadingTime,@DownloadCount AS DownloadCount,ReadingTime  ,          
   @SharedCount as SharedCount,        
   LastView,        
   r.LevelId,     
   ViewCount,    
   (SELECT [Level] FROM lu_Level WHERE Id = r.LevelId) AS [LevelName],        
   (SELECT Level_Ar FROM lu_Level WHERE Id = r.LevelId) AS [Level_Ar],        
   r.EducationalStandardId,        
   (SELECT [Standard] FROM lu_educational_standard WHERE Id = r.EducationalStandardId) AS [Standard],        
   (SELECT [Standard_Ar] FROM lu_educational_standard WHERE Id = r.EducationalStandardId) AS [Standard_Ar],        
   r.EducationalUseId,        
    (SELECT [EducationalUse] FROM lu_educational_use WHERE Id = r.EducationalUseId) AS [EducationalUseName],        
   (SELECT [EducationalUse_Ar] FROM lu_educational_use WHERE Id = r.EducationalUseId) AS [EducationalUse_Ar]        
  FROM CourseMaster r              
   inner join CategoryMaster c on r.CategoryId = c.Id                 
   left join SubCategoryMaster sc on r.SubCategoryId=sc.Id              
   left join CopyrightMaster cm on r.CopyRightId = cm.Id              
   left join EducationMaster edm on edm.Id= r.EducationId              
   left join ProfessionMaster pm on pm.Id= r.ProfessionId              
   inner join UserMaster um on r.CreatedBy =um.Id where r.Id=@Id Order by r.Id desc               
              
              
   Update CourseMaster SET LastView = GETDATE() WHERE ID = @Id      
              
        
    
    
SELECT caf.Id              
      ,CourseId              
      ,AssociatedFile              
      ,caf.CreatedOn  ,  
  caf.FileName  
  FROM CourseAssociatedFiles caf INNER JOIN CourseMaster cm ON cm.Id=caf.CourseId  AND cm.Id=@Id              
              
              
              
  SELECT cur.Id              
      ,CourseId              
      ,cur.URLReferenceId,uwl.URL as URLReference              
      ,cur.CreatedOn              
  FROM dbo.CourseURLReferences cur INNER JOIN CourseMaster cm ON cm.Id=cur.CourseId              
           inner join WhiteListingURLs uwl on uwl.Id=cur.URLReferenceId AND cm.Id=@Id              
              
              
SELECT cc.Id              
      ,[CourseId]              
      ,[Comments]              
      ,CONCAT(um.FirstName, ' ', um.LastName) as CommentedBy, um.Id As CommentedById, um.Photo as CommentorImage              
      ,[CommentDate]              
      ,[ReportAbuseCount]              
  FROM [dbo].[CourseComments] cc inner join UserMaster um on cc.UserId=um.Id AND cc.CourseId=@Id where cc.IsHidden=0              
                
            
 SELECT * from Coursesections WHERE CourseId = @Id              
              
--SELECT rm.id,rm.title,sr.SectionId FROM SectionResource sr              
--INNER JOIN ResourceMaster rm              
--ON sr.resourceid = rm.id              
              
--WHERE sr.SectionId in (SELECT ID from Coursesections WHERE CourseId = @Id)         
      
         
 SELECT r.Id              
      ,r.Title   ,      
   sr.SectionId       
      ,r.CategoryId, c.Name as CategoryName              
      ,SubCategoryId, sc.Name as SubCategoryName              
      ,Thumbnail              
      ,ResourceDescription              
      ,Keywords              
      ,ResourceContent              
      ,MaterialTypeId, mt.Name as MaterialTypeName              
      ,CopyRightId, cm.Title as CopyrightTitle, cm.Description as CopyrightDescription              
      ,IsDraft              
      ,CONCAT(um.FirstName, '', um.LastName) as CreatedBy, um.Id as CreatedById            
      ,r.CreatedOn              
      ,IsApproved              
      ,Rating       
      ,AlignmentRating              
  ,ReportAbuseCount,ViewCount,AverageReadingTime,@DownloadCount AS DownloadCount,r.ReadingTime  ,            
   es.Standard,            
 eu.EducationalUse,            
 el.Level,            
 Objective,           
 @SharedCount as SharedCount,          
 LastView,          
 [Format]                
  FROM       
  SectionResource sr              
INNER JOIN ResourceMaster r             
ON sr.resourceid = r.id                 
   inner join CategoryMaster c on r.CategoryId = c.Id              
   inner join MaterialTypeMaster mt on r.MaterialTypeId=mt.Id              
   left join SubCategoryMaster sc on r.SubCategoryId=sc.Id              
   left join CopyrightMaster cm on r.CopyRightId = cm.Id              
    LEFT JOIN lu_Educational_Standard es ON r.EducationalStandardId = es.Id            
   LEFT JOIN lu_Educational_Use eu ON r.EducationalUseId = eu.Id            
   LEFT JOIN lu_Level el ON r.LevelId = el.Id            
            
   inner join UserMaster um on r.CreatedBy =um.Id       
  where sr.SectionId in (SELECT ID from Coursesections WHERE CourseId = @Id)      
  END       
        
         
 END        
        
 ELSE SET @return= 107         
        
    RETURN @return        
END        

GO
/****** Object:  StoredProcedure [dbo].[CreateEducation]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[CreateEducation] 
	@Name NVARCHAR(150),
	@CreatedBy INT,
	@Name_Ar NVARCHAR(MAX),
	@Active BIT
AS
BEGIN
Declare @return INT
IF NOT EXISTS (SELECT * FROM EducationMaster WHERE Name=@Name and Status = 1)
BEGIN	
		INSERT INTO EducationMaster (Name,CreatedBy,CreatedOn, UpdatedBy,UpdatedOn,Name_Ar,Active,Status)
		VALUES (@Name,@CreatedBy,GETDATE(),@CreatedBy,GETDATE(),@Name_Ar,@Active,1)
		
		-- do log entry here

	SET	@return= 100 -- creation success

END

ELSE
	BEGIN
		SET	@return=105 -- Record exists
	END

	SELECT	em.Id,
				em.Name,
				em.CreatedOn,				
				c.FirstName,
				 CONCAT(c.FirstName, ' ', c.LastName) as CreatedBy,
				CONCAT(l.FirstName, ' ', l.LastName) as UpdatedBy,
				em.UpdatedOn, em.Active,em.Name_Ar
		 from EducationMaster em 
		 inner join UserMaster c  on em.CreatedBy= c.Id
		 inner join UserMaster l on em.UpdatedBy =l.Id	
		 where em.Status = 1 --and c.Active = 1 and l.Active = 1
		 Order by em.Id desc 	
		 
		 RETURN @return	
END
GO
/****** Object:  StoredProcedure [dbo].[CreateInstitution]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[CreateInstitution]   
 @Name NVARCHAR(255),  
 @CreatedBy INT  ,
 @Name_Ar NVARCHAR(500)
AS  
BEGIN  
Declare @return INT  
IF NOT EXISTS (SELECT * FROM InstitutionMaster WHERE Name=@Name)  
BEGIN   
  INSERT INTO InstitutionMaster (Name,CreatedBy,CreatedOn,UpdatedBy,UpdatedOn,Name_Ar)  
  VALUES (@Name,@CreatedBy,GETDATE(),@CreatedBy,GETDATE(),@Name_Ar)  
    
  -- do log entry here  
    
  SET @return= 100 -- creation success  
END  
  
ELSE  
 BEGIN  
  SET @return=105 -- Record exists  
 END  
  
 SELECT cr.Id,  
    cr.Name,  
    cr.CreatedOn,      
    CONCAT(c.FirstName, ' ', c.LastName) as CreatedBy,  
    CONCAT(l.FirstName, ' ', l.LastName) as UpdatedBy,  
    cr.UpdatedOn, cr.Active ,cr.Name_Ar 
   from InstitutionMaster cr   
   inner join UserMaster c  on cr.CreatedBy= c.Id  
   inner join UserMaster l on cr.UpdatedBy =l.Id Order by cr.Id desc    
     
   RETURN @return   
END  
GO
/****** Object:  StoredProcedure [dbo].[CreateLogEntry]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CreateLogEntry] (
	@LogModuleId INT,
	@UserId INT,
	@ActionId INT,
	@ActionDetail NVARCHAR(250))

AS
BEGIN	
        INSERT INTO [dbo].[LogAction]
           ([LogModuleId]
           ,[UserId]
           ,[ActionId]
           ,[ActionDate]
           ,[ActionDetail])
     VALUES
           (@LogModuleId
           ,@UserId
           ,@ActionId
           ,GETDATE()
           ,@ActionDetail)
END

GO
/****** Object:  StoredProcedure [dbo].[CreateMaterialType]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CreateMaterialType] 
	@Name NVARCHAR(150),
	@Name_Ar NVARCHAR(150),
	@CreatedBy INT
AS
BEGIN
Declare @return INT
IF NOT EXISTS (SELECT * FROM MaterialTypeMaster WHERE Name=@Name and Status = 1)
BEGIN	
		INSERT INTO MaterialTypeMaster (Name,Name_Ar,CreatedBy,CreatedOn, UpdatedBy,UpdatedOn,Active,Status)
		VALUES (@Name,@Name_Ar,@CreatedBy,GETDATE(),@CreatedBy,GETDATE(),1,1)
		
		-- do log entry here

		SET @return =100 -- creation success
END

ELSE
	BEGIN	
		SET @return = 105 -- Record exists
	END

	SELECT	cm.Id,
				cm.Name,
				cm.Name_Ar,
				cm.CreatedOn,				
			 CONCAT(c.FirstName, ' ', c.LastName) as CreatedBy,
				CONCAT(l.FirstName, ' ', l.LastName) as UpdatedBy,
				cm.UpdatedOn, cm.Active
		 from MaterialTypeMaster cm 
		 inner join UserMaster c  on cm.CreatedBy= c.Id
		 inner join UserMaster l on cm.UpdatedBy =l.Id
		 where cm.Status = 1 --and c.Active = 1 and l.Active = 1
		 order by cm.Id desc 	
		 RETURN @return
END

GO
/****** Object:  StoredProcedure [dbo].[CreateProfession]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CreateProfession] 
	@Name NVARCHAR(200),
	@CreatedBy INT,
	@Name_Ar NVARCHAR(200)
AS
BEGIN
Declare @return INT
IF NOT EXISTS (SELECT * FROM ProfessionMaster WHERE Name=@Name and Status=1)
BEGIN	
		INSERT INTO ProfessionMaster (Name,CreatedBy,CreatedOn, UpdatedBy,UpdatedOn,Name_Ar,Active,Status)
		VALUES (@Name,@CreatedBy,GETDATE(),@CreatedBy,GETDATE(),@Name_Ar,1,1)
		
		-- do log entry here

		SET @return =100 -- creation success
END

ELSE
	BEGIN	
		SET @return = 105 -- Record exists
	END

	SELECT	cm.Id,
				cm.Name,
				cm.Name_Ar,
				cm.CreatedOn,				
			 CONCAT(c.FirstName, ' ', c.LastName) as CreatedBy,
				CONCAT(l.FirstName, ' ', l.LastName) as UpdatedBy,
				cm.UpdatedOn, cm.Active
		 from ProfessionMaster cm 
		 inner join UserMaster c  on cm.CreatedBy= c.Id
		 inner join UserMaster l on cm.UpdatedBy =l.Id where cm.Status=1 order by cm.Id desc 	
		 RETURN @return
END
GO
/****** Object:  StoredProcedure [dbo].[CreateQRC]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
      
      
CREATE PROCEDURE [dbo].[CreateQRC]       
 @Name NVARCHAR(150),      
 @Description NVARCHAR(2000),      
 @CreatedBy INT,      
 @CategoryIds NVARCHAR(450)      
AS      
BEGIN      
Declare @return INT      
IF NOT EXISTS (SELECT * FROM QRCMaster WHERE Name=@Name)      
BEGIN       
      
CREATE TABLE #temp      
(      
Name nvarchar(50),      
Description nvarchar(1000),      
CreatedBy int,      
CatId int      
)      
      
INSERT INTO #temp (      
Name,      
Description,      
CreatedBy,      
CatId      
)      
SELECT       
@Name,      
@Description,      
@CreatedBy,      
value        
FROM StringSplit(@CategoryIds, ',')        
      
      
DECLARE @QRCID INT;      
  INSERT INTO QRCMaster (Name,Description,CreatedBy,CreatedOn, UpdatedBy,UpdatedOn,Active)      
  VALUES (@Name,@Description,@CreatedBy,GETDATE(),@CreatedBy,GETDATE(),1)      
      
      
  SET @QRCID = SCOPE_IDENTITY();      
      
  print @QRCID      
  INSERT INTO QRCCategory(QRCId,CategoryId,CreatedOn)      
  SELECT @QRCID,CatId,GETDATE() FROM #temp;      
        
  -- do log entry here      
      
  SET @return =100 -- creation success      
END      
      
ELSE      
 BEGIN       
  SET @return = 105 -- Record exists      
 END      
      
 SELECT cm.Id,      
    cm.Name,Description,      
    cm.CreatedOn,          
    CONCAT(c.FirstName, ' ', c.LastName) as CreatedBy,      
    CONCAT(l.FirstName, ' ', l.LastName) as UpdatedBy,      
    cm.UpdatedOn, cm.Active      
   from QRCMaster cm       
   inner join UserMaster c  on cm.CreatedBy= c.Id      
   inner join UserMaster l on cm.UpdatedBy =l.Id order by cm.Id desc        
   RETURN @return      
END      
      
GO
/****** Object:  StoredProcedure [dbo].[CreateResource]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CreateResource]           
(          
     @Title NVARCHAR(250),          
           @CategoryId INT,          
           @SubCategoryId INT=NULL,          
           @Thumbnail  NVARCHAR(400) = NULL,          
           @ResourceDescription  NVARCHAR(2000)=NULL,          
           @Keywords  NVARCHAR(1500)= NULL,          
           @ResourceContent NTEXT=NULL,          
           @MaterialTypeId INT=NULL,          
           @CopyRightId INT=NULL,          
           @IsDraft BIT,          
     @CreatedBy INT,          
     @References NVARCHAR(MAX)=null,            
     @ResourceFiles NVARCHAR(MAX)=null,          
     @ReadingTime INT = NULL,          
     @ResourceSourceId INT = NULL,          
     @LevelId INT = NULL,          
     @EducationalStandardId INT = NULL,          
     @EducationalUseId INT = NULL,          
     @Format NVARCHAR(100) = NULL,          
     @Objective NVARCHAR(MAX) = NULL          
               
     )          
AS          
BEGIN          
          
DECLARE @Id INT          
Declare @return INT   
DECLARE @MessageTypeId INT       
          
INSERT INTO [dbo].[ResourceMaster]          
           ([Title]          
           ,[CategoryId]          
           ,[SubCategoryId]          
           ,[Thumbnail]          
           ,[ResourceDescription]          
           ,[Keywords]          
           ,[ResourceContent]          
           ,[MaterialTypeId]          
           ,[CopyRightId]          
           ,[IsDraft]          
           ,[CreatedBy]          
           ,[CreatedOn],          
     [ReadingTime],          
     [LevelId],          
     [EducationalStandardId],          
     [EducationalUseId],          
     [Format],          
     [Objective],
	 [IsApproved]
   )          
     VALUES          
           (@Title,          
           @CategoryId,          
           @SubCategoryId,          
           @Thumbnail,           
           @ResourceDescription,           
           @Keywords,           
           @ResourceContent,          
           @MaterialTypeId,          
           @CopyRightId,          
           @IsDraft,           
     @CreatedBy,          
     GETDATE(),          
     @ReadingTime,          
     @LevelId,          
     @EducationalStandardId,          
     @EducationalUseId,          
     @Format,          
     @Objective,
	 NULL
     )          
          
 SET @Id=SCOPE_IDENTITY();          
          
IF(@IsDraft = 0)  
BEGIN           
DECLARE @TotalCount INT;          
DECLARE @QrcID INT;          
DECLARE @RecordId INT;          
          
--select top 10 * from QRCCategory where CategoryId =@CategoryID          
--select * from QRCCategory WHERE CategoryID = @CategoryID AND IsAvailable =0 order by CreatedOn asc          
Declare @Date Datetime
SET @Date = getdate() 
SELECT @MessageTypeId = ID FROM MessageType WHERE [Type] = 'Course Approval'

--exec spi_Notifications @Id,1,@Title,@ResourceDescription,@MessageTypeId,0,@Date,0,0,@CreatedBy,NULL,NULL,NULL
        
       
select @TotalCount = count(*) from QRCCategory WHERE CategoryID = @CategoryID AND IsAvailable =0 --order by CreatedOn asc          
          
IF(@TotalCount>0)          
BEGIN          
select TOP 1 @RecordId=ID,@QrcID=QRCId from QRCCategory WHERE CategoryID = @CategoryID AND IsAvailable =0   
AND QRCID IN (SELECT DISTINCT QRCID FROM QRCUserMapping  
   EXCEPT (SELECT DISTINCT QRCID FROM QRCUserMapping WHERE UserId = @CreatedBy AND Active = 1))  
order by CreatedOn asc          
          
Update QRCCategory SET IsAvailable = 1 WHERE ID = @RecordId          
          
IF EXISTS(          
SELECT           
TOP 1 1          
from QRCusermapping  where QRCID =@QrcID and active = 1 and CategoryId = @CategoryId)          
BEGIN          
INSERT INTO ContentApproval(ContentId,          
ContentType,          
CreatedBy,          
CreatedOn,          
AssignedTo,          
AssignedDate,    
QrcId)          
          
SELECT           
@Id,          
2, -- Resource          
@CreatedBy,          
GETDATE(),          
userid,          
GETDATE(),    
@QrcID    
from QRCusermapping  where QRCID =@QrcID and active = 1  
and QRCusermapping.CategoryId = @CategoryId          
      UPDATE ResourceMaster SET IsDraft = 0 WHERE Id = @Id       
END          
          
          
          
END          
    
IF(@TotalCount=1)         
BEGIN          
Update QRCCategory SET IsAvailable = 0  WHERE CategoryID = @CategoryID           
END          
          
          
--select top 10 * from QRCCategory where CategoryId =@CategoryID          
            
 --IF NOT EXISTS(SELECT ContentId FROm ContentApproval WHERE ContentId = @id and ContentType = 2)  
 --BEGIN  
 --UPDATE ResourceMaster SET IsDraft = 1 WHERE Id = @Id  
 --END   
END   
 IF @Id>0   
 BEGIN          
 SET @return =100 -- creation success          
          
 IF @References IS NOT NULL          
 BEGIN          
 -- INSERT Resource URL References FROM JSON          
           
  INSERT INTO ResourceURLReferences          
              
  SELECT @Id,URLReferenceId,GETDATE() FROM            
   OPENJSON ( @References )            
  WITH (             
              URLReferenceId   int '$.URLReferenceId'           
  )           
          
 END          
          
 IF @ResourceFiles IS NOT NULL          
 BEGIN          
 -- INSERT Resource Associated Files FROM JSON          
           
  INSERT INTO ResourceAssociatedFiles          
              
  SELECT @Id,AssociatedFile,GETDATE(),FileName,1 FROM            
   OPENJSON ( @ResourceFiles )            
  WITH (             
              AssociatedFile   nvarchar(MAX) '$.AssociatedFile',      
     FileName   nvarchar(MAX) '$.FileName'       
  )           
          
 END          
          
 DECLARE @Version INT;          
 IF(@ResourceSourceId<>'')          
 BEGIN          
          
 SELECT TOP 1 @Version=Version FROM ResourceRemixHistory WHERE ResourceSourceId=@ResourceSourceId order by CreatedOn desc          
 IF(@Version IS NULL)          
 BEGIN          
 SET @Version=1          
 END          
          
 ELSE          
          
 BEGIN          
SET @Version=@Version+1          
 END          
 INSERT INTO ResourceRemixHistory(ResourceSourceID,          
ResourceRemixedID,          
Version,          
CreatedOn)          
VALUES(          
@ResourceSourceId,          
@Id,          
@Version,          
GETDATE()          
)          
          
END          
 SELECT Id,          
TitleId,          
FirstName +' ' +LastName as UserName,          
Email FROM UserMaster WHERE ID in (          
 SELECT           
userid          
from QRCusermapping  where QRCID = @QrcID and active = 1)          
AND           
IsEmailNotification = 1 
 
DECLARE @QRCUsers TABLE(ID INT IDENTITY(1,1),UserId INT)
INSERT INTO @QRCUsers  
SELECT Id FROM UserMaster WHERE ID in (SELECT userid from QRCusermapping  where QRCID = @QrcID and active = 1) AND IsEmailNotification = 1 
DECLARE @TotalRows INT
DECLARE @ict INT = 1
SELECT @TotalRows = COUNT(*) FROM @QRCUsers
WHILE @ict <= @TotalRows
BEGIN
	SELECT @MessageTypeId = ID FROM MessageType WHERE [Type] = 'QRC Review'
	DECLARE @UserIdToNotify INT 
	SELECT @UserIdToNotify = UserId FROm @QRCUsers WHERE Id= @ict
	exec spi_Notifications @Id,1,@Title,@ResourceDescription,@MessageTypeId,0,@Date,0,0,@UserIdToNotify,NULL,NULL,NULL
SET @ict = @ict + 1
END
        
 exec GetResourceById  @Id          
 END          
          
 ELSE SET @return= 107           
          
    RETURN @return          
END          
GO
/****** Object:  StoredProcedure [dbo].[CreateStream]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CreateStream] 
	@Name NVARCHAR(200),
	@Name_Ar NVARCHAR(200),  
	@CreatedBy INT
AS
BEGIN
Declare @return INT
IF NOT EXISTS (SELECT * FROM StreamMaster WHERE Name=@Name)
BEGIN	
		INSERT INTO StreamMaster (Name,Name_Ar,CreatedBy,CreatedOn, UpdatedBy,UpdatedOn)
		VALUES (@Name,@Name_Ar,@CreatedBy,GETDATE(),@CreatedBy,GETDATE())
		
		-- do log entry here

		SET @return =100 -- creation success
END

ELSE
	BEGIN	
		SET @return = 105 -- Record exists
	END

	SELECT	cm.Id,
				cm.Name,
				cm.Name_Ar,
				cm.CreatedOn,				
			 CONCAT(c.FirstName, '', c.LastName) as CreatedBy,
				CONCAT(l.FirstName, '', l.LastName) as UpdatedBy,
				cm.UpdatedOn, cm.Active
		 from StreamMaster cm 
		 inner join UserMaster c  on cm.CreatedBy= c.Id
		 inner join UserMaster l on cm.UpdatedBy =l.Id order by cm.Id desc 	
		 RETURN @return
END

GO
/****** Object:  StoredProcedure [dbo].[CreateSubCategory]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CreateSubCategory] 
	@Name NVARCHAR(200),
	@Name_Ar NVARCHAR(200),
	@CategoryId INT,
	@CreatedBy INT
AS
BEGIN
Declare @return INT
IF NOT EXISTS (SELECT * FROM SubCategoryMaster WHERE Name=@Name AND CategoryId=@CategoryId and Status = 1)
BEGIN	
		INSERT INTO SubCategoryMaster (Name,Name_Ar,CategoryId, CreatedBy,CreatedOn, UpdatedBy,UpdatedOn,Active,Status)
		VALUES (@Name,@Name_Ar,@CategoryId,@CreatedBy,GETDATE(),@CreatedBy,GETDATE(),1,1)
		
		-- do log entry here

		SET @return =100 -- creation success
END

ELSE
	BEGIN	
		SET @return = 105 -- Record exists
	END

	SELECT	sm.Id,
				sm.Name,
				sm.Name_Ar,
				cm.Name as CategoryName, 
				cm.Id as CategoryId,
				sm.CreatedOn,				
			    CONCAT(c.FirstName, '', c.LastName) as CreatedBy,
				CONCAT(l.FirstName, '', l.LastName) as UpdatedBy,
				sm.UpdatedOn, 
				sm.Active
		 from SubCategoryMaster sm 
		 INNER JOIN CategoryMaster cm on sm.CategoryId=cm.Id
		 inner join UserMaster c  on sm.CreatedBy= c.Id
		 inner join UserMaster l on sm.UpdatedBy =l.Id 
		 where cm.Status = 1 --and c.Active = 1 and l.Active = 1
		 order by cm.Id desc 	
		 RETURN @return
END

GO
/****** Object:  StoredProcedure [dbo].[CreateUserInitialProfile]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CreateUserInitialProfile]       
  (       
           @FirstName nvarchar(250),      
           @LastName nvarchar(250)=NULL,        
           @Email nvarchar(250),                            
           @IsContributor bit,      
     @IsAdmin bit      
     )      
AS      
BEGIN      
Declare @return INT      
      
IF NOT EXISTS (SELECT * FROM UserMaster WHERE Email=@Email)      
BEGIN      
      
DECLARE @UserId INT      
INSERT INTO UserMaster      
           (      
           FirstName                 
           ,LastName                
           ,Email                
           ,IsContributor,    
     Active    
     ,IsAdmin,
	 CreatedOn,
	 ApprovalStatus)      
      
    VALUES(      
            @FirstName                 
           ,@LastName                
           ,@Email       
           ,@IsContributor,    
     1,      
      @IsAdmin,
	  GETDATE(),
	  0)      
      
SET @UserId=@@IDENTITY      
      
IF @UserId>0       
BEGIN      
SET @return=100      
      
select @UserId as Id      
      
END      
ELSE       
      
 SET @return=107      
      
END      
      
ELSE       
begin      
SET @return=108 --  EMAIL EXISTS      
      
select Id from UserMaster where Email=@Email      
end      
      
return @return      
END

GO
/****** Object:  StoredProcedure [dbo].[CreateUserProfile]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[CreateUserProfile]     
  (  @TitleId int=NULL,    
           @FirstName nvarchar(250),    
           @MiddleName nvarchar(250)=NULL,    
           @LastName nvarchar(250)=NULL,    
           @CountryId int=NULL,    
           @StateId int=NULL,    
           @Gender int,    
           @Email nvarchar(250),    
           @PortalLanguageId int=NULL,    
          --  @DepartmentId int=NULL,    
         --   @DesignationId int=NULL,    
           @DateOfBirth date,    
           @Photo nvarchar(200)=NULL,    
           @ProfileDescription nvarchar(4000)=NULL,    
           @SubjectsInterested nvarchar(MAX)=NULL,                   
           @IsContributor bit,    
     @UserCertifications nvarchar(max)=NULL,    
     @UserEducations nvarchar(max)=NULL,    
     @UserExperiences nvarchar(max)=NULL,    
     @UserLanguages nvarchar(max)=NULL,    
     @UserSocialMedias nvarchar(max)=NULL    
     )    
AS    
BEGIN    
Declare @return INT    
DECLARE @MessageTypeId INT  
Declare @Date Datetime  
  
SET @Date = getdate() 
IF NOT EXISTS (SELECT * FROM UserMaster WHERE Email=@Email)    
BEGIN    
    
DECLARE @UserId INT    
INSERT INTO UserMaster    
           (TitleId    
           ,FirstName    
           ,MiddleName    
           ,LastName    
           ,CountryId    
           ,StateId    
           ,Gender    
           ,Email    
           ,PortalLanguageId    
          --  ,DepartmentId    
           -- ,DesignationId    
           ,DateOfBirth    
           ,Photo    
           ,ProfileDescription    
           ,SubjectsInterested    
           ,ApprovalStatus    
           ,CreatedOn    
           ,UpdatedOn    
           ,Active    
           ,IsContributor  
     ,IsAdmin)    
    
    VALUES(@TitleId    
           ,@FirstName    
           ,@MiddleName    
           ,@LastName    
           ,@CountryId    
           ,@StateId    
           ,@Gender    
           ,@Email    
           ,@PortalLanguageId    
           -- ,@DepartmentId    
          --  ,@DesignationId    
           ,@DateOfBirth    
           ,@Photo    
           ,@ProfileDescription    
           ,@SubjectsInterested    
           ,0    
           ,GETDATE()    
           ,GETDATE()    
           ,1  
           ,@IsContributor  
     ,0)    
    SET @UserId=@@IDENTITY    
    
	  IF(@IsContributor = 1)  
  BEGIN  
  SELECT @MessageTypeId = ID FROM MessageType WHERE [Type] = 'Contributor Access Approved'  
  END  
  ELSE  
  
  BEGIN  
  SELECT @MessageTypeId = ID FROM MessageType WHERE [Type] = 'Contributor Access Rejected'  
  END;  
     exec spi_Notifications @UserId,3,'User Contributor','User Contributor Access Status',@MessageTypeId,@IsContributor,@Date,0,0,@UserId  


IF @UserId>0     
BEGIN    
SET @return=100    
    
    
IF @UserCertifications IS NOT NULL    
 BEGIN    
 -- INSERT USER CERTIFICATION FROM JSON    
     
INSERT INTO UserCertification    
        
SELECT @UserId,CertificationName,Year,GETDATE() FROM      
 OPENJSON ( @UserCertifications )      
WITH (       
              CertificationName   nvarchar(250) '$.CertificationName',                  
              [Year] int '$.Year'      
 )     
    
 END    
    
 IF @UserEducations IS NOT NULL    
 BEGIN    
  -- INSERT USER EDUCATION FROM JSON    
 INSERT INTO UserEducation    
        
SELECT @UserId,UniversitySchool,Major,Grade,FromDate,ToDate,GETDATE() FROM      
 OPENJSON ( @UserEducations )      
WITH (       
              UniversitySchool   nvarchar(250) '$.UniversitySchool',                  
              Major nvarchar(100)          '$.Major',      
     Grade nvarchar(10)          '$.Grade',     
     FromDate varchar(100)       '$.FromDate',    
     ToDate varchar(100)         '$.ToDate'    
 )     
    
 END    
    
  IF @UserExperiences IS NOT NULL    
 BEGIN    
 -- INSERT USER EXPERIENCE FROM JSON    
 INSERT INTO UserExperiences    
        
SELECT @UserId,OrganizationName,Designation,FromDate,ToDate,GETDATE() FROM      
 OPENJSON ( @UserExperiences )      
WITH (       
     OrganizationName   nvarchar(250) '$.OrganizationName',                  
              Designation nvarchar(250)          '$.Designation',    
     FromDate DATE      '$.FromDate',    
     ToDate DATE         '$.ToDate'    
 )     
    
 END    
    
IF @UserLanguages IS NOT NULL    
 BEGIN    
 -- INSERT INTO LANGUAGE MASTER FIRST    
    
    
DROP TABLE IF EXISTS #tempUserLangs    
    
create table #tempUserLangs    
(    
 Language nvarchar(250),    
 LanguageId int,    
 IsRead bit,     
 IsWrite bit,    
 IsSpeak bit    
)    
    
INSERT INTO #tempUserLangs    
        
SELECT Language,0,IsRead,IsWrite,IsSpeak FROM      
 OPENJSON ( @UserLanguages )      
WITH (       
     Language   nvarchar(250) '$.Language',                  
              IsRead bit  '$.IsRead',    
     IsWrite bit  '$.IsWrite',    
     IsSpeak bit  '$.IsSpeak'    
 )     
    
DECLARE @source NVARCHAR(max)    
 MERGE LanguageMaster AS TARGET    
  USING    
    (    
    SELECT DISTINCT Language    
      FROM    
      OPENJSON(@UserLanguages)    
      WITH (Language nvarchar(100) '$.Language')    
    ) AS source (Language)    
  ON TARGET.Name = source.Language    
  WHEN NOT MATCHED     
  THEN     
    INSERT (Name,createdBy, UpdatedBy)    
      VALUES (source.Language,@UserId,@UserId);    
    
UPDATE #tempUserLangs     
SET LanguageId=m.id FROM     
   languageMaster m inner JOIN #tempUserLangs     
   t ON m.Name= t.language     
    
    
INSERT INTO UserLanguages(UserId,    
      LanguageId,    
      IsRead,    
      IsSpeak,     
      IsWrite, CreatedOn)     
SELECT  @UserId,    
  LanguageId,    
  IsRead,    
  IsSpeak,    
  IsWrite, GETDATE() FROM  #tempUserLangs    
    
END    
    
IF @UserSocialMedias IS NOT NULL    
 BEGIN    
 INSERT INTO UserSocialMedia    
        
SELECT @UserId,SocialMediaId,URL,GETDATE() FROM      
 OPENJSON ( @UserSocialMedias )      
WITH (       
              SocialMediaId   INT '$.SocialMediaId',                  
              URL varchar(100) '$.URL'    
 )     
    
END    
    
select @UserId as Id    
    
END    
ELSE     
    
 SET @return=107    
    
 END    
    
 ELSE    
 SET @return= 108    
 RETURN @return    
END    
GO
/****** Object:  StoredProcedure [dbo].[CreateWhiteListingRequest]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec CreateWhiteListingRequest 'https://bitbucket.org/dashbosdvsd',2

CREATE PROCEDURE [dbo].[CreateWhiteListingRequest]   
 @URL NVARCHAR(1000),  
 @RequestedBy INT  
AS  
BEGIN  
Declare @return INT, @ID INT; 
IF NOT EXISTS (SELECT * FROM WhiteListingURLs WHERE URL=@URL)  
BEGIN   
  INSERT INTO WhiteListingURLs (URL,RequestedBy,RequestedOn)  
  VALUES (@URL,@RequestedBy,GETDATE())  
   
  -- exec CreateLogEntry @LogModuleId=1,@UserId=@CreatedBy,@ActionId=1,@ActionDetail=''  
  SET @ID = SCOPE_IDENTITY()

  SELECT 
  wu.Id,
(SELECT u.FirstName + ' '+ u.LastName from UserMaster u where u.Id = wu.RequestedBy) as RequestedBy,
(SELECT u1.FirstName + ' '+ u1.LastName from UserMaster u1 where u1.Id = wu.VerifiedBy) as VerifiedBy,
  wu.URL,
  wu.IsApproved,
  wu.RequestedOn,
  wu.VerifiedOn,
  wu.RejectedReason,
  wu.IsActive
FROM WhiteListingURLs wu WHERE URL = @URL

  SET @return =100 -- creation success  
END  
  
ELSE  
 BEGIN  
 
    SELECT
  wu.Id,
(SELECT u.FirstName + ' '+ u.LastName from UserMaster u where u.Id = wu.RequestedBy) as RequestedBy,
(SELECT u1.FirstName + ' '+ u1.LastName from UserMaster u1 where u1.Id = wu.VerifiedBy) as VerifiedBy,
  wu.URL,
  wu.IsApproved,
  wu.RequestedOn,
  wu.VerifiedOn,
  wu.RejectedReason,
  wu.IsActive
FROM WhiteListingURLs wu WHERE URL=@URL

  SET @return = 105 -- Record exists  
 END   
  
 return @return  
END  
  
GO
/****** Object:  StoredProcedure [dbo].[CreateWhiteListingRequestAfterCheck]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CreateWhiteListingRequestAfterCheck] 
	@URL NVARCHAR(1000),
	@RequestedBy INT,
	@VerifiedBy INT,
	@IsApproved BIT,
	@RequestedOn DATETIME,
	@VerifiedOn DATETIME
AS
BEGIN
Declare @return INT
IF NOT EXISTS (SELECT TOP 1 1 FROM WhiteListingURLs WHERE URL=@URL)
BEGIN	
		INSERT INTO WhiteListingURLs (URL,RequestedBy,VerifiedBy,IsApproved,RequestedOn,VerifiedOn)
		VALUES (@URL,@RequestedBy,@VerifiedBy,@IsApproved,GETDATE(),GETDATE())
		
		-- exec CreateLogEntry @LogModuleId=1,@UserId=@CreatedBy,@ActionId=1,@ActionDetail=''

		SET @return =100 -- creation success
END

ELSE
	BEGIN	
		SET @return = 105 -- Record exists
	END	
	IF EXISTS(SELECT TOP 1 1 FROM WhiteListingURLs WHERE URL=@URL)
	BEGIN
		SELECT [URL],RequestedBy,VerifiedBy,IsApproved,RequestedOn,VerifiedOn,Id,RejectedReason FROM WhiteListingURLs WHERE [URL] = @URL
	END
	return @return
END


GO
/****** Object:  StoredProcedure [dbo].[DeleteCategory]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
CREATE PROCEDURE [dbo].[DeleteCategory]   
(  
@Id INT  
)  
AS  
BEGIN   
Declare @return INT  
Declare @catCount INT  
  IF EXISTS (SELECT * FROM CategoryMaster  WHERE Id=@Id)  
  BEGIN  
   SET @catCount =   
   (SELECT COUNT(*) FROM CourseMaster  WHERE CategoryId=@Id)+  
   (SELECT COUNT(*) FROM ResourceMaster  WHERE CategoryId=@Id)+  
   (SELECT COUNT(*) FROM SubCategoryMaster  WHERE CategoryId=@Id)+  
   (SELECT COUNT(*) FROM QRCCategory  WHERE CategoryId=@Id) +  
   (SELECT COUNT(*) FROM QRCMasterCategory  WHERE CategoryId=@Id)+  
   (select count(X.id) from(SELECT id ,value FROM UserMaster       CROSS APPLY STRING_SPLIT(subjectsinterested, ','))X      where X.value=(select Name from CategoryMaster where Id=@Id));  
   IF(@catCount > 0)  
    SET @return = 121  
   ELSE  
   BEGIN  
    --DELETE FROM CategoryMaster  WHERE Id=@Id and Active=0;  
    update CategoryMaster set Status = 0 where Id=@Id  
     IF @@ROWCOUNT>0    
      SET @return =103 -- record DELETED    
     ELSE    
      SET @return = 121 -- trying active record deletion     
   END  
  END  
  
  ELSE  
  BEGIN  
   SET @return = 102 -- reconrd does not exists  
  END  
  SELECT cm.Id,  
    cm.Name,  
    cm.CreatedOn,      
    CONCAT(c.FirstName, '', c.LastName) as CreatedBy,  
    CONCAT(l.FirstName, '', l.LastName) as UpdatedBy,  
    cm.UpdatedOn, cm.Active  
   from CategoryMaster cm   
   inner join UserMaster c  on cm.CreatedBy= c.Id  
   inner join UserMaster l on cm.UpdatedBy =l.Id    
   where cm.Status = 1  
    order by cm.Id desc    
    RETURN @return  
    
END  
GO
/****** Object:  StoredProcedure [dbo].[DeleteCopyright]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
CREATE PROCEDURE [dbo].[DeleteCopyright]   
(  
@Id INT  
)  
AS  
BEGIN   
Declare @return INT  
Declare @catCount INT 
  IF EXISTS (SELECT * FROM CopyrightMaster  WHERE Id=@Id)  
  BEGIN  
 
    SET @catCount =   
   (SELECT COUNT(*) FROM CourseMaster  WHERE CopyRightId=@Id)+  
   (SELECT COUNT(*) FROM ResourceMaster  WHERE CopyRightId=@Id);  


   IF(@catCount > 0)  
    SET @return = 121  
   ELSE  
   BEGIN  
    --DELETE FROM CategoryMaster  WHERE Id=@Id and Active=0;  
    Update CopyrightMaster set Status = 0 where Id=@Id  
     IF @@ROWCOUNT>0    
      SET @return =103 -- record DELETED    
     ELSE    
      SET @return = 121 -- trying active record deletion     
   END  
END  
  
  ELSE  
  SET @return = 102 -- reconrd does not exists  
    
  SELECT cr.Id,  
    cr.Title,  
    Cr.[Description],  
    cr.CreatedOn,  
    CONCAT(c.FirstName, '', c.LastName) as CreatedBy,  
    CONCAT(l.FirstName, '', l.LastName) as UpdatedBy, cr.Active, cr.UpdatedOn  
   from CopyrightMaster cr   
   inner join UserMaster c  on cr.CreatedBy= c.Id  
   inner join UserMaster l on cr.UpdatedBy =l.Id where cr.Status = 1 order by cr.Id desc   
    RETURN @return  
    
END  
  
GO
/****** Object:  StoredProcedure [dbo].[DeleteCourse]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteCourse] 
(
@Id INT
)
AS
BEGIN	
Declare @return INT

IF EXISTS (SELECT * FROM CourseMaster  WHERE Id=@Id)
		BEGIN
		IF NOT EXISTS (select * from CourseApprovals where CourseId=@Id)

			BEGIN

			BEGIN TRY
				BEGIN TRANSACTION 
					Delete from CourseURLReferences WHERE CourseId=@Id

					DELETE FROM CourseAssociatedFiles WHERE CourseId=@Id

					DELETE FROM CourseApprovals WHERE CourseId=@Id

					DELETE FROM CourseComments WHERE CourseId=@Id

					DELETE FROM CourseEnrollment WHERE CourseId=@Id

					DELETE FROM CourseResourceMapping WHERE CourseId=@Id

					DELETE FROM UserCourseTests WHERE CourseId=@Id

					DELETE FROM AnswerOptions WHERE QuestionId IN (SELECT Id FROM Questions WHERE TestId IN 
					(SELECT Id FROM Tests WHERE CourseId=@Id))

					DELETE FROM Questions WHERE TestId IN 
					(SELECT Id FROM Tests WHERE CourseId=@Id)

					DELETE FROM Tests WHERE CourseId=@Id	
					
					DELETE FROM UserBookmarks WHERE ContentId = @Id AND ContentType = 1		

					DELETE FROM CourseMaster  WHERE Id=@Id;
				COMMIT
				SET @return =103 -- record DELETED	
			END TRY
			BEGIN CATCH
				IF @@TRANCOUNT > 0
					ROLLBACK TRAN
					SET @return =114 --record deletion failed
					
			END CATCH
				
					
			END	

			ELSE

			SET @return = 104 -- trying active record deletion			

		END

	    ELSE 

		SET @return = 102 -- reconrd does not exists	
			
	    RETURN @return
END



GO
/****** Object:  StoredProcedure [dbo].[DeleteCourseComment]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteCourseComment]		  
		   @Id Numeric(18,0),		
		   @RequestedBy INT
AS
BEGIN

DECLARE @result INT

IF EXISTS(SELECT * FROM CourseComments WHERE Id= @Id)

BEGIN

DELETE FROM CourseComments WHERE Id=@Id AND UserId=@RequestedBy
if @@ROWCOUNT >0

SET @result =103 -- delete success

else 

SET @result =114  -- delete failed

end

ELSE

SET @result =102 -- comment does not exists


RETURN @result

END

GO
/****** Object:  StoredProcedure [dbo].[DeleteEducation]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
CREATE PROCEDURE [dbo].[DeleteEducation]   
(  
@Id INT  
)  
AS  
BEGIN   
Declare @return INT 
Declare @catCount INT 
  IF EXISTS (SELECT * FROM EducationMaster  WHERE Id=@Id)  
    BEGIN  
 
    SET @catCount =   
   (SELECT COUNT(*) FROM CourseMaster  WHERE EducationId=@Id);  


   IF(@catCount > 0)  
    SET @return = 121  
   ELSE  
   BEGIN  
    Update EducationMaster set Status = 0 where Id=@Id  
     IF @@ROWCOUNT>0    
      SET @return =103 -- record DELETED    
     ELSE    
      SET @return = 121 -- trying active record deletion     
   END  
END  
  
  ELSE  
  SET @return = 102 -- reconrd does not exists  
    
    
  SELECT em.Id,  
    em.Name,  
    em.CreatedOn,      
    c.FirstName,  
     CONCAT(c.FirstName, '', c.LastName) as CreatedBy,  
    CONCAT(l.FirstName, '', l.LastName) as UpdatedBy,  
    em.UpdatedOn, em.Active  
   from EducationMaster em   
   inner join UserMaster c  on em.CreatedBy= c.Id  
   inner join UserMaster l on em.UpdatedBy =l.Id where Status=1 Order by em.Id desc   
  RETURN @return  
END  
  
GO
/****** Object:  StoredProcedure [dbo].[DeleteInstitution]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[DeleteInstitution] 
(
@Id	INT
)
AS
BEGIN	
Declare @return INT
		IF EXISTS (SELECT * FROM InstitutionMaster  WHERE Id=@Id)
		BEGIN
		
			DELETE FROM InstitutionMaster  WHERE Id=@Id and Active=0;
		
				IF @@ROWCOUNT>0		
		
					SET @return =103 -- record DELETED		
				ELSE		
					SET @return = 104 -- trying active record deletion			
		END

		ELSE
		SET @return = 102 -- reconrd does not exists
		
		SELECT	cm.Id,
				cm.Name,
				cm.CreatedOn,				
			 CONCAT(c.FirstName, '', c.LastName) as CreatedBy,
				CONCAT(l.FirstName, '', l.LastName) as UpdatedBy,
				cm.UpdatedOn, cm.Active
		 from InstitutionMaster cm 
		 inner join UserMaster c  on cm.CreatedBy= c.Id
		 inner join UserMaster l on cm.UpdatedBy =l.Id 	
		  order by cm.Id desc 	
		  RETURN @return
		
END

GO
/****** Object:  StoredProcedure [dbo].[DeleteMaterialType]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
CREATE PROCEDURE [dbo].[DeleteMaterialType]   
(  
@Id INT  
)  
AS  
BEGIN   
Declare @return INT  
Declare @catCount INT 
  IF EXISTS (SELECT * FROM MaterialTypeMaster  WHERE Id=@Id)  
     BEGIN  
 
    SET @catCount =   
   (SELECT COUNT(*) FROM ResourceMaster  WHERE MaterialTypeId=@Id);  


   IF(@catCount > 0)  
    SET @return = 121  
   ELSE  
   BEGIN  
    Update MaterialTypeMaster set Status = 0 where Id=@Id  
     IF @@ROWCOUNT>0    
      SET @return =103 -- record DELETED    
     ELSE    
      SET @return = 121 -- trying active record deletion     
   END  
END  
  
  ELSE  
  SET @return = 102 -- reconrd does not exists  
  SELECT cm.Id,  
    cm.Name,  
    cm.CreatedOn,      
    CONCAT(c.FirstName, '', c.LastName) as CreatedBy,  
    CONCAT(l.FirstName, '', l.LastName) as UpdatedBy,  
    cm.UpdatedOn, cm.Active  
   from MaterialTypeMaster cm   
   inner join UserMaster c  on cm.CreatedBy= c.Id  
   inner join UserMaster l on cm.UpdatedBy =l.Id    
   where cm.Status=1  
    order by cm.Id desc    
    RETURN @return  
    
END  
  
GO
/****** Object:  StoredProcedure [dbo].[DeleteProfession]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
CREATE PROCEDURE [dbo].[DeleteProfession]   
(  
@Id INT  
)  
AS  
BEGIN   
Declare @return INT  
Declare @catCount INT 
  IF EXISTS (SELECT * FROM ProfessionMaster  WHERE Id=@Id)  
  BEGIN   
    SET @catCount =   
   (SELECT COUNT(*) FROM CourseMaster  WHERE ProfessionId=@Id);  


   IF(@catCount > 0)  
    SET @return = 121  
   ELSE  
   BEGIN  
    Update ProfessionMaster set Status = 0 where Id=@Id  
     IF @@ROWCOUNT>0    
      SET @return =103 -- record DELETED    
     ELSE    
      SET @return = 121 -- trying active record deletion     
   END  
END  
  
  ELSE  
  SET @return = 102 -- reconrd does not exists    
    
  SELECT cm.Id,  
    cm.Name,  
    cm.CreatedOn,      
    CONCAT(c.FirstName, '', c.LastName) as CreatedBy,  
    CONCAT(l.FirstName, '', l.LastName) as UpdatedBy,  
    cm.UpdatedOn, cm.Active  
   from ProfessionMaster cm   
   inner join UserMaster c  on cm.CreatedBy= c.Id  
   inner join UserMaster l on cm.UpdatedBy =l.Id    
   where cm.Status=1  
   order by cm.Id desc   
    RETURN @return  
    
END  
  
GO
/****** Object:  StoredProcedure [dbo].[DeleteQRC]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[DeleteQRC] 
(
@Id	INT
)
AS
BEGIN	
Declare @return INT
		IF EXISTS (SELECT * FROM QRCMaster  WHERE Id=@Id)
		BEGIN
		
			DELETE FROM QRCMaster  WHERE Id=@Id and Active=0;
		
				IF @@ROWCOUNT>0		
		
					SET @return =103 -- record DELETED		
				ELSE		
					SET @return = 104 -- trying active record deletion			
		END

		ELSE
		SET @return = 102 -- reconrd does not exists
		
		SELECT	cm.Id,
				cm.Name,Description,
				cm.CreatedOn,				
			 CONCAT(c.FirstName, '', c.LastName) as CreatedBy,
				CONCAT(l.FirstName, '', l.LastName) as UpdatedBy,
				cm.UpdatedOn, cm.Active
		 from QRCMaster cm 
		 inner join UserMaster c  on cm.CreatedBy= c.Id
		 inner join UserMaster l on cm.UpdatedBy =l.Id 	
		  order by cm.Id desc 	
		  RETURN @return
		
END

GO
/****** Object:  StoredProcedure [dbo].[DeleteResource]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

 CREATE PROCEDURE [dbo].[DeleteResource]
(
@Id INT
)
AS
BEGIN	
Declare @return INT

IF EXISTS (SELECT * FROM ResourceMaster  WHERE Id=@Id)
		BEGIN
		--IF NOT EXISTS (select * from ResourceApprovals where ResourceId=@Id)

		--	BEGIN

			BEGIN TRY
				BEGIN TRANSACTION 
					Delete from ResourceURLReferences WHERE ResourceId=@Id

					DELETE FROM ResourceAssociatedFiles WHERE ResourceId=@Id

					Delete from resourceapprovals WHERE ResourceId=@Id

					Delete from resourcecomments WHERE ResourceId=@Id

					DELETE FROM UserBookMarks WHERE ContentId = @Id AND ContentType = 2

					UPDATE CourseResourceMapping SET ResourcesId = NULL WHERE ResourcesId=@Id;

					UPDATE SectionResource SET ResourceId = NULL WHERE ResourceId=@Id;

					DELETE FROM ResourceMaster  WHERE Id=@Id;
				COMMIT
				SET @return =103 -- record DELETED	
			END TRY
			BEGIN CATCH
				IF @@TRANCOUNT > 0
					ROLLBACK TRAN
					SET @return =120 --record deletion failed
					
			END CATCH
			
					
			--END	

			--ELSE

			--SET @return = 104 -- trying active record deletion			

		END

	    ELSE 

		SET @return = 102 -- reconrd does not exists	
		PRINT @return
	    RETURN @return
END
GO
/****** Object:  StoredProcedure [dbo].[DeleteResourceComment]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteResourceComment]		  
		   @Id Numeric(18,0),		
		   @RequestedBy INT
AS
BEGIN

DECLARE @result INT

IF EXISTS(SELECT * FROM ResourceComments WHERE Id= @Id)

BEGIN
SET NOCOUNT off 
DELETE FROM ResourceComments WHERE Id=@Id AND UserId=@RequestedBy
if @@ROWCOUNT >0

SET @result =103 -- delete success

else 

SET @result =114  -- delete failed

end

ELSE

SET @result =102 -- comment does not exists


RETURN @result

END

GO
/****** Object:  StoredProcedure [dbo].[DeleteStream]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[DeleteStream] 
(
@Id	INT
)
AS
BEGIN	
Declare @return INT
		IF EXISTS (SELECT * FROM StreamMaster  WHERE Id=@Id)
		BEGIN
		
			DELETE FROM StreamMaster  WHERE Id=@Id and Active=0;
		
				IF @@ROWCOUNT>0		
		
					SET @return =103 -- record DELETED		
				ELSE		
					SET @return = 104 -- trying active record deletion			
		END

		ELSE
		SET @return = 102 -- reconrd does not exists
		
		SELECT	cm.Id,
				cm.Name,
				cm.CreatedOn,				
			 CONCAT(c.FirstName, '', c.LastName) as CreatedBy,
				CONCAT(l.FirstName, '', l.LastName) as UpdatedBy,
				cm.UpdatedOn, cm.Active
		 from StreamMaster cm 
		 inner join UserMaster c  on cm.CreatedBy= c.Id
		 inner join UserMaster l on cm.UpdatedBy =l.Id 	
		  order by cm.Id desc 	
		  RETURN @return
		
END

GO
/****** Object:  StoredProcedure [dbo].[DeleteSubCategory]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
CREATE PROCEDURE [dbo].[DeleteSubCategory]   
(  
@Id INT  
)  
AS  
BEGIN   
Declare @return INT  
Declare @catCount INT  
  IF EXISTS (SELECT * FROM SubCategoryMaster  WHERE Id=@Id)  
  BEGIN  
    SET @catCount =   
   (SELECT COUNT(*) FROM CourseMaster  WHERE SubCategoryId=@Id)+  
   (SELECT COUNT(*) FROM ResourceMaster  WHERE SubCategoryId=@Id);  


   IF(@catCount > 0)  
    SET @return = 121  
   ELSE  
   BEGIN  
    --DELETE FROM CategoryMaster  WHERE Id=@Id and Active=0;  
    Update SubCategoryMaster set Status = 0 where Id=@Id  
     IF @@ROWCOUNT>0    
      SET @return =103 -- record DELETED    
     ELSE    
      SET @return = 121 -- trying active record deletion     
   END  
END  
  
  ELSE  
  SET @return = 102 -- reconrd does not exists  
    
  SELECT sm.Id,  
    sm.Name,  
    cm.Name as CategoryName,   
    cm.Id as CategoryId,  
    sm.CreatedOn,      
       CONCAT(c.FirstName, '', c.LastName) as CreatedBy,  
    CONCAT(l.FirstName, '', l.LastName) as UpdatedBy,  
    sm.UpdatedOn,   
    sm.Active  
   from SubCategoryMaster sm   
   INNER JOIN CategoryMaster cm on sm.CategoryId=cm.Id  
   inner join UserMaster c  on sm.CreatedBy= c.Id  
   inner join UserMaster l on sm.UpdatedBy =l.Id where sm.Status=1 order by cm.Id desc    
   RETURN @return  
END  
  
GO
/****** Object:  StoredProcedure [dbo].[GetCategory]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
CREATE PROCEDURE [dbo].[GetCategory]    
AS  
BEGIN    
 IF Exists(select * from CategoryMaster)   
 BEGIN   
  SELECT cm.Id,  
    cm.Name,  
    cm.Name_Ar,  
    cm.CreatedOn,      
    CONCAT(c.FirstName, '', c.LastName) as CreatedBy,  
    CONCAT(l.FirstName, '', l.LastName) as UpdatedBy,  
    cm.UpdatedOn, cm.Active  
   from CategoryMaster cm   
   inner join UserMaster c  on cm.CreatedBy= c.Id  
   inner join UserMaster l on cm.UpdatedBy =l.Id    
   where cm.Status = 1 --and c.Active = 1 and l.Active = 1 -- added Active check
   order by cm.Id desc   
     
  RETURN 105 -- record exists  
 end  
  ELSE  
  RETURN 102 -- reconrd does not exists  
END  
  
GO
/****** Object:  StoredProcedure [dbo].[GetCategoryById]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetCategoryById] 
(
@Id	INT
)
AS
BEGIN	

IF Exists(select * from CategoryMaster WHERE Id=@Id)	
	BEGIN	
		SELECT	cm.Id,
				cm.Name,
				cm.Name_Ar,
				cm.CreatedOn,				
			 CONCAT(c.FirstName, '', c.LastName) as CreatedBy,
				CONCAT(l.FirstName, '', l.LastName) as UpdatedBy,
				cm.UpdatedOn, cm.Active
		 from CategoryMaster cm 
		 inner join UserMaster c  on cm.CreatedBy= c.Id
		 inner join UserMaster l on cm.UpdatedBy =l.Id 	
		 WHERE cm.Id=@Id order by cm.Id desc 
		 
		return 105 -- record exists
	end
		ELSE
		return 102 -- reconrd does not exists
END

GO
/****** Object:  StoredProcedure [dbo].[GetConfigurationsByType]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetConfigurationsByType]
	@ConfigType NVARCHAR(50)
AS
BEGIN

	SELECT [key]
      ,[value]     
  FROM [OerConfig] WHERE ConfigType=@ConfigType
	
	IF @@ROWCOUNT>0

		RETURN 105 -- exists

	ELSE
	
		RETURN 102 -- record does not exists
END
GO
/****** Object:  StoredProcedure [dbo].[GetCopyright]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
CREATE PROCEDURE [dbo].[GetCopyright]        
AS      
BEGIN        
  IF Exists(select * from CopyrightMaster)      
  BEGIN       
  SELECT cr.Id,      
    cr.Title,      
    Cr.Description,      
    cr.Title_Ar,      
    Cr.Description_Ar,      
    cr.CreatedOn,      
    CONCAT(c.FirstName, '', c.LastName) as CreatedBy,      
    CONCAT(l.FirstName, '', l.LastName) as UpdatedBy, cr.Active, cr.UpdatedOn, cr.Media ,cr.Protected ,  
 cr.IsResourceProtect  
   from CopyrightMaster cr       
   inner join UserMaster c  on cr.CreatedBy= c.Id      
   inner join UserMaster l on cr.UpdatedBy =l.Id        
   where cr.Status = 1 --and c.Active = 1 and l.Active = 1 -- added active check 
   order by cr.Id desc      
      
  RETURN 105 -- record exists      
  END      
  ELSE      
  RETURN 102 -- record does not exists      
END      
GO
/****** Object:  StoredProcedure [dbo].[GetCopyrightById]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetCopyrightById]     
(    
@Id INT    
)    
AS    
BEGIN      
      
  IF Exists(select * from CopyrightMaster  WHERE Id=@Id)    
   BEGIN     
  SELECT cr.Id,    
    cr.Title,    
    Cr.Description,    
    cr.Title_Ar,    
    Cr.Description_Ar,    
    cr.CreatedOn,    
    CONCAT(c.FirstName, '', c.LastName) as CreatedBy,    
    CONCAT(l.FirstName, '', l.LastName) as UpdatedBy, cr.Active, cr.UpdatedOn,cr.Media,cr.Protected ,	cr.IsResourceProtect 
   from CopyrightMaster cr     
   inner join UserMaster c  on cr.CreatedBy= c.Id    
   inner join UserMaster l on cr.UpdatedBy =l.Id      
   WHERE cr.Id=@Id order by cr.Id desc    
    
  return 105 -- record exists    
  END    
  ELSE    
  return 102 -- record does not exists    
END    
GO
/****** Object:  StoredProcedure [dbo].[GetCourseById]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetCourseById]                     
(                    
@Id INT                    
)                    
AS                    
BEGIN                  
                
DECLARE @SharedCount INT               
DECLARE @DownloadCount INT             
DECLARE @VisitCount INT    
DECLARE @IsApproved BIT    
                
IF Exists(select TOP 1 1 from CourseMaster WHERE Id=@Id)                     
 BEGIN                    
                 
 SET @SharedCount = (select count(Id) from ContentSharedInfo WHERE ContentId = @Id AND ContentTypeId = 1)                
 SET @DownloadCount = (select count(Id) from ContentDownloadInfo WHERE ContentId = @Id AND ContentTypeId = 1)                
          
          
  SELECT @VisitCount = ViewCount,@IsApproved = IsApproved FROM CourseMaster WHERE ID = @Id          
           
 IF(@VisitCount IS NULL AND @IsApproved = 1)          
 BEGIN          
 Update CourseMaster SET ViewCount =  1 WHERE ID = @Id           
          
 END          
 ELSE IF(@IsApproved = 1)          
 BEGIN          
  Update CourseMaster SET ViewCount = @VisitCount+ 1 WHERE ID = @Id           
 END;          
          
          
 SELECT r.Id                    
      ,r.Title                    
      ,r.CategoryId, c.Name as CategoryName ,c.Name_Ar as CategoryName_Ar                   
      ,SubCategoryId, sc.Name as SubCategoryName  ,sc.Name_Ar as SubCategoryName_Ar                  
      ,Thumbnail                    
      ,CourseDescription                    
      ,Keywords                    
      ,CourseContent                         
      ,CopyRightId, cm.Title as CopyrightTitle, cm.Title_Ar as CopyrightTitle_Ar, cm.Description as CopyrightDescription,
	  cm.Description_Ar AS CopyrightDescription_Ar,cm.Media,cm.Protected           
      ,IsDraft                    
   , EducationId, edm.Name as EducationName ,edm.Name_Ar as EducationName_Ar              
   , ProfessionId, pm.Name as ProfessionName ,pm.Name_Ar as ProfessionName_Ar                   
      , um.FirstName+ ' '+ um.LastName as CreatedBy, um.Id as CreatedById                    
      ,r.CreatedOn                    
      ,IsApproved                    
      ,Rating                         
      ,ReportAbuseCount,ViewCount,AverageReadingTime,@DownloadCount AS DownloadCount,ReadingTime  ,                
   @SharedCount as SharedCount,              
   LastView,              
   r.LevelId,           
   ViewCount,          
   (SELECT [Level] FROM lu_Level WHERE Id = r.LevelId) AS [LevelName],              
   (SELECT Level_Ar FROM lu_Level WHERE Id = r.LevelId) AS [Level_Ar],              
   r.EducationalStandardId,              
   (SELECT [Standard] FROM lu_educational_standard WHERE Id = r.EducationalStandardId) AS [Standard],              
   (SELECT [Standard_Ar] FROM lu_educational_standard WHERE Id = r.EducationalStandardId) AS [Standard_Ar],              
   r.EducationalUseId,              
    (SELECT [EducationalUse] FROM lu_educational_use WHERE Id = r.EducationalUseId) AS [EducationalUseName],              
   (SELECT [EducationalUse_Ar] FROM lu_educational_use WHERE Id = r.EducationalUseId) AS [EducationalUse_Ar] ,    
   CommunityBadge,    
   MoEBadge             
  FROM CourseMaster r                    
   inner join CategoryMaster c on r.CategoryId = c.Id                       
   left join SubCategoryMaster sc on r.SubCategoryId=sc.Id                    
   left join CopyrightMaster cm on r.CopyRightId = cm.Id                    
   left join EducationMaster edm on edm.Id= r.EducationId                    
   left join ProfessionMaster pm on pm.Id= r.ProfessionId                    
   inner join UserMaster um on r.CreatedBy =um.Id where r.Id=@Id Order by r.Id desc                     
                    
                    
   Update CourseMaster SET LastView = GETDATE() WHERE ID = @Id            
                    
              
          
          
SELECT caf.Id                    
      ,CourseId                    
      ,AssociatedFile                    
      ,caf.CreatedOn  ,        
  caf.FileName       
  FROM CourseAssociatedFiles caf INNER JOIN CourseMaster cm ON cm.Id=caf.CourseId  AND cm.Id=@Id               
  WHERE IsInclude = 1      
                    
                    
                    
  SELECT cur.Id                    
      ,CourseId                    
      ,cur.URLReferenceId,uwl.URL as URLReference           
      ,cur.CreatedOn ,
	  uwl.IsActive,
	  uwl.IsApproved
  FROM dbo.CourseURLReferences cur INNER JOIN CourseMaster cm ON cm.Id=cur.CourseId                    
           inner join WhiteListingURLs uwl on uwl.Id=cur.URLReferenceId AND cm.Id=@Id      
     --AND uwl.IsApproved = 1                     
           
                    
SELECT cc.Id                    
      ,[CourseId]                    
      ,[Comments]                    
      ,um.FirstName+' '+ um.LastName as CommentedBy, um.Id As CommentedById, um.Photo as CommentorImage                    
      ,[CommentDate]                    
      ,[ReportAbuseCount]                    
  FROM [dbo].[CourseComments] cc inner join UserMaster um on cc.UserId=um.Id AND cc.CourseId=@Id where cc.IsHidden=0                    
                      
                  
 SELECT * from Coursesections WHERE CourseId = @Id                    
                    
--SELECT rm.id,rm.title,sr.SectionId FROM SectionResource sr                    
--INNER JOIN ResourceMaster rm                    
--ON sr.resourceid = rm.id                    
                    
--WHERE sr.SectionId in (SELECT ID from Coursesections WHERE CourseId = @Id)               
            
               
  SELECT r.Id                    
      ,r.Title   ,            
   sr.SectionId             
      ,r.CategoryId, c.Name as CategoryName ,c.Name_Ar as CategoryName_Ar                   
      ,SubCategoryId, sc.Name as SubCategoryName   ,sc.Name_Ar as SubCategoryName_Ar                   
      ,Thumbnail                    
      ,ResourceDescription                    
      ,Keywords                    
      ,ResourceContent                    
      ,MaterialTypeId, mt.Name as MaterialTypeName                    
      ,CopyRightId, cm.Title as CopyrightTitle, cm.Description as CopyrightDescription,cm.Description_Ar AS CopyrightDescription_Ar                    
      ,IsDraft                    
      ,um.FirstName+ ' ' + um.LastName as CreatedBy, um.Id as CreatedById                    
      ,r.CreatedOn                    
      ,IsApproved                    
      ,Rating                    
      ,AlignmentRating                    
      ,ReportAbuseCount,ViewCount,AverageReadingTime,@DownloadCount AS DownloadCount,r.ReadingTime  ,                  
   es.Standard,                  
 eu.EducationalUse,                  
 el.Level,                  
 Objective,                 
 @SharedCount as SharedCount,                
 LastView,                
 [Format]                      
  FROM             
  SectionResource sr                    
INNER JOIN ResourceMaster r                   
ON sr.resourceid = r.id                       
   inner join CategoryMaster c on r.CategoryId = c.Id                    
   inner join MaterialTypeMaster mt on r.MaterialTypeId=mt.Id                    
   left join SubCategoryMaster sc on r.SubCategoryId=sc.Id                    
   left join CopyrightMaster cm on r.CopyRightId = cm.Id                    
    LEFT JOIN lu_Educational_Standard es ON r.EducationalStandardId = es.Id                  
   LEFT JOIN lu_Educational_Use eu ON r.EducationalUseId = eu.Id                  
   LEFT JOIN lu_Level el ON r.LevelId = el.Id                  
                  
   inner join UserMaster um on r.CreatedBy =um.Id             
  where sr.SectionId in (SELECT ID from Coursesections WHERE CourseId = @Id)               
       
 SELECT Rating,COUNT(RatedBy) As NoOfUsers      
from [CourseRating] where courseid = @Id      
GROUP BY Rating      
    
SELECT r.Rating,COUNT(r.RatedBy) As NoOfUsers  ,r.ResourceId    
from [resourcerating] r where r.resourceid in( select resourceid from SectionResource where sectionid in (SELECT ID from Coursesections WHERE CourseId = @Id))    
GROUP BY Rating  ,r.ResourceId    
--select * from resourcerating     
          
 return 105 -- record exists                    
                    
 end                    
  ELSE                    
 return 102 -- reconrd does not exists                    
END               

GO
/****** Object:  StoredProcedure [dbo].[GetCourses]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
CREATE PROCEDURE [dbo].[GetCourses]     
(
    @PageNo int = 1,
	@PageSize int = 5
)
AS    
BEGIN    
 
	 Declare @return INT
declare @start int, @end int
set @start = (@PageNo - 1) * @PageSize + 1
set	@end = @PageNo * @PageSize

	;with sqlpaging as (
	SELECT Rownumber = ROW_NUMBER() OVER(ORDER BY r.Id desc), r.Id    
      ,r.Title    
      ,r.CategoryId, c.Name as CategoryName    
      ,SubCategoryId, sc.Name as SubCategoryName    
      ,Thumbnail    
      ,CourseDescription    
      ,Keywords    
      ,CourseContent         
      ,CopyRightId, cm.Title as CopyrightTitle, cm.Description as CopyrightDescription    
      ,IsDraft    
   , EducationId, edm.Name as EducationName    
   , ProfessionId, pm.Name as ProfessionName    
      , CONCAT(um.FirstName, '', um.LastName) as CreatedBy, um.Id as CreatedById    
      ,r.CreatedOn    
      ,IsApproved    
      ,Rating         
      ,ReportAbuseCount,ViewCount,AverageReadingTime,(SELECT COUNT(*) FROM ContentSharedInfo WHERE ContentId = r.Id AND ContentTypeId = 1) As SharedCount,    
   r.ReadingTime  ,
   (SELECT COUNT(*) FROM courseenrollment WHERE CourseId = r.Id) AS EnrollmentCount,
   (SELECT COUNT(*) FROM ContentDownloadInfo WHERE ContentId = r.Id AND ContentTypeId = 1) AS DownloadCount,
   CommunityBadge,
   MoEBadge
  FROM CourseMaster r    
   inner join CategoryMaster c on r.CategoryId = c.Id       
   left join SubCategoryMaster sc on r.SubCategoryId=sc.Id    
   left join CopyrightMaster cm on r.CopyRightId = cm.Id    
   left join EducationMaster edm on edm.Id= r.EducationId    
   left join ProfessionMaster pm on pm.Id= r.ProfessionId    
   inner join UserMaster um on r.CreatedBy =um.Id 
   	
			)
			select
 top (@PageSize) *,
 (select max(rownumber) from sqlpaging) as 
 Totalrows
from sqlpaging
where Rownumber between @start and @end
Order by Id desc     
    
    
SELECT caf.Id    
      ,CourseId    
      ,AssociatedFile    
      ,caf.CreatedOn    
  FROM CourseAssociatedFiles caf INNER JOIN CourseMaster cm ON cm.Id=caf.CourseId     
    
  SELECT cur.Id    
      ,CourseId    
      ,cur.URLReferenceId,uwl.URL as URLReference    
      ,cur.CreatedOn    
  FROM dbo.CourseURLReferences cur INNER JOIN CourseMaster cm ON cm.Id=cur.CourseId    
           inner join WhiteListingURLs uwl on uwl.Id=cur.URLReferenceId    
    
    
 SELECT cc.Id    
      ,[CourseId]    
      ,[Comments]    
      ,CONCAT(um.FirstName, ' ', um.LastName) as CommentedBy, um.Id As CommentedById, um.Photo as CommentorImage    
      ,[CommentDate]    
      ,[ReportAbuseCount]    
  FROM [dbo].[CourseComments] cc inner join UserMaster um on cc.UserId=um.Id where cc.IsHidden=0    
       
 --IF @@ROWCOUNT>0    
        
 return 105 -- record exists    
    
-- ELSE    
--  return 102 -- reconrd does not exists    
END 
GO
/****** Object:  StoredProcedure [dbo].[GetEducation]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
CREATE PROCEDURE [dbo].[GetEducation]    
AS  
BEGIN    
 IF Exists(select * from EducationMaster)   
 BEGIN   
  SELECT em.Id,  
    em.Name,  
    em.CreatedOn,      
    c.FirstName,  
     CONCAT(c.FirstName, '', c.LastName) as CreatedBy,  
    CONCAT(l.FirstName, '', l.LastName) as UpdatedBy,  
    em.UpdatedOn, em.Active,  
    em.Name_Ar  
   from EducationMaster em   
   inner join UserMaster c  on em.CreatedBy= c.Id  
   inner join UserMaster l on em.UpdatedBy =l.Id where em.Status=1
   Order by em.Id desc   
   
  RETURN 105 -- record exists  
 end  
  ELSE  
  RETURN 102 -- reconrd does not exists  
END  
GO
/****** Object:  StoredProcedure [dbo].[GetEducationById]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetEducationById] 
(
@Id	INT
)
AS
BEGIN	
IF Exists(select * from EducationMaster WHERE Id=@Id)	
	BEGIN	
		SELECT	em.Id,
				em.Name,
				em.CreatedOn,				
				c.FirstName,
				 CONCAT(c.FirstName, '', c.LastName) as CreatedBy,
				CONCAT(l.FirstName, '', l.LastName) as UpdatedBy,
				em.UpdatedOn, em.Active
		 from EducationMaster em 
		 inner join UserMaster c  on em.CreatedBy= c.Id
		 inner join UserMaster l on em.UpdatedBy =l.Id where em.Id=@Id	Order by em.Id desc 
	 		 
		return 105 -- record exists
	end
		ELSE
		return 102 -- reconrd does not exists
END

GO
/****** Object:  StoredProcedure [dbo].[GetInstitution]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
CREATE PROCEDURE [dbo].[GetInstitution]      
AS    
BEGIN      
 IF Exists(select top 1 1 from InstitutionMaster)     
 BEGIN     
  SELECT em.Id,    
    em.Name,    
 em.Name_Ar,    
    em.CreatedOn,        
    c.FirstName,    
     CONCAT(c.FirstName, '', c.LastName) as CreatedBy,    
    CONCAT(l.FirstName, '', l.LastName) as UpdatedBy,    
    em.UpdatedOn, em.Active    
   from InstitutionMaster em     
   inner join UserMaster c  on em.CreatedBy= c.Id    
   inner join UserMaster l on em.UpdatedBy =l.Id 
   WHERE em.Active = 1
   Order by em.Id desc     
      
  RETURN 105 -- record exists    
 end    
  ELSE    
  RETURN 102 -- reconrd does not exists    
END    
GO
/****** Object:  StoredProcedure [dbo].[GetInstitutionById]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetInstitutionById]  
(  
@Id INT  
)  
AS  
BEGIN   
IF Exists(select * from InstitutionMaster WHERE Id=@Id)   
 BEGIN   
  SELECT em.Id,  
    em.Name,  
	  em.Name_Ar,  
    em.CreatedOn,      
    c.FirstName,  
     CONCAT(c.FirstName, '', c.LastName) as CreatedBy,  
    CONCAT(l.FirstName, '', l.LastName) as UpdatedBy,  
    em.UpdatedOn, em.Active  
   from InstitutionMaster em   
   inner join UserMaster c  on em.CreatedBy= c.Id  
   inner join UserMaster l on em.UpdatedBy =l.Id where em.Id=@Id Order by em.Id desc   
       
  return 105 -- record exists  
 end  
  ELSE  
  return 102 -- reconrd does not exists  
END 
GO
/****** Object:  StoredProcedure [dbo].[GetMaterialType]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
CREATE PROCEDURE [dbo].[GetMaterialType]    
AS  
BEGIN    
 IF Exists(select top 1 1 from MaterialTypeMaster)   
 BEGIN   
  SELECT cm.Id,  
    cm.Name,  
    cm.Name_Ar,  
    cm.CreatedOn,      
    CONCAT(c.FirstName, '', c.LastName) as CreatedBy,  
    CONCAT(l.FirstName, '', l.LastName) as UpdatedBy,  
    cm.UpdatedOn, cm.Active  
   from MaterialTypeMaster cm   
   inner join UserMaster c  on cm.CreatedBy= c.Id  
   inner join UserMaster l on cm.UpdatedBy =l.Id    
   where cm.Status =1 --and l.Active = 1 --Added active check  
   order by cm.Id desc   
     
  RETURN 105 -- record exists  
 end  
  ELSE  
  RETURN 102 -- reconrd does not exists  
END  
  
GO
/****** Object:  StoredProcedure [dbo].[GetMaterialTypeById]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetMaterialTypeById] 
(
@Id	INT
)
AS
BEGIN	

IF Exists(select * from MaterialTypeMaster WHERE Id=@Id)	
	BEGIN	
		SELECT	cm.Id,
				cm.Name,
				cm.Name_Ar,
				cm.CreatedOn,				
			 CONCAT(c.FirstName, '', c.LastName) as CreatedBy,
				CONCAT(l.FirstName, '', l.LastName) as UpdatedBy,
				cm.UpdatedOn, cm.Active
		 from MaterialTypeMaster cm 
		 inner join UserMaster c  on cm.CreatedBy= c.Id
		 inner join UserMaster l on cm.UpdatedBy =l.Id 	
		 WHERE cm.Id=@Id order by cm.Id desc 
		 
		return 105 -- record exists
	end
		ELSE
		return 102 -- reconrd does not exists
END

GO
/****** Object:  StoredProcedure [dbo].[GetProfession]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
CREATE PROCEDURE [dbo].[GetProfession]    
AS  
BEGIN    
 IF Exists(select * from ProfessionMaster where Status=1)   
 BEGIN   
  SELECT cm.Id,  
    cm.Name,  
    cm.Name_Ar,  
    cm.CreatedOn,      
    CONCAT(c.FirstName, '', c.LastName) as CreatedBy,  
    CONCAT(l.FirstName, '', l.LastName) as UpdatedBy,  
    cm.UpdatedOn, cm.Active  
   from ProfessionMaster cm   
   inner join UserMaster c  on cm.CreatedBy= c.Id  
   inner join UserMaster l on cm.UpdatedBy =l.Id   
   where cm.Status=1   
   order by cm.Id desc   
     
  RETURN 105 -- record exists  
 end  
  ELSE  
  RETURN 102 -- reconrd does not exists  
END  
GO
/****** Object:  StoredProcedure [dbo].[GetProfessionById]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetProfessionById] 
(
@Id	INT
)
AS
BEGIN	

IF Exists(select * from ProfessionMaster WHERE Id=@Id)	
	BEGIN	
		SELECT	cm.Id,
				cm.Name,
				cm.Name_Ar,
				cm.CreatedOn,				
			 CONCAT(c.FirstName, '', c.LastName) as CreatedBy,
				CONCAT(l.FirstName, '', l.LastName) as UpdatedBy,
				cm.UpdatedOn, cm.Active
		 from ProfessionMaster cm 
		 inner join UserMaster c  on cm.CreatedBy= c.Id
		 inner join UserMaster l on cm.UpdatedBy =l.Id 	
		 WHERE cm.Id=@Id order by cm.Id desc 
		 
		return 105 -- record exists
	end
		ELSE
		return 102 -- reconrd does not exists
END
GO
/****** Object:  StoredProcedure [dbo].[GetProfileAppData]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetProfileAppData] 	
AS
BEGIN		
	
SELECT Id,[Name],[Name_Ar]  FROM TitleMaster where IsActive=1

SELECT Id ,Code ,[Name],[Name_Ar]  FROM dbo.CountryMaster where IsActive=1

SELECT Id ,CountryId, Trim(Rtrim(Name)) as Name,[Name_Ar]  FROM dbo.StateMaster where IsActive=1

Select Id, Name,Name_Ar FROM SocialMediaMaster 

IF @@ROWCOUNT>0 
RETURN 105

END
GO
/****** Object:  StoredProcedure [dbo].[GetQRC]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
CREATE PROCEDURE [dbo].[GetQRC]    
AS  
BEGIN    
 IF Exists(select * from QRCMaster)   
 BEGIN   
  SELECT cm.Id,  
    cm.Name,Description,  
    cm.CreatedOn,      
    CONCAT(c.FirstName, '', c.LastName) as CreatedBy,  
    CONCAT(l.FirstName, '', l.LastName) as UpdatedBy,  
    cm.UpdatedOn, cm.Active  
   from QRCMaster cm   
   inner join UserMaster c  on cm.CreatedBy= c.Id  
   inner join UserMaster l on cm.UpdatedBy =l.Id  
   WHERE cm.Active = 1
   order by cm.Id desc   
     
  RETURN 105 -- record exists  
 end  
  ELSE  
  RETURN 102 -- reconrd does not exists  
END  
  
GO
/****** Object:  StoredProcedure [dbo].[GetQRCbyCategoryId]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 -- exec GetQRCbyCategoryId 1    
CREATE PROCEDURE [dbo].[GetQRCbyCategoryId]      
(    
@Id INT    
)    
AS      
BEGIN        
 IF Exists(select top 1 1 from QRCMaster where ID in (SELECT QRCId from QRCCategory WHERE CategoryID = @Id))       
 BEGIN       
  SELECT cm.Id,      
    cm.Name,Description,      
    cm.CreatedOn,          
    CONCAT(c.FirstName, '', c.LastName) as CreatedBy,      
    CONCAT(l.FirstName, '', l.LastName) as UpdatedBy,      
    cm.UpdatedOn, cm.Active      
   from QRCMaster cm       
   inner join UserMaster c  on cm.CreatedBy= c.Id      
   inner join UserMaster l on cm.UpdatedBy =l.Id      
   WHERE cm.Active = 1    
   AND cm.Id in (SELECT QRCId from QRCCategory WHERE CategoryID = @Id)    
   order by cm.Id desc       
         
  RETURN 105 -- record exists      
 end      
  ELSE      
  RETURN 102 -- reconrd does not exists      
END      
GO
/****** Object:  StoredProcedure [dbo].[GetQRCById]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetQRCById] 
(
@Id	INT
)
AS
BEGIN	

IF Exists(select * from QRCMaster WHERE Id=@Id)	
	BEGIN	
		SELECT	cm.Id,
				cm.Name,Description,
				cm.CreatedOn,				
			 CONCAT(c.FirstName, '', c.LastName) as CreatedBy,
				CONCAT(l.FirstName, '', l.LastName) as UpdatedBy,
				cm.UpdatedOn, cm.Active
		 from QRCMaster cm 
		 inner join UserMaster c  on cm.CreatedBy= c.Id
		 inner join UserMaster l on cm.UpdatedBy =l.Id 	
		 WHERE cm.Id=@Id order by cm.Id desc 
		 
		return 105 -- record exists
	end
		ELSE
		return 102 -- reconrd does not exists
END

GO
/****** Object:  StoredProcedure [dbo].[GetRatingsByContent]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetRatingsByContent]-- '[{"ContentId" : 1, "ContentType" : 1},{"ContentId" : 1, "ContentType" : 2}]'
(
@Content NVARCHAR(MAX)
)
AS 
BEGIN
	DECLARE @Return INT
	DECLARE @ContentTemp TABLE (ContentID INT, ContentType INT)
	DECLARE @ContentRating TABLE (ContentID INT, ContentType INT, Rating DECIMAL, AllRatings NVARCHAR(MAX))
	IF(@Content IS NOT NULL)
	BEGIN
		INSERT INTO @ContentTemp      
          
		SELECT ContentId,ContentType FROM        
		OPENJSON ( @Content )        
		WITH (         
              ContentId   INT '$.ContentId' ,  
			  ContentType INT '$.ContentType'
		)  
		DECLARE @ContentIdCur INT,@ContentTypeCur INT
		DECLARE db_cursor CURSOR FOR 
		SELECT ContentId,ContentType 
		FROM @ContentTemp 
		
		OPEN db_cursor  
		FETCH NEXT FROM db_cursor INTO @ContentIdCur, @ContentTypeCur 

		WHILE @@FETCH_STATUS = 0  
		BEGIN  
			  IF(@ContentTypeCur = 1)
				  BEGIN
					INSERT INTO @ContentRating VALUES(@ContentIdCur,@ContentTypeCur,(SELECT Rating FROM CourseMaster WHERE Id = @ContentIdCur),
					(SELECT CONVERT(INT,Rating) AS Star,COUNT(RatedBy) As UserCount
					from [CourseRating] where courseid = @ContentIdCur
					GROUP BY Rating FOR JSON AUTO))
				  END
			  ELSE IF(@ContentTypeCur = 2)
				  BEGIN
					INSERT INTO @ContentRating VALUES(@ContentIdCur,@ContentTypeCur,(SELECT Rating FROM ResourceMaster WHERE Id = @ContentIdCur),
					(SELECT CONVERT(INT,Rating) AS Star,COUNT(RatedBy) As UserCount
					from [ResourceRating] where resourceId = @ContentIdCur
					GROUP BY Rating FOR JSON AUTO))
				  END
			  FETCH NEXT FROM db_cursor INTO @ContentIdCur, @ContentTypeCur  
		END 

		CLOSE db_cursor  
		DEALLOCATE db_cursor 
		IF EXISTS(SELECT TOP 1 1 FROM @ContentRating)
		   SET @Return = 105
		ELSE
		   SET @Return = 102
		SELECT * FROM @ContentRating 
	END
	ELSE
		SET @Return = 102
	RETURN @Return
END
GO
/****** Object:  StoredProcedure [dbo].[GetResource]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetResource]       
    (  
     @PageNo int = 1,  
  @PageSize int = 5  
 )  
AS      
BEGIN      
      
   Declare @return INT  
declare @start int, @end int  
set @start = (@PageNo - 1) * @PageSize + 1  
set @end = @PageNo * @PageSize  
IF EXISTS(SELECT TOP 1 *  FROM ResourceMaster)      
 begin      
;with sqlpaging as (  
 SELECT Rownumber = ROW_NUMBER() OVER(ORDER BY r.Id desc), r.Id      
      ,r.Title      
      ,r.CategoryId, c.Name as CategoryName      
      ,SubCategoryId, sc.Name as SubCategoryName      
      ,Thumbnail      
      ,ResourceDescription      
      ,Keywords      
      ,ResourceContent      
      ,MaterialTypeId, mt.Name as MaterialTypeName      
      ,CopyRightId, cm.Title as CopyrightTitle, cm.Description as CopyrightDescription      
      ,IsDraft      
      ,CONCAT(um.FirstName, ' ', um.LastName) as CreatedBy, um.Id as CreatedById      
      ,r.CreatedOn      
      ,IsApproved      
      ,Rating      
      ,AlignmentRating      
      ,ReportAbuseCount,ViewCount,AverageReadingTime,(SELECT COUNT(*) FROM ContentDownloadInfo WHERE ContentId = r.Id AND ContentTypeId = 2) AS DownloadCount,  
   (SELECT COUNT(*) FROM ContentSharedInfo WHERE ContentId = r.Id AND ContentTypeId = 2) As SharedCount,     
    r.ReadingTime ,    
 es.Standard,    
 eu.EducationalUse,    
 el.Level,    
 Objective,    
 [Format] ,
 CommunityBadge,
 MoEBadge   
  FROM ResourceMaster r      
   inner join CategoryMaster c on r.CategoryId = c.Id      
   inner join MaterialTypeMaster mt on r.MaterialTypeId=mt.Id      
   left join SubCategoryMaster sc on r.SubCategoryId=sc.Id      
   left join CopyrightMaster cm on r.CopyRightId = cm.Id      
   LEFT JOIN lu_Educational_Standard es ON r.EducationalStandardId = es.Id    
   LEFT JOIN lu_Educational_Use eu ON r.EducationalUseId = eu.Id    
   LEFT JOIN lu_Level el ON r.LevelId = el.Id    
    
   inner join UserMaster um on r.CreatedBy =um.Id   
   )  
   select  
 top (@PageSize) *,  
 (select max(rownumber) from sqlpaging) as   
 Totalrows  
from sqlpaging  
where Rownumber between @start and @end  
Order by Id desc       
      
      
SELECT raf.Id      
      ,ResourceId      
      ,AssociatedFile      
      ,UploadedDate      
  FROM ResourceAssociatedFiles raf INNER JOIN ResourceMaster r ON r.Id=raf.ResourceId       
        
      
   SELECT rur.Id      
      ,ResourceId      
      ,URLReferenceId, uwl.URL as URLReference      
      ,rur.CreatedOn      
  FROM dbo.ResourceURLReferences rur       
  INNER JOIN ResourceMaster r ON r.Id=rur.ResourceId      
  inner join WhiteListingURLs uwl on uwl.Id=rur.URLReferenceId        
      
  SELECT rc.Id      
      ,[ResourceId]      
      ,[Comments]      
      ,CONCAT(um.FirstName, ' ', um.LastName) as CommentedBy, um.Id As CommentedById, um.Photo as CommentorImage      
      ,[CommentDate]      
      ,[ReportAbuseCount]      
  FROM [dbo].[ResourceComments] rc inner join UserMaster um on rc.UserId=um.Id where rc.IsHidden=0      
            
 return 105 -- record exists      
       
END      
      
else return 102 -- recond does not exists      
end  
GO
/****** Object:  StoredProcedure [dbo].[GetResourceByCourseId]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetResourceByCourseId] 
(
@CourseId INT
)
AS
BEGIN



IF EXISTS(SELECT TOP 1 1  FROM ResourceMaster r
		 inner join CategoryMaster c on r.CategoryId = c.Id
		 inner join MaterialTypeMaster mt on r.MaterialTypeId=mt.Id
		 left join SubCategoryMaster sc on r.SubCategoryId=sc.Id
		 left join CopyrightMaster cm on r.CopyRightId = cm.Id
		 inner join UserMaster um on r.CreatedBy =um.Id
		 WHERE r.id in (SELECT ResourcesId from CourseResourceMapping WHERE CourseId = @CourseId))
	begin
		SELECT r.Id
      ,r.Title
      ,r.CategoryId, c.Name as CategoryName
      ,SubCategoryId, sc.Name as SubCategoryName
      ,Thumbnail
      ,ResourceDescription
      ,Keywords
      ,ResourceContent
      ,MaterialTypeId, mt.Name as MaterialTypeName
      ,CopyRightId, cm.Title as CopyrightTitle, cm.Description as CopyrightDescription
      ,IsDraft
      ,CONCAT(um.FirstName, ' ', um.LastName) as CreatedBy, um.Id as CreatedById
      ,r.CreatedOn
      ,IsApproved
      ,Rating
      ,AlignmentRating
      ,ReportAbuseCount
  FROM ResourceMaster r
		 inner join CategoryMaster c on r.CategoryId = c.Id
		 inner join MaterialTypeMaster mt on r.MaterialTypeId=mt.Id
		 left join SubCategoryMaster sc on r.SubCategoryId=sc.Id
		 left join CopyrightMaster cm on r.CopyRightId = cm.Id
		 inner join UserMaster um on r.CreatedBy =um.Id
		 WHERE r.id in (SELECT ResourcesId from CourseResourceMapping WHERE CourseId = @CourseId)
		  Order by r.Id desc 


SELECT raf.Id
      ,ResourceId
      ,AssociatedFile
      ,UploadedDate
  FROM ResourceAssociatedFiles raf INNER JOIN ResourceMaster r ON r.Id=raf.ResourceId 
   WHERE r.id in (SELECT ResourcesId from CourseResourceMapping WHERE CourseId = @CourseId)

   SELECT rur.Id
      ,ResourceId
      ,URLReferenceId, uwl.URL as URLReference
      ,rur.CreatedOn
  FROM dbo.ResourceURLReferences rur 
  INNER JOIN ResourceMaster r ON r.Id=rur.ResourceId
  inner join WhiteListingURLs uwl on uwl.Id=rur.URLReferenceId 
   WHERE r.id in (SELECT ResourcesId from CourseResourceMapping WHERE CourseId = @CourseId)	

  SELECT rc.Id
      ,[ResourceId]
      ,[Comments]
      ,CONCAT(um.FirstName, ' ', um.LastName) as CommentedBy, um.Id As CommentedById, um.Photo as CommentorImage
      ,[CommentDate]
      ,[ReportAbuseCount]
  FROM [dbo].[ResourceComments] rc inner join UserMaster um on rc.UserId=um.Id where rc.IsHidden=0
   AND rc.[ResourceId] in (SELECT ResourcesId from CourseResourceMapping WHERE CourseId = @CourseId)
  			 
	return 105 -- record exists
	
END

else return 102 -- recond does not exists
end

GO
/****** Object:  StoredProcedure [dbo].[GetResourceById]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetResourceById]                       
(                      
@Id INT                      
)                      
AS                      
BEGIN                       
                    
DECLARE @SharedCount INT                  
DECLARE @DownloadCount INT                  
DECLARE @IsRemix BIT                   
DECLARE @VisitCount INT      
DECLARE @IsApproved BIT  
SET @IsRemix = 0                      
                      
IF EXISTS(SELECT TOP 1 1 from ResourceRemixHistory WHERE ResourceRemixedID =@Id)                      
BEGIN                      
SET @IsRemix= 1                      
END                      
IF Exists(select TOP 1 1 from ResourceMaster WHERE Id=@Id)                       
 BEGIN                       
 SET @SharedCount = (select count(Id) from ContentSharedInfo WHERE ContentId = @Id AND ContentTypeId = 2)                   
  SET @DownloadCount = (select count(Id) from ContentDownloadInfo WHERE ContentId = @Id AND ContentTypeId = 2)                
                
  SELECT @VisitCount = ViewCount,@IsApproved =IsApproved FROM ResourceMaster WHERE ID = @Id           
               
 IF(@VisitCount IS NULL AND @IsApproved = 1)              
 BEGIN              
 Update ResourceMaster SET ViewCount =  1 WHERE ID = @Id               
              
 END              
              
 ELSE IF(@IsApproved = 1)        
 BEGIN                               
  Update ResourceMaster SET ViewCount = @VisitCount+ 1 WHERE ID = @Id               
 END;              
              
              
  SELECT r.Id                      
      ,r.Title                      
      ,r.CategoryId, c.Name as CategoryName                      
      ,SubCategoryId, sc.Name as SubCategoryName,
	   c.Name_Ar as CategoryName_Ar,
	   sc.Name_Ar as SubCategoryName_Ar
      ,Thumbnail                      
      ,ResourceDescription                      
      ,Keywords                      
      ,ResourceContent                      
      ,MaterialTypeId, mt.Name as MaterialTypeName ,
	  mt.Name_Ar as MaterialTypeName_Ar
      ,CopyRightId, cm.Title as CopyrightTitle, 
	  cm.Title_Ar as CopyrightTitle_Ar
	  ,cm.Description as CopyrightDescription
	  ,cm.Description_Ar as CopyrightDescription_Ar,
   cm.Media,  
   cm.Protected  
      ,IsDraft                      
      ,um.FirstName+' '+ um.LastName as CreatedBy, um.Id as CreatedById                      
      ,r.CreatedOn                      
      ,IsApproved                      
      ,Rating                      
      ,AlignmentRating                      
      ,ReportAbuseCount,ViewCount,AverageReadingTime,@DownloadCount AS DownloadCount,r.ReadingTime  ,                    
   es.Standard,                          
 Objective,                   
 @SharedCount as SharedCount,                  
 CAST(LastView as DATETIME) as LastView,                  
 [Format]                    
   ,@IsRemix AS IsRemix ,            
   LevelId ,            
      (SELECT [Level] FROM lu_Level WHERE Id = r.LevelId) AS [LevelName],                  
   (SELECT Level_Ar FROM lu_Level WHERE Id = r.LevelId) AS [Level_Ar],                  
   r.EducationalStandardId,                  
   (SELECT [Standard] FROM lu_educational_standard WHERE Id = r.EducationalStandardId) AS [Standard],                  
   (SELECT [Standard_Ar] FROM lu_educational_standard WHERE Id = r.EducationalStandardId) AS [Standard_Ar],                  
   r.EducationalUseId,                  
    (SELECT [EducationalUse] FROM lu_educational_use WHERE Id = r.EducationalUseId) AS [EducationalUseName],                  
   (SELECT [EducationalUse_Ar] FROM lu_educational_use WHERE Id = r.EducationalUseId) AS [EducationalUse_Ar],  
   CommunityBadge,  
   MoEBadge             
  FROM ResourceMaster r                      
   inner join CategoryMaster c on r.CategoryId = c.Id                      
   LEFT join MaterialTypeMaster mt on r.MaterialTypeId=mt.Id                      
   left join SubCategoryMaster sc on r.SubCategoryId=sc.Id                      
   left join CopyrightMaster cm on r.CopyRightId = cm.Id                      
    LEFT JOIN lu_Educational_Standard es ON r.EducationalStandardId = es.Id                    
   LEFT JOIN lu_Educational_Use eu ON r.EducationalUseId = eu.Id                    
   LEFT JOIN lu_Level el ON r.LevelId = el.Id                    
                    
   inner join UserMaster um on r.CreatedBy =um.Id where r.Id=@Id Order by r.Id desc                       
    Update ResourceMaster SET LastView = GETDATE() WHERE ID = @Id                  
                      
SELECT raf.Id                      
      ,ResourceId                      
      ,AssociatedFile                      
      ,UploadedDate,          
   FileName          
  FROM ResourceAssociatedFiles raf INNER JOIN ResourceMaster r ON r.Id=raf.ResourceId AND r.Id=@Id     
  WHERE IsInclude = 1    
                      
                      
                      
   SELECT rur.Id                      
      ,ResourceId                      
      ,URLReferenceId, uwl.URL as URLReference                      
      ,rur.CreatedOn
	  ,uwl.IsActive
	  ,uwl.IsApproved
  FROM dbo.ResourceURLReferences rur                       
  INNER JOIN ResourceMaster r ON r.Id=rur.ResourceId                 
  inner join WhiteListingURLs uwl on uwl.Id=rur.URLReferenceId AND r.Id=@Id   
  --AND uwl.IsApproved = 1                     
                      
   SELECT rc.Id                      
      ,[ResourceId]                      
      ,[Comments]                      
      ,um.FirstName+ ' '+ um.LastName as CommentedBy, um.Id As CommentedById, um.Photo as CommentorImage                      
      ,[CommentDate]                      
      ,[ReportAbuseCount]                      
  FROM [dbo].[ResourceComments] rc inner join UserMaster um on rc.UserId=um.Id AND rc.ResourceId=@Id where rc.IsHidden=0                    
      
SELECT Rating,COUNT(RatedBy) As NoOfUsers    
from [ResourceRating] where resourceid = @Id    
GROUP BY Rating                   
 return 105 -- record exists                      
                      
 end                      
  ELSE                      
  return 102 -- reconrd does not exists                      
END     

GO
/****** Object:  StoredProcedure [dbo].[GetStream]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
CREATE PROCEDURE [dbo].[GetStream]    
AS  
BEGIN    
 IF Exists(select top 1 1 from StreamMaster)   
 BEGIN   
  SELECT cm.Id,  
    cm.Name,  
    cm.Name_Ar,  
    cm.CreatedOn,      
    CONCAT(c.FirstName, '', c.LastName) as CreatedBy,  
    CONCAT(l.FirstName, '', l.LastName) as UpdatedBy,  
    cm.UpdatedOn, cm.Active  
   from StreamMaster cm   
   inner join UserMaster c  on cm.CreatedBy= c.Id  
   inner join UserMaster l on cm.UpdatedBy =l.Id  
   WHERE cm.Active = 1
   order by cm.Id desc   
     
  RETURN 105 -- record exists  
 end  
  ELSE  
  RETURN 102 -- reconrd does not exists  
END  
  
GO
/****** Object:  StoredProcedure [dbo].[GetStreamById]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetStreamById] 
(
@Id	INT
)
AS
BEGIN	

IF Exists(select * from StreamMaster WHERE Id=@Id)	
	BEGIN	
		SELECT	cm.Id,
				cm.Name,
				cm.Name_Ar,
				cm.CreatedOn,				
			 CONCAT(c.FirstName, '', c.LastName) as CreatedBy,
				CONCAT(l.FirstName, '', l.LastName) as UpdatedBy,
				cm.UpdatedOn, cm.Active
		 from StreamMaster cm 
		 inner join UserMaster c  on cm.CreatedBy= c.Id
		 inner join UserMaster l on cm.UpdatedBy =l.Id 	
		 WHERE cm.Id=@Id order by cm.Id desc 
		 
		return 105 -- record exists
	end
		ELSE
		return 102 -- reconrd does not exists
END

GO
/****** Object:  StoredProcedure [dbo].[GetSubCategory]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
CREATE PROCEDURE [dbo].[GetSubCategory]    
AS  
BEGIN    
 IF Exists(select * from SubCategoryMaster where Status=1)   
 BEGIN   
  SELECT sm.Id,  
    sm.Name,  
    sm.Name_Ar,  
    cm.Name as CategoryName,   
    cm.Id as CategoryId,  
    sm.CreatedOn,      
       CONCAT(c.FirstName, '', c.LastName) as CreatedBy,  
    CONCAT(l.FirstName, '', l.LastName) as UpdatedBy,  
    sm.UpdatedOn,   
    sm.Active  
   from SubCategoryMaster sm   
   INNER JOIN CategoryMaster cm on sm.CategoryId=cm.Id  
   inner join UserMaster c  on sm.CreatedBy= c.Id  
   inner join UserMaster l on sm.UpdatedBy =l.Id  
   where sm.Status = 1 --and cm.Active = 1 and l.Active = 1 and c.Active = 1  
   order by cm.Id desc    
     
  RETURN 105 -- record exists  
 end  
  ELSE  
  RETURN 102 -- reconrd does not exists  
END  
  
GO
/****** Object:  StoredProcedure [dbo].[GetSubCategoryById]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetSubCategoryById] 
(
@Id	INT
)
AS
BEGIN	

IF Exists(select * from SubCategoryMaster WHERE Id=@Id)	
	BEGIN	
		SELECT	sm.Id,
				sm.Name,
				sm.Name_Ar,
				cm.Name as CategoryName, 
				cm.Id as CategoryId,
				sm.CreatedOn,				
			    CONCAT(c.FirstName, '', c.LastName) as CreatedBy,
				CONCAT(l.FirstName, '', l.LastName) as UpdatedBy,
				sm.UpdatedOn, 
				sm.Active
		 from SubCategoryMaster sm 
		 INNER JOIN CategoryMaster cm on sm.CategoryId=cm.Id 
		 inner join UserMaster c  on cm.CreatedBy= c.Id
		 inner join UserMaster l on cm.UpdatedBy =l.Id WHERE sm.Id=@Id order by cm.Id desc 	
		 
		return 105 -- record exists
	end
		ELSE
		return 102 -- reconrd does not exists
END

GO
/****** Object:  StoredProcedure [dbo].[GetUserProfileByEmail]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--EXEC GetUserProfileByEmail 'avnp@gmail.com'  
  
CREATE PROCEDURE [dbo].[GetUserProfileByEmail]   
(  
@Email NVARCHAR(250)  
)  
AS  
BEGIN   
  
IF Exists(select * from UserMaster WHERE EMAIL=@Email)   
 BEGIN   
  
DECLARE @Id INT  
  
SELECT @Id=Id from UserMaster WHERE Email=@Email  
  
 SELECT TOP(1) um.Id  
      ,um.TitleId, tm.[Name] AS Title  
      ,um.FirstName  
      ,um.MiddleName  
      ,um.LastName  
      ,um.CountryId, cm.[Name] AS Country  
      ,um.StateId, sm.[Name] AS [State]  
      ,um.Gender -- Enumeration  
      ,um.Email  
      ,um.PortalLanguageId -- Enumeration  
      --,um.DepartmentId, dm.Name as Department  
     -- ,um.DesignationId, dsm.Name as Designation  
      ,um.DateOfBirth  
      ,um.Photo  
      ,um.ProfileDescription  
      ,um.SubjectsInterested  
      ,um.ApprovalStatus  
      ,um.CreatedOn  
      ,um.UpdatedOn  
      ,um.Active  
      ,um.IsContributor, um.IsAdmin  ,um.Theme
  FROM UserMaster um left JOIN TitleMaster tm on um.titleId=tm.Id  
      left JOIN CountryMaster cm on um.CountryId=cm.Id  
      left JOIN StateMaster sm on um.StateId=sm.Id  
     -- INNER JOIN DepartmentMaster dm on um.DepartmentId=dm.Id  
     -- INNER JOIN DesignationMaster dsm on um.DesignationId=dsm.Id  
  
     where Email=@Email  
  
SELECT uc.Id  
      ,UserId  
      ,CertificationName  
      ,[Year]  
      ,uc.CreatedOn  
  FROM UserCertification uc INNER JOIN UserMaster um on um.Id=uc.UserId WHERE um.Id=@Id  
  
SELECT ue.Id  
      ,UserId  
      ,UniversitySchool  
      ,Major  
      ,Grade  
      ,FromDate  
      ,ToDate  
      ,ue.CreatedOn  
  FROM UserEducation ue INNER JOIN UserMaster um on um.Id=ue.UserId WHERE um.Id=@Id  
  
  SELECT ul.Id  
      ,UserId  
      ,LanguageId, lm.[Name] as [Language]  
      ,IsSpeak  
      ,IsRead  
      ,IsWrite  
      ,ul.CreatedOn  
  FROM UserLanguages ul INNER JOIN LanguageMaster lm on lm.Id=ul.LanguageId    
      INNER JOIN UserMaster um on um.Id=ul.UserId WHERE um.Id=@Id  
  
  SELECT uexp.Id  
      ,UserId  
      ,OrganizationName  
      ,Designation  
      ,FromDate  
      ,ToDate  
      ,uexp.CreatedOn  
  FROM dbo.UserExperiences uexp INNER JOIN UserMaster um on um.Id=uexp.UserId WHERE um.Id=@Id  
    
SELECT sm.Id  
      ,UserId  
      ,SocialMediaId,smm.Name as SocialMedia  
      ,[URL]  
      ,sm.CreatedOn  
  FROM dbo.UserSocialMedia sm INNER JOIN SocialMediaMaster smm on sm.SocialMediaId=smm.Id  
    
    
  INNER JOIN UserMaster um on um.Id=sm.UserId WHERE um.Id=@Id  
  
  exec CreateLogEntry @LogModuleId=18,@UserId=@Id,@ActionId=4,@ActionDetail='Fetching UserProfile'  
  
RETURN 105 -- record exists  
  
END  
  
ELSE RETURN 102 -- No record exist  
  
END  
  
GO
/****** Object:  StoredProcedure [dbo].[GetUserProfileById]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetUserProfileById] 
(
@Id	INT
)
AS
BEGIN	

IF Exists(select * from UserMaster WHERE Id=@Id)	
	BEGIN	

	SELECT um.Id
      ,um.TitleId, tm.[Name] AS Title
      ,um.FirstName
      ,um.MiddleName
      ,um.LastName
      ,(CASE WHEN um.CountryId IS NULL THEN um.CountryId ELSE 0 END) AS CountryId
	  ,cm.[Name] AS Country
      ,(CASE WHEN um.StateId IS NULL THEN um.StateId ELSE 0 END) AS StateId, 
	  sm.[Name] AS [State]
      ,um.Gender -- Enumeration
      ,um.Email
      ,um.PortalLanguageId -- Enumeration
     -- ,um.DepartmentId, dm.Name as Department
     -- ,um.DesignationId, dsm.Name as Designation
      ,um.DateOfBirth
      ,um.Photo
      ,um.ProfileDescription
      ,um.SubjectsInterested
      ,um.ApprovalStatus
      ,um.CreatedOn
      ,um.UpdatedOn
      ,um.Active
      ,um.IsContributor, um.IsAdmin
  FROM UserMaster um left JOIN TitleMaster tm on um.titleId=tm.Id
					 left JOIN CountryMaster cm on um.CountryId=cm.Id
					 left JOIN StateMaster sm on um.StateId=sm.Id
					-- INNER JOIN DepartmentMaster dm on um.DepartmentId=dm.Id
					-- INNER JOIN DesignationMaster dsm on um.DesignationId=dsm.Id
					where um.Id=@Id

SELECT uc.Id
      ,UserId
      ,CertificationName
      ,[Year]
      ,uc.CreatedOn
  FROM UserCertification uc INNER JOIN UserMaster um on um.Id=uc.UserId WHERE um.Id=@Id

SELECT ue.Id
      ,UserId
      ,UniversitySchool
      ,Major
      ,Grade
      ,FromDate
      ,ToDate
      ,ue.CreatedOn
  FROM UserEducation ue INNER JOIN UserMaster um on um.Id=ue.UserId WHERE um.Id=@Id

  SELECT ul.Id
      ,UserId
      ,LanguageId, lm.[Name] as [Language]
      ,IsSpeak
      ,IsRead
      ,IsWrite
      ,ul.CreatedOn
  FROM UserLanguages ul INNER JOIN LanguageMaster lm on lm.Id=ul.LanguageId  
						INNER JOIN UserMaster um on um.Id=ul.UserId WHERE um.Id=@Id

  SELECT uexp.Id
      ,UserId
      ,OrganizationName
      ,Designation
      ,FromDate
      ,ToDate
      ,uexp.CreatedOn
  FROM dbo.UserExperiences uexp INNER JOIN UserMaster um on um.Id=uexp.UserId WHERE um.Id=@Id
  
SELECT sm.Id
      ,UserId
      ,SocialMediaId,smm.Name as SocialMedia
      ,[URL]
      ,sm.CreatedOn
  FROM dbo.UserSocialMedia sm INNER JOIN SocialMediaMaster smm on sm.SocialMediaId=smm.Id
  
  
  INNER JOIN UserMaster um on um.Id=sm.UserId WHERE um.Id=@Id

  exec CreateLogEntry @LogModuleId=18,@UserId=@Id,@ActionId=4,@ActionDetail='Fetching UserProfile'

RETURN 105 -- record exists

END

ELSE RETURN 102 -- No record exist

END
GO
/****** Object:  StoredProcedure [dbo].[GetWhitelistedUrls]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetWhitelistedUrls] 

AS
BEGIN		
		
	SELECT wlu.Id,    
	   CONCAT(c.FirstName, '', c.LastName) as RequestedBy,
	   CONCAT(l.FirstName, '', l.LastName) as VerifiedBy
      ,URL
      ,IsApproved
      ,RequestedOn
      ,VerifiedOn
      ,RejectedReason
  FROM WhiteListingURLs wlu 
   inner join UserMaster c  on wlu.RequestedBy= c.Id
		 left join UserMaster l on wlu.VerifiedBy =l.Id where IsApproved=1

  IF @@ROWCOUNT >0
		return 105 -- record exists
		
		ELSE
		return 102 -- record does not exists
END

GO
/****** Object:  StoredProcedure [dbo].[GetWhitelistingRequests]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


    
CREATE PROCEDURE [dbo].[GetWhitelistingRequests]     
  (  
  @IsApproved BIT  
  )  
AS    
BEGIN      
      
 SELECT wlu.Id,        
    CONCAT(c.FirstName, '', c.LastName) as RequestedBy,    
    CONCAT(l.FirstName, '', l.LastName) as VerifiedBy    
      ,URL    
      ,IsApproved    
      ,RequestedOn    
      ,VerifiedOn    
      ,RejectedReason    
  FROM WhiteListingURLs wlu     
   inner join UserMaster c  on wlu.RequestedBy= c.Id    
   left join UserMaster l on wlu.VerifiedBy =l.Id where IsApproved=@IsApproved AND IsActive =1 order by wlu.Id desc      
      
    
  IF @@ROWCOUNT >0    
  return 105 -- record exists    
      
  ELSE    
  return 102 -- record does not exists    
END    
GO
/****** Object:  StoredProcedure [dbo].[HideCourseCommentByAuthor]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[HideCourseCommentByAuthor]		  
		   @Id Numeric(18,0),
		   @CourseId Numeric(18,0),
		   @RequestedBy INT
AS
BEGIN

DECLARE @result INT

IF EXISTS(SELECT * FROM CourseComments WHERE Id= @Id)

BEGIN

Declare @Owner INT

SELECT @Owner=CreatedBy FROM CourseMaster WHERE Id= @CourseId

IF @RequestedBy = @Owner

BEGIN
 
 UPDATE CourseComments SET IsHidden=1 where Id=@Id and CourseId=@CourseId

 If @@ERROR <>0

 set @result=106 -- failed updation

 else set @result =101 --set as hidden
END

else

set @result = 117--course comments can only be set to hidded by owner

END

ELSE

SET @result =102 -- comment does not exists


return @result
END

GO
/****** Object:  StoredProcedure [dbo].[HideResourceCommentByAuthor]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[HideResourceCommentByAuthor]		  
		   @Id Numeric(18,0),
		   @ResourceId Numeric(18,0),
		   @RequestedBy INT
AS
BEGIN

DECLARE @result INT

IF EXISTS(SELECT * FROM ResourceComments WHERE Id= @Id)

BEGIN

Declare @Owner INT

SELECT @Owner=CreatedBy FROM ResourceMaster WHERE Id= @ResourceId

IF @RequestedBy = @Owner

BEGIN
 
 UPDATE ResourceComments SET IsHidden=1 where Id=@Id and ResourceId=@ResourceId

 If @@ERROR <>0

 set @result=106 -- failed updation

 else set @result =101 --set as hidden
END

else

set @result = 117--resource comments can only be set to hidded by owner

END

ELSE

SET @result =102 -- comment does not exists


return @result
END

GO
/****** Object:  StoredProcedure [dbo].[RateCourse]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[RateCourse]   
 @CourseId NUMERIC(18,0),  
 @Rating INT,  
 @Comments NVARCHAR(2000),    
 @RatedBy INT  
AS  
BEGIN  
Declare @return INT  
IF NOT EXISTS (SELECT TOP 1 1 FROM CourseRating WHERE CourseId=@CourseId AND RatedBy=@RatedBy)  
BEGIN   
 INSERT INTO [dbo].[CourseRating]  
           ([CourseId]  
           ,[Rating]      
           ,[Comments]  
      ,[RatedBy])  
     VALUES  
           (@CourseId,@Rating,@Comments,@RatedBy)   
       
      IF @@ROWCOUNT >0  
  
   begin  
  
   update CourseMaster  SET  Rating = (select SUM(rating)/COUNT(id) from CourseRating where Courseid = @CourseId) where id= @CourseId  
  
   SET @return =100 -- creation success  
  
   end  
  
  ELSE SET @return = 107  
END  
  
ELSE  
BEGIN
  UPDATE [CourseRating]
  SET Rating = @Rating,
  Comments = @Comments
  WHERE CourseId=@CourseId AND RatedBy=@RatedBy

  update CourseMaster  SET  Rating = (select SUM(rating)/COUNT(id) from CourseRating where Courseid = @CourseId) where id= @CourseId  
  SET @return = 100 -- Record exists  
END  
   RETURN @return  
END  
GO
/****** Object:  StoredProcedure [dbo].[RateResource]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[RateResource]   
 @ResourceId NUMERIC(18,0),  
 @Rating INT,  
 @Comments NVARCHAR(2000),    
 @RatedBy INT  
AS  
BEGIN  
Declare @return INT  
IF NOT EXISTS (SELECT TOP 1 1 FROM ResourceRating WHERE ResourceId=@ResourceId AND RatedBy=@RatedBy)  
BEGIN   
 INSERT INTO [dbo].[ResourceRating]  
           ([ResourceId]  
           ,[Rating]      
           ,[Comments]  
      ,[RatedBy])  
     VALUES  
           (@ResourceId,@Rating,@Comments,@RatedBy)   
       
      IF @@ROWCOUNT >0  
  
   begin  
  
   update ResourceMaster  SET  Rating = (select SUM(rating)/COUNT(id) from ResourceRating where ResourceId=@ResourceId ) where id= @ResourceId  
  
   SET @return =100 -- creation success  
  
   end  
  
  ELSE SET @return = 107  
END  
  
ELSE  
BEGIN
 UPDATE [ResourceRating]
  SET Rating = @Rating,
  Comments = @Comments
  WHERE ResourceId=@ResourceId AND RatedBy=@RatedBy

  update ResourceMaster  SET  Rating = (select SUM(rating)/COUNT(id) from ResourceRating where ResourceId=@ResourceId ) where id= @ResourceId    
  SET @return = 100 -- Record exists  
END  
   RETURN @return  
END  
GO
/****** Object:  StoredProcedure [dbo].[RateResourceAlignment]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[RateResourceAlignment] 
	@ResourceId NUMERIC(18,0),	
	@CategoryId INT=null,	
	@LevelId nvarchar(50)=NULL,	
	@RatedBy INT,
	@Rating INT
AS
BEGIN
Declare @return INT
IF NOT EXISTS (SELECT * FROM ResourceAlignmentRating WHERE ResourceId=@ResourceId AND RatedBy=@RatedBy)
BEGIN	

declare @cat int

if @CategoryId= 0

 set @cat=null

 else set @cat= @CategoryId

 INSERT INTO [dbo].[ResourceAlignmentRating]
           ([ResourceId]
		   , CategoryId
		    , Grade
           ,[Rating]
		   ,RatedBy
			
			)
     VALUES
           (@ResourceId,@cat,@LevelId, @Rating,@RatedBy)	
		   
		   	IF @@ROWCOUNT >0

			begin

			update ResourceMaster  SET  AlignmentRating = (select SUM(rating)/COUNT(id) from ResourceAlignmentRating ) where id= @ResourceId

			SET @return =100 -- creation success

			end

		ELSE SET @return = 107
END

ELSE
		SET @return = 105 -- Record exists
	
		 RETURN @return
END



GO
/****** Object:  StoredProcedure [dbo].[ReportCourse]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[ReportCourse]		  
		   @Id INT            
AS
BEGIN

Declare @return INT

IF EXISTS (SELECT * FROM CourseMaster WHERE Id=@Id)
BEGIN	
UPDATE CourseMaster SET ReportAbuseCount=ReportAbuseCount + 1
									  WHERE Id=@Id
		-- do log entry here
		 
		  IF @@ERROR <> 0
		
		  SET @return= 106 -- update failed	

	ELSE
		SET @return= 101 -- update success	

RETURN @return
END

END
GO
/****** Object:  StoredProcedure [dbo].[ReportCourseComment]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ReportCourseComment]      
     @Id INT              
AS  
BEGIN  
  
Declare @return INT  
  
IF EXISTS (SELECT * FROM CourseComments WHERE Id=@Id)  
BEGIN   
UPDATE CourseComments SET ReportAbuseCount=ReportAbuseCount + 1  ,
UpdateDate = GETDATE()
           WHERE Id=@Id  
  -- do log entry here  
     
    IF @@ERROR <> 0  
    
    SET @return= 106 -- update failed   
  
 ELSE  
  SET @return= 101 -- update success   
  
END  
  
ELSE   
  
SET @return= 102 -- record does not exists  
  
RETURN @return  
END  
  
GO
/****** Object:  StoredProcedure [dbo].[ReportCourseCommentWithComment]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ReportCourseCommentWithComment] 
	@CourseCommentId NUMERIC(18,0),
	@ReportReasons NVARCHAR(50),
	@Comments NVARCHAR(200),
	@ReportedBy INT
AS
BEGIN
Declare @return INT
IF NOT EXISTS (SELECT * FROM CourseCommentAbuseReports WHERE CourseCommentId=@CourseCommentId AND ReportedBy=@ReportedBy)
BEGIN	
		INSERT INTO [dbo].[CourseCommentAbuseReports]
           ([CourseCommentId]          
           ,[ReportReasons]
           ,[Comments]
		   ,[ReportedBy])
     VALUES
           (@CourseCommentId,@ReportReasons,@Comments,@ReportedBy)	
		   
		   	IF @@ROWCOUNT >0

			begin

			update CourseComments set ReportAbuseCount = ReportAbuseCount + 1 where Id=@CourseCommentId

			SET @return =100 -- creation success

			end

		ELSE SET @return = 107
END

ELSE
		SET @return = 105 -- Record exists
	
		 return @return
END

GO
/****** Object:  StoredProcedure [dbo].[ReportCourseWithComment]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ReportCourseWithComment]   
 @CourseId NUMERIC(18,0),  
 @ReportReasons NVARCHAR(50),  
 @Comments NVARCHAR(200),  
 @ReportedBy INT  
AS  
BEGIN  
Declare @return INT  
IF NOT EXISTS (SELECT * FROM CourseAbuseReports WHERE CourseId=@CourseId AND ReportedBy=@ReportedBy)  
BEGIN   
  INSERT INTO [dbo].[CourseAbuseReports]  
           ([CourseId]            
           ,[ReportReasons]  
           ,[Comments]  
     ,[ReportedBy]  
     ,[IsHidden],
	 CreatedDate,
	 UpdateDate)  
     VALUES  
           (@CourseId,@ReportReasons,@Comments,@ReportedBy,0,GETDATE(),GETDATE())   
       
      IF @@ROWCOUNT >0  
  
   begin  
  
   update CourseMaster set ReportAbuseCount = ReportAbuseCount + 1 where Id=@CourseId  
  
   SET @return =100 -- creation success  
  
   end  
  
  ELSE SET @return = 107  
END  
  
ELSE  
  SET @return = 105 -- Record exists  
   
   return @return  
END  
  
  
GO
/****** Object:  StoredProcedure [dbo].[ReportResource]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[ReportResource]		  
		   @Id INT            
AS
BEGIN

Declare @return INT

IF EXISTS (SELECT * FROM ResourceMaster WHERE @Id=@Id)
BEGIN	
UPDATE ResourceMaster SET ReportAbuseCount=ReportAbuseCount + 1
									  WHERE Id=@Id
		-- do log entry here
		 
		  IF @@ERROR <> 0
		
		  SET @return= 106 -- update failed	


	ELSE

		SET @return= 101 -- update success	


END

else SET @return= 102

RETURN @return

END
GO
/****** Object:  StoredProcedure [dbo].[ReportResourceComment]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ReportResourceComment]      
     @Id INT              
AS  
BEGIN  
  
Declare @return INT  
  
IF EXISTS (SELECT * FROM ResourceComments WHERE Id=@Id)  
BEGIN   
UPDATE ResourceComments SET ReportAbuseCount=ReportAbuseCount + 1 , UpdateDate = GETDATE() 
           WHERE Id=@Id  
  -- do log entry here  
     
    IF @@ERROR <> 0  
    
    SET @return= 106 -- update failed   
  
 ELSE  
  SET @return= 101 -- update success   
  
  
END  
else    
SET @return= 102 -- does not exist  
  
RETURN @return  
END  
  
GO
/****** Object:  StoredProcedure [dbo].[ReportResourceCommentWithComment]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ReportResourceCommentWithComment] 
	@ResourceCommentId NUMERIC(18,0),
	@ReportReasons NVARCHAR(50),
	@Comments NVARCHAR(200),
	@ReportedBy INT
AS
BEGIN
Declare @return INT
IF NOT EXISTS (SELECT * FROM ResourceCommentsAbuseReports WHERE ResourceCommentId=@ResourceCommentId AND ReportedBy=@ReportedBy)
BEGIN	
		INSERT INTO [dbo].[ResourceCommentsAbuseReports]
           ([ResourceCommentId]          
           ,[ReportReasons]
           ,[Comments]
		   ,[ReportedBy])
     VALUES
           (@ResourceCommentId,@ReportReasons,@Comments,@ReportedBy)	
		   
		   	IF @@ROWCOUNT >0

			begin

			update ResourceComments set ReportAbuseCount = ReportAbuseCount + 1 where Id=@ResourceCommentId

			SET @return =100 -- creation success

			end

		ELSE SET @return = 107
END

ELSE
		SET @return = 105 -- Record exists
	
		 return @return
END


GO
/****** Object:  StoredProcedure [dbo].[ReportResourceWithComment]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ReportResourceWithComment]     
 @ResourceId NUMERIC(18,0),    
 @ReportReasons NVARCHAR(50),    
 @Comments NVARCHAR(200),    
 @ReportedBy INT    
AS    
BEGIN    
Declare @return INT    
IF NOT EXISTS (SELECT * FROM ResourceAbuseReports WHERE ResourceId=@ResourceId AND ReportedBy=@ReportedBy)    
BEGIN     
    
    
    
  INSERT INTO [dbo].[ResourceAbuseReports]    
           ([ResourceId]              
           ,[ReportReasons]    
           ,[Comments]    
     ,[ReportedBy]    
     ,[IsHidden],  
  [CreatedDate],
  [UpdateDate])    
     VALUES    
           (@ResourceId,@ReportReasons,@Comments,@ReportedBy,0,GETDATE(),GETDATE());    
         
      IF @@ROWCOUNT >0    
    
   begin    
    
   update ResourceMaster set ReportAbuseCount = ReportAbuseCount + 1 where Id=@ResourceId    
    
   SET @return =100 -- creation success    
    
   end    
    
  ELSE SET @return = 107    
END    
    
ELSE    
  SET @return = 105 -- Record exists    
     
   RETURN @return    
END    
    
GO
/****** Object:  StoredProcedure [dbo].[spd_AbuseReport]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
  
-- =============================================  
-- Author:  <Author,,Name>  
-- Create date: <Create Date,,>  
-- Description: <Description,,>  
-- =============================================  
CREATE PROCEDURE [dbo].[spd_AbuseReport]  
 (  
 @Id INT,  
 @ContentType INT  ,
 @Reason NVARCHAR(500)
 )  
  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
 DECLARE @Result INT;  
 DECLARE @ItemId INT
   IF(@ContentType=1)  
 BEGIN  
 SELECT @ItemId = CourseId FROM CourseAbuseReports WHERE Id = @Id;
 --UPDATE CourseMaster SET isDraft = 1, isApproved = 0 WHERE Id = @ItemId;
-- UPDATE CourseAbuseReports SET IsHidden = 1,Reason= @Reason WHERE Id = @Id;  
 UPDATE CourseAbuseReports SET Reason= @Reason WHERE Id = @Id;  
 SET @Result= 103;  
 END  
  
 ELSE IF (@ContentType=2)  
 BEGIN  
 SELECT @ItemId = ResourceId FROM ResourceAbuseReports WHERE Id = @Id;
 --UPDATE ResourceMaster SET isDraft = 1, isApproved = 0 WHERE Id = @ItemId;
-- UPDATE ResourceAbuseReports SET IsHidden = 1,Reason= @Reason  WHERE Id = @Id;  
 UPDATE ResourceAbuseReports SET Reason= @Reason  WHERE Id = @Id;  
  SET @Result= 103;  
 END  
 ELSE IF (@ContentType=3)  
 BEGIN  
-- UPDATE ResourceComments SET IsHidden = 1,Reason= @Reason  WHERE Id = @Id;  
 UPDATE ResourceComments SET Reason= @Reason  WHERE Id = @Id;
 SET @Result= 103;  
 END  
 ELSE IF (@ContentType=4)  
 BEGIN  
 --UPDATE CourseComments SET IsHidden = 1,Reason= @Reason  WHERE Id = @Id;  
 UPDATE CourseComments SET Reason= @Reason  WHERE Id = @Id;  
 SET @Result= 103;  
 END  
 ELSE  
 BEGIN  
 SET @Result= 102;  
 END  
  
 RETURN @Result;  
END  
  
GO
/****** Object:  StoredProcedure [dbo].[spd_Announcements]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[spd_Announcements]
(
@Id INT
)
AS
BEGIN
Declare @return INT  
	-- SET NOCOUNT ON added to prevent extra result sets fromAnnouncements`
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
  IF EXISTS (SELECT TOP 1 1 FROM Announcements  WHERE Id=@Id AND Active = 1)  
  BEGIN  
     UPDATE  Announcements SET Active = 0   WHERE Id=@Id;

	  IF @@ROWCOUNT>0    
		SET @return =103 -- record DELETED    
    ELSE    
     SET @return = 104 -- trying active record deletion     
  END  
  
  ELSE  
	SET @return = 102 -- reconrd does not exists  
  END;

  RETURN @return;
GO
/****** Object:  StoredProcedure [dbo].[spd_lu_Educational_Standard]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
-- =============================================  
-- Author:  <Author,,Name>  
-- Create date: <Create Date,,>  
-- Description: <Description,,>  
-- =============================================  
CREATE PROCEDURE [dbo].[spd_lu_Educational_Standard]  
(  
@Id INT  
)  
AS  
BEGIN  
Declare @return INT 
Declare @catCount INT 
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  IF EXISTS (SELECT TOP 1 1 FROM lu_Educational_Standard  WHERE Id=@Id AND Status = 1) 
  BEGIN
  SET @catCount =   
   (SELECT COUNT(*) FROM CourseMaster  WHERE EducationalStandardId=@Id)+
   (SELECT COUNT(*) FROM ResourceMaster  WHERE EducationalStandardId=@Id);  


   IF(@catCount > 0)  
    SET @return = 121  
   ELSE  
   BEGIN  
    Update lu_Educational_Standard set Status = 0 where Id=@Id  
     IF @@ROWCOUNT>0    
      SET @return =103 -- record DELETED    
     ELSE    
      SET @return = 121 -- trying active record deletion     
   END  
END  
  
  ELSE  
  SET @return = 102 -- reconrd does not exists    
  
  END
  RETURN @return;  
GO
/****** Object:  StoredProcedure [dbo].[spd_lu_Educational_Use]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================  
-- Author:  <Author,,Name>  
-- Create date: <Create Date,,>  
-- Description: <Description,,>  
-- =============================================  
CREATE PROCEDURE [dbo].[spd_lu_Educational_Use]  
(  
@Id INT  
)  
AS  
BEGIN  
Declare @return INT    
Declare @catCount INT 
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  IF EXISTS (SELECT TOP 1 1 FROM lu_Educational_Use  WHERE Id=@Id AND Status = 1)    
  BEGIN
  SET @catCount =   
   (SELECT COUNT(*) FROM CourseMaster  WHERE EducationalUseId=@Id)+
   (SELECT COUNT(*) FROM ResourceMaster  WHERE EducationalUseId=@Id);  


   IF(@catCount > 0)  
    SET @return = 121  
   ELSE  
   BEGIN  
    Update lu_Educational_Use set Status = 0 where Id=@Id  
     IF @@ROWCOUNT>0    
      SET @return =103 -- record DELETED    
     ELSE    
      SET @return = 121 -- trying active record deletion     
   END  
END  
  
  ELSE  
  SET @return = 102 -- reconrd does not exists    
  
  END
  RETURN @return;  
  
GO
/****** Object:  StoredProcedure [dbo].[spd_lu_Level]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
CREATE PROCEDURE [dbo].[spd_lu_Level]  
(  
@Id INT  
)  
AS  
BEGIN  
Declare @return INT 
Declare @catCount INT 
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  IF EXISTS (SELECT TOP 1 1 FROM lu_Level  WHERE Id=@Id AND Status = 1)    
   BEGIN
  SET @catCount =   
   (SELECT COUNT(*) FROM CourseMaster  WHERE LevelId=@Id)+
   (SELECT COUNT(*) FROM ResourceMaster  WHERE LevelId=@Id);  


   IF(@catCount > 0)  
    SET @return = 121  
   ELSE  
   BEGIN  
    Update lu_Level set Status = 0 where Id=@Id  
     IF @@ROWCOUNT>0    
      SET @return =103 -- record DELETED    
     ELSE    
      SET @return = 121 -- trying active record deletion     
   END  
END  
  
  ELSE  
  SET @return = 102 -- reconrd does not exists    
  
  END
  RETURN @return;  
GO
/****** Object:  StoredProcedure [dbo].[spd_Notifications]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spd_Notifications] 
	(
	@UserID INT,
	@NotificationId INT
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
		DECLARE @Return INT
    -- Insert statements for procedure here
UPDATE Notifications SET IsDelete = 1, DeletedDate = GETDATE() WHERE ID = @NotificationId AND UserId = @UserID

SET @Return = 103

RETURN @Return
END
GO
/****** Object:  StoredProcedure [dbo].[spd_UnAssignedQRC]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--spd_UnAssignedQRC 1
CREATE PROCEDURE [dbo].[spd_UnAssignedQRC]   
(  
@Id INT  
)  
AS  
BEGIN   
Declare @return INT  



if exists(select top 1 1 from QRCUserMapping where qrcid = @Id and Active = 1)
begin 
 SET @return =104 -- record DELETED   
end

else
begin 
Update QRCMaster SET Active = 0 WHERE Id = @Id
 SET @return =103 -- record DELETED   

end

RETURN @return  
    
END  
GO
/****** Object:  StoredProcedure [dbo].[spd_WhiteListingURLs]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



--  exec spd_WhiteListingURLs 1
CREATE PROCEDURE [dbo].[spd_WhiteListingURLs]   
(  
@Id INT  
)  
AS  
BEGIN   
Declare @return INT  
  IF EXISTS (SELECT TOP 1 1 FROM WhiteListingURLs  WHERE Id=@Id)  
  BEGIN  
    
   Update WhiteListingURLs SET IsActive = 0 WHERE Id = @Id    
     SET @return =103 -- record DELETED       
  END  
  
  ELSE  
  SET @return = 102 -- reconrd does not exists  
    RETURN @return  
    
END  
GO
/****** Object:  StoredProcedure [dbo].[spi_AddUserBookmarkedContent]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spi_AddUserBookmarkedContent]
(
@UserId INT,
@ContentId INT,
@ContentType INT
)
AS 
BEGIN
DECLARE @Return INT = 0
IF NOT EXISTS (SELECT TOP 1 1 FROM UserBookmarks WHERE UserId = @UserId AND ContentType = @ContentType AND ContentId = @ContentId)
BEGIN
	IF(@ContentType = 1)
	BEGIN
		IF EXISTS (SELECT TOP 1 1 FROM CourseMaster (NOLOCK) WHERE Id = @ContentId)
		BEGIN
			BEGIN TRY	
				INSERT INTO UserBookmarks (UserId,ContentId,ContentType) VALUES (@UserId,@ContentId,1)
				SET @Return = 100
			END TRY
			BEGIN CATCH	
				SET @Return = 102
			END CATCH
		END
	END
	ELSE IF(@ContentType =  2)
	BEGIN
		IF EXISTS (SELECT TOP 1 1 FROM ResourceMaster (NOLOCK) WHERE Id = @ContentId)
		BEGIN
			BEGIN TRY	
				INSERT INTO UserBookmarks (UserId,ContentId,ContentType) VALUES (@UserId,@ContentId,2)
				SET @Return = 100
			END TRY
			BEGIN CATCH	
				SET @Return = 102
			END CATCH
		END
	END
END
ELSE
BEGIN
	SET @Return = 105
END
 SELECT u.Id,u.ContentId,u.ContentType, 
	 (CASE WHEN u.ContentType = 1 THEN 
	 (SELECT Title FROM CourseMaster WHERE Id = u.ContentId) ELSE 
	 (SELECT Title FROM ResourceMaster WHERE Id = u.ContentId) END) AS Title,
	 (CASE WHEN u.ContentType = 1 THEN 
	 (SELECT CourseDescription FROM CourseMaster WHERE Id = u.ContentId) ELSE 
	 (SELECT ResourceDescription FROM ResourceMaster WHERE Id = u.ContentId) END) AS [Description],
	 (CASE WHEN u.ContentType = 1 THEN 
	 (SELECT Thumbnail FROM CourseMaster WHERE Id = u.ContentId) ELSE 
	 (SELECT Thumbnail FROM ResourceMaster WHERE Id = u.ContentId) END) AS Thumbnail
	  FROM UserBookmarks u (NOLOCK) WHERE u.UserId =  @UserId
 RETURN @Return
END
GO
/****** Object:  StoredProcedure [dbo].[spi_Announcements]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spi_Announcements]  
(  
@Text NVARCHAR(200),  
@CreatedBy INT,
@Active BIT,
@Text_Ar NVARCHAR(200)
)  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
  
 SET NOCOUNT ON;  
 DECLARE @Return INT;  
 DECLARE @Id INT;  
 INSERT INTO Announcements  
    (  
    [Text],  
    CreatedBy,  
    CreatedOn,  
    UpdatedBy,  
    UpdatedOn,  
    Active,
	[Text_Ar] 
    )  
  VALUES(  
    @Text,  
    @CreatedBy,  
    GETDATE(),  
    @CreatedBy,  
    GETDATE(),  
    @Active,
	@Text_Ar
  );  
  
  SET @Id = SCOPE_IDENTITY()  
  
SELECT an.[Text],an.[Text_Ar],um.FirstName,um.LastName,an.Active,an.CreatedOn,an.UpdatedOn  
from Announcements an INNER JOIN UserMaster um  
ON an.Createdby = um.Id   
INNER JOIN UserMaster um1  
ON an.UpdatedBy = um1.Id WHERE an.id = @Id  
SET @Return = 100;  
  
RETURN @Return;  
END  

GO
/****** Object:  StoredProcedure [dbo].[spi_CommunityApproveReject]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spi_CommunityApproveReject]-- 101,10361,2,1,'test'  
(  
  @UserId INT,  
  @ContentId INT,  
  @ContentType INT,  
  @Status INT,  
  @Comments NVARCHAR(MAX) = NULL     
)  
AS  
BEGIN  
DECLARE @Date DATETIME = GETDATE()  
DECLARE @notify_user INT = 0;  
  
 IF NOT EXISTS(SELECT TOP 1 1 FROM CommunityCheckMaster WHERE UserId = @UserId AND ContentId = @ContentId AND ContentType = @ContentType AND IsCurrent = 1)  
 BEGIN  
 --print 'test'
 INSERT INTO CommunityCheckMaster(UserId,ContentId,ContentType,Status,Comments) VALUES (@UserId,@ContentId,@ContentType,@Status,@Comments)  
   DECLARE @Count INT = 0,@CountRejected INT = 0, @CountApproved INT = 0  
   SELECT @CountRejected = RejectCount, @CountApproved = ApproveCount FROM CommunityApproveRejectCount  
   IF(@Status = 1)  
   BEGIN   
 SELECT @Count = COUNT(ID) FROM CommunityCheckMaster WHERE ContentId = @ContentId AND ContentType = @ContentType  
 AND [Status] = 1  
 IF @Count = @CountApproved  
 BEGIN  
  IF @ContentType = 1  
  BEGIN  
   UPDATE CourseMaster SET IsApproved = 1, CommunityBadge = 1 WHERE Id = @ContentId  
   SELECT Email,(FirstName + ' ' + LastName) AS UserName,PortalLanguageId FROM UserMaster WHERE Id = (SELECT CreatedBy FROM CourseMaster  
   WHERE Id = @ContentId)  
   select @notify_user=CreatedBy from CourseMaster where Id=@ContentId  
   exec spi_Notifications @ContentId,1,'Course Approval','Course has been approved during community check',3,1,@Date,0,0,@notify_user,NULL,@Comments,NULL  
  END  
  ELSE  
  BEGIN  
   UPDATE ResourceMaster SET IsApproved = 1, CommunityBadge = 1 WHERE Id = @ContentId  
   SELECT Email,(FirstName + ' ' + LastName) AS UserName,PortalLanguageId FROM UserMaster WHERE Id = (SELECT CreatedBy FROM ResourceMaster  
   WHERE Id = @ContentId)  
   select @notify_user=CreatedBy from ResourceMaster where Id=@ContentId  
   exec spi_Notifications @ContentId,2,'Resource Approval','Resource has been approved during community check',6,1,@Date,0,0,@notify_user,NULL,@Comments,NULL  
  END  
  RETURN 203  
 END  
   END  
   ELSE IF (@Status = 0)  
   BEGIN  
 SELECT @Count = COUNT(ID) FROM CommunityCheckMaster WHERE ContentId = @ContentId AND ContentType = @ContentType  
 AND [Status] = 0  
 IF @Count = @CountRejected  
 BEGIN  
  IF @ContentType = 1  
  BEGIN  
   UPDATE CourseMaster SET IsApproved = 0, IsDraft = 1 WHERE Id = @ContentId  
   SELECT Email,(FirstName + ' ' + LastName) AS UserName,PortalLanguageId FROM UserMaster WHERE Id = (SELECT CreatedBy FROM CourseMaster  
   WHERE Id = @ContentId)  
   select @notify_user=CreatedBy from CourseMaster where Id=@ContentId  
  
   select X.comment As 'Comments' from(  
    select distinct top 1   
     STUFF((SELECT distinct t2.Comments + ';;'  
      from CommunityCheckMaster t2  
      where t1.ContentId = t2.ContentId  
      and t2.Status = 0  
      FOR XML PATH(''), TYPE  
      ).value('.', 'NVARCHAR(MAX)')   
     ,1,0,'') comment  
   from CommunityCheckMaster t1 where t1.ContentId=@ContentId and t1.ContentType=@ContentType and t1.Status=0)X  
  
   exec spi_Notifications @ContentId,1,'Course Rejection','Course has been rejected during community check',2,0,@Date,0,0,@notify_user,NULL,@Comments,NULL  
  END  
  ELSE  
  BEGIN  
   UPDATE ResourceMaster SET IsApproved = 0, IsDraft = 1 WHERE Id = @ContentId  
   SELECT Email,(FirstName + ' ' + LastName) AS UserName,PortalLanguageId FROM UserMaster WHERE Id = (SELECT CreatedBy FROM ResourceMaster  
   WHERE Id = @ContentId)  
   select @notify_user=CreatedBy from ResourceMaster where Id=@ContentId  
  
   select X.comment As 'Comments' from(  
    select distinct top 1   
     STUFF((SELECT distinct t2.Comments + ';;'  
      from CommunityCheckMaster t2  
      where t1.ContentId = t2.ContentId  
      and t2.Status = 0  
      FOR XML PATH(''), TYPE  
      ).value('.', 'NVARCHAR(MAX)')   
     ,1,0,'') comment  
   from CommunityCheckMaster t1 where t1.ContentId=@ContentId and t1.ContentType=@ContentType and t1.Status=0)X  
     
   exec spi_Notifications @ContentId,2,'Resource Rejection','Resource has been rejected during community check',5,0,@Date,0,0,@notify_user,NULL,@Comments,NULL  
  END  
  UPDATE CommunityCheckMaster SET IsCurrent = 0 WHERE ContentId = @ContentId AND ContentType = @ContentType  
  RETURN 204  
 END  
   END  
   IF @Status = 1  
   RETURN 115  
   ELSE  
   RETURN 204  
   END  
   ELSE  
   RETURN 205   
END  
  
GO
/****** Object:  StoredProcedure [dbo].[spi_ContactUs]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[spi_ContactUs]
(
@FirstName NVARCHAR(100),
@LastName NVARCHAR(100),
@Email NVARCHAR(100),
@Telephone NVARCHAR(50),
@Subject NVARCHAR(100),
@Message NVARCHAR(max)
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.

	SET NOCOUNT ON;
	DECLARE @Return INT;
	INSERT INTO ContactUs
				(
				FirstName,
				LastName,
				Email,
				Telephone,
				Subject,
				Message,
				CreatedOn
				)
		VALUES(
				@FirstName,
				@LastName,
				@Email,
				@Telephone,
				@Subject,
				@Message,
				GETDATE()
		);
SET @Return = 100;
 SELECT ID, FirstName + ' ' + LastName as UserName,
Email from UserMaster WHERE IsAdmin = 1 and Active =1;

RETURN @Return;
END
GO
/****** Object:  StoredProcedure [dbo].[spi_ContentApproval]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spi_ContentApproval] --1890,2,0,'Test',1,10301,'/test'
    @ContentApprovalId INT,
	@ContentType INT,
	@Status INT,
	@Comment NVARCHAR(500) = NULL,
	@CreatedBy INT,
	@ContentId INT,
	@EmailUrl NVARCHAR(400)

AS
BEGIN


DECLARE @TotalApproval Decimal;
DECLARE @TotalAssignes Decimal;
DECLARE  @PublishContent INT = 2;
DECLARE @TotalRejections Decimal;
Declare @return INT
Declare @Date Datetime
Declare @Subject NVARCHAR(400)
Declare @Content NVARCHAR(MAX)
DECLARE @MessageTypeId INT
DECLARE @UserID INT
DECLARE @RejectionReasons NVARCHAR(MAX)

SET @Date = getdate()

IF  EXISTS(
	SELECT 1 FROM ContentApproval
	WHERE ID = @ContentApprovalId
	--ContentId =@ContentId AND
		  --ContentType = @ContentType 
		 -- [Status] = @Status AND
		 --AND AssignedTo = @CreatedBy
		 )
		  

BEGIN

 UPDATE ContentApproval SET 
		[Status]  = @Status,
		Comment = @Comment,
		UpdatedBy = @CreatedBy,
		UpdatedOn = GETDATE(),
		ApprovedBy = @CreatedBy,
		ApprovedDate = GETDATE()
		
		WHERE ID = @ContentApprovalId

		IF(@Status = 1 AND @ContentType = 1)
		BEGIN
		SELECT @MessageTypeId = ID FROM MessageType WHERE [Type] = 'Course Approval'
		END;
		ELSE IF (@Status = 1 AND @ContentType = 2)
		BEGIN
			SELECT @MessageTypeId = ID FROM MessageType WHERE [Type] = 'Resource Approval'
		END
		ELSE IF (@Status = 0 AND @ContentType = 1)
		BEGIN
			SELECT @MessageTypeId = ID FROM MessageType WHERE [Type] = 'Course Rejection'
		END
		ELSE IF (@Status = 0 AND @ContentType = 2)
		BEGIN
			SELECT @MessageTypeId = ID FROM MessageType WHERE [Type] = 'Resource Rejection'
		END

		IF(@ContentType = 1)
			 BEGIN
			 SELECT @Subject = Title,@Content = CourseDescription,@UserID = CreatedBy FROM CourseMaster WHERE ID = @ContentId
			 END;
			 ELSE
			 BEGIN
			 SELECT @Subject = Title,@Content = ResourceDescription,@UserID = CreatedBy FROM ResourceMaster WHERE ID = @ContentId
			 END;

		--exec spi_Notifications @ContentId,@ContentType,@Subject,@Content,@MessageTypeId,@Status,@Date,0,0,@UserID,@CreatedBy,@Comment,@EmailUrl
		 -- ContentId =@ContentId AND
		 -- ContentType = @ContentType 
		 ---- [Status] = @Status AND
		 --AND AssignedTo = @CreatedBy;
		
		SELECT @TotalApproval = COUNT(Id) FROM ContentApproval
		  WHERE 
		  ContentId = @ContentId AND
		  ContentType = @ContentType AND
		  Status = 1

		SELECT @TotalRejections = COUNT(Id) FROM ContentApproval
		  WHERE 
		  ContentId = @ContentId AND
		  ContentType = @ContentType AND
		  Status = 0

		SELECT  @TotalAssignes= COUNT(Id) FROM ContentApproval
		  WHERE 
		  ContentId = @ContentId AND
		  ContentType = @ContentType --AND
		--  Status = 1
		  IF(@TotalAssignes/2<=@TotalApproval)
		  BEGIN
		     IF(@ContentType = 1)
			 BEGIN
			 UPDATE CourseMaster SET IsApproved = 1,CommunityBadge = 1 WHERE ID = @ContentId
			 SELECT @MessageTypeId = ID FROM MessageType WHERE [Type] = 'Course Promotion'
			 END;
			 ELSE
			 BEGIN
			 UPDATE ResourceMaster SET IsApproved = 1,CommunityBadge = 1 WHERE ID = @ContentId
			 SELECT @MessageTypeId = ID FROM MessageType WHERE [Type] = 'Resource Promotion'
			 END;
			 SET @PublishContent = 1;

			 
			 exec spi_Notifications @ContentId,@ContentType,@Subject,@Content,@MessageTypeId,@Status,@Date,0,0,@UserID,@CreatedBy,@Comment,@EmailUrl
		  END;

		  ELSE IF(@TotalAssignes/2<=@TotalRejections)

		  BEGIN
		     IF(@ContentType = 1)
			 BEGIN
			 UPDATE CourseMaster SET IsDraft = 1 WHERE ID = @ContentId
			 END;
			 ELSE
			 BEGIN
			 UPDATE ResourceMaster SET IsDraft = 1 WHERE ID = @ContentId
			 END;
			 SET @PublishContent = 0;

			 

			 exec spi_Notifications @ContentId,@ContentType,@Subject,@Content,@MessageTypeId,@Status,@Date,0,0,@UserID,@CreatedBy,@comment,@EmailUrl
		  END;
		  ELSE
			BEGIN
				exec spi_Notifications @ContentId,@ContentType,@Subject,@Content,@MessageTypeId,@Status,@Date,0,0,@UserID,@CreatedBy,@Comment,@EmailUrl,0
			END;
		 	SELECT Id,
TitleId,
FirstName +' ' +LastName as UserName,
Email,PortalLanguageId,@PublishContent as PublishContent

 FROM UserMaster WHERE ID in (SELECT CreatedBy FROM ContentApproval WHERE Id=@ContentApprovalId) AND IsEmailNotification = 1

 select X.comment As 'Comments' from(
			 select distinct top 1 
			  STUFF((SELECT distinct t2.Comment + ';;'
					 from ContentApproval t2
					 where t1.ContentId = t2.ContentId
					 and t2.Status = 0
						FOR XML PATH(''), TYPE
						).value('.', 'NVARCHAR(MAX)') 
					,1,0,'') comment
			from ContentApproval t1 where t1.ContentId=@ContentId and t1.ContentType=@ContentType and t1.Status=0)X
END;

			 
			
SET @return = 105 -- reconrd does not exists
	  RETURN @return
END

GO
/****** Object:  StoredProcedure [dbo].[spi_ContentDownloadInfo]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spi_ContentDownloadInfo]  
 (  
 @ContentId INT,  
 @ContentTypeId INT,  
 @DownloadedBy INT  
  
 )  
AS  
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @DownloadCount INT;
		INSERT INTO ContentDownloadInfo(  
		 ContentId,  
		 ContentTypeId,  
		 DownloadedBy,  
		 DownloadedOn  
   
		)  
  
	   VALUES  
		(  
		@ContentId,  
		@ContentTypeId,  
		@DownloadedBy,  
		GETDATE()  
		)  
				BEGIN
					SELECT @DownloadCount=Count(Id) FROM ContentDownloadInfo WHERE ContentId = @ContentId and ContentTypeId = @ContentTypeId
				END
				    IF @ContentTypeId = 1  
						BEGIN      
						Update CourseMaster SET DownloadCount = @DownloadCount WHERE ID = @ContentId       
						END;   
					else
						BEGIN      
						Update ResourceMaster SET DownloadCount = @DownloadCount WHERE ID = @ContentId       
						END;   
				RETURN 100;
END


GO
/****** Object:  StoredProcedure [dbo].[spi_ContentSharedInfo]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spi_ContentSharedInfo]
	(
	@ContentId INT,
	@ContentTypeId INT,
	@SocialMediaName VARCHAR(MAX) = NULL,
	@CreatedBy INT

	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @SharedCount INT;
    INSERT INTO ContentSharedInfo(
					ContentId,
					ContentTypeId,
					SocialMediaName,
					CreatedBy,
					CreatedOn
	
				)

			VALUES
				(
				@ContentId,
				@ContentTypeId,
				@SocialMediaName,
				@CreatedBy,
				GETDATE()
				)
				BEGIN
					SELECT @SharedCount=Count(Id) FROM ContentSharedInfo WHERE ContentId = @ContentId and ContentTypeId = @ContentTypeId
				END
				    IF @ContentTypeId = 1  
						BEGIN      
						Update CourseMaster SET SharedCount = @SharedCount WHERE ID = @ContentId       
						END;   
					else
						BEGIN      
						Update ResourceMaster SET SharedCount = @SharedCount WHERE ID = @ContentId       
						END;   
				RETURN 100;
END


GO
/****** Object:  StoredProcedure [dbo].[spi_CourseAssociatedFiles]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spi_CourseAssociatedFiles]
	(
	@CourseId INT,
    @AssociatedFile NVARCHAR(500),
	@FileName NVARCHAR(400)
	)
AS
BEGIN
	
	SET NOCOUNT ON;

	DECLARE @Result INT;
	IF NOT EXISTS(SELECT TOP 1 1 FROM CourseAssociatedFiles WHERE AssociatedFile=@AssociatedFile AND CourseId=@CourseId)
	BEGIN
		INSERT INTO CourseAssociatedFiles(CourseId,AssociatedFile,CreatedOn,FileName,IsInclude)
		VALUES(@CourseId,@AssociatedFile,GETDATE(),@FileName,0)
		SET @Result = 100;
    END

	ELSE
	BEGIN
	SET @Result = 107;
	END
	
	RETURN @Result;
END


GO
/****** Object:  StoredProcedure [dbo].[spi_CourseEnrollment]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spi_CourseEnrollment]
	(
	@CourseId INT,
    @UserId INT	
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @Return INT;

 IF NOT EXISTS(SELECT TOP 1 1 FROM CourseEnrollment WHERE CourseId = @CourseId AND UserId = @UserId AND Active=1)
 BEGIN
 INSERT INTO CourseEnrollment(
 CourseId,
UserId,
CreatedOn,
UpdatedOn,
Active
 )
 VALUES(
 @CourseId,
 @UserId,
 GETDATE(),
 GETDATE(),
 1
 )

 SET @Return = 100;
 END
ELSE

BEGIN
 SET @Return = 107;




 END;

 RETURN @Return;
END
GO
/****** Object:  StoredProcedure [dbo].[spi_CourseTest]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spi_CourseTest]
	(
	@CourseId INT,
	@TestName NVARCHAR(100),
	@UT_Questions [UT_QuestionsForContent] Readonly,
	@UT_AnswerOptions [UT_AnswerOptions] Readonly,
	@CreatedBy INT
	)
AS
BEGIN
	
DECLARE @QuestionCount INT;
DECLARE @TestID INT;
DECLARE @QID INT;
DECLARE @i INT;
SET @i = 1;


IF NOT EXISTS(SELECT TOP 1 1 FROM tests WHERE TestName = @TestName and CourseID = @CourseId)

BEGIN
INSERT INTO tests(TestName,
CreatedBy,
CreatedOn,
CourseId) VALUES(
@TestName,
@CreatedBy,
GETDATE(),
@CourseId);

SET @TestID = SCOPE_IDENTITY();
SET @QuestionCount = (SELECT Count(*) FROM @UT_Questions)


WHILE @i <= @QuestionCount

BEGIN

INSERT INTO questions
(
QuestionText,
TestId,
Media,
FileName
)

SELECT QuestionText,@TestID,Media,[FileName] FROM @UT_Questions WHERE QuestionId = @i

SET @QID = SCOPE_IDENTITY();

INSERT INTO answeroptions(QuestionId,
AnswerOption,
CorrectOption)

SELECT 
@QID,
OptionText,
CorrectOption FROM @UT_AnswerOptions WHERE QuestionId =(SELECT  QuestionId FROM @UT_Questions WHERE QuestionId = @i)
SET @i= @i+1;
END;



END;

RETURN 100;
END

GO
/****** Object:  StoredProcedure [dbo].[spi_CourseTestTake]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================


CREATE PROCEDURE [dbo].[spi_CourseTestTake]
(	@UT_AnswerOptions [UT_AnswerOptions] Readonly,
	@UserId INT,
	@CourseId INT
	
	)
AS
BEGIN
DECLARE @output TABLE (ID int)
	INSERT INTO UserCourseTests
	(
	QuestionId,
AnswerId,
UserId,
CreatedOn,
CourseId
	)
	OUTPUT inserted.ID INTO @output
	SELECT QuestionId,AnswerId, @UserId, GETDATE(),@CourseId FROM @UT_AnswerOptions
	SELECT ID FROM @output
	RETURN 100;
END

GO
/****** Object:  StoredProcedure [dbo].[spi_DeleteUserBookmarkedContent]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spi_DeleteUserBookmarkedContent]
(
@UserId INT,
@ContentId INT,
@ContentType INT
)
AS 
BEGIN
DECLARE @Return INT = 0
IF EXISTS (SELECT TOP 1 1 FROM UserBookmarks WHERE UserId = @UserId AND ContentType = @ContentType AND ContentId = @ContentId)
BEGIN	
	BEGIN TRY	
		DELETE FROM UserBookmarks WHERE UserId = @UserId AND ContentType = @ContentType AND ContentId = @ContentId
		SET @Return = 103
	END TRY
	BEGIN CATCH	
		SET @Return = 114
	END CATCH
END
ELSE
BEGIN
	SET @Return = 102
END
 SELECT u.Id,u.ContentId,u.ContentType, 
	 (CASE WHEN u.ContentType = 1 THEN 
	 (SELECT Title FROM CourseMaster WHERE Id = u.ContentId) ELSE 
	 (SELECT Title FROM ResourceMaster WHERE Id = u.ContentId) END) AS Title,
	 (CASE WHEN u.ContentType = 1 THEN 
	 (SELECT CourseDescription FROM CourseMaster WHERE Id = u.ContentId) ELSE 
	 (SELECT ResourceDescription FROM ResourceMaster WHERE Id = u.ContentId) END) AS [Description],
	 (CASE WHEN u.ContentType = 1 THEN 
	 (SELECT Thumbnail FROM CourseMaster WHERE Id = u.ContentId) ELSE 
	 (SELECT Thumbnail FROM ResourceMaster WHERE Id = u.ContentId) END) AS Thumbnail
	  FROM UserBookmarks u (NOLOCK) WHERE u.UserId =  @UserId
 RETURN @Return
END
GO
/****** Object:  StoredProcedure [dbo].[spi_lu_Educational_Standard]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- spi_lu_Educational_Standard @Standard='Standard4',@CreatedBy=66
CREATE PROCEDURE [dbo].[spi_lu_Educational_Standard]
	(
	@Standard NVARCHAR(200),
	@Standard_Ar NVARCHAR(200) = NULL,
	@CreatedBy INT
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @ID INT;
	IF NOT EXISTS(SELECT *  FROM lu_Educational_Standard WHERE [Standard] = @Standard AND Status =1)
	BEGIN

INSERT INTO lu_Educational_Standard(
[Standard],
Active,
CreatedBy,
UpdatedBy,
CreatedOn,
UpdatedOn,
Standard_Ar,
Status
)
VALUES
(
@Standard,
1,
@CreatedBy,
@CreatedBy,
GETDATE(),
GETDATE(),
@Standard_Ar,
1
)

SET @ID =(SELECT scope_identity())
--print @ID
	SELECT es.Id,
es.[Standard],
es.[Standard_Ar],
CONCAT(c.FirstName, '', c.LastName) as CreatedBy,  
CONCAT(l.FirstName, '', l.LastName) as UpdatedBy, 
es.CreatedOn,
es.UpdatedOn,
es.Active FROM lu_Educational_Standard  es
   INNER join UserMaster c  on es.CreatedBy= c.Id  
   INNER join UserMaster l on es.UpdatedBy =l.Id 
   WHERE es.Status = 1 AND es.id = @ID

RETURN 100;
	END;
	ELSE
	BEGIN
	RETURN 107;
	END;

END
GO
/****** Object:  StoredProcedure [dbo].[spi_lu_Educational_Use]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

-- spi_lu_Educational_Use @EducationalUse='Standard4',@CreatedBy=66
CREATE PROCEDURE [dbo].[spi_lu_Educational_Use]
	(
	@EducationalUse NVARCHAR(200),
	@EducationalUse_Ar NVARCHAR(200) = NULL,
	@CreatedBy INT
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @ID INT;
	IF NOT EXISTS(SELECT * FROM lu_Educational_Use WHERE EducationalUse = @EducationalUse AND Status =1)
	BEGIN

INSERT INTO lu_Educational_Use(
EducationalUse,
EducationalUse_Ar,
CreatedBy,
CreatedOn,
UpdatedOn,
UpdatedBy,
Active,
Status
)
VALUES
(
@EducationalUse,
@EducationalUse_Ar,
@CreatedBy,
GETDATE(),
GETDATE(),
@CreatedBy,
1,
1
)

SET @ID =(SELECT scope_identity())
print @ID
	SELECT eu.Id,
eu.EducationalUse,
eu.EducationalUse_Ar,
CONCAT(c.FirstName, '', c.LastName) as CreatedBy,  
CONCAT(l.FirstName, '', l.LastName) as UpdatedBy, 
eu.CreatedOn,
eu.UpdatedOn,
eu.Active FROM lu_Educational_Use  eu 
   INNER join UserMaster c  on eu.CreatedBy= c.Id  
   INNER join UserMaster l on eu.UpdatedBy =l.Id 
   WHERE eu.Status = 1 AND eu.id = @ID

RETURN 100;
	END;
	ELSE
	BEGIN
	RETURN 107;
	END;

END
GO
/****** Object:  StoredProcedure [dbo].[spi_lu_Level]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

-- spi_lu_Level @Level='Level1',@CreatedBy=66
CREATE PROCEDURE [dbo].[spi_lu_Level]
	(
	@Level NVARCHAR(200),
	@Level_Ar NVARCHAR(200) = NULL,
	@CreatedBy INT
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @ID INT;
	IF NOT EXISTS(SELECT * FROM lu_Level WHERE [Level] = @Level AND Status =1)
	BEGIN

INSERT INTO lu_Level(
[Level],
[Level_Ar],
CreatedBy,
CreatedOn,
UpdatedOn,
UpdatedBy,
Active,
Status
)
VALUES
(
@Level,
@Level_Ar,
@CreatedBy,
GETDATE(),
GETDATE(),
@CreatedBy,
1,
1
)

SET @ID =(SELECT scope_identity())
print @ID
	SELECT el.Id,
el.[Level],
el.Level_Ar,
CONCAT(c.FirstName, '', c.LastName) as CreatedBy,  
CONCAT(l.FirstName, '', l.LastName) as UpdatedBy, 
el.CreatedOn,
el.UpdatedOn,
el.Active FROM lu_Level  el 
   INNER join UserMaster c  on el.CreatedBy= c.Id  
   INNER join UserMaster l on el.UpdatedBy =l.Id 
   WHERE el.Status = 1 AND el.id = @ID

RETURN 100;
	END;
	ELSE
	BEGIN
	RETURN 107;
	END;

END
GO
/****** Object:  StoredProcedure [dbo].[spi_MoEApproveReject]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--select * from MoECheckMaster where contentid = 10223
CREATE   PROCEDURE [dbo].[spi_MoEApproveReject]
(
  @UserId INT,
  @ContentId INT,
  @ContentType INT,
  @Status INT,
  @Comments NVARCHAR(MAX) = NULL  ,
  @EmailUrl NVARCHAR(MAX) 
)
AS
BEGIN
DECLARE @Date DATETIME = GETDATE()
DECLARE @notify_user INT = 0;
IF NOT EXISTS(SELECT TOP 1 1 FROM MoECheckMaster WHERE UserId = @UserId AND ContentId = @ContentId AND ContentType = @ContentType AND IsCurrent = 1)
	BEGIN
   INSERT INTO MoECheckMaster(UserId,ContentId,ContentType,Status,Comments) VALUES (@UserId,@ContentId,@ContentType,@Status,@Comments)
	IF(@Status = 1)
	BEGIN
		IF @ContentType = 1
		BEGIN
			
			UPDATE CourseMaster SET IsApproved = 1, MoEBadge = 1 WHERE Id = @ContentId 
			select @notify_user=CreatedBy from CourseMaster where Id=@ContentId
			
			exec spi_Notifications @ContentId,1,'Course Approval','Course has been published during expert check',3,1,@Date,0,0,@notify_user,NULL,@Comments,@EmailUrl  
			SELECT Email,(FirstName + ' ' + LastName) AS UserName,PortalLanguageId,IsEmailNotification FROM UserMaster WHERE Id = (SELECT CreatedBy FROM CourseMaster
			WHERE Id = @ContentId)
		END
		ELSE
		BEGIN
			UPDATE ResourceMaster SET IsApproved = 1, MoEBadge = 1 WHERE Id = @ContentId
		    select @notify_user=CreatedBy from ResourceMaster where Id=@ContentId
			exec spi_Notifications @ContentId,2,'Resource Approval','Resource has been published during expert check',6,1,@Date,0,0,@notify_user,NULL,@Comments,@EmailUrl  
			SELECT Email,(FirstName + ' ' + LastName) AS UserName,PortalLanguageId,IsEmailNotification FROM UserMaster WHERE Id = (SELECT CreatedBy FROM ResourceMaster
			WHERE Id = @ContentId)
		END
		RETURN 203
	END
	ELSE
	BEGIN
		IF @ContentType = 1
		BEGIN
			UPDATE CourseMaster SET IsApproved = 0, IsDraft = 1 WHERE Id = @ContentId
			select @notify_user=CreatedBy from CourseMaster where Id=@ContentId
			exec spi_Notifications @ContentId,1,'Course Rejection','Course has been rejected during expert check',2,0,@Date,0,0,@notify_user,NULL,@Comments,@EmailUrl  
			SELECT Email,(FirstName + ' ' + LastName) AS UserName,PortalLanguageId,IsEmailNotification FROM UserMaster WHERE Id = (SELECT CreatedBy FROM CourseMaster
			WHERE Id = @ContentId)
		END
		ELSE
		BEGIN
			UPDATE ResourceMaster SET IsApproved = 0, IsDraft = 1 WHERE Id = @ContentId
			select @notify_user=CreatedBy from ResourceMaster where Id=@ContentId
			exec spi_Notifications @ContentId,2,'Resource Rejection','Resource has been rejected during expert check',5,0,@Date,0,0,@notify_user,NULL,@Comments,@EmailUrl  
			SELECT Email,(FirstName + ' ' + LastName) AS UserName,PortalLanguageId,IsEmailNotification FROM UserMaster WHERE Id = (SELECT CreatedBy FROM ResourceMaster
			WHERE Id = @ContentId)
		END
		UPDATE MoECheckMaster SET IsCurrent = 0 WHERE ContentId = @ContentId AND ContentType = @ContentType
		AND UserId =  @UserId
		RETURN 204
	END
   
   END
   ELSE
   RETURN 205
END


GO
/****** Object:  StoredProcedure [dbo].[spi_Notifications]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- =============================================  
-- Author:  <Author,,Name>  
-- Create date: <Create Date,,>  
-- Description: <Description,,>  
-- =============================================  
CREATE PROCEDURE [dbo].[spi_Notifications]  
 (  
 @ReferenceId INT,  
 @ReferenceTypeId INT,  
 @Subject NVARCHAR(400),  
 @Content NVARCHAR(MAX),  
 @MessageTypeId INT,  
 @IsApproved BIT,  
 @CreatedDate DATETIME,  
 @IsRead BIT,  
 @IsDelete BIT,
 @UserId INT,
 @Reviewer INT = NULL,
 @Comment NVARCHAR(MAX)= NULL,
 @EmailUrl NVARCHAR(400) = NULL,
 @Status INT = 1
 )  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
 INSERT INTO notifications (ReferenceId,  
        ReferenceTypeId,  
        Subject,  
        Content,  
        MessageTypeId,  
        IsApproved,  
        CreatedDate,  
        IsRead,  
        IsDelete,
		UserId,
		Reviewer,
		Comment,
		[Url],
		Status)  
      VALUES(  
        @ReferenceId,  
        @ReferenceTypeId,  
        @Subject,  
        @Content,  
        @MessageTypeId,  
        @IsApproved,  
        @CreatedDate,  
        0,  
        0,
		@UserId,
		@Reviewer,
		@Comment,
		@EmailUrl,
		@Status
        )  
  
    -- Insert statements for procedure here  
-- SELECT * from notifications  
END  

GO
/****** Object:  StoredProcedure [dbo].[spi_ResourceAssociatedFiles]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

  
-- =============================================  
-- Author:  <Author,,Name>  
-- Create date: <Create Date,,>  
-- Description: <Description,,>  
-- =============================================  
CREATE PROCEDURE [dbo].[spi_ResourceAssociatedFiles]  
 (  
 @ResourceId INT,  
    @AssociatedFile NVARCHAR(500),  
 @FileName NVARCHAR(400),
 @IsInclude BIT = 0  
 )  
AS  
BEGIN  
   
 SET NOCOUNT ON;  
  
 DECLARE @Result INT;  
 IF NOT EXISTS(SELECT TOP 1 1 FROM ResourceAssociatedFiles WHERE AssociatedFile=@AssociatedFile AND ResourceId=@ResourceId)  
 BEGIN  
  INSERT INTO ResourceAssociatedFiles(ResourceId,AssociatedFile,UploadedDate,FileName,IsInclude)  
  VALUES(@ResourceId,@AssociatedFile,GETDATE(),@FileName,0)  
  SET @Result = 100;  
    END  
  
 ELSE  
 BEGIN  
 SET @Result = 107;  
 END  
   
 RETURN @Result;  
END  
  

GO
/****** Object:  StoredProcedure [dbo].[spi_SensoryApproveReject]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spi_SensoryApproveReject]    
(    
  @UserId INT,    
  @ContentId INT,    
  @ContentType INT,    
  @Status BIT,  
  @Comments NVARCHAR(MAX) = NULL        
)    
AS    
BEGIN  
DECLARE @Date DATETIME = GETDATE()
DECLARE @notify_user INT = 0;
IF (@ContentType = 1)
BEGIN
	SELECT (FirstName + ' ' + LastName) AS UserName, Email,PortalLanguageId,IsEmailNotification FROM UserMaster WHERE Id = (SELECT CreatedBy FROM CourseMaster WHERE 
	Id = @ContentId) 
END
ELSE
BEGIN
	SELECT (FirstName + ' ' + LastName) AS UserName, Email,PortalLanguageId,IsEmailNotification FROM UserMaster WHERE Id = (SELECT CreatedBy FROM ResourceMaster WHERE 
	Id = @ContentId) 
END  

IF NOT EXISTS(SELECT TOP 1 1 FROM SensoryCheckMaster WHERE UserId = @UserId AND ContentId = @ContentId AND ContentType = @ContentType AND IsCurrent = 1)
	BEGIN
   INSERT INTO SensoryCheckMaster(UserId,ContentId,ContentType,Status,Comments) VALUES (@UserId,@ContentId,@ContentType,@Status,@Comments) 
      
   IF(@Status = 0)    
    BEGIN    
			IF @ContentType = 1 
			BEGIN 
					UPDATE CourseMaster SET IsApproved = 0,IsApprovedSensory = 0, IsDraft = 1 WHERE Id = @ContentId
					UPDATE SensoryCheckMaster SET IsCurrent = 0 WHERE ContentId = @ContentId AND ContentType = @ContentType 
					select @notify_user=CreatedBy from CourseMaster where Id=@ContentId
					exec spi_Notifications @ContentId,1,'Course Rejection','Course has been rejected during sensitivity check',2,0,@Date,0,0,@notify_user,NULL,@Comments,NULL
					RETURN 204; 
			END
			ELSE  
			BEGIN
					UPDATE ResourceMaster SET IsApproved = 0 ,IsApprovedSensory = 0, IsDraft = 1 WHERE Id = @ContentId 
					UPDATE SensoryCheckMaster SET IsCurrent = 0 WHERE ContentId = @ContentId AND ContentType = @ContentType
					select @notify_user=CreatedBy from ResourceMaster where Id=@ContentId
					exec spi_Notifications @ContentId,2,'Resource Rejection','Resource has been rejected during sensitivity check',5,0,@Date,0,0,@notify_user,NULL,@Comments,NULL
					RETURN 204; 
			END
			--AND UserId =  @UserId
			END 
			ELSE
			BEGIN
				IF @ContentType = 1 
				BEGIN 
					UPDATE CourseMaster SET IsApproved = 1,IsApprovedSensory = 1, IsDraft = 0 WHERE Id = @ContentId
					UPDATE SensoryCheckMaster SET IsCurrent = 0 WHERE ContentId = @ContentId AND ContentType = @ContentType 
					select @notify_user=CreatedBy from CourseMaster where Id=@ContentId
					exec spi_Notifications @ContentId,1,'Course Approval','Course has been approved during sensitivity check',3,1,@Date,0,0,@notify_user,NULL,@Comments,NULL
					RETURN 115; 
			END
			ELSE  
			BEGIN
					UPDATE ResourceMaster SET IsApproved = 1 ,IsApprovedSensory = 1, IsDraft = 0 WHERE Id = @ContentId
					UPDATE SensoryCheckMaster SET IsCurrent = 0 WHERE ContentId = @ContentId AND ContentType = @ContentType 
					select @notify_user=CreatedBy from ResourceMaster where Id=@ContentId
					exec spi_Notifications @ContentId,2,'Resource Approval','Resource has been approved during sensitivity check',6,1,@Date,0,0,@notify_user,NULL,@Comments,NULL
					RETURN 115; 
			END
			END   
   END
END
RETURN 205 

GO
/****** Object:  StoredProcedure [dbo].[spi_Visiters]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- spi_Visiters 1
CREATE PROCEDURE [dbo].[spi_Visiters]
	(
	@UserId INT = NULL
	)
AS
BEGIN
	INSERT INTO Visiters (UserId, CreatedOn)
	VALUES(@UserId,GETDATE())

	RETURN 100;
END
GO
/****** Object:  StoredProcedure [dbo].[spi_WebPageContent]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================    
-- Author:  <Author,,Name>    
-- Create date: <Create Date,,>    
-- Description: <Description,,>    
-- =============================================    
CREATE PROCEDURE [dbo].[spi_WebPageContent]    
 (    
 @PageID INT,     
 @PageContent NVARCHAR(MAX),    
 @PageContent_Ar NVARCHAR(MAX),   
 @CreatedBy INT    
 )    
AS    
BEGIN    
    
IF NOT EXISTS(SELECT TOP 1 1 FROM WebPageContent WHERE PageId =@PageID )    
BEGIN    
 INSERT INTO WebPageContent(PageID, PageContent,PageContent_Ar,CreatedBy,CreatedOn)    
VALUES(    
@PageID,    
@PageContent,    
@PageContent_Ar,    
@CreatedBy    
,GETDATE()    
)    
RETURN 100    
END    
    
ELSE    
    
BEGIN    
RETURN 105    
END    
    
    
END 
GO
/****** Object:  StoredProcedure [dbo].[sps_AdminRejectedList]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[sps_AdminRejectedList]
(  
  @PageNo int = 1,        
  @PageSize int = 5      
)  
AS  
BEGIN  
 DECLARE @return INT, @query NVARCHAR(MAX)   
 DECLARE @start INT, @end INT        
 SET @start = (@PageNo - 1) * @PageSize + 1        
 SET @end = @PageNo * @PageSize  
 CREATE table #tempData        
 (        
 ContentId INT,         
 ContentType INT,  
 Title NVARCHAR(MAX),  
 Category NVARCHAR(50) 
 )      
 INSERT INTO #tempData  
 SELECT cm.ID,1,cm.Title,cat.Name from CourseMaster cm inner join CategoryMaster cat on cm.categoryid = cat.id 
 WHERE cm.IsApproved = 0
  INSERT INTO #tempData  
 SELECT cm.ID,2,cm.Title,cat.Name from ResourceMaster cm inner join CategoryMaster cat on cm.categoryid = cat.id
  WHERE cm.IsApproved = 0
IF EXISTS(SELECT TOP 1 1 FROM #tempData)  
BEGIN  
SET @query =        
 ';with sqlpaging as (        
 SELECT         
 Rownumber = ROW_NUMBER() OVER(order by  ContentId desc) ,        
 * FROM #tempData)        
        
 select          
  top ('+CAST(@PageSize AS VARCHAR(50))+') *,          
  (select max(rownumber) from sqlpaging) as           
  Totalrows    from sqlpaging          
 where Rownumber between '+CAST(@start AS VARCHAR(50))+' and '+CAST(@end AS VARCHAR(50))+''        
        
 EXEC sp_executesql @query        
   RETURN 105;    
   drop table #tempData   
 END  
 ELSE  
 BEGIN  
  RETURN 113  
 END  
END 
GO
/****** Object:  StoredProcedure [dbo].[sps_Announcements]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================    
-- Author:  <Author,,Name>    
-- Create date: <Create Date,,>    
-- Description: <Description,,>    
-- =============================================    
CREATE PROCEDURE [dbo].[sps_Announcements]    
     
AS    
BEGIN    
 -- SET NOCOUNT ON added to prevent extra result sets from    
 -- interfering with SELECT statements.    
 SET NOCOUNT ON;    
    
 IF EXISTS(SELECT TOP 1 1 FROM Announcements)    
 BEGIN    
   SELECT es.Id,    
es.Text,  
es.Text_Ar,    
CONCAT(c.FirstName, '', c.LastName) as CreatedBy,      
CONCAT(l.FirstName, '', l.LastName) as UpdatedBy,     
es.CreatedOn,    
es.UpdatedOn,    
es.Active FROM Announcements  es    
   INNER join UserMaster c  on es.CreatedBy= c.Id      
   INNER join UserMaster l on es.UpdatedBy =l.Id  
   WHERE es.Active = 1
ORDER BY es.CreatedOn DESC    
     return 105     
    
  END    
    
  ELSE    
  BEGIN    
    RETURN 102 -- reconrd does not exists      
  END;    
END    
GO
/****** Object:  StoredProcedure [dbo].[sps_AnnouncementsById]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[sps_AnnouncementsById]-- 1
	(
	@Id INT
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
IF Exists(select TOP 1 1 from Announcements WHERE Id=@Id)   
 BEGIN  
SELECT el.Id,
					el.[Text],
					el.Text_Ar,
					CONCAT(c.FirstName, '', c.LastName) as CreatedBy,  
					CONCAT(l.FirstName, '', l.LastName) as UpdatedBy, 
					el.CreatedOn,
					el.UpdatedOn,
					el.Active FROM Announcements  el 
			   INNER join UserMaster c  on el.CreatedBy= c.Id  
			   INNER join UserMaster l on el.UpdatedBy =l.Id 
			   WHERE el.Active = 1 

--ON eu.updatedby = um.id

AND el.Id = @Id
ORDER BY el.CreatedOn DESC
  return 105 -- record exists  
  END
ELSE 

BEGIN
	
  return 102 -- reconrd does not exists  
END
END
GO
/****** Object:  StoredProcedure [dbo].[sps_AnnouncementsToUser]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

-- =============================================    
-- Author:  <Author,,Name>    
-- Create date: <Create Date,,>    
-- Description: <Description,,>    
-- =============================================    
    
-- sps_AnnouncementsToUser 115    
CREATE PROCEDURE [dbo].[sps_AnnouncementsToUser]     
 (    
 @UserId INT,    
 @PageNo int = 1,    
 @PageSize int = 5    
 )    
AS    
BEGIN    
 -- SET NOCOUNT ON added to prevent extra result sets from    
 -- interfering with SELECT statements.    
 SET NOCOUNT ON;    
 DECLARE @LastLogin DATETIME    
 Declare @return INT    
declare @start int, @end int    
set @start = (@PageNo - 1) * @PageSize + 1    
set @end = @PageNo * @PageSize    
    
 SET @LastLogin = (SELECT LastLogin FROM UserMaster WHERE ID = @UserId)    
    -- Insert statements for procedure here    
     
     
;with sqlpaging as (    
 SELECT Rownumber = ROW_NUMBER() OVER(ORDER BY an.CreatedOn desc),    
 an.Id,    
 an.[Text],   
 an.Text_Ar, 
 um.FirstName,    
 um.LastName,    
 an.CreatedOn,    
 --UpdatedBy,    
 an.UpdatedOn,    
 an.Active,    
 @LastLogin AS LastLogin    
 from Announcements an INNER JOIN UserMaster um    
 ON an.CreatedBy = um.Id    
 WHERE an.Active = 1  
 )    
   select    
 top (@PageSize) *,    
 (select max(rownumber) from sqlpaging) as     
 Totalrows    
from sqlpaging    
where Rownumber between @start and @end    
ORDER BY CreatedOn DESC
    
    
 SET @return = 105 -- reconrd does not exists    
   RETURN @return    
END   
GO
/****** Object:  StoredProcedure [dbo].[sps_CategoryNotInQRC]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sps_CategoryNotInQRC]
	
AS
BEGIN
	SELECT  cm.Id,
cm.Name,
cm.Name_Ar,
cast(cm.CreatedBy as nvarchar(50)) as CreatedBy,
cm.CreatedOn,
cast(cm.UpdatedBy as nvarchar(50)) as UpdatedBy,
cm.UpdatedOn,
cm.Active from CategoryMaster cm  LEFT JOIN QRCCategory qc
	ON qc.CategoryId = cm.Id WHERE qc.ID IS NULL AND Active = 1

	RETURN 105;
END
GO
/****** Object:  StoredProcedure [dbo].[sps_CommunityApprovedByUser]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sps_CommunityApprovedByUser]
(
  @UserId INT,
  @PageNo int = 1,      
  @PageSize int = 5    
)
AS
BEGIN
	DECLARE @return INT, @query NVARCHAR(MAX) 
	DECLARE @start INT, @end INT      
	SET @start = (@PageNo - 1) * @PageSize + 1      
	SET @end = @PageNo * @PageSize
	CREATE table #tempData      
	(      
	ContentId INT,       
	ContentType INT,
	Title NVARCHAR(MAX) 
	)    
	INSERT INTO #tempData
	SELECT c.ContentId, c.ContentType,(CASE WHEN c.ContentType = 1 THEN (SELECT Title FROM CourseMaster WHERE Id = c.ContentId) 
	ELSE ((SELECT Title FROM ResourceMaster WHERE Id = c.ContentId))END) FROM CommunityCheckMaster c WHERE [Status] = 1 
IF EXISTS(SELECT TOP 1 1 FROM #tempData)
BEGIN
SET @query =      
	';with sqlpaging as (      
	SELECT       
	Rownumber = ROW_NUMBER() OVER(order by  ContentId desc) ,      
	* FROM #tempData)      
      
	select        
	 top ('+CAST(@PageSize AS VARCHAR(50))+') *,        
	 (select max(rownumber) from sqlpaging) as         
	 Totalrows    from sqlpaging        
	where Rownumber between '+CAST(@start AS VARCHAR(50))+' and '+CAST(@end AS VARCHAR(50))+''      
      
	EXEC sp_executesql @query      
	  RETURN 105;  
	  drop table #tempData 
	END
	ELSE
	BEGIN
		RETURN 113
	END
END
GO
/****** Object:  StoredProcedure [dbo].[sps_ContactUs]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- =============================================    
-- Author:  <Author,,Name>    
-- Create date: <Create Date,,>    
-- Description: <Description,,>    
-- =============================================    
CREATE PROCEDURE [dbo].[sps_ContactUs] 
 @PageNo int = 1,    
 @PageSize int = 5    
AS    
BEGIN    
 -- SET NOCOUNT ON added to prevent extra result sets from    
 -- interfering with SELECT statements.    
 SET NOCOUNT ON;    
Declare @return INT    
declare @start int, @end int    
set @start = (@PageNo - 1) * @PageSize + 1    
set @end = @PageNo * @PageSize    
    -- Insert statements for procedure here    
    
 ;with sqlpaging as (    
 SELECT Rownumber = ROW_NUMBER() OVER(ORDER BY CreatedOn desc),    
   c.Id,    
   c.FirstName,    
   c.LastName,    
   c.Email,    
   c.Telephone,    
   c.Subject,    
   c.Message,    
   c.RepliedText,
   (SELECT FirstName + ' ' +LastName  FROM UserMaster WHERE Id = c.RepliedBy) AS RepliedBy,
   c.RepliedBy AS RepliedById,
   c.RepliedOn,
   c.CreatedOn,  
   CASE  
    WHEN c.IsReplied is NULL  THEN 0  
    ELSE 1  
 END AS IsReplied  
   from ContactUs c  
       
       
   )    
   select    
 top (@PageSize) *,    
 (select max(rownumber) from sqlpaging) as     
 Totalrows    
from sqlpaging    
where Rownumber between @start and @end    
ORDER BY CreatedOn    
    
    
 SET @return = 105 -- reconrd does not exists    
   RETURN @return    
END 

GO
/****** Object:  StoredProcedure [dbo].[sps_ContentApprovalForVerifiers]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================    
-- Author:  <Author,,Name>    
-- Create date: <Create Date,,>    
-- Description: <Description,,>    
-- =============================================    
CREATE PROCEDURE [dbo].[sps_ContentApprovalForVerifiers]    
 (    
  @PageNo int = 1,      
    @PageSize int = 5     
 )    
AS    
BEGIN    
 -- SET NOCOUNT ON added to prevent extra result sets from    
 -- interfering with SELECT statements.    
 SET NOCOUNT ON;    
    
Declare @return INT, @query NVARCHAR(MAX)      
declare @start int, @end int      
set @start = (@PageNo - 1) * @PageSize + 1      
set @end = @PageNo * @PageSize         
CREATE table #tempData      
(      
ContentId INT,       
Title NVARCHAR(500),      
CreatedOn DATETIME,      
ContentType INT    
)      
      
  INSERT INTO #tempData(ContentId,Title,CreatedOn,ContentType)    
  SELECT ID,Title,CreatedOn,1 FROM CourseMaster WHERE IsApproved IS NULL and IsDraft =0
    
   INSERT INTO #tempData(ContentId,Title,CreatedOn,ContentType)    
  SELECT ID,Title,CreatedOn,2 FROM ResourceMaster WHERE IsApproved IS NULL  and IsDraft =0  
    
 SET @query =      
';with sqlpaging as (      
SELECT       
Rownumber = ROW_NUMBER() OVER(order by  CreatedOn desc) ,      
* FROM #tempData)      
      
select        
 top ('+CAST(@PageSize AS VARCHAR(50))+') *,        
 (select max(rownumber) from sqlpaging) as         
 Totalrows    from sqlpaging        
where Rownumber between '+CAST(@start AS VARCHAR(50))+' and '+CAST(@end AS VARCHAR(50))+''      
      
EXEC sp_executesql @query      
  RETURN 105;  
  drop table #tempData    
END 
GO
/****** Object:  StoredProcedure [dbo].[sps_ContentFileNames]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
    
/*___________________________________________________________________________________________________      
      
Copyright (c) YYYY-YYYY XYZ Corp. All Rights Reserved      
WorldWide.      
      
$Revision:  $1.0      
$Author:    $ Prince Kumar    
&Date       June 06, 2019    
    
Ticket: Ticket URL    
      
PURPOSE:       
This store procedure will get the reports of top contirbutors ,reviever, and other dashboard data    
    
EXEC sps_ContentFileNames 36 ,2  
___________________________________________________________________________________________________*/    
CREATE PROCEDURE [dbo].[sps_ContentFileNames]     
(    
@ContentId INT,    
@ContentType INT    
)    
AS    
BEGIN    
    
SET NOCOUNT ON;    
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;    
    
    
    
    
DECLARE @Keyword NVARCHAR(100)    
DECLARE @Return INT    
    
IF(@ContentType=1)    
BEGIN    
SELECT Id,    
CourseId as ContentId,    
AssociatedFile,
FileName FROM [dbo].[CourseAssociatedFiles] WHERE courseid =@ContentId    
END    
    
ELSE    
BEGIN    
BEGIN    
select  Id,    
ResourceId as ContentId,    
AssociatedFile,
FileName from [dbo].[ResourceAssociatedFiles] WHERE resourceid =@ContentId    
END    
END;    
    
 SET @Return = 105;    
    
 RETURN @Return;    
    
END; 
GO
/****** Object:  StoredProcedure [dbo].[sps_ContentForApproval]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

--exec sps_ContentForApproval  @UserId=15,@QrcId =7,@CategoryID=2,@PageNo=1,@PageSize=5
CREATE PROCEDURE [dbo].[sps_ContentForApproval]
	(
	@QrcId INT = NULL,
	@CategoryID INT = NULL,
	@UserId INT,
	@PageNo int = 1,
    @PageSize int = 5
	)
AS
BEGIN
Declare @return INT, @query NVARCHAR(MAX)
declare @start int, @end int
set @start = (@PageNo - 1) * @PageSize + 1
set	@end = @PageNo * @PageSize

CREATE table #tempData
(
ContentId INT,
ContentApprovalID INT,
TITLE NVARCHAR(200),
CreatedOn DATETIME,
ContentType INT,
CategoryId INT
)

IF(@CategoryID IS NULL AND @QrcId IS NULL)
BEGIN
--print 'both null'
    INSERT INTO #tempData(ContentId,ContentApprovalID,Title,CreatedOn,ContentType,CategoryId)
	SELECT DISTINCT cm.ID as ContentId,ca.Id as ContentApprovalID,cm.Title, cm.CreatedOn,1 As ContentType,cm.CategoryId FROM CourseMaster cm
	INNER JOIN ContentApproval ca ON cm.Id  = ca.ContentId WHERE ca.AssignedTo = @UserId AND ca.[Status] IS NULL AND ca.ContentType = 1
	AND cm.MoEBadge <> 1 AND cm.CommunityBadge <> 1
	UNION ALL
	SELECT DISTINCT cm.ID as ContentId,ca.Id as ContentApprovalID,cm.Title, cm.CreatedOn,2 As ContentType,cm.CategoryId  FROM ResourceMaster cm
	INNER JOIN ContentApproval ca ON cm.Id  = ca.ContentId WHERE ca.AssignedTo = @UserId AND ca.[Status] IS NULL AND ca.ContentType = 2
	AND cm.MoEBadge <> 1 AND cm.CommunityBadge <> 1
END;

ELSE IF( @QrcId IS NOT NULL AND @CategoryID IS NULL )
BEGIN
--print 'one null one not'
    INSERT INTO #tempData(ContentId,ContentApprovalID,Title,CreatedOn,ContentType,CategoryId)
    SELECT DISTINCT cm.ID as ContentId,ca.Id as ContentApprovalID,cm.Title, cm.CreatedOn,1 As ContentType,cm.CategoryId FROM CourseMaster cm
	INNER JOIN ContentApproval ca ON cm.Id  = ca.ContentId WHERE ca.AssignedTo = @UserId
	AND cm.CategoryId in(SELECT CategoryId FROM QRCUserMapping WHERE QRCId = @QrcId) AND ca.[Status] IS NULL AND ca.ContentType = 1
	AND cm.MoEBadge <> 1 AND cm.CommunityBadge <> 1
	UNION ALL
	SELECT DISTINCT cm.ID as ContentId,ca.Id as ContentApprovalID,cm.Title, cm.CreatedOn,2 As ContentType,cm.CategoryId  FROM ResourceMaster cm
	INNER JOIN ContentApproval ca ON cm.Id  = ca.ContentId WHERE ca.AssignedTo = @UserId
	AND cm.CategoryId in (SELECT CategoryId FROM QRCUserMapping WHERE QRCId = @QrcId) AND ca.[Status] IS NULL AND ca.ContentType = 2
	AND cm.MoEBadge <> 1 AND cm.CommunityBadge <> 1
END;

IF(@CategoryID IS NOT NULL AND @QrcId IS NOT NULL)
BEGIN

--print 'both not null'
    INSERT INTO #tempData(ContentId,ContentApprovalID,Title,CreatedOn,ContentType,CategoryId)
    SELECT DISTINCT cm.ID as ContentId,ca.Id as ContentApprovalID,cm.Title, cm.CreatedOn,1 As ContentType,cm.CategoryId FROM CourseMaster cm
	INNER JOIN ContentApproval ca ON cm.Id  = ca.ContentId WHERE ca.AssignedTo = @UserId AND ca.ContentType = 1
	AND cm.CategoryId = @CategoryID AND ca.[Status] IS NULL AND cm.MoEBadge <> 1 AND cm.CommunityBadge <> 1
	UNION ALL
	SELECT DISTINCT cm.ID as ContentId,ca.Id as ContentApprovalID,cm.Title, cm.CreatedOn,2 As ContentType,cm.CategoryId  FROM ResourceMaster cm
	INNER JOIN ContentApproval ca ON cm.Id  = ca.ContentId WHERE ca.AssignedTo = @UserId AND ca.ContentType = 2
	AND cm.CategoryId = @CategoryID AND ca.[Status] IS NULL AND cm.MoEBadge <> 1 AND cm.CommunityBadge <> 1
END;

SET @query =
';with sqlpaging as (
SELECT 
Rownumber = ROW_NUMBER() OVER(order by  ContentApprovalID desc) ,
* FROM #tempData)

select  
 top ('+CAST(@PageSize AS VARCHAR(50))+') *,  
 (select max(rownumber) from sqlpaging) as   
 Totalrows  
from sqlpaging  
where Rownumber between '+CAST(@start AS VARCHAR(50))+' and '+CAST(@end AS VARCHAR(50))+''

EXEC sp_executesql @query

SET @return = 105 -- reconrd does not exists
	  RETURN @return
END
GO
/****** Object:  StoredProcedure [dbo].[sps_CourseAssociatedFiles]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================  
-- Author:  <Author,,Name>  
-- Create date: <Create Date,,>  
-- Description: <Description,,>  
-- =============================================  
CREATE PROCEDURE [dbo].[sps_CourseAssociatedFiles]   
 (  
 @CourseID INT  
 )  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
select * from [dbo].[CourseAssociatedFiles] WHERE CourseID= @CourseID  
  
  
select * from [dbo].[ResourceAssociatedFiles] WHERE ResourceId in  
(  
select resourceid from[dbo].[SectionResource] where sectionid in (select id from [dbo].[CourseSections] where courseid = @CourseID) )  
  
END  
GO
/****** Object:  StoredProcedure [dbo].[sps_CourseEnrollmentStatus]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sps_CourseEnrollmentStatus] 
	(
	@CourseId INT,
    @UserId INT	
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @Return INT;
    IF EXISTS(SELECT TOP 1 1 FROM CourseEnrollment WHERE CourseId = @CourseId AND UserId = @UserId AND Active=1)
	BEGIN
	     SELECT 
			 Id,
			CourseId,
			UserId,
			CreatedOn,
			UpdatedOn,
			Active
			FROM CourseEnrollment
		WHERE CourseId = @CourseId AND UserId = @UserId AND Active=1
		SET @Return = 105
	END
ELSE 
    BEGIN
	SELECT 0 as Id, 0 as Active
	SET @Return = 102
	END

	
 RETURN @Return;
END
GO
/****** Object:  StoredProcedure [dbo].[sps_CoursesByKeyword]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- sps_CoursesByKeyword @SearchKeyword ='test'  
CREATE PROCEDURE [dbo].[sps_CoursesByKeyword]   
(  
@SearchKeyword NVARCHAR(200) ,
 @PageNo int = 1,  
 @PageSize int = 5  
)  
AS  
BEGIN  
   
   declare @start int, @end int  
set @start = (@PageNo - 1) * @PageSize + 1  
set @end = @PageNo * @PageSize  


  
  ;with sqlpaging as (  
	 SELECT Rownumber = ROW_NUMBER() OVER(ORDER BY r.Id desc),
	 r.Id  
      ,r.Title  
      ,r.CategoryId, c.Name as CategoryName  
      ,SubCategoryId, sc.Name as SubCategoryName  
      ,Thumbnail  
      ,CourseDescription  
      ,Keywords  
      ,CourseContent       
      ,CopyRightId, cm.Title as CopyrightTitle, cm.Description as CopyrightDescription  
      ,IsDraft  
   , EducationId, edm.Name as EducationName  
   , ProfessionId, pm.Name as ProfessionName  
      , CONCAT(um.FirstName, '', um.LastName) as CreatedBy, um.Id as CreatedById  
      ,r.CreatedOn  
      ,IsApproved  
      ,Rating       
      ,ReportAbuseCount  
  FROM CourseMaster r  
   inner join CategoryMaster c on r.CategoryId = c.Id     
   left join SubCategoryMaster sc on r.SubCategoryId=sc.Id  
   left join CopyrightMaster cm on r.CopyRightId = cm.Id  
   left join EducationMaster edm on edm.Id= r.EducationId  
   left join ProfessionMaster pm on pm.Id= r.ProfessionId  
   inner join UserMaster um on r.CreatedBy =um.Id   
   WHERE r.Title Like '%'+@SearchKeyword+'%' AND r.IsApproved = 1  
    )  
   select  
 top (@PageSize) *,  
 (select max(rownumber) from sqlpaging) as   
 Totalrows  
from sqlpaging  
where Rownumber between @start and @end  
  
     
 IF @@ROWCOUNT>0  
      
 return 105 -- record exists  
  
 ELSE  
  return 102 -- reconrd does not exists  
END  
  
  
GO
/****** Object:  StoredProcedure [dbo].[sps_CoursesByUserId]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*___________________________________________________________________________________________________  
  
Copyright (c) YYYY-YYYY XYZ Corp. All Rights Reserved  
WorldWide.  
  
$Revision:  $1.0  
$Author:    $ Prince Kumar
&Date       June 06, 2019

Ticket: Ticket URL
  
PURPOSE:   
This store procedure will get all reasources based upon user id.

EXEC sps_OerDashboardReport 
___________________________________________________________________________________________________*/
CREATE PROCEDURE [dbo].[sps_CoursesByUserId] 
(
@UserId INT
)
AS
BEGIN

SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

DECLARE @Return INT;
	SELECT r.Id
      ,r.Title
      ,r.CategoryId, c.Name as CategoryName
      ,SubCategoryId, sc.Name as SubCategoryName
      ,Thumbnail
      ,CourseDescription
      ,Keywords
      ,CourseContent     
      ,CopyRightId, cm.Title as CopyrightTitle, cm.Description as CopyrightDescription
      ,IsDraft
	  , EducationId, edm.Name as EducationName
	  , ProfessionId, pm.Name as ProfessionName
      , CONCAT(um.FirstName, '', um.LastName) as CreatedBy, um.Id as CreatedById
      ,r.CreatedOn
      ,IsApproved
      ,Rating     
      ,ReportAbuseCount,ViewCount,AverageReadingTime,DownloadCount,SharedCount,ReadingTime
  FROM CourseMaster r
		 inner join CategoryMaster c on r.CategoryId = c.Id		 
		 left join SubCategoryMaster sc on r.SubCategoryId=sc.Id
		 left join CopyrightMaster cm on r.CopyRightId = cm.Id
		 left join EducationMaster edm on edm.Id= r.EducationId
		 left join ProfessionMaster pm on pm.Id= r.ProfessionId
		 inner join UserMaster um on r.CreatedBy =um.Id where um.Id=@UserId	Order by r.Id desc 


 SET @Return = 105;

 RETURN @Return;

END;
GO
/****** Object:  StoredProcedure [dbo].[sps_DashboardReportByUserId]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


  
    
/*___________________________________________________________________________________________________      
      
Copyright (c) YYYY-YYYY XYZ Corp. All Rights Reserved      
WorldWide.      
      
$Revision:  $1.0      
$Author:    $ Prince Kumar    
&Date       June 06, 2019    
    
Ticket: Ticket URL    
      
PURPOSE:       
This store procedure will get the reports of top contirbutors ,reviever, and other dashboard data    
    
EXEC sps_DashboardReportByUserId 11     
___________________________________________________________________________________________________*/    
CREATE PROCEDURE [dbo].[sps_DashboardReportByUserId]
(    
@UserId INT    
)     
AS    
BEGIN    
    
SET NOCOUNT ON;    
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;    
    
    
    
DECLARE @DraftCourses INT,    
  @DraftResources INT,    
  @PublishedCourses INT,    
  @PubishedResources INT,    
  @CourseToApprove INT,    
  @ResourceToApprove INT,    
  @Return INT,    
  @SharedResources INT,    
  @SharedCourses INT,    
  @DownloadedCourses INT,    
  @DownloadedResources INT    
    
    
    
CREATE table #tempData    
(    
Id INT,    
Title NVARCHAR(MAX),    
Description NVARCHAR(MAX),    
ContentType INT,    
Thumbnail NVARCHAR(500)    
    
)    
    
SET @DraftCourses =(select  count(id)  from coursemaster where createdby = @UserId and IsDraft = 1)    
SET @DraftResources = (select  count(id) from ResourceMaster where createdby = @UserId and IsDraft = 1)    
SET @PublishedCourses =(select  count(id) from coursemaster where createdby = @UserId and IsApproved = 1)    
SET @PubishedResources =(select  count(id)  from ResourceMaster where createdby = @UserId and IsApproved = 1)    
SET @CourseToApprove = (select  count(id) from CourseMaster Where IsApproved IS NULL and IsDraft = 0 and createdby = @UserId )    
SET @ResourceToApprove =(select  count(id) from ResourceMaster Where IsApproved IS NULL and IsDraft = 0 and createdby = @UserId ) 
    
SET @SharedResources = (select  count(id) from ContentSharedInfo where createdby = @UserId and ContentTypeId = 2 )    
SET @SharedCourses =(select  count(id)  from ContentSharedInfo where createdby = @UserId and ContentTypeId = 1 )    
    
SET @DownloadedResources = (select  count(id) from ContentDownloadInfo where DownloadedBy = @UserId and ContentTypeId = 2 )    
SET @DownloadedCourses =(select  count(id)  from ContentDownloadInfo where DownloadedBy = @UserId and ContentTypeId = 1 )    
    
    
    
INSERT INTO #tempData(Id,Title,Description,ContentType,Thumbnail)    
SELECT TOP 6 ID,Title,CourseDescription,1,Thumbnail from CourseMaster where createdby=@UserId and IsApproved = 1 order by CreatedOn desc    
    
INSERT INTO #tempData(Id,Title,Description,ContentType,Thumbnail)    
SELECT TOP 6 ID,Title,ResourceDescription,2,Thumbnail from ResourceMaster where createdby=@UserId and IsApproved = 1 order by CreatedOn  desc    
    
    
    
    
SELECT @DraftCourses as DraftCourses ,@DraftResources as DraftResources,@PublishedCourses as PublishedCourses,    
@PubishedResources as PubishedResources ,@CourseToApprove as CourseToApprove,@ResourceToApprove as ResourceToApprove,@SharedResources AS SharedResources, @SharedCourses AS SharedCourses,    
@DownloadedResources AS DownloadedResources, @DownloadedCourses AS DownloadedCourses,    
 FirstName,    
MiddleName,    
LastName,    
Photo,    
ProfileDescription FROM UserMaster WHERE ID=@UserId    
    
SELECT Id,Title,Description,ContentType,Thumbnail FROM #tempData    
    
 SET @Return = 105;    
    
 RETURN @Return;    
    
END;    
GO
/****** Object:  StoredProcedure [dbo].[sps_EducationalUse]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================  
-- Author:  <Author,,Name>  
-- Create date: <Create Date,,>  
-- Description: <Description,,>  
-- =============================================  
CREATE PROCEDURE [dbo].[sps_EducationalUse]  
   
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
 IF EXISTS(SELECT TOP 1 1 FROM lu_Educational_Use WHERE Status = 1)  
 BEGIN  
   SELECT eu.Id,  
eu.EducationalUse,  
eu.EducationalUse_Ar,  
CONCAT(c.FirstName, '', c.LastName) as CreatedBy,    
CONCAT(l.FirstName, '', l.LastName) as UpdatedBy,   
eu.CreatedOn,  
eu.UpdatedOn,  
eu.Active FROM lu_Educational_Use  eu   
   INNER join UserMaster c  on eu.CreatedBy= c.Id    
   INNER join UserMaster l on eu.UpdatedBy =l.Id   
   WHERE eu.Status = 1 --and c.Active = 1 and l.Active = 1 --added Active check 
  
     return 105   
  
  END  
  
  ELSE  
  BEGIN  
    RETURN 102 -- reconrd does not exists    
  END;  
END  
GO
/****** Object:  StoredProcedure [dbo].[sps_EducationalUseById]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sps_EducationalUseById]-- 1
	(
	@Id INT
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
IF Exists(select TOP 1 1 from lu_Educational_Use WHERE Id=@Id)   
 BEGIN  
	SELECT eu.Id,
eu.EducationalUse,
eu.EducationalUse_Ar,
CONCAT(c.FirstName, '', c.LastName) as CreatedBy,  
CONCAT(l.FirstName, '', l.LastName) as UpdatedBy, 
eu.CreatedOn,
eu.UpdatedOn,
eu.Active FROM lu_Educational_Use  eu 
   INNER join UserMaster c  on eu.CreatedBy= c.Id  
   INNER join UserMaster l on eu.UpdatedBy =l.Id 
   WHERE eu.Active = 1
--ON eu.updatedby = um.id

AND eu.Id = @Id
  return 105 -- record exists  
  END
ELSE 

BEGIN
	
  return 102 -- reconrd does not exists  
END




END
GO
/****** Object:  StoredProcedure [dbo].[sps_EmailNotificationStatus]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sps_EmailNotificationStatus]
	(
	@UserId INT
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

SELECT ID, IsEmailNotification FROM UserMaster WHERE ID = @UserId
RETURN 105
END
GO
/****** Object:  StoredProcedure [dbo].[sps_GetCategoryByQrcId]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- exe sps_GetCategoryByQrcId @QrcID= 9
-- =============================================
CREATE PROCEDURE [dbo].[sps_GetCategoryByQrcId] 
	@QrcID int
AS
BEGIN

Declare @return INT
	SELECT Id,
Name,
--CreatedBy,
CreatedOn,
--UpdatedBy,
UpdatedOn
Active FROM CategoryMaster WHERE ID in (
	
	SELECT CategoryID FROM QRCCategory WHERE QRCID =@QrcID
	)
	SET @return =105 -- creation success
	RETURN @return
END
GO
/****** Object:  StoredProcedure [dbo].[sps_GetCommunityApproveRejectCount]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sps_GetCommunityApproveRejectCount]
AS
BEGIN
SELECT ApproveCount,RejectCount,LastUpdatedOn,LastUpdatedBy FROM CommunityApproveRejectCount
RETURN 105
END
GO
/****** Object:  StoredProcedure [dbo].[sps_GetCommunityCategories]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
     -- sps_GetCommunityCategories   2,1, 10
Create PROCEDURE [dbo].[sps_GetCommunityCategories]   
(    
  @UserId INT,    
  @PageNo int = 1,          
  @PageSize int = 5         
)    
AS    
BEGIN    
DECLARE @Subjects NVARCHAR(MAX) = (SELECT SubjectsInterested FROM UserMaster WHERE id = @UserId)    
DECLARE @return INT, @query NVARCHAR(MAX) , @ContentId INT, @ContentType INT         
DECLARE @start INT, @end INT          
SET @start = (@PageNo - 1) * @PageSize + 1          
SET @end = @PageNo * @PageSize    
CREATE table #tempData          
(          
ContentId INT,           
ContentType INT,    
Title NVARCHAR(MAX),    
CategoryId INT
)        
INSERT INTO #tempData    
SELECT c.Id, 1, c.Title, (SELECT Id FROM CategoryMaster WHERE Id = c.CategoryId) FROM CourseMaster c WHERE (c.IsApproved IS NULL OR (IsApproved = 1 AND IsApprovedSensory = 1)) AND c.IsDraft = 0    
AND c.CategoryId IN (SELECT Id FROM CategoryMaster WHERE [Name] IN (SELECT VALUE FROM STRING_SPLIT(@Subjects,','))and Active = 1)    
AND c.CreatedBy <> @UserId       
UNION     
SELECT c.Id, 2, c.Title, (SELECT Id FROM CategoryMaster WHERE Id = c.CategoryId) FROM ResourceMaster c WHERE (c.IsApproved IS NULL OR (IsApproved = 1 AND IsApprovedSensory = 1)) AND c.IsDraft = 0    
AND c.CategoryId IN (SELECT Id FROM CategoryMaster WHERE [Name] IN (SELECT VALUE FROM STRING_SPLIT(@Subjects,','))and Active = 1)    
AND c.CreatedBy <> @UserId       
    
DECLARE @CountsRejected INT = 0, @CountApproved INT = 0    
SELECT @CountsRejected = RejectCount, @CountApproved = ApproveCount FROM CommunityApproveRejectCount    
    
IF EXISTS(SELECT TOP 1 1 FROM #tempData)    
BEGIN    
DECLARE temp_cursor CURSOR FOR         
SELECT ContentId,ContentType        
FROM #tempData      
      
      
OPEN temp_cursor        
      
FETCH NEXT FROM temp_cursor         
INTO @ContentId,@ContentType        
      
      
WHILE @@FETCH_STATUS = 0        
BEGIN        
 IF EXISTS (SELECT TOP 1 1 FROM CommunityCheckMaster WHERE ContentId = @ContentId AND ContentType = @ContentType AND UserId = @UserID )    
 BEGIN    
 DELETE FROM #tempData WHERE ContentId = @ContentId AND ContentType = @ContentType    
 END       
 --IF EXISTS (SELECT TOP 1 1 FROM SensoryCheckMaster WHERE ContentId = @ContentId AND ContentType = @ContentType AND Status = 0)    
 --BEGIN    
 --DELETE FROM #tempData WHERE ContentId = @ContentId AND ContentType = @ContentType    
 --END     
     
 --IF EXISTS (SELECT TOP 1 1 FROM MoECheckMaster WHERE ContentId = @ContentId AND ContentType = @ContentType AND Status = 0)    
 --BEGIN    
 --DELETE FROM #tempData WHERE ContentId = @ContentId AND ContentType = @ContentType    
 --END        
FETCH NEXT FROM temp_cursor         
INTO @ContentId,@ContentType            
       
END         
CLOSE temp_cursor;        
DEALLOCATE temp_cursor    
    
IF EXISTS(SELECT TOP 1 1 FROM #tempData)    
BEGIN    
SELECT distinct cm.id,cm.Name,cm.Name_Ar from #tempData t INNER JOIN CategoryMaster cm
on t.CategoryId = cm.Id and cm.Active=1

          
   RETURN 105;      
   drop table #tempData     
 END    
 ELSE    
 BEGIN    
  RETURN 113    
 END    
END    
ELSE    
BEGIN    
 RETURN 113    
END    
END 
GO
/****** Object:  StoredProcedure [dbo].[sps_GetCommunityCheckList]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

  -- sps_GetCommunityCheckList   2,1,10,1
CREATE PROCEDURE [dbo].[sps_GetCommunityCheckList]  
(  
  @UserId INT,  
  @PageNo int = 1,        
  @PageSize int = 5,
  @CategoryId INT = NULL
)  
AS  
BEGIN  
DECLARE @Subjects NVARCHAR(MAX) = (SELECT SubjectsInterested FROM UserMaster WHERE id = @UserId)  
DECLARE @return INT, @query NVARCHAR(MAX) , @ContentId INT, @ContentType INT       
DECLARE @start INT, @end INT        
SET @start = (@PageNo - 1) * @PageSize + 1        
SET @end = @PageNo * @PageSize  
CREATE table #tempData        
(        
ContentId INT,         
ContentType INT,  
Title NVARCHAR(MAX),  
Category NVARCHAR(400),
CategoryId INT
)      
INSERT INTO #tempData  
SELECT c.Id, 1, c.Title, (SELECT Name FROM CategoryMaster WHERE Id = c.CategoryId),c.CategoryId FROM CourseMaster c WHERE (c.IsApproved IS NULL OR (IsApproved = 1 AND IsApprovedSensory = 1)) AND c.IsDraft = 0  
AND c.CategoryId IN (SELECT Id FROM CategoryMaster WHERE [Name] IN (SELECT VALUE FROM STRING_SPLIT(@Subjects,',')))  
AND c.CreatedBy <> @UserId AND c.CategoryId  = @CategoryId
UNION   
SELECT c.Id, 2, c.Title, (SELECT Name FROM CategoryMaster WHERE Id = c.CategoryId),c.CategoryId FROM ResourceMaster c WHERE (c.IsApproved IS NULL OR (IsApproved = 1 AND IsApprovedSensory = 1)) AND c.IsDraft = 0  
AND c.CategoryId IN (SELECT Id FROM CategoryMaster WHERE [Name] IN (SELECT VALUE FROM STRING_SPLIT(@Subjects,',')))  
AND c.CreatedBy <> @UserId AND c.CategoryId  = @CategoryId   
  
DECLARE @CountsRejected INT = 0, @CountApproved INT = 0  
SELECT @CountsRejected = RejectCount, @CountApproved = ApproveCount FROM CommunityApproveRejectCount  
  
IF EXISTS(SELECT TOP 1 1 FROM #tempData)  
BEGIN  
DECLARE temp_cursor CURSOR FOR       
SELECT ContentId,ContentType      
FROM #tempData    
    
    
OPEN temp_cursor      
    
FETCH NEXT FROM temp_cursor       
INTO @ContentId,@ContentType      
    
    
WHILE @@FETCH_STATUS = 0      
BEGIN      
 IF EXISTS (SELECT TOP 1 1 FROM CommunityCheckMaster WHERE ContentId = @ContentId AND ContentType = @ContentType AND UserId = @UserID )  
 BEGIN  
 DELETE FROM #tempData WHERE ContentId = @ContentId AND ContentType = @ContentType  
 END     
 --IF EXISTS (SELECT TOP 1 1 FROM SensoryCheckMaster WHERE ContentId = @ContentId AND ContentType = @ContentType AND Status = 0)  
 --BEGIN  
 --DELETE FROM #tempData WHERE ContentId = @ContentId AND ContentType = @ContentType  
 --END   
   
 --IF EXISTS (SELECT TOP 1 1 FROM MoECheckMaster WHERE ContentId = @ContentId AND ContentType = @ContentType AND Status = 0)  
 --BEGIN  
 --DELETE FROM #tempData WHERE ContentId = @ContentId AND ContentType = @ContentType  
 --END      
FETCH NEXT FROM temp_cursor       
INTO @ContentId,@ContentType          
     
END       
CLOSE temp_cursor;      
DEALLOCATE temp_cursor  
  
IF EXISTS(SELECT TOP 1 1 FROM #tempData)  
BEGIN  
SET @query =        
 ';with sqlpaging as (        
 SELECT         
 Rownumber = ROW_NUMBER() OVER(order by  ContentId desc) ,        
 * FROM #tempData)        
        
 select          
  top ('+CAST(@PageSize AS VARCHAR(50))+') *,          
  (select max(rownumber) from sqlpaging) as           
  Totalrows    from sqlpaging          
 where Rownumber between '+CAST(@start AS VARCHAR(50))+' and '+CAST(@end AS VARCHAR(50))+'' --and sqlpaging.CategoryId ='+CAST(@CategoryId AS INT)+''      
        
 EXEC sp_executesql @query        
   RETURN 105;    
   drop table #tempData   
 END  
 ELSE  
 BEGIN  
  RETURN 113  
 END  
END  
ELSE  
BEGIN  
 RETURN 113  
END  
END  
GO
/****** Object:  StoredProcedure [dbo].[sps_GetContentCountInfo]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

-- sps_GetContentCountInfo @ContentId=NULL,@ContentTypeId=1,@UserId=1
CREATE PROCEDURE [dbo].[sps_GetContentCountInfo] 
	(
	@ContentId INT = NULL,
	@ContentTypeId INT ,
	@UserId INT = NULL
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @ContentCount INT,
	        @UserSharedCount INT;
    -- Insert statements for procedure here
	IF(@ContentId <> '')
	BEGIN
	SELECT @ContentCount=Count(Id) FROM ContentSharedInfo WHERE ContentId = @ContentId AND ContentTypeId = @ContentTypeId

	END
	IF(@UserId <> '')
	BEGIN
		SELECT @UserSharedCount=Count(Id) FROM ContentSharedInfo WHERE CreatedBy = @UserId;
	END

	SELECT @ContentCount as ContentCount , @UserSharedCount as UserSharedCount;
	RETURN 105;
END
GO
/****** Object:  StoredProcedure [dbo].[sps_GetCourseTest]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
-- sps_GetCourseTest @CourseId =20023
CREATE PROCEDURE [dbo].[sps_GetCourseTest]
	(
	@CourseId INT
	)
AS
BEGIN
	SELECT  Id,
		TestName
		 from Tests WHERE CourseId = @CourseId
	
	
	select Id,
QuestionText,
TestId,
Media,FileName from Questions WHERE TestId IN (SELECT  Id
		 from Tests WHERE CourseId = @CourseId
	)

	select Id,
QuestionId,
AnswerOption,
CorrectOption from AnswerOptions WHERE QuestionId in (
	select Id from Questions WHERE TestId IN (SELECT  Id
		 from Tests WHERE CourseId = @CourseId
	)
	)
	return 105
END

GO
/****** Object:  StoredProcedure [dbo].[sps_GetMoEcategories]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
    
CREATE PROCEDURE [dbo].[sps_GetMoEcategories]  
(    
  @UserId INT        
)    
AS    
BEGIN    
DECLARE @Subjects NVARCHAR(MAX) = (SELECT SubjectsInterested FROM UserMaster WHERE id = @UserId)    
DECLARE @return INT, @query NVARCHAR(MAX) , @ContentId INT, @ContentType INT         
 
CREATE table #tempData          
(          
ContentId INT,           
ContentType INT,    
Title NVARCHAR(MAX),    
CategoryId INT   
)        
INSERT INTO #tempData    
SELECT c.Id, 1, c.Title, (SELECT Id FROM CategoryMaster WHERE Id = c.CategoryId) FROM CourseMaster c WHERE (c.IsApproved IS NULL OR (IsApproved = 1 AND IsApprovedSensory = 1)) AND  c.IsDraft = 0    
AND c.CategoryId IN (SELECT Id from  CategoryMaster WHERE [Name] IN (SELECT VALUE FROM STRING_SPLIT(@Subjects,',')))    
AND c.CreatedBy <> @UserId       
UNION     
SELECT c.Id, 2, c.Title, (SELECT Id FROM CategoryMaster WHERE Id = c.CategoryId) FROM ResourceMaster c WHERE (c.IsApproved IS NULL OR (IsApproved = 1 AND IsApprovedSensory = 1)) AND  c.IsDraft = 0    
AND c.CategoryId IN (SELECT Id FROM CategoryMaster WHERE [Name] IN (SELECT VALUE FROM STRING_SPLIT(@Subjects,',')))    
AND c.CreatedBy <> @UserId       
    
    
IF EXISTS(SELECT TOP 1 1 FROM #tempData)    
BEGIN    
SELECT distinct cm.id,cm.Name,cm.Name_Ar from #tempData t INNER JOIN CategoryMaster cm
on t.CategoryId = cm.Id
   RETURN 105;      
   drop table #tempData     
 END    
 ELSE    
 BEGIN    
  RETURN 113    
 END    
END 
GO
/****** Object:  StoredProcedure [dbo].[sps_GetMoECheckList]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
    
CREATE PROCEDURE [dbo].[sps_GetMoECheckList]    
(    
  @UserId INT,    
  @PageNo int = 1,          
  @PageSize int = 5 ,
  @CategoryId INT = NULL
)    
AS    
BEGIN    
DECLARE @Subjects NVARCHAR(MAX) = (SELECT SubjectsInterested FROM UserMaster WHERE id = @UserId)    
DECLARE @return INT, @query NVARCHAR(MAX) , @ContentId INT, @ContentType INT         
DECLARE @start INT, @end INT          
SET @start = (@PageNo - 1) * @PageSize + 1          
SET @end = @PageNo * @PageSize    
CREATE table #tempData          
(          
ContentId INT,           
ContentType INT,    
Title NVARCHAR(MAX),    
Category NVARCHAR(400)    
)        
INSERT INTO #tempData    
SELECT c.Id, 1, c.Title, (SELECT Name FROM CategoryMaster WHERE Id = c.CategoryId) FROM CourseMaster c WHERE (c.IsApproved IS NULL OR (IsApproved = 1 AND IsApprovedSensory = 1)) AND  c.IsDraft = 0    
AND c.CategoryId IN (SELECT Id FROM CategoryMaster WHERE [Name] IN (SELECT VALUE FROM STRING_SPLIT(@Subjects,',')))    
AND c.CreatedBy <> @UserId    AND c.CategoryId =  @CategoryId   
UNION     
SELECT c.Id, 2, c.Title, (SELECT Name FROM CategoryMaster WHERE Id = c.CategoryId) FROM ResourceMaster c WHERE (c.IsApproved IS NULL OR (IsApproved = 1 AND IsApprovedSensory = 1)) AND  c.IsDraft = 0    
AND c.CategoryId IN (SELECT Id FROM CategoryMaster WHERE [Name] IN (SELECT VALUE FROM STRING_SPLIT(@Subjects,',')))    
AND c.CreatedBy <> @UserId  AND  c.CategoryId =  @CategoryId      
    
    
IF EXISTS(SELECT TOP 1 1 FROM #tempData)    
BEGIN    
SET @query =          
 ';with sqlpaging as (          
 SELECT           
 Rownumber = ROW_NUMBER() OVER(order by  ContentId desc) ,          
 * FROM #tempData)          
          
 select            
  top ('+CAST(@PageSize AS VARCHAR(50))+') *,            
  (select max(rownumber) from sqlpaging) as             
  Totalrows    from sqlpaging            
 where Rownumber between '+CAST(@start AS VARCHAR(50))+' and '+CAST(@end AS VARCHAR(50))+''          
          
 EXEC sp_executesql @query          
   RETURN 105;      
   drop table #tempData     
 END    
 ELSE    
 BEGIN    
  RETURN 113    
 END    
END 
GO
/****** Object:  StoredProcedure [dbo].[sps_GetSensoryCategory]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================  
-- Author:  <Author,,Name>  
-- Create date: <Create Date,,>  
-- Description: <Description,,>  
-- =============================================  
CREATE PROCEDURE [dbo].[sps_GetSensoryCategory]  -- 2
 (  
 @UserId INT  
 )  
AS  
BEGIN  

SET NOCOUNT ON;
 SELECT cm.Id, cm.Name,cm.Name_Ar FROM CategoryMaster cm INNER JOIN
 CourseMaster c on cm.Id = c.CategoryId WHERE c.IsApproved IS NULL AND c.IsDraft = 0     
AND c.Id NOT IN (SELECT ContentId FROM SensoryCheckMaster WHERE ContentType = 1 AND Userid = @UserId AND IsCurrent = 1)    
AND c.CreatedBy <> @UserId AND cm.Active = 1 AND cm.Status = 1   
UNION         
SELECT cm.Id, cm.Name,cm.Name_Ar FROM CategoryMaster cm INNER JOIN    
  ResourceMaster c on cm.Id = c.CategoryId WHERE c.IsApproved IS NULL AND IsDraft = 0        
 AND c.Id NOT IN (SELECT ContentId FROM SensoryCheckMaster WHERE ContentType = 2 AND Userid = @UserId AND IsCurrent = 1)    
 AND c.CreatedBy <>@UserId AND cm.Active = 1 AND cm.Status = 1

  RETURN 105
  
END
GO
/****** Object:  StoredProcedure [dbo].[sps_GetSensoryCheckList]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
CREATE PROCEDURE [dbo].[sps_GetSensoryCheckList] --3    
(      
  @UserId INT,      
  @PageNo int = 1,            
  @PageSize int = 5 ,
  @CategoryId INT = NULL
)      
AS      
BEGIN        
DECLARE @return INT, @query NVARCHAR(MAX) , @ContentId INT, @ContentType INT           
DECLARE @start INT, @end INT            
SET @start = (@PageNo - 1) * @PageSize + 1            
SET @end = @PageNo * @PageSize      
CREATE table #tempData            
(            
ContentId INT,             
ContentType INT,    
Title NVARCHAR(MAX),    
Category NVARCHAR(400)    
)          
INSERT INTO #tempData      
SELECT c.Id, 1, c.Title, (SELECT Name FROM CategoryMaster WHERE id = c.CategoryId)     
FROM CourseMaster c WHERE c.IsApproved IS NULL AND c.IsDraft = 0   
AND c.Id NOT IN (SELECT ContentId FROM SensoryCheckMaster WHERE ContentType = 1 AND Userid = @UserId AND IsCurrent = 1)  
AND c.CreatedBy <> @UserId  AND c.CategoryId= @CategoryId
--AND CategoryId IN (SELECT Id FROM CategoryMaster WHERE [Name] IN (SELECT VALUE FROM STRING_SPLIT(@Subjects,',')))      
UNION       
SELECT Id, 2, Title, (SELECT Name FROM CategoryMaster WHERE id = c.CategoryId)     
 FROM ResourceMaster c WHERE c.IsApproved IS NULL AND IsDraft = 0      
 AND c.Id NOT IN (SELECT ContentId FROM SensoryCheckMaster WHERE ContentType = 2 AND Userid = @UserId AND IsCurrent = 1)  
 AND c.CreatedBy <> @UserId    AND c.CategoryId= @CategoryId    
--AND CategoryId IN (SELECT Id FROM CategoryMaster WHERE [Name] IN (SELECT VALUE FROM STRING_SPLIT(@Subjects,',')))      
     
    
        
      
IF EXISTS(SELECT TOP 1 1 FROM #tempData)      
BEGIN      
SET @query =            
 ';with sqlpaging as (            
 SELECT             
 Rownumber = ROW_NUMBER() OVER(order by  ContentId desc) ,            
 * FROM #tempData)            
            
 select              
  top ('+CAST(@PageSize AS VARCHAR(50))+') *,              
  (select max(rownumber) from sqlpaging) as               
  Totalrows    from sqlpaging              
 where Rownumber between '+CAST(@start AS VARCHAR(50))+' and '+CAST(@end AS VARCHAR(50))+''            
            
 EXEC sp_executesql @query            
   RETURN 105;        
   drop table #tempData       
 END      
 ELSE      
 BEGIN      
  RETURN 113      
 END      
END   
GO
/****** Object:  StoredProcedure [dbo].[sps_GetUserBookmarkedContent]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--CREATE TABLE UserBookmarks
--(
--Id INT IDENTITY(1,1),
--UserId INT CONSTRAINT FK_UserMaster_UserId REFERENCES UserMaster (Id),
--ContentId INT,
--ContentType INT
--)
CREATE PROCEDURE [dbo].[sps_GetUserBookmarkedContent]
(
@UserId INT
)
AS
BEGIN
DECLARE @Return INT = 0
IF EXISTS(SELECT TOP 1 1 FROM UserBookmarks (NOLOCK) WHERE UserId =  @UserId)
BEGIN
	 SELECT u.Id,u.ContentId,u.ContentType, 
	 (CASE WHEN u.ContentType = 1 THEN 
	 (SELECT Title FROM CourseMaster WHERE Id = u.ContentId) ELSE 
	 (SELECT Title FROM ResourceMaster WHERE Id = u.ContentId) END) AS Title,
	 (CASE WHEN u.ContentType = 1 THEN 
	 (SELECT CourseDescription FROM CourseMaster WHERE Id = u.ContentId) ELSE 
	 (SELECT ResourceDescription FROM ResourceMaster WHERE Id = u.ContentId) END) AS [Description],
	 (CASE WHEN u.ContentType = 1 THEN 
	 (SELECT Thumbnail FROM CourseMaster WHERE Id = u.ContentId) ELSE 
	 (SELECT Thumbnail FROM ResourceMaster WHERE Id = u.ContentId) END) AS Thumbnail
	  FROM UserBookmarks u (NOLOCK) WHERE u.UserId =  @UserId
	 SET @Return = 105
END
ELSE
BEGIN
 SET @Return = 102
END
RETURN @Return
END
GO
/****** Object:  StoredProcedure [dbo].[sps_GetUserFavouritesByContentID]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sps_GetUserFavouritesByContentID]
(
@UserId INT,
@ContentId INT,
@ContentType INT
)
AS
BEGIN
	IF EXISTS(SELECT TOP 1 1 FROM UserBookmarks WHERE UserId = @UserId 
	AND ContentId = @ContentId 
	AND ContentType = @ContentType)
	BEGIN
		RETURN 105
	END
	ELSE
	RETURN 102
END
GO
/****** Object:  StoredProcedure [dbo].[sps_GetUserForQRC]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--exec [dbo].[sps_GetUserForQRC] @QrcId =22 ,@Category= 74  
CREATE PROCEDURE [dbo].[sps_GetUserForQRC] 
(  
@QrcId int,  
@Category Int,  
@PageNo int = 1,  
@PageSize int = 5  
)  
  
AS    
BEGIN    
Declare @return INT  
--select top 30 ID, FirstName + ' ' + LastName as UserName  
--,10 as ResourceContributed  
--,0 as CourseCreated  
--,5 as CurrentQRCS  
--,9 as NoOfReviews from UserMaster where  IsContributor = 1 AND FirstName <> '' AND LastName <>''  
  
  
declare @start int, @end int  
set @start = (@PageNo - 1) * @PageSize + 1  
set @end = @PageNo * @PageSize  
  
  
;with sqlpaging as (  
SELECT   
Rownumber = ROW_NUMBER() OVER(ORDER BY qr.userid ASC) ,  
qr.UserId,u.FirstName + '' + u.LastName as UserName,  
u.Email,u.Photo  
,COUNT(DISTINCT rm.Id)  as ResourceContributed,  
COUNT(DISTINCT c.Id) as CourseCreated  
,(select count(qrcid) from qrcusermapping where userid =u.id and active =1) as CurrentQRCS  
,(SELECT count(ID)  from [dbo].[ContentApproval] WHERE AssignedTo = qr.UserId AND Status <>'' ) as NoOfReviews   
 from [dbo].[QRCUserMapping] qr  
LEFT JOIN ResourceMaster rm   
ON qr.UserId = rm.CreatedBy  
LEFT JOIN UserMaster u  
ON qr.UserId = u.Id   
LEFT JOIN CourseMaster c  
ON qr.Userid = c.CreatedBy   
  
WHERE qr.QRCId = @QrcId and qr.CategoryId = @Category and qr.active = 1  
group by qr.UserId, u.FirstName,u.LastName,u.Email,u.Photo,u.id)  
  
  
select  
 top (@PageSize) *,  
 (select max(rownumber) from sqlpaging) as   
 Totalrows  
from sqlpaging  
where Rownumber between @start and @end  
  
  
  
  
 SET @return = 105 -- reconrd does not exists  
   RETURN @return  
END   
GO
/****** Object:  StoredProcedure [dbo].[sps_GetUserNotInQRC]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sps_GetUserNotInQRC]  
(  
@QrcId int = NULL,  
@Category Int = NULL,  
@PageNo int = 1,  
@PageSize int = 5  ,
@FilterCategoryId INT = NULL
)  
  
AS    
BEGIN    
SET ANSI_NULLS ON
Declare @return INT  
--select top 30 ID, FirstName + ' ' + LastName as UserName  
--,10 as ResourceContributed  
--,0 as CourseCreated  
--,5 as CurrentQRCS  
--,9 as NoOfReviews from UserMaster where  IsContributor = 1 AND FirstName <> '' AND LastName <>''  

  DECLARE @query NVARCHAR(MAX)  
  
  
declare @start int, @end int  
set @start = (@PageNo - 1) * @PageSize + 1  
set @end = @PageNo * @PageSize  


--IF (@FilterCategoryId IS NOT NULL)
--BEGIN
--  --Select 'test'
--END

--ELSE

--BEGIN
---- Select 'test2'
--END
IF (@FilterCategoryId IS  NULL  OR @FilterCategoryId = 0 )
BEGIN
SET @query = 
';with sqlpaging as (  
SELECT   
Rownumber = ROW_NUMBER() OVER(ORDER BY u.ID ASC) ,  
u.ID as UserID,u.FirstName + '' '' + u.LastName as UserName,  
u.Email,  
u.Photo  
,COUNT(rm.Id)  as ResourceContributed,  
COUNT(c.Id) as CourseCreated  
,(select count(qrcid) from qrcusermapping where userid =u.id) as CurrentQRCS  
,(SELECT count(ID)  from [dbo].[ContentApproval] WHERE AssignedTo = u.ID AND Status <>'''' ) as NoOfReviews 
,SubjectsInterested 
 from   
 UserMaster u 
LEFT JOIN ResourceMaster rm   
ON u.id = rm.CreatedBy    
LEFT JOIN CourseMaster c  
ON u.id = c.CreatedBy   
WHERE  
u.id not in (select userid from qrcusermapping where active=1 and CategoryId='+CAST(@Category AS VARCHAR(50))+') AND u.IsContributor = 1
group by u.ID, u.FirstName,u.LastName,u.Email,u.Photo,SubjectsInterested)  
  
  
select  
 top ('+CAST(@PageSize AS VARCHAR(50))+') *,  
 (select max(rownumber) from sqlpaging) as   
 Totalrows  
from sqlpaging  
where Rownumber between '+CAST(@start AS VARCHAR(50))+' and '+CAST(@end AS VARCHAR(50))+''
END
ELSE
BEGIN
   SET @query = 
';with sqlpaging as (  
SELECT 
Rownumber = ROW_NUMBER() OVER(ORDER BY u.ID ASC) ,  
u.ID as UserID,u.FirstName + '' '' + u.LastName as UserName,  
u.Email,  
u.Photo  
,COUNT(rm.Id)  as ResourceContributed,  
COUNT(c.Id) as CourseCreated  
,(select count(qrcid) from qrcusermapping where userid =u.id) as CurrentQRCS   
,(SELECT count(ID)  from [dbo].[ContentApproval] WHERE AssignedTo = u.ID AND Status <>'''' ) as NoOfReviews 
,SubjectsInterested 
 from   
 UserMaster u  LEFT JOIN ResourceMaster rm   
ON u.id = rm.CreatedBy  
LEFT JOIN CourseMaster c  
ON u.id = c.CreatedBy   
WHERE  
u.id not in (select userid from qrcusermapping where active=1 AND CategoryId='+CAST(@Category AS VARCHAR(50))+') and u.IsContributor = 1
AND CHARINDEX((SELECT Value FROM STRING_SPLIT(SubjectsInterested,'','') WHERE Value IN 
(SELECT Name from CAtegoryMaster WHERE Id =  '+CAST(@FilterCategoryId AS VARCHAR(50))+')),SubjectsInterested) > 0
group by u.ID, u.FirstName,u.LastName,u.Email,u.Photo,SubjectsInterested)  
  
select  
 top ('+CAST(@PageSize AS VARCHAR(50))+') *,  
 (select max(rownumber) from sqlpaging) as   
 Totalrows  
from sqlpaging  
where Rownumber between '+CAST(@start AS VARCHAR(50))+' and '+CAST(@end AS VARCHAR(50))

END

EXEC sp_executesql @query
    
 SET @return = 105 -- reconrd does not exists  
   RETURN @return  
END   
GO
/****** Object:  StoredProcedure [dbo].[sps_GetVerifierReport]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
CREATE PROCEDURE [dbo].[sps_GetVerifierReport]
(
 @PageNo int = 1,      
 @PageSize int = 5     
)
AS BEGIN
Declare @return INT, @query NVARCHAR(MAX)      
declare @start int, @end int      
set @start = (@PageNo - 1) * @PageSize + 1      
set @end = @PageNo * @PageSize 
CREATE table #tempData      
(      
Verifier NVARCHAR(500),       
ApprovedCount INT,      
RejectedCount INT    
)      
IF EXISTS (SELECT TOP 1 1 FROM MoECheckMaster)
BEGIN
	INSERT INTO #tempData(Verifier,ApprovedCount,RejectedCount)
	SELECT DISTINCT
	(SELECT FirstName + ' ' + LastName FROM UserMaster WHERE Id = r.UserId) AS Verifier,
	(SELECT COUNT(*) FROM MoECheckMaster WHERE UserId = r.UserId AND Status = 1) AS ApprovedCount,
	(SELECT COUNT(*) FROM MoECheckMaster WHERE UserId = r.UserId AND Status = 0) AS RejectedCount
	FROM MoECheckMaster r

	SET @query =      
	';with sqlpaging as (      
	SELECT       
	Rownumber = ROW_NUMBER() OVER(order by  Verifier desc) ,      
	* FROM #tempData)      
      
	select        
	 top ('+CAST(@PageSize AS VARCHAR(50))+') *,        
	 (select max(rownumber) from sqlpaging) as         
	 Totalrows    from sqlpaging        
	where Rownumber between '+CAST(@start AS VARCHAR(50))+' and '+CAST(@end AS VARCHAR(50))+''      
      
	EXEC sp_executesql @query      
	  RETURN 105;  
	  drop table #tempData 
END
ELSE
BEGIN
	RETURN 113
END
END
GO
/****** Object:  StoredProcedure [dbo].[sps_lu_Educational_Standard]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
-- =============================================  
-- Author:  <Author,,Name>  
-- Create date: <Create Date,,>  
-- Description: <Description,,>  
-- =============================================  
CREATE PROCEDURE [dbo].[sps_lu_Educational_Standard]  
   
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
 IF EXISTS(SELECT TOP 1 1 FROM lu_Educational_Standard WHERE Active = 1)  
 BEGIN  
   SELECT es.Id,  
es.Standard,  
es.Standard_Ar,  
CONCAT(c.FirstName, '', c.LastName) as CreatedBy,    
CONCAT(l.FirstName, '', l.LastName) as UpdatedBy,   
es.CreatedOn,  
es.UpdatedOn,  
es.Active FROM lu_Educational_Standard  es  
   INNER join UserMaster c  on es.CreatedBy= c.Id    
   INNER join UserMaster l on es.UpdatedBy =l.Id   
   WHERE es.Status = 1 --and c.Active = 1 and l. Active = 1 -- added active check 
  
     return 105   
  
  END  
  
  ELSE  
  BEGIN  
    RETURN 102 -- reconrd does not exists    
  END;  
END  
GO
/****** Object:  StoredProcedure [dbo].[sps_lu_Educational_StandardById]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sps_lu_Educational_StandardById]-- 1
	(
	@Id INT
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
IF Exists(select TOP 1 1 from lu_Educational_Standard WHERE Id=@Id)   
 BEGIN  
	SELECT es.Id,
es.Standard,
es.Standard_Ar,
CONCAT(c.FirstName, '', c.LastName) as CreatedBy,  
CONCAT(l.FirstName, '', l.LastName) as UpdatedBy, 
es.CreatedOn,
es.UpdatedOn,
es.Active FROM lu_Educational_Standard  es
   INNER join UserMaster c  on es.CreatedBy= c.Id  
   INNER join UserMaster l on es.UpdatedBy =l.Id 
   WHERE es.Active = 1 AND es.Id=@Id
--ON eu.updatedby = um.id

  return 105 -- record exists  
  END
ELSE 

BEGIN
	
  return 102 -- reconrd does not exists  
END




END
GO
/****** Object:  StoredProcedure [dbo].[sps_lu_Level]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
CREATE PROCEDURE [dbo].[sps_lu_Level]  
   
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
 IF EXISTS(SELECT TOP 1 1 FROM lu_Level WHERE Status = 1)  
 BEGIN  
    
   SELECT el.Id,  
     el.[Level],  
     el.Level_Ar,  
     CONCAT(c.FirstName, '', c.LastName) as CreatedBy,    
     CONCAT(l.FirstName, '', l.LastName) as UpdatedBy,   
     el.CreatedOn,  
     el.UpdatedOn,  
     el.Active FROM lu_Level  el   
      INNER join UserMaster c  on el.CreatedBy= c.Id    
      INNER join UserMaster l on el.UpdatedBy =l.Id   
      WHERE el.Status = 1 --and c.Active = 1 and l.Active = 1 -- added active check 
     return 105   
  
  END  
  
  ELSE  
  BEGIN  
    RETURN 102 -- reconrd does not exists    
  END;  
END  
GO
/****** Object:  StoredProcedure [dbo].[sps_lu_LevelById]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sps_lu_LevelById]-- 1
	(
	@Id INT
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
IF Exists(select TOP 1 1 from lu_Level WHERE Id=@Id)   
 BEGIN  
SELECT el.Id,
					el.[Level],
					el.Level_Ar,
					CONCAT(c.FirstName, '', c.LastName) as CreatedBy,  
					CONCAT(l.FirstName, '', l.LastName) as UpdatedBy, 
					el.CreatedOn,
					el.UpdatedOn,
					el.Active FROM lu_Level  el 
			   INNER join UserMaster c  on el.CreatedBy= c.Id  
			   INNER join UserMaster l on el.UpdatedBy =l.Id 
			   WHERE el.Active = 1 
--ON eu.updatedby = um.id

AND el.Id = @Id
  return 105 -- record exists  
  END
ELSE 

BEGIN
	
  return 102 -- reconrd does not exists  
END




END
GO
/****** Object:  StoredProcedure [dbo].[sps_MoEApproved]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sps_MoEApproved]
(
  @PageNo int = 1,      
  @PageSize int = 5    
)
AS
BEGIN
	DECLARE @return INT, @query NVARCHAR(MAX) 
	DECLARE @start INT, @end INT      
	SET @start = (@PageNo - 1) * @PageSize + 1      
	SET @end = @PageNo * @PageSize
	CREATE table #tempData      
	(      
	ContentId INT,       
	ContentType INT,
	Title NVARCHAR(MAX),
	ApprovedBy NVARCHAR(MAX)
	)    
	INSERT INTO #tempData
	SELECT c.ContentId, c.ContentType,(CASE WHEN c.ContentType = 1 THEN (SELECT Title FROM CourseMaster WHERE Id = c.ContentId) 
	ELSE ((SELECT Title FROM ResourceMaster WHERE Id = c.ContentId))END),(SELECT FirstName + ' ' + LastName FROM UserMaster WHERE Id = c.UserId) 
	FROM MoECheckMaster c WHERE [Status] = 1 
IF EXISTS(SELECT TOP 1 1 FROM #tempData)
BEGIN
SET @query =      
	';with sqlpaging as (      
	SELECT       
	Rownumber = ROW_NUMBER() OVER(order by  ContentId desc) ,      
	* FROM #tempData)      
      
	select        
	 top ('+CAST(@PageSize AS VARCHAR(50))+') *,        
	 (select max(rownumber) from sqlpaging) as         
	 Totalrows    from sqlpaging        
	where Rownumber between '+CAST(@start AS VARCHAR(50))+' and '+CAST(@end AS VARCHAR(50))+''      
      
	EXEC sp_executesql @query      
	  RETURN 105;  
	  drop table #tempData 
	END
	ELSE
	BEGIN
		RETURN 113
	END
END
GO
/****** Object:  StoredProcedure [dbo].[sps_Notifications]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
  
-- =============================================  
-- Author:  <Author,,Name>  
-- Create date: <Create Date,,>  
-- Description: <Description,,>  
-- =============================================  
-- exec sps_Notifications 20 
CREATE PROCEDURE [dbo].[sps_Notifications]   
 (  
 @UserID int ,  
 @PageNo int = 1,    
@PageSize int = 10    
 )  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
 DECLARE @Return INT, @Total INT,@RecordCount INT  
 declare @start int, @end int    
set @start = (@PageNo - 1) * @PageSize + 1    
set @end = @PageNo * @PageSize    
    -- Insert statements for procedure here  
  
   
 SELECT @RecordCount = Count(ID)  FROM Notifications    
    WHERE UserID = @UserID  AND IsDelete<>1  
 SELECT @Total = Count(ID)  FROM Notifications    
    WHERE UserID = @UserID AND IsRead = 0  AND messagetypeid <> null
;with sqlpaging as (    
SELECT     
Rownumber = ROW_NUMBER() OVER(ORDER BY n.Id desc) ,   
n.Id,  
 @Total as Total,  
n.ReferenceId,  
n.ReferenceTypeId,  
n.Subject,  
n.Content,  
n.MessageTypeId,  
m.type as MessageType,  
n.IsApproved,  
n.CreatedDate,  
n.ReadDate,  
n.DeletedDate,  
n.IsRead,  
n.IsDelete,  
n.Comment,  
n.Status,
um.FirstName + ' ' + um.LastName as Reviewer,  
n.[Url] As EmailUrl,  
n.UserId from Notifications n   
inner join MessageType m  
ON n.MessageTypeID = m.id  
LEFT join UserMaster um  
ON n.Reviewer = um.id  
WHERE UserID = @UserID AND IsDelete<>1)  
  
    
select    
 top (@PageSize) *,    
 (select max(rownumber) from sqlpaging) as     
 Totalrows    
from sqlpaging    
where Rownumber between @start and @end    
ORDER by CreatedDate desc  
  
IF(@RecordCount>0)  
BEGIN  
SET @Return = 105  
END  
ELSE  
BEGIN  
SET @Return = 113  
END  
  
RETURN @Return  
END  
  
GO
/****** Object:  StoredProcedure [dbo].[sps_OerDashboardReport]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*___________________________________________________________________________________________________      
      
Copyright (c) YYYY-YYYY XYZ Corp. All Rights Reserved      
WorldWide.      
      
$Revision:  $1.0      
$Author:    $ Prince Kumar    
&Date       June 06, 2019    
    
Ticket: Ticket URL    
      
PURPOSE:       
This store procedure will get the reports of top contirbutors ,reviever, and other dashboard data    
    
EXEC sps_OerDashboardReport     
___________________________________________________________________________________________________*/    
CREATE PROCEDURE [dbo].[sps_OerDashboardReport]     
AS    
BEGIN    
    
SET NOCOUNT ON;    
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;    
    
DECLARE @Contributors INT,    
  @Courses INT,    
  @Resources INT,    
  @Return INT,  
  @TotalVisit INT;    
    
SET @Contributors =(SELECT count(*) from usermaster WHERE  IsContributor = 1)    
SET @Courses = (SELECT count(*)  from coursemaster WHERE IsApproved = 1 )    
SET @Resources =(SELECT count(*)  from resourcemaster WHERE IsApproved = 1)    
SET @TotalVisit= (SELECT  Count(ID) AS TotalVisit FROM Visiters)  
    
SELECT @Contributors as Contributors ,@Courses as Courses,@Resources as Resources ,@TotalVisit as  TotalVisit  
    
;WITH TopContributor_with(ID, UserName, CourseCount,Photo) AS    
(    
SELECT  u.id,u.FirstName+' '+u.LastName as UserName,count(c.id),Photo FROM usermaster u    
LEFT JOIN coursemaster c    
on u.id = c.createdby    
    
 WHERE  u.IsContributor = 1 AND c.isApproved =1     
    
 group by u.id,u.FirstName,u.LastName,Photo)    
    
 SELECT TOP 6 ID, UserName, CourseCount,Photo from  TopContributor_with where CourseCount>0  ORDER BY 3 DESC;    
    
    
     
WITH TopReviewer_with(ID, UserName, CourseCount,Photo) as(    
SELECT  u.id,u.FirstName+' '+u.LastName as UserName,count(c.id),Photo FROM usermaster u    
LEFT JOIN ContentApproval c    
on u.id = c.assignedto    
    
 GROUP BY u.id,u.FirstName,u.LastName,Photo)    
    
 SELECT TOP  6 ID, UserName, CourseCount,Photo from  TopReviewer_with where CourseCount>0  ORDER BY 3 DESC;    
    
 SET @Return = 105;    
    
 RETURN @Return;    
    
END;    
GO
/****** Object:  StoredProcedure [dbo].[sps_PageContentByPageId]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================    
-- Author:  <Author,,Name>    
-- Create date: <Create Date,,>    
-- Description: <Description,,>    
-- =============================================    
-- sps_PageContentByPageId 2  
CREATE PROCEDURE [dbo].[sps_PageContentByPageId]    
 (    
 @PageId INT    
 )    
AS    
BEGIN    
SELECT     
Id,    
PageID, PageContent,PageContent_Ar,CreatedBy,CreatedOn,UpdatedBy,UpdatedOn    
    
 FROM WebPageContent WHERE PageID = @PageId    
    
 RETURN 105    
END 
GO
/****** Object:  StoredProcedure [dbo].[sps_Pages]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================    
-- Author:  <Author,,Name>    
-- Create date: <Create Date,,>    
-- Description: <Description,,>    
-- =============================================    
CREATE PROCEDURE [dbo].[sps_Pages]    
     
AS    
BEGIN    
SELECT Id,    
PageName, PageName_Ar from WebContentPages    
    
RETURN 105    
END 
GO
/****** Object:  StoredProcedure [dbo].[sps_QRCByUserID]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================  
-- Author:  <Author,,Name>  
-- Create date: <Create Date,,>  
-- Description: <Description,,>  
-- =============================================  
  
-- sps_QRCByUserID 2  
CREATE PROCEDURE [dbo].[sps_QRCByUserID]   
 (  
 @UserID INT  
 )  
AS  
BEGIN 




   
 SELECT   
 qm.Id,  
qm.Name,  
Description,  
CAST(qm.CreatedBy AS nvarchar(50)) as CreatedBy,  
qm.CreatedOn,  
CAST(qm.UpdatedBy AS nvarchar(50)) as UpdatedBy,  
qm.UpdatedOn,  
qm.Active,  
qmc.Id As CategoryId,  
qmc.Name AS CategoryName,  
qmc.Name_Ar AS CategoryNameAr  
   
  FROM QRCMaster qm  
  INNER JOIN QRCCategory qc  
  ON qm.Id = qc.QRCId  
  INNER JOIN CategoryMaster qmc  
  ON qmc.Id = qc.CategoryId  
   WHERE qm.ID in ( SELECT QRCId FROM QRCUserMapping WHERE UserId = @UserID AND Active = 1) 
   AND qm.Active =1 AND qmc.Status = 1
   AND qmc.id in (
    SELECT DISTINCT cm.CategoryId FROM CourseMaster cm  
 INNER JOIN ContentApproval ca ON cm.Id  = ca.ContentId WHERE ca.AssignedTo = 2 AND ca.ContentType = 1  
 AND ca.[Status] IS NULL AND cm.MoEBadge <> 1 AND cm.CommunityBadge <> 1  
 UNION   
 SELECT DISTINCT cm.CategoryId  FROM ResourceMaster cm  
 INNER JOIN ContentApproval ca ON cm.Id  = ca.ContentId WHERE ca.AssignedTo = 2 AND ca.ContentType = 2  
 AND  ca.[Status] IS NULL AND cm.MoEBadge <> 1 AND cm.CommunityBadge <> 1 
   )
 RETURN 105  
END
GO
/****** Object:  StoredProcedure [dbo].[sps_QRCReport]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
--select * from contentapproval  where qrcid = 10  and status is null
--update contentapproval set status = 1 where id = 214
--select distinct ContentId, ContentType , QrcId, 0 As UserCount, NULL As [Status] FROM ContentApproval


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sps_QRCReport]
	
AS
BEGIN
DECLARE @ContentId int, @ContentType int, @QrcId  int,@UserCount INT, @Status VARCHAR(20)
CREATE TABLE #tempData(ContentId INT, ContentType INT, QrcId INT, [Status] VARCHAR(20))
DECLARE qrc_cursor CURSOR FOR     
select distinct ContentId, ContentType , QrcId FROM ContentApproval;    
  
OPEN qrc_cursor    
  
FETCH NEXT FROM qrc_cursor     
INTO @ContentId , @ContentType , @QrcId     
  
WHILE @@FETCH_STATUS = 0    
BEGIN    
	IF EXISTS(SELECT TOP 1 1 FROM ContentApproval WHERE ContentId = @ContentId AND ContentType= @ContentType
			  AND QrcId = @QrcId AND [Status] = 0)
		BEGIN
		    INSERT INTO #tempData VALUES(@ContentId , @ContentType , @QrcId , 'R')
		END
	ELSE IF EXISTS(SELECT TOP 1 1 FROM ContentApproval WHERE ContentId = @ContentId AND ContentType= @ContentType
			  AND QrcId = @QrcId AND [Status] = 1)
		BEGIN
			IF(@ContentType = 1)
			  BEGIN
					IF EXISTS(SELECT TOP 1 1 FROM CourseMaster WHERE Id = @ContentId AND IsApproved = 1)
						BEGIN
							INSERT INTO #tempData VALUES(@ContentId , @ContentType , @QrcId , 'A')
						END
					ELSE
					   BEGIN
							INSERT INTO #tempData VALUES(@ContentId , @ContentType , @QrcId , 'P')
					   END
			  END
			  ELSE IF(@ContentType = 2)
			  BEGIN
					IF EXISTS(SELECT TOP 1 1 FROM ResourceMaster WHERE Id = @ContentId AND IsApproved = 1)
						BEGIN
							INSERT INTO #tempData VALUES(@ContentId , @ContentType , @QrcId , 'A')
						END
					ELSE
					   BEGIN
							INSERT INTO #tempData VALUES(@ContentId , @ContentType , @QrcId , 'P')
					   END
			  END
		END
	 ELSE
		BEGIN
			INSERT INTO #tempData VALUES(@ContentId , @ContentType , @QrcId , 'P')
		END
    FETCH NEXT FROM qrc_cursor     
INTO @ContentId , @ContentType , @QrcId      
   
END     
CLOSE qrc_cursor;    
DEALLOCATE qrc_cursor; 

select DISTINCT [Name],
(SELECT Count(DISTINCT qrcu.UserId) from [dbo].[QRCMaster] qrc
INNER JOIN [dbo].[QRCUserMapping]  qrcu
on qrc.id = qrcu.qrcid
WHERE
 qrc.id = qm.id
 AND qrcu.Active = 1) as UserCount
,id,
(SELECT COUNT(ContentId) FROM #tempData WHERE #tempData.QrcId = qm.Id
AND #tempData.Status = 'A') AS ApproveCount,
(SELECT COUNT(ContentId) FROM #tempData WHERE #tempData.QrcId = qm.Id
AND #tempData.Status = 'R') AS RejectCount,
(SELECT COUNT(ContentId) FROM #tempData WHERE #tempData.QrcId = qm.Id
AND #tempData.Status = 'P') AS PendingAction,
(SELECT COUNT(ContentId) FROM #tempData WHERE #tempData.QrcId = qm.Id) 
AS Submission
from [dbo].[QRCMaster]	qm
RETURN 105;
END

GO
/****** Object:  StoredProcedure [dbo].[sps_QRCReportV1]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sps_QRCReportV1]
	
AS
BEGIN
select top 1 qrc.Name,Count(qrcu.qrcid) as UserCount,qrc.id,

(
select  top 1 count( distinct ca.contentid)from ContentApproval ca inner join QRCUserMapping qrcm on ca.AssignedTo = qrcm.userid
inner join QRCMaster qrc on qrc.Id = qrcm.QRCId where ca.Status =1  and ca.QrcId is not NULL--and QRC.ID <>NULL
group by qrc.id
) as ApproveCount,
(
select  top 1 count( distinct ca.contentid)from ContentApproval ca inner join QRCUserMapping qrcm on ca.AssignedTo = qrcm.userid
inner join QRCMaster qrc on qrc.Id = qrcm.QRCId where ca.Status =0  and ca.QrcId is not NULL--and QRC.ID <>NULL
group by qrc.id
) as RejectCount,
(
select  top 1 count( distinct ca.contentid)from ContentApproval ca inner join QRCUserMapping qrcm on ca.AssignedTo = qrcm.userid
inner join QRCMaster qrc on qrc.Id = qrcm.QRCId where ca.Status =null and ca.QrcId is not NULL--and QRC.ID <>NULL
group by qrc.id
) as PendingAction,
(SELECT  top 1 count( distinct ca.contentid)from ContentApproval ca inner join QRCUserMapping qrcm on ca.AssignedTo = qrcm.userid
inner join QRCMaster qrc on qrc.Id = qrcm.QRCId where ca.Status =null and ca.QrcId is not NULL--and QRC.ID <>NULL
group by qrc.id
) as Submission
from [dbo].[QRCMaster]		qrc
inner join [dbo].[QRCUserMapping]  qrcu
on qrc.id = qrcu.qrcid
WHERE qrcu.UserId  IN (SELECT Id FROM UserMaster WHERE Active = 1)
group by qrcu.QRCId,qrc.Name,qrc.id
RETURN 105;
END
GO
/****** Object:  StoredProcedure [dbo].[sps_RecommendedContent]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
CREATE     PROCEDURE [dbo].[sps_RecommendedContent] --21 1 24      
(      
@UserId INT = NULL,    
@PageNo int = 1,    
@PageSize int = 5    
)      
AS      
BEGIN      
      
SET NOCOUNT ON;      
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;      
      
      
  
      
DECLARE @Keyword NVARCHAR(100)      
DECLARE @Return INT      
DECLARE @totalRecords INT;    
DECLARE @query NVARCHAR(MAX)    
    
    
declare @start int, @end int    
set @start = (@PageNo - 1) * @PageSize + 1    
set @end = @PageNo * @PageSize    
     
 IF(@UserId IS NOT NULL)    
 BEGIN    
   DECLARE @TempTable TABLE(Id NUMERIC, Title NVARCHAR(MAX),Description NVARCHAR(MAX),ContentType INT, Thumbnail NVARCHAR(MAX),CTitle NVARCHAR(MAX), CTitle_Ar NVARCHAR(MAX), Cdescription NVARCHAR(MAX),CDescription_ar NVARCHAR(MAX), media NVARCHAR(MAX) )    
   INSERT INTO @TempTable    
   SELECT r.Id,r.Title,r.ResourceDescription as Description,2 as ContentType,Thumbnail,cpm.Title as CTitle, cpm.Title_Ar as  CTitle_Ar , cpm.Description as  Cdescription ,cpm.Description_Ar as  CDescription_ar , cpm.media as media    
    from resourcemaster r LEFT JOIN CategoryMaster cm      
   on      
   r.categoryid = cm.id  
   LEFT JOIN CopyrightMaster cpm      
   on      
   r.CopyRightId = cpm.id    
   LEFT JOIN SubCategoryMaster sbm      
   ON r.subcategoryId = sbm.ID  WHERE     
   EXISTS(select value from  StringSplit((select subjectsinterested      
    from usermaster where id = @userid and subjectsinterested <>''),',')      
      
    WHERE  trim(value)<>''      
    AND(      
    r.Keywords Like '%'+CAST(value AS NVARCHAR(100))+'%'      
      
   OR cm.Name Like '%'+CAST(value AS NVARCHAR(100))+'%'      
      
   OR sbm.Name Like '%'+CAST(value AS NVARCHAR(100))+'%')) AND    
    r.IsApproved = 1    
   UNION ALL    
   SELECT c.Id,c.Title,c.CourseDescription as Description,1 as ContentType,Thumbnail,cpm.Title as CTitle, cpm.Title_Ar as  CTitle_Ar  , cpm.Description as  Cdescription ,cpm.Description_Ar as  CDescription_ar , cpm.media as media  
    from coursemaster c LEFT JOIN CategoryMaster cm      
   on      
   c.categoryid = cm.id
    LEFT JOIN CopyrightMaster cpm      
   on      
   c.CopyRightId = cpm.id   
   LEFT JOIN SubCategoryMaster sbm      
   ON c.subcategoryId = sbm.ID  WHERE     
   EXISTS(select value from  StringSplit((select subjectsinterested      
    from usermaster where id = @userid and subjectsinterested <>''),',')      
      
    WHERE  trim(value)<>''      
    AND(      
    c.Keywords Like '%'+CAST(value AS NVARCHAR(100))+'%'      
      
   OR cm.Name Like '%'+CAST(value AS NVARCHAR(100))+'%'      
      
   OR sbm.Name Like '%'+CAST(value AS NVARCHAR(100))+'%')) AND    
    c.IsApproved = 1    
  
    IF NOT EXISTS(SELECT value from  StringSplit((select subjectsinterested      
    from usermaster where id = @UserId and subjectsinterested <>''),','))    
   BEGIN    
      
     
    
   SET @QUery = '    
   DECLARE @Table TABLE(Id NUMERIC, Title NVARCHAR(MAX),Description NVARCHAR(MAX),ContentType INT, Thumbnail VARCHAR(MAX),CreatedOn DateTime,CTitle NVARCHAR(MAX), CTitle_Ar NVARCHAR(MAX), Cdescription NVARCHAR(MAX),CDescription_ar NVARCHAR(MAX), media NVARCHAR(MAX))    
   INSERT INTO @Table    
   SELECT r.Id,r.Title,r.ResourceDescription as Description,2 as ContentType,Thumbnail,r.CreatedOn,cpm.Title as CTitle, cpm.Title_Ar as  CTitle_Ar , cpm.Description as  Cdescription ,cpm.Description_Ar as  CDescription_Ar , cpm.media as media    
    from resourcemaster r LEFT JOIN CategoryMaster cm      
   on      
   r.categoryid = cm.id 
   LEFT JOIN CopyrightMaster cpm      
    on      
     r.CopyRightId = cpm.id      
   LEFT JOIN SubCategoryMaster sbm      
   ON r.subcategoryId = sbm.ID  WHERE r.IsApproved = 1    
   UNION ALL    
   SELECT c.Id,c.Title,c.CourseDescription as Description,1 as ContentType,Thumbnail,c.CreatedOn,r.CreatedOn,cpm.Title as CTitle, cpm.Title_Ar as  CTitle_Ar , cpm.Description as  Cdescription ,cpm.Description_Ar as  CDescription_Ar , cpm.media as media    
    from coursemaster c LEFT JOIN CategoryMaster cm      
   on      
   c.categoryid = cm.id 
   LEFT JOIN CopyrightMaster cpm      
   on      
   c.CopyRightId = cpm.id     
   LEFT JOIN SubCategoryMaster sbm      
   ON c.subcategoryId = sbm.ID  WHERE c.IsApproved = 1    
   ;with sqlpaging as (    
   SELECT Rownumber = ROW_NUMBER() OVER(ORDER BY CreatedOn desc),Id,Title,Description,ContentType,Thumbnail,CreatedOn,CreatedOn, CTitle, CTitle_Ar ,Cdescription ,CDescription_ar ,  media      
    from @Table    
    )    
   select    
    top ('+CONVERT(VARCHAR(MAX),@PageSize)+') *,    
    (select max(rownumber) from sqlpaging) as     
    Totalrows    
   from sqlpaging    
   where Rownumber between '+CONVERT(VARCHAR(MAX),@start)+' and '+CONVERT(VARCHAR(MAX),@end)+'    
   Order by Id desc '     
    
  
   END    
   ELSE    
   BEGIN    
   IF EXISTS(SELECT TOP 1 1 FROM @TempTable)  
   BEGIN  
    SET @QUery =     
    'DECLARE @Table TABLE(Id NUMERIC, Title NVARCHAR(MAX),Description NVARCHAR(MAX),ContentType INT, Thumbnail NVARCHAR(MAX),CTitle NVARCHAR(MAX), CTitle_Ar NVARCHAR(MAX), Cdescription NVARCHAR(MAX),CDescription_ar NVARCHAR(MAX), media NVARCHAR(MAX))    
   INSERT INTO @Table    
   SELECT r.Id,r.Title,r.ResourceDescription as Description,2 as ContentType,Thumbnail, cpm.Title as CTitle, cpm.Title_Ar as  CTitle_Ar , cpm.Description as  Cdescription ,cpm.Description_Ar as  CDescription_Ar , cpm.media as media     
    from resourcemaster r LEFT JOIN CategoryMaster cm      
   on      
   r.categoryid = cm.id    
LEFT JOIN CopyrightMaster cpm      
   on      
   r.CopyRightId = cpm.id   
 
   LEFT JOIN SubCategoryMaster sbm      
   ON r.subcategoryId = sbm.ID  WHERE     
   EXISTS(select value from  StringSplit((select subjectsinterested      
    from usermaster where id = '+CONVERT(VARCHAR(MAX),@userid)+' and subjectsinterested <>''''),'','')      
      
    WHERE  trim(value)<>''''      
    AND(      
    r.Keywords Like ''%''+CAST(value AS NVARCHAR(100))+''%''      
      
   OR cm.Name Like ''%''+CAST(value AS NVARCHAR(100))+''%''      
      
   OR sbm.Name Like ''%''+CAST(value AS NVARCHAR(100))+''%'')) AND    
    r.IsApproved = 1    
   UNION ALL    
   SELECT c.Id,c.Title,c.CourseDescription as Description,1 as ContentType,Thumbnail,cpm.Title as CTitle, Title_Ar as  CTitle_Ar , cpm.Description as  Cdescription ,cpm.Description_Ar as  CDescription_Ar , cpm.media as media       
    from coursemaster c LEFT JOIN CategoryMaster cm      
   on      
   c.categoryid = cm.id
 
 LEFT JOIN CopyrightMaster cpm      
   on      
   c.CopyRightId = cpm.id          
   LEFT JOIN SubCategoryMaster sbm      
   ON c.subcategoryId = sbm.ID  WHERE     
   EXISTS(select value from  StringSplit((select subjectsinterested      
    from usermaster where id = '+CONVERT(VARCHAR(MAX),@userid)+' and subjectsinterested <>''''),'','')      
      
    WHERE  trim(value)<>''''      
    AND(      
    c.Keywords Like ''%''+CAST(value AS NVARCHAR(100))+''%''      
      
   OR cm.Name Like ''%''+CAST(value AS NVARCHAR(100))+''%''      
      
   OR sbm.Name Like ''%''+CAST(value AS NVARCHAR(100))+''%'')) AND    
    c.IsApproved = 1    
   ;with sqlpaging as (    
   SELECT Rownumber = ROW_NUMBER() OVER(ORDER BY ID desc),Id,Title,Description,ContentType,Thumbnail,CTitle, CTitle_Ar ,Cdescription ,CDescription_Ar ,  media     
    from @Table)    
   select    
    top ('+CONVERT(VARCHAR(MAX),@PageSize)+') *,    
    (select max(rownumber) from sqlpaging) as     
    Totalrows    
   from sqlpaging    
   where Rownumber between '+CONVERT(VARCHAR(MAX),@start)+' and '+CONVERT(VARCHAR(MAX),@end)+'    
   Order by Id desc'    
    
    --print @QUery  
   END  
   ELSE  
   BEGIN  
     SET @QUery = '    
     DECLARE @Table TABLE(Id NUMERIC, Title NVARCHAR(MAX),Description NVARCHAR(MAX),ContentType INT, Thumbnail NVARCHAR(MAX),CreatedOn DateTime,CTitle NVARCHAR(MAX), CTitle_Ar NVARCHAR(MAX), Cdescription NVARCHAR(MAX),CDescription_ar NVARCHAR(MAX), media NVARCHAR(MAX))    
     INSERT INTO @Table    
     SELECT r.Id,r.Title,r.ResourceDescription as Description,2 as ContentType,Thumbnail,r.CreatedOn    
      from resourcemaster r LEFT JOIN CategoryMaster cm      
     on      
     r.categoryid = cm.id   
 LEFT JOIN CopyrightMaster cpm      
   on      
   r.CopyRightId = cpm.id     
     LEFT JOIN SubCategoryMaster sbm      
     ON r.subcategoryId = sbm.ID  WHERE r.IsApproved = 1    
     UNION ALL    
     SELECT c.Id,c.Title,c.CourseDescription as Description,1 as ContentType,Thumbnail,c.CreatedOn     
      from coursemaster c LEFT JOIN CategoryMaster cm      
     on  
     c.categoryid = cm.id      
     LEFT JOIN SubCategoryMaster sbm      
     ON c.subcategoryId = sbm.ID  WHERE c.IsApproved = 1    
     ;with sqlpaging as (    
     SELECT Rownumber = ROW_NUMBER() OVER(ORDER BY CreatedOn desc),Id,Title,Description,ContentType,Thumbnail,CTitle, CTitle_Ar ,Cdescription ,CDescription_ar ,  media      
      from @Table    
      )    
     select    
      top ('+CONVERT(VARCHAR(MAX),@PageSize)+') *,    
      (select max(rownumber) from sqlpaging) as     
      Totalrows    
     from sqlpaging    
     where Rownumber between '+CONVERT(VARCHAR(MAX),@start)+' and '+CONVERT(VARCHAR(MAX),@end)+'    
     Order by Id desc '     
   END  
    --select count(*) as testingcount from @TABLE  
   END    
   END    
ELSE    
BEGIN    
  SET @QUery = '    
DECLARE @Table TABLE(Id NUMERIC, Title NVARCHAR(MAX),Description NVARCHAR(MAX),ContentType INT, Thumbnail NVARCHAR(MAX), CreatedOn DateTime,CTitle NVARCHAR(MAX), CTitle_Ar NVARCHAR(MAX), Cdescription NVARCHAR(MAX),CDescription_ar NVARCHAR(MAX), media NVARCHAR(MAX))    
INSERT INTO @Table    
SELECT r.Id,r.Title,r.ResourceDescription as Description,2 as ContentType,Thumbnail, r.CreatedOn,cpm.Title as CTitle, Title_Ar as  CTitle_Ar , cpm.Description as  Cdescription ,cpm.Description_Ar as  CDescription_Ar , cpm.media as media      
 from resourcemaster r LEFT JOIN CategoryMaster cm      
on      
r.categoryid = cm.id    
LEFT JOIN CopyrightMaster cpm      
   on      
   r.CopyRightId = cpm.id    
LEFT JOIN SubCategoryMaster sbm      
ON r.subcategoryId = sbm.ID  WHERE r.IsApproved = 1    
UNION ALL    
SELECT c.Id,c.Title,c.CourseDescription as Description,1 as ContentType,Thumbnail, c.CreatedOn ,cpm.Title as CTitle, Title_Ar as  CTitle_Ar , cpm.Description as  Cdescription ,cpm.Description_Ar as  CDescription_Ar , cpm.media as media     
 from coursemaster c LEFT JOIN CategoryMaster cm      
on      
c.categoryid = cm.id
 LEFT JOIN CopyrightMaster cpm      
   on      
   c.CopyRightId = cpm.id        
LEFT JOIN SubCategoryMaster sbm      
ON c.subcategoryId = sbm.ID  WHERE c.IsApproved = 1    
;with sqlpaging as (    
SELECT Rownumber = ROW_NUMBER() OVER(ORDER BY CreatedOn desc),Id,Title,Description,ContentType,Thumbnail,CTitle, CTitle_Ar ,Cdescription ,CDescription_ar ,  media      
 from @Table    
 )    
select    
 top ('+CONVERT(VARCHAR(MAX),@PageSize)+') *,    
 (select max(rownumber) from sqlpaging) as     
 Totalrows    
from sqlpaging    
where Rownumber between '+CONVERT(VARCHAR(MAX),@start)+' and '+CONVERT(VARCHAR(MAX),@end)+'    
Order by Id desc '      
END    

--return @Query;

EXEC sp_executesql @Query       
-- WHERE c.Keywords Like '%'+CAST(@Keyword AS NVARCHAR(100))+'%'      
      
--OR cm.Name Like '%'+CAST(@Keyword AS NVARCHAR(100))+'%'      
      
--OR sbm.Name Like '%'+CAST(@Keyword AS NVARCHAR(100))+'%'      
      --drop table @TempTable
 SET @Return = 105;      
      
 RETURN @Return;      
      
END;     
    
    

GO
/****** Object:  StoredProcedure [dbo].[sps_RecommendedContent_test]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
CREATE     PROCEDURE [dbo].[sps_RecommendedContent_test] --21 1 24      
(      
@UserId INT = NULL,    
@PageNo int = 1,    
@PageSize int = 5    
)      
AS      
BEGIN      
      
SET NOCOUNT ON;      
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;      
      
      
  
      
DECLARE @Keyword NVARCHAR(100)      
DECLARE @Return INT      
DECLARE @totalRecords INT;    
DECLARE @query NVARCHAR(MAX)    
    
    
declare @start int, @end int    
set @start = (@PageNo - 1) * @PageSize + 1    
set @end = @PageNo * @PageSize    
     
 IF(@UserId IS NOT NULL)    
 BEGIN    
   DECLARE @TempTable TABLE(Id NUMERIC, Title NVARCHAR(MAX),Description NVARCHAR(MAX),ContentType INT, Thumbnail NVARCHAR(MAX),CTitle NVARCHAR(MAX), CTitle_Ar NVARCHAR(MAX), Cdescription NVARCHAR(MAX),CDescription_ar NVARCHAR(MAX), media NVARCHAR(MAX) )    
   INSERT INTO @TempTable    
   SELECT r.Id,r.Title,r.ResourceDescription as Description,2 as ContentType,Thumbnail,cpm.Title as CTitle, cpm.Title_Ar as  CTitle_Ar , cpm.Description as  Cdescription ,cpm.Description_Ar as  CDescription_ar , cpm.media as media    
    from resourcemaster r LEFT JOIN CategoryMaster cm      
   on      
   r.categoryid = cm.id  
   LEFT JOIN CopyrightMaster cpm      
   on      
   r.CopyRightId = cpm.id    
   LEFT JOIN SubCategoryMaster sbm      
   ON r.subcategoryId = sbm.ID  WHERE     
   EXISTS(select value from  StringSplit((select subjectsinterested      
    from usermaster where id = @userid and subjectsinterested <>''),',')      
      
    WHERE  trim(value)<>''      
    AND(      
    r.Keywords Like '%'+CAST(value AS NVARCHAR(100))+'%'      
      
   OR cm.Name Like '%'+CAST(value AS NVARCHAR(100))+'%'      
      
   OR sbm.Name Like '%'+CAST(value AS NVARCHAR(100))+'%')) AND    
    r.IsApproved = 1    
   UNION ALL    
   SELECT c.Id,c.Title,c.CourseDescription as Description,1 as ContentType,Thumbnail,cpm.Title as CTitle, cpm.Title_Ar as  CTitle_Ar  , cpm.Description as  Cdescription ,cpm.Description_Ar as  CDescription_ar , cpm.media as media  
    from coursemaster c LEFT JOIN CategoryMaster cm      
   on      
   c.categoryid = cm.id
    LEFT JOIN CopyrightMaster cpm      
   on      
   c.CopyRightId = cpm.id   
   LEFT JOIN SubCategoryMaster sbm      
   ON c.subcategoryId = sbm.ID  WHERE     
   EXISTS(select value from  StringSplit((select subjectsinterested      
    from usermaster where id = @userid and subjectsinterested <>''),',')      
      
    WHERE  trim(value)<>''      
    AND(      
    c.Keywords Like '%'+CAST(value AS NVARCHAR(100))+'%'      
      
   OR cm.Name Like '%'+CAST(value AS NVARCHAR(100))+'%'      
      
   OR sbm.Name Like '%'+CAST(value AS NVARCHAR(100))+'%')) AND    
    c.IsApproved = 1    
  
    IF NOT EXISTS(SELECT value from  StringSplit((select subjectsinterested      
    from usermaster where id = @UserId and subjectsinterested <>''),','))    
   BEGIN    
      
     
    
   SET @QUery = '    
   DECLARE @Table TABLE(Id NUMERIC, Title NVARCHAR(MAX),Description NVARCHAR(MAX),ContentType INT, Thumbnail VARCHAR(MAX),CreatedOn DateTime,CTitle NVARCHAR(MAX), CTitle_Ar NVARCHAR(MAX), Cdescription NVARCHAR(MAX),CDescription_ar NVARCHAR(MAX), media NVARCHAR(MAX))    
   INSERT INTO @Table    
   SELECT r.Id,r.Title,r.ResourceDescription as Description,2 as ContentType,Thumbnail,r.CreatedOn,cpm.Title as CTitle, cpm.Title_Ar as  CTitle_Ar , cpm.Description as  Cdescription ,cpm.Description_Ar as  CDescription_Ar , cpm.media as media    
    from resourcemaster r LEFT JOIN CategoryMaster cm      
   on      
   r.categoryid = cm.id 
   LEFT JOIN CopyrightMaster cpm      
    on      
     r.CopyRightId = cpm.id      
   LEFT JOIN SubCategoryMaster sbm      
   ON r.subcategoryId = sbm.ID  WHERE r.IsApproved = 1    
   UNION ALL    
   SELECT c.Id,c.Title,c.CourseDescription as Description,1 as ContentType,Thumbnail,c.CreatedOn,r.CreatedOn,cpm.Title as CTitle, cpm.Title_Ar as  CTitle_Ar , cpm.Description as  Cdescription ,cpm.Description_Ar as  CDescription_Ar , cpm.media as media    
    from coursemaster c LEFT JOIN CategoryMaster cm      
   on      
   c.categoryid = cm.id 
   LEFT JOIN CopyrightMaster cpm      
   on      
   c.CopyRightId = cpm.id     
   LEFT JOIN SubCategoryMaster sbm      
   ON c.subcategoryId = sbm.ID  WHERE c.IsApproved = 1    
   ;with sqlpaging as (    
   SELECT Rownumber = ROW_NUMBER() OVER(ORDER BY CreatedOn desc),Id,Title,Description,ContentType,Thumbnail,CreatedOn,CreatedOn, CTitle, CTitle_Ar ,Cdescription ,CDescription_ar ,  media      
    from @Table    
    )    
   select    
    top ('+CONVERT(VARCHAR(MAX),@PageSize)+') *,    
    (select max(rownumber) from sqlpaging) as     
    Totalrows    
   from sqlpaging    
   where Rownumber between '+CONVERT(VARCHAR(MAX),@start)+' and '+CONVERT(VARCHAR(MAX),@end)+'    
   Order by Id desc '     
    
  
   END    
   ELSE    
   BEGIN    
   IF EXISTS(SELECT TOP 1 1 FROM @TempTable)  
   BEGIN  
    SET @QUery =     
    'DECLARE @Table TABLE(Id NUMERIC, Title NVARCHAR(MAX),Description NVARCHAR(MAX),ContentType INT, Thumbnail NVARCHAR(MAX),CTitle NVARCHAR(MAX), CTitle_Ar NVARCHAR(MAX), Cdescription NVARCHAR(MAX),CDescription_ar NVARCHAR(MAX), media NVARCHAR(MAX))    
   INSERT INTO @Table    
   SELECT r.Id,r.Title,r.ResourceDescription as Description,2 as ContentType,Thumbnail, cpm.Title as CTitle, cpm.Title_Ar as  CTitle_Ar , cpm.Description as  Cdescription ,cpm.Description_Ar as  CDescription_Ar , cpm.media as media     
    from resourcemaster r LEFT JOIN CategoryMaster cm      
   on      
   r.categoryid = cm.id    
LEFT JOIN CopyrightMaster cpm      
   on      
   r.CopyRightId = cpm.id   
 LEFT JOIN CopyrightMaster cpm      
   on      
   r.CopyRightId = cpm.id  
   LEFT JOIN SubCategoryMaster sbm      
   ON r.subcategoryId = sbm.ID  WHERE     
   EXISTS(select value from  StringSplit((select subjectsinterested      
    from usermaster where id = '+CONVERT(VARCHAR(MAX),@userid)+' and subjectsinterested <>''''),'','')      
      
    WHERE  trim(value)<>''''      
    AND(      
    r.Keywords Like ''%''+CAST(value AS NVARCHAR(100))+''%''      
      
   OR cm.Name Like ''%''+CAST(value AS NVARCHAR(100))+''%''      
      
   OR sbm.Name Like ''%''+CAST(value AS NVARCHAR(100))+''%'')) AND    
    r.IsApproved = 1    
   UNION ALL    
   SELECT c.Id,c.Title,c.CourseDescription as Description,1 as ContentType,Thumbnail,cpm.Title as CTitle, Title_Ar as  CTitle_Ar , cpm.Description as  Cdescription ,cpm.Description_Ar as  CDescription_Ar , cpm.media as media       
    from coursemaster c LEFT JOIN CategoryMaster cm      
   on      
   c.categoryid = cm.id
   LEFT JOIN CopyrightMaster cpm      
   on      
   c.CopyRightId = cpm.id 
 LEFT JOIN CopyrightMaster cpm      
   on      
   c.CopyRightId = cpm.id          
   LEFT JOIN SubCategoryMaster sbm      
   ON c.subcategoryId = sbm.ID  WHERE     
   EXISTS(select value from  StringSplit((select subjectsinterested      
    from usermaster where id = '+CONVERT(VARCHAR(MAX),@userid)+' and subjectsinterested <>''''),'','')      
      
    WHERE  trim(value)<>''''      
    AND(      
    c.Keywords Like ''%''+CAST(value AS NVARCHAR(100))+''%''      
      
   OR cm.Name Like ''%''+CAST(value AS NVARCHAR(100))+''%''      
      
   OR sbm.Name Like ''%''+CAST(value AS NVARCHAR(100))+''%'')) AND    
    c.IsApproved = 1    
   ;with sqlpaging as (    
   SELECT Rownumber = ROW_NUMBER() OVER(ORDER BY ID desc),Id,Title,Description,ContentType,Thumbnail,CTitle, CTitle_Ar ,Cdescription ,CDescription_Ar ,  media     
    from @Table)    
   select    
    top ('+CONVERT(VARCHAR(MAX),@PageSize)+') *,    
    (select max(rownumber) from sqlpaging) as     
    Totalrows    
   from sqlpaging    
   where Rownumber between '+CONVERT(VARCHAR(MAX),@start)+' and '+CONVERT(VARCHAR(MAX),@end)+'    
   Order by Id desc'    
    
    --print @QUery  
   END  
   ELSE  
   BEGIN  
     SET @QUery = '    
     DECLARE @Table TABLE(Id NUMERIC, Title NVARCHAR(MAX),Description NVARCHAR(MAX),ContentType INT, Thumbnail NVARCHAR(MAX),CreatedOn DateTime,CTitle NVARCHAR(MAX), CTitle_Ar NVARCHAR(MAX), Cdescription NVARCHAR(MAX),CDescription_ar NVARCHAR(MAX), media NVARCHAR(MAX))    
     INSERT INTO @Table    
     SELECT r.Id,r.Title,r.ResourceDescription as Description,2 as ContentType,Thumbnail,r.CreatedOn    
      from resourcemaster r LEFT JOIN CategoryMaster cm      
     on      
     r.categoryid = cm.id   
 LEFT JOIN CopyrightMaster cpm      
   on      
   r.CopyRightId = cpm.id     
     LEFT JOIN SubCategoryMaster sbm      
     ON r.subcategoryId = sbm.ID  WHERE r.IsApproved = 1    
     UNION ALL    
     SELECT c.Id,c.Title,c.CourseDescription as Description,1 as ContentType,Thumbnail,c.CreatedOn     
      from coursemaster c LEFT JOIN CategoryMaster cm      
     on  
     c.categoryid = cm.id      
     LEFT JOIN SubCategoryMaster sbm      
     ON c.subcategoryId = sbm.ID  WHERE c.IsApproved = 1    
     ;with sqlpaging as (    
     SELECT Rownumber = ROW_NUMBER() OVER(ORDER BY CreatedOn desc),Id,Title,Description,ContentType,Thumbnail,CTitle, CTitle_Ar ,Cdescription ,CDescription_ar ,  media      
      from @Table    
      )    
     select    
      top ('+CONVERT(VARCHAR(MAX),@PageSize)+') *,    
      (select max(rownumber) from sqlpaging) as     
      Totalrows    
     from sqlpaging    
     where Rownumber between '+CONVERT(VARCHAR(MAX),@start)+' and '+CONVERT(VARCHAR(MAX),@end)+'    
     Order by Id desc '     
   END  
    --select count(*) as testingcount from @TABLE  
   END    
   END    
ELSE    
BEGIN    
  SET @QUery = '    
DECLARE @Table TABLE(Id NUMERIC, Title NVARCHAR(MAX),Description NVARCHAR(MAX),ContentType INT, Thumbnail NVARCHAR(MAX), CreatedOn DateTime,CTitle NVARCHAR(MAX), CTitle_Ar NVARCHAR(MAX), Cdescription NVARCHAR(MAX),CDescription_ar NVARCHAR(MAX), media NVARCHAR(MAX))    
INSERT INTO @Table    
SELECT r.Id,r.Title,r.ResourceDescription as Description,2 as ContentType,Thumbnail, r.CreatedOn,cpm.Title as CTitle, Title_Ar as  CTitle_Ar , cpm.Description as  Cdescription ,cpm.Description_Ar as  CDescription_Ar , cpm.media as media      
 from resourcemaster r LEFT JOIN CategoryMaster cm      
on      
r.categoryid = cm.id    
LEFT JOIN CopyrightMaster cpm      
   on      
   r.CopyRightId = cpm.id    
LEFT JOIN SubCategoryMaster sbm      
ON r.subcategoryId = sbm.ID  WHERE r.IsApproved = 1    
UNION ALL    
SELECT c.Id,c.Title,c.CourseDescription as Description,1 as ContentType,Thumbnail, c.CreatedOn ,cpm.Title as CTitle, Title_Ar as  CTitle_Ar , cpm.Description as  Cdescription ,cpm.Description_Ar as  CDescription_Ar , cpm.media as media     
 from coursemaster c LEFT JOIN CategoryMaster cm      
on      
c.categoryid = cm.id
 LEFT JOIN CopyrightMaster cpm      
   on      
   c.CopyRightId = cpm.id        
LEFT JOIN SubCategoryMaster sbm      
ON c.subcategoryId = sbm.ID  WHERE c.IsApproved = 1    
;with sqlpaging as (    
SELECT Rownumber = ROW_NUMBER() OVER(ORDER BY CreatedOn desc),Id,Title,Description,ContentType,Thumbnail,CTitle, CTitle_Ar ,Cdescription ,CDescription_ar ,  media      
 from @Table    
 )    
select    
 top ('+CONVERT(VARCHAR(MAX),@PageSize)+') *,    
 (select max(rownumber) from sqlpaging) as     
 Totalrows    
from sqlpaging    
where Rownumber between '+CONVERT(VARCHAR(MAX),@start)+' and '+CONVERT(VARCHAR(MAX),@end)+'    
Order by Id desc '      
END    

--return @Query;

EXEC sp_executesql @Query       
-- WHERE c.Keywords Like '%'+CAST(@Keyword AS NVARCHAR(100))+'%'      
      
--OR cm.Name Like '%'+CAST(@Keyword AS NVARCHAR(100))+'%'      
      
--OR sbm.Name Like '%'+CAST(@Keyword AS NVARCHAR(100))+'%'      
      --drop table @TempTable
 SET @Return = 105;      
      
 RETURN @Return;      
      
END;     
    
    

GO
/****** Object:  StoredProcedure [dbo].[sps_RemixPreviousVersion]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sps_RemixPreviousVersion]
	(
	@ResourceRemixedID INT
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @Id INT;
	SELECT TOP 1 @Id = ResourceSourceId from ResourceRemixHistory WHERE ResourceRemixedID =@ResourceRemixedID order by [version]

	exec GetResourceById  @Id

	IF(@Id IS NOT NULL)
	RETURN 105
	ELSE
	RETURN 102
END

GO
/****** Object:  StoredProcedure [dbo].[sps_ReportAbuseContent]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

/*___________________________________________________________________________________________________            
            
Copyright (c) YYYY-YYYY XYZ Corp. All Rights Reserved            
WorldWide.            
            
$Revision:  $1.0            
$Author:    $ Prince Kumar          
&Date       June 02, 2019          
          
Ticket: Ticket URL    
delete from reportabusereport       
            
PURPOSE:             
This store procedure will get all the content which is report abuse          
          
EXEC sps_ReportAbuseContent           
___________________________________________________________________________________________________*/          
CREATE PROCEDURE [dbo].[sps_ReportAbuseContent]           
AS          
BEGIN          
          
SET NOCOUNT ON;          
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;          
          
          
DECLARE @Return INT;          
          
    
Create table #tempReport    
(    
Id decimal ,    
Title NVARCHAR(500),    
ReportAbuseCount INT,    
Description NVARCHAR(500),    
ContentType INT,    
IsHidden BIT,    
Reason NVARCHAR(500),    
IsDeleted BIT,    
UpdateDate DATETIME ,
ContentId INT
    
)    
    
INSERT INTO #tempReport(    
Id,    
Title,    
ReportAbuseCount,    
Description,    
ContentType,    
IsHidden,    
Reason,    
IsDeleted,    
UpdateDate,
ContentId
)    
          
SELECT ca.ID,cm.Title,cm.ReportAbuseCount,        
ca.comments as Description,        
1 as ContentType,ca.IsHidden,ca.Reason,ca.IsHidden as IsDeleted,ca.UpdateDate,cm.id as ContentId  FROM        
CourseAbuseReports ca inner join CourseMaster cm on ca.courseid = cm.id   
and cm.ReportAbuseCount > 0      
-- WHERE ca.IsHidden <> 1         
union all          
SELECT rc.ID,rm.Title,rm.ReportAbuseCount,        
rc.Comments as Description,        
2 as ContentType,rc.IsHidden,rc.Reason,rc.IsHidden as IsDeleted,rc.UpdateDate,rm.id as ContentId    FROM        
ResourceAbuseReports rc INNER join ResourceMaster rm on rc.ResourceId = rm.Id  
and rm.ReportAbuseCount > 0
--where        
--rc.IsHidden <>1         
        
          
union all         
        
select rc.Id as ID,        
rm.title,        
rc.ReportAbuseCount,        
rc.comments as Description,        
3 as ContentType,        
rc.IsHidden,rc.Reason,rc.IsHidden as IsDeleted,rc.UpdateDate,rm.id as ContentId   from [dbo].[ResourceComments] rc        
 INNER join resourcemaster rm on rc.ResourceId = rm.id
 and rc.ReportAbuseCount > 0        
 --WHERE rc.IsHidden <> 1         
        
Union all        
        
        
select rc.Id as ID,        
rm.title,rc.ReportAbuseCount,rc.comments as Description,4 as ContentType,rc.IsHidden ,rc.Reason,      
rc.IsHidden as IsDeleted  ,rc.UpdateDate ,rm.id as ContentId     
from [dbo].[CourseComments] rc INNER join coursemaster rm on rc.CourseID = rm.id        
 --WHERE --rc.IsHidden <> 1 AND      
AND rc.ReportAbuseCount > 0       
     
    
    
 SELECT * FROM #tempReport order by UpdateDate desc    
    
DROP table #tempReport    
 SET @Return = 105;          
          
 RETURN @Return;          
          
END;   


GO
/****** Object:  StoredProcedure [dbo].[sps_ResourceMasterData]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================          
-- Author:  <Author,,Name>          
-- Create date: <Create Date,,>          
-- Description: <Description,,>          
-- =============================================          
CREATE PROCEDURE [dbo].[sps_ResourceMasterData]          
          
AS          
BEGIN          
 -- SET NOCOUNT ON added to prevent extra result sets from          
 -- interfering with SELECT statements.          
 SET NOCOUNT ON;          
          
    -- Insert statements for procedure here          
 select Id,[Standard],Standard_Ar from [dbo].[lu_Educational_Standard] where Active = 1 and Status = 1   
          
select Id,          
EducationalUse,EducationalUse_Ar from [dbo].[lu_Educational_Use] where Active = 1    and Status = 1       
          
select Id,          
[Level],Level_Ar from [dbo].[lu_Level] where Active = 1  and Status = 1         
          
        
SELECT cm.Id,          
    cm.Name,          
    cm.Name_Ar FROM CategoryMaster cm where cm.Active = 1   and cm.Status = 1      
        
        
    SELECT sm.Id,      
    sm.Name,      
    sm.Name_Ar,      
    cm.Name as CategoryName,       
    cm.Id as CategoryId    
   from SubCategoryMaster sm       
   INNER JOIN CategoryMaster cm on sm.CategoryId=cm.Id  
   where sm.Active = 1 and cm.Active = 1   and cm.Status = 1  and sm.Status = 1
        
        
  SELECT cm.Id,          
    cm.Name,          
    cm.Name_Ar        
 from MaterialTypeMaster cm where cm.Active = 1  and cm.Status = 1          
        
        
  SELECT em.Id,          
    em.Name ,Name_Ar from EducationMaster em where em.Active = 1   and em.Status = 1         
        
        
   SELECT cm.Id,          
    cm.Name,Name_Ar        
 from ProfessionMaster cm where cm.Active = 1    and cm.Status = 1        
        
  SELECT cr.Id,          
    cr.Title,          
    Cr.Description,          
    cr.Title_Ar,          
    Cr.Description_Ar  ,  
 Media  
 from CopyrightMaster cr where cr.Active = 1  and cr.Status = 1         
RETURN 105;          
END 
GO
/****** Object:  StoredProcedure [dbo].[sps_ResourcesByKeyword]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

-- =============================================  
-- Author:  <Author,,Name>  
-- Create date: <Create Date,,>  
-- Description: <Description,,>  
-- =============================================  
  
-- sps_ResourcesByKeyword ''  
CREATE PROCEDURE [dbo].[sps_ResourcesByKeyword]  
 (  
	 @SearchKeyword NVARCHAR(200),
	 @PageNo int = 1,  
	 @PageSize int = 5  
 )  
AS  
BEGIN  
   
declare @start int, @end int  
set @start = (@PageNo - 1) * @PageSize + 1  
set @end = @PageNo * @PageSize  
  
  ;with sqlpaging as (  
	 SELECT Rownumber = ROW_NUMBER() OVER(ORDER BY r.Id desc),
	  r.Id  
      ,r.Title  
      ,r.CategoryId, c.Name as CategoryName  
      ,SubCategoryId, sc.Name as SubCategoryName  
      ,Thumbnail  
      ,ResourceDescription  
      ,Keywords  
      ,ResourceContent  
      ,MaterialTypeId, mt.Name as MaterialTypeName  
      ,CopyRightId, cm.Title as CopyrightTitle, cm.Description as CopyrightDescription,
	  cm.Media As Media, cm.Protected AS Protected  
      ,IsDraft  
      ,CONCAT(um.FirstName, ' ', um.LastName) as CreatedBy, um.Id as CreatedById  
      ,r.CreatedOn  
      ,IsApproved  
      ,Rating  
      ,AlignmentRating  
      ,ReportAbuseCount  
	  ,cm.IsResourceProtect
  FROM ResourceMaster r  
   inner join CategoryMaster c on r.CategoryId = c.Id  
   inner join MaterialTypeMaster mt on r.MaterialTypeId=mt.Id  
   left join SubCategoryMaster sc on r.SubCategoryId=sc.Id  
   left join CopyrightMaster cm on r.CopyRightId = cm.Id  
   inner join UserMaster um on r.CreatedBy =um.Id   
   WHERE r.Title Like '%'+@SearchKeyword+'%' AND r.IsApproved = 1  
  
  
    )  
   select  
 top (@PageSize) *,  
 (select max(rownumber) from sqlpaging) as   
 Totalrows  
from sqlpaging  
where Rownumber between @start and @end  

  
  
  
RETURN 105;  
END  
GO
/****** Object:  StoredProcedure [dbo].[sps_ResourcesByUserId]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*___________________________________________________________________________________________________  
  
Copyright (c) YYYY-YYYY XYZ Corp. All Rights Reserved  
WorldWide.  
  
$Revision:  $1.0  
$Author:    $ Prince Kumar
&Date       June 06, 2019

Ticket: Ticket URL
  
PURPOSE:   
This store procedure will get all reasources based upon user id.

EXEC sps_OerDashboardReport 10
___________________________________________________________________________________________________*/
CREATE PROCEDURE [dbo].[sps_ResourcesByUserId] 
(
@UserId INT
)
AS
BEGIN

SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

DECLARE @Return INT;
SELECT r.Id
      ,r.Title
      ,r.CategoryId, c.Name as CategoryName
      ,SubCategoryId, sc.Name as SubCategoryName
      ,Thumbnail
      ,ResourceDescription
      ,Keywords
      ,ResourceContent
      ,MaterialTypeId, mt.Name as MaterialTypeName
      ,CopyRightId, cm.Title as CopyrightTitle, cm.Description as CopyrightDescription
      ,IsDraft
      ,CONCAT(um.FirstName, '', um.LastName) as CreatedBy, um.Id as CreatedById
      ,r.CreatedOn
      ,IsApproved
      ,Rating
      ,AlignmentRating
      ,ReportAbuseCount,ViewCount,AverageReadingTime,DownloadCount,SharedCount
	  ,r.ReadingTime
  FROM ResourceMaster r
		 inner join CategoryMaster c on r.CategoryId = c.Id
		 LEFT join MaterialTypeMaster mt on r.MaterialTypeId=mt.Id
		 left join SubCategoryMaster sc on r.SubCategoryId=sc.Id
		 left join CopyrightMaster cm on r.CopyRightId = cm.Id
		 inner join UserMaster um on r.CreatedBy =um.Id where um.Id=@UserID	Order by r.Id desc 


 SET @Return = 105;

 RETURN @Return;

END;
GO
/****** Object:  StoredProcedure [dbo].[sps_TestResults]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sps_TestResults]
(
@UserId INT,
@CourseId INT,
@UserTestId NVARCHAR(MAX)
)
AS
BEGIN
DECLARE @Query NVARCHAR(MAX)
SET @Query = 
	'SELECT FirstName + '' '' + LastName AS UserName , Email, q.Id,(SELECT AnswerOption FROM AnswerOptions WHERE id = u.AnswerId) As UserAnswer, QuestionText,CorrectOption,AnswerOption 
	FROM UserCourseTests u
	INNER JOIN Questions q
		ON q.Id = u.QuestionId
	INNER JOIN AnswerOptions a
		ON q.Id = a.QuestionId
	INNER JOIN UserMaster um
		ON um.Id = u.UserId
	INNER JOIN Tests t
		ON q.TestId = t.Id
	WHERE u.CourseId ='+ CAST(@CourseId AS VARCHAR(100))+' and UserId = '+CAST(@UserId AS VARCHAR(100))+' AND u.Id IN ('+@UserTestId+')
	ORDER BY q.Id'
	EXEC sp_executesql @Query
END
GO
/****** Object:  StoredProcedure [dbo].[sps_UserByEmail]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sps_UserByEmail] -- 'shubhdeep.s@pintlab.com' 
(  
@Email NVARCHAR(500) 
)  
AS  
BEGIN   
  
IF Exists(SELECT TOP 1 1 from UserMaster WHERE Email = @Email)   
 BEGIN   
   
 SELECT TOP 1 IsEmailNotification  
  FROM UserMaster 
     WHERE Email = @Email
  
RETURN 105 -- record exists  
  
END  
  
ELSE RETURN 102 -- No record exist  
  
END 

GO
/****** Object:  StoredProcedure [dbo].[sps_UserById]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sps_UserById]   
(  
@Id INT 
)  
AS  
BEGIN   
  
IF Exists(SELECT TOP 1 1 from UserMaster WHERE Id=@Id)   
 BEGIN   
  

  
 SELECT um.Id  
      ,um.TitleId, tm.[Name] AS Title  
      ,um.FirstName  
      ,um.MiddleName  
      ,um.LastName  
      ,um.CountryId, cm.[Name] AS Country  
      ,um.StateId, sm.[Name] AS [State]  
      ,um.Gender -- Enumeration  
      ,um.Email  
      ,um.PortalLanguageId -- Enumeration  
      --,um.DepartmentId, dm.Name as Department  
     -- ,um.DesignationId, dsm.Name as Designation  
      ,um.DateOfBirth  
      ,um.Photo  
      ,um.ProfileDescription  
      ,um.SubjectsInterested  
      ,um.ApprovalStatus  
      ,um.CreatedOn  
      ,um.UpdatedOn  
      ,um.Active  
      ,um.IsContributor, um.IsAdmin, um.IsEmailNotification  
  FROM UserMaster um left JOIN TitleMaster tm on um.titleId=tm.Id  
      left JOIN CountryMaster cm on um.CountryId=cm.Id  
      left JOIN StateMaster sm on um.StateId=sm.Id  
     -- INNER JOIN DepartmentMaster dm on um.DepartmentId=dm.Id  
     -- INNER JOIN DesignationMaster dsm on um.DesignationId=dsm.Id  
  
     where um.Id=@Id  
 
 
 SELECT uc.Id
      ,UserId
      ,CertificationName
      ,[Year]
      ,uc.CreatedOn
  FROM UserCertification uc INNER JOIN UserMaster um on um.Id=uc.UserId WHERE um.Id=@Id

SELECT ue.Id
      ,UserId
      ,UniversitySchool
      ,Major
      ,Grade
      ,FromDate
      ,ToDate
      ,ue.CreatedOn
  FROM UserEducation ue INNER JOIN UserMaster um on um.Id=ue.UserId WHERE um.Id=@Id

  SELECT ul.Id
      ,UserId
      ,LanguageId, lm.[Name] as [Language]
      ,IsSpeak
      ,IsRead
      ,IsWrite
      ,ul.CreatedOn
  FROM UserLanguages ul INNER JOIN LanguageMaster lm on lm.Id=ul.LanguageId  
						INNER JOIN UserMaster um on um.Id=ul.UserId WHERE um.Id=@Id

  SELECT uexp.Id
      ,UserId
      ,OrganizationName
      ,Designation
      ,FromDate
      ,ToDate
      ,uexp.CreatedOn
  FROM dbo.UserExperiences uexp INNER JOIN UserMaster um on um.Id=uexp.UserId WHERE um.Id=@Id
  
SELECT sm.Id
      ,UserId
      ,SocialMediaId,smm.Name as SocialMedia
	  ,smm.Name_Ar
      ,[URL]
      ,sm.CreatedOn
  FROM dbo.UserSocialMedia sm INNER JOIN SocialMediaMaster smm on sm.SocialMediaId=smm.Id
  
  
  INNER JOIN UserMaster um on um.Id=sm.UserId WHERE um.Id=@Id

RETURN 105 -- record exists  
  
END  
  
ELSE RETURN 102 -- No record exist  
  
END  
GO
/****** Object:  StoredProcedure [dbo].[sps_Users]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*___________________________________________________________________________________________________  
  
Copyright (c) YYYY-YYYY XYZ Corp. All Rights Reserved  
WorldWide.  
  
$Revision:  $1.0  
$Author:    $ Prince Kumar
&Date       June 06, 2019

Ticket: Ticket URL
  
PURPOSE:   
This store procedure will returns all users information.

EXEC sps_Users 
___________________________________________________________________________________________________*/
CREATE PROCEDURE [dbo].[sps_Users] 
AS
BEGIN

SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;


DECLARE @Return INT;
SELECT um.Id
      ,um.TitleId, tm.[Name] AS Title
      ,um.FirstName
      ,um.MiddleName
      ,um.LastName
      ,um.CountryId, cm.[Name] AS Country
      ,um.StateId, sm.[Name] AS [State]
      ,um.Gender -- Enumeration
      ,um.Email
      ,um.PortalLanguageId -- Enumeration
      --,um.DepartmentId, dm.Name as Department
     -- ,um.DesignationId, dsm.Name as Designation
      ,um.DateOfBirth
      ,um.Photo
      ,um.ProfileDescription
      ,um.SubjectsInterested
      ,um.ApprovalStatus
      ,um.CreatedOn
      ,um.UpdatedOn
      ,um.Active
      ,um.IsContributor, um.IsAdmin
  FROM UserMaster um left JOIN TitleMaster tm on um.titleId=tm.Id
					 left JOIN CountryMaster cm on um.CountryId=cm.Id
					 left JOIN StateMaster sm on um.StateId=sm.Id

 SET @Return = 105;

 RETURN @Return;

END;
GO
/****** Object:  StoredProcedure [dbo].[sps_Visiters]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sps_Visiters]
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT  Count(ID) AS TotalVisit FROM Visiters
	RETURN 105;
END
GO
/****** Object:  StoredProcedure [dbo].[spu_Announcements]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

--=======================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

-- spu_Announcements @Id=1,@Text='This is updated Text',@Updatedby=112,@Active=1
CREATE PROCEDURE [dbo].[spu_Announcements]
	(
	@Id INT,
	@Text NVARCHAR(500),
	@Updatedby INT,
	@Active BIT,
	@Text_Ar NVARCHAR(500)
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	Declare @return INT;
  
IF EXISTS (SELECT TOP 1 1 FROM Announcements WHERE Id = @Id)  
	BEGIN  
				UPDATE Announcements
				SET 
					[Text] = @Text,
					UpdatedOn = GETDATE(),
					UpdatedBy = @Updatedby,
					Active  = @Active,
					Text_Ar = @Text_Ar
				WHERE Id = @Id

				  IF @@ERROR <> 0  
					SET @return= 106 -- update failed   
				 ELSE  
				  SET @return= 101; -- update success   
				 
	END

ELSE
	BEGIN
	  SET @return= 102; -- Record does not exists  
	END

			SELECT el.Id,
					el.[Text],
					el.[Text_Ar],
					CONCAT(c.FirstName, '', c.LastName) as CreatedBy,  
					CONCAT(l.FirstName, '', l.LastName) as UpdatedBy, 
					el.CreatedOn,
					el.UpdatedOn,
					el.Active FROM Announcements  el 
			   INNER join UserMaster c  on el.CreatedBy= c.Id  
			   INNER join UserMaster l on el.UpdatedBy =l.Id 
			   WHERE el.Active = 1 AND el.id = @Id
	 RETURN @return;  
END

GO
/****** Object:  StoredProcedure [dbo].[spu_AppTheme]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spu_AppTheme]
	(
	@UserId INT,
	@Theme NVARCHAR(50)
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @Result INT
    -- Insert statements for procedure here
	IF EXISTS(SELECT TOP 1 1 FROM UserMaster WHERE ID = @UserId)
		BEGIN
			UPDATE UserMaster SET Theme = @Theme WHERE ID = @UserId;
			SET @Result= 101;
		END
		ELSE
		BEGIN
	      SET @Result= 102;
		END;

		RETURN @Result;

END
GO
/****** Object:  StoredProcedure [dbo].[spu_CommunityApproveRejectCount]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spu_CommunityApproveRejectCount]
(
  @ApproveCount INT,
  @RejectCount INT,
  @UserId INT
)
AS
BEGIN
UPDATE CommunityApproveRejectCount SET ApproveCount = @ApproveCount, RejectCount = @RejectCount , LastUpdatedBy = @UserId,
LastUpdatedOn = GETDATE()
RETURN 101
END
GO
/****** Object:  StoredProcedure [dbo].[spu_ContactUs]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

-- spu_ContactUs @Id=1,@RepliedBy=66,@RepliedText='your query has been resolved.'
CREATE PROCEDURE [dbo].[spu_ContactUs]
	(
	@Id INT,
	@RepliedBy INT,
	@RepliedText NVARCHAR(Max)
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @Return INT;
	IF EXISTS(SELECT TOP 1 1 FROM Contactus WHERE ID = @Id AND IsReplied IS NULL)
	BEGIN
			UPDATE Contactus SET 
			IsReplied = 1,
			RepliedBy = @RepliedBy,
			RepliedOn = GETDATE(),
			RepliedText = @RepliedText
			WHERE ID = @Id;
    -- Insert statements for procedure here
	SET @Return = 101;
	END

	ELSE
	BEGIN
		SET @Return = 106;
	END;

	RETURN @Return;
END
GO
/****** Object:  StoredProcedure [dbo].[spu_ContentStatus]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spu_ContentStatus]
	(
	@UserId INT,
	@ContentId INT,
	@ContentType INT,
	@Status INT
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

   IF(@ContentType =1)
  BEGIN
  Update coursemaster SET IsApproved = @Status, MoEBadge = 1 WHERE ID = @ContentId
  RETURN 101
  END
  ELSE
  BEGIN
    Update ResourceMaster SET IsApproved = @Status, MoEBadge = 1 WHERE ID = @ContentId
	RETURN 101
  END
END
GO
/****** Object:  StoredProcedure [dbo].[spu_ContentWithdrawal]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- =============================================  
-- Author:  <Author,,Name>  
-- Create date: <Create Date,,>  
-- Description: <Description,,>  
-- =============================================  
CREATE PROCEDURE [dbo].[spu_ContentWithdrawal]  
 (  
 @ContentId INT,  
 @ContentType INT  
 )  
AS  
BEGIN  
  
DECLARE @Result INT;  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
 IF NOT EXISTS(SELECT TOP 1 1 FROM contentapproval Where [Status] in (0,1)   AND ContentId = @ContentId AND ContentType = @ContentType)  
 BEGIN  
     IF(@ContentType = 1)  
   BEGIN  
   Update CourseMaster SET IsDraft = 1,IsApproved = 0 WHERE ID = @ContentId  
   DELETE FROM ContentApproval WHERE ContentId = @ContentId AND ContentType = 1
   END  
  
  ELSE  
  
   BEGIN  
    Update ResourceMaster SET IsDraft = 1,IsApproved = 0 WHERE ID = @ContentId 
	DELETE FROM ContentApproval WHERE ContentId = @ContentId AND ContentType = 2
   END  
   SET @Result = 101;  
 END  
    -- Insert statements for procedure here  
 ELSE  
  
 BEGIN  
  SET @Result = 119;  
 END  
 RETURN @Result;  
END  

GO
/****** Object:  StoredProcedure [dbo].[spu_CourseTest]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spu_CourseTest]
	(
	@CourseId INT,
	@TestName NVARCHAR(100),
	@UT_Questions [UT_QuestionsForContent] Readonly,
	@UT_AnswerOptions [UT_AnswerOptions] Readonly,
	@CreatedBy INT
	)
AS
BEGIN
	
DECLARE @QuestionCount INT;
DECLARE @TestID INT;
DECLARE @QID INT;
DECLARE @i INT;
SET @i = 1;


IF EXISTS(SELECT TOP 1 1 FROM tests WHERE TestName = @TestName and CourseID = @CourseId)

BEGIN
SELECT @TestID = Id from Tests WHERE TestName = @TestName
DELETE FROM  usercoursetests WHERE CourseID = @CourseId
DELETE FROM answeroptions WHERE QuestionId IN (Select Id FROM Questions WHERE  TestId = @TestID)
DELETE FROM Questions WHERE TestId = @TestID

SET @QuestionCount = (SELECT Count(*) FROM @UT_Questions)


WHILE @i <= @QuestionCount

BEGIN

INSERT INTO questions
(
QuestionText,
TestId,
Media,
FileName
)

SELECT QuestionText,@TestID,Media,[FileName] FROM @UT_Questions WHERE QuestionId = @i

SET @QID = SCOPE_IDENTITY();

INSERT INTO answeroptions(QuestionId,
AnswerOption,
CorrectOption)

SELECT 
@QID,
OptionText,
CorrectOption FROM @UT_AnswerOptions WHERE QuestionId =(SELECT  QuestionId FROM @UT_Questions WHERE QuestionId = @i)
SET @i= @i+1;
END;



END;

RETURN 100;
END



GO
/****** Object:  StoredProcedure [dbo].[spu_EmailNotification]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spu_EmailNotification]
	(
	@UserId INT,
	@IsEmailNotification BIT
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

IF EXISTS(SELECT TOP 1 1 FROM UserMaster WHERE ID = @UserId)
BEGIN

Update UserMaster SET IsEmailNotification = @IsEmailNotification WHERE ID = @UserId
RETURN 101
END
ELSE
BEGIN
RETURN 102
END;

END
GO
/****** Object:  StoredProcedure [dbo].[spu_lu_Educational_Standard]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spu_lu_Educational_Standard]
	(
	@Id INT,
	@Standard NVARCHAR(200),
	@Standard_Ar NVARCHAR(200) = NULL,
	@Updatedby INT,
	@Active BIT
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	Declare @return INT;
  
IF EXISTS (SELECT TOP 1 1 FROM lu_Educational_Standard WHERE Id = @Id)  
	BEGIN  
				UPDATE lu_Educational_Standard
				SET 
					[Standard] = @Standard,
					Standard_Ar = @Standard_Ar,
					UpdatedOn = GETDATE(),
					UpdatedBy = @Updatedby,
					Active  = @Active
				WHERE Id = @Id

				  IF @@ERROR <> 0  
					SET @return= 106 -- update failed   
				 ELSE  
				  SET @return= 101; -- update success   
				 
	END

ELSE
	BEGIN
	  SET @return= 102; -- Record does not exists  
	END

		SELECT es.Id,
			es.[Standard],
			es.[Standard_Ar],
			CONCAT(c.FirstName, '', c.LastName) as CreatedBy,  
			CONCAT(l.FirstName, '', l.LastName) as UpdatedBy, 
			es.CreatedOn,
			es.UpdatedOn,
			es.Active 
			FROM lu_Educational_Standard  es
			   INNER join UserMaster c  on es.CreatedBy= c.Id  
			   INNER join UserMaster l on es.UpdatedBy =l.Id 
		WHERE es.Status = 1 AND es.id = @Id;
	 RETURN @return;  
END
GO
/****** Object:  StoredProcedure [dbo].[spu_lu_Educational_Use]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spu_lu_Educational_Use]
	(
	@Id INT,
	@EducationalUse NVARCHAR(200),
	@EducationalUse_Ar NVARCHAR(200) = NULL,
	@Updatedby INT,
	@Active BIT
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	Declare @return INT;
  
IF EXISTS (SELECT TOP 1 1 FROM lu_Educational_Use WHERE Id = @Id)  
	BEGIN  
				UPDATE lu_Educational_Use
				SET 
					EducationalUse = @EducationalUse,
					EducationalUse_Ar = @EducationalUse_Ar,
					UpdatedOn = GETDATE(),
					UpdatedBy = @Updatedby,
					Active  = @Active
				WHERE Id = @Id

				  IF @@ERROR <> 0  
					SET @return= 106 -- update failed   
				 ELSE  
				  SET @return= 101; -- update success   
				 
	END

ELSE
	BEGIN
	  SET @return= 102; -- Record does not exists  
	END

		SELECT eu.Id,
			eu.EducationalUse,
			eu.EducationalUse_Ar,
			CONCAT(c.FirstName, '', c.LastName) as CreatedBy,  
			CONCAT(l.FirstName, '', l.LastName) as UpdatedBy, 
			eu.CreatedOn,
			eu.UpdatedOn,
			eu.Active 
			FROM lu_Educational_Use  eu 
			   INNER join UserMaster c  on eu.CreatedBy= c.Id  
			   INNER join UserMaster l on eu.UpdatedBy =l.Id 
		WHERE eu.Status = 1 AND eu.id = @Id;
	 RETURN @return;  
END
GO
/****** Object:  StoredProcedure [dbo].[spu_lu_Level]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--=======================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spu_lu_Level]
	(
	@Id INT,
	@Level NVARCHAR(200),
	@Level_Ar NVARCHAR(200) = NULL,
	@Updatedby INT,
	@Active BIT
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	Declare @return INT;
  
IF EXISTS (SELECT TOP 1 1 FROM lu_Level WHERE Id = @Id)  
	BEGIN  
				UPDATE lu_Level
				SET 
					[Level] = @Level,
					Level_Ar = @Level_Ar,
					UpdatedOn = GETDATE(),
					UpdatedBy = @Updatedby,
					Active  = @Active
				WHERE Id = @Id

				  IF @@ERROR <> 0  
					SET @return= 106 -- update failed   
				 ELSE  
				  SET @return= 101; -- update success   
				 
	END

ELSE
	BEGIN
	  SET @return= 102; -- Record does not exists  
	END

			SELECT el.Id,
					el.[Level],
					el.Level_Ar,
					CONCAT(c.FirstName, '', c.LastName) as CreatedBy,  
					CONCAT(l.FirstName, '', l.LastName) as UpdatedBy, 
					el.CreatedOn,
					el.UpdatedOn,
					el.Active FROM lu_Level  el 
			   INNER join UserMaster c  on el.CreatedBy= c.Id  
			   INNER join UserMaster l on el.UpdatedBy =l.Id 
			   WHERE el.Status = 1 AND el.id = @ID
	 RETURN @return;  
END
GO
/****** Object:  StoredProcedure [dbo].[spu_Notifications]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spu_Notifications]
	(
	@UserID INT,
	@NotificationId INT
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @Return INT
    -- Insert statements for procedure here
UPDATE Notifications SET IsRead = 1, ReadDate = GETDATE() WHERE ID = @NotificationId AND UserId = @UserID


SET @Return = 101

RETURN @Return
END
GO
/****** Object:  StoredProcedure [dbo].[spu_QRCUserMapping]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spu_QRCUserMapping] 
	(
	@QrcID INT,
	@CategoryID INT,
	@UserID INT
	)
AS
BEGIN
	
	UPDATE QRCUserMapping SET Active = 0 WHERE QRCId = @QrcID AND CategoryID =@CategoryID AND UserId = @UserID
	RETURN 101 -- Updated.
END
GO
/****** Object:  StoredProcedure [dbo].[spu_UserLastLogin]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spu_UserLastLogin]
	(
	@UserId INT
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

UPDATE UserMaster SET LastLogin = GETDATE() WHERE ID = @UserId;
RETURN 101; -- Update successfully.

END
GO
/****** Object:  StoredProcedure [dbo].[spu_UserMasterStatus]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

-- spu_UserMasterStatus 66,0
CREATE PROCEDURE [dbo].[spu_UserMasterStatus]
(
@UserId INT,
@Status Bit
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	update UserMaster SET Active = @Status WHERE ID = @UserId
	RETURN 101
END
GO
/****** Object:  StoredProcedure [dbo].[spu_WebPageContent]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================    
-- Author:  <Author,,Name>    
-- Create date: <Create Date,,>    
-- Description: <Description,,>    
-- =============================================    
CREATE PROCEDURE [dbo].[spu_WebPageContent]    
 (    
 @PageID INT,     
 @PageContent NVARCHAR(MAX),    
 @PageContent_Ar NVARCHAR(MAX),    
 @UpdatedBy INT    
 )    
AS    
BEGIN    
IF EXISTS(SELECT TOP 1 1 FROM WebPageContent  WHERE PageId =@PageID)    
BEGIN    
 UPDATE WebPageContent    
    
 SET PageContent = @PageContent,    
  PageContent_Ar = @PageContent_Ar,  
  UpdatedBy = @UpdatedBy,    
  UpdatedOn = GETDATE()    
    
 WHERE  PageId =@PageID    
    
RETURN 101;    
END    
    
ELSE    
    
BEGIN    
RETURN 105    
END;    
    
END 
GO
/****** Object:  StoredProcedure [dbo].[UpdateCategory]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UpdateCategory] 
	@Id INT,
	@Name NVARCHAR(100),
	@Name_Ar NVARCHAR(300),
	@UpdatedBy INT,
	@Active BIT
AS
BEGIN

Declare @return INT

IF EXISTS (SELECT TOP 1 1 FROM CategoryMaster WHERE Id = @Id)
BEGIN	
		UPDATE CategoryMaster  SET Name=@Name,
		Name_Ar = @Name_Ar,
									  UpdatedBy=@UpdatedBy,
									  UpdatedOn=GETDATE(), Active=@Active
									  WHERE Id=@Id
		-- do log entry here
		 
		  IF @@ERROR <> 0
		
		  SET @return= 106 -- update failed	
	ELSE
		SET @return= 101 -- update success	
END

ELSE
	BEGIN
		SET @return= 102 -- Record does not exists
	END

	SELECT	cm.Id,
				cm.Name,
				cm.Name_Ar,
				cm.CreatedOn,				
			 CONCAT(c.FirstName, '', c.LastName) as CreatedBy,
				CONCAT(l.FirstName, '', l.LastName) as UpdatedBy,
				cm.UpdatedOn, cm.Active
		 from CategoryMaster cm 		 
		 inner join UserMaster c  on cm.CreatedBy= c.Id
		 inner join UserMaster l on cm.UpdatedBy =l.Id 	
		 where cm.Status = 1
		 order by cm.Id desc 

		 RETURN @return
END

GO
/****** Object:  StoredProcedure [dbo].[UpdateCopyright]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
      
-- exec UpdateCopyright @Id=1006,@Title='testsdfdf',@Description='dfsfds',@UpdatedBy=84,@Active=1,@Media='x3d.png'      
CREATE PROCEDURE [dbo].[UpdateCopyright]       
 @Id INT,      
 @Title NVARCHAR(100),      
 @Description Text,      
 @Title_Ar NVARCHAR(100),      
 @Description_Ar NVARCHAR(MAX),      
 @UpdatedBy INT,       
 @Active BIT,      
 @Media NVARCHAR(200) = NULL,    
 @Protected BIT = 0,
 @IsResourceProtect BIT = 0
AS      
BEGIN      
Declare @return INT      
IF EXISTS (SELECT TOP 1 1 FROM CopyrightMaster WHERE  Id<> @Id)      
BEGIN       
  UPDATE CopyrightMaster  SET Title=@Title,       
  [Description]=@Description,      
  Title_Ar = @Title_Ar,      
  Description_Ar = @Description_Ar,      
  UpdatedBy = @UpdatedBy,        
  UpdatedOn=GETDATE(),       
  Active=@Active,       
  Media = @Media  ,    
  Protected = @Protected,
  IsResourceProtect = @IsResourceProtect
  WHERE Id=@Id      
  -- do log entry here       
      
    IF @@ERROR <> 0      
          
   SET @return = 106 -- update failed       
    ELSE      
   SET @return = 101 -- update success       
END      
      
ELSE      
 BEGIN      
  SET @return =105 -- Record exists      
 END      
      
 SELECT cr.Id,      
    cr.Title,      
    Cr.[Description],      
    cr.Title_Ar,      
    Cr.Description_Ar,      
    cr.CreatedOn,      
    CONCAT(c.FirstName, '', c.LastName) as CreatedBy,      
    CONCAT(l.FirstName, '', l.LastName) as UpdatedBy, cr.Active, cr.UpdatedOn,cr.Media  ,cr.Protected, cr.IsResourceProtect    
   from CopyrightMaster cr       
   inner join UserMaster c  on cr.CreatedBy= c.Id      
   inner join UserMaster l on cr.UpdatedBy =l.Id where cr.Status=1 order by cr.Id desc       
      
   RETURN @return      
END      
GO
/****** Object:  StoredProcedure [dbo].[UpdateCourse]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateCourse]          
           @Title NVARCHAR(250) = NULL,      
           @CategoryId INT,      
           @SubCategoryId INT=NULL,      
           @Thumbnail  NVARCHAR(400)  = NULL,      
           @CourseDescription  NVARCHAR(2000)  = NULL,      
           @Keywords  NVARCHAR(1500)  = NULL,      
           @CourseContent NTEXT  = NULL,                
           @CopyRightId INT  = NULL,      
           @IsDraft BIT,         
           @EducationId int = NULL,      
           @ProfessionId int = NULL,        
           @References NVARCHAR(MAX)=null,        
           @CourseFiles NVARCHAR(MAX)=null,       
           @UT_Sections [UT_Sections] Readonly,        
           @UT_Resource [UT_Resource] Readonly,     
           @Id INT,      
           @ReadingTime INT = NULL,      
           @LevelId INT = NULL,      
           @EducationalStandardId INT = NULL,      
           @EducationalUseId INT = NULL              
AS      
BEGIN      
DECLARE @i INT;      
DECLARE @SectionId INT;    
SET @i = 1;      
Declare @return INT      
DECLARE @SectionsCount INT;        
IF EXISTS (SELECT * FROM CourseMaster WHERE @Id=@Id)      
BEGIN       
UPDATE CourseMaster SET Title=@Title,      
           CategoryId=@CategoryId,      
           SubCategoryId=@SubCategoryId,      
           Thumbnail=@Thumbnail,      
           CourseDescription=@CourseDescription,      
           Keywords=@Keywords,      
           CourseContent=@CourseContent,                
           CopyRightId=@CopyRightId,      
           IsDraft=@IsDraft,      
           EducationId=@EducationId,      
           ProfessionId=@ProfessionId,      
           ReadingTime=@ReadingTime,      
           LevelId= @LevelId,      
           EducationalStandardId = @EducationalStandardId,      
           EducationalUseId = @EducationalUseId ,
		   IsApproved = NULL     
           WHERE Id=@Id      
  -- do log entry here      
    
SET @SectionsCount = (SELECT Count(*) FROM @UT_Sections)        
      
      
DELETE FROM SectionResource WHERE SectionId in(SELECT ID FROM  CourseSections WHERE CourseId = @Id)    
DELETE FROM CourseSections WHERE CourseId = @Id    
    
WHILE @i <= @SectionsCount        
        
BEGIN        
        
INSERT INTO CourseSections        
(        
Name,        
CourseId        
)        
        
SELECT Name,@Id FROM @UT_Sections WHERE Id = @i        
        
SET @SectionId = SCOPE_IDENTITY();        
        
INSERT INTO [dbo].[SectionResource](ResourceId,        
SectionId)        
SELECT         
ResourceId,        
@SectionId        
FROM @UT_Resource WHERE SectionId in (SELECT distinct  ID FROM @UT_Sections WHERE Id = @i)        
        
SET @i= @i+1;        
END;        
         
    IF @@ERROR <> 0      
        
    SET @return= 106 -- update failed       
      
      
 ELSE      
    BEGIN  
  SET @return= 101 -- update success   
IF(@IsDraft = 0)  
BEGIN  
   DECLARE @TotalCount INT;        
DECLARE @QrcID INT;        
DECLARE @RecordId INT;              
DECLARE @CreatedBy INT        
SELECT @CreatedBy = CreatedBy FROM CourseMaster WHERE Id = @Id  
      
        
select @TotalCount = count(*) from QRCCategory WHERE CategoryID = @CategoryID AND IsAvailable =0 --order by CreatedOn asc        
        
IF(@TotalCount>0)        
BEGIN        
   select TOP 1 @RecordId=ID,@QrcID=QRCId from QRCCategory WHERE CategoryID = @CategoryID AND IsAvailable =0   
   AND QRCID IN (SELECT DISTINCT QRCID FROM QRCUserMapping  
   EXCEPT (SELECT DISTINCT QRCID FROM QRCUserMapping WHERE UserId = @CreatedBy AND Active = 1))  
   order by CreatedOn asc        
        
   Update QRCCategory SET IsAvailable = 1 WHERE ID = @RecordId        
        
   IF EXISTS(        
   SELECT         
     TOP 1 1        
     from QRCusermapping  where QRCID =@QrcID and active = 1 and CategoryId = @CategoryId)        
     BEGIN 
   IF NOT EXISTS(SELECT TOP 1 1 FROM ContentApproval WHERE ContentId = @Id AND ContentType = 1)  
   BEGIN         
      INSERT INTO ContentApproval(ContentId,        
      ContentType,        
      CreatedBy,        
      CreatedOn,        
      AssignedTo,      
      AssignedDate,  
      QrcId)        
        
      SELECT         
      @Id,        
      1, -- Course        
  @CreatedBy,        
      GETDATE(),        
      userid,        
      GETDATE() ,  
      @QrcID  
      from QRCusermapping  where QRCID =@QrcID and active = 1   
      and QRCusermapping.CategoryId = @CategoryId    
  END  
  ELSE  
   BEGIN  
   UPDATE ContentApproval  
   SET [Status] = NULL,  
   ApprovedBy = NULL,  
   ApprovedDate = NULL,  
   UpdatedOn = NULL,  
   UpdatedBy = NULL,  
   Comment = NULL  
   WHERE Id = (SELECT TOP(1) Id FROM ContentApproval WHERE ContentId = @Id AND ContentType = 1)  
   END       
  
    UPDATE CourseMaster SET IsDraft = 0 WHERE Id = @Id  
      END        
    
   
END        
IF(@TotalCount=1)          
BEGIN          
Update QRCCategory SET IsAvailable = 0  WHERE CategoryID = @CategoryID           
END   
 END  
  END      
 --IF NOT EXISTS(SELECT ContentId FROm ContentApproval WHERE ContentId = @id and ContentType = 1)  
 --BEGIN  
 --UPDATE CourseMaster SET IsDraft = 1 WHERE Id = @Id  
 --END   
IF @CourseFiles IS NOT NULL      
 BEGIN      
 -- Update Resource Associated Files FROM JSON      
       
MERGE CourseAssociatedFiles AS TARGET      
  USING      
    (      
    SELECT DISTINCT AssociatedFile,FileName     
      FROM      
      OPENJSON(@CourseFiles)      
      WITH (AssociatedFile nvarchar(500) '$.AssociatedFile',FileName nvarchar(500) '$.FileName')      
    ) AS source (AssociatedFile,FileName)      
  ON TARGET.AssociatedFile = source.AssociatedFile AND TARGET.FileName = source.FileName
    AND TARGET.CourseId = @Id     
  WHEN NOT MATCHED       
  THEN       
    INSERT (CourseId,AssociatedFile,FileName, CreatedOn)      
      VALUES (@Id,source.AssociatedFile,source.FileName,GETDATE())
  WHEN NOT MATCHED BY SOURCE AND TARGET.CourseId = @Id
    THEN DELETE;       
      
 END      
      
 IF @References IS NOT NULL      
 BEGIN      
MERGE CourseURLReferences AS TARGET      
  USING      
    (      
    SELECT DISTINCT URLReferenceId      
      FROM      
      OPENJSON(@References)      
      WITH (URLReferenceId int '$.URLReferenceId')      
    ) AS source (URLReferenceId)      
  ON TARGET.URLReferenceId = source.URLReferenceId  
  AND TARGET.CourseId = @Id   
   WHEN MATCHED THEN     
        UPDATE SET CourseId = @Id,     
                   URLReferenceId =source.URLReferenceId,     
                   CreatedOn = GETDATE()     
  WHEN NOT MATCHED       
  THEN       
    INSERT (CourseId,URLReferenceId, CreatedOn)      
      VALUES (@Id,source.URLReferenceId,GETDATE())
  	WHEN NOT MATCHED BY SOURCE AND TARGET.CourseId = @Id
    THEN DELETE;     
 END      
END      
      
ELSE      
 BEGIN      
  SET @return= 102 -- Record does not exists      
 END      
      
exec GetCourseById  @Id      
RETURN @return      
END;      

GO
/****** Object:  StoredProcedure [dbo].[UpdateCourseComment]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
CREATE PROCEDURE [dbo].[UpdateCourseComment]      
   @Id Numeric(18,0),   
     @CourseId Numeric(18,0),  
     @Comments nvarchar(2000),  
     @CommentedBy int      
                 
AS  
BEGIN  
  
Declare @return INT  
  
IF EXISTS (SELECT * FROM CourseComments WHERE Id=@Id and CourseId=@CourseId and UserId=@CommentedBy)  
BEGIN   
UPDATE CourseComments SET Comments=@Comments, UpdateDate = GETDATE() WHERE Id=@Id and CourseId=@CourseId and UserId=@CommentedBy  
  -- do log entry here  
     
    IF @@ERROR <> 0  
    
    SET @return= 106 -- update failed   
  
  
 ELSE  
  
  SET @return= 101 -- update success   
  
  
END  
else  
 BEGIN  
  SET @return= 102 -- Record does not exists  
 END  
return @return  
END  
  
GO
/****** Object:  StoredProcedure [dbo].[UpdateEducation]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE  PROCEDURE [dbo].[UpdateEducation] 
	@Id INT,
	@Name NVARCHAR(150),
	@UpdatedBy INT,
	@Active BIT,
	@Name_Ar NVARCHAR(MAX)
AS
BEGIN
Declare @return INT
IF NOT EXISTS (SELECT * FROM EducationMaster WHERE Name=@Name AND Id<> @Id)
BEGIN	
		UPDATE EducationMaster  SET Name=@Name,
									  UpdatedBy=@UpdatedBy,
									  UpdatedOn=GETDATE(), Active=@Active,
									  Name_Ar = @Name_Ar
									  WHERE Id=@Id
		-- do log entry here	

	IF @@ERROR <> 0
		
		  SET @return= 106 -- update failed	
	ELSE
		SET @return= 101 -- update success	
END

ELSE
	BEGIN
		SET @return= 102 -- Record does not exists
	END

	SELECT	em.Id,
				em.Name,
				em.CreatedOn,				
				c.FirstName,
				 CONCAT(c.FirstName, '', c.LastName) as CreatedBy,
				CONCAT(l.FirstName, '', l.LastName) as UpdatedBy,
				em.UpdatedOn, em.Active,
				em.Name_Ar
		 from EducationMaster em 
		 inner join UserMaster c  on em.CreatedBy= c.Id
		 inner join UserMaster l on em.UpdatedBy =l.Id where em.Status=1 Order by em.Id desc 

	RETURN @return
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateInstitution]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UpdateInstitution] 
	@Id INT,
	@Name NVARCHAR(150),
	@UpdatedBy INT,
	@Active BIT
AS
BEGIN
Declare @return INT
IF NOT EXISTS (SELECT * FROM InstitutionMaster WHERE Name=@Name AND Id<> @Id)
BEGIN	
		UPDATE InstitutionMaster  SET Name=@Name,
									  UpdatedBy=@UpdatedBy,
									   UpdatedOn=GETDATE(),Active=@Active
									  WHERE Id=@Id
		-- do log entry here	

	IF @@ERROR <> 0
		
		  SET @return= 106 -- update failed	
	ELSE
		SET @return= 101 -- update success	
END

ELSE
	BEGIN
		SET @return= 102 -- Record does not exists
	END

	SELECT	em.Id,
				em.Name,
				em.CreatedOn,				
				c.FirstName,
				 CONCAT(c.FirstName, '', c.LastName) as CreatedBy,
				CONCAT(l.FirstName, '', l.LastName) as UpdatedBy,
				em.UpdatedOn, em.Active
		 from InstitutionMaster em 
		 inner join UserMaster c  on em.CreatedBy= c.Id
		 inner join UserMaster l on em.UpdatedBy =l.Id Order by em.Id desc 

	RETURN @return
END

GO
/****** Object:  StoredProcedure [dbo].[UpdateMaterialType]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UpdateMaterialType] 
	@Id INT,
	@Name NVARCHAR(100),
	@Name_Ar NVARCHAR(100),
	@UpdatedBy INT,
	@Active BIT
AS
BEGIN

Declare @return INT

IF NOT EXISTS (SELECT * FROM MaterialTypeMaster WHERE Name=@Name and Status=1 AND Id<> @Id)
BEGIN	
		UPDATE MaterialTypeMaster  SET Name=@Name,
		Name_Ar=@Name_Ar,
									  UpdatedBy=@UpdatedBy,
									   UpdatedOn=GETDATE(),Active=@Active
									  WHERE Id=@Id
		-- do log entry here
		 
		  IF @@ERROR <> 0
		
		  SET @return= 106 -- update failed	
	ELSE
		SET @return= 101 -- update success	
END

ELSE
	BEGIN
		SET @return= 102 -- Record does not exists
	END

	SELECT	cm.Id,
				cm.Name,
				cm.Name_Ar,
				cm.CreatedOn,				
			 CONCAT(c.FirstName, '', c.LastName) as CreatedBy,
				CONCAT(l.FirstName, '', l.LastName) as UpdatedBy,
				cm.UpdatedOn, cm.Active
		 from MaterialTypeMaster cm 
		 inner join UserMaster c  on cm.CreatedBy= c.Id
		 inner join UserMaster l on cm.UpdatedBy =l.Id where cm.Status=1 order by cm.Id desc 	

		 RETURN @return
END

GO
/****** Object:  StoredProcedure [dbo].[UpdateProfession]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UpdateProfession] 
	@Id INT,
	@Name NVARCHAR(200),
	@UpdatedBy INT,
	@Active BIT,
	@Name_Ar NVARCHAR(200)
AS
BEGIN

Declare @return INT

IF NOT EXISTS (SELECT * FROM ProfessionMaster WHERE Name=@Name and Status=1 AND Id<> @Id)
BEGIN	
		UPDATE ProfessionMaster  SET Name=@Name,
									  UpdatedBy=@UpdatedBy,
									  UpdatedOn=GETDATE(), Active=@Active,
									  Name_Ar = @Name_Ar
									  WHERE Id=@Id
		-- do log entry here
		 
		  IF @@ERROR <> 0
		
		  SET @return= 106 -- update failed	
	ELSE
		SET @return= 101 -- update success	
END

ELSE
	BEGIN
		SET @return= 102 -- Record does not exists
	END

	SELECT	cm.Id,
				cm.Name,
				cm.Name_Ar,
				cm.CreatedOn,				
			 CONCAT(c.FirstName, '', c.LastName) as CreatedBy,
				CONCAT(l.FirstName, '', l.LastName) as UpdatedBy,
				cm.UpdatedOn, cm.Active
		 from ProfessionMaster cm 
		 inner join UserMaster c  on cm.CreatedBy= c.Id
		 inner join UserMaster l on cm.UpdatedBy =l.Id where cm.Status=1 order by cm.Id desc 	

		 RETURN @return
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateQRC]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UpdateQRC] 
	@Id INT,
	@Name NVARCHAR(150),
	@Description NVARCHAR(200),
	@UpdatedBy INT,
	@Active BIT,
	@CategoryIds NVARCHAR(450)
AS
BEGIN

Declare @return INT
CREATE TABLE #temp
(
Name nvarchar(50),
Description nvarchar(1000),
CreatedBy int,
CatId int
)

INSERT INTO #temp (
Name,
Description,
CreatedBy,
CatId
)
SELECT 
@Name,
@Description,
@UpdatedBy,
value  
FROM StringSplit(@CategoryIds, ',')  


IF EXISTS (SELECT * FROM QRCMaster WHERE Id = @Id)
BEGIN	
		UPDATE QRCMaster  SET Name=@Name,
									  UpdatedBy=@UpdatedBy,
									  Description=@Description,
									  Active=@Active, UpdatedOn=GETDATE()
									  WHERE Id=@Id
		-- do log entry here
		 

		 DELETE FROM QRCCategory WHERE QRCId =  @Id

		 INSERT INTO QRCCategory(QRCId,CategoryId)
		 SELECT @Id,CatId FROM #temp;


		  IF @@ERROR <> 0
		
		  SET @return= 106 -- update failed	
	ELSE
		SET @return= 101 -- update success	
END

ELSE
	BEGIN
		SET @return= 102 -- Record does not exists
	END

	SELECT	cm.Id,
				cm.Name,Description,
				cm.CreatedOn,				
			 CONCAT(c.FirstName, '', c.LastName) as CreatedBy,
				CONCAT(l.FirstName, '', l.LastName) as UpdatedBy,
				cm.UpdatedOn, cm.Active
		 from QRCMaster cm 
		 inner join UserMaster c  on cm.CreatedBy= c.Id
		 inner join UserMaster l on cm.UpdatedBy =l.Id order by cm.Id desc 	

		 RETURN @return
END


GO
/****** Object:  StoredProcedure [dbo].[UpdateResource]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateResource]      
     @Title  NVARCHAR(250),  
           @CategoryId INT,  
           @SubCategoryId INT= NULL,  
           @Thumbnail  NVARCHAR(400) = NULL,  
           @ResourceDescription  NVARCHAR(2000) = NULL,  
           @Keywords  NVARCHAR(1500) = NULL,  
           @ResourceContent ntext = NULL,  
           @MaterialTypeId INT= NULL,  
           @CopyRightId INT = NULL,  
           @IsDraft BIT = 0,  
     @References NVARCHAR(MAX)=null,    
     @ResourceFiles NVARCHAR(MAX)=null,    
     @Id INT,  
     @ReadingTime INT = NULL,  
     @LevelId INT = NULL,  
     @EducationalStandardId INT = NULL,  
     @EducationalUseId INT = NULL,  
     @Format NVARCHAR(100) = NULL,  
     @Objective NVARCHAR(4000) = NULL  
AS  
BEGIN  
  
Declare @return INT  
  
IF EXISTS (SELECT * FROM ResourceMaster WHERE @Id=@Id)  
BEGIN   
UPDATE ResourceMaster SET Title=@Title,  
           CategoryId=@CategoryId,  
           SubCategoryId=@SubCategoryId,  
           Thumbnail=@Thumbnail,  
           ResourceDescription=@ResourceDescription,  
           Keywords=@Keywords,  
           ResourceContent=@ResourceContent,  
           MaterialTypeId=@MaterialTypeId,  
           CopyRightId=@CopyRightId,  
           IsDraft=@IsDraft,  
           ReadingTime = @ReadingTime,  
           LevelId = @LevelId,  
           EducationalStandardId = @EducationalStandardId,  
           EducationalUseId = @EducationalUseId,  
           [Format] = @Format,  
           Objective = @Objective  ,
		   IsApproved = NULL
           WHERE Id=@Id  
  -- do log entry here  
     
    IF @@ERROR <> 0  
    BEGIN
    SET @return= 106 -- update failed   
  

END  
 ELSE  
  BEGIN
  SET @return= 101 -- update success  
IF(@IsDraft = 0)
BEGIN 
  DECLARE @TotalCount INT;      
DECLARE @QrcID INT;      
DECLARE @RecordId INT;      
DECLARE @SectionId INT;      
DECLARE @i INT; 
DECLARE @CreatedBy INT     
SET @i = 1;      
SELECT @CreatedBy = CreatedBy FROM ResourceMaster WHERE Id = @Id
    
      
select @TotalCount = count(*) from QRCCategory WHERE CategoryID = @CategoryID AND IsAvailable =0 --order by CreatedOn asc      
      
IF(@TotalCount>0)      
BEGIN      
   select TOP 1 @RecordId=ID,@QrcID=QRCId from QRCCategory WHERE CategoryID = @CategoryID AND IsAvailable =0 
   AND QRCID IN (SELECT DISTINCT QRCID FROM QRCUserMapping
   EXCEPT (SELECT DISTINCT QRCID FROM QRCUserMapping WHERE UserId = @CreatedBy AND Active = 1))
   order by CreatedOn asc      
      
   Update QRCCategory SET IsAvailable = 1 WHERE ID = @RecordId      
      
   IF EXISTS(      
   SELECT       
     TOP 1 1      
     from QRCusermapping  where QRCID =@QrcID and active = 1 and CategoryId = @CategoryId)      
     BEGIN   
		 IF NOT EXISTS(SELECT TOP 1 1 FROM ContentApproval WHERE ContentId = @Id AND ContentType = 2)
		 BEGIN   
			   INSERT INTO ContentApproval(ContentId,      
			   ContentType,      
			   CreatedBy,      
			   CreatedOn,      
			   AssignedTo,      
			   AssignedDate,
			   QrcId)      
      
			   SELECT       
			   @Id,      
			   2, -- Resource      
			   @CreatedBy,      
			   GETDATE(),      
			   userid,      
			   GETDATE() ,
			   @QrcID
			   from QRCusermapping  where QRCID =@QrcID and active = 1 
			   and QRCusermapping.CategoryId = @CategoryId  
		 END
		 ELSE
		 BEGIN
			UPDATE ContentApproval
			SET [Status] = NULL,
			ApprovedBy = NULL,
			ApprovedDate = NULL,
			UpdatedOn = NULL,
			UpdatedBy = NULL,
			Comment = NULL
			WHERE Id = (SELECT TOP(1) Id FROM ContentApproval WHERE ContentId = @Id AND ContentType = 2)
		 END
	   
	   UPDATE ResourceMaster SET IsDraft = 0 WHERE Id = @Id     
      END   
   
END      
IF(@TotalCount=1)        
BEGIN        
Update QRCCategory SET IsAvailable = 0  WHERE CategoryID = @CategoryID         
END 
 END
  END
 --IF NOT EXISTS(SELECT ContentId FROm ContentApproval WHERE ContentId = @id and ContentType = 2)
 --BEGIN
	--UPDATE ResourceMaster SET IsDraft = 1 WHERE Id = @Id
 --END 
IF @ResourceFiles IS NOT NULL  
 BEGIN  
 -- Update Resource Associated Files FROM JSON  
   
MERGE ResourceAssociatedFiles AS TARGET  
  USING  
    (  
    SELECT DISTINCT AssociatedFile,FileName
      FROM  
      OPENJSON(@ResourceFiles)  
      WITH (AssociatedFile nvarchar(200) '$.AssociatedFile',FileName nvarchar(200) '$.FileName')  
    ) AS source (AssociatedFile,FileName)  
  ON TARGET .AssociatedFile = source.AssociatedFile AND TARGET.FileName = source.FileName
  WHEN NOT MATCHED   
  THEN   
    INSERT (ResourceId,AssociatedFile,FileName, UploadedDate)  
      VALUES (@Id,source.AssociatedFile,source.FileName,GETDATE())
	WHEN NOT MATCHED BY SOURCE AND TARGET.ResourceId = @Id
    THEN DELETE;  
  
 END  
  
 IF @References IS NOT NULL  
 BEGIN  
MERGE ResourceURLReferences AS TARGET  
  USING  
    (  
    SELECT DISTINCT URLReferenceId  
      FROM  
      OPENJSON(@References)  
      WITH (URLReferenceId Int '$.URLReferenceId')  
    ) AS source (URLReferenceId)  
  ON TARGET.URLReferenceId = source.URLReferenceId
  AND TARGET.ResourceId = @Id
  WHEN NOT MATCHED   
  THEN   
    INSERT (ResourceId,URLReferenceId, CreatedOn)  
      VALUES (@Id,source.URLReferenceId,GETDATE())
  WHEN NOT MATCHED BY SOURCE AND TARGET.ResourceId = @Id
    THEN DELETE;
 END  
END  
  
ELSE  
 BEGIN  
  SET @return= 102 -- Record does not exists  
 END  
  
exec GetResourceById  @Id  
RETURN @return  
END  

GO
/****** Object:  StoredProcedure [dbo].[UpdateResourceComment]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
CREATE PROCEDURE [dbo].[UpdateResourceComment]      
     @Id Numeric(18,0),   
     @ResourceId Numeric(18,0),  
     @Comments nvarchar(2000),  
     @CommentedBy int      
                 
AS  
BEGIN  
  
Declare @return INT  
  
IF EXISTS (SELECT * FROM ResourceComments WHERE Id=@Id and ResourceId=@ResourceId and UserId=@CommentedBy)  
BEGIN   
UPDATE ResourceComments SET Comments=@Comments,UpdateDate = GETDATE() WHERE Id=@Id and ResourceId=@ResourceId and UserId=@CommentedBy  
  -- do log entry here  
     
    IF @@ERROR <> 0  
    
    SET @return= 106 -- update failed   
  
  
 ELSE  
  
  SET @return= 101 -- update success   
  
  
END  
else  
 BEGIN  
  SET @return= 102 -- Record does not exists  
 END  
return @return  
END  
  
GO
/****** Object:  StoredProcedure [dbo].[UpdateRole]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[UpdateRole]		  
		   @UserID INT,
		   @RoleType INT,
		   @RoleValue Bit            
AS
BEGIN

Declare @return INT

IF EXISTS (SELECT * FROM UserMaster WHERE Id=@UserID)
BEGIN	

IF @RoleType=1

UPDATE UserMaster SET IsAdmin=@RoleValue WHERE Id=@UserID
		
IF @RoleType =2 	

UPDATE UserMaster SET IsContributor=@RoleValue WHERE Id=@UserID 
		
		  IF @@ERROR <> 0
		
		  SET @return= 106 -- update failed	

	ELSE
		SET @return= 101 -- update success	

RETURN @return
END

END
GO
/****** Object:  StoredProcedure [dbo].[UpdateStream]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UpdateStream] 
	@Id INT,
	@Name NVARCHAR(100),
	@Name_Ar NVARCHAR(200),
	@UpdatedBy INT,
	@Active BIT
AS
BEGIN

Declare @return INT

IF EXISTS (SELECT TOP 1 1  FROM StreamMaster WHERE  Id = @Id)
BEGIN	
		UPDATE StreamMaster  SET Name=@Name,
		Name_Ar=@Name_Ar,
									  UpdatedBy=@UpdatedBy,
									  Active=@Active,
									  UpdatedOn=GETDATE()
									  WHERE Id=@Id
		-- do log entry here
		 
		  IF @@ERROR <> 0
		
		  SET @return= 106 -- update failed	
	ELSE
		SET @return= 101 -- update success	
END

ELSE
	BEGIN
		SET @return= 102 -- Record does not exists
	END

	SELECT	cm.Id,
				cm.Name,
				Name_Ar,
				cm.CreatedOn,				
			 CONCAT(c.FirstName, '', c.LastName) as CreatedBy,
				CONCAT(l.FirstName, '', l.LastName) as UpdatedBy,
				cm.UpdatedOn, cm.Active
		 from StreamMaster cm 
		 inner join UserMaster c  on cm.CreatedBy= c.Id
		 inner join UserMaster l on cm.UpdatedBy =l.Id order by cm.Id desc 	

		 RETURN @return
END

GO
/****** Object:  StoredProcedure [dbo].[UpdateSubCategory]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UpdateSubCategory] 
	@Id INT,
	@CategoryId INT,
	@Name NVARCHAR(255),
	@Name_Ar NVARCHAR(255),
	@UpdatedBy INT,
	@Active INT
AS
BEGIN

Declare @return INT

IF NOT EXISTS (SELECT * FROM SubCategoryMaster WHERE Name=@Name and Status=1 AND Id<> @Id)
BEGIN	
		UPDATE SubCategoryMaster  SET Name=@Name,
									 Name_Ar=@Name_Ar,
									  CategoryId=@CategoryId,
									  UpdatedBy=@UpdatedBy,
									  UpdatedOn=GETDATE(),
									  Active=@Active
									  WHERE Id=@Id
		-- do log entry here
		 
		  IF @@ERROR <> 0
		
		  SET @return= 106 -- update failed	
	ELSE
		SET @return= 101 -- update success	
END

ELSE
	BEGIN
		SET @return= 102 -- Record does not exists
	END

	SELECT	sm.Id,
				sm.Name,
				sm.Name_Ar,
				cm.Name as CategoryName, 
				cm.Id as CategoryId,
				sm.CreatedOn,				
			    CONCAT(c.FirstName, '', c.LastName) as CreatedBy,
				CONCAT(l.FirstName, '', l.LastName) as UpdatedBy,
				sm.UpdatedOn, 
				sm.Active
		 from SubCategoryMaster sm 
		 INNER JOIN CategoryMaster cm on sm.CategoryId=cm.Id 
		 inner join UserMaster c  on sm.CreatedBy= c.Id
		 inner join UserMaster l on sm.UpdatedBy =l.Id where sm.Status=1 order by cm.Id desc 	

		 RETURN @return
END

GO
/****** Object:  StoredProcedure [dbo].[UpdateUserProfile]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateUserProfile]     
  (  @TitleId int=NULL,    
           @FirstName nvarchar(250),    
           @MiddleName nvarchar(250)=NULL,    
           @LastName nvarchar(250)=NULL,    
           @CountryId int=NULL,    
           @StateId int=NULL,    
           @Gender int,    
           @Email nvarchar(250),    
           @PortalLanguageId int=NULL,    
          --  @DepartmentId int=NULL,    
         --   @DesignationId int=NULL,    
           @DateOfBirth date,    
           @Photo nvarchar(200)=NULL,    
           @ProfileDescription nvarchar(4000)=NULL,    
           @SubjectsInterested nvarchar(MAX)=NULL,                   
           @IsContributor bit,    
     @UserCertifications nvarchar(max)=NULL,    
     @UserEducations nvarchar(max)=NULL,    
     @UserExperiences nvarchar(max)=NULL,    
     @UserLanguages nvarchar(max)=NULL,    
     @UserSocialMedias nvarchar(max)=NULL,    
     @Id INT  ,  
  @EmailUrl NVARCHAR(400) = NULL  
     )    
AS    
BEGIN    
Declare @return INT    
  DECLARE @MessageTypeId INT    
Declare @Date Datetime   
DECLARE @AlreadyContributer BIT = 0  
  
SELECT @AlreadyContributer = IsContributor FROm UserMaster WHERE Id= @Id   
    
SET @Date = getdate()   
IF EXISTS (SELECT * FROM UserMaster WHERE ID=@Id)    
BEGIN    
    
    
     UPDATE UserMaster SET     
     TitleId =@TitleId    
           ,FirstName=@FirstName    
           ,MiddleName=@MiddleName    
           ,LastName=@LastName    
           ,CountryId=@CountryId    
           ,StateId=@StateId    
           ,Gender=@Gender    
           ,Email=@Email    
           ,PortalLanguageId=@PortalLanguageId    
          --  ,DepartmentId    
           -- ,DesignationId    
           ,DateOfBirth=@DateOfBirth    
           ,Photo=@Photo    
           ,ProfileDescription=@ProfileDescription    
           ,SubjectsInterested=@SubjectsInterested,    
      IsContributor=@IsContributor    
           ,UpdatedOn=GETDATE()    
WHERE ID = @Id    
    
    
       
IF(@IsContributor = 1)    
  BEGIN    
  SELECT @MessageTypeId = ID FROM MessageType WHERE [Type] = 'Contributor Access Approved'    
  END    
  ELSE    
    
  BEGIN    
  SELECT @MessageTypeId = ID FROM MessageType WHERE [Type] = 'Contributor Access Rejected'    
  END;    
  IF @AlreadyContributer = 0  
     exec spi_Notifications @Id,3,'User Contributor','User Contributor Access Status',@MessageTypeId,@IsContributor,@Date,0,0,@Id,NULL,NULL,@EmailUrl    
  
    
    
IF @UserCertifications IS NOT NULL    
 BEGIN    
 -- INSERT USER CERTIFICATION FROM JSON    
 DELETE FROM UserCertification WHERE userid = @ID     
INSERT INTO UserCertification    
        
SELECT @Id,CertificationName,Year,GETDATE() FROM      
 OPENJSON ( @UserCertifications )      
WITH (       
              CertificationName   nvarchar(250) '$.CertificationName',                  
              [Year] int '$.Year'      
 )     
    
 END    
    
 IF @UserEducations IS NOT NULL    
 BEGIN    
  -- INSERT USER EDUCATION FROM JSON    
  DELETE FROM UserEducation WHERE userid = @ID     
 INSERT INTO UserEducation    
        
SELECT @Id,UniversitySchool,Major,Grade,FromDate,ToDate,GETDATE() FROM      
 OPENJSON ( @UserEducations )      
WITH (       
              UniversitySchool   nvarchar(250) '$.UniversitySchool',                  
              Major nvarchar(100)          '$.Major',      
     Grade nvarchar(10)          '$.Grade',     
     FromDate varchar(100)       '$.FromDate',    
     ToDate varchar(100)         '$.ToDate'    
 )     
    
 END    
    
  IF @UserExperiences IS NOT NULL    
 BEGIN    
 -- INSERT USER EXPERIENCE FROM JSON    
    
   DELETE FROM UserExperiences WHERE userid = @ID     
 INSERT INTO UserExperiences    
        
SELECT @Id,OrganizationName,Designation,FromDate,ToDate,GETDATE() FROM      
 OPENJSON ( @UserExperiences )      
WITH (       
     OrganizationName   nvarchar(250) '$.OrganizationName',                  
              Designation nvarchar(250)          '$.Designation',    
     FromDate DATE      '$.FromDate',    
     ToDate DATE         '$.ToDate'    
 )     
    
 END    
    
IF @UserLanguages IS NOT NULL    
 BEGIN    
 -- INSERT INTO LANGUAGE MASTER FIRST    
    
    
DROP TABLE IF EXISTS #tempUserLangs    
    
create table #tempUserLangs    
(    
 Language nvarchar(250),    
 LanguageId int,    
 IsRead bit,     
 IsWrite bit,    
 IsSpeak bit    
)    
    
     
INSERT INTO #tempUserLangs    
        
SELECT Language,0,IsRead,IsWrite,IsSpeak FROM      
 OPENJSON ( @UserLanguages )      
WITH (       
     Language   nvarchar(250) '$.Language',                  
              IsRead bit  '$.IsRead',    
     IsWrite bit  '$.IsWrite',    
     IsSpeak bit  '$.IsSpeak'    
 )     
    
DECLARE @source NVARCHAR(max)    
 MERGE LanguageMaster AS TARGET    
  USING    
    (    
    SELECT DISTINCT Language    
      FROM    
      OPENJSON(@UserLanguages)    
      WITH (Language nvarchar(100) '$.Language')    
    ) AS source (Language)    
  ON TARGET.Name = source.Language    
  WHEN NOT MATCHED     
  THEN     
    INSERT (Name,createdBy, UpdatedBy)    
      VALUES (source.Language,@Id,@Id);    
    
UPDATE #tempUserLangs     
SET LanguageId=m.id FROM     
   languageMaster m inner JOIN #tempUserLangs     
   t ON m.Name= t.language     
    
      DELETE FROM UserLanguages WHERE userid = @ID     
INSERT INTO UserLanguages(UserId,    
      LanguageId,    
      IsRead,    
      IsSpeak,     
      IsWrite, CreatedOn)     
SELECT  @Id,    
  LanguageId,    
  IsRead,    
  IsSpeak,    
  IsWrite, GETDATE() FROM  #tempUserLangs    
    
END    
    
IF @UserSocialMedias IS NOT NULL    
 BEGIN    
    
     
      DELETE FROM UserSocialMedia WHERE userid = @ID     
 INSERT INTO UserSocialMedia    
        
SELECT @Id,SocialMediaId,URL,GETDATE() FROM      
 OPENJSON ( @UserSocialMedias )      
WITH (       
              SocialMediaId   INT '$.SocialMediaId',                  
              URL varchar(100) '$.URL'    
 )     
    
END    
    
    
    
END    
ELSE     
    
 SET @return=107    
    
 END    
 SET @return = 101;    
select Id,IsEmailNotification,IsContributor,@AlreadyContributer As IsAlreadyContributer,PortalLanguageId FROM UserMaster WHERE ID = @Id    
 RETURN @return;   
GO
/****** Object:  StoredProcedure [dbo].[UpdateUserRole]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE  PROCEDURE [dbo].[UpdateUserRole]          
     @UserID INT,      
     @IsContributor Bit = NULL,      
     @IsAdmin Bit = NULL,    
  @portalLanguageId INT = NULL                
AS      
BEGIN      
      
Declare @return INT   
DECLARE @MessageTypeId INT  
Declare @Date Datetime  
  
SET @Date = getdate()  
      
IF EXISTS (SELECT * FROM UserMaster WHERE Id=@UserID)      
BEGIN       
      
      
      
UPDATE UserMaster SET IsAdmin = (CASE WHEN @IsAdmin IS NULL THEN IsAdmin ELSE @IsAdmin END ),  
IsContributor =(CASE WHEN @IsContributor IS NULL THEN IsContributor ELSE @IsContributor END ),  
PortalLanguageId=(CASE WHEN @PortalLanguageId IS NULL THEN PortalLanguageId ELSE @PortalLanguageId END ) WHERE Id=@UserID      
    
  IF(@IsContributor = 1)  
  BEGIN  
  SELECT @MessageTypeId = ID FROM MessageType WHERE [Type] = 'Contributor Access Approved'  
  END  
  ELSE  
  
  BEGIN  
  SELECT @MessageTypeId = ID FROM MessageType WHERE [Type] = 'Contributor Access Rejected'  
  END;  
  
  --3 means User Contributor as Type  
  IF(@IsContributor IS NOT NULL)  
   exec spi_Notifications @UserID,3,'User Contributor','User Contributor Access Status',@MessageTypeId,@IsContributor,@Date,0,0,@UserID  
   
 SET @return= 101 -- update success      
      
      
      
END      
else set @return= 102      
      
return @return      
END      
    
GO
/****** Object:  StoredProcedure [dbo].[UrlIsWhitelisted]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UrlIsWhitelisted] 
@URL nvarchar(2000)
AS
BEGIN		
		
	SELECT wlu.Id,    
	   CONCAT(c.FirstName, '', c.LastName) as RequestedBy,
	   CONCAT(l.FirstName, '', l.LastName) as VerifiedBy,
	   VerifiedBy AS [VerifiedById]
      ,URL
      ,IsApproved
      ,RequestedOn
      ,VerifiedOn
      ,RejectedReason
  FROM WhiteListingURLs wlu 
   inner join UserMaster c  on wlu.RequestedBy= c.Id
		 left join UserMaster l on wlu.VerifiedBy =l.Id  where IsApproved=1

  IF @@ROWCOUNT >0
		return 105 -- record exists
		
		ELSE
		return 102 -- record does not exists
END


GO
/****** Object:  StoredProcedure [dbo].[USP_Insert_QRCUsers]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
  
  
  
CREATE PROCEDURE [dbo].[USP_Insert_QRCUsers](  
@QRCUserDetails [UT_QRCUsers] Readonly  
  
)    
AS    
BEGIN    
Declare @return INT  
INSERT INTO QRCUserMapping    
(    
[QRCId]  
           ,[CategoryId]  
           ,[UserId]  
     ,[CreatedBy]  
     ,[CreatedOn]  
     ,[UpdatedOn]  
     ,[Active]  
)    
SELECT   
t.[QRCId]  
           ,t.[CategoryId]  
           ,t.[UserId]  
     ,t.[CreatedBy]  
     ,GETDATE()  
     ,GETDATE()  
     ,1  
     FROM @QRCUserDetails t LEFT JOIN QRCUserMapping u  
     ON  t.[QRCId]= u.[QRCId] AND t.[CategoryId]= u.[CategoryId] AND t.[UserId]= u.[UserId] and u.Active = 1  
WHERE u.QRCId is null AND u.CategoryId  is null and u.UserId is null  
  
SELECT ID, Email,FirstName, LastName,PortalLanguageId
FROM UserMaster  
 WHERE ID IN (SELECT UserId FROM @QRCUserDetails) AND IsEmailNotification = 1  
  
 SET @return = 100 -- reconrd does not exists  
   RETURN @return  
END   
  
GO
/****** Object:  StoredProcedure [dbo].[VerifyWhiteListingRequest]    Script Date: 09-01-2020 11:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[VerifyWhiteListingRequest]     
 @Id DECIMAL,     
 @VerifiedBy INT,    
 @IsApproved BIT,    
 @RejectedReason NVARCHAR(100)=NULL,  
 @EmailUrl NVARCHAR(400) = NULL  
AS    
BEGIN    
    
Declare @return INT    
DECLARE @MessageTypeId INT  
DECLARE @UserID INT  
Declare @Date Datetime  
SET @Date = getdate()  
  
SELECT @UserID= RequestedBy FROM WhiteListingURLs WHERE Id=@Id  
  
IF EXISTS (SELECT TOP 1 1 FROM WhiteListingURLs WHERE Id=@Id)    
BEGIN     
  Update WhiteListingURLs    
      
  set VerifiedBy=@VerifiedBy,    
   IsApproved=@IsApproved,    
   RejectedReason=@RejectedReason where Id=@Id    
    
  -- do log entry here    
  IF(@IsApproved = 1)  
  BEGIN  
  SELECT @MessageTypeId = ID FROM MessageType WHERE [Type] = 'URL Whitelist Approval'  
  END;  
  ELSE   
  BEGIN  
   SELECT @MessageTypeId = ID FROM MessageType WHERE [Type] = 'URL Whitelist Rejection'  
  END  
      exec spi_Notifications @Id,4,'URL Whitelist','URL Whitelist',@MessageTypeId,@IsApproved,@Date,0,0,@UserID,@VerifiedBy,@RejectedReason,@EmailUrl  
    IF @@ERROR <> 0    
      
    SET @return= 106 -- update failed     
 ELSE    
    
 SELECT Id,    
TitleId,    
FirstName +' ' +LastName as UserName,    
Email,PortalLanguageId FROM UserMaster WHERE ID in (SELECT RequestedBy FROM WhiteListingURLs WHERE Id=@Id)    
  SET @return= 101 -- update success     
END    
    
ELSE    
 BEGIN    
  SET @return= 102 -- Record does not exists    
 END     
    
 RETURN @return    
    
END    
  
GO
USE [master]
GO
ALTER DATABASE oerdevdb SET  READ_WRITE 
GO
