using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using StructureMap;
using SunNet.PMNew.Core.ShareModule;
using SunNet.PMNew.Core.ShareModule.Interfaces;
using SunNet.PMNew.Entity.ShareModel;
using SunNet.PMNew.Entity.ShareModel.DTO;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.Framework.Utils;


/**************************************************************************
 * Developer: 		Jack Zhang
 * Computer:		JACKZ
 * Domain:			Jackz
 * CreatedOn:		5/26 00:37:52
 * Description:		Knowledge share application
 * Version History:	Created,5/26 00:37:52
 * 
 * 
 **************************************************************************/

namespace SunNet.PMNew.App
{
    public class ShareApplication : BaseApp
    {
        private readonly ShareManager _manager = null;
        public ShareApplication()
        {
            _manager = new ShareManager(ObjectFactory.GetInstance<IShareRepository>(),
                ObjectFactory.GetInstance<ICache<ShareManager>>());
        }

        public bool Save(ShareEntity share)
        {
            _manager.ClearBrokenRuleMessages();
#if !DEBUG
            using (TransactionScope tran = new TransactionScope())
            {
#endif
            bool result = false;
            if (share.Type == 0)
            {
                share.TypeEntity.CreatedBy = share.CreatedBy;
                share.TypeEntity.CreatedOn = DateTime.Now;
                result = _manager.InsertType(share.TypeEntity);
                if (!result)
                {
                    this.AddBrokenRuleMessages(_manager.BrokenRuleMessages);
                    return false;
                }
                share.Type = share.TypeEntity.ID;
            }
            if (share.ID < 1)
                result = _manager.Insert(share);
            else
                result = _manager.Update(share);
            if (!result)
            {
                this.AddBrokenRuleMessages(_manager.BrokenRuleMessages);
                return false;
            }
#if !DEBUG
                tran.Complete();
            }
#endif
            return true;
        }

        public ShareEntity Get(int id)
        {
            _manager.ClearBrokenRuleMessages();
            return _manager.Get(id);
        }
        public bool Delete(int id)
        {
            _manager.ClearBrokenRuleMessages();
            if (!_manager.Delete(id))
            {
                this.AddBrokenRuleMessages(_manager.BrokenRuleMessages);
                return false;
            }
            return true;
        }
        public SearchShareResponse GetShares(SearchShareRequest request)
        {
            return _manager.GetShares(request);
        }

        public List<ShareTypeEntity> GetShareTypes()
        {
            return _manager.GetShareTypes();
        }
    }
}
