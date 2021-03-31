ALTER PROCEDURE [dbo].[GetResource] 
    (  
  @PageNo int = 1,  
  @PageSize int = 111 ,
  @SortField int=0,
  @SortType nvarchar(max)='asc',
  @Keyword nvarchar(max)=N''
 )  
AS      
BEGIN      
      
   Declare @return INT  
declare @start int, @end int  
set @start = (@PageNo - 1) * @PageSize + 1  
set @end = @PageNo * @PageSize  
IF EXISTS(SELECT TOP 1 *  FROM ResourceMaster)      
 begin
 
 declare @Orderby nvarchar(max) = '';
  declare @Sort nvarchar(max) = '';

 --Sort
 select @Sort = @SortType;
 select @Orderby = case when @SortField = 1 then 'r.Title' when @SortField=2 then 'ResourceDescription' when @SortField=3 then 'c.Name' when @SortField=4 then concat('um.FirstName ',@Sort,' , ','um.LastName ',@Sort) when @SortField=5 then 
 'case when isApproved = 1 then'+''''+'Approved'+''''+' when isDraft=1 then '+''''+'Draft'+''''+' else '+''''+'Submitted for Approval'+''''+' end' else 'r.Id' end
 if(@SortField !=4)
 select @Orderby = concat(@Orderby,' '+ @Sort)
 

 declare @query nvarchar(max) ='';
