ALTER PROCEDURE [dbo].[DeleteCourse] 
(
@Id INT
)
AS
BEGIN	
Declare @return INT

IF EXISTS (SELECT * FROM CourseMaster  WHERE Id=@Id)
		BEGIN
		IF NOT EXISTS (select * from CourseApprovals where CourseId=@Id)

			BEGIN

			BEGIN TRY
				BEGIN TRANSACTION 
					Delete from CourseURLReferences WHERE CourseId=@Id

					DELETE FROM CourseAssociatedFiles WHERE CourseId=@Id

					DELETE FROM CourseApprovals WHERE CourseId=@Id

					DELETE FROM CourseComments WHERE CourseId=@Id

					DELETE FROM CourseEnrollment WHERE CourseId=@Id

					DELETE FROM MoECheckMaster WHERE ContentId=@Id and ContentType =1

					DELETE FROM CommunityCheckMaster WHERE ContentId=@Id and ContentType =1

					DELETE FROM CourseResourceMapping WHERE CourseId=@Id

					DELETE FROM UserCourseTests WHERE CourseId=@Id

					DELETE FROM AnswerOptions WHERE QuestionId IN (SELECT Id FROM Questions WHERE TestId IN 
					(SELECT Id FROM Tests WHERE CourseId=@Id))

					DELETE FROM Questions WHERE TestId IN 
					(SELECT Id FROM Tests WHERE CourseId=@Id)

					DELETE FROM Tests WHERE CourseId=@Id	
					
					DELETE FROM UserBookmarks WHERE ContentId = @Id AND ContentType = 1		

					DELETE FROM CourseMaster  WHERE Id=@Id;
				COMMIT
				SET @return =103 -- record DELETED	
			END TRY
			BEGIN CATCH
				IF @@TRANCOUNT > 0
					ROLLBACK TRAN
					SET @return =114 --record deletion failed
					
			END CATCH
				
					
			END	

			ELSE

			SET @return = 104 -- trying active record deletion			

		END

	    ELSE 

		SET @return = 102 -- reconrd does not exists	
			
	    RETURN @return
END
GO

 ALTER PROCEDURE [dbo].[DeleteResource]
(
@Id INT
)
AS
BEGIN	
Declare @return INT

IF EXISTS (SELECT * FROM ResourceMaster  WHERE Id=@Id)
		BEGIN
		--IF NOT EXISTS (select * from ResourceApprovals where ResourceId=@Id)

		--	BEGIN

			BEGIN TRY
				BEGIN TRANSACTION 
					Delete from ResourceURLReferences WHERE ResourceId=@Id

					DELETE FROM ResourceAssociatedFiles WHERE ResourceId=@Id

					Delete from resourceapprovals WHERE ResourceId=@Id

					Delete from resourcecomments WHERE ResourceId=@Id

					DELETE FROM MoECheckMaster WHERE ContentId=@Id and ContentType=2

					DELETE FROM CommunityCheckMaster WHERE ContentId=@Id and ContentType=2

					DELETE FROM UserBookMarks WHERE ContentId = @Id AND ContentType = 2

					UPDATE CourseResourceMapping SET ResourcesId = NULL WHERE ResourcesId=@Id;

					UPDATE SectionResource SET ResourceId = NULL WHERE ResourceId=@Id;

					DELETE FROM ResourceMaster  WHERE Id=@Id;
				COMMIT
				SET @return =103 -- record DELETED	
			END TRY
			BEGIN CATCH
				IF @@TRANCOUNT > 0
					ROLLBACK TRAN
					SET @return =120 --record deletion failed
					
			END CATCH
			
					
			--END	

			--ELSE

			--SET @return = 104 -- trying active record deletion			

		END

	    ELSE 

		SET @return = 102 -- reconrd does not exists	
		PRINT @return
	    RETURN @return
END
Go 

delete from CommunityCheckMaster where (ContentType=2 and ContentId not in (select Id from ResourceMaster))
delete from CommunityCheckMaster where (ContentType=1 and ContentId not in (select Id from CourseMaster))
delete from MoECheckMaster where (ContentType=2 and ContentId not in (select Id from ResourceMaster))
delete from MoECheckMaster where (ContentType=1 and ContentId not in (select Id from CourseMaster))