USE [PM2012]
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [TicketsOrder_Add] - Insert Procedure Script for TicketsOrder
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[TicketsOrder_Add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[TicketsOrder_Add]
GO

CREATE PROCEDURE [dbo].[TicketsOrder_Add]
(
	@TicketOrderID int,
	@ProjectID int = NULL,
	@TicketID int,
	@OrderNum int = NULL
)
AS
	SET NOCOUNT ON

	INSERT INTO [TicketsOrder]
	(
		[TicketOrderID],
		[ProjectID],
		[TicketID],
		[OrderNum]
	)
	VALUES
	(
		@TicketOrderID,
		@ProjectID,
		@TicketID,
		@OrderNum
	)

	RETURN @@Error
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [TicketsOrder_Update] - Update Procedure Script for TicketsOrder
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[TicketsOrder_Update]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[TicketsOrder_Update]
GO

CREATE PROCEDURE [dbo].[TicketsOrder_Update]
(
	@TicketOrderID int,
	@ProjectID int = NULL,
	@TicketID int,
	@OrderNum int = NULL
)
AS
	SET NOCOUNT ON
	
	UPDATE [TicketsOrder]
	SET
		[TicketOrderID] = @TicketOrderID,
		[ProjectID] = @ProjectID,
		[TicketID] = @TicketID,
		[OrderNum] = @OrderNum
	WHERE 
		[TicketOrderID] = @TicketOrderID

	RETURN @@Error
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [TicketsOrder_Delete] - Update Procedure Script for TicketsOrder
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[TicketsOrder_Delete]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[TicketsOrder_Delete]
GO

CREATE PROCEDURE [dbo].[TicketsOrder_Delete]
(
	@TicketOrderID int
)
AS
	SET NOCOUNT ON

	DELETE 
	FROM   [TicketsOrder]
	WHERE  
		[TicketOrderID] = @TicketOrderID

	RETURN @@Error
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [TicketsOrder_GetModel] - GetInfo Procedure Script for TicketsOrder
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[TicketsOrder_GetModel]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[TicketsOrder_GetModel]
GO

CREATE PROCEDURE [dbo].[TicketsOrder_GetModel]
(
	@TicketOrderID int
)
AS
	SET NOCOUNT ON
	
	SELECT * FROM [TicketsOrder]
	
	WHERE 
		[TicketOrderID] = @TicketOrderID

	RETURN @@Error
GO

