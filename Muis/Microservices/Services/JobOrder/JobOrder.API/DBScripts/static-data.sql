INSERT [dbo].[SettingsLookup] ([ID], [Text]) VALUES (200, N'PeriodicHawker')
GO
INSERT [dbo].[SettingsLookup] ([ID], [Text]) VALUES (201, N'PeriodicCanteen')
GO
INSERT [dbo].[SettingsLookup] ([ID], [Text]) VALUES (202, N'PeriodicRestaurant')
GO
INSERT [dbo].[SettingsLookup] ([ID], [Text]) VALUES (203, N'PeriodicHalalSection')
GO
INSERT [dbo].[SettingsLookup] ([ID], [Text]) VALUES (204, N'PeriodicFoodKiosk')
GO
INSERT [dbo].[SettingsLookup] ([ID], [Text]) VALUES (205, N'PeriodicFoodStation')
GO
INSERT [dbo].[SettingsLookup] ([ID], [Text]) VALUES (206, N'PeriodicSnakBarBakery')
GO
INSERT [dbo].[SettingsLookup] ([ID], [Text]) VALUES (207, N'PeriodicShotTerm')
GO
INSERT [dbo].[SettingsLookup] ([ID], [Text]) VALUES (208, N'PeriodicCentralKitchen')
GO
INSERT [dbo].[SettingsLookup] ([ID], [Text]) VALUES (209, N'PeriodicCatering')
GO
INSERT [dbo].[SettingsLookup] ([ID], [Text]) VALUES (210, N'PeriodicPreSchoolKitchen')
GO
INSERT [dbo].[SettingsLookup] ([ID], [Text]) VALUES (211, N'PeriodicProduct')
GO
INSERT [dbo].[SettingsLookup] ([ID], [Text]) VALUES (212, N'PeriodicWholePlant')
GO
INSERT [dbo].[SettingsLookup] ([ID], [Text]) VALUES (213, N'PeriodicPolutry')
GO
INSERT [dbo].[SettingsLookup] ([ID], [Text]) VALUES (214, N'PeriodicEndorsement')
GO
INSERT [dbo].[SettingsLookup] ([ID], [Text]) VALUES (215, N'PeriodicStorageFacility')
GO
INSERT [dbo].[SettingsLookup] ([ID], [Text]) VALUES (216, N'PeriodicHighPriority')
GO
INSERT [dbo].[SettingsLookup] ([ID], [Text]) VALUES (217, N'PeriodicGradingA')
GO
INSERT [dbo].[SettingsLookup] ([ID], [Text]) VALUES (218, N'PeriodicGradingB')
GO
INSERT [dbo].[SettingsLookup] ([ID], [Text]) VALUES (219, N'PeriodicGradingC')
GO

INSERT [dbo].[Locale] ([ID], [Text], [Code]) VALUES (0, N'English', N'en')
GO

