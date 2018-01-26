USE [PM2012]
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [RoleModules_Add] - Insert Procedure Script for RoleModules
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[RoleModules_Add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[RoleModules_Add]
GO

CREATE PROCEDURE [dbo].[RoleModules_Add]
(
	@RMID int = NULL OUTPUT,
	@RoleID int,
	@ModuleID int
)
AS
	SET NOCOUNT ON

	INSERT INTO [RoleModules]
	(
		[RoleID],
		[ModuleID]
	)
	VALUES
	(
		@RoleID,
		@ModuleID
	)

	SELECT @RMID =ISNULL( SCOPE_IDENTITY(),0);

	RETURN @@Error
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [RoleModules_Update] - Update Procedure Script for RoleModules
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[RoleModules_Update]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[RoleModules_Update]
GO

CREATE PROCEDURE [dbo].[RoleModules_Update]
(
	@RMID int,
	@RoleID int,
	@ModuleID int
)
AS
	SET NOCOUNT ON
	
	UPDATE [RoleModules]
	SET
		[RoleID] = @RoleID,
		[ModuleID] = @ModuleID
	WHERE 
		[RMID] = @RMID

	RETURN @@Error
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [RoleModules_Delete] - Update Procedure Script for RoleModules
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[RoleModules_Delete]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[RoleModules_Delete]
GO

CREATE PROCEDURE [dbo].[RoleModules_Delete]
(
	@RMID int
)
AS
	SET NOCOUNT ON

	DELETE 
	FROM   [RoleModules]
	WHERE  
		[RMID] = @RMID

	RETURN @@Error
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [RoleModules_GetModel] - GetInfo Procedure Script for RoleModules
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[RoleModules_GetModel]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[RoleModules_GetModel]
GO

CREATE PROCEDURE [dbo].[RoleModules_GetModel]
(
	@RMID int
)
AS
	SET NOCOUNT ON
	
	SELECT * FROM [RoleModules]
	
	WHERE 
		[RMID] = @RMID

	RETURN @@Error
GO

