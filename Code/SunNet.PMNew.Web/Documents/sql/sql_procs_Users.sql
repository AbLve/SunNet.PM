USE [PM2012]
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [Users_Add] - Insert Procedure Script for Users
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[Users_Add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[Users_Add]
GO

CREATE PROCEDURE [dbo].[Users_Add]
(
	@UserID int = NULL OUTPUT,
	@CompanyName varchar(200),
	@CompanyID int,
	@RoleID int,
	@FirstName varchar(20),
	@LastName varchar(20),
	@UserName varchar(20),
	@Email varchar(50),
	@PassWord varchar(50),
	@Title varchar(100),
	@Phone varchar(12),
	@EmergencyContactFirstName varchar(20),
	@EmergencyContactLastName varchar(20),
	@EmergencyContactPhone varchar(20),
	@EmergencyContactEmail varchar(50),
	@HasAMaintenancePlan bit,
	@DoesNotHaveAMaintenancePlan bit,
	@NeedsAQuoteApproval bit,
	@DoesNotNeedAQuoteApproval bit,
	@AllowMeToChoosePerSubmission bit,
	@CreatedOn datetime,
	@AccountStatus int,
	@ForgotPassword int,
	@IsDelete bit,
	@Status varchar(18),
	@UserType varchar(18),
	@Skype varchar(50),
	@Office varchar(2)
)
AS
	SET NOCOUNT ON

	INSERT INTO [Users]
	(
		[CompanyName],
		[CompanyID],
		[RoleID],
		[FirstName],
		[LastName],
		[UserName],
		[Email],
		[PassWord],
		[Title],
		[Phone],
		[EmergencyContactFirstName],
		[EmergencyContactLastName],
		[EmergencyContactPhone],
		[EmergencyContactEmail],
		[HasAMaintenancePlan],
		[DoesNotHaveAMaintenancePlan],
		[NeedsAQuoteApproval],
		[DoesNotNeedAQuoteApproval],
		[AllowMeToChoosePerSubmission],
		[CreatedOn],
		[AccountStatus],
		[ForgotPassword],
		[IsDelete],
		[Status],
		[UserType],
		[Skype],
		[Office]
	)
	VALUES
	(
		@CompanyName,
		@CompanyID,
		@RoleID,
		@FirstName,
		@LastName,
		@UserName,
		@Email,
		@PassWord,
		@Title,
		@Phone,
		@EmergencyContactFirstName,
		@EmergencyContactLastName,
		@EmergencyContactPhone,
		@EmergencyContactEmail,
		@HasAMaintenancePlan,
		@DoesNotHaveAMaintenancePlan,
		@NeedsAQuoteApproval,
		@DoesNotNeedAQuoteApproval,
		@AllowMeToChoosePerSubmission,
		@CreatedOn,
		@AccountStatus,
		@ForgotPassword,
		@IsDelete,
		@Status,
		@UserType,
		@Skype,
		@Office
	)

	SELECT @UserID =ISNULL( SCOPE_IDENTITY(),0);

	RETURN @@Error
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [Users_Update] - Update Procedure Script for Users
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[Users_Update]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[Users_Update]
GO

CREATE PROCEDURE [dbo].[Users_Update]
(
	@UserID int,
	@CompanyName varchar(200),
	@CompanyID int,
	@RoleID int,
	@FirstName varchar(20),
	@LastName varchar(20),
	@UserName varchar(20),
	@Email varchar(50),
	@PassWord varchar(50),
	@Title varchar(100),
	@Phone varchar(12),
	@EmergencyContactFirstName varchar(20),
	@EmergencyContactLastName varchar(20),
	@EmergencyContactPhone varchar(20),
	@EmergencyContactEmail varchar(50),
	@HasAMaintenancePlan bit,
	@DoesNotHaveAMaintenancePlan bit,
	@NeedsAQuoteApproval bit,
	@DoesNotNeedAQuoteApproval bit,
	@AllowMeToChoosePerSubmission bit,
	@CreatedOn datetime,
	@AccountStatus int,
	@ForgotPassword int,
	@IsDelete bit,
	@Status varchar(18),
	@UserType varchar(18),
	@Skype varchar(50),
	@Office varchar(2)
)
AS
	SET NOCOUNT ON
	
	UPDATE [Users]
	SET
		[CompanyName] = @CompanyName,
		[CompanyID] = @CompanyID,
		[RoleID] = @RoleID,
		[FirstName] = @FirstName,
		[LastName] = @LastName,
		[UserName] = @UserName,
		[Email] = @Email,
		[PassWord] = @PassWord,
		[Title] = @Title,
		[Phone] = @Phone,
		[EmergencyContactFirstName] = @EmergencyContactFirstName,
		[EmergencyContactLastName] = @EmergencyContactLastName,
		[EmergencyContactPhone] = @EmergencyContactPhone,
		[EmergencyContactEmail] = @EmergencyContactEmail,
		[HasAMaintenancePlan] = @HasAMaintenancePlan,
		[DoesNotHaveAMaintenancePlan] = @DoesNotHaveAMaintenancePlan,
		[NeedsAQuoteApproval] = @NeedsAQuoteApproval,
		[DoesNotNeedAQuoteApproval] = @DoesNotNeedAQuoteApproval,
		[AllowMeToChoosePerSubmission] = @AllowMeToChoosePerSubmission,
		[CreatedOn] = @CreatedOn,
		[AccountStatus] = @AccountStatus,
		[ForgotPassword] = @ForgotPassword,
		[IsDelete] = @IsDelete,
		[Status] = @Status,
		[UserType] = @UserType,
		[Skype] = @Skype,
		[Office] = @Office
	WHERE 
		[UserID] = @UserID

	RETURN @@Error
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [Users_Delete] - Update Procedure Script for Users
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[Users_Delete]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[Users_Delete]
GO

CREATE PROCEDURE [dbo].[Users_Delete]
(
	@UserID int
)
AS
	SET NOCOUNT ON

	DELETE 
	FROM   [Users]
	WHERE  
		[UserID] = @UserID

	RETURN @@Error
GO

----------------------------------------------------------------------------------
-- [author:Jackz.Sunnet]
-- [datetime:2012-09-18]
-- [Users_GetModel] - GetInfo Procedure Script for Users
----------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[Users_GetModel]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[Users_GetModel]
GO

CREATE PROCEDURE [dbo].[Users_GetModel]
(
	@UserID int
)
AS
	SET NOCOUNT ON
	
	SELECT * FROM [Users]
	
	WHERE 
		[UserID] = @UserID

	RETURN @@Error
GO

