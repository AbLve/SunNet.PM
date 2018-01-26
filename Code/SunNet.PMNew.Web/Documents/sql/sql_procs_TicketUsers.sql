USE [PM2012]
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [TicketUsers_Add] - Insert Procedure Script for TicketUsers
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[TicketUsers_Add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[TicketUsers_Add]
GO

CREATE PROCEDURE [dbo].[TicketUsers_Add]
(
	@TUID int = NULL OUTPUT,
	@TicketID int,
	@UserID int
)
AS
	SET NOCOUNT ON

	INSERT INTO [TicketUsers]
	(
		[TicketID],
		[UserID]
	)
	VALUES
	(
		@TicketID,
		@UserID
	)

	SELECT @TUID =ISNULL( SCOPE_IDENTITY(),0);

	RETURN @@Error
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [TicketUsers_Update] - Update Procedure Script for TicketUsers
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[TicketUsers_Update]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[TicketUsers_Update]
GO

CREATE PROCEDURE [dbo].[TicketUsers_Update]
(
	@TUID int,
	@TicketID int,
	@UserID int
)
AS
	SET NOCOUNT ON
	
	UPDATE [TicketUsers]
	SET
		[TicketID] = @TicketID,
		[UserID] = @UserID
	WHERE 
		[TUID] = @TUID

	RETURN @@Error
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [TicketUsers_Delete] - Update Procedure Script for TicketUsers
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[TicketUsers_Delete]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[TicketUsers_Delete]
GO

CREATE PROCEDURE [dbo].[TicketUsers_Delete]
(
	@TUID int
)
AS
	SET NOCOUNT ON

	DELETE 
	FROM   [TicketUsers]
	WHERE  
		[TUID] = @TUID

	RETURN @@Error
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [TicketUsers_GetModel] - GetInfo Procedure Script for TicketUsers
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[TicketUsers_GetModel]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[TicketUsers_GetModel]
GO

CREATE PROCEDURE [dbo].[TicketUsers_GetModel]
(
	@TUID int
)
AS
	SET NOCOUNT ON
	
	SELECT * FROM [TicketUsers]
	
	WHERE 
		[TUID] = @TUID

	RETURN @@Error
GO

