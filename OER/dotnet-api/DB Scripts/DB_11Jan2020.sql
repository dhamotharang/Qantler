/****** Object:  StoredProcedure [dbo].[sps_OerDashboardReport]    Script Date: 11-01-2020 17:33:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sps_OerDashboardReport]     
AS    
BEGIN    
    
SET NOCOUNT ON;    
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;    
    
DECLARE @Contributors INT,    
  @Courses INT,    
  @Resources INT,    
  @Return INT,  
  @TotalVisit INT;    
    
SET @Contributors =(SELECT count(*) from usermaster WHERE  IsContributor = 1)    
SET @Courses = (SELECT count(*)  from coursemaster WHERE IsApproved = 1 )    
SET @Resources =(SELECT count(*)  from resourcemaster WHERE IsApproved = 1)    
SET @TotalVisit= (SELECT  Count(ID) AS TotalVisit FROM Visiters)  
    
SELECT @Contributors as Contributors ,@Courses as Courses,@Resources as Resources ,@TotalVisit as  TotalVisit  
    
;WITH TopContributor_with(ID, UserName, CourseCount,Photo) AS    
(    
SELECT  u.id,u.FirstName+' '+u.LastName as UserName,(count(Distinct c.id)+count(Distinct r.id)),Photo FROM usermaster u    
LEFT JOIN coursemaster c on u.id = c.createdby  AND c.isApproved =1 
LEFT JOIN ResourceMaster r  on u.Id = r.CreatedBy AND r.IsApproved =1
 WHERE  u.IsContributor = 1    
    
 group by u.id,u.FirstName,u.LastName,Photo)    
    
 SELECT TOP 6 ID, UserName, CourseCount,Photo from  TopContributor_with where CourseCount>0  ORDER BY 3 DESC;    
    
    
     
WITH TopReviewer_with(ID, UserName, CourseCount,Photo) as(    
SELECT  u.id,u.FirstName+' '+u.LastName as UserName,count(c.id),Photo FROM usermaster u    
LEFT JOIN ContentApproval c    
on u.id = c.assignedto    
    
 GROUP BY u.id,u.FirstName,u.LastName,Photo)    
    
 SELECT TOP  6 ID, UserName, CourseCount,Photo from  TopReviewer_with where CourseCount>0  ORDER BY 3 DESC;    
    
 SET @Return = 105;    
    
 RETURN @Return;    
    
END;    
