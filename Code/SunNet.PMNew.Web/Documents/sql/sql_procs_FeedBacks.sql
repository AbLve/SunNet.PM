USE [PM2012]
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [FeedBacks_Add] - Insert Procedure Script for FeedBacks
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[FeedBacks_Add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[FeedBacks_Add]
GO

CREATE PROCEDURE [dbo].[FeedBacks_Add]
(
	@FeedBackID int = NULL OUTPUT,
	@TicketID int,
	@Title varchar(200),
	@Description varchar(4000),
	@CreatedBy int,
	@CreateUserName varchar(100),
	@CreatedOn datetime,
	@IsDelete bit,
	@IsReview bit,
	@ReviewFeedBackID int,
	@IsPublic bit,
	@IsClientResponse bit
)
AS
	SET NOCOUNT ON

	INSERT INTO [FeedBacks]
	(
		[TicketID],
		[Title],
		[Description],
		[CreatedBy],
		[CreateUserName],
		[CreatedOn],
		[IsDelete],
		[IsReview],
		[ReviewFeedBackID],
		[IsPublic],
		[IsClientResponse]
	)
	VALUES
	(
		@TicketID,
		@Title,
		@Description,
		@CreatedBy,
		@CreateUserName,
		@CreatedOn,
		@IsDelete,
		@IsReview,
		@ReviewFeedBackID,
		@IsPublic,
		@IsClientResponse
	)

	SELECT @FeedBackID =ISNULL( SCOPE_IDENTITY(),0);

	RETURN @@Error
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [FeedBacks_Update] - Update Procedure Script for FeedBacks
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[FeedBacks_Update]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[FeedBacks_Update]
GO

CREATE PROCEDURE [dbo].[FeedBacks_Update]
(
	@FeedBackID int,
	@TicketID int,
	@Title varchar(200),
	@Description varchar(4000),
	@CreatedBy int,
	@CreateUserName varchar(100),
	@CreatedOn datetime,
	@IsDelete bit,
	@IsReview bit,
	@ReviewFeedBackID int,
	@IsPublic bit,
	@IsClientResponse bit
)
AS
	SET NOCOUNT ON
	
	UPDATE [FeedBacks]
	SET
		[TicketID] = @TicketID,
		[Title] = @Title,
		[Description] = @Description,
		[CreatedBy] = @CreatedBy,
		[CreateUserName] = @CreateUserName,
		[CreatedOn] = @CreatedOn,
		[IsDelete] = @IsDelete,
		[IsReview] = @IsReview,
		[ReviewFeedBackID] = @ReviewFeedBackID,
		[IsPublic] = @IsPublic,
		[IsClientResponse] = @IsClientResponse
	WHERE 
		[FeedBackID] = @FeedBackID

	RETURN @@Error
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [FeedBacks_Delete] - Update Procedure Script for FeedBacks
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[FeedBacks_Delete]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[FeedBacks_Delete]
GO

CREATE PROCEDURE [dbo].[FeedBacks_Delete]
(
	@FeedBackID int
)
AS
	SET NOCOUNT ON

	DELETE 
	FROM   [FeedBacks]
	WHERE  
		[FeedBackID] = @FeedBackID

	RETURN @@Error
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [FeedBacks_GetModel] - GetInfo Procedure Script for FeedBacks
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[FeedBacks_GetModel]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[FeedBacks_GetModel]
GO

CREATE PROCEDURE [dbo].[FeedBacks_GetModel]
(
	@FeedBackID int
)
AS
	SET NOCOUNT ON
	
	SELECT * FROM [FeedBacks]
	
	WHERE 
		[FeedBackID] = @FeedBackID

	RETURN @@Error
GO

