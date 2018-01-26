using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace SunNet.PMNew.Entity.FileModel
{
    public class FileDetailDto : FilesEntity
    {
        public static FileDetailDto ReaderBind(IDataReader dataReader)
        {
            FileDetailDto model = new FileDetailDto();
            object ojb;
            ojb = dataReader["FileID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.FileID = (int)ojb;
            }
            ojb = dataReader["ProjectId"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ProjectId = (int)ojb;
            }
            ojb = dataReader["TicketId"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TicketId = (int)ojb;
            }
            ojb = dataReader["FeedbackId"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.FeedbackId = (int)ojb;
            }
            //ojb = dataReader["ProposalTrackerId"];
            //if (ojb != null && ojb != DBNull.Value)
            //{
            //    model.ProposalTrackerId = (int)ojb;
            //}
            ojb = dataReader["SourceType"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.SourceType = (int)ojb;
            }
            model.FileTitle = dataReader["FileTitle"].ToString();
            model.ContentType = dataReader["ContentType"].ToString();
            ojb = dataReader["FileSize"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.FileSize = (decimal)ojb;
            }
            model.FilePath = dataReader["FilePath"].ToString();
            model.ThumbPath = dataReader["ThumbPath"].ToString();
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
            ojb = dataReader["IsDelete"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.IsDelete = (bool)ojb;
            }
            ojb = dataReader["IsPublic"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.IsPublic = (bool)ojb;
            }
            model.CompanyName = dataReader["CompanyName"].ToString();
            model.ProjectTitle = dataReader["ProjectTitle"].ToString();
            model.TicketTitle = dataReader["TicketTitle"].ToString();
            model.FeedBackTitle = dataReader["FeedBackTitle"].ToString();
            model.TicketCode = dataReader["TicketCode"].ToString();
            model.FirstName = dataReader["FirstName"].ToString();
            model.LastName = dataReader["LastName"].ToString();
            model.Tags = dataReader["Tags"].ToString();
            //ojb = dataReader["TicketID"];
            //if (ojb != null && ojb != DBNull.Value)
            //{
            //    model.TicketId = (int)ojb;
            //}
            //ojb = dataReader["TableType"];
            //if (ojb != DBNull.Value && ojb != null)
            //{
            //    model.TableType = (int)ojb;
            //}
            model.TableType = 1;
            return model;
        }
        public string CompanyName { get; set; }
        public string ProjectTitle { get; set; }
        public string TicketTitle { get; set; }
        public string TicketCode { get; set; }
        public string FeedBackTitle { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Tags { get; set; }
        //1--Files表，2--DocManagement表
        public int TableType { get; set; }
    }
}
