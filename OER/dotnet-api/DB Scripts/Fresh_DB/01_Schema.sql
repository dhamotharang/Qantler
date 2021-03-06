/****** Object:  UserDefinedTableType [dbo].[UT_AnswerOptions]    Script Date: 12/17/2019 12:42:58 PM ******/
CREATE TYPE [UT_AnswerOptions] AS TABLE(
	[QuestionId] [int] NOT NULL,
	[AnswerId] [int] NOT NULL,
	[OptionText] [nvarchar](max) NULL,
	[CorrectOption] [bit] NULL
)
GO
/****** Object:  UserDefinedTableType [UT_QRCUsers]    Script Date: 12/17/2019 12:42:58 PM ******/
CREATE TYPE [UT_QRCUsers] AS TABLE(
	[QRCId] [int] NULL,
	[CategoryId] [int] NULL,
	[UserId] [int] NULL,
	[CreatedBy] [int] NULL
)
GO
/****** Object:  UserDefinedTableType [UT_Ques]    Script Date: 12/17/2019 12:42:58 PM ******/
CREATE TYPE [UT_Ques] AS TABLE(
	[QuestionId] [int] NOT NULL,
	[QuestionText] [nvarchar](max) NULL,
	[Nedia] [varchar](400) NULL,
	[FileName] [varchar](400) NULL
)
GO
/****** Object:  UserDefinedTableType [UT_Question]    Script Date: 12/17/2019 12:42:58 PM ******/
CREATE TYPE [UT_Question] AS TABLE(
	[QuestionId] [int] NOT NULL,
	[QuestionText] [nvarchar](max) NULL,
	[Media] [varchar](400) NULL,
	[FileName] [varchar](400) NULL
)
GO
/****** Object:  UserDefinedTableType [UT_Questions]    Script Date: 12/17/2019 12:42:58 PM ******/
CREATE TYPE [UT_Questions] AS TABLE(
	[QuestionId] [int] NOT NULL,
	[QuestionText] [nvarchar](max) NULL,
	[Media] [nvarchar](200) NULL
)
GO
/****** Object:  UserDefinedTableType [UT_QuestionsForContent]    Script Date: 12/17/2019 12:42:58 PM ******/
CREATE TYPE [UT_QuestionsForContent] AS TABLE(
	[QuestionId] [int] NOT NULL,
	[QuestionText] [nvarchar](max) NULL,
	[Media] [nvarchar](400) NULL,
	[FileName] [nvarchar](400) NULL
)
GO
/****** Object:  UserDefinedTableType [UT_Resource]    Script Date: 12/17/2019 12:42:58 PM ******/
CREATE TYPE [UT_Resource] AS TABLE(
	[Id] [int] NOT NULL,
	[ResourceId] [int] NOT NULL,
	[SectionId] [int] NOT NULL
)
GO
/****** Object:  UserDefinedTableType [UT_Sections]    Script Date: 12/17/2019 12:42:58 PM ******/
CREATE TYPE [UT_Sections] AS TABLE(
	[Id] [int] NOT NULL,
	[Name] [nvarchar](100) NULL
)
GO
/****** Object:  UserDefinedFunction [StringSplit]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [StringSplit]
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
/****** Object:  Table [UserMaster]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [UserMaster](
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
/****** Object:  Table [CategoryMaster]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [CategoryMaster](
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
/****** Object:  Table [CourseMaster]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [CourseMaster](
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
/****** Object:  Table [CourseRating]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [CourseRating](
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
/****** Object:  Table [CopyrightMaster]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [CopyrightMaster](
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
/****** Object:  Table [CourseEnrollment]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [CourseEnrollment](
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
/****** Object:  View [VCourseMaster]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE  VIEW [VCourseMaster]  
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
/****** Object:  Table [ResourceRating]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ResourceRating](
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
/****** Object:  Table [ResourceMaster]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ResourceMaster](
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
/****** Object:  View [VResourceMaster]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [VResourceMaster]  
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
/****** Object:  Table [Announcements]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Announcements](
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
/****** Object:  Table [AnswerOptions]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [AnswerOptions](
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
/****** Object:  Table [CommunityApproveRejectCount]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [CommunityApproveRejectCount](
	[ApproveCount] [int] NULL,
	[RejectCount] [int] NULL,
	[LastUpdatedBy] [int] NULL,
	[LastUpdatedOn] [datetime] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [CommunityCheckMaster]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [CommunityCheckMaster](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[ContentId] [int] NULL,
	[ContentType] [int] NULL,
	[Status] [bit] NULL,
	[Comments] [nvarchar](max) NULL,
	[IsCurrent] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [ContactUs]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ContactUs](
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
/****** Object:  Table [ContentApproval]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ContentApproval](
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
/****** Object:  Table [ContentDownloadInfo]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ContentDownloadInfo](
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
/****** Object:  Table [ContentSharedInfo]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ContentSharedInfo](
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
/****** Object:  Table [CountryMaster]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [CountryMaster](
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
/****** Object:  Table [CourseAbuseReports]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [CourseAbuseReports](
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
/****** Object:  Table [CourseApprovals]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [CourseApprovals](
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
/****** Object:  Table [CourseAssociatedFiles]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [CourseAssociatedFiles](
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
/****** Object:  Table [CourseCommentAbuseReports]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [CourseCommentAbuseReports](
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
/****** Object:  Table [CourseComments]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [CourseComments](
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
/****** Object:  Table [CourseResourceMapping]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [CourseResourceMapping](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CourseId] [numeric](18, 0) NULL,
	[ResourcesId] [numeric](18, 0) NULL,
 CONSTRAINT [PK_CourseResourceMapping] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [CourseSections]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [CourseSections](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NULL,
	[CourseId] [numeric](18, 0) NULL,
 CONSTRAINT [PK_CourseSections] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [CourseURLReferences]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [CourseURLReferences](
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
/****** Object:  Table [DepartmentMaster]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [DepartmentMaster](
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
/****** Object:  Table [DesignationMaster]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [DesignationMaster](
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
/****** Object:  Table [EducationMaster]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [EducationMaster](
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
/****** Object:  Table [GradeMaster]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [GradeMaster](
	[Id] [int] NULL,
	[Code] [nvarchar](20) NULL,
	[Description] [nvarchar](200) NULL,
	[Active] [bit] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [InstitutionMaster]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [InstitutionMaster](
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
/****** Object:  Table [LanguageMaster]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [LanguageMaster](
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
/****** Object:  Table [LogAction]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [LogAction](
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
/****** Object:  Table [LogActionMaster]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [LogActionMaster](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](25) NOT NULL,
 CONSTRAINT [PK_LogActionMaster] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [LogError]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [LogError](
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
/****** Object:  Table [LogModuleMaster]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [LogModuleMaster](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_LogModuleMaster] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [lu_Educational_Standard]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [lu_Educational_Standard](
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
/****** Object:  Table [lu_Educational_Use]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [lu_Educational_Use](
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
/****** Object:  Table [lu_Level]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [lu_Level](
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
/****** Object:  Table [MaterialTypeMaster]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [MaterialTypeMaster](
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
/****** Object:  Table [MessageType]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [MessageType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Type] [nvarchar](200) NULL,
 CONSTRAINT [PK_MessageType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [MoECheckMaster]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [MoECheckMaster](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[ContentId] [int] NULL,
	[ContentType] [int] NULL,
	[Status] [bit] NULL,
	[Comments] [nvarchar](max) NULL,
	[IsCurrent] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [Notifications]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Notifications](
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
/****** Object:  Table [OerConfig]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [OerConfig](
	[key] [nvarchar](255) NULL,
	[value] [nvarchar](255) NULL,
	[ConfigType] [nvarchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [ProfessionMaster]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ProfessionMaster](
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
/****** Object:  Table [QRCCategory]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [QRCCategory](
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
/****** Object:  Table [QRCMaster]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [QRCMaster](
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
/****** Object:  Table [QRCMasterCategory]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [QRCMasterCategory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[QrcId] [int] NOT NULL,
	[CategoryId] [int] NOT NULL,
 CONSTRAINT [PK_QRCMasterCategory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [QRCUserMapping]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [QRCUserMapping](
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
/****** Object:  Table [Questions]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Questions](
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
/****** Object:  Table [ResourceAbuseReports]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ResourceAbuseReports](
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
/****** Object:  Table [ResourceAlignmentRating]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ResourceAlignmentRating](
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
/****** Object:  Table [ResourceApprovals]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ResourceApprovals](
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
/****** Object:  Table [ResourceAssociatedFiles]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ResourceAssociatedFiles](
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
/****** Object:  Table [ResourceComments]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ResourceComments](
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
/****** Object:  Table [ResourceCommentsAbuseReports]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ResourceCommentsAbuseReports](
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
/****** Object:  Table [ResourceRemixHistory]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ResourceRemixHistory](
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
/****** Object:  Table [ResourceURLReferences]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ResourceURLReferences](
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
/****** Object:  Table [SectionResource]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [SectionResource](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SectionId] [int] NULL,
	[ResourceId] [numeric](18, 0) NULL,
 CONSTRAINT [PK_SectionResource] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [SensoryCheckMaster]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [SensoryCheckMaster](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[ContentId] [int] NULL,
	[ContentType] [int] NULL,
	[Status] [bit] NULL,
	[Comments] [nvarchar](max) NULL,
	[IsCurrent] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [SocialMediaMaster]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [SocialMediaMaster](
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
/****** Object:  Table [StateMaster]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [StateMaster](
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
/****** Object:  Table [StreamMaster]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [StreamMaster](
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
/****** Object:  Table [SubCategoryMaster]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [SubCategoryMaster](
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
/****** Object:  Table [Tests]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Tests](
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
/****** Object:  Table [TitleMaster]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [TitleMaster](
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
/****** Object:  Table [Titles]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Titles](
	[Id] [int] NULL,
	[Name] [nvarchar](20) NULL,
	[Active] [bit] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [UserBookmarks]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [UserBookmarks](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[ContentId] [int] NULL,
	[ContentType] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [UserCertification]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [UserCertification](
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
/****** Object:  Table [UserCourseTests]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [UserCourseTests](
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
/****** Object:  Table [UserEducation]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [UserEducation](
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
/****** Object:  Table [UserExperiences]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [UserExperiences](
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
/****** Object:  Table [UserLanguages]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [UserLanguages](
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
/****** Object:  Table [UserSocialMedia]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [UserSocialMedia](
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
/****** Object:  Table [Visiters]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Visiters](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[CreatedOn] [datetime] NULL,
 CONSTRAINT [PK_Visiters] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [WebContentPages]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [WebContentPages](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PageName] [nvarchar](50) NULL,
	[PageName_Ar] [nvarchar](100) NULL,
 CONSTRAINT [PK_WebContentPages] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [WebPageContent]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [WebPageContent](
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
/****** Object:  Table [WhiteListingURLs]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [WhiteListingURLs](
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
ALTER TABLE [CategoryMaster] ADD  CONSTRAINT [DF_CategoryMaster_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [CategoryMaster] ADD  CONSTRAINT [DF_CategoryMaster_UpdatedOn]  DEFAULT (getdate()) FOR [UpdatedOn]
GO
ALTER TABLE [CategoryMaster] ADD  CONSTRAINT [DF_CategoryMaster_Active]  DEFAULT ((0)) FOR [Active]
GO
ALTER TABLE [CategoryMaster] ADD  CONSTRAINT [DF_CategoryMaster_Status]  DEFAULT ((1)) FOR [Status]
GO
ALTER TABLE [CommunityCheckMaster] ADD  DEFAULT ((1)) FOR [IsCurrent]
GO
ALTER TABLE [CopyrightMaster] ADD  CONSTRAINT [DF_CopyrightMaster_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [CopyrightMaster] ADD  CONSTRAINT [DF_CopyrightMaster_UpdatedOn]  DEFAULT (getdate()) FOR [UpdatedOn]
GO
ALTER TABLE [CopyrightMaster] ADD  CONSTRAINT [DF_CopyrightMaster_Active]  DEFAULT ((0)) FOR [Active]
GO
ALTER TABLE [CopyrightMaster] ADD  DEFAULT ((0)) FOR [Protected]
GO
ALTER TABLE [CopyrightMaster] ADD  DEFAULT ((0)) FOR [IsResourceProtect]
GO
ALTER TABLE [CopyrightMaster] ADD  CONSTRAINT [DF_CopyrightMaster_Status]  DEFAULT ((1)) FOR [Status]
GO
ALTER TABLE [CountryMaster] ADD  CONSTRAINT [DF_CountryMaster_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [CountryMaster] ADD  CONSTRAINT [DF_CountryMaster_UpdatedOn]  DEFAULT (getdate()) FOR [UpdatedOn]
GO
ALTER TABLE [CountryMaster] ADD  CONSTRAINT [DF_CountryMaster_IsActive]  DEFAULT ((0)) FOR [IsActive]
GO
ALTER TABLE [CourseApprovals] ADD  CONSTRAINT [DF_CourseApprovals_ApprovedOn]  DEFAULT (getdate()) FOR [ApprovedOn]
GO
ALTER TABLE [CourseAssociatedFiles] ADD  CONSTRAINT [DF_CourseAssociatedFiles_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [CourseAssociatedFiles] ADD  DEFAULT ((1)) FOR [IsInclude]
GO
ALTER TABLE [CourseComments] ADD  CONSTRAINT [DF_CourseComments_CommentDate]  DEFAULT (getdate()) FOR [CommentDate]
GO
ALTER TABLE [CourseComments] ADD  CONSTRAINT [DF_CourseComments_ReportAbuseCount]  DEFAULT ((0)) FOR [ReportAbuseCount]
GO
ALTER TABLE [CourseComments] ADD  CONSTRAINT [DF_CourseComments_IsHidden]  DEFAULT ((0)) FOR [IsHidden]
GO
ALTER TABLE [CourseMaster] ADD  CONSTRAINT [DF_CourseMaster_IsDraft]  DEFAULT ((0)) FOR [IsDraft]
GO
ALTER TABLE [CourseMaster] ADD  CONSTRAINT [DF_CourseMaster_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [CourseMaster] ADD  CONSTRAINT [DF_CourseMaster_IsApproved]  DEFAULT ((0)) FOR [IsApproved]
GO
ALTER TABLE [CourseMaster] ADD  CONSTRAINT [DF_CourseMaster_Rating]  DEFAULT ((0)) FOR [Rating]
GO
ALTER TABLE [CourseMaster] ADD  CONSTRAINT [DF_CourseMaster_ReportAbuseCount]  DEFAULT ((0)) FOR [ReportAbuseCount]
GO
ALTER TABLE [CourseMaster] ADD  DEFAULT ((0)) FOR [CommunityBadge]
GO
ALTER TABLE [CourseMaster] ADD  DEFAULT ((0)) FOR [MoEBadge]
GO
ALTER TABLE [CourseMaster] ADD  DEFAULT ((0)) FOR [IsApprovedSensory]
GO
ALTER TABLE [CourseRating] ADD  CONSTRAINT [DF_CourseRating_Rating]  DEFAULT ((0)) FOR [Rating]
GO
ALTER TABLE [CourseRating] ADD  CONSTRAINT [DF_CourseRating_RatedOn]  DEFAULT (getdate()) FOR [RatedOn]
GO
ALTER TABLE [CourseURLReferences] ADD  CONSTRAINT [DF_CourseURLReferences_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [DepartmentMaster] ADD  CONSTRAINT [DF_DepartmentMaster_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [DepartmentMaster] ADD  CONSTRAINT [DF_DepartmentMaster_UpdatedOn]  DEFAULT (getdate()) FOR [UpdatedOn]
GO
ALTER TABLE [DepartmentMaster] ADD  CONSTRAINT [DF_DepartmentMaster_Active]  DEFAULT ((0)) FOR [Active]
GO
ALTER TABLE [DesignationMaster] ADD  CONSTRAINT [DF_DesignationMaster_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [DesignationMaster] ADD  CONSTRAINT [DF_DesignationMaster_UpdatedOn]  DEFAULT (getdate()) FOR [UpdatedOn]
GO
ALTER TABLE [DesignationMaster] ADD  CONSTRAINT [DF_DesignationMaster_Active]  DEFAULT ((0)) FOR [Active]
GO
ALTER TABLE [EducationMaster] ADD  CONSTRAINT [DF_EducationMaster_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [EducationMaster] ADD  CONSTRAINT [DF_EducationMaster_UpdatedOn]  DEFAULT (getdate()) FOR [UpdatedOn]
GO
ALTER TABLE [EducationMaster] ADD  CONSTRAINT [DF_EducationMaster_Active]  DEFAULT ((0)) FOR [Active]
GO
ALTER TABLE [EducationMaster] ADD  CONSTRAINT [DF_EducationMaster_Status]  DEFAULT ((1)) FOR [Status]
GO
ALTER TABLE [GradeMaster] ADD  CONSTRAINT [DF_GradeMaster_Active]  DEFAULT ((1)) FOR [Active]
GO
ALTER TABLE [InstitutionMaster] ADD  CONSTRAINT [DF_InstitutionMaster_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [InstitutionMaster] ADD  CONSTRAINT [DF_InstitutionMaster_UpdatedOn]  DEFAULT (getdate()) FOR [UpdatedOn]
GO
ALTER TABLE [InstitutionMaster] ADD  CONSTRAINT [DF_InstitutionMaster_Active]  DEFAULT ((0)) FOR [Active]
GO
ALTER TABLE [LanguageMaster] ADD  CONSTRAINT [DF_LanguageMaster_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [LanguageMaster] ADD  CONSTRAINT [DF_LanguageMaster_UpdatedOn]  DEFAULT (getdate()) FOR [UpdatedOn]
GO
ALTER TABLE [LanguageMaster] ADD  CONSTRAINT [DF_LanguageMaster_Active]  DEFAULT ((0)) FOR [Active]
GO
ALTER TABLE [LogAction] ADD  CONSTRAINT [DF_LogAction_ActionDate]  DEFAULT (getdate()) FOR [ActionDate]
GO
ALTER TABLE [LogError] ADD  CONSTRAINT [DF_LogError_MessageDate]  DEFAULT (getdate()) FOR [MessageDate]
GO
ALTER TABLE [lu_Educational_Standard] ADD  CONSTRAINT [DF_lu_Educational_Standard_Status]  DEFAULT ((1)) FOR [Status]
GO
ALTER TABLE [lu_Educational_Use] ADD  CONSTRAINT [DF_lu_Educational_Use_Status]  DEFAULT ((1)) FOR [Status]
GO
ALTER TABLE [lu_Level] ADD  CONSTRAINT [DF_lu_Level_Status]  DEFAULT ((1)) FOR [Status]
GO
ALTER TABLE [MaterialTypeMaster] ADD  CONSTRAINT [DF_MaterialTypeMaster_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [MaterialTypeMaster] ADD  CONSTRAINT [DF_MaterialTypeMaster_UpdatedOn]  DEFAULT (getdate()) FOR [UpdatedOn]
GO
ALTER TABLE [MaterialTypeMaster] ADD  CONSTRAINT [DF_MaterialTypeMaster_Active]  DEFAULT ((0)) FOR [Active]
GO
ALTER TABLE [MaterialTypeMaster] ADD  CONSTRAINT [DF_MaterialTypeMaster_Status]  DEFAULT ((1)) FOR [Status]
GO
ALTER TABLE [MoECheckMaster] ADD  DEFAULT ((1)) FOR [IsCurrent]
GO
ALTER TABLE [Notifications] ADD  CONSTRAINT [DF_Notifications_Status]  DEFAULT ((1)) FOR [Status]
GO
ALTER TABLE [ProfessionMaster] ADD  CONSTRAINT [DF_ProfessionMaster_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [ProfessionMaster] ADD  CONSTRAINT [DF_ProfessionMaster_UpdatedOn]  DEFAULT (getdate()) FOR [UpdatedOn]
GO
ALTER TABLE [ProfessionMaster] ADD  CONSTRAINT [DF_ProfessionMaster_Active]  DEFAULT ((0)) FOR [Active]
GO
ALTER TABLE [ProfessionMaster] ADD  CONSTRAINT [DF_ProfessionMaster_Status]  DEFAULT ((1)) FOR [Status]
GO
ALTER TABLE [QRCCategory] ADD  DEFAULT ((0)) FOR [IsAvailable]
GO
ALTER TABLE [QRCMaster] ADD  CONSTRAINT [DF_QRCMaster_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [QRCMaster] ADD  CONSTRAINT [DF_QRCMaster_UpdatedOn]  DEFAULT (getdate()) FOR [UpdatedOn]
GO
ALTER TABLE [QRCMaster] ADD  CONSTRAINT [DF_QRCMaster_Active]  DEFAULT ((0)) FOR [Active]
GO
ALTER TABLE [ResourceAlignmentRating] ADD  CONSTRAINT [DF_ResourceAlignmentRating_Rating]  DEFAULT ((0)) FOR [Rating]
GO
ALTER TABLE [ResourceAlignmentRating] ADD  CONSTRAINT [DF_ResourceAlignmentRating_RatedOn]  DEFAULT (getdate()) FOR [RatedOn]
GO
ALTER TABLE [ResourceApprovals] ADD  CONSTRAINT [DF_ResourceApprovals_ApprovedOn]  DEFAULT (getdate()) FOR [ApprovedOn]
GO
ALTER TABLE [ResourceAssociatedFiles] ADD  CONSTRAINT [DF_ResourceAssociatedFiles_UploadedDate]  DEFAULT (getdate()) FOR [UploadedDate]
GO
ALTER TABLE [ResourceAssociatedFiles] ADD  DEFAULT ((1)) FOR [IsInclude]
GO
ALTER TABLE [ResourceComments] ADD  CONSTRAINT [DF_ResourceComments_CommentDate]  DEFAULT (getdate()) FOR [CommentDate]
GO
ALTER TABLE [ResourceComments] ADD  CONSTRAINT [DF_ResourceComments_ReportAbuseCount]  DEFAULT ((0)) FOR [ReportAbuseCount]
GO
ALTER TABLE [ResourceComments] ADD  CONSTRAINT [DF_ResourceComments_IsHidden]  DEFAULT ((0)) FOR [IsHidden]
GO
ALTER TABLE [ResourceMaster] ADD  CONSTRAINT [DF_ResourceMaster_IsDraft]  DEFAULT ((0)) FOR [IsDraft]
GO
ALTER TABLE [ResourceMaster] ADD  CONSTRAINT [DF_ResourceMaster_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [ResourceMaster] ADD  CONSTRAINT [DF_ResourceMaster_IsApproved]  DEFAULT ((0)) FOR [IsApproved]
GO
ALTER TABLE [ResourceMaster] ADD  CONSTRAINT [DF_ResourceMaster_Rating]  DEFAULT ((0)) FOR [Rating]
GO
ALTER TABLE [ResourceMaster] ADD  CONSTRAINT [DF_ResourceMaster_AlignmentRating]  DEFAULT ((0)) FOR [AlignmentRating]
GO
ALTER TABLE [ResourceMaster] ADD  CONSTRAINT [DF_ResourceMaster_ReportAbuseCount]  DEFAULT ((0)) FOR [ReportAbuseCount]
GO
ALTER TABLE [ResourceMaster] ADD  DEFAULT ((0)) FOR [CommunityBadge]
GO
ALTER TABLE [ResourceMaster] ADD  DEFAULT ((0)) FOR [MoEBadge]
GO
ALTER TABLE [ResourceMaster] ADD  DEFAULT ((0)) FOR [IsApprovedSensory]
GO
ALTER TABLE [ResourceRating] ADD  CONSTRAINT [DF_ResourceRating_Rating]  DEFAULT ((0)) FOR [Rating]
GO
ALTER TABLE [ResourceRating] ADD  CONSTRAINT [DF_ResourceRating_RatedOn]  DEFAULT (getdate()) FOR [RatedOn]
GO
ALTER TABLE [ResourceURLReferences] ADD  CONSTRAINT [DF_ResourceURLReferences_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [SensoryCheckMaster] ADD  DEFAULT ((1)) FOR [IsCurrent]
GO
ALTER TABLE [SocialMediaMaster] ADD  CONSTRAINT [DF_SocialMediaMaster_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [StateMaster] ADD  CONSTRAINT [DF_StateMaster_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [StateMaster] ADD  CONSTRAINT [DF_StateMaster_UpdatedOn]  DEFAULT (getdate()) FOR [UpdatedOn]
GO
ALTER TABLE [StateMaster] ADD  CONSTRAINT [DF_StateMaster_IsActive]  DEFAULT ((0)) FOR [IsActive]
GO
ALTER TABLE [StreamMaster] ADD  CONSTRAINT [DF_StreamMaster_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [StreamMaster] ADD  CONSTRAINT [DF_StreamMaster_UpdatedOn]  DEFAULT (getdate()) FOR [UpdatedOn]
GO
ALTER TABLE [StreamMaster] ADD  CONSTRAINT [DF_StreamMaster_Active]  DEFAULT ((0)) FOR [Active]
GO
ALTER TABLE [SubCategoryMaster] ADD  CONSTRAINT [DF_SubCategoryMaster_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [SubCategoryMaster] ADD  CONSTRAINT [DF_SubCategoryMaster_UpdatedOn]  DEFAULT (getdate()) FOR [UpdatedOn]
GO
ALTER TABLE [SubCategoryMaster] ADD  CONSTRAINT [DF_SubCategoryMaster_Active]  DEFAULT ((0)) FOR [Active]
GO
ALTER TABLE [SubCategoryMaster] ADD  CONSTRAINT [DF_SubCategoryMaster_Status]  DEFAULT ((1)) FOR [Status]
GO
ALTER TABLE [TitleMaster] ADD  CONSTRAINT [DF_TitleMaster_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [TitleMaster] ADD  CONSTRAINT [DF_TitleMaster_UpdatedOn]  DEFAULT (getdate()) FOR [UpdatedOn]
GO
ALTER TABLE [TitleMaster] ADD  CONSTRAINT [DF_TitleMaster_IsActive]  DEFAULT ((0)) FOR [IsActive]
GO
ALTER TABLE [Titles] ADD  CONSTRAINT [DF_Titles_Active]  DEFAULT ((1)) FOR [Active]
GO
ALTER TABLE [UserCertification] ADD  CONSTRAINT [DF_UserCertification_Year]  DEFAULT (datepart(year,getdate())) FOR [Year]
GO
ALTER TABLE [UserCertification] ADD  CONSTRAINT [DF_UserCertification_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [UserEducation] ADD  CONSTRAINT [DF_UserEducation_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [UserExperiences] ADD  CONSTRAINT [DF_UserExperiences_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [UserLanguages] ADD  CONSTRAINT [DF_Table1_Speak]  DEFAULT ((0)) FOR [IsSpeak]
GO
ALTER TABLE [UserLanguages] ADD  CONSTRAINT [DF_Table1_Read]  DEFAULT ((0)) FOR [IsRead]
GO
ALTER TABLE [UserLanguages] ADD  CONSTRAINT [DF_Table1_Write]  DEFAULT ((0)) FOR [IsWrite]
GO
ALTER TABLE [UserLanguages] ADD  CONSTRAINT [DF_UserLanguages_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [UserMaster] ADD  CONSTRAINT [DF_UserMaster_Gender]  DEFAULT ((0)) FOR [Gender]
GO
ALTER TABLE [UserMaster] ADD  CONSTRAINT [DF_AdminUser_ApprovalStatus]  DEFAULT ((0)) FOR [ApprovalStatus]
GO
ALTER TABLE [UserMaster] ADD  CONSTRAINT [DF_AdminUser_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [UserMaster] ADD  CONSTRAINT [DF_AdminUser_UpdatedOn]  DEFAULT (getdate()) FOR [UpdatedOn]
GO
ALTER TABLE [UserMaster] ADD  CONSTRAINT [DF_AdminUser_Active]  DEFAULT ((0)) FOR [Active]
GO
ALTER TABLE [UserMaster] ADD  CONSTRAINT [DF_UserMaster_IsContributor]  DEFAULT ((0)) FOR [IsContributor]
GO
ALTER TABLE [UserMaster] ADD  CONSTRAINT [DF_UserMaster_IsAdmin]  DEFAULT ((0)) FOR [IsAdmin]
GO
ALTER TABLE [UserMaster] ADD  CONSTRAINT [D_usermaster_IsEmailNotification]  DEFAULT ((1)) FOR [IsEmailNotification]
GO
ALTER TABLE [UserMaster] ADD  DEFAULT ('Blue') FOR [Theme]
GO
ALTER TABLE [UserSocialMedia] ADD  CONSTRAINT [DF_UserSocialMedia_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [WhiteListingURLs] ADD  CONSTRAINT [DF_URLWhiteListing_IsApproved]  DEFAULT ((0)) FOR [IsApproved]
GO
ALTER TABLE [WhiteListingURLs] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [Announcements]  WITH CHECK ADD  CONSTRAINT [FK_Announcements_UserMaster] FOREIGN KEY([CreatedBy])
REFERENCES [UserMaster] ([Id])
GO
ALTER TABLE [Announcements] CHECK CONSTRAINT [FK_Announcements_UserMaster]
GO
ALTER TABLE [Announcements]  WITH CHECK ADD  CONSTRAINT [FK_Announcements_UserMaster1] FOREIGN KEY([UpdatedBy])
REFERENCES [UserMaster] ([Id])
GO
ALTER TABLE [Announcements] CHECK CONSTRAINT [FK_Announcements_UserMaster1]
GO
ALTER TABLE [AnswerOptions]  WITH CHECK ADD  CONSTRAINT [FK_AnswerOptions_Questions] FOREIGN KEY([QuestionId])
REFERENCES [Questions] ([Id])
GO
ALTER TABLE [AnswerOptions] CHECK CONSTRAINT [FK_AnswerOptions_Questions]
GO
ALTER TABLE [CategoryMaster]  WITH CHECK ADD  CONSTRAINT [FK_CategoryMaster_UserMaster] FOREIGN KEY([CreatedBy])
REFERENCES [UserMaster] ([Id])
GO
ALTER TABLE [CategoryMaster] CHECK CONSTRAINT [FK_CategoryMaster_UserMaster]
GO
ALTER TABLE [CategoryMaster]  WITH CHECK ADD  CONSTRAINT [FK_CategoryMaster_UserMaster1] FOREIGN KEY([UpdatedBy])
REFERENCES [UserMaster] ([Id])
GO
ALTER TABLE [CategoryMaster] CHECK CONSTRAINT [FK_CategoryMaster_UserMaster1]
GO
ALTER TABLE [CommunityCheckMaster]  WITH CHECK ADD  CONSTRAINT [fk_communitycheckuser] FOREIGN KEY([UserId])
REFERENCES [UserMaster] ([Id])
GO
ALTER TABLE [CommunityCheckMaster] CHECK CONSTRAINT [fk_communitycheckuser]
GO
ALTER TABLE [ContactUs]  WITH CHECK ADD  CONSTRAINT [FK_ContactUs_UserMaster] FOREIGN KEY([RepliedBy])
REFERENCES [UserMaster] ([Id])
GO
ALTER TABLE [ContactUs] CHECK CONSTRAINT [FK_ContactUs_UserMaster]
GO
ALTER TABLE [ContentApproval]  WITH CHECK ADD  CONSTRAINT [FK_ContentApproval_UserMaster] FOREIGN KEY([QrcId])
REFERENCES [QRCMaster] ([Id])
GO
ALTER TABLE [ContentApproval] CHECK CONSTRAINT [FK_ContentApproval_UserMaster]
GO
ALTER TABLE [ContentApproval]  WITH CHECK ADD  CONSTRAINT [FK_ContentApproval_UserMaster1] FOREIGN KEY([UpdatedBy])
REFERENCES [UserMaster] ([Id])
GO
ALTER TABLE [ContentApproval] CHECK CONSTRAINT [FK_ContentApproval_UserMaster1]
GO
ALTER TABLE [ContentApproval]  WITH CHECK ADD  CONSTRAINT [FK_ContentApproval_UserMaster2] FOREIGN KEY([AssignedTo])
REFERENCES [UserMaster] ([Id])
GO
ALTER TABLE [ContentApproval] CHECK CONSTRAINT [FK_ContentApproval_UserMaster2]
GO
ALTER TABLE [ContentApproval]  WITH CHECK ADD  CONSTRAINT [FK_ContentApproval_UserMaster3] FOREIGN KEY([ApprovedBy])
REFERENCES [UserMaster] ([Id])
GO
ALTER TABLE [ContentApproval] CHECK CONSTRAINT [FK_ContentApproval_UserMaster3]
GO
ALTER TABLE [ContentDownloadInfo]  WITH CHECK ADD  CONSTRAINT [FK_ContentDownloadInfo_UserMaster] FOREIGN KEY([DownloadedBy])
REFERENCES [UserMaster] ([Id])
GO
ALTER TABLE [ContentDownloadInfo] CHECK CONSTRAINT [FK_ContentDownloadInfo_UserMaster]
GO
ALTER TABLE [ContentSharedInfo]  WITH CHECK ADD  CONSTRAINT [FK_ContentSharedInfo_UserMaster] FOREIGN KEY([CreatedBy])
REFERENCES [UserMaster] ([Id])
GO
ALTER TABLE [ContentSharedInfo] CHECK CONSTRAINT [FK_ContentSharedInfo_UserMaster]
GO
ALTER TABLE [CopyrightMaster]  WITH CHECK ADD  CONSTRAINT [FK_CopyrightMaster_UserMaster] FOREIGN KEY([CreatedBy])
REFERENCES [UserMaster] ([Id])
GO
ALTER TABLE [CopyrightMaster] CHECK CONSTRAINT [FK_CopyrightMaster_UserMaster]
GO
ALTER TABLE [CopyrightMaster]  WITH CHECK ADD  CONSTRAINT [FK_CopyrightMaster_UserMaster1] FOREIGN KEY([UpdatedBy])
REFERENCES [UserMaster] ([Id])
GO
ALTER TABLE [CopyrightMaster] CHECK CONSTRAINT [FK_CopyrightMaster_UserMaster1]
GO
ALTER TABLE [CountryMaster]  WITH CHECK ADD  CONSTRAINT [FK_CountryMaster_UserMaster] FOREIGN KEY([CreatedBy])
REFERENCES [UserMaster] ([Id])
GO
ALTER TABLE [CountryMaster] CHECK CONSTRAINT [FK_CountryMaster_UserMaster]
GO
ALTER TABLE [CountryMaster]  WITH CHECK ADD  CONSTRAINT [FK_CountryMaster_UserMaster1] FOREIGN KEY([UpdatedBy])
REFERENCES [UserMaster] ([Id])
GO
ALTER TABLE [CountryMaster] CHECK CONSTRAINT [FK_CountryMaster_UserMaster1]
GO
ALTER TABLE [CourseApprovals]  WITH CHECK ADD  CONSTRAINT [FK_CourseApprovals_CourseMaster] FOREIGN KEY([CourseId])
REFERENCES [CourseMaster] ([Id])
GO
ALTER TABLE [CourseApprovals] CHECK CONSTRAINT [FK_CourseApprovals_CourseMaster]
GO
ALTER TABLE [CourseApprovals]  WITH CHECK ADD  CONSTRAINT [FK_CourseApprovals_UserMaster] FOREIGN KEY([ApprovedBy])
REFERENCES [UserMaster] ([Id])
GO
ALTER TABLE [CourseApprovals] CHECK CONSTRAINT [FK_CourseApprovals_UserMaster]
GO
ALTER TABLE [CourseAssociatedFiles]  WITH CHECK ADD  CONSTRAINT [FK_CourseAssociatedFiles_CourseMaster] FOREIGN KEY([CourseId])
REFERENCES [CourseMaster] ([Id])
GO
ALTER TABLE [CourseAssociatedFiles] CHECK CONSTRAINT [FK_CourseAssociatedFiles_CourseMaster]
GO
ALTER TABLE [CourseComments]  WITH CHECK ADD  CONSTRAINT [FK_CourseComments_CourseMaster] FOREIGN KEY([CourseId])
REFERENCES [CourseMaster] ([Id])
GO
ALTER TABLE [CourseComments] CHECK CONSTRAINT [FK_CourseComments_CourseMaster]
GO
ALTER TABLE [CourseComments]  WITH CHECK ADD  CONSTRAINT [FK_CourseComments_UserMaster] FOREIGN KEY([UserId])
REFERENCES [UserMaster] ([Id])
GO
ALTER TABLE [CourseComments] CHECK CONSTRAINT [FK_CourseComments_UserMaster]
GO
ALTER TABLE [CourseEnrollment]  WITH CHECK ADD  CONSTRAINT [FK_CourseEnrollment_CourseEnrollment] FOREIGN KEY([CourseId])
REFERENCES [CourseMaster] ([Id])
GO
ALTER TABLE [CourseEnrollment] CHECK CONSTRAINT [FK_CourseEnrollment_CourseEnrollment]
GO
ALTER TABLE [CourseEnrollment]  WITH CHECK ADD  CONSTRAINT [FK_CourseEnrollment_UserMaster] FOREIGN KEY([UserId])
REFERENCES [UserMaster] ([Id])
GO
ALTER TABLE [CourseEnrollment] CHECK CONSTRAINT [FK_CourseEnrollment_UserMaster]
GO
ALTER TABLE [CourseMaster]  WITH CHECK ADD  CONSTRAINT [FK_CourseMaster_CategoryMaster] FOREIGN KEY([CategoryId])
REFERENCES [CategoryMaster] ([Id])
GO
ALTER TABLE [CourseMaster] CHECK CONSTRAINT [FK_CourseMaster_CategoryMaster]
GO
ALTER TABLE [CourseMaster]  WITH CHECK ADD  CONSTRAINT [FK_CourseMaster_UserMaster] FOREIGN KEY([CreatedBy])
REFERENCES [UserMaster] ([Id])
GO
ALTER TABLE [CourseMaster] CHECK CONSTRAINT [FK_CourseMaster_UserMaster]
GO
ALTER TABLE [CourseResourceMapping]  WITH CHECK ADD  CONSTRAINT [FK_CourseResourceMapping_CourseResourceMapping] FOREIGN KEY([CourseId])
REFERENCES [CourseMaster] ([Id])
GO
ALTER TABLE [CourseResourceMapping] CHECK CONSTRAINT [FK_CourseResourceMapping_CourseResourceMapping]
GO
ALTER TABLE [CourseResourceMapping]  WITH CHECK ADD  CONSTRAINT [FK_CourseResourceMapping_ResourceMaster] FOREIGN KEY([ResourcesId])
REFERENCES [ResourceMaster] ([Id])
GO
ALTER TABLE [CourseResourceMapping] CHECK CONSTRAINT [FK_CourseResourceMapping_ResourceMaster]
GO
ALTER TABLE [CourseURLReferences]  WITH CHECK ADD  CONSTRAINT [FK_CourseURLReferences_CourseMaster] FOREIGN KEY([CourseId])
REFERENCES [CourseMaster] ([Id])
GO
ALTER TABLE [CourseURLReferences] CHECK CONSTRAINT [FK_CourseURLReferences_CourseMaster]
GO
ALTER TABLE [CourseURLReferences]  WITH CHECK ADD  CONSTRAINT [FK_CourseURLReferences_WhiteListingURLs] FOREIGN KEY([URLReferenceId])
REFERENCES [WhiteListingURLs] ([Id])
GO
ALTER TABLE [CourseURLReferences] CHECK CONSTRAINT [FK_CourseURLReferences_WhiteListingURLs]
GO
ALTER TABLE [DepartmentMaster]  WITH CHECK ADD  CONSTRAINT [FK_DepartmentMaster_UserMaster] FOREIGN KEY([CreatedBy])
REFERENCES [UserMaster] ([Id])
GO
ALTER TABLE [DepartmentMaster] CHECK CONSTRAINT [FK_DepartmentMaster_UserMaster]
GO
ALTER TABLE [DepartmentMaster]  WITH CHECK ADD  CONSTRAINT [FK_DepartmentMaster_UserMaster1] FOREIGN KEY([UpdatedBy])
REFERENCES [UserMaster] ([Id])
GO
ALTER TABLE [DepartmentMaster] CHECK CONSTRAINT [FK_DepartmentMaster_UserMaster1]
GO
ALTER TABLE [DesignationMaster]  WITH CHECK ADD  CONSTRAINT [FK_DesignationMaster_UserMaster] FOREIGN KEY([CreatedBy])
REFERENCES [UserMaster] ([Id])
GO
ALTER TABLE [DesignationMaster] CHECK CONSTRAINT [FK_DesignationMaster_UserMaster]
GO
ALTER TABLE [DesignationMaster]  WITH CHECK ADD  CONSTRAINT [FK_DesignationMaster_UserMaster1] FOREIGN KEY([UpdatedBy])
REFERENCES [UserMaster] ([Id])
GO
ALTER TABLE [DesignationMaster] CHECK CONSTRAINT [FK_DesignationMaster_UserMaster1]
GO
ALTER TABLE [EducationMaster]  WITH CHECK ADD  CONSTRAINT [FK_EducationMaster_UserMaster] FOREIGN KEY([CreatedBy])
REFERENCES [UserMaster] ([Id])
GO
ALTER TABLE [EducationMaster] CHECK CONSTRAINT [FK_EducationMaster_UserMaster]
GO
ALTER TABLE [EducationMaster]  WITH CHECK ADD  CONSTRAINT [FK_EducationMaster_UserMaster1] FOREIGN KEY([UpdatedBy])
REFERENCES [UserMaster] ([Id])
GO
ALTER TABLE [EducationMaster] CHECK CONSTRAINT [FK_EducationMaster_UserMaster1]
GO
ALTER TABLE [InstitutionMaster]  WITH CHECK ADD  CONSTRAINT [FK_InstitutionMaster_UserMaster] FOREIGN KEY([CreatedBy])
REFERENCES [UserMaster] ([Id])
GO
ALTER TABLE [InstitutionMaster] CHECK CONSTRAINT [FK_InstitutionMaster_UserMaster]
GO
ALTER TABLE [InstitutionMaster]  WITH CHECK ADD  CONSTRAINT [FK_InstitutionMaster_UserMaster1] FOREIGN KEY([UpdatedBy])
REFERENCES [UserMaster] ([Id])
GO
ALTER TABLE [InstitutionMaster] CHECK CONSTRAINT [FK_InstitutionMaster_UserMaster1]
GO
ALTER TABLE [LanguageMaster]  WITH CHECK ADD  CONSTRAINT [FK_LanguageMaster_UserMaster] FOREIGN KEY([CreatedBy])
REFERENCES [UserMaster] ([Id])
GO
ALTER TABLE [LanguageMaster] CHECK CONSTRAINT [FK_LanguageMaster_UserMaster]
GO
ALTER TABLE [LanguageMaster]  WITH CHECK ADD  CONSTRAINT [FK_LanguageMaster_UserMaster1] FOREIGN KEY([UpdatedBy])
REFERENCES [UserMaster] ([Id])
GO
ALTER TABLE [LanguageMaster] CHECK CONSTRAINT [FK_LanguageMaster_UserMaster1]
GO
ALTER TABLE [LogAction]  WITH CHECK ADD  CONSTRAINT [FK_LogAction_LogActionMaster] FOREIGN KEY([ActionId])
REFERENCES [LogActionMaster] ([Id])
GO
ALTER TABLE [LogAction] CHECK CONSTRAINT [FK_LogAction_LogActionMaster]
GO
ALTER TABLE [LogAction]  WITH CHECK ADD  CONSTRAINT [FK_LogAction_LogModuleMaster] FOREIGN KEY([LogModuleId])
REFERENCES [LogModuleMaster] ([Id])
GO
ALTER TABLE [LogAction] CHECK CONSTRAINT [FK_LogAction_LogModuleMaster]
GO
ALTER TABLE [LogAction]  WITH CHECK ADD  CONSTRAINT [FK_LogAction_UserMaster] FOREIGN KEY([UserId])
REFERENCES [UserMaster] ([Id])
GO
ALTER TABLE [LogAction] CHECK CONSTRAINT [FK_LogAction_UserMaster]
GO
ALTER TABLE [LogError]  WITH CHECK ADD  CONSTRAINT [FK_LogError_LogModuleMaster] FOREIGN KEY([LogModuleId])
REFERENCES [LogModuleMaster] ([Id])
GO
ALTER TABLE [LogError] CHECK CONSTRAINT [FK_LogError_LogModuleMaster]
GO
ALTER TABLE [LogError]  WITH CHECK ADD  CONSTRAINT [FK_LogError_UserMaster] FOREIGN KEY([UserId])
REFERENCES [UserMaster] ([Id])
GO
ALTER TABLE [LogError] CHECK CONSTRAINT [FK_LogError_UserMaster]
GO
ALTER TABLE [MaterialTypeMaster]  WITH CHECK ADD  CONSTRAINT [FK_MaterialTypeMaster_UserMaster] FOREIGN KEY([CreatedBy])
REFERENCES [UserMaster] ([Id])
GO
ALTER TABLE [MaterialTypeMaster] CHECK CONSTRAINT [FK_MaterialTypeMaster_UserMaster]
GO
ALTER TABLE [MaterialTypeMaster]  WITH CHECK ADD  CONSTRAINT [FK_MaterialTypeMaster_UserMaster1] FOREIGN KEY([UpdatedBy])
REFERENCES [UserMaster] ([Id])
GO
ALTER TABLE [MaterialTypeMaster] CHECK CONSTRAINT [FK_MaterialTypeMaster_UserMaster1]
GO
ALTER TABLE [MoECheckMaster]  WITH CHECK ADD  CONSTRAINT [fk_moecheckuser] FOREIGN KEY([UserId])
REFERENCES [UserMaster] ([Id])
GO
ALTER TABLE [MoECheckMaster] CHECK CONSTRAINT [fk_moecheckuser]
GO
ALTER TABLE [Notifications]  WITH CHECK ADD  CONSTRAINT [FK_Notifications_MessageType] FOREIGN KEY([MessageTypeId])
REFERENCES [MessageType] ([Id])
GO
ALTER TABLE [Notifications] CHECK CONSTRAINT [FK_Notifications_MessageType]
GO
ALTER TABLE [Notifications]  WITH CHECK ADD  CONSTRAINT [FK_Notifications_UserMaster] FOREIGN KEY([UserId])
REFERENCES [UserMaster] ([Id])
GO
ALTER TABLE [Notifications] CHECK CONSTRAINT [FK_Notifications_UserMaster]
GO
ALTER TABLE [ProfessionMaster]  WITH CHECK ADD  CONSTRAINT [FK_ProfessionMaster_UserMaster] FOREIGN KEY([CreatedBy])
REFERENCES [UserMaster] ([Id])
GO
ALTER TABLE [ProfessionMaster] CHECK CONSTRAINT [FK_ProfessionMaster_UserMaster]
GO
ALTER TABLE [ProfessionMaster]  WITH CHECK ADD  CONSTRAINT [FK_ProfessionMaster_UserMaster1] FOREIGN KEY([UpdatedBy])
REFERENCES [UserMaster] ([Id])
GO
ALTER TABLE [ProfessionMaster] CHECK CONSTRAINT [FK_ProfessionMaster_UserMaster1]
GO
ALTER TABLE [QRCCategory]  WITH CHECK ADD  CONSTRAINT [FK_QRCCategory_CategoryMaster] FOREIGN KEY([CategoryId])
REFERENCES [CategoryMaster] ([Id])
GO
ALTER TABLE [QRCCategory] CHECK CONSTRAINT [FK_QRCCategory_CategoryMaster]
GO
ALTER TABLE [QRCCategory]  WITH CHECK ADD  CONSTRAINT [FK_QRCCategory_QRCMaster] FOREIGN KEY([QRCId])
REFERENCES [QRCMaster] ([Id])
GO
ALTER TABLE [QRCCategory] CHECK CONSTRAINT [FK_QRCCategory_QRCMaster]
GO
ALTER TABLE [QRCMaster]  WITH CHECK ADD  CONSTRAINT [FK_QRCMaster_UserMaster] FOREIGN KEY([CreatedBy])
REFERENCES [UserMaster] ([Id])
GO
ALTER TABLE [QRCMaster] CHECK CONSTRAINT [FK_QRCMaster_UserMaster]
GO
ALTER TABLE [QRCMaster]  WITH CHECK ADD  CONSTRAINT [FK_QRCMaster_UserMaster1] FOREIGN KEY([UpdatedBy])
REFERENCES [UserMaster] ([Id])
GO
ALTER TABLE [QRCMaster] CHECK CONSTRAINT [FK_QRCMaster_UserMaster1]
GO
ALTER TABLE [QRCMasterCategory]  WITH CHECK ADD  CONSTRAINT [FK_QRCMasterCategory_QRCMaster] FOREIGN KEY([CategoryId])
REFERENCES [QRCMaster] ([Id])
GO
ALTER TABLE [QRCMasterCategory] CHECK CONSTRAINT [FK_QRCMasterCategory_QRCMaster]
GO
ALTER TABLE [QRCUserMapping]  WITH CHECK ADD  CONSTRAINT [FK_QRCUserMapping_CategoryMaster] FOREIGN KEY([CategoryId])
REFERENCES [CategoryMaster] ([Id])
GO
ALTER TABLE [QRCUserMapping] CHECK CONSTRAINT [FK_QRCUserMapping_CategoryMaster]
GO
ALTER TABLE [QRCUserMapping]  WITH CHECK ADD  CONSTRAINT [FK_QRCUserMapping_QRCMaster] FOREIGN KEY([QRCId])
REFERENCES [QRCMaster] ([Id])
GO
ALTER TABLE [QRCUserMapping] CHECK CONSTRAINT [FK_QRCUserMapping_QRCMaster]
GO
ALTER TABLE [QRCUserMapping]  WITH CHECK ADD  CONSTRAINT [FK_QRCUserMapping_UserMaster] FOREIGN KEY([UserId])
REFERENCES [UserMaster] ([Id])
GO
ALTER TABLE [QRCUserMapping] CHECK CONSTRAINT [FK_QRCUserMapping_UserMaster]
GO
ALTER TABLE [QRCUserMapping]  WITH CHECK ADD  CONSTRAINT [FK_QRCUserMapping_UserMaster1] FOREIGN KEY([CreatedBy])
REFERENCES [UserMaster] ([Id])
GO
ALTER TABLE [QRCUserMapping] CHECK CONSTRAINT [FK_QRCUserMapping_UserMaster1]
GO
ALTER TABLE [Questions]  WITH CHECK ADD  CONSTRAINT [FK_Questions_Tests] FOREIGN KEY([TestId])
REFERENCES [Tests] ([Id])
GO
ALTER TABLE [Questions] CHECK CONSTRAINT [FK_Questions_Tests]
GO
ALTER TABLE [ResourceApprovals]  WITH CHECK ADD  CONSTRAINT [FK_ResourceApprovals_ResourceMaster] FOREIGN KEY([ResourceId])
REFERENCES [ResourceMaster] ([Id])
GO
ALTER TABLE [ResourceApprovals] CHECK CONSTRAINT [FK_ResourceApprovals_ResourceMaster]
GO
ALTER TABLE [ResourceApprovals]  WITH CHECK ADD  CONSTRAINT [FK_ResourceApprovals_UserMaster] FOREIGN KEY([ApprovedBy])
REFERENCES [UserMaster] ([Id])
GO
ALTER TABLE [ResourceApprovals] CHECK CONSTRAINT [FK_ResourceApprovals_UserMaster]
GO
ALTER TABLE [ResourceAssociatedFiles]  WITH CHECK ADD  CONSTRAINT [FK_ResourceAssociatedFiles_ResourceMaster] FOREIGN KEY([ResourceId])
REFERENCES [ResourceMaster] ([Id])
GO
ALTER TABLE [ResourceAssociatedFiles] CHECK CONSTRAINT [FK_ResourceAssociatedFiles_ResourceMaster]
GO
ALTER TABLE [ResourceComments]  WITH CHECK ADD  CONSTRAINT [FK_ResourceComments_ResourceMaster] FOREIGN KEY([ResourceId])
REFERENCES [ResourceMaster] ([Id])
GO
ALTER TABLE [ResourceComments] CHECK CONSTRAINT [FK_ResourceComments_ResourceMaster]
GO
ALTER TABLE [ResourceComments]  WITH CHECK ADD  CONSTRAINT [FK_ResourceComments_UserMaster] FOREIGN KEY([UserId])
REFERENCES [UserMaster] ([Id])
GO
ALTER TABLE [ResourceComments] CHECK CONSTRAINT [FK_ResourceComments_UserMaster]
GO
ALTER TABLE [ResourceMaster]  WITH CHECK ADD  CONSTRAINT [FK_ResourceMaster_CategoryMaster] FOREIGN KEY([CategoryId])
REFERENCES [CategoryMaster] ([Id])
GO
ALTER TABLE [ResourceMaster] CHECK CONSTRAINT [FK_ResourceMaster_CategoryMaster]
GO
ALTER TABLE [ResourceMaster]  WITH CHECK ADD  CONSTRAINT [FK_ResourceMaster_CopyrightMaster] FOREIGN KEY([CopyRightId])
REFERENCES [CopyrightMaster] ([Id])
GO
ALTER TABLE [ResourceMaster] CHECK CONSTRAINT [FK_ResourceMaster_CopyrightMaster]
GO
ALTER TABLE [ResourceMaster]  WITH CHECK ADD  CONSTRAINT [FK_ResourceMaster_lu_Educational_Standard] FOREIGN KEY([EducationalStandardId])
REFERENCES [lu_Educational_Standard] ([Id])
GO
ALTER TABLE [ResourceMaster] CHECK CONSTRAINT [FK_ResourceMaster_lu_Educational_Standard]
GO
ALTER TABLE [ResourceMaster]  WITH CHECK ADD  CONSTRAINT [FK_ResourceMaster_lu_Educational_Use] FOREIGN KEY([EducationalUseId])
REFERENCES [lu_Educational_Use] ([Id])
GO
ALTER TABLE [ResourceMaster] CHECK CONSTRAINT [FK_ResourceMaster_lu_Educational_Use]
GO
ALTER TABLE [ResourceMaster]  WITH CHECK ADD  CONSTRAINT [FK_ResourceMaster_lu_Level] FOREIGN KEY([LevelId])
REFERENCES [lu_Level] ([Id])
GO
ALTER TABLE [ResourceMaster] CHECK CONSTRAINT [FK_ResourceMaster_lu_Level]
GO
ALTER TABLE [ResourceMaster]  WITH CHECK ADD  CONSTRAINT [FK_ResourceMaster_MaterialTypeMaster] FOREIGN KEY([MaterialTypeId])
REFERENCES [MaterialTypeMaster] ([Id])
GO
ALTER TABLE [ResourceMaster] CHECK CONSTRAINT [FK_ResourceMaster_MaterialTypeMaster]
GO
ALTER TABLE [ResourceMaster]  WITH CHECK ADD  CONSTRAINT [FK_ResourceMaster_UserMaster] FOREIGN KEY([CreatedBy])
REFERENCES [UserMaster] ([Id])
GO
ALTER TABLE [ResourceMaster] CHECK CONSTRAINT [FK_ResourceMaster_UserMaster]
GO
ALTER TABLE [ResourceURLReferences]  WITH CHECK ADD  CONSTRAINT [FK_ResourceURLReferences_ResourceMaster] FOREIGN KEY([ResourceId])
REFERENCES [ResourceMaster] ([Id])
GO
ALTER TABLE [ResourceURLReferences] CHECK CONSTRAINT [FK_ResourceURLReferences_ResourceMaster]
GO
ALTER TABLE [ResourceURLReferences]  WITH CHECK ADD  CONSTRAINT [FK_ResourceURLReferences_WhiteListingURLs] FOREIGN KEY([URLReferenceId])
REFERENCES [WhiteListingURLs] ([Id])
GO
ALTER TABLE [ResourceURLReferences] CHECK CONSTRAINT [FK_ResourceURLReferences_WhiteListingURLs]
GO
ALTER TABLE [SectionResource]  WITH CHECK ADD  CONSTRAINT [FK_SectionResource_CourseSections] FOREIGN KEY([SectionId])
REFERENCES [CourseSections] ([Id])
GO
ALTER TABLE [SectionResource] CHECK CONSTRAINT [FK_SectionResource_CourseSections]
GO
ALTER TABLE [SectionResource]  WITH CHECK ADD  CONSTRAINT [FK_SectionResource_SectionResource] FOREIGN KEY([ResourceId])
REFERENCES [ResourceMaster] ([Id])
GO
ALTER TABLE [SectionResource] CHECK CONSTRAINT [FK_SectionResource_SectionResource]
GO
ALTER TABLE [SensoryCheckMaster]  WITH CHECK ADD  CONSTRAINT [fk_sensorycheckuser] FOREIGN KEY([UserId])
REFERENCES [UserMaster] ([Id])
GO
ALTER TABLE [SensoryCheckMaster] CHECK CONSTRAINT [fk_sensorycheckuser]
GO
ALTER TABLE [SocialMediaMaster]  WITH CHECK ADD  CONSTRAINT [FK_SocialMediaMaster_UserMaster] FOREIGN KEY([CreatedBy])
REFERENCES [UserMaster] ([Id])
GO
ALTER TABLE [SocialMediaMaster] CHECK CONSTRAINT [FK_SocialMediaMaster_UserMaster]
GO
ALTER TABLE [SocialMediaMaster]  WITH CHECK ADD  CONSTRAINT [FK_SocialMediaMaster_UserMaster1] FOREIGN KEY([UpdatedBy])
REFERENCES [UserMaster] ([Id])
GO
ALTER TABLE [SocialMediaMaster] CHECK CONSTRAINT [FK_SocialMediaMaster_UserMaster1]
GO
ALTER TABLE [StateMaster]  WITH CHECK ADD  CONSTRAINT [FK_StateMaster_CountryMaster] FOREIGN KEY([CountryId])
REFERENCES [CountryMaster] ([Id])
GO
ALTER TABLE [StateMaster] CHECK CONSTRAINT [FK_StateMaster_CountryMaster]
GO
ALTER TABLE [StateMaster]  WITH CHECK ADD  CONSTRAINT [FK_StateMaster_UserMaster] FOREIGN KEY([CreatedBy])
REFERENCES [UserMaster] ([Id])
GO
ALTER TABLE [StateMaster] CHECK CONSTRAINT [FK_StateMaster_UserMaster]
GO
ALTER TABLE [StateMaster]  WITH CHECK ADD  CONSTRAINT [FK_StateMaster_UserMaster1] FOREIGN KEY([UpdatedBy])
REFERENCES [UserMaster] ([Id])
GO
ALTER TABLE [StateMaster] CHECK CONSTRAINT [FK_StateMaster_UserMaster1]
GO
ALTER TABLE [StreamMaster]  WITH CHECK ADD  CONSTRAINT [FK_StreamMaster_UserMaster] FOREIGN KEY([CreatedBy])
REFERENCES [UserMaster] ([Id])
GO
ALTER TABLE [StreamMaster] CHECK CONSTRAINT [FK_StreamMaster_UserMaster]
GO
ALTER TABLE [StreamMaster]  WITH CHECK ADD  CONSTRAINT [FK_StreamMaster_UserMaster1] FOREIGN KEY([UpdatedBy])
REFERENCES [UserMaster] ([Id])
GO
ALTER TABLE [StreamMaster] CHECK CONSTRAINT [FK_StreamMaster_UserMaster1]
GO
ALTER TABLE [SubCategoryMaster]  WITH CHECK ADD  CONSTRAINT [FK_SubCategoryMaster_CategoryMaster] FOREIGN KEY([CategoryId])
REFERENCES [CategoryMaster] ([Id])
GO
ALTER TABLE [SubCategoryMaster] CHECK CONSTRAINT [FK_SubCategoryMaster_CategoryMaster]
GO
ALTER TABLE [SubCategoryMaster]  WITH CHECK ADD  CONSTRAINT [FK_SubCategoryMaster_UserMaster] FOREIGN KEY([CreatedBy])
REFERENCES [UserMaster] ([Id])
GO
ALTER TABLE [SubCategoryMaster] CHECK CONSTRAINT [FK_SubCategoryMaster_UserMaster]
GO
ALTER TABLE [SubCategoryMaster]  WITH CHECK ADD  CONSTRAINT [FK_SubCategoryMaster_UserMaster1] FOREIGN KEY([UpdatedBy])
REFERENCES [UserMaster] ([Id])
GO
ALTER TABLE [SubCategoryMaster] CHECK CONSTRAINT [FK_SubCategoryMaster_UserMaster1]
GO
ALTER TABLE [Tests]  WITH CHECK ADD  CONSTRAINT [FK_Tests_Tests] FOREIGN KEY([CourseId])
REFERENCES [CourseMaster] ([Id])
GO
ALTER TABLE [Tests] CHECK CONSTRAINT [FK_Tests_Tests]
GO
ALTER TABLE [Tests]  WITH CHECK ADD  CONSTRAINT [FK_Tests_UserMaster] FOREIGN KEY([CreatedBy])
REFERENCES [UserMaster] ([Id])
GO
ALTER TABLE [Tests] CHECK CONSTRAINT [FK_Tests_UserMaster]
GO
ALTER TABLE [Tests]  WITH CHECK ADD  CONSTRAINT [FK_Tests_UserMaster1] FOREIGN KEY([UpdatedBy])
REFERENCES [UserMaster] ([Id])
GO
ALTER TABLE [Tests] CHECK CONSTRAINT [FK_Tests_UserMaster1]
GO
ALTER TABLE [TitleMaster]  WITH CHECK ADD  CONSTRAINT [FK_TitleMaster_UserMaster] FOREIGN KEY([CreatedBy])
REFERENCES [UserMaster] ([Id])
GO
ALTER TABLE [TitleMaster] CHECK CONSTRAINT [FK_TitleMaster_UserMaster]
GO
ALTER TABLE [TitleMaster]  WITH CHECK ADD  CONSTRAINT [FK_TitleMaster_UserMaster1] FOREIGN KEY([UpdatedBy])
REFERENCES [UserMaster] ([Id])
GO
ALTER TABLE [TitleMaster] CHECK CONSTRAINT [FK_TitleMaster_UserMaster1]
GO
ALTER TABLE [UserBookmarks]  WITH CHECK ADD  CONSTRAINT [FK_UserMaster_UserId] FOREIGN KEY([UserId])
REFERENCES [UserMaster] ([Id])
GO
ALTER TABLE [UserBookmarks] CHECK CONSTRAINT [FK_UserMaster_UserId]
GO
ALTER TABLE [UserCertification]  WITH CHECK ADD  CONSTRAINT [FK_UserCertification_UserMaster] FOREIGN KEY([UserId])
REFERENCES [UserMaster] ([Id])
GO
ALTER TABLE [UserCertification] CHECK CONSTRAINT [FK_UserCertification_UserMaster]
GO
ALTER TABLE [UserCourseTests]  WITH CHECK ADD  CONSTRAINT [FK_UserCourseTests_AnswerOptions1] FOREIGN KEY([AnswerId])
REFERENCES [AnswerOptions] ([Id])
GO
ALTER TABLE [UserCourseTests] CHECK CONSTRAINT [FK_UserCourseTests_AnswerOptions1]
GO
ALTER TABLE [UserCourseTests]  WITH CHECK ADD  CONSTRAINT [FK_UserCourseTests_CourseMaster] FOREIGN KEY([CourseId])
REFERENCES [CourseMaster] ([Id])
GO
ALTER TABLE [UserCourseTests] CHECK CONSTRAINT [FK_UserCourseTests_CourseMaster]
GO
ALTER TABLE [UserCourseTests]  WITH CHECK ADD  CONSTRAINT [FK_UserCourseTests_Questions1] FOREIGN KEY([QuestionId])
REFERENCES [Questions] ([Id])
GO
ALTER TABLE [UserCourseTests] CHECK CONSTRAINT [FK_UserCourseTests_Questions1]
GO
ALTER TABLE [UserCourseTests]  WITH CHECK ADD  CONSTRAINT [FK_UserCourseTests_UserMaster1] FOREIGN KEY([UserId])
REFERENCES [UserMaster] ([Id])
GO
ALTER TABLE [UserCourseTests] CHECK CONSTRAINT [FK_UserCourseTests_UserMaster1]
GO
ALTER TABLE [UserEducation]  WITH CHECK ADD  CONSTRAINT [FK_UserEducation_UserMaster] FOREIGN KEY([UserId])
REFERENCES [UserMaster] ([Id])
GO
ALTER TABLE [UserEducation] CHECK CONSTRAINT [FK_UserEducation_UserMaster]
GO
ALTER TABLE [UserExperiences]  WITH CHECK ADD  CONSTRAINT [FK_UserExperiences_UserMaster] FOREIGN KEY([UserId])
REFERENCES [UserMaster] ([Id])
GO
ALTER TABLE [UserExperiences] CHECK CONSTRAINT [FK_UserExperiences_UserMaster]
GO
ALTER TABLE [UserLanguages]  WITH CHECK ADD  CONSTRAINT [FK_UserLanguages_LanguageMaster] FOREIGN KEY([LanguageId])
REFERENCES [LanguageMaster] ([Id])
GO
ALTER TABLE [UserLanguages] CHECK CONSTRAINT [FK_UserLanguages_LanguageMaster]
GO
ALTER TABLE [UserLanguages]  WITH CHECK ADD  CONSTRAINT [FK_UserLanguages_UserMaster] FOREIGN KEY([UserId])
REFERENCES [UserMaster] ([Id])
GO
ALTER TABLE [UserLanguages] CHECK CONSTRAINT [FK_UserLanguages_UserMaster]
GO
ALTER TABLE [UserMaster]  WITH CHECK ADD  CONSTRAINT [FK_UserMaster_TitleMaster] FOREIGN KEY([TitleId])
REFERENCES [TitleMaster] ([Id])
GO
ALTER TABLE [UserMaster] CHECK CONSTRAINT [FK_UserMaster_TitleMaster]
GO
ALTER TABLE [UserSocialMedia]  WITH CHECK ADD  CONSTRAINT [FK_UserSocialMedia_SocialMediaMaster] FOREIGN KEY([SocialMediaId])
REFERENCES [SocialMediaMaster] ([Id])
GO
ALTER TABLE [UserSocialMedia] CHECK CONSTRAINT [FK_UserSocialMedia_SocialMediaMaster]
GO
ALTER TABLE [UserSocialMedia]  WITH CHECK ADD  CONSTRAINT [FK_UserSocialMedia_UserMaster] FOREIGN KEY([UserId])
REFERENCES [UserMaster] ([Id])
GO
ALTER TABLE [UserSocialMedia] CHECK CONSTRAINT [FK_UserSocialMedia_UserMaster]
GO
ALTER TABLE [Visiters]  WITH CHECK ADD  CONSTRAINT [FK_Visiters_UserMaster] FOREIGN KEY([UserId])
REFERENCES [UserMaster] ([Id])
GO
ALTER TABLE [Visiters] CHECK CONSTRAINT [FK_Visiters_UserMaster]
GO
ALTER TABLE [WebPageContent]  WITH CHECK ADD  CONSTRAINT [FK_WebPageContent_WebContentPages] FOREIGN KEY([PageID])
REFERENCES [WebContentPages] ([Id])
GO
ALTER TABLE [WebPageContent] CHECK CONSTRAINT [FK_WebPageContent_WebContentPages]
GO
ALTER TABLE [WhiteListingURLs]  WITH CHECK ADD  CONSTRAINT [FK_URLWhiteListing_UserMaster] FOREIGN KEY([VerifiedBy])
REFERENCES [UserMaster] ([Id])
GO
ALTER TABLE [WhiteListingURLs] CHECK CONSTRAINT [FK_URLWhiteListing_UserMaster]
GO
ALTER TABLE [WhiteListingURLs]  WITH CHECK ADD  CONSTRAINT [FK_WhiteListingURLs_UserMaster] FOREIGN KEY([RequestedBy])
REFERENCES [UserMaster] ([Id])
GO
ALTER TABLE [WhiteListingURLs] CHECK CONSTRAINT [FK_WhiteListingURLs_UserMaster]
GO
/****** Object:  StoredProcedure [ApproveCourse]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ApproveCourse @CourseId=3,@CreatedBy=36
CREATE PROCEDURE [ApproveCourse] 
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
/****** Object:  StoredProcedure [ApproveResource]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ApproveResource @resourceId=33,@CreatedBy =36
CREATE PROCEDURE [ApproveResource] 
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
/****** Object:  StoredProcedure [CommentOnCourse]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [CommentOnCourse]      
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
/****** Object:  StoredProcedure [CommentOnResource]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [CommentOnResource]        
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
/****** Object:  StoredProcedure [CreateCategory]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [CreateCategory] 
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
/****** Object:  StoredProcedure [CreateCopyright]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [CreateCopyright]     
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
/****** Object:  StoredProcedure [CreateCourse]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [CreateCourse]         
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
        
INSERT INTO [CourseMaster]        
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
        
INSERT INTO [SectionResource](ResourceId,        
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
  FROM [CourseComments] cc inner join UserMaster um on cc.UserId=um.Id AND cc.CourseId=@Id where cc.IsHidden=0              
                
            
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
/****** Object:  StoredProcedure [CreateEducation]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [CreateEducation] 
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
/****** Object:  StoredProcedure [CreateInstitution]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [CreateInstitution]   
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
/****** Object:  StoredProcedure [CreateLogEntry]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [CreateLogEntry] (
	@LogModuleId INT,
	@UserId INT,
	@ActionId INT,
	@ActionDetail NVARCHAR(250))

AS
BEGIN	
        INSERT INTO [LogAction]
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
/****** Object:  StoredProcedure [CreateMaterialType]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [CreateMaterialType] 
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
/****** Object:  StoredProcedure [CreateProfession]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [CreateProfession] 
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
/****** Object:  StoredProcedure [CreateQRC]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
      
      
CREATE PROCEDURE [CreateQRC]       
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
/****** Object:  StoredProcedure [CreateResource]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [CreateResource]           
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
          
INSERT INTO [ResourceMaster]          
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
/****** Object:  StoredProcedure [CreateStream]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [CreateStream] 
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
/****** Object:  StoredProcedure [CreateSubCategory]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [CreateSubCategory] 
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
/****** Object:  StoredProcedure [CreateUserInitialProfile]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [CreateUserInitialProfile]       
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
/****** Object:  StoredProcedure [CreateUserProfile]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [CreateUserProfile]     
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
/****** Object:  StoredProcedure [CreateWhiteListingRequest]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec CreateWhiteListingRequest 'https://bitbucket.org/dashbosdvsd',2

CREATE PROCEDURE [CreateWhiteListingRequest]   
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
/****** Object:  StoredProcedure [CreateWhiteListingRequestAfterCheck]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [CreateWhiteListingRequestAfterCheck] 
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
/****** Object:  StoredProcedure [DeleteCategory]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
CREATE PROCEDURE [DeleteCategory]   
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
/****** Object:  StoredProcedure [DeleteCopyright]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
CREATE PROCEDURE [DeleteCopyright]   
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
/****** Object:  StoredProcedure [DeleteCourse]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [DeleteCourse] 
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
/****** Object:  StoredProcedure [DeleteCourseComment]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [DeleteCourseComment]		  
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
/****** Object:  StoredProcedure [DeleteEducation]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
CREATE PROCEDURE [DeleteEducation]   
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
/****** Object:  StoredProcedure [DeleteInstitution]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [DeleteInstitution] 
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
/****** Object:  StoredProcedure [DeleteMaterialType]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
CREATE PROCEDURE [DeleteMaterialType]   
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
/****** Object:  StoredProcedure [DeleteProfession]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
CREATE PROCEDURE [DeleteProfession]   
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
/****** Object:  StoredProcedure [DeleteQRC]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [DeleteQRC] 
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
/****** Object:  StoredProcedure [DeleteResource]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

 CREATE PROCEDURE [DeleteResource]
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
/****** Object:  StoredProcedure [DeleteResourceComment]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [DeleteResourceComment]		  
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
/****** Object:  StoredProcedure [DeleteStream]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [DeleteStream] 
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
/****** Object:  StoredProcedure [DeleteSubCategory]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
CREATE PROCEDURE [DeleteSubCategory]   
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
/****** Object:  StoredProcedure [GetCategory]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
CREATE PROCEDURE [GetCategory]    
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
/****** Object:  StoredProcedure [GetCategoryById]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [GetCategoryById] 
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
/****** Object:  StoredProcedure [GetConfigurationsByType]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [GetConfigurationsByType]
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
/****** Object:  StoredProcedure [GetCopyright]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
CREATE PROCEDURE [GetCopyright]        
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
/****** Object:  StoredProcedure [GetCopyrightById]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [GetCopyrightById]     
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
/****** Object:  StoredProcedure [GetCourseById]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [GetCourseById]                     
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
  FROM [CourseComments] cc inner join UserMaster um on cc.UserId=um.Id AND cc.CourseId=@Id where cc.IsHidden=0                    
                      
                  
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
/****** Object:  StoredProcedure [GetCourses]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
CREATE PROCEDURE [GetCourses]     
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
  FROM [CourseComments] cc inner join UserMaster um on cc.UserId=um.Id where cc.IsHidden=0    
       
 --IF @@ROWCOUNT>0    
        
 return 105 -- record exists    
    
-- ELSE    
--  return 102 -- reconrd does not exists    
END 
GO
/****** Object:  StoredProcedure [GetEducation]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
CREATE PROCEDURE [GetEducation]    
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
/****** Object:  StoredProcedure [GetEducationById]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [GetEducationById] 
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
/****** Object:  StoredProcedure [GetInstitution]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
CREATE PROCEDURE [GetInstitution]      
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
/****** Object:  StoredProcedure [GetInstitutionById]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [GetInstitutionById]  
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
/****** Object:  StoredProcedure [GetMaterialType]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
CREATE PROCEDURE [GetMaterialType]    
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
/****** Object:  StoredProcedure [GetMaterialTypeById]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [GetMaterialTypeById] 
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
/****** Object:  StoredProcedure [GetProfession]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
CREATE PROCEDURE [GetProfession]    
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
/****** Object:  StoredProcedure [GetProfessionById]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [GetProfessionById] 
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
/****** Object:  StoredProcedure [GetProfileAppData]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [GetProfileAppData] 	
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
/****** Object:  StoredProcedure [GetQRC]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
CREATE PROCEDURE [GetQRC]    
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
/****** Object:  StoredProcedure [GetQRCbyCategoryId]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 -- exec GetQRCbyCategoryId 1    
CREATE PROCEDURE [GetQRCbyCategoryId]      
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
/****** Object:  StoredProcedure [GetQRCById]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [GetQRCById] 
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
/****** Object:  StoredProcedure [GetRatingsByContent]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [GetRatingsByContent]-- '[{"ContentId" : 1, "ContentType" : 1},{"ContentId" : 1, "ContentType" : 2}]'
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
/****** Object:  StoredProcedure [GetResource]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [GetResource]       
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
  FROM [ResourceComments] rc inner join UserMaster um on rc.UserId=um.Id where rc.IsHidden=0      
            
 return 105 -- record exists      
       
END      
      
else return 102 -- recond does not exists      
end  
GO
/****** Object:  StoredProcedure [GetResourceByCourseId]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [GetResourceByCourseId] 
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
  FROM [ResourceComments] rc inner join UserMaster um on rc.UserId=um.Id where rc.IsHidden=0
   AND rc.[ResourceId] in (SELECT ResourcesId from CourseResourceMapping WHERE CourseId = @CourseId)
  			 
	return 105 -- record exists
	
END

else return 102 -- recond does not exists
end

GO
/****** Object:  StoredProcedure [GetResourceById]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [GetResourceById]                       
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
  FROM [ResourceComments] rc inner join UserMaster um on rc.UserId=um.Id AND rc.ResourceId=@Id where rc.IsHidden=0                    
      
SELECT Rating,COUNT(RatedBy) As NoOfUsers    
from [ResourceRating] where resourceid = @Id    
GROUP BY Rating                   
 return 105 -- record exists                      
                      
 end                      
  ELSE                      
  return 102 -- reconrd does not exists                      
END     

GO
/****** Object:  StoredProcedure [GetStream]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
CREATE PROCEDURE [GetStream]    
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
/****** Object:  StoredProcedure [GetStreamById]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [GetStreamById] 
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
/****** Object:  StoredProcedure [GetSubCategory]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
CREATE PROCEDURE [GetSubCategory]    
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
/****** Object:  StoredProcedure [GetSubCategoryById]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [GetSubCategoryById] 
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
/****** Object:  StoredProcedure [GetUserProfileByEmail]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--EXEC GetUserProfileByEmail 'avnp@gmail.com'  
  
CREATE PROCEDURE [GetUserProfileByEmail]   
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
/****** Object:  StoredProcedure [GetUserProfileById]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [GetUserProfileById] 
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
/****** Object:  StoredProcedure [GetWhitelistedUrls]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [GetWhitelistedUrls] 

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
/****** Object:  StoredProcedure [GetWhitelistingRequests]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


    
CREATE PROCEDURE [GetWhitelistingRequests]     
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
/****** Object:  StoredProcedure [HideCourseCommentByAuthor]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [HideCourseCommentByAuthor]		  
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
/****** Object:  StoredProcedure [HideResourceCommentByAuthor]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [HideResourceCommentByAuthor]		  
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
/****** Object:  StoredProcedure [RateCourse]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [RateCourse]   
 @CourseId NUMERIC(18,0),  
 @Rating INT,  
 @Comments NVARCHAR(2000),    
 @RatedBy INT  
AS  
BEGIN  
Declare @return INT  
IF NOT EXISTS (SELECT TOP 1 1 FROM CourseRating WHERE CourseId=@CourseId AND RatedBy=@RatedBy)  
BEGIN   
 INSERT INTO [CourseRating]  
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
/****** Object:  StoredProcedure [RateResource]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [RateResource]   
 @ResourceId NUMERIC(18,0),  
 @Rating INT,  
 @Comments NVARCHAR(2000),    
 @RatedBy INT  
AS  
BEGIN  
Declare @return INT  
IF NOT EXISTS (SELECT TOP 1 1 FROM ResourceRating WHERE ResourceId=@ResourceId AND RatedBy=@RatedBy)  
BEGIN   
 INSERT INTO [ResourceRating]  
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
/****** Object:  StoredProcedure [RateResourceAlignment]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [RateResourceAlignment] 
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

 INSERT INTO [ResourceAlignmentRating]
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
/****** Object:  StoredProcedure [ReportCourse]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [ReportCourse]		  
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
/****** Object:  StoredProcedure [ReportCourseComment]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [ReportCourseComment]      
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
/****** Object:  StoredProcedure [ReportCourseCommentWithComment]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [ReportCourseCommentWithComment] 
	@CourseCommentId NUMERIC(18,0),
	@ReportReasons NVARCHAR(50),
	@Comments NVARCHAR(200),
	@ReportedBy INT
AS
BEGIN
Declare @return INT
IF NOT EXISTS (SELECT * FROM CourseCommentAbuseReports WHERE CourseCommentId=@CourseCommentId AND ReportedBy=@ReportedBy)
BEGIN	
		INSERT INTO [CourseCommentAbuseReports]
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
/****** Object:  StoredProcedure [ReportCourseWithComment]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [ReportCourseWithComment]   
 @CourseId NUMERIC(18,0),  
 @ReportReasons NVARCHAR(50),  
 @Comments NVARCHAR(200),  
 @ReportedBy INT  
AS  
BEGIN  
Declare @return INT  
IF NOT EXISTS (SELECT * FROM CourseAbuseReports WHERE CourseId=@CourseId AND ReportedBy=@ReportedBy)  
BEGIN   
  INSERT INTO [CourseAbuseReports]  
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
/****** Object:  StoredProcedure [ReportResource]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [ReportResource]		  
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
/****** Object:  StoredProcedure [ReportResourceComment]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [ReportResourceComment]      
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
/****** Object:  StoredProcedure [ReportResourceCommentWithComment]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [ReportResourceCommentWithComment] 
	@ResourceCommentId NUMERIC(18,0),
	@ReportReasons NVARCHAR(50),
	@Comments NVARCHAR(200),
	@ReportedBy INT
AS
BEGIN
Declare @return INT
IF NOT EXISTS (SELECT * FROM ResourceCommentsAbuseReports WHERE ResourceCommentId=@ResourceCommentId AND ReportedBy=@ReportedBy)
BEGIN	
		INSERT INTO [ResourceCommentsAbuseReports]
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
/****** Object:  StoredProcedure [ReportResourceWithComment]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [ReportResourceWithComment]     
 @ResourceId NUMERIC(18,0),    
 @ReportReasons NVARCHAR(50),    
 @Comments NVARCHAR(200),    
 @ReportedBy INT    
AS    
BEGIN    
Declare @return INT    
IF NOT EXISTS (SELECT * FROM ResourceAbuseReports WHERE ResourceId=@ResourceId AND ReportedBy=@ReportedBy)    
BEGIN     
    
    
    
  INSERT INTO [ResourceAbuseReports]    
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
/****** Object:  StoredProcedure [spd_AbuseReport]    Script Date: 12/17/2019 12:42:58 PM ******/
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
CREATE PROCEDURE [spd_AbuseReport]  
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
/****** Object:  StoredProcedure [spd_Announcements]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [spd_Announcements]
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
/****** Object:  StoredProcedure [spd_lu_Educational_Standard]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
-- =============================================  
-- Author:  <Author,,Name>  
-- Create date: <Create Date,,>  
-- Description: <Description,,>  
-- =============================================  
CREATE PROCEDURE [spd_lu_Educational_Standard]  
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
/****** Object:  StoredProcedure [spd_lu_Educational_Use]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================  
-- Author:  <Author,,Name>  
-- Create date: <Create Date,,>  
-- Description: <Description,,>  
-- =============================================  
CREATE PROCEDURE [spd_lu_Educational_Use]  
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
/****** Object:  StoredProcedure [spd_lu_Level]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
CREATE PROCEDURE [spd_lu_Level]  
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
/****** Object:  StoredProcedure [spd_Notifications]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [spd_Notifications] 
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
/****** Object:  StoredProcedure [spd_UnAssignedQRC]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--spd_UnAssignedQRC 1
CREATE PROCEDURE [spd_UnAssignedQRC]   
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
/****** Object:  StoredProcedure [spd_WhiteListingURLs]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



--  exec spd_WhiteListingURLs 1
CREATE PROCEDURE [spd_WhiteListingURLs]   
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
/****** Object:  StoredProcedure [spi_AddUserBookmarkedContent]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [spi_AddUserBookmarkedContent]
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
/****** Object:  StoredProcedure [spi_Announcements]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spi_Announcements]  
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
/****** Object:  StoredProcedure [spi_CommunityApproveReject]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spi_CommunityApproveReject]-- 101,10361,2,1,'test'  
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
/****** Object:  StoredProcedure [spi_ContactUs]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [spi_ContactUs]
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
/****** Object:  StoredProcedure [spi_ContentApproval]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spi_ContentApproval] --1890,2,0,'Test',1,10301,'/test'
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
/****** Object:  StoredProcedure [spi_ContentDownloadInfo]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spi_ContentDownloadInfo]  
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
/****** Object:  StoredProcedure [spi_ContentSharedInfo]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spi_ContentSharedInfo]
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
/****** Object:  StoredProcedure [spi_CourseAssociatedFiles]    Script Date: 12/17/2019 12:42:58 PM ******/
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
CREATE PROCEDURE [spi_CourseAssociatedFiles]
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
/****** Object:  StoredProcedure [spi_CourseEnrollment]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [spi_CourseEnrollment]
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
/****** Object:  StoredProcedure [spi_CourseTest]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spi_CourseTest]
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
/****** Object:  StoredProcedure [spi_CourseTestTake]    Script Date: 12/17/2019 12:42:58 PM ******/
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


