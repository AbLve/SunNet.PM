using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using SunNet.PMNew.Entity.TicketModel.Enums;
using SunNet.PMNew.Framework.Core;
using System.ComponentModel.DataAnnotations;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.Entity.UserModel;

namespace SunNet.PMNew.Entity.TicketModel
{
    //Tickets
    public class TicketsEntity : BaseEntity
    {
        #region ticketCode

        public static string ShowTicketCode(string type, string tid)
        {
            string tickeCode = "";

            if (type.Length != 0)
            {
                tickeCode = type + tid;
            }

            return tickeCode;
        }

        #endregion

        /// <summary>
        /// Bind IDataReader to Entity
        /// </summary>
        public static TicketsEntity ReaderBind(IDataReader dataReader, bool isPorjectCode)
        {
            TicketsEntity model = new TicketsEntity();
            object ojb;
            ojb = dataReader["TicketID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ID = (int)ojb;
                model.TicketID = (int)ojb;
            }
            ojb = dataReader["CompanyID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CompanyID = (int)ojb;
            }
            ojb = dataReader["ProjectID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ProjectID = (int)ojb;
            }
            model.Title = dataReader["Title"].ToString();

            ojb = dataReader["TicketCode"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TicketCode = ShowTicketCode(ojb.ToString(), model.TicketID.ToString());
            }
            ojb = dataReader["TicketType"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TicketType = (TicketsType)Enum.Parse(typeof(TicketsType), ojb.ToString());
            }

            model.FullDescription = dataReader["Description"].ToString();
            model.Description = model.FullDescription.SubString(100);

            ojb = dataReader["CreatedOn"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CreatedOn = (DateTime)ojb;
            }
            ojb = dataReader["CreatedBy"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CreatedBy = (int)ojb;
            }
            ojb = dataReader["ModifiedOn"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ModifiedOn = (DateTime)ojb;
            }
            ojb = dataReader["ModifiedBy"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ModifiedBy = (int)ojb;
            }
            ojb = dataReader["PublishDate"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.PublishDate = (DateTime)ojb;
            }
            ojb = dataReader["ClientPublished"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ClientPublished = (bool)ojb;
            }
            ojb = dataReader["StartDate"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.StartDate = (DateTime)ojb;
            }
            ojb = dataReader["DeliveryDate"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.DeliveryDate = (DateTime)ojb;
            }
            ojb = dataReader["ContinueDate"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ContinueDate = (int)ojb;
            }
            model.URL = dataReader["URL"].ToString();
            ojb = dataReader["Priority"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Priority = (PriorityState)ojb;
            }
            ojb = dataReader["Accounting"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Accounting = (AccountingState)Enum.Parse(typeof(AccountingState), ojb.ToString());
            }
            ojb = dataReader["Status"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Status = (TicketsState)ojb;
            }

            if (dataReader.Contains("RealStatus"))
            {
                model.RealStatus = (TicketsState)((int)dataReader["RealStatus"]);
            }

            ojb = dataReader["ConvertDelete"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ConvertDelete = (CovertDeleteState)ojb;
            }
            ojb = dataReader["IsInternal"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.IsInternal = (bool)ojb;
            }
            ojb = dataReader["CreateType"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CreateType = (int)ojb;
            }
            ojb = dataReader["SourceTicketID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.SourceTicketID = (int)ojb;
            }
            ojb = dataReader["IsEstimates"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.IsEstimates = (bool)ojb;
            }
            ojb = dataReader["InitialTime"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.InitialTime = (decimal)ojb;
            }
            ojb = dataReader["FinalTime"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.FinalTime = (decimal)ojb;
            }
            ojb = dataReader["EsUserID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.EsUserID = (int)ojb;
            }
            ojb = dataReader["Star"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Star = (int)ojb;
            }
            ojb = dataReader["IsRead"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.IsRead = (TicketIsRead)ojb;
            }

            model.ProjectCode = dataReader.Contains("projectCode") ? dataReader["projectCode"].ToString() : null;
            model.ProjectTitle = dataReader.Contains("ProjectTitle") ? dataReader["ProjectTitle"].ToString() : null;
            model.FileTitle = dataReader.Contains("FileTitle") ? dataReader["FileTitle"].ToString() : null;
            model.ContentType = dataReader.Contains("ContentType") ? dataReader["ContentType"].ToString() : null;
            model.FileID = dataReader.Contains("FileID") ? dataReader["FileID"].ToString() : null;
            model.FileSize = dataReader.Contains("FileSize") ? dataReader["FileSize"].ToString() : null;
            model.FilePath = dataReader.Contains("FilePath") ? dataReader["FilePath"].ToString() : null;

            ojb = dataReader["Source"];
            if (ojb != DBNull.Value && ojb != null)
            {
                model.Source = (RolesEnum)Enum.Parse(typeof(RolesEnum),
                               ojb.ToString());
            }

            ojb = dataReader["Source"];
            if (ojb != DBNull.Value && ojb != null)
            {
                model.Source = (RolesEnum)Enum.Parse(typeof(RolesEnum),
                               ojb.ToString());
            }

            ojb = dataReader["AdditionalState"];
            if (ojb != DBNull.Value && ojb != null)
            {
                model.AdditionalState = (AdditionalStates)((int)ojb);
            }
            else
                model.AdditionalState = AdditionalStates.Normal;

            model.ShowNotification = dataReader.Contains("ShowNotification")
                && bool.Parse(dataReader["ShowNotification"].ToString());

            model.WorkingOnStatus = dataReader.Contains("WorkingOnStatus")
                ? (TicketUserStatus)int.Parse(dataReader["WorkingOnStatus"].ToString())
                : TicketUserStatus.None;

            ojb = dataReader["ConfirmEstmateUserId"];
            if (ojb != DBNull.Value && ojb != null)
                model.ConfirmEstmateUserId = (int)ojb;

            if (dataReader.Contains("CreatedByFirstName"))
                model.CreatedByFirstName = dataReader["CreatedByFirstName"].ToString();
            if (dataReader.Contains("CreatedByLastName"))
                model.CreatedByLastName = dataReader["CreatedByLastName"].ToString();
            if(dataReader.Contains("ResponsibleUserName"))
                model.ResponsibleUserName= dataReader["ResponsibleUserName"].ToString();

            ojb = dataReader["ResponsibleUser"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ResponsibleUser = (int)ojb;
            }
            
            ojb = dataReader["ResponsibleUser"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ResponsibleUser = (int)ojb;
            }

            if (dataReader.Contains("ProprosalName"))
            {
                ojb = dataReader["ProprosalName"];
                if (ojb != null && ojb != DBNull.Value)
                {
                    model.ProprosalName = (string)ojb;
                }
                ojb = dataReader["WorkPlanName"];
                if (ojb != null && ojb != DBNull.Value)
                {
                    model.WorkPlanName = (string)ojb;
                }
                ojb = dataReader["WorkScope"];
                if (ojb != null && ojb != DBNull.Value)
                {
                    model.WorkScope = (string)ojb;
                }
                ojb = dataReader["Invoice"];
                if (ojb != null && ojb != DBNull.Value)
                {
                    model.Invoice = (string)ojb;
                }
            }
            return model;
        }


