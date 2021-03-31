CREATE TYPE [UT_AnswerOptions] AS TABLE(
	[QuestionId] [int] NOT NULL,
	[AnswerId] [int] NOT NULL,
	[OptionText] [nvarchar](max) NULL,
	[CorrectOption] [bit] NULL
)
GO
CREATE TYPE [UT_QRCUsers] AS TABLE(
	[QRCId] [int] NULL,
	[CategoryId] [int] NULL,
	[UserId] [int] NULL,
	[CreatedBy] [int] NULL
)
GO
CREATE TYPE [UT_Questions] AS TABLE(
	[QuestionId] [int] NOT NULL,
	[QuestionText] [nvarchar](max) NULL,
	[Media] [nvarchar](200) NULL
)
GO
CREATE TYPE [UT_Resource] AS TABLE(
	[Id] [int] NOT NULL,
	[ResourceId] [int] NOT NULL,
	[SectionId] [int] NOT NULL
)
GO
CREATE TYPE [UT_Sections] AS TABLE(
	[Id] [int] NOT NULL,
	[Name] [nvarchar](100) NULL
)
GO
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
 CONSTRAINT [PK_Announcements] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
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
 CONSTRAINT [PK_CategoryMaster] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
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
 CONSTRAINT [PK_ContentApproval] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
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
 CONSTRAINT [PK_CopyrightMaster] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
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
 CONSTRAINT [PK_CourseAbuseReports] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [CourseAssociatedFiles](
	[Id] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[CourseId] [numeric](18, 0) NOT NULL,
	[AssociatedFile] [nvarchar](50) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_CourseAssociatedFiles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
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
 CONSTRAINT [PK_CourseComments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [CourseMaster](
	[Id] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](250) NOT NULL,
	[CategoryId] [int] NOT NULL,
	[SubCategoryId] [int] NULL,
	[Thumbnail] [nvarchar](50) NULL,
	[CourseDescription] [nvarchar](2000) NULL,
	[Keywords] [nvarchar](1500) NULL,
	[CourseContent] [ntext] NULL,
	[CopyRightId] [int] NULL,
	[IsDraft] [bit] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[IsApproved] [bit] NOT NULL,
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
 CONSTRAINT [PK_CourseMaster] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
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
 CONSTRAINT [PK_EducationMaster] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
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
 CONSTRAINT [PK_InstitutionMaster] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
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
 CONSTRAINT [PK_lu_Educational_Standard] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
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
 CONSTRAINT [PK_lu_Educational_Use] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
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
 CONSTRAINT [PK_lu_Level] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
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
 CONSTRAINT [PK_MaterialTypeMaster] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
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
 CONSTRAINT [PK_ProfessionMaster] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Questions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[QuestionText] [nvarchar](max) NULL,
	[TestId] [int] NULL,
	[Media] [nvarchar](200) NULL,
 CONSTRAINT [PK_Questions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
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
 CONSTRAINT [PK_ResourceAbuseReports] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ResourceAlignmentRating](
	[Id] [decimal](18, 0) IDENTITY(1,1) NOT NULL,
	[ResourceId] [decimal](18, 0) NOT NULL,
	[CategoryId] [int] NULL,
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ResourceAssociatedFiles](
	[Id] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[ResourceId] [numeric](18, 0) NOT NULL,
	[AssociatedFile] [nvarchar](50) NOT NULL,
	[UploadedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_ResourceAssociatedFiles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
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
 CONSTRAINT [PK_ResourceComments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ResourceMaster](
	[Id] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](250) NOT NULL,
	[CategoryId] [int] NOT NULL,
	[SubCategoryId] [int] NULL,
	[Thumbnail] [nvarchar](50) NULL,
	[ResourceDescription] [nvarchar](2000) NULL,
	[Keywords] [nvarchar](1500) NULL,
	[ResourceContent] [ntext] NULL,
	[MaterialTypeId] [int] NOT NULL,
	[CopyRightId] [int] NULL,
	[IsDraft] [bit] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[IsApproved] [bit] NOT NULL,
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
 CONSTRAINT [PK_ResourceMaster] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
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
 CONSTRAINT [PK_SocialMediaMaster] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
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
 CONSTRAINT [PK_SubCategoryMaster] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
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
	[Photo] [nvarchar](50) NULL,
	[ProfileDescription] [nvarchar](4000) NULL,
	[SubjectsInterested] [nvarchar](500) NULL,
	[ApprovalStatus] [bit] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[UpdatedOn] [datetime] NULL,
	[Active] [bit] NOT NULL,
	[IsContributor] [bit] NULL,
	[IsAdmin] [bit] NULL,
	[IsEmailNotification] [bit] NULL,
	[LastLogin] [datetime] NULL,
 CONSTRAINT [PK_AdminUser] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
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
	[PageContent_Ar] [nvarchar](1000) NULL,
 CONSTRAINT [PK_WebPageContent] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
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
ALTER TABLE [CopyrightMaster] ADD  CONSTRAINT [DF_CopyrightMaster_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [CopyrightMaster] ADD  CONSTRAINT [DF_CopyrightMaster_UpdatedOn]  DEFAULT (getdate()) FOR [UpdatedOn]
GO
ALTER TABLE [CopyrightMaster] ADD  CONSTRAINT [DF_CopyrightMaster_Active]  DEFAULT ((0)) FOR [Active]
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
ALTER TABLE [MaterialTypeMaster] ADD  CONSTRAINT [DF_MaterialTypeMaster_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [MaterialTypeMaster] ADD  CONSTRAINT [DF_MaterialTypeMaster_UpdatedOn]  DEFAULT (getdate()) FOR [UpdatedOn]
GO
ALTER TABLE [MaterialTypeMaster] ADD  CONSTRAINT [DF_MaterialTypeMaster_Active]  DEFAULT ((0)) FOR [Active]
GO
ALTER TABLE [ProfessionMaster] ADD  CONSTRAINT [DF_ProfessionMaster_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [ProfessionMaster] ADD  CONSTRAINT [DF_ProfessionMaster_UpdatedOn]  DEFAULT (getdate()) FOR [UpdatedOn]
GO
ALTER TABLE [ProfessionMaster] ADD  CONSTRAINT [DF_ProfessionMaster_Active]  DEFAULT ((0)) FOR [Active]
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
ALTER TABLE [ResourceRating] ADD  CONSTRAINT [DF_ResourceRating_Rating]  DEFAULT ((0)) FOR [Rating]
GO
ALTER TABLE [ResourceRating] ADD  CONSTRAINT [DF_ResourceRating_RatedOn]  DEFAULT (getdate()) FOR [RatedOn]
GO
ALTER TABLE [ResourceURLReferences] ADD  CONSTRAINT [DF_ResourceURLReferences_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
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
ALTER TABLE [UserSocialMedia] ADD  CONSTRAINT [DF_UserSocialMedia_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [WhiteListingURLs] ADD  CONSTRAINT [DF_URLWhiteListing_IsApproved]  DEFAULT ((0)) FOR [IsApproved]
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
ALTER TABLE [ContactUs]  WITH CHECK ADD  CONSTRAINT [FK_ContactUs_UserMaster] FOREIGN KEY([RepliedBy])
REFERENCES [UserMaster] ([Id])
GO
ALTER TABLE [ContactUs] CHECK CONSTRAINT [FK_ContactUs_UserMaster]
GO
ALTER TABLE [ContentApproval]  WITH CHECK ADD  CONSTRAINT [FK_ContentApproval_UserMaster] FOREIGN KEY([CreatedBy])
REFERENCES [UserMaster] ([Id])
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

		INSERT INTO CourseComments(CourseId,Comments,UserId, CommentDate)
		VALUES (@CourseId,@Comments,@CommentedBy, GETDATE())	

		IF @@IDENTITY>0
		SET @return =100 -- creation success

		ELSE
		SET @return =107
		


		RETURN @return
END

GO
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

		INSERT INTO ResourceComments(ResourceId,Comments,UserId, CommentDate)
		VALUES (@ResourceId,@Comments,@CommentedBy, GETDATE())	

		IF @@IDENTITY>0
		SET @return =100 -- creation success

		ELSE
		SET @return =107
		


		RETURN @return
END

GO
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
IF NOT EXISTS (SELECT * FROM CategoryMaster WHERE Name=@Name)
BEGIN	
		INSERT INTO CategoryMaster (Name,CreatedBy,CreatedOn, UpdatedBy,UpdatedOn,Name_Ar)
		VALUES (@Name,@CreatedBy,GETDATE(),@CreatedBy,GETDATE(),@Name_Ar)
		
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
		 inner join UserMaster l on cm.UpdatedBy =l.Id order by cm.Id desc 	
		 RETURN @return
END

GO
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
	@Media NVARCHAR(100)=NULL
AS
BEGIN
Declare @return INT
IF NOT EXISTS (SELECT * FROM CopyrightMaster WHERE Title=@Title)
BEGIN	

		INSERT INTO CopyrightMaster (Title,Description,Title_Ar,Description_Ar, CreatedBy,CreatedOn, UpdatedBy,UpdatedOn,Media)
		VALUES (@Title, @Description,@Title_Ar,@Description_Ar,@CreatedBy,GETDATE(),@CreatedBy,GETDATE(),@Media)
		
		-- do log entry here	

		SET @return = 100 -- creation success
END

ELSE
	BEGIN
		SET @return = 105 -- Record exists
	END

		SELECT	cr.Id,
				cr.Title,
				Cr.Description,
				cr.Title_Ar,
				cr.Description_Ar,
				cr.CreatedOn,
				CONCAT(c.FirstName, ' ', c.LastName) as CreatedBy,
				CONCAT(l.FirstName, ' ', l.LastName) as UpdatedBy, cr.Active, cr.UpdatedOn,Media
		 from CopyrightMaster cr 
		 inner join UserMaster c  on cr.CreatedBy= c.Id
		 inner join UserMaster l on cr.UpdatedBy =l.Id	order by cr.Id desc	

		 RETURN  @return

END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
  
  
   
  
CREATE PROCEDURE [CreateCourse]   
(  
		   @Title NVARCHAR(250),  
           @CategoryId INT,  
           @SubCategoryId INT=NULL,  
           @Thumbnail  NVARCHAR(50),  
           @CourseDescription  NVARCHAR(2000),  
           @Keywords  NVARCHAR(1500),  
           @CourseContent NTEXT,            
           @CopyRightId INT,  
           @IsDraft BIT,  
		   @CreatedBy INT,  
		   @EducationId int= NULL,  
           @ProfessionId int=NULL,    
			@References NVARCHAR(MAX)=null,    
			@CourseFiles NVARCHAR(MAX)=null,
			@ReadingTime INT = NULL,  
			@UT_Sections [UT_Sections] Readonly,  
			@UT_Resource [UT_Resource] Readonly  
     )  
AS  
BEGIN  
  
DECLARE @Id INT  
Declare @return INT  
DECLARE @SectionsCount INT;  
  
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
			ReadingTime  
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
		   @ReadingTime  
     )  
  
 SET @Id=SCOPE_IDENTITY();  
  
 IF @Id>0  
 BEGIN  
 SET @return =100 -- creation success  
   
   
DECLARE @TotalCount INT;  
DECLARE @QrcID INT;  
DECLARE @RecordId INT;  
DECLARE @SectionId INT;  
DECLARE @i INT;  
SET @i = 1;  
  

  
select @TotalCount = count(*) from QRCCategory WHERE CategoryID = @CategoryID AND IsAvailable =0 --order by CreatedOn asc  
  
IF(@TotalCount>0)  
BEGIN  
   select TOP 1 @RecordId=ID,@QrcID=QRCId from QRCCategory WHERE CategoryID = @CategoryID AND IsAvailable =0 order by CreatedOn asc  
  
   Update QRCCategory SET IsAvailable = 1 WHERE ID = @RecordId  
  
   IF EXISTS(  
   SELECT   
     TOP 1 1  
     from QRCusermapping  where QRCID =@QrcID and active = 1)  
     BEGIN  
       INSERT INTO ContentApproval(ContentId,  
       ContentType,  
       CreatedBy,  
       CreatedOn,  
       AssignedTo,  
       AssignedDate)  
  
       SELECT   
       @Id,  
       1, -- Course  
       @CreatedBy,  
       GETDATE(),  
       userid,  
       GETDATE()  
       from QRCusermapping  where QRCID =@QrcID and active = 1  
      END  
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
      
  SELECT @Id,AssociatedFile,GETDATE() FROM    
   OPENJSON ( @CourseFiles )    
  WITH (     
              AssociatedFile   varchar(50) '$.AssociatedFile'   
  )   
  
 END  
  
  
  SELECT Id,  
TitleId,  
FirstName +' ' +LastName as UserName,  
Email FROM UserMaster WHERE ID in (  
 SELECT   
userid  
from QRCusermapping  where QRCID in  (select  QRCID from QRCCategory WHERE CategoryId = @CategoryId) and active = 1) AND   
IsEmailNotification = 1  
  
--SELECT * from Coursesections WHERE CourseId = @Id  
  
--SELECT rm.id,rm.title,sr.SectionId FROM SectionResource sr  
--INNER JOIN ResourceMaster rm  
--ON sr.resourceid = rm.id  
  
--WHERE sr.SectionId in (SELECT ID from Coursesections WHERE CourseId = @Id)  
  
  
 exec GetCourseById  @Id  
  
   
 END  
  
 ELSE SET @return= 107   
  
    RETURN @return  
END  
  
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [CreateEducation] 
	@Name NVARCHAR(150),
	@CreatedBy INT
AS
BEGIN
Declare @return INT
IF NOT EXISTS (SELECT * FROM EducationMaster WHERE Name=@Name)
BEGIN	
		INSERT INTO EducationMaster (Name,CreatedBy,CreatedOn, UpdatedBy,UpdatedOn)
		VALUES (@Name,@CreatedBy,GETDATE(),@CreatedBy,GETDATE())
		
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
				em.UpdatedOn, em.Active
		 from EducationMaster em 
		 inner join UserMaster c  on em.CreatedBy= c.Id
		 inner join UserMaster l on em.UpdatedBy =l.Id	Order by em.Id desc 	
		 
		 RETURN @return	
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [CreateInstitution] 
	@Name NVARCHAR(255),
	@CreatedBy INT
AS
BEGIN
Declare @return INT
IF NOT EXISTS (SELECT * FROM InstitutionMaster WHERE Name=@Name)
BEGIN	
		INSERT INTO InstitutionMaster (Name,CreatedBy,CreatedOn,UpdatedBy,UpdatedOn)
		VALUES (@Name,@CreatedBy,GETDATE(),@CreatedBy,GETDATE())
		
		-- do log entry here
		
		SET	@return= 100 -- creation success
END

ELSE
	BEGIN
		SET	@return=105 -- Record exists
	END

	SELECT	cr.Id,
				cr.Name,
				cr.CreatedOn,				
				CONCAT(c.FirstName, ' ', c.LastName) as CreatedBy,
				CONCAT(l.FirstName, ' ', l.LastName) as UpdatedBy,
				cr.UpdatedOn, cr.Active
		 from InstitutionMaster cr 
		 inner join UserMaster c  on cr.CreatedBy= c.Id
		 inner join UserMaster l on cr.UpdatedBy =l.Id Order by cr.Id desc 	
		 
		 RETURN @return	
END

GO
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
IF NOT EXISTS (SELECT * FROM MaterialTypeMaster WHERE Name=@Name)
BEGIN	
		INSERT INTO MaterialTypeMaster (Name,Name_Ar,CreatedBy,CreatedOn, UpdatedBy,UpdatedOn)
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
			 CONCAT(c.FirstName, ' ', c.LastName) as CreatedBy,
				CONCAT(l.FirstName, ' ', l.LastName) as UpdatedBy,
				cm.UpdatedOn, cm.Active
		 from MaterialTypeMaster cm 
		 inner join UserMaster c  on cm.CreatedBy= c.Id
		 inner join UserMaster l on cm.UpdatedBy =l.Id order by cm.Id desc 	
		 RETURN @return
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [CreateProfession] 
	@Name NVARCHAR(200),
	@CreatedBy INT
AS
BEGIN
Declare @return INT
IF NOT EXISTS (SELECT * FROM ProfessionMaster WHERE Name=@Name)
BEGIN	
		INSERT INTO ProfessionMaster (Name,CreatedBy,CreatedOn, UpdatedBy,UpdatedOn)
		VALUES (@Name,@CreatedBy,GETDATE(),@CreatedBy,GETDATE())
		
		-- do log entry here

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
				cm.UpdatedOn, cm.Active
		 from ProfessionMaster cm 
		 inner join UserMaster c  on cm.CreatedBy= c.Id
		 inner join UserMaster l on cm.UpdatedBy =l.Id order by cm.Id desc 	
		 RETURN @return
END

GO
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
  INSERT INTO QRCMaster (Name,Description,CreatedBy,CreatedOn, UpdatedBy,UpdatedOn)    
  VALUES (@Name,@Description,@CreatedBy,GETDATE(),@CreatedBy,GETDATE())    
    
    
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [CreateResource]   
(  
     @Title NVARCHAR(250),  
           @CategoryId INT,  
           @SubCategoryId INT=NULL,  
           @Thumbnail  NVARCHAR(50) = NULL,  
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
     @Objective NVARCHAR(100) = NULL  
       
     )  
AS  
BEGIN  
  
DECLARE @Id INT  
Declare @return INT  
  
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
     [Objective]  
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
     @Objective  
     )  
  
 SET @Id=SCOPE_IDENTITY();  
  
   
DECLARE @TotalCount INT;  
DECLARE @QrcID INT;  
DECLARE @RecordId INT;  
  
--select top 10 * from QRCCategory where CategoryId =@CategoryID  
--select * from QRCCategory WHERE CategoryID = @CategoryID AND IsAvailable =0 order by CreatedOn asc  
  
select @TotalCount = count(*) from QRCCategory WHERE CategoryID = @CategoryID AND IsAvailable =0 --order by CreatedOn asc  
  
IF(@TotalCount>0)  
BEGIN  
select TOP 1 @RecordId=ID,@QrcID=QRCId from QRCCategory WHERE CategoryID = @CategoryID AND IsAvailable =0 order by CreatedOn asc  
  
Update QRCCategory SET IsAvailable = 1 WHERE ID = @RecordId  
  
IF EXISTS(  
SELECT   
TOP 1 1  
from QRCusermapping  where QRCID =@QrcID and active = 1)  
BEGIN  
INSERT INTO ContentApproval(ContentId,  
ContentType,  
CreatedBy,  
CreatedOn,  
AssignedTo,  
AssignedDate)  
  
SELECT   
@Id,  
2, -- Resource  
@CreatedBy,  
GETDATE(),  
userid,  
GETDATE()  
from QRCusermapping  where QRCID =@QrcID and active = 1  
  
END  
  
  
  
END  
  
IF(@TotalCount=1)  
BEGIN  
Update QRCCategory SET IsAvailable = 0  WHERE CategoryID = @CategoryID   
END  
  
  
--select top 10 * from QRCCategory where CategoryId =@CategoryID  
   
  
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
      
  SELECT @Id,AssociatedFile,GETDATE() FROM    
   OPENJSON ( @ResourceFiles )    
  WITH (     
              AssociatedFile   varchar(50) '$.AssociatedFile'   
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
  
 END;  
 SELECT Id,  
TitleId,  
FirstName +' ' +LastName as UserName,  
Email FROM UserMaster WHERE ID in (  
 SELECT   
userid  
from QRCusermapping  where QRCID in  (select  QRCID from QRCCategory WHERE CategoryId = @CategoryId) and active = 1)  
AND   
IsEmailNotification = 1  
 exec GetResourceById  @Id  
 END  
  
 ELSE SET @return= 107   
  
    RETURN @return  
END  
  
GO
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
IF NOT EXISTS (SELECT * FROM SubCategoryMaster WHERE Name=@Name AND CategoryId=@CategoryId)
BEGIN	
		INSERT INTO SubCategoryMaster (Name,Name_Ar,CategoryId, CreatedBy,CreatedOn, UpdatedBy,UpdatedOn)
		VALUES (@Name,@Name_Ar,@CategoryId,@CreatedBy,GETDATE(),@CreatedBy,GETDATE())
		
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
		 inner join UserMaster l on sm.UpdatedBy =l.Id order by cm.Id desc 	
		 RETURN @return
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- CreateUserInitialProfile @FirstName='Prince',@LastName='Kumar',@Email='test@gmail.com',@IsContributor=1,@IsAdmin=1

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
	 CreatedOn)      
      
    VALUES(      
            @FirstName                 
           ,@LastName                
           ,@Email       
           ,@IsContributor,    
     1,      
      @IsAdmin,
	  GETDATE())      
      
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
           @Photo nvarchar(50)=NULL,  
           @ProfileDescription nvarchar(4000)=NULL,  
           @SubjectsInterested nvarchar(500)=NULL,                 
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
              CertificationName   varchar(250) '$.CertificationName',                
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
              UniversitySchool   varchar(250) '$.UniversitySchool',                
              Major varchar(100)          '$.Major',    
     Grade varchar(10)          '$.Grade',   
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
     OrganizationName   varchar(250) '$.OrganizationName',                
              Designation varchar(250)          '$.Designation',  
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
 Language varchar(250),  
 LanguageId int,  
 IsRead bit,   
 IsWrite bit,  
 IsSpeak bit  
)  
  
INSERT INTO #tempUserLangs  
      
SELECT Language,0,IsRead,IsWrite,IsSpeak FROM    
 OPENJSON ( @UserLanguages )    
WITH (     
     Language   varchar(250) '$.Language',                
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [CreateWhiteListingRequest] 
	@URL NVARCHAR(1000),
	@RequestedBy INT
AS
BEGIN
Declare @return INT
IF NOT EXISTS (SELECT * FROM WhiteListingURLs WHERE URL=@URL)
BEGIN	
		INSERT INTO WhiteListingURLs (URL,RequestedBy,RequestedOn)
		VALUES (@URL,@RequestedBy,GETDATE())
		
		-- exec CreateLogEntry @LogModuleId=1,@UserId=@CreatedBy,@ActionId=1,@ActionDetail=''

		SET @return =100 -- creation success
END

ELSE
	BEGIN	
		SET @return = 105 -- Record exists
	END	

	return @return
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [DeleteCategory] 
(
@Id	INT
)
AS
BEGIN	
Declare @return INT
		IF EXISTS (SELECT * FROM CategoryMaster  WHERE Id=@Id)
		BEGIN
		
			DELETE FROM CategoryMaster  WHERE Id=@Id and Active=0;
		
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
		 from CategoryMaster cm 
		 inner join UserMaster c  on cm.CreatedBy= c.Id
		 inner join UserMaster l on cm.UpdatedBy =l.Id 	
		  order by cm.Id desc 	
		  RETURN @return
		
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [DeleteCopyright] 
(
@Id	INT
)
AS
BEGIN	
Declare @return INT
		IF EXISTS (SELECT * FROM CopyrightMaster  WHERE Id=@Id)
		BEGIN
		
			DELETE FROM CopyrightMaster  WHERE Id=@Id and Active=0;
		
				IF @@ROWCOUNT>0		
		
					SET @return =103 -- record DELETED		
				ELSE		
					SET @return = 104 -- trying active record deletion			
		END

		ELSE
		SET @return = 102 -- reconrd does not exists
		
		SELECT	cr.Id,
				cr.Title,
				Cr.[Description],
				cr.CreatedOn,
				CONCAT(c.FirstName, '', c.LastName) as CreatedBy,
				CONCAT(l.FirstName, '', l.LastName) as UpdatedBy, cr.Active, cr.UpdatedOn
		 from CopyrightMaster cr 
		 inner join UserMaster c  on cr.CreatedBy= c.Id
		 inner join UserMaster l on cr.UpdatedBy =l.Id	order by cr.Id desc	
		  RETURN @return
		
END

GO
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

			Delete from CourseURLReferences WHERE CourseId=@Id

			DELETE FROM CourseAssociatedFiles WHERE CourseId=@Id

			DELETE FROM CourseMaster  WHERE Id=@Id;
		
				IF @@ERROR<>0		
				
				SET @return =114 -- record deletion	failed						
				
				ELSE	
				
				SET @return =103 -- record DELETED		
				
					
			END	

			ELSE

			SET @return = 104 -- trying active record deletion			

		END

	    ELSE 

		SET @return = 102 -- reconrd does not exists	
			
	    RETURN @return
END

GO
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [DeleteEducation] 
(
@Id	INT
)
AS
BEGIN	
Declare @return INT
		IF EXISTS (SELECT * FROM EducationMaster  WHERE Id=@Id)
		BEGIN
		
			DELETE FROM EducationMaster  WHERE Id=@Id and Active=0;
		
				IF @@ROWCOUNT>0		
		
					SET @return =103 -- record DELETED		
				ELSE		
					SET @return = 104 -- trying active record deletion			
		END

		ELSE
		SET @return = 102 -- reconrd does not exists
		
		SELECT	em.Id,
				em.Name,
				em.CreatedOn,				
				c.FirstName,
				 CONCAT(c.FirstName, '', c.LastName) as CreatedBy,
				CONCAT(l.FirstName, '', l.LastName) as UpdatedBy,
				em.UpdatedOn, em.Active
		 from EducationMaster em 
		 inner join UserMaster c  on em.CreatedBy= c.Id
		 inner join UserMaster l on em.UpdatedBy =l.Id Order by em.Id desc 
		RETURN @return
END

GO
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [DeleteMaterialType] 
(
@Id	INT
)
AS
BEGIN	
Declare @return INT
		IF EXISTS (SELECT * FROM MaterialTypeMaster  WHERE Id=@Id)
		BEGIN
		
			DELETE FROM MaterialTypeMaster  WHERE Id=@Id and Active=0;
		
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
		 from MaterialTypeMaster cm 
		 inner join UserMaster c  on cm.CreatedBy= c.Id
		 inner join UserMaster l on cm.UpdatedBy =l.Id 	
		  order by cm.Id desc 	
		  RETURN @return
		
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [DeleteProfession] 
(
@Id	INT
)
AS
BEGIN	
Declare @return INT
		IF EXISTS (SELECT * FROM ProfessionMaster  WHERE Id=@Id)
		BEGIN
		
			DELETE FROM ProfessionMaster  WHERE Id=@Id and Active=0;
		
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
		 from ProfessionMaster cm 
		 inner join UserMaster c  on cm.CreatedBy= c.Id
		 inner join UserMaster l on cm.UpdatedBy =l.Id 	
		 order by cm.Id desc 
		  RETURN @return
		
END

GO
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
		IF NOT EXISTS (select * from ResourceApprovals where ResourceId=@Id)

			BEGIN

			Delete from ResourceURLReferences WHERE ResourceId=@Id

			DELETE FROM ResourceAssociatedFiles WHERE ResourceId=@Id

			DELETE FROM ResourceMaster  WHERE Id=@Id;
		
				IF @@ERROR<>0		
				
				SET @return =114 -- record deletion	failed						
				
				ELSE	
				
				SET @return =103 -- record DELETED		
				
					
			END	

			ELSE

			SET @return = 104 -- trying active record deletion			

		END

	    ELSE 

		SET @return = 102 -- reconrd does not exists	
			
	    RETURN @return
END

GO
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [DeleteSubCategory] 
(
@Id	INT
)
AS
BEGIN	
Declare @return INT
		IF EXISTS (SELECT * FROM SubCategoryMaster  WHERE Id=@Id)
		BEGIN
		
			DELETE FROM SubCategoryMaster  WHERE Id=@Id and Active=0;
		
				IF @@ROWCOUNT>0		
		
					SET @return =103 -- record DELETED		
				ELSE		
					SET @return = 104 -- trying active record deletion			
		END

		ELSE
		SET @return = 102 -- reconrd does not exists
		
		SELECT	sm.Id,
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
		 inner join UserMaster l on sm.UpdatedBy =l.Id order by cm.Id desc 	
		 RETURN @return
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [GetCategory] 	
AS
BEGIN		
	IF Exists(select * from CategoryMaster)	
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
		 order by cm.Id desc 
		 
		RETURN 105 -- record exists
	end
		ELSE
		RETURN 102 -- reconrd does not exists
END

GO
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [GetCopyright] 	
AS
BEGIN		
		IF Exists(select * from CopyrightMaster)
		BEGIN	
		SELECT	cr.Id,
				cr.Title,
				Cr.Description,
				cr.Title_Ar,
				Cr.Description_Ar,
				cr.CreatedOn,
				CONCAT(c.FirstName, '', c.LastName) as CreatedBy,
				CONCAT(l.FirstName, '', l.LastName) as UpdatedBy, cr.Active, cr.UpdatedOn, cr.Media
		 from CopyrightMaster cr 
		 inner join UserMaster c  on cr.CreatedBy= c.Id
		 inner join UserMaster l on cr.UpdatedBy =l.Id		
		 order by cr.Id desc

		RETURN 105 -- record exists
		END
		ELSE
		RETURN 102 -- record does not exists
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [GetCopyrightById] 
(
@Id	INT
)
AS
BEGIN		
		
		IF Exists(select * from CopyrightMaster  WHERE Id=@Id)
		 BEGIN	
		SELECT	cr.Id,
				cr.Title,
				Cr.Description,
				cr.Title_Ar,
				Cr.Description_Ar,
				cr.CreatedOn,
				CONCAT(c.FirstName, '', c.LastName) as CreatedBy,
				CONCAT(l.FirstName, '', l.LastName) as UpdatedBy, cr.Active, cr.UpdatedOn,cr.Media
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
  
IF Exists(select TOP 1 1 from CourseMaster WHERE Id=@Id)       
 BEGIN      
   
 SET @SharedCount = (select count(Id) from ContentSharedInfo WHERE ContentId = @Id AND ContentTypeId = 1)  
 SET @DownloadCount = (select count(Id) from ContentDownloadInfo WHERE ContentId = @Id AND ContentTypeId = 1)  
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
   LastView
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
      ,caf.CreatedOn      
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
      
SELECT rm.id,rm.title,sr.SectionId FROM SectionResource sr      
INNER JOIN ResourceMaster rm      
ON sr.resourceid = rm.id      
      
WHERE sr.SectionId in (SELECT ID from Coursesections WHERE CourseId = @Id)      
    
 return 105 -- record exists      
      
 end      
  ELSE      
 return 102 -- reconrd does not exists      
END 


GO
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
   (SELECT COUNT(*) FROM ContentDownloadInfo WHERE ContentId = r.Id AND ContentTypeId = 1) AS DownloadCount
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [GetEducation] 	
AS
BEGIN		
	IF Exists(select * from EducationMaster)	
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
		 inner join UserMaster l on em.UpdatedBy =l.Id	Order by em.Id desc 
	 
		RETURN 105 -- record exists
	end
		ELSE
		RETURN 102 -- reconrd does not exists
END

GO
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [GetInstitution] 	
AS
BEGIN		
	IF Exists(select * from InstitutionMaster)	
	BEGIN	
		SELECT	em.Id,
				em.Name,
				em.CreatedOn,				
				c.FirstName,
				 CONCAT(c.FirstName, '', c.LastName) as CreatedBy,
				CONCAT(l.FirstName, '', l.LastName) as UpdatedBy,
				em.UpdatedOn, em.Active
		 from InstitutionMaster em 
		 inner join UserMaster c  on em.CreatedBy= c.Id
		 inner join UserMaster l on em.UpdatedBy =l.Id	Order by em.Id desc 
	 
		RETURN 105 -- record exists
	end
		ELSE
		RETURN 102 -- reconrd does not exists
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [GetInstitutionById]
(
@Id	INT
)
AS
BEGIN	
IF Exists(select * from InstitutionMaster WHERE Id=@Id)	
	BEGIN	
		SELECT	em.Id,
				em.Name,
				em.CreatedOn,				
				c.FirstName,
				 CONCAT(c.FirstName, '', c.LastName) as CreatedBy,
				CONCAT(l.FirstName, '', l.LastName) as UpdatedBy,
				em.UpdatedOn, em.Active
		 from InstitutionMaster em 
		 inner join UserMaster c  on em.CreatedBy= c.Id
		 inner join UserMaster l on em.UpdatedBy =l.Id where em.Id=@Id	Order by em.Id desc 
	 		 
		return 105 -- record exists
	end
		ELSE
		return 102 -- reconrd does not exists
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [GetMaterialType] 	
AS
BEGIN		
	IF Exists(select * from MaterialTypeMaster)	
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
		 order by cm.Id desc 
		 
		RETURN 105 -- record exists
	end
		ELSE
		RETURN 102 -- reconrd does not exists
END

GO
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [GetProfession] 	
AS
BEGIN		
	IF Exists(select * from ProfessionMaster)	
	BEGIN	
		SELECT	cm.Id,
				cm.Name,
				cm.CreatedOn,				
			 CONCAT(c.FirstName, '', c.LastName) as CreatedBy,
				CONCAT(l.FirstName, '', l.LastName) as UpdatedBy,
				cm.UpdatedOn, cm.Active
		 from ProfessionMaster cm 
		 inner join UserMaster c  on cm.CreatedBy= c.Id
		 inner join UserMaster l on cm.UpdatedBy =l.Id 	
		 order by cm.Id desc 
		 
		RETURN 105 -- record exists
	end
		ELSE
		RETURN 102 -- reconrd does not exists
END

GO
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

Select Id, Name FROM SocialMediaMaster 

IF @@ROWCOUNT>0 
RETURN 105

END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [GetQRC] 	
AS
BEGIN		
	IF Exists(select * from QRCMaster)	
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
		 order by cm.Id desc 
		 
		RETURN 105 -- record exists
	end
		ELSE
		RETURN 102 -- reconrd does not exists
END

GO
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
set	@end = @PageNo * @PageSize
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
 [Format]  
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



IF EXISTS(SELECT TOP 1 *  FROM ResourceMaster)
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
SET @IsRemix = 0    
    
IF EXISTS(SELECT TOP 1 1 from ResourceRemixHistory WHERE ResourceRemixedID =@Id)    
BEGIN    
SET @IsRemix= 1    
END    
IF Exists(select TOP 1 1 from ResourceMaster WHERE Id=@Id)     
 BEGIN     
 SET @SharedCount = (select count(Id) from ContentSharedInfo WHERE ContentId = @Id AND ContentTypeId = 2) 
  SET @DownloadCount = (select count(Id) from ContentDownloadInfo WHERE ContentId = @Id AND ContentTypeId = 2) 
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
      ,ReportAbuseCount,ViewCount,AverageReadingTime,@DownloadCount AS DownloadCount,r.ReadingTime  ,  
   es.Standard,  
 eu.EducationalUse,  
 el.Level,  
 Objective, 
 @SharedCount as SharedCount,
 LastView,
 [Format]  
   ,@IsRemix AS IsRemix    
  FROM ResourceMaster r    
   inner join CategoryMaster c on r.CategoryId = c.Id    
   inner join MaterialTypeMaster mt on r.MaterialTypeId=mt.Id    
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
      ,UploadedDate    
  FROM ResourceAssociatedFiles raf INNER JOIN ResourceMaster r ON r.Id=raf.ResourceId AND r.Id=@Id    
    
    
    
   SELECT rur.Id    
      ,ResourceId    
      ,URLReferenceId, uwl.URL as URLReference    
      ,rur.CreatedOn    
  FROM dbo.ResourceURLReferences rur     
  INNER JOIN ResourceMaster r ON r.Id=rur.ResourceId    
  inner join WhiteListingURLs uwl on uwl.Id=rur.URLReferenceId AND r.Id=@Id    
    
   SELECT rc.Id    
      ,[ResourceId]    
      ,[Comments]    
      ,CONCAT(um.FirstName, ' ', um.LastName) as CommentedBy, um.Id As CommentedById, um.Photo as CommentorImage    
      ,[CommentDate]    
      ,[ReportAbuseCount]    
  FROM [dbo].[ResourceComments] rc inner join UserMaster um on rc.UserId=um.Id AND rc.ResourceId=@Id where rc.IsHidden=0  
         
 return 105 -- record exists    
    
 end    
  ELSE    
  return 102 -- reconrd does not exists    
END 


GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [GetStream] 	
AS
BEGIN		
	IF Exists(select * from StreamMaster)	
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
		 order by cm.Id desc 
		 
		RETURN 105 -- record exists
	end
		ELSE
		RETURN 102 -- reconrd does not exists
END

GO
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [GetSubCategory] 	
AS
BEGIN		
	IF Exists(select * from SubCategoryMaster)	
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
		 inner join UserMaster c  on sm.CreatedBy= c.Id
		 inner join UserMaster l on sm.UpdatedBy =l.Id order by cm.Id desc 	
		 
		RETURN 105 -- record exists
	end
		ELSE
		RETURN 102 -- reconrd does not exists
END

GO
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [GetUserProfileByEmail] 
(
@Email	NVARCHAR(250)
)
AS
BEGIN	

IF Exists(select * from UserMaster WHERE EMAIL=@Email)	
	BEGIN	

DECLARE @Id INT

SELECT @Id=Id from UserMaster WHERE Email=@Email

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
      ,um.CountryId, cm.[Name] AS Country
      ,um.StateId, sm.[Name] AS [State]
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
CREATE PROCEDURE [GetWhitelistingRequests]   
  
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
   left join UserMaster l on wlu.VerifiedBy =l.Id  order by wlu.Id desc    
    
  
  IF @@ROWCOUNT >0  
  return 105 -- record exists  
    
  ELSE  
  return 102 -- record does not exists  
END  
  
GO
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
IF NOT EXISTS (SELECT * FROM CourseRating WHERE CourseId=@CourseId AND RatedBy=@RatedBy)
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

			update CourseMaster  SET  Rating = (select SUM(rating)/COUNT(id) from CourseRating ) where id= @CourseId

			SET @return =100 -- creation success

			end

		ELSE SET @return = 107
END

ELSE
		SET @return = 105 -- Record exists
	
		 RETURN @return
END

GO
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
IF NOT EXISTS (SELECT * FROM ResourceRating WHERE ResourceId=@ResourceId AND RatedBy=@RatedBy)
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

			update ResourceMaster  SET  Rating = (select SUM(rating)/COUNT(id) from ResourceRating ) where id= @ResourceId

			SET @return =100 -- creation success

			end

		ELSE SET @return = 107
END

ELSE
		SET @return = 105 -- Record exists
	
		 RETURN @return
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [RateResourceAlignment]   
 @ResourceId NUMERIC(18,0),   
 @CategoryId INT=null,   
 @LevelId INT=NULL,   
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
      , LevelId  
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [ReportCourseComment]		  
		   @Id INT            
AS
BEGIN

Declare @return INT

IF EXISTS (SELECT * FROM CourseComments WHERE @Id=@Id)
BEGIN	
UPDATE CourseComments SET ReportAbuseCount=ReportAbuseCount + 1
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
		INSERT INTO [dbo].[CourseAbuseReports]
           ([CourseId]          
           ,[ReportReasons]
           ,[Comments]
		   ,[ReportedBy])
     VALUES
           (@CourseId,@ReportReasons,@Comments,@ReportedBy)	
		   
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [ReportResourceComment]		  
		   @Id INT            
AS
BEGIN

Declare @return INT

IF EXISTS (SELECT * FROM ResourceComments WHERE @Id=@Id)
BEGIN	
UPDATE ResourceComments SET ReportAbuseCount=ReportAbuseCount + 1
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
		INSERT INTO [dbo].[ResourceCommentsAbuseReports]
           ([ResourceCommentId]          
           ,[ReportReasons]
           ,[Comments]
		   ,[ReportedBy])
     VALUES
           (@ResourceCommentId,@ReportReasons,@Comments,@ReportedBy)	
		   
		   	IF @@ROWCOUNT >0

			begin

			update CourseComments set ReportAbuseCount = ReportAbuseCount + 1 where Id=@ResourceCommentId

			SET @return =100 -- creation success

			end

		ELSE SET @return = 107
END

ELSE
		SET @return = 105 -- Record exists
	
		 return @return
END

GO
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



		INSERT INTO [dbo].[ResourceAbuseReports]
           ([ResourceId]          
           ,[ReportReasons]
           ,[Comments]
		   ,[ReportedBy])
     VALUES
           (@ResourceId,@ReportReasons,@Comments,@ReportedBy)	
		   
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [spd_lu_Educational_Standard]
(
@Id INT
)
AS
BEGIN
Declare @return INT  
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
  IF EXISTS (SELECT TOP 1 1 FROM lu_Educational_Standard  WHERE Id=@Id AND Active = 1)  
  BEGIN  
     UPDATE  lu_Educational_Standard SET Active = 0   WHERE Id=@Id;

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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [spd_lu_Educational_Use]
(
@Id INT
)
AS
BEGIN
Declare @return INT  
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
  IF EXISTS (SELECT TOP 1 1 FROM lu_Educational_Use  WHERE Id=@Id AND Active = 1)  
  BEGIN  
     UPDATE  lu_Educational_Use SET Active = 0   WHERE Id=@Id;

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
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
  IF EXISTS (SELECT TOP 1 1 FROM lu_Level  WHERE Id=@Id AND Active = 1)  
  BEGIN  
     UPDATE  lu_Level SET Active = 0   WHERE Id=@Id;

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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
  
CREATE PROCEDURE [spi_Announcements]  
(  
@Text NVARCHAR(200),  
@CreatedBy INT,
@Active BIT
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
    Active  
    )  
  VALUES(  
    @Text,  
    @CreatedBy,  
    GETDATE(),  
    @CreatedBy,  
    GETDATE(),  
    @Active 
  );  
  
  SET @Id = SCOPE_IDENTITY()  
  
SELECT an.[Text],um.FirstName,um.LastName,an.Active,an.CreatedOn,an.UpdatedOn  
from Announcements an INNER JOIN UserMaster um  
ON an.Createdby = um.Id   
INNER JOIN UserMaster um1  
ON an.UpdatedBy = um1.Id WHERE an.id = @Id  
SET @Return = 100;  
  
RETURN @Return;  
END  
GO
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

--exec spi_ContentApproval @ContentApprovalId=6, @ContentId=34,@ContentType=2,@Status=1,@Comment='2 from my side',@CreatedBy=2
CREATE PROCEDURE [spi_ContentApproval]
    @ContentApprovalId INT,
	@ContentType INT,
	@Status INT,
	@Comment NVARCHAR(500) = NULL,
	@CreatedBy INT,
	@ContentId INT

AS
BEGIN


DECLARE @TotalApproval Decimal;
DECLARE @TotalAssignes Decimal;
DECLARE  @PublishContent INT;

IF  EXISTS(
	SELECT 1 FROM ContentApproval
	WHERE ID = @ContentApprovalId
	--ContentId =@ContentId AND
		  --ContentType = @ContentType 
		 -- [Status] = @Status AND
		 --AND AssignedTo = @CreatedBy
		 )
		  

BEGIN
Declare @return INT
 UPDATE ContentApproval SET 
		[Status]  = @Status,
		Comment = @Comment,
		UpdatedBy = @CreatedBy,
		UpdatedOn = GETDATE(),
		ApprovedBy = @CreatedBy,
		ApprovedDate = GETDATE()
		
		WHERE ID = @ContentApprovalId
		 -- ContentId =@ContentId AND
		 -- ContentType = @ContentType 
		 ---- [Status] = @Status AND
		 --AND AssignedTo = @CreatedBy;
		
		SELECT @TotalApproval = COUNT(Id) FROM ContentApproval
		  WHERE 
		  ContentId = @ContentId AND
		  ContentType = @ContentType AND
		  Status = 1

		SELECT  @TotalAssignes= COUNT(Id) FROM ContentApproval
		  WHERE 
		  ContentId = @ContentId AND
		  ContentType = @ContentType --AND
		--  Status = 1
		  IF(@TotalAssignes/2<=@TotalApproval)
		  BEGIN
		     IF(@ContentType = 1)
			 BEGIN
			 UPDATE CourseMaster SET IsApproved = 1 WHERE ID = @ContentId
			 END;
			 ELSE
			 BEGIN
			 UPDATE ResourceMaster SET IsApproved = 1 WHERE ID = @ContentId
			 END;
			 SET @PublishContent = 1;
		  END;

		  ELSE

		  BEGIN
		  SET @PublishContent = 0;
		  END;
		 	SELECT Id,
TitleId,
FirstName +' ' +LastName as UserName,
Email,@PublishContent as PublishContent

 FROM UserMaster WHERE ID in (SELECT CreatedBy FROM ContentApproval WHERE Id=@ContentApprovalId) AND IsEmailNotification = 1
END;
SET @return = 105 -- reconrd does not exists
	  RETURN @return
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

-- spi_ContentDownloadInfo @ContentId=1,@ContentTypeId=1,@SocialMediaId=3,@CreatedBy=1
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

				RETURN 100;
END

GO
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

-- spi_ContentSharedInfo @ContentId=1,@ContentTypeId=1,@SocialMediaId=3,@CreatedBy=1
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

				RETURN 100;
END

GO
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [spi_CourseTest]
	(
	@CourseId INT,
	@TestName NVARCHAR(100),
	@UT_Questions [UT_Questions] Readonly,
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
Media
)

SELECT QuestionText,@TestID,Media FROM @UT_Questions WHERE QuestionId = @i

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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
	INSERT INTO UserCourseTests
	(
	QuestionId,
AnswerId,
UserId,
CreatedOn,
CourseId
	)
	SELECT QuestionId,AnswerId, @UserId, GETDATE(),@CourseId FROM @UT_AnswerOptions

	RETURN 100;
END
GO
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
	IF NOT EXISTS(SELECT TOP 1 1 FROM lu_Educational_Standard WHERE [Standard] = @Standard AND Active =1)
	BEGIN

INSERT INTO lu_Educational_Standard(
[Standard],
Active,
CreatedBy,
UpdatedBy,
CreatedOn,
UpdatedOn,
Standard_Ar
)
VALUES
(
@Standard,
1,
@CreatedBy,
@CreatedBy,
GETDATE(),
GETDATE(),
@Standard_Ar
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
   WHERE es.Active = 1 AND es.id = @ID

RETURN 100;
	END;
	ELSE
	BEGIN
	RETURN 107;
	END;

END
GO
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
	IF NOT EXISTS(SELECT TOP 1 1 FROM lu_Educational_Use WHERE EducationalUse = @EducationalUse AND Active =1)
	BEGIN

INSERT INTO lu_Educational_Use(
EducationalUse,
EducationalUse_Ar,
CreatedBy,
CreatedOn,
UpdatedOn,
UpdatedBy,
Active
)
VALUES
(
@EducationalUse,
@EducationalUse_Ar,
@CreatedBy,
GETDATE(),
GETDATE(),
@CreatedBy,
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
   WHERE eu.Active = 1 AND eu.id = @ID

RETURN 100;
	END;
	ELSE
	BEGIN
	RETURN 107;
	END;

END
GO
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
	IF NOT EXISTS(SELECT TOP 1 1 FROM lu_Level WHERE [Level] = @Level AND Active =1)
	BEGIN

INSERT INTO lu_Level(
[Level],
[Level_Ar],
CreatedBy,
CreatedOn,
UpdatedOn,
UpdatedBy,
Active
)
VALUES
(
@Level,
@Level_Ar,
@CreatedBy,
GETDATE(),
GETDATE(),
@CreatedBy,
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
   WHERE el.Active = 1 AND el.id = @ID

RETURN 100;
	END;
	ELSE
	BEGIN
	RETURN 107;
	END;

END
GO
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
CONCAT(c.FirstName, '', c.LastName) as CreatedBy,    
CONCAT(l.FirstName, '', l.LastName) as UpdatedBy,   
es.CreatedOn,  
es.UpdatedOn,  
es.Active FROM Announcements  es  
   INNER join UserMaster c  on es.CreatedBy= c.Id    
   INNER join UserMaster l on es.UpdatedBy =l.Id   
  
     return 105   
  
  END  
  
  ELSE  
  BEGIN  
    RETURN 102 -- reconrd does not exists    
  END;  
END  
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


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
  return 105 -- record exists  
  END
ELSE 

BEGIN
	
  return 102 -- reconrd does not exists  
END




END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
 um.FirstName,  
 um.LastName,  
 an.CreatedOn,  
 --UpdatedBy,  
 --UpdatedOn,  
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
ORDER BY CreatedOn  
  
  
 SET @return = 105 -- reconrd does not exists  
   RETURN @return  
END  
GO
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
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
set	@end = @PageNo * @PageSize
    -- Insert statements for procedure here

	;with sqlpaging as (
	SELECT Rownumber = ROW_NUMBER() OVER(ORDER BY CreatedOn desc),
			Id,
			FirstName,
			LastName,
			Email,
			Telephone,
			Subject,
			Message,
			CreatedOn
			from ContactUs WHERE IsReplied is NULL
			
			
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
  
EXEC sps_ContentFileNames 1 ,1  
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
AssociatedFile FROM [dbo].[CourseAssociatedFiles] WHERE courseid =@ContentId  
END  
  
ELSE  
BEGIN  
BEGIN  
select  Id,  
ResourceId as ContentId,  
AssociatedFile from [dbo].[ResourceAssociatedFiles] WHERE resourceid =@ContentId  
END  
END;  
  
 SET @Return = 105;  
  
 RETURN @Return;  
  
END;  
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

--exec sps_ContentForApproval  @UserId=1,@QrcId =4,@CategoryID=2,@PageNo=1,@PageSize=5
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
Declare @return INT
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
print 'both null'
    INSERT INTO #tempData(ContentId,ContentApprovalID,Title,CreatedOn,ContentType,CategoryId)
	SELECT cm.ID as ContentId,ca.Id as ContentApprovalID,cm.Title, cm.CreatedOn,1 As ContentType,cm.CategoryId FROM CourseMaster cm
	INNER JOIN ContentApproval ca ON cm.Id  = ca.ContentId WHERE ca.AssignedTo = @UserId AND ca.[Status] IS NULL
	UNION ALL
	SELECT cm.ID as ContentId,ca.Id as ContentApprovalID,cm.Title, cm.CreatedOn,2 As ContentType,cm.CategoryId  FROM ResourceMaster cm
	INNER JOIN ContentApproval ca ON cm.Id  = ca.ContentId WHERE ca.AssignedTo = @UserId AND ca.[Status] IS NULL
END;

ELSE IF( @QrcId <> '' AND @CategoryID IS NULL )
BEGIN
print 'one null one not'
    INSERT INTO #tempData(ContentId,ContentApprovalID,Title,CreatedOn,ContentType,CategoryId)
    SELECT cm.ID as ContentId,ca.Id as ContentApprovalID,cm.Title, cm.CreatedOn,1 As ContentType,cm.CategoryId FROM CourseMaster cm
	INNER JOIN ContentApproval ca ON cm.Id  = ca.ContentId WHERE ca.AssignedTo = @UserId
	AND cm.CategoryId in(SELECT CategoryId FROM QRCUserMapping WHERE QRCId = @QrcId) AND ca.[Status] IS NULL
	UNION ALL
	SELECT cm.ID as ContentId,ca.Id as ContentApprovalID,cm.Title, cm.CreatedOn,2 As ContentType,cm.CategoryId  FROM ResourceMaster cm
	INNER JOIN ContentApproval ca ON cm.Id  = ca.ContentId WHERE ca.AssignedTo = @UserId
	AND cm.CategoryId in (SELECT CategoryId FROM QRCUserMapping WHERE QRCId = @QrcId) AND ca.[Status] IS NULL
END;

IF(@CategoryID <> '' AND @QrcId <> '')
BEGIN

print 'both not null'
    INSERT INTO #tempData(ContentId,ContentApprovalID,Title,CreatedOn,ContentType,CategoryId)
    SELECT cm.ID as ContentId,ca.Id as ContentApprovalID,cm.Title, cm.CreatedOn,1 As ContentType,cm.CategoryId FROM CourseMaster cm
	INNER JOIN ContentApproval ca ON cm.Id  = ca.ContentId WHERE ca.AssignedTo = @UserId
	AND cm.CategoryId = @CategoryID AND ca.[Status] IS NULL
	UNION ALL
	SELECT cm.ID as ContentId,ca.Id as ContentApprovalID,cm.Title, cm.CreatedOn,2 As ContentType,cm.CategoryId  FROM ResourceMaster cm
	INNER JOIN ContentApproval ca ON cm.Id  = ca.ContentId WHERE ca.AssignedTo = @UserId
	AND cm.CategoryId = @CategoryID AND ca.[Status] IS NULL
END;


;with sqlpaging as (
SELECT 
Rownumber = ROW_NUMBER() OVER(order by  ContentApprovalID desc) ,
* FROM #tempData)
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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

EXEC sps_DashboardReportByUserId 66 
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
SET @CourseToApprove = (select  count(id) from ContentApproval where createdby = @UserId and Contenttype = 1 )
SET @ResourceToApprove =(select  count(id)  from ContentApproval where createdby = @UserId and Contenttype = 2 )

SET @SharedResources = (select  count(id) from ContentSharedInfo where createdby = @UserId and ContentTypeId = 2 )
SET @SharedCourses =(select  count(id)  from ContentSharedInfo where createdby = @UserId and ContentTypeId = 1 )

SET @DownloadedResources = (select  count(id) from ContentDownloadInfo where DownloadedBy = @UserId and ContentTypeId = 2 )
SET @DownloadedCourses =(select  count(id)  from ContentDownloadInfo where DownloadedBy = @UserId and ContentTypeId = 1 )



INSERT INTO #tempData(Id,Title,Description,ContentType,Thumbnail)
SELECT TOP 6 ID,Title,CourseDescription,1,Thumbnail from CourseMaster where createdby=@UserId order by CreatedOn desc

INSERT INTO #tempData(Id,Title,Description,ContentType,Thumbnail)
SELECT TOP 6 ID,Title,ResourceDescription,2,Thumbnail from ResourceMaster where createdby=@UserId order by CreatedOn  desc




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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [sps_EducationalUse]
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF EXISTS(SELECT TOP 1 1 FROM lu_Educational_Use WHERE Active = 1)
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

     return 105 

	 END

	 ELSE
	 BEGIN
	   RETURN 102 -- reconrd does not exists  
	 END;
END
GO
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
Media from Questions WHERE TestId IN (SELECT  Id
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--exec [dbo].[sps_GetUserForQRC] @QrcId =17 ,@Category= 27
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
set	@end = @PageNo * @PageSize


;with sqlpaging as (
SELECT 
Rownumber = ROW_NUMBER() OVER(ORDER BY qr.userid ASC) ,
qr.UserId,u.FirstName + '' + u.LastName as UserName,
u.Email,u.Photo
,COUNT(rm.Id)  as ResourceContributed,
COUNT(c.Id) as CourseCreated
,COUNT(qr.UserId) as CurrentQRCS
,(SELECT count(ID)  from [dbo].[ContentApproval] WHERE AssignedTo = qr.UserId AND Status <>'' ) as NoOfReviews 
 from [dbo].[QRCUserMapping] qr
LEFT JOIN ResourceMaster rm 
ON qr.UserId = rm.CreatedBy
LEFT JOIN UserMaster u
ON qr.UserId = u.Id	
LEFT JOIN CourseMaster c
ON qr.Userid = c.CreatedBy 

WHERE qr.QRCId = @QrcId and qr.CategoryId = @Category and qr.active = 1
group by qr.UserId, u.FirstName,u.LastName,u.Email,u.Photo)


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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--exec [dbo].[sps_GetUserNotInQRC] @QrcId =17 ,@Category= 27,@PageSize=30
CREATE PROCEDURE [sps_GetUserNotInQRC]
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
set	@end = @PageNo * @PageSize


;with sqlpaging as (
SELECT 
Rownumber = ROW_NUMBER() OVER(ORDER BY u.ID ASC) ,
u.ID as UserID,u.FirstName + '' + u.LastName as UserName,
u.Email,
u.Photo
,COUNT(rm.Id)  as ResourceContributed,
COUNT(c.Id) as CourseCreated
,COUNT(qr.UserId) as CurrentQRCS
,(SELECT count(ID)  from [dbo].[ContentApproval] WHERE AssignedTo = u.ID AND Status <>'' ) as NoOfReviews 
 from 
 UserMaster u
left join  
 [dbo].[QRCUserMapping] qr
 on qr.UserId = u.Id	AND qr.QRCId = @QrcId and qr.CategoryId = @Category AND qr.Active = 1
LEFT JOIN ResourceMaster rm 
ON qr.UserId = rm.CreatedBy

LEFT JOIN CourseMaster c
ON qr.Userid = c.CreatedBy 

WHERE  
qr.userid IS NULL
group by u.ID, u.FirstName,u.LastName,u.Email,u.Photo)


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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
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
   WHERE es.Active = 1

     return 105 

	 END

	 ELSE
	 BEGIN
	   RETURN 102 -- reconrd does not exists  
	 END;
END
GO
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create PROCEDURE [sps_lu_Level]
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF EXISTS(SELECT TOP 1 1 FROM lu_Level WHERE Active = 1)
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
     return 105 

	 END

	 ELSE
	 BEGIN
	   RETURN 102 -- reconrd does not exists  
	 END;
END
GO
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
		@Return INT;

SET @Contributors =(SELECT count(*) from usermaster WHERE  IsContributor = 1)
SET @Courses = (SELECT count(*)  from coursemaster )
SET @Resources =(SELECT count(*)  from resourcemaster )

SELECT @Contributors as Contributors ,@Courses as Courses,@Resources as Resources

;WITH TopContributor_with(ID, UserName, CourseCount,Photo) AS
(
SELECT  u.id,u.FirstName+' '+u.LastName as UserName,count(c.id),Photo FROM usermaster u
LEFT JOIN coursemaster c
on u.id = c.createdby

 WHERE  u.IsContributor = 1 

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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

-- sps_QRCByUserID 1
CREATE PROCEDURE [sps_QRCByUserID] 
	(
	@UserID INT
	)
AS
BEGIN
	
	SELECT 
	Id,
Name,
Description,
CAST(CreatedBy AS nvarchar(50)) as CreatedBy,
CreatedOn,
CAST(UpdatedBy AS nvarchar(50)) as UpdatedBy,
UpdatedOn,
Active
	
	 FROM QRCMaster WHERE ID in (	SELECT QRCId FROM QRCUserMapping WHERE UserId = @UserID) 
	RETURN 105
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
  
EXEC sps_RecommendedContent 114   
___________________________________________________________________________________________________*/  
CREATE PROCEDURE [sps_RecommendedContent]   
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
set	@end = @PageNo * @PageSize
 
 IF(@UserId IS NOT NULL)
 BEGIN

 IF NOT EXISTS(SELECT value from  StringSplit((select subjectsinterested  
 from usermaster where id = @UserId and subjectsinterested <>''),','))
BEGIN
  
	

SET @QUery = '
DECLARE @Table TABLE(Id NUMERIC, Title VARCHAR(MAX),Description VARCHAR(MAX),ContentType INT, Thumbnail VARCHAR(MAX),CreatedOn DateTime)
INSERT INTO @Table
SELECT r.Id,r.Title,r.ResourceDescription as Description,2 as ContentType,Thumbnail,r.CreatedOn
	from resourcemaster r LEFT JOIN CategoryMaster cm  
on  
r.categoryid = cm.id  
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
SELECT Rownumber = ROW_NUMBER() OVER(ORDER BY CreatedOn desc),Id,Title,Description,ContentType,Thumbnail 
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
 SET @QUery = 
 'DECLARE @Table TABLE(Id NUMERIC, Title VARCHAR(MAX),Description VARCHAR(MAX),ContentType INT, Thumbnail VARCHAR(MAX))
INSERT INTO @Table
SELECT r.Id,r.Title,r.ResourceDescription as Description,2 as ContentType,Thumbnail 
	from resourcemaster r LEFT JOIN CategoryMaster cm  
on  
r.categoryid = cm.id  
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
SELECT c.Id,c.Title,c.CourseDescription as Description,1 as ContentType,Thumbnail 
	from coursemaster c LEFT JOIN CategoryMaster cm  
on  
c.categoryid = cm.id  
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
SELECT Rownumber = ROW_NUMBER() OVER(ORDER BY ID desc),Id,Title,Description,ContentType,Thumbnail 
	from @Table)
select
 top ('+CONVERT(VARCHAR(MAX),@PageSize)+') *,
 (select max(rownumber) from sqlpaging) as 
 Totalrows
from sqlpaging
where Rownumber between '+CONVERT(VARCHAR(MAX),@start)+' and '+CONVERT(VARCHAR(MAX),@end)+'
Order by Id desc'

END
END
ELSE
BEGIN
	 SET @QUery = '
DECLARE @Table TABLE(Id NUMERIC, Title VARCHAR(MAX),Description VARCHAR(MAX),ContentType INT, Thumbnail VARCHAR(MAX),CreatedOn DateTime)
INSERT INTO @Table
SELECT r.Id,r.Title,r.ResourceDescription as Description,2 as ContentType,Thumbnail,r.CreatedOn
	from resourcemaster r LEFT JOIN CategoryMaster cm  
on  
r.categoryid = cm.id  
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
SELECT Rownumber = ROW_NUMBER() OVER(ORDER BY CreatedOn desc),Id,Title,Description,ContentType,Thumbnail 
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

EXEC sp_executesql @Query   
-- WHERE c.Keywords Like '%'+CAST(@Keyword AS NVARCHAR(100))+'%'  
  
--OR cm.Name Like '%'+CAST(@Keyword AS NVARCHAR(100))+'%'  
  
--OR sbm.Name Like '%'+CAST(@Keyword AS NVARCHAR(100))+'%'  
  
 SET @Return = 105;  
  
 RETURN @Return;  
  
END; 



GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*___________________________________________________________________________________________________  
  
Copyright (c) YYYY-YYYY XYZ Corp. All Rights Reserved  
WorldWide.  
  
$Revision:  $1.0  
$Author:    $ Prince Kumar
&Date       June 02, 2019

Ticket: Ticket URL
  
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


SELECT ID,Title,ReportAbuseCount,CourseDescription as Description,1 as ContentType FROM CourseMaster WHERE ReportAbuseCount>1

union all
SELECT ID,Title,ReportAbuseCount,ResourceDescription as Description,2 as ContentType  FROM ResourceMaster WHERE ReportAbuseCount>1

 SET @Return = 105;

 RETURN @Return;

END;
GO
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
 select Id,[Standard] from [dbo].[lu_Educational_Standard]      
      
select Id,      
EducationalUse from [dbo].[lu_Educational_Use]      
      
select Id,      
[Level] from [dbo].[lu_Level]      
      
    
SELECT cm.Id,      
    cm.Name,      
    cm.Name_Ar FROM CategoryMaster cm    
    
    
    SELECT sm.Id,  
    sm.Name,  
    sm.Name_Ar,  
    cm.Name as CategoryName,   
    cm.Id as CategoryId
   from SubCategoryMaster sm   
   INNER JOIN CategoryMaster cm on sm.CategoryId=cm.Id 
    
    
  SELECT cm.Id,      
    cm.Name,      
    cm.Name_Ar    
 from MaterialTypeMaster cm       
    
    
  SELECT em.Id,      
    em.Name  from EducationMaster em       
    
    
   SELECT cm.Id,      
    cm.Name    
 from ProfessionMaster cm       
    
  SELECT cr.Id,      
    cr.Title,      
    Cr.Description,      
    cr.Title_Ar,      
    Cr.Description_Ar    
 from CopyrightMaster cr      
RETURN 105;      
END 
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
		 inner join MaterialTypeMaster mt on r.MaterialTypeId=mt.Id
		 left join SubCategoryMaster sc on r.SubCategoryId=sc.Id
		 left join CopyrightMaster cm on r.CopyRightId = cm.Id
		 inner join UserMaster um on r.CreatedBy =um.Id where um.Id=@UserID	Order by r.Id desc 


 SET @Return = 105;

 RETURN @Return;

END;
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  

  -- sps_UserById    1
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
      ,um.IsContributor, um.IsAdmin  
  FROM UserMaster um left JOIN TitleMaster tm on um.titleId=tm.Id  
      left JOIN CountryMaster cm on um.CountryId=cm.Id  
      left JOIN StateMaster sm on um.StateId=sm.Id  
     -- INNER JOIN DepartmentMaster dm on um.DepartmentId=dm.Id  
     -- INNER JOIN DesignationMaster dsm on um.DesignationId=dsm.Id  
  
     where um.Id=@Id  
  
  
RETURN 105 -- record exists  
  
END  
  
ELSE RETURN 102 -- No record exist  
  
END  
GO
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

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
	@Active BIT
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
					el.[Text],
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
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
			END

		ELSE

			BEGIN
				Update ResourceMaster SET IsDraft = 1,IsApproved = 0 WHERE ID = @ContentId
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
		WHERE es.Active = 1 AND es.id = @Id;
	 RETURN @return;  
END
GO
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
		WHERE eu.Active = 1 AND eu.id = @Id;
	 RETURN @return;  
END
GO
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
			   WHERE el.Active = 1 AND el.id = @ID
	 RETURN @return;  
END
GO
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
		 inner join UserMaster l on cm.UpdatedBy =l.Id order by cm.Id desc 	

		 RETURN @return
END

GO
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
	@Media NVARCHAR(100)
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
		Media = @Media
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

	SELECT	cr.Id,
				cr.Title,
				Cr.[Description],
				cr.Title_Ar,
				Cr.Description_Ar,
				cr.CreatedOn,
				CONCAT(c.FirstName, '', c.LastName) as CreatedBy,
				CONCAT(l.FirstName, '', l.LastName) as UpdatedBy, cr.Active, cr.UpdatedOn,cr.Media
		 from CopyrightMaster cr 
		 inner join UserMaster c  on cr.CreatedBy= c.Id
		 inner join UserMaster l on cr.UpdatedBy =l.Id	order by cr.Id desc	

		 RETURN @return
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [UpdateCourse]		  
		   @Title NVARCHAR(250),
           @CategoryId INT,
           @SubCategoryId INT=NULL,
           @Thumbnail  NVARCHAR(50),
           @CourseDescription  NVARCHAR(2000),
           @Keywords  NVARCHAR(1500),
           @CourseContent NTEXT,          
           @CopyRightId INT,
           @IsDraft BIT,		 
		   @EducationId int,
           @ProfessionId int,  
		   @References NVARCHAR(MAX)=null,  
		   @CourseFiles NVARCHAR(MAX)=null, 
		   @Id INT,
		   @ReadingTime INT = NULL        
AS
BEGIN

Declare @return INT

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
									  ReadingTime=@ReadingTime
									  WHERE Id=@Id
		-- do log entry here
		 
		  IF @@ERROR <> 0
		
		  SET @return= 106 -- update failed	


	ELSE

		SET @return= 101 -- update success	

IF @CourseFiles IS NOT NULL
	BEGIN
 -- Update Resource Associated Files FROM JSON
 
MERGE CourseAssociatedFiles AS TARGET
  USING
    (
    SELECT DISTINCT AssociatedFile
      FROM
      OPENJSON(@CourseFiles)
      WITH (AssociatedFile nvarchar(50) '$.AssociatedFile')
    ) AS source (AssociatedFile)
  ON TARGET.AssociatedFile = source.AssociatedFile
  WHEN NOT MATCHED 
  THEN 
    INSERT (CourseId,AssociatedFile, CreatedOn)
      VALUES (@Id,source.AssociatedFile,GETDATE());

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
  WHEN NOT MATCHED 
  THEN 
    INSERT (CourseId,URLReferenceId, CreatedOn)
      VALUES (@Id,source.URLReferenceId,GETDATE());
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
UPDATE CourseComments SET Comments=@Comments WHERE Id=@Id and CourseId=@CourseId and UserId=@CommentedBy
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [UpdateEducation] 
	@Id INT,
	@Name NVARCHAR(150),
	@UpdatedBy INT,
	@Active BIT
AS
BEGIN
Declare @return INT
IF NOT EXISTS (SELECT * FROM EducationMaster WHERE Name=@Name AND Id<> @Id)
BEGIN	
		UPDATE EducationMaster  SET Name=@Name,
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

	SELECT	em.Id,
				em.Name,
				em.CreatedOn,				
				c.FirstName,
				 CONCAT(c.FirstName, '', c.LastName) as CreatedBy,
				CONCAT(l.FirstName, '', l.LastName) as UpdatedBy,
				em.UpdatedOn, em.Active
		 from EducationMaster em 
		 inner join UserMaster c  on em.CreatedBy= c.Id
		 inner join UserMaster l on em.UpdatedBy =l.Id Order by em.Id desc 

	RETURN @return
END

GO
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

IF NOT EXISTS (SELECT * FROM MaterialTypeMaster WHERE Name=@Name AND Id<> @Id)
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
		 inner join UserMaster l on cm.UpdatedBy =l.Id order by cm.Id desc 	

		 RETURN @return
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [UpdateProfession] 
	@Id INT,
	@Name NVARCHAR(200),
	@UpdatedBy INT,
	@Active BIT
AS
BEGIN

Declare @return INT

IF NOT EXISTS (SELECT * FROM ProfessionMaster WHERE Name=@Name AND Id<> @Id)
BEGIN	
		UPDATE ProfessionMaster  SET Name=@Name,
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
				cm.CreatedOn,				
			 CONCAT(c.FirstName, '', c.LastName) as CreatedBy,
				CONCAT(l.FirstName, '', l.LastName) as UpdatedBy,
				cm.UpdatedOn, cm.Active
		 from ProfessionMaster cm 
		 inner join UserMaster c  on cm.CreatedBy= c.Id
		 inner join UserMaster l on cm.UpdatedBy =l.Id order by cm.Id desc 	

		 RETURN @return
END

GO
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [UpdateResource]		  
		   @Title  NVARCHAR(250),
           @CategoryId INT,
           @SubCategoryId INT= NULL,
           @Thumbnail  NVARCHAR(50),
           @ResourceDescription  NVARCHAR(2000),
           @Keywords  NVARCHAR(1500),
           @ResourceContent ntext,
           @MaterialTypeId INT,
           @CopyRightId INT,
           @IsDraft BIT,
		   @References NVARCHAR(MAX)=null,  
		   @ResourceFiles NVARCHAR(MAX)=null,  
		   @Id INT,
		   @ReadingTime INT = NULL,
		   @LevelId INT = NULL,
		   @EducationalStandardId INT = NULL,
		   @EducationalUseId INT = NULL,
		   @Format NVARCHAR(100) = NULL,
		   @Objective NVARCHAR(100) = NULL
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
									  Objective = @Objective
									  WHERE Id=@Id
		-- do log entry here
		 
		  IF @@ERROR <> 0
		
		  SET @return= 106 -- update failed	


	ELSE

		SET @return= 101 -- update success	

IF @ResourceFiles IS NOT NULL
	BEGIN
 -- Update Resource Associated Files FROM JSON
 
MERGE ResourceAssociatedFiles AS TARGET
  USING
    (
    SELECT DISTINCT AssociatedFile
      FROM
      OPENJSON(@ResourceFiles)
      WITH (AssociatedFile nvarchar(50) '$.AssociatedFile')
    ) AS source (AssociatedFile)
  ON TARGET.AssociatedFile = source.AssociatedFile
  WHEN NOT MATCHED 
  THEN 
    INSERT (ResourceId,AssociatedFile, UploadedDate)
      VALUES (@Id,source.AssociatedFile,GETDATE());

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
  WHEN NOT MATCHED 
  THEN 
    INSERT (ResourceId,URLReferenceId, CreatedOn)
      VALUES (@Id,source.URLReferenceId,GETDATE());
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
UPDATE ResourceComments SET Comments=@Comments WHERE Id=@Id and ResourceId=@ResourceId and UserId=@CommentedBy
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

IF NOT EXISTS (SELECT * FROM SubCategoryMaster WHERE Name=@Name AND Id<> @Id)
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
		 inner join UserMaster l on sm.UpdatedBy =l.Id order by cm.Id desc 	

		 RETURN @return
END

GO
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
           @Photo nvarchar(50)=NULL,
           @ProfileDescription nvarchar(4000)=NULL,
           @SubjectsInterested nvarchar(500)=NULL,               
           @IsContributor bit,
		   @UserCertifications nvarchar(max)=NULL,
		   @UserEducations nvarchar(max)=NULL,
		   @UserExperiences nvarchar(max)=NULL,
		   @UserLanguages nvarchar(max)=NULL,
		   @UserSocialMedias nvarchar(max)=NULL,
		   @Id INT
		   )
AS
BEGIN
Declare @return INT

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





IF @UserCertifications IS NOT NULL
 BEGIN
 -- INSERT USER CERTIFICATION FROM JSON
 DELETE FROM UserCertification WHERE userid = @ID 
INSERT INTO UserCertification
    
SELECT @Id,CertificationName,Year,GETDATE() FROM  
 OPENJSON ( @UserCertifications )  
WITH (   
              CertificationName   varchar(250) '$.CertificationName',              
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
              UniversitySchool   varchar(250) '$.UniversitySchool',              
              Major varchar(100)          '$.Major',  
			  Grade varchar(10)          '$.Grade', 
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
			  OrganizationName   varchar(250) '$.OrganizationName',              
              Designation varchar(250)          '$.Designation',
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
 Language varchar(250),
 LanguageId int,
 IsRead bit, 
 IsWrite bit,
 IsSpeak bit
)

 
INSERT INTO #tempUserLangs
    
SELECT Language,0,IsRead,IsWrite,IsSpeak FROM  
 OPENJSON ( @UserLanguages )  
WITH (   
			  Language   varchar(250) '$.Language',              
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
 select Id,IsEmailNotification,IsContributor FROM UserMaster WHERE ID = @Id
 RETURN @return;
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [UpdateUserRole]		  
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


END
else set @return= 102

return @return
END
GO
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
	   CONCAT(l.FirstName, '', l.LastName) as VerifiedBy
      ,URL
      ,IsApproved
      ,RequestedOn
      ,VerifiedOn
      ,RejectedReason
  FROM WhiteListingURLs wlu 
   inner join UserMaster c  on wlu.RequestedBy= c.Id
		 left join UserMaster l on wlu.VerifiedBy =l.Id  where IsApproved=1 and URL=@URL

  IF @@ROWCOUNT >0
		return 105 -- record exists
		
		ELSE
		return 102 -- record does not exists
END

GO
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
WHERE u.QRCId is null AND u.CategoryId	 is null and u.UserId is null

SELECT ID, Email,FirstName, LastName
FROM UserMaster
 WHERE ID IN (SELECT UserId FROM @QRCUserDetails) AND IsEmailNotification = 1

	SET @return = 100 -- reconrd does not exists
	  RETURN @return
END 

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [VerifyWhiteListingRequest] 
	@Id DECIMAL,	
	@VerifiedBy INT,
	@IsApproved BIT,
	@RejectedReason NVARCHAR(100)=NULL
AS
BEGIN

Declare @return INT

IF EXISTS (SELECT TOP 1 1 FROM WhiteListingURLs WHERE Id=@Id)
BEGIN	
		Update WhiteListingURLs
		
		set VerifiedBy=@VerifiedBy,
			IsApproved=@IsApproved,
			RejectedReason=@RejectedReason where Id=@Id

		-- do log entry here
		 
		  IF @@ERROR <> 0
		
		  SET @return= 106 -- update failed	
	ELSE

	SELECT Id,
TitleId,
FirstName +' ' +LastName as UserName,
Email FROM UserMaster WHERE ID in (SELECT RequestedBy FROM WhiteListingURLs WHERE Id=@Id)
		SET @return= 101 -- update success	
END

ELSE
	BEGIN
		SET @return= 102 -- Record does not exists
	END	

 RETURN @return

END

GO
