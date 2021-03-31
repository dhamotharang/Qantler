INSERT [dbo].[Master] ([ID], [Type], [Value], [ParentID], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (N'34266358-5030-4e05-a1cb-0046e4f683e3', 501, N'BreachOfHCC3', NULL, CAST(N'2021-01-25T10:29:08.0000000' AS DateTime2), CAST(N'2021-01-25T10:29:08.0000000' AS DateTime2), 0)
INSERT [dbo].[Master] ([ID], [Type], [Value], [ParentID], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (N'68542723-5d57-45b0-93fe-15e8cc710cc7', 502, N'MisUse of Halal Logo1', N'c0cfd285-8838-4058-8835-dd88e3d21486', CAST(N'2021-01-26T11:01:19.0000000' AS DateTime2), CAST(N'2021-01-26T11:01:19.0000000' AS DateTime2), 0)
INSERT [dbo].[Master] ([ID], [Type], [Value], [ParentID], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (N'c69c1081-33a1-458c-a3b7-6ccca5a3deb9', 502, N'MisUse of Halal Logo3', N'04fc4680-f2e5-413a-bf75-d87ba0616fbe', CAST(N'2021-01-25T10:29:08.0000000' AS DateTime2), CAST(N'2021-01-25T10:29:08.0000000' AS DateTime2), 0)
INSERT [dbo].[Master] ([ID], [Type], [Value], [ParentID], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (N'1a72466c-b52f-44ce-8b8b-805ef1964369', 502, N'MisUse of Halal Logo4', N'04fc4680-f2e5-413a-bf75-d87ba0616fbe', CAST(N'2021-01-25T10:29:08.0000000' AS DateTime2), CAST(N'2021-01-25T10:29:08.0000000' AS DateTime2), 0)
INSERT [dbo].[Master] ([ID], [Type], [Value], [ParentID], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (N'2262bdd0-317c-496d-a942-afc0a6ecfeca', 501, N'BreachOfHCC4', NULL, CAST(N'2021-01-25T10:29:08.0000000' AS DateTime2), CAST(N'2021-01-25T10:29:08.0000000' AS DateTime2), 0)
INSERT [dbo].[Master] ([ID], [Type], [Value], [ParentID], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (N'04fc4680-f2e5-413a-bf75-d87ba0616fbe', 501, N'BreachOfHCC2', NULL, CAST(N'2021-01-26T11:01:19.0000000' AS DateTime2), CAST(N'2021-01-26T11:01:19.0000000' AS DateTime2), 0)
INSERT [dbo].[Master] ([ID], [Type], [Value], [ParentID], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (N'c0cfd285-8838-4058-8835-dd88e3d21486', 501, N'BreachOfHCC1', NULL, CAST(N'2021-01-26T11:01:19.0000000' AS DateTime2), CAST(N'2021-01-26T11:01:19.0000000' AS DateTime2), 0)
INSERT [dbo].[Master] ([ID], [Type], [Value], [ParentID], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (N'b6a7d76a-a7eb-47a6-b9ee-f3484ba95adf', 502, N'MisUse of Halal Logo2', N'c0cfd285-8838-4058-8835-dd88e3d21486', CAST(N'2021-01-26T11:01:19.0000000' AS DateTime2), CAST(N'2021-01-26T11:01:19.0000000' AS DateTime2), 0)
GO

SET IDENTITY_INSERT [dbo].[EmailTemplate] ON 
GO
INSERT [dbo].[EmailTemplate] ([ID], [Type], [From], [Cc], [Bcc], [Title], [Body], [IsBodyHtml], [CreatedOn], [ModifiedOn], [IsDeleted],[Keyword]) VALUES (1, 400, N'vasu.shanmugam@totalebizsolutions.com', N'', N'{officerEmail}', N'Show case for {customer}', N'<div>Salaam All,<br /><br /> Regards,<br /><strong>Islamic Religious Council of Singapore (Muis)</strong>&nbsp; <br />Tel: 6812 6060 <strong>&bull; </strong>Fax: 6812 6061<br />Visit us at: <a href="http://www.halal.sg/">www.halal.sg</a> <strong>&bull;</strong> Connect with us on:&nbsp;<img src="../../api/file/919b6f8f-1401-49ca-b4b5-5a4f1857b25a?fileName=image001.gif" alt="" width="12" height="12" /></div>
<div><hr /></div>
<div><span style="color: #27ae60; font-size: 8pt; font-family: tahoma, arial, helvetica, sans-serif;"><em>&ldquo;Building a gracious Muslim community of excellence that inspires and radiates blessings to all.&rdquo;</em></span><br /><br /><span style="font-size: 8pt; font-family: tahoma, arial, helvetica, sans-serif;"><em>This email may contain privileged and confidential information. If you receive this email by mistake, you should immediately notify the sender and delete the email. </em></span><br /><span style="font-size: 8pt; font-family: tahoma, arial, helvetica, sans-serif;"><em>Unauthorised communication and disclosure of any information in the email is an offence under the Official Secrets Act (Cap 213).</em></span></div>', 1, CAST(N'2020-09-23T06:30:49.0000000' AS DateTime2), CAST(N'2020-11-18T14:54:18.0000000' AS DateTime2), 0,'{customer} {premise} {reportedOn} {officerName} {officerEmail}')
GO
INSERT [dbo].[EmailTemplate] ([ID], [Type], [From], [Cc], [Bcc], [Title], [Body], [IsBodyHtml], [CreatedOn], [ModifiedOn], [IsDeleted],[Keyword]) VALUES (2, 401, N'vasu.shanmugam@totalebizsolutions.com', N'', N'{officerEmail}', N'Warning case for {customer}', N'<div>Salaam All,<br /><br /> Regards,<br /><strong>Islamic Religious Council of Singapore (Muis)</strong>&nbsp; <br />Tel: 6812 6060 <strong>&bull; </strong>Fax: 6812 6061<br />Visit us at: <a href="http://www.halal.sg/">www.halal.sg</a> <strong>&bull;</strong> Connect with us on:&nbsp;<img src="../../api/file/919b6f8f-1401-49ca-b4b5-5a4f1857b25a?fileName=image001.gif" alt="" width="12" height="12" /></div>
<div><hr /></div>
<div><span style="color: #27ae60; font-size: 8pt; font-family: tahoma, arial, helvetica, sans-serif;"><em>&ldquo;Building a gracious Muslim community of excellence that inspires and radiates blessings to all.&rdquo;</em></span><br /><br /><span style="font-size: 8pt; font-family: tahoma, arial, helvetica, sans-serif;"><em>This email may contain privileged and confidential information. If you receive this email by mistake, you should immediately notify the sender and delete the email. </em></span><br /><span style="font-size: 8pt; font-family: tahoma, arial, helvetica, sans-serif;"><em>Unauthorised communication and disclosure of any information in the email is an offence under the Official Secrets Act (Cap 213).</em></span></div>', 1, CAST(N'2020-09-23T06:30:49.0000000' AS DateTime2), CAST(N'2020-11-18T14:54:18.0000000' AS DateTime2), 0,'{customer} {premise} {reportedOn} {officerName} {officerEmail}')
GO
INSERT [dbo].[EmailTemplate] ([ID], [Type], [From], [Cc], [Bcc], [Title], [Body], [IsBodyHtml], [CreatedOn], [ModifiedOn], [IsDeleted],[Keyword]) VALUES (3, 402, N'vasu.shanmugam@totalebizsolutions.com', N'', N'{officerEmail}', N'Compound case for {customer}', N'<div>Salaam All,<br /><br /> Regards,<br /><strong>Islamic Religious Council of Singapore (Muis)</strong>&nbsp; <br />Tel: 6812 6060 <strong>&bull; </strong>Fax: 6812 6061<br />Visit us at: <a href="http://www.halal.sg/">www.halal.sg</a> <strong>&bull;</strong> Connect with us on:&nbsp;<img src="../../api/file/919b6f8f-1401-49ca-b4b5-5a4f1857b25a?fileName=image001.gif" alt="" width="12" height="12" /></div>
<div><hr /></div>
<div><span style="color: #27ae60; font-size: 8pt; font-family: tahoma, arial, helvetica, sans-serif;"><em>&ldquo;Building a gracious Muslim community of excellence that inspires and radiates blessings to all.&rdquo;</em></span><br /><br /><span style="font-size: 8pt; font-family: tahoma, arial, helvetica, sans-serif;"><em>This email may contain privileged and confidential information. If you receive this email by mistake, you should immediately notify the sender and delete the email. </em></span><br /><span style="font-size: 8pt; font-family: tahoma, arial, helvetica, sans-serif;"><em>Unauthorised communication and disclosure of any information in the email is an offence under the Official Secrets Act (Cap 213).</em></span></div>', 1, CAST(N'2020-09-23T06:30:49.0000000' AS DateTime2), CAST(N'2020-11-18T14:54:18.0000000' AS DateTime2), 0,'{customer} {premise} {reportedOn} {officerName} {officerEmail}')
GO
INSERT [dbo].[EmailTemplate] ([ID], [Type], [From], [Cc], [Bcc], [Title], [Body], [IsBodyHtml], [CreatedOn], [ModifiedOn], [IsDeleted],[Keyword]) VALUES (4, 403, N'vasu.shanmugam@totalebizsolutions.com', N'', N'{officerEmail}', N'Suspension case for {customer}', N'<div>Salaam All,<br /><br /> Regards,<br /><strong>Islamic Religious Council of Singapore (Muis)</strong>&nbsp; <br />Tel: 6812 6060 <strong>&bull; </strong>Fax: 6812 6061<br />Visit us at: <a href="http://www.halal.sg/">www.halal.sg</a> <strong>&bull;</strong> Connect with us on:&nbsp;<img src="../../api/file/919b6f8f-1401-49ca-b4b5-5a4f1857b25a?fileName=image001.gif" alt="" width="12" height="12" /></div>
<div><hr /></div>
<div><span style="color: #27ae60; font-size: 8pt; font-family: tahoma, arial, helvetica, sans-serif;"><em>&ldquo;Building a gracious Muslim community of excellence that inspires and radiates blessings to all.&rdquo;</em></span><br /><br /><span style="font-size: 8pt; font-family: tahoma, arial, helvetica, sans-serif;"><em>This email may contain privileged and confidential information. If you receive this email by mistake, you should immediately notify the sender and delete the email. </em></span><br /><span style="font-size: 8pt; font-family: tahoma, arial, helvetica, sans-serif;"><em>Unauthorised communication and disclosure of any information in the email is an offence under the Official Secrets Act (Cap 213).</em></span></div>', 1, CAST(N'2020-09-23T06:30:49.0000000' AS DateTime2), CAST(N'2020-11-18T14:54:18.0000000' AS DateTime2), 0,'{customer} {premise} {reportedOn} {officerName} {officerEmail}')
GO
INSERT [dbo].[EmailTemplate] ([ID], [Type], [From], [Cc], [Bcc], [Title], [Body], [IsBodyHtml], [CreatedOn], [ModifiedOn], [IsDeleted],[Keyword]) VALUES (5, 404, N'vasu.shanmugam@totalebizsolutions.com', N'', N'{officerEmail}', N'Immediate Suspension case for {customer}', N'<div>Salaam All,<br /><br /> Regards,<br /><strong>Islamic Religious Council of Singapore (Muis)</strong>&nbsp; <br />Tel: 6812 6060 <strong>&bull; </strong>Fax: 6812 6061<br />Visit us at: <a href="http://www.halal.sg/">www.halal.sg</a> <strong>&bull;</strong> Connect with us on:&nbsp;<img src="../../api/file/919b6f8f-1401-49ca-b4b5-5a4f1857b25a?fileName=image001.gif" alt="" width="12" height="12" /></div>
<div><hr /></div>
<div><span style="color: #27ae60; font-size: 8pt; font-family: tahoma, arial, helvetica, sans-serif;"><em>&ldquo;Building a gracious Muslim community of excellence that inspires and radiates blessings to all.&rdquo;</em></span><br /><br /><span style="font-size: 8pt; font-family: tahoma, arial, helvetica, sans-serif;"><em>This email may contain privileged and confidential information. If you receive this email by mistake, you should immediately notify the sender and delete the email. </em></span><br /><span style="font-size: 8pt; font-family: tahoma, arial, helvetica, sans-serif;"><em>Unauthorised communication and disclosure of any information in the email is an offence under the Official Secrets Act (Cap 213).</em></span></div>', 1, CAST(N'2020-09-23T06:30:49.0000000' AS DateTime2), CAST(N'2020-11-18T14:54:18.0000000' AS DateTime2), 0,'{customer} {premise} {reportedOn} {officerName} {officerEmail}')
GO
INSERT [dbo].[EmailTemplate] ([ID], [Type], [From], [Cc], [Bcc], [Title], [Body], [IsBodyHtml], [CreatedOn], [ModifiedOn], [IsDeleted],[Keyword]) VALUES (6, 405, N'vasu.shanmugam@totalebizsolutions.com', N'', N'{officerEmail}', N'Revocation case for {customer}', N'<div>Salaam All,<br /><br /> Regards,<br /><strong>Islamic Religious Council of Singapore (Muis)</strong>&nbsp; <br />Tel: 6812 6060 <strong>&bull; </strong>Fax: 6812 6061<br />Visit us at: <a href="http://www.halal.sg/">www.halal.sg</a> <strong>&bull;</strong> Connect with us on:&nbsp;<img src="../../api/file/919b6f8f-1401-49ca-b4b5-5a4f1857b25a?fileName=image001.gif" alt="" width="12" height="12" /></div>
<div><hr /></div>
<div><span style="color: #27ae60; font-size: 8pt; font-family: tahoma, arial, helvetica, sans-serif;"><em>&ldquo;Building a gracious Muslim community of excellence that inspires and radiates blessings to all.&rdquo;</em></span><br /><br /><span style="font-size: 8pt; font-family: tahoma, arial, helvetica, sans-serif;"><em>This email may contain privileged and confidential information. If you receive this email by mistake, you should immediately notify the sender and delete the email. </em></span><br /><span style="font-size: 8pt; font-family: tahoma, arial, helvetica, sans-serif;"><em>Unauthorised communication and disclosure of any information in the email is an offence under the Official Secrets Act (Cap 213).</em></span></div>', 1, CAST(N'2020-09-23T06:30:49.0000000' AS DateTime2), CAST(N'2020-11-18T14:54:18.0000000' AS DateTime2), 0,'{customer} {premise} {reportedOn} {officerName} {officerEmail}')
GO
SET IDENTITY_INSERT [dbo].[EmailTemplate] OFF
GO

SET IDENTITY_INSERT [dbo].[LetterTemplate] ON 
GO
INSERT [dbo].[LetterTemplate] ([ID], [Type], [Body], [CreatedOn], [ModifiedOn], [IsDeleted],[Keyword]) VALUES (1, 400, N'', CAST(N'2020-09-23T06:30:49.0000000' AS DateTime2), CAST(N'2020-11-18T14:54:18.0000000' AS DateTime2), 0,'{customer} {premise} {reportedOn} {officerName} {officerEmail}')
GO
INSERT [dbo].[LetterTemplate] ([ID], [Type], [Body], [CreatedOn], [ModifiedOn], [IsDeleted],[Keyword]) VALUES (2, 401, N'', CAST(N'2020-09-23T06:30:49.0000000' AS DateTime2), CAST(N'2020-11-18T14:54:18.0000000' AS DateTime2), 0,'{customer} {premise} {reportedOn} {officerName} {officerEmail}')
GO
INSERT [dbo].[LetterTemplate] ([ID], [Type], [Body], [CreatedOn], [ModifiedOn], [IsDeleted],[Keyword]) VALUES (3, 402, N'', CAST(N'2020-09-23T06:30:49.0000000' AS DateTime2), CAST(N'2020-11-18T14:54:18.0000000' AS DateTime2), 0,'{customer} {premise} {reportedOn} {officerName} {officerEmail}')
GO
INSERT [dbo].[LetterTemplate] ([ID], [Type], [Body], [CreatedOn], [ModifiedOn], [IsDeleted],[Keyword]) VALUES (4, 403, N'', CAST(N'2020-09-23T06:30:49.0000000' AS DateTime2), CAST(N'2020-11-18T14:54:18.0000000' AS DateTime2), 0,'{customer} {premise} {reportedOn} {officerName} {officerEmail}')
GO
INSERT [dbo].[LetterTemplate] ([ID], [Type], [Body], [CreatedOn], [ModifiedOn], [IsDeleted],[Keyword]) VALUES (5, 404, N'', CAST(N'2020-09-23T06:30:49.0000000' AS DateTime2), CAST(N'2020-11-18T14:54:18.0000000' AS DateTime2), 0,'{customer} {premise} {reportedOn} {officerName} {officerEmail}')
GO
INSERT [dbo].[LetterTemplate] ([ID], [Type], [Body], [CreatedOn], [ModifiedOn], [IsDeleted],[Keyword]) VALUES (6, 405, N'', CAST(N'2020-09-23T06:30:49.0000000' AS DateTime2), CAST(N'2020-11-18T14:54:18.0000000' AS DateTime2), 0,'{customer} {premise} {reportedOn} {officerName} {officerEmail}')
GO
INSERT [dbo].[LetterTemplate] ([ID], [Type], [Body], [CreatedOn], [ModifiedOn], [IsDeleted],[Keyword]) VALUES (7, 406, N'', CAST(N'2020-09-23T06:30:49.0000000' AS DateTime2), CAST(N'2020-11-18T14:54:18.0000000' AS DateTime2), 0,'{customer} {premise} {reportedOn} {officerName} {officerEmail}')
GO
INSERT [dbo].[LetterTemplate] ([ID], [Type], [Body], [CreatedOn], [ModifiedOn], [IsDeleted],[Keyword]) VALUES (8, 407, N'', CAST(N'2020-09-23T06:30:49.0000000' AS DateTime2), CAST(N'2020-11-18T14:54:18.0000000' AS DateTime2), 0,'{customer} {premise} {reportedOn} {officerName} {officerEmail}')
GO
INSERT [dbo].[LetterTemplate] ([ID], [Type], [Body], [CreatedOn], [ModifiedOn], [IsDeleted],[Keyword]) VALUES (9, 408, N'', CAST(N'2020-09-23T06:30:49.0000000' AS DateTime2), CAST(N'2020-11-18T14:54:18.0000000' AS DateTime2), 0,'{customer} {premise} {reportedOn} {officerName} {officerEmail}')
GO
SET IDENTITY_INSERT [dbo].[LetterTemplate] OFF
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

INSERT [dbo].[BillTypeLookup] ([ID], [Text]) VALUES (0, N'Stage 1')
GO
INSERT [dbo].[BillTypeLookup] ([ID], [Text]) VALUES (1, N'Stage 2')
GO
INSERT [dbo].[BillTypeLookup] ([ID], [Text]) VALUES (2, N'Composition Sum')
GO