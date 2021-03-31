ALTER PROCEDURE [dbo].[spu_Announcements]
	(
	@Id INT,
	@Text NVARCHAR(max),
	@Updatedby INT,
	@Active BIT,
	@Text_Ar NVARCHAR(max)
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	Declare @return INT;
  
IF EXISTS (SELECT TOP 1 1 FROM Announcements WHERE Id = @Id)  
	BEGIN  
				UPDATE Announcements
				SET 
					[Text] = @Text,
					UpdatedOn = GETDATE(),
					UpdatedBy = @Updatedby,
					Active  = @Active,
					Text_Ar = @Text_Ar
				WHERE Id = @Id

				  IF @@ERROR <> 0  
					SET @return= 106 -- update failed   
				 ELSE  
				  SET @return= 101; -- update success   
				 
	END

ELSE
	BEGIN
	  SET @return= 102; -- Record does not exists  
	END

			SELECT el.Id,
					el.[Text],
					el.[Text_Ar],
					CONCAT(c.FirstName, '', c.LastName) as CreatedBy,  
					CONCAT(l.FirstName, '', l.LastName) as UpdatedBy, 
					el.CreatedOn,
					el.UpdatedOn,
					el.Active FROM Announcements  el 
			   INNER join UserMaster c  on el.CreatedBy= c.Id  
			   INNER join UserMaster l on el.UpdatedBy =l.Id 
			   WHERE el.Active = 1 AND el.id = @Id
	 RETURN @return;  
END

GO

ALTER PROCEDURE [dbo].[spi_Announcements]  
(  
@Text NVARCHAR(max),  
@CreatedBy INT,
@Active BIT,
@Text_Ar NVARCHAR(max)
)  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
  
 SET NOCOUNT ON;  
 DECLARE @Return INT;  
 DECLARE @Id INT;  
 INSERT INTO Announcements  
    (  
    [Text],  
    CreatedBy,  
    CreatedOn,  
    UpdatedBy,  
    UpdatedOn,  
    Active,
	[Text_Ar] 
    )  
  VALUES(  
    @Text,  
    @CreatedBy,  
    GETDATE(),  
    @CreatedBy,  
    GETDATE(),  
    @Active,
	@Text_Ar
  );  
  
  SET @Id = SCOPE_IDENTITY()  
  
SELECT an.[Text],an.[Text_Ar],um.FirstName,um.LastName,an.Active,an.CreatedOn,an.UpdatedOn  
from Announcements an INNER JOIN UserMaster um  
ON an.Createdby = um.Id   
INNER JOIN UserMaster um1  
ON an.UpdatedBy = um1.Id WHERE an.id = @Id  
SET @Return = 100;  
  
RETURN @Return;  
END  


