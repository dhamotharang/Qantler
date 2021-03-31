INSERT [dbo].[FieldLookup] ([ID], [Text]) VALUES (0, N'Area')
GO

INSERT [dbo].[OperatorLookup] ([ID], [Text]) VALUES (0, N'==')
GO
INSERT [dbo].[OperatorLookup] ([ID], [Text]) VALUES (1, N'<')
GO
INSERT [dbo].[OperatorLookup] ([ID], [Text]) VALUES (2, N'>')
GO
INSERT [dbo].[OperatorLookup] ([ID], [Text]) VALUES (3, N'<=')
GO
INSERT [dbo].[OperatorLookup] ([ID], [Text]) VALUES (4, N'>=')
GO
INSERT [dbo].[OperatorLookup] ([ID], [Text]) VALUES (5, N'!=')
GO

INSERT [dbo].[LogicalLookup] ([ID], [Text]) VALUES (0, N'OR')
GO
INSERT [dbo].[LogicalLookup] ([ID], [Text]) VALUES (1, N'AND')
GO
INSERT [dbo].[LogicalLookup] ([ID], [Text]) VALUES (2, N'NOT')
GO

INSERT [dbo].[Locale] ([ID], [Text], [Code]) VALUES (0, N'English', N'en')
GO

INSERT [dbo].[SettingsLookup] ([ID], [Text]) VALUES (100, N'GST')
GO
INSERT [dbo].[SettingsLookup] ([ID], [Text]) VALUES (200, N'Stage 1 Percentage')
GO

