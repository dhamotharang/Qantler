ALTER PROCEDURE [dbo].[spu_CommunityApproveRejectCount]
(
  @ApproveCount INT,
  @RejectCount INT,
  @UserId INT
)
AS
BEGIN
if((select count(*) from CommunityApproveRejectCount)= 0)
begin
insert into CommunityApproveRejectCount(ApproveCount,RejectCount,LastUpdatedOn,LastUpdatedBy) values(@ApproveCount,@RejectCount,GETDATE(),@UserId)
end
else
begin
UPDATE CommunityApproveRejectCount SET ApproveCount = @ApproveCount, RejectCount = @RejectCount , LastUpdatedBy = @UserId,
LastUpdatedOn = GETDATE()
RETURN 101
end
END