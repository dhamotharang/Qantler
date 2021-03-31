INSERT [dbo].[CategoryLookup] ([ID], [Text]) VALUES (0, N'Push Notification')
GO
INSERT [dbo].[CategoryLookup] ([ID], [Text]) VALUES (1, N'Email')
GO

INSERT [dbo].[ContentTypeLookup] ([ID], [Text]) VALUES (0, N'Text')
GO
INSERT [dbo].[ContentTypeLookup] ([ID], [Text]) VALUES (1, N'Html')
GO

INSERT [dbo].[LevelLookup] ([ID], [Text]) VALUES (0, N'Info')
GO
INSERT [dbo].[LevelLookup] ([ID], [Text]) VALUES (1, N'Warning')
GO
INSERT [dbo].[LevelLookup] ([ID], [Text]) VALUES (2, N'Danger')
GO

INSERT [dbo].[StateLookup] ([ID], [Text]) VALUES (0, N'Unread')
GO
INSERT [dbo].[StateLookup] ([ID], [Text]) VALUES (1, N'Read')
GO
INSERT [dbo].[StateLookup] ([ID], [Text]) VALUES (2, N'Removed')
GO
