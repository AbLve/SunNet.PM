using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace SunNet.PMNew.Web.Codes
{
    public class RoleEnumComparer : IComparer<ListItem>
    {

        public int Compare(ListItem x, ListItem y)
        {
            if (x.Value == y.Value)
            {
                return 0;
            }
            else if (x.Value.CompareTo(y.Value) > 0)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
    }
}