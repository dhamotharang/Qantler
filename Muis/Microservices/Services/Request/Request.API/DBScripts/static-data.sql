INSERT [dbo].[Locale] ([ID], [Text], [Code]) VALUES (0, N'English', N'en')
GO

SET IDENTITY_INSERT [dbo].[Translations] ON 
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (1, 0, N'EscalateRequest', N'escalated the request.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (2, 0, N'RequestRaisedRFA', N'raised a new RFA.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (3, 0, N'BeginReview', N'begin the review.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (4, 0, N'BeginApproval', N'begin the approval.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (5, 0, N'RequestReview', N'recommended')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (7, 0, N'ExtendRFADueDate', N'extended RFA to')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (8, 0, N'NewRequestNotification', N'A new request has been submitted that is pending for your review')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (9, 0, N'ForInspection', N'scheduled an inspection on')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (10, 0, N'NewRFAOpenExists', N'Unable to submit a new RFA. An existing RFA is still open.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (11, 0, N'RFACreated', N'raised the RFA.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (12, 0, N'RFAUpdated', N'updated the RFA.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (13, 0, N'RFAClosed', N'closed the RFA.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (14, 0, N'RFAExtendDueDate', N'extended the due date to {0}.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (15, 0, N'RequestKIV', N'set into KIV.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (16, 0, N'RevertKIV', N'reverted status from KIV.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (17, 0, N'RequestScheduleInspection', N'scheduled an inspection on {0}.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (18, 0, N'EscalateNotes', N'added escalation notes.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (19, 0, N'EscalateClosed', N'closed the escalation.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (20, 0, N'ReEscalateRequest', N're-escalated the request.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (21, 0, N'RecommendRequest', N'submitted a recommendation to AO.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (22, 0, N'UpdatedRequestSettings', N'updated {0} from {1} to {2}')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (23, 0, N'RequestReaudit', N'requested to re-audit.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (24, 0, N'InvalidAction', N'Invalid action')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (25, 0, N'RequestReauditNotifTitle', N'Re-audit Request')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (26, 0, N'RequestReauditNotifMsg', N'Request has been re-assigned back to you for re-audit.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (29, 0, N'RequestApproved', N'approved the request.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (30, 0, N'RequestRejected', N'rejected the request.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (32, 0, N'RequestForApprovalNotifTitle', N'For Approval')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (33, 0, N'RequestForApprovalNotifBody', N'{0} provided its recommendation for {1} and its pending for approval.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (34, 0, N'RequestApproveBlockRecommend', N'You are not allowed to approve request that you have recommended.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (35, 0, N'RequestApproveBlockReassign', N'You are not allowed to approve request that you have re-assigned.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (36, 0, N'RequestReassign', N'reassigned to {0}.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (37, 0, N'RequestReassignNotifTitle', N'Re-assign')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (38, 0, N'RequestReassignNotifBody', N'A request has been re-assigned to you.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (39, 0, N'ValidateCustomer', N'Custormer already exists.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (40, 0, N'ValidatePremise', N'Premise already exists.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (10032, 0, N'MuftiCommentTitle', N'Mufti')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (10033, 0, N'MuftiAcknowledge', N'Certificate %s has been acknowledged by Mufti.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (10034, 0, N'MuftiAcknowledgeNotifTitle', N'Mufti')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (10035, 0, N'MuftiAcknowledgeNotifBody', N'Certificate Batch {0} has been acknowledged by Mufti and it''s now ready for printing.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (10036, 0, N'BatchCertificateSentToCourier', N'Certificate %s has been sent to courier.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (10037, 0, N'BatchCertificateDelivered', N'Certificate %s has been delivered.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (10038, 0, N'RequestBillReadyTitle', N'Request Invoice')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (10039, 0, N'RequestBillReadyBody', N'Draft invoice has been created. And it''s ready for your review.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (10040, 0, N'RequestSubmitForPayment', N'submitted invoice for payment.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (10041, 0, N'RequestBillPaidTitle', N'Request Invoice Processed')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (10042, 0, N'RequestBillPaidBody', N'Request invoice payment has been processed.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (10043, 0, N'RequestApproveNoCode', N'Unable to approve. Please set customer code.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (10044, 0, N'RequestRecommendRFABlock', N'Cannot recommend request with pending RFA.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (10046, 0, N'AssignCANotFoundNotifTitle', N'Assign CA')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (10047, 0, N'AssignCANotFoundNotifBody', N'Certificate Auditor not assigned to postal code {0}.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (10048, 0, N'Outdated', N'Invalid action. Your data is out dated. Please refresh.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (10049, 0, N'RFAProcessedByBody', N'RFA has been closed by {0}.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (10050, 0, N'RFAProcessedByTitle', N'RFA')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (10051, 0, N'RFANotAllowed', N'Can only submit RFA when status is Open or site inspection has been done.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (20058, 0, N'CustomerCodeAssignTitle', N'Customer Code Assignement')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (20059, 0, N'CustomerCodeAssignBody', N'New application has been received that requires to assign')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (20060, 0, N'RequestReviewNoCode', N'Unable to review. Please set customer code.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (20061, 0, N'RequestNotFound', N'Request not found')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (20062, 0, N'ReinstateRequest', N'Re-instated the request.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (20063, 0, N'AcceptOnlyFoodManufactoringScheme', N'Unable to approve. HC03 approval process only accept food manufactoring schemes.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (20064, 0, N'RequestApproveNoCertificate', N'Unable to approve. Please set certificate.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (20065, 0, N'AcceptOnlyEatingEstablishmentScheme', N'Unable to approve. HC04 approval process only accept eating establishment schemes.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (20066, 0, N'RequestForReviewApprovalNotifTitle', N'Review recommendation approval.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (20067, 0, N'RequestForReviewApprovalNotifBody', N'A request recommended for rejection is pending for your approval.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (20069, 0, N'RecommendReviewApproval', N'Submitted recommendation for approval.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (20070, 0, N'ReviewedRecommendation', N'Reviewed the recommendation.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (20071, 0, N'RequestApproveBlockPendingRequest', N'You are not allowed to approve request since you have pending ammend (HC03/HC04) application on same premise.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (20072, 0, N'RequestApproveBlockCertiSuspended', N'Unable to recommend/approved application. Certificate has been suspended until {0}.')
GO
INSERT [dbo].[Translations] ([ID], [Locale], [Key], [Text]) VALUES (20073, 0, N'RequestApproveBlockCertiRevoked', N'Unable to recommend/approved application. Certificate has been revoked.')
GO
SET IDENTITY_INSERT [dbo].[Translations] OFF
GO

INSERT [dbo].[EmailTemplateTypeLookup] ([ID], [Text]) VALUES (100, N'Rejection Email Template')
GO

SET IDENTITY_INSERT [dbo].[EmailTemplate] ON 
GO
INSERT [dbo].[EmailTemplate] ([ID], [Type], [From], [Cc], [Bcc], [Title], [Body], [IsBodyHtml], [CreatedOn], [ModifiedOn], [IsDeleted],[Keyword]) VALUES (1, 100, N'keyfe.a@totalebizsolutions.com', N'', N'{officerEmail}', N'Rejection for {customer}', N'<div>Salaam All,<br /><br />{draft}<br />I refer to the {type} application (#{refID}) for {customer} @ {premise}.<br /><br />Audit date: {inspectedOn}, {auditorName}<br /><br />{findings}<br /><br />{enddraft}<br /><br />Regards,<br /><strong>Islamic Religious Council of Singapore (Muis)</strong>&nbsp; <br />Tel: 6812 6060 <strong>&bull; </strong>Fax: 6812 6061<br />Visit us at: <a href="http://www.halal.sg/">www.halal.sg</a> <strong>&bull;</strong> Connect with us on:&nbsp;<img src="../../api/file/919b6f8f-1401-49ca-b4b5-5a4f1857b25a?fileName=image001.gif" alt="" width="12" height="12" /></div>
<div><hr /></div>
<div><span style="color: #27ae60; font-size: 8pt; font-family: tahoma, arial, helvetica, sans-serif;"><em>&ldquo;Building a gracious Muslim community of excellence that inspires and radiates blessings to all.&rdquo;</em></span><br /><br /><span style="font-size: 8pt; font-family: tahoma, arial, helvetica, sans-serif;"><em>This email may contain privileged and confidential information. If you receive this email by mistake, you should immediately notify the sender and delete the email. </em></span><br /><span style="font-size: 8pt; font-family: tahoma, arial, helvetica, sans-serif;"><em>Unauthorised communication and disclosure of any information in the email is an offence under the Official Secrets Act (Cap 213).</em></span></div>', 1, CAST(N'2020-09-23T06:30:49.0000000' AS DateTime2), CAST(N'2020-11-18T14:54:18.0000000' AS DateTime2), 0,'{officerName} {officerEmail} {customer} {refID} {type} {auditorName} {inspectedOn} {premise} {findings} {draft} {enddraft}')
GO
SET IDENTITY_INSERT [dbo].[EmailTemplate] OFF
GO

INSERT [dbo].[CertificateBatchStatusLookup] ([ID], [Text]) VALUES (100, N'Pending')
GO
INSERT [dbo].[CertificateBatchStatusLookup] ([ID], [Text]) VALUES (200, N'Acknowledged')
GO
INSERT [dbo].[CertificateBatchStatusLookup] ([ID], [Text]) VALUES (300, N'Downloaded')
GO
INSERT [dbo].[CertificateBatchStatusLookup] ([ID], [Text]) VALUES (400, N'Sent to Courier')
GO
INSERT [dbo].[CertificateBatchStatusLookup] ([ID], [Text]) VALUES (500, N'Delivered')
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

INSERT [dbo].[CertificateTemplateLookup] ([ID], [Text]) VALUES (0, N'Product')
GO
INSERT [dbo].[CertificateTemplateLookup] ([ID], [Text]) VALUES (1, N'Whole Plant')
GO
INSERT [dbo].[CertificateTemplateLookup] ([ID], [Text]) VALUES (2, N'Catering')
GO
INSERT [dbo].[CertificateTemplateLookup] ([ID], [Text]) VALUES (3, N'Central Kitchen')
GO
INSERT [dbo].[CertificateTemplateLookup] ([ID], [Text]) VALUES (4, N'Poultry')
GO
INSERT [dbo].[CertificateTemplateLookup] ([ID], [Text]) VALUES (5, N'Endorsement')
GO
INSERT [dbo].[CertificateTemplateLookup] ([ID], [Text]) VALUES (6, N'Eating Establishment')
GO
INSERT [dbo].[CertificateTemplateLookup] ([ID], [Text]) VALUES (7, N'Storage')
GO

INSERT [dbo].[ChangeTypeLookup] ([ID], [Text]) VALUES (0, N'Default')
GO
INSERT [dbo].[ChangeTypeLookup] ([ID], [Text]) VALUES (1, N'New')
GO
INSERT [dbo].[ChangeTypeLookup] ([ID], [Text]) VALUES (2, N'Edit')
GO
INSERT [dbo].[ChangeTypeLookup] ([ID], [Text]) VALUES (3, N'Delete')
GO

INSERT [dbo].[CharTypeLookup] ([ID], [Text]) VALUES (0, N'Duration')
GO
INSERT [dbo].[CharTypeLookup] ([ID], [Text]) VALUES (1, N'OtherInfo')
GO
INSERT [dbo].[CharTypeLookup] ([ID], [Text]) VALUES (2, N'Issued Certificate')
GO
INSERT [dbo].[CharTypeLookup] ([ID], [Text]) VALUES (3, N'Certificate Expiry')
GO
INSERT [dbo].[CharTypeLookup] ([ID], [Text]) VALUES (4, N'No. of copies')
GO
INSERT [dbo].[CharTypeLookup] ([ID], [Text]) VALUES (5, N'No. of Products')
GO
INSERT [dbo].[CharTypeLookup] ([ID], [Text]) VALUES (6, N'Product Classifications')
GO
INSERT [dbo].[CharTypeLookup] ([ID], [Text]) VALUES (7, N'Endorsement Label Count')
GO
INSERT [dbo].[CharTypeLookup] ([ID], [Text]) VALUES (8, N'Short Term Start Date')
GO
INSERT [dbo].[CharTypeLookup] ([ID], [Text]) VALUES (9, N'Short Term End Date')
GO
INSERT [dbo].[CharTypeLookup] ([ID], [Text]) VALUES (10, N'Poultry Tags')
GO
INSERT [dbo].[CharTypeLookup] ([ID], [Text]) VALUES (11, N'Certificate Issued Date')
GO
INSERT [dbo].[CharTypeLookup] ([ID], [Text]) VALUES (12, N'Old Certificate Issued Date')
GO
INSERT [dbo].[CharTypeLookup] ([ID], [Text]) VALUES (13, N'Old Certificate Expiry')
GO
INSERT [dbo].[CharTypeLookup] ([ID], [Text]) VALUES (14, N'Slaughter Date')
GO
INSERT [dbo].[CharTypeLookup] ([ID], [Text]) VALUES (15, N'Rejection Email')
GO

INSERT [dbo].[CodeTypeLookup] ([ID], [Text]) VALUES (0, N'Code')
GO
INSERT [dbo].[CodeTypeLookup] ([ID], [Text]) VALUES (1, N'Group')
GO

INSERT [dbo].[EscalateStatusLookup] ([ID], [Text]) VALUES (0, N'Default')
GO
INSERT [dbo].[EscalateStatusLookup] ([ID], [Text]) VALUES (100, N'Open')
GO
INSERT [dbo].[EscalateStatusLookup] ([ID], [Text]) VALUES (200, N'Closed')
GO

INSERT [dbo].[LogTypeLookup] ([ID], [Text]) VALUES (0, N'Default')
GO
INSERT [dbo].[LogTypeLookup] ([ID], [Text]) VALUES (1, N'Request')
GO
INSERT [dbo].[LogTypeLookup] ([ID], [Text]) VALUES (2, N'RFA')
GO
INSERT [dbo].[LogTypeLookup] ([ID], [Text]) VALUES (3, N'JobOrder')
GO

INSERT [dbo].[MasterTypeLookup] ([ID], [Text]) VALUES (100, N'EscalationCategory')
GO
INSERT [dbo].[MasterTypeLookup] ([ID], [Text]) VALUES (101, N'ReinstateReason')
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
INSERT [dbo].[PremiseTypeLookup] ([ID], [Text]) VALUES (6, N'Office')
GO
INSERT [dbo].[PremiseTypeLookup] ([ID], [Text]) VALUES (7, N'Show Room')
GO
INSERT [dbo].[PremiseTypeLookup] ([ID], [Text]) VALUES (8, N'Warehouse')
GO
INSERT [dbo].[PremiseTypeLookup] ([ID], [Text]) VALUES (99, N'Others')
GO

INSERT [dbo].[RequestStatusLookup] ([ID], [Text]) VALUES (100, N'Draft')
GO
INSERT [dbo].[RequestStatusLookup] ([ID], [Text]) VALUES (200, N'Open')
GO
INSERT [dbo].[RequestStatusLookup] ([ID], [Text]) VALUES (250, N'Pending Review Approval')
GO
INSERT [dbo].[RequestStatusLookup] ([ID], [Text]) VALUES (300, N'Scheduled for Inspection')
GO
INSERT [dbo].[RequestStatusLookup] ([ID], [Text]) VALUES (400, N'Pending Approval')
GO
INSERT [dbo].[RequestStatusLookup] ([ID], [Text]) VALUES (500, N'Approved')
GO
INSERT [dbo].[RequestStatusLookup] ([ID], [Text]) VALUES (550, N'For Mufti Acknowledgement')
GO
INSERT [dbo].[RequestStatusLookup] ([ID], [Text]) VALUES (600, N'Billing in Progress')
GO
INSERT [dbo].[RequestStatusLookup] ([ID], [Text]) VALUES (700, N'Pending Payment')
GO
INSERT [dbo].[RequestStatusLookup] ([ID], [Text]) VALUES (800, N'Issuance')
GO
INSERT [dbo].[RequestStatusLookup] ([ID], [Text]) VALUES (900, N'Closed')
GO
INSERT [dbo].[RequestStatusLookup] ([ID], [Text]) VALUES (1000, N'Rejected')
GO
INSERT [dbo].[RequestStatusLookup] ([ID], [Text]) VALUES (1100, N'Cancelled')
GO
INSERT [dbo].[RequestStatusLookup] ([ID], [Text]) VALUES (1200, N'Expired')
GO
INSERT [dbo].[RequestStatusLookup] ([ID], [Text]) VALUES (1300, N'KIV')
GO


INSERT [dbo].[RequestStatusMinorLookup] ([ID], [Text]) VALUES (110, N'Pending Bill')
GO
INSERT [dbo].[RequestStatusMinorLookup] ([ID], [Text]) VALUES (120, N'Pending Customer Code')
GO
INSERT [dbo].[RequestStatusMinorLookup] ([ID], [Text]) VALUES (150, N'Pending Offline Payment')
GO
INSERT [dbo].[RequestStatusMinorLookup] ([ID], [Text]) VALUES (210, N'Review in Progress')
GO
INSERT [dbo].[RequestStatusMinorLookup] ([ID], [Text]) VALUES (310, N'Inspection in Progress')
GO
INSERT [dbo].[RequestStatusMinorLookup] ([ID], [Text]) VALUES (320, N'Inspection Done')
GO
INSERT [dbo].[RequestStatusMinorLookup] ([ID], [Text]) VALUES (410, N'Approval in Progress')
GO
INSERT [dbo].[RequestStatusMinorLookup] ([ID], [Text]) VALUES (610, N'Bill Ready')
GO
INSERT [dbo].[RequestStatusMinorLookup] ([ID], [Text]) VALUES (810, N'Delivery in Progress')
GO
INSERT [dbo].[RequestStatusMinorLookup] ([ID], [Text]) VALUES (820, N'Delivered')
GO
INSERT [dbo].[RequestStatusMinorLookup] ([ID], [Text]) VALUES (330, N'Inspection Cancelled')
GO

INSERT [dbo].[RequestTypeLookup] ([ID], [Text]) VALUES (0, N'New')
GO
INSERT [dbo].[RequestTypeLookup] ([ID], [Text]) VALUES (1, N'Renewal')
GO
INSERT [dbo].[RequestTypeLookup] ([ID], [Text]) VALUES (2, N'HC01')
GO
INSERT [dbo].[RequestTypeLookup] ([ID], [Text]) VALUES (3, N'HC02')
GO
INSERT [dbo].[RequestTypeLookup] ([ID], [Text]) VALUES (4, N'HC03')
GO
INSERT [dbo].[RequestTypeLookup] ([ID], [Text]) VALUES (5, N'HC04')
GO
INSERT [dbo].[RequestTypeLookup] ([ID], [Text]) VALUES (6, N'Legacy')
GO

INSERT [dbo].[RFAStatusLookup] ([ID], [Text]) VALUES (1, N'Draft')
GO
INSERT [dbo].[RFAStatusLookup] ([ID], [Text]) VALUES (100, N'Open')
GO
INSERT [dbo].[RFAStatusLookup] ([ID], [Text]) VALUES (200, N'Pending Review')
GO
INSERT [dbo].[RFAStatusLookup] ([ID], [Text]) VALUES (300, N'Closed')
GO
INSERT [dbo].[RFAStatusLookup] ([ID], [Text]) VALUES (400, N'Expired')
GO

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

INSERT [dbo].[SchemeLookup] ([ID], [Text]) VALUES (100, N'Eating Establishment')
GO
INSERT [dbo].[SchemeLookup] ([ID], [Text]) VALUES (200, N'Food Preparation Area')
GO
INSERT [dbo].[SchemeLookup] ([ID], [Text]) VALUES (300, N'Food Manufacturing')
GO
INSERT [dbo].[SchemeLookup] ([ID], [Text]) VALUES (400, N'Poultry Abattoir')
GO
INSERT [dbo].[SchemeLookup] ([ID], [Text]) VALUES (500, N'Endorsement')
GO
INSERT [dbo].[SchemeLookup] ([ID], [Text]) VALUES (600, N'Storage Facility')
GO

INSERT [dbo].[SettingsLookup] ([ID], [Text]) VALUES (100, N'WorkingDaysNormal')
GO
INSERT [dbo].[SettingsLookup] ([ID], [Text]) VALUES (101, N'WorkingDaysExpress')
GO
INSERT [dbo].[SettingsLookup] ([ID], [Text]) VALUES (102, N'RFANormal')
GO
INSERT [dbo].[SettingsLookup] ([ID], [Text]) VALUES (103, N'RFAExpress')
GO
INSERT [dbo].[SettingsLookup] ([ID], [Text]) VALUES (104, N'RenewalHawker')
GO
INSERT [dbo].[SettingsLookup] ([ID], [Text]) VALUES (105, N'RenewalCanteen')
GO
INSERT [dbo].[SettingsLookup] ([ID], [Text]) VALUES (106, N'RenewalRestaurant')
GO
INSERT [dbo].[SettingsLookup] ([ID], [Text]) VALUES (107, N'RenewalHalalSection')
GO
INSERT [dbo].[SettingsLookup] ([ID], [Text]) VALUES (108, N'RenewalFoodKiosk')
GO
INSERT [dbo].[SettingsLookup] ([ID], [Text]) VALUES (109, N'RenewalFoodStation')
GO
INSERT [dbo].[SettingsLookup] ([ID], [Text]) VALUES (110, N'RenewalSnakBarBakery')
GO
INSERT [dbo].[SettingsLookup] ([ID], [Text]) VALUES (111, N'RenewalShotTerm')
GO
INSERT [dbo].[SettingsLookup] ([ID], [Text]) VALUES (112, N'RenewalCentralKitchen')
GO
INSERT [dbo].[SettingsLookup] ([ID], [Text]) VALUES (113, N'RenewalCatering')
GO
INSERT [dbo].[SettingsLookup] ([ID], [Text]) VALUES (114, N'RenewalPreSchoolKitchen')
GO
INSERT [dbo].[SettingsLookup] ([ID], [Text]) VALUES (115, N'RenewalTriggerProduct')
GO
INSERT [dbo].[SettingsLookup] ([ID], [Text]) VALUES (116, N'RenewalTriggerWholePlant')
GO
INSERT [dbo].[SettingsLookup] ([ID], [Text]) VALUES (117, N'RenewalTriggerPolutry')
GO
INSERT [dbo].[SettingsLookup] ([ID], [Text]) VALUES (118, N'RenewalTriggerEndorsement')
GO
INSERT [dbo].[SettingsLookup] ([ID], [Text]) VALUES (119, N'RenewalTriggerStorageFacility')
GO
INSERT [dbo].[SettingsLookup] ([ID], [Text]) VALUES (120, N'CertificateRenewalTriggerGSO')
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

INSERT [dbo].[RequestActionLookup] ([ID], [Text]) VALUES (1, N'Review')
GO
INSERT [dbo].[RequestActionLookup] ([ID], [Text]) VALUES (2, N'Site Inspection')
GO
INSERT [dbo].[RequestActionLookup] ([ID], [Text]) VALUES (3, N'Approved')
GO
INSERT [dbo].[RequestActionLookup] ([ID], [Text]) VALUES (4, N'Invoice Review')
GO
INSERT [dbo].[RequestActionLookup] ([ID], [Text]) VALUES (5, N'Partial Payment')
GO
INSERT [dbo].[RequestActionLookup] ([ID], [Text]) VALUES (6, N'Full Payment')
GO
INSERT [dbo].[RequestActionLookup] ([ID], [Text]) VALUES (7, N'Issuance')
GO
INSERT [dbo].[RequestActionLookup] ([ID], [Text]) VALUES (8, N'Reassign')
GO
INSERT [dbo].[RequestActionLookup] ([ID], [Text]) VALUES (9, N'Reaudit')
GO
INSERT [dbo].[RequestActionLookup] ([ID], [Text]) VALUES (10, N'Reinstate')
GO
INSERT [dbo].[RequestActionLookup] ([ID], [Text]) VALUES (11, N'Review Approval')
GO

INSERT [dbo].[IngredientStatusLookup] ([ID], [Text]) VALUES (0, N'Unverified')
GO
INSERT [dbo].[IngredientStatusLookup] ([ID], [Text]) VALUES (1, N'Verified')
GO

INSERT [dbo].[CertifyingBodyStatusLookup] ([ID], [Text]) VALUES (0, N'Unrecognized')
GO
INSERT [dbo].[CertifyingBodyStatusLookup] ([ID], [Text]) VALUES (1, N'Recognized')
GO

GO
INSERT [dbo].[CertificateDeliveryStatusLookup] ([ID], [Text]) VALUES (100, N'Pending')
GO
INSERT [dbo].[CertificateDeliveryStatusLookup] ([ID], [Text]) VALUES (200, N'Delivered')
GO
INSERT [dbo].[CertificateDeliveryStatusLookup] ([ID], [Text]) VALUES (300, N'Returned')
GO

INSERT [dbo].[IDTypeLookup] ([ID], [Text]) VALUES (0, N'UEN')
GO
INSERT [dbo].[IDTypeLookup] ([ID], [Text]) VALUES (1, N'NRIC')
GO
INSERT [dbo].[IDTypeLookup] ([ID], [Text]) VALUES (2, N'Passport')
GO
INSERT [dbo].[IDTypeLookup] ([ID], [Text]) VALUES (3, N'FIN')
GO