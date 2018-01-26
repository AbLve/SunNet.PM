using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace SunNet.PMNew.Entity.TicketModel
{
    public class GetProjectIdAndUserIDResponse
    {
        public GetProjectIdAndUserIDResponse() { }
        public int ProjectId { get; set; }
        public int CreateUserId { get; set; }
        public int CompanyId { get; set; }

        public static GetProjectIdAndUserIDResponse ReaderBind(IDataReader dataReader)
        {
            GetProjectIdAndUserIDResponse model = new GetProjectIdAndUserIDResponse();
            object ojb;
            ojb = dataReader["projectId"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ProjectId = (int)ojb;
            }
            ojb = dataReader["CreatedBy"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CreateUserId = (int)ojb;
            }

            ojb = dataReader["CompanyId"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CompanyId = (int)ojb;
            }
            return model;
        }
    }
}
