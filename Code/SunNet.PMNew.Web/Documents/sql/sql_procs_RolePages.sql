USE [PM2012]
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [RolePages_Add] - Insert Procedure Script for RolePages
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[RolePages_Add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[RolePages_Add]
GO

CREATE PROCEDURE [dbo].[RolePages_Add]
(
	@RPID int = NULL OUTPUT,
	@RoleID int,
	@PageID int
)
AS
	SET NOCOUNT ON

	INSERT INTO [RolePages]
	(
		[RoleID],
		[PageID]
	)
	VALUES
	(
		@RoleID,
		@PageID
	)

	SELECT @RPID =ISNULL( SCOPE_IDENTITY(),0);

	RETURN @@Error
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [RolePages_Update] - Update Procedure Script for RolePages
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[RolePages_Update]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[RolePages_Update]
GO

CREATE PROCEDURE [dbo].[RolePages_Update]
(
	@RPID int,
	@RoleID int,
	@PageID int
)
AS
	SET NOCOUNT ON
	
	UPDATE [RolePages]
	SET
		[RoleID] = @RoleID,
		[PageID] = @PageID
	WHERE 
		[RPID] = @RPID

	RETURN @@Error
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [RolePages_Delete] - Update Procedure Script for RolePages
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[RolePages_Delete]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[RolePages_Delete]
GO

CREATE PROCEDURE [dbo].[RolePages_Delete]
(
	@RPID int
)
AS
	SET NOCOUNT ON

	DELETE 
	FROM   [RolePages]
	WHERE  
		[RPID] = @RPID

	RETURN @@Error
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [RolePages_GetModel] - GetInfo Procedure Script for RolePages
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[RolePages_GetModel]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[RolePages_GetModel]
GO

CREATE PROCEDURE [dbo].[RolePages_GetModel]
(
	@RPID int
)
AS
	SET NOCOUNT ON
	
	SELECT * FROM [RolePages]
	
	WHERE 
		[RPID] = @RPID

	RETURN @@Error
GO