        /// <summary>
        /// Bind IDataReader to Entity
        /// </summary>
        public static TicketsEntity ReaderBind(IDataReader dataReader)
        {
            TicketsEntity model = new TicketsEntity();
            object ojb;
            ojb = dataReader["TicketID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ID = (int)ojb;
                model.TicketID = (int)ojb;
            }            
            ojb = dataReader["ProjectID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ProjectID = (int)ojb;
            }
            model.Title = dataReader["Title"].ToString();

            model.FullDescription = dataReader["Description"].ToString();

            ojb = dataReader["ModifiedOn"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ModifiedOn = (DateTime)ojb;
            }            
            ojb = dataReader["FinalTime"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.FinalTime = (decimal)ojb;
            }           
            model.ProjectTitle = dataReader.Contains("ProjectTitle") ? dataReader["ProjectTitle"].ToString() : null;
            model.FileTitle = dataReader.Contains("FileTitle") ? dataReader["FileTitle"].ToString() : null;
            model.ContentType = dataReader.Contains("ContentType") ? dataReader["ContentType"].ToString() : null;
            model.FileID = dataReader.Contains("FileID") ? dataReader["FileID"].ToString() : null;
            model.FileSize = dataReader.Contains("FileSize") ? dataReader["FileSize"].ToString() : null;
            model.FilePath = dataReader.Contains("FilePath") ? dataReader["FilePath"].ToString() : null;
            return model;
        }

        public TicketsEntity()
        {
            ProprosalName = string.Empty;
            WorkPlanName = string.Empty;
            WorkScope = string.Empty;
            Invoice = string.Empty;
        }

        #region extend
        public string ProjectCode { get; set; }
        public string ProjectTitle { get; set; }
        public UsersEntity CreatedUserEntity { get; set; }
        public string CreatedByFirstName { get; set; }
        public string CreatedByLastName { get; set; }
        public string ResponsibleUserName { get; set; }
        public TicketsState RealStatus { get; set; }
        public string FileTitle { get; set; }
        public string ContentType { get; set; }
        public string FileID { get; set; }
        public string FileSize { get; set; }
        public string FilePath { get; set; }

