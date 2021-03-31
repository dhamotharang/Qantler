GO
CREATE TABLE WebContentCategoryMaster(
   ID   INT  NOT NULL identity(1,1),
   CategoryNAME nvarchar(50),      
   PRIMARY KEY (ID)
);
GO
INSERT INTO [dbo].[WebContentCategoryMaster]([CategoryNAME])
	VALUES('Fotter Section');
INSERT INTO [dbo].[WebContentCategoryMaster]([CategoryNAME])
	VALUES('Home Section');
Go	
ALTER TABLE [WebContentPages]
ADD CategoryID INT
FOREIGN KEY(CategoryID) REFERENCES [WebContentCategoryMaster]([ID]);
Go
UPDATE [dbo].[WebContentPages]
   SET [CategoryID] = 1
GO  
INSERT INTO [dbo].[WebContentPages]([PageName],[PageName_Ar],[CategoryID])
    VALUES('Banner Section','????',2)
INSERT INTO [dbo].[WebContentPages]([PageName],[PageName_Ar],[CategoryID])
    VALUES('Course Carousel','????',2)
INSERT INTO [dbo].[WebContentPages]([PageName],[PageName_Ar],[CategoryID])
    VALUES('Video Section','????',2)
INSERT INTO [dbo].[WebContentPages]([PageName],[PageName_Ar],[CategoryID])
    VALUES('Glance Description','????',2)
INSERT INTO [dbo].[WebContentPages]([PageName],[PageName_Ar],[CategoryID])
    VALUES('Features Description','????',2)
INSERT INTO [dbo].[WebContentPages]([PageName],[PageName_Ar],[CategoryID])
    VALUES('Resource Carousel','????',2)
GO

