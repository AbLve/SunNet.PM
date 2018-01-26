using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Entity.CompanyModel;

namespace SunNet.PMNew.Core.CompanyModule
{
    public static class CompanyFactory
    {
        public static CompanysEntity Create(int createUserID, ISystemDateTime datetimeProvider)
        {
            CompanysEntity companyEntity = new CompanysEntity();

            companyEntity.ID = 0;
            companyEntity.CreatedBy = createUserID;
            companyEntity.CreatedOn = datetimeProvider.Now;
            companyEntity.ModifiedBy = createUserID;
            companyEntity.ModifiedOn = datetimeProvider.Now;

            companyEntity.Address1 = string.Empty;
            companyEntity.Address2 = string.Empty;
            companyEntity.AssignedSystemUrl = string.Empty;
            companyEntity.City = string.Empty;
            companyEntity.CompanyName = string.Empty;
            companyEntity.Fax = string.Empty;
            companyEntity.Logo = string.Empty;
            companyEntity.Phone = string.Empty;
            companyEntity.State = string.Empty;
            companyEntity.Status = string.Empty;
            companyEntity.Website = string.Empty;

            return companyEntity;
        }
    }
}
