INSERT [dbo].[RiskCategoryLookup] ([ID], [Text]) VALUES (100, N'Low')
GO
INSERT [dbo].[RiskCategoryLookup] ([ID], [Text]) VALUES (200, N'Medium-Low Risk')
GO
INSERT [dbo].[RiskCategoryLookup] ([ID], [Text]) VALUES (300, N'Medium-High Risk')
GO
INSERT [dbo].[RiskCategoryLookup] ([ID], [Text]) VALUES (400, N'High Risk')
GO
INSERT [dbo].[RiskCategoryLookup] ([ID], [Text]) VALUES (500, N'Non-Halal')
GO
INSERT [dbo].[RiskCategoryLookup] ([ID], [Text]) VALUES (999, N'Uncategorized')
GO

INSERT [dbo].[IngredientStatusLookup] ([ID], [Text]) VALUES (0, N'Unverified')
GO
INSERT [dbo].[IngredientStatusLookup] ([ID], [Text]) VALUES (1, N'Verified')
GO

INSERT [dbo].[CertifyingBodyStatusLookup] ([ID], [Text]) VALUES (0, N'Unrecognized')
GO
INSERT [dbo].[CertifyingBodyStatusLookup] ([ID], [Text]) VALUES (1, N'Recognized')
GO

SET IDENTITY_INSERT [dbo].[Translations] ON 
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (1, 0, N'ValidateIngredient', N'Ingredient already exists.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (2, 0, N'ValidateCertifyingBody', N'Certifying Body already exists.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (3, 0, N'ValidateSupplier', N'Supplier already exists.')
GO
SET IDENTITY_INSERT [dbo].[Translations] OFF

GO
INSERT [dbo].[Locale] ([ID], [Text], [Code]) VALUES (0, N'English', N'en')
GO