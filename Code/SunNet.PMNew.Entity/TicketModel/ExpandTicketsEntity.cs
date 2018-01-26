using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SunNet.PMNew.Framework.Utils.Helpers;

namespace SunNet.PMNew.Entity.TicketModel
{
    public class ExpandTicketsEntity : TicketsEntity
    {
        private static TicketsEntity ReaderBindExpand(IDataReader dataReader)
        {
            TicketsEntity model = new ExpandTicketsEntity();
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
            model.TicketCode = dataReader["TicketCode"].ToString();
            ojb = dataReader["TicketType"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TicketType = (TicketsType)Enum.Parse(typeof(TicketsType), ojb.ToString());
            }

            model.Description = dataReader["Description"].ToString();
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
            ojb = dataReader["Status"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Status = (TicketsState)ojb;
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

            if (dataReader.Contains("CreatedByFirstName"))
                model.CreatedByFirstName = dataReader["CreatedByFirstName"].ToString();
            if (dataReader.Contains("CreatedByLastName"))
                model.CreatedByLastName = dataReader["CreatedByLastName"].ToString();
            
            return model;
        }

        public static ExpandTicketsEntity ExReaderBind(IDataReader dataReader)
        {
            ExpandTicketsEntity model = (ExpandTicketsEntity)ExpandTicketsEntity.ReaderBindExpand(dataReader);
            try
            {
                object ojb = dataReader["Percentage"];
                if (ojb != null && ojb != DBNull.Value)
                {
                    model.Percentage = Convert.ToDecimal(ojb.ToString());
                }
                else
                {
                    model.Percentage = 0;
                }
                model.ProjectTitle = dataReader["ProjectTitle"].ToString();

                ojb = dataReader["OrderNum"];
                if (ojb != null && ojb != DBNull.Value)
                {
                    model.OrderNum = int.Parse(ojb.ToString());
                }
                else
                    model.OrderNum = 0;

                model.FirstName = dataReader["FirstName"].ToString();
                model.LastName = dataReader["LastName"].ToString();

                model.FullDescription = model.Description;
                int maxLength = 150;
                if (model.Description.Length > maxLength)
                {
                    model.Description = model.Description.Substring(0, maxLength);
                }
            }
            catch
            {
            }
            return model;
        }
        #region Extra Attrs
        public int OrderNum { get; set; }
        public decimal Percentage { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        #endregion

    }
}
