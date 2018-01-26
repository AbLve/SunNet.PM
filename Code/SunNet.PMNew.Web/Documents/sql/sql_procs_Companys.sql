USE [PM2012]
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [Companys_Add] - Insert Procedure Script for Companys
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[Companys_Add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[Companys_Add]
GO

CREATE PROCEDURE [dbo].[Companys_Add]
(
	@ComID int = NULL OUTPUT,
	@CompanyName varchar(200),
	@Phone varchar(20),
	@Fax varchar(20),
	@Website varchar(100),
	@AssignedSystemUrl varchar(100),
	@Address1 varchar(500),
	@Address2 varchar(500),
	@City varchar(100),
	@State varchar(2),
	@Logo varchar(100),
	@Status varchar(10),
	@CreatedOn datetime,
	@CreatedBy int,
	@CreateUserName varchar(200),
	@ModifiedOn datetime,
	@ModifiedBy int
)
AS
	SET NOCOUNT ON

	INSERT INTO [Companys]
	(
		[CompanyName],
		[Phone],
		[Fax],
		[Website],
		[AssignedSystemUrl],
		[Address1],
		[Address2],
		[City],
		[State],
		[Logo],
		[Status],
		[CreatedOn],
		[CreatedBy],
		[CreateUserName],
		[ModifiedOn],
		[ModifiedBy]
	)
	VALUES
	(
		@CompanyName,
		@Phone,
		@Fax,
		@Website,
		@AssignedSystemUrl,
		@Address1,
		@Address2,
		@City,
		@State,
		@Logo,
		@Status,
		@CreatedOn,
		@CreatedBy,
		@CreateUserName,
		@ModifiedOn,
		@ModifiedBy
	)

	SELECT @ComID =ISNULL( SCOPE_IDENTITY(),0);

	RETURN @@Error
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [Companys_Update] - Update Procedure Script for Companys
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[Companys_Update]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[Companys_Update]
GO

CREATE PROCEDURE [dbo].[Companys_Update]
(
	@ComID int,
	@CompanyName varchar(200),
	@Phone varchar(20),
	@Fax varchar(20),
	@Website varchar(100),
	@AssignedSystemUrl varchar(100),
	@Address1 varchar(500),
	@Address2 varchar(500),
	@City varchar(100),
	@State varchar(2),
	@Logo varchar(100),
	@Status varchar(10),
	@CreatedOn datetime,
	@CreatedBy int,
	@CreateUserName varchar(200),
	@ModifiedOn datetime,
	@ModifiedBy int
)
AS
	SET NOCOUNT ON
	
	UPDATE [Companys]
	SET
		[CompanyName] = @CompanyName,
		[Phone] = @Phone,
		[Fax] = @Fax,
		[Website] = @Website,
		[AssignedSystemUrl] = @AssignedSystemUrl,
		[Address1] = @Address1,
		[Address2] = @Address2,
		[City] = @City,
		[State] = @State,
		[Logo] = @Logo,
		[Status] = @Status,
		[CreatedOn] = @CreatedOn,
		[CreatedBy] = @CreatedBy,
		[CreateUserName] = @CreateUserName,
		[ModifiedOn] = @ModifiedOn,
		[ModifiedBy] = @ModifiedBy
	WHERE 
		[ComID] = @ComID

	RETURN @@Error
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [Companys_Delete] - Update Procedure Script for Companys
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[Companys_Delete]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[Companys_Delete]
GO

CREATE PROCEDURE [dbo].[Companys_Delete]
(
	@ComID int
)
AS
	SET NOCOUNT ON

	DELETE 
	FROM   [Companys]
	WHERE  
		[ComID] = @ComID

	RETURN @@Error
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [Companys_GetModel] - GetInfo Procedure Script for Companys
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[Companys_GetModel]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[Companys_GetModel]
GO

CREATE PROCEDURE [dbo].[Companys_GetModel]
(
	@ComID int
)
AS
	SET NOCOUNT ON
	
	SELECT * FROM [Companys]
	
	WHERE 
		[ComID] = @ComID

	RETURN @@Error
GO

