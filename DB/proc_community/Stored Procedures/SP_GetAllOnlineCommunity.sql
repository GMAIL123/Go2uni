CREATE PROC [proc_community].[SP_GetAllOnlineCommunity]
AS
BEGIN
	SELECT * FROM studentinfo.Online_Community
END