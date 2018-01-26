USE [PM2012]
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [Files_Add] - Insert Procedure Script for Files
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[Files_Add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[Files_Add]
GO

CREATE PROCEDURE [dbo].[Files_Add]
(
	@FileID int = NULL OUTPUT,
	@SourceID int,
	@SourceType int,
	@FileTitle varchar(200),
	@ContentType varchar(100),
	@FileSize int,
	@FilePath varchar(200),
	@ThumbPath varchar(100),
	@CreatedOn datetime,
	@CreatedBy int,
	@IsDelete bit,
	@IsPublic bit
)
AS
	SET NOCOUNT ON

	INSERT INTO [Files]
	(
		[SourceID],
		[SourceType],
		[FileTitle],
		[ContentType],
		[FileSize],
		[FilePath],
		[ThumbPath],
		[CreatedOn],
		[CreatedBy],
		[IsDelete],
		[IsPublic]
	)
	VALUES
	(
		@SourceID,
		@SourceType,
		@FileTitle,
		@ContentType,
		@FileSize,
		@FilePath,
		@ThumbPath,
		@CreatedOn,
		@CreatedBy,
		@IsDelete,
		@IsPublic
	)

	SELECT @FileID =ISNULL( SCOPE_IDENTITY(),0);

	RETURN @@Error
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [Files_Update] - Update Procedure Script for Files
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[Files_Update]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[Files_Update]
GO

CREATE PROCEDURE [dbo].[Files_Update]
(
	@FileID int,
	@SourceID int,
	@SourceType int,
	@FileTitle varchar(200),
	@ContentType varchar(100),
	@FileSize int,
	@FilePath varchar(200),
	@ThumbPath varchar(100),
	@CreatedOn datetime,
	@CreatedBy int,
	@IsDelete bit,
	@IsPublic bit
)
AS
	SET NOCOUNT ON
	
	UPDATE [Files]
	SET
		[SourceID] = @SourceID,
		[SourceType] = @SourceType,
		[FileTitle] = @FileTitle,
		[ContentType] = @ContentType,
		[FileSize] = @FileSize,
		[FilePath] = @FilePath,
		[ThumbPath] = @ThumbPath,
		[CreatedOn] = @CreatedOn,
		[CreatedBy] = @CreatedBy,
		[IsDelete] = @IsDelete,
		[IsPublic] = @IsPublic
	WHERE 
		[FileID] = @FileID

	RETURN @@Error
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [Files_Delete] - Update Procedure Script for Files
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[Files_Delete]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[Files_Delete]
GO

CREATE PROCEDURE [dbo].[Files_Delete]
(
	@FileID int
)
AS
	SET NOCOUNT ON

	DELETE 
	FROM   [Files]
	WHERE  
		[FileID] = @FileID

	RETURN @@Error
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [Files_GetModel] - GetInfo Procedure Script for Files
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[Files_GetModel]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[Files_GetModel]
GO

CREATE PROCEDURE [dbo].[Files_GetModel]
(
	@FileID int
)
AS
	SET NOCOUNT ON
	
	SELECT * FROM [Files]
	
	WHERE 
		[FileID] = @FileID

	RETURN @@Error
GO

