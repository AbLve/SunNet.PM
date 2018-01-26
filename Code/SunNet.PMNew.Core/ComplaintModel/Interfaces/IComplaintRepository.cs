using SunNet.PMNew.Entity.ComplaintModel;
using SunNet.PMNew.Framework.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Core.ComplaintModel.Interfaces
{
    public interface IComplaintRepository : IRepository<ComplaintEntity>
    {
        List<ComplaintEntity> SearchComplaints(ComplaintSearchEntity request, out int recordCount);
        string GetComItem(string connStr, string spName, string type, int id);
        bool DeleteComItem(string connStr, string spName, string type, int id);
    }
}
