USE [PM2012]
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [Tasks_Add] - Insert Procedure Script for Tasks
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[Tasks_Add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[Tasks_Add]
GO

CREATE PROCEDURE [dbo].[Tasks_Add]
(
	@TaskID int,
	@ProjectID int,
	@TicketID int,
	@Title varchar(200),
	@Description varchar(4000),
	@IsCompleted bit
)
AS
	SET NOCOUNT ON

	INSERT INTO [Tasks]
	(
		[TaskID],
		[ProjectID],
		[TicketID],
		[Title],
		[Description],
		[IsCompleted]
	)
	VALUES
	(
		@TaskID,
		@ProjectID,
		@TicketID,
		@Title,
		@Description,
		@IsCompleted
	)

	RETURN @@Error
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [Tasks_Update] - Update Procedure Script for Tasks
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[Tasks_Update]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[Tasks_Update]
GO

CREATE PROCEDURE [dbo].[Tasks_Update]
(
	@TaskID int,
	@ProjectID int,
	@TicketID int,
	@Title varchar(200),
	@Description varchar(4000),
	@IsCompleted bit
)
AS
	SET NOCOUNT ON
	
	UPDATE [Tasks]
	SET
		[TaskID] = @TaskID,
		[ProjectID] = @ProjectID,
		[TicketID] = @TicketID,
		[Title] = @Title,
		[Description] = @Description,
		[IsCompleted] = @IsCompleted
	WHERE 
		[TaskID] = @TaskID

	RETURN @@Error
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [Tasks_Delete] - Update Procedure Script for Tasks
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[Tasks_Delete]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[Tasks_Delete]
GO

CREATE PROCEDURE [dbo].[Tasks_Delete]
(
	@TaskID int
)
AS
	SET NOCOUNT ON

	DELETE 
	FROM   [Tasks]
	WHERE  
		[TaskID] = @TaskID

	RETURN @@Error
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [Tasks_GetModel] - GetInfo Procedure Script for Tasks
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[Tasks_GetModel]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[Tasks_GetModel]
GO

CREATE PROCEDURE [dbo].[Tasks_GetModel]
(
	@TaskID int
)
AS
	SET NOCOUNT ON
	
	SELECT * FROM [Tasks]
	
	WHERE 
		[TaskID] = @TaskID

	RETURN @@Error
GO

