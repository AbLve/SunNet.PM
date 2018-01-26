using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SunNet.PMNew.Framework.Utils;

using SunNet.PMNew.Core.CompanyModule;

namespace SunNet.PMNew.App.Messages.Company
{
    public class PostCompanyRequest
    {
        #region Attr
        /// <summary>
        /// CompanyName
        /// </summary>		
        public string CompanyName { get; set; }
        /// <summary>
        /// Phone
        /// </summary>		
        public string Phone { get; set; }
        /// <summary>
        /// Fax
        /// </summary>		
        public string Fax { get; set; }
        /// <summary>
        /// Website
        /// </summary>		
        public string Website { get; set; }
        /// <summary>
        /// AssignedSystemUrl
        /// </summary>		
        public string AssignedSystemUrl { get; set; }
        /// <summary>
        /// Address1
        /// </summary>		
        public string Address1 { get; set; }
        /// <summary>
        /// Address2
        /// </summary>		
        public string Address2 { get; set; }
        /// <summary>
        /// City
        /// </summary>		
        public string City { get; set; }
        /// <summary>
        /// State
        /// </summary>		
        public string State { get; set; }
        /// <summary>
        /// Logo
        /// </summary>		
        public string Logo { get; set; }
        /// <summary>
        /// Status
        /// </summary>		
        public string Status { get; set; }

        public int PostByUserID{get;set;}
        #endregion

        public CompanysEntity ToBusinessEntity(ISystemDateTime timeProvider)
        {
            CompanysEntity companyEntity = CompanyFactory.Create(this.PostByUserID, timeProvider);

            companyEntity.Address1 = this.Address1;
            companyEntity.Address2 = this.Address2;
            companyEntity.AssignedSystemUrl = this.AssignedSystemUrl;
            companyEntity.City = this.City;
            companyEntity.CompanyName = this.CompanyName;
            companyEntity.Fax = this.Fax;
            companyEntity.Logo = this.Logo;
            companyEntity.Phone = this.Phone;
            companyEntity.State = this.State;
            companyEntity.Status = this.Status;
            companyEntity.Website =this.Website;

            return companyEntity;
        }
    }
}