set @query = ';with sqlpaging as (
 SELECT Rownumber = ROW_NUMBER() OVER(ORDER BY '+@Orderby+'), r.Id    
      ,r.Title    
      ,r.CategoryId, c.Name as CategoryName    
      ,SubCategoryId, sc.Name as SubCategoryName    
      ,Thumbnail    
      ,ResourceDescription    
      ,Keywords    
      ,ResourceContent    
      ,MaterialTypeId, mt.Name as MaterialTypeName    
      ,CopyRightId, cm.Title as CopyrightTitle, cm.Description as CopyrightDescription    
      ,IsDraft    
      ,CONCAT(um.FirstName, '''', um.LastName) as CreatedBy, um.Id as CreatedById    
      ,r.CreatedOn    
      ,IsApproved    
      ,Rating    
      ,AlignmentRating    
      ,ReportAbuseCount,ViewCount,AverageReadingTime,(SELECT COUNT(*) FROM ContentDownloadInfo WHERE ContentId = r.Id AND ContentTypeId = 2) AS DownloadCount,
   (SELECT COUNT(*) FROM ContentSharedInfo WHERE ContentId = r.Id AND ContentTypeId = 2) As SharedCount,   
    r.ReadingTime ,  
 es.Standard,  
 eu.EducationalUse,  
 el.Level,  
 Objective,  
 [Format] ,
 CommunityBadge,
 MoEBadge
  FROM ResourceMaster r    
   inner join CategoryMaster c on r.CategoryId = c.Id    
   inner join MaterialTypeMaster mt on r.MaterialTypeId=mt.Id    
   left join SubCategoryMaster sc on r.SubCategoryId=sc.Id    
   left join CopyrightMaster cm on r.CopyRightId = cm.Id    
   LEFT JOIN lu_Educational_Standard es ON r.EducationalStandardId = es.Id  
   LEFT JOIN lu_Educational_Use eu ON r.EducationalUseId = eu.Id  
   LEFT JOIN lu_Level el ON r.LevelId = el.Id  
  
   inner join UserMaster um on r.CreatedBy =um.Id
      where ((r.Title) like case when '+''''+@Keyword+''''+' ='''' then r.Title else N'+''''+'%'+@Keyword+'%'+''''+' end) or 
	  ((r.ResourceDescription) like case when '+''''+@Keyword+''''+' ='''' then r.Title else N'+''''+'%'+@Keyword+'%'+''''+' end) or 
	   ((c.Name) like case when '+''''+@Keyword+''''+' ='''' then c.Name else N'+''''+'%'+@Keyword+'%'+''''+' end) or 
		 ((CONCAT(um.FirstName, '''', um.LastName)) like case when '+''''+@Keyword+''''+' ='''' then CONCAT(um.FirstName, '''', um.LastName) else N'+''''+'%'+@Keyword+'%'+''''+' end)or
		  ((case when isApproved = 1 then'+''''+'Approved'+''''+' when isDraft=1 then '+''''+'Draft'+''''+' else '+''''+'Submitted for Approval'+''''+' end) like case when '+''''+@Keyword+''''+' ='''' then c.Name else N'+''''+'%'+@Keyword+'%'+''''+' end) )

    
   select
 top ('+cast(@PageSize as nvarchar(10))+') *,
 (select max(rownumber) from sqlpaging) as
 Totalrows
from sqlpaging
where Rownumber between '+cast(@start as nvarchar(10))+' and '+cast(@end as nvarchar(10))
  
--select @query

EXECUTE sp_executesql @query
      
SELECT raf.Id      
      ,ResourceId      
      ,AssociatedFile      
      ,UploadedDate      
  FROM ResourceAssociatedFiles raf INNER JOIN ResourceMaster r ON r.Id=raf.ResourceId       
        
      
   SELECT rur.Id      
      ,ResourceId      
      ,URLReferenceId, uwl.URL as URLReference      
      ,rur.CreatedOn      
  FROM dbo.ResourceURLReferences rur       
  INNER JOIN ResourceMaster r ON r.Id=rur.ResourceId      
  inner join WhiteListingURLs uwl on uwl.Id=rur.URLReferenceId        
      
  SELECT rc.Id      
      ,[ResourceId]      
      ,[Comments]      
      ,CONCAT(um.FirstName, ' ', um.LastName) as CommentedBy, um.Id As CommentedById, um.Photo as CommentorImage      
      ,[CommentDate]      
      ,[ReportAbuseCount]      
  FROM [dbo].[ResourceComments] rc inner join UserMaster um on rc.UserId=um.Id where rc.IsHidden=0      
            
 return 105 -- record exists      
       
END      
      
else return 102 -- recond does not exists      
end  
Go

ALTER PROCEDURE [dbo].[GetCourses] 
(
    @PageNo int = 1,
	@PageSize int = 5,
	@Keyword nvarchar(max)=N'',
    @SortType nvarchar(max)='asc',
	@SortField int=0
)
AS    
BEGIN    
 
	 Declare @return INT
declare @start int, @end int
set @start = (@PageNo - 1) * @PageSize + 1
set	@end = @PageNo * @PageSize

declare @Orderby nvarchar(max) = '';
  declare @Sort nvarchar(max) = '';

 --Sort
 select @Sort = @SortType;
 select @Orderby = case when @SortField = 1 then 'r.Title' when @SortField=2 then 'CourseDescription' when @SortField=3 then 'c.Name' when @SortField=4 then concat('um.FirstName ',@Sort,' , ','um.LastName ',@Sort) when @SortField=5 then 
 'case when isApproved = 1 then'+''''+'Approved'+''''+' when isDraft=1 then '+''''+'Draft'+''''+' else '+''''+'Submitted for Approval'+''''+' end' else 'r.Id' end
 if(@SortField !=4)
 select @Orderby = concat(@Orderby,' '+ @Sort)

 declare @query nvarchar(max) ='';
set @query = ';with sqlpaging as (
	SELECT Rownumber = ROW_NUMBER() OVER(ORDER BY '+@Orderby+'), r.Id  
      ,r.Title    
      ,r.CategoryId, c.Name as CategoryName    
      ,SubCategoryId, sc.Name as SubCategoryName    
      ,Thumbnail    
      ,CourseDescription    
      ,Keywords    
      ,CourseContent         
      ,CopyRightId, cm.Title as CopyrightTitle, cm.Description as CopyrightDescription    
      ,IsDraft    
   , EducationId, edm.Name as EducationName    
   , ProfessionId, pm.Name as ProfessionName    
      , CONCAT(um.FirstName,'''', um.LastName) as CreatedBy, um.Id as CreatedById    
      ,r.CreatedOn    
      ,IsApproved    
      ,Rating         
      ,ReportAbuseCount,ViewCount,AverageReadingTime,(SELECT COUNT(*) FROM ContentSharedInfo WHERE ContentId = r.Id AND ContentTypeId = 1) As SharedCount,    
   r.ReadingTime  ,
   (SELECT COUNT(*) FROM courseenrollment WHERE CourseId = r.Id) AS EnrollmentCount,
   (SELECT COUNT(*) FROM ContentDownloadInfo WHERE ContentId = r.Id AND ContentTypeId = 1) AS DownloadCount,
   CommunityBadge,
   MoEBadge
  FROM CourseMaster r    
   inner join CategoryMaster c on r.CategoryId = c.Id       
   left join SubCategoryMaster sc on r.SubCategoryId=sc.Id    
   left join CopyrightMaster cm on r.CopyRightId = cm.Id    
   left join EducationMaster edm on edm.Id= r.EducationId    
   left join ProfessionMaster pm on pm.Id= r.ProfessionId    
   inner join UserMaster um on r.CreatedBy =um.Id 

   where ((r.Title) like case when '+''''+@Keyword+''''+' ='''' then r.Title else N'+''''+'%'+@Keyword+'%'+''''+' end) or 
	  ((r.CourseDescription) like case when '+''''+@Keyword+''''+' ='''' then r.CourseDescription else N'+''''+'%'+@Keyword+'%'+''''+' end) or 
	   ((c.Name) like case when '+''''+@Keyword+''''+' ='''' then c.Name else N'+''''+'%'+@Keyword+'%'+''''+' end) or 
		 ((CONCAT(um.FirstName, '''', um.LastName)) like case when '+''''+@Keyword+''''+' ='''' then CONCAT(um.FirstName, '''', um.LastName) else N'+''''+'%'+@Keyword+'%'+''''+' end) or
		  ((case when isApproved = 1 then'+''''+'Approved'+''''+' when isDraft=1 then '+''''+'Draft'+''''+' else '+''''+'Submitted for Approval'+''''+' end) like case when '+''''+@Keyword+''''+' ='''' then c.Name else N'+''''+'%'+@Keyword+'%'+''''+' end) )

			select
 top ('+cast(@PageSize as nvarchar(10))+') *,
 (select max(rownumber) from sqlpaging) as
 Totalrows
 from sqlpaging
where Rownumber between '+cast(@start as nvarchar(10))+' and '+cast(@end as nvarchar(10))     
    
 EXECUTE sp_executesql @query

-- select @query
    
SELECT caf.Id    
      ,CourseId    
      ,AssociatedFile    
      ,caf.CreatedOn    
  FROM CourseAssociatedFiles caf INNER JOIN CourseMaster cm ON cm.Id=caf.CourseId     
    
  SELECT cur.Id    
      ,CourseId    
      ,cur.URLReferenceId,uwl.URL as URLReference    
      ,cur.CreatedOn    
  FROM dbo.CourseURLReferences cur INNER JOIN CourseMaster cm ON cm.Id=cur.CourseId    
           inner join WhiteListingURLs uwl on uwl.Id=cur.URLReferenceId    
    
    
 SELECT cc.Id    
      ,[CourseId]    
      ,[Comments]    
      ,CONCAT(um.FirstName, ' ', um.LastName) as CommentedBy, um.Id As CommentedById, um.Photo as CommentorImage    
      ,[CommentDate]    
      ,[ReportAbuseCount]    
  FROM [dbo].[CourseComments] cc inner join UserMaster um on cc.UserId=um.Id where cc.IsHidden=0    
       
 --IF @@ROWCOUNT>0    
        
 return 105 -- record exists    
    
-- ELSE    
--  return 102 -- reconrd does not exists    
END 
Go

ALTER PROCEDURE [dbo].[sps_ContactUs] 
(
 @PageNo int = 1,    
 @PageSize int = 5 ,
 @Keyword nvarchar(max)=N'',
 @SortType nvarchar(max)='asc',
 @SortField int=0
 )
AS    
BEGIN    
 -- SET NOCOUNT ON added to prevent extra result sets from    
 -- interfering with SELECT statements.    
 SET NOCOUNT ON;    
Declare @return INT    
declare @start int, @end int    
set @start = (@PageNo - 1) * @PageSize + 1    
set @end = @PageNo * @PageSize    
    -- Insert statements for procedure here    
  declare @Orderby nvarchar(max) = '';
  declare @Sort nvarchar(max) = '';

  
 --Sort
 select @Sort = @SortType;
 select @Orderby = case when @SortField = 1 then concat('c.FirstName ',@Sort,' , ','c.LastName ',@Sort) when @SortField=2 then 'c.Email' when @SortField=3 then 'c.Telephone' when @SortField=4 then 'c.Subject' when @SortField=5 then 
 'c.Message' else 'c.Id' end
 if(@SortField !=1)
 select @Orderby = concat(@Orderby,' '+ @Sort)

 declare @query nvarchar(max) ='';
    
 set @query = ';with sqlpaging as (    
 SELECT Rownumber = ROW_NUMBER() OVER(ORDER BY '+@Orderby+'),    
   c.Id,    
   c.FirstName,    
   c.LastName,    
   c.Email,    
   c.Telephone,    
   c.Subject,    
   c.Message,    
   c.RepliedText,
   (SELECT FirstName + '''' +LastName  FROM UserMaster WHERE Id = c.RepliedBy) AS RepliedBy,
   c.RepliedBy AS RepliedById,
   c.RepliedOn,
   c.CreatedOn,  
   CASE  
    WHEN c.IsReplied is NULL  THEN 0  
    ELSE 1  
 END AS IsReplied  
   from ContactUs c  
        where ((c.Email) like case when '+''''+@Keyword+''''+' ='''' then c.Email else N'+''''+'%'+@Keyword+'%'+''''+' end) or 
	  ((c.Telephone) like case when '+''''+@Keyword+''''+' ='''' then c.Telephone else N'+''''+'%'+@Keyword+'%'+''''+' end) or 
	   ((c.Subject) like case when '+''''+@Keyword+''''+' ='''' then c.Subject else N'+''''+'%'+@Keyword+'%'+''''+' end) or 
		 ((CONCAT(c.FirstName, '''', c.LastName)) like case when '+''''+@Keyword+''''+' ='''' then CONCAT(c.FirstName, '''', c.LastName) else N'+''''+'%'+@Keyword+'%'+''''+' end) or
		 ((c.Message) like case when '+''''+@Keyword+''''+' ='''' then c.Message else N'+''''+'%'+@Keyword+'%'+''''+' end) 
       
   )    
   select    
 top ('+cast(@PageSize as nvarchar(10))+') *,    
 (select max(rownumber) from sqlpaging) as     
 Totalrows    
from sqlpaging    
where Rownumber between '+ cast(@start as nvarchar(10))+' and '+cast(@end as nvarchar(10))
    
 EXECUTE sp_executesql @query



 SET @return = 105 -- reconrd does not exists    
   RETURN @return    
END 
Go

ALTER PROCEDURE [dbo].[sps_GetVerifierReport]  
(
 @PageNo int = 1,      
 @PageSize int = 5,
 @Keyword nvarchar(max)=N'',
 @SortType nvarchar(max)='asc',
 @SortField int=0
)
AS BEGIN
Declare @return INT, @query NVARCHAR(MAX)      
declare @start int, @end int      
set @start = (@PageNo - 1) * @PageSize + 1      
set @end = @PageNo * @PageSize 

 declare @Orderby nvarchar(max) = '';
  declare @Sort nvarchar(max) = '';

  
 --Sort
 select @Sort = @SortType;
 select @Orderby = case when @SortField = 1 then 'Verifier' when @SortField=2 then 'ApprovedCount' when @SortField=3 then 'RejectedCount'  else 'Verifier' end
select @Orderby = concat(@Orderby,' '+ @Sort)

CREATE table #tempData      
(      
Verifier NVARCHAR(500),       
ApprovedCount INT,      
RejectedCount INT    
)      
IF EXISTS (SELECT TOP 1 1 FROM MoECheckMaster)
BEGIN

	

	set @query = '
	declare @result table(
	Rownumber int,
	verifier nvarchar(max),
	approvedcount int,
	rejectedcount int
	)
	
	;with sqlpaging1 as ( 
	SELECT DISTINCT (SELECT FirstName + '''' + LastName FROM UserMaster WHERE Id = r.UserId) AS Verifier,
	(SELECT COUNT(*) FROM MoECheckMaster WHERE UserId = r.UserId AND Status = 1) AS ApprovedCount,
	(SELECT COUNT(*) FROM MoECheckMaster WHERE UserId = r.UserId AND Status = 0) AS RejectedCount
	FROM MoECheckMaster r 
		)
	
	insert into @result
	select  Rownumber = ROW_NUMBER() OVER(ORDER BY '+@Orderby+'),* from sqlpaging1  
	where ((verifier) like case when '+''''+@Keyword+''''+' ='''' then verifier else N'+''''+'%'+@Keyword+'%'+''''+' end) or 
	  (cast(approvedcount as nvarchar(255)) like '+''''+'%'+@Keyword+'%'+''''+') or 
	   (cast(rejectedcount as nvarchar(255)) like '+''''+'%'+@Keyword+'%'+''''+') 
	

	select        
	 top ('+CAST(@PageSize AS VARCHAR(50))+') *,        
	 (select max(Rownumber) from @result) as         
	 Totalrows    from @result        
	where Rownumber between '+CAST(@start AS VARCHAR(50))+' and '+CAST(@end AS VARCHAR(50))+''      
      
	  --select @query;
	EXEC sp_executesql @query      
	  RETURN 105;  
	  drop table #tempData 
END
ELSE
BEGIN
	RETURN 113
END
END
Go

ALTER PROCEDURE [dbo].[sps_AdminRejectedList]
(  
  @PageNo int = 1,        
  @PageSize int = 5,
  @Keyword nvarchar(max)=N'',
  @SortType nvarchar(max)='asc',
  @SortField int=0
)  
AS  
BEGIN  
 DECLARE @return INT, @query NVARCHAR(MAX)   
 DECLARE @start INT, @end INT        
 SET @start = (@PageNo - 1) * @PageSize + 1        
 SET @end = @PageNo * @PageSize  


  declare @Orderby nvarchar(max) = '';
  declare @Sort nvarchar(max) = '';

  
 --Sort
 select @Sort = @SortType;
 select @Orderby = case when @SortField = 1 then 'Title' when @SortField=2 then 'ContentType'  else 'Title' end
 select @Orderby = concat(@Orderby,' '+ @Sort)


 CREATE table #tempData        
 (        
 ContentId INT,         
 ContentType INT,  
 Title NVARCHAR(MAX),  
 Category NVARCHAR(50) 
 )      
 INSERT INTO #tempData  
 SELECT cm.ID,1,cm.Title,cat.Name from CourseMaster cm inner join CategoryMaster cat on cm.categoryid = cat.id 
 WHERE cm.IsApproved = 0 and 
  ((cm.Title like case when @Keyword = '' then cm.Title else  '%'+@Keyword+'%' end) or  
 ('Course' like case when @Keyword = '' then 'Course' else  '%'+@Keyword+'%' end))

  INSERT INTO #tempData  
 SELECT cm.ID,2,cm.Title,cat.Name from ResourceMaster cm inner join CategoryMaster cat on cm.categoryid = cat.id
  WHERE cm.IsApproved = 0 and 
  ((cm.Title like case when @Keyword = '' then cm.Title else '%'+@Keyword+'%' end) or  
  ('Resource' like case when @Keyword = '' then 'Resource' else '%'+@Keyword+'%' end))


IF EXISTS(SELECT TOP 1 1 from CourseMaster cm inner join CategoryMaster cat on cm.categoryid = cat.id) or  EXISTS(SELECT TOP 1 1 from ResourceMaster cm inner join CategoryMaster cat on cm.categoryid = cat.id)
BEGIN  
SET @query =        
 ';with sqlpaging as (        
 SELECT         
 Rownumber = ROW_NUMBER() OVER(order by  '+@Orderby+') ,        
 * FROM #tempData)        
        
 select          
  top ('+CAST(@PageSize AS VARCHAR(50))+') *,          
  (select max(rownumber) from sqlpaging) as           
  Totalrows    from sqlpaging          
 where Rownumber between '+CAST(@start AS VARCHAR(50))+' and '+CAST(@end AS VARCHAR(50))+''        
      --  select @query
 EXEC sp_executesql @query        
   RETURN 105;    
   drop table #tempData   
 END  
 ELSE  
 BEGIN  
  RETURN 113  
 END  
END 