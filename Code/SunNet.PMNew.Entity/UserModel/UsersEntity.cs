using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.ComponentModel.DataAnnotations;
using SunNet.PMNew.Entity.Common;
using SunNet.PMNew.Framework.Core;
namespace SunNet.PMNew.Entity.UserModel
{
    //Users
    public class UsersEntity : BaseEntity, ICloneable, IShowUserName
    {
        /// <summary>
        /// Forgot password flag
        /// </summary>
        public const int ForgotPasswordFlag = 1;
        /// <summary>
        /// Password found by self
        /// </summary>
        public const int ResetPasswordFlag = 2;

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

            model.MaintenancePlanOption = dataReader["MaintenancePlanOption"].ToString();

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
            model.IsNotice = (bool)dataReader["IsNotice"];
            model.PTOHoursOfYear = Convert.ToInt16(dataReader["PTOHoursOfYear"]);

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
        public RolesEnum Role
        {
            get
            {
                return (RolesEnum)this.RoleID;
            }
        }
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
        [StringLength(50)]
        public string UserName { get; set; }

        public string FirstAndLastName
        {
            get
            {
                return string.Format("{0} {1}", FirstName, LastName);
            }
        }

        public string LastNameAndFirst
        {
            get
            {
                return string.Format("{0}, {1}", LastName, FirstName);
            }
        }

        public string UserNameWithOffice
        {
            get
            {
                return UserName + " (" + Office.Trim() + ")";
            }
        }
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
        public string Title { get; set; }
        /// <summary>
        /// Phone
        /// </summary>		
        [StringLength(20)]
        public string Phone { get; set; }
        /// <summary>
        /// EmergencyContactFirstName
        /// </summary>		
        [StringLength(20)]
        public string EmergencyContactFirstName { get; set; }
        /// <summary>
        /// EmergencyContactLastName
        /// </summary>		
        [StringLength(20)]
        public string EmergencyContactLastName { get; set; }
        /// <summary>
        /// EmergencyContactPhone
        /// </summary>		
        [StringLength(20)]
        public string EmergencyContactPhone { get; set; }
        /// <summary>
        /// EmergencyContactEmail
        /// </summary>		
        [StringLength(50)]
        [RegularExpression(@"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$")]
        public string EmergencyContactEmail { get; set; }
        /// <summary>
        /// Maintenance Plan Option: HAS  NEEDAPPROVAL  DONTNEEDAPPROVAL  ALLOWME NONE
        /// </summary>
        public string MaintenancePlanOption { set; get; }
        /// <summary>
        /// Enum for MaintenancePlanOption
        /// </summary>
        public UserMaintenancePlanOption MainPlanOption
        {
            get
            {
                if (string.IsNullOrEmpty(this.MaintenancePlanOption))
                {
                    return UserMaintenancePlanOption.NONE;
                }
                return (UserMaintenancePlanOption)Enum.Parse(typeof(UserMaintenancePlanOption), this.MaintenancePlanOption, true);
            }
        }
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
        /// Status = "ACTIVE" OR "INACTIVE"
        /// </summary>		
        [Required]
        [StringLength(20)]
        public string Status { get; set; }
        /// <summary>
        /// UserType:SUNNET or CLIENT
        /// </summary>		
        [Required]
        [StringLength(6)]
        public string UserType { get; set; }
        /// <summary>
        /// Skype
        /// </summary>		
        public string Skype { get; set; }
        /// <summary>
        /// Office US OR CN
        /// </summary>		
        [Required]
        [StringLength(2)]
        public string Office { get; set; }
        /// <summary>
        /// PTOHoursOfYear
        /// </summary>		
        [Required]
        public double PTOHoursOfYear { get; set; }


        public object Clone()
        {
            UsersEntity clonedUser = new UsersEntity();
            clonedUser.ID = this.ID;
            clonedUser.AccountStatus = this.AccountStatus;
            clonedUser.CompanyID = this.CompanyID;
            clonedUser.CompanyName = this.CompanyName;
            clonedUser.CreatedBy = this.CreatedBy;
            clonedUser.CreatedOn = this.CreatedOn;
            clonedUser.Email = this.Email;
            clonedUser.EmergencyContactEmail = this.EmergencyContactEmail;
            clonedUser.EmergencyContactFirstName = this.EmergencyContactFirstName;
            clonedUser.EmergencyContactLastName = this.EmergencyContactLastName;
            clonedUser.EmergencyContactPhone = this.EmergencyContactPhone;
            clonedUser.FirstName = this.FirstName;
            clonedUser.ForgotPassword = this.ForgotPassword;
            clonedUser.IsDelete = this.IsDelete;
            clonedUser.LastName = this.LastName;
            clonedUser.MaintenancePlanOption = this.MaintenancePlanOption;
            clonedUser.ModifiedBy = this.ModifiedBy;
            clonedUser.ModifiedOn = this.ModifiedOn;
            clonedUser.Office = this.Office;
            clonedUser.PassWord = this.PassWord;
            clonedUser.Phone = this.Phone;
            clonedUser.RoleID = this.RoleID;
            clonedUser.Skype = this.Skype;
            clonedUser.Status = this.Status;
            clonedUser.Title = this.Title;
            clonedUser.UserID = this.UserID;
            clonedUser.UserName = this.UserName;
            clonedUser.UserType = this.UserType;
            return clonedUser;
        }


        /// <summary>
        /// 获取客户的姓名.
        /// </summary>
        /// <param name="visitor">当前登录者.</param>
        /// <param name="format">The format(FN,LN).</param>
        /// <returns></returns>
        /// Author  :  Jack Zhang (JACKZ)
        /// Date    :  4/21 15:05
        public string GetClientUserName(UsersEntity visitor, string format = "FN LN")
        {
            if (this.Role == RolesEnum.CLIENT && visitor.Office.ToUpper().Equals("CN"))
            {
                return "Client";
            }
            return string.Format("<span style='padding-left:0px;padding-right:0px;'>{0}</span>",
                format.ToUpper().Replace("FN", this.FirstName).Replace("LN", this.LastName),
                string.Format("{0},{1}", this.LastName, this.FirstName));
        }

        public bool IsNotice { get; set; }
    }
}