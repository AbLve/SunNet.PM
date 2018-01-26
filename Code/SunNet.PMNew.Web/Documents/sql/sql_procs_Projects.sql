USE [PM2012]
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [Projects_Add] - Insert Procedure Script for Projects
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[Projects_Add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[Projects_Add]
GO

CREATE PROCEDURE [dbo].[Projects_Add]
(
	@ProjectID int,
	@CompanyID int,
	@ProjectCode varchar(64),
	@Title varchar(128),
	@Description varchar(4000),
	@StartDate datetime,
	@EndDate datetime,
	@Status int,
	@CreatedBy int,
	@CreatedOn datetime,
	@ModifiedBy int,
	@ModifiedOn datetime,
	@PMID int,
	@Priority varchar(128),
	@Billable bit,
	@TestLinkURL varchar(250),
	@TestUserName varchar(50),
	@TestPassword varchar(50),
	@FreeHour int,
	@BugNeedApproved bit,
	@RequestNeedApproved bit,
	@IsOverFreeTime bit
)
AS
	SET NOCOUNT ON

	INSERT INTO [Projects]
	(
		[ProjectID],
		[CompanyID],
		[ProjectCode],
		[Title],
		[Description],
		[StartDate],
		[EndDate],
		[Status],
		[CreatedBy],
		[CreatedOn],
		[ModifiedBy],
		[ModifiedOn],
		[PMID],
		[Priority],
		[Billable],
		[TestLinkURL],
		[TestUserName],
		[TestPassword],
		[FreeHour],
		[BugNeedApproved],
		[RequestNeedApproved],
		[IsOverFreeTime]
	)
	VALUES
	(
		@ProjectID,
		@CompanyID,
		@ProjectCode,
		@Title,
		@Description,
		@StartDate,
		@EndDate,
		@Status,
		@CreatedBy,
		@CreatedOn,
		@ModifiedBy,
		@ModifiedOn,
		@PMID,
		@Priority,
		@Billable,
		@TestLinkURL,
		@TestUserName,
		@TestPassword,
		@FreeHour,
		@BugNeedApproved,
		@RequestNeedApproved,
		@IsOverFreeTime
	)

	RETURN @@Error
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [Projects_Update] - Update Procedure Script for Projects
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[Projects_Update]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[Projects_Update]
GO

CREATE PROCEDURE [dbo].[Projects_Update]
(
	@ProjectID int,
	@CompanyID int,
	@ProjectCode varchar(64),
	@Title varchar(128),
	@Description varchar(4000),
	@StartDate datetime,
	@EndDate datetime,
	@Status int,
	@CreatedBy int,
	@CreatedOn datetime,
	@ModifiedBy int,
	@ModifiedOn datetime,
	@PMID int,
	@Priority varchar(128),
	@Billable bit,
	@TestLinkURL varchar(250),
	@TestUserName varchar(50),
	@TestPassword varchar(50),
	@FreeHour int,
	@BugNeedApproved bit,
	@RequestNeedApproved bit,
	@IsOverFreeTime bit
)
AS
	SET NOCOUNT ON
	
	UPDATE [Projects]
	SET
		[ProjectID] = @ProjectID,
		[CompanyID] = @CompanyID,
		[ProjectCode] = @ProjectCode,
		[Title] = @Title,
		[Description] = @Description,
		[StartDate] = @StartDate,
		[EndDate] = @EndDate,
		[Status] = @Status,
		[CreatedBy] = @CreatedBy,
		[CreatedOn] = @CreatedOn,
		[ModifiedBy] = @ModifiedBy,
		[ModifiedOn] = @ModifiedOn,
		[PMID] = @PMID,
		[Priority] = @Priority,
		[Billable] = @Billable,
		[TestLinkURL] = @TestLinkURL,
		[TestUserName] = @TestUserName,
		[TestPassword] = @TestPassword,
		[FreeHour] = @FreeHour,
		[BugNeedApproved] = @BugNeedApproved,
		[RequestNeedApproved] = @RequestNeedApproved,
		[IsOverFreeTime] = @IsOverFreeTime
	WHERE 
		[ProjectID] = @ProjectID

	RETURN @@Error
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [Projects_Delete] - Update Procedure Script for Projects
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[Projects_Delete]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[Projects_Delete]
GO

CREATE PROCEDURE [dbo].[Projects_Delete]
(
	@ProjectID int
)
AS
	SET NOCOUNT ON

	DELETE 
	FROM   [Projects]
	WHERE  
		[ProjectID] = @ProjectID

	RETURN @@Error
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [Projects_GetModel] - GetInfo Procedure Script for Projects
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[Projects_GetModel]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[Projects_GetModel]
GO

CREATE PROCEDURE [dbo].[Projects_GetModel]
(
	@ProjectID int
)
AS
	SET NOCOUNT ON
	
	SELECT * FROM [Projects]
	
	WHERE 
		[ProjectID] = @ProjectID

	RETURN @@Error
GO

