USE [PM2012]
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [TimeSheets_Add] - Insert Procedure Script for TimeSheets
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[TimeSheets_Add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[TimeSheets_Add]
GO

CREATE PROCEDURE [dbo].[TimeSheets_Add]
(
	@ID int = NULL OUTPUT,
	@SheetDate datetime,
	@ProjectID int,
	@TicketID int,
	@UserID int,
	@Hours decimal(18,2),
	@Percentage decimal(18,2),
	@Description nvarchar(4000),
	@IsSubmitted bit,
	@CreatedOn datetime,
	@CreatedBy int,
	@ModifiedOn datetime,
	@ModifiedBy int,
	@IsMeeting bit
)
AS
	SET NOCOUNT ON

	INSERT INTO [TimeSheets]
	(
		[SheetDate],
		[ProjectID],
		[TicketID],
		[UserID],
		[Hours],
		[Percentage],
		[Description],
		[IsSubmitted],
		[CreatedOn],
		[CreatedBy],
		[ModifiedOn],
		[ModifiedBy],
		[IsMeeting]
	)
	VALUES
	(
		@SheetDate,
		@ProjectID,
		@TicketID,
		@UserID,
		@Hours,
		@Percentage,
		@Description,
		@IsSubmitted,
		@CreatedOn,
		@CreatedBy,
		@ModifiedOn,
		@ModifiedBy,
		@IsMeeting
	)

	SELECT @ID =ISNULL( SCOPE_IDENTITY(),0);

	RETURN @@Error
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [TimeSheets_Update] - Update Procedure Script for TimeSheets
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[TimeSheets_Update]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[TimeSheets_Update]
GO

CREATE PROCEDURE [dbo].[TimeSheets_Update]
(
	@ID int,
	@SheetDate datetime,
	@ProjectID int,
	@TicketID int,
	@UserID int,
	@Hours decimal(18,2),
	@Percentage decimal(18,2),
	@Description nvarchar(4000),
	@IsSubmitted bit,
	@CreatedOn datetime,
	@CreatedBy int,
	@ModifiedOn datetime,
	@ModifiedBy int,
	@IsMeeting bit
)
AS
	SET NOCOUNT ON
	
	UPDATE [TimeSheets]
	SET
		[SheetDate] = @SheetDate,
		[ProjectID] = @ProjectID,
		[TicketID] = @TicketID,
		[UserID] = @UserID,
		[Hours] = @Hours,
		[Percentage] = @Percentage,
		[Description] = @Description,
		[IsSubmitted] = @IsSubmitted,
		[CreatedOn] = @CreatedOn,
		[CreatedBy] = @CreatedBy,
		[ModifiedOn] = @ModifiedOn,
		[ModifiedBy] = @ModifiedBy,
		[IsMeeting] = @IsMeeting
	WHERE 
		[ID] = @ID

	RETURN @@Error
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [TimeSheets_Delete] - Update Procedure Script for TimeSheets
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[TimeSheets_Delete]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[TimeSheets_Delete]
GO

CREATE PROCEDURE [dbo].[TimeSheets_Delete]
(
	@ID int
)
AS
	SET NOCOUNT ON

	DELETE 
	FROM   [TimeSheets]
	WHERE  
		[ID] = @ID

	RETURN @@Error
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [TimeSheets_GetModel] - GetInfo Procedure Script for TimeSheets
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[TimeSheets_GetModel]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[TimeSheets_GetModel]
GO

CREATE PROCEDURE [dbo].[TimeSheets_GetModel]
(
	@ID int
)
AS
	SET NOCOUNT ON
	
	SELECT * FROM [TimeSheets]
	
	WHERE 
		[ID] = @ID

	RETURN @@Error
GO

