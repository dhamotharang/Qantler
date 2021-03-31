SET IDENTITY_INSERT [dbo].[ChecklistHistory] ON 
GO
INSERT [dbo].[ChecklistHistory] ([ID], [Scheme], [Version], [CreatedBy], [EffectiveFrom], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (1, 100, 1, N'83cab734-5171-4221-a0cc-b882da31b138', CAST(N'2021-12-02T00:00:00.0000000' AS DateTime2), CAST(N'2020-12-01T09:41:48.0000000' AS DateTime2), CAST(N'2020-12-01T11:06:04.0000000' AS DateTime2), 0)
GO
INSERT [dbo].[ChecklistHistory] ([ID], [Scheme], [Version], [CreatedBy], [EffectiveFrom], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (2, 200, 1, N'83cab734-5171-4221-a0cc-b882da31b138', CAST(N'2021-12-02T00:00:00.0000000' AS DateTime2), CAST(N'2020-12-01T11:54:29.0000000' AS DateTime2), CAST(N'2020-12-01T12:18:19.0000000' AS DateTime2), 0)
GO
INSERT [dbo].[ChecklistHistory] ([ID], [Scheme], [Version], [CreatedBy], [EffectiveFrom], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (3, 400, 1, N'83cab734-5171-4221-a0cc-b882da31b138', CAST(N'2021-12-02T00:00:00.0000000' AS DateTime2), CAST(N'2020-12-01T13:42:29.0000000' AS DateTime2), CAST(N'2020-12-01T13:42:29.0000000' AS DateTime2), 0)
GO
INSERT [dbo].[ChecklistHistory] ([ID], [Scheme], [Version], [CreatedBy], [EffectiveFrom], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (4, 600, 1, N'83cab734-5171-4221-a0cc-b882da31b138', CAST(N'2021-12-02T00:00:00.0000000' AS DateTime2), CAST(N'2020-12-02T05:44:40.0000000' AS DateTime2), CAST(N'2020-12-02T05:44:40.0000000' AS DateTime2), 0)
GO
SET IDENTITY_INSERT [dbo].[ChecklistHistory] OFF

GO
SET IDENTITY_INSERT [dbo].[ChecklistCategory] ON 
GO
INSERT [dbo].[ChecklistCategory] ([ID], [Index], [Text], [HistoryID]) VALUES (1, 1, N'APPLICATION', 1)
GO
INSERT [dbo].[ChecklistCategory] ([ID], [Index], [Text], [HistoryID]) VALUES (2, 2, N'HALAL CERTIFICATE', 1)
GO
INSERT [dbo].[ChecklistCategory] ([ID], [Index], [Text], [HistoryID]) VALUES (3, 3, N'OTHERS', 1)
GO
INSERT [dbo].[ChecklistCategory] ([ID], [Index], [Text], [HistoryID]) VALUES (4, 4, N'MENU ITEMS', 1)
GO
INSERT [dbo].[ChecklistCategory] ([ID], [Index], [Text], [HistoryID]) VALUES (5, 5, N'RAW MATERIALS', 1)
GO
INSERT [dbo].[ChecklistCategory] ([ID], [Index], [Text], [HistoryID]) VALUES (6, 6, N'UTENSILS', 1)
GO
INSERT [dbo].[ChecklistCategory] ([ID], [Index], [Text], [HistoryID]) VALUES (7, 7, N'MUSLIM STAFF', 1)
GO
INSERT [dbo].[ChecklistCategory] ([ID], [Index], [Text], [HistoryID]) VALUES (8, 8, N'HALAL TEAM', 1)
GO
INSERT [dbo].[ChecklistCategory] ([ID], [Index], [Text], [HistoryID]) VALUES (9, 9, N'SYSTEM', 1)
GO
INSERT [dbo].[ChecklistCategory] ([ID], [Index], [Text], [HistoryID]) VALUES (10, 1, N'APPLICATION', 2)
GO
INSERT [dbo].[ChecklistCategory] ([ID], [Index], [Text], [HistoryID]) VALUES (11, 2, N'HALAL CERTIFICATE', 2)
GO
INSERT [dbo].[ChecklistCategory] ([ID], [Index], [Text], [HistoryID]) VALUES (12, 3, N'OTHERS', 2)
GO
INSERT [dbo].[ChecklistCategory] ([ID], [Index], [Text], [HistoryID]) VALUES (13, 4, N'MENU ITEMS', 2)
GO
INSERT [dbo].[ChecklistCategory] ([ID], [Index], [Text], [HistoryID]) VALUES (14, 5, N'RAW MATERIALS', 2)
GO
INSERT [dbo].[ChecklistCategory] ([ID], [Index], [Text], [HistoryID]) VALUES (15, 6, N'UTENSILS', 2)
GO
INSERT [dbo].[ChecklistCategory] ([ID], [Index], [Text], [HistoryID]) VALUES (16, 7, N'MUSLIM STAFF', 2)
GO
INSERT [dbo].[ChecklistCategory] ([ID], [Index], [Text], [HistoryID]) VALUES (17, 8, N'HALAL TEAM', 2)
GO
INSERT [dbo].[ChecklistCategory] ([ID], [Index], [Text], [HistoryID]) VALUES (18, 9, N'SYSTEM', 2)
GO
INSERT [dbo].[ChecklistCategory] ([ID], [Index], [Text], [HistoryID]) VALUES (19, 1, N'APPLICATION', 3)
GO
INSERT [dbo].[ChecklistCategory] ([ID], [Index], [Text], [HistoryID]) VALUES (20, 2, N'OTHERS', 3)
GO
INSERT [dbo].[ChecklistCategory] ([ID], [Index], [Text], [HistoryID]) VALUES (21, 3, N'PRE SLAUGHTERING & SLAUGHTERING', 3)
GO
INSERT [dbo].[ChecklistCategory] ([ID], [Index], [Text], [HistoryID]) VALUES (22, 4, N'POST-SLAUGHTERING', 3)
GO
INSERT [dbo].[ChecklistCategory] ([ID], [Index], [Text], [HistoryID]) VALUES (23, 5, N'PREMISES, DOCUMENTS & LABELS', 3)
GO
INSERT [dbo].[ChecklistCategory] ([ID], [Index], [Text], [HistoryID]) VALUES (24, 6, N'MUSLIM STAFF', 3)
GO
INSERT [dbo].[ChecklistCategory] ([ID], [Index], [Text], [HistoryID]) VALUES (25, 7, N'HALAL TEAM', 3)
GO
INSERT [dbo].[ChecklistCategory] ([ID], [Index], [Text], [HistoryID]) VALUES (26, 8, N'SYSTEM', 3)
GO
INSERT [dbo].[ChecklistCategory] ([ID], [Index], [Text], [HistoryID]) VALUES (27, 1, N'APPLICATION', 4)
GO
INSERT [dbo].[ChecklistCategory] ([ID], [Index], [Text], [HistoryID]) VALUES (28, 2, N'APPLICATION - PRODUCTS', 4)
GO
INSERT [dbo].[ChecklistCategory] ([ID], [Index], [Text], [HistoryID]) VALUES (29, 3, N'OTHERS', 4)
GO
INSERT [dbo].[ChecklistCategory] ([ID], [Index], [Text], [HistoryID]) VALUES (30, 4, N'PRODUCTS', 4)
GO
INSERT [dbo].[ChecklistCategory] ([ID], [Index], [Text], [HistoryID]) VALUES (31, 5, N'RAW MATERIALS', 4)
GO
INSERT [dbo].[ChecklistCategory] ([ID], [Index], [Text], [HistoryID]) VALUES (32, 6, N'EQUIPMENT', 4)
GO
INSERT [dbo].[ChecklistCategory] ([ID], [Index], [Text], [HistoryID]) VALUES (33, 7, N'MUSLIM STAFF', 4)
GO
INSERT [dbo].[ChecklistCategory] ([ID], [Index], [Text], [HistoryID]) VALUES (34, 8, N'HALAL TEAM', 4)
GO
INSERT [dbo].[ChecklistCategory] ([ID], [Index], [Text], [HistoryID]) VALUES (35, 9, N'SYSTEM', 4)
GO
SET IDENTITY_INSERT [dbo].[ChecklistCategory] OFF
GO

SET IDENTITY_INSERT [dbo].[ChecklistItem] ON 
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (1, 1, N'Establishment name in MeS corresponds to the description on display board', 1, N'<ul>
<li>Stalls in food-courts to follow this naming convention &lt; Name of stall (Stall No) @ Name of food-court &gt; (e.g. Western stall (S1) @ Banquet)</li>
<li>Food stations in hotel restaurants to follow this naming convention &lt; Name of food station @ Name of hotel restaurant &gt; (e.g. Asian Food Station @ Straits Kitchen)</li>
</ul>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (2, 2, N'Establishment address in MeS corresponds to the exact location address', 1, N'<p>To also check that the building name &amp; unit no.</p>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (3, 3, N'Type of scheme & sub-scheme corresponds to nature of business', 1, N'')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (4, 4, N'Size of establishment corresponds to floor plan', 1, N'')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (5, 5, N'Establishment has applied separately for other outlets, bearing the same name, under EE scheme', 1, N'')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (6, 6, N'Establishment has applied for SF scheme if it stores raw materials offsite and self manages the storage', 1, N'')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (7, 7, N'Establishment has engaged 3PL for storage of raw materials offsite. 3PL has applied for SF scheme and this applicant had appointed a management representative in the Halal team of 3PL', 1, N'')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (8, 8, N'Establishment has applied for FPA scheme if it has an offsite food preparation area or provides catering services from the current location', 1, N'<p>To check the NEA license &amp; website for evidence that the establishment provides catering services</p>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (9, 9, N'Establishment has applied for PRO / WP scheme if it prepares & pack food products for sale at other retail outlets or delivers to food services which are not Halal certified and / or managed by the applicant.', 1, N'')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (10, 10, N'Establishment engages third party for delivery services?
If yes check to ensure if the:
• Delivery service provider is Halal certified.
• Security labels purchased / ordered from Muis.
', 1, N'')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (11, 1, N'Original Halal certificate is prominently displayed', 2, N'<p>Not applicable to new applications</p>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (12, 2, N'Expired Halal certificate is not displayed and returned to Muis', 2, N'')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (13, 1, N'No other operator is to be allowed usage of the Halal certified premises without written prior approval from Muis', 3, N'<p>To look out for an approval letter from Muis if subletting occurs</p>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (14, 2, N'Any printed or published materials, posters, adverts, signage & packaging that may mislead on the Halal status of the food / products sold are not allowed', 3, N'<p>To check website and other marketing materials for misleading claims</p>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (15, 3, N'NEA / AVA license available', 3, N'')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (16, 4, N'For chain outlets, LOU indicating the following is available:
• List of all chain outlets operating in Singapore including their location address and size of premises
• Name and location address of central kitchens and/or storage facilities, if any
• Indicate if any of the respective outlets provide catering services', 3, N'')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (17, 1, N'All menu items are declared in the MeS', 4, N'')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (18, 2, N'Supplier details of menu items, which are not prepared internally, are updated in the MeS', 4, N'')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (19, 3, N'Menu items are not misleading', 4, N'<p>Words such as &lsquo;bacon&rsquo;, &lsquo;ham&rsquo; &amp; sausages should be prefixed with the meat type e.g. &lsquo;chicken ham&rsquo;</p>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (20, 1, N'All raw materials are declared in the MeS', 5, N'<ul>
<li>To randomly check with chef / relevant employees the ingredients used to prepare selected menu item</li>
<li>To check the raw materials inventory list</li>
<li>Check and verify with items in the store, chiller, freezer and kitchen premises</li>
</ul>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (21, 2, N'Supplier details of all raw materials are updated in the MeS', 5, N'')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (22, 3, N'Doubtful and non-Halal raw materials are not stored, used, sold or brought into the premises applied for certification', 5, N'<p>To check documentations and ensure that the raw materials are not supplied by Muis-suspended companies</p>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (23, 4, N'All raw materials stored, used, sold or brought into the premises applied for certification must be properly packed & labelled with the product description, manufacturers’ name & plant address', 5, N'')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (24, 5, N'Purchase invoices / delivery orders for all raw materials are endorsed by Muslim staff within the Halal team', 5, N'<ul>
<li>Minimum past 3 months record (only for renewal appln)</li>
<li>Look out for irregularities where non-declared items were purchased / delivered (only for renewal appln)</li>
<li>Endorsement by Muslim staff must entail the staff name, signature &amp; date of check</li>
</ul>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (25, 6, N'All raw materials must be Halal & document-substantiated with the following:
i) Certificates/Marks from Muis or Muis-recognised Halal certifying bodies
ii) Product specifications
iii) Detailed Halal questionnaire
iv) Laboratory analysis report', 5, N'<ul>
<li>The product information (i.e. product description, manufacturer&rsquo;s name / add &amp; Halal mark) tally with the supporting documents</li>
<li><strong>High risk items</strong> &ndash; Must be substantiated with valid Halal certificates from Muis-recognised bodies</li>
<li><strong>Med-High risk items</strong> &ndash; Good to be substantiated with Halal certificate/mark. Otherwise, Halal questionnaire required (Check that the document is completed &amp; endorsed with company stamp, representative name &amp; signature, &amp; date. Document is not valid if after &gt;1 year)</li>
<li><strong>Med-Low risk items</strong> &ndash; Good to be substantiated with Halal certificate/mark. Otherwise, specifications required (If doubtful ingredients found, Halal questionnaire required)</li>
<li><strong>Low risk items</strong> &ndash; Must be substantiated with product label</li>
<li>If in doubt, take picture / sample of item for clarification with HCSU or laboratory testing</li>
<li>For all Halal questionnaires ensure there is traceability to supplier &amp; manufacturer as indicated in the questionnaire</li>
</ul>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (26, 1, N'All production lines, crockery, kitchen utensils and equipment’s, cooking place, chillers, freezers, cold rooms, etc. must be ritually cleansed by Muis appointed personnel if they had been previously used for the preparation of pork and pork-related items
', 6, N'')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (27, 2, N'All Halal food / raw materials must be physically segregated from non-Halal items during transportation.', 6, N'<p>Look out for evidence where raw materials are transported with pork items</p>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (28, 3, N'Cross contamination between the equipment’s / utensils used for Halal & non-Halal food should be avoided.
', 6, N'<ul>
<li>For stalls in food-court / food centres, ensure that the plates &amp; utensils are demarcated, dedicated &amp; segregated during storage, collection &amp; washing.</li>
<li>If brushes are used, ensure that they are not made of hog bristles</li>
<li>Separate washing area for Halal certified stalls</li>
<li>If third-party dish washing facility is used, ensure the dish washing facility has a valid Halal certificate.</li>
</ul>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (29, 1, N'Roving Muslim supervisor if any has been approved by Muis', 7, N'')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (30, 2, N'Is the Roving Muslim supervisor a staff of a Halal consultant? If yes:
• Check to ensure the Roving Muslim supervisor is not employed by any other outlet and / or establishment
• Check and verify the duty roster and time sheet of the Roving Muslim supervisor for the last 6 months
• Check and verify the employment contract of the Roving Muslim supervisor', 7, N'')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (31, 3, N'Roving Muslim supervisor conducts weekly checks at chain outlets', 7, N'<ul>
<li>Only applicable to chain outlets for which Muis has granted approval for (1+1) Muslim staff arrangement<br />(To check for Muis approval letter)</li>
<li>To check internal audit records conducted by supervisor</li>
</ul>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (32, 4, N'Employ at least 2 competent permanent Muslim staff', 7, N'<ul>
<li>There must be at least 1 Muslim staff during audit</li>
<li>Muslim staff may be permanent full-time or permanent part-time</li>
<li>Adequacy test &ndash; Check evidence for employment of &gt;2 Muslim staff through employment letter, punch cards, letter from Muis on waiver of Muslim staff &amp; interview of other staff</li>
<li>Competency test &ndash; Interview Muslim staff on general Halal certification guidelines &amp; his/her Halal-related job scope</li>
<li>Permanency test &ndash; Check terms in employment letter</li>
</ul>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (33, 1, N'Establish a Halal Team, comprising a managementappointed leader, at least one Muslim staff and relevant personnel from multi-disciplinary background, to ensure that the HCTC are adhered to at all times [HALMQ1]', 8, N'<ul>
<li>Check for either one of the following:<br />i) List of Halal team members comprising the name, designation &amp; Halal-related job scope of all members of the Halal team. The list must be dated with the signatures of the Management &amp; appointees<br />ii) Appointment letters of all members in the Halal team. Each letter must indicate the member&rsquo;s Halal-related job scope as well as duly dated and signed by the Management &amp; appointee</li>
<li>Interview Halal team members to verify the above documents &amp; educate them on their roles &amp; responsibilities</li>
<li>For chain outlets, suffices for one company to appoint one Halal team</li>
</ul>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (34, 2, N'Halal team, particularly the Muslim staff & at least one other member, undergo Muis Halal training programme', 8, N'<ul>
<li>Check for one of the following documents as evidence that at least 2 Halal team members have gone for the MA Halal Foundation Prog:<br />i) Training certificate<br />ii) Receipt of training<br />iii) Letter of confirmation</li>
<li>Ensure that those 2 Halal team members, who have been trained, are still working</li>
</ul>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (35, 3, N'Primary contact persons (2) / all Halal Team members are correctly updated in the MeS', 8, N'')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (36, 1, N'Product / nature of business defined [HalMQ2]', 9, N'<p>This refers to the MeS Master Print</p>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (37, 2, N'Flow chart constructed & verified [HalMQ3]', 9, N'<ul>
<li>Steps in the flow chart include RM procurement, RM receiving, RM storage, food preparation, cooking, serving, collection of utensils and washing of utensils</li>
<li>To interview Halal team</li>
</ul>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (38, 3, N'Halal threats & their control measures identified [HalMQ4]', 9, N'<ul>
<li>Halal threats include purchase of non-Halal RM,<br />mixing of utensils involved in non-Halal food<br />handling, etc.</li>
<li>Control measures include Muis&rsquo; prior approval for<br />RM, colour code utensils differently from other stalls,<br />etc.</li>
<li>To interview Halal team</li>
</ul>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (39, 4, N'Halal Assurance Points (HAP), their allowable limits & prescribed practices determined [HalMQ5]', 9, N'<ul>
<li>HAPs include point of RM purchase, point of receiving of RM, point of utensil collection, etc.</li>
<li>Allowable limits / prescribed practices include zero tolerance, 0.5% ethanol content, etc.</li>
<li>Advisable for HAPs to be indicated in flowchart</li>
<li>To interview Halal team</li>
</ul>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (40, 5, N'Monitoring system for each HAP established [HalMQ6]', 9, N'<ul>
<li>How, when, who &amp; what should be done to eliminate the Halal threats</li>
<li>To interview key personnel at HAP</li>
</ul>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (41, 6, N'Corrective action for each HAP established [HalMQ7]', 9, N'<ul>
<li>How, when, who &amp; what should be done to rectify any occurrence of a Halal threat</li>
<li>To interview key personnel at HAP</li>
</ul>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (42, 7, N'Documentation & record keeping system established [HalMQ8]', 9, N'<ul>
<li>Halal file for site audit shall comprise all docs required for Halal certification as indicated in HCC, HalMQ8</li>
<li>Halal file for desktop audit @ Muis shall comprise MeS master print, HalMQ plan, staffing docs (Muslim<br />staff &amp; Halal team), menu card &amp; supporting docs for all raw materials [Applicable only to new applns &amp; chain outlets]</li>
<li>Halal file for chain outlets shall comprise of:<br />
<ul>
<li>MeS master print, staffing docs (Muslim staff &amp;Halal team) &amp; RM delivery order</li>
<li>List of all outlets maintaining complete Halal file</li>
</ul>
</li>
</ul>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (43, 1, N'Company name in MeS corresponds to the ACRA business profile / AVA license / NEA license / Marketing name', 10, N'<ul>
<li>Central kitchens in hospitals / airport / hotels to follow this naming convention &lt; Name of kitchen @ Name of company &gt; (e.g. Muslim kitchen @ Alexandra hospital, Western kitchen @ SATS, Muslim kitchen @ Marina Bay Sands)</li>
</ul>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (44, 2, N'Premises address in MeS corresponds to the exact location address', 10, N'<p>To also check that the building name &amp; unit no.</p>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (45, 3, N'Type of scheme & sub-scheme corresponds to nature of business', 10, N'')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (46, 4, N'Size of premises corresponds to floor plan', 10, N'')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (47, 5, N'Company submits multiple applications for two or more kitchens that are intended for Halal certification', 10, N'<ul>
<li>Applies to &gt;2 kitchens which are located on different levels or physically separated by a wall (e.g. Muslim kitchen at level 1 and level 3 are considered 2 separate applications)</li>
<li>Applies to &gt;2 kitchens which are separated by another section(s) that will not be Halal-certified (e.g. Muslim section and Western section, within the same kitchen, are considered 2 separate applications if they are separated by a non-certified Pastry section)</li>
</ul>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (48, 6, N'Establishment has applied for SF scheme if it stores raw materials offsite and self manages the storage', 10, N'')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (49, 7, N'Establishment has engaged 3PL for storage of raw materials offsite. 3PL has applied for SF scheme and this applicant had appointed a management representative in the Halal team of 3PL', 10, N'')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (50, 8, N'Company has applied for EE scheme if it bears the same name as its retail outlets', 10, N'<p>To check the NEA license &amp; website for evidence that the establishment provides catering services</p>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (51, 9, N'Establishment has applied for PRO / WP scheme if it prepares & pack food products for sale at other retail outlets or delivers to food services which are not Halal certified and / or managed by the applicant.', 10, N'')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (52, 10, N'Establishment engages third party for delivery services?
If yes check to ensure if the:
• Delivery service provider is Halal certified.
• Security labels purchased / ordered from Muis.', 10, N'')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (53, 1, N'Ensure the organisation has in stock or has requested for multiple certified true copies of Original Halal certificate to be displayed prominently at all events.', 11, N'<ul>
<li>Ensure the organisation does not have in store photocopies of Halal certificate.</li>
<li>Not applicable to Central Kitchen &amp; Pre-School Kitchen</li>
</ul>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (54, 2, N'Expired Halal certificate(s) returned to Muis', 11, N'')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (55, 1, N'No other operator is to be allowed usage of the Halal certified premises without written prior approval from Muis.', 12, N'<p>To look out for an approval letter from Muis if subletting occurs</p>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (56, 2, N'Any printed or published materials, posters, adverts, signage & packaging that may mislead on the Halal status of the food / products sold are not allowed', 12, N'<p>To check website and other marketing materials for misleading claims</p>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (57, 3, N'NEA / AVA license available', 12, N'')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (58, 1, N'All menu items are declared in the MeS', 13, N'')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (59, 2, N'Supplier details of menu items, which are not prepared internally, are updated in the MeS', 13, N'')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (60, 3, N'Menu items are not misleading', 13, N'<p>Words such as &lsquo;bacon&rsquo;, &lsquo;ham&rsquo; &amp; sausages should be prefixed with the meat type e.g. &lsquo;chicken ham&rsquo;</p>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (61, 1, N'All raw materials are declared in the MeS', 14, N'<ul>
<li>To randomly check with chef / relevant employees the ingredients used to prepare selected menu item</li>
<li>To check the raw materials inventory list</li>
<li>Check and verify with items in the store, chiller, freezer and kitchen premises</li>
</ul>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (62, 2, N'Supplier details of all raw materials are updated in the MeS', 14, N'')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (63, 3, N'Doubtful and non-Halal raw materials are not stored, used, sold or brought into the premises applied for certification', 14, N'<p>To check documentations and ensure that the raw materials are not supplied by Muis-suspended companies</p>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (64, 4, N'All raw materials stored, used, sold or brought into the premises applied for certification must be properly packed & labelled with the product description, manufacturers’ name & plant address', 14, N'')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (65, 5, N'Purchase invoices / delivery orders for all raw materials are endorsed by Muslim staff within the Halal team ', 14, N'<ul>
<li>Minimum past 3 months record (only for renewal appln)</li>
<li>Look out for irregularities where non-declared items were purchased / delivered (only for renewal appln)</li>
<li>Endorsement by Muslim staff must entail the staff name, signature &amp; date of check</li>
</ul>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (66, 6, N'All raw materials must be Halal & documentsubstantiated with the following:
i) Certificates/Marks from Muis or Muis-recognised
Halal certifying bodies
ii) Product specifications
iii) Detailed Halal questionnaire
iv) Laboratory analysis report', 14, N'<ul>
<li>The product information (i.e. product description, manufacturer&rsquo;s name / add &amp; Halal mark) tally with the supporting documents</li>
<li><strong>High risk items</strong> &ndash; Must be substantiated with valid Halal certificates from Muis-recognised bodies</li>
<li><strong>Med-High risk items</strong> &ndash; Good to be substantiated with Halal certificate/mark. Otherwise, Halal questionnaire required (Check that the document is completed &amp; endorsed with company stamp, representative name &amp; signature, &amp; date. Document is not valid if after &gt;1 year)</li>
<li><strong>Med-Low risk items</strong> &ndash; Good to be substantiated with Halal certificate/mark. Otherwise, specifications required (If doubtful ingredients found, Halal questionnaire required)</li>
<li><strong>Low risk items</strong> &ndash; Must be substantiated with product label</li>
<li>If in doubt, take picture / sample of item for clarification with HCSU or laboratory testing</li>
<li>For all Halal questionnaires ensure there is traceability to supplier &amp; manufacturer as indicated in the questionnaire</li>
</ul>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (67, 1, N'All production lines, crockery, kitchen utensils and equipment’s, cooking place, chillers, freezers, cold rooms, etc. must be ritually cleansed by Muis appointed personnel if they had been previously used for the preparation of pork and pork-related items', 15, N'')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (68, 2, N'All Halal food / raw materials must be physically segregated from non-Halal items during transportation to the intended certified location', 15, N'<p>Look out for evidence where raw materials are transported with pork items</p>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (69, 3, N'All cooked food for catering events must be transported in dedicated vehicles declared to Muis. All vehicles must be self-managed and not outsourced to a third party logistics provider.', 15, N'<p>To check past order forms &amp; delivery orders issued to clients to ensure that non-Halal / doubtful items are not delivered (only for renewal appl). If found, report to Muis.</p>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (70, 4, N'Cross contamination between the equipment’s and utensils used for Halal & non-Halal food should be avoided.', 15, N'<ul>
<li>Applicable for central kitchen(s) and pre-school kitchen(s) only.</li>
<li>Ensure that the plates &amp; utensils are demarcated, dedicated &amp; segregated during storage, collection &amp; washing.</li>
<li>If brushes are used, ensure that they are not made of hog bristles</li>
<li>Separate washing area for Halal certified kitchen(s)</li>
<li>If third-party dish washing facility is used, ensure the dish washing facility has a valid Halal certificate.</li>
</ul>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (71, 1, N'Employ at least 3 competent permanent Muslim staff for Catering & Central Kitchen and 2 for Pre-School Kitchen (including a Muslim staff of supervisory level). ', 16, N'<ul>
<li>There must be at least 1 Muslim staff during audit</li>
<li>Muslim staff may be permanent full-time or permanent part-time</li>
<li>Adequacy test &ndash; Check evidence for employment of &gt;3 Muslim staff through employment letter, punch cards, letter from Muis on waiver of Muslim staff &amp; interview of other staff</li>
<li>Competency test &ndash; Interview Muslim staff on general Halal certification guidelines &amp; his/her Halal-related job scope</li>
<li>Permanency test &ndash; Check terms in employment letter</li>
</ul>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (72, 1, N'Establish a Halal Team, comprising a managementappointed leader, at least one Muslim staff and relevant personnel from multi-disciplinary background, to ensure that the HCTC are adhered to at all times [HALMQ1]', 17, N'<ul>
<li>Check for either one of the following:<br />i) List of Halal team members comprising the name, designation &amp; Halal-related job scope of all members of the Halal team. The list must be dated with the signatures of the Management &amp; appointees<br />ii) Appointment letters of all members in the Halal team. Each letter must indicate the member&rsquo;s Halal-related job scope as well as duly dated and signed by the Management &amp; appointee</li>
<li>Interview Halal team members to verify the above documents &amp; educate them on their roles &amp; responsibilities</li>
<li>For chain outlets, suffices for one company to appoint one Halal team</li>
</ul>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (73, 2, N'Halal team, particularly the Muslim staff & at least one other member, undergo Muis Halal training programme', 17, N'<ul>
<li>Check for one of the following documents as evidence that at least 2 Halal team members have gone for the MA Halal Foundation Prog:<br />i) Training certificate<br />ii) Receipt of training<br />iii) Letter of confirmation</li>
<li>Ensure that those 2 Halal team members, who have been trained, are still working</li>
</ul>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (74, 3, N'Primary contact persons (2) / all Halal Team members are correctly updated in the MeS', 17, N'')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (75, 1, N'Product / nature of business defined [HalMQ2]', 18, N'<p>This refers to the MeS Master Print</p>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (76, 2, N'Flow chart constructed & verified [HalMQ3]', 18, N'<ul>
<li>Steps in the flow chart include RM procurement, RM receiving, RM storage, food preparation, cooking, serving, collection of utensils and washing of utensils</li>
<li>To interview Halal team</li>
</ul>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (77, 3, N'Halal threats & their control measures identified [HalMQ4]', 18, N'<ul>
<li>Halal threats include purchase of non-Halal RM, mixing of utensils involved in non-Halal food handling, etc.</li>
<li>Control measures include Muis&rsquo; prior approval for RM, colour code utensils differently from other stalls, etc.</li>
<li>To interview Halal team</li>
</ul>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (78, 4, N'Halal Assurance Points (HAP), their allowable limits & prescribed practices determined [HalMQ5]', 18, N'<ul>
<li>HAPs include point of RM purchase, point of receiving of RM, point of utensil collection, etc.</li>
<li>Allowable limits / prescribed practices include zero tolerance, 0.5% ethanol content, etc.</li>
<li>Advisable for HAPs to be indicated in flowchart</li>
<li>To interview Halal team</li>
</ul>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (79, 5, N'Monitoring system for each HAP established [HalMQ6]', 18, N'<ul>
<li>How, when, who &amp; what should be done to eliminate the Halal threats</li>
<li>To interview key personnel at HAP</li>
</ul>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (80, 6, N'Corrective action for each HAP established [HalMQ7]', 18, N'<ul>
<li>How, when, who &amp; what should be done to rectify any occurrence of a Halal threat</li>
<li>To interview key personnel at HAP</li>
</ul>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (81, 7, N'Documentation & record keeping system established [HalMQ8]', 18, N'<ul>
<li>Halal file for site audit shall comprise all docs required for Halal certification as indicated in HCC, HalMQ8</li>
<li>Halal file for desktop audit @ Muis shall comprise MeS master print, HalMQ plan, staffing docs (Muslim staff &amp; Halal team), menu card &amp; supporting docs for all raw materials [Applicable only to new applns &amp; chain outlets]</li>
<li>Halal file for chain outlets shall comprise of:
<ul>
<li>MeS master print, staffing docs (Muslim staff &amp;Halal team) &amp; RM delivery order</li>
<li>List of all outlets maintaining complete Halal file</li>
</ul>
</li>
</ul>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (82, 1, N'Company name in MeS corresponds to the ACRA business profile / AVA license / Name printed on packaging labels', 19, N'')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (83, 2, N'Premises address in MeS corresponds to the exact location address', 19, N'<p>Check and verify the building name &amp; unit no.</p>')
GO
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (84, 3, N'Type of scheme corresponds to nature of business', 19, N'')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (85, 1, N'No other operator is to be allowed usage of the Halal certified premises without written prior approval from Muis.', 20, N'<p>To look out for an approval letter from Muis if subletting occurs</p>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (86, 2, N'Any printed or published materials, posters, adverts, signage & packaging that may mislead on the Halal status of the food / products sold are not allowed', 20, N'<p>To check website and other marketing materials for misleading claims</p>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (87, 3, N'NEA / AVA / HSA license available', 20, N'')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (88, 1, N'If non-Halal slaughtering has been done prior to Halal slaughtering, ensure that all non-Halal poultry has been cleared and no offals are remaining in the area when Halal slaughtering is being done. ', 21, N'<p>If the slaughtered poultry are not tagged with Halal tags, they are considered non-Halal regardless of who conducts the slaughtering.</p>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (89, 2, N'Poultry are still alive and healthy during the sorting stage', 21, N'<p>Dead poultry must be properly segregated and disposed off</p>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (90, 3, N'Check and ensure the voltage for stunning is within allowable levels to ensure the poultry is not dead due to stunning', 21, N'<p>Only electrical stunning is allowed</p>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (91, 4, N'Poultry are still alive and healthy after being stunned', 21, N'<p>Dead poultry must be properly segregated and disposed off</p>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (92, 5, N'Poultry are slaughtered by qualified Muslim slaughterers', 21, N'')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (93, 6, N'Slaughtering is done in a single cut by cutting the windpipe, the gullet, the jugular vein and the carotid artery', 21, N'')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (94, 7, N'The knife used for slaughtering is constantly sharp', 21, N'')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (95, 1, N'Poultry are dead before the de-feathering stage', 22, N'<ul>
<li>Muslim staff is placed at this Halal Assurance Point to check each poultry before it enters the de-feathering machine</li>
<li>If the poultry is found alive, they should be removed from the conveyor</li>
</ul>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (96, 2, N'Only properly Halal-slaughtered poultry are tagged by Muis approved Halal labels', 22, N'')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (97, 3, N'Halal and non-Halal poultry are segregated and clearly demarcated', 22, N'')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (98, 1, N'Doubtful and non-Halal raw materials are not stored, used, sold or brought into the premises applied for certification', 23, N'<p>To check that the raw materials are not derived from Muis-suspended companies</p>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (99, 2, N'Only Muis’ approved Halal labels are used', 23, N'<ul>
<li>To check the delivery orders for printed Halal labels to ensure that they have been endorsed by Muis</li>
<li>To randomly check the serial number of Halal labels present at the premises to ensure that they tally with the description on the delivery order for printed Halal labels that have been endorsed by Muis</li>
</ul>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (100, 3, N'No duplicate serial numbers for Halal labels', 23, N'')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (101, 1, N'Employ at least 2 competent permanent Muslim staff (including a Muslim staff of supervisory level).', 24, N'<ul>
<li>There must be at least 1 Muslim staff during audit</li>
<li>Muslim staff may be permanent full-time or permanent part-time</li>
<li>Adequacy test &ndash; Check evidence for employment of &gt;3 Muslim staff through employment letter, punch cards, letter from Muis on waiver of Muslim staff &amp; interview of other staff</li>
<li>Competency test &ndash; Interview Muslim staff on general Halal certification guidelines &amp; his/her Halal-related job scope</li>
<li>Permanency test &ndash; Check terms in employment letter</li>
</ul>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (102, 2, N'Muslim staff must be involved in the overseeing of these areas:
1) Sorting of poultry
2) Slaughtering of poultry
3) De-feathering of poultry
4) Labeling of poultry', 24, N'')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (103, 1, N'Establish a Halal Team, comprising a managementappointed leader, at least one Muslim staff and relevant personnel from multi-disciplinary background, to ensure that the HCC are adhered to at all times [HALMQ1]', 25, N'<ul>
<li>Check for either one of the following:<br />i) List of Halal team members comprising the name, designation &amp; Halal-related job scope of all members of the Halal team. The list must be dated with the signatures of the Management &amp; appointees<br />ii) Appointment letters of all members in the Halal team. Each letter must indicate the member&rsquo;s Halal-related job scope as well as duly dated and signed by the Management &amp; appointee</li>
<li>Interview Halal team members to verify the above documents &amp; educate them on their roles &amp; responsibilities</li>
</ul>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (104, 2, N'Halal team, particularly the Muslim staff & at least one other member, undergo Muis Halal training programme', 25, N'<ul>
<li>Check for one of the following documents as evidence that at least 2 Halal team members have gone for the MA Halal Foundation Prog:<br />i) Training certificate<br />ii) Receipt of training<br />iii) Letter of confirmation</li>
<li>Ensure that those 2 Halal team members, who have been trained, are still working</li>
</ul>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (105, 3, N'Primary contact persons (2) / all Halal Team members are correctly updated in the MeS', 25, N'')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (106, 1, N'Product / nature of business defined [HalMQ2]', 26, N'<p>This refers to the MeS Master Print</p>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (107, 2, N'Flow chart constructed & verified [HalMQ3]', 26, N'<ul>
<li>Steps in the flow chart include raw materials (RM) procurement, receiving, storage, food preparation, cooking, serving, collection of utensils and washing of utensils</li>
<li>To interview Halal team</li>
</ul>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (108, 3, N'Halal threats & their control measures identified [HalMQ4]', 26, N'<ul>
<li>Halal threats include purchase of non-Halal RM,<br />mixing of utensils involved in non-Halal food<br />handling, etc.</li>
<li>Control measures include Muis&rsquo; prior approval for RM,<br />colour code utensils differently from other stalls, etc.</li>
<li>To interview Halal team</li>
</ul>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (109, 4, N'Halal Assurance Points (HAP), their allowable limits & prescribed practices determined [HalMQ5]', 26, N'<ul>
<li>HAPs include point of RM purchase, point of receiving of RM, point of utensil collection, etc.</li>
<li>Allowable limits / prescribed practices include zero tolerance, 0.5% ethanol content, etc.</li>
<li>Advisable for HAPs to be indicated in flowchart</li>
<li>To interview Halal team</li>
</ul>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (110, 5, N'Monitoring system for each HAP established [HalMQ6]', 26, N'<ul>
<li>How, when, who &amp; what should be done to eliminate the Halal threats</li>
<li>To interview key personnel at HAP</li>
</ul>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (111, 6, N'Corrective action for each HAP established [HalMQ7]', 26, N'<ul>
<li>How, when, who &amp; what should be done to rectify any occurrence of a Halal threat</li>
<li>To interview key personnel at HAP</li>
</ul>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (112, 7, N'Documentation & record keeping system established [HalMQ8]', 26, N'<ul>
<li>Halal file for site audit shall comprise all docs required for Halal certification</li>
<li>Halal file for desktop audit @ Muis shall comprise MeS master print, HalMQ plan, staffing docs (Muslim staff &amp; Halal team), menu card &amp; supporting docs for all raw materials [Applicable only to new applns &amp; chain outlets]</li>
</ul>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (113, 1, N'Company name in MeS corresponds to the ACRA business profile / AVA license / HSA license / Name printed on packaging labels', 27, N'')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (114, 2, N'Premises address in MeS corresponds to the exact location address', 27, N'<p>Check and verify the building name &amp; unit no.</p>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (115, 3, N'Type of scheme corresponds to nature of business', 27, N'<ul>
<li>Whole Plant Scheme is not applicable to OEM and / or Brand owner</li>
</ul>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (116, 4, N'Size of premises corresponds to floor plan', 27, N'')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (117, 5, N'Establishment has applied for SF scheme if it stores raw materials offsite and self manages the storage', 27, N'')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (118, 6, N'Establishment has engaged 3PL for storage of raw materials offsite. 3PL has applied for SF scheme and this applicant had appointed a management representative in the Halal team of 3PL ', 27, N'')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (119, 7, N'Company has applied for EE scheme if it bears the same name as its retail outlets', 27, N'<p>To check the NEA license &amp; website for evidence that the establishment provides catering services</p>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (120, 8, N'Company has applied for FPA scheme if it delivers products, in loosely packed form, as a raw material for further processing', 27, N'')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (121, 9, N'Establishment engages third party for delivery services?
If yes check to ensure if the:
• Delivery service provider is Halal certified.
• Security labels purchased / ordered from Muis.', 27, N'')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (122, 1, N'Number of products applied for Halal certification is correct', 28, N'')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (123, 2, N'All products applied for Halal certification by Muis must be consistent with the names printed on the packaging', 28, N'<ul>
<li>To check the mockup of product labels</li>
<li>Must include the brand name &amp; product description</li>
<li>ABC strawberry milk &amp; ABC full cream milk are 2 pdts</li>
<li>ABC strawberry milk &amp; CDE strawberry milk are 2 pdts</li>
<li>ABC vanilla milk (1L) &amp; ABC vanilla milk (2L) is 1 pdt</li>
</ul>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (124, 3, N'Identify products that are not manufactured by the applicant (Trading Items).

For Product Scheme applicants:
• Check if the applicant had submitted list of trading items
• Ensure they are placed outside the scope of certification
• None of the trading items have Muis Halal logo printed on them, if they do report to Muis with evidences.
• None of the trading items involve pork and or its derivatives
• No Muis Halal logo is displayed on the transportation vehicle

For Whole Plant Scheme applicants:
• Ensure they are NOT placed within the same premises
• Ensure they are not traded under the same company name as applied for Halal certification. ', 28, N'')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (125, 4, N'For OEM products (not applicable for Whole Plant Scheme):

i) The manufacturer must be Halal-certified
ii) The brand owner must submit the Halal application if the product packaging does not bear details of the manufacturer
iii) The brand owner must submit separate Halal applns for pdts manufactured by diff companies
iv) The brand owner must be represented in the Halal team of the manufacturer for matters concerning the former’s products', 28, N'<p>For OEM products that do not bear the manufacturer&rsquo;s details, the applicant must be the brand owner.</p>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (126, 1, N'No other operator is to be allowed usage of the Halal certified premises without written prior approval from Muis.', 29, N'<p>To look out for an approval letter from Muis if subletting occurs</p>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (127, 2, N'Any printed or published materials, posters, adverts, signage & packaging that may mislead on the Halal status of the food / products sold are not allowed', 29, N'<p>To check website and other marketing materials for misleading claims</p>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (128, 3, N'NEA / AVA / HSA license available', 29, N'')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (129, 4, N'LOU indicating the following is available:

• Updated list of ALL stock-keeping units (SKUs) manufactured at the said location
• Agree to inform Muis, through the submission of ''Change Application'' via the MeS, prior to developing a new SKU and/or changing any raw materials/suppliers.', 29, N'')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (130, 1, N'All products intended for Halal certification are declared in the MeS', 30, N'')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (131, 2, N'Product labels are not misleading', 30, N'<p>Words such as &lsquo;bacon&rsquo;, &lsquo;ham&rsquo; &amp; &lsquo;sausages should be prefixed with the meat type e.g. &lsquo;chicken ham&rsquo;</p>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (132, 3, N'The MeS-generated Customer Code of the manufacturer is printed below the Muis Halal certification mark (coloured or black outline version) on all Muis Halal-certified products', 30, N'<p>Applies only to renewal applications</p>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (133, 1, N'All raw materials are declared in the MeS', 31, N'<ul>
<li>To randomly check with chef / relevant employees the ingredients used to prepare selected menu item</li>
<li>To check the raw materials inventory list</li>
<li>Check and verify with items in the store, chiller, freezer and kitchen premises</li>
</ul>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (134, 2, N'Supplier details of all raw materials are updated in the MeS', 31, N'')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (135, 3, N'Doubtful and non-Halal raw materials are not stored, used, sold or brought into the premises applied for certification', 31, N'<ul>
<li>Check documentations and ensure that the raw materials are not supplied by Muis-suspended companies</li>
<li>Check ALL raw materials, regardless of their use in the products applied for certification, to ensure that no pork or its derivatives are present (applicable for Product Scheme only)</li>
</ul>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (136, 4, N'All raw materials stored, used, sold or brought into the premises applied for certification must be properly packed & labelled with the product description, manufacturers’ name & plant address', 31, N'')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (137, 5, N'Purchase invoices / delivery orders for all raw materials are endorsed by Muslim staff within the Halal team', 31, N'<ul>
<li>Minimum past 3 months record (only for renewal appln)</li>
<li>Look out for irregularities where non-declared items were purchased / delivered (only for renewal appln)</li>
<li>Endorsement by Muslim staff must entail the staff name, signature &amp; date of check</li>
</ul>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (138, 6, N'All raw materials must be Halal & document-substantiated with the following:
i) Certificates/Marks from Muis or Muis-recognised
Halal certifying bodies
ii) Product specifications
iii) Detailed Halal questionnaire
iv) Laboratory analysis report', 31, N'<ul>
<li>The product information (i.e. product description, manufacturer&rsquo;s name / add &amp; Halal mark) tally with the supporting documents</li>
<li><strong>High risk items</strong> &ndash; Must be substantiated with valid Halal certificates from Muis-recognised bodies</li>
<li><strong>Med-High risk items</strong> &ndash; Good to be substantiated with Halal certificate/mark. Otherwise, Halal questionnaire required (Check that the document is completed &amp; endorsed with company stamp, representative name &amp; signature, &amp; date. Document is not valid if after &gt;1 year)</li>
<li><strong>Med-Low risk items</strong> &ndash; Good to be substantiated with Halal certificate/mark. Otherwise, specifications required (If doubtful ingredients found, Halal questionnaire required)</li>
<li><strong>Low risk items</strong> &ndash; Must be substantiated with product label</li>
<li>If in doubt, take picture / sample of item for clarification with HCSU or laboratory testing</li>
<li>For all Halal questionnaires ensure there is traceability to supplier &amp; manufacturer as indicated in the questionnaire</li>
</ul>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (139, 1, N'All production lines, crockery, kitchen utensils and equipment’s, cooking place, chillers, freezers, cold rooms, etc. must be ritually cleansed by Muis appointed personnel if they had been previously used for the preparation of pork and pork-related items', 32, N'')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (140, 2, N'All Halal food / raw materials must be physically segregated from non-Halal items during transportation.', 32, N'<p>Look out for evidence where raw materials are transported with pork items (Applicable for Product Scheme only)</p>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (141, 3, N'Cross contamination between the equipments/utensils used for Halal & non-Halal food should be avoided. (Applicable for Product Scheme only)', 32, N'<ul>
<li>To check the cleaning procedure if company uses doubtful raw materials for other products not intended for Halal certification</li>
<li>If brushes are used, ensure that they are not made of hog bristles</li>
</ul>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (142, 1, N'Employ at least 2 competent permanent Muslim staff (including a Muslim staff of supervisory level).', 33, N'<ul>
<li>There must be at least 1 Muslim staff during audit</li>
<li>Muslim staff may be permanent full-time or permanent part-time</li>
<li>Adequacy test &ndash; Check evidence for employment of &gt;3 Muslim staff through employment letter, punch cards, letter from Muis on waiver of Muslim staff &amp; interview of other staff</li>
<li>Competency test &ndash; Interview Muslim staff on general Halal certification guidelines &amp; his/her Halal-related job scope</li>
<li>Permanency test &ndash; Check terms in employment letter</li>
</ul>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (143, 1, N'Establish a Halal Team, comprising a managementappointed leader, at least one Muslim staff and relevant personnel from multi-disciplinary background, to ensure that the HCC are adhered to at all times [HALMQ1]', 34, N'<ul>
<li>Check for either one of the following:<br />i) List of Halal team members comprising the name, designation &amp; Halal-related job scope of all members of the Halal team. The list must be dated with the signatures of the Management &amp; appointees<br />ii) Appointment letters of all members in the Halal team. Each letter must indicate the member&rsquo;s Halal-related job scope as well as duly dated and signed by the Management &amp; appointee</li>
<li>Interview Halal team members to verify the above documents &amp; educate them on their roles &amp; responsibilities</li>
</ul>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (144, 2, N'Halal team, particularly the Muslim staff & at least one other member, undergo Muis Halal training programme', 34, N'<ul>
<li>Check for one of the following documents as evidence that at least 2 Halal team members have gone for the MA Halal Foundation Prog:<br />i) Training certificate<br />ii) Receipt of training<br />iii) Letter of confirmation</li>
<li>Ensure that those 2 Halal team members, who have been trained, are still working</li>
</ul>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (145, 3, N'Primary contact persons (2) / all Halal Team members are correctly updated in the MeS', 34, N'')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (146, 1, N'Product / nature of business defined [HalMQ2]', 35, N'<p>This refers to the MeS Master Print</p>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (147, 2, N'Flow chart constructed & verified [HalMQ3]', 35, N'<ul>
<li>Steps in the flow chart include raw materials (RM) procurement, receiving, storage, food preparation, cooking, serving, collection of utensils and washing of utensils</li>
<li>To interview Halal team</li>
</ul>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (148, 3, N'Halal threats & their control measures identified [HalMQ4]', 35, N'<ul>
<li>Halal threats include purchase of non-Halal RM, mixing of utensils involved in non-Halal food handling, etc.</li>
<li>Control measures include Muis&rsquo; prior approval for RM, colour code utensils differently from other stalls, etc.</li>
<li>To interview Halal team</li>
</ul>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (149, 4, N'Halal Assurance Points (HAP), their allowable limits & prescribed practices determined [HalMQ5]', 35, N'<ul>
<li>HAPs include point of RM purchase, point of receiving of RM, point of utensil collection, etc.</li>
<li>Allowable limits / prescribed practices include zero tolerance, 0.5% ethanol content, etc.</li>
<li>Advisable for HAPs to be indicated in flowchart</li>
<li>To interview Halal team</li>
</ul>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (150, 5, N'Monitoring system for each HAP established [HalMQ6]', 35, N'<ul>
<li>How, when, who &amp; what should be done to eliminate the Halal threats</li>
<li>To interview key personnel at HAP</li>
</ul>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (151, 6, N'Corrective action for each HAP established [HalMQ7]', 35, N'<ul>
<li>How, when, who &amp; what should be done to rectify any occurrence of a Halal threat</li>
<li>To interview key personnel at HAP</li>
</ul>')
GO
INSERT [dbo].[ChecklistItem] ([ID], [Index], [Text], [CategoryID], [Notes]) VALUES (152, 7, N'Documentation & record keeping system established [HalMQ8]', 35, N'<ul>
<li>Halal file for site audit shall comprise all docs required for Halal certification</li>
<li>Halal file for desktop audit @ Muis shall comprise MeS master print, HalMQ plan, staffing docs (Muslim staff &amp; Halal team), menu card &amp; supporting docs for all raw materials [Applicable only to new applns &amp; chain outlets]</li>
<li>Halal file for OEM products shall be placed at the OEM premises. The Halal file shall contain the brand owner&rsquo;s representative details, appointment letter (incl roles &amp; responsibilities), contract between brand owner &amp; OEM &amp; notes of meeting (Applicable for Product Scheme only)</li>
</ul>')
GO
SET IDENTITY_INSERT [dbo].[ChecklistItem] OFF
GO

SET IDENTITY_INSERT [dbo].[Cluster] ON 
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (1, N'01', N'Raffles Place, Cecil, Marina, People''s Park', CAST(N'2020-09-28T00:00:00.0000000' AS DateTime2), CAST(N'2020-10-14T13:57:38.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (2, N'02', N'Anson, Tanjong Pagar', CAST(N'2020-09-30T01:17:51.0000000' AS DateTime2), CAST(N'2020-10-01T00:17:07.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (3, N'03', N'Queenstown, Tiong Bahru', CAST(N'2020-09-30T11:49:35.0000000' AS DateTime2), CAST(N'2020-10-14T13:58:02.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (4, N'04', N'Telok Blangah, Harbourfront', CAST(N'2020-09-30T15:55:42.0000000' AS DateTime2), CAST(N'2020-10-15T14:49:58.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (5, N'06', N'High Street, Beach Road (part)', CAST(N'2020-10-01T16:32:37.0000000' AS DateTime2), CAST(N'2020-10-01T16:32:37.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (7, N'07', N'Middle Road, Golden Mile', CAST(N'2020-10-01T17:33:18.0000000' AS DateTime2), CAST(N'2020-10-01T17:33:18.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (8, N'08', N'Little India', CAST(N'2020-10-01T17:35:53.0000000' AS DateTime2), CAST(N'2020-10-01T17:35:53.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10, N'09', N'Orchard, Cairnhill, River Valley', CAST(N'2020-10-01T18:05:36.0000000' AS DateTime2), CAST(N'2020-10-01T18:05:36.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (11, N'10', N'Ardmore, Bukit Timah, Holland Road, Tanglin', CAST(N'2020-10-01T18:08:28.0000000' AS DateTime2), CAST(N'2020-10-01T18:08:28.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (12, N'11', N'Watten Estate, Novena, Thomson', CAST(N'2020-10-01T18:51:39.0000000' AS DateTime2), CAST(N'2020-10-01T18:51:39.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (13, N'05', N'Pasir Panjang, Hong Leong Garden, Clementi New Town', CAST(N'2020-10-01T19:22:21.0000000' AS DateTime2), CAST(N'2020-10-15T15:56:49.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (14, N'14', N'mys', CAST(N'2020-10-02T01:47:43.0000000' AS DateTime2), CAST(N'2020-10-02T01:49:29.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (15, N'15', N'mys', CAST(N'2020-10-02T15:37:02.0000000' AS DateTime2), CAST(N'2020-10-04T00:39:28.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (16, N'20', N'mys', CAST(N'2020-10-05T10:37:33.0000000' AS DateTime2), CAST(N'2020-10-05T10:37:53.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (17, N'', N'', CAST(N'2020-10-05T13:04:25.0000000' AS DateTime2), CAST(N'2020-10-05T13:04:25.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (18, N'', N'', CAST(N'2020-10-05T13:07:45.0000000' AS DateTime2), CAST(N'2020-10-05T13:07:45.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (19, N'', N'', CAST(N'2020-10-05T13:18:20.0000000' AS DateTime2), CAST(N'2020-10-05T13:18:20.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (20, N'', N'', CAST(N'2020-10-05T13:49:52.0000000' AS DateTime2), CAST(N'2020-10-05T13:49:52.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (21, N'21', N'mys', CAST(N'2020-10-05T13:52:20.0000000' AS DateTime2), CAST(N'2020-10-05T13:53:40.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (22, N'22', N'Mysore,Banglore', CAST(N'2020-10-05T14:52:43.0000000' AS DateTime2), CAST(N'2020-10-05T14:53:04.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (23, N'23', N'Raffies,Mys', CAST(N'2020-10-05T14:53:14.0000000' AS DateTime2), CAST(N'2020-10-05T14:53:32.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (24, N'24', N'Raffies,Mys,Ban', CAST(N'2020-10-05T14:53:40.0000000' AS DateTime2), CAST(N'2020-10-05T14:53:53.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (25, N'25', N'undefined', CAST(N'2020-10-05T15:24:04.0000000' AS DateTime2), CAST(N'2020-10-05T17:46:53.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (26, N'26', N'mys', CAST(N'2020-10-05T16:11:31.0000000' AS DateTime2), CAST(N'2020-10-14T13:58:08.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (27, N'27', N'mys', CAST(N'2020-10-05T16:16:26.0000000' AS DateTime2), CAST(N'2020-10-05T16:26:05.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (28, N'28', N'mys', CAST(N'2020-10-05T19:40:12.0000000' AS DateTime2), CAST(N'2020-10-05T19:40:56.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (29, N'', N'', CAST(N'2020-10-05T19:43:54.0000000' AS DateTime2), CAST(N'2020-10-05T19:43:54.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (30, N'', N'', CAST(N'2020-10-05T19:46:03.0000000' AS DateTime2), CAST(N'2020-10-05T19:46:03.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (31, N'31', N'Mysore,Banglore', CAST(N'2020-10-08T18:04:20.0000000' AS DateTime2), CAST(N'2020-10-08T18:04:47.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10005, N'', N'', CAST(N'2020-10-13T14:48:00.0000000' AS DateTime2), CAST(N'2020-10-13T14:48:00.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10006, N'10006', N'mys', CAST(N'2020-10-15T15:31:04.0000000' AS DateTime2), CAST(N'2020-10-15T15:32:21.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10007, N'1007', N'mys', CAST(N'2020-10-15T17:52:50.0000000' AS DateTime2), CAST(N'2020-10-15T18:00:38.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10008, N'10008', N'location1', CAST(N'2020-10-15T18:29:19.0000000' AS DateTime2), CAST(N'2020-10-15T18:33:51.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10009, N'', N'', CAST(N'2020-10-29T18:19:02.0000000' AS DateTime2), CAST(N'2020-10-29T18:19:02.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10010, N'', N'', CAST(N'2020-10-29T18:19:14.0000000' AS DateTime2), CAST(N'2020-10-29T18:19:14.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10011, N'', N'', CAST(N'2020-10-29T18:19:33.0000000' AS DateTime2), CAST(N'2020-10-29T18:19:33.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10012, N'', N'', CAST(N'2020-11-02T15:01:57.0000000' AS DateTime2), CAST(N'2020-11-02T15:01:57.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10013, N'', N'', CAST(N'2020-11-02T15:02:03.0000000' AS DateTime2), CAST(N'2020-11-02T15:02:03.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10014, N'', N'', CAST(N'2020-11-02T15:02:34.0000000' AS DateTime2), CAST(N'2020-11-02T15:02:34.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10015, N'', N'', CAST(N'2020-11-02T15:04:02.0000000' AS DateTime2), CAST(N'2020-11-02T15:04:02.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10016, N'', N'', CAST(N'2020-11-02T15:06:29.0000000' AS DateTime2), CAST(N'2020-11-02T15:06:29.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10017, N'', N'', CAST(N'2020-11-03T14:23:27.0000000' AS DateTime2), CAST(N'2020-11-03T14:23:27.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10018, N'', N'', CAST(N'2020-11-03T14:23:44.0000000' AS DateTime2), CAST(N'2020-11-03T14:23:44.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10019, N'', N'', CAST(N'2020-11-03T14:54:54.0000000' AS DateTime2), CAST(N'2020-11-03T14:54:54.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10020, N'', N'', CAST(N'2020-11-03T15:07:51.0000000' AS DateTime2), CAST(N'2020-11-03T15:07:51.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10021, N'12', N'131', CAST(N'2020-11-17T21:05:25.0000000' AS DateTime2), CAST(N'2020-11-17T22:17:14.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10022, N'01', N'Raffles Place, Cecil, Marina, People''s Park', CAST(N'2020-11-17T15:20:43.0000000' AS DateTime2), CAST(N'2020-11-17T15:20:43.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10023, N'02', N'Anson, Tanjong Pagar', CAST(N'2020-11-17T15:20:43.0000000' AS DateTime2), CAST(N'2020-11-17T15:20:43.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10024, N'03', N'Queenstown, Tiong Bahru', CAST(N'2020-11-17T15:20:43.0000000' AS DateTime2), CAST(N'2020-11-17T15:20:43.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10025, N'04', N'Telok Blangah, Harbourfront', CAST(N'2020-11-17T15:20:43.0000000' AS DateTime2), CAST(N'2020-11-17T15:20:43.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10026, N'05', N'Pasir Panjang, Hong Leong Garden, Clementi New Town', CAST(N'2020-11-17T15:20:43.0000000' AS DateTime2), CAST(N'2020-11-17T15:20:43.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10027, N'06', N'High Street, Beach Road (part)', CAST(N'2020-11-17T15:20:43.0000000' AS DateTime2), CAST(N'2020-11-17T15:20:43.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10028, N'07', N'Middle Road, Golden Mile', CAST(N'2020-11-17T15:20:43.0000000' AS DateTime2), CAST(N'2020-11-17T15:20:43.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10029, N'08', N'Little India', CAST(N'2020-11-17T15:20:43.0000000' AS DateTime2), CAST(N'2020-11-17T15:20:43.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10030, N'09', N'Orchard, Cairnhill, River Valley', CAST(N'2020-11-17T15:20:43.0000000' AS DateTime2), CAST(N'2020-11-17T15:20:43.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10031, N'10', N'Ardmore, Bukit Timah, Holland Road, Tanglin', CAST(N'2020-11-17T15:20:43.0000000' AS DateTime2), CAST(N'2020-11-17T15:20:43.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10032, N'11', N'Watten Estate, Novena, Thomson', CAST(N'2020-11-17T15:20:43.0000000' AS DateTime2), CAST(N'2020-11-17T15:20:43.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10033, N'12', N'Balestier, Toa Payoh, Serangoon', CAST(N'2020-11-17T15:20:43.0000000' AS DateTime2), CAST(N'2020-11-17T15:20:43.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10034, N'13', N'Macpherson, Braddell', CAST(N'2020-11-17T15:20:43.0000000' AS DateTime2), CAST(N'2020-11-17T15:20:43.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10035, N'14', N'Geylang, Eunos', CAST(N'2020-11-17T15:20:43.0000000' AS DateTime2), CAST(N'2020-11-17T15:20:43.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10036, N'15', N'Katong, Joo Chiat, Amber Road', CAST(N'2020-11-17T15:20:43.0000000' AS DateTime2), CAST(N'2020-11-17T15:20:43.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10037, N'16', N'Bedok, Upper East Coast, Eastwood, Kew Drive', CAST(N'2020-11-17T15:20:43.0000000' AS DateTime2), CAST(N'2020-11-17T15:20:43.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10038, N'17', N'Loyang, Changi', CAST(N'2020-11-17T15:20:43.0000000' AS DateTime2), CAST(N'2020-11-17T15:20:43.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10039, N'18', N'Tampines, Pasir Ris', CAST(N'2020-11-17T15:20:43.0000000' AS DateTime2), CAST(N'2020-11-17T15:20:43.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10040, N'19', N'Serangoon Garden, Hougang, Ponggol', CAST(N'2020-11-17T15:20:43.0000000' AS DateTime2), CAST(N'2020-11-17T15:20:43.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10041, N'20', N'Bishan, Ang Mo Kio', CAST(N'2020-11-17T15:20:43.0000000' AS DateTime2), CAST(N'2020-11-17T15:20:43.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10042, N'21', N'Upper Bukit Timah, Clementi Park, Ulu Pandan', CAST(N'2020-11-17T15:20:43.0000000' AS DateTime2), CAST(N'2020-11-17T15:20:43.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10043, N'22', N'Jurong', CAST(N'2020-11-17T15:20:43.0000000' AS DateTime2), CAST(N'2020-11-17T15:20:43.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10044, N'23', N'Hillview, Dairy Farm, Bukit Panjang, Choa Chu Kang', CAST(N'2020-11-17T15:20:43.0000000' AS DateTime2), CAST(N'2020-11-17T15:20:43.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10045, N'24', N'Lim Chu Kang, Tengah', CAST(N'2020-11-17T15:20:43.0000000' AS DateTime2), CAST(N'2020-11-17T15:20:43.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10046, N'25', N'Kranji, Woodgrove', CAST(N'2020-11-17T15:20:43.0000000' AS DateTime2), CAST(N'2020-11-17T15:20:43.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10047, N'26', N'Upper Thomson, Springleaf', CAST(N'2020-11-17T15:20:43.0000000' AS DateTime2), CAST(N'2020-11-17T15:20:43.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10048, N'27', N'Yishun, Sembawang', CAST(N'2020-11-17T15:20:43.0000000' AS DateTime2), CAST(N'2020-11-17T15:20:43.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10049, N'28', N'Seletar', CAST(N'2020-11-17T15:20:43.0000000' AS DateTime2), CAST(N'2020-11-17T15:20:43.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10050, N'01', N'RafflesPlace,Cecil,Marina,People''sPark', CAST(N'2020-11-17T15:24:51.0000000' AS DateTime2), CAST(N'2020-11-18T22:21:19.0000000' AS DateTime2), 0)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10051, N'02', N'Anson, Tanjong Pagar', CAST(N'2020-11-17T15:24:51.0000000' AS DateTime2), CAST(N'2020-11-17T15:24:51.0000000' AS DateTime2), 0)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10052, N'03', N'Queenstown, Tiong Bahru', CAST(N'2020-11-17T15:24:51.0000000' AS DateTime2), CAST(N'2020-11-17T15:24:51.0000000' AS DateTime2), 0)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10053, N'04', N'Telok Blangah, Harbourfront', CAST(N'2020-11-17T15:24:51.0000000' AS DateTime2), CAST(N'2020-11-17T15:24:51.0000000' AS DateTime2), 0)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10054, N'05', N'Pasir Panjang, Hong Leong Garden, Clementi New Town', CAST(N'2020-11-17T15:24:51.0000000' AS DateTime2), CAST(N'2020-11-17T15:24:51.0000000' AS DateTime2), 0)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10055, N'06', N'High Street, Beach Road (part)', CAST(N'2020-11-17T15:24:51.0000000' AS DateTime2), CAST(N'2020-11-17T15:24:51.0000000' AS DateTime2), 0)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10056, N'07', N'Middle Road, Golden Mile', CAST(N'2020-11-17T15:24:51.0000000' AS DateTime2), CAST(N'2020-11-17T15:24:51.0000000' AS DateTime2), 0)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10057, N'08', N'Little India', CAST(N'2020-11-17T15:24:51.0000000' AS DateTime2), CAST(N'2020-11-17T15:24:51.0000000' AS DateTime2), 0)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10058, N'09', N'Orchard, Cairnhill, River Valley', CAST(N'2020-11-17T15:24:51.0000000' AS DateTime2), CAST(N'2020-11-17T15:24:51.0000000' AS DateTime2), 0)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10059, N'10', N'Ardmore, Bukit Timah, Holland Road, Tanglin', CAST(N'2020-11-17T15:24:51.0000000' AS DateTime2), CAST(N'2020-11-17T15:24:51.0000000' AS DateTime2), 0)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10060, N'11', N'Watten Estate, Novena, Thomson', CAST(N'2020-11-17T15:24:51.0000000' AS DateTime2), CAST(N'2020-11-17T15:24:51.0000000' AS DateTime2), 0)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10061, N'12', N'Balestier, Toa Payoh, Serangoon', CAST(N'2020-11-17T15:24:51.0000000' AS DateTime2), CAST(N'2020-11-17T15:24:51.0000000' AS DateTime2), 0)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10062, N'13', N'Macpherson, Braddell', CAST(N'2020-11-17T15:24:51.0000000' AS DateTime2), CAST(N'2020-11-17T15:24:51.0000000' AS DateTime2), 0)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10063, N'14', N'Geylang, Eunos', CAST(N'2020-11-17T15:24:51.0000000' AS DateTime2), CAST(N'2020-11-17T15:24:51.0000000' AS DateTime2), 0)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10064, N'15', N'Katong, Joo Chiat, Amber Road', CAST(N'2020-11-17T15:24:51.0000000' AS DateTime2), CAST(N'2020-11-17T15:24:51.0000000' AS DateTime2), 0)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10065, N'16', N'Bedok, Upper East Coast, Eastwood, Kew Drive', CAST(N'2020-11-17T15:24:51.0000000' AS DateTime2), CAST(N'2020-11-17T15:24:51.0000000' AS DateTime2), 0)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10066, N'17', N'Loyang, Changi', CAST(N'2020-11-17T15:24:51.0000000' AS DateTime2), CAST(N'2020-11-17T15:24:51.0000000' AS DateTime2), 0)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10067, N'18', N'Tampines, Pasir Ris', CAST(N'2020-11-17T15:24:51.0000000' AS DateTime2), CAST(N'2020-11-17T15:24:51.0000000' AS DateTime2), 0)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10068, N'19', N'Serangoon Garden, Hougang, Ponggol', CAST(N'2020-11-17T15:24:51.0000000' AS DateTime2), CAST(N'2020-11-17T15:24:51.0000000' AS DateTime2), 0)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10069, N'20', N'Bishan, Ang Mo Kio', CAST(N'2020-11-17T15:24:51.0000000' AS DateTime2), CAST(N'2020-11-17T15:24:51.0000000' AS DateTime2), 0)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10070, N'21', N'Upper Bukit Timah, Clementi Park, Ulu Pandan', CAST(N'2020-11-17T15:24:51.0000000' AS DateTime2), CAST(N'2020-11-17T15:24:51.0000000' AS DateTime2), 0)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10071, N'22', N'Jurong', CAST(N'2020-11-17T15:24:51.0000000' AS DateTime2), CAST(N'2020-11-17T15:24:51.0000000' AS DateTime2), 0)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10072, N'23', N'Hillview, Dairy Farm, Bukit Panjang, Choa Chu Kang', CAST(N'2020-11-17T15:24:51.0000000' AS DateTime2), CAST(N'2020-11-17T15:24:51.0000000' AS DateTime2), 0)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10073, N'24', N'Lim Chu Kang, Tengah', CAST(N'2020-11-17T15:24:51.0000000' AS DateTime2), CAST(N'2020-11-17T15:24:51.0000000' AS DateTime2), 0)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10074, N'25', N'Kranji, Woodgrove', CAST(N'2020-11-17T15:24:51.0000000' AS DateTime2), CAST(N'2020-11-17T15:24:51.0000000' AS DateTime2), 0)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10075, N'26', N'Upper Thomson, Springleaf', CAST(N'2020-11-17T15:24:51.0000000' AS DateTime2), CAST(N'2020-11-17T15:24:51.0000000' AS DateTime2), 0)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10076, N'27', N'Yishun, Sembawang', CAST(N'2020-11-17T15:24:52.0000000' AS DateTime2), CAST(N'2020-11-17T15:24:52.0000000' AS DateTime2), 0)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10077, N'28', N'Seletar', CAST(N'2020-11-17T15:24:52.0000000' AS DateTime2), CAST(N'2020-11-17T15:24:52.0000000' AS DateTime2), 0)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10078, N'Name11', N'Location11', CAST(N'2020-11-18T00:29:29.0000000' AS DateTime2), CAST(N'2020-11-18T12:45:28.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10079, N'Name121', N'Location121', CAST(N'2020-11-18T12:07:56.0000000' AS DateTime2), CAST(N'2020-11-18T12:44:42.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10080, N'Test1234', N'test1234', CAST(N'2020-11-18T16:56:48.0000000' AS DateTime2), CAST(N'2020-11-18T17:00:35.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10081, N'12312312333', N'12312312333', CAST(N'2020-11-18T17:13:31.0000000' AS DateTime2), CAST(N'2020-11-18T17:15:13.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Cluster] ([ID], [District], [Locations], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10082, N'New Cluster', N'TEst', CAST(N'2020-11-19T11:55:55.0000000' AS DateTime2), CAST(N'2020-11-19T11:55:55.0000000' AS DateTime2), 0)
GO
SET IDENTITY_INSERT [dbo].[Cluster] OFF
GO


INSERT [dbo].[Identity] ([ID], [Name], [Designation], [Role], [Permissions], [Email], [Status], [AccessFailCount], [LockoutEnabled], [LockoutEndOn], [Sequence], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (N'00000000-0000-0000-0000-000000000000', N'Admin', N'Administrator', 800, N'1111111111111111111111111111111', N'admin@muis.gov.sg', 0, NULL, NULL, NULL, NULL, CAST(N'2020-10-30T16:05:06.0000000' AS DateTime2), CAST(N'2020-10-30T16:05:06.0000000' AS DateTime2), 0)
GO
INSERT [dbo].[Identity] ([ID], [Name], [Designation], [Role], [Permissions], [Email], [Status], [AccessFailCount], [LockoutEnabled], [LockoutEndOn], [Sequence], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (N'17ee427e-2519-49b0-878c-0a4ed1c393a4', N'Certificate Auditor 4', N'Certificate Auditor', 100, N'1100000000000000000000', N'ca.4@muis.gov.sg', 0, NULL, NULL, NULL, 1, CAST(N'2020-11-09T09:04:21.0000000' AS DateTime2), CAST(N'2020-11-11T05:49:29.0000000' AS DateTime2), 0)
GO
INSERT [dbo].[Identity] ([ID], [Name], [Designation], [Role], [Permissions], [Email], [Status], [AccessFailCount], [LockoutEnabled], [LockoutEndOn], [Sequence], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (N'8999818b-9cec-4425-b433-2111506c6bf5', N'Issuance Officer 1', N'Issuance Officer', 400, N'1001100000000000000000', N'io.1@muis.gov.sg', 0, NULL, NULL, NULL, NULL, CAST(N'2020-11-08T16:22:45.0000000' AS DateTime2), CAST(N'2020-11-08T16:22:45.0000000' AS DateTime2), 0)
GO
INSERT [dbo].[Identity] ([ID], [Name], [Designation], [Role], [Permissions], [Email], [Status], [AccessFailCount], [LockoutEnabled], [LockoutEndOn], [Sequence], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (N'5584839e-b6a6-4337-b275-2481cab35fb1', N'RFA Support 1', N'RFA Support', 101, N'1000000000000000000001', N'rfa.1@muis.gov.sg', 0, NULL, NULL, NULL, NULL, CAST(N'2020-11-17T05:57:27.0000000' AS DateTime2), CAST(N'2020-11-17T07:23:06.0000000' AS DateTime2), 0)
GO
INSERT [dbo].[Identity] ([ID], [Name], [Designation], [Role], [Permissions], [Email], [Status], [AccessFailCount], [LockoutEnabled], [LockoutEndOn], [Sequence], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (N'6f9c6554-7b31-45f7-906f-3d785d4d687f', N'Certificate Auditor 2', N'Certificate Auditor', 100, N'0100000000000000000010', N'ca.2@muis.gov.sg', 0, NULL, NULL, NULL, 1, CAST(N'2020-10-30T08:04:02.0000000' AS DateTime2), CAST(N'2020-11-12T05:20:07.0000000' AS DateTime2), 0)
GO
INSERT [dbo].[Identity] ([ID], [Name], [Designation], [Role], [Permissions], [Email], [Status], [AccessFailCount], [LockoutEnabled], [LockoutEndOn], [Sequence], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (N'2b15f3d8-4114-46c1-ae23-4a26e77356f0', N'Certificate Auditor 3', N'Certificate Auditor', 100, N'1100000000000000000000', N'ca.3@muis.gov.sg', 0, NULL, NULL, NULL, NULL, CAST(N'2020-11-09T09:01:00.0000000' AS DateTime2), CAST(N'2020-11-09T09:01:00.0000000' AS DateTime2), 0)
GO
INSERT [dbo].[Identity] ([ID], [Name], [Designation], [Role], [Permissions], [Email], [Status], [AccessFailCount], [LockoutEnabled], [LockoutEndOn], [Sequence], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (N'cc1004bc-d1b1-4b34-8675-75f90ebbee25', N'Mufti Officer', N'Mufti', 700, N'0000000000111000000000', N'mufti.1@muis.gov.sg', 0, NULL, NULL, NULL, NULL, CAST(N'2020-11-17T08:22:40.0000000' AS DateTime2), CAST(N'2020-11-17T08:22:40.0000000' AS DateTime2), 0)
GO
INSERT [dbo].[Identity] ([ID], [Name], [Designation], [Role], [Permissions], [Email], [Status], [AccessFailCount], [LockoutEnabled], [LockoutEndOn], [Sequence], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (N'1a65cc74-4951-4ca0-8b42-b5d522650d31', N'test', N'test', 800, N'111111111111111111111111111111111111111111', N'test@muis.gov.sg', 0, NULL, NULL, NULL, NULL, CAST(N'2020-11-18T04:10:38.0000000' AS DateTime2), CAST(N'2020-11-18T04:10:39.0000000' AS DateTime2), 0)
GO
INSERT [dbo].[Identity] ([ID], [Name], [Designation], [Role], [Permissions], [Email], [Status], [AccessFailCount], [LockoutEnabled], [LockoutEndOn], [Sequence], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (N'83cab734-5171-4221-a0cc-b882da31b138', N'Admin', N'Administrator', 800, N'111111111111111111111111111111111111111111', N'admin@muis.gov.sg', 0, NULL, NULL, NULL, NULL, CAST(N'2020-11-01T12:59:30.0000000' AS DateTime2), CAST(N'2020-11-17T08:58:35.0000000' AS DateTime2), 0)
GO
INSERT [dbo].[Identity] ([ID], [Name], [Designation], [Role], [Permissions], [Email], [Status], [AccessFailCount], [LockoutEnabled], [LockoutEndOn], [Sequence], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (N'4bfa6063-372f-4ba3-aebe-cb4080c18734', N'Certificate Auditor 1', N'Certificate Auditor', 100, N'1100000000000000000001', N'ca.1@muis.gov.sg', 0, NULL, NULL, NULL, 1, CAST(N'2020-10-28T09:13:30.0000000' AS DateTime2), CAST(N'2020-11-19T04:12:11.0000000' AS DateTime2), 0)
GO
INSERT [dbo].[Identity] ([ID], [Name], [Designation], [Role], [Permissions], [Email], [Status], [AccessFailCount], [LockoutEnabled], [LockoutEndOn], [Sequence], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (N'fad9fe68-7e8d-42bc-9278-cbbbb79588f3', N'Approving Officer 1', N'Approving Officer', 200, N'1010001000000000000001', N'ao.1@muis.gov.sg', 0, NULL, NULL, NULL, NULL, CAST(N'2020-11-11T09:33:19.0000000' AS DateTime2), CAST(N'2020-11-18T10:37:39.0000000' AS DateTime2), 0)
GO
INSERT [dbo].[Identity] ([ID], [Name], [Designation], [Role], [Permissions], [Email], [Status], [AccessFailCount], [LockoutEnabled], [LockoutEndOn], [Sequence], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (N'4056a344-b721-4bda-b63f-cfe95d62f43c', N'Razali', N'Administrator', 800, N'111111111111111111111111111111111111111111', N'razali_mohd_shariff@muis.gov.sg', 0, NULL, NULL, NULL, NULL, CAST(N'2020-11-19T04:52:06.0000000' AS DateTime2), CAST(N'2020-11-19T04:52:06.0000000' AS DateTime2), 0)
GO
INSERT [dbo].[Identity] ([ID], [Name], [Designation], [Role], [Permissions], [Email], [Status], [AccessFailCount], [LockoutEnabled], [LockoutEndOn], [Sequence], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (N'3ee6e60e-edcc-4179-9acf-d73c6c543ec7', N'Administrator 2', N'Administrator', 800, N'111111111111111111111111111111111111111111', N'admin.2@muis.gov.sg', 0, NULL, NULL, NULL, NULL, CAST(N'2020-11-19T02:09:39.0000000' AS DateTime2), CAST(N'2020-11-19T02:09:39.0000000' AS DateTime2), 0)
GO
INSERT [dbo].[Identity] ([ID], [Name], [Designation], [Role], [Permissions], [Email], [Status], [AccessFailCount], [LockoutEnabled], [LockoutEndOn], [Sequence], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (N'c59ef51a-8259-4880-bb86-e3cc40bbd79b', N'Periodic Inspection Officer', N'Periodic Inspector', 500, N'1000000011000000000000', N'pi.1@muis.gov.sg', 0, NULL, NULL, NULL, NULL, CAST(N'2020-11-18T10:32:08.0000000' AS DateTime2), CAST(N'2020-11-18T10:32:08.0000000' AS DateTime2), 0)
GO
INSERT [dbo].[Identity] ([ID], [Name], [Designation], [Role], [Permissions], [Email], [Status], [AccessFailCount], [LockoutEnabled], [LockoutEndOn], [Sequence], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (N'f4193b30-5199-4ac4-8e1c-e68beddcbdc9', N'Finance Officer 1', N'Finance Officer', 300, N'1000000000000111100000', N'fo.1@muis.gov.sg', 0, NULL, NULL, NULL, NULL, CAST(N'2020-11-08T16:19:52.0000000' AS DateTime2), CAST(N'2020-11-08T16:19:52.0000000' AS DateTime2), 0)
GO

INSERT [dbo].[IdentityClusters] ([IdentityID], [ClusterID]) VALUES (N'17ee427e-2519-49b0-878c-0a4ed1c393a4', 1)
GO
INSERT [dbo].[IdentityClusters] ([IdentityID], [ClusterID]) VALUES (N'17ee427e-2519-49b0-878c-0a4ed1c393a4', 2)
GO
INSERT [dbo].[IdentityClusters] ([IdentityID], [ClusterID]) VALUES (N'17ee427e-2519-49b0-878c-0a4ed1c393a4', 3)
GO
INSERT [dbo].[IdentityClusters] ([IdentityID], [ClusterID]) VALUES (N'6f9c6554-7b31-45f7-906f-3d785d4d687f', 1)
GO
INSERT [dbo].[IdentityClusters] ([IdentityID], [ClusterID]) VALUES (N'6f9c6554-7b31-45f7-906f-3d785d4d687f', 3)
GO
INSERT [dbo].[IdentityClusters] ([IdentityID], [ClusterID]) VALUES (N'6f9c6554-7b31-45f7-906f-3d785d4d687f', 4)
GO
INSERT [dbo].[IdentityClusters] ([IdentityID], [ClusterID]) VALUES (N'6f9c6554-7b31-45f7-906f-3d785d4d687f', 13)
GO
INSERT [dbo].[IdentityClusters] ([IdentityID], [ClusterID]) VALUES (N'2b15f3d8-4114-46c1-ae23-4a26e77356f0', 2)
GO
INSERT [dbo].[IdentityClusters] ([IdentityID], [ClusterID]) VALUES (N'2b15f3d8-4114-46c1-ae23-4a26e77356f0', 5)
GO
INSERT [dbo].[IdentityClusters] ([IdentityID], [ClusterID]) VALUES (N'4bfa6063-372f-4ba3-aebe-cb4080c18734', 1)
GO
INSERT [dbo].[IdentityClusters] ([IdentityID], [ClusterID]) VALUES (N'4bfa6063-372f-4ba3-aebe-cb4080c18734', 2)
GO
INSERT [dbo].[IdentityClusters] ([IdentityID], [ClusterID]) VALUES (N'4bfa6063-372f-4ba3-aebe-cb4080c18734', 10051)
GO
INSERT [dbo].[IdentityClusters] ([IdentityID], [ClusterID]) VALUES (N'c59ef51a-8259-4880-bb86-e3cc40bbd79b', 10050)
GO

INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10050, N'01')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10050, N'02')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10050, N'03')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10050, N'04')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10050, N'05')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10051, N'07')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10051, N'08')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10052, N'14')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10052, N'15')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10052, N'16')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10053, N'09')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10053, N'10')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10054, N'11')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10054, N'12')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10054, N'13')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10055, N'17')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10056, N'18')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10056, N'19')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10057, N'20')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10057, N'21')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10058, N'22')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10058, N'23')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10059, N'24')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10059, N'25')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10059, N'26')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10059, N'27')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10060, N'28')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10060, N'29')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10060, N'30')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10061, N'31')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10061, N'32')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10061, N'33')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10062, N'34')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10062, N'35')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10062, N'36')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10062, N'37')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10063, N'38')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10063, N'39')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10063, N'40')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10063, N'41')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10064, N'42')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10064, N'43')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10064, N'44')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10064, N'45')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10065, N'46')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10065, N'47')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10065, N'48')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10066, N'49')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10066, N'50')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10066, N'81')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10067, N'51')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10067, N'52')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10068, N'53')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10068, N'54')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10068, N'55')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10068, N'82')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10069, N'56')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10069, N'57')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10070, N'58')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10070, N'59')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10071, N'60')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10071, N'61')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10071, N'62')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10071, N'63')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10071, N'64')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10072, N'65')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10072, N'66')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10072, N'67')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10072, N'68')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10073, N'69')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10073, N'70')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10073, N'71')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10074, N'72')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10074, N'73')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10075, N'77')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10075, N'78')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10076, N'75')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10076, N'76')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10077, N'79')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10077, N'80')
GO
INSERT [dbo].[Nodes] ([ClusterID], [Node]) VALUES (10082, N'06')
GO


