using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using SunNet.PMNew.Framework.Core;

namespace SunNet.PMNew.Entity.TicketModel
{
    //Tasks
    public class TasksEntity : BaseEntity
    {
        /// <summary>
        /// Bind IDataReader to Entity
        /// </summary>
        public static TasksEntity ReaderBind(IDataReader dataReader)
        {
            TasksEntity model = new TasksEntity();
            object ojb;
            ojb = dataReader["TaskID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TaskID = (int)ojb;
            }
            ojb = dataReader["ProjectID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ProjectID = (int)ojb;
            }
            ojb = dataReader["TicketID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TicketID = (int)ojb;
            }
            model.Title = dataReader["Title"].ToString();
            model.Description = dataReader["Description"].ToString();
            ojb = dataReader["IsCompleted"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.IsCompleted = (bool)ojb;
            }
            ojb = dataReader["CompletedDate"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CompletedDate = (DateTime)ojb;
                model.CreatedOn = model.CompletedDate;
                model.ModifiedOn = model.CompletedDate;
            }
            return model;
        }
        /// <summary>
        /// TaskID
        /// </summary>		
        public int TaskID { get; set; }
        /// <summary>
        /// ProjectID
        /// </summary>		
        public int ProjectID { get; set; }
        /// <summary>
        /// TicketID
        /// </summary>		
        public int TicketID { get; set; }
        /// <summary>
        /// Title
        /// </summary>		
        public string Title { get; set; }
        /// <summary>
        /// Description
        /// </summary>		
        public string Description { get; set; }
        /// <summary>
        /// IsCompleted
        /// </summary>		
        public bool IsCompleted { get; set; }

        /// <summary>
        /// CompletedDate only date, time part disallow
        /// </summary>
        public DateTime CompletedDate
        {
            get;
            set;
        }

    }
}