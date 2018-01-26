USE [PM2012]
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [CateGory_Add] - Insert Procedure Script for CateGory
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[CateGory_Add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[CateGory_Add]
GO

CREATE PROCEDURE [dbo].[CateGory_Add]
(
	@GID int,
	@Title nvarchar(100),
	@CreatedOn datetime,
	@CreatedBy int,
	@IsOnlyShowTody bit,
	@IsDelete bit
)
AS
	SET NOCOUNT ON

	INSERT INTO [CateGory]
	(
		[GID],
		[Title],
		[CreatedOn],
		[CreatedBy],
		[IsOnlyShowTody],
		[IsDelete]
	)
	VALUES
	(
		@GID,
		@Title,
		@CreatedOn,
		@CreatedBy,
		@IsOnlyShowTody,
		@IsDelete
	)

	RETURN @@Error
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [CateGory_Update] - Update Procedure Script for CateGory
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[CateGory_Update]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[CateGory_Update]
GO

CREATE PROCEDURE [dbo].[CateGory_Update]
(
	@GID int,
	@Title nvarchar(100),
	@CreatedOn datetime,
	@CreatedBy int,
	@IsOnlyShowTody bit,
	@IsDelete bit
)
AS
	SET NOCOUNT ON
	
	UPDATE [CateGory]
	SET
		[GID] = @GID,
		[Title] = @Title,
		[CreatedOn] = @CreatedOn,
		[CreatedBy] = @CreatedBy,
		[IsOnlyShowTody] = @IsOnlyShowTody,
		[IsDelete] = @IsDelete
	WHERE 
		[GID] = @GID

	RETURN @@Error
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [CateGory_Delete] - Update Procedure Script for CateGory
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[CateGory_Delete]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[CateGory_Delete]
GO

CREATE PROCEDURE [dbo].[CateGory_Delete]
(
	@GID int
)
AS
	SET NOCOUNT ON

	DELETE 
	FROM   [CateGory]
	WHERE  
		[GID] = @GID

	RETURN @@Error
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [CateGory_GetModel] - GetInfo Procedure Script for CateGory
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[CateGory_GetModel]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[CateGory_GetModel]
GO

CREATE PROCEDURE [dbo].[CateGory_GetModel]
(
	@GID int
)
AS
	SET NOCOUNT ON
	
	SELECT * FROM [CateGory]
	
	WHERE 
		[GID] = @GID

	RETURN @@Error
GO