CREATE PROCEDURE [spi_CourseTestTake]
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
/****** Object:  StoredProcedure [spi_DeleteUserBookmarkedContent]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [spi_DeleteUserBookmarkedContent]
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
/****** Object:  StoredProcedure [spi_lu_Educational_Standard]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- spi_lu_Educational_Standard @Standard='Standard4',@CreatedBy=66
CREATE PROCEDURE [spi_lu_Educational_Standard]
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
/****** Object:  StoredProcedure [spi_lu_Educational_Use]    Script Date: 12/17/2019 12:42:58 PM ******/
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
CREATE PROCEDURE [spi_lu_Educational_Use]
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
/****** Object:  StoredProcedure [spi_lu_Level]    Script Date: 12/17/2019 12:42:58 PM ******/
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
CREATE PROCEDURE [spi_lu_Level]
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
/****** Object:  StoredProcedure [spi_MoEApproveReject]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--select * from MoECheckMaster where contentid = 10223
CREATE   PROCEDURE [spi_MoEApproveReject]
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
/****** Object:  StoredProcedure [spi_Notifications]    Script Date: 12/17/2019 12:42:58 PM ******/
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
CREATE PROCEDURE [spi_Notifications]  
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
/****** Object:  StoredProcedure [spi_ResourceAssociatedFiles]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

  
-- =============================================  
-- Author:  <Author,,Name>  
-- Create date: <Create Date,,>  
-- Description: <Description,,>  
-- =============================================  
CREATE PROCEDURE [spi_ResourceAssociatedFiles]  
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
/****** Object:  StoredProcedure [spi_SensoryApproveReject]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [spi_SensoryApproveReject]    
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
/****** Object:  StoredProcedure [spi_Visiters]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- spi_Visiters 1
CREATE PROCEDURE [spi_Visiters]
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
/****** Object:  StoredProcedure [spi_WebPageContent]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================    
-- Author:  <Author,,Name>    
-- Create date: <Create Date,,>    
-- Description: <Description,,>    
-- =============================================    
CREATE PROCEDURE [spi_WebPageContent]    
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
/****** Object:  StoredProcedure [sps_AdminRejectedList]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [sps_AdminRejectedList]
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
/****** Object:  StoredProcedure [sps_Announcements]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================    
-- Author:  <Author,,Name>    
-- Create date: <Create Date,,>    
-- Description: <Description,,>    
-- =============================================    
CREATE PROCEDURE [sps_Announcements]    
     
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
/****** Object:  StoredProcedure [sps_AnnouncementsById]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [sps_AnnouncementsById]-- 1
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
/****** Object:  StoredProcedure [sps_AnnouncementsToUser]    Script Date: 12/17/2019 12:42:58 PM ******/
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
CREATE PROCEDURE [sps_AnnouncementsToUser]     
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
/****** Object:  StoredProcedure [sps_CategoryNotInQRC]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [sps_CategoryNotInQRC]
	
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
/****** Object:  StoredProcedure [sps_CommunityApprovedByUser]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [sps_CommunityApprovedByUser]
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
/****** Object:  StoredProcedure [sps_ContactUs]    Script Date: 12/17/2019 12:42:58 PM ******/
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
CREATE PROCEDURE [sps_ContactUs] 
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
/****** Object:  StoredProcedure [sps_ContentApprovalForVerifiers]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================    
-- Author:  <Author,,Name>    
-- Create date: <Create Date,,>    
-- Description: <Description,,>    
-- =============================================    
CREATE PROCEDURE [sps_ContentApprovalForVerifiers]    
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
/****** Object:  StoredProcedure [sps_ContentFileNames]    Script Date: 12/17/2019 12:42:58 PM ******/
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
CREATE PROCEDURE [sps_ContentFileNames]     
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
FileName FROM [CourseAssociatedFiles] WHERE courseid =@ContentId    
END    
    
