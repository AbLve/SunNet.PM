USE [PM2012]
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [Pages_Add] - Insert Procedure Script for Pages
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[Pages_Add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[Pages_Add]
GO

CREATE PROCEDURE [dbo].[Pages_Add]
(
	@PageID int = NULL OUTPUT,
	@PageName varchar(50),
	@PageTitle varchar(50),
	@MID int,
	@Orders int,
	@Status int,
	@IsMenu bit
)
AS
	SET NOCOUNT ON

	INSERT INTO [Pages]
	(
		[PageName],
		[PageTitle],
		[MID],
		[Orders],
		[Status],
		[IsMenu]
	)
	VALUES
	(
		@PageName,
		@PageTitle,
		@MID,
		@Orders,
		@Status,
		@IsMenu
	)

	SELECT @PageID =ISNULL( SCOPE_IDENTITY(),0);

	RETURN @@Error
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [Pages_Update] - Update Procedure Script for Pages
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[Pages_Update]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[Pages_Update]
GO

CREATE PROCEDURE [dbo].[Pages_Update]
(
	@PageID int,
	@PageName varchar(50),
	@PageTitle varchar(50),
	@MID int,
	@Orders int,
	@Status int,
	@IsMenu bit
)
AS
	SET NOCOUNT ON
	
	UPDATE [Pages]
	SET
		[PageName] = @PageName,
		[PageTitle] = @PageTitle,
		[MID] = @MID,
		[Orders] = @Orders,
		[Status] = @Status,
		[IsMenu] = @IsMenu
	WHERE 
		[PageID] = @PageID

	RETURN @@Error
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [Pages_Delete] - Update Procedure Script for Pages
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[Pages_Delete]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[Pages_Delete]
GO

CREATE PROCEDURE [dbo].[Pages_Delete]
(
	@PageID int
)
AS
	SET NOCOUNT ON

	DELETE 
	FROM   [Pages]
	WHERE  
		[PageID] = @PageID

	RETURN @@Error
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [Pages_GetModel] - GetInfo Procedure Script for Pages
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[Pages_GetModel]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[Pages_GetModel]
GO

CREATE PROCEDURE [dbo].[Pages_GetModel]
(
	@PageID int
)
AS
	SET NOCOUNT ON
	
	SELECT * FROM [Pages]
	
	WHERE 
		[PageID] = @PageID

	RETURN @@Error
GO

