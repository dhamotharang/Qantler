INSERT [dbo].[SchemeLookup] ([ID], [Text]) VALUES (100, N'Eating Establishment')
GO
INSERT [dbo].[SchemeLookup] ([ID], [Text]) VALUES (200, N'Food Preparation Area')
GO
INSERT [dbo].[SchemeLookup] ([ID], [Text]) VALUES (300, N'Food Manufacturing')
GO
INSERT [dbo].[SchemeLookup] ([ID], [Text]) VALUES (400, N'Poultry')
GO
INSERT [dbo].[SchemeLookup] ([ID], [Text]) VALUES (500, N'Endorsement')
GO
INSERT [dbo].[SchemeLookup] ([ID], [Text]) VALUES (600, N'Storage Facility')
GO

INSERT [dbo].[Locale] ([ID], [Text], [Code]) VALUES (0, N'English', N'en')
GO

SET IDENTITY_INSERT [dbo].[Translations] ON 
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (2, 0, N'UpdatedIdentitySettings', N'updated {0} from {1} to {2}')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (3, 0, N'AddNewCluster', N'created cluster')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (4, 0, N'AddNewNode', N'added : {0}')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (5, 0, N'UpdateCluster', N'updated location:{0} and nodes:{1}')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (6, 0, N'DeleteCluster', N'deleted cluster of district:{0}')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (7, 0, N'UpdateNode', N'updated Node from {0} to {1}')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (9, 0, N'DeleteNode', N'removed: {0}')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (10, 0, N'ValidateUniqueName', N'Please enter unique name value')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (11, 0, N'ValidateNode', N'Nodes {0} already exists.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (10004, 0, N'UpdateClusterLocation', N'update location: {0}')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (10005, 0, N'IdentityEmailExsits', N'User with email {0} already exists.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (10006, 0, N'IdentityCreated', N'created the user.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (10007, 0, N'IdentityUpdated', N'updated the user.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (10008, 0, N'IdentityPasswordReset', N'reset the password.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (10009, 0, N'UpdateClusterName', N'update name: {0}')
GO
SET IDENTITY_INSERT [dbo].[Translations] OFF
GO

INSERT [dbo].[IdentityStatusLookup] ([ID], [Text]) VALUES (0, N'Active')
GO
INSERT [dbo].[IdentityStatusLookup] ([ID], [Text]) VALUES (1, N'In-Active')
GO
INSERT [dbo].[IdentityStatusLookup] ([ID], [Text]) VALUES (2, N'Suspended')
GO

INSERT [dbo].[EmailTemplateTypeLookup] ([ID], [Text]) VALUES (200, N'New Account Email Template')
GO
INSERT [dbo].[EmailTemplateTypeLookup] ([ID], [Text]) VALUES (201, N'Reset Password Email Template')
GO

SET IDENTITY_INSERT [dbo].[EmailTemplate] ON 
GO
INSERT [dbo].[EmailTemplate] ([ID], [Type], [From], [Cc], [Bcc], [Title], [Body], [IsBodyHtml], [CreatedOn], [ModifiedOn], [IsDeleted],[Keyword]) VALUES (1, 201, N'keyfe.a@totalebizsolutions.com', NULL, NULL, N'eHS Account Password Reset', N'<div>eHS account</div><div><br /><span style="color: #008355; font-size: 36pt;">Your password has been reset</span></div><div><br />Your password for the eHS account {email} was reset.</div><div>&nbsp;</div><div>Use the temporary generated password to login to eHS.</div><div>&nbsp;</div><div>&nbsp;</div><div>Here is your temporary password: <strong>{secret}</strong></div><div><br /><br />Thanks,<br />The eHS System</div>', 1, CAST(N'2020-11-08T15:43:14.0000000' AS DateTime2), CAST(N'2020-11-08T15:43:14.0000000' AS DateTime2), 0,'{officerName} {officerEmail} {customer} {refID} {type} {auditorName} {inspectedOn} {premise} {findings} {draft} {enddraft}')
GO
INSERT [dbo].[EmailTemplate] ([ID], [Type], [From], [Cc], [Bcc], [Title], [Body], [IsBodyHtml], [CreatedOn], [ModifiedOn], [IsDeleted],[Keyword]) VALUES (2, 200, N'keyfe.a@totalebizsolutions.com', NULL, NULL, N'eHS Account Created', N'<div>eHS account</div><div><br /><span style="color: #008355; font-size: 36pt;">Account created</span></div><div>&nbsp;</div><div><br />Welcome to eHS System.</div><div>&nbsp;</div><div>You account {email} has been created with temporary password.</div><div>&nbsp;</div><div>Use this temporary password to login.</div><div>&nbsp;</div><div>&nbsp;</div><div><span style="font-size: 0.875rem;">Here is your account: </span><strong style="font-size: 0.875rem;">{username}</strong></div><div>&nbsp;</div><div>Here is your temporary password: <strong>{secret}</strong></div><div><br /><br />Thanks,<br />The eHS System</div>', 1, CAST(N'2020-11-08T15:55:53.0000000' AS DateTime2), CAST(N'2020-11-08T15:55:53.0000000' AS DateTime2), 0,'{officerName} {officerEmail} {customer} {refID} {type} {auditorName} {inspectedOn} {premise} {findings} {draft} {enddraft}')
GO
SET IDENTITY_INSERT [dbo].[EmailTemplate] OFF
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

INSERT [dbo].[CustomerStatusLookup] ([ID], [Text]) VALUES (0, N'Active')
GO
INSERT [dbo].[CustomerStatusLookup] ([ID], [Text]) VALUES (1, N'Certified')
GO
INSERT [dbo].[CustomerStatusLookup] ([ID], [Text]) VALUES (2, N'Expired')
GO
INSERT [dbo].[CustomerStatusLookup] ([ID], [Text]) VALUES (3, N'InActive')
GO

INSERT [dbo].[IDTypeLookup] ([ID], [Text]) VALUES (0, N'UEN')
GO
INSERT [dbo].[IDTypeLookup] ([ID], [Text]) VALUES (1, N'NRIC')
GO
INSERT [dbo].[IDTypeLookup] ([ID], [Text]) VALUES (2, N'Passport')
GO
INSERT [dbo].[IDTypeLookup] ([ID], [Text]) VALUES (3, N'FIN')
GO

INSERT [dbo].[LogTypeLookup] ([ID], [Text]) VALUES (0, N'Default')
GO
INSERT [dbo].[LogTypeLookup] ([ID], [Text]) VALUES (1, N'Request')
GO
INSERT [dbo].[LogTypeLookup] ([ID], [Text]) VALUES (2, N'RFA')
GO
INSERT [dbo].[LogTypeLookup] ([ID], [Text]) VALUES (3, N'JobOrder')
GO

INSERT [dbo].[PremiseTypeLookup] ([ID], [Text]) VALUES (0, N'Home')
GO
INSERT [dbo].[PremiseTypeLookup] ([ID], [Text]) VALUES (1, N'Outlet')
GO
INSERT [dbo].[PremiseTypeLookup] ([ID], [Text]) VALUES (2, N'Organization')
GO
INSERT [dbo].[PremiseTypeLookup] ([ID], [Text]) VALUES (3, N'Mailing')
GO
INSERT [dbo].[PremiseTypeLookup] ([ID], [Text]) VALUES (4, N'Billing')
GO
INSERT [dbo].[PremiseTypeLookup] ([ID], [Text]) VALUES (5, N'Shipping')
GO
INSERT [dbo].[PremiseTypeLookup] ([ID], [Text]) VALUES (99, N'Others')
GO

INSERT [dbo].[SubSchemeLookup] ([ID], [Text]) VALUES (101, N'Canteen')
GO
INSERT [dbo].[SubSchemeLookup] ([ID], [Text]) VALUES (102, N'Halal Section')
GO
INSERT [dbo].[SubSchemeLookup] ([ID], [Text]) VALUES (103, N'Hawker')
GO
INSERT [dbo].[SubSchemeLookup] ([ID], [Text]) VALUES (104, N'Food Kiosk')
GO
INSERT [dbo].[SubSchemeLookup] ([ID], [Text]) VALUES (105, N'Food Station')
GO
INSERT [dbo].[SubSchemeLookup] ([ID], [Text]) VALUES (106, N'Restaurant')
GO
INSERT [dbo].[SubSchemeLookup] ([ID], [Text]) VALUES (107, N'Short Term')
GO
INSERT [dbo].[SubSchemeLookup] ([ID], [Text]) VALUES (108, N'Snack Bar / Bakery')
GO
INSERT [dbo].[SubSchemeLookup] ([ID], [Text]) VALUES (109, N'Staff Canteen')
GO
INSERT [dbo].[SubSchemeLookup] ([ID], [Text]) VALUES (201, N'Central Kitchen')
GO
INSERT [dbo].[SubSchemeLookup] ([ID], [Text]) VALUES (202, N'Catering')
GO
INSERT [dbo].[SubSchemeLookup] ([ID], [Text]) VALUES (203, N'Pre-School Kitchen')
GO
INSERT [dbo].[SubSchemeLookup] ([ID], [Text]) VALUES (301, N'Product')
GO
INSERT [dbo].[SubSchemeLookup] ([ID], [Text]) VALUES (302, N'Whole Plant')
GO
