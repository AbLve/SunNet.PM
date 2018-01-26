USE [PM2012]
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [Tickets_Add] - Insert Procedure Script for Tickets
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[Tickets_Add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[Tickets_Add]
GO

CREATE PROCEDURE [dbo].[Tickets_Add]
(
	@TicketID int,
	@CompanyID int,
	@ProjectID int,
	@Title varchar(200),
	@TicketType varchar(18),
	@Description varchar(4000),
	@CreatedOn datetime,
	@CreatedBy int,
	@CreateUserName varchar(100),
	@ModifiedOn datetime,
	@ModifiedBy int,
	@PublishDate datetime,
	@ClientPublished bit,
	@StartDate datetime,
	@DeliveryDate datetime,
	@ContinueDate int,
	@URL varchar(100),
	@Priority int,
	@Status int,
	@ConvertDelete int,
	@IsInternal bit,
	@CreateType int,
	@SourceTicketID int,
	@IsEstimates bit,
	@DevTsHours int,
	@QaTsHours int,
	@Hours int
)
AS
	SET NOCOUNT ON

	INSERT INTO [Tickets]
	(
		[TicketID],
		[CompanyID],
		[ProjectID],
		[Title],
		[TicketType],
		[Description],
		[CreatedOn],
		[CreatedBy],
		[CreateUserName],
		[ModifiedOn],
		[ModifiedBy],
		[PublishDate],
		[ClientPublished],
		[StartDate],
		[DeliveryDate],
		[ContinueDate],
		[URL],
		[Priority],
		[Status],
		[ConvertDelete],
		[IsInternal],
		[CreateType],
		[SourceTicketID],
		[IsEstimates],
		[DevTsHours],
		[QaTsHours],
		[Hours]
	)
	VALUES
	(
		@TicketID,
		@CompanyID,
		@ProjectID,
		@Title,
		@TicketType,
		@Description,
		@CreatedOn,
		@CreatedBy,
		@CreateUserName,
		@ModifiedOn,
		@ModifiedBy,
		@PublishDate,
		@ClientPublished,
		@StartDate,
		@DeliveryDate,
		@ContinueDate,
		@URL,
		@Priority,
		@Status,
		@ConvertDelete,
		@IsInternal,
		@CreateType,
		@SourceTicketID,
		@IsEstimates,
		@DevTsHours,
		@QaTsHours,
		@Hours
	)

	RETURN @@Error
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [Tickets_Update] - Update Procedure Script for Tickets
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[Tickets_Update]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[Tickets_Update]
GO

CREATE PROCEDURE [dbo].[Tickets_Update]
(
	@TicketID int,
	@CompanyID int,
	@ProjectID int,
	@Title varchar(200),
	@TicketType varchar(18),
	@Description varchar(4000),
	@CreatedOn datetime,
	@CreatedBy int,
	@CreateUserName varchar(100),
	@ModifiedOn datetime,
	@ModifiedBy int,
	@PublishDate datetime,
	@ClientPublished bit,
	@StartDate datetime,
	@DeliveryDate datetime,
	@ContinueDate int,
	@URL varchar(100),
	@Priority int,
	@Status int,
	@ConvertDelete int,
	@IsInternal bit,
	@CreateType int,
	@SourceTicketID int,
	@IsEstimates bit,
	@DevTsHours int,
	@QaTsHours int,
	@Hours int
)
AS
	SET NOCOUNT ON
	
	UPDATE [Tickets]
	SET
		[TicketID] = @TicketID,
		[CompanyID] = @CompanyID,
		[ProjectID] = @ProjectID,
		[Title] = @Title,
		[TicketType] = @TicketType,
		[Description] = @Description,
		[CreatedOn] = @CreatedOn,
		[CreatedBy] = @CreatedBy,
		[CreateUserName] = @CreateUserName,
		[ModifiedOn] = @ModifiedOn,
		[ModifiedBy] = @ModifiedBy,
		[PublishDate] = @PublishDate,
		[ClientPublished] = @ClientPublished,
		[StartDate] = @StartDate,
		[DeliveryDate] = @DeliveryDate,
		[ContinueDate] = @ContinueDate,
		[URL] = @URL,
		[Priority] = @Priority,
		[Status] = @Status,
		[ConvertDelete] = @ConvertDelete,
		[IsInternal] = @IsInternal,
		[CreateType] = @CreateType,
		[SourceTicketID] = @SourceTicketID,
		[IsEstimates] = @IsEstimates,
		[DevTsHours] = @DevTsHours,
		[QaTsHours] = @QaTsHours,
		[Hours] = @Hours
	WHERE 
		[TicketID] = @TicketID

	RETURN @@Error
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [Tickets_Delete] - Update Procedure Script for Tickets
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[Tickets_Delete]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[Tickets_Delete]
GO

CREATE PROCEDURE [dbo].[Tickets_Delete]
(
	@TicketID int
)
AS
	SET NOCOUNT ON

	DELETE 
	FROM   [Tickets]
	WHERE  
		[TicketID] = @TicketID

	RETURN @@Error
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [Tickets_GetModel] - GetInfo Procedure Script for Tickets
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[Tickets_GetModel]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[Tickets_GetModel]
GO

CREATE PROCEDURE [dbo].[Tickets_GetModel]
(
	@TicketID int
)
AS
	SET NOCOUNT ON
	
	SELECT * FROM [Tickets]
	
	WHERE 
		[TicketID] = @TicketID

	RETURN @@Error
GO

