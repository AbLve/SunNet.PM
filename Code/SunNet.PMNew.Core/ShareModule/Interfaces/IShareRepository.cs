using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

using SunNet.PMNew.Entity.ShareModel;
using SunNet.PMNew.Entity.ShareModel.DTO;
using SunNet.PMNew.Framework.Core.Repository;

/**************************************************************************
 * Developer: 		Jack Zhang
 * Computer:		JACKZ
 * Domain:			Jackz
 * CreatedOn:		5/25 21:04:09
 * Description:		Knowledge share repository
 * Version History:	Created,5/25 21:04:09
 * 
 * 
 **************************************************************************/


namespace SunNet.PMNew.Core.ShareModule.Interfaces
{
    public interface IShareRepository : IRepository<ShareEntity>
    {
        int InsertShareType(ShareTypeEntity entity);
        List<ShareTypeEntity> GetShareTypes();
        SearchShareResponse GetShares(SearchShareRequest request);
    }
}