INSERT [dbo].[IdentityRequestTypes] ([IdentityID], [RequestType]) VALUES (N'17ee427e-2519-49b0-878c-0a4ed1c393a4', 0)
GO
INSERT [dbo].[IdentityRequestTypes] ([IdentityID], [RequestType]) VALUES (N'17ee427e-2519-49b0-878c-0a4ed1c393a4', 3)
GO
INSERT [dbo].[IdentityRequestTypes] ([IdentityID], [RequestType]) VALUES (N'6f9c6554-7b31-45f7-906f-3d785d4d687f', 0)
GO
INSERT [dbo].[IdentityRequestTypes] ([IdentityID], [RequestType]) VALUES (N'6f9c6554-7b31-45f7-906f-3d785d4d687f', 1)
GO
INSERT [dbo].[IdentityRequestTypes] ([IdentityID], [RequestType]) VALUES (N'2b15f3d8-4114-46c1-ae23-4a26e77356f0', 0)
GO
INSERT [dbo].[IdentityRequestTypes] ([IdentityID], [RequestType]) VALUES (N'2b15f3d8-4114-46c1-ae23-4a26e77356f0', 1)
GO
INSERT [dbo].[IdentityRequestTypes] ([IdentityID], [RequestType]) VALUES (N'4bfa6063-372f-4ba3-aebe-cb4080c18734', 0)
GO
INSERT [dbo].[IdentityRequestTypes] ([IdentityID], [RequestType]) VALUES (N'4bfa6063-372f-4ba3-aebe-cb4080c18734', 1)
GO
INSERT [dbo].[IdentityRequestTypes] ([IdentityID], [RequestType]) VALUES (N'c59ef51a-8259-4880-bb86-e3cc40bbd79b', 0)
GO
INSERT [dbo].[IdentityRequestTypes] ([IdentityID], [RequestType]) VALUES (N'c59ef51a-8259-4880-bb86-e3cc40bbd79b', 1)
GO
INSERT [dbo].[IdentityRequestTypes] ([IdentityID], [RequestType]) VALUES (N'c59ef51a-8259-4880-bb86-e3cc40bbd79b', 3)
GO
INSERT [dbo].[IdentityRequestTypes] ([IdentityID], [RequestType]) VALUES (N'c59ef51a-8259-4880-bb86-e3cc40bbd79b', 4)
GO