SET IDENTITY_INSERT [dbo].[Translations] ON 
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (1, 0, N'UpdatedJobOrderSettings', N'updated {0} from {1} to {2}')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (2, 0, N'JobOrderCreate', N'scheduled the inspection.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (3, 0, N'RescheduledInspection', N're-scheduled from {0} to {1}')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (4, 0, N'ScheduledInspection', N'scheduled an inspection on {0}.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (5, 0, N'InviteSentTitle', N'Job Order')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (6, 0, N'InviteSentBody', N'You have been invited to support site inspection for job order {0}.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (7, 0, N'InviteAddLog', N'sent invite to {0}.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (8, 0, N'InviteRemoveLog', N'removed invite for {0}.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (9, 0, N'InviteRemoveTitle', N'Job Order')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (10, 0, N'InviteRemoveBody', N'Site inpsection support invite has been cancelled for job order {0}.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (11, 0, N'JobOrderCancel', N'cancelled the job order.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (12, 0, N'PeriodicSchedulerTitle', N'Periodic Inspection Scheduled')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (13, 0, N'PeriodicSchedulerBody', N'A periodic inspection scheduled on 28 Jan. 2021 has been created. And it''s pending for further action.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (14, 0, N'ScheduledPIInspection', N'scheduled a periodic inspection on {0}.')
GO
SET IDENTITY_INSERT [dbo].[Translations] OFF
GO

INSERT [dbo].[MasterTypeLookup] ([ID], [Text]) VALUES (200, N'RescheduleReason')
GO
INSERT [dbo].[MasterTypeLookup] ([ID], [Text]) VALUES (201, N'CancelReason')
GO

INSERT [dbo].[JobOrderStatusLookup] ([ID], [Text]) VALUES (100, N'Draft')
GO
INSERT [dbo].[JobOrderStatusLookup] ([ID], [Text]) VALUES (200, N'Pending')
GO
INSERT [dbo].[JobOrderStatusLookup] ([ID], [Text]) VALUES (300, N'Work in Progress')
GO
INSERT [dbo].[JobOrderStatusLookup] ([ID], [Text]) VALUES (400, N'Done')
GO
INSERT [dbo].[JobOrderStatusLookup] ([ID], [Text]) VALUES (500, N'Cancelled')
GO
INSERT [dbo].[JobOrderStatusLookup] ([ID], [Text]) VALUES (600, N'Expired')
GO
INSERT [dbo].[JobOrderStatusLookup] ([ID], [Text]) VALUES (700, N'Closed')
GO

INSERT [dbo].[JobOrderTypeLookup] ([ID], [Text]) VALUES (0, N'Audit Site Inspection')
GO
INSERT [dbo].[JobOrderTypeLookup] ([ID], [Text]) VALUES (1, N'Periodic Site Inspection')
GO
INSERT [dbo].[JobOrderTypeLookup] ([ID], [Text]) VALUES (2, N'Enforcement Case Inspection')
GO
INSERT [dbo].[JobOrderTypeLookup] ([ID], [Text]) VALUES (3, N'Reinstate Inspection')
GO

INSERT [dbo].[LogTypeLookup] ([ID], [Text]) VALUES (0, N'Default')
GO
INSERT [dbo].[LogTypeLookup] ([ID], [Text]) VALUES (1, N'Request')
GO
INSERT [dbo].[LogTypeLookup] ([ID], [Text]) VALUES (2, N'RFA')
GO
INSERT [dbo].[LogTypeLookup] ([ID], [Text]) VALUES (3, N'JobOrder')
GO

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

INSERT [dbo].[PeriodicSchedulerStatusLookup] ([ID], [Text]) VALUES (100, N'Pending')
GO
INSERT [dbo].[PeriodicSchedulerStatusLookup] ([ID], [Text]) VALUES (200, N'Scheduled')
GO
INSERT [dbo].[PeriodicSchedulerStatusLookup] ([ID], [Text]) VALUES (300, N'Suspended')
GO

INSERT [dbo].[CertificateStatusLookup] ([ID], [Text]) VALUES (100, N'Active')
GO
INSERT [dbo].[CertificateStatusLookup] ([ID], [Text]) VALUES (200, N'Cancelled')
GO
INSERT [dbo].[CertificateStatusLookup] ([ID], [Text]) VALUES (300, N'Invalid')
GO
INSERT [dbo].[CertificateStatusLookup] ([ID], [Text]) VALUES (400, N'Expired')
GO
INSERT [dbo].[CertificateStatusLookup] ([ID], [Text]) VALUES (500, N'Suspended')
GO

INSERT [dbo].[EmailTemplateTypeLookup] ([ID], [Text]) values(300,'Audit Inspection')
GO
INSERT [dbo].[EmailTemplateTypeLookup] ([ID], [Text]) values(301,'Reschedule Audit Inspection')
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

INSERT [dbo].[IDTypeLookup] ([ID], [Text]) VALUES (0, N'UEN')
GO
INSERT [dbo].[IDTypeLookup] ([ID], [Text]) VALUES (1, N'NRIC')
GO
INSERT [dbo].[IDTypeLookup] ([ID], [Text]) VALUES (2, N'Passport')
GO
INSERT [dbo].[IDTypeLookup] ([ID], [Text]) VALUES (3, N'FIN')
GO

INSERT [dbo].[JobOrderActionLookup] ([ID], [Text]) VALUES (300, N'Work In Progress')
GO
INSERT [dbo].[JobOrderActionLookup] ([ID], [Text]) VALUES (400, N'Done')
GO