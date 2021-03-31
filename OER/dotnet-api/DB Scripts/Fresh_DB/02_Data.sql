SET IDENTITY_INSERT [UserMaster] ON 
GO
INSERT [UserMaster] ([Id], [TitleId], [FirstName], [MiddleName], [LastName], [CountryId], [StateId], [Gender], [Email], [PortalLanguageId], [DepartmentId], [DesignationId], [DateOfBirth], [Photo], [ProfileDescription], [SubjectsInterested], [ApprovalStatus], [CreatedOn], [UpdatedOn], [Active], [IsContributor], [IsAdmin], [IsEmailNotification], [LastLogin], [Theme], [IsVerifier]) VALUES (1, NULL, N'Admin', NULL, N'User', NULL, NULL, NULL, N'admin@oer.com', NULL, NULL, NULL, CAST(N'2018-12-30' AS Date), NULL, N'DO NOT DELETE', N'', 1, CAST(N'2019-04-06T10:11:34.780' AS DateTime), CAST(N'2019-06-06T19:38:05.543' AS DateTime), 1, 1, 1, 0, NULL, N'Gold', NULL)
SET IDENTITY_INSERT [UserMaster] OFF
GO
SET IDENTITY_INSERT [WebContentPages] ON 
GO
INSERT [WebContentPages] ([Id], [PageName], [PageName_Ar]) VALUES (1, N'About Organisation', N'?? ???????')
GO
INSERT [WebContentPages] ([Id], [PageName], [PageName_Ar]) VALUES (2, N'Help Center', N'???? ????????')
GO
INSERT [WebContentPages] ([Id], [PageName], [PageName_Ar]) VALUES (3, N'Contact Us', N'???? ???')
GO
INSERT [WebContentPages] ([Id], [PageName], [PageName_Ar]) VALUES (4, N'Terms of Service', N'???? ??????')
GO
INSERT [WebContentPages] ([Id], [PageName], [PageName_Ar]) VALUES (5, N'Privacy Policy', N'????? ????')
GO
INSERT [WebContentPages] ([Id], [PageName], [PageName_Ar]) VALUES (6, N'How it Works', N'??? ????')
GO
SET IDENTITY_INSERT [WebContentPages] OFF
GO
SET IDENTITY_INSERT [WebPageContent] ON 
GO
INSERT [WebPageContent] ([Id], [PageID], [PageContent], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [PageContent_Ar]) VALUES (1, 2, N'<!DOCTYPE html>
<html>
<head>
</head>
<body>
<section class="cs-g article-list" style="box-sizing: border-box; margin-bottom: 10px; float: left; width: 302.156px; margin-right: 30.8906px; clear: left; color: #474f60; font-family: ''Open Sans'', Helvetica, Arial, sans-serif; font-size: 14px; background-color: #ffffff;">
<div class="list-lead" style="box-sizing: border-box; line-height: 28.4375px; font-size: 18px; color: inherit; font-family: Arvo, Helvetica, sans-serif; margin: 10px 0px;"><a style="box-sizing: border-box; color: #474f60; text-decoration-line: none; font-size: 16px; font-weight: 600; letter-spacing: 0.2px;" title="About" href="https://help.oercommons.org/support/solutions/folders/42000070344">About&nbsp;<span class="item-count" style="box-sizing: border-box; display: inline-block; color: #9aa1a6; font-weight: normal; font-family: ''Open Sans'', Helvetica, Arial, sans-serif;">3</span></a></div>
<ul style="box-sizing: border-box; padding: 0px; margin: 10px 0px 0px; list-style-position: initial; list-style-image: initial;">
<li style="box-sizing: border-box; line-height: 21.875px; list-style: none; position: relative; padding-left: 30px; margin-bottom: 10px; background-image: none;">
<div class="ellipsis" style="box-sizing: border-box; white-space: nowrap; overflow: hidden; text-overflow: ellipsis;"><a style="box-sizing: border-box; color: #0b6a8e; text-decoration-line: none; line-height: 1.6;" href="https://help.oercommons.org/support/solutions/articles/42000014831-about-oer-commons">About OER Commons</a></div>
</li>
<li style="box-sizing: border-box; line-height: 21.875px; list-style: none; position: relative; padding-left: 30px; margin-bottom: 10px; background-image: none;">
<div class="ellipsis" style="box-sizing: border-box; white-space: nowrap; overflow: hidden; text-overflow: ellipsis;"><a style="box-sizing: border-box; color: #0b6a8e; text-decoration-line: none; line-height: 1.6;" href="https://help.oercommons.org/support/solutions/articles/42000014832-oer-commons-partners">OER Commons Partners</a></div>
</li>
<li style="box-sizing: border-box; line-height: 21.875px; list-style: none; position: relative; padding-left: 30px; margin-bottom: 10px; background-image: none;">
<div class="ellipsis" style="box-sizing: border-box; white-space: nowrap; overflow: hidden; text-overflow: ellipsis;"><a style="box-sizing: border-box; color: #0b6a8e; text-decoration-line: none; line-height: 1.6;" href="https://help.oercommons.org/support/solutions/articles/42000014835-what-are-open-educational-resources-">What are Open Educational Resources?</a></div>
</li>
</ul>
</section>
<section class="cs-g article-list" style="box-sizing: border-box; margin-bottom: 10px; float: left; width: 302.156px; margin-right: 0px; clear: right; color: #474f60; font-family: ''Open Sans'', Helvetica, Arial, sans-serif; font-size: 14px; background-color: #ffffff;">
<div class="list-lead" style="box-sizing: border-box; line-height: 28.4375px; font-size: 18px; color: inherit; font-family: Arvo, Helvetica, sans-serif; margin: 10px 0px;"><a style="box-sizing: border-box; color: #474f60; text-decoration-line: none; font-size: 16px; font-weight: 600; letter-spacing: 0.2px;" title="FAQ" href="https://help.oercommons.org/support/solutions/folders/42000069914">FAQ&nbsp;<span class="item-count" style="box-sizing: border-box; display: inline-block; color: #9aa1a6; font-weight: normal; font-family: ''Open Sans'', Helvetica, Arial, sans-serif;">1</span></a></div>
<ul style="box-sizing: border-box; padding: 0px; margin: 10px 0px 0px; list-style-position: initial; list-style-image: initial;">
<li style="box-sizing: border-box; line-height: 21.875px; list-style: none; position: relative; padding-left: 30px; margin-bottom: 10px; background-image: none;">
<div class="ellipsis" style="box-sizing: border-box; white-space: nowrap; overflow: hidden; text-overflow: ellipsis;"><a style="box-sizing: border-box; color: #0b6a8e; text-decoration-line: none; line-height: 1.6;" href="https://help.oercommons.org/support/solutions/articles/42000014826-how-do-i-publish-my-content-in-other-languages-">How do I publish my content in other languages?</a></div>
</li>
</ul>
</section>
<section class="cs-g article-list" style="box-sizing: border-box; margin-bottom: 10px; float: left; width: 302.156px; margin-right: 30.8906px; clear: left; color: #474f60; font-family: ''Open Sans'', Helvetica, Arial, sans-serif; font-size: 14px; background-color: #ffffff;">
<div class="list-lead" style="box-sizing: border-box; line-height: 28.4375px; font-size: 18px; color: inherit; font-family: Arvo, Helvetica, sans-serif; margin: 10px 0px;"><a style="box-sizing: border-box; color: #474f60; text-decoration-line: none; font-size: 16px; font-weight: 600; letter-spacing: 0.2px;" title="OER Commons Help" href="https://help.oercommons.org/support/solutions/folders/42000070354">OER Commons Help&nbsp;<span class="item-count" style="box-sizing: border-box; display: inline-block; color: #9aa1a6; font-weight: normal; font-family: ''Open Sans'', Helvetica, Arial, sans-serif;">3</span></a></div>
<ul style="box-sizing: border-box; padding: 0px; margin: 10px 0px 0px; list-style-position: initial; list-style-image: initial;">
<li style="box-sizing: border-box; line-height: 21.875px; list-style: none; position: relative; padding-left: 30px; margin-bottom: 10px; background-image: none;">
<div class="ellipsis" style="box-sizing: border-box; white-space: nowrap; overflow: hidden; text-overflow: ellipsis;"><a style="box-sizing: border-box; color: #0b6a8e; text-decoration-line: none; line-height: 1.6;" href="https://help.oercommons.org/support/solutions/articles/42000014833-how-resources-are-organized">How Resources Are Organized</a></div>
</li>
<li style="box-sizing: border-box; line-height: 21.875px; list-style: none; position: relative; padding-left: 30px; margin-bottom: 10px; background-image: none;">
<div class="ellipsis" style="box-sizing: border-box; white-space: nowrap; overflow: hidden; text-overflow: ellipsis;"><a style="box-sizing: border-box; color: #0b6a8e; text-decoration-line: none; line-height: 1.6;" href="https://help.oercommons.org/support/solutions/articles/42000014834-finding-resources">Finding Resources</a></div>
</li>
<li style="box-sizing: border-box; line-height: 21.875px; list-style: none; position: relative; padding-left: 30px; margin-bottom: 10px; background-image: none;">
<div class="ellipsis" style="box-sizing: border-box; white-space: nowrap; overflow: hidden; text-overflow: ellipsis;"><a style="box-sizing: border-box; color: #b2d46f; text-decoration-line: none; outline: 0px; outline-offset: -2px; line-height: 1.6;" href="https://help.oercommons.org/support/solutions/articles/42000014836-member-tools">Member Tools</a>&nbsp; &nbsp;&nbsp;</div>
</li>
</ul>
</section>
</body>
</html>', 1, CAST(N'2019-05-27T12:32:39.000' AS DateTime), 1, CAST(N'2019-11-21T14:35:52.337' AS DateTime), N'<!DOCTYPE html>
<html>
<head>
</head>
<body>
<p dir="RTL" style="margin-top: 0in; text-align: right; background: #F0F0F0; direction: rtl; unicode-bidi: embed;"><span lang="AR-SA" style="font-size: 14.0pt; mso-ansi-font-size: 13.5pt; mso-ascii-font-family: DroidArabicKufi; mso-hansi-font-family: DroidArabicKufi; color: #121212;">نبذة عن منارة</span></p>
<p dir="RTL" style="margin-top: 0in; text-align: justify; background: #F0F0F0; direction: rtl; unicode-bidi: embed;"><span lang="AR-SA" style="font-size: 14.0pt; mso-ansi-font-size: 13.5pt; mso-ascii-font-family: DroidArabicKufi; mso-hansi-font-family: DroidArabicKufi; color: #121212;">الأسئلة الشّائعة حول منصّة "منارة".</span></p>
<h1 dir="RTL" style="margin: 15.0pt 0in 7.5pt 0in;"><span lang="AR-SA" style="font-size: 14.0pt; mso-ansi-font-size: 13.5pt; line-height: 107%; font-family: ''Times New Roman'',serif; mso-ascii-font-family: DroidArabicKufi; mso-hansi-font-family: DroidArabicKufi; mso-bidi-font-family: ''Times New Roman''; mso-bidi-theme-font: major-bidi; color: #121212;">ما هي الموارد التعليمية المفتوحة؟؟؟</span></h1>
<p style="padding-left: 40px;"><span lang="AR-SA" style="font-size: 14.0pt; mso-ansi-font-size: 13.5pt; line-height: 107%; font-family: ''Times New Roman'',serif; mso-ascii-font-family: DroidArabicKufi; mso-hansi-font-family: DroidArabicKufi; mso-bidi-font-family: ''Times New Roman''; mso-bidi-theme-font: major-bidi; color: #121212;">الشرط الأول </span></p>
</body>
</html>')
GO
INSERT [WebPageContent] ([Id], [PageID], [PageContent], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [PageContent_Ar]) VALUES (2, 1, N'<!DOCTYPE html>
<html>
<head>
</head>
<body>
<p><span style="font-size: 10.0pt; line-height: 107%; font-family: ''Book Antiqua'',serif; mso-fareast-font-family: Calibri; mso-fareast-theme-font: minor-latin; mso-bidi-font-family: ''Simplified Arabic''; color: #121212; mso-ansi-language: EN-US; mso-fareast-language: EN-US; mso-bidi-language: AR-SA;">The UAE Ministry of Education launched (Manara) platform for the open educational sources, in order to support the learning and teaching all over the world, promote the culture of participation in the open learning, and provide a platform for the reliable Arab educational and learning sources, based in&nbsp;</span><span style="color: #121212; font-family: ''Book Antiqua'', serif; font-size: 10pt;">United Arab Emirates across the region and the world. These resources are &ldquo;open&rdquo; for all of us, and available for use in the learning and teaching. In addition, the resources and pathways available on the (Manara) platform contain licenses and copyrights to legitimize the use of these various educational resources and pathways.&nbsp;</span></p>
<p><span style="font-size: 10.0pt; line-height: 107%; font-family: ''Book Antiqua'',serif; mso-fareast-font-family: Calibri; mso-fareast-theme-font: minor-latin; mso-bidi-font-family: ''Simplified Arabic''; color: #121212; mso-ansi-language: EN-US; mso-fareast-language: EN-US; mso-bidi-language: AR-SA;">The platform engages community with rich educational resources which can support and develop the high quality educational curricula&nbsp; and meaningful learning, and enable students access to the educational sources and educational pathways which lead them to learn. Moreover, platform provides the basic system and tools required for users to effectively use it, create, share, amend and review of the open educational resources, as well as collaborate with others to create and discuss the open educational resources. The resources available include a wide range of the open educational resources, which will contribute to promote the resources based on Arabic language in the United Arab Emirates and region.</span></p>
<p>&nbsp;</p>
</body>
</html>', 1, CAST(N'2019-05-27T07:27:34.790' AS DateTime), 1, CAST(N'2019-11-22T08:53:39.487' AS DateTime), N'<!DOCTYPE html>
<html>
<head>
</head>
<body>
<p class="MsoNormal" style="text-align: right;" align="right"><strong><span dir="RTL" lang="AR-AE" style="font-size: 12.0pt; line-height: 107%; font-family: ''Sakkal Majalla''; mso-hansi-font-family: ''Sakkal Majalla''; mso-bidi-language: AR-AE;">حول منصة "منارة"</span></strong></p>
<p class="MsoNormal" style="mso-margin-bottom-alt: auto; text-align: right; mso-outline-level: 5;" align="right"><span dir="RTL" lang="AR">&nbsp;</span><span dir="RTL" lang="AR">دشّنت</span><span dir="RTL" lang="AR"> وزارة التّربية والتّعليم في دولة الإمارات العربية المتحدة منصّة (منارة) </span><span dir="RTL" lang="AR-AE">للمصادر التّعليمية المفتوحة</span><span dir="RTL" lang="AR-AE">؛</span> <span dir="RTL" lang="AR">من أجل دعم التّعليم والتعلّم على مستوى العالم، وكذلك من أجل تعزيز ثقافة المشاركة في التعليم المفتوح، وتوفير منصة للمصادر التربوية والتعليمية العربية الموثوقة، تكون قاعدتها &nbsp;دولة الإمارات العربية المتحدة على مستوى المنطقة والعالم. وهذه الموارد "مفتوحة" لنا جميعاً، ومتاحة للاستخدام في التعليم والتعلم. وبالإضافة إلى ذلك تحتوي الموارد والمساقات الموجودة على منصة (منارة) على تراخيص وحقوق نشر لإضفاء الشرعية على استخدام هذه الموارد والمساقات التعليمية المختلفة.</span></p>
<p id="tw-target-text" dir="rtl" data-placeholder="Translation"><span lang="ar" tabindex="0">وتعمل المنصة على إشراك المجتمع بموارد تعليمية غنية يمكنها دعم وتطوير المناهج التعليمية العالية الجودة، والتّعليم الهادف، وتمكين وصول الطلاب إلى المصادر التعليمية والمساقات التربوية التي تقودهم للتعلّم. بالإضافة إلى ذلك، توفر المنصة النظام الأساسي والأدوات اللازمة للمستخدمين لاستخدامها بفعالية، وإنشاء ومشاركة وتعديل ومراجعة الموارد التعليمية المفتوحة، بالإضافة إلى التعاون مع الآخرين؛ لإنشاء ومناقشة الموارد التعليمية المفتوحة. وتشمل</span>الموارد المتوفرة مجموعة واسعة من الموارد التعليمية المفتوحة، والتي من شأنها أن تسهم في تعزيز الموارد القائمة على اللغة العربية في دولة الإمارات العربية المتحدة والمنطقة.</p>
</body>
</html>')
GO
INSERT [WebPageContent] ([Id], [PageID], [PageContent], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [PageContent_Ar]) VALUES (3, 5, N'<!DOCTYPE html>
<html>
<head>
</head>
<body>
<h3 style="box-sizing: border-box; margin: 0px 0px 10px; padding: 0px; border: 0px; outline: 0px; font-variant-numeric: normal; font-variant-east-asian: normal; font-weight: 400; font-stretch: normal; font-size: 24px; line-height: 1.4; font-family: Arvo, serif; vertical-align: baseline; background: #f1f2f2; color: #474f60;">The Movement</h3>
<p style="box-sizing: border-box; margin: 0px 0px 10px; padding: 0px; border: 0px; outline: 0px; font-size: 16px; vertical-align: baseline; background: #f1f2f2; color: #474f60; font-family: ''Open Sans'', sans-serif;">The worldwide OER movement is rooted in the human right to access high-quality education. This shift in educational practice is not just about cost savings and easy access to openly licensed content; it&rsquo;s about participation and co-creation. Open Educational Resources (OER) offer opportunities for systemic change in teaching and learning content through engaging educators in new participatory processes and effective technologies for engaging with learning.</p>
<h3 style="box-sizing: border-box; margin: 0px 0px 10px; padding: 0px; border: 0px; outline: 0px; font-variant-numeric: normal; font-variant-east-asian: normal; font-weight: 400; font-stretch: normal; font-size: 24px; line-height: 1.4; font-family: Arvo, serif; vertical-align: baseline; background: #ffffff; color: #474f60;">Open Education Practice</h3>
<p style="box-sizing: border-box; margin: 0px 0px 10px; padding: 0px; border: 0px; outline: 0px; font-size: 16px; vertical-align: baseline; background: #ffffff; color: #474f60; font-family: ''Open Sans'', sans-serif;">The move to open education practice (OEP) is more than a shift in content, it is an immersive experience in collaborative teaching and learning. OEP leverages open education resources (OER) to expand the role of educators, allowing teachers to become curators, curriculum designers, and content creators. In sharing teaching tools and strategies, educators network their strengths and improve the quality of education for their students. </p>
<p style="box-sizing: border-box; margin: 0px; padding: 0px; border: 0px; outline: 0px; font-size: 16px; vertical-align: baseline; background: #ffffff; color: #474f60; font-family: ''Open Sans'', sans-serif;">With an open practice, educators are able to adjust their content, pedagogies, and approach based on their learners, without the limitations of &ldquo;all rights reserved&rdquo;.</p>
</body>
</html>', 1, CAST(N'2019-05-27T13:39:18.067' AS DateTime), 1, CAST(N'2019-10-26T10:17:54.703' AS DateTime), N'<!DOCTYPE html>
<html>
<head>
</head>
<body>
<p>الحركة</p>
<p>تتجذر حركة الموارد التعليمية المفتوحة في جميع أنحاء العالم في حق الإنسان في الحصول على تعليم عالي الجودة. هذا التحول في الممارسة التعليمية لا يقتصر فقط على توفير التكاليف وسهولة الوصول إلى المحتوى المرخّص بشكل مفتوح ؛ إنه يتعلق بالمشاركة والإبداع المشترك. توفر المصادر التعليمية المفتوحة (OER) فرصًا للتغيير المنهجي في محتوى التعليم والتعلم من خلال إشراك المعلمين في العمليات التشاركية الجديدة والتقنيات الفعالة للمشاركة في التعلم.</p>
<p>&nbsp;</p>
<p>ممارسة التعليم المفتوح</p>
<p>الانتقال إلى ممارسة التعليم المفتوح (OEP) هو أكثر من مجرد تحول في المحتوى ، بل هو تجربة غامرة في التعليم والتعلم التعاوني. تعزز OEP موارد التعليم المفتوح (OER) لتوسيع دور المعلمين ، مما يسمح للمعلمين بأن يصبحوا منسقين ، ومصممي المناهج ، ومنشئي المحتوى. عند مشاركة الأدوات والاستراتيجيات التعليمية ، يقوم المعلمون بربط نقاط قوتهم وتحسين جودة التعليم لطلابهم.</p>
<p>&nbsp;</p>
<p>من خلال الممارسة المفتوحة ، يكون المعلمون قادرين على ضبط محتواهم وتعليمهم ونهجهم استنادًا إلى متعلميهم ، دون قيود "جميع الحقوق محفوظة".</p>
</body>
</html>')
GO
INSERT [WebPageContent] ([Id], [PageID], [PageContent], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [PageContent_Ar]) VALUES (4, 4, N'<!DOCTYPE html>
<html>
<head>
</head>
<body>
<p><span style="font-size: 10pt; line-height: 107%; font-family: ''Book Antiqua'', serif;">The terms and conditions in the &ldquo;terms and conditions&rdquo; document are intended to technically and morally regulate the services, in order to provide the best level of service for all customers. This agreement is for kind information purposes only. Manara platform reserves the right to change&nbsp; any paragraph or/ and paragraphs contained in this document without obligation to notify the customer.</span></p>
<p><span style="font-size: 10pt; line-height: 107%; font-family: ''Book Antiqua'', serif;"><span style="font-size: 10pt; line-height: 107%;">You must read this document carefully before requesting any of the (Manara) services.</span></span></p>
<p><span style="font-size: 10pt; line-height: 107%; font-family: ''Book Antiqua'', serif;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;">The provision of service by us is based primarily on your full consent to all what contained in the terms and conditions document.</span></span></span></p>
<p><span style="font-size: 10pt; line-height: 107%; font-family: ''Book Antiqua'', serif;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;">General conditions</span></span></span></span></p>
<p><span style="font-size: 10pt; line-height: 107%; font-family: ''Book Antiqua'', serif;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;">1- Manara reserves the right to reject and / or stop any service provided by Manara team to any person or entity without obligation to give reasons.</span></span></span></span></span></p>
<p><span style="font-size: 10pt; line-height: 107%; font-family: ''Book Antiqua'', serif;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;">2- Manara is committed to being clear and transparent with its customers in all stages of service provision.</span></span></span></span></span></span></p>
<p><span style="font-size: 10pt; line-height: 107%; font-family: ''Book Antiqua'', serif;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;">3- The circumvention in the use of any services shall be deemed a breach of the agreement between the customer and Manara platform.</span></span></span></span></span></span></span></p>
<p><span style="font-size: 10pt; line-height: 107%; font-family: ''Book Antiqua'', serif;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;">The fair usage</span></span></span></span></span></span></span></span></p>
<p><span style="font-size: 10pt; line-height: 107%; font-family: ''Book Antiqua'', serif;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;">- </span><span style="font-size: 10pt; line-height: 107%;">Manara has the fair usage policy in the use of resources and services provided by us, and reserves its right to take what it sees fit for the services that misuse the resources, which may affect the level of quality provided.</span></span></span></span></span></span></span></span></span></p>
<p><span style="font-size: 10pt; line-height: 107%; font-family: ''Book Antiqua'', serif;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;">-Consumption of the processors and memory on the servers is measured to ensure they dont reach the maximum per package.</span></span></span></span></span></span></span></span></span></span></p>
<p><span style="font-size: 10pt; line-height: 107%; font-family: ''Book Antiqua'', serif;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;">-The fair usage policy applies to all pathways and resources, which are uploaded on the Manara platform</span></span></span></span></span></span></span></span></span></span></p>
<p><span style="font-size: 10pt; line-height: 107%; font-family: ''Book Antiqua'', serif;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><strong><span style="font-size: 10pt; line-height: 107%;">Reactivation of the customer&rsquo;s account</span></strong></span></span></span></span></span></span></span></span></span></span></p>
<p><span style="font-size: 10pt; line-height: 107%; font-family: ''Book Antiqua'', serif;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;">In case of closure of the customer&rsquo;s account and his desire to reopen it, contact the technical support team.</span></span></span></span></span></span></span></span></span></span></span></p>
<p><strong><span style="font-size: 10pt; line-height: 107%; font-family: ''Book Antiqua'', serif;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;">Cancellation of services</span></span></span></span></span></span></span></span></span></span></span></span></strong></p>
<p><span style="font-size: 10pt; line-height: 107%; font-family: ''Book Antiqua'', serif;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;">Manara reserves the right to stop&nbsp; provision of technical support for the accounts in violation of the terms and conditions of services.</span></span></span></span></span></span></span></span></span></span></span></span></span></p>
<p><span style="font-size: 10pt; line-height: 107%; font-family: ''Book Antiqua'', serif;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;">In case the customer requests to cancel the service, this is checked with the customer, in order to cancel the service by (Manara)</span></span></span></span></span></span></span></span></span></span></span></span></span></span></p>
<p><strong><span style="font-size: 10pt; line-height: 107%; font-family: ''Book Antiqua'', serif;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;">Responsibility of (Manara)</span></span></span></span></span></span></span></span></span></span></span></span></span></span></span></strong></p>
<p><span style="font-size: 10pt; line-height: 107%; font-family: ''Book Antiqua'', serif;">Manara is not responsible for the content of sites it hosts or the opinions contained herein. Bust the customer bears the full responsibility before the legal and governmental entities.</span></p>
<p><span style="font-size: 10pt; line-height: 107%; font-family: ''Book Antiqua'', serif;"><span style="font-size: 10pt; line-height: 107%;">Manara is not responsible for any damages caused to <span style="background: white; mso-shading-themecolor: background1;">the customers</span> due to interruption of any service and exerts every efforts to provide the best possible performance of service.</span></span></p>
<p><span style="font-size: 10pt; line-height: 107%; font-family: ''Book Antiqua'', serif;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;">Manara is solely responsible for the technical aspect. The technical estimates are exclusively valued by the technical team of the platform.</span></span></span></p>
<p><span style="font-size: 10pt; line-height: 107%; font-family: ''Book Antiqua'', serif;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;">The customers is fully responsible for his account and contents associated with it in any Manara services. It may be also influenced by the external conditions, and we exert our best to ensure the best result.</span></span></span></span></p>
<p><span style="font-size: 10pt; line-height: 107%; font-family: ''Book Antiqua'', serif;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;">Licenses of programs and systems</span></span></span></span></span></p>
<p><span style="font-size: 10pt; line-height: 107%; font-family: ''Book Antiqua'', serif;">Manara commits to provide a licensed and legal services for the programs and systems used to deliver the services through it or by its respective team.</span></p>
<p><span style="font-size: 10pt; line-height: 107%; font-family: ''Book Antiqua'', serif;"><span style="font-size: 10pt; line-height: 107%;">Prohibited materials</span></span></p>
<p><span style="font-size: 10pt; line-height: 107%; font-family: ''Book Antiqua'', serif;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%; color: #333333;">1.&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span style="font-size: 10pt; line-height: 107%; color: #333333;">Using the site in order to commit an offense or to encourage others to involve in any actions which may be considered a crime or involve a civil liability.</span></span></span></p>
<p><span style="font-size: 10pt; line-height: 107%; font-family: ''Book Antiqua'', serif;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%; color: #333333;"><span style="font-size: 10pt; line-height: 107%;">2.&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span style="font-size: 10pt; line-height: 107%;">Including or publishing any unlawful contents which include discrimination, defamation, abuse or inappropriate materials.</span></span></span></span></p>
<p><span style="font-size: 10pt; line-height: 107%; font-family: ''Book Antiqua'', serif;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%; color: #333333;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;">3.&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span style="font-size: 10pt; line-height: 107%;">Using the site in order to impersonate other persons or parties.</span></span></span></span></span></p>
<p><span style="font-size: 10pt; line-height: 107%; font-family: ''Book Antiqua'', serif;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%; color: #333333;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;"><span style="font-size: 10pt; line-height: 107%;">4.&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span style="font-size: 10pt; line-height: 107%;">Using the site in order to upload any material has programs containing viruses, Trujan horses, and any computer codes, files and programs&nbsp; that may alter, damage or impede the site, any device or program belonging to any person accesses the site.</span></span></span></span></span></span></p>
</body>
</html>', 1, CAST(N'2019-05-27T15:42:29.723' AS DateTime), 1, CAST(N'2019-11-23T08:00:51.510' AS DateTime), N'<!DOCTYPE html>
<html>
<head>
</head>
<body>
<p>هدف</p>
<p>يتم توفير محتويات وخدمات ACU-OER من قبل أمانة ACU في دائرة المعلومات التعليمية والبحثية الكورية (KERIS). يرجى قراءة شروط الاستخدام هذه ("الشروط") وسياسة خصوصية ACU-OER قبل التسجيل في www.aseanoer.net أو استخدام أي جزء من موقع ACU-OER ("الموقع الإلكتروني" ، الذي يتكون من جميع المحتويات و الصفحات الموجودة داخل نطاق الويب www.aseanoer.net). يمكنك الوصول إلى موقع الويب ، بما في ذلك الوصول إلى أي مواد تعليمية أو لوحات إعلانات أو خدمات إلكترونية أخرى ("الخدمات"). نظرًا لأن بعض خدماتنا قد تكون برنامجًا يتم تنزيله على أجهزة الكمبيوتر أو الهواتف أو الأجهزة اللوحية أو الأجهزة الأخرى ، فإنك توافق على أننا قد نقوم بتحديث هذا البرنامج تلقائيًا ، وأن هذه الشروط سوف تنطبق على هذه التحديثات. هذه الشروط وسياسة الخصوصية هي اتفاقيات ("الاتفاقيات") بينك وبين ACU-OER. باستخدام خدماتنا ، فإنك توافق على الالتزام بهذه الشروط ، بما في ذلك سياسة الخصوصية ، سواء كنت مستخدماً مسجلاً أم لا. إذا كنت لا تفهم أو لا توافق على الالتزام بشروط الاتفاقيات ، فيجب ألا تستخدم موقع الويب.</p>
<p>تأثير وتعديل الشروط</p>
<p>أ. الخدمات جميع الخدمات المتعلقة بالتعلم عبر الإنترنت التي تقدمها ACU-OER.</p>
<p>ب. ACU-OER لموقع الويب وصفحات LMS الرئيسية و CMS ، والتي تتكون من جميع المحتوى والصفحات الموجودة داخل نطاق الويب www.aseanoer.net</p>
<p>ج. المستخدم أي شخص يوافق على هذه الشروط من خلال تسجيل الدخول إلى الموقع الإلكتروني ويسجل حساب المستخدم الخاص به / لها في موقع ACU-OER من خلال تقديم معلوماته الشخصية ومصرح له باستخدام الخدمات التي يوفرها ACT</p>
<p>د. اتفاقيات استخدام الخدمة - جميع الاتفاقيات ، بما في ذلك هذه الشروط ، المبرمة بين ACU-OER وأعضائها فيما يتعلق باستخدام الخدمات</p>
<p>ه. كلمة المرور هي مجموعة من الأحرف والأرقام الفريدة التي يعدها العضو لمصادقة المستخدم</p>
<p>F. الانسحاب من اتفاقات استخدام الخدمة من قبل ACU-OER أو من قبل العضو</p>
<p>يجوز لأي مصطلح لم يتم تعريفه في ما سبق من بين المصطلحات المستخدمة في هذه الشروط اتباع القوانين والأدلة ذات الصلة على كل موقع ، أو الامتثال بطريقة أخرى للممارسات العرفية.</p>
<p>&nbsp;</p>
<p>&nbsp;</p>
</body>
</html>')
GO
INSERT [WebPageContent] ([Id], [PageID], [PageContent], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [PageContent_Ar]) VALUES (5, 3, N'<!DOCTYPE html>
<html>
<head>
</head>
<body>
<p>This is a text message for contact us. This is a text message for contact us. This is a text message for contact us. This is a text message for contact us. This is a text message for contact us. This is a text message for contact us. This is a text message for contact us. This is a text message for contact us. This is a text message for contact us. This is a text message for contact us. This is a text message for contact us. This is a text message for contact us. This is a text message for contact us. This is a text message for contact us. This is a text message for contact us. This is a text message for contact us. This is a text message for contact us. This is a text message for contact us. This is a text message for contact us. This is a text message for contact us.&nbsp;</p>
<p>&nbsp;</p>
</body>
</html>', 1, CAST(N'2019-05-29T06:43:34.720' AS DateTime), 1, CAST(N'2019-10-26T10:20:40.203' AS DateTime), N'<!DOCTYPE html>
<html>
<head>
</head>
<body>
<p>هذه رسالة نصية للاتصال بنا. هذه رسالة نصية للاتصال بنا. هذه رسالة نصية للاتصال بنا. هذه رسالة نصية للاتصال بنا. هذه رسالة نصية للاتصال بنا. هذه رسالة نصية للاتصال بنا. هذه رسالة نصية للاتصال بنا. هذه رسالة نصية للاتصال بنا. هذه رسالة نصية للاتصال بنا. هذه رسالة نصية للاتصال بنا. هذه رسالة نصية للاتصال بنا. هذه رسالة نصية للاتصال بنا. هذه رسالة نصية للاتصال بنا. هذه رسالة نصية للاتصال بنا. هذه رسالة نصية للاتصال بنا. هذه رسالة نصية للاتصال بنا. هذه رسالة نصية للاتصال بنا. هذه رسالة نصية للاتصال بنا. هذه رسالة نصية للاتصال بنا. هذه رسالة نصية للاتصال بنا.</p>
</body>
</html>')
GO
INSERT [WebPageContent] ([Id], [PageID], [PageContent], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [PageContent_Ar]) VALUES (8, 6, N'<!DOCTYPE html>
<html>
<head>
</head>
<body>
<p>This is how it works!!!!&nbsp;&nbsp;</p>
</body>
</html>', 1, CAST(N'2019-06-06T10:11:17.627' AS DateTime), 1, CAST(N'2019-10-26T10:18:13.100' AS DateTime), N'<!DOCTYPE html>
<html>
<head>
</head>
<body>
<p>هذه بيانات اختبار لكيفية عملها</p>
</body>
</html>')
GO
SET IDENTITY_INSERT [WebPageContent] OFF
GO
SET IDENTITY_INSERT [CountryMaster] ON 
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (4, N'AE', N'United Arab Emirates', 1, CAST(N'2019-06-10T07:06:07.680' AS DateTime), 1, CAST(N'2019-06-10T07:06:07.680' AS DateTime), 1, N'الإمارات العربية المتحدة')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (5, NULL, N'Afghanistan', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'أفغانستان')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (6, NULL, N'Albania', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'ألبانيا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (7, NULL, N'Algeria', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'الجزائر')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (8, NULL, N'American Samoa', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'ساموا-الأمريكي')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (9, NULL, N'Andorra', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'أندورا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (10, NULL, N'Angola', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'أنغولا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (11, NULL, N'Anguilla', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'أنغويلا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (12, NULL, N'Antarctica', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'أنتاركتيكا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (13, NULL, N'Antigua and Barbuda', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'أنتيغوا وبربودا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (14, NULL, N'Argentina', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'الأرجنتين')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (15, NULL, N'Armenia', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'أرمينيا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (16, NULL, N'Aruba', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'أروبه')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (17, NULL, N'Australia', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'أستراليا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (18, NULL, N'Austria', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'النمسا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (19, NULL, N'Azerbaijan', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'أذربيجان')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (20, NULL, N'Bahamas', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'الباهاماس')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (21, NULL, N'Bahrain', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'البحرين')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (22, NULL, N'Bangladesh', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'بنغلاديش')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (23, NULL, N'Barbados', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'بربادوس')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (24, NULL, N'Belarus', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'روسيا البيضاء')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (25, NULL, N'Belgium', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'بلجيكا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (26, NULL, N'Belize', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'بيليز')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (27, NULL, N'Benin', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'بنين')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (28, NULL, N'Bermuda', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'جزر برمودا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (29, NULL, N'Bhutan', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'بوتان')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (30, NULL, N'Bolivia', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'بوليفيا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (31, NULL, N'Bosnia and Herzegovina', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'البوسنة و الهرسك')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (32, NULL, N'Botswana', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'بوتسوانا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (33, NULL, N'Brazil', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'البرازيل')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (34, NULL, N'Brunei Darussalam', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'بروني')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (35, NULL, N'Bulgaria', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'بلغاريا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (36, NULL, N'Burkina Faso', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'بوركينا فاسو')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (37, NULL, N'Burundi', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'بوروندي')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (38, NULL, N'Cambodia', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'كمبوديا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (39, NULL, N'Cameroon', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'كاميرون')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (40, NULL, N'Canada', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'كندا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (41, NULL, N'Cape Verde', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'الرأس الأخضر')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (42, NULL, N'Central African Republic', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'جمهورية أفريقيا الوسطى')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (43, NULL, N'Chad', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'تشاد')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (44, NULL, N'Chile', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'شيلي')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (45, NULL, N'China', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'جمهورية الصين الشعبية')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (46, NULL, N'Colombia', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'كولومبيا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (47, NULL, N'Comoros', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'جزر القمر')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (48, NULL, N'Democratic Republic', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'جمهورية الكونغو الديمقراطية')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (49, NULL, N'Congo, Republic of (Brazzaville)', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'جمهورية الكونغو')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (50, NULL, N'Cook Islands', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'جزر كوك')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (51, NULL, N'Costa Rica', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'كوستاريكا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (52, NULL, N'Cote d”Ivoire', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'ساحل العاج')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (53, NULL, N'Croatia', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'كرواتيا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (54, NULL, N'Cuba', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'كوبا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (55, NULL, N'Cyprus', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'قبرص')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (56, NULL, N'Czech Republic', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'الجمهورية التشيكية')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (57, NULL, N'Denmark', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'الدانمارك')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (58, NULL, N'Djibouti', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'جيبوتي')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (59, NULL, N'Dominica', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'دومينيكا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (60, NULL, N'Dominican Republic', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'الجمهورية الدومينيكية')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (61, NULL, N'East Timor Timor-Leste', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'تيمور الشرقية')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (62, NULL, N'Ecuador', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'إكوادور')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (63, NULL, N'Egypt', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'مصر')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (64, NULL, N'El Salvador', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'إلسلفادور')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (65, NULL, N'Equatorial Guinea', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'غينيا الاستوائي')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (66, NULL, N'Eritrea', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'إريتريا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (67, NULL, N'Estonia', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'استونيا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (68, NULL, N'Ethiopia', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'أثيوبيا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (69, NULL, N'Faroe Islands', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'جزر فارو')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (70, NULL, N'Fiji', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'فيجي')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (71, NULL, N'Finland', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'فنلندا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (72, NULL, N'France', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'فرنسا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (73, NULL, N'French Guiana', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'غويانا الفرنسية')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (74, NULL, N'French Polynesia', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'بولينيزيا الفرنسية')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (75, NULL, N'Gabon', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'الغابون')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (76, NULL, N'Gambia', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'غامبيا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (77, NULL, N'Georgia', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'جيورجيا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (78, NULL, N'Germany', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'ألمانيا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (79, NULL, N'Ghana', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'غانا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (80, NULL, N'Gibraltar', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'جبل طارق')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (81, NULL, N'Greece', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'اليونان')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (82, NULL, N'Greenland', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'جرينلاند')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (83, NULL, N'Grenada', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'غرينادا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (84, NULL, N'Guadeloupe', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'جزر جوادلوب')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (85, NULL, N'Guam', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'جوام')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (86, NULL, N'Guatemala', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'غواتيمال')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (87, NULL, N'Guinea', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'غينيا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (88, NULL, N'Guinea-Bissau', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'غينيا-بيساو')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (89, NULL, N'Guyana', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'غيانا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (90, NULL, N'Haiti', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'هايتي')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (91, NULL, N'Honduras', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'هندوراس')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (92, NULL, N'Hong Kong', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'هونغ كونغ')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (93, NULL, N'Hungary', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'المجر')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (94, NULL, N'Iceland', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'آيسلندا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (95, NULL, N'India', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'الهند')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (96, NULL, N'Indonesia', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'أندونيسيا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (97, NULL, N'Iran', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'إيران')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (98, NULL, N'Iraq', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'العراق')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (99, NULL, N'Ireland', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'إيرلندا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (100, NULL, N'Italy', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'إيطاليا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (101, NULL, N'Jamaica', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'جمايكا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (102, NULL, N'Japan', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'اليابان')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (103, NULL, N'Jordan', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'الأردن')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (104, NULL, N'Kazakhstan', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'كازاخستان')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (105, NULL, N'Kenya', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'كينيا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (106, NULL, N'Kiribati', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'كيريباتي')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (107, NULL, N'Korea, (North Korea)', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'كوريا الشمالية')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (108, NULL, N'Korea, (South Korea)', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'كوريا الجنوبية')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (109, NULL, N'Kuwait', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'الكويت')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (110, NULL, N'Kyrgyzstan', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'قيرغيزستان')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (111, NULL, N'Lao, PDR', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'لاوس')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (112, NULL, N'Latvia', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'لاتفيا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (113, NULL, N'Lebanon', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'لبنان')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (114, NULL, N'Lesotho', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'ليسوتو')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (115, NULL, N'Liberia', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'ليبيريا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (116, NULL, N'Libya', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'ليبيا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (117, NULL, N'Liechtenstein', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'ليختنشتين')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (118, NULL, N'Lithuania', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'لتوانيا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (119, NULL, N'Luxembourg', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'لوكسمبورغ')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (120, NULL, N'Macao', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'ماكاو')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (121, NULL, N'Macedonia, Rep. of', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'مقدونيا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (122, NULL, N'Madagascar', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'مدغشقر')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (123, NULL, N'Malawi', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'مالاوي')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (124, NULL, N'Malaysia', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'ماليزيا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (125, NULL, N'Maldives', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'المالديف')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (126, NULL, N'Mali', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'مالي')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (127, NULL, N'Malta', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'مالطا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (128, NULL, N'Marshall Islands', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'جزر مارشال')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (129, NULL, N'Martinique', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'مارتينيك')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (130, NULL, N'Mauritania', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'موريتانيا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (131, NULL, N'Mauritius', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'موريشيوس')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (132, NULL, N'Mexico', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'المكسيك')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (133, NULL, N'Micronesia', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'مايكرونيزيا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (134, NULL, N'Moldova', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'مولدافيا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (135, NULL, N'Monaco', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'موناكو')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (136, NULL, N'Mongolia', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'منغوليا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (137, NULL, N'Montenegro', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'الجبل الأسو')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (138, NULL, N'Montserrat', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'مونتسيرات')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (139, NULL, N'Morocco', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'المغرب')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (140, NULL, N'Mozambique', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'موزمبيق')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (141, NULL, N'Myanmar, Burma', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'ميانمار')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (142, NULL, N'Namibia', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'ناميبيا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (143, NULL, N'Nauru', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'نورو')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (144, NULL, N'Nepal', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'نيبال')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (145, NULL, N'Netherlands', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'هولندا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (146, NULL, N'Netherlands Antilles', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'جزر الأنتيل الهولندي')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (147, NULL, N'New Caledonia', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'كاليدونيا الجديدة')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (148, NULL, N'New Zealand', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'نيوزيلندا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (149, NULL, N'Nicaragua', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'نيكاراجوا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (150, NULL, N'Niger', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'النيجر')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (151, NULL, N'Nigeria', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'نيجيريا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (152, NULL, N'Niue', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'ني')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (153, NULL, N'Northern Mariana Islands', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'جزر ماريانا الشمالية')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (154, NULL, N'Norway', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'النرويج')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (155, NULL, N'Oman', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'عُمان')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (156, NULL, N'Pakistan', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'باكستان')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (157, NULL, N'Palau', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'بالاو')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (158, NULL, N'Palestine', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'فلسطين')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (159, NULL, N'Panama', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'بنما')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (160, NULL, N'Papua New Guinea', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'بابوا غينيا الجديدة')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (161, NULL, N'Paraguay', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'باراغواي')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (162, NULL, N'Peru', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'بيرو')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (163, NULL, N'Philippines', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'الفليبين')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (164, NULL, N'Poland', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'بولونيا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (165, NULL, N'Portugal', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'البرتغال')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (166, NULL, N'Puerto Rico', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'بورتو ريكو')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (167, NULL, N'Qatar', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'قطر')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (168, NULL, N'Reunion Island', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'ريونيون')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (169, NULL, N'Romania', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'رومانيا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (170, NULL, N'Russia', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'روسيا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (171, NULL, N'Rwanda', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'رواندا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (172, NULL, N'Saint Kitts and Nevis', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'سانت كيتس ونيفس')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (173, NULL, N'Saint Lucia', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'سانت لوسيا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (174, NULL, N'Saint Vincent and the', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'سانت فنسنت وجزر غرينادين')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (175, NULL, N'Samoa', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'المناطق')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (176, NULL, N'San Marino', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'سان مارينو')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (177, NULL, N'Sao Tome and Príncipe', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'ساو تومي وبرينسيبي')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (178, NULL, N'Saudi Arabia', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'المملكة العربية السعودية')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (179, NULL, N'Senegal', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'السنغال')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (180, NULL, N'Serbia', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'جمهورية صربيا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (181, NULL, N'Seychelles', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'سيشيل')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (182, NULL, N'Sierra Leone', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'سيراليون')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (183, NULL, N'Singapore', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'سنغافورة')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (184, NULL, N'Slovakia', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'سلوفاكيا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (185, NULL, N'Slovenia', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'سلوفينيا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (186, NULL, N'Solomon Islands', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'جزر سليمان')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (187, NULL, N'Somalia', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'الصومال')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (188, NULL, N'South Africa', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'جنوب أفريقيا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (189, NULL, N'Spain', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'إسبانيا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (190, NULL, N'Sri Lanka', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'سريلانكا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (191, NULL, N'Sudan', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'السودان')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (192, NULL, N'Suriname', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'سورينام')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (193, NULL, N'Swaziland', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'سوازيلند')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (194, NULL, N'Sweden', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'السويد')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (195, NULL, N'Switzerland', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'سويسرا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (196, NULL, N'Syria', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'سوريا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (197, NULL, N'Taiwan', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'تايوان')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (198, NULL, N'Tajikistan', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'طاجيكستان')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (199, NULL, N'Tanzania', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'تنزانيا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (200, NULL, N'Thailand', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'تايلندا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (201, NULL, N'Tibet', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'تبت')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (202, NULL, N'Timor-Leste (East Timor)', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'تيمور الشرقية')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (203, NULL, N'Togo', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'توغو')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (204, NULL, N'Tonga', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'تونغا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (205, NULL, N'Trinidad and Tobago', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'ترينيداد وتوباغو')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (206, NULL, N'Tunisia', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'تونس')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (207, NULL, N'Turkey', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'تركيا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (208, NULL, N'Turkmenistan', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'تركمانستان')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (209, NULL, N'Tuvalu', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'توفالو')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (210, NULL, N'Uganda', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'أوغندا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (211, NULL, N'Ukraine', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'أوكرانيا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (212, NULL, N'United Kingdom', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'المملكة المتحدة')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (213, NULL, N'United States', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'الولايات المتحدة')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (214, NULL, N'Uruguay', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'أورغواي')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (215, NULL, N'Uzbekistan', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'أوزباكستان')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (216, NULL, N'Vanuatu', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'فانواتو')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (217, NULL, N'Vatican City State', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'الفاتيكان')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (218, NULL, N'Venezuela', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'فنزويلا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (219, NULL, N'Vietnam', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'فيتنام')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (220, NULL, N'Virgin Islands (British)', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'الجزر العذراء البريطانية')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (221, NULL, N'Virgin Islands (U.S.)', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'الجزر العذراء الأمريكي')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (222, NULL, N'Wallis and ', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'والس وفوتونا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (223, NULL, N'Western Sahara', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'الصحراء الغربية')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (224, NULL, N'Yemen', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'اليمن')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (225, NULL, N'Zambia', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'زامبيا')
GO
INSERT [CountryMaster] ([Id], [Code], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (226, NULL, N'Zimbabwe', 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, CAST(N'2019-10-13T23:24:24.320' AS DateTime), 1, N'زمبابوي')
GO
SET IDENTITY_INSERT [CountryMaster] OFF
GO
SET IDENTITY_INSERT [StateMaster] ON 
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (8, 4, N'Abu Dhabi', 1, CAST(N'2019-06-10T07:16:58.653' AS DateTime), 1, CAST(N'2019-06-10T07:16:58.653' AS DateTime), 1, N'أبو ظبي')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (9, 4, N'Ajman', 1, CAST(N'2019-06-10T07:17:24.557' AS DateTime), 1, CAST(N'2019-06-10T07:17:24.557' AS DateTime), 1, N'عجمان')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (10, 4, N'Dubai', 1, CAST(N'2019-06-10T07:17:54.750' AS DateTime), 1, CAST(N'2019-06-10T07:17:54.750' AS DateTime), 1, N'دبي')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (11, 4, N'Fujairah', 1, CAST(N'2019-06-10T07:18:20.653' AS DateTime), 1, CAST(N'2019-06-10T07:18:20.653' AS DateTime), 1, N'الفجيرة')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (12, 4, N'Ras Al Khaimah', 1, CAST(N'2019-06-10T07:18:48.653' AS DateTime), 1, CAST(N'2019-06-10T07:18:48.653' AS DateTime), 1, N'رأس الخيمة')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (13, 4, N'Sharjah', 1, CAST(N'2019-06-10T07:19:55.980' AS DateTime), 1, CAST(N'2019-06-10T07:19:55.980' AS DateTime), 1, N'الشارقة')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (14, 4, N'Umm Al Quwain', 1, CAST(N'2019-06-10T07:20:37.043' AS DateTime), 1, CAST(N'2019-06-10T07:20:37.043' AS DateTime), 1, N'أم القيوين')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (15, 5, N'N/A', 1, CAST(N'2019-10-13T23:56:55.900' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.900' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (16, 6, N'N/A', 1, CAST(N'2019-10-13T23:56:55.900' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.900' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (17, 7, N'N/A', 1, CAST(N'2019-10-13T23:56:55.903' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.903' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (18, 8, N'N/A', 1, CAST(N'2019-10-13T23:56:55.903' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.903' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (19, 9, N'N/A', 1, CAST(N'2019-10-13T23:56:55.907' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.907' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (20, 10, N'N/A', 1, CAST(N'2019-10-13T23:56:55.907' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.907' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (21, 11, N'N/A', 1, CAST(N'2019-10-13T23:56:55.910' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.910' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (22, 12, N'N/A', 1, CAST(N'2019-10-13T23:56:55.910' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.910' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (23, 13, N'N/A', 1, CAST(N'2019-10-13T23:56:55.910' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.910' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (24, 14, N'N/A', 1, CAST(N'2019-10-13T23:56:55.910' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.910' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (25, 15, N'N/A', 1, CAST(N'2019-10-13T23:56:55.913' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.913' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (26, 16, N'N/A', 1, CAST(N'2019-10-13T23:56:55.913' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.913' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (27, 17, N'N/A', 1, CAST(N'2019-10-13T23:56:55.913' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.913' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (28, 18, N'N/A', 1, CAST(N'2019-10-13T23:56:55.917' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.917' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (29, 19, N'N/A', 1, CAST(N'2019-10-13T23:56:55.917' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.917' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (30, 20, N'N/A', 1, CAST(N'2019-10-13T23:56:55.917' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.917' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (31, 21, N'N/A', 1, CAST(N'2019-10-13T23:56:55.917' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.917' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (32, 22, N'N/A', 1, CAST(N'2019-10-13T23:56:55.920' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.920' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (33, 23, N'N/A', 1, CAST(N'2019-10-13T23:56:55.920' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.920' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (34, 24, N'N/A', 1, CAST(N'2019-10-13T23:56:55.923' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.923' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (35, 25, N'N/A', 1, CAST(N'2019-10-13T23:56:55.923' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.923' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (36, 26, N'N/A', 1, CAST(N'2019-10-13T23:56:55.927' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.927' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (37, 27, N'N/A', 1, CAST(N'2019-10-13T23:56:55.927' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.927' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (38, 28, N'N/A', 1, CAST(N'2019-10-13T23:56:55.930' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.930' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (39, 29, N'N/A', 1, CAST(N'2019-10-13T23:56:55.930' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.930' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (40, 30, N'N/A', 1, CAST(N'2019-10-13T23:56:55.930' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.930' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (41, 31, N'N/A', 1, CAST(N'2019-10-13T23:56:55.930' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.930' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (42, 32, N'N/A', 1, CAST(N'2019-10-13T23:56:55.930' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.930' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (43, 33, N'N/A', 1, CAST(N'2019-10-13T23:56:55.930' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.930' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (44, 34, N'N/A', 1, CAST(N'2019-10-13T23:56:55.933' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.933' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (45, 35, N'N/A', 1, CAST(N'2019-10-13T23:56:55.933' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.933' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (46, 36, N'N/A', 1, CAST(N'2019-10-13T23:56:55.940' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.940' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (47, 37, N'N/A', 1, CAST(N'2019-10-13T23:56:55.940' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.940' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (48, 38, N'N/A', 1, CAST(N'2019-10-13T23:56:55.940' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.940' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (49, 39, N'N/A', 1, CAST(N'2019-10-13T23:56:55.943' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.943' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (50, 40, N'N/A', 1, CAST(N'2019-10-13T23:56:55.943' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.943' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (51, 41, N'N/A', 1, CAST(N'2019-10-13T23:56:55.943' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.943' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (52, 42, N'N/A', 1, CAST(N'2019-10-13T23:56:55.943' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.943' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (53, 43, N'N/A', 1, CAST(N'2019-10-13T23:56:55.947' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.947' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (54, 44, N'N/A', 1, CAST(N'2019-10-13T23:56:55.947' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.947' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (55, 45, N'N/A', 1, CAST(N'2019-10-13T23:56:55.947' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.947' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (56, 46, N'N/A', 1, CAST(N'2019-10-13T23:56:55.947' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.947' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (57, 47, N'N/A', 1, CAST(N'2019-10-13T23:56:55.947' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.947' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (58, 48, N'N/A', 1, CAST(N'2019-10-13T23:56:55.950' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.950' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (59, 49, N'N/A', 1, CAST(N'2019-10-13T23:56:55.950' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.950' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (60, 50, N'N/A', 1, CAST(N'2019-10-13T23:56:55.953' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.953' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (61, 51, N'N/A', 1, CAST(N'2019-10-13T23:56:55.953' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.953' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (62, 52, N'N/A', 1, CAST(N'2019-10-13T23:56:55.953' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.953' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (63, 53, N'N/A', 1, CAST(N'2019-10-13T23:56:55.953' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.953' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (64, 54, N'N/A', 1, CAST(N'2019-10-13T23:56:55.957' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.957' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (65, 55, N'N/A', 1, CAST(N'2019-10-13T23:56:55.957' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.957' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (66, 56, N'N/A', 1, CAST(N'2019-10-13T23:56:55.960' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.960' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (67, 57, N'N/A', 1, CAST(N'2019-10-13T23:56:55.960' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.960' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (68, 58, N'N/A', 1, CAST(N'2019-10-13T23:56:55.960' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.960' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (69, 59, N'N/A', 1, CAST(N'2019-10-13T23:56:55.960' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.960' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (70, 60, N'N/A', 1, CAST(N'2019-10-13T23:56:55.960' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.960' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (71, 61, N'N/A', 1, CAST(N'2019-10-13T23:56:55.960' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.960' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (72, 62, N'N/A', 1, CAST(N'2019-10-13T23:56:55.963' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.963' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (73, 63, N'N/A', 1, CAST(N'2019-10-13T23:56:55.963' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.963' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (74, 64, N'N/A', 1, CAST(N'2019-10-13T23:56:55.963' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.963' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (75, 65, N'N/A', 1, CAST(N'2019-10-13T23:56:55.963' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.963' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (76, 66, N'N/A', 1, CAST(N'2019-10-13T23:56:55.967' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.967' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (77, 67, N'N/A', 1, CAST(N'2019-10-13T23:56:55.967' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.967' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (78, 68, N'N/A', 1, CAST(N'2019-10-13T23:56:55.970' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.970' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (79, 69, N'N/A', 1, CAST(N'2019-10-13T23:56:55.970' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.970' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (80, 70, N'N/A', 1, CAST(N'2019-10-13T23:56:55.970' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.970' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (81, 71, N'N/A', 1, CAST(N'2019-10-13T23:56:55.973' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.973' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (82, 72, N'N/A', 1, CAST(N'2019-10-13T23:56:55.973' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.973' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (83, 73, N'N/A', 1, CAST(N'2019-10-13T23:56:55.973' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.973' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (84, 74, N'N/A', 1, CAST(N'2019-10-13T23:56:55.973' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.973' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (85, 75, N'N/A', 1, CAST(N'2019-10-13T23:56:55.977' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.977' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (86, 76, N'N/A', 1, CAST(N'2019-10-13T23:56:55.977' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.977' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (87, 77, N'N/A', 1, CAST(N'2019-10-13T23:56:55.977' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.977' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (88, 78, N'N/A', 1, CAST(N'2019-10-13T23:56:55.977' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.977' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (89, 79, N'N/A', 1, CAST(N'2019-10-13T23:56:55.980' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.980' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (90, 80, N'N/A', 1, CAST(N'2019-10-13T23:56:55.980' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.980' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (91, 81, N'N/A', 1, CAST(N'2019-10-13T23:56:55.980' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.980' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (92, 82, N'N/A', 1, CAST(N'2019-10-13T23:56:55.980' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.980' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (93, 83, N'N/A', 1, CAST(N'2019-10-13T23:56:55.980' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.980' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (94, 84, N'N/A', 1, CAST(N'2019-10-13T23:56:55.983' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.983' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (95, 85, N'N/A', 1, CAST(N'2019-10-13T23:56:55.983' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.983' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (96, 86, N'N/A', 1, CAST(N'2019-10-13T23:56:55.987' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.987' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (97, 87, N'N/A', 1, CAST(N'2019-10-13T23:56:55.987' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.987' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (98, 88, N'N/A', 1, CAST(N'2019-10-13T23:56:55.990' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.990' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (99, 89, N'N/A', 1, CAST(N'2019-10-13T23:56:55.990' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.990' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (100, 90, N'N/A', 1, CAST(N'2019-10-13T23:56:55.990' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.990' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (101, 91, N'N/A', 1, CAST(N'2019-10-13T23:56:55.990' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.990' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (102, 92, N'N/A', 1, CAST(N'2019-10-13T23:56:55.990' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.990' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (103, 93, N'N/A', 1, CAST(N'2019-10-13T23:56:55.990' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.990' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (104, 94, N'N/A', 1, CAST(N'2019-10-13T23:56:55.993' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.993' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (105, 95, N'N/A', 1, CAST(N'2019-10-13T23:56:55.993' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.993' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (106, 96, N'N/A', 1, CAST(N'2019-10-13T23:56:55.993' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.993' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (107, 97, N'N/A', 1, CAST(N'2019-10-13T23:56:55.993' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.993' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (108, 98, N'N/A', 1, CAST(N'2019-10-13T23:56:55.997' AS DateTime), 1, CAST(N'2019-10-13T23:56:55.997' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (109, 99, N'N/A', 1, CAST(N'2019-10-13T23:56:56.000' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.000' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (110, 100, N'N/A', 1, CAST(N'2019-10-13T23:56:56.000' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.000' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (111, 101, N'N/A', 1, CAST(N'2019-10-13T23:56:56.003' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.003' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (112, 102, N'N/A', 1, CAST(N'2019-10-13T23:56:56.007' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.007' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (113, 103, N'N/A', 1, CAST(N'2019-10-13T23:56:56.010' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.010' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (114, 104, N'N/A', 1, CAST(N'2019-10-13T23:56:56.010' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.010' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (115, 105, N'N/A', 1, CAST(N'2019-10-13T23:56:56.010' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.010' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (116, 106, N'N/A', 1, CAST(N'2019-10-13T23:56:56.013' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.013' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (117, 107, N'N/A', 1, CAST(N'2019-10-13T23:56:56.017' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.017' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (118, 108, N'N/A', 1, CAST(N'2019-10-13T23:56:56.017' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.017' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (119, 109, N'N/A', 1, CAST(N'2019-10-13T23:56:56.017' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.017' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (120, 110, N'N/A', 1, CAST(N'2019-10-13T23:56:56.020' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.020' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (121, 111, N'N/A', 1, CAST(N'2019-10-13T23:56:56.020' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.020' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (122, 112, N'N/A', 1, CAST(N'2019-10-13T23:56:56.020' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.020' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (123, 113, N'N/A', 1, CAST(N'2019-10-13T23:56:56.020' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.020' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (124, 114, N'N/A', 1, CAST(N'2019-10-13T23:56:56.020' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.020' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (125, 115, N'N/A', 1, CAST(N'2019-10-13T23:56:56.020' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.020' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (126, 116, N'N/A', 1, CAST(N'2019-10-13T23:56:56.020' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.020' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (127, 117, N'N/A', 1, CAST(N'2019-10-13T23:56:56.023' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.023' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (128, 118, N'N/A', 1, CAST(N'2019-10-13T23:56:56.023' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.023' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (129, 119, N'N/A', 1, CAST(N'2019-10-13T23:56:56.023' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.023' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (130, 120, N'N/A', 1, CAST(N'2019-10-13T23:56:56.023' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.023' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (131, 121, N'N/A', 1, CAST(N'2019-10-13T23:56:56.027' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.027' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (132, 122, N'N/A', 1, CAST(N'2019-10-13T23:56:56.027' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.027' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (133, 123, N'N/A', 1, CAST(N'2019-10-13T23:56:56.027' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.027' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (134, 124, N'N/A', 1, CAST(N'2019-10-13T23:56:56.030' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.030' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (135, 125, N'N/A', 1, CAST(N'2019-10-13T23:56:56.030' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.030' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (136, 126, N'N/A', 1, CAST(N'2019-10-13T23:56:56.030' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.030' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (137, 127, N'N/A', 1, CAST(N'2019-10-13T23:56:56.030' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.030' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (138, 128, N'N/A', 1, CAST(N'2019-10-13T23:56:56.033' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.033' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (139, 129, N'N/A', 1, CAST(N'2019-10-13T23:56:56.033' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.033' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (140, 130, N'N/A', 1, CAST(N'2019-10-13T23:56:56.033' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.033' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (141, 131, N'N/A', 1, CAST(N'2019-10-13T23:56:56.033' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.033' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (142, 132, N'N/A', 1, CAST(N'2019-10-13T23:56:56.037' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.037' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (143, 133, N'N/A', 1, CAST(N'2019-10-13T23:56:56.037' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.037' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (144, 134, N'N/A', 1, CAST(N'2019-10-13T23:56:56.037' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.037' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (145, 135, N'N/A', 1, CAST(N'2019-10-13T23:56:56.040' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.040' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (146, 136, N'N/A', 1, CAST(N'2019-10-13T23:56:56.040' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.040' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (147, 137, N'N/A', 1, CAST(N'2019-10-13T23:56:56.040' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.040' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (148, 138, N'N/A', 1, CAST(N'2019-10-13T23:56:56.040' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.040' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (149, 139, N'N/A', 1, CAST(N'2019-10-13T23:56:56.040' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.040' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (150, 140, N'N/A', 1, CAST(N'2019-10-13T23:56:56.040' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.040' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (151, 141, N'N/A', 1, CAST(N'2019-10-13T23:56:56.043' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.043' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (152, 142, N'N/A', 1, CAST(N'2019-10-13T23:56:56.047' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.047' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (153, 143, N'N/A', 1, CAST(N'2019-10-13T23:56:56.047' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.047' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (154, 144, N'N/A', 1, CAST(N'2019-10-13T23:56:56.050' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.050' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (155, 145, N'N/A', 1, CAST(N'2019-10-13T23:56:56.050' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.050' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (156, 146, N'N/A', 1, CAST(N'2019-10-13T23:56:56.053' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.053' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (157, 147, N'N/A', 1, CAST(N'2019-10-13T23:56:56.053' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.053' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (158, 148, N'N/A', 1, CAST(N'2019-10-13T23:56:56.053' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.053' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (159, 149, N'N/A', 1, CAST(N'2019-10-13T23:56:56.057' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.057' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (160, 150, N'N/A', 1, CAST(N'2019-10-13T23:56:56.060' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.060' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (161, 151, N'N/A', 1, CAST(N'2019-10-13T23:56:56.060' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.060' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (162, 152, N'N/A', 1, CAST(N'2019-10-13T23:56:56.060' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.060' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (163, 153, N'N/A', 1, CAST(N'2019-10-13T23:56:56.063' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.063' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (164, 154, N'N/A', 1, CAST(N'2019-10-13T23:56:56.067' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.067' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (165, 155, N'N/A', 1, CAST(N'2019-10-13T23:56:56.070' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.070' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (166, 156, N'N/A', 1, CAST(N'2019-10-13T23:56:56.070' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.070' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (167, 157, N'N/A', 1, CAST(N'2019-10-13T23:56:56.073' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.073' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (168, 158, N'N/A', 1, CAST(N'2019-10-13T23:56:56.073' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.073' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (169, 159, N'N/A', 1, CAST(N'2019-10-13T23:56:56.077' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.077' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (170, 160, N'N/A', 1, CAST(N'2019-10-13T23:56:56.077' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.077' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (171, 161, N'N/A', 1, CAST(N'2019-10-13T23:56:56.080' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.080' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (172, 162, N'N/A', 1, CAST(N'2019-10-13T23:56:56.080' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.080' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (173, 163, N'N/A', 1, CAST(N'2019-10-13T23:56:56.080' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.080' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (174, 164, N'N/A', 1, CAST(N'2019-10-13T23:56:56.080' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.080' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (175, 165, N'N/A', 1, CAST(N'2019-10-13T23:56:56.083' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.083' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (176, 166, N'N/A', 1, CAST(N'2019-10-13T23:56:56.083' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.083' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (177, 167, N'N/A', 1, CAST(N'2019-10-13T23:56:56.083' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.083' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (178, 168, N'N/A', 1, CAST(N'2019-10-13T23:56:56.087' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.087' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (179, 169, N'N/A', 1, CAST(N'2019-10-13T23:56:56.090' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.090' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (180, 170, N'N/A', 1, CAST(N'2019-10-13T23:56:56.093' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.093' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (181, 171, N'N/A', 1, CAST(N'2019-10-13T23:56:56.093' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.093' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (182, 172, N'N/A', 1, CAST(N'2019-10-13T23:56:56.097' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.097' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (183, 173, N'N/A', 1, CAST(N'2019-10-13T23:56:56.097' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.097' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (184, 174, N'N/A', 1, CAST(N'2019-10-13T23:56:56.100' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.100' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (185, 175, N'N/A', 1, CAST(N'2019-10-13T23:56:56.100' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.100' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (186, 176, N'N/A', 1, CAST(N'2019-10-13T23:56:56.100' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.100' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (187, 177, N'N/A', 1, CAST(N'2019-10-13T23:56:56.100' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.100' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (188, 178, N'N/A', 1, CAST(N'2019-10-13T23:56:56.103' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.103' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (189, 179, N'N/A', 1, CAST(N'2019-10-13T23:56:56.103' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.103' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (190, 180, N'N/A', 1, CAST(N'2019-10-13T23:56:56.103' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.103' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (191, 181, N'N/A', 1, CAST(N'2019-10-13T23:56:56.107' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.107' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (192, 182, N'N/A', 1, CAST(N'2019-10-13T23:56:56.110' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.110' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (193, 183, N'N/A', 1, CAST(N'2019-10-13T23:56:56.110' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.110' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (194, 184, N'N/A', 1, CAST(N'2019-10-13T23:56:56.110' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.110' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (195, 185, N'N/A', 1, CAST(N'2019-10-13T23:56:56.110' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.110' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (196, 186, N'N/A', 1, CAST(N'2019-10-13T23:56:56.113' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.113' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (197, 187, N'N/A', 1, CAST(N'2019-10-13T23:56:56.113' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.113' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (198, 188, N'N/A', 1, CAST(N'2019-10-13T23:56:56.113' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.113' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (199, 189, N'N/A', 1, CAST(N'2019-10-13T23:56:56.117' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.117' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (200, 190, N'N/A', 1, CAST(N'2019-10-13T23:56:56.120' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.120' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (201, 191, N'N/A', 1, CAST(N'2019-10-13T23:56:56.120' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.120' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (202, 192, N'N/A', 1, CAST(N'2019-10-13T23:56:56.120' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.120' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (203, 193, N'N/A', 1, CAST(N'2019-10-13T23:56:56.120' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.120' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (204, 194, N'N/A', 1, CAST(N'2019-10-13T23:56:56.123' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.123' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (205, 195, N'N/A', 1, CAST(N'2019-10-13T23:56:56.127' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.127' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (206, 196, N'N/A', 1, CAST(N'2019-10-13T23:56:56.127' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.127' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (207, 197, N'N/A', 1, CAST(N'2019-10-13T23:56:56.130' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.130' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (208, 198, N'N/A', 1, CAST(N'2019-10-13T23:56:56.130' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.130' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (209, 199, N'N/A', 1, CAST(N'2019-10-13T23:56:56.130' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.130' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (210, 200, N'N/A', 1, CAST(N'2019-10-13T23:56:56.130' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.130' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (211, 201, N'N/A', 1, CAST(N'2019-10-13T23:56:56.130' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.130' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (212, 202, N'N/A', 1, CAST(N'2019-10-13T23:56:56.133' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.133' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (213, 203, N'N/A', 1, CAST(N'2019-10-13T23:56:56.133' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.133' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (214, 204, N'N/A', 1, CAST(N'2019-10-13T23:56:56.133' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.133' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (215, 205, N'N/A', 1, CAST(N'2019-10-13T23:56:56.137' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.137' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (216, 206, N'N/A', 1, CAST(N'2019-10-13T23:56:56.137' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.137' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (217, 207, N'N/A', 1, CAST(N'2019-10-13T23:56:56.140' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.140' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (218, 208, N'N/A', 1, CAST(N'2019-10-13T23:56:56.140' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.140' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (219, 209, N'N/A', 1, CAST(N'2019-10-13T23:56:56.147' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.147' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (220, 210, N'N/A', 1, CAST(N'2019-10-13T23:56:56.147' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.147' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (221, 211, N'N/A', 1, CAST(N'2019-10-13T23:56:56.147' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.147' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (222, 212, N'N/A', 1, CAST(N'2019-10-13T23:56:56.150' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.150' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (223, 213, N'N/A', 1, CAST(N'2019-10-13T23:56:56.150' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.150' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (224, 214, N'N/A', 1, CAST(N'2019-10-13T23:56:56.150' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.150' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (225, 215, N'N/A', 1, CAST(N'2019-10-13T23:56:56.150' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.150' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (226, 216, N'N/A', 1, CAST(N'2019-10-13T23:56:56.153' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.153' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (227, 217, N'N/A', 1, CAST(N'2019-10-13T23:56:56.157' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.157' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (228, 218, N'N/A', 1, CAST(N'2019-10-13T23:56:56.160' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.160' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (229, 219, N'N/A', 1, CAST(N'2019-10-13T23:56:56.163' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.163' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (230, 220, N'N/A', 1, CAST(N'2019-10-13T23:56:56.167' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.167' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (231, 221, N'N/A', 1, CAST(N'2019-10-13T23:56:56.170' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.170' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (232, 222, N'N/A', 1, CAST(N'2019-10-13T23:56:56.173' AS DateTime), 1, CAST(N'2019-10-13T23:56:56.173' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (233, 223, N'N/A', 1, CAST(N'2019-10-14T00:00:58.423' AS DateTime), 1, CAST(N'2019-10-14T00:00:58.423' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (234, 224, N'N/A', 1, CAST(N'2019-10-14T00:00:58.427' AS DateTime), 1, CAST(N'2019-10-14T00:00:58.427' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (235, 225, N'N/A', 1, CAST(N'2019-10-14T00:00:58.430' AS DateTime), 1, CAST(N'2019-10-14T00:00:58.430' AS DateTime), 1, N'غير قابل للتطبيق')
GO
INSERT [StateMaster] ([Id], [CountryId], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (236, 226, N'N/A', 1, CAST(N'2019-10-14T00:00:58.430' AS DateTime), 1, CAST(N'2019-10-14T00:00:58.430' AS DateTime), 1, N'غير قابل للتطبيق')
GO
SET IDENTITY_INSERT [StateMaster] OFF
GO
SET IDENTITY_INSERT [LogActionMaster] ON 
GO
INSERT [LogActionMaster] ([Id], [Name]) VALUES (1, N'Create')
GO
INSERT [LogActionMaster] ([Id], [Name]) VALUES (2, N'Update')
GO
INSERT [LogActionMaster] ([Id], [Name]) VALUES (3, N'Delete')
GO
INSERT [LogActionMaster] ([Id], [Name]) VALUES (4, N'View')
GO
SET IDENTITY_INSERT [LogActionMaster] OFF
GO
SET IDENTITY_INSERT [LogModuleMaster] ON 
GO
INSERT [LogModuleMaster] ([Id], [Name]) VALUES (1, N'CategoryMaster')
GO
INSERT [LogModuleMaster] ([Id], [Name]) VALUES (2, N'CopyRightMaster')
GO
INSERT [LogModuleMaster] ([Id], [Name]) VALUES (3, N'CountryMaster')
GO
INSERT [LogModuleMaster] ([Id], [Name]) VALUES (4, N'CourseMaster')
GO
INSERT [LogModuleMaster] ([Id], [Name]) VALUES (5, N'DepartmentMaster')
GO
INSERT [LogModuleMaster] ([Id], [Name]) VALUES (6, N'DesignationMaster')
GO
INSERT [LogModuleMaster] ([Id], [Name]) VALUES (7, N'EducationMaster')
GO
INSERT [LogModuleMaster] ([Id], [Name]) VALUES (8, N'InstitutionMaster')
GO
INSERT [LogModuleMaster] ([Id], [Name]) VALUES (9, N'LanguageMaster')
GO
INSERT [LogModuleMaster] ([Id], [Name]) VALUES (10, N'MaterialTypeMaster')
GO
INSERT [LogModuleMaster] ([Id], [Name]) VALUES (11, N'ProfessionMaster')
GO
INSERT [LogModuleMaster] ([Id], [Name]) VALUES (12, N'QRCMaster')
GO
INSERT [LogModuleMaster] ([Id], [Name]) VALUES (13, N'ResourceMaster')
GO
INSERT [LogModuleMaster] ([Id], [Name]) VALUES (14, N'SocialMediaMaster')
GO
INSERT [LogModuleMaster] ([Id], [Name]) VALUES (15, N'StateMaster')
GO
INSERT [LogModuleMaster] ([Id], [Name]) VALUES (16, N'StreamMaster')
GO
INSERT [LogModuleMaster] ([Id], [Name]) VALUES (17, N'SubCategoryMaster')
GO
INSERT [LogModuleMaster] ([Id], [Name]) VALUES (18, N'UserMaster')
GO
SET IDENTITY_INSERT [LogModuleMaster] OFF
GO
SET IDENTITY_INSERT [MessageType] ON 
GO
INSERT [MessageType] ([Id], [Type]) VALUES (1, N'Course Approval')
GO
INSERT [MessageType] ([Id], [Type]) VALUES (2, N'Course Rejection')
GO
INSERT [MessageType] ([Id], [Type]) VALUES (3, N'Course Promotion')
GO
INSERT [MessageType] ([Id], [Type]) VALUES (4, N'Resource Approval')
GO
INSERT [MessageType] ([Id], [Type]) VALUES (5, N'Resource Rejection')
GO
INSERT [MessageType] ([Id], [Type]) VALUES (6, N'Resource Promotion')
GO
INSERT [MessageType] ([Id], [Type]) VALUES (7, N'Contributor Access Approved')
GO
INSERT [MessageType] ([Id], [Type]) VALUES (8, N'Contributor Access Rejected')
GO
INSERT [MessageType] ([Id], [Type]) VALUES (9, N'URL Whitelist Approval')
GO
INSERT [MessageType] ([Id], [Type]) VALUES (10, N'URL Whitelist Rejection')
GO
INSERT [MessageType] ([Id], [Type]) VALUES (11, N'QRC Review')
GO
SET IDENTITY_INSERT [MessageType] OFF
GO
INSERT [OerConfig] ([key], [value], [ConfigType]) VALUES (N'AWSEndPoint', N'http://oer-admin-bucket.s3.amazonaws.com/oer-admin-bucket/', N'AWS')
GO
INSERT [OerConfig] ([key], [value], [ConfigType]) VALUES (N'AWSAccessKey', N'AKIAXFRZTMWJNBH5N2VC', N'AWS')
GO
INSERT [OerConfig] ([key], [value], [ConfigType]) VALUES (N'AWSSecretKey', N'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', N'AWS')
GO
INSERT [OerConfig] ([key], [value], [ConfigType]) VALUES (N'AWSUser', N'us-east-2', N'AWS')
GO
INSERT [OerConfig] ([key], [value], [ConfigType]) VALUES (N'AWSBucketName', N'oer-admin-bucket', N'AWS')
GO
SET IDENTITY_INSERT [TitleMaster] ON 
GO
INSERT [TitleMaster] ([Id], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (1, N'Mr.', 1, CAST(N'2019-04-06T10:13:52.433' AS DateTime), 1, CAST(N'2019-04-06T10:13:52.433' AS DateTime), 1, N'السيد.')
GO
INSERT [TitleMaster] ([Id], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (2, N'Mrs.', 1, CAST(N'2019-04-06T10:18:47.737' AS DateTime), 1, CAST(N'2019-04-06T10:18:47.737' AS DateTime), 1, N'السيدة.')
GO
INSERT [TitleMaster] ([Id], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (3, N'Miss.', 1, CAST(N'2019-04-06T10:30:06.090' AS DateTime), 1, CAST(N'2019-04-06T10:30:06.090' AS DateTime), 1, N'الاّنسة')
GO
INSERT [TitleMaster] ([Id], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (4, N'Ms.', 1, CAST(N'2019-04-06T10:30:22.400' AS DateTime), 1, CAST(N'2019-04-06T10:30:22.400' AS DateTime), 0, N'الآنسة.')
GO
INSERT [TitleMaster] ([Id], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (5, N'Dr.', 1, CAST(N'2019-04-06T10:30:32.643' AS DateTime), 1, CAST(N'2019-04-06T10:30:32.643' AS DateTime), 1, N'الدكتور.')
GO
INSERT [TitleMaster] ([Id], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [IsActive], [Name_Ar]) VALUES (6, N'H.E', 1, CAST(N'2019-04-06T10:30:41.863' AS DateTime), 1, CAST(N'2019-04-06T10:30:41.863' AS DateTime), 1, N'سعادة.')
GO
SET IDENTITY_INSERT [TitleMaster] OFF
GO
SET IDENTITY_INSERT [dbo].[SocialMediaMaster] ON 
GO
INSERT [dbo].[SocialMediaMaster] ([Id], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [Name_Ar]) VALUES (2, N'Facebook', 1, CAST(N'2019-04-07T09:37:51.167' AS DateTime), 1, CAST(N'2019-04-07T09:37:51.167' AS DateTime), N'فيسبوك')
GO
INSERT [dbo].[SocialMediaMaster] ([Id], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [Name_Ar]) VALUES (3, N'Twitter', 1, CAST(N'2019-04-07T09:38:18.630' AS DateTime), 1, CAST(N'2019-04-07T09:38:18.630' AS DateTime), N'تويتر')
GO
INSERT [dbo].[SocialMediaMaster] ([Id], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [Name_Ar]) VALUES (4, N'LinkedIn', 1, CAST(N'2019-04-07T09:38:28.383' AS DateTime), 1, CAST(N'2019-04-07T09:38:28.383' AS DateTime), N'لينكدإن')
GO
INSERT [dbo].[SocialMediaMaster] ([Id], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [Name_Ar]) VALUES (5, N'Google Plus', 1, CAST(N'2019-04-07T09:38:48.760' AS DateTime), 1, CAST(N'2019-04-07T09:38:48.760' AS DateTime), N'جووجل بلس')
GO
INSERT [dbo].[SocialMediaMaster] ([Id], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [Name_Ar]) VALUES (6, N'Whats App', 1, CAST(N'2019-04-07T09:39:07.020' AS DateTime), 1, CAST(N'2019-04-07T09:39:07.020' AS DateTime), N'واتساب')
GO
SET IDENTITY_INSERT [dbo].[SocialMediaMaster] OFF
GO