SET IDENTITY_INSERT [dbo].[Settings] ON 
GO
INSERT [dbo].[Settings] ([ID], [Type], [Value], [Text], [DataType], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (1, 100, N'0.07', N'GST', N'System.Decimal', CAST(N'2020-10-07T10:09:11.0000000' AS DateTime2), CAST(N'2020-10-07T10:09:11.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Settings] ([ID], [Type], [Value], [Text], [DataType], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (2, 200, N'0.4', N'Stage 1 - Percentage', N'System.Float', CAST(N'2020-10-07T10:09:49.0000000' AS DateTime2), CAST(N'2020-10-07T10:09:49.0000000' AS DateTime2), 1)
GO
SET IDENTITY_INSERT [dbo].[Settings] OFF
GO

INSERT [dbo].[BillRequestTypeLookup] ([ID], [Text]) VALUES (0, N'New')
GO
INSERT [dbo].[BillRequestTypeLookup] ([ID], [Text]) VALUES (1, N'Renewal')
GO
INSERT [dbo].[BillRequestTypeLookup] ([ID], [Text]) VALUES (3, N'HC02')
GO
INSERT [dbo].[BillRequestTypeLookup] ([ID], [Text]) VALUES (4, N'HC03')
GO

INSERT [dbo].[BillStatusLookup] ([ID], [Text]) VALUES (100, N'Draft')
GO
INSERT [dbo].[BillStatusLookup] ([ID], [Text]) VALUES (200, N'Pending')
GO
INSERT [dbo].[BillStatusLookup] ([ID], [Text]) VALUES (300, N'Paid')
GO
INSERT [dbo].[BillStatusLookup] ([ID], [Text]) VALUES (400, N'Overdue')
GO
INSERT [dbo].[BillStatusLookup] ([ID], [Text]) VALUES (600, N'Canceled')
GO

INSERT [dbo].[BillTypeLookup] ([ID], [Text]) VALUES (0, N'Stage 1')
GO
INSERT [dbo].[BillTypeLookup] ([ID], [Text]) VALUES (1, N'Stage 2')
GO
INSERT [dbo].[BillTypeLookup] ([ID], [Text]) VALUES (2, N'Composition Sum')
GO

INSERT [dbo].[LogTypeLookup] ([ID], [Text]) VALUES (0, N'Default')
GO
INSERT [dbo].[LogTypeLookup] ([ID], [Text]) VALUES (1, N'Request')
GO
INSERT [dbo].[LogTypeLookup] ([ID], [Text]) VALUES (2, N'RFA')
GO
INSERT [dbo].[LogTypeLookup] ([ID], [Text]) VALUES (3, N'JobOrder')
GO
INSERT [dbo].[LogTypeLookup] ([ID], [Text]) VALUES (4, N'Finance')
GO

INSERT [dbo].[PaymentMethodLookup] ([ID], [Text]) VALUES (0, N'Cash')
GO
INSERT [dbo].[PaymentMethodLookup] ([ID], [Text]) VALUES (1, N'PayPal')
GO
INSERT [dbo].[PaymentMethodLookup] ([ID], [Text]) VALUES (2, N'GIRO')
GO
INSERT [dbo].[PaymentMethodLookup] ([ID], [Text]) VALUES (3, N'Bank Transfer')
GO
INSERT [dbo].[PaymentMethodLookup] ([ID], [Text]) VALUES (4, N'DDA')
GO
INSERT [dbo].[PaymentModeLookup] ([ID], [Text]) VALUES (0, N'Online')
GO
INSERT [dbo].[PaymentModeLookup] ([ID], [Text]) VALUES (1, N'Offline')
GO

INSERT [dbo].[PaymentStatusLookup] ([ID], [Text]) VALUES (100, N'Draft')
GO
INSERT [dbo].[PaymentStatusLookup] ([ID], [Text]) VALUES (200, N'Pending')
GO
INSERT [dbo].[PaymentStatusLookup] ([ID], [Text]) VALUES (300, N'Processed')
GO
INSERT [dbo].[PaymentStatusLookup] ([ID], [Text]) VALUES (400, N'Rejected')
GO
INSERT [dbo].[PaymentStatusLookup] ([ID], [Text]) VALUES (500, N'Failed')
GO
INSERT [dbo].[PaymentStatusLookup] ([ID], [Text]) VALUES (600, N'Expired')
GO
INSERT [dbo].[PaymentStatusLookup] ([ID], [Text]) VALUES (700, N'Canceled')
GO

SET IDENTITY_INSERT [dbo].[Translations] ON 
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (5, 0, N'ApprovedPayment', N'approved the payment.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (6, 0, N'RejectedPayment', N'rejected the payment.')
GO
SET IDENTITY_INSERT [dbo].[Translations] OFF 
GO

INSERT [dbo].[ContactInfoTypeLookup] ([ID], [Text]) VALUES (0, N'Office')
GO
INSERT [dbo].[ContactInfoTypeLookup] ([ID], [Text]) VALUES (1, N'Office Extension')
GO
INSERT [dbo].[ContactInfoTypeLookup] ([ID], [Text]) VALUES (2, N'Home')
GO
INSERT [dbo].[ContactInfoTypeLookup] ([ID], [Text]) VALUES (3, N'Fax')
GO
INSERT [dbo].[ContactInfoTypeLookup] ([ID], [Text]) VALUES (4, N'Mobile')
GO
INSERT [dbo].[ContactInfoTypeLookup] ([ID], [Text]) VALUES (5, N'Email')
GO
INSERT [dbo].[ContactInfoTypeLookup] ([ID], [Text]) VALUES (6, N'Alternative Email')
GO
INSERT [dbo].[ContactInfoTypeLookup] ([ID], [Text]) VALUES (99, N'Others')
GO

INSERT [dbo].[DDAStatusLookup] ([ID], [Text]) VALUES (100, N'Default')
GO
INSERT [dbo].[DDAStatusLookup] ([ID], [Text]) VALUES (200, N'Approved')
GO
INSERT [dbo].[DDAStatusLookup] ([ID], [Text]) VALUES (300, N'Rejected')
GO

INSERT [dbo].[MasterTypeLookup] ([ID], [Text]) VALUES (600, N'Bank')
GO