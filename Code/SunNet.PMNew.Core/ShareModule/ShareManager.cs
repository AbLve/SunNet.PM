using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using SunNet.PMNew.Core.ShareModule.Interfaces;
using SunNet.PMNew.Core.ShareModule.Validators;
using SunNet.PMNew.Core.UserModule;
using SunNet.PMNew.Entity.ShareModel;
using SunNet.PMNew.Entity.ShareModel.DTO;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.Framework.Core.Validator;
/**************************************************************************
 * Developer: 		Jack Zhang
 * Computer:		JACKZ
 * Domain:			Jackz
 * CreatedOn:		5/25 21:24:16
 * Description:		Knowledge share basic function
 * Version History:	Created,5/25 21:24:16
 * 
 * 
 **************************************************************************/
using SunNet.PMNew.Framework.Utils;


namespace SunNet.PMNew.Core.ShareModule
{
    public class ShareManager : BaseMgr
    {
        private readonly IShareRepository _shareRepo;
        private readonly ICache<ShareManager> _cache;
        private const string CacheKeyAllShareTypes = "AllShareTypes";

        public ShareManager(IShareRepository shareRepo, ICache<ShareManager> cache)
        {
            _shareRepo = shareRepo;
            _cache = cache;
        }

        public bool InsertType(ShareTypeEntity type)
        {
            this.ClearBrokenRuleMessages();
            BaseValidator<ShareTypeEntity> validator = new ShareTypeValidator();
            if (!validator.Validate(type))
            {
                this.AddBrokenRuleMessages(validator.BrokenRuleMessages);
            }
            int id = _shareRepo.InsertShareType(type);
            if (id <= 0)
            {
                this.AddBrokenRuleMessage();
                return false;
            }
            type.ID = id;

            _cache[CacheKeyAllShareTypes] = null;
            return true;
        }

        public bool Insert(ShareEntity entity)
        {
            this.ClearBrokenRuleMessages();
            BaseValidator<ShareEntity> validator = new ShareValidator();
            if (!validator.Validate(entity))
            {
                this.AddBrokenRuleMessages(validator.BrokenRuleMessages);
            }
            int id = _shareRepo.Insert(entity);
            if (id <= 0)
            {
                this.AddBrokenRuleMessage();
                return false;
            }
            entity.ID = id;

            if (entity.TypeEntity != null && entity.TypeEntity.ID == 0)
                _cache[CacheKeyAllShareTypes] = null;
            return true;
        }

        public bool Update(ShareEntity entity)
        {
            this.ClearBrokenRuleMessages();
            BaseValidator<ShareEntity> validator = new ShareValidator();
            if (!validator.Validate(entity))
            {
                this.AddBrokenRuleMessages(validator.BrokenRuleMessages);
            }
            var result = _shareRepo.Update(entity);
            if (!result)
            {
                this.AddBrokenRuleMessage("Error", "Update failed.");
                return false;
            }
            return true;
        }

        public ShareEntity Get(int id)
        {
            if (id < 1)
                return null;
            return _shareRepo.Get(id);
        }

        public SearchShareResponse GetShares(SearchShareRequest request)
        {
            return _shareRepo.GetShares(request);
        }

        public List<ShareTypeEntity> GetShareTypes()
        {
            var list = _cache[CacheKeyAllShareTypes] as List<ShareTypeEntity>;
            if (list != null) return list;

            list = _shareRepo.GetShareTypes();
            _cache[CacheKeyAllShareTypes] = list;
            return list;
        }

        public bool Delete(int id)
        {
            if (id < 1)
                return true;
            var count = _shareRepo.Delete(id);
            return count;
        }
    }
}
