USE [PM2012]
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [Roles_Add] - Insert Procedure Script for Roles
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[Roles_Add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[Roles_Add]
GO

CREATE PROCEDURE [dbo].[Roles_Add]
(
	@RoleID int = NULL OUTPUT,
	@RoleName nvarchar(50),
	@Description varchar(500),
	@Status int,
	@CreatedOn datetime,
	@CreatedBy int
)
AS
	SET NOCOUNT ON

	INSERT INTO [Roles]
	(
		[RoleName],
		[Description],
		[Status],
		[CreatedOn],
		[CreatedBy]
	)
	VALUES
	(
		@RoleName,
		@Description,
		@Status,
		@CreatedOn,
		@CreatedBy
	)

	SELECT @RoleID =ISNULL( SCOPE_IDENTITY(),0);

	RETURN @@Error
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [Roles_Update] - Update Procedure Script for Roles
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[Roles_Update]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[Roles_Update]
GO

CREATE PROCEDURE [dbo].[Roles_Update]
(
	@RoleID int,
	@RoleName nvarchar(50),
	@Description varchar(500),
	@Status int,
	@CreatedOn datetime,
	@CreatedBy int
)
AS
	SET NOCOUNT ON
	
	UPDATE [Roles]
	SET
		[RoleName] = @RoleName,
		[Description] = @Description,
		[Status] = @Status,
		[CreatedOn] = @CreatedOn,
		[CreatedBy] = @CreatedBy
	WHERE 
		[RoleID] = @RoleID

	RETURN @@Error
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [Roles_Delete] - Update Procedure Script for Roles
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[Roles_Delete]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[Roles_Delete]
GO

CREATE PROCEDURE [dbo].[Roles_Delete]
(
	@RoleID int
)
AS
	SET NOCOUNT ON

	DELETE 
	FROM   [Roles]
	WHERE  
		[RoleID] = @RoleID

	RETURN @@Error
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [Roles_GetModel] - GetInfo Procedure Script for Roles
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[Roles_GetModel]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[Roles_GetModel]
GO

CREATE PROCEDURE [dbo].[Roles_GetModel]
(
	@RoleID int
)
AS
	SET NOCOUNT ON
	
	SELECT * FROM [Roles]
	
	WHERE 
		[RoleID] = @RoleID

	RETURN @@Error
GO

