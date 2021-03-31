/****** Object:  UserDefinedTableType [dbo].[BigIntType]    Script Date: 2020-11-19 11:21:54 AM ******/
CREATE TYPE [dbo].[BigIntType] AS TABLE(
	[Val] [bigint] NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[CertificateHistoryType]    Script Date: 2020-11-19 11:21:54 AM ******/
CREATE TYPE [dbo].[CertificateHistoryType] AS TABLE(
	[RefID] [bigint] NULL,
	[IssuedOn] [datetimeoffset](7) NULL,
	[ExpiresOn] [datetimeoffset](7) NULL,
	[ApprovedOn] [datetimeoffset](7) NULL,
	[ApprovedBy] [uniqueidentifier] NULL,
	[ApprovedByName] [nvarchar](200) NULL,
	[CertificateID] [bigint] NULL,
	[CreatedOn] [datetimeoffset](7) NULL,
	[ModifiedOn] [datetimeoffset](7) NULL,
	[RequestID] [bigint] NULL,
	[IsDeleted] [bit] NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[CertificatesType]    Script Date: 2020-11-19 11:21:54 AM ******/
CREATE TYPE [dbo].[CertificatesType] AS TABLE(
	[Number] [varchar](100) NULL,
	[Status] [int] NULL,
	[Scheme] [int] NULL,
	[SubScheme] [int] NULL,
	[IssuedOn] [datetimeoffset](7) NULL,
	[ExpiresOn] [datetimeoffset](7) NULL,
	[CustomerID] [uniqueidentifier] NULL,
	[PremiseID] [bigint] NULL,
	[CreatedOn] [datetimeoffset](7) NULL,
	[ModifiedOn] [datetimeoffset](7) NULL,
	[IsDeleted] [bit] NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[ContactInfoType]    Script Date: 2020-11-19 11:21:54 AM ******/
CREATE TYPE [dbo].[ContactInfoType] AS TABLE(
	[Type] [smallint] NOT NULL,
	[Value] [nvarchar](100) NOT NULL,
	[IsPrimary] [bit] NOT NULL,
	[CreatedOn] [datetimeoffset](7) NULL,
	[ModifiedOn] [datetimeoffset](7) NULL,
	[PersonID] [uniqueidentifier] NOT NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[CustomerType]    Script Date: 2020-11-19 11:21:54 AM ******/
CREATE TYPE [dbo].[CustomerType] AS TABLE(
	[ID] [uniqueidentifier] NULL,
	[IDType] [int] NULL,
	[AltID] [nvarchar](50) NULL,
	[Name] [nvarchar](50) NULL,
	[Code] [nvarchar](50) NULL,
	[CodeGroup] [nvarchar](50) NULL,
	[Status] [int] NULL,
	[ParentID] [uniqueidentifier] NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[IDMappingType]    Script Date: 2020-11-19 11:21:54 AM ******/
CREATE TYPE [dbo].[IDMappingType] AS TABLE(
	[A] [bigint] NULL,
	[B] [bigint] NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[PersonType]    Script Date: 2020-11-19 11:21:54 AM ******/
CREATE TYPE [dbo].[PersonType] AS TABLE(
	[ID] [uniqueidentifier] NOT NULL,
	[Salutation] [varchar](10) NULL,
	[Name] [varchar](150) NULL,
	[Nationality] [nvarchar](60) NULL,
	[Gender] [varchar](10) NULL,
	[DOB] [datetimeoffset](7) NULL,
	[Designation] [nvarchar](100) NULL,
	[DesignationOther] [nvarchar](100) NULL,
	[IDType] [smallint] NULL,
	[AltID] [varchar](30) NULL,
	[PassportIssuingCountry] [varchar](100) NULL,
	[CreatedOn] [datetimeoffset](7) NULL,
	[ModifiedOn] [datetimeoffset](7) NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[PremiseCertificateHistoryType]    Script Date: 2020-11-19 11:21:54 AM ******/
CREATE TYPE [dbo].[PremiseCertificateHistoryType] AS TABLE(
	[RefID] [bigint] NULL,
	[IssuedOn] [datetimeoffset](7) NULL,
	[ExpiresOn] [datetimeoffset](7) NULL,
	[ApprovedOn] [datetimeoffset](7) NULL,
	[ApprovedBy] [uniqueidentifier] NULL,
	[ApprovedByName] [nvarchar](200) NULL,
	[CertificateID] [bigint] NULL,
	[CreatedOn] [datetimeoffset](7) NULL,
	[ModifiedOn] [datetimeoffset](7) NULL,
	[RequestID] [bigint] NULL,
	[IsDeleted] [bit] NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[PremiseCertificatesType]    Script Date: 2020-11-19 11:21:54 AM ******/
CREATE TYPE [dbo].[PremiseCertificatesType] AS TABLE(
	[Number] [varchar](100) NULL,
	[Status] [int] NULL,
	[Scheme] [int] NULL,
	[SubScheme] [int] NULL,
	[IssuedOn] [datetimeoffset](7) NULL,
	[ExpiresOn] [datetimeoffset](7) NULL,
	[CustomerID] [uniqueidentifier] NULL,
	[PremiseID] [bigint] NULL,
	[CreatedOn] [datetimeoffset](7) NULL,
	[ModifiedOn] [datetimeoffset](7) NULL,
	[IsDeleted] [bit] NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[PremiseType]    Script Date: 2020-11-19 11:21:54 AM ******/
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
	[CreatedOn] [datetimeoffset](7) NULL,
	[ModifiedOn] [datetimeoffset](7) NULL,
	[CustomerID] [uniqueidentifier] NULL,
	[CertificateID] [bigint] NULL,
	[IsDeleted] [bit] NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[SmallIntType]    Script Date: 2020-11-19 11:21:54 AM ******/
CREATE TYPE [dbo].[SmallIntType] AS TABLE(
	[Val] [smallint] NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[UniqueIdentifierType]    Script Date: 2020-11-19 11:21:54 AM ******/
CREATE TYPE [dbo].[UniqueIdentifierType] AS TABLE(
	[Val] [uniqueidentifier] NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[NvarcharType]    Script Date: 10/2/2021 12:00:23 PM ******/
CREATE TYPE [dbo].[NvarcharType] AS TABLE(
	[Val] [nvarchar](255) NULL
)
GO
