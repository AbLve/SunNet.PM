USE [PM2012]
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [Modules_Add] - Insert Procedure Script for Modules
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[Modules_Add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[Modules_Add]
GO

CREATE PROCEDURE [dbo].[Modules_Add]
(
	@ModuleID int = NULL OUTPUT,
	@ModuleTitle nvarchar(50),
	@ModulePath nvarchar(50),
	@Status int,
	@Orders int,
	@ParentID int
)
AS
	SET NOCOUNT ON

	INSERT INTO [Modules]
	(
		[ModuleTitle],
		[ModulePath],
		[Status],
		[Orders],
		[ParentID]
	)
	VALUES
	(
		@ModuleTitle,
		@ModulePath,
		@Status,
		@Orders,
		@ParentID
	)

	SELECT @ModuleID =ISNULL( SCOPE_IDENTITY(),0);

	RETURN @@Error
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [Modules_Update] - Update Procedure Script for Modules
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[Modules_Update]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[Modules_Update]
GO

CREATE PROCEDURE [dbo].[Modules_Update]
(
	@ModuleID int,
	@ModuleTitle nvarchar(50),
	@ModulePath nvarchar(50),
	@Status int,
	@Orders int,
	@ParentID int
)
AS
	SET NOCOUNT ON
	
	UPDATE [Modules]
	SET
		[ModuleTitle] = @ModuleTitle,
		[ModulePath] = @ModulePath,
		[Status] = @Status,
		[Orders] = @Orders,
		[ParentID] = @ParentID
	WHERE 
		[ModuleID] = @ModuleID

	RETURN @@Error
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [Modules_Delete] - Update Procedure Script for Modules
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[Modules_Delete]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[Modules_Delete]
GO

CREATE PROCEDURE [dbo].[Modules_Delete]
(
	@ModuleID int
)
AS
	SET NOCOUNT ON

	DELETE 
	FROM   [Modules]
	WHERE  
		[ModuleID] = @ModuleID

	RETURN @@Error
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [Modules_GetModel] - GetInfo Procedure Script for Modules
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[Modules_GetModel]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[Modules_GetModel]
GO

CREATE PROCEDURE [dbo].[Modules_GetModel]
(
	@ModuleID int
)
AS
	SET NOCOUNT ON
	
	SELECT * FROM [Modules]
	
	WHERE 
		[ModuleID] = @ModuleID

	RETURN @@Error
GO

