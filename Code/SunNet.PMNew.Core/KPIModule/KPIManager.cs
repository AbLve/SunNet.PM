using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Framework.Utils.Providers;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.Framework.Core.Notify;
using SunNet.PMNew.Framework.Core.BrokenMessage;
using SunNet.PMNew.Framework.Core.Validator;
using SunNet.PMNew.Core.KPIModule.Validators;
using SunNet.PMNew.Entity.KPIModel;
using SunNet.PMNew.Entity.CompanyModel;
using SunNet.PMNew.Framework;
using System.Web;
using System.Text.RegularExpressions;
using System.Transactions;

namespace SunNet.PMNew.Core.KPIModule
{
   public class KPIManager : BaseMgr
    {
        private IKPICategoryRepository categoryRepository;

        public KPIManager(IKPICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }
        
        public int AddCategory(KPICategoriesEntity category)
        {
            this.ClearBrokenRuleMessages();
            BaseValidator<KPICategoriesEntity> validator = new AddCategoryValidator();
            if (!validator.Validate(category))
            {
                this.AddBrokenRuleMessages(validator.BrokenRuleMessages);
                return 0;
            }
              int id = categoryRepository.Insert(category);
              if (id <= 0)
              {
                  this.AddBrokenRuleMessage();
                  return 0;
              }
              category.ID = id;

              return id;
        }

        public bool DeleteCategory(int ID)
        {
            this.ClearBrokenRuleMessages();
            return categoryRepository.Delete(ID);
         
        }
        public bool UpdateCategory(KPICategoriesEntity category)
        {
            this.ClearBrokenRuleMessages();
            BaseValidator<KPICategoriesEntity> validator = new UpdateCategoryValidator();
            if (!validator.Validate(category))
            {
                this.AddBrokenRuleMessages(validator.BrokenRuleMessages);
                return false;
            }
            if (!categoryRepository.Update(category))
            {
                this.AddBrokenRuleMessage();
                return false;
            }
            return true;
        }
        public KPICategoriesEntity GetCategory(int ID)
        {
            if (ID <= 0)
            {
                return null;
            }
            KPICategoriesEntity CategoryID = categoryRepository.Get(ID);
            if (CategoryID == null)
            {
                return null;
            }
            return CategoryID;
        }
    }
}
