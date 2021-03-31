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
IF EXISTS (SELECT TOP 1 1 FROM MoECheckMaster union SELECT TOP 1 1 FROM SensoryCheckMaster)
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
	union
	SELECT DISTINCT (SELECT FirstName + '''' + LastName FROM UserMaster WHERE Id = s.UserId) AS Verifier,
	(SELECT COUNT(*) FROM SensoryCheckMaster WHERE UserId = s.UserId AND Status = 1) AS ApprovedCount,
	(SELECT COUNT(*) FROM SensoryCheckMaster WHERE UserId = s.UserId AND Status = 0) AS RejectedCount
	from SensoryCheckMaster s
		)
	
	insert into @result
	select  Rownumber = ROW_NUMBER() OVER(ORDER BY '+@Orderby+'),Verifier,sum(approvedcount) as approveCount,
	sum(RejectedCount) as RejectedCount from sqlpaging1
	where ((verifier) like case when '+''''+@Keyword+''''+' ='''' then verifier else N'+''''+'%'+@Keyword+'%'+''''+' end) or 
	  (cast(approvedcount as nvarchar(255)) like '+''''+'%'+@Keyword+'%'+''''+') or 
	   (cast(rejectedcount as nvarchar(255)) like '+''''+'%'+@Keyword+'%'+''''+') group by Verifier
	

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