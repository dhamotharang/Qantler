
ALTER PROCEDURE [dbo].[spu_CourseTest]
	(
	@CourseId INT,
	@TestName NVARCHAR(100),
	@UT_Questions [UT_QuestionsForContent] Readonly,
	@UT_AnswerOptions [UT_AnswerOptions] Readonly,
	@CreatedBy INT
	)
AS
BEGIN
	
DECLARE @QuestionCount INT;
DECLARE @TestID INT;
DECLARE @QID INT;
DECLARE @i INT;
SET @i = 1;


IF EXISTS(SELECT TOP 1 1 FROM tests WHERE TestName = @TestName and CourseID = @CourseId)

BEGIN
SELECT @TestID = Id from Tests WHERE TestName = @TestName
DELETE FROM  usercoursetests WHERE CourseID = @CourseId
DELETE FROM answeroptions WHERE QuestionId IN (Select Id FROM Questions WHERE  TestId = @TestID)
DELETE FROM Questions WHERE TestId = @TestID

SET @QuestionCount = (SELECT Count(*) FROM @UT_Questions)


WHILE @i <= @QuestionCount

BEGIN

INSERT INTO questions
(
QuestionText,
TestId,
Media,
FileName
)

SELECT QuestionText,@TestID,Media,[FileName] FROM @UT_Questions WHERE QuestionId = @i

SET @QID = SCOPE_IDENTITY();

INSERT INTO answeroptions(QuestionId,
AnswerOption,
CorrectOption)

SELECT 
@QID,
OptionText,
CorrectOption FROM @UT_AnswerOptions WHERE QuestionId =(SELECT  QuestionId FROM @UT_Questions WHERE QuestionId = @i)
SET @i= @i+1;
END;



END;
ELSE

BEGIN
INSERT INTO tests(TestName,
CreatedBy,
CreatedOn,
CourseId) VALUES(
@TestName,
@CreatedBy,
GETDATE(),
@CourseId);

SET @TestID = SCOPE_IDENTITY();
SET @QuestionCount = (SELECT Count(*) FROM @UT_Questions)


WHILE @i <= @QuestionCount

BEGIN

INSERT INTO questions
(
QuestionText,
TestId,
Media,
FileName
)

SELECT QuestionText,@TestID,Media,[FileName] FROM @UT_Questions WHERE QuestionId = @i

SET @QID = SCOPE_IDENTITY();

INSERT INTO answeroptions(QuestionId,
AnswerOption,
CorrectOption)

SELECT 
@QID,
OptionText,
CorrectOption FROM @UT_AnswerOptions WHERE QuestionId =(SELECT  QuestionId FROM @UT_Questions WHERE QuestionId = @i)
SET @i= @i+1;
END;



END;

RETURN 100;
END