/****** Object:  StoredProcedure [dbo].[sps_Pages]    Script Date: 24/1/2020 5:48:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================    
-- Author:  <Author,,Name>    
-- Create date: <Create Date,,>    
-- Description: <Description,,>    
-- =============================================    
ALTER PROCEDURE [dbo].[sps_Pages]   --  [sps_Pages] 0 
     @category_id int
AS    
BEGIN    
SELECT Id,    
PageName, PageName_Ar from WebContentPages where [CategoryID] = case when @category_id = 0 then CategoryID else @category_id end
    
RETURN 105    
END 
GO

Create PROCEDURE [dbo].[sps_PageContents]      
AS    
BEGIN    
select WP.Id, WP.PageID, WP.PageContent,WP.PageContent_Ar,WP.CreatedBy,WP.CreatedOn,WP.UpdatedBy,WP.UpdatedOn,WC.PageName
from WebPageContent as WP inner join WebContentPages as WC on Wp.PageID=WC.Id 
    
 RETURN 105    
END 
GO
SET IDENTITY_INSERT [dbo].[WebPageContent] ON
INSERT [dbo].[WebPageContent] ([Id], [PageID], [PageContent], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [PageContent_Ar]) VALUES (9, 12, N'<!DOCTYPE html>
<html>
<head>
</head>
<body>
<p>&nbsp;The resource builder makes it easy to combine text, pictures, sounds, files, and video, and save them as&nbsp; openly licensed educational resources &mdash; which can then be shared with friends, colleagues and educators from around the world.You can print and download your resources as a PDF, as well as download all included media&nbsp;</p>
</body>
</html>', 9, CAST(N'2020-01-25T13:31:08.530' AS DateTime), 9, CAST(N'2020-01-25T20:14:50.983' AS DateTime), N'<!DOCTYPE html>
<html>
<head>
</head>
<body>
<p style="text-align: right;">&nbsp;تجعل أداة انشاء المصادر التعليمية المفتوحة من عملية حفظ وجمع النصوص والصور والأصوات والملفات ومقاطع الفيديو أمراً بغاية السهولة، لتتوافر هذه المصادر للجميع بشكل مرخص ليتم مشاركتها مع الأهل و الأصدقاء والزملاء والمعلمين من جميع أنحاء العالم، كما ويمكن طباعة جميع هذه المصادر وحفظها كملفات "بي دي إف" وتنزيل الوسائط المتضمنة في هذه المصادر التعليمية.&nbsp;</p>
</body>
</html>')
INSERT [dbo].[WebPageContent] ([Id], [PageID], [PageContent], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [PageContent_Ar]) VALUES (10, 7, N'<!DOCTYPE html>
<html>
<head>
</head>
<body>
<h2 class="banner-tagline" style="box-sizing: border-box; margin-top: 0px; margin-bottom: 0.5rem; font-weight: 400; line-height: 1.2; font-size: 20px; color: #3f3d56; font-family: CronosPro; background-color: #ffffff;"><strong>Manara is a public digital library of open educational resources. Explore, create, and collaborate with people around the world to improve knowledge gathering.</strong></h2>
</body>
</html>', 9, CAST(N'2020-01-25T14:20:23.907' AS DateTime), NULL, NULL, N'<!DOCTYPE html>
<html>
<head>
</head>
<body>
<h2 class="banner-tagline" style="box-sizing: border-box; margin-top: 0px; margin-bottom: 0.5rem; font-weight: 400; line-height: 1.2; font-size: 17px; color: #3f3d56; font-family: DroidArabicKufi; text-align: right; background-color: #ffffff;"><strong>منارة: هي منصة رقمية لمصادر التعليم المفتوحة. استكشف، أنشئ، وتعاون معنا لنشر المعرفة حول العالم</strong></h2>
</body>
</html>')
INSERT [dbo].[WebPageContent] ([Id], [PageID], [PageContent], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [PageContent_Ar]) VALUES (11, 9, N'<!DOCTYPE html>
<html>
<head>
</head>
<body>
<p><strong><span style="color: #121212; font-family: CronosPro; font-size: 20px; background-color: #ffffff;">Watch this (video) and know how to make use of the important capabilities and advantages for the sources of open educational (Manara) platform.</span></strong></p>
</body>
</html>', 9, CAST(N'2020-01-25T14:21:19.817' AS DateTime), NULL, NULL, N'<!DOCTYPE html>
<html>
<head>
</head>
<body>
<p style="text-align: right;"><strong><span style="color: #121212; font-family: DroidArabicKufi; font-size: 17px; text-align: right; background-color: #ffffff;">شاهد هذا (الفيديو) وتعرّف إلى كيفية الاستفادة من الإمكانات والمزايا الهامّة لمصادر منصة (منارة) التربوية المفتوحة.</span></strong></p>
</body>
</html>')
INSERT [dbo].[WebPageContent] ([Id], [PageID], [PageContent], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [PageContent_Ar]) VALUES (12, 10, N'<!DOCTYPE html>
<html>
<head>
</head>
<body>
<p>Manara platform provides opportunities for the systematic change in the content of learning and teaching by involving teachers in a new educational experiments and effective techniques for learning.</p>
</body>
</html>', 9, CAST(N'2020-01-25T14:23:05.687' AS DateTime), 9, CAST(N'2020-01-25T20:10:49.860' AS DateTime), N'<!DOCTYPE html>
<html>
<head>
</head>
<body>
<p style="text-align: right;">توفّر منصة (منارة) فرصاً للتغيير المنهجي في محتوى التّعلّم والتعليم من خلال إشراك المعلمين في تجارب تعليمية جديدة، وتقنيات فعّالة للتعلم.</p>
</body>
</html>')
INSERT [dbo].[WebPageContent] ([Id], [PageID], [PageContent], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [PageContent_Ar]) VALUES (13, 11, N'<!DOCTYPE html>
<html>
<head>
</head>
<body>
<p><span style="color: #121212; font-family: CronosPro; font-size: 20px; text-align: center; background-color: #ffffff;">Manara provides an attractive setting for the work of both students and faculty and is an important information centre for learners and scholars from all disciplines.</span></p>
</body>
</html>', 9, CAST(N'2020-01-25T14:24:58.690' AS DateTime), NULL, NULL, N'<!DOCTYPE html>
<html>
<head>
</head>
<body>
<p style="text-align: right;"><span style="color: #121212; font-family: DroidArabicKufi; font-size: 17px; text-align: center; background-color: #ffffff;">توفر منصة منارة بيئة جذابة لعمل كل من الطلاب وأعضاء هيئة التدريس، وهي مركز معلومات مهم للمتعلمين والعلماء من جميع التخصصات.</span></p>
</body>
</html>')
INSERT [dbo].[WebPageContent] ([Id], [PageID], [PageContent], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [PageContent_Ar]) VALUES (14, 8, N'<!DOCTYPE html>
<html>
<head>
</head>
<body>
<p>&nbsp;The tool of course creation allows for authors and writers to create easy content presentation methods for both students and teachers, and they are encouraged to include their feedback, and to provide students, teachers and users of platform &ldquo;Manara&rdquo; with the educational materials that support the available pathways.&nbsp;</p>
</body>
</html>', 9, CAST(N'2020-01-25T14:28:06.707' AS DateTime), 9, CAST(N'2020-01-25T20:12:43.090' AS DateTime), N'<!DOCTYPE html>
<html>
<head>
</head>
<body>
<p style="text-align: right;">&nbsp;تسمح أداة إنشاء المساقات للمؤلّفين والكتّاب بإنشاء طرائق عرض سهلة للمحتوى لكلّ من الطلاب والمعلمين، كما يتم تشجيعهم لتضمين ملحوظاتهم، وتزويد الطّلاب والأساتذة ومستخدمي منصة (منارة) بالمادة التربوية الداعمة للمساقات المتاdحة.&nbsp;</p>
</body>
</html>')
SET IDENTITY_INSERT [dbo].[WebPageContent] OFF