ELSE    
BEGIN    
BEGIN    
select  Id,    
ResourceId as ContentId,    
AssociatedFile,
FileName from [ResourceAssociatedFiles] WHERE resourceid =@ContentId    
END    
END;    
    
 SET @Return = 105;    
    
 RETURN @Return;    
    
END; 
GO
/****** Object:  StoredProcedure [sps_ContentForApproval]    Script Date: 12/17/2019 12:42:58 PM ******/
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
CREATE PROCEDURE [sps_ContentForApproval]
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
/****** Object:  StoredProcedure [sps_CourseAssociatedFiles]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================  
-- Author:  <Author,,Name>  
-- Create date: <Create Date,,>  
-- Description: <Description,,>  
-- =============================================  
CREATE PROCEDURE [sps_CourseAssociatedFiles]   
 (  
 @CourseID INT  
 )  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
select * from [CourseAssociatedFiles] WHERE CourseID= @CourseID  
  
  
select * from [ResourceAssociatedFiles] WHERE ResourceId in  
(  
select resourceid from[SectionResource] where sectionid in (select id from [CourseSections] where courseid = @CourseID) )  
  
END  
GO
/****** Object:  StoredProcedure [sps_CourseEnrollmentStatus]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [sps_CourseEnrollmentStatus] 
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
/****** Object:  StoredProcedure [sps_CoursesByKeyword]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- sps_CoursesByKeyword @SearchKeyword ='test'  
CREATE PROCEDURE [sps_CoursesByKeyword]   
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
/****** Object:  StoredProcedure [sps_CoursesByUserId]    Script Date: 12/17/2019 12:42:58 PM ******/
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
CREATE PROCEDURE [sps_CoursesByUserId] 
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
/****** Object:  StoredProcedure [sps_DashboardReportByUserId]    Script Date: 12/17/2019 12:42:58 PM ******/
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
CREATE PROCEDURE [sps_DashboardReportByUserId]
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
/****** Object:  StoredProcedure [sps_EducationalUse]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================  
-- Author:  <Author,,Name>  
-- Create date: <Create Date,,>  
-- Description: <Description,,>  
-- =============================================  
CREATE PROCEDURE [sps_EducationalUse]  
   
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
/****** Object:  StoredProcedure [sps_EducationalUseById]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [sps_EducationalUseById]-- 1
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
/****** Object:  StoredProcedure [sps_EmailNotificationStatus]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [sps_EmailNotificationStatus]
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
/****** Object:  StoredProcedure [sps_GetCategoryByQrcId]    Script Date: 12/17/2019 12:42:58 PM ******/
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
CREATE PROCEDURE [sps_GetCategoryByQrcId] 
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
/****** Object:  StoredProcedure [sps_GetCommunityApproveRejectCount]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [sps_GetCommunityApproveRejectCount]
AS
BEGIN
SELECT ApproveCount,RejectCount,LastUpdatedOn,LastUpdatedBy FROM CommunityApproveRejectCount
RETURN 105
END
GO
/****** Object:  StoredProcedure [sps_GetCommunityCategories]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

     -- sps_GetCommunityCategories   2,1, 10
CREATE PROCEDURE [sps_GetCommunityCategories]   
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
AND c.CategoryId IN (SELECT Id FROM CategoryMaster WHERE [Name] IN (SELECT VALUE FROM STRING_SPLIT(@Subjects,',')))    
AND c.CreatedBy <> @UserId       
UNION     
SELECT c.Id, 2, c.Title, (SELECT Id FROM CategoryMaster WHERE Id = c.CategoryId) FROM ResourceMaster c WHERE (c.IsApproved IS NULL OR (IsApproved = 1 AND IsApprovedSensory = 1)) AND c.IsDraft = 0    
AND c.CategoryId IN (SELECT Id FROM CategoryMaster WHERE [Name] IN (SELECT VALUE FROM STRING_SPLIT(@Subjects,',')))    
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
on t.CategoryId = cm.Id

          
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
/****** Object:  StoredProcedure [sps_GetCommunityCheckList]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

  -- sps_GetCommunityCheckList   2,1,10,1
CREATE PROCEDURE [sps_GetCommunityCheckList]  
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
/****** Object:  StoredProcedure [sps_GetContentCountInfo]    Script Date: 12/17/2019 12:42:58 PM ******/
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
CREATE PROCEDURE [sps_GetContentCountInfo] 
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
/****** Object:  StoredProcedure [sps_GetCourseTest]    Script Date: 12/17/2019 12:42:58 PM ******/
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
CREATE PROCEDURE [sps_GetCourseTest]
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
/****** Object:  StoredProcedure [sps_GetMoEcategories]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
    
CREATE PROCEDURE [sps_GetMoEcategories]  
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
/****** Object:  StoredProcedure [sps_GetMoECheckList]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
    
CREATE PROCEDURE [sps_GetMoECheckList]    
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
/****** Object:  StoredProcedure [sps_GetSensoryCategory]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================  
-- Author:  <Author,,Name>  
-- Create date: <Create Date,,>  
-- Description: <Description,,>  
-- =============================================  
CREATE PROCEDURE [sps_GetSensoryCategory]  -- 2
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
/****** Object:  StoredProcedure [sps_GetSensoryCheckList]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
CREATE PROCEDURE [sps_GetSensoryCheckList] --3    
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
/****** Object:  StoredProcedure [sps_GetUserBookmarkedContent]    Script Date: 12/17/2019 12:42:58 PM ******/
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
CREATE PROCEDURE [sps_GetUserBookmarkedContent]
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
/****** Object:  StoredProcedure [sps_GetUserFavouritesByContentID]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [sps_GetUserFavouritesByContentID]
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
/****** Object:  StoredProcedure [sps_GetUserForQRC]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--exec [sps_GetUserForQRC] @QrcId =22 ,@Category= 74  
CREATE PROCEDURE [sps_GetUserForQRC] 
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
,(SELECT count(ID)  from [ContentApproval] WHERE AssignedTo = qr.UserId AND Status <>'' ) as NoOfReviews   
 from [QRCUserMapping] qr  
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
/****** Object:  StoredProcedure [sps_GetUserNotInQRC]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [sps_GetUserNotInQRC]  
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
,(SELECT count(ID)  from [ContentApproval] WHERE AssignedTo = u.ID AND Status <>'''' ) as NoOfReviews 
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
,(SELECT count(ID)  from [ContentApproval] WHERE AssignedTo = u.ID AND Status <>'''' ) as NoOfReviews 
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
/****** Object:  StoredProcedure [sps_GetVerifierReport]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
CREATE PROCEDURE [sps_GetVerifierReport]
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
/****** Object:  StoredProcedure [sps_lu_Educational_Standard]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
-- =============================================  
-- Author:  <Author,,Name>  
-- Create date: <Create Date,,>  
-- Description: <Description,,>  
-- =============================================  
CREATE PROCEDURE [sps_lu_Educational_Standard]  
   
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
/****** Object:  StoredProcedure [sps_lu_Educational_StandardById]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [sps_lu_Educational_StandardById]-- 1
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
/****** Object:  StoredProcedure [sps_lu_Level]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
CREATE PROCEDURE [sps_lu_Level]  
   
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
/****** Object:  StoredProcedure [sps_lu_LevelById]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [sps_lu_LevelById]-- 1
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
/****** Object:  StoredProcedure [sps_MoEApproved]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [sps_MoEApproved]
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
/****** Object:  StoredProcedure [sps_Notifications]    Script Date: 12/17/2019 12:42:58 PM ******/
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
CREATE PROCEDURE [sps_Notifications]   
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
/****** Object:  StoredProcedure [sps_OerDashboardReport]    Script Date: 12/17/2019 12:42:58 PM ******/
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
CREATE PROCEDURE [sps_OerDashboardReport]     
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
/****** Object:  StoredProcedure [sps_PageContentByPageId]    Script Date: 12/17/2019 12:42:58 PM ******/
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
CREATE PROCEDURE [sps_PageContentByPageId]    
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
/****** Object:  StoredProcedure [sps_Pages]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================    
-- Author:  <Author,,Name>    
-- Create date: <Create Date,,>    
-- Description: <Description,,>    
-- =============================================    
CREATE PROCEDURE [sps_Pages]    
     
AS    
BEGIN    
SELECT Id,    
PageName, PageName_Ar from WebContentPages    
    
RETURN 105    
END 
GO
/****** Object:  StoredProcedure [sps_QRCByUserID]    Script Date: 12/17/2019 12:42:58 PM ******/
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
CREATE PROCEDURE [sps_QRCByUserID]   
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
/****** Object:  StoredProcedure [sps_QRCReport]    Script Date: 12/17/2019 12:42:58 PM ******/
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
CREATE PROCEDURE [sps_QRCReport]
	
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
(SELECT Count(DISTINCT qrcu.UserId) from [QRCMaster] qrc
INNER JOIN [QRCUserMapping]  qrcu
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
from [QRCMaster]	qm
RETURN 105;
END

GO
/****** Object:  StoredProcedure [sps_QRCReportV1]    Script Date: 12/17/2019 12:42:58 PM ******/
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
CREATE PROCEDURE [sps_QRCReportV1]
	
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
from [QRCMaster]		qrc
inner join [QRCUserMapping]  qrcu
on qrc.id = qrcu.qrcid
WHERE qrcu.UserId  IN (SELECT Id FROM UserMaster WHERE Active = 1)
group by qrcu.QRCId,qrc.Name,qrc.id
RETURN 105;
END
GO
/****** Object:  StoredProcedure [sps_RecommendedContent]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
CREATE     PROCEDURE [sps_RecommendedContent] --21 1 24      
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
/****** Object:  StoredProcedure [sps_RecommendedContent_test]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
CREATE     PROCEDURE [sps_RecommendedContent_test] --21 1 24      
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
/****** Object:  StoredProcedure [sps_RemixPreviousVersion]    Script Date: 12/17/2019 12:42:58 PM ******/
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
CREATE PROCEDURE [sps_RemixPreviousVersion]
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
/****** Object:  StoredProcedure [sps_ReportAbuseContent]    Script Date: 12/17/2019 12:42:58 PM ******/
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
CREATE PROCEDURE [sps_ReportAbuseContent]           
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
rc.IsHidden,rc.Reason,rc.IsHidden as IsDeleted,rc.UpdateDate,rm.id as ContentId   from [ResourceComments] rc        
 INNER join resourcemaster rm on rc.ResourceId = rm.id
 and rc.ReportAbuseCount > 0        
 --WHERE rc.IsHidden <> 1         
        
Union all        
        
        
select rc.Id as ID,        
rm.title,rc.ReportAbuseCount,rc.comments as Description,4 as ContentType,rc.IsHidden ,rc.Reason,      
rc.IsHidden as IsDeleted  ,rc.UpdateDate ,rm.id as ContentId     
from [CourseComments] rc INNER join coursemaster rm on rc.CourseID = rm.id        
 --WHERE --rc.IsHidden <> 1 AND      
AND rc.ReportAbuseCount > 0       
     
    
    
 SELECT * FROM #tempReport order by UpdateDate desc    
    
DROP table #tempReport    
 SET @Return = 105;          
          
 RETURN @Return;          
          
END;   


GO
/****** Object:  StoredProcedure [sps_ResourceMasterData]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================          
-- Author:  <Author,,Name>          
-- Create date: <Create Date,,>          
-- Description: <Description,,>          
-- =============================================          
CREATE PROCEDURE [sps_ResourceMasterData]          
          
AS          
BEGIN          
 -- SET NOCOUNT ON added to prevent extra result sets from          
 -- interfering with SELECT statements.          
 SET NOCOUNT ON;          
          
    -- Insert statements for procedure here          
 select Id,[Standard],Standard_Ar from [lu_Educational_Standard] where Active = 1 and Status = 1   
          
select Id,          
EducationalUse,EducationalUse_Ar from [lu_Educational_Use] where Active = 1    and Status = 1       
          
select Id,          
[Level],Level_Ar from [lu_Level] where Active = 1  and Status = 1         
          
        
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
/****** Object:  StoredProcedure [sps_ResourcesByKeyword]    Script Date: 12/17/2019 12:42:58 PM ******/
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
CREATE PROCEDURE [sps_ResourcesByKeyword]  
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
/****** Object:  StoredProcedure [sps_ResourcesByUserId]    Script Date: 12/17/2019 12:42:58 PM ******/
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
CREATE PROCEDURE [sps_ResourcesByUserId] 
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
/****** Object:  StoredProcedure [sps_TestResults]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [sps_TestResults]
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
/****** Object:  StoredProcedure [sps_UserByEmail]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [sps_UserByEmail] -- 'shubhdeep.s@pintlab.com' 
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
/****** Object:  StoredProcedure [sps_UserById]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [sps_UserById]   
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
/****** Object:  StoredProcedure [sps_Users]    Script Date: 12/17/2019 12:42:58 PM ******/
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
CREATE PROCEDURE [sps_Users] 
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
/****** Object:  StoredProcedure [sps_Visiters]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [sps_Visiters]
	
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
/****** Object:  StoredProcedure [spu_Announcements]    Script Date: 12/17/2019 12:42:58 PM ******/
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
CREATE PROCEDURE [spu_Announcements]
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
/****** Object:  StoredProcedure [spu_AppTheme]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [spu_AppTheme]
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
/****** Object:  StoredProcedure [spu_CommunityApproveRejectCount]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [spu_CommunityApproveRejectCount]
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
/****** Object:  StoredProcedure [spu_ContactUs]    Script Date: 12/17/2019 12:42:58 PM ******/
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
CREATE PROCEDURE [spu_ContactUs]
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
/****** Object:  StoredProcedure [spu_ContentStatus]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [spu_ContentStatus]
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
/****** Object:  StoredProcedure [spu_ContentWithdrawal]    Script Date: 12/17/2019 12:42:58 PM ******/
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
CREATE PROCEDURE [spu_ContentWithdrawal]  
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
/****** Object:  StoredProcedure [spu_CourseTest]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spu_CourseTest]
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
/****** Object:  StoredProcedure [spu_EmailNotification]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [spu_EmailNotification]
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
/****** Object:  StoredProcedure [spu_lu_Educational_Standard]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [spu_lu_Educational_Standard]
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
/****** Object:  StoredProcedure [spu_lu_Educational_Use]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [spu_lu_Educational_Use]
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
/****** Object:  StoredProcedure [spu_lu_Level]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--=======================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [spu_lu_Level]
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
/****** Object:  StoredProcedure [spu_Notifications]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [spu_Notifications]
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
/****** Object:  StoredProcedure [spu_QRCUserMapping]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [spu_QRCUserMapping] 
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
/****** Object:  StoredProcedure [spu_UserLastLogin]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [spu_UserLastLogin]
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
/****** Object:  StoredProcedure [spu_UserMasterStatus]    Script Date: 12/17/2019 12:42:58 PM ******/
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
CREATE PROCEDURE [spu_UserMasterStatus]
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
/****** Object:  StoredProcedure [spu_WebPageContent]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================    
-- Author:  <Author,,Name>    
-- Create date: <Create Date,,>    
-- Description: <Description,,>    
-- =============================================    
CREATE PROCEDURE [spu_WebPageContent]    
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
/****** Object:  StoredProcedure [UpdateCategory]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [UpdateCategory] 
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
/****** Object:  StoredProcedure [UpdateCopyright]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
      
-- exec UpdateCopyright @Id=1006,@Title='testsdfdf',@Description='dfsfds',@UpdatedBy=84,@Active=1,@Media='x3d.png'      
CREATE PROCEDURE [UpdateCopyright]       
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
/****** Object:  StoredProcedure [UpdateCourse]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [UpdateCourse]          
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
        
INSERT INTO [SectionResource](ResourceId,        
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
/****** Object:  StoredProcedure [UpdateCourseComment]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
CREATE PROCEDURE [UpdateCourseComment]      
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
/****** Object:  StoredProcedure [UpdateEducation]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE  PROCEDURE [UpdateEducation] 
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
/****** Object:  StoredProcedure [UpdateInstitution]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [UpdateInstitution] 
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
/****** Object:  StoredProcedure [UpdateMaterialType]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [UpdateMaterialType] 
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
/****** Object:  StoredProcedure [UpdateProfession]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [UpdateProfession] 
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
/****** Object:  StoredProcedure [UpdateQRC]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [UpdateQRC] 
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
/****** Object:  StoredProcedure [UpdateResource]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [UpdateResource]      
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
/****** Object:  StoredProcedure [UpdateResourceComment]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
CREATE PROCEDURE [UpdateResourceComment]      
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
/****** Object:  StoredProcedure [UpdateRole]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [UpdateRole]		  
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
/****** Object:  StoredProcedure [UpdateStream]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [UpdateStream] 
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
/****** Object:  StoredProcedure [UpdateSubCategory]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [UpdateSubCategory] 
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
/****** Object:  StoredProcedure [UpdateUserProfile]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [UpdateUserProfile]     
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
/****** Object:  StoredProcedure [UpdateUserRole]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE  PROCEDURE [UpdateUserRole]          
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
/****** Object:  StoredProcedure [UrlIsWhitelisted]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [UrlIsWhitelisted] 
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
/****** Object:  StoredProcedure [USP_Insert_QRCUsers]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
  
  
  
CREATE PROCEDURE [USP_Insert_QRCUsers](  
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
/****** Object:  StoredProcedure [VerifyWhiteListingRequest]    Script Date: 12/17/2019 12:42:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [VerifyWhiteListingRequest]     
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
ALTER DATABASE [oeruat] SET  READ_WRITE 
GO
