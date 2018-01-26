using StructureMap;
using SunNet.PMNew.Core.ComplaintModel;
using SunNet.PMNew.Core.ComplaintModel.Interfaces;
using SunNet.PMNew.Entity.ComplaintModel;
using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.Impl.SqlDataProvider.Complaint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.App
{
    public class ComplaintApplication : BaseApp
    {
        public ComplaintManager complaintMgr;

        public ComplaintApplication()
        {
            complaintMgr = new ComplaintManager(ObjectFactory.GetInstance<IComplaintRepository>());
        }

        public List<ComplaintEntity> SearchComplaints(ComplaintSearchEntity request, out int recordCount) 
        {
            return complaintMgr.SearchComplaints(request, out recordCount);
        }

        public int AddComplaint(ComplaintEntity complaintEntity)
        {
            return complaintMgr.AddComplaint(complaintEntity);
        }

        public bool UpdateComplaint(ComplaintEntity complaintEntity)
        {
            return complaintMgr.UpdateComplaint(complaintEntity);
        }


        public string GetComItem(string connStr, string spName, string type, int id)
        {
            return complaintMgr.GetComItem(connStr, spName, type, id);
        }

        public bool DeleteComItem(string connStr, string spName, string type, int id)
        {
            return complaintMgr.DeleteComItem(connStr, spName, type, id);
        }
    }
}
