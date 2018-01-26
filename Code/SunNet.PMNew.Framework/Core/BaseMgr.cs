using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Web;
using SunNet.PMNew.Framework.Core.BrokenMessage;

namespace SunNet.PMNew.Framework.Core
{
    public class BaseMgr : BrokenMessageEnabled
    {
        public int ContextUserID
        {
            get
            {
                return IdentityContext.UserID;
            }
        }

    }
}
