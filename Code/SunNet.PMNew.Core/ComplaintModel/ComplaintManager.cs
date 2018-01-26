using StructureMap;
using SunNet.PMNew.Core.ComplaintModel.Interfaces;
using SunNet.PMNew.Entity.ComplaintModel;
using SunNet.PMNew.Framework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Core.ComplaintModel
{
    public class ComplaintManager : BaseMgr
    {
        private IComplaintRepository complaintRepository;

        public ComplaintManager(IComplaintRepository complaintRepository)
        {
            this.complaintRepository = complaintRepository;
        }

        public List<ComplaintEntity> SearchComplaints(ComplaintSearchEntity request, out int recordCount)
        {
            return complaintRepository.SearchComplaints(request, out recordCount);
        }

        public int AddComplaint(ComplaintEntity complaintEntity)
        {
            return complaintRepository.Insert(complaintEntity);
        }

        public bool UpdateComplaint(ComplaintEntity complaintEntity)
        {
            return complaintRepository.Update(complaintEntity);
        }


        public string GetComItem(string connStr, string spName, string type, int id)
        {
            return complaintRepository.GetComItem(connStr, spName, type, id);
        }

        public bool DeleteComItem(string connStr, string spName, string type, int id)
        {
            return complaintRepository.DeleteComItem(connStr, spName, type, id);
        }
    }
}
