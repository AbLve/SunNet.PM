using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Entity.ProposalTrackerModel
{
    public class ProposalViewModel
    {
        public static ProposalViewModel ReaderBind(IDataReader dataReader)
        {
            ProposalViewModel model = new ProposalViewModel();

            object obj;
            obj = dataReader["CompanyId"];
            if (obj != null && obj != DBNull.Value)
            {
                model.CompanyId = (int)obj;
            }
            model.CompanyName = dataReader["CompanyName"].ToString();

            obj = dataReader["ProjectId"];
            if (obj != null && obj != DBNull.Value)
            {
                model.ProjectId = (int)obj;
            }
            model.ProjectTitle = dataReader["ProjectTitle"].ToString();

            obj = dataReader["ProposalId"];
            if (obj != null && obj != DBNull.Value)
            {
                model.ProposalId = (int)obj;
            }
            model.ProposalTitle = dataReader["ProposalTitle"].ToString();
            model.PONo = dataReader["PONo"].ToString();
            return model;
        }
        public int CompanyId { get; set; }

        public string CompanyName { get; set; }

        public int ProjectId { get; set; }

        public string ProjectTitle { get; set; }

        public int ProposalId { get; set; }

        public string ProposalTitle { get; set; }

        public string PONo { get; set; }
    }
}
