/****** Object:  UserDefinedTableType [dbo].[ApplicationRequestType]    Script Date: 2020-11-19 1:41:53 PM ******/
CREATE TYPE [dbo].[ApplicationRequestType] AS TABLE(
	[Step] [int] NULL,
	[RequestorID] [uniqueidentifier] NULL,
	[RequestorName] [nvarchar](150) NULL,
	[AgentID] [uniqueidentifier] NULL,
	[AgentName] [nvarchar](150) NULL,
	[CustomerID] [uniqueidentifier] NULL,
	[CustomerCode] [varchar](30) NULL,
	[CustomerName] [varchar](150) NULL,
	[Type] [int] NULL,
	[Status] [int] NULL,
	[StatusMinor] [int] NULL,
	[OldStatus] [int] NULL,
	[RefID] [varchar](36) NULL,
	[ParentID] [bigint] NULL,
	[Expedite] [bit] NULL,
	[Escalate] [bit] NULL,
	[AssignedTo] [uniqueidentifier] NULL,
	[AssignedToName] [nvarchar](150) NULL,
	[CreatedOn] [datetimeoffset](7) NULL,
	[TargetCompletion] [datetimeoffset](7) NULL,
	[DueOn] [datetimeoffset](7) NULL,
	[ModifiedOn] [datetimeoffset](7) NULL,
	[SubmittedOn] [datetimeoffset](7) NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[AssignedToType]    Script Date: 2020-11-19 1:41:53 PM ******/
CREATE TYPE [dbo].[AssignedToType] AS TABLE(
	[Assigned] [uniqueidentifier] NOT NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[AttachmentsType]    Script Date: 2020-11-19 1:41:53 PM ******/
CREATE TYPE [dbo].[AttachmentsType] AS TABLE(
	[FileID] [uniqueidentifier] NULL,
	[FileName] [nvarchar](150) NULL,
	[Extension] [nvarchar](30) NULL,
	[Size] [bigint] NULL,
	[CreatedOn] [datetimeoffset](7) NULL,
	[ModifiedOn] [datetimeoffset](7) NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[BigIntType]    Script Date: 2020-11-19 1:41:53 PM ******/
CREATE TYPE [dbo].[BigIntType] AS TABLE(
	[Val] [bigint] NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[CharacteristicsType]    Script Date: 2020-11-19 1:41:53 PM ******/
CREATE TYPE [dbo].[CharacteristicsType] AS TABLE(
	[Type] [int] NULL,
	[Value] [nvarchar](2000) NULL,
	[CreatedOn] [datetimeoffset](7) NULL,
	[ModifiedOn] [datetimeoffset](7) NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[EscalateStatusType]    Script Date: 2020-11-19 1:41:53 PM ******/
CREATE TYPE [dbo].[EscalateStatusType] AS TABLE(
	[EscalateStatus] [int] NOT NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[HalalTeamType]    Script Date: 2020-11-19 1:41:53 PM ******/
CREATE TYPE [dbo].[HalalTeamType] AS TABLE(
	[AltID] [nvarchar](36) NULL,
	[Name] [varchar](150) NULL,
	[Designation] [nvarchar](100) NULL,
	[Role] [nvarchar](50) NULL,
	[IsCertified] [bit] NULL,
	[JoinedOn] [datetimeoffset](7) NULL,
	[ChangeType] [int] NULL,
	[RequestID] [bigint] NULL,
	[CreatedOn] [datetimeoffset](7) NULL,
	[ModifiedOn] [datetimeoffset](7) NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[IDMappingType]    Script Date: 2020-11-19 1:41:53 PM ******/
CREATE TYPE [dbo].[IDMappingType] AS TABLE(
	[A] [bigint] NULL,
	[B] [bigint] NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[IngredientType]    Script Date: 2020-11-19 1:41:53 PM ******/
CREATE TYPE [dbo].[IngredientType] AS TABLE(
	[ID] [bigint] NULL,
	[Text] [nvarchar](500) NULL,
	[SubText] [nvarchar](2000) NULL,
	[RiskCategory] [int] NULL,
	[Approved] [bit] NULL,
	[Remarks] [nvarchar](2000) NULL,
	[ReviewedBy] [uniqueidentifier] NULL,
	[ReviewedByName] [nvarchar](150) NULL,
	[ReviewedOn] [datetimeoffset](7) NULL,
	[ChangeType] [int] NULL,
	[RequestID] [bigint] NULL,
	[CreatedOn] [datetimeoffset](7) NULL,
	[ModifiedOn] [datetimeoffset](7) NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[LineItemAttachmentsType]    Script Date: 2020-11-19 1:41:53 PM ******/
CREATE TYPE [dbo].[LineItemAttachmentsType] AS TABLE(
	[Index] [int] NULL,
	[FileID] [uniqueidentifier] NULL,
	[FileName] [nvarchar](150) NULL,
	[Extension] [nvarchar](30) NULL,
	[Size] [bigint] NULL,
	[CreatedOn] [datetimeoffset](7) NULL,
	[ModifiedOn] [datetimeoffset](7) NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[LineItemReplyAttachmentsType]    Script Date: 2020-11-19 1:41:53 PM ******/
CREATE TYPE [dbo].[LineItemReplyAttachmentsType] AS TABLE(
	[LineItemID] [bigint] NOT NULL,
	[FileID] [uniqueidentifier] NULL,
	[FileName] [nvarchar](150) NULL,
	[Extension] [nvarchar](30) NULL,
	[Size] [bigint] NULL,
	[CreatedOn] [datetimeoffset](7) NULL,
	[ModifiedOn] [datetimeoffset](7) NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[MenuType]    Script Date: 2020-11-19 1:41:53 PM ******/
CREATE TYPE [dbo].[MenuType] AS TABLE(
	[ID] [bigint] NULL,
	[Scheme] [int] NULL,
	[Text] [nvarchar](500) NULL,
	[SubText] [nvarchar](2000) NULL,
	[Approved] [bit] NULL,
	[Remarks] [nvarchar](2000) NULL,
	[ReviewedBy] [uniqueidentifier] NULL,
	[ReviewedByName] [nvarchar](150) NULL,
	[ReviewedOn] [datetimeoffset](7) NULL,
	[ChangeType] [int] NULL,
	[RequestID] [bigint] NULL,
	[CreatedOn] [datetimeoffset](7) NULL,
	[ModifiedOn] [datetimeoffset](7) NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[PremiseType]    Script Date: 2020-11-19 1:41:53 PM ******/
CREATE TYPE [dbo].[PremiseType] AS TABLE(
	[IsLocal] [bit] NULL,
	[Name] [nvarchar](50) NULL,
	[Type] [int] NULL,
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
	[IsPrimary] [bit] NULL,
	[ChangeType] [int] NULL,
	[RequestID] [bigint] NULL,
	[CreatedOn] [datetimeoffset](7) NULL,
	[ModifiedOn] [datetimeoffset](7) NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[RequestActionType]    Script Date: 2020-11-19 1:41:53 PM ******/
CREATE TYPE [dbo].[RequestActionType] AS TABLE(
	[action] [int] NOT NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[RequestLineItemCharacteristicsType]    Script Date: 2020-11-19 1:41:53 PM ******/
CREATE TYPE [dbo].[RequestLineItemCharacteristicsType] AS TABLE(
	[Type] [int] NULL,
	[Value] [nvarchar](2000) NULL,
	[Index] [int] NULL,
	[RefIndex] [int] NULL,
	[CreatedOn] [datetimeoffset](7) NULL,
	[ModifiedOn] [datetimeoffset](7) NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[RequestLineItemType]    Script Date: 2020-11-19 1:41:53 PM ******/
CREATE TYPE [dbo].[RequestLineItemType] AS TABLE(
	[Scheme] [int] NULL,
	[SubScheme] [int] NULL,
	[Index] [int] NULL,
	[ComplianceHistoryID] [bigint] NULL,
	[RequestID] [bigint] NULL,
	[CreatedOn] [datetimeoffset](7) NULL,
	[ModifiedOn] [datetimeoffset](7) NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[RequestStatusType]    Script Date: 2020-11-19 1:41:53 PM ******/
CREATE TYPE [dbo].[RequestStatusType] AS TABLE(
	[Status] [int] NOT NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[RequestType]    Script Date: 2020-11-19 1:41:53 PM ******/
CREATE TYPE [dbo].[RequestType] AS TABLE(
	[Type] [int] NOT NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[ReviewType]    Script Date: 2020-11-19 1:41:53 PM ******/
CREATE TYPE [dbo].[ReviewType] AS TABLE(
	[Step] [int] NULL,
	[Grade] [int] NULL,
	[ReviewerID] [uniqueidentifier] NULL,
	[ReviewerName] [nvarchar](150) NULL,
	[RequestID] [bigint] NULL,
	[CreatedOn] [datetimeoffset](7) NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[RFALineItemReplyType]    Script Date: 2020-11-19 1:41:53 PM ******/
CREATE TYPE [dbo].[RFALineItemReplyType] AS TABLE(
	[RFAID] [bigint] NOT NULL,
	[LineItemID] [bigint] NOT NULL,
	[Scheme] [smallint] NOT NULL,
	[ComplianceCategoryID] [bigint] NOT NULL,
	[ComplianceID] [bigint] NOT NULL,
	[Text] [nvarchar](2000) NOT NULL,
	[CreatedOn] [datetimeoffset](7) NULL,
	[ModifiedOn] [datetimeoffset](7) NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[RFALineItemType]    Script Date: 2020-11-19 1:41:53 PM ******/
CREATE TYPE [dbo].[RFALineItemType] AS TABLE(
	[Scheme] [int] NULL,
	[Index] [int] NULL,
	[ComplianceCategoryID] [bigint] NULL,
	[ComplianceCategoryText] [nvarchar](80) NULL,
	[ComplianceID] [bigint] NULL,
	[ComplianceText] [nvarchar](4000) NULL,
	[Remarks] [nvarchar](2000) NULL,
	[RFAID] [bigint] NULL,
	[CreatedOn] [datetimeoffset](7) NULL,
	[ModifiedOn] [datetimeoffset](7) NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[RFAStatusType]    Script Date: 2020-11-19 1:41:53 PM ******/
CREATE TYPE [dbo].[RFAStatusType] AS TABLE(
	[Status] [int] NOT NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[RFAType]    Script Date: 2020-11-19 1:41:53 PM ******/
CREATE TYPE [dbo].[RFAType] AS TABLE(
	[Status] [int] NULL,
	[RaisedBy] [uniqueidentifier] NULL,
	[RaisedByName] [nvarchar](150) NULL,
	[RequestID] [bigint] NULL,
	[CreatedOn] [datetimeoffset](7) NULL,
	[DueOn] [datetimeoffset](7) NULL,
	[ModifiedOn] [datetimeoffset](7) NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[SchemeType]    Script Date: 2020-11-19 1:41:53 PM ******/
CREATE TYPE [dbo].[SchemeType] AS TABLE(
	[ID] [int] NOT NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[SmallIntType]    Script Date: 2020-11-19 1:41:53 PM ******/
CREATE TYPE [dbo].[SmallIntType] AS TABLE(
	[Val] [smallint] NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[UniqueIdentifierType]    Script Date: 2020-11-19 1:41:53 PM ******/
CREATE TYPE [dbo].[UniqueIdentifierType] AS TABLE(
	[Val] [uniqueidentifier] NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[UniqueIdentifierType]    Script Date: 2020-11-19 1:41:53 PM ******/
CREATE TYPE [dbo].[VarcharType] AS TABLE(
	[Val] [varchar](36) NULL
)
GO
/****** Object:  UserDefinedFunction [dbo].[FormatPremise]    Script Date: 2020-11-19 1:41:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[FormatPremise](
    @Name NVARCHAR(150) = NULL,
    @BlockNo VARCHAR(15) = NULL,
    @Address1 NVARCHAR(150) = NULL,
    @Address2 NVARCHAR(150) = NULL,
    @FloorNo VARCHAR(5) = NULL,
    @UnitNo VARCHAR(5) = NULL,
    @BuildingName NVARCHAR(100) = NULL,
    @Province NVARCHAR(100) = NULL,
    @City NVARCHAR(100) = NULL,
    @Country NVARCHAR(100) = NULL,
    @Postal VARCHAR(20) = NULL)
RETURNS NVARCHAR(700)
BEGIN
    RETURN CONCAT(@BlockNo, ' ', @Address1, ' ', @Address2, CASE WHEN @FloorNo IS NOT NULL THEN ' #' ELSE ' ' END, @FloorNo, CASE WHEN @FloorNo IS NOT NULL THEN '-' ELSE ' ' END, @UnitNo, ' ', @BuildingName, ' ', @Province, ' ', @City, ' ', @Country, ' ', @Postal)
END
GO