        #endregion
        /// <summary>
        /// TicketID
        /// </summary>		
        public int TicketID { get; set; }
        /// <summary>
        /// CompanyID
        /// </summary>		
        public int CompanyID { get; set; }
        /// <summary>
        /// ProjectID
        /// </summary>		
        public int ProjectID { get; set; }
        /// <summary>
        /// TicketCode => ID now
        /// </summary>
        [Required]
        [StringLength(10)]
        public string TicketCode
        {
            get { return ID.ToString(); }
            set { }
        }
        /// <summary>
        /// Title
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Title { get; set; }
        /// <summary>
        /// TicketType
        /// </summary>	
        public TicketsType TicketType { get; set; }
        /// <summary>
        /// 读取时，默认显示100个字，访问完整Description请使用FullDescription
        /// </summary>		
        [StringLength(5000)]
        public string Description { get; set; }

        /// <summary>
        /// 完整Description
        /// </summary>
        /// Author  :  Jack Zhang (JACKZ)
        /// Date    :  4/24 12:23
        public string FullDescription { get; set; }
        /// <summary>
        /// PublishDate
        /// </summary>		
        [Required]
        [DataType(DataType.Date)]
        public DateTime PublishDate { get; set; }
        /// <summary>
        /// ClientPublished
        /// </summary>		
        public bool ClientPublished { get; set; }
        /// <summary>
        /// StartDate
        /// </summary>		
        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        /// <summary>
        /// DeliveryDate
        /// </summary>		
        [Required]
        [DataType(DataType.Date)]
        public DateTime DeliveryDate { get; set; }
        /// <summary>
        /// ContinueDate
        /// </summary>		
        [Required]
        [RegularExpression(@"^[0-9]*$")]
        public int ContinueDate { get; set; }
        /// <summary>
        /// URL
        /// </summary>		
        public string URL { get; set; }
        /// <summary>
        /// Priority
        /// </summary>		
        public PriorityState Priority { get; set; }
        /// <summary>
        /// Status
        /// </summary>
        public TicketsState Status { get; set; }

        public TicketIsRead IsRead { get; set; }

        private TicketsState ExpandStatus
        {
            get
            {
                if (AdditionalState != AdditionalStates.Normal)
                {
                    return (TicketsState)(int)AdditionalState;
                }
                else
                {
                    return Status;
                }
            }
        }

        /// <summary>
        /// 扩展状态，用来标识不受流程控制的状态，如 feedback
        /// </summary>
        private AdditionalStates AdditionalState { get; set; }

        /// <summary>
        /// ConvertDelete,Normal:1,IsForverDelete:3,IsHistory: 2
        /// </summary>		
        public CovertDeleteState ConvertDelete { get; set; }
        /// <summary>
        /// IsInternal
        /// </summary>		
        public bool IsInternal { get; set; }
        /// <summary>
        /// CreateType
        /// </summary>		
        public int CreateType { get; set; }
        /// <summary>
        /// SourceTicketID
        /// </summary>		
        [Required]
        [RegularExpression(@"^[0-9]*$")]
        public int SourceTicketID { get; set; }
        /// <summary>
        /// IsEstimates
        /// </summary>		
        public bool IsEstimates { get; set; }

        /// <summary>
        /// Accounting
        /// </summary>
        [Required]
        public AccountingState Accounting { get; set; }
        /// <summary>
        /// InitialTime
        /// </summary>		
        public decimal InitialTime { get; set; }
        /// <summary>
        /// FinalTime
        /// </summary>	
        public decimal FinalTime { get; set; }
        /// <summary>
        /// EsUserID 分配进行估时的人
        /// </summary>
        public int EsUserID { get; set; }
        public int Star { get; set; }

        public RolesEnum? Source { get; set; }

        /// <summary>
        /// 是否需要显示气泡提示
        /// </summary>
        public bool ShowNotification { get; set; }

        public TicketUserStatus WorkingOnStatus { get; set; }

        /// <summary>
        /// 指派谁来最终确认估时
        /// </summary>
        public int ConfirmEstmateUserId { get; set; }

        /// <summary>
        /// 负责人
        /// </summary>
        public int ResponsibleUser { get; set; }


        public string ProprosalName { get; set; }

        public string WorkPlanName { get; set; }

        public string WorkScope { get; set; }

        public string Invoice { get; set; }
    }
}