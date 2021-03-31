/****** Object:  UserDefinedTableType [dbo].[BigIntType]    Script Date: 2020-11-19 11:21:54 AM ******/
CREATE TYPE [dbo].[BigIntType] AS TABLE(
	[Val] [bigint] NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[IDMappingType]    Script Date: 2020-11-19 11:49:20 AM ******/
CREATE TYPE [dbo].[IDMappingType] AS TABLE(
	[A] [bigint] NULL,
	[B] [bigint] NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[SmallIntType]    Script Date: 2020-11-19 11:49:20 AM ******/
CREATE TYPE [dbo].[SmallIntType] AS TABLE(
	[Val] [smallint] NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[UniqueIdentifierType]    Script Date: 2020-11-19 11:49:20 AM ******/
CREATE TYPE [dbo].[UniqueIdentifierType] AS TABLE(
	[Val] [uniqueidentifier] NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[AttendeeType]    Script Date: 2020-11-19 11:49:20 AM ******/
CREATE TYPE [dbo].[AttendeeType] AS TABLE(
	[ID] [bigint] NULL,
	[Name] [nvarchar](150) NULL,
	[Designation] [nvarchar](150) NULL,
	[Start] [bit] NULL,
	[End] [bit] NULL,
	[JobID] [bigint] NULL
)
GO
