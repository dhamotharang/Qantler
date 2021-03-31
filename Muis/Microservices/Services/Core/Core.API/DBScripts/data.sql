INSERT INTO [dbo].[PremiseTypeLookup] VALUES (0, 'Home')
GO
INSERT INTO [dbo].[PremiseTypeLookup] VALUES (1, 'Outlet')
GO
INSERT INTO [dbo].[PremiseTypeLookup] VALUES (2, 'Organization')
GO
INSERT INTO [dbo].[PremiseTypeLookup] VALUES (3, 'Mailing')
GO
INSERT INTO [dbo].[PremiseTypeLookup] VALUES (4, 'Billing')
GO
INSERT INTO [dbo].[PremiseTypeLookup] VALUES (5, 'Shipping')
GO
INSERT INTO [dbo].[PremiseTypeLookup] VALUES (6, 'Office')
GO
INSERT INTO [dbo].[PremiseTypeLookup] VALUES (7, 'Show Room')
GO
INSERT INTO [dbo].[PremiseTypeLookup] VALUES (8, 'Warehouse')
GO
INSERT INTO [dbo].[PremiseTypeLookup] VALUES (99, 'Others')
GO

INSERT INTO [dbo].[SchemeLookup] VALUES (100, 'Eating Establishment')
GO
INSERT INTO [dbo].[SchemeLookup] VALUES (200, 'Food Preparation Area')
GO
INSERT INTO [dbo].[SchemeLookup] VALUES (300, 'Food Manufacturing')
GO
INSERT INTO [dbo].[SchemeLookup] VALUES (400, 'Poultry')
GO
INSERT INTO [dbo].[SchemeLookup] VALUES (500, 'Endorsement')
GO
INSERT INTO [dbo].[SchemeLookup] VALUES (600, 'Storage Facility')
GO

INSERT INTO [dbo].[SubSchemeLookup] VALUES (101, 'Canteen')
GO
INSERT INTO [dbo].[SubSchemeLookup] VALUES (102, 'Halal Section')
GO
INSERT INTO [dbo].[SubSchemeLookup] VALUES (103, 'Hawker')
GO
INSERT INTO [dbo].[SubSchemeLookup] VALUES (104, 'Food Kiosk')
GO
INSERT INTO [dbo].[SubSchemeLookup] VALUES (105, 'Food Station')
GO
INSERT INTO [dbo].[SubSchemeLookup] VALUES (106, 'Restaurant')
GO
INSERT INTO [dbo].[SubSchemeLookup] VALUES (107, 'Short Term')
GO
INSERT INTO [dbo].[SubSchemeLookup] VALUES (108, 'Snack Bar / Bakery')
GO
INSERT INTO [dbo].[SubSchemeLookup] VALUES (109, 'Staff Canteen')
GO
INSERT INTO [dbo].[SubSchemeLookup] VALUES (201, 'Central Kitchen')
GO
INSERT INTO [dbo].[SubSchemeLookup] VALUES (202, 'Catering')
GO
INSERT INTO [dbo].[SubSchemeLookup] VALUES (203, 'Pre-School Kitchen')
GO
INSERT INTO [dbo].[SubSchemeLookup] VALUES (301, 'Product')
GO
INSERT INTO [dbo].[SubSchemeLookup] VALUES (302, 'Whole Plant')
GO

INSERT INTO [dbo].[Locale] VALUES (0, 'English', 'en')
GO

INSERT INTO [dbo].[EscalateStatusLookup] VALUES (0, 'Default')
GO
INSERT INTO [dbo].[EscalateStatusLookup] VALUES (100, 'Open')
GO
INSERT INTO [dbo].[EscalateStatusLookup] VALUES (200, 'Closed')
GO

INSERT INTO [dbo].[LogTypeLookup] VALUES (0, 'Default')
GO
INSERT INTO [dbo].[LogTypeLookup] VALUES (1, 'Request')
GO
INSERT INTO [dbo].[LogTypeLookup] VALUES (2, 'RFA')
GO
INSERT INTO [dbo].[LogTypeLookup] VALUES (3, 'JobOrder')
GO
INSERT INTO [dbo].[LogTypeLookup] VALUES (4, 'Finance')
GO
