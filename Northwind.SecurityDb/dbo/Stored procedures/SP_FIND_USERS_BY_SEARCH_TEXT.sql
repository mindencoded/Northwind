CREATE PROCEDURE [dbo].[SP_FIND_USERS_BY_SEARCH_TEXT]
	@SearchText varchar,
	@IncludeInactiveUsers bit
AS
BEGIN
	SELECT u.ID, u.USERNAME, u.ACTIVE FROM [dbo].[USER] u WHERE u.USERNAME LIKE '%' + @SearchText + '%' AND u.ACTIVE <> @IncludeInactiveUsers ORDER BY u.USERNAME DESC;
END

