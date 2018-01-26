using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Core.TicketModule;
using SunNet.PMNew.Framework.Utils;

namespace Pm2012TEST.Fakes
{
    class FakeTicketCache : ICache<TicketsManager>
    {
        #region ICache<TicketsManager> Members

        public object this[string key]
        {
            get
            {
                return null;
            }
            set
            {
            }
        }

        #endregion
    }
}
