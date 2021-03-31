INSERT [dbo].[Locale] ([ID], [Text], [Code]) VALUES (0, N'English', N'en')
GO

SET IDENTITY_INSERT [dbo].[Translations] ON 
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (1, 0, N'ScheduledInspection', N'Scheduled Site Inspection')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (2, 0, N'ScheduledInspectionCompleted', N'Inspection has been Completed')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (3, 0, N'ScheduledInspectionCancelled', N'Inspection has been Cancelled')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (4, 0, N'LetterStatusDraft', N'Drafting Show Cause Letter')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (5, 0, N'LetterStatusFinal', N'Show Cause Letter Sent')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (6, 0, N'AcknowledgeShowCause', N'Offender Acknowledged')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (7, 0, N'FOCStatusDraft', N'Drafting Fact of Case')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (8, 0, N'FOCStatusFinal', N'Submitted FOC for Approval')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (9, 0, N'InviteFOCFinalTitle', N'FOC for Approval.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (10, 0, N'InviteFOCFinalBody', N'A Fact of Case draft has been submitted for your approval.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (11, 0, N'FOCApproverDraft', N'Reviewing Fact of Case')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (12, 0, N'FOCApprove', N'Approved the Fact of Case')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (13, 0, N'FOCApproverRevert', N'Reverted Fact of Case')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (14, 0, N'InviteFOCRevertTitle', N'FOC Reverted')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (15, 0, N'InviteFOCRevertBody', N'A Fact of Case has been re-assigned back to you. Notes: {0}.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (16, 0, N'InviteFOCApproveTitle', N'FOC Approved.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (17, 0, N'InviteFOCApproveBody', N'Fact of Case has been approved by {0}.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (18, 0, N'FOCDecision', N'Fact of Case Decision')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (19, 0, N'SuspensionDraftLetter', N'Drafting Suspension Letter')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (20, 0, N'RevocationDraftLetter', N'DraftingRevocation Letter')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (21, 0, N'WarningDraftLetter', N'Drafting Warning Letter')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (22, 0, N'CompoundDraftLetter', N'Drafting Compound Letter')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (23, 0, N'SuspensionFinalLetter', N'Suspension Letter Sent')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (24, 0, N'RevocationFinalLetter', N'Revocation Letter Sent')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (25, 0, N'WarningFinalLetter', N'Warning Letter Sent')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (26, 0, N'CompoundFinalLetter', N'Compound Letter Sent')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (27, 0, N'ImmediateSuspensionFinalLetter', N'Immediate Suspension Letter Sent')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (28, 0, N'AddPayment', N'Added Payment')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (29, 0, N'AddPaymentNote', N'<span class="font-weight-bold">Bank Account Name</span> : {0}<br><span class="font-weight-bold">Amount</span> : {1} <br><br>{2}')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (30, 0, N'PaymentProcessed', N'Payment has been processed by finance')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (31, 0, N'PaymentRejected', N'Payment has been rejected by finance')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (32, 0, N'InviteCompositionPaymentTitle', N'Composition Sum Payment')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (33, 0, N'CertifcateReceived', N'Certificate Received')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (34, 0, N'ScheduledInspectionNote', N'<span class="font-weight-bold">Permise</span> : {0}<br><span class="font-weight-bold">Scheduled Date</span> : {1}<br><span class="font-weight-bold">Start Time</span> : {2}<br><span class="font-weight-bold">End Time</span> : {3}<br><br>{4}')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (35, 0, N'CertifcateReceivedNote', N'I, {0}, hereby acknowledge that I have returned the original Muis Halal certificate(s), the details of which are listed above, on behalf of my company / establishment <u>{1}</u> as a result of the {2} that has been imposed upon us.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (36, 0, N'ReinstateRevoke', N'Reinstate decision to revoke certificate')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (37, 0, N'ReinstateRevokeNote', N'<span class="font-weight-bold">Decision</span> : Revocation<br><br>{0}')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (38, 0, N'ReinstateSuspension', N'Reinstate decision to extend certificate suspension')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (39, 0, N'ReinstateSuspensionNote', N'<span class="font-weight-bold">Decision</span> : Extend Suspension<br><span class="font-weight-bold">Months</span> : {0}<br><br>{1}')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (40, 0, N'ReinstateClosed', N'Case has been closed')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (41, 0, N'SuspensionSanctionNote', N'<span class="font-weight-bold">Decision</span> : Suspension<br><span class="font-weight-bold">Months</span> : {0}<br><br>{1}')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (42, 0, N'CompoundSanctionNote', N'<span class="font-weight-bold">Decision</span> : Composition Sum<br><span class="font-weight-bold">Amount</span> : {0}<br><br>{1}')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (43, 0, N'WarningSanctionNote', N'<span class="font-weight-bold">Decision</span> : Warning<br><br>{0}')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (44, 0, N'RevocationSanctionNote', N'<span class="font-weight-bold">Decision</span> : Revocation<br><br>{0}')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (45, 0, N'DismissedSanctionNote', N'<span class="font-weight-bold">Decision</span> : Dismissed<br><br>{0}')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (46, 0, N'SubmitAppeal', N'Submitted an Appeal')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (47, 0, N'AppealWarningNote', N'<span class="font-weight-bold">Decision</span> : Warning<br><br>{0}')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (48, 0, N'AppealCompoundNote', N'<span class="font-weight-bold">Decision</span> : Composition Sum<br><span class="font-weight-bold">Amount</span> : {0}<br><br>{1}')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (49, 0, N'AppealSuspensionNote', N'<span class="font-weight-bold">Decision</span> : Suspension<br><span class="font-weight-bold">Months</span> : {0}<br><br>{1}')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (50, 0, N'AppealReinstateNote', N'<span class="font-weight-bold">Decision</span> : Reinstate<br><br>{0}')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (51, 0, N'AppealRejectedNote', N'<span class="font-weight-bold">Decision</span> : Appeal Rejected<br><br>{0}')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (52, 0, N'AppealDecision', N'Appeal Decision')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (53, 0, N'DraftApprovedAppeal', N'Drafting Approved Appeal Letter')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (54, 0, N'DraftRejectedAppeal', N'Drafting Rejected Appeal Letter')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (55, 0, N'ApprovedAppealSent', N'Approved Appeal Letter Sent')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (56, 0, N'RejectedAppealSent', N'Rejected Appeal Sent')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (57, 0, N'FileCaseToCourt', N'Submitted case to court for proceedings')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (58, 0, N'CaseVerdict', N'Court Verdict')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (59, 0, N'CaseDismissed', N'Case Dismissed')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (60, 0, N'CaseImmediateSuspension', N'Immediate Suspension')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (61, 0, N'ImmediateSuspensionSanctionNote', N'<span class="font-weight-bold">Decision</span> : Immediate Suspension<br><br>{0}')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (62, 0, N'ImmediateSuspensionDraftLetter', N'Drafting Immediate Suspension Letter')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (63, 0, N'ReInitSanctionNote', N'<span class="font-weight-bold">Decision</span> : Reinstate<br><br>{0}')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (64, 0, N'ReinstateDecision', N'Reinstate decision to Reinstate')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (65, 0, N'ReinstateDecisionNote', N'<span class="font-weight-bold">Decision</span> : Reinstate<br><br>{0}')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (66, 0, N'FOCCaseFileStatusDraft', N'Drafting Case File')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (67, 0, N'FOCCaseFileStatusFinal', N'Submitted Case File')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (68, 0, N'CertifcateReinstated', N'Certificate reinstated.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (69, 0, N'CertifcateReinstateNote', N'I, {0}, hereby acknowledge that I have received the original Muis Halal certificate(s), the details of which are listed above, on behalf of my company / establishment  <u>{1}</u>. We note that {2} marks the end of the suspension period imposed upon us.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (70, 0, N'CaseReopen', N'Reopened the case.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (71, 0, N'CaseClose', N'Closed the case.')

SET IDENTITY_INSERT [dbo].[Translations] OFF 
GO

INSERT [dbo].[ActivityTypeLookup] ([ID], [Text]) VALUES (0, N'Default')
GO
INSERT [dbo].[ActivityTypeLookup] ([ID], [Text]) VALUES (1, N'JobOrder')
GO
INSERT [dbo].[ActivityTypeLookup] ([ID], [Text]) VALUES (2, N'Payment')
GO
INSERT [dbo].[ActivityTypeLookup] ([ID], [Text]) VALUES (3, N'Show Cause Letter')
GO
INSERT [dbo].[ActivityTypeLookup] ([ID], [Text]) VALUES (4, N'FOC Letter')
GO
INSERT [dbo].[ActivityTypeLookup] ([ID], [Text]) VALUES (5, N'Sanction Letter')
GO
INSERT [dbo].[ActivityTypeLookup] ([ID], [Text]) VALUES (6, N'Appeal Outcome Letter')
GO

INSERT [dbo].[CaseStatusLookup] ([ID], [Text]) VALUES (100, N'Open')
GO
INSERT [dbo].[CaseStatusLookup] ([ID], [Text]) VALUES (200, N'Pending Inspection')
GO
INSERT [dbo].[CaseStatusLookup] ([ID], [Text]) VALUES (300, N'Pending Show Cause')
GO
INSERT [dbo].[CaseStatusLookup] ([ID], [Text]) VALUES (400, N'Show Cause For Approval')
GO
INSERT [dbo].[CaseStatusLookup] ([ID], [Text]) VALUES (500, N'Pending Acknowledgement')
GO
INSERT [dbo].[CaseStatusLookup] ([ID], [Text]) VALUES (600, N'Pending  FOC')
GO
INSERT [dbo].[CaseStatusLookup] ([ID], [Text]) VALUES (700, N'FOC For Approval')
GO
INSERT [dbo].[CaseStatusLookup] ([ID], [Text]) VALUES (750,N'Pending FOC Decision')
GO
INSERT [dbo].[CaseStatusLookup] ([ID], [Text]) VALUES (800, N'Pending Sanction Letter')
GO
INSERT [dbo].[CaseStatusLookup] ([ID], [Text]) VALUES (900, N'Pending Payment')
GO
INSERT [dbo].[CaseStatusLookup] ([ID], [Text]) VALUES (1000, N'Certificate Collection')
GO
INSERT [dbo].[CaseStatusLookup] ([ID], [Text]) VALUES (1100, N'Pending Reinstate Inspection')
GO
INSERT [dbo].[CaseStatusLookup] ([ID], [Text]) VALUES (1200, N'Pending Appeal')
GO
INSERT [dbo].[CaseStatusLookup] ([ID], [Text]) VALUES (1210, N'Pending Approved Appeal Letter')
GO
INSERT [dbo].[CaseStatusLookup] ([ID], [Text]) VALUES (1220, N'Pending Rejected Appeal Letter')
GO
INSERT [dbo].[CaseStatusLookup] ([ID], [Text]) VALUES (1250, N'Proceeding InProgress')
GO
INSERT [dbo].[CaseStatusLookup] ([ID], [Text]) VALUES (1300, N'Closed')
GO
INSERT [dbo].[CaseStatusLookup] ([ID], [Text]) VALUES (1400, N'Dismissed')
GO
INSERT [dbo].[CaseStatusLookup] ([ID], [Text]) VALUES (1275, N'Reinstate Certificate')
GO

INSERT [dbo].[CaseStatusMinorLookup] ([ID], [Text]) VALUES (210, N'InspectionDone')
GO
INSERT [dbo].[CaseStatusMinorLookup] ([ID], [Text]) VALUES (220, N'InspectionInProgress')
GO
INSERT [dbo].[CaseStatusMinorLookup] ([ID], [Text]) VALUES (230, N'InspectionCancelled')
GO
INSERT [dbo].[CaseStatusMinorLookup] ([ID], [Text]) VALUES (810, N'SanctionLetterSent')
GO
INSERT [dbo].[CaseStatusMinorLookup] ([ID], [Text]) VALUES (910, N'PaymentReceived')
GO
INSERT [dbo].[CaseStatusMinorLookup] ([ID], [Text]) VALUES (920, N'PaymentProcessed')
GO
INSERT [dbo].[CaseStatusMinorLookup] ([ID], [Text]) VALUES (930, N'PaymentRejected')
GO
INSERT [dbo].[CaseStatusMinorLookup] ([ID], [Text]) VALUES (1010, N'CertificateCollected')
GO

INSERT [dbo].[CaseTypeLookup] ([ID], [Text]) VALUES (0, N'Enforcement')
GO

INSERT [dbo].[IDTypeLookup] ([ID], [Text]) VALUES (0, N'UEN')
GO
INSERT [dbo].[IDTypeLookup] ([ID], [Text]) VALUES (1, N'NRIC')
GO
INSERT [dbo].[IDTypeLookup] ([ID], [Text]) VALUES (2, N'Passport')
GO
INSERT [dbo].[IDTypeLookup] ([ID], [Text]) VALUES (3, N'FIN')
GO

INSERT [dbo].[LetterTypeLookup] ([ID], [Text]) VALUES (400, N'Show Cause')
GO
INSERT [dbo].[LetterTypeLookup] ([ID], [Text]) VALUES (401, N'FOC')
GO
INSERT [dbo].[LetterTypeLookup] ([ID], [Text]) VALUES (402, N'Warning')
GO
INSERT [dbo].[LetterTypeLookup] ([ID], [Text]) VALUES (403, N'Compound')
GO
INSERT [dbo].[LetterTypeLookup] ([ID], [Text]) VALUES (404, N'Suspension')
GO
INSERT [dbo].[LetterTypeLookup] ([ID], [Text]) VALUES (405, N'Immediate Suspension')
GO
INSERT [dbo].[LetterTypeLookup] ([ID], [Text]) VALUES (406, N'Revocation')
GO
INSERT [dbo].[LetterTypeLookup] ([ID], [Text]) VALUES (407, N'Approved Appeal')
GO
INSERT [dbo].[LetterTypeLookup] ([ID], [Text]) VALUES (408, N'Rejected Appeal')
GO

INSERT [dbo].[MasterTypeLookup] ([ID], [Text]) VALUES (501, N'Breach Category')
GO
INSERT [dbo].[MasterTypeLookup] ([ID], [Text]) VALUES (502, N'Offence')
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

INSERT [dbo].[SanctionInfoTypeLookup] ([ID], [Text]) VALUES (0, N'Recommended')
GO
INSERT [dbo].[SanctionInfoTypeLookup] ([ID], [Text]) VALUES (1, N'Final')
GO

INSERT [dbo].[SanctionLookup] ([ID], [Text]) VALUES (0, N'Warning')
GO
INSERT [dbo].[SanctionLookup] ([ID], [Text]) VALUES (1, N'Compound')
GO
INSERT [dbo].[SanctionLookup] ([ID], [Text]) VALUES (2, N'Suspension')
GO
INSERT [dbo].[SanctionLookup] ([ID], [Text]) VALUES (3, N'Immediate Suspension')
GO
INSERT [dbo].[SanctionLookup] ([ID], [Text]) VALUES (4, N'Revocation')
GO
INSERT [dbo].[SanctionLookup] ([ID], [Text]) VALUES (5, N'Dismissed')
GO
INSERT [dbo].[SanctionLookup] ([ID], [Text]) VALUES (6, N'Closed')
GO
INSERT [dbo].[SanctionLookup] ([ID], [Text]) VALUES (7, N'Reinstate')
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

INSERT [dbo].[SourceLookup] ([ID], [Text]) VALUES (0, N'Request')
GO
INSERT [dbo].[SourceLookup] ([ID], [Text]) VALUES (1, N'JobOrder')
GO
INSERT [dbo].[SourceLookup] ([ID], [Text]) VALUES (2, N'Social Media')
GO
INSERT [dbo].[SourceLookup] ([ID], [Text]) VALUES (3, N'iFAQ')
GO
INSERT [dbo].[SourceLookup] ([ID], [Text]) VALUES (4, N'Internal')
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

INSERT [dbo].[EmailTemplateTypeLookup] ([ID], [Text]) VALUES (400, N'Show Cause')
GO
INSERT [dbo].[EmailTemplateTypeLookup] ([ID], [Text]) VALUES (401, N'Warning')
GO
INSERT [dbo].[EmailTemplateTypeLookup] ([ID], [Text]) VALUES (402, N'Compound')
GO
INSERT [dbo].[EmailTemplateTypeLookup] ([ID], [Text]) VALUES (403, N'Suspension')
GO
INSERT [dbo].[EmailTemplateTypeLookup] ([ID], [Text]) VALUES (404, N'Immediate Suspension')
GO
INSERT [dbo].[EmailTemplateTypeLookup] ([ID], [Text]) VALUES (405, N'Revocation')
GO

INSERT [dbo].[LetterStatusLookup] ([ID], [Text]) VALUES (100, N'Draft')
GO
INSERT [dbo].[LetterStatusLookup] ([ID], [Text]) VALUES (200, N'Final')
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
INSERT [dbo].[CertificateStatusLookup] ([ID], [Text]) VALUES (600, N'Revoked')
GO