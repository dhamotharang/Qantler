
ALTER   PROCEDURE [dbo].[spi_MoEApproveReject]
(
  @UserId INT,
  @ContentId INT,
  @ContentType INT,
  @Status INT,
  @Comments NVARCHAR(MAX) = NULL  ,
  @EmailUrl NVARCHAR(MAX) 
)
AS
BEGIN
DECLARE @Date DATETIME = GETDATE()
DECLARE @notify_user INT = 0;
IF NOT EXISTS(SELECT TOP 1 1 FROM MoECheckMaster WHERE UserId = @UserId AND ContentId = @ContentId AND ContentType = @ContentType AND IsCurrent = 1)
	BEGIN
	IF @ContentType = 1
		BEGIN
			IF EXISTS(select top 1 1 from CourseMaster where ((IsApproved = 1 and MoEBadge = 1) OR (IsApproved = 0 and IsDraft = 1)) and Id = @ContentId)
			RETURN 207
		END
	ELSE
		BEGIN
			IF EXISTS(select top 1 1 from ResourceMaster where ((IsApproved = 1 and MoEBadge = 1) OR (IsApproved = 0 and IsDraft = 1)) and Id = @ContentId)
			RETURN 207
		END

   INSERT INTO MoECheckMaster(UserId,ContentId,ContentType,Status,Comments) VALUES (@UserId,@ContentId,@ContentType,@Status,@Comments)
  
	IF(@Status = 1)
	BEGIN
		IF @ContentType = 1
		BEGIN
			
			UPDATE CourseMaster SET IsApproved = 1, MoEBadge = 1 WHERE Id = @ContentId 
			select @notify_user=CreatedBy from CourseMaster where Id=@ContentId
			
			exec spi_Notifications @ContentId,1,'Course Approval','Course has been published during expert check',3,1,@Date,0,0,@notify_user,NULL,@Comments,@EmailUrl  
			SELECT Email,(FirstName + ' ' + LastName) AS UserName,PortalLanguageId,IsEmailNotification FROM UserMaster WHERE Id = (SELECT CreatedBy FROM CourseMaster
			WHERE Id = @ContentId)
		END
		ELSE
		BEGIN
			UPDATE ResourceMaster SET IsApproved = 1, MoEBadge = 1 WHERE Id = @ContentId
		    select @notify_user=CreatedBy from ResourceMaster where Id=@ContentId
			exec spi_Notifications @ContentId,2,'Resource Approval','Resource has been published during expert check',6,1,@Date,0,0,@notify_user,NULL,@Comments,@EmailUrl  
			SELECT Email,(FirstName + ' ' + LastName) AS UserName,PortalLanguageId,IsEmailNotification FROM UserMaster WHERE Id = (SELECT CreatedBy FROM ResourceMaster
			WHERE Id = @ContentId)
		END
		RETURN 203
	END
	ELSE
	BEGIN
		IF @ContentType = 1
		BEGIN
			UPDATE CourseMaster SET IsApproved = 0, IsDraft = 1 WHERE Id = @ContentId
			select @notify_user=CreatedBy from CourseMaster where Id=@ContentId
			exec spi_Notifications @ContentId,1,'Course Rejection','Course has been rejected during expert check',2,0,@Date,0,0,@notify_user,NULL,@Comments,@EmailUrl  
			SELECT Email,(FirstName + ' ' + LastName) AS UserName,PortalLanguageId,IsEmailNotification FROM UserMaster WHERE Id = (SELECT CreatedBy FROM CourseMaster
			WHERE Id = @ContentId)
		END
		ELSE
		BEGIN
			UPDATE ResourceMaster SET IsApproved = 0, IsDraft = 1 WHERE Id = @ContentId
			select @notify_user=CreatedBy from ResourceMaster where Id=@ContentId
			exec spi_Notifications @ContentId,2,'Resource Rejection','Resource has been rejected during expert check',5,0,@Date,0,0,@notify_user,NULL,@Comments,@EmailUrl  
			SELECT Email,(FirstName + ' ' + LastName) AS UserName,PortalLanguageId,IsEmailNotification FROM UserMaster WHERE Id = (SELECT CreatedBy FROM ResourceMaster
			WHERE Id = @ContentId)
		END
		UPDATE MoECheckMaster SET IsCurrent = 0 WHERE ContentId = @ContentId AND ContentType = @ContentType
		AND UserId =  @UserId
		RETURN 204
	END
   
   END
   ELSE
   RETURN 205
END

GO

ALTER PROCEDURE [dbo].[spi_SensoryApproveReject]    
(    
  @UserId INT,    
  @ContentId INT,    
  @ContentType INT,    
  @Status BIT,  
  @Comments NVARCHAR(MAX) = NULL        
)    
AS    
BEGIN  
DECLARE @Date DATETIME = GETDATE()
DECLARE @notify_user INT = 0;
IF (@ContentType = 1)
BEGIN
	SELECT (FirstName + ' ' + LastName) AS UserName, Email,PortalLanguageId,IsEmailNotification FROM UserMaster WHERE Id = (SELECT CreatedBy FROM CourseMaster WHERE 
	Id = @ContentId)
END
ELSE
BEGIN
	SELECT (FirstName + ' ' + LastName) AS UserName, Email,PortalLanguageId,IsEmailNotification FROM UserMaster WHERE Id = (SELECT CreatedBy FROM ResourceMaster WHERE 
	Id = @ContentId)
END  

IF NOT EXISTS(SELECT TOP 1 1 FROM SensoryCheckMaster WHERE UserId = @UserId AND ContentId = @ContentId AND ContentType = @ContentType AND IsCurrent = 1)
	BEGIN
	IF (@ContentType = 1)
		BEGIN
		IF EXISTS(select top 1 1 from CourseMaster where ((IsApproved = 1 and IsApprovedSensory = 1 and IsDraft = 0) OR (IsApproved = 0 and IsApprovedSensory = 0 and IsDraft = 1)) and Id = @ContentId)
			 RETURN 207
		END
	ELSE
		BEGIN
			IF EXISTS(select top 1 1 from ResourceMaster where ((IsApproved = 1 and IsApprovedSensory = 1 and IsDraft = 0) OR (IsApproved = 0 and IsApprovedSensory = 0 and IsDraft = 1)) and Id = @ContentId)
			 RETURN 207
		END  

   INSERT INTO SensoryCheckMaster(UserId,ContentId,ContentType,Status,Comments) VALUES (@UserId,@ContentId,@ContentType,@Status,@Comments) 
      
   IF(@Status = 0)    
    BEGIN    
			IF @ContentType = 1 
			BEGIN 
					UPDATE CourseMaster SET IsApproved = 0,IsApprovedSensory = 0, IsDraft = 1 WHERE Id = @ContentId
					UPDATE SensoryCheckMaster SET IsCurrent = 0 WHERE ContentId = @ContentId AND ContentType = @ContentType 
					select @notify_user=CreatedBy from CourseMaster where Id=@ContentId
					exec spi_Notifications @ContentId,1,'Course Rejection','Course has been rejected during sensitivity check',2,0,@Date,0,0,@notify_user,NULL,@Comments,NULL
					RETURN 204; 
			END
			ELSE  
			BEGIN
					UPDATE ResourceMaster SET IsApproved = 0 ,IsApprovedSensory = 0, IsDraft = 1 WHERE Id = @ContentId 
					UPDATE SensoryCheckMaster SET IsCurrent = 0 WHERE ContentId = @ContentId AND ContentType = @ContentType
					select @notify_user=CreatedBy from ResourceMaster where Id=@ContentId
					exec spi_Notifications @ContentId,2,'Resource Rejection','Resource has been rejected during sensitivity check',5,0,@Date,0,0,@notify_user,NULL,@Comments,NULL
					RETURN 204; 
			END
			--AND UserId =  @UserId
			END 
			ELSE
			BEGIN
				IF @ContentType = 1 
				BEGIN 
					UPDATE CourseMaster SET IsApproved = 1,IsApprovedSensory = 1, IsDraft = 0 WHERE Id = @ContentId
					UPDATE SensoryCheckMaster SET IsCurrent = 0 WHERE ContentId = @ContentId AND ContentType = @ContentType 
					select @notify_user=CreatedBy from CourseMaster where Id=@ContentId
					exec spi_Notifications @ContentId,1,'Course Approval','Course has been approved during sensitivity check',3,1,@Date,0,0,@notify_user,NULL,@Comments,NULL
					RETURN 115; 
			END
			ELSE  
			BEGIN
					UPDATE ResourceMaster SET IsApproved = 1 ,IsApprovedSensory = 1, IsDraft = 0 WHERE Id = @ContentId
					UPDATE SensoryCheckMaster SET IsCurrent = 0 WHERE ContentId = @ContentId AND ContentType = @ContentType 
					select @notify_user=CreatedBy from ResourceMaster where Id=@ContentId
					exec spi_Notifications @ContentId,2,'Resource Approval','Resource has been approved during sensitivity check',6,1,@Date,0,0,@notify_user,NULL,@Comments,NULL
					RETURN 115; 
			END
			END   
   END
END
RETURN 205 


