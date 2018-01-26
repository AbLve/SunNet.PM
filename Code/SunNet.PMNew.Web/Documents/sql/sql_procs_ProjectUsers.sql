USE [PM2012]
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [ProjectUsers_Add] - Insert Procedure Script for ProjectUsers
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[ProjectUsers_Add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[ProjectUsers_Add]
GO

CREATE PROCEDURE [dbo].[ProjectUsers_Add]
(
	@PUID int = NULL OUTPUT,
	@ProjectID int,
	@UserID int,
	@ISClient bit
)
AS
	SET NOCOUNT ON

	INSERT INTO [ProjectUsers]
	(
		[ProjectID],
		[UserID],
		[ISClient]
	)
	VALUES
	(
		@ProjectID,
		@UserID,
		@ISClient
	)

	SELECT @PUID =ISNULL( SCOPE_IDENTITY(),0);

	RETURN @@Error
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [ProjectUsers_Update] - Update Procedure Script for ProjectUsers
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[ProjectUsers_Update]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[ProjectUsers_Update]
GO

CREATE PROCEDURE [dbo].[ProjectUsers_Update]
(
	@PUID int,
	@ProjectID int,
	@UserID int,
	@ISClient bit
)
AS
	SET NOCOUNT ON
	
	UPDATE [ProjectUsers]
	SET
		[ProjectID] = @ProjectID,
		[UserID] = @UserID,
		[ISClient] = @ISClient
	WHERE 
		[PUID] = @PUID

	RETURN @@Error
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [ProjectUsers_Delete] - Update Procedure Script for ProjectUsers
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[ProjectUsers_Delete]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[ProjectUsers_Delete]
GO

CREATE PROCEDURE [dbo].[ProjectUsers_Delete]
(
	@PUID int
)
AS
	SET NOCOUNT ON

	DELETE 
	FROM   [ProjectUsers]
	WHERE  
		[PUID] = @PUID

	RETURN @@Error
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [ProjectUsers_GetModel] - GetInfo Procedure Script for ProjectUsers
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[ProjectUsers_GetModel]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[ProjectUsers_GetModel]
GO

CREATE PROCEDURE [dbo].[ProjectUsers_GetModel]
(
	@PUID int
)
AS
	SET NOCOUNT ON
	
	SELECT * FROM [ProjectUsers]
	
	WHERE 
		[PUID] = @PUID

	RETURN @@Error
GO

