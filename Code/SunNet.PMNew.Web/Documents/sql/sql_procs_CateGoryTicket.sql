USE [PM2012]
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [CateGoryTicket_Add] - Insert Procedure Script for CateGoryTicket
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[CateGoryTicket_Add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[CateGoryTicket_Add]
GO

CREATE PROCEDURE [dbo].[CateGoryTicket_Add]
(
	@CGTID int = NULL OUTPUT,
	@GategoryID int,
	@TicketID int,
	@CreatedOn datetime
)
AS
	SET NOCOUNT ON

	INSERT INTO [CateGoryTicket]
	(
		[GategoryID],
		[TicketID],
		[CreatedOn]
	)
	VALUES
	(
		@GategoryID,
		@TicketID,
		@CreatedOn
	)

	SELECT @CGTID =ISNULL( SCOPE_IDENTITY(),0);

	RETURN @@Error
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [CateGoryTicket_Update] - Update Procedure Script for CateGoryTicket
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[CateGoryTicket_Update]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[CateGoryTicket_Update]
GO

CREATE PROCEDURE [dbo].[CateGoryTicket_Update]
(
	@CGTID int,
	@GategoryID int,
	@TicketID int,
	@CreatedOn datetime
)
AS
	SET NOCOUNT ON
	
	UPDATE [CateGoryTicket]
	SET
		[GategoryID] = @GategoryID,
		[TicketID] = @TicketID,
		[CreatedOn] = @CreatedOn
	WHERE 
		[CGTID] = @CGTID

	RETURN @@Error
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [CateGoryTicket_Delete] - Update Procedure Script for CateGoryTicket
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[CateGoryTicket_Delete]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[CateGoryTicket_Delete]
GO

CREATE PROCEDURE [dbo].[CateGoryTicket_Delete]
(
	@CGTID int
)
AS
	SET NOCOUNT ON

	DELETE 
	FROM   [CateGoryTicket]
	WHERE  
		[CGTID] = @CGTID

	RETURN @@Error
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [CateGoryTicket_GetModel] - GetInfo Procedure Script for CateGoryTicket
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[CateGoryTicket_GetModel]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[CateGoryTicket_GetModel]
GO

CREATE PROCEDURE [dbo].[CateGoryTicket_GetModel]
(
	@CGTID int
)
AS
	SET NOCOUNT ON
	
	SELECT * FROM [CateGoryTicket]
	
	WHERE 
		[CGTID] = @CGTID

	RETURN @@Error
GO

