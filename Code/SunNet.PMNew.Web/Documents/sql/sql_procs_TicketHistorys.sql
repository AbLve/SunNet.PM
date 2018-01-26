USE [PM2012]
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [TicketHistorys_Add] - Insert Procedure Script for TicketHistorys
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[TicketHistorys_Add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[TicketHistorys_Add]
GO

CREATE PROCEDURE [dbo].[TicketHistorys_Add]
(
	@TDHID int = NULL OUTPUT,
	@TicketID int,
	@Description varchar(4000),
	@ModifiedOn datetime,
	@ModifiedBy int
)
AS
	SET NOCOUNT ON

	INSERT INTO [TicketHistorys]
	(
		[TicketID],
		[Description],
		[ModifiedOn],
		[ModifiedBy]
	)
	VALUES
	(
		@TicketID,
		@Description,
		@ModifiedOn,
		@ModifiedBy
	)

	SELECT @TDHID =ISNULL( SCOPE_IDENTITY(),0);

	RETURN @@Error
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [TicketHistorys_Update] - Update Procedure Script for TicketHistorys
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[TicketHistorys_Update]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[TicketHistorys_Update]
GO

CREATE PROCEDURE [dbo].[TicketHistorys_Update]
(
	@TDHID int,
	@TicketID int,
	@Description varchar(4000),
	@ModifiedOn datetime,
	@ModifiedBy int
)
AS
	SET NOCOUNT ON
	
	UPDATE [TicketHistorys]
	SET
		[TicketID] = @TicketID,
		[Description] = @Description,
		[ModifiedOn] = @ModifiedOn,
		[ModifiedBy] = @ModifiedBy
	WHERE 
		[TDHID] = @TDHID

	RETURN @@Error
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [TicketHistorys_Delete] - Update Procedure Script for TicketHistorys
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[TicketHistorys_Delete]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[TicketHistorys_Delete]
GO

CREATE PROCEDURE [dbo].[TicketHistorys_Delete]
(
	@TDHID int
)
AS
	SET NOCOUNT ON

	DELETE 
	FROM   [TicketHistorys]
	WHERE  
		[TDHID] = @TDHID

	RETURN @@Error
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [TicketHistorys_GetModel] - GetInfo Procedure Script for TicketHistorys
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[TicketHistorys_GetModel]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[TicketHistorys_GetModel]
GO

CREATE PROCEDURE [dbo].[TicketHistorys_GetModel]
(
	@TDHID int
)
AS
	SET NOCOUNT ON
	
	SELECT * FROM [TicketHistorys]
	
	WHERE 
		[TDHID] = @TDHID

	RETURN @@Error
GO

