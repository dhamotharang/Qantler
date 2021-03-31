/****** Object:  Sequence [dbo].[CertificateSequence]    Script Date: 2020-11-19 1:44:57 PM ******/
CREATE SEQUENCE [dbo].[CertificateSequence] 
 AS [int]
 START WITH 1
 INCREMENT BY 1
 MINVALUE -2147483648
 MAXVALUE 2147483647
 CACHE 
GO
/****** Object:  Sequence [dbo].[CertificateSerialNo]    Script Date: 2020-11-19 1:45:19 PM ******/
CREATE SEQUENCE [dbo].[CertificateSerialNo] 
 AS [int]
 START WITH 1
 INCREMENT BY 1
 MINVALUE -2147483648
 MAXVALUE 2147483647
 CACHE 
GO
/****** Object:  Sequence [dbo].[CustomerCodeSeries]    Script Date: 2020-11-19 1:45:30 PM ******/
CREATE SEQUENCE [dbo].[CustomerCodeSeries] 
 AS [bigint]
 START WITH 0
 INCREMENT BY 1
 MINVALUE -9223372036854775808
 MAXVALUE 9223372036854775807
 CACHE 
GO
/****** Object:  Sequence [dbo].[CustomerGroupCodeSeries]    Script Date: 2020-11-19 1:45:39 PM ******/
CREATE SEQUENCE [dbo].[CustomerGroupCodeSeries] 
 AS [bigint]
 START WITH 0
 INCREMENT BY 1
 MINVALUE -9223372036854775808
 MAXVALUE 9223372036854775807
 CACHE 
GO


