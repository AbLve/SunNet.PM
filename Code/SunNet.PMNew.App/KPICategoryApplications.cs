using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StructureMap;

using SunNet.PMNew.Framework.Core;

using SunNet.PMNew.Core.KPIModule;

using SunNet.PMNew.Entity.KPIModel;


namespace SunNet.PMNew.App
{

    public class KPICategoryApplications : BaseApp
    {
        private KPIManager mgr;

        public KPICategoryApplications()
        {
            mgr = new KPIManager(ObjectFactory.GetInstance<IKPICategoryRepository>());
        }
        #region Category
        public int AddCategory(KPICategoriesEntity category)
        {
            this.ClearBrokenRuleMessages();
            int id = mgr.AddCategory(category);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return id;
        }


        public bool deleteCategory(int ID)
        {
            this.ClearBrokenRuleMessages();
            return  mgr.DeleteCategory(ID);
            //this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
        }

        public bool UpdateCategory(KPICategoriesEntity category)
        {
            this.ClearBrokenRuleMessages();
            bool updated = mgr.UpdateCategory(category);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return updated;
        }
        public KPICategoriesEntity GetCategory(int ID)
        {
            this.ClearBrokenRuleMessages();
            KPICategoriesEntity role = mgr.GetCategory(ID);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return role;
        }
        #endregion
    }
        

}