SET IDENTITY_INSERT [dbo].[Credential] ON 
GO
INSERT [dbo].[Credential] ([ID], [ProviderID], [Key], [Secret], [ExpiresOn], [IdentityID], [IsTemporary], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (2, 0, N'ca.1@muis.gov.sg', N'fzgO/nbIK28=', NULL, N'4bfa6063-372f-4ba3-aebe-cb4080c18734', 0, CAST(N'2020-10-28T09:13:30.0000000' AS DateTime2), CAST(N'2020-11-11T05:38:16.0000000' AS DateTime2), 0)
GO
INSERT [dbo].[Credential] ([ID], [ProviderID], [Key], [Secret], [ExpiresOn], [IdentityID], [IsTemporary], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (3, 0, N'ca.2@muis.gov.sg', N'fzgO/nbIK28=', NULL, N'6f9c6554-7b31-45f7-906f-3d785d4d687f', 0, CAST(N'2020-10-30T08:04:02.0000000' AS DateTime2), CAST(N'2020-11-01T12:47:24.0000000' AS DateTime2), 0)
GO
INSERT [dbo].[Credential] ([ID], [ProviderID], [Key], [Secret], [ExpiresOn], [IdentityID], [IsTemporary], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (4, 0, N'sysadmin@muis.gov.sg', N'fzgO/nbIK28=', NULL, N'00000000-0000-0000-0000-000000000000', 0, CAST(N'2020-10-30T16:05:06.0000000' AS DateTime2), CAST(N'2020-10-30T16:05:28.0000000' AS DateTime2), 0)
GO
INSERT [dbo].[Credential] ([ID], [ProviderID], [Key], [Secret], [ExpiresOn], [IdentityID], [IsTemporary], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (11, 0, N'rfa.1@muis.gov.sg', N'fzgO/nbIK28=', NULL, N'5584839e-b6a6-4337-b275-2481cab35fb1', 0, CAST(N'2020-11-17T05:57:27.0000000' AS DateTime2), CAST(N'2020-11-17T06:08:47.0000000' AS DateTime2), 0)
GO
INSERT [dbo].[Credential] ([ID], [ProviderID], [Key], [Secret], [ExpiresOn], [IdentityID], [IsTemporary], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (12, 0, N'mufti.1@muis.gov.sg', N'fzgO/nbIK28=', NULL, N'cc1004bc-d1b1-4b34-8675-75f90ebbee25', 0, CAST(N'2020-11-17T08:22:40.0000000' AS DateTime2), CAST(N'2020-11-17T08:23:29.0000000' AS DateTime2), 0)
GO
INSERT [dbo].[Credential] ([ID], [ProviderID], [Key], [Secret], [ExpiresOn], [IdentityID], [IsTemporary], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (14, 0, N'pi.1@muis.gov.sg', N'fzgO/nbIK28=', NULL, N'c59ef51a-8259-4880-bb86-e3cc40bbd79b', 0, CAST(N'2020-11-18T10:32:08.0000000' AS DateTime2), CAST(N'2020-11-18T10:35:15.0000000' AS DateTime2), 0)
GO
INSERT [dbo].[Credential] ([ID], [ProviderID], [Key], [Secret], [ExpiresOn], [IdentityID], [IsTemporary], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (15, 0, N'admin.2@muis.gov.sg', N'fzgO/nbIK28=', NULL, N'3ee6e60e-edcc-4179-9acf-d73c6c543ec7', 0, CAST(N'2020-11-19T02:09:39.0000000' AS DateTime2), CAST(N'2020-11-19T02:15:42.0000000' AS DateTime2), 0)
GO
INSERT [dbo].[Credential] ([ID], [ProviderID], [Key], [Secret], [ExpiresOn], [IdentityID], [IsTemporary], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (16, 0, N'razali_mohd_shariff@muis.gov.sg', N'jBH5l5I6lD8=', NULL, N'4056a344-b721-4bda-b63f-cfe95d62f43c', 1, CAST(N'2020-11-19T04:52:06.0000000' AS DateTime2), CAST(N'2020-11-19T04:52:58.0000000' AS DateTime2), 0)
GO
INSERT [dbo].[Credential] ([ID], [ProviderID], [Key], [Secret], [ExpiresOn], [IdentityID], [IsTemporary], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (5, 0, N'admin@muis.gov.sg', N'fzgO/nbIK28=', NULL, N'83cab734-5171-4221-a0cc-b882da31b138', 0, CAST(N'2020-11-01T12:59:31.0000000' AS DateTime2), CAST(N'2020-11-01T12:59:59.0000000' AS DateTime2), 0)
GO
INSERT [dbo].[Credential] ([ID], [ProviderID], [Key], [Secret], [ExpiresOn], [IdentityID], [IsTemporary], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (6, 0, N'fo.1@muis.gov.sg', N'fzgO/nbIK28=', NULL, N'f4193b30-5199-4ac4-8e1c-e68beddcbdc9', 0, CAST(N'2020-11-08T16:19:52.0000000' AS DateTime2), CAST(N'2020-11-11T09:35:20.0000000' AS DateTime2), 0)
GO
INSERT [dbo].[Credential] ([ID], [ProviderID], [Key], [Secret], [ExpiresOn], [IdentityID], [IsTemporary], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (7, 0, N'io.1@muis.gov.sg', N'fzgO/nbIK28=', NULL, N'8999818b-9cec-4425-b433-2111506c6bf5', 0, CAST(N'2020-11-08T16:22:45.0000000' AS DateTime2), CAST(N'2020-11-17T08:16:15.0000000' AS DateTime2), 0)
GO
INSERT [dbo].[Credential] ([ID], [ProviderID], [Key], [Secret], [ExpiresOn], [IdentityID], [IsTemporary], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (8, 0, N'ca.3@muis.gov.sg', N'fzgO/nbIK28=', NULL, N'2b15f3d8-4114-46c1-ae23-4a26e77356f0', 0, CAST(N'2020-11-09T09:01:00.0000000' AS DateTime2), CAST(N'2020-11-11T08:42:45.0000000' AS DateTime2), 0)
GO
INSERT [dbo].[Credential] ([ID], [ProviderID], [Key], [Secret], [ExpiresOn], [IdentityID], [IsTemporary], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (9, 0, N'ca.4@muis.gov.sg', N'fzgO/nbIK28=', NULL, N'17ee427e-2519-49b0-878c-0a4ed1c393a4', 0, CAST(N'2020-11-09T09:04:21.0000000' AS DateTime2), CAST(N'2020-11-11T05:51:26.0000000' AS DateTime2), 0)
GO
INSERT [dbo].[Credential] ([ID], [ProviderID], [Key], [Secret], [ExpiresOn], [IdentityID], [IsTemporary], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (10, 0, N'ao.1@muis.gov.sg', N'fzgO/nbIK28=', NULL, N'fad9fe68-7e8d-42bc-9278-cbbbb79588f3', 0, CAST(N'2020-11-11T09:33:19.0000000' AS DateTime2), CAST(N'2020-11-11T09:33:45.0000000' AS DateTime2), 0)
GO
INSERT [dbo].[Credential] ([ID], [ProviderID], [Key], [Secret], [ExpiresOn], [IdentityID], [IsTemporary], [CreatedOn], [ModifiedOn], [IsDeleted]) VALUES (13, 0, N'test@muis.gov.sg', N'fzgO/nbIK28=', NULL, N'1a65cc74-4951-4ca0-8b42-b5d522650d31', 1, CAST(N'2020-11-18T04:10:39.0000000' AS DateTime2), CAST(N'2020-11-18T04:10:39.0000000' AS DateTime2), 0)
GO
SET IDENTITY_INSERT [dbo].[Credential] OFF
GO