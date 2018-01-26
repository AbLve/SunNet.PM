using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.ComponentModel.DataAnnotations;

using SunNet.PMNew.Core.UserModule.Enum;

namespace SunNet.PMNew.Core.UserModule
{
    //Users
    public class UsersEntity : SunNet.PMNew.Framework.Core.BaseEntity
    {
        /// <summary>
        /// Bind IDataReader to Entity
        /// </summary>
        public static UsersEntity ReaderBind(IDataReader dataReader)
        {
            UsersEntity model = new UsersEntity();
            object ojb;
            ojb = dataReader["UserID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.UserID = (int)ojb;
                model.ID = model.UserID;
            }
            model.CompanyName = dataReader["CompanyName"].ToString();
            ojb = dataReader["CompanyID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CompanyID = (int)ojb;
            }
            ojb = dataReader["RoleID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.RoleID = (int)ojb;
                model.Role = (RolesEnum.Roles)model.RoleID;
            }
            model.FirstName = dataReader["FirstName"].ToString();
            model.LastName = dataReader["LastName"].ToString();
            model.UserName = dataReader["UserName"].ToString();
            model.Email = dataReader["Email"].ToString();
            model.PassWord = dataReader["PassWord"].ToString();
            model.Title = dataReader["Title"].ToString();
            model.Phone = dataReader["Phone"].ToString();
            model.EmergencyContactFirstName = dataReader["EmergencyContactFirstName"].ToString();
            model.EmergencyContactLastName = dataReader["EmergencyContactLastName"].ToString();
            model.EmergencyContactPhone = dataReader["EmergencyContactPhone"].ToString();
            model.EmergencyContactEmail = dataReader["EmergencyContactEmail"].ToString();
            ojb = dataReader["HasAMaintenancePlan"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.HasAMaintenancePlan = (bool)ojb;
            }
            ojb = dataReader["DoesNotHaveAMaintenancePlan"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.DoesNotHaveAMaintenancePlan = (bool)ojb;
            }
            ojb = dataReader["NeedsAQuoteApproval"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.NeedsAQuoteApproval = (bool)ojb;
            }
            ojb = dataReader["DoesNotNeedAQuoteApproval"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.DoesNotNeedAQuoteApproval = (bool)ojb;
            }
            ojb = dataReader["AllowMeToChoosePerSubmission"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.AllowMeToChoosePerSubmission = (bool)ojb;
            }
            ojb = dataReader["CreatedOn"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CreatedOn = (DateTime)ojb;
            }
            ojb = dataReader["AccountStatus"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.AccountStatus = (int)ojb;
            }
            ojb = dataReader["ForgotPassword"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ForgotPassword = (int)ojb;
            }
            ojb = dataReader["IsDelete"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.IsDelete = (bool)ojb;
            }
            model.Status = dataReader["Status"].ToString();
            model.UserType = dataReader["UserType"].ToString();
            model.Skype = dataReader["Skype"].ToString();
            model.Office = dataReader["Office"].ToString();
            return model;
        }
        /// <summary>
        /// UserID
        /// </summary>		
        public int UserID { get; set; }
        /// <summary>
        /// CompanyName
        /// </summary>		
        [Required]
        [StringLength(200)]
        public string CompanyName { get; set; }
        /// <summary>
        /// CompanyID
        /// </summary>	
        [Required]
        public int CompanyID { get; set; }
        /// <summary>
        /// RoleID
        /// </summary>		
        [Required]
        public int RoleID { get; set; }
        /// <summary>
        /// User's Role
        /// </summary>
        public RolesEnum.Roles Role { get; set; }
        /// <summary>
        /// FirstName
        /// </summary>		
        [Required]
        [StringLength(20)]
        public string FirstName { get; set; }
        /// <summary>
        /// LastName
        /// </summary>		
        [Required]
        [StringLength(20)]
        public string LastName { get; set; }
        /// <summary>
        /// UserName
        /// </summary>		
        [Required]
        [StringLength(20)]
        public string UserName { get; set; }
        /// <summary>
        /// Email
        /// </summary>		
        [Required]
        [StringLength(50)]
        [RegularExpression(@"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$")]
        public string Email { get; set; }
        /// <summary>
        /// PassWord
        /// </summary>		
        [Required]
        [StringLength(50)]
        public string PassWord { get; set; }
        /// <summary>
        /// Title
        /// </summary>		
        [Required]
        [StringLength(100)]
        public string Title { get; set; }
        /// <summary>
        /// Phone
        /// </summary>		
        [Required]
        [StringLength(12)]
        public string Phone { get; set; }
        /// <summary>
        /// EmergencyContactFirstName
        /// </summary>		
        [Required]
        [StringLength(20)]
        public string EmergencyContactFirstName { get; set; }
        /// <summary>
        /// EmergencyContactLastName
        /// </summary>		
        [Required]
        [StringLength(20)]
        public string EmergencyContactLastName { get; set; }
        /// <summary>
        /// EmergencyContactPhone
        /// </summary>		
        [Required]
        [StringLength(20)]
        public string EmergencyContactPhone { get; set; }
        /// <summary>
        /// EmergencyContactEmail
        /// </summary>		
        [Required]
        [StringLength(50)]
        [RegularExpression(@"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$")]
        public string EmergencyContactEmail { get; set; }
        /// <summary>
        /// HasAMaintenancePlan
        /// </summary>		
        [Required]
        public bool HasAMaintenancePlan { get; set; }
        /// <summary>
        /// DoesNotHaveAMaintenancePlan
        /// </summary>		
        [Required]
        public bool DoesNotHaveAMaintenancePlan { get; set; }
        /// <summary>
        /// NeedsAQuoteApproval
        /// </summary>		
        [Required]
        public bool NeedsAQuoteApproval { get; set; }
        /// <summary>
        /// DoesNotNeedAQuoteApproval
        /// </summary>		
        [Required]
        public bool DoesNotNeedAQuoteApproval { get; set; }
        /// <summary>
        /// AllowMeToChoosePerSubmission
        /// </summary>		
        [Required]
        public bool AllowMeToChoosePerSubmission { get; set; }
        /// <summary>
        /// CreatedOn
        /// </summary>		
        [Required]
        public DateTime CreatedOn { get; set; }
        /// <summary>
        /// AccountStatus
        /// </summary>		
        [Required]
        public int AccountStatus { get; set; }
        /// <summary>
        /// ForgotPassword
        /// </summary>		
        [Required]
        public int ForgotPassword { get; set; }
        /// <summary>
        /// IsDelete
        /// </summary>		
        [Required]
        public bool IsDelete { get; set; }
        /// <summary>
        /// Status
        /// </summary>		
        [Required]
        [StringLength(20)]
        public string Status { get; set; }
        /// <summary>
        /// UserType
        /// </summary>		
        [Required]
        [StringLength(18)]
        public string UserType { get; set; }
        /// <summary>
        /// Skype
        /// </summary>		
        [Required]
        [StringLength(50)]
        public string Skype { get; set; }
        /// <summary>
        /// Office
        /// </summary>		
        [Required]
        [StringLength(2)]
        public string Office { get; set; }

    }
}